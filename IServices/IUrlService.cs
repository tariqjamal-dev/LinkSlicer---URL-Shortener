using LinkSlicer.Models;

namespace LinkSlicer.IServices
{
    public interface IUrlService
    {
        Task<List<ShortUrl>> GetUserShortUrls(string? userId);
    }
}
