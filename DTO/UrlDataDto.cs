using System.ComponentModel.DataAnnotations;

namespace Shortener.DTO
{
    public class UrlDataDto
    {
        [Required]
        public string Url { get; set; }
      
    }

    public class ShortUrlDto
    {   
        [Required]
        public string ShortUrl { get; set; }
    }
}