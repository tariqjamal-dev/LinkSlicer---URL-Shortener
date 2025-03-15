using LinkSlicer.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LinkSlicer.Controllers
{
    [Authorize]
    public class DashboardController(IUrlService urlService, IAccessLogService accessLogService) : Controller
    {
        private readonly IUrlService _urlService = urlService;
        private readonly IAccessLogService _accessLogService = accessLogService;

        public async Task<IActionResult> HomeAsync()
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var urls = await _urlService.GetUserShortUrls(userId);
            return View(urls);

        }

        [HttpGet("ViewLogs/{shortUrlId}")]
        public async Task<IActionResult> ViewLogs(int shortUrlId)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

            var logs = await _accessLogService.GetAccessLogs(shortUrlId, userId);
            if (logs == null)
                return NotFound("Short URL not found or access denied.");

            return View(logs);
        }

    }
}
