using LoyalitySystem.Contracts;
using LoyalitySystem.Contracts.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoyalitySystem.Host.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
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
        public async Task<IActionResult> EarnPoints(Guid userId, [FromBody] EarnPointsInputModel input)
        {
            //validation set on this function will be handled by FluentValidation
            //kept in case fluent validation is not working or having a bug
            if (input.Points <= 0)
            {
                _logger.LogWarning("Invalid points value {Points} provided for user with ID {UserId}", input.Points, userId);
                return BadRequest("Points must be greater than zero.");
            }

            try
            {
                await _loyalitySystemService.Earn(userId, input.Points);
                _logger.LogInformation("P   oints {Points} earned for user with ID {UserId}", input.Points, userId);
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
