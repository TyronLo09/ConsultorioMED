using System;
using System.Collections.Generic;

namespace ConsultorioMedAPP.Models;

public partial class TipoTurno
{
    public int IdTipoTurno { get; set; }

    public string Descripcion { get; set; } = null!;

    public TimeOnly HoraEntrada { get; set; }

    public TimeOnly HoraSalida { get; set; }

    public virtual ICollection<Turno> Turnos { get; set; } = new List<Turno>();
}
