using LinkSlicer.IServices;
using Newtonsoft.Json;

namespace LinkSlicer.Services
{
    public class GeoLocationService : IGeoLocationService
    {
        public async Task<(string Country, string City, string Region)> GetGeoLocation(string ip)
        {
            if (string.IsNullOrEmpty(ip) || ip == "::1")
                return ("Unknown", "Unknown", "Unknown");

            try
            {
                using var httpClient = new HttpClient();
                var response = await httpClient.GetStringAsync($"http://ip-api.com/json/{ip}");
                dynamic data = JsonConvert.DeserializeObject(response);

                return (data.country, data.city, data.regionName);
            }
            catch
            {
                return ("Unknown", "Unknown", "Unknown");
            }
        }
    }
}
