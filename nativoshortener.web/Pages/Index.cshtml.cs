using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace nativoshortener.web.Pages
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public IndexModel(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(configuration["apiurl"]);
            _httpClient.DefaultRequestHeaders.Add("Authorization", httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString());
        }

        public async Task<IActionResult> OnGet()
        {
            string path = string.Join("",Request.Path.Value.SkipWhile(c => c == '/'));

            if (path == string.Empty || path == "Index")    
                return Page(); 

           return await RedirectShortCode(path);
        }

        public async Task<IActionResult> OnGetTop20MostVisitedAsync()
        {
            var response = await _httpClient.GetAsync("shortener/top20mostvisited");
            string responseBody = await response.Content.ReadAsStringAsync();

            return new JsonResult(responseBody);
        }

        public async Task<IActionResult> OnPostShortenUrlAsync([FromBody] string url)
        {
            string json = JsonConvert.SerializeObject(url);
            
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{_httpClient.BaseAddress}shortener/shortenlink"),
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            var response = await _httpClient.SendAsync(request);
            var responseBody = await response.Content.ReadAsStringAsync();
                
            return new JsonResult($"{Request.Headers["Referer"]}{responseBody}");
        }

        private async Task<IActionResult> RedirectShortCode(string shortCode)
        {
            var response = await _httpClient.GetAsync($"shortener/geturl?shortCode={shortCode}");

            string responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return Redirect("/Index");

            return Redirect(responseBody);
        }
    }
}
