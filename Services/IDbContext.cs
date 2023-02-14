using Shortener.Model;

namespace Shortener.Services
{
    public interface IDbContext
    {

        int AddUrl(UrlData urlData);
        UrlData GetUrl(int id);
        bool CheckIfUrlExists(string longUrl);
        UrlData GetExistingShortUrl(string longUrl);
        bool UpdateUrl(UrlData urlData);
    }
}