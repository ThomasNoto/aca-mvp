using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Data;

public class ApiDbContext : DbContext
{
    public DbSet<Flight> Flights => Set<Flight>();
    public DbSet<Airport> Airports => Set<Airport>();
    public DbSet<AppUser> Users => Set<AppUser>();

    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }

    // enforces a one-to-many relationship of airports to flights for destination and
    // each flight reference two airports (origin and destination) with *foreign keys*
    // EF core enforces this relationship at the db level when schema is generated
    // .OnDelete(DeleteBehavior.Restrict); make it so that an airport cant be
    // deleted if it is referenced by any flight
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Flight>()
            .HasOne<Airport>()
            .WithMany()        
            .HasForeignKey(f => f.Origin_Airport_Id)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Flight>()
            .HasOne<Airport>()
            .WithMany()
            .HasForeignKey(f => f.Destination_Airport_Id)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
