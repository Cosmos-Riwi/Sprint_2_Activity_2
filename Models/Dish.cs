using System.ComponentModel.DataAnnotations.Schema;
namespace Pedrito.Models
{
    [Table("dishes")]
    public class Dish
    {
    [Column("id")]
    public int Id { get; set; }
    [Column("price")]
    public decimal Price { get; set; }
    [Column("name")]
    public string? Name { get; set; }
    [Column("description")]
    public string? Description { get; set; }
    [Column("category")]
    public DishCategory Category { get; set; }
    }
}
