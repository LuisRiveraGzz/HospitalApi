using PacienteApp.Models.DTOs;
using System.Net.Http.Json;

namespace PacienteApp.Services
{
    public class ApiService
    {
        private readonly HttpClient Client = new()
        {
            BaseAddress = new Uri("https://hospitalapi.websitos256.com/api/Pacientes/")
        };
        public async Task AgregarPaciente(PacienteDTO dto)
        {
            try
            {
                var response = await Client.PostAsJsonAsync("Agregar", dto);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
            }
        }
    }
}
