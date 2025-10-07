using System;

using System.ComponentModel.DataAnnotations.Schema;
namespace Pedrito.Models
{
    [Table("orders")]
    public class Order
    {
    [Column("id")]
    public int Id { get; set; }
    [Column("order_number")]
    public string? OrderNumber { get; set; }
    [Column("order_date")]
    public DateTime OrderDate { get; set; }
    [Column("status")]
    public OrderStatus Status { get; set; }
    [Column("client_id")]
    public int ClientId { get; set; }
    public Client? Client { get; set; }
    }
}
