using LinkSlicer.Models;

namespace LinkSlicer.IServices
{
    public interface IUserUrlService
    {
        Task MigrateAnonymousUrlsToUser(ApplicationUser user, HttpContext httpContext);
    }
}
