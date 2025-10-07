using System.ComponentModel.DataAnnotations.Schema;
namespace Pedrito.Models
{
    [Table("waiters")]
    public class Waiter
    {
    [Column("id")]
    public int Id { get; set; }
    [Column("first_name")]
    public string? FirstName { get; set; }
    [Column("last_name")]
    public string? LastName { get; set; }
    [Column("shift")]
    public string? Shift { get; set; }
    [Column("years_experience")]
    public int YearsExperience { get; set; }
    }
}
