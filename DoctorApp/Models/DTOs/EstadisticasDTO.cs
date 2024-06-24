namespace DoctorApp.Models.DTOs
{
    public class EstadisticasDTO
    {
        public int IdDoctor { get; set; }
        public int NumPacientes { get; set; } = 0;
        private TimeOnly promedioEspera;
        public TimeOnly PromedioEspera
        {
            get { return promedioEspera; }
            set
            {
                Random r = new();
                //Asigna un numero entre 50 y 100 segundos
                promedioEspera = new TimeOnly(r.Next(50000, 1000000));
            }
        }
    }
}
