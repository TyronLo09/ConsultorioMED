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
    public class PacientesController : Controller
    {
        private readonly ConsultorioMedDBContext _context;

        public PacientesController(ConsultorioMedDBContext context)
        {
            _context = context;
        }

        // GET: Pacientes
        public async Task<IActionResult> Index()
        {
            var consultorioMedDBContext = _context.Pacientes.Include(p => p.IdCedulaNavigation).Include(p => p.SeguroPacienteIdSeguroNavigation);
            return View(await consultorioMedDBContext.ToListAsync());
        }

        // GET: Pacientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paciente = await _context.Pacientes
                .Include(p => p.IdCedulaNavigation)
                .Include(p => p.SeguroPacienteIdSeguroNavigation)
                .FirstOrDefaultAsync(m => m.IdCedula == id);
            if (paciente == null)
            {
                return NotFound();
            }

            return View(paciente);
        }

        // GET: Pacientes/Create
        public IActionResult Create()
        {
            ViewData["IdCedula"] = new SelectList(_context.Personas, "IdCedula", "IdCedula");
            ViewData["SeguroPacienteIdSeguro"] = new SelectList(_context.Seguros, "IdSeguro", "IdSeguro");
            return View();
        }

        // POST: Pacientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Paciente paciente)
        {
            try
            {
                if (paciente != null)
                {
                    _context.Add(paciente);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewData["Error"] = "Los datos del paciente no pueden estar vacíos";
                }
            }
            catch (Exception ex)
            {
                ViewData["Error"] = "Error al crear el paciente: " + ex.Message;
            }

            ViewData["IdCedula"] = new SelectList(_context.Personas, "IdCedula", "IdCedula", paciente?.IdCedula);
            ViewData["SeguroPacienteIdSeguro"] = new SelectList(_context.Seguros, "IdSeguro", "IdSeguro", paciente?.SeguroPacienteIdSeguro);
            return View(paciente);
        }

        // GET: Pacientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente == null)
            {
                return NotFound();
            }
            ViewData["IdCedula"] = new SelectList(_context.Personas, "IdCedula", "IdCedula", paciente.IdCedula);
            ViewData["SeguroPacienteIdSeguro"] = new SelectList(_context.Seguros, "IdSeguro", "IdSeguro", paciente.SeguroPacienteIdSeguro);
            return View(paciente);
        }

        // POST: Pacientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Paciente paciente)
        {
            if (id != paciente.IdCedula)
            {
                return NotFound();
            }

            try
            {
                // Cargar la entidad existente
                var pacienteExistente = await _context.Pacientes.FindAsync(id);
                if (pacienteExistente == null)
                {
                    return NotFound();
                }

                // Actualizar solo las propiedades necesarias
                pacienteExistente.SeguroPacienteIdSeguro = paciente.SeguroPacienteIdSeguro;
                pacienteExistente.FechaRegistro = paciente.FechaRegistro;
                pacienteExistente.Estado = paciente.Estado;

                // Guardar cambios
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Verificar directamente si el paciente existe
                var existePaciente = await _context.Pacientes.AnyAsync(p => p.IdCedula == paciente.IdCedula);
                if (!existePaciente)
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
                ViewData["Error"] = "Error al actualizar el paciente: " + ex.Message;
            }

            ViewData["IdCedula"] = new SelectList(_context.Personas, "IdCedula", "IdCedula", paciente.IdCedula);
            ViewData["SeguroPacienteIdSeguro"] = new SelectList(_context.Seguros, "IdSeguro", "IdSeguro", paciente.SeguroPacienteIdSeguro);
            return View(paciente);
        }

        // GET: Pacientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paciente = await _context.Pacientes
                .Include(p => p.IdCedulaNavigation)
                .Include(p => p.SeguroPacienteIdSeguroNavigation)
                .FirstOrDefaultAsync(m => m.IdCedula == id);
            if (paciente == null)
            {
                return NotFound();
            }

            return View(paciente);
        }

        // POST: Pacientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente != null)
            {
                _context.Pacientes.Remove(paciente);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PacienteExists(int id)
        {
            return _context.Pacientes.Any(e => e.IdCedula == id);
        }
    }
}
