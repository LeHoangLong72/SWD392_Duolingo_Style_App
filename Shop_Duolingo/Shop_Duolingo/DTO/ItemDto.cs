using System;

namespace Shop_Duolingo.DTOs
{
    public class ItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string NameVi { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string DescriptionVi { get; set; } = string.Empty;
        public int Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public bool IsPurchased { get; set; }
        public bool IsEquipped { get; set; }

        public int? DurationMinutes { get; set; }
        public DateTime? EquippedAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public bool IsExpired { get; set; }
        public int? RemainingMinutes { get; set; }
    }
}