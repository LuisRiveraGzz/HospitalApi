using AdminApp.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace AdminApp.Services
{
    public class AutenticaciónService
    {
        private readonly HttpClient _httpClient = new();
        public AutenticaciónService()
        {
            _httpClient.BaseAddress = new Uri("https://localhost:7095/");
        }

        [HttpPost]
        public async Task<string> Login(LoginViewModel dto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Login", dto);
                if (response.IsSuccessStatusCode)
                {
                    var token = await response.Content.ReadAsStringAsync();
                    //Almacenamos la autorizacion
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token);
                    return token ?? "";
                }
            }
            catch { }
            return "";
        }
    }
}
