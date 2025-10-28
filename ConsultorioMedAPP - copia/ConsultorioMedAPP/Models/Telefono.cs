using System;
using System.Collections.Generic;

namespace ConsultorioMedAPP.Models;

public partial class Telefono
{
    public int IdTelefono { get; set; }

    public string Numero { get; set; } = null!;

    public int IdTipoTelefono { get; set; }

    public int IdCedula { get; set; }

    public bool? Activo { get; set; }

    public virtual Persona IdCedulaNavigation { get; set; } = null!;

    public virtual TipoTelefono IdTipoTelefonoNavigation { get; set; } = null!;
}
