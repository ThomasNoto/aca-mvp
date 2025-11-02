using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public class AirportService
    {
        private readonly ApiDbContext _context;

        public AirportService(ApiDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Airport>> GetAirportsAsync()
        {
            return await _context.Airports.ToListAsync();
        }

        public async Task<Airport?> GetAirportByIdAsync(int id)
        {
            return await _context.Airports.FindAsync(id);
        }
    }
}
