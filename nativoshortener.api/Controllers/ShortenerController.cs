using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using nativoshortener.api.DTOs;
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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ShortenerController : ControllerBase
    {
        private readonly NativoDbContext _context;
        private readonly IURLEncoder _encoder;

        public ShortenerController(NativoDbContext context, IURLEncoder encoder)
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

            shortenedUrl = new ShortenedUrl()
            {
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

            if (shortenedUrl == null)
                return BadRequest("Invalid shortcode.");

            await AddURLVisit(id);
            return Ok(shortenedUrl.URL);
        }

        [HttpGet("top20mostvisited")]
        public async Task<ActionResult<IEnumerable<MostVisitedDTO>>> Get20MostVisited()
        {
            var mostVisiteds = new List<MostVisitedDTO>();

            var visits = await _context.ShortenedUrls
                .Include(s => s.Visits)
                .OrderByDescending(s => s.Visits.Count)
                .Where(s => s.Visits.Count > 0)
                .Take(20)
                .ToListAsync();

            foreach (var v in visits)
            {
                mostVisiteds.Add(new MostVisitedDTO()
                {
                    URL = v.URL,
                    ShortCode = v.ShortCode,
                    Visits = v.Visits.Count
                });
            }

            return mostVisiteds;
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

        private async Task AddURLVisit(int shortenedUrlId)
        {
            var visit = new Visit()
            {
                VisitDate = DateTime.Now,
                ShortenedUrlId = shortenedUrlId
            };

            _context.Visits.Add(visit);
            await _context.SaveChangesAsync();
        }

    }
}
