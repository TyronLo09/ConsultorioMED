using System;
using System.Collections.Generic;

namespace ConsultorioMedAPP.Models;

public partial class Seguro
{
    public int IdSeguro { get; set; }

    public int IdTipoSeguro { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public int IdCedula { get; set; }

    public bool? Activo { get; set; }

    public virtual Persona IdCedulaNavigation { get; set; } = null!;

    public virtual TipoSeguro IdTipoSeguroNavigation { get; set; } = null!;

    public virtual ICollection<Paciente> Pacientes { get; set; } = new List<Paciente>();
}
