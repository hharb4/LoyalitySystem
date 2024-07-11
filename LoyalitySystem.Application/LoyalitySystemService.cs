using LoyalitySystem.Contracts;
using LoyalitySystem.Domain;
using LoyalitySystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LoyalitySystem.Application
{
    public class LoyalitySystemService : ILoyalitySystemService
    {
        private readonly LoyalitySystemDbContext _context;
        private readonly ILogger<LoyalitySystemService> _logger;

        public LoyalitySystemService(LoyalitySystemDbContext context, ILogger<LoyalitySystemService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Earn(Guid userId, int points)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.Points)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                {
                    _logger.LogWarning("User with ID {UserId} not found", userId);
                    throw new Exception("User not found");
                }

                user.Points.Add(new Point
                {
                    UserId = userId,
                    Value = points,
                    ExpiryDate = DateTime.UtcNow.AddMonths(2),
                    EarnedDate = DateTime.UtcNow,
                    CreatedDate = DateTime.UtcNow,
                    CreatedById = userId // or some other userId who is making the transaction
                });

                await _context.SaveChangesAsync();
                _logger.LogInformation("Points {Points} added to user {username} with ID {UserId} ", points, user.Name, userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding points to user with ID {UserId}", userId);
                throw;
            }
        }
    }
}
