namespace DoctorApp.Models.DTOs
{
    public class SalaDTO
    {
        public int Id { get; set; }
        public string NumeroSala { get; set; } = null!;
        public int Doctor { get; set; }
        public int Paciente { get; set; }
        public sbyte Estado { get; set; }

    }
}
