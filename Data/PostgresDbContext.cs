using Microsoft.EntityFrameworkCore;
using Pedrito.Models;

namespace Pedrito.Data
{
    public class PostgresDbContext : DbContext
    {
        public PostgresDbContext(DbContextOptions<PostgresDbContext> options) : base(options) {}

        public DbSet<Client> Clients { get; set; }
        public DbSet<Waiter> Waiters { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relación: Client -> Orders (uno a muchos)
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Client)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.ClientId);

            // Relación: Client -> Reservations (uno a muchos)
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Client)
                .WithMany(c => c.Reservations)
                .HasForeignKey(r => r.ClientId);

            // Configurar nombres de tablas en minúsculas
            modelBuilder.Entity<Client>().ToTable("clients");
            modelBuilder.Entity<Waiter>().ToTable("waiters");
            modelBuilder.Entity<Dish>().ToTable("dishes");
            modelBuilder.Entity<Order>().ToTable("orders");
            modelBuilder.Entity<Reservation>().ToTable("reservations");
        }
    }
}
