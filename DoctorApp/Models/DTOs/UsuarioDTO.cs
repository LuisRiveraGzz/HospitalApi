namespace DoctorApp.Models.DTOs
{
    public class UsuarioDTO
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;

        public string Contraseña { get; set; } = null!;

        public sbyte Rol { get; set; }
    }
}