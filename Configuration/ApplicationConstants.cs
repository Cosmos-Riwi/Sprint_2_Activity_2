namespace RestaurantSystem.Configuration
{
    /// <summary>
    /// Application constants and configuration values
    /// </summary>
    public static class ApplicationConstants
    {
        // Database table names
        public const string CustomersTable = "customers";
        public const string WaitersTable = "waiters";
        public const string DishesTable = "dishes";
        public const string OrdersTable = "orders";
        public const string ReservationsTable = "reservations";
        
        // Validation constants
        public const int MaxNameLength = 100;
        public const int MaxEmailLength = 150;
        public const int MaxPhoneLength = 20;
        public const int MaxDescriptionLength = 500;
        public const int MaxNotesLength = 1000;
        public const decimal MinPrice = 0.01m;
        public const int MinPeopleCount = 1;
        public const int MaxPeopleCount = 20;
        
        // Application messages
        public const string SuccessMessage = "Operación completada exitosamente";
        public const string ErrorMessage = "Error en la operación";
        public const string ValidationErrorMessage = "Error de validación";
        public const string NotFoundMessage = "Registro no encontrado";
        public const string ConnectionErrorMessage = "Error de conexión a la base de datos";
    }
}
