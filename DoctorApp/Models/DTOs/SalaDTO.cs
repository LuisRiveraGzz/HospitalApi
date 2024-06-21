using Newtonsoft.Json;

namespace DoctorApp.Models.DTOs
{
    public class SalaDTO
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("numeroSala")]
        public string NumeroSala { get; set; } = null!;
        [JsonProperty("doctor")]
        public int? Doctor { get; set; }
        [JsonProperty("paciente")]
        public int? Paciente { get; set; }
        [JsonProperty("estado")]
        public sbyte Estado { get; set; }
    }
}
