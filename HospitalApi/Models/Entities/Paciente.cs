using System;
using System.Collections.Generic;

namespace HospitalApi.Models.Entities;

public partial class Paciente
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Sala> Sala { get; set; } = new List<Sala>();
}
