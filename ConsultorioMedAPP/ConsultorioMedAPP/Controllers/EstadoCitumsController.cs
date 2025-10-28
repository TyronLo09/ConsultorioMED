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
    public class EstadoCitumsController : Controller
    {
        private readonly ConsultorioMedDBContext _context;

        public EstadoCitumsController(ConsultorioMedDBContext context)
        {
            _context = context;
        }

        // GET: EstadoCitums
        public async Task<IActionResult> Index()
        {
            return View(await _context.EstadoCita.ToListAsync());
        }

        // GET: EstadoCitums/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estadoCitum = await _context.EstadoCita
                .FirstOrDefaultAsync(m => m.IdEstadoCita == id);
            if (estadoCitum == null)
            {
                return NotFound();
            }

            return View(estadoCitum);
        }

        // GET: EstadoCitums/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EstadoCitums/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdEstadoCita,Descripcion,Activo")] EstadoCitum estadoCitum)
        {
            if (ModelState.IsValid)
            {
                _context.Add(estadoCitum);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(estadoCitum);
        }

        // GET: EstadoCitums/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estadoCitum = await _context.EstadoCita.FindAsync(id);
            if (estadoCitum == null)
            {
                return NotFound();
            }
            return View(estadoCitum);
        }

        // POST: EstadoCitums/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdEstadoCita,Descripcion,Activo")] EstadoCitum estadoCitum)
        {
            if (id != estadoCitum.IdEstadoCita)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(estadoCitum);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EstadoCitumExists(estadoCitum.IdEstadoCita))
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
            return View(estadoCitum);
        }

        // GET: EstadoCitums/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estadoCitum = await _context.EstadoCita
                .FirstOrDefaultAsync(m => m.IdEstadoCita == id);
            if (estadoCitum == null)
            {
                return NotFound();
            }

            return View(estadoCitum);
        }

        // POST: EstadoCitums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var estadoCitum = await _context.EstadoCita.FindAsync(id);
            if (estadoCitum != null)
            {
                _context.EstadoCita.Remove(estadoCitum);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EstadoCitumExists(int id)
        {
            return _context.EstadoCita.Any(e => e.IdEstadoCita == id);
        }
    }
}
