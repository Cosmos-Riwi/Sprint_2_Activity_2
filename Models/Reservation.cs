using System;

using System.ComponentModel.DataAnnotations.Schema;
namespace Pedrito.Models
{
    [Table("reservations")]
    public class Reservation
    {
    [Column("id")]
    public int Id { get; set; }
    [Column("reservation_date")]
    public DateTime ReservationDate { get; set; }
    [Column("reservation_time")]
    public TimeSpan ReservationTime { get; set; }
    [Column("party_size")]
    public int PartySize { get; set; }
    [Column("notes")]
    public string? Notes { get; set; }
    [Column("client_id")]
    public int ClientId { get; set; }
    public Client? Client { get; set; }
    }
}
