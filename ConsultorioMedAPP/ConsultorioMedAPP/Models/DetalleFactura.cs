using System;
using System.Collections.Generic;

namespace ConsultorioMedAPP.Models;

public partial class DetalleFactura
{
    public int IdDetalleFactura { get; set; }

    public int FacturaIdFactura { get; set; }

    public string Descripcion { get; set; } = null!;

    public int IdCitaCita { get; set; }

    public decimal Subtotal { get; set; }

    public bool? Activo { get; set; }

    public virtual Factura FacturaIdFacturaNavigation { get; set; } = null!;

    public virtual Citum IdCitaCitaNavigation { get; set; } = null!;
}
