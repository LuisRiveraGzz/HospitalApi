using Newtonsoft.Json;

namespace DoctorApp.Models.DTOs
{
    public class UsuarioDTO
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("nombre")]
        public string Nombre { get; set; } = null!;
        [JsonProperty("contraseña")]
        public string Contraseña { get; set; } = null!;
        [JsonProperty("rol")]
        public sbyte Rol { get; set; }
        [JsonProperty("sala")]
        public IEnumerable<SalaDTO> Sala { get; set; } = null!;
    }
}