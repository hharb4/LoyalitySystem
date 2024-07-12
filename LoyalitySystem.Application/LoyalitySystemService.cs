using LoyalitySystem.Contracts;
using LoyalitySystem.Domain;
using LoyalitySystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace LoyalitySystem.Application
{
    public class LoyalitySystemService : ILoyalitySystemService
    {
        private readonly LoyalitySystemDbContext _context;
        private readonly ILogger<LoyalitySystemService> _logger;
        private readonly IDistributedCache _cache;

        public LoyalitySystemService(LoyalitySystemDbContext context, ILogger<LoyalitySystemService> logger, IDistributedCache cache)
        {
            _context = context;
            _logger = logger;
            _cache = cache;
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

                // Update cache
                await UpdateUserPointsCacheAsync(userId, user.Points.Sum(p => p.Value));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding points to user with ID {UserId}", userId);
                throw;
            }
        }

        public async Task<int> GetPointsAsync(Guid userId)
        {
            var cachedPoints = await _cache.GetStringAsync($"user:{userId}:points");
            if (!string.IsNullOrEmpty(cachedPoints))
            {
                return JsonSerializer.Deserialize<int>(cachedPoints);
            }

            var user = await _context.Users.Include(u => u.Points).FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) throw new Exception("User not found");

            var points = user.Points.Sum(p => p.Value);

            // Cache points
            await UpdateUserPointsCacheAsync(userId, points);

            return points;
        }

        private async Task UpdateUserPointsCacheAsync(Guid userId, int points)
        {
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60) // Set cache expiration time as needed
            };
            await _cache.SetStringAsync($"user:{userId}:points", JsonSerializer.Serialize(points), cacheOptions);
        }
    }
}
