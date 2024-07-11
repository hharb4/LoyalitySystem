namespace LoyalitySystem.Domain
{
    public interface IFullAuditEntity
    {
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public Guid CreatedById { get; set; }

        public Guid? ModifiedById { get; set; }

        public Guid? DeletedById { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
