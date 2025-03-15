namespace LinkSlicer.IServices
{
    public interface IBrowserInfoService
    {
        (string Browser, string Version) GetBrowserInfo(string userAgent);
        string GetDeviceType(string userAgent);
    }
}
