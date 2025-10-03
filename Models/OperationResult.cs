namespace RestaurantSystem.Models
{
    /// <summary>
    /// Represents the result of an operation with success/failure status and message
    /// </summary>
    /// <typeparam name="T">Type of the result data</typeparam>
    public class OperationResult<T>
    {
        public bool IsSuccess { get; private set; }
        public T? Data { get; private set; }
        public string Message { get; private set; } = string.Empty;
        public string? ErrorDetails { get; private set; }

        private OperationResult(bool isSuccess, T? data, string message, string? errorDetails = null)
        {
            IsSuccess = isSuccess;
            Data = data;
            Message = message;
            ErrorDetails = errorDetails;
        }

        /// <summary>
        /// Creates a successful operation result
        /// </summary>
        public static OperationResult<T> Success(T data, string message = "Operación exitosa")
        {
            return new OperationResult<T>(true, data, message);
        }

        /// <summary>
        /// Creates a failed operation result
        /// </summary>
        public static OperationResult<T> Failure(string message, string? errorDetails = null)
        {
            return new OperationResult<T>(false, default, message, errorDetails);
        }

        /// <summary>
        /// Creates a successful operation result without data
        /// </summary>
        public static OperationResult<bool> Success(string message = "Operación exitosa")
        {
            return new OperationResult<bool>(true, true, message);
        }
    }
}
