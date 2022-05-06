using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using nativoshortener.api.Models;
using nativoshortener.api.Persistance;
using nativoshortener.api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nativoshortener.api.Controllers
{
    [ApiController]
    [Route("api/shortener")]
    public class ShortenerController : Controller
    {
        private readonly NativoDbContext _context;
        private readonly IURLEncoder _encoder;

        public ShortenerController(NativoDbContext context,IURLEncoder encoder)
        {
            _context = context;
            _encoder = encoder;
        }

        [HttpPost("shortenlink")]
        public async Task<ActionResult<string>> ShortenLink([FromBody] string url)
        {
            if (!IsValidUrl(url))
                return BadRequest("The URL format is not valid.");

            ShortenedUrl shortenedUrl;

            shortenedUrl = _context.ShortenedUrls.FirstOrDefault(s => s.URL == url);

            if (shortenedUrl != null)
                return shortenedUrl.ShortCode;

            int id = GetNextId();

            shortenedUrl = new ShortenedUrl() {
                Id = id,
                CreationDate = DateTime.Today,
                URL = url,
                ShortCode = _encoder.CreateShortURLFromId(id)
            };

            _context.Add(shortenedUrl);
            await _context.SaveChangesAsync();

            return shortenedUrl.ShortCode;
        }

        [HttpGet("geturl")]
        public async Task<ActionResult<string>> GetURLFromShortCode([FromQuery] string shortCode)
        {
            int id = _encoder.GetIdFromShortURL(shortCode);

            var shortenedUrl = await _context.ShortenedUrls.FindAsync(id);

            return (shortenedUrl == null) ? BadRequest("Invalid shortcode.") : Ok(shortenedUrl.URL);
        }

        private bool IsValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var inputUri);
        }

        private int GetNextId()
        {
            int? maxId = _context.ShortenedUrls.Max(s => (int?)s.Id);

            return (maxId ?? 0) + 1;
        }

    }
}
