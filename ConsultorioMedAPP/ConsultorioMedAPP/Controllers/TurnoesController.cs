using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ConsultorioMedAPP.Models;

namespace ConsultorioMedAPP.Controllers
{
    public class TurnoesController : Controller
    {
        private readonly ConsultorioMedDBContext _context;

        public TurnoesController(ConsultorioMedDBContext context)
        {
            _context = context;
        }

        // GET: Turnoes
        public async Task<IActionResult> Index()
        {
            var turnos = await _context.Turnos
                .Include(t => t.IdCedulaNavigation)    // Doctor/Persona
                .Include(t => t.IdTipoTurnoNavigation) // TipoTurno
                .ToListAsync();

            return View(turnos);
        }

        // GET: Turnoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var turno = await _context.Turnos
                .Include(t => t.IdCedulaNavigation)
                .Include(t => t.IdTipoTurnoNavigation)
                .FirstOrDefaultAsync(t => t.IdTurno == id);

            if (turno == null) return NotFound();

            return View(turno);
        }

        // GET: Turnoes/Create
        public IActionResult Create()
        {
            // Usamos NombreCompleto del doctor para que se vea bien en el dropdown
            ViewData["IdCedula"] = new SelectList(
                _context.Doctors.Select(d => new
                {
                    d.IdCedula,
                    NombreCompleto = d.IdCedulaNavigation.Nombre + " " +
                                     d.IdCedulaNavigation.Apellido1 + " " +
                                     d.IdCedulaNavigation.Apellido2
                }),
                "IdCedula",
                "NombreCompleto"
            );

            ViewData["IdTipoTurno"] = new SelectList(_context.TipoTurnos, "IdTipoTurno", "Descripcion");

            return View();
        }

        // POST: Turnoes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Turno turno)
        {
            try
            {
                if (turno != null)
                {
                    _context.Add(turno);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewData["Error"] = "Los datos del turno no pueden estar vacíos";
                }
            }
            catch (Exception ex)
            {
                ViewData["Error"] = "Error al crear el turno: " + ex.Message;
            }

            ViewData["IdCedula"] = new SelectList(
                _context.Doctors.Select(d => new
                {
                    d.IdCedula,
                    NombreCompleto = d.IdCedulaNavigation.Nombre + " " +
                                     d.IdCedulaNavigation.Apellido1 + " " +
                                     d.IdCedulaNavigation.Apellido2
                }),
                "IdCedula",
                "NombreCompleto",
                turno?.IdCedula
            );

            ViewData["IdTipoTurno"] = new SelectList(_context.TipoTurnos, "IdTipoTurno", "Descripcion", turno?.IdTipoTurno);
            return View(turno);
        }

        // GET: Turnoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var turno = await _context.Turnos.FindAsync(id);
            if (turno == null) return NotFound();

            ViewData["IdCedula"] = new SelectList(
                _context.Doctors.Select(d => new
                {
                    d.IdCedula,
                    NombreCompleto = d.IdCedulaNavigation.Nombre + " " +
                                     d.IdCedulaNavigation.Apellido1 + " " +
                                     d.IdCedulaNavigation.Apellido2
                }),
                "IdCedula",
                "NombreCompleto",
                turno.IdCedula
            );

            ViewData["IdTipoTurno"] = new SelectList(_context.TipoTurnos, "IdTipoTurno", "Descripcion", turno.IdTipoTurno);

            return View(turno);
        }

        // POST: Turnoes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Turno turno)
        {
            if (id != turno.IdTurno) return NotFound();

            try
            {
                var turnoExistente = await _context.Turnos.FindAsync(id);
                if (turnoExistente == null) return NotFound();

                turnoExistente.IdCedula = turno.IdCedula;
                turnoExistente.IdTipoTurno = turno.IdTipoTurno;
                turnoExistente.Activo = turno.Activo;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Turnos.Any(t => t.IdTurno == turno.IdTurno)) return NotFound();
                ViewData["Error"] = "Error de concurrencia: El registro fue modificado por otro usuario. Por favor, recargue la página e intente nuevamente.";
            }
            catch (Exception ex)
            {
                ViewData["Error"] = "Error al actualizar el turno: " + ex.Message;
            }

            ViewData["IdCedula"] = new SelectList(
                _context.Doctors.Select(d => new
                {
                    d.IdCedula,
                    NombreCompleto = d.IdCedulaNavigation.Nombre + " " +
                                     d.IdCedulaNavigation.Apellido1 + " " +
                                     d.IdCedulaNavigation.Apellido2
                }),
                "IdCedula",
                "NombreCompleto",
                turno.IdCedula
            );

            ViewData["IdTipoTurno"] = new SelectList(_context.TipoTurnos, "IdTipoTurno", "Descripcion", turno.IdTipoTurno);

            return View(turno);
        }

        // GET: Turnoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var turno = await _context.Turnos
                .Include(t => t.IdCedulaNavigation)
                .Include(t => t.IdTipoTurnoNavigation)
                .FirstOrDefaultAsync(m => m.IdTurno == id);

            if (turno == null) return NotFound();

            return View(turno);
        }

        // POST: Turnoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var turno = await _context.Turnos.FindAsync(id);
            if (turno != null) _context.Turnos.Remove(turno);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TurnoExists(int id)
        {
            return _context.Turnos.Any(e => e.IdTurno == id);
        }
    }
}
