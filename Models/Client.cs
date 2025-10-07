using System.Collections.Generic;

using System.ComponentModel.DataAnnotations.Schema;
namespace Pedrito.Models
{
    [Table("clients")]
    public class Client
    {
    [Column("id")]
    public int Id { get; set; }
    [Column("first_name")]
    public string? FirstName { get; set; }
    [Column("last_name")]
    public string? LastName { get; set; }
    [Column("email")]
    public string? Email { get; set; }
    [Column("phone")]
    public string? Phone { get; set; }

    public ICollection<Order>? Orders { get; set; }
    public ICollection<Reservation>? Reservations { get; set; }
    }
}
