using System.ComponentModel.DataAnnotations;

namespace LinkSlicer.Models
{
    public class ShortUrl
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z0-9_-]+$", ErrorMessage = "Only letters, numbers, '-' and '_' are allowed.")]
        public string OriginalUrl { get; set; }

        [Required]
        public string ShortCode { get; set; }
        public string? CreatedByUserId { get; set; } // Nullable for anonymous users
        public string? IPAddress { get; set; } // Store IP for anonymous users
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
