using System;
using System.Collections.Generic;

namespace ConsultorioMedAPP.Models;

public partial class Doctor
{
    public int IdCedula { get; set; }

    public int IdEspecialidad { get; set; }

    public bool? Activo { get; set; }

    public virtual ICollection<Citum> Cita { get; set; } = new List<Citum>();

    public virtual Persona IdCedulaNavigation { get; set; } = null!;

    public virtual Especialidad IdEspecialidadNavigation { get; set; } = null!;

    public virtual ICollection<Turno> Turnos { get; set; } = new List<Turno>();
}
