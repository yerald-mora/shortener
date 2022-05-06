using HashidsNet;

namespace nativoshortener.api.Services
{
    public class HashIdURLEncoder : IURLEncoder
    {
        private const string Salt = "This is my pepper";

        public string CreateShortURLFromId(int id)
        {
            var hashids = new Hashids(Salt);

            return hashids.Encode(id);
        }

        public int GetIdFromShortURL(string shortUrl)
        {
            var hashids = new Hashids(Salt);
            int[] decoded = hashids.Decode(shortUrl);

            return (decoded.Length == 0) ? 0 : decoded[0];
        }
    }
}
