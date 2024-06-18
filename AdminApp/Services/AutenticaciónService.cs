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
            _httpClient.BaseAddress = new Uri("https://hospitalapi.websitos256.com/api/");
        }

        [HttpPost]
        public async Task<string> Login(LoginViewModel dto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Login", dto);
                if (response.IsSuccessStatusCode)
                {
                    var token = await response.Content.ReadAsStringAsync();
                    //Almacenamos la autorizacion en el cliente para hacer peticiones despues
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token);
                    return token ?? "";
                }
            }
            catch { }
            return "";
        }

    }
}
