using System;
using System.ComponentModel.DataAnnotations;

namespace GestionRestaurante.Models
{
    public class Reserva
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime fecha { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan hora { get; set; }

        [Range(1, 1000)]
        public int numero_personas { get; set; }

        public string? observaciones { get; set; }

        [Required]
        public int cliente_id { get; set; }

        public Cliente? Cliente { get; set; }
    }
}


