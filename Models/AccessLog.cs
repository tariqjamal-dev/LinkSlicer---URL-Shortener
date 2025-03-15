namespace LinkSlicer.Models
{
    public class AccessLog
    {
        public int Id { get; set; }
        public int ShortenedUrlId { get; set; }
        public string? IPAddress { get; set; }
        public string? UserAgent { get; set; }
        public string? ReferrerUrl { get; set; }
        public string? UTMSource { get; set; }
        public string? UTMMedium { get; set; }
        public string? UTMCampaign { get; set; }
        public DateTime ClickTimestamp { get; set; } = DateTime.UtcNow;
        public string? Country { get; set; }  // Country from IP
        public string? City { get; set; }  // City from IP
        public string? Region { get; set; }  // Region from IP
    }
}
