using System;
using Shortener.DTO;

namespace Shortener.Services
{
    public interface IControllerService
    {
        UrlDataDto AddUrlData(UrlDataDto urlDataDto, string requestScheme, string requestHost);
        UrlDataDto GetUrlData(string shortUrl);
        bool CheckUserAgent(string headersUserAgent);
        bool CheckIfUrlExists(UrlDataDto urlDataDto);
        bool ValidateHttpUrl(string url, out Uri result);
        ShortUrlDto GetExistingShortUrlData(string shortUrl);
    }
}