using System;
using System.Collections.Generic;

namespace ConsultorioMedAPP.Models;

public partial class Inventario
{
    public int IdInventario { get; set; }

    public int ProductoIdProducto { get; set; }

    public string? DetalleInventario { get; set; }

    public int Stock { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public bool? Estado { get; set; }

    public virtual Producto ProductoIdProductoNavigation { get; set; } = null!;
}
