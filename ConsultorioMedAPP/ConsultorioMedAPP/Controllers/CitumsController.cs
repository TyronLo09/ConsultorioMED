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
    public class CitumsController : Controller
    {
        private readonly ConsultorioMedDBContext _context;

        public CitumsController(ConsultorioMedDBContext context)
        {
            _context = context;
        }

        // GET: Citums
        public async Task<IActionResult> Index()
        {
            var consultorioMedDBContext = _context.Cita.Include(c => c.DoctorIdCedulaNavigation).Include(c => c.EstadoCitaIdEstadoCitaNavigation).Include(c => c.PacienteIdCedulaNavigation);
            return View(await consultorioMedDBContext.ToListAsync());
        }

        // GET: Citums/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var citum = await _context.Cita
                .Include(c => c.DoctorIdCedulaNavigation)
                .Include(c => c.EstadoCitaIdEstadoCitaNavigation)
                .Include(c => c.PacienteIdCedulaNavigation)
                .FirstOrDefaultAsync(m => m.IdCita == id);
            if (citum == null)
            {
                return NotFound();
            }

            return View(citum);
        }

        // GET: Citums/Create
        public IActionResult Create()
        {
            ViewData["DoctorIdCedula"] = new SelectList(_context.Doctors, "IdCedula", "IdCedula");
            ViewData["EstadoCitaIdEstadoCita"] = new SelectList(_context.EstadoCita, "IdEstadoCita", "IdEstadoCita");
            ViewData["PacienteIdCedula"] = new SelectList(_context.Pacientes, "IdCedula", "IdCedula");
            return View();
        }

        // POST: Citums/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Citum citum)
        {
            try
            {
                // DEBUG: Verificar qué datos llegan
                Console.WriteLine($"Datos recibidos - Paciente: {citum?.PacienteIdCedula}, Doctor: {citum?.DoctorIdCedula}, Estado: {citum?.EstadoCitaIdEstadoCita}");

                if (citum == null)
                {
                    ViewData["Error"] = "El objeto cita está vacío";
                    CargarSelectLists();
                    return View(citum);
                }

                // Validar campos requeridos
                if (citum.PacienteIdCedula == 0)
                {
                    ViewData["Error"] = "Debe seleccionar un paciente";
                    CargarSelectLists(citum);
                    return View(citum);
                }

                if (citum.DoctorIdCedula == 0)
                {
                    ViewData["Error"] = "Debe seleccionar un doctor";
                    CargarSelectLists(citum);
                    return View(citum);
                }

                if (citum.EstadoCitaIdEstadoCita == 0)
                {
                    ViewData["Error"] = "Debe seleccionar un estado de cita";
                    CargarSelectLists(citum);
                    return View(citum);
                }

                // Asignar fechas automáticamente
                citum.FechaCreacion = DateTime.Now;
                citum.FechaActualizacion = DateTime.Now;

                Console.WriteLine($"Intentando crear cita: Paciente={citum.PacienteIdCedula}, Doctor={citum.DoctorIdCedula}");

                _context.Add(citum);
                int resultado = await _context.SaveChangesAsync();

                Console.WriteLine($"Resultado SaveChanges: {resultado}");

                if (resultado > 0)
                {
                    TempData["Success"] = "Cita creada exitosamente";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewData["Error"] = "No se pudo guardar la cita en la base de datos";
                    CargarSelectLists(citum);
                    return View(citum);
                }
            }
            catch (Exception ex)
            {
                // Capturar TODOS los errores
                string errorCompleto = ex.Message;
                if (ex.InnerException != null)
                    errorCompleto += $" - {ex.InnerException.Message}";

                ViewData["Error"] = $"Error: {errorCompleto}";
                Console.WriteLine($"ERROR COMPLETO: {errorCompleto}");

                CargarSelectLists(citum);
                return View(citum);
            }
        }

        // Método auxiliar para cargar los SelectList
        private void CargarSelectLists(Citum citum = null)
        {
            ViewData["DoctorIdCedula"] = new SelectList(_context.Doctors, "IdCedula", "IdCedula", citum?.DoctorIdCedula);
            ViewData["EstadoCitaIdEstadoCita"] = new SelectList(_context.EstadoCita, "IdEstadoCita", "IdEstadoCita", citum?.EstadoCitaIdEstadoCita);
            ViewData["PacienteIdCedula"] = new SelectList(_context.Pacientes, "IdCedula", "IdCedula", citum?.PacienteIdCedula);
        }

        // GET: Citums/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var citum = await _context.Cita.FindAsync(id);
            if (citum == null)
            {
                return NotFound();
            }
            ViewData["DoctorIdCedula"] = new SelectList(_context.Doctors, "IdCedula", "IdCedula", citum.DoctorIdCedula);
            ViewData["EstadoCitaIdEstadoCita"] = new SelectList(_context.EstadoCita, "IdEstadoCita", "IdEstadoCita", citum.EstadoCitaIdEstadoCita);
            ViewData["PacienteIdCedula"] = new SelectList(_context.Pacientes, "IdCedula", "IdCedula", citum.PacienteIdCedula);
            return View(citum);
        }

        // POST: Citums/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Citum citum)
        {
            if (id != citum.IdCita)
            {
                return NotFound();
            }

            try
            {
                var citaExistente = await _context.Cita.FindAsync(id);
                if (citaExistente == null)
                {
                    return NotFound();
                }

                // Actualizar propiedades
                citaExistente.PacienteIdCedula = citum.PacienteIdCedula;
                citaExistente.DoctorIdCedula = citum.DoctorIdCedula;
                citaExistente.Fecha = citum.Fecha;
                citaExistente.Hora = citum.Hora;
                citaExistente.EstadoCitaIdEstadoCita = citum.EstadoCitaIdEstadoCita;
                citaExistente.Precio = citum.Precio;
                citaExistente.FechaActualizacion = DateTime.Now;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!_context.Cita.Any(e => e.IdCita == citum.IdCita))
                {
                    return NotFound();
                }
                else
                {
                    ViewData["Error"] = "Error de concurrencia: " + ex.Message;
                }
            }
            catch (Exception ex)
            {
                ViewData["Error"] = "Error al actualizar: " + ex.Message;
            }

            ViewData["DoctorIdCedula"] = new SelectList(_context.Doctors, "IdCedula", "IdCedula", citum.DoctorIdCedula);
            ViewData["EstadoCitaIdEstadoCita"] = new SelectList(_context.EstadoCita, "IdEstadoCita", "IdEstadoCita", citum.EstadoCitaIdEstadoCita);
            ViewData["PacienteIdCedula"] = new SelectList(_context.Pacientes, "IdCedula", "IdCedula", citum.PacienteIdCedula);
            return View(citum);
        }

        // GET: Citums/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var citum = await _context.Cita
                .Include(c => c.DoctorIdCedulaNavigation)
                .Include(c => c.EstadoCitaIdEstadoCitaNavigation)
                .Include(c => c.PacienteIdCedulaNavigation)
                .FirstOrDefaultAsync(m => m.IdCita == id);
            if (citum == null)
            {
                return NotFound();
            }

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
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CitumExists(int id)
        {
            return _context.Cita.Any(e => e.IdCita == id);
        }
    }
}
