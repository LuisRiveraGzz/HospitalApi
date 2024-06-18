using AdminApp.Models.ViewModels;
using System.Net.Http.Headers;

namespace AdminApp.Services
{
    public class SalasService(HttpClient Client, TokenService tokenService)
    {
        private void AsignarToken()
        {
            string? token = tokenService.GetToken();
            // Configurar la cabecera de autorización con el token JWT
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        public async Task<string> AsignarDoctor(int idSala, int idDoctor)
        {
            AsignarToken();
            HttpResponseMessage response = await Client.PutAsync($"{idSala}/AsignarDoctor/{idDoctor}", null);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                // Manejar el caso cuando la petición no tiene éxito
                throw new HttpRequestException($"Error al obtener los doctores. Código de estado: {response.StatusCode}");
            }
        }
        public async Task<string> QuitarDoctor(int idSala)
        {
            AsignarToken();
            HttpResponseMessage response = await Client.PutAsync($"QuitarDoctor/{idSala}", null);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new HttpRequestException($"Error al obtener los doctores. Código de estado: {response.StatusCode}");
            }
        }
        public async Task AgregarSala(SalaViewModel vm)
        {
            try
            {
                AsignarToken();
                var response = await Client.PostAsJsonAsync("Agregar", vm);
                response.EnsureSuccessStatusCode();
            }
            catch { }
        }
        public async Task UtilizarSala(int idSala)
        {
            try
            {
                var response = await Client.PutAsync($"UtilizarSala/{idSala}", null);
                response.EnsureSuccessStatusCode();
            }
            catch { }
        }
        public async Task InutilizarSala(int idSala)
        {
            try
            {
                var response = await Client.PutAsync($"UtilizarSala/{idSala}", null);
                response.EnsureSuccessStatusCode();
            }
            catch { }
        }
        public async Task EditarSala(SalaViewModel vm)
        {
            try
            {
                AsignarToken();
                var response = await Client.PutAsJsonAsync("Agregar", vm);
                response.EnsureSuccessStatusCode();
            }
            catch { }
        }
        public async Task EliminarSala(SalaViewModel vm)
        {
            try
            {
                AsignarToken();
                var response = await Client.DeleteAsync($"Eliminar/{vm.Id}");
                response.EnsureSuccessStatusCode();

            }
            catch { }
        }
    }
}
