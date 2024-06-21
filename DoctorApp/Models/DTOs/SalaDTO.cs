namespace DoctorApp.Models.DTOs
{
    public class SalaDTO
    {
        public int id { get; set; }
        public string numeroSala { get; set; } = null!;
        public int? doctor { get; set; }
        public int? paciente { get; set; }
        public sbyte estado { get; set; }
    }
}
