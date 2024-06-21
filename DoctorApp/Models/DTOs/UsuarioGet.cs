using Newtonsoft.Json;

namespace DoctorApp.Models.DTOs
{
    public class UsuarioGet
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
        public string Sala { get; set; } = null!;
    }
}
