namespace LinkSlicer.Helpers
{
    public static class UrlHelper
    {
        private const string Characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private const int ShortCodeLength = 6;

        public static string GenerateShortCode()
        {
            Random rand = new();
            return new string(Enumerable.Repeat(Characters, ShortCodeLength)
                .Select(s => s[rand.Next(s.Length)]).ToArray());
        }
    }
}
