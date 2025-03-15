namespace LinkSlicer.Models
{
    public class AccessLogDTO : AccessLog
    {
        public string? Browser { get; set; }  // Browser name
        public string? BrowserVersion { get; set; } // Browser version
        public string? DeviceType { get; set; }
    }
}
