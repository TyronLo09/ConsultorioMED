using System;
using System.Collections.Generic;

namespace ConsultorioMedAPP.Models;

public partial class Reorden
{
    public int IdReorden { get; set; }

    public int ProductoIdProducto { get; set; }

    public DateTime? FechaAlerta { get; set; }

    public bool? Estado { get; set; }

    public virtual Producto ProductoIdProductoNavigation { get; set; } = null!;
}
