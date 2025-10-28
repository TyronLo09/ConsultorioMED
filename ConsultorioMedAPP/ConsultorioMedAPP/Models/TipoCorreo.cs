using System;
using System.Collections.Generic;

namespace ConsultorioMedAPP.Models;

public partial class TipoCorreo
{
    public int IdTipoCorreo { get; set; }

    public string Descripcion { get; set; } = null!;

    public bool? Activo { get; set; }

    public virtual ICollection<Correo> Correos { get; set; } = new List<Correo>();
}
