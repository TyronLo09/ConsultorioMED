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
    public class TipoCorreosController : Controller
    {
        private readonly ConsultorioMedDBContext _context;

        public TipoCorreosController(ConsultorioMedDBContext context)
        {
            _context = context;
        }

        // GET: TipoCorreos
        public async Task<IActionResult> Index()
        {
            return View(await _context.TipoCorreos.ToListAsync());
        }

        // GET: TipoCorreos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoCorreo = await _context.TipoCorreos
                .FirstOrDefaultAsync(m => m.IdTipoCorreo == id);
            if (tipoCorreo == null)
            {
                return NotFound();
            }

            return View(tipoCorreo);
        }

        // GET: TipoCorreos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoCorreos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTipoCorreo,Descripcion,Activo")] TipoCorreo tipoCorreo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipoCorreo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoCorreo);
        }

        // GET: TipoCorreos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoCorreo = await _context.TipoCorreos.FindAsync(id);
            if (tipoCorreo == null)
            {
                return NotFound();
            }
            return View(tipoCorreo);
        }

        // POST: TipoCorreos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdTipoCorreo,Descripcion,Activo")] TipoCorreo tipoCorreo)
        {
            if (id != tipoCorreo.IdTipoCorreo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipoCorreo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipoCorreoExists(tipoCorreo.IdTipoCorreo))
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
            return View(tipoCorreo);
        }

        // GET: TipoCorreos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoCorreo = await _context.TipoCorreos
                .FirstOrDefaultAsync(m => m.IdTipoCorreo == id);
            if (tipoCorreo == null)
            {
                return NotFound();
            }

            return View(tipoCorreo);
        }

        // POST: TipoCorreos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tipoCorreo = await _context.TipoCorreos.FindAsync(id);
            if (tipoCorreo != null)
            {
                _context.TipoCorreos.Remove(tipoCorreo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipoCorreoExists(int id)
        {
            return _context.TipoCorreos.Any(e => e.IdTipoCorreo == id);
        }
    }
}
