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
        private readonly ILogger<FlightsController> _logger;

        public FlightsController(FlightService flightService, ILogger<FlightsController> logger)
        {
            _flightService = flightService;
            _logger = logger;
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
        public async Task<ActionResult<Flight>> CreateFlight([FromBody] FlightCreateDTO flightDto)
        {
            _logger.LogInformation("Received CreateFlight request: {@FlightDto}", flightDto);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state: {@ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            try
            {
                var newFlight = new Flight
                {
                    Flight_Number = flightDto.Flight_Number,
                    Origin_Airport_Id = flightDto.Origin_Airport_Id,
                    Destination_Airport_Id = flightDto.Destination_Airport_Id,
                    Departure_Time = flightDto.Departure_Time
                };

                var createdFlight = await _flightService.CreateFlightAsync(newFlight);

                _logger.LogInformation("Created flight with Id: {Id}", createdFlight.Id);

                return CreatedAtAction(nameof(CreateFlight), new { id = createdFlight.Id }, createdFlight);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating flight: {@FlightDto}", flightDto);
                return StatusCode(500, "An error occurred while creating the flight.");
            }
        }
    }
}
