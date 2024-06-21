using Newtonsoft.Json;

namespace DoctorApp.Models.DTOs
{
    public class LoginDTO
    {
        [JsonProperty("usuario")]
        public string Usuario { get; set; } = null!;
        [JsonProperty("contraseña")]
        public string Contraseña { get; set; } = null!;
    }
}
