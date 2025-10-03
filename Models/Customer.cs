using System.ComponentModel.DataAnnotations;

namespace RestaurantSystem.Models
{
    /// <summary>
    /// Represents a customer entity in the restaurant system
    /// Entidad: Clientes - id, nombre, apellido, correo, teléfono
    /// </summary>
    public class Customer
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string FirstName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "El apellido es requerido")]
        [StringLength(100, ErrorMessage = "El apellido no puede exceder 100 caracteres")]
        public string LastName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "El correo es requerido")]
        [EmailAddress(ErrorMessage = "Formato de correo inválido")]
        [StringLength(255, ErrorMessage = "El correo no puede exceder 255 caracteres")]
        public string Email { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "El teléfono es requerido")]
        [Phone(ErrorMessage = "Formato de teléfono inválido")]
        [StringLength(20, ErrorMessage = "El teléfono no puede exceder 20 caracteres")]
        public string Phone { get; set; } = string.Empty;
        
        /// <summary>
        /// Full name property for display purposes
        /// </summary>
        public string FullName => $"{FirstName} {LastName}".Trim();
    }
}
