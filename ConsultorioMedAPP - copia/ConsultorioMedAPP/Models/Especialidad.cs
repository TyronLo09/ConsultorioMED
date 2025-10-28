using System;
using System.Collections.Generic;

namespace ConsultorioMedAPP.Models;

public partial class Especialidad
{
    public int IdEspecialidad { get; set; }

    public string Descripcion { get; set; } = null!;

    public bool? Activo { get; set; }

    public virtual ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
}
