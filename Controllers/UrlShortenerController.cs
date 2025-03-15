using LinkSlicer.Data;
using LinkSlicer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Sockets;
using System.Net;
using System.Security.Claims;
using Newtonsoft.Json;
using LinkSlicer.IServices;
using LinkSlicer.Services;

namespace LinkSlicer.Controllers
{
    public class UrlShortenerController(IUrlShortenerService urlShortenerService, IIpService ipService) : Controller
    {
        private readonly IUrlShortenerService _urlShortenerService = urlShortenerService;
        private readonly IIpService _ipService = ipService;

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Shorten(string originalUrl, string customAlias)
        {
            if (string.IsNullOrEmpty(originalUrl))
                return BadRequest("Invalid URL");

            string? userId = User.Identity.IsAuthenticated ? User.FindFirst(ClaimTypes.NameIdentifier)?.Value : null;
            string userIp = await _ipService.GetClientIp(HttpContext);

            try
            {
                string shortCode = await _urlShortenerService.ShortenUrl(originalUrl, customAlias, userId, userIp);
                return Json(new { shortUrl = $"{Request.Scheme}://{Request.Host}/{shortCode}" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{shortCode}")]
        public async Task<IActionResult> RedirectToOriginal(string shortCode)
        {
            var redirectUrl = await _urlShortenerService.RedirectToOriginal(shortCode, HttpContext);

            if (redirectUrl == null)
                return NotFound();

            return Redirect(redirectUrl);
        }

    }
}
