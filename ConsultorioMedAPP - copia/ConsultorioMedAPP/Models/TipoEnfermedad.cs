using System;
using System.Collections.Generic;

namespace ConsultorioMedAPP.Models;

public partial class TipoEnfermedad
{
    public int IdTipoEnfermedad { get; set; }

    public string? Descripcion { get; set; }

    public bool? Activo { get; set; }

    public virtual ICollection<AntecedentesMedico> AntecedentesMedicos { get; set; } = new List<AntecedentesMedico>();
}
