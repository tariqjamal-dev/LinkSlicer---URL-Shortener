namespace LinkSlicer.IServices
{
    public interface IIpService
    {
        Task<string> GetClientIp(HttpContext context);
    }
}
