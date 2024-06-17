using System;
using System.Collections.Generic;

namespace HospitalApi.Models.Entities;

public partial class Sala
{
    public int Id { get; set; }

    public string NumeroSala { get; set; } = null!;

    public int? Doctor { get; set; }

    public int? Paciente { get; set; }

    public sbyte Estado { get; set; }

    public virtual Usuario? DoctorNavigation { get; set; }

    public virtual Paciente? PacienteNavigation { get; set; }
}
