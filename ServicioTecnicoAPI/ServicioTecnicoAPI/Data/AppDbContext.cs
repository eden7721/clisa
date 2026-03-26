using Microsoft.EntityFrameworkCore;
using ServicioTecnicoAPI.Models;
using System.ClientModel.Primitives;

namespace ServicioTecnicoAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Equipo> Equipos { get; set; }
        public DbSet<Marca> Marcas { get; set; }
        public DbSet<TipoEquipo> TiposEquipo { get; set; }
        public DbSet<OrdenServicio> OrdenesServicio { get; set;}
        public DbSet<EstadoServicio> EstadosServicio { get; set;}
        public DbSet<TipoServicio> TiposServicio { get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Equipo>()
                .HasOne(e => e.Cliente)
                .WithMany(m => m.Equipos)
                .HasForeignKey(e => e.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Equipo>()
                .HasOne(e => e.Marca)
                .WithMany()
                .HasForeignKey(fk => fk.MarcaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Equipo>()
                .HasOne(e => e.TipoEquipo)
                .WithMany()
                .HasForeignKey(fk => fk.TipoEquipoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrdenServicio>()
                .HasOne(o => o.Equipo)
                .WithMany(m => m.OrdenesServicio)
                .HasForeignKey(fk => fk.EquipoId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<OrdenServicio>()
                .Property(o => o.Precio)
                .HasPrecision(10, 2);

            modelBuilder.Entity<OrdenServicio>()
                .HasOne(o => o.EstadoServicio)
                .WithMany()
                .HasForeignKey(fk => fk.EstadoServicioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrdenServicio>()
                .HasOne(o => o.TipoServicio)
                .WithMany()
                .HasForeignKey(fk => fk.TipoServicioId)
                .OnDelete(DeleteBehavior.Restrict);
        }


    }
}
