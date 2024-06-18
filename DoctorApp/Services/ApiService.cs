using DoctorApp.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace DoctorApp.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        public ApiService()
        {
                _httpClient = new HttpClient
                {
                    BaseAddress = new Uri("https://hospitalapi.websitos256.com/")
                };
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }
        public async Task<string> Login(LoginDTO loginDto)
        {
            var jsonContent = new StringContent(JsonSerializer.Serialize(loginDto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/Login", jsonContent);
            try
            {
               var result = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    return result;
                }
                else
                {
                    
                    return null;
                }
            }
            catch (Exception e)
            {

                MessageBox.Show($"{e.Message}", "Errors");
                return null;
            }
        }
    }
}
