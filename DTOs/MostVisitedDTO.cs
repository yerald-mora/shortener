using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nativoshortener.api.DTOs
{
    public class MostVisitedDTO
    {
        public string URL { get; set; }
        public int Visits { get; set; }
        public string ShortCode { get; set; }
    }
}
