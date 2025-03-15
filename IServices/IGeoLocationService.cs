namespace LinkSlicer.IServices
{
    public interface IGeoLocationService
    {
        Task<(string Country, string City, string Region)> GetGeoLocation(string ip);
    }
}
