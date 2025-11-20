using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ConsultorioMedAPP.Models;
using System.Collections.Generic;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using iText.Kernel.Font;
using iText.IO.Font.Constants;

namespace ConsultorioMedAPP.Controllers
{
    public class PacientesController : Controller
    {
        private readonly ConsultorioMedDBContext _context;

        public PacientesController(ConsultorioMedDBContext context)
        {
            _context = context;
           

        }

        // --- INDEX ---
        public async Task<IActionResult> Index(string busqueda)
        {
            var query = _context.Pacientes
                .Include(p => p.IdCedulaNavigation)
                .Include(p => p.SeguroPacienteIdSeguroNavigation)
                    .ThenInclude(s => s.IdTipoSeguroNavigation)
                .Include(p => p.AntecedentesMedicos)
                    .ThenInclude(a => a.IdTipoEnfermedadNavigation)
                .AsQueryable();

            if (!string.IsNullOrEmpty(busqueda))
            {
                ViewData["BusquedaSel"] = busqueda;
                string busquedaNormalizada = busqueda.ToLower();

                query = query.Where(p =>
                    p.IdCedula.ToString().Contains(busquedaNormalizada) ||
                    p.IdCedulaNavigation.Nombre.ToLower().Contains(busquedaNormalizada) ||
                    p.IdCedulaNavigation.Apellido1.ToLower().Contains(busquedaNormalizada) ||
                    p.IdCedulaNavigation.Apellido2.ToLower().Contains(busquedaNormalizada)
                );
            }

            var pacientes = await query.OrderBy(p => p.IdCedula).ToListAsync();

            var vistaPacientes = pacientes.Select(p =>
            {
                var persona = p.IdCedulaNavigation;
                var seguro = p.SeguroPacienteIdSeguroNavigation;
                var antecedente = p.AntecedentesMedicos.FirstOrDefault();

                // Obtener el porcentaje directamente de la tabla TipoSeguro
                string tipoSeguroDescripcion = seguro?.IdTipoSeguroNavigation?.Descripcion ?? "Sin Seguro";
                decimal? porcentaje = seguro?.IdTipoSeguroNavigation?.Porcentaje;

                return new
                {
                    Cedula = persona.IdCedula,
                    Nombre = persona.Nombre,
                    Apellido1 = persona.Apellido1,
                    Apellido2 = persona.Apellido2,
                    Estado = p.Estado,
                    TipoSeguro = tipoSeguroDescripcion,
                    PorcentajeSeguro = porcentaje.HasValue ? $"{porcentaje.Value}%" : "0%",
                    TipoEnfermedadDescripcion = antecedente?.IdTipoEnfermedadNavigation?.Descripcion ?? "Sin registro",
                    EsCronico = (antecedente?.Cronico ?? false) ? "Sí" : "No",
                    AntecedentesDescripcion = antecedente?.Descripcion ?? "Sin antecedentes registrados",
                    IdSeguro = seguro?.IdSeguro
                };
            }).ToList();

            ViewBag.Pacientes = vistaPacientes;
            return View();
        }

