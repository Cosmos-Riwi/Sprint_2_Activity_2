using Npgsql;
using RestaurantSystem.Configuration;
using System.Data;

namespace RestaurantSystem.Data
{
    /// <summary>
    /// PostgreSQL database context for managing database operations
    /// </summary>
    public class PostgreSqlDbContext : IDisposable
    {
        private readonly string _connectionString;
        private NpgsqlConnection? _connection;
        private bool _disposed = false;

        public PostgreSqlDbContext()
        {
            _connectionString = DatabaseConfiguration.ConnectionString;
        }

        /// <summary>
        /// Gets a database connection
        /// </summary>
        public async Task<NpgsqlConnection> GetConnectionAsync()
        {
            if (_connection == null || _connection.State != ConnectionState.Open)
            {
                _connection = new NpgsqlConnection(_connectionString);
                await _connection.OpenAsync();
            }
            return _connection;
        }

        /// <summary>
        /// Tests the database connection
        /// </summary>
        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();
                return connection.State == ConnectionState.Open;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error de conexion: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Initializes database tables
        /// </summary>
        public async Task InitializeTablesAsync()
        {
            try
            {
                using var connection = await GetConnectionAsync();
                
                var createTablesScript = GetCreateTablesScript();
                using var command = new NpgsqlCommand(createTablesScript, connection);
                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al inicializar las tablas: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets the SQL script for creating tables
        /// </summary>
        private string GetCreateTablesScript()
        {
            return $@"
                -- Create customers table
                CREATE TABLE IF NOT EXISTS {ApplicationConstants.CustomersTable} (
                    id SERIAL PRIMARY KEY,
                    first_name VARCHAR({ApplicationConstants.MaxNameLength}) NOT NULL,
                    last_name VARCHAR({ApplicationConstants.MaxNameLength}) NOT NULL,
                    email VARCHAR({ApplicationConstants.MaxEmailLength}) UNIQUE NOT NULL,
                    phone VARCHAR({ApplicationConstants.MaxPhoneLength}) NOT NULL,
                    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
                );

                -- Create waiters table
                CREATE TABLE IF NOT EXISTS {ApplicationConstants.WaitersTable} (
                    id SERIAL PRIMARY KEY,
                    first_name VARCHAR({ApplicationConstants.MaxNameLength}) NOT NULL,
                    last_name VARCHAR({ApplicationConstants.MaxNameLength}) NOT NULL,
                    shift VARCHAR(50) NOT NULL,
                    years_of_experience INTEGER NOT NULL CHECK (years_of_experience >= 0),
                    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
                );

                -- Create dishes table
                CREATE TABLE IF NOT EXISTS {ApplicationConstants.DishesTable} (
                    id SERIAL PRIMARY KEY,
                    name VARCHAR({ApplicationConstants.MaxNameLength}) NOT NULL,
                    description TEXT,
                    price DECIMAL(10,2) NOT NULL CHECK (price >= {ApplicationConstants.MinPrice.ToString("F2", System.Globalization.CultureInfo.InvariantCulture)}),
                    category VARCHAR(50) NOT NULL CHECK (category IN ('Appetizer', 'MainCourse', 'Dessert', 'Beverage')),
                    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
                );

                -- Create orders table
                CREATE TABLE IF NOT EXISTS {ApplicationConstants.OrdersTable} (
                    id SERIAL PRIMARY KEY,
                    order_number VARCHAR(50) UNIQUE NOT NULL,
                    order_date TIMESTAMP NOT NULL,
                    status VARCHAR(20) NOT NULL CHECK (status IN ('Pending', 'Served', 'Cancelled')),
                    customer_id INTEGER NOT NULL,
                    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                    FOREIGN KEY (customer_id) REFERENCES {ApplicationConstants.CustomersTable}(id) ON DELETE CASCADE
                );

                -- Create reservations table
                CREATE TABLE IF NOT EXISTS {ApplicationConstants.ReservationsTable} (
                    id SERIAL PRIMARY KEY,
                    reservation_date DATE NOT NULL,
                    reservation_time TIME NOT NULL,
                    number_of_people INTEGER NOT NULL CHECK (number_of_people >= {ApplicationConstants.MinPeopleCount} AND number_of_people <= {ApplicationConstants.MaxPeopleCount}),
                    notes TEXT,
                    customer_id INTEGER NOT NULL,
                    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                    FOREIGN KEY (customer_id) REFERENCES {ApplicationConstants.CustomersTable}(id) ON DELETE CASCADE
                );

                -- Create indexes for better performance
                CREATE INDEX IF NOT EXISTS idx_orders_customer_id ON {ApplicationConstants.OrdersTable}(customer_id);
                CREATE INDEX IF NOT EXISTS idx_orders_date ON {ApplicationConstants.OrdersTable}(order_date);
                CREATE INDEX IF NOT EXISTS idx_reservations_customer_id ON {ApplicationConstants.ReservationsTable}(customer_id);
                CREATE INDEX IF NOT EXISTS idx_reservations_date ON {ApplicationConstants.ReservationsTable}(reservation_date);
            ";
        }

        /// <summary>
        /// Disposes the database context
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _connection?.Dispose();
                _disposed = true;
            }
        }
    }
}
