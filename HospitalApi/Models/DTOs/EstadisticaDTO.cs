namespace HospitalApi.Models.DTOs
{
    public class EstadisticaDTO
    {
        public TimeSpan Ocio { get; set; }
        public TimeSpan TiempoAtencion { get; set; }
        public int PacientesAtendidos { get; set; }

    }
}
