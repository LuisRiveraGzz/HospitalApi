using DoctorApp.Models.DTOs;
using DoctorApp.Properties;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace DoctorApp.Services
{
    internal class SalasService
    {
        private readonly HttpClient Client;
        public SalasService()
        {
            Client = new()
            {
                BaseAddress = new Uri("https://hospitalapi.websitos256.com/api/Salas/")
            };
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Settings.Default.Token);
        }

        public async Task<IEnumerable<SalaDTO>> GetSalas()
        {
            try
            {
                var response = await Client.GetAsync("");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                var Salas = JsonConvert.DeserializeObject<IEnumerable<SalaDTO>>(json);
                return Salas ?? [];
            }
            catch
            {
                return [];
            }
        }

        public async Task Agregar(SalaDTO dto)
        {
            try
            {
                var response = await Client.PostAsJsonAsync("Agregar", dto);
                response.EnsureSuccessStatusCode();
            }
            catch { }
        }
        public async Task Editar(SalaDTO dto)
        {
            try
            {
                var response = await Client.PutAsJsonAsync("Editar", dto);
                response.EnsureSuccessStatusCode();
            }
            catch { }
        }
        public async Task Eliminar(SalaDTO dto)
        {
            try
            {
                var response = await Client.DeleteAsync($"Eliminar {dto.Id}");
                response.EnsureSuccessStatusCode();
            }
            catch { }
        }
    }
}
