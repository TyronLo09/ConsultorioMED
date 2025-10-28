using System;
using System.Collections.Generic;

namespace ConsultorioMedAPP.Models;

public partial class Diagnostico
{
    public int IdDiagnostico { get; set; }

    public int CitaIdCita { get; set; }

    public string Descripcion { get; set; } = null!;

    public string CodigoDiagnostico { get; set; } = null!;

    public DateOnly FechaDiagnostico { get; set; }

    public string Recomendaciones { get; set; } = null!;

    public DateTime FechaCreacion { get; set; }

    public bool? Estado { get; set; }

    public virtual Citum CitaIdCitaNavigation { get; set; } = null!;

    public virtual ICollection<Medicina> Medicinas { get; set; } = new List<Medicina>();
}
