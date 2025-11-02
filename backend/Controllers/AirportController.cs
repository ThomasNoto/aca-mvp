using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AirportsController : ControllerBase
    {
        private readonly AirportService _airportService;

        public AirportsController(AirportService airportService)
        {
            _airportService = airportService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Airport>>> GetAirports()
        {
            var airports = await _airportService.GetAirportsAsync();
            return Ok(airports);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Airport>> GetAirportById(int id)
        {
            var airport = await _airportService.GetAirportByIdAsync(id);
            if (airport == null)
                return NotFound();
            return Ok(airport);
        }
    }
}
