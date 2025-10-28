using System;
using System.Collections.Generic;

namespace ConsultorioMedAPP.Models;

public partial class Medicina
{
    public int IdMedicinas { get; set; }

    public int IdProducto { get; set; }

    public int IdDiagnostico { get; set; }

    public int Cantidad { get; set; }

    public bool? Activo { get; set; }

    public virtual Diagnostico IdDiagnosticoNavigation { get; set; } = null!;

    public virtual Producto IdProductoNavigation { get; set; } = null!;
}
