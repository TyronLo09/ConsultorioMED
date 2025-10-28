using System;
using System.Collections.Generic;

namespace ConsultorioMedAPP.Models;

public partial class TipoTelefono
{
    public int IdTipoTelefono { get; set; }

    public string Descripcion { get; set; } = null!;

    public bool? Activo { get; set; }

    public virtual ICollection<Telefono> Telefonos { get; set; } = new List<Telefono>();
}