        // --- EXPORTAR A PDF (CÓDIGO COMPLETO CORREGIDO) ---
        public async Task<IActionResult> ExportarPdf(string busqueda)
        {
            var pacientes = await ObtenerPacientesParaExportar(busqueda);

            using (var memoryStream = new MemoryStream())
            {
                var writer = new PdfWriter(memoryStream);
                var pdf = new PdfDocument(writer);
                var document = new Document(pdf);

                // 1. CREAMOS LA FUENTE NEGRITA
                PdfFont fuenteNegrita = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

                // --- TÍTULO ---
                document.Add(new Paragraph("REPORTE DE PACIENTES")
                    .SetFont(fuenteNegrita) // Usamos .SetFont
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(16));

                document.Add(new Paragraph($"Generado el: {DateTime.Now:dd/MM/yyyy HH:mm}"));

                if (!string.IsNullOrEmpty(busqueda))
                {
                    document.Add(new Paragraph($"Búsqueda: {busqueda}"));
                }

                document.Add(new Paragraph("\n"));

                // --- TABLA ---
                var table = new Table(8); // 8 columnas
                table.SetWidth(UnitValue.CreatePercentValue(100));

                // --- ENCABEZADOS ---
                string[] headers = { "Cédula", "Nombre", "Apellido1", "Apellido2", "Estado", "Tipo Seguro", "% Seguro", "Enfermedad" };

                foreach (var header in headers)
                {
                    Cell celda = new Cell();

                    // Aplicamos la fuente al párrafo, no a la celda
                    Paragraph p = new Paragraph(header).SetFont(fuenteNegrita);

                    celda.Add(p);
                    celda.SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY);
                    celda.SetTextAlignment(TextAlignment.CENTER);

                    table.AddHeaderCell(celda);
                }

                // --- DATOS (Esto faltaba en tu recorte) ---
                foreach (var paciente in pacientes)
                {
                    table.AddCell(new Cell().Add(new Paragraph(paciente.Cedula.ToString())));
                    table.AddCell(new Cell().Add(new Paragraph(paciente.Nombre)));
                    table.AddCell(new Cell().Add(new Paragraph(paciente.Apellido1)));
                    table.AddCell(new Cell().Add(new Paragraph(paciente.Apellido2)));
                    table.AddCell(new Cell().Add(new Paragraph(paciente.Estado ? "Activo" : "Inactivo")));
                    table.AddCell(new Cell().Add(new Paragraph(paciente.TipoSeguro)));
                    table.AddCell(new Cell().Add(new Paragraph(paciente.PorcentajeSeguro)));
                    table.AddCell(new Cell().Add(new Paragraph(paciente.TipoEnfermedadDescripcion)));
                }

                document.Add(table);

                // Pie de página
                document.Add(new Paragraph($"\nTotal de pacientes: {pacientes.Count}")
                    .SetTextAlignment(TextAlignment.RIGHT));

                document.Close(); // ¡Importante cerrar el documento!

                var bytes = memoryStream.ToArray();
                return File(bytes, "application/pdf", $"Pacientes_{DateTime.Now:yyyyMMddHHmmss}.pdf");
            }
        }

        // --- EXPORTAR A EXCEL ---
        // --- EXPORTAR A EXCEL ---
        public async Task<IActionResult> ExportarExcel(string busqueda)
        {
            // 1. AGREGA ESTA LÍNEA AQUÍ PARA SOLUCIONAR EL ERROR
            

            var pacientes = await ObtenerPacientesParaExportar(busqueda);

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Pacientes");

                // Título
                worksheet.Cells[1, 1].Value = "REPORTE DE PACIENTES";
                worksheet.Cells[1, 1, 1, 9].Merge = true;
                worksheet.Cells[1, 1].Style.Font.Size = 16;
                worksheet.Cells[1, 1].Style.Font.Bold = true;
                worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                // Información del reporte
                worksheet.Cells[2, 1].Value = $"Generado el: {DateTime.Now:dd/MM/yyyy HH:mm}";
                if (!string.IsNullOrEmpty(busqueda))
                {
                    worksheet.Cells[3, 1].Value = $"Búsqueda: {busqueda}";
                }

                // Encabezados
                int row = 5;
                string[] headers = { "Cédula", "Nombre", "Apellido1", "Apellido2", "Estado", "Tipo Seguro", "% Seguro", "Enfermedad", "Crónico", "Antecedentes" };
                
                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cells[row, i + 1].Value = headers[i];
                    worksheet.Cells[row, i + 1].Style.Font.Bold = true;
                    worksheet.Cells[row, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[row, i + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    worksheet.Cells[row, i + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                }

                // Datos
                row++;
                foreach (var paciente in pacientes)
                {
                    worksheet.Cells[row, 1].Value = paciente.Cedula;
                    worksheet.Cells[row, 2].Value = paciente.Nombre;
                    worksheet.Cells[row, 3].Value = paciente.Apellido1;
                    worksheet.Cells[row, 4].Value = paciente.Apellido2;
                    worksheet.Cells[row, 5].Value = paciente.Estado ? "Activo" : "Inactivo";
                    worksheet.Cells[row, 6].Value = paciente.TipoSeguro;
                    worksheet.Cells[row, 7].Value = paciente.PorcentajeSeguro;
                    worksheet.Cells[row, 8].Value = paciente.TipoEnfermedadDescripcion;
                    worksheet.Cells[row, 9].Value = paciente.EsCronico;
                    worksheet.Cells[row, 10].Value = paciente.AntecedentesDescripcion;

                    // Aplicar bordes a todas las celdas de la fila
                    for (int col = 1; col <= 10; col++)
                    {
                        worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    }

                    row++;
                }

                // Autoajustar columnas
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                // Total de pacientes
                worksheet.Cells[row + 1, 1].Value = $"Total de pacientes: {pacientes.Count}";
                worksheet.Cells[row + 1, 1].Style.Font.Bold = true;

                var bytes = package.GetAsByteArray();
                return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
                    $"Pacientes_{DateTime.Now:yyyyMMddHHmmss}.xlsx");
            }
        }

