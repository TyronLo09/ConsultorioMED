using System;
using System.Collections.Generic;

namespace ConsultorioMedAPP.Models;

public partial class Genero
{
    public int IdGenero { get; set; }

    public string? Descripcion { get; set; }

    public bool? Activo { get; set; }

    public virtual ICollection<Persona> Personas { get; set; } = new List<Persona>();
}
