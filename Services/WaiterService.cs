using Npgsql;
using RestaurantSystem.Models;
using RestaurantSystem.Configuration;
using RestaurantSystem.Data;

namespace RestaurantSystem.Services
{
    /// <summary>
    /// Service for waiter-related database operations
    /// </summary>
    public class WaiterService : BaseService
    {
        public WaiterService(PostgreSqlDbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// Gets all waiters from the database
        /// </summary>
        public async Task<List<Waiter>> GetAllAsync()
        {
            var waiters = new List<Waiter>();
            var sql = $"SELECT id, first_name, last_name, shift, years_of_experience FROM {ApplicationConstants.WaitersTable} ORDER BY id";
            
            using var reader = await ExecuteReaderAsync(sql);
            
            while (await reader.ReadAsync())
            {
                waiters.Add(MapWaiterFromReader(reader));
            }
            
            return waiters;
        }

        /// <summary>
        /// Gets a waiter by ID
        /// </summary>
        public async Task<Waiter?> GetByIdAsync(int id)
        {
            if (id <= 0)
                return null;

            var sql = $"SELECT id, first_name, last_name, shift, years_of_experience FROM {ApplicationConstants.WaitersTable} WHERE id = @id";
            using var reader = await ExecuteReaderAsync(sql, CreateParameter("id", id));
            
            if (await reader.ReadAsync())
            {
                return MapWaiterFromReader(reader);
            }
            
            return null;
        }

        /// <summary>
        /// Creates a new waiter
        /// </summary>
        public async Task<OperationResult<Waiter>> CreateAsync(Waiter waiter)
        {
            try
            {
                var validation = ValidationService.ValidateWaiter(waiter);
                if (!validation.IsValid)
                {
                    return OperationResult<Waiter>.Failure(validation.GetErrorsAsString());
                }

                var sql = $@"
                    INSERT INTO {ApplicationConstants.WaitersTable} (first_name, last_name, shift, years_of_experience) 
                    VALUES (@firstName, @lastName, @shift, @yearsOfExperience) 
                    RETURNING id";

                var parameters = new[]
                {
                    CreateParameter("firstName", waiter.FirstName),
                    CreateParameter("lastName", waiter.LastName),
                    CreateParameter("shift", waiter.Shift),
                    CreateParameter("yearsOfExperience", waiter.YearsOfExperience)
                };

                var result = await ExecuteScalarAsync(sql, parameters);
                if (result != null)
                {
                    waiter.Id = Convert.ToInt32(result);
                    return OperationResult<Waiter>.Success(waiter, ApplicationConstants.SuccessMessage);
                }

                return OperationResult<Waiter>.Failure(ApplicationConstants.ErrorMessage);
            }
            catch (Exception ex)
            {
                return OperationResult<Waiter>.Failure($"Error al crear mesero: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing waiter
        /// </summary>
        public async Task<OperationResult<bool>> UpdateAsync(Waiter waiter)
        {
            try
            {
                var validation = ValidationService.ValidateWaiter(waiter);
                if (!validation.IsValid)
                {
                    return OperationResult<bool>.Failure(validation.GetErrorsAsString());
                }

                var sql = $@"
                    UPDATE {ApplicationConstants.WaitersTable} 
                    SET first_name = @firstName, last_name = @lastName, shift = @shift, years_of_experience = @yearsOfExperience 
                    WHERE id = @id";

                var parameters = new[]
                {
                    CreateParameter("id", waiter.Id),
                    CreateParameter("firstName", waiter.FirstName),
                    CreateParameter("lastName", waiter.LastName),
                    CreateParameter("shift", waiter.Shift),
                    CreateParameter("yearsOfExperience", waiter.YearsOfExperience)
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
                return OperationResult<bool>.Failure($"Error al actualizar mesero: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a waiter by ID
        /// </summary>
        public async Task<OperationResult<bool>> DeleteAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return OperationResult<bool>.Failure("ID de mesero inválido");
                }

                var sql = $"DELETE FROM {ApplicationConstants.WaitersTable} WHERE id = @id";
                var rowsAffected = await ExecuteNonQueryAsync(sql, CreateParameter("id", id));
                
                if (rowsAffected > 0)
                {
                    return OperationResult<bool>.Success(ApplicationConstants.SuccessMessage);
                }

                return OperationResult<bool>.Failure(ApplicationConstants.NotFoundMessage);
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.Failure($"Error al eliminar mesero: {ex.Message}");
            }
        }

        /// <summary>
        /// Maps a data reader row to a Waiter object
        /// </summary>
        private static Waiter MapWaiterFromReader(NpgsqlDataReader reader)
        {
            return new Waiter
            {
                Id = reader.GetInt32(0),
                FirstName = reader.GetString(1),
                LastName = reader.GetString(2),
                Shift = reader.GetString(3),
                YearsOfExperience = reader.GetInt32(4)
            };
        }
    }
}
