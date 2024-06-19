namespace HospitalApi.Models.DTOs
{
    public class UsuarioGet
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;

        public string Contraseña { get; set; } = null!;

        public sbyte Rol { get; set; }

        public string Sala { get; set; } = null!;
    }
}
