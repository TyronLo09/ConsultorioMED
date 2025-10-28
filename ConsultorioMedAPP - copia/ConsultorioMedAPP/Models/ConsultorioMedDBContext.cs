using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ConsultorioMedAPP.Models;

public partial class ConsultorioMedDBContext : DbContext
{
    public ConsultorioMedDBContext()
    {
    }

    public ConsultorioMedDBContext(DbContextOptions<ConsultorioMedDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AntecedentesMedico> AntecedentesMedicos { get; set; }

    public virtual DbSet<Auditorium> Auditoria { get; set; }

    public virtual DbSet<Citum> Cita { get; set; }

    public virtual DbSet<Correo> Correos { get; set; }

    public virtual DbSet<DetalleFactura> DetalleFacturas { get; set; }

    public virtual DbSet<Diagnostico> Diagnosticos { get; set; }

    public virtual DbSet<Doctor> Doctors { get; set; }

    public virtual DbSet<Especialidad> Especialidads { get; set; }

    public virtual DbSet<EstadoCitum> EstadoCita { get; set; }

    public virtual DbSet<Factura> Facturas { get; set; }

    public virtual DbSet<Genero> Generos { get; set; }

    public virtual DbSet<Inventario> Inventarios { get; set; }

    public virtual DbSet<Medicina> Medicinas { get; set; }

    public virtual DbSet<Paciente> Pacientes { get; set; }

    public virtual DbSet<Persona> Personas { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Proveedor> Proveedors { get; set; }

    public virtual DbSet<Reorden> Reordens { get; set; }

    public virtual DbSet<RolUsuario> RolUsuarios { get; set; }

    public virtual DbSet<Seguro> Seguros { get; set; }

    public virtual DbSet<Telefono> Telefonos { get; set; }

    public virtual DbSet<TipoCorreo> TipoCorreos { get; set; }

    public virtual DbSet<TipoEnfermedad> TipoEnfermedads { get; set; }

    public virtual DbSet<TipoSeguro> TipoSeguros { get; set; }

    public virtual DbSet<TipoTelefono> TipoTelefonos { get; set; }

    public virtual DbSet<TipoTurno> TipoTurnos { get; set; }

    public virtual DbSet<Turno> Turnos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=TYRON;Database=ConsultorioMedDB;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AntecedentesMedico>(entity =>
        {
            entity.HasKey(e => e.IdAntecedentesMedicos).HasName("PK__Antecede__D3CC663466CDEFF2");

            entity.ToTable("Antecedentes_Medicos");

            entity.Property(e => e.IdAntecedentesMedicos).HasColumnName("IdAntecedentes_Medicos");
            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Cronico).HasDefaultValue(true);
            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.IdTipoEnfermedad).HasColumnName("IdTipo_Enfermedad");

            entity.HasOne(d => d.IdCedulaNavigation).WithMany(p => p.AntecedentesMedicos)
                .HasForeignKey(d => d.IdCedula)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Antecedente_Paciente");

            entity.HasOne(d => d.IdTipoEnfermedadNavigation).WithMany(p => p.AntecedentesMedicos)
                .HasForeignKey(d => d.IdTipoEnfermedad)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Antecedente_Tipo");
        });

        modelBuilder.Entity<Auditorium>(entity =>
        {
            entity.HasKey(e => e.IdAuditoria).HasName("PK__Auditori__7FD13FA061CC21DB");

            entity.Property(e => e.Accion)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.DireccionIp)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("DireccionIP");
            entity.Property(e => e.FechaAccion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.RegistroId).HasColumnName("RegistroID");
            entity.Property(e => e.TablaAfectada)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UsuarioIdUsuario).HasColumnName("Usuario_IdUsuario");
            entity.Property(e => e.ValorAnterior).IsUnicode(false);
            entity.Property(e => e.ValorNuevo).IsUnicode(false);

