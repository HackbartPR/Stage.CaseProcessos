using Microsoft.EntityFrameworkCore;
using Stage.Domain.Config;
using Stage.Domain.Entities;

namespace Stage.Infrastructure.Persistence
{
    public class SqlContext : DbContext
    {
        public DbSet<Area> Areas { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Processo> Processos { get; set; }
        
        public DbSet<Ferramenta> Ferramentas { get; set; }

        public SqlContext(DbContextOptions<SqlContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Settings.Database.ConnectionString);
                base.OnConfiguring(optionsBuilder);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>()
                .HasMany(us => us.Areas)
                .WithOne(ar => ar.Responsible)
                .HasForeignKey(us => us.IdResponsible)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Area>()
                .HasMany(ar => ar.Processos)
                .WithMany(pr => pr.Areas);

            modelBuilder.Entity<Processo>()
                .HasMany(pr => pr.SubProcessos)
                .WithOne(su => su.ParentProcess)
                .HasForeignKey(us => us.IdParentProccess);

            modelBuilder.Entity<Ferramenta>()
                .HasMany(fe => fe.Processos)
                .WithMany(pr => pr.Ferramentas);
        }
    }
}
