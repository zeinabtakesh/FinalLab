using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistance ;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;


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
