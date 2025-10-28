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
    public class SeguroesController : Controller
    {
        private readonly ConsultorioMedDBContext _context;

        public SeguroesController(ConsultorioMedDBContext context)
        {
            _context = context;
        }

        // GET: Seguroes
        public async Task<IActionResult> Index()
        {
            var consultorioMedDBContext = _context.Seguros.Include(s => s.IdCedulaNavigation).Include(s => s.IdTipoSeguroNavigation);
            return View(await consultorioMedDBContext.ToListAsync());
        }

        // GET: Seguroes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seguro = await _context.Seguros
                .Include(s => s.IdCedulaNavigation)
                .Include(s => s.IdTipoSeguroNavigation)
                .FirstOrDefaultAsync(m => m.IdSeguro == id);
            if (seguro == null)
            {
                return NotFound();
            }

            return View(seguro);
        }

        // GET: Seguroes/Create
        public IActionResult Create()
        {
            ViewData["IdCedula"] = new SelectList(_context.Personas, "IdCedula", "IdCedula");
            ViewData["IdTipoSeguro"] = new SelectList(_context.TipoSeguros, "IdTipoSeguro", "IdTipoSeguro");
            return View();
        }

        // POST: Seguroes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Seguro seguro)
        {
            try
            {
                if (seguro != null)
                {
                    _context.Add(seguro);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewData["Error"] = "Los datos del seguro no pueden estar vacíos";
                }
            }
            catch (Exception ex)
            {
                ViewData["Error"] = "Error al crear el seguro: " + ex.Message;
            }

            ViewData["IdCedula"] = new SelectList(_context.Personas, "IdCedula", "IdCedula", seguro?.IdCedula);
            ViewData["IdTipoSeguro"] = new SelectList(_context.TipoSeguros, "IdTipoSeguro", "IdTipoSeguro", seguro?.IdTipoSeguro);
            return View(seguro);
        }

        // GET: Seguroes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seguro = await _context.Seguros.FindAsync(id);
            if (seguro == null)
            {
                return NotFound();
            }
            ViewData["IdCedula"] = new SelectList(_context.Personas, "IdCedula", "IdCedula", seguro.IdCedula);
            ViewData["IdTipoSeguro"] = new SelectList(_context.TipoSeguros, "IdTipoSeguro", "IdTipoSeguro", seguro.IdTipoSeguro);
            return View(seguro);
        }

        // POST: Seguroes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Seguro seguro)
        {
            if (id != seguro.IdSeguro)
            {
                return NotFound();
            }

            try
            {
                // Cargar la entidad existente
                var seguroExistente = await _context.Seguros.FindAsync(id);
                if (seguroExistente == null)
                {
                    return NotFound();
                }

                // Actualizar solo las propiedades necesarias
                seguroExistente.IdTipoSeguro = seguro.IdTipoSeguro;
                seguroExistente.FechaCreacion = seguro.FechaCreacion;
                seguroExistente.IdCedula = seguro.IdCedula;
                seguroExistente.Activo = seguro.Activo;

                // Guardar cambios
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Verificar directamente si el seguro existe
                var existeSeguro = await _context.Seguros.AnyAsync(s => s.IdSeguro == seguro.IdSeguro);
                if (!existeSeguro)
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
                ViewData["Error"] = "Error al actualizar el seguro: " + ex.Message;
            }

            ViewData["IdCedula"] = new SelectList(_context.Personas, "IdCedula", "IdCedula", seguro.IdCedula);
            ViewData["IdTipoSeguro"] = new SelectList(_context.TipoSeguros, "IdTipoSeguro", "IdTipoSeguro", seguro.IdTipoSeguro);
            return View(seguro);
        }
        // GET: Seguroes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seguro = await _context.Seguros
                .Include(s => s.IdCedulaNavigation)
                .Include(s => s.IdTipoSeguroNavigation)
                .FirstOrDefaultAsync(m => m.IdSeguro == id);
            if (seguro == null)
            {
                return NotFound();
            }

            return View(seguro);
        }

        // POST: Seguroes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var seguro = await _context.Seguros.FindAsync(id);
            if (seguro != null)
            {
                _context.Seguros.Remove(seguro);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SeguroExists(int id)
        {
            return _context.Seguros.Any(e => e.IdSeguro == id);
        }
    }
}
