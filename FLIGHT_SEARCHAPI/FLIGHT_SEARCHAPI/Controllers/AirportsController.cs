using Microsoft.AspNetCore.Mvc;

namespace FLIGHT_SEARCHAPI.Controllers
{
    [ApiController]
    [Route("api/airports")]
    public class AirportsController : Controller
    {
        private readonly ILogger<AirportsController> _logger;
    
        private static readonly Dictionary<string, List<string>> Routes =
            new()
            {
                { "DEL", new List<string> { "LON", "PAR", "DXB", "BLR" } },
                { "LON", new List<string> { "DEL", "NYC" }},
                { "BLR", new List<string> { "DEL", "DXB" } }
            };

        public AirportsController(ILogger<AirportsController> logger)
        {
            _logger = logger;
        }

        [HttpGet("destinations/{origin}")]
       public IActionResult GetDestinations(string origin)
        {
            try {
                _logger.LogInformation("Request received at {time} for origin {origin}", DateTime.Now, origin);
                if (string.IsNullOrWhiteSpace(origin))
                    return BadRequest("Origin airport code is required.");

                origin = origin.ToUpper();

                if (!Routes.ContainsKey(origin))
                {
                    _logger.LogWarning("No destinations found for origin {Origin}", origin);
                    return NotFound($"No destinations found for origin {origin}");
                }
                    

                var result = Routes[origin];
                _logger.LogInformation( "Returning {Count} destinations for origin {Origin}",result.Count, origin);

                return Ok(Routes[origin]);
            }
            catch(Exception ex) {
                _logger.LogError(ex, "Error while fetching destinations for {Origin}", origin);
                return StatusCode(500, "Internal server error");

            }

        }
    }
}
