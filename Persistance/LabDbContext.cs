using Microsoft.EntityFrameworkCore;
using Domain.Entities;
namespace Persistance;
public class LabDbContext : DbContext
    {
        public LabDbContext(DbContextOptions<LabDbContext> options)
            : base(options) { }
        public DbSet<Vehicle> Vehicles => Set<Vehicle>();
        public DbSet<Driver> Drivers => Set<Driver>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LabDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    
}