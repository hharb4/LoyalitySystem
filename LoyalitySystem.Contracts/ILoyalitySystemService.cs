namespace LoyalitySystem.Contracts
{
    public interface ILoyalitySystemService
    {
        Task Earn(Guid userId, int points);
    }
}
