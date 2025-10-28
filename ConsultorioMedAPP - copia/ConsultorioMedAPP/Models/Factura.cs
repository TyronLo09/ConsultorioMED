using System;
using System.Collections.Generic;

namespace ConsultorioMedAPP.Models;

public partial class Factura
{
    public int IdFactura { get; set; }

    public DateOnly Fecha { get; set; }

    public decimal Descuento { get; set; }

    public decimal Total { get; set; }

    public decimal? Impuesto { get; set; }

    public string NumeroFactura { get; set; } = null!;

    public DateTime? FechaCreacion { get; set; }

    public bool? Activo { get; set; }

    public virtual ICollection<DetalleFactura> DetalleFacturas { get; set; } = new List<DetalleFactura>();
}
