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

        //Get: /
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
        //Post: /api/Salas/Agregar
        public async Task Agregar(SalaDTO dto)
        {
            try
            {
                var response = await Client.PostAsJsonAsync("Agregar", dto);
                response.EnsureSuccessStatusCode();
            }
            catch { }
        }
        //Put: /api/Salas/Editar
        public async Task Editar(SalaDTO dto)
        {
            try
            {
                var response = await Client.PutAsJsonAsync("Editar", dto);
                response.EnsureSuccessStatusCode();
            }
            catch { }
        }
        //Delete: /api/Salas/Eliminar
        public async Task Eliminar(SalaDTO dto)
        {
            try
            {
                var response = await Client.DeleteAsync($"Eliminar {dto.Id}");
                response.EnsureSuccessStatusCode();
            }
            catch { }
        }
        //Put: /api/Salas/1/paciente/1
        public async Task AsignarPaciente(int idSala, int idPaciente)
        {
            try
            {
                var response = await Client.PutAsync($"{idSala}/Paciente/{idPaciente}", null);
                response.EnsureSuccessStatusCode();
            }
            catch { }
        }
        //Put: /api/Salas/1
        public async Task QuitarPaciente(int idSala)
        {
            try
            {
                var response = await Client.PutAsync($"{idSala}", null);
                response.EnsureSuccessStatusCode();
            }
            catch { }
        }

    }
}