        // --- MÉTODO PRIVADO PARA OBTENER DATOS DE EXPORTACIÓN ---
        private async Task<List<dynamic>> ObtenerPacientesParaExportar(string busqueda)
        {
            var query = _context.Pacientes
                .Include(p => p.IdCedulaNavigation)
                .Include(p => p.SeguroPacienteIdSeguroNavigation)
                    .ThenInclude(s => s.IdTipoSeguroNavigation)
                .Include(p => p.AntecedentesMedicos)
                    .ThenInclude(a => a.IdTipoEnfermedadNavigation)
                .AsQueryable();

            if (!string.IsNullOrEmpty(busqueda))
            {
                string busquedaNormalizada = busqueda.ToLower();
                query = query.Where(p =>
                    p.IdCedula.ToString().Contains(busquedaNormalizada) ||
                    p.IdCedulaNavigation.Nombre.ToLower().Contains(busquedaNormalizada) ||
                    p.IdCedulaNavigation.Apellido1.ToLower().Contains(busquedaNormalizada) ||
                    p.IdCedulaNavigation.Apellido2.ToLower().Contains(busquedaNormalizada)
                );
            }

            var pacientes = await query.OrderBy(p => p.IdCedula).ToListAsync();

            return pacientes.Select(p =>
            {
                var persona = p.IdCedulaNavigation;
                var seguro = p.SeguroPacienteIdSeguroNavigation;
                var antecedente = p.AntecedentesMedicos.FirstOrDefault();

                string tipoSeguroDescripcion = seguro?.IdTipoSeguroNavigation?.Descripcion ?? "Sin Seguro";
                decimal? porcentaje = seguro?.IdTipoSeguroNavigation?.Porcentaje;

                return new
                {
                    Cedula = persona.IdCedula,
                    Nombre = persona.Nombre,
                    Apellido1 = persona.Apellido1,
                    Apellido2 = persona.Apellido2,
                    Estado = p.Estado,
                    TipoSeguro = tipoSeguroDescripcion,
                    PorcentajeSeguro = porcentaje.HasValue ? $"{porcentaje.Value}%" : "0%",
                    TipoEnfermedadDescripcion = antecedente?.IdTipoEnfermedadNavigation?.Descripcion ?? "Sin registro",
                    EsCronico = (antecedente?.Cronico ?? false) ? "Sí" : "No",
                    AntecedentesDescripcion = antecedente?.Descripcion ?? "Sin antecedentes registrados",
                    IdSeguro = seguro?.IdSeguro
                };
            }).ToList<dynamic>();
        }

        // --- DETAILS ---
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var paciente = await _context.Pacientes
                .Include(p => p.IdCedulaNavigation)
                .Include(p => p.SeguroPacienteIdSeguroNavigation)
                    .ThenInclude(s => s.IdTipoSeguroNavigation)
                .Include(p => p.AntecedentesMedicos)
                    .ThenInclude(a => a.IdTipoEnfermedadNavigation)
                .FirstOrDefaultAsync(m => m.IdCedula == id);

            if (paciente == null) return NotFound();

