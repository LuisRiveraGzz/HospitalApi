using System;
using System.Collections.Generic;

namespace HospitalApi.Models.Entities;

public partial class Estadistica
{
    public int Id { get; set; }

    public int IdDoctor { get; set; }

    public int PacientesAtendidos { get; set; }

    public TimeOnly TiempoPromedioEspera { get; set; }

    public virtual Usuario IdDoctorNavigation { get; set; } = null!;
}
