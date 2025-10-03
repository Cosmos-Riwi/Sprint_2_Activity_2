using GestionRestaurante.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace GestionRestaurante.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<Cliente> Clientes => Set<Cliente>();
        public DbSet<Mesero> Meseros => Set<Mesero>();
        public DbSet<Plato> Platos => Set<Plato>();
        public DbSet<Pedido> Pedidos => Set<Pedido>();
        public DbSet<Reserva> Reservas => Set<Reserva>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    // Configurar nombres de tablas
    modelBuilder.Entity<Cliente>().ToTable("clientes");
    modelBuilder.Entity<Mesero>().ToTable("meseros");
    modelBuilder.Entity<Plato>().ToTable("platos");
    modelBuilder.Entity<Pedido>().ToTable("pedidos");
    modelBuilder.Entity<Reserva>().ToTable("reservas");

    // Configurar nombres de columnas para Cliente
    modelBuilder.Entity<Cliente>(entity =>
    {
        entity.Property(e => e.Id).HasColumnName("id");
        entity.Property(e => e.nombre).HasColumnName("nombre");
        entity.Property(e => e.apellido).HasColumnName("apellido");
        entity.Property(e => e.correo).HasColumnName("correo");
        entity.Property(e => e.telefono).HasColumnName("telefono");
    });

    // Configurar nombres de columnas para Mesero
    modelBuilder.Entity<Mesero>(entity =>
    {
        entity.Property(e => e.Id).HasColumnName("id");
        entity.Property(e => e.nombre).HasColumnName("nombre");
        entity.Property(e => e.apellido).HasColumnName("apellido");
        entity.Property(e => e.turno).HasColumnName("turno");
        entity.Property(e => e.anos_experiencia).HasColumnName("anos_experiencia");
    });

     // Configurar nombres de columnas para Plato
     modelBuilder.Entity<Plato>(entity =>
     {
         entity.Property(e => e.Id).HasColumnName("id");
         entity.Property(e => e.nombre).HasColumnName("nombre");
         entity.Property(e => e.descripcion).HasColumnName("descripcion");
         entity.Property(e => e.precio)
             .HasColumnName("precio")
             .HasColumnType("decimal(10,2)")
             .HasConversion(
                 v => v,
                 v => v
             );
         entity.Property(e => e.categoria)
             .HasColumnName("categoria")
             .HasConversion(
                 v => v.ToString().Replace("_", " "),
                 v => (CategoriaPlato)Enum.Parse(typeof(CategoriaPlato), v.Replace(" ", "_"))
             )
             .HasMaxLength(20);
     });

    // Configurar nombres de columnas para Pedido
    modelBuilder.Entity<Pedido>(entity =>
    {
        entity.Property(e => e.Id).HasColumnName("id");
        entity.Property(e => e.numero_pedido).HasColumnName("numero_pedido");
        entity.Property(e => e.fecha).HasColumnName("fecha").HasColumnType("date");
         entity.Property(e => e.estado)
             .HasColumnName("estado")
             .HasConversion<string>()
             .HasMaxLength(20);
        entity.Property(e => e.cliente_id).HasColumnName("cliente_id");
        entity.HasOne(e => e.Cliente)
            .WithMany(c => c.Pedidos)
            .HasForeignKey(e => e.cliente_id)
            .OnDelete(DeleteBehavior.Cascade);
    });

    // Configurar nombres de columnas para Reserva
    modelBuilder.Entity<Reserva>(entity =>
    {
        entity.Property(e => e.Id).HasColumnName("id");
        entity.Property(e => e.fecha).HasColumnName("fecha").HasColumnType("date");
        entity.Property(e => e.hora).HasColumnName("hora").HasColumnType("time");
        entity.Property(e => e.numero_personas).HasColumnName("numero_personas");
        entity.Property(e => e.observaciones).HasColumnName("observaciones");
        entity.Property(e => e.cliente_id).HasColumnName("cliente_id");
        entity.HasOne(e => e.Cliente)
              .WithMany(c => c.Reservas)
              .HasForeignKey(e => e.cliente_id)
              .OnDelete(DeleteBehavior.Cascade);
    });
}
    }
}


