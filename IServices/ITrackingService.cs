namespace LinkSlicer.IServices
{
    public interface ITrackingService
    {
        Task SaveAccessLog(int shortUrlId, HttpContext context);
    }
}
