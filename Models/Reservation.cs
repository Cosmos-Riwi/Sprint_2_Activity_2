using System.ComponentModel.DataAnnotations;

namespace RestaurantSystem.Models
{
    /// <summary>
    /// Represents a reservation entity in the restaurant system
    /// Entidad: Reservas - id, fecha, hora, número_personas, observaciones, cliente_id
    /// Relación: un cliente puede tener varias reservas
    /// </summary>
    public class Reservation
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "La fecha es requerida")]
        public DateTime ReservationDate { get; set; }
        
        [Required(ErrorMessage = "La hora es requerida")]
        public TimeSpan ReservationTime { get; set; }
        
        [Required(ErrorMessage = "El número de personas es requerido")]
        [Range(1, 50, ErrorMessage = "El número de personas debe ser mayor a 0 y menor a 50")]
        public int NumberOfPeople { get; set; }
        
        [StringLength(500, ErrorMessage = "Las observaciones no pueden exceder 500 caracteres")]
        public string Notes { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "El cliente es requerido")]
        public int CustomerId { get; set; }
        
        /// <summary>
        /// Navigation property to customer
        /// </summary>
        public Customer? Customer { get; set; }
        
        /// <summary>
        /// Combined date and time for display purposes
        /// </summary>
        public DateTime FullDateTime => ReservationDate.Date.Add(ReservationTime);
    }
}
