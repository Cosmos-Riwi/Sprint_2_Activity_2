using Npgsql;
using RestaurantSystem.Configuration;
using RestaurantSystem.Data;
using System.Data;

namespace RestaurantSystem.Services
{
    /// <summary>
    /// Base service class for common database operations
    /// </summary>
    public abstract class BaseService
    {
        protected readonly PostgreSqlDbContext _dbContext;

        protected BaseService(PostgreSqlDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        /// <summary>
        /// Executes a command and returns the number of affected rows
        /// </summary>
        protected async Task<int> ExecuteNonQueryAsync(string sql, params NpgsqlParameter[] parameters)
        {
            try
            {
                using var connection = await _dbContext.GetConnectionAsync();
                using var command = new NpgsqlCommand(sql, connection);
                
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }
                
                return await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error executing command: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Executes a command and returns a scalar value
        /// </summary>
        protected async Task<object?> ExecuteScalarAsync(string sql, params NpgsqlParameter[] parameters)
        {
            try
            {
                using var connection = await _dbContext.GetConnectionAsync();
                using var command = new NpgsqlCommand(sql, connection);
                
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }
                
                return await command.ExecuteScalarAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error executing scalar command: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Executes a command and returns a data reader
        /// </summary>
        protected async Task<NpgsqlDataReader> ExecuteReaderAsync(string sql, params NpgsqlParameter[] parameters)
        {
            try
            {
                var connection = await _dbContext.GetConnectionAsync();
                var command = new NpgsqlCommand(sql, connection);
                
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }
                
                return await command.ExecuteReaderAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error executing reader command: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Creates a parameter with the specified name and value
        /// </summary>
        protected static NpgsqlParameter CreateParameter(string name, object? value)
        {
            return new NpgsqlParameter(name, value ?? DBNull.Value);
        }
    }
}
