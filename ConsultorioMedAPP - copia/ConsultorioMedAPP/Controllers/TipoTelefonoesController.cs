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
    public class TipoTelefonoesController : Controller
    {
        private readonly ConsultorioMedDBContext _context;

        public TipoTelefonoesController(ConsultorioMedDBContext context)
        {
            _context = context;
        }

        // GET: TipoTelefonoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.TipoTelefonos.ToListAsync());
        }

        // GET: TipoTelefonoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoTelefono = await _context.TipoTelefonos
                .FirstOrDefaultAsync(m => m.IdTipoTelefono == id);
            if (tipoTelefono == null)
            {
                return NotFound();
            }

            return View(tipoTelefono);
        }

        // GET: TipoTelefonoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoTelefonoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTipoTelefono,Descripcion,Activo")] TipoTelefono tipoTelefono)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipoTelefono);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoTelefono);
        }

        // GET: TipoTelefonoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoTelefono = await _context.TipoTelefonos.FindAsync(id);
            if (tipoTelefono == null)
            {
                return NotFound();
            }
            return View(tipoTelefono);
        }

        // POST: TipoTelefonoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdTipoTelefono,Descripcion,Activo")] TipoTelefono tipoTelefono)
        {
            if (id != tipoTelefono.IdTipoTelefono)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipoTelefono);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipoTelefonoExists(tipoTelefono.IdTipoTelefono))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tipoTelefono);
        }

        // GET: TipoTelefonoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoTelefono = await _context.TipoTelefonos
                .FirstOrDefaultAsync(m => m.IdTipoTelefono == id);
            if (tipoTelefono == null)
            {
                return NotFound();
            }

            return View(tipoTelefono);
        }

        // POST: TipoTelefonoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tipoTelefono = await _context.TipoTelefonos.FindAsync(id);
            if (tipoTelefono != null)
            {
                _context.TipoTelefonos.Remove(tipoTelefono);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipoTelefonoExists(int id)
        {
            return _context.TipoTelefonos.Any(e => e.IdTipoTelefono == id);
        }
    }
}
