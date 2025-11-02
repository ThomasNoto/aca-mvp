using backend.Models;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace backend.Data;

// connects C# models in app to PostgreSQL db
public class ApiDbContext : DbContext
{
    public virtual DbSet<Flight> Flights { get; set; }
    public virtual DbSet<Airport> Airports { get; set; }
    public virtual DbSet<AppUser> Users { get; set; }
    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base()
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Flight>()
            .HasOne(f => f.OriginAirport)
            .WithMany()
            .HasForeignKey(f => f.Origin_Airport_Id)
            .OnDelete(DeleteBehavior.Restrict) // prevents cascade loop, can't delete anything with a connections
            .HasConstraintName("FK_Origin_Airport");

        modelBuilder.Entity<Flight>()
            .HasOne(f => f.DestinationAirport)
            .WithMany()
            .HasForeignKey(f => f.Destination_Airport_Id)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Destination_Airport");

        // If i were creating airports, model builder example
        // each airport must have a unique iata_code
        // modelBuilder.Entity<Airport>()
        //     .HasIndex(a => a.Iata_Code)
        //     .IsUnique();

        // enums as strings so they can be read in the db
        modelBuilder
            .Entity<Flight>()
            .Property(f => f.Status)
            .HasConversion<string>();

        modelBuilder
            .Entity<AppUser>()
            .Property(u => u.Role)
            .HasConversion<string>();

        modelBuilder
            .Entity<Airport>()
            .Property(a => a.Timezone)
            .HasConversion<string>();

        // SEED DATA for local dev and demo purposes
        modelBuilder.Entity<Airport>().HasData(
            new Airport { Id = 1, Iata_Code = "PIT", Name = "Pittsburgh Intl", City = "Pittsburgh", State = "PA", Timezone = Timezones.EST },
            new Airport { Id = 2, Iata_Code = "LGA", Name = "LaGuardia", City = "New York", State = "NY", Timezone = Timezones.EST },
            new Airport { Id = 3, Iata_Code = "ORD", Name = "O’Hare Intl", City = "Chicago", State = "IL", Timezone = Timezones.CST },
            new Airport { Id = 4, Iata_Code = "RDU", Name = "Raleigh–Durham Intl", City = "Raleigh", State = "NC", Timezone = Timezones.EST }
        );

        modelBuilder.Entity<Flight>().HasData(
            new Flight { Id = 1, Flight_Number = "AA100", Origin_Airport_Id = 1, Destination_Airport_Id = 2, Departure_Time = DateTimeOffset.Parse("2025-11-02T10:30:00-05:00"), Arrival_Time = DateTimeOffset.Parse("2025-11-02T12:00:00-05:00"), Aircraft_Type = "Boeing 737", Status = FlightStatus.OnTime },
            new Flight { Id = 2, Flight_Number = "UA204", Origin_Airport_Id = 2, Destination_Airport_Id = 3, Departure_Time = DateTimeOffset.Parse("2025-11-02T14:15:00-05:00"), Arrival_Time = DateTimeOffset.Parse("2025-11-02T15:45:00-06:00"), Aircraft_Type = "Airbus A320", Status = FlightStatus.Delayed },
            new Flight { Id = 3, Flight_Number = "DL301", Origin_Airport_Id = 3, Destination_Airport_Id = 1, Departure_Time = DateTimeOffset.Parse("2025-11-02T09:00:00-06:00"), Arrival_Time = DateTimeOffset.Parse("2025-11-02T11:00:00-05:00"), Aircraft_Type = "Boeing 757", Status = FlightStatus.OnTime },
            new Flight { Id = 4, Flight_Number = "SW505", Origin_Airport_Id = 1, Destination_Airport_Id = 4, Departure_Time = DateTimeOffset.Parse("2025-11-02T13:00:00-05:00"), Arrival_Time = DateTimeOffset.Parse("2025-11-02T15:00:00-05:00"), Aircraft_Type = "Boeing 737", Status = FlightStatus.OnTime },
            new Flight { Id = 5, Flight_Number = "AA606", Origin_Airport_Id = 4, Destination_Airport_Id = 1, Departure_Time = DateTimeOffset.Parse("2025-11-03T09:00:00-05:00"), Arrival_Time = DateTimeOffset.Parse("2025-11-03T11:00:00-05:00"), Aircraft_Type = "Airbus A319", Status = FlightStatus.OnTime }
        );

        modelBuilder.Entity<AppUser>().HasData(
            new AppUser { Id = 1, Name = "Thomas Noto", Role = UserRole.Admin },
            new AppUser { Id = 2, Name = "John Doe", Role = UserRole.User }
        );

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=PostgreSQL_Local;Username=postgres;Password=postgres;Pooling=true");
        }
    }
}