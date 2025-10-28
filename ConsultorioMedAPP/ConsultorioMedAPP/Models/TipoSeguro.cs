using System;
using System.Collections.Generic;

namespace ConsultorioMedAPP.Models;

public partial class TipoSeguro
{
    public int IdTipoSeguro { get; set; }

    public string? Descripcion { get; set; }

    public decimal? Porcentaje { get; set; }

    public bool? Activo { get; set; }

    public virtual ICollection<Seguro> Seguros { get; set; } = new List<Seguro>();
}
