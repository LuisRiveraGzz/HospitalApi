using Newtonsoft.Json;
using PacienteApp.Models.DTOs;

namespace PacienteApp.Services
{
    public class ApiService
    {
        public async Task<PacienteDTO> GetPaciente(string Nombre)
        {
            try
            {
                HttpClient client = new()
                {
                    BaseAddress = new Uri("https://hospitalapi.websitos256.com/api/Pacientes/")
                };
                var response = await client.GetAsync($"{Nombre}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var paciente = JsonConvert.DeserializeObject<PacienteDTO>(json);
                    return paciente ?? new();
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Ok");

            }
            return new();
        }

    }
}
