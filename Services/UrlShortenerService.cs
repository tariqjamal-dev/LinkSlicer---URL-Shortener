using LinkSlicer.Data;
using LinkSlicer.Helpers;
using LinkSlicer.IServices;
using LinkSlicer.Models;
using Microsoft.EntityFrameworkCore;

namespace LinkSlicer.Services
{
    public class UrlShortenerService(ApplicationDbContext context, ITrackingService trackingService) : IUrlShortenerService
    {
        private readonly ApplicationDbContext _context = context;
        private readonly ITrackingService _trackingService = trackingService;

        public async Task<string> ShortenUrl(string originalUrl, string? customAlias, string? userId, string userIp)
        {
            if (string.IsNullOrEmpty(originalUrl))
                throw new ArgumentException("Invalid URL");

            if (userId == null && _context.ShortUrls.Count(u => u.IPAddress == userIp) >= 30)
                throw new InvalidOperationException("URL limit exceeded");

            string shortCode = customAlias;

            if (!string.IsNullOrEmpty(customAlias))
            {
                // Check if custom alias is already taken
                if (_context.ShortUrls.Any(x => x.ShortCode == customAlias))
                {
                    throw new InvalidOperationException("This alias is unavailable. Please try another one.");
                }
            }
            else
            {
                // Generate random short code
                do
                {
                    shortCode = UrlHelper.GenerateShortCode();
                } while (_context.ShortUrls.Any(x => x.ShortCode == shortCode)); // Ensure uniqueness
            }

            var shortUrl = new ShortUrl
            {
                OriginalUrl = originalUrl,
                ShortCode = shortCode,
                CreatedByUserId = userId,
                IPAddress = userIp
            };

            await _context.ShortUrls.AddAsync(shortUrl);
            await _context.SaveChangesAsync();

            return shortCode;
        }

        public async Task<string?> RedirectToOriginal(string shortCode, HttpContext context)
        {
            var shortUrl = await _context.ShortUrls.FirstOrDefaultAsync(x => x.ShortCode == shortCode);
            if (shortUrl == null) return null;

            await _trackingService.SaveAccessLog(shortUrl.Id, context);

            return shortUrl.OriginalUrl;
        }
    }
}
