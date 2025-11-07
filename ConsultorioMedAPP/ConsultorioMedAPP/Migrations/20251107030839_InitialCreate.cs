using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConsultorioMedAPP.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Especialidad",
                columns: table => new
                {
                    IdEspecialidad = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: true, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Especial__693FA0AF91024FBF", x => x.IdEspecialidad);
                });

            migrationBuilder.CreateTable(
                name: "EstadoCita",
                columns: table => new
                {
                    IdEstadoCita = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "varchar(45)", unicode: false, maxLength: 45, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: true, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__EstadoCi__EF486D223204E7DE", x => x.IdEstadoCita);
                });

            migrationBuilder.CreateTable(
                name: "Factura",
                columns: table => new
                {
                    IdFactura = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false, defaultValueSql: "(CONVERT([date],getdate()))"),
                    Descuento = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(13,2)", nullable: false),
                    Impuesto = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    NumeroFactura = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    Activo = table.Column<bool>(type: "bit", nullable: true, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Factura__50E7BAF1BD327356", x => x.IdFactura);
                });

            migrationBuilder.CreateTable(
                name: "Genero",
                columns: table => new
                {
                    IdGenero = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: true, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Genero__0F834988AFA00C99", x => x.IdGenero);
                });

            migrationBuilder.CreateTable(
                name: "Proveedor",
                columns: table => new
                {
                    IdProveedor = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: true, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Proveedo__E8B631AF764EAE80", x => x.IdProveedor);
                });

            migrationBuilder.CreateTable(
                name: "RolUsuario",
                columns: table => new
                {
                    IdRolUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: true, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__RolUsuar__3FC7F91FBA54CD2B", x => x.IdRolUsuario);
                });

            migrationBuilder.CreateTable(
                name: "Tipo_Correo",
                columns: table => new
                {
                    IdTipo_Correo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: true, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Tipo_Cor__7F323C75CE951642", x => x.IdTipo_Correo);
                });

            migrationBuilder.CreateTable(
                name: "Tipo_Enfermedad",
                columns: table => new
                {
                    IdTipo_Enfermedad = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: true, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Tipo_Enf__4C402D9E6618A37C", x => x.IdTipo_Enfermedad);
                });

            migrationBuilder.CreateTable(
                name: "Tipo_Seguro",
                columns: table => new
                {
                    IdTipo_Seguro = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Porcentaje = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: true, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Tipo_Seg__6834CDAC05C6B3B1", x => x.IdTipo_Seguro);
                });

            migrationBuilder.CreateTable(
                name: "Tipo_Telefono",
                columns: table => new
                {
                    IdTipo_Telefono = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: true, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Tipo_Tel__B2049411D6EAF689", x => x.IdTipo_Telefono);
                });

            migrationBuilder.CreateTable(
                name: "Tipo_Turno",
                columns: table => new
                {
                    IdTipo_Turno = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Hora_Entrada = table.Column<TimeOnly>(type: "time", nullable: false),
                    Hora_Salida = table.Column<TimeOnly>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Tipo_Tur__40BD99EB12BACB20", x => x.IdTipo_Turno);
                });

            migrationBuilder.CreateTable(
                name: "Persona",
                columns: table => new
                {
                    IdCedula = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Apellido1 = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Apellido2 = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Fecha_Nacimiento = table.Column<DateOnly>(type: "date", nullable: false),
                    Fecha_Creacion = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    IdGenero = table.Column<int>(type: "int", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: true, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Persona__748527307F727542", x => x.IdCedula);
                    table.ForeignKey(
                        name: "FK_Persona_Genero",
                        column: x => x.IdGenero,
                        principalTable: "Genero",
                        principalColumn: "IdGenero");
                });

            migrationBuilder.CreateTable(
                name: "Producto",
                columns: table => new
                {
                    IdProducto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    StockMinimo = table.Column<int>(type: "int", nullable: true, defaultValue: 5),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    IdProveedor = table.Column<int>(type: "int", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    Estado = table.Column<bool>(type: "bit", nullable: true, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Producto__09889210AFBCEE65", x => x.IdProducto);
                    table.ForeignKey(
                        name: "FK_Producto_Proveedor",
                        column: x => x.IdProveedor,
                        principalTable: "Proveedor",
                        principalColumn: "IdProveedor");
                });

            migrationBuilder.CreateTable(
                name: "Correo",
                columns: table => new
                {
                    IdCorreo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Direc_Correo = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    IdTipo_Correo = table.Column<int>(type: "int", nullable: false),
                    IdCedula = table.Column<int>(type: "int", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: true, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Correo__872F8EAE8E89A8E2", x => x.IdCorreo);
                    table.ForeignKey(
                        name: "FK_Correo_Persona",
                        column: x => x.IdCedula,
                        principalTable: "Persona",
                        principalColumn: "IdCedula");
                    table.ForeignKey(
                        name: "FK_Correo_Tipo",
                        column: x => x.IdTipo_Correo,
                        principalTable: "Tipo_Correo",
                        principalColumn: "IdTipo_Correo");
                });

            migrationBuilder.CreateTable(
                name: "Doctor",
                columns: table => new
                {
                    IdCedula = table.Column<int>(type: "int", nullable: false),
                    IdEspecialidad = table.Column<int>(type: "int", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: true, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Doctor__7485273046802B18", x => x.IdCedula);
                    table.ForeignKey(
                        name: "FK_Doctor_Especialidad",
                        column: x => x.IdEspecialidad,
                        principalTable: "Especialidad",
                        principalColumn: "IdEspecialidad");
                    table.ForeignKey(
                        name: "FK_Doctor_Persona",
                        column: x => x.IdCedula,
                        principalTable: "Persona",
                        principalColumn: "IdCedula");
                });

            migrationBuilder.CreateTable(
                name: "Seguro",
                columns: table => new
                {
                    IdSeguro = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdTipo_Seguro = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    IdCedula = table.Column<int>(type: "int", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: true, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Seguro__730AB2BA36336805", x => x.IdSeguro);
                    table.ForeignKey(
                        name: "FK_Seguro_Persona",
                        column: x => x.IdCedula,
                        principalTable: "Persona",
                        principalColumn: "IdCedula");
                    table.ForeignKey(
                        name: "FK_Seguro_Tipo",
                        column: x => x.IdTipo_Seguro,
                        principalTable: "Tipo_Seguro",
                        principalColumn: "IdTipo_Seguro");
                });

            migrationBuilder.CreateTable(
                name: "Telefono",
                columns: table => new
                {
                    IdTelefono = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Numero = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: false),
                    IdTipo_Telefono = table.Column<int>(type: "int", nullable: false),
                    IdCedula = table.Column<int>(type: "int", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: true, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Telefono__9B8AC753509AFCE2", x => x.IdTelefono);
                    table.ForeignKey(
                        name: "FK_Telefono_Persona",
                        column: x => x.IdCedula,
                        principalTable: "Persona",
                        principalColumn: "IdCedula");
                    table.ForeignKey(
                        name: "FK_Telefono_Tipo",
                        column: x => x.IdTipo_Telefono,
                        principalTable: "Tipo_Telefono",
                        principalColumn: "IdTipo_Telefono");
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Personas_IdCedula = table.Column<int>(type: "int", nullable: false),
                    Contraseña = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    RolUsuario_IdRolUsuario = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    UltimoAcceso = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Usuario__5B65BF97590CF5DB", x => x.IdUsuario);
                    table.ForeignKey(
                        name: "FK_Usuario_Persona",
                        column: x => x.Personas_IdCedula,
                        principalTable: "Persona",
                        principalColumn: "IdCedula");
                    table.ForeignKey(
                        name: "FK_Usuario_Rol",
                        column: x => x.RolUsuario_IdRolUsuario,
                        principalTable: "RolUsuario",
                        principalColumn: "IdRolUsuario");
                });

            migrationBuilder.CreateTable(
                name: "Inventario",
                columns: table => new
                {
                    IdInventario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Producto_IdProducto = table.Column<int>(type: "int", nullable: false),
                    DetalleInventario = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    Estado = table.Column<bool>(type: "bit", nullable: true, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Inventar__1927B20C8ED0BF0A", x => x.IdInventario);
                    table.ForeignKey(
                        name: "FK_Inventario_Producto",
                        column: x => x.Producto_IdProducto,
                        principalTable: "Producto",
                        principalColumn: "IdProducto");
                });

            migrationBuilder.CreateTable(
                name: "Reorden",
                columns: table => new
                {
                    IdReorden = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Producto_IdProducto = table.Column<int>(type: "int", nullable: false),
                    FechaAlerta = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    Estado = table.Column<bool>(type: "bit", nullable: true, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Reorden__5C9CA002B2878CF4", x => x.IdReorden);
                    table.ForeignKey(
                        name: "FK_Reorden_Producto",
                        column: x => x.Producto_IdProducto,
                        principalTable: "Producto",
                        principalColumn: "IdProducto");
                });

            migrationBuilder.CreateTable(
                name: "Turno",
                columns: table => new
                {
                    IdTurno = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCedula = table.Column<int>(type: "int", nullable: false),
                    IdTipo_Turno = table.Column<int>(type: "int", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: true, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Turno__C1ECF79A59E07C58", x => x.IdTurno);
                    table.ForeignKey(
                        name: "FK_Turno_Doctor",
                        column: x => x.IdCedula,
                        principalTable: "Doctor",
                        principalColumn: "IdCedula");
                    table.ForeignKey(
                        name: "FK_Turno_Tipo",
                        column: x => x.IdTipo_Turno,
                        principalTable: "Tipo_Turno",
                        principalColumn: "IdTipo_Turno");
                });

            migrationBuilder.CreateTable(
                name: "Pacientes",
                columns: table => new
                {
                    IdCedula = table.Column<int>(type: "int", nullable: false),
                    SeguroPaciente_idSeguro = table.Column<int>(type: "int", nullable: true),
                    FechaRegistro = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    Estado = table.Column<bool>(type: "bit", nullable: true, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Paciente__7485273009008672", x => x.IdCedula);
                    table.ForeignKey(
                        name: "FK_Paciente_Persona",
                        column: x => x.IdCedula,
                        principalTable: "Persona",
                        principalColumn: "IdCedula");
                    table.ForeignKey(
                        name: "FK_Paciente_Seguro",
                        column: x => x.SeguroPaciente_idSeguro,
                        principalTable: "Seguro",
                        principalColumn: "IdSeguro");
                });

            migrationBuilder.CreateTable(
                name: "Auditoria",
                columns: table => new
                {
                    IdAuditoria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Usuario_IdUsuario = table.Column<int>(type: "int", nullable: true),
                    Accion = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    TablaAfectada = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    RegistroID = table.Column<int>(type: "int", nullable: true),
                    ValorAnterior = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    ValorNuevo = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    FechaAccion = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    DireccionIP = table.Column<string>(type: "varchar(45)", unicode: false, maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Auditori__7FD13FA061CC21DB", x => x.IdAuditoria);
                    table.ForeignKey(
                        name: "FK_Auditoria_Usuario",
                        column: x => x.Usuario_IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario");
                });

            migrationBuilder.CreateTable(
                name: "Antecedentes_Medicos",
                columns: table => new
                {
                    IdAntecedentes_Medicos = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdTipo_Enfermedad = table.Column<int>(type: "int", nullable: false),
                    IdCedula = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Cronico = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    Activo = table.Column<bool>(type: "bit", nullable: true, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Antecede__D3CC663466CDEFF2", x => x.IdAntecedentes_Medicos);
                    table.ForeignKey(
                        name: "FK_Antecedente_Paciente",
                        column: x => x.IdCedula,
                        principalTable: "Pacientes",
                        principalColumn: "IdCedula");
                    table.ForeignKey(
                        name: "FK_Antecedente_Tipo",
                        column: x => x.IdTipo_Enfermedad,
                        principalTable: "Tipo_Enfermedad",
                        principalColumn: "IdTipo_Enfermedad");
                });

            migrationBuilder.CreateTable(
                name: "Cita",
                columns: table => new
                {
                    IdCita = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Paciente_idCedula = table.Column<int>(type: "int", nullable: false),
                    Doctor_idCedula = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    Hora = table.Column<TimeOnly>(type: "time", nullable: false),
                    EstadoCita_idEstadoCita = table.Column<int>(type: "int", nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    FechaActualizacion = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Cita__394B020205DDB0BF", x => x.IdCita);
                    table.ForeignKey(
                        name: "FK_Cita_Doctor",
                        column: x => x.Doctor_idCedula,
                        principalTable: "Doctor",
                        principalColumn: "IdCedula");
                    table.ForeignKey(
                        name: "FK_Cita_Estado",
                        column: x => x.EstadoCita_idEstadoCita,
                        principalTable: "EstadoCita",
                        principalColumn: "IdEstadoCita");
                    table.ForeignKey(
                        name: "FK_Cita_Paciente",
                        column: x => x.Paciente_idCedula,
                        principalTable: "Pacientes",
                        principalColumn: "IdCedula");
                });

            migrationBuilder.CreateTable(
                name: "Detalle_Factura",
                columns: table => new
                {
                    IdDetalleFactura = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Factura_IdFactura = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    IdCita_Cita = table.Column<int>(type: "int", nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(13,2)", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: true, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Detalle___DB5F463128D4DCBA", x => x.IdDetalleFactura);
                    table.ForeignKey(
                        name: "FK_DetalleFactura_Cita",
                        column: x => x.IdCita_Cita,
                        principalTable: "Cita",
                        principalColumn: "IdCita");
                    table.ForeignKey(
                        name: "FK_DetalleFactura_Factura",
                        column: x => x.Factura_IdFactura,
                        principalTable: "Factura",
                        principalColumn: "IdFactura");
                });

            migrationBuilder.CreateTable(
                name: "Diagnostico",
                columns: table => new
                {
                    IdDiagnostico = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cita_IdCita = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    CodigoDiagnostico = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    FechaDiagnostico = table.Column<DateOnly>(type: "date", nullable: false),
                    Recomendaciones = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Estado = table.Column<bool>(type: "bit", nullable: true, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Diagnost__BD16DB697B9DBF64", x => x.IdDiagnostico);
                    table.ForeignKey(
                        name: "FK_Diagnostico_Cita",
                        column: x => x.Cita_IdCita,
                        principalTable: "Cita",
                        principalColumn: "IdCita");
                });

            migrationBuilder.CreateTable(
                name: "Medicinas",
                columns: table => new
                {
                    IdMedicinas = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdProducto = table.Column<int>(type: "int", nullable: false),
                    IdDiagnostico = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: true, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Medicina__2230931489006481", x => x.IdMedicinas);
                    table.ForeignKey(
                        name: "FK_Medicina_Diagnostico",
                        column: x => x.IdDiagnostico,
                        principalTable: "Diagnostico",
                        principalColumn: "IdDiagnostico");
                    table.ForeignKey(
                        name: "FK_Medicina_Producto",
                        column: x => x.IdProducto,
                        principalTable: "Producto",
                        principalColumn: "IdProducto");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Antecedentes_Medicos_IdCedula",
                table: "Antecedentes_Medicos",
                column: "IdCedula");

            migrationBuilder.CreateIndex(
                name: "IX_Antecedentes_Medicos_IdTipo_Enfermedad",
                table: "Antecedentes_Medicos",
                column: "IdTipo_Enfermedad");

            migrationBuilder.CreateIndex(
                name: "IX_Auditoria_Usuario_IdUsuario",
                table: "Auditoria",
                column: "Usuario_IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Cita_Doctor_idCedula",
                table: "Cita",
                column: "Doctor_idCedula");

            migrationBuilder.CreateIndex(
                name: "IX_Cita_EstadoCita_idEstadoCita",
                table: "Cita",
                column: "EstadoCita_idEstadoCita");

            migrationBuilder.CreateIndex(
                name: "IX_Cita_Paciente_idCedula",
                table: "Cita",
                column: "Paciente_idCedula");

            migrationBuilder.CreateIndex(
                name: "IX_Correo_IdCedula",
                table: "Correo",
                column: "IdCedula");

            migrationBuilder.CreateIndex(
                name: "IX_Correo_IdTipo_Correo",
                table: "Correo",
                column: "IdTipo_Correo");

            migrationBuilder.CreateIndex(
                name: "IX_Detalle_Factura_Factura_IdFactura",
                table: "Detalle_Factura",
                column: "Factura_IdFactura");

            migrationBuilder.CreateIndex(
                name: "IX_Detalle_Factura_IdCita_Cita",
                table: "Detalle_Factura",
                column: "IdCita_Cita");

            migrationBuilder.CreateIndex(
                name: "IX_Diagnostico_Cita_IdCita",
                table: "Diagnostico",
                column: "Cita_IdCita");

            migrationBuilder.CreateIndex(
                name: "IX_Doctor_IdEspecialidad",
                table: "Doctor",
                column: "IdEspecialidad");

            migrationBuilder.CreateIndex(
                name: "UQ__Factura__CF12F9A6451A4456",
                table: "Factura",
                column: "NumeroFactura",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Inventario_Producto_IdProducto",
                table: "Inventario",
                column: "Producto_IdProducto");

            migrationBuilder.CreateIndex(
                name: "IX_Medicinas_IdDiagnostico",
                table: "Medicinas",
                column: "IdDiagnostico");

            migrationBuilder.CreateIndex(
                name: "IX_Medicinas_IdProducto",
                table: "Medicinas",
                column: "IdProducto");

            migrationBuilder.CreateIndex(
                name: "IX_Pacientes_SeguroPaciente_idSeguro",
                table: "Pacientes",
                column: "SeguroPaciente_idSeguro");

            migrationBuilder.CreateIndex(
                name: "IX_Persona_IdGenero",
                table: "Persona",
                column: "IdGenero");

            migrationBuilder.CreateIndex(
                name: "IX_Producto_IdProveedor",
                table: "Producto",
                column: "IdProveedor");

            migrationBuilder.CreateIndex(
                name: "IX_Reorden_Producto_IdProducto",
                table: "Reorden",
                column: "Producto_IdProducto");

            migrationBuilder.CreateIndex(
                name: "IX_Seguro_IdCedula",
                table: "Seguro",
                column: "IdCedula");

            migrationBuilder.CreateIndex(
                name: "IX_Seguro_IdTipo_Seguro",
                table: "Seguro",
                column: "IdTipo_Seguro");

            migrationBuilder.CreateIndex(
                name: "IX_Telefono_IdCedula",
                table: "Telefono",
                column: "IdCedula");

            migrationBuilder.CreateIndex(
                name: "IX_Telefono_IdTipo_Telefono",
                table: "Telefono",
                column: "IdTipo_Telefono");

            migrationBuilder.CreateIndex(
                name: "IX_Turno_IdCedula",
                table: "Turno",
                column: "IdCedula");

            migrationBuilder.CreateIndex(
                name: "IX_Turno_IdTipo_Turno",
                table: "Turno",
                column: "IdTipo_Turno");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_RolUsuario_IdRolUsuario",
                table: "Usuario",
                column: "RolUsuario_IdRolUsuario");

            migrationBuilder.CreateIndex(
                name: "UQ__Usuario__89A413B99E6383F6",
                table: "Usuario",
                column: "Personas_IdCedula",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Antecedentes_Medicos");

            migrationBuilder.DropTable(
                name: "Auditoria");

            migrationBuilder.DropTable(
                name: "Correo");

            migrationBuilder.DropTable(
                name: "Detalle_Factura");

            migrationBuilder.DropTable(
                name: "Inventario");

            migrationBuilder.DropTable(
                name: "Medicinas");

            migrationBuilder.DropTable(
                name: "Reorden");

            migrationBuilder.DropTable(
                name: "Telefono");

            migrationBuilder.DropTable(
                name: "Turno");

            migrationBuilder.DropTable(
                name: "Tipo_Enfermedad");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "Tipo_Correo");

            migrationBuilder.DropTable(
                name: "Factura");

            migrationBuilder.DropTable(
                name: "Diagnostico");

            migrationBuilder.DropTable(
                name: "Producto");

            migrationBuilder.DropTable(
                name: "Tipo_Telefono");

            migrationBuilder.DropTable(
                name: "Tipo_Turno");

            migrationBuilder.DropTable(
                name: "RolUsuario");

            migrationBuilder.DropTable(
                name: "Cita");

            migrationBuilder.DropTable(
                name: "Proveedor");

            migrationBuilder.DropTable(
                name: "Doctor");

            migrationBuilder.DropTable(
                name: "EstadoCita");

            migrationBuilder.DropTable(
                name: "Pacientes");

            migrationBuilder.DropTable(
                name: "Especialidad");

            migrationBuilder.DropTable(
                name: "Seguro");

            migrationBuilder.DropTable(
                name: "Persona");

            migrationBuilder.DropTable(
                name: "Tipo_Seguro");

            migrationBuilder.DropTable(
                name: "Genero");
        }
    }
}
