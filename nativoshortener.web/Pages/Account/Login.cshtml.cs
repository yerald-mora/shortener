using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using nativoshortener.api.DTOs;
using Newtonsoft.Json;

namespace nativoshortener.web.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public LoginDTO Login { get; set; }

        public LoginModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(configuration["apiurl"]);
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync(LoginDTO login)
        {
            string json = JsonConvert.SerializeObject(login);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{_httpClient.BaseAddress}users/login"),
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("LoginFailed", response.Content.ReadAsStringAsync().Result);
                return Page();
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<TokenDTO>(responseBody);

            HttpContext.Session.SetString("Token", token.Token);

            return Redirect("/Index");
        }

        public IActionResult OnPostLogoutAsync()
        {
            HttpContext.Session.Remove("Token");

            return Page();
        }
    }
}
