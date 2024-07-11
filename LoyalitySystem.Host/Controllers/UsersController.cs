using LoyalitySystem.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace LoyalitySystem.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILoyalitySystemService _loyalitySystemService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(ILoyalitySystemService loyalitySystemService, ILogger<UsersController> logger)
        {
            _loyalitySystemService = loyalitySystemService;
            _logger = logger;
        }

        [HttpPost("{userId}/earn")]
        public async Task<IActionResult> EarnPoints(Guid userId, [FromBody] int points)
        {
            if (points <= 0)
            {
                _logger.LogWarning("Invalid points value {Points} provided for user with ID {UserId}", points, userId);
                return BadRequest("Points must be greater than zero.");
            }

            try
            {
                await _loyalitySystemService.Earn(userId, points);
                _logger.LogInformation("P   oints {Points} earned for user with ID {UserId}", points, userId);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while earning points for user with ID {UserId}", userId);
                return StatusCode(500, ex.Message);
            }
        }
    }
}
