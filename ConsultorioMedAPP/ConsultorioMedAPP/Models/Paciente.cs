using System;
using System.Collections.Generic;

namespace ConsultorioMedAPP.Models;

public partial class Paciente
{
    public int IdCedula { get; set; }

    public int? SeguroPacienteIdSeguro { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public bool? Estado { get; set; }

    public virtual ICollection<AntecedentesMedico> AntecedentesMedicos { get; set; } = new List<AntecedentesMedico>();

    public virtual ICollection<Citum> Cita { get; set; } = new List<Citum>();

    public virtual Persona IdCedulaNavigation { get; set; } = null!;

    public virtual Seguro? SeguroPacienteIdSeguroNavigation { get; set; }
}
