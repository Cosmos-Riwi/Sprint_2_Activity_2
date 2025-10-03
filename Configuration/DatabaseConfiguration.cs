namespace RestaurantSystem.Configuration
{
    /// <summary>
    /// Database configuration constants
    /// </summary>
    public static class DatabaseConfiguration
    {
        public const string Host = "168.119.183.3";
        public const int Port = 5432;
        public const string Username = "root";
        public const string Password = "s7cq453mt2jnicTaQXKT";
        public const string DatabaseName = "restaurante_jeronimo";
        
        /// <summary>
        /// Connection string for PostgreSQL database
        /// </summary>
        public static string ConnectionString => 
            $"Host={Host};Port={Port};Username={Username};Password={Password};Database={DatabaseName};";
    }
}
