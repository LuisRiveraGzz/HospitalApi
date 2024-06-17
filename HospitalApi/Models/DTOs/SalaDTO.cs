namespace HospitalApi.Models.DTOs
{
    public class SalaDTO
    {
        public int id { get; set; }
        public string NumeroSala { get; set; } = null!;
        public int Doctor { get; set; }
    }
}
