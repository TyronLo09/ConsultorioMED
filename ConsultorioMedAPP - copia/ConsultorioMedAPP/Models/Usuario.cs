using System;
using System.Collections.Generic;

namespace ConsultorioMedAPP.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public int PersonasIdCedula { get; set; }

    public string Contraseña { get; set; } = null!;

    public int RolUsuarioIdRolUsuario { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? UltimoAcceso { get; set; }

    public virtual ICollection<Auditorium> Auditoria { get; set; } = new List<Auditorium>();

    public virtual Persona PersonasIdCedulaNavigation { get; set; } = null!;

    public virtual RolUsuario RolUsuarioIdRolUsuarioNavigation { get; set; } = null!;
}
