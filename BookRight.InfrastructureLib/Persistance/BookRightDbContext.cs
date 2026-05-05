using Microsoft.EntityFrameworkCore;

namespace BookRight.InfrastructureLib.Persistance;

public class BookRightDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured != false)
        {
            optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=BookRightDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False;Command Timeout=30"); 
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BookRightDbContext).Assembly);
    }
}
