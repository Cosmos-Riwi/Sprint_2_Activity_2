using Npgsql;
using RestaurantSystem.Models;
using RestaurantSystem.Configuration;
using RestaurantSystem.Data;

namespace RestaurantSystem.Services
{
    /// <summary>
    /// Service for dish-related database operations
    /// </summary>
    public class DishService : BaseService
    {
        public DishService(PostgreSqlDbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// Gets all dishes from the database
        /// </summary>
        public async Task<List<Dish>> GetAllAsync()
        {
            var dishes = new List<Dish>();
            var sql = $"SELECT id, name, description, price, category FROM {ApplicationConstants.DishesTable} ORDER BY id";
            
            using var reader = await ExecuteReaderAsync(sql);
            
            while (await reader.ReadAsync())
            {
                dishes.Add(MapDishFromReader(reader));
            }
            
            return dishes;
        }

        /// <summary>
        /// Gets a dish by ID
        /// </summary>
        public async Task<Dish?> GetByIdAsync(int id)
        {
            if (id <= 0)
                return null;

            var sql = $"SELECT id, name, description, price, category FROM {ApplicationConstants.DishesTable} WHERE id = @id";
            using var reader = await ExecuteReaderAsync(sql, CreateParameter("id", id));
            
            if (await reader.ReadAsync())
            {
                return MapDishFromReader(reader);
            }
            
            return null;
        }

        /// <summary>
        /// Creates a new dish
        /// </summary>
        public async Task<OperationResult<Dish>> CreateAsync(Dish dish)
        {
            try
            {
                var validation = ValidationService.ValidateDish(dish);
                if (!validation.IsValid)
                {
                    return OperationResult<Dish>.Failure(validation.GetErrorsAsString());
                }

                var sql = $@"
                    INSERT INTO {ApplicationConstants.DishesTable} (name, description, price, category) 
                    VALUES (@name, @description, @price, @category) 
                    RETURNING id";

                var parameters = new[]
                {
                    CreateParameter("name", dish.Name),
                    CreateParameter("description", string.IsNullOrWhiteSpace(dish.Description) ? null : dish.Description),
                    CreateParameter("price", dish.Price),
                    CreateParameter("category", dish.Category.ToString())
                };

                var result = await ExecuteScalarAsync(sql, parameters);
                if (result != null)
                {
                    dish.Id = Convert.ToInt32(result);
                    return OperationResult<Dish>.Success(dish, ApplicationConstants.SuccessMessage);
                }

                return OperationResult<Dish>.Failure(ApplicationConstants.ErrorMessage);
            }
            catch (Exception ex)
            {
                return OperationResult<Dish>.Failure($"Error al crear plato: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing dish
        /// </summary>
        public async Task<OperationResult<bool>> UpdateAsync(Dish dish)
        {
            try
            {
                var validation = ValidationService.ValidateDish(dish);
                if (!validation.IsValid)
                {
                    return OperationResult<bool>.Failure(validation.GetErrorsAsString());
                }

                var sql = $@"
                    UPDATE {ApplicationConstants.DishesTable} 
                    SET name = @name, description = @description, price = @price, category = @category 
                    WHERE id = @id";

                var parameters = new[]
                {
                    CreateParameter("id", dish.Id),
                    CreateParameter("name", dish.Name),
                    CreateParameter("description", string.IsNullOrWhiteSpace(dish.Description) ? null : dish.Description),
                    CreateParameter("price", dish.Price),
                    CreateParameter("category", dish.Category.ToString())
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
                return OperationResult<bool>.Failure($"Error al actualizar plato: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a dish by ID
        /// </summary>
        public async Task<OperationResult<bool>> DeleteAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return OperationResult<bool>.Failure("ID de plato inválido");
                }

                var sql = $"DELETE FROM {ApplicationConstants.DishesTable} WHERE id = @id";
                var rowsAffected = await ExecuteNonQueryAsync(sql, CreateParameter("id", id));
                
                if (rowsAffected > 0)
                {
                    return OperationResult<bool>.Success(ApplicationConstants.SuccessMessage);
                }

                return OperationResult<bool>.Failure(ApplicationConstants.NotFoundMessage);
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.Failure($"Error al eliminar plato: {ex.Message}");
            }
        }

        /// <summary>
        /// Maps a data reader row to a Dish object
        /// </summary>
        private static Dish MapDishFromReader(NpgsqlDataReader reader)
        {
            return new Dish
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Description = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                Price = reader.GetDecimal(3),
                Category = Enum.Parse<DishCategory>(reader.GetString(4))
            };
        }
    }
}
