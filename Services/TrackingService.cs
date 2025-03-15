using Azure.Core;
using LinkSlicer.Data;
using LinkSlicer.IServices;
using LinkSlicer.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace LinkSlicer.Services
{
    public class TrackingService(ApplicationDbContext context, IIpService ipService, IGeoLocationService geoLocationService) : ITrackingService
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IIpService _ipService = ipService;
        private readonly IGeoLocationService _geoLocationService = geoLocationService;
        public async Task SaveAccessLog(int shortUrlId, HttpContext context)
        {
            string ip = await _ipService.GetClientIp(context);
            var (country, city, region) = await _geoLocationService.GetGeoLocation(ip);

            var log = new AccessLog
            {
                ShortenedUrlId = shortUrlId,
                IPAddress = ip,
                UserAgent = context.Request.Headers.UserAgent.ToString(),
                ReferrerUrl = context.Request.Headers.Referer.ToString(),
                UTMSource = context.Request.Query["utm_source"],
                UTMMedium = context.Request.Query["utm_medium"],
                UTMCampaign = context.Request.Query["utm_campaign"],
                Country = country,
                City = city,
                Region = region
            };

            await _context.AccessLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }
    }
}
