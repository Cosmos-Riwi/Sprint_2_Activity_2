using System.ComponentModel.DataAnnotations;

namespace RestaurantSystem.Models
{
    /// <summary>
    /// Represents a dish entity in the restaurant system
    /// Entidad: Platos - id, nombre, descripción, precio, categoría (entrada, plato fuerte, postre, bebida)
    /// </summary>
    public class Dish
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
        public string Description { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "El precio es requerido")]
        [Range(0.01, 999999.99, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal Price { get; set; }
        
        [Required(ErrorMessage = "La categoría es requerida")]
        public DishCategory Category { get; set; }
    }

    /// <summary>
    /// Enum for dish categories
    /// </summary>
    public enum DishCategory
    {
        Appetizer,      // entrada
        MainCourse,     // plato fuerte
        Dessert,        // postre
        Beverage        // bebida
    }
}
