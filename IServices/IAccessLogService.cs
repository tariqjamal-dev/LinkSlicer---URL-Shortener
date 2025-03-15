using LinkSlicer.Models;

namespace LinkSlicer.IServices
{
    public interface IAccessLogService
    {
        Task<List<AccessLogDTO>?> GetAccessLogs(int shortUrlId, string userId);
    }
}
