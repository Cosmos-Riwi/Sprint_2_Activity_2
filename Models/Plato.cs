using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using GestionRestaurante.Helpers;

namespace GestionRestaurante.Models
{
    public enum CategoriaPlato
    {
        entrada,
        [Display(Name = "Plato Fuerte")]
        plato_fuerte,
        postre,
        bebida
    }

    public class Plato
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string nombre { get; set; } = string.Empty;

        public string? descripcion { get; set; }

        [Range(0.01, 1000000)]
        [TypeConverter(typeof(CustomDecimalConverter))]
        public decimal precio { get; set; }

        [Required]
        public CategoriaPlato categoria { get; set; } = CategoriaPlato.entrada;
    }
}


