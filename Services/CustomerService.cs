using Npgsql;
using RestaurantSystem.Models;
using RestaurantSystem.Configuration;
using RestaurantSystem.Data;

namespace RestaurantSystem.Services
{
    /// <summary>
    /// Service for customer-related database operations
    /// </summary>
    public class CustomerService : BaseService
    {
        public CustomerService(PostgreSqlDbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// Gets all customers from the database
        /// </summary>
        public async Task<List<Customer>> GetAllAsync()
        {
            var customers = new List<Customer>();
            var sql = $"SELECT id, first_name, last_name, email, phone FROM {ApplicationConstants.CustomersTable} ORDER BY id";
            
            using var reader = await ExecuteReaderAsync(sql);
            
            while (await reader.ReadAsync())
            {
                customers.Add(MapCustomerFromReader(reader));
            }
            
            return customers;
        }

        /// <summary>
        /// Gets a customer by ID
        /// </summary>
        public async Task<Customer?> GetByIdAsync(int id)
        {
            if (id <= 0)
                return null;

            var sql = $"SELECT id, first_name, last_name, email, phone FROM {ApplicationConstants.CustomersTable} WHERE id = @id";
            using var reader = await ExecuteReaderAsync(sql, CreateParameter("id", id));
            
            if (await reader.ReadAsync())
            {
                return MapCustomerFromReader(reader);
            }
            
            return null;
        }

        /// <summary>
        /// Creates a new customer
        /// </summary>
        public async Task<OperationResult<Customer>> CreateAsync(Customer customer)
        {
            try
            {
                var validation = ValidationService.ValidateCustomer(customer);
                if (!validation.IsValid)
                {
                    return OperationResult<Customer>.Failure(validation.GetErrorsAsString());
                }

                var sql = $@"
                    INSERT INTO {ApplicationConstants.CustomersTable} (first_name, last_name, email, phone) 
                    VALUES (@firstName, @lastName, @email, @phone) 
                    RETURNING id";

                var parameters = new[]
                {
                    CreateParameter("firstName", customer.FirstName),
                    CreateParameter("lastName", customer.LastName),
                    CreateParameter("email", customer.Email),
                    CreateParameter("phone", customer.Phone)
                };

                var result = await ExecuteScalarAsync(sql, parameters);
                if (result != null)
                {
                    customer.Id = Convert.ToInt32(result);
                    return OperationResult<Customer>.Success(customer, ApplicationConstants.SuccessMessage);
                }

                return OperationResult<Customer>.Failure(ApplicationConstants.ErrorMessage);
            }
            catch (Exception ex)
            {
                return OperationResult<Customer>.Failure($"Error al crear cliente: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing customer
        /// </summary>
        public async Task<OperationResult<bool>> UpdateAsync(Customer customer)
        {
            try
            {
                var validation = ValidationService.ValidateCustomer(customer);
                if (!validation.IsValid)
                {
                    return OperationResult<bool>.Failure(validation.GetErrorsAsString());
                }

                var sql = $@"
                    UPDATE {ApplicationConstants.CustomersTable} 
                    SET first_name = @firstName, last_name = @lastName, email = @email, phone = @phone 
                    WHERE id = @id";

                var parameters = new[]
                {
                    CreateParameter("id", customer.Id),
                    CreateParameter("firstName", customer.FirstName),
                    CreateParameter("lastName", customer.LastName),
                    CreateParameter("email", customer.Email),
                    CreateParameter("phone", customer.Phone)
                };

                var rowsAffected = await ExecuteNonQueryAsync(sql, parameters);
                
                if (rowsAffected > 0)
                {
                    return OperationResult<bool>.Success(true, ApplicationConstants.SuccessMessage);
                }

                return OperationResult<bool>.Failure(ApplicationConstants.NotFoundMessage);
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.Failure($"Error al actualizar cliente: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a customer by ID
        /// </summary>
        public async Task<OperationResult<bool>> DeleteAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return OperationResult<bool>.Failure("ID de cliente inválido");
                }

                var sql = $"DELETE FROM {ApplicationConstants.CustomersTable} WHERE id = @id";
                var rowsAffected = await ExecuteNonQueryAsync(sql, CreateParameter("id", id));
                
                if (rowsAffected > 0)
                {
                    return OperationResult<bool>.Success(true, ApplicationConstants.SuccessMessage);
                }

                return OperationResult<bool>.Failure(ApplicationConstants.NotFoundMessage);
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.Failure($"Error al eliminar cliente: {ex.Message}");
            }
        }

        /// <summary>
        /// Maps a data reader row to a Customer object
        /// </summary>
        private static Customer MapCustomerFromReader(NpgsqlDataReader reader)
        {
            return new Customer
            {
                Id = reader.GetInt32(0),
                FirstName = reader.GetString(1),
                LastName = reader.GetString(2),
                Email = reader.GetString(3),
                Phone = reader.GetString(4)
            };
        }
    }
}
