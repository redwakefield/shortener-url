using System;
using System.ComponentModel.DataAnnotations;

namespace Shortener.Model 
{
    public class UrlData
    {
        public int Id { get; set; }
        [Required]
        public string Url { get; set; }
        public DateTime ShorteningDateTime { get; set; }
        public string ShortUrl { get; set; }
        public int Hit { get; set; }
    }
}