using System;
using System.Collections.Generic;

namespace ConsultorioMedAPP.Models;

public partial class Citum
{
    public int IdCita { get; set; }

    public int PacienteIdCedula { get; set; }

    public int DoctorIdCedula { get; set; }

    public DateOnly Fecha { get; set; }

    public TimeOnly Hora { get; set; }

    public int EstadoCitaIdEstadoCita { get; set; }

    public decimal Precio { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public virtual ICollection<DetalleFactura> DetalleFacturas { get; set; } = new List<DetalleFactura>();

    public virtual ICollection<Diagnostico> Diagnosticos { get; set; } = new List<Diagnostico>();

    public virtual Doctor DoctorIdCedulaNavigation { get; set; } = null!;

    public virtual EstadoCitum EstadoCitaIdEstadoCitaNavigation { get; set; } = null!;

    public virtual Paciente PacienteIdCedulaNavigation { get; set; } = null!;
}
