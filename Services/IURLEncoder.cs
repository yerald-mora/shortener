using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nativoshortener.api.Services
{
    public interface IURLEncoder
    {
        public string CreateShortURLFromId(int id);
        public int GetIdFromShortURL(string shortUrl);
    }
}
