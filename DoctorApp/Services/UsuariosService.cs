using DoctorApp.Models.DTOs;
using DoctorApp.Properties;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace DoctorApp.Services
{
    public class UsuariosService
    {
        private readonly HttpClient Client;
        public UsuariosService()
        {
            Client = new()
            {
                BaseAddress = new Uri("https://hospitalapi.websitos256.com/api/Usuarios/")
            };
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Settings.Default.Token);
        }

        public async Task<IEnumerable<UsuarioDTO>> GetUsuarios()
        {
            try
            {
                var response = await Client.GetAsync("");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                var usuarios = JsonConvert.DeserializeObject<IEnumerable<UsuarioDTO>>(json);
                return usuarios ?? [];
            }
            catch
            {
                return [];
            }
        }
        public async Task<UsuarioGet> GetUsuario(int id)
        {
            try
            {
                var response = await Client.GetAsync($"{id}");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                var usuario = JsonConvert.DeserializeObject<UsuarioGet>(json);
                return usuario ?? new();
            }
            catch
            {
                return new();
            }
        }
        public async Task Agregar(UsuarioDTO dto)
        {
            try
            {
                var response = await Client.PostAsJsonAsync("Agregar", dto);
                response.EnsureSuccessStatusCode();
            }
            catch { }
        }
        public async Task Editar(UsuarioDTO dto)
        {
            try
            {
                var response = await Client.PutAsJsonAsync("Editar", dto);
                response.EnsureSuccessStatusCode();
            }
            catch { }
        }
        public async Task Eliminar(UsuarioDTO dto)
        {
            try
            {
                var response = await Client.DeleteAsync($"Eliminar/{dto.Id}");
                response.EnsureSuccessStatusCode();
            }
            catch { }
        }
    }
}
