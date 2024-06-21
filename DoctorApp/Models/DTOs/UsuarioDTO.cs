namespace DoctorApp.Models.DTOs
{
    public class UsuarioDTO
    {
        public int id { get; set; }

        public string nombre { get; set; } = null!;
        public string contraseña { get; set; } = null!;
        public sbyte rol { get; set; }
        public IEnumerable<SalaDTO> sala { get; set; } = null!;
    }
}