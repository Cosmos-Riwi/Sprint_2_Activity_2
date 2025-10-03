using System.ComponentModel.DataAnnotations;

namespace GestionRestaurante.Models
{
    public class Mesero
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string nombre { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string apellido { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string turno { get; set; } = string.Empty;

        [Range(0, 60)]
        public int anos_experiencia { get; set; }
    }
}