            entity.HasOne(d => d.UsuarioIdUsuarioNavigation).WithMany(p => p.Auditoria)
                .HasForeignKey(d => d.UsuarioIdUsuario)
                .HasConstraintName("FK_Auditoria_Usuario");
        });

        modelBuilder.Entity<Citum>(entity =>
        {
            entity.HasKey(e => e.IdCita).HasName("PK__Cita__394B020205DDB0BF");

            entity.Property(e => e.DoctorIdCedula).HasColumnName("Doctor_idCedula");
            entity.Property(e => e.EstadoCitaIdEstadoCita).HasColumnName("EstadoCita_idEstadoCita");
            entity.Property(e => e.FechaActualizacion).HasColumnType("datetime");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PacienteIdCedula).HasColumnName("Paciente_idCedula");
            entity.Property(e => e.Precio).HasColumnType("decimal(6, 2)");

            entity.HasOne(d => d.DoctorIdCedulaNavigation).WithMany(p => p.Cita)
                .HasForeignKey(d => d.DoctorIdCedula)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cita_Doctor");

            entity.HasOne(d => d.EstadoCitaIdEstadoCitaNavigation).WithMany(p => p.Cita)
                .HasForeignKey(d => d.EstadoCitaIdEstadoCita)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cita_Estado");

            entity.HasOne(d => d.PacienteIdCedulaNavigation).WithMany(p => p.Cita)
                .HasForeignKey(d => d.PacienteIdCedula)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cita_Paciente");
        });

        modelBuilder.Entity<Correo>(entity =>
        {
            entity.HasKey(e => e.IdCorreo).HasName("PK__Correo__872F8EAE8E89A8E2");

            entity.ToTable("Correo");

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.DirecCorreo)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Direc_Correo");
            entity.Property(e => e.IdTipoCorreo).HasColumnName("IdTipo_Correo");

            entity.HasOne(d => d.IdCedulaNavigation).WithMany(p => p.Correos)
                .HasForeignKey(d => d.IdCedula)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Correo_Persona");

            entity.HasOne(d => d.IdTipoCorreoNavigation).WithMany(p => p.Correos)
                .HasForeignKey(d => d.IdTipoCorreo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Correo_Tipo");
        });

        modelBuilder.Entity<DetalleFactura>(entity =>
        {
            entity.HasKey(e => e.IdDetalleFactura).HasName("PK__Detalle___DB5F463128D4DCBA");

            entity.ToTable("Detalle_Factura");

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Descripcion)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.FacturaIdFactura).HasColumnName("Factura_IdFactura");
            entity.Property(e => e.IdCitaCita).HasColumnName("IdCita_Cita");
            entity.Property(e => e.Subtotal).HasColumnType("decimal(13, 2)");

            entity.HasOne(d => d.FacturaIdFacturaNavigation).WithMany(p => p.DetalleFacturas)
                .HasForeignKey(d => d.FacturaIdFactura)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetalleFactura_Factura");

            entity.HasOne(d => d.IdCitaCitaNavigation).WithMany(p => p.DetalleFacturas)
                .HasForeignKey(d => d.IdCitaCita)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetalleFactura_Cita");
        });

        modelBuilder.Entity<Diagnostico>(entity =>
        {
            entity.HasKey(e => e.IdDiagnostico).HasName("PK__Diagnost__BD16DB697B9DBF64");

            entity.ToTable("Diagnostico");

            entity.Property(e => e.CitaIdCita).HasColumnName("Cita_IdCita");
            entity.Property(e => e.CodigoDiagnostico)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Descripcion).IsUnicode(false);
            entity.Property(e => e.Estado).HasDefaultValue(true);
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Recomendaciones).IsUnicode(false);

            entity.HasOne(d => d.CitaIdCitaNavigation).WithMany(p => p.Diagnosticos)
                .HasForeignKey(d => d.CitaIdCita)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Diagnostico_Cita");
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.IdCedula).HasName("PK__Doctor__7485273046802B18");

            entity.ToTable("Doctor");

            entity.Property(e => e.IdCedula).ValueGeneratedNever();
            entity.Property(e => e.Activo).HasDefaultValue(true);

            entity.HasOne(d => d.IdCedulaNavigation).WithOne(p => p.Doctor)
                .HasForeignKey<Doctor>(d => d.IdCedula)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Doctor_Persona");

            entity.HasOne(d => d.IdEspecialidadNavigation).WithMany(p => p.Doctors)
                .HasForeignKey(d => d.IdEspecialidad)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Doctor_Especialidad");
        });

        modelBuilder.Entity<Especialidad>(entity =>
        {
            entity.HasKey(e => e.IdEspecialidad).HasName("PK__Especial__693FA0AF91024FBF");

            entity.ToTable("Especialidad");

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<EstadoCitum>(entity =>
        {
            entity.HasKey(e => e.IdEstadoCita).HasName("PK__EstadoCi__EF486D223204E7DE");

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Descripcion)
                .HasMaxLength(45)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Factura>(entity =>
        {
            entity.HasKey(e => e.IdFactura).HasName("PK__Factura__50E7BAF1BD327356");

            entity.ToTable("Factura");

            entity.HasIndex(e => e.NumeroFactura, "UQ__Factura__CF12F9A6451A4456").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Descuento).HasColumnType("decimal(6, 2)");
            entity.Property(e => e.Fecha).HasDefaultValueSql("(CONVERT([date],getdate()))");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Impuesto).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.NumeroFactura)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Total).HasColumnType("decimal(13, 2)");
        });

        modelBuilder.Entity<Genero>(entity =>
        {
            entity.HasKey(e => e.IdGenero).HasName("PK__Genero__0F834988AFA00C99");

            entity.ToTable("Genero");

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Inventario>(entity =>
        {
            entity.HasKey(e => e.IdInventario).HasName("PK__Inventar__1927B20C8ED0BF0A");

            entity.ToTable("Inventario");

            entity.Property(e => e.DetalleInventario)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Estado).HasDefaultValue(true);
            entity.Property(e => e.FechaActualizacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ProductoIdProducto).HasColumnName("Producto_IdProducto");

            entity.HasOne(d => d.ProductoIdProductoNavigation).WithMany(p => p.Inventarios)
                .HasForeignKey(d => d.ProductoIdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Inventario_Producto");
        });

        modelBuilder.Entity<Medicina>(entity =>
        {
            entity.HasKey(e => e.IdMedicinas).HasName("PK__Medicina__2230931489006481");

            entity.Property(e => e.Activo).HasDefaultValue(true);

            entity.HasOne(d => d.IdDiagnosticoNavigation).WithMany(p => p.Medicinas)
                .HasForeignKey(d => d.IdDiagnostico)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Medicina_Diagnostico");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.Medicinas)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Medicina_Producto");
        });

        modelBuilder.Entity<Paciente>(entity =>
        {
            entity.HasKey(e => e.IdCedula).HasName("PK__Paciente__7485273009008672");

            entity.Property(e => e.IdCedula).ValueGeneratedNever();
            entity.Property(e => e.Estado).HasDefaultValue(true);
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.SeguroPacienteIdSeguro).HasColumnName("SeguroPaciente_idSeguro");

            entity.HasOne(d => d.IdCedulaNavigation).WithOne(p => p.Paciente)
                .HasForeignKey<Paciente>(d => d.IdCedula)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Paciente_Persona");

            entity.HasOne(d => d.SeguroPacienteIdSeguroNavigation).WithMany(p => p.Pacientes)
                .HasForeignKey(d => d.SeguroPacienteIdSeguro)
                .HasConstraintName("FK_Paciente_Seguro");
        });

        modelBuilder.Entity<Persona>(entity =>
        {
            entity.HasKey(e => e.IdCedula).HasName("PK__Persona__748527307F727542");

            entity.ToTable("Persona");

            entity.Property(e => e.IdCedula).ValueGeneratedNever();
            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Apellido1)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Apellido2)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Creacion");
            entity.Property(e => e.FechaNacimiento).HasColumnName("Fecha_Nacimiento");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.IdGeneroNavigation).WithMany(p => p.Personas)
                .HasForeignKey(d => d.IdGenero)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Persona_Genero");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.IdProducto).HasName("PK__Producto__09889210AFBCEE65");

            entity.ToTable("Producto");

            entity.Property(e => e.Descripcion).IsUnicode(false);
            entity.Property(e => e.Estado).HasDefaultValue(true);
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PrecioUnitario).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.StockMinimo).HasDefaultValue(5);

            entity.HasOne(d => d.IdProveedorNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdProveedor)
                .HasConstraintName("FK_Producto_Proveedor");
        });

        modelBuilder.Entity<Proveedor>(entity =>
        {
            entity.HasKey(e => e.IdProveedor).HasName("PK__Proveedo__E8B631AF764EAE80");

            entity.ToTable("Proveedor");

            entity.Property(e => e.Estado).HasDefaultValue(true);
            entity.Property(e => e.Nombre)
                .HasMaxLength(40)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Reorden>(entity =>
        {
            entity.HasKey(e => e.IdReorden).HasName("PK__Reorden__5C9CA002B2878CF4");

            entity.ToTable("Reorden");

            entity.Property(e => e.Estado).HasDefaultValue(true);
            entity.Property(e => e.FechaAlerta)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ProductoIdProducto).HasColumnName("Producto_IdProducto");

            entity.HasOne(d => d.ProductoIdProductoNavigation).WithMany(p => p.Reordens)
                .HasForeignKey(d => d.ProductoIdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reorden_Producto");
        });

        modelBuilder.Entity<RolUsuario>(entity =>
        {
            entity.HasKey(e => e.IdRolUsuario).HasName("PK__RolUsuar__3FC7F91FBA54CD2B");

            entity.ToTable("RolUsuario");

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Descripcion)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Seguro>(entity =>
        {
            entity.HasKey(e => e.IdSeguro).HasName("PK__Seguro__730AB2BA36336805");

            entity.ToTable("Seguro");

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IdTipoSeguro).HasColumnName("IdTipo_Seguro");

            entity.HasOne(d => d.IdCedulaNavigation).WithMany(p => p.Seguros)
                .HasForeignKey(d => d.IdCedula)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Seguro_Persona");

            entity.HasOne(d => d.IdTipoSeguroNavigation).WithMany(p => p.Seguros)
                .HasForeignKey(d => d.IdTipoSeguro)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Seguro_Tipo");
        });

        modelBuilder.Entity<Telefono>(entity =>
        {
            entity.HasKey(e => e.IdTelefono).HasName("PK__Telefono__9B8AC753509AFCE2");

            entity.ToTable("Telefono");

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.IdTipoTelefono).HasColumnName("IdTipo_Telefono");
            entity.Property(e => e.Numero)
                .HasMaxLength(8)
                .IsUnicode(false);

            entity.HasOne(d => d.IdCedulaNavigation).WithMany(p => p.Telefonos)
                .HasForeignKey(d => d.IdCedula)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Telefono_Persona");

            entity.HasOne(d => d.IdTipoTelefonoNavigation).WithMany(p => p.Telefonos)
                .HasForeignKey(d => d.IdTipoTelefono)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Telefono_Tipo");
        });

        modelBuilder.Entity<TipoCorreo>(entity =>
        {
            entity.HasKey(e => e.IdTipoCorreo).HasName("PK__Tipo_Cor__7F323C75CE951642");

            entity.ToTable("Tipo_Correo");

            entity.Property(e => e.IdTipoCorreo).HasColumnName("IdTipo_Correo");
            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TipoEnfermedad>(entity =>
        {
            entity.HasKey(e => e.IdTipoEnfermedad).HasName("PK__Tipo_Enf__4C402D9E6618A37C");

            entity.ToTable("Tipo_Enfermedad");

            entity.Property(e => e.IdTipoEnfermedad).HasColumnName("IdTipo_Enfermedad");
            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TipoSeguro>(entity =>
        {
            entity.HasKey(e => e.IdTipoSeguro).HasName("PK__Tipo_Seg__6834CDAC05C6B3B1");

            entity.ToTable("Tipo_Seguro");

            entity.Property(e => e.IdTipoSeguro).HasColumnName("IdTipo_Seguro");
            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Porcentaje).HasColumnType("decimal(5, 2)");
        });

        modelBuilder.Entity<TipoTelefono>(entity =>
        {
            entity.HasKey(e => e.IdTipoTelefono).HasName("PK__Tipo_Tel__B2049411D6EAF689");

            entity.ToTable("Tipo_Telefono");

            entity.Property(e => e.IdTipoTelefono).HasColumnName("IdTipo_Telefono");
            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TipoTurno>(entity =>
        {
            entity.HasKey(e => e.IdTipoTurno).HasName("PK__Tipo_Tur__40BD99EB12BACB20");

            entity.ToTable("Tipo_Turno");

            entity.Property(e => e.IdTipoTurno).HasColumnName("IdTipo_Turno");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.HoraEntrada).HasColumnName("Hora_Entrada");
            entity.Property(e => e.HoraSalida).HasColumnName("Hora_Salida");
        });

        modelBuilder.Entity<Turno>(entity =>
        {
            entity.HasKey(e => e.IdTurno).HasName("PK__Turno__C1ECF79A59E07C58");

            entity.ToTable("Turno");

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.IdTipoTurno).HasColumnName("IdTipo_Turno");

            entity.HasOne(d => d.IdCedulaNavigation).WithMany(p => p.Turnos)
                .HasForeignKey(d => d.IdCedula)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Turno_Doctor");

            entity.HasOne(d => d.IdTipoTurnoNavigation).WithMany(p => p.Turnos)
                .HasForeignKey(d => d.IdTipoTurno)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Turno_Tipo");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuario__5B65BF97590CF5DB");

            entity.ToTable("Usuario");

            entity.HasIndex(e => e.PersonasIdCedula, "UQ__Usuario__89A413B99E6383F6").IsUnique();

            entity.Property(e => e.Contraseña)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PersonasIdCedula).HasColumnName("Personas_IdCedula");
            entity.Property(e => e.RolUsuarioIdRolUsuario).HasColumnName("RolUsuario_IdRolUsuario");
            entity.Property(e => e.UltimoAcceso).HasColumnType("datetime");

            entity.HasOne(d => d.PersonasIdCedulaNavigation).WithOne(p => p.Usuario)
                .HasForeignKey<Usuario>(d => d.PersonasIdCedula)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuario_Persona");

            entity.HasOne(d => d.RolUsuarioIdRolUsuarioNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.RolUsuarioIdRolUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuario_Rol");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
