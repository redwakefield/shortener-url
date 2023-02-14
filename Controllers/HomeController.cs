using System;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Shortener.DTO;
using Shortener.Services;

namespace Shortener.Controllers 
{

    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IControllerService _controllerService;

        public HomeController(IControllerService controllerService)
    {
        _controllerService = controllerService;
    }

        [HttpGet("{shortUrl}")]
        public IActionResult GetUrl(string shortUrl)
        {
            if (String.IsNullOrEmpty(shortUrl))
                return BadRequest();

            var urlDataDto = _controllerService.GetUrlData(shortUrl);

            if (urlDataDto == null)
                return NotFound();

            var userAgentBrowser = _controllerService.CheckUserAgent(this.Request.Headers["User-Agent"].ToString());

            if (userAgentBrowser)
                return RedirectPermanent(urlDataDto.Url);

            return Ok(urlDataDto);
        }
    
        [HttpPost("shorten")]
        public IActionResult ShortenUrl([FromBody] UrlDataDto requestUrlDataDto)
        {
            if (requestUrlDataDto == null)
                return BadRequest();

            if (!Uri.TryCreate(requestUrlDataDto.Url, UriKind.Absolute, out Uri result))
                ModelState.AddModelError("Message", "URL shouldn't be empty");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // if (!_controllerService.ValidateHttpUrl(requestUrlDataDto.Url, out result))
            //     ModelState.AddModelError("Message", "URL is not a valid http url");
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (_controllerService.CheckIfUrlExists(requestUrlDataDto))
                ModelState.AddModelError("URL Exist:", _controllerService.GetExistingShortUrlData(requestUrlDataDto.Url).ShortUrl);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var responseUrlDataDto = _controllerService.AddUrlData(requestUrlDataDto, this.Request.Scheme, this.Request.Host.ToString());

            return Created("shortUrl", responseUrlDataDto);

        }
    }
}