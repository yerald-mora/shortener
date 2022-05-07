using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nativoshortener.api.Models
{
    public class ShortenedUrl
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public string URL { get; set; }
        public string ShortCode { get; set; }

        public ICollection<Visit> Visits { get; set; }
    }
}
