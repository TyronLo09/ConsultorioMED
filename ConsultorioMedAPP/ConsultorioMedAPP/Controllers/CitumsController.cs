using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ConsultorioMedAPP.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using ClosedXML.Excel;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Font;
using iText.IO.Font.Constants;
using iText.Layout.Element;
using iText.Layout.Properties;



namespace ConsultorioMedAPP.Controllers
{
    public class CitumsController : Controller
    {
        private readonly ConsultorioMedDBContext _context;

        public CitumsController(ConsultorioMedDBContext context)
        {
            _context = context;
        }
        public IActionResult ExportExcel()
        {
            var citas = _context.Cita
                .Include(c => c.DoctorIdCedulaNavigation)
                    .ThenInclude(d => d.IdCedulaNavigation)
                .Include(c => c.EstadoCitaIdEstadoCitaNavigation)
                .Include(c => c.PacienteIdCedulaNavigation)
                    .ThenInclude(p => p.IdCedulaNavigation)
                .ToList();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Citas");

                // Encabezados
                worksheet.Cell(1, 1).Value = "ID";
                worksheet.Cell(1, 2).Value = "Paciente";
                worksheet.Cell(1, 3).Value = "Doctor";
                worksheet.Cell(1, 4).Value = "Estado";
                worksheet.Cell(1, 5).Value = "Fecha";
                worksheet.Cell(1, 6).Value = "Hora";
                worksheet.Cell(1, 7).Value = "Precio";

                int row = 2;

                foreach (var c in citas)
                {
                    worksheet.Cell(row, 1).Value = c.IdCita;
                    worksheet.Cell(row, 2).Value = c.PacienteIdCedulaNavigation?.IdCedulaNavigation?.Nombre;
                    worksheet.Cell(row, 3).Value = c.DoctorIdCedulaNavigation?.IdCedulaNavigation?.Nombre;
                    worksheet.Cell(row, 4).Value = c.EstadoCitaIdEstadoCitaNavigation?.Descripcion;
                    worksheet.Cell(row, 5).Value = c.Fecha.ToString("dd/MM/yyyy");
                    worksheet.Cell(row, 6).Value = c.Hora.ToString("HH:mm");
                    worksheet.Cell(row, 7).Value = c.Precio;
                    row++;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return File(
                       stream.ToArray(),
                       "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                       $"ListadoCitas_{DateTime.Now:ddMMyyyy}.xlsx"
                    );
                }
            }
        }
        public IActionResult ExportPDF()
        {
            var citas = _context.Cita
                .Include(c => c.DoctorIdCedulaNavigation)
                    .ThenInclude(d => d.IdCedulaNavigation)
                .Include(c => c.EstadoCitaIdEstadoCitaNavigation)
                .Include(c => c.PacienteIdCedulaNavigation)
                    .ThenInclude(p => p.IdCedulaNavigation)
                .ToList();

            using (var stream = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(stream);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);

                var fontNormal = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
                var fontBold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

                // TÍTULO
                document.Add(
                    new Paragraph("LISTADO DE CITAS")
                    .SetFont(fontBold)
                    .SetFontSize(14)
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                );

                // FECHA
                document.Add(
                    new Paragraph($"Generado el: {DateTime.Now:dd/MM/yyyy HH:mm}")
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT)
                    .SetFontSize(9)
                );

                document.Add(new Paragraph("\n"));

                var table = new iText.Layout.Element.Table(6).UseAllAvailableWidth();
                table.SetTextAlignment(TextAlignment.CENTER);



                // ENCABEZADOS
                string[] headers =
                {
            "ID", "Paciente", "Doctor", "Estado", "Fecha", "Hora", "Precio"
        };

