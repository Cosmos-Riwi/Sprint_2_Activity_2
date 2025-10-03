using System.ComponentModel.DataAnnotations;

namespace RestaurantSystem.Models
{
    /// <summary>
    /// Represents a waiter entity in the restaurant system
    /// Entidad: Meseros - id, nombre, apellido, turno, años_experiencia
    /// </summary>
    public class Waiter
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string FirstName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "El apellido es requerido")]
        [StringLength(100, ErrorMessage = "El apellido no puede exceder 100 caracteres")]
        public string LastName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "El turno es requerido")]
        [StringLength(50, ErrorMessage = "El turno no puede exceder 50 caracteres")]
        public string Shift { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Los años de experiencia son requeridos")]
        [Range(0, 50, ErrorMessage = "Los años de experiencia deben estar entre 0 y 50")]
        public int YearsOfExperience { get; set; }
        
        /// <summary>
        /// Full name property for display purposes
        /// </summary>
        public string FullName => $"{FirstName} {LastName}".Trim();
    }
}
