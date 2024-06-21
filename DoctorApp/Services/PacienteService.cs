using DoctorApp.Models.DTOs;
using DoctorApp.Properties;
using DoctorApp.Views;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Windows;

namespace DoctorApp.Services
{
    public class PacienteService
    {
        private readonly HttpClient Client;
        public PacienteService()
        {
            Client = new()
            {
                BaseAddress = new Uri("https://hospitalapi.websitos256.com/api/Pacientes/")
            };
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Settings.Default.Token);
        }
        public async Task CerrarSesion()
        {
            //Quitar token el token a la configuración
            Settings.Default.Token = null;
            //Quitar token al cliente http
            Client.DefaultRequestHeaders.Authorization = null;
            //Mostrar Login
            LoginView login = new();
            login.Show();
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<PacienteDTO>> GetPacientes()
        {
            try
            {
                var response = await Client.GetAsync("");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                var pacientes = JsonConvert.DeserializeObject<IEnumerable<PacienteDTO>>(json);
                return pacientes ?? [];
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    MessageBox.Show("Credenciales Expiradas",
                        "Han expirado sus credenciales, inicia sesion nuevamente",
                        MessageBoxButton.OK);
                    await CerrarSesion();
                }
            }
            return [];
        }
        public async Task<PacienteDTO> GetPaciente(string NumSala)
        {
            try
            {
                var response = await Client.GetAsync($"{NumSala}");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                var pacientes = JsonConvert.DeserializeObject<PacienteDTO>(json);
                return pacientes ?? new();
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    MessageBox.Show("Credenciales Expiradas", "Han expirado sus credenciales, inicia sesion nuevamente", MessageBoxButton.OK);
                    await CerrarSesion();
                }
            }
            return new();
        }
        public async Task Agregar(PacienteDTO dto)
        {
            try
            {
                var response = await Client.PostAsJsonAsync("Agregar", dto);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    MessageBox.Show("Credenciales Expiradas", "Han expirado sus credenciales, inicia sesion nuevamente", MessageBoxButton.OK);
                    await CerrarSesion();
                }
            }
        }
        public async Task Editar(PacienteDTO dto)
        {
            try
            {
                var response = await Client.PutAsJsonAsync("Editar", dto);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    MessageBox.Show("Credenciales Expiradas", "Han expirado sus credenciales, inicia sesion nuevamente", MessageBoxButton.OK);
                    await CerrarSesion();
                }
            }
        }
        public async Task Eliminar(PacienteDTO dto)
        {
            try
            {
                var response = await Client.DeleteAsync($"Eliminar {dto.Id}");
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    MessageBox.Show("Credenciales Expiradas", "Han expirado sus credenciales, inicia sesion nuevamente", MessageBoxButton.OK);
            }
        }
    }
}
