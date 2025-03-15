using LinkSlicer.Data;
using LinkSlicer.IServices;
using LinkSlicer.Models;
using Microsoft.EntityFrameworkCore;

namespace LinkSlicer.Services
{
    public class UserUrlService(ApplicationDbContext context, IIpService ipService) : IUserUrlService
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IIpService _ipService = ipService;

        public async Task MigrateAnonymousUrlsToUser(ApplicationUser user, HttpContext httpContext)
        {
            string userIp = await _ipService.GetClientIp(httpContext);
            var anonymousUrls = await _context.ShortUrls
                .Where(u => u.IPAddress == userIp && u.CreatedByUserId == null)
                .ToListAsync();

            if (anonymousUrls.Count != 0)
            {
                anonymousUrls.ForEach(u => u.CreatedByUserId = user.Id);
                await _context.SaveChangesAsync();
            }
        }

    }
}
