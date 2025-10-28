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
    public class TelefonoesController : Controller
    {
        private readonly ConsultorioMedDBContext _context;

        public TelefonoesController(ConsultorioMedDBContext context)
        {
            _context = context;
        }

        // GET: Telefonoes
        public async Task<IActionResult> Index()
        {
            var consultorioMedDBContext = _context.Telefonos.Include(t => t.IdCedulaNavigation).Include(t => t.IdTipoTelefonoNavigation);
            return View(await consultorioMedDBContext.ToListAsync());
        }

        // GET: Telefonoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var telefono = await _context.Telefonos
                .Include(t => t.IdCedulaNavigation)
                .Include(t => t.IdTipoTelefonoNavigation)
                .FirstOrDefaultAsync(m => m.IdTelefono == id);
            if (telefono == null)
            {
                return NotFound();
            }

            return View(telefono);
        }

        // GET: Telefonoes/Create
        public IActionResult Create()
        {
            ViewData["IdCedula"] = new SelectList(_context.Personas, "IdCedula", "IdCedula");
            ViewData["IdTipoTelefono"] = new SelectList(_context.TipoTelefonos, "IdTipoTelefono", "IdTipoTelefono");
            return View();
        }

        // POST: Telefonoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Telefono telefono)
        {
            try
            {
                if (telefono != null)
                {
                    _context.Add(telefono);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewData["Error"] = "Los datos del teléfono no pueden estar vacíos";
                }
            }
            catch (Exception ex)
            {
                ViewData["Error"] = "Error al crear el teléfono: " + ex.Message;
            }

            ViewData["IdCedula"] = new SelectList(_context.Personas, "IdCedula", "IdCedula", telefono?.IdCedula);
            ViewData["IdTipoTelefono"] = new SelectList(_context.TipoTelefonos, "IdTipoTelefono", "IdTipoTelefono", telefono?.IdTipoTelefono);
            return View(telefono);
        }

        // GET: Telefonoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var telefono = await _context.Telefonos.FindAsync(id);
            if (telefono == null)
            {
                return NotFound();
            }
            ViewData["IdCedula"] = new SelectList(_context.Personas, "IdCedula", "IdCedula", telefono.IdCedula);
            ViewData["IdTipoTelefono"] = new SelectList(_context.TipoTelefonos, "IdTipoTelefono", "IdTipoTelefono", telefono.IdTipoTelefono);
            return View(telefono);
        }

        // POST: Telefonoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Telefono telefono)
        {
            if (id != telefono.IdTelefono)
            {
                return NotFound();
            }

            try
            {
                // Cargar la entidad existente
                var telefonoExistente = await _context.Telefonos.FindAsync(id);
                if (telefonoExistente == null)
                {
                    return NotFound();
                }

                // Actualizar solo las propiedades necesarias
                telefonoExistente.Numero = telefono.Numero;
                telefonoExistente.IdTipoTelefono = telefono.IdTipoTelefono;
                telefonoExistente.IdCedula = telefono.IdCedula;
                telefonoExistente.Activo = telefono.Activo;

                // Guardar cambios
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Verificar directamente si el teléfono existe
                var existeTelefono = await _context.Telefonos.AnyAsync(t => t.IdTelefono == telefono.IdTelefono);
                if (!existeTelefono)
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
                ViewData["Error"] = "Error al actualizar el teléfono: " + ex.Message;
            }

            ViewData["IdCedula"] = new SelectList(_context.Personas, "IdCedula", "IdCedula", telefono.IdCedula);
            ViewData["IdTipoTelefono"] = new SelectList(_context.TipoTelefonos, "IdTipoTelefono", "IdTipoTelefono", telefono.IdTipoTelefono);
            return View(telefono);
        }
        // GET: Telefonoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var telefono = await _context.Telefonos
                .Include(t => t.IdCedulaNavigation)
                .Include(t => t.IdTipoTelefonoNavigation)
                .FirstOrDefaultAsync(m => m.IdTelefono == id);
            if (telefono == null)
            {
                return NotFound();
            }

            return View(telefono);
        }

        // POST: Telefonoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var telefono = await _context.Telefonos.FindAsync(id);
            if (telefono != null)
            {
                _context.Telefonos.Remove(telefono);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TelefonoExists(int id)
        {
            return _context.Telefonos.Any(e => e.IdTelefono == id);
        }
    }
}
