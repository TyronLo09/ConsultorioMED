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
    public class TipoEnfermedadsController : Controller
    {
        private readonly ConsultorioMedDBContext _context;

        public TipoEnfermedadsController(ConsultorioMedDBContext context)
        {
            _context = context;
        }

        // GET: TipoEnfermedads
        public async Task<IActionResult> Index()
        {
            return View(await _context.TipoEnfermedads.ToListAsync());
        }

        // GET: TipoEnfermedads/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoEnfermedad = await _context.TipoEnfermedads
                .FirstOrDefaultAsync(m => m.IdTipoEnfermedad == id);
            if (tipoEnfermedad == null)
            {
                return NotFound();
            }

            return View(tipoEnfermedad);
        }

        // GET: TipoEnfermedads/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoEnfermedads/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTipoEnfermedad,Descripcion,Activo")] TipoEnfermedad tipoEnfermedad)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipoEnfermedad);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoEnfermedad);
        }

        // GET: TipoEnfermedads/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoEnfermedad = await _context.TipoEnfermedads.FindAsync(id);
            if (tipoEnfermedad == null)
            {
                return NotFound();
            }
            return View(tipoEnfermedad);
        }

        // POST: TipoEnfermedads/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdTipoEnfermedad,Descripcion,Activo")] TipoEnfermedad tipoEnfermedad)
        {
            if (id != tipoEnfermedad.IdTipoEnfermedad)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipoEnfermedad);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipoEnfermedadExists(tipoEnfermedad.IdTipoEnfermedad))
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
            return View(tipoEnfermedad);
        }

        // GET: TipoEnfermedads/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoEnfermedad = await _context.TipoEnfermedads
                .FirstOrDefaultAsync(m => m.IdTipoEnfermedad == id);
            if (tipoEnfermedad == null)
            {
                return NotFound();
            }

            return View(tipoEnfermedad);
        }

        // POST: TipoEnfermedads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tipoEnfermedad = await _context.TipoEnfermedads.FindAsync(id);
            if (tipoEnfermedad != null)
            {
                _context.TipoEnfermedads.Remove(tipoEnfermedad);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipoEnfermedadExists(int id)
        {
            return _context.TipoEnfermedads.Any(e => e.IdTipoEnfermedad == id);
        }
    }
}
