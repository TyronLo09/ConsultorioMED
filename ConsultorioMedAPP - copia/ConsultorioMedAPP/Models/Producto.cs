using System;
using System.Collections.Generic;

namespace ConsultorioMedAPP.Models;

public partial class Producto
{
    public int IdProducto { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public int Stock { get; set; }

    public int? StockMinimo { get; set; }

    public decimal PrecioUnitario { get; set; }

    public int? IdProveedor { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public bool? Estado { get; set; }

    public virtual Proveedor? IdProveedorNavigation { get; set; }

    public virtual ICollection<Inventario> Inventarios { get; set; } = new List<Inventario>();

    public virtual ICollection<Medicina> Medicinas { get; set; } = new List<Medicina>();

    public virtual ICollection<Reorden> Reordens { get; set; } = new List<Reorden>();
}
