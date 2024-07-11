﻿using LoyalitySystem.Application;
using LoyalitySystem.Domain;
using LoyalitySystem.Domain.Shared;
using LoyalitySystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace LoyalitySystem.Tests
{
    public class LoyalitySystemServiceTests
    {
        private readonly Mock<ILogger<LoyalitySystemService>> _loggerMock;
        private readonly LoyalitySystemDbContext _dbContext;
        private readonly LoyalitySystemService _service;

        public LoyalitySystemServiceTests()
        {
            var options = new DbContextOptionsBuilder<LoyalitySystemDbContext>()
                .UseInMemoryDatabase(databaseName: "LoyalitySystemTestDb")
                .Options;
            _dbContext = new LoyalitySystemDbContext(options);
            _loggerMock = new Mock<ILogger<LoyalitySystemService>>();
            _service = new LoyalitySystemService(_dbContext, _loggerMock.Object);
        }

        [Fact]
        public async Task Earn_ShouldAddPointsToUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User
            {
                Id = userId,
                Name = "Test User",
                Email = "testuser@example.com",
                CreatedDate = DateTime.UtcNow,
                CreatedById = LoaylitySystemConsts.SystemUserId,
            };

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            var pointsToAdd = 100;

            // Act
            await _service.Earn(userId, pointsToAdd);

            // Assert
            var updatedUser = await _dbContext.Users.Include(u => u.Points).FirstOrDefaultAsync(u => u.Id == userId);
            Assert.NotNull(updatedUser);
            Assert.Single(updatedUser.Points);
            Assert.Equal(pointsToAdd, updatedUser.Points.First().Value);
        }

        [Fact]
        public async Task Earn_ShouldThrowException_WhenUserNotFound()
        {
            // Arrange
            var nonExistentUserId = Guid.NewGuid();
            var pointsToAdd = 100;

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.Earn(nonExistentUserId, pointsToAdd));
            Assert.Equal("User not found", exception.Message);
        }
    }
}