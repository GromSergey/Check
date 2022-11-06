namespace Check.Database.Entities
{
    public class Transaction : BaseEntity
    {
        public Guid? GiftId { get; set; }
        public Guid? UserId { get; set; }
        public Guid? GifterId { get; set; }
        public bool IsCompleted { get; set; } = false;

        #region Virtual
        public virtual Gift? Gift { get; set; }
        public virtual User? User { get; set; }
        public virtual User? Gifter { get; set; }
        #endregion
    }
}
