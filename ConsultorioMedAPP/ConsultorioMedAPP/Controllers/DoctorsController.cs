using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ConsultorioMedAPP.Models;

namespace ConsultorioMedAPP.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly ConsultorioMedDBContext _context;

        public DoctorsController(ConsultorioMedDBContext context)
        {
            _context = context;
        }

        // GET: Doctors
        public async Task<IActionResult> Index()
        {
            var consultorioMedDBContext = _context.Doctors.Include(d => d.IdCedulaNavigation).Include(d => d.IdEspecialidadNavigation);
            return View(await consultorioMedDBContext.ToListAsync());
        }

        // GET: Doctors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors
                .Include(d => d.IdCedulaNavigation)
                .Include(d => d.IdEspecialidadNavigation)
                .FirstOrDefaultAsync(m => m.IdCedula == id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }
        public async Task<IActionResult> ExportExcel()
        {
            var listado = await _context.Doctors
                .Include(d => d.IdCedulaNavigation)
                .Include(d => d.IdEspecialidadNavigation)
                .ToListAsync();

            using (var workbook = new ClosedXML.Excel.XLWorkbook())
            {
                var ws = workbook.Worksheets.Add("Doctores");

                ws.Cell(1, 1).Value = "Cédula";
                ws.Cell(1, 2).Value = "Nombre Completo";
                ws.Cell(1, 3).Value = "Especialidad";
                ws.Cell(1, 4).Value = "Activo";

                int row = 2;
                foreach (var d in listado)
                {
                    ws.Cell(row, 1).Value = d.IdCedula;
                    ws.Cell(row, 2).Value = d.IdCedulaNavigation.Nombre + " " + d.IdCedulaNavigation.Apellido1 + " " + d.IdCedulaNavigation.Apellido2;
                    ws.Cell(row, 3).Value = d.IdEspecialidadNavigation.Descripcion;
                    ws.Cell(row, 4).Value = d.Activo == true ? "Sí" : "No";

                    row++;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Doctores.xlsx");
                }
            }
        }
        public async Task<IActionResult> ExportPDF()
        {
            var listado = await _context.Doctors
                .Include(d => d.IdCedulaNavigation)
                .Include(d => d.IdEspecialidadNavigation)
                .ToListAsync();

            using (var ms = new MemoryStream())
            {
                var writer = new iText.Kernel.Pdf.PdfWriter(ms);
                var pdf = new iText.Kernel.Pdf.PdfDocument(writer);
                var document = new iText.Layout.Document(pdf);

                document.Add(new iText.Layout.Element.Paragraph("Lista de Doctores")
                .SetFontSize(16)
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
);


                var table = new iText.Layout.Element.Table(4).UseAllAvailableWidth();

                table.AddHeaderCell("Cédula");
                table.AddHeaderCell("Nombre Completo");
                table.AddHeaderCell("Especialidad");
                table.AddHeaderCell("Activo");

                foreach (var d in listado)
                {
                    table.AddCell(d.IdCedula.ToString());
                    table.AddCell(d.IdCedulaNavigation.Nombre + " " +
                                  d.IdCedulaNavigation.Apellido1 + " " +
                                  d.IdCedulaNavigation.Apellido2);
                    table.AddCell(d.IdEspecialidadNavigation.Descripcion);
                    table.AddCell(d.Activo == true ? "Sí" : "No");

                }

                document.Add(table);
                document.Close();

                return File(ms.ToArray(),
                    "application/pdf",
                    "Doctores.pdf");
            }
        }



        // GET: Doctors/Create
        public IActionResult Create()
        {
            ViewData["IdCedula"] = new SelectList(_context.Personas.Select(p => new
            {
                p.IdCedula,
                NombreCompleto = p.IdCedula + " - " + p.Nombre + " " + p.Apellido1 + " " + p.Apellido2
            }), "IdCedula", "NombreCompleto");

            ViewData["IdEspecialidad"] = new SelectList(_context.Especialidads, "IdEspecialidad", "Descripcion");
            return View();
        }

        // POST: Doctors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Doctor doctor)
        {
            Console.WriteLine("🩺 Entró al método POST Create de Doctor");

            // 🔹 Cargar las entidades relacionadas
            doctor.IdCedulaNavigation = await _context.Personas.FindAsync(doctor.IdCedula);
            doctor.IdEspecialidadNavigation = await _context.Especialidads.FindAsync(doctor.IdEspecialidad);

            // 🔹 Eliminar errores de validación de navegación
            ModelState.Remove("IdCedulaNavigation");
            ModelState.Remove("IdEspecialidadNavigation");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(doctor);
                    await _context.SaveChangesAsync();
                    Console.WriteLine("✅ Doctor guardado correctamente.");
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("❌ Error al guardar doctor: " + ex.Message);
                    ViewData["Error"] = "Error al crear el doctor: " + ex.Message;
                }
            }
            else
            {
                Console.WriteLine("❌ ModelState inválido");
                foreach (var err in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine("⚠️ " + err.ErrorMessage);
                }
            }

            // 🔹 Recargar listas si hay error
            ViewData["IdCedula"] = new SelectList(
                _context.Personas.Select(p => new
                {
                    p.IdCedula,
                    NombreCompleto = p.IdCedula + " - " + p.Nombre + " " + p.Apellido1 + " " + p.Apellido2
                }),
                "IdCedula",
                "NombreCompleto",
                doctor.IdCedula
            );

            ViewData["IdEspecialidad"] = new SelectList(
                _context.Especialidads,
                "IdEspecialidad",
                "Descripcion",
                doctor.IdEspecialidad
            );

            // ✅ Retorno final garantizado
            return View(doctor);
        }






        // GET: Doctors/Edit/5
        // GET: Doctors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors
                .Include(d => d.IdCedulaNavigation)
                .Include(d => d.IdEspecialidadNavigation)
                .FirstOrDefaultAsync(d => d.IdCedula == id);

            if (doctor == null)
            {
                return NotFound();
            }

            // ✅ Mostrar el nombre completo de la persona en lugar del número de cédula
            ViewData["IdCedula"] = new SelectList(
                _context.Personas.Select(p => new
                {
                    p.IdCedula,
                    NombreCompleto = p.Nombre + " " + p.Apellido1 + " " + p.Apellido2
                }),
                "IdCedula",
                "NombreCompleto",
                doctor.IdCedula
            );

            // ✅ Mostrar la descripción de la especialidad en lugar del ID
            ViewData["IdEspecialidad"] = new SelectList(
                _context.Especialidads,
                "IdEspecialidad",
                "Descripcion",
                doctor.IdEspecialidad
            );

            return View(doctor);
        }


        // POST: Doctors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Doctor doctor)
        {
            if (id != doctor.IdCedula)
            {
                return NotFound();
            }

            try
            {
                // Cargar la entidad existente
                var doctorExistente = await _context.Doctors.FindAsync(id);
                if (doctorExistente == null)
                {
                    return NotFound();
                }

                // Actualizar solo las propiedades necesarias
                doctorExistente.IdEspecialidad = doctor.IdEspecialidad;
                doctorExistente.Activo = doctor.Activo;

                // Guardar cambios
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Verificar directamente si el doctor existe
                var existeDoctor = await _context.Doctors.AnyAsync(d => d.IdCedula == doctor.IdCedula);
                if (!existeDoctor)
                {
                    return NotFound();
                }
                else
                {
                    ViewData["Error"] = "Error de concurrencia: El registro fue modificado por otro usuario. Por favor, recargue la página e intente nuevamente.";
                }
            }
            catch (Exception ex)
            {
                ViewData["Error"] = "Error al actualizar: " + ex.Message;
            }

            ViewData["IdCedula"] = new SelectList(_context.Personas, "IdCedula", "IdCedula", doctor.IdCedula);
            ViewData["IdEspecialidad"] = new SelectList(_context.Especialidads, "IdEspecialidad", "IdEspecialidad", doctor.IdEspecialidad);
            return View(doctor);
        }

        // GET: Doctors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors
                .Include(d => d.IdCedulaNavigation)
                .Include(d => d.IdEspecialidadNavigation)
                .FirstOrDefaultAsync(m => m.IdCedula == id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // POST: Doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor != null)
            {
                _context.Doctors.Remove(doctor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DoctorExists(int id)
        {
            return _context.Doctors.Any(e => e.IdCedula == id);
        }
    }
}

