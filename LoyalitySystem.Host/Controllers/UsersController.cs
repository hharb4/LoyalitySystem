using LoyalitySystem.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoyalitySystem.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILoyalitySystemService _loyalitySystemService;

        public UsersController(ILoyalitySystemService loyalitySystemService)
        {
            _loyalitySystemService = loyalitySystemService;
        }

        [HttpPost("{userId}/earn")]
        public async Task<IActionResult> EarnPoints(Guid userId, [FromBody] int points)
        {
            if (points <= 0)
            {
                return BadRequest("Points must be greater than zero.");
            }

            try
            {
                await _loyalitySystemService.Earn(userId, points);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
