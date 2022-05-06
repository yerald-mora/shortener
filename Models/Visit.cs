using System;

namespace nativoshortener.api.Models
{
    public class Visit
    {
        public int Id { get; set; }
        public DateTime VisitDate { get; set; }
        
        public int ShortenedUrlId { get; set; }
        public ShortenedUrl ShortenedUrl { get; set; }
    }
}
