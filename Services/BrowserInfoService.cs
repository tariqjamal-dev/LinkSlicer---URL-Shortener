using LinkSlicer.IServices;

namespace LinkSlicer.Services
{
    public class BrowserInfoService : IBrowserInfoService
    {
        public (string Browser, string Version) GetBrowserInfo(string userAgent)
        {
            if (string.IsNullOrEmpty(userAgent))
                return ("Unknown", "Unknown");

            if (userAgent.Contains("Chrome"))
                return ("Chrome", userAgent.Split("Chrome/")[1].Split(' ')[0]);
            if (userAgent.Contains("Firefox"))
                return ("Firefox", userAgent.Split("Firefox/")[1].Split(' ')[0]);
            if (userAgent.Contains("Edge"))
                return ("Edge", userAgent.Split("Edg/")[1].Split(' ')[0]);
            if (userAgent.Contains("Safari") && !userAgent.Contains("Chrome"))
                return ("Safari", userAgent.Split("Version/")[1].Split(' ')[0]);
            if (userAgent.Contains("MSIE") || userAgent.Contains("Trident"))
                return ("Internet Explorer", "Unknown");

            return ("Other", "Unknown");
        }

        public string GetDeviceType(string userAgent)
        {
            if (string.IsNullOrEmpty(userAgent))
                return "Unknown";

            if (userAgent.Contains("Mobile"))
                return "Mobile";
            if (userAgent.Contains("Tablet"))
                return "Tablet";
            return "Desktop";
        }
    }
}
