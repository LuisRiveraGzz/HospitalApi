using Newtonsoft.Json;

namespace DoctorApp.Models.DTOs
{
    public class PacienteDTO
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("nombre")]
        public string Nombre { get; set; } = null!;

    }
}
