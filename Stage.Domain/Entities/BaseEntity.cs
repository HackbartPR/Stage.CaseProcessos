namespace Stage.Domain.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        public bool Active { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
