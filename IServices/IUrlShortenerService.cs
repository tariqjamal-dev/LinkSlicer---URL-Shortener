namespace LinkSlicer.IServices
{
    public interface IUrlShortenerService
    {
        Task<string> ShortenUrl(string originalUrl, string? customAlias, string? userId, string userIp);
        Task<string?> RedirectToOriginal(string shortCode, HttpContext context);
    }
}
