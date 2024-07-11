using LoyalitySystem.Contracts;
using LoyalitySystem.Domain;
using LoyalitySystem.Domain.Shared;
using LoyalitySystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LoyalitySystem.Application
{
    public class LoyalitySystemService : ILoyalitySystemService
    {
        private readonly LoyalitySystemDbContext _context;

        public LoyalitySystemService(LoyalitySystemDbContext context)
        {
            _context = context;
        }

        public async Task Earn(Guid userId, int points)
        {
            var user = await _context.Users
                .Include(u => u.Points)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            user.Points.Add(new Point
            {
                UserId = userId,
                Value = points,
                EarnedDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(1),
                CreatedDate = DateTime.UtcNow,
                CreatedById = LoaylitySystemConsts.SystemUserId // or some other userId who is making the transaction
            });

            await _context.SaveChangesAsync();
        }
    }
}
