using System;
using System.Collections.Generic;

namespace HospitalApi.Models.Entities;

public partial class Usuario
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Contraseña { get; set; } = null!;

    public sbyte Rol { get; set; }

    public virtual ICollection<Sala> Sala { get; set; } = new List<Sala>();
}
