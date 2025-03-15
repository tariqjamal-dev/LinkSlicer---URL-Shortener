using LinkSlicer.IServices;

namespace LinkSlicer.Services
{
    public class IpService : IIpService
    {
        public async Task<string> GetClientIp(HttpContext context)
        {
            string ip = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

            if (IsPrivateIp(ip))
                ip = await GetPublicIpAsync();

            return ip;
        }

        private static async Task<string> GetPublicIpAsync()
        {
            using var httpClient = new HttpClient();
            return await httpClient.GetStringAsync("https://api64.ipify.org");
        }

        private static bool IsPrivateIp(string ip) =>
            ip.StartsWith("192.") || ip.StartsWith("10.") || ip.StartsWith("172.16") || ip == "::1" || ip == "127.0.0.1";

    }
}
