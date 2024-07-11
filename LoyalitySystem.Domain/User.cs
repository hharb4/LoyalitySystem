namespace LoyalitySystem.Domain
{
    public class User : BaseEntity<Guid>, IFullAuditEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public ICollection<Point> Points { get; set; }

        // IFullAuditEntity implementation
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid CreatedById { get; set; }
        public Guid? ModifiedById { get; set; }
        public Guid? DeletedById { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
