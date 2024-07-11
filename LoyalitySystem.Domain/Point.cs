namespace LoyalitySystem.Domain
{
    public class Point: BaseEntity<Guid>, IFullAuditEntity
    {
        public User User { get; set; }
        public Guid UserId { get; set; }
        public int Value { get; set; }
        public DateTime EarnedDate { get; set; }
        public DateTime ExpiryDate { get; set; }


        // IFullAuditEntity implementation
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid CreatedById { get; set; }
        public Guid? ModifiedById { get; set; }
        public Guid? DeletedById { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
