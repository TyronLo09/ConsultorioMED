using System;
using System.Collections.Generic;

namespace ConsultorioMedAPP.Models;

public partial class Correo
{
    public int IdCorreo { get; set; }

    public string DirecCorreo { get; set; } = null!;

    public int IdTipoCorreo { get; set; }

    public int IdCedula { get; set; }

    public bool? Activo { get; set; }

    public virtual Persona IdCedulaNavigation { get; set; } = null!;

    public virtual TipoCorreo IdTipoCorreoNavigation { get; set; } = null!;
}
