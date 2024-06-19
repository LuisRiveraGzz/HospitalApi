namespace DoctorApp.Models.DTOs
{
    public class UsuarioGet
    {
        public int id { get; set; }
        public string nombre { get; set; } = null!;
        public string contraseña { get; set; } = null!;
        public sbyte rol { get; set; }
        public string sala { get; set; } = null!;
    }
}
