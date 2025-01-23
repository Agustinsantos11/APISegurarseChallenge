using ChallengeAPISegurarse.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChallengeAPISegurarse.Context
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Poliza> Polizas { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.Property(c => c.Nombre)
                .HasMaxLength(50)
                    .IsRequired();

                entity.Property(c => c.Apellido)
                .HasMaxLength(50)
                    .IsRequired();

                entity.Property(c => c.DNI)
                    .IsRequired();

                entity.HasIndex(c => c.DNI)
                    .IsUnique();
                    
                entity.Property(c => c.FechaNacimiento)
                    .IsRequired();
            });

            modelBuilder.Entity<Poliza>(entity =>
            {
                entity.HasOne(p => p.Cliente)
                .WithMany(c => c.Polizas)
                .HasForeignKey(p => p.ClienteId);
            });
        }
    }
}
