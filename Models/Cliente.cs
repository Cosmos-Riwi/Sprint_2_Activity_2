using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GestionRestaurante.Models
{
    public class Cliente
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string nombre { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string apellido { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string correo { get; set; } = string.Empty;

        [StringLength(15)]
        public string? telefono { get; set; }

        public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();

        public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
    }
}


