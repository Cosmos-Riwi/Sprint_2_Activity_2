using Npgsql;
using RestaurantSystem.Models;
using RestaurantSystem.Configuration;
using RestaurantSystem.Data;

namespace RestaurantSystem.Services
{
    /// <summary>
    /// Service for order-related database operations
    /// </summary>
    public class OrderService : BaseService
    {
        public OrderService(PostgreSqlDbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// Gets all orders from the database with customer information
        /// </summary>
        public async Task<List<Order>> GetAllAsync()
        {
            var orders = new List<Order>();
            var sql = $@"
                SELECT o.id, o.order_number, o.order_date, o.status, o.customer_id,
                       c.first_name, c.last_name
                FROM {ApplicationConstants.OrdersTable} o
                LEFT JOIN {ApplicationConstants.CustomersTable} c ON o.customer_id = c.id
                ORDER BY o.id";
            
            using var reader = await ExecuteReaderAsync(sql);
            
            while (await reader.ReadAsync())
            {
                orders.Add(MapOrderFromReader(reader));
            }
            
            return orders;
        }

        /// <summary>
        /// Gets an order by ID
        /// </summary>
        public async Task<Order?> GetByIdAsync(int id)
        {
            if (id <= 0)
                return null;

            var sql = $@"
                SELECT o.id, o.order_number, o.order_date, o.status, o.customer_id,
                       c.first_name, c.last_name
                FROM {ApplicationConstants.OrdersTable} o
                LEFT JOIN {ApplicationConstants.CustomersTable} c ON o.customer_id = c.id
                WHERE o.id = @id";
            
            using var reader = await ExecuteReaderAsync(sql, CreateParameter("id", id));
            
            if (await reader.ReadAsync())
            {
                return MapOrderFromReader(reader);
            }
            
            return null;
        }

        /// <summary>
        /// Creates a new order
        /// </summary>
        public async Task<OperationResult<Order>> CreateAsync(Order order)
        {
            try
            {
                var validation = ValidationService.ValidateOrder(order);
                if (!validation.IsValid)
                {
                    return OperationResult<Order>.Failure(validation.GetErrorsAsString());
                }

                // Verify customer exists
                var customerExists = await CustomerExistsAsync(order.CustomerId);
                if (!customerExists)
                {
                    return OperationResult<Order>.Failure("El cliente especificado no existe");
                }

                var sql = $@"
                    INSERT INTO {ApplicationConstants.OrdersTable} (order_number, order_date, status, customer_id) 
                    VALUES (@orderNumber, @orderDate, @status, @customerId) 
                    RETURNING id";

                var parameters = new[]
                {
                    CreateParameter("orderNumber", order.OrderNumber),
                    CreateParameter("orderDate", order.OrderDate),
                    CreateParameter("status", order.Status.ToString()),
                    CreateParameter("customerId", order.CustomerId)
                };

                var result = await ExecuteScalarAsync(sql, parameters);
                if (result != null)
                {
                    order.Id = Convert.ToInt32(result);
                    return OperationResult<Order>.Success(order, ApplicationConstants.SuccessMessage);
                }

                return OperationResult<Order>.Failure(ApplicationConstants.ErrorMessage);
            }
            catch (Exception ex)
            {
                return OperationResult<Order>.Failure($"Error al crear pedido: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing order
        /// </summary>
        public async Task<OperationResult<bool>> UpdateAsync(Order order)
        {
            try
            {
                var validation = ValidationService.ValidateOrder(order);
                if (!validation.IsValid)
                {
                    return OperationResult<bool>.Failure(validation.GetErrorsAsString());
                }

                // Verify customer exists
                var customerExists = await CustomerExistsAsync(order.CustomerId);
                if (!customerExists)
                {
                    return OperationResult<bool>.Failure("El cliente especificado no existe");
                }

                var sql = $@"
                    UPDATE {ApplicationConstants.OrdersTable} 
                    SET order_number = @orderNumber, order_date = @orderDate, status = @status, customer_id = @customerId 
                    WHERE id = @id";

                var parameters = new[]
                {
                    CreateParameter("id", order.Id),
                    CreateParameter("orderNumber", order.OrderNumber),
                    CreateParameter("orderDate", order.OrderDate),
                    CreateParameter("status", order.Status.ToString()),
                    CreateParameter("customerId", order.CustomerId)
                };

                var rowsAffected = await ExecuteNonQueryAsync(sql, parameters);
                
                if (rowsAffected > 0)
                {
                    return OperationResult<bool>.Success(ApplicationConstants.SuccessMessage);
                }

                return OperationResult<bool>.Failure(ApplicationConstants.NotFoundMessage);
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.Failure($"Error al actualizar pedido: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes an order by ID
        /// </summary>
        public async Task<OperationResult<bool>> DeleteAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return OperationResult<bool>.Failure("ID de pedido inválido");
                }

                var sql = $"DELETE FROM {ApplicationConstants.OrdersTable} WHERE id = @id";
                var rowsAffected = await ExecuteNonQueryAsync(sql, CreateParameter("id", id));
                
                if (rowsAffected > 0)
                {
                    return OperationResult<bool>.Success(ApplicationConstants.SuccessMessage);
                }

                return OperationResult<bool>.Failure(ApplicationConstants.NotFoundMessage);
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.Failure($"Error al eliminar pedido: {ex.Message}");
            }
        }

        /// <summary>
        /// Checks if a customer exists
        /// </summary>
        private async Task<bool> CustomerExistsAsync(int customerId)
        {
            var sql = $"SELECT COUNT(1) FROM {ApplicationConstants.CustomersTable} WHERE id = @id";
            var result = await ExecuteScalarAsync(sql, CreateParameter("id", customerId));
            return Convert.ToInt32(result) > 0;
        }

        /// <summary>
        /// Maps a data reader row to an Order object
        /// </summary>
        private static Order MapOrderFromReader(NpgsqlDataReader reader)
        {
            return new Order
            {
                Id = reader.GetInt32(0),
                OrderNumber = reader.GetString(1),
                OrderDate = reader.GetDateTime(2),
                Status = Enum.Parse<OrderStatus>(reader.GetString(3)),
                CustomerId = reader.GetInt32(4),
                Customer = new Customer
                {
                    Id = reader.GetInt32(4),
                    FirstName = reader.GetString(5),
                    LastName = reader.GetString(6)
                }
            };
        }
    }
}
