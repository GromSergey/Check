namespace Check.Models
{
    public class GiftVm : BaseVm
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ShopName { get; set; } = string.Empty;
        public string ShopUrl { get; set; } = string.Empty;
        public int Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsGifted { get; set; } = false;
        public Guid? UserId { get; set; }
    }
}
