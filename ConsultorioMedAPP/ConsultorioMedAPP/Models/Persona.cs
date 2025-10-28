using System;
using System.Collections.Generic;

namespace ConsultorioMedAPP.Models;

public partial class Persona
{
    public int IdCedula { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido1 { get; set; } = null!;

    public string Apellido2 { get; set; } = null!;

    public DateOnly FechaNacimiento { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public int IdGenero { get; set; }

    public bool? Activo { get; set; }

    public virtual ICollection<Correo> Correos { get; set; } = new List<Correo>();

    public virtual Doctor? Doctor { get; set; }

    public virtual Genero IdGeneroNavigation { get; set; } = null!;

    public virtual Paciente? Paciente { get; set; }

    public virtual ICollection<Seguro> Seguros { get; set; } = new List<Seguro>();

    public virtual ICollection<Telefono> Telefonos { get; set; } = new List<Telefono>();

    public virtual Usuario? Usuario { get; set; }
}
