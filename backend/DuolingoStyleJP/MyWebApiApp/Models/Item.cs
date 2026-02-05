using System.ComponentModel.DataAnnotations.Schema;

namespace MyWebApiApp.Models
{
    [Table("Item")]
    public class Item
    {
        public int ItemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Price { get; set; }
        public string Category { get; set; } = string.Empty;
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; } = true;
        
    }
}
