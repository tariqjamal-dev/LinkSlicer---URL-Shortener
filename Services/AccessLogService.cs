using LinkSlicer.Data;
using LinkSlicer.IServices;
using LinkSlicer.Models;
using Microsoft.EntityFrameworkCore;

namespace LinkSlicer.Services
{
    public class AccessLogService(ApplicationDbContext context, IBrowserInfoService browserInfoService) : IAccessLogService
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IBrowserInfoService _browserInfoService = browserInfoService;

        public async Task<List<AccessLogDTO>?> GetAccessLogs(int shortUrlId, string userId)
        {
            var shortUrl = await _context.ShortUrls
                .FirstOrDefaultAsync(u => u.Id == shortUrlId && u.CreatedByUserId == userId);

            if (shortUrl == null)
                return null;

            var logs = await _context.AccessLogs
                .Where(l => l.ShortenedUrlId == shortUrlId)
                .OrderByDescending(l => l.ClickTimestamp)
                .ToListAsync();

            return logs.Select(log => new AccessLogDTO
            {
                Id = log.Id,
                ShortenedUrlId = log.ShortenedUrlId,
                IPAddress = log.IPAddress,
                UserAgent = log.UserAgent,
                ReferrerUrl = log.ReferrerUrl,
                UTMSource = log.UTMSource,
                UTMMedium = log.UTMMedium,
                UTMCampaign = log.UTMCampaign,
                ClickTimestamp = TimeZoneInfo.ConvertTimeFromUtc(log.ClickTimestamp, TimeZoneInfo.Local),
                Browser = _browserInfoService.GetBrowserInfo(log.UserAgent).Browser,
                BrowserVersion = _browserInfoService.GetBrowserInfo(log.UserAgent).Version,
                DeviceType = _browserInfoService.GetDeviceType(log.UserAgent),
                Country = log.Country,
                City = log.City,
                Region = log.Region
            }).ToList();

        }
    }
}
