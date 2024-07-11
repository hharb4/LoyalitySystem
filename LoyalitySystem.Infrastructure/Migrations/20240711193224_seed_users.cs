using LoyalitySystem.Domain.Shared;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoyalitySystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class seed_users : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Insert dummy data into Users table
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Name", "Email", "CreatedDate", "ModifiedDate", "CreatedById", "ModifiedById", "DeletedById", "DeletedAt" },
                values: new object[,]
                {
                    { Guid.Parse("5eaaa701-b148-4b6b-8a75-477036694078"), "John Doe", "john.doe@example.com@example.com", DateTime.UtcNow, null, LoaylitySystemConsts.SystemUserId, null, null, null },
                    { Guid.Parse("fddb12e2-3a9a-47e5-96dc-b82b11df87c9"), "Jane Smith", "jane.smith@example.com@example.com", DateTime.UtcNow, null, LoaylitySystemConsts.SystemUserId, null, null, null },
                    { Guid.Parse("7faf0979-8548-4d81-82e8-303c5d2f040e"), "Alice Johnson", "alice.johnson@example.com@example.com", DateTime.UtcNow, null, LoaylitySystemConsts.SystemUserId, null, null, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValues: new object[]
                {
                    Guid.Parse("5eaaa701-b148-4b6b-8a75-477036694078"),
                    Guid.Parse("fddb12e2-3a9a-47e5-96dc-b82b11df87c9"),
                    Guid.Parse("7faf0979-8548-4d81-82e8-303c5d2f040e")
                });
        }
    }
}
