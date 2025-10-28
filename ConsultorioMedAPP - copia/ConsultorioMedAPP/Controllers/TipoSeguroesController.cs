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
    public class TipoSeguroesController : Controller
    {
        private readonly ConsultorioMedDBContext _context;

        public TipoSeguroesController(ConsultorioMedDBContext context)
        {
            _context = context;
        }

        // GET: TipoSeguroes
        public async Task<IActionResult> Index()
        {
            return View(await _context.TipoSeguros.ToListAsync());
        }

        // GET: TipoSeguroes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoSeguro = await _context.TipoSeguros
                .FirstOrDefaultAsync(m => m.IdTipoSeguro == id);
            if (tipoSeguro == null)
            {
                return NotFound();
            }

            return View(tipoSeguro);
        }

        // GET: TipoSeguroes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoSeguroes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTipoSeguro,Descripcion,Porcentaje,Activo")] TipoSeguro tipoSeguro)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipoSeguro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoSeguro);
        }

        // GET: TipoSeguroes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoSeguro = await _context.TipoSeguros.FindAsync(id);
            if (tipoSeguro == null)
            {
                return NotFound();
            }
            return View(tipoSeguro);
        }

        // POST: TipoSeguroes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdTipoSeguro,Descripcion,Porcentaje,Activo")] TipoSeguro tipoSeguro)
        {
            if (id != tipoSeguro.IdTipoSeguro)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipoSeguro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipoSeguroExists(tipoSeguro.IdTipoSeguro))
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
            return View(tipoSeguro);
        }

        // GET: TipoSeguroes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoSeguro = await _context.TipoSeguros
                .FirstOrDefaultAsync(m => m.IdTipoSeguro == id);
            if (tipoSeguro == null)
            {
                return NotFound();
            }

            return View(tipoSeguro);
        }

        // POST: TipoSeguroes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tipoSeguro = await _context.TipoSeguros.FindAsync(id);
            if (tipoSeguro != null)
            {
                _context.TipoSeguros.Remove(tipoSeguro);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipoSeguroExists(int id)
        {
            return _context.TipoSeguros.Any(e => e.IdTipoSeguro == id);
        }
    }
}
