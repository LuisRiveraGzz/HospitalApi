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
    public class SalasService
    {
        private readonly HttpClient Client;
        public SalasService()
        {
            Client = new()
            {
                BaseAddress = new Uri("https://hospitalapi.websitos256.com/api/Salas/")
            };
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Settings.Default.Token);
            Client.Timeout = TimeSpan.FromMinutes(10);
        }
        public async Task CerrarSesion()
        {
            //Quitar token el token a la configuración
            Settings.Default.Reset();
            //Quitar token al cliente http
            Client.DefaultRequestHeaders.Authorization = null;
            //Mostrar Login
            LoginView login = new();
            login.Show();
            //Cierra la antigua
            var doctoresWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is not LoginView);
            doctoresWindow?.Close();
            await Task.CompletedTask;
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
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    MessageBox.Show("Credenciales Expiradas", "Han expirado sus credenciales, inicia sesion nuevamente", MessageBoxButton.OK);
                    await CerrarSesion();
                }
            }
            return [];
        }
        public async Task<IEnumerable<SalaDTO>> GetSala(string NumSala)
        {
            try
            {
                var response = await Client.GetAsync($"{NumSala}");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                var Salas = JsonConvert.DeserializeObject<IEnumerable<SalaDTO>>(json);
                return Salas ?? [];
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    MessageBox.Show("Credenciales Expiradas", "Han expirado sus credenciales, inicia sesion nuevamente", MessageBoxButton.OK);
                    await CerrarSesion();
                }
            }
            return [];
        }
        //Post: /api/Salas/Agregar
        public async Task Agregar(SalaDTO dto)
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
        //Put: /api/Salas/Editar
        public async Task Editar(SalaDTO dto)
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
        //Delete: /api/Salas/Eliminar
        public async Task Eliminar(SalaDTO dto)
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
        //Put: /api/Salas/1/paciente/1
        public async Task AsignarPaciente(int idSala, int idPaciente)
        {
            try
            {
                var response = await Client.PutAsync($"{idSala}/AsignarPaciente/{idPaciente}", null);
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
        //Put: /api/Salas/1
        public async Task QuitarPaciente(int idSala)
        {
            try
            {
                var response = await Client.PutAsync($"QuitarPaciente/{idSala}", null);
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
        //Get: /api/Salas/1
        public async Task<SalaDTO> GetSalaByDoctor(int iddoctor)
        {
            try
            {
                var response = await Client.GetAsync($"{iddoctor}");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                var sala = JsonConvert.DeserializeObject<SalaDTO>(json);
                return sala ?? new();
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
        public async Task ActivarSala(int idSala)
        {
            try
            {
                var response = await Client.PutAsync($"UtilizarSala/{idSala}", null);
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
        public async Task DesactivarSala(int idSala)
        {
            try
            {
                var response = await Client.PutAsync($"InutilizarSala/{idSala}", null);
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

    }
}
