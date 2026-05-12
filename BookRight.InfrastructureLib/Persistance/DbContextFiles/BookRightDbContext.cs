using BookRight.DomainLib.Entities.Bookings;
using BookRight.DomainLib.Entities.Clinics;
using BookRight.DomainLib.Entities.Customers;
using BookRight.DomainLib.Entities.Therapists;
using BookRight.DomainLib.Entities.Treatments;
using Microsoft.EntityFrameworkCore;

namespace BookRight.InfrastructureLib.Persistance.DbContextFiles;

public class BookRightDbContext : DbContext
{
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Clinic> Clinics { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Therapist> Therapists { get; set; }
    public DbSet<Treatment> Treatments { get; set; }

    public BookRightDbContext(DbContextOptions<BookRightDbContext> options) : base(options) { }

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
