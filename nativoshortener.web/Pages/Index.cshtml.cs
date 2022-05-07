using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace nativoshortener.web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiUrl;

        public IndexModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _apiUrl = configuration["apiurl"];
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostShortenUrlAsync([FromBody] string url)
        {
            string json = JsonConvert.SerializeObject(url);
            
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{_apiUrl}/shortener/shortenlink"),
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            using (var httpClient = _httpClientFactory.CreateClient())
            {
                var response = await httpClient.SendAsync(request);
                var responseBody = await response.Content.ReadAsStringAsync();
                
                return new JsonResult($"{Request.Headers["Referer"]}{responseBody}");
            }

        }
    }
}
