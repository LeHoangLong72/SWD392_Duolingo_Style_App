namespace MyWebApiApp.Models
{
    public class UserItem
    {
        public int UserItemId { get; set; }
        public int UserId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; } = 1;
        public DateTime PurchasedAt { get; set; } = DateTime.Now;

        public AppUser User { get; set; } = null!;
        public Item Item { get; set; } = null!;
    }
}