            ViewData["Antecedente"] = paciente.AntecedentesMedicos.FirstOrDefault();
            return View(paciente);
        }

        // --- CREATE GET ---
        public IActionResult Create()
        {
            RecargarSelectLists(null, null, null);
            return View();
        }

        // --- CREATE POST ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int IdCedula, int IdTipoSeguro, int IdTipoEnfermedad, string DescripcionAntecedente, bool Cronico, bool Estado)
        {
            // Validar que la Persona exista
            var persona = await _context.Personas.FindAsync(IdCedula);
            if (persona == null)
            {
                TempData["Error"] = "La Cédula seleccionada no existe en la tabla de Personas.";
                RecargarSelectLists(IdCedula, IdTipoSeguro, IdTipoEnfermedad);
                return View();
            }

            // Validar que no sea ya un Paciente
            var pacienteExiste = await _context.Pacientes.AnyAsync(p => p.IdCedula == IdCedula);
            if (pacienteExiste)
            {
                TempData["Error"] = "La Persona con esta Cédula ya está registrada como Paciente.";
                RecargarSelectLists(IdCedula, IdTipoSeguro, IdTipoEnfermedad);
                return View();
            }

            var strategy = _context.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                using var trans = await _context.Database.BeginTransactionAsync();
                try
                {
                    // Crear el Seguro
                    var seguro = new Seguro
                    {
                        IdTipoSeguro = IdTipoSeguro,
                        IdCedula = IdCedula,
                        FechaCreacion = DateTime.Now,
                        Activo = true
                    };
                    _context.Seguros.Add(seguro);
                    await _context.SaveChangesAsync();

                    // Crear el Paciente
                    var paciente = new Paciente
                    {
                        IdCedula = IdCedula,
                        SeguroPacienteIdSeguro = seguro.IdSeguro,
                        FechaRegistro = DateTime.Now,
                        Estado = Estado
                    };
                    _context.Pacientes.Add(paciente);

                    // Crear el Antecedente
                    var antecedente = new AntecedentesMedico
                    {
                        IdCedula = IdCedula,
                        IdTipoEnfermedad = IdTipoEnfermedad,
                        Descripcion = string.IsNullOrWhiteSpace(DescripcionAntecedente) ? "Sin descripción" : DescripcionAntecedente,
                        Cronico = Cronico,
                        Activo = true
                    };
                    _context.AntecedentesMedicos.Add(antecedente);

                    await _context.SaveChangesAsync();
                    await trans.CommitAsync();
                }
                catch (Exception ex)
                {
                    await trans.RollbackAsync();
                    throw;
                }
            });

            TempData["Success"] = "Paciente creado exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        // --- EDIT GET ---
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var paciente = await _context.Pacientes
                .Include(p => p.IdCedulaNavigation)
                .Include(p => p.SeguroPacienteIdSeguroNavigation)
                    .ThenInclude(s => s.IdTipoSeguroNavigation)
                .Include(p => p.AntecedentesMedicos)
                    .ThenInclude(a => a.IdTipoEnfermedadNavigation)
                .FirstOrDefaultAsync(p => p.IdCedula == id);

            if (paciente == null) return NotFound();

            var antecedente = paciente.AntecedentesMedicos.FirstOrDefault();
            var seguro = paciente.SeguroPacienteIdSeguroNavigation;
            var persona = paciente.IdCedulaNavigation;

            RecargarSelectLists(paciente.IdCedula, seguro?.IdTipoSeguro, antecedente?.IdTipoEnfermedad);

            ViewData["Antecedente"] = antecedente;
            ViewData["Persona"] = persona;
            ViewData["SeguroId"] = seguro?.IdSeguro;
            ViewData["EstadoActual"] = paciente.Estado;

            return View(paciente);
        }

        // --- EDIT POST ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int IdTipoSeguro, int IdTipoEnfermedad, string DescripcionAntecedente, bool Cronico, bool Estado, int IdSeguro)
        {
            var strategy = _context.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                using var trans = await _context.Database.BeginTransactionAsync();
                try
                {
                    // Actualizar Paciente
                    var pacienteExistente = await _context.Pacientes.FindAsync(id);
                    if (pacienteExistente != null)
                    {
                        pacienteExistente.Estado = Estado;
                        _context.Pacientes.Update(pacienteExistente);
                    }

                    // Actualizar Seguro
                    var seguroExistente = await _context.Seguros.FindAsync(IdSeguro);
                    if (seguroExistente != null)
                    {
                        seguroExistente.IdTipoSeguro = IdTipoSeguro;
                        _context.Seguros.Update(seguroExistente);
                    }

                    // Actualizar Antecedente
                    var antecedenteExistente = await _context.AntecedentesMedicos
                        .FirstOrDefaultAsync(a => a.IdCedula == id);

                    if (antecedenteExistente != null)
                    {
                        antecedenteExistente.IdTipoEnfermedad = IdTipoEnfermedad;
                        antecedenteExistente.Descripcion = string.IsNullOrWhiteSpace(DescripcionAntecedente) ? "Sin descripción" : DescripcionAntecedente;
                        antecedenteExistente.Cronico = Cronico;
                        _context.AntecedentesMedicos.Update(antecedenteExistente);
                    }
                    else
                    {
                        // Si no existe, crear uno nuevo
                        var nuevoAntecedente = new AntecedentesMedico
                        {
                            IdCedula = id,
                            IdTipoEnfermedad = IdTipoEnfermedad,
                            Descripcion = string.IsNullOrWhiteSpace(DescripcionAntecedente) ? "Sin descripción" : DescripcionAntecedente,
                            Cronico = Cronico,
                            Activo = true
                        };
                        _context.AntecedentesMedicos.Add(nuevoAntecedente);
                    }

                    await _context.SaveChangesAsync();
                    await trans.CommitAsync();
                }
                catch (Exception ex)
                {
                    await trans.RollbackAsync();
                    throw;
                }
            });

            TempData["Success"] = "Paciente actualizado exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        // --- DELETE GET ---
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var paciente = await _context.Pacientes
                .Include(p => p.IdCedulaNavigation)
                .Include(p => p.SeguroPacienteIdSeguroNavigation)
                    .ThenInclude(s => s.IdTipoSeguroNavigation)
                .FirstOrDefaultAsync(m => m.IdCedula == id);

            if (paciente == null) return NotFound();
            return View(paciente);
        }

        // --- DELETE POST ---
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var strategy = _context.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                using var trans = await _context.Database.BeginTransactionAsync();
                try
                {
                    var paciente = await _context.Pacientes
                        .Include(p => p.AntecedentesMedicos)
                        .FirstOrDefaultAsync(p => p.IdCedula == id);

                    if (paciente != null)
                    {
                        var seguroId = paciente.SeguroPacienteIdSeguro;

                        // Eliminar Antecedentes
                        if (paciente.AntecedentesMedicos.Any())
                        {
                            _context.AntecedentesMedicos.RemoveRange(paciente.AntecedentesMedicos);
                        }

                        // Eliminar Paciente
                        _context.Pacientes.Remove(paciente);

                        // Eliminar Seguro
                        if (seguroId.HasValue)
                        {
                            var seguro = await _context.Seguros.FindAsync(seguroId.Value);
                            if (seguro != null)
                            {
                                _context.Seguros.Remove(seguro);
                            }
                        }

                        await _context.SaveChangesAsync();
                        await trans.CommitAsync();
                    }
                }
                catch (Exception ex)
                {
                    await trans.RollbackAsync();
                    throw;
                }
            });

            TempData["Success"] = "Paciente eliminado exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        // --- MÉTODO AUXILIAR ---
        private void RecargarSelectLists(int? idCedulaSeleccionada, int? idTipoSeguroSeleccionado, int? idTipoEnfermedadSeleccionado)
        {
            var personasNoPacientes = _context.Personas
                .Where(p => !_context.Pacientes.Any(pa => pa.IdCedula == p.IdCedula))
                .OrderBy(p => p.IdCedula)
                .Select(p => new
                {
                    p.IdCedula,
                    NombreCompleto = $"{p.IdCedula} - {p.Nombre} {p.Apellido1}"
                });

            ViewData["IdCedula"] = new SelectList(personasNoPacientes.AsEnumerable(), "IdCedula", "NombreCompleto", idCedulaSeleccionada);

            ViewData["IdTipoSeguro"] = new SelectList(
                _context.TipoSeguros.OrderBy(t => t.Descripcion).ToList(),
                "IdTipoSeguro",
                "Descripcion",
                idTipoSeguroSeleccionado
            );

            ViewData["IdTipoEnfermedad"] = new SelectList(
                _context.TipoEnfermedads.OrderBy(t => t.Descripcion).ToList(),
                "IdTipoEnfermedad",
                "Descripcion",
                idTipoEnfermedadSeleccionado
            );
        }

        // --- AJAX ---
        [HttpGet]
        public async Task<IActionResult> GetPersonaData(int idCedula)
        {
            var persona = await _context.Personas.FirstOrDefaultAsync(p => p.IdCedula == idCedula);

            if (persona == null)
            {
                return NotFound();
            }

            return Json(new
            {
                nombre = persona.Nombre,
                apellido1 = persona.Apellido1,
                apellido2 = persona.Apellido2,
                fechaNacimiento = persona.FechaNacimiento.ToString("dd/MM/yyyy")
            });
        }
    }
}