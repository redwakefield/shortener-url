using System;
using System.Text.Json;
using System.Text.RegularExpressions;
using AutoMapper;
using Shortener.DTO;
using Shortener.Model;

namespace Shortener.Services
{
    public class ControllerService : IControllerService
    {
        private IDbContext _dbContext;
        public IUrlHelper _urlHelper;
        private IMapper _mapper;
        private string targetUrl = "redwakefield.com";
        public ControllerService(IDbContext dbContext, IUrlHelper urlHelper, IMapper mapper)
        {
            _dbContext = dbContext;
            _urlHelper = urlHelper;
            _mapper = mapper;
        }

        public UrlDataDto GetUrlData(string shortUrl)
        {
            var id = _urlHelper.GetId(shortUrl);
            var urlData = _dbContext.GetUrl(id);
            var urlDataDto = _mapper.Map<UrlDataDto>(urlData);
            return urlDataDto;
        }

        public UrlDataDto AddUrlData(UrlDataDto urlDataDto, string requestScheme, string requestHost)
        {

            var newEntry = new UrlData
            {
                Url = urlDataDto.Url,
                ShorteningDateTime = DateTime.Now.Date,
                ShortUrl = "",
                Hit = 1
            };

            var id = _dbContext.AddUrl(newEntry);
            UrlDataDto responseUrlDataDto = new UrlDataDto() { Url = $"{requestScheme}://{requestHost}/{_urlHelper.GetShortUrl(id)}" };
            
            var updateEntry = new UrlData
            {
                Id = id,
                Url = urlDataDto.Url,
                ShortUrl = responseUrlDataDto.Url,
                Hit = 1
            };

            _dbContext.UpdateUrl(updateEntry);

            return responseUrlDataDto;
        }


        public bool CheckIfUrlExists(UrlDataDto urlDataDto)
        {
            bool urlExists = false;
            urlExists = _dbContext.CheckIfUrlExists(urlDataDto.Url);

            
            return urlExists;

        }

        public ShortUrlDto GetExistingShortUrlData(string longUrl)
        {
            var urlData = _dbContext.GetExistingShortUrl(longUrl);

            var updateEntry = new UrlData
            {
                Id = urlData.Id,
                Url = urlData.Url,
                ShorteningDateTime = DateTime.Now.Date,
                ShortUrl = urlData.ShortUrl,
                Hit = urlData.Hit + 1
            };
            
            var result = _dbContext.UpdateUrl(updateEntry);
            var urlDataDto = _mapper.Map<ShortUrlDto>(urlData);

            return  urlDataDto;
        }

        public bool CheckUserAgent(string headersUserAgent)
        {
            bool userAgentBrowser = false;
            if (headersUserAgent.Contains("Mozilla") ||
                headersUserAgent.Contains("AppleWebKit") ||
                headersUserAgent.Contains("Chrome") ||
                headersUserAgent.Contains("Safari") ||
                headersUserAgent.Contains("Edg"))
                userAgentBrowser = true;

            return userAgentBrowser;
        }

        public bool ValidateHttpUrl(string url, out Uri result)
        {
            if (!Regex.IsMatch(url, @"https?:\/\/", RegexOptions.IgnoreCase))
                url = "https://" + url;
            
            if (Uri.TryCreate(url, UriKind.Absolute, out result))
            return (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps);

            return false;
        }

    }
}