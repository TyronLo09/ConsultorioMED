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
    public class CorreosController : Controller
    {
        private readonly ConsultorioMedDBContext _context;

        public CorreosController(ConsultorioMedDBContext context)
        {
            _context = context;
        }

        // GET: Correos
        public async Task<IActionResult> Index()
        {
            var consultorioMedDBContext = _context.Correos.Include(c => c.IdCedulaNavigation).Include(c => c.IdTipoCorreoNavigation);
            return View(await consultorioMedDBContext.ToListAsync());
        }

        // GET: Correos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var correo = await _context.Correos
                .Include(c => c.IdCedulaNavigation)
                .Include(c => c.IdTipoCorreoNavigation)
                .FirstOrDefaultAsync(m => m.IdCorreo == id);
            if (correo == null)
            {
                return NotFound();
            }

            return View(correo);
        }

        // GET: Correos/Create
        public IActionResult Create()
        {
            ViewData["IdCedula"] = new SelectList(_context.Personas, "IdCedula", "IdCedula");
            ViewData["IdTipoCorreo"] = new SelectList(_context.TipoCorreos, "IdTipoCorreo", "IdTipoCorreo");
            return View();
        }

        // POST: Correos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Correo correo)
        {
            try
            {
                if (correo != null)
                {
                    _context.Add(correo);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewData["Error"] = "Los datos del correo no pueden estar vacíos";
                }
            }
            catch (Exception ex)
            {
                ViewData["Error"] = "Error al crear el correo: " + ex.Message;
            }

            ViewData["IdCedula"] = new SelectList(_context.Personas, "IdCedula", "IdCedula", correo?.IdCedula);
            ViewData["IdTipoCorreo"] = new SelectList(_context.TipoCorreos, "IdTipoCorreo", "IdTipoCorreo", correo?.IdTipoCorreo);
            return View(correo);
        }
        // GET: Correos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var correo = await _context.Correos.FindAsync(id);
            if (correo == null)
            {
                return NotFound();
            }
            ViewData["IdCedula"] = new SelectList(_context.Personas, "IdCedula", "IdCedula", correo.IdCedula);
            ViewData["IdTipoCorreo"] = new SelectList(_context.TipoCorreos, "IdTipoCorreo", "IdTipoCorreo", correo.IdTipoCorreo);
            return View(correo);
        }

        // POST: Correos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Correo correo)
        {
            if (id != correo.IdCorreo)
            {
                return NotFound();
            }

            try
            {
                // Cargar la entidad existente
                var correoExistente = await _context.Correos.FindAsync(id);
                if (correoExistente == null)
                {
                    return NotFound();
                }

                // Actualizar solo las propiedades necesarias
                correoExistente.DirecCorreo = correo.DirecCorreo;
                correoExistente.IdTipoCorreo = correo.IdTipoCorreo;
                correoExistente.IdCedula = correo.IdCedula;
                correoExistente.Activo = correo.Activo;

                // Guardar cambios
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Verificar directamente si el correo existe
                var existeCorreo = await _context.Correos.AnyAsync(c => c.IdCorreo == correo.IdCorreo);
                if (!existeCorreo)
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
                ViewData["Error"] = "Error al actualizar el correo: " + ex.Message;
            }

            ViewData["IdCedula"] = new SelectList(_context.Personas, "IdCedula", "IdCedula", correo.IdCedula);
            ViewData["IdTipoCorreo"] = new SelectList(_context.TipoCorreos, "IdTipoCorreo", "IdTipoCorreo", correo.IdTipoCorreo);
            return View(correo);
        }
        // GET: Correos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var correo = await _context.Correos
                .Include(c => c.IdCedulaNavigation)
                .Include(c => c.IdTipoCorreoNavigation)
                .FirstOrDefaultAsync(m => m.IdCorreo == id);
            if (correo == null)
            {
                return NotFound();
            }

            return View(correo);
        }

        // POST: Correos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var correo = await _context.Correos.FindAsync(id);
            if (correo != null)
            {
                _context.Correos.Remove(correo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CorreoExists(int id)
        {
            return _context.Correos.Any(e => e.IdCorreo == id);
        }
    }
}
