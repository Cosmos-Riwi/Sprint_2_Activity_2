using System.ComponentModel.DataAnnotations;

namespace RestaurantSystem.Models
{
    /// <summary>
    /// Represents an order entity in the restaurant system
    /// Entidad: Pedidos - id, numero_pedido, fecha, estado (pendiente, servido, cancelado)
    /// Relación: un cliente puede tener varios pedidos
    /// </summary>
    public class Order
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "El número de pedido es requerido")]
        [StringLength(50, ErrorMessage = "El número de pedido no puede exceder 50 caracteres")]
        public string OrderNumber { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "La fecha es requerida")]
        public DateTime OrderDate { get; set; }
        
        [Required(ErrorMessage = "El estado es requerido")]
        public OrderStatus Status { get; set; }
        
        [Required(ErrorMessage = "El cliente es requerido")]
        public int CustomerId { get; set; }
        
        /// <summary>
        /// Navigation property to customer
        /// </summary>
        public Customer? Customer { get; set; }
    }

    /// <summary>
    /// Enum for order status
    /// </summary>
    public enum OrderStatus
    {
        Pending,    // pendiente
        Served,     // servido
        Cancelled   // cancelado
    }
}
