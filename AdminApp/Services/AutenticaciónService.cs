using AdminApp.Models.ViewModels;
using System.Net.Http.Headers;

namespace AdminApp.Services
{
    public class AutenticaciónService
    {
        private readonly HttpClient _httpClient;
        private readonly TokenService _tokenService;

        public AutenticaciónService(HttpClient httpClient, TokenService tokenService, ILogger<AutenticaciónService> logger)
        {
            _httpClient = httpClient;
            _tokenService = tokenService;
            _httpClient.BaseAddress = new Uri("https://hospitalweb.websitos256.com/api/");
        }

        public async Task<string> Login(LoginViewModel dto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("login", dto);
                if (response.IsSuccessStatusCode)
                {
                    var token = await response.Content.ReadAsStringAsync();
                    _tokenService.SetToken(token); // Almacenar el token
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    return token;
                }
            }
            catch { }
            return "";
        }
    }
}