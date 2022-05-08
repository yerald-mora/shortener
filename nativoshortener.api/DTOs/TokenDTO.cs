using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nativoshortener.api.DTOs
{
    public class TokenDTO
    {
        public string Token { get; set; }
        public DateTime Expiracion { get; set; }
        public string UserName { get; set; }
    }
}
