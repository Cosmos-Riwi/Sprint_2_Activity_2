using System;
using System.ComponentModel.DataAnnotations;

namespace GestionRestaurante.Models
{
    public enum EstadoPedido
    {
        pendiente,
        servido,
        cancelado
    }

    public class Pedido
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string numero_pedido { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime fecha { get; set; } = DateTime.UtcNow;

        [Required]
        public EstadoPedido estado { get; set; } = EstadoPedido.pendiente;

        [Required]
        public int cliente_id { get; set; }

        public Cliente? Cliente { get; set; }
    }
}


