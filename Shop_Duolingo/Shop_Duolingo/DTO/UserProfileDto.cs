namespace Shop_Duolingo.DTOs
{
    public class UserProfileDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Gems { get; set; }
        public List<ItemDto> PurchasedItems { get; set; } = new();
    }
}