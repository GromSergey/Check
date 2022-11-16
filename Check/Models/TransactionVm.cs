namespace Check.Models
{
    public class TransactionVm : BaseVm
    {
        public Guid? GiftId { get; set; }
        public Guid? UserId { get; set; }
        public Guid? GifterId { get; set; }
        public bool IsCompleted { get; set; } = false;
    }
}
