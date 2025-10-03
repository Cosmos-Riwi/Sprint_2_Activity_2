using RestaurantSystem.Models;
using RestaurantSystem.Configuration;
using System.Text.RegularExpressions;

namespace RestaurantSystem.Services
{
    /// <summary>
    /// Service for validating business entities
    /// </summary>
    public static class ValidationService
    {
        private static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);
        private static readonly Regex PhoneRegex = new(@"^[\+]?[1-9][\d]{0,15}$", RegexOptions.Compiled);

        /// <summary>
        /// Validates a customer entity
        /// </summary>
        public static ValidationResult ValidateCustomer(Customer customer)
        {
            var result = new ValidationResult();

            if (string.IsNullOrWhiteSpace(customer.FirstName))
                result.AddError("El nombre es requerido");
            else if (customer.FirstName.Length > ApplicationConstants.MaxNameLength)
                result.AddError($"El nombre no puede exceder {ApplicationConstants.MaxNameLength} caracteres");

            if (string.IsNullOrWhiteSpace(customer.LastName))
                result.AddError("El apellido es requerido");
            else if (customer.LastName.Length > ApplicationConstants.MaxNameLength)
                result.AddError($"El apellido no puede exceder {ApplicationConstants.MaxNameLength} caracteres");

            if (string.IsNullOrWhiteSpace(customer.Email))
                result.AddError("El correo electrónico es requerido");
            else if (customer.Email.Length > ApplicationConstants.MaxEmailLength)
                result.AddError($"El correo no puede exceder {ApplicationConstants.MaxEmailLength} caracteres");
            else if (!EmailRegex.IsMatch(customer.Email))
                result.AddError("El formato del correo electrónico no es válido");

            if (string.IsNullOrWhiteSpace(customer.Phone))
                result.AddError("El teléfono es requerido");
            else if (customer.Phone.Length > ApplicationConstants.MaxPhoneLength)
                result.AddError($"El teléfono no puede exceder {ApplicationConstants.MaxPhoneLength} caracteres");
            else if (!PhoneRegex.IsMatch(customer.Phone))
                result.AddError("El formato del teléfono no es válido");

            return result;
        }

        /// <summary>
        /// Validates a waiter entity
        /// </summary>
        public static ValidationResult ValidateWaiter(Waiter waiter)
        {
            var result = new ValidationResult();

            if (string.IsNullOrWhiteSpace(waiter.FirstName))
                result.AddError("El nombre es requerido");
            else if (waiter.FirstName.Length > ApplicationConstants.MaxNameLength)
                result.AddError($"El nombre no puede exceder {ApplicationConstants.MaxNameLength} caracteres");

            if (string.IsNullOrWhiteSpace(waiter.LastName))
                result.AddError("El apellido es requerido");
            else if (waiter.LastName.Length > ApplicationConstants.MaxNameLength)
                result.AddError($"El apellido no puede exceder {ApplicationConstants.MaxNameLength} caracteres");

            if (string.IsNullOrWhiteSpace(waiter.Shift))
                result.AddError("El turno es requerido");

            if (waiter.YearsOfExperience < 0)
                result.AddError("Los años de experiencia no pueden ser negativos");

            return result;
        }

        /// <summary>
        /// Validates a dish entity
        /// </summary>
        public static ValidationResult ValidateDish(Dish dish)
        {
            var result = new ValidationResult();

            if (string.IsNullOrWhiteSpace(dish.Name))
                result.AddError("El nombre del plato es requerido");
            else if (dish.Name.Length > ApplicationConstants.MaxNameLength)
                result.AddError($"El nombre no puede exceder {ApplicationConstants.MaxNameLength} caracteres");

            if (dish.Description.Length > ApplicationConstants.MaxDescriptionLength)
                result.AddError($"La descripción no puede exceder {ApplicationConstants.MaxDescriptionLength} caracteres");

            if (dish.Price < ApplicationConstants.MinPrice)
                result.AddError($"El precio debe ser mayor a {ApplicationConstants.MinPrice:C}");

            if (!Enum.IsDefined(typeof(DishCategory), dish.Category))
                result.AddError("La categoría del plato no es válida");

            return result;
        }

        /// <summary>
        /// Validates an order entity
        /// </summary>
        public static ValidationResult ValidateOrder(Order order)
        {
            var result = new ValidationResult();

            if (string.IsNullOrWhiteSpace(order.OrderNumber))
                result.AddError("El número de pedido es requerido");

            if (!Enum.IsDefined(typeof(OrderStatus), order.Status))
                result.AddError("El estado del pedido no es válido");

            if (order.CustomerId <= 0)
                result.AddError("Debe seleccionar un cliente válido");

            if (order.OrderDate > DateTime.Now.AddDays(1))
                result.AddError("La fecha del pedido no puede ser futura");

            return result;
        }

        /// <summary>
        /// Validates a reservation entity
        /// </summary>
        public static ValidationResult ValidateReservation(Reservation reservation)
        {
            var result = new ValidationResult();

            if (reservation.NumberOfPeople < ApplicationConstants.MinPeopleCount)
                result.AddError($"El número de personas debe ser mayor a {ApplicationConstants.MinPeopleCount - 1}");
            else if (reservation.NumberOfPeople > ApplicationConstants.MaxPeopleCount)
                result.AddError($"El número de personas no puede exceder {ApplicationConstants.MaxPeopleCount}");

            if (reservation.CustomerId <= 0)
                result.AddError("Debe seleccionar un cliente válido");

            if (reservation.ReservationDate < DateTime.Today)
                result.AddError("La fecha de reserva no puede ser anterior a hoy");

            if (reservation.Notes.Length > ApplicationConstants.MaxNotesLength)
                result.AddError($"Las observaciones no pueden exceder {ApplicationConstants.MaxNotesLength} caracteres");

            return result;
        }
    }

    /// <summary>
    /// Represents the result of a validation operation
    /// </summary>
    public class ValidationResult
    {
        public List<string> Errors { get; } = new List<string>();
        public bool IsValid => Errors.Count == 0;

        public void AddError(string error)
        {
            if (!string.IsNullOrWhiteSpace(error))
                Errors.Add(error);
        }

        public string GetErrorsAsString()
        {
            return string.Join("\n", Errors);
        }
    }
}
