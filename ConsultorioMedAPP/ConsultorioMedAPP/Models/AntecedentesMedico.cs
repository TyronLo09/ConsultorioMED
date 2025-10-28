using System;
using System.Collections.Generic;

namespace ConsultorioMedAPP.Models;

public partial class AntecedentesMedico
{
    public int IdAntecedentesMedicos { get; set; }

    public int IdTipoEnfermedad { get; set; }

    public int IdCedula { get; set; }

    public string Descripcion { get; set; } = null!;

    public bool? Cronico { get; set; }

    public bool? Activo { get; set; }

    public virtual Paciente IdCedulaNavigation { get; set; } = null!;

    public virtual TipoEnfermedad IdTipoEnfermedadNavigation { get; set; } = null!;
}