                foreach (var h in headers)
                {
                    table.AddHeaderCell(
                        new Cell()
                        .Add(new Paragraph(h).SetFont(fontBold))
                        .SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY)
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                    );
                }

                // FILAS
                foreach (var c in citas)
                {
                    string paciente = $"{c.PacienteIdCedulaNavigation?.IdCedulaNavigation?.Nombre} " +
                                      $"{c.PacienteIdCedulaNavigation?.IdCedulaNavigation?.Apellido1}";

                    string doctor = $"{c.DoctorIdCedulaNavigation?.IdCedulaNavigation?.Nombre} " +
                                    $"{c.DoctorIdCedulaNavigation?.IdCedulaNavigation?.Apellido1}";

                    table.AddCell(new Cell().Add(new Paragraph(c.IdCita.ToString())));
                    table.AddCell(new Cell().Add(new Paragraph(paciente)));
                    table.AddCell(new Cell().Add(new Paragraph(doctor)));
                    table.AddCell(new Cell().Add(new Paragraph(c.EstadoCitaIdEstadoCitaNavigation?.Descripcion ?? "")));
                    table.AddCell(new Cell().Add(new Paragraph(c.Fecha.ToString("dd/MM/yyyy"))));
                    table.AddCell(new Cell().Add(new Paragraph(c.Hora.ToString("HH:mm"))));
                    table.AddCell(new Cell().Add(new Paragraph(c.Precio.ToString("N2"))));
                }

                document.Add(table);

                // TOTAL AL FINAL
                document.Add(
                    new Paragraph($"Total de citas: {citas.Count}")
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT)
                    .SetFont(fontBold)
                );

                document.Close();

                return File(
                    stream.ToArray(),
                    "application/pdf",
                    $"ListadoCitas_{DateTime.Now:ddMMyyyy}.pdf"
                );
            }
        }



        // GET: Citums
        public async Task<IActionResult> Index()
        {
            var citas = _context.Cita
                .Include(c => c.DoctorIdCedulaNavigation)
                    .ThenInclude(d => d.IdCedulaNavigation)
                .Include(c => c.EstadoCitaIdEstadoCitaNavigation)
                .Include(c => c.PacienteIdCedulaNavigation)
                    .ThenInclude(p => p.IdCedulaNavigation);

            return View(await citas.ToListAsync());
        }

        // GET: Citums/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var citum = await _context.Cita
                .Include(c => c.DoctorIdCedulaNavigation)
                    .ThenInclude(d => d.IdCedulaNavigation)
                .Include(c => c.EstadoCitaIdEstadoCitaNavigation)
                .Include(c => c.PacienteIdCedulaNavigation)
                    .ThenInclude(p => p.IdCedulaNavigation)
                .FirstOrDefaultAsync(m => m.IdCita == id);

            if (citum == null) return NotFound();

            return View(citum);
        }

        // GET: Citums/Create
        public IActionResult Create()
        {
            CargarSelectLists();
            return View();
        }

        // POST: Citums/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection form)
        {
            Citum citum = new Citum();

            try
            {
                // Parse IDs (validar antes)
                if (!int.TryParse(form["PacienteIdCedula"], out var pacienteId))
                {
                    ModelState.AddModelError("PacienteIdCedula", "Seleccione un paciente válido.");
                }
                if (!int.TryParse(form["DoctorIdCedula"], out var doctorId))
                {
                    ModelState.AddModelError("DoctorIdCedula", "Seleccione un doctor válido.");
                }
                if (!int.TryParse(form["EstadoCitaIdEstadoCita"], out var estadoId))
                {
                    ModelState.AddModelError("EstadoCitaIdEstadoCita", "Seleccione un estado válido.");
                }

                // Fecha (DateOnly) y Hora (TimeOnly)
                DateOnly fechaParsed = default;
                TimeOnly horaParsed = default;
                bool fechaOk = DateOnly.TryParse(form["Fecha"], out fechaParsed);
                bool horaOk = TimeOnly.TryParse(form["Hora"], out horaParsed);

                if (!fechaOk) ModelState.AddModelError("Fecha", "Fecha inválida.");
                if (!horaOk) ModelState.AddModelError("Hora", "Hora inválida.");

                // Precio opcional/required según tu lógica
                decimal precioParsed = 0m;
                if (!string.IsNullOrWhiteSpace(form["Precio"]) && !decimal.TryParse(form["Precio"], out precioParsed))
                    ModelState.AddModelError("Precio", "Precio inválido.");

                // Si hay errores de validación, recargar y devolver vista
                if (!ModelState.IsValid)
                {
                    CargarSelectLists();
                    return View(citum);
                }

                // Asignar valores al modelo
                citum.PacienteIdCedula = pacienteId;
                citum.DoctorIdCedula = doctorId;
                citum.EstadoCitaIdEstadoCita = estadoId;
                citum.Fecha = fechaParsed;
                citum.Hora = horaParsed;
                citum.Precio = precioParsed;
                citum.FechaCreacion = DateTime.Now;
                citum.FechaActualizacion = DateTime.Now;

                _context.Cita.Add(citum);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Cita creada correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log completo (útil para ver inner exceptions)
                Console.WriteLine("ERROR Create Citum: " + ex.ToString());

                // Mostrar mensaje amigable y recargar selects
                ModelState.AddModelError(string.Empty, "Ocurrió un error al crear la cita. " + ex.Message);
                CargarSelectLists();
                return View(citum);
            }
        }

        // GET: Citums/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var citum = await _context.Cita.FindAsync(id);
            if (citum == null) return NotFound();

            CargarSelectLists();
            return View(citum);
        }

        // POST: Citums/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IFormCollection form)
        {
            var citum = await _context.Cita.FindAsync(id);
            if (citum == null) return NotFound();

            try
            {
                // Parse y validaciones similares al Create
                if (!int.TryParse(form["PacienteIdCedula"], out var pacienteId))
                    ModelState.AddModelError("PacienteIdCedula", "Seleccione un paciente válido.");
                if (!int.TryParse(form["DoctorIdCedula"], out var doctorId))
                    ModelState.AddModelError("DoctorIdCedula", "Seleccione un doctor válido.");
                if (!int.TryParse(form["EstadoCitaIdEstadoCita"], out var estadoId))
                    ModelState.AddModelError("EstadoCitaIdEstadoCita", "Seleccione un estado válido.");

                if (!DateOnly.TryParse(form["Fecha"], out var fechaParsed))
                    ModelState.AddModelError("Fecha", "Fecha inválida.");
                if (!TimeOnly.TryParse(form["Hora"], out var horaParsed))
                    ModelState.AddModelError("Hora", "Hora inválida.");

                decimal precioParsed = 0m;
                if (!string.IsNullOrWhiteSpace(form["Precio"]) && !decimal.TryParse(form["Precio"], out precioParsed))
                    ModelState.AddModelError("Precio", "Precio inválido.");

                if (!ModelState.IsValid)
                {
                    CargarSelectLists();
                    return View(citum);
                }

                // Asignar
                citum.PacienteIdCedula = pacienteId;
                citum.DoctorIdCedula = doctorId;
                citum.EstadoCitaIdEstadoCita = estadoId;
                citum.Fecha = fechaParsed;
                citum.Hora = horaParsed;
                citum.Precio = precioParsed;
                citum.FechaActualizacion = DateTime.Now;

                _context.Cita.Update(citum);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Cita actualizada correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR Edit Citum: " + ex.ToString());
                ModelState.AddModelError(string.Empty, "Ocurrió un error al actualizar la cita. " + ex.Message);
                CargarSelectLists();
                return View(citum);
            }
        }

        // GET: Citums/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var citum = await _context.Cita
                .Include(c => c.DoctorIdCedulaNavigation)
                    .ThenInclude(d => d.IdCedulaNavigation)
                .Include(c => c.EstadoCitaIdEstadoCitaNavigation)
                .Include(c => c.PacienteIdCedulaNavigation)
                    .ThenInclude(p => p.IdCedulaNavigation)
                .FirstOrDefaultAsync(m => m.IdCita == id);

            if (citum == null) return NotFound();

            return View(citum);
        }

        // POST: Citums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var citum = await _context.Cita.FindAsync(id);
            if (citum != null)
            {
                _context.Cita.Remove(citum);
                await _context.SaveChangesAsync();
            }
            TempData["Success"] = "Cita eliminada.";
            return RedirectToAction(nameof(Index));
        }

        private bool CitumExists(int id)
        {
            return _context.Cita.Any(e => e.IdCita == id);
        }

        // Cargar selects con nombres (usando navigation Persona)
        private void CargarSelectLists()
        {
            // Pacientes
            ViewData["PacienteIdCedula"] = new SelectList(
                _context.Pacientes
                    .Include(p => p.IdCedulaNavigation)
                    .Select(p => new
                    {
                        Id = p.IdCedula,
                        NombreCompleto = p.IdCedulaNavigation.Nombre + " " + p.IdCedulaNavigation.Apellido1
                    })
                    .ToList(),
                "Id",
                "NombreCompleto"
            );

            // Doctores
            ViewData["DoctorIdCedula"] = new SelectList(
                _context.Doctors
                    .Include(d => d.IdCedulaNavigation)
                    .Select(d => new
                    {
                        Id = d.IdCedula,
                        NombreCompleto = "Dr. " + d.IdCedulaNavigation.Nombre + " " + d.IdCedulaNavigation.Apellido1
                    })
                    .ToList(),
                "Id",
                "NombreCompleto"
            );

            // Estados de cita
            ViewData["EstadoCitaIdEstadoCita"] = new SelectList(
                _context.EstadoCita
                    .Select(e => new { e.IdEstadoCita, e.Descripcion })
                    .ToList(),
                "IdEstadoCita",
                "Descripcion"
            );
        }
    }
}
