using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public class FlightService
    {
        private readonly ApiDbContext _context;

        public FlightService(ApiDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Flight>> GetFlightsAsync()
        {
            return await _context.Flights
                .Include(f => f.OriginAirport)
                .Include(f => f.DestinationAirport)
                .ToListAsync();
        }

        //IEnumerbale is an interface. It can be returned as an array, list, or other collections
        public async Task<IEnumerable<Flight>> SearchFlightsAsync(string? origin, string? destination)
        {
            var query = _context.Flights
                .Include(f => f.OriginAirport)
                .Include(f => f.DestinationAirport)
                .AsQueryable();

            if (!string.IsNullOrEmpty(origin))
            {
                query = query.Where(f => f.OriginAirport.Iata_Code == origin);
            }

            if (!string.IsNullOrEmpty(destination))
            {
                query = query.Where(f => f.DestinationAirport.Iata_Code == destination);
            }

            return await query.ToListAsync();
        }


        public async Task<Flight> CreateFlightAsync(Flight newFlight)
        {
            _context.Flights.Add(newFlight);
            await _context.SaveChangesAsync();
            return newFlight;
        }
    }
}
