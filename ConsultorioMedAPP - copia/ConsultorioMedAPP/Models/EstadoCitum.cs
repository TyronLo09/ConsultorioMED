using System;
using System.Collections.Generic;

namespace ConsultorioMedAPP.Models;

public partial class EstadoCitum
{
    public int IdEstadoCita { get; set; }

    public string Descripcion { get; set; } = null!;

    public bool? Activo { get; set; }

    public virtual ICollection<Citum> Cita { get; set; } = new List<Citum>();
}
