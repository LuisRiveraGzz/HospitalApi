using AdminApp.Models.ViewModels;
using System.Net.Http.Headers;

namespace AdminApp.Services
{
    public class DoctoresService(HttpClient Client, TokenService tokenService)
    {
        private void AsignarToken()
        {
            string? token = tokenService.GetToken();
            // Configurar la cabecera de autorización con el token JWT
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        public async Task<string> ObtenerDoctoresAsync()
        {
            AsignarToken();
            HttpResponseMessage response = await Client.GetAsync("doctores");
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
        public async Task AgregarDoctor(UsuarioViewModel vm)
        {
            try
            {
                AsignarToken();
                var response = await Client.PostAsJsonAsync("Agregar", vm);
                response.EnsureSuccessStatusCode();
            }
            catch { }
        }
        public async Task EditarDoctor(UsuarioViewModel vm)
        {
            try
            {
                AsignarToken();
                var response = await Client.PutAsJsonAsync("Agregar", vm);
                response.EnsureSuccessStatusCode();
            }
            catch { }
        }
        public async Task EliminarDoctor(UsuarioViewModel vm)
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
