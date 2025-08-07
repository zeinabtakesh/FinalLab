namespace Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;


public class LabDbContextFactory : IDesignTimeDbContextFactory<LabDbContext>
{
    public LabDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<LabDbContext>();

        optionsBuilder.UseNpgsql("Host=localhost;Database=final_lab_db;Username=postgres;Password=postgres");

        return new LabDbContext(optionsBuilder.Options);
    }
    
}




    
