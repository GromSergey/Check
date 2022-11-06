using Check.Interfaces;

namespace Check.Database.Entities
{
    public class BaseEntity : ITimestamped, ISoftDeletable
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
