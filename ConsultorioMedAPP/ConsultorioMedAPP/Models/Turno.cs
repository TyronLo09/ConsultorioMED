using System;
using System.Collections.Generic;

namespace ConsultorioMedAPP.Models;

public partial class Turno
{
    public int IdTurno { get; set; }

    public int IdCedula { get; set; }

    public int IdTipoTurno { get; set; }

    public bool? Activo { get; set; }

    public virtual Doctor IdCedulaNavigation { get; set; } = null!;

    public virtual TipoTurno IdTipoTurnoNavigation { get; set; } = null!;
}
