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

        // GET: Doctors/Create
        public IActionResult Create()
        {
            ViewData["IdCedula"] = new SelectList(_context.Personas, "IdCedula", "IdCedula");
            ViewData["IdEspecialidad"] = new SelectList(_context.Especialidads, "IdEspecialidad", "IdEspecialidad");
            return View();
        }
        // POST: Doctors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Doctor doctor)
        {
            try
            {
                if (doctor != null)
                {
                    _context.Add(doctor);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ViewData["Error"] = "Error al crear el doctor: " + ex.Message;
            }

            ViewData["IdCedula"] = new SelectList(_context.Personas, "IdCedula", "IdCedula", doctor?.IdCedula);
            ViewData["IdEspecialidad"] = new SelectList(_context.Especialidads, "IdEspecialidad", "IdEspecialidad", doctor?.IdEspecialidad);
            return View(doctor);
        }

        // GET: Doctors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }
            ViewData["IdCedula"] = new SelectList(_context.Personas, "IdCedula", "IdCedula", doctor.IdCedula);
            ViewData["IdEspecialidad"] = new SelectList(_context.Especialidads, "IdEspecialidad", "IdEspecialidad", doctor.IdEspecialidad);
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
