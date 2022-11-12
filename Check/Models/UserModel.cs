namespace Check.Models
{
    public class UserModel
    {
        public string Username { get; set; } = string.Empty;
        public string Fullname { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string About { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string BackgroundUrl { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public bool IsVerified { get; set; } = false;
        public string Role { get; set; } = string.Empty;
        public string TiktokName { get; set; } = string.Empty;
        public string TwitterName { get; set; } = string.Empty;
        public string VkName { get; set; } = string.Empty;
        public string TelegramName { get; set; } = string.Empty;
        public string InstagramName { get; set; } = string.Empty;
    }
}
