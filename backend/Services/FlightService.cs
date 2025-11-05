using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public class FlightService
    {
        // _context is an instance of the ApiDbContext class
        // this gives EF Core access to all tables in db
        private readonly ApiDbContext _context;
        private readonly ILogger<FlightService> _logger;
        
        public FlightService(ApiDbContext context, ILogger<FlightService> logger)
        {
            _context = context;
            _logger = logger;
        }


        public async Task<IEnumerable<FlightDTO>> GetFlightsAsync()
        {
            // fetches all rows in flights table
            // converts rows into a list of flight objects
            // Async means we are performing a task that takes time such as fetching data
            // but the thread is not blocked while waiting for this request to be fulfilled
            var flights = await _context.Flights.ToListAsync();

            // builds a dict mapping airport id -> iata code
            var airports = await _context.Airports.ToDictionaryAsync(a => a.Id, a => a.Iata_Code);

            return flights.Select(f => new FlightDTO
            {
                Id = f.Id,
                Flight_Number = f.Flight_Number,
                Origin_Iata = airports[f.Origin_Airport_Id],
                Destination_Iata = airports[f.Destination_Airport_Id],
                Departure_Time = f.Departure_Time
            });
        }

        // retrieves flights for user based on optional origin and destination IATA airport codes
        // search cases:
        // 1) User provides both origin and destination IATA -> flights with both airports are shown
        // 2) User provides only origin or destination -> all outgoing or incoming flights are shown
        // 3) User provides neither origin nor destination -> all flights in db are shown (unsure if practical)
        // IEnumerbale is an interface. It can be returned as an array, list, or other collections
        public async Task<IEnumerable<FlightDTO>> SearchFlightsAsync(string? origin, string? destination)
        {
            _logger.LogInformation("Searching flights with origin: {Origin}, destination: {Destination}", origin, destination);
            // querying the entire flight table
            // could be bad in production if there were millions of entries
            // but it works for the sake of an MVP like this
            var query = _context.Flights.AsQueryable();
            // saves db queries by relating all mapping all IATAs to the airport IDs
            // and vice versa for DTO mapping
            // there are only 487 commerical airports in the USA
            // so query the entire airport table into a dictionary would result nelgible
            // space complexity even in real production
            var airportsIataToId = await _context.Airports.ToDictionaryAsync(a => a.Iata_Code, a => a.Id);
            var airportsIdToIata = await _context.Airports.ToDictionaryAsync(a => a.Id, a => a.Iata_Code);

            _logger.LogInformation("Loaded dict with {Count} airports", airportsIataToId.Count);

            if (!string.IsNullOrEmpty(origin))
            {
                origin = origin.ToUpper();

                if (airportsIataToId.ContainsKey(origin))
                {
                    var id = airportsIataToId[origin];
                    _logger.LogInformation("Found origin: {Origin},  Id: {Id}", origin, id);
                    query = query.Where(f => f.Origin_Airport_Id == id);
                }
                else
                {
                    _logger.LogWarning("Origin IATA {Origin} not found in airport dictionary", origin);
                }                
            }

            if (!string.IsNullOrEmpty(destination))
            {
                destination = destination.ToUpper();

                if (airportsIataToId.ContainsKey(destination))
                    query = query.Where(f => f.Destination_Airport_Id == airportsIataToId[destination]);
            }

            // execute the query
            var flightList = await query.ToListAsync();

            var flightDtos = flightList.Select(f => new FlightDTO
            {
                Id = f.Id,
                Flight_Number = f.Flight_Number,
                Origin_Iata = airportsIdToIata[f.Origin_Airport_Id],
                Destination_Iata = airportsIdToIata[f.Destination_Airport_Id],
                Departure_Time = f.Departure_Time
            }).ToList();

            return flightDtos;
        }


        public async Task<Flight> CreateFlightAsync(Flight newFlight)
        {
            _context.Flights.Add(newFlight);
            await _context.SaveChangesAsync();
            return newFlight;
        }
    }
}
