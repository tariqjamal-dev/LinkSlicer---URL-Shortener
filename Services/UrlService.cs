using LinkSlicer.Data;
using LinkSlicer.IServices;
using LinkSlicer.Models;
using Microsoft.EntityFrameworkCore;

namespace LinkSlicer.Services
{
    public class UrlService(ApplicationDbContext context) : IUrlService
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<List<ShortUrl>> GetUserShortUrls(string? userId)
        {
            var urls = await _context.ShortUrls
                .Where(u => u.CreatedByUserId == userId)
                .OrderByDescending(u => u.CreatedAt)
                .ToListAsync();

            foreach (var url in urls)
            {
                url.CreatedAt = TimeZoneInfo.ConvertTimeFromUtc(url.CreatedAt, TimeZoneInfo.Local);
            }

            return urls;
        }
    }
}
