using System;
using System.Collections.Generic;

namespace ConsultorioMedAPP.Models;

public partial class Auditorium
{
    public int IdAuditoria { get; set; }

    public int? UsuarioIdUsuario { get; set; }

    public string Accion { get; set; } = null!;

    public string TablaAfectada { get; set; } = null!;

    public int? RegistroId { get; set; }

    public string? ValorAnterior { get; set; }

    public string? ValorNuevo { get; set; }

    public DateTime? FechaAccion { get; set; }

    public string? DireccionIp { get; set; }

    public virtual Usuario? UsuarioIdUsuarioNavigation { get; set; }
}
