using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlightsController : ControllerBase
    {
        private readonly FlightService _flightService;

        public FlightsController(FlightService flightService)
        {
            _flightService = flightService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Flight>>> GetFlights()
        {
            var flights = await _flightService.GetFlightsAsync();
            return Ok(flights);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Flight>>> SearchFlights(
            [FromQuery] string? origin,
            [FromQuery] string? destination)
        {
            if (string.IsNullOrEmpty(origin) && string.IsNullOrEmpty(destination))
            {
                return BadRequest("Please specify at least an origin or destination.");
            }

            var flights = await _flightService.SearchFlightsAsync(origin, destination);

            if (!flights.Any())
            {
                return NotFound("No flights found matching your search criteria.");
            }

            return Ok(flights);
        }


        [HttpPost]
        public async Task<ActionResult<Flight>> CreateFlight([FromBody] Flight newFlight)
        {
            var created = await _flightService.CreateFlightAsync(newFlight);
            return CreatedAtAction(nameof(GetFlights), new { id = created.Id }, created);
        }
    }
}
