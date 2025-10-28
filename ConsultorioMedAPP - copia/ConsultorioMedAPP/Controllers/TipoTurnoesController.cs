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
    public class TipoTurnoesController : Controller
    {
        private readonly ConsultorioMedDBContext _context;

        public TipoTurnoesController(ConsultorioMedDBContext context)
        {
            _context = context;
        }

        // GET: TipoTurnoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.TipoTurnos.ToListAsync());
        }

        // GET: TipoTurnoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoTurno = await _context.TipoTurnos
                .FirstOrDefaultAsync(m => m.IdTipoTurno == id);
            if (tipoTurno == null)
            {
                return NotFound();
            }

            return View(tipoTurno);
        }

        // GET: TipoTurnoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoTurnoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTipoTurno,Descripcion,HoraEntrada,HoraSalida")] TipoTurno tipoTurno)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipoTurno);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoTurno);
        }

        // GET: TipoTurnoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoTurno = await _context.TipoTurnos.FindAsync(id);
            if (tipoTurno == null)
            {
                return NotFound();
            }
            return View(tipoTurno);
        }

        // POST: TipoTurnoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdTipoTurno,Descripcion,HoraEntrada,HoraSalida")] TipoTurno tipoTurno)
        {
            if (id != tipoTurno.IdTipoTurno)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipoTurno);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipoTurnoExists(tipoTurno.IdTipoTurno))
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
            return View(tipoTurno);
        }

        // GET: TipoTurnoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoTurno = await _context.TipoTurnos
                .FirstOrDefaultAsync(m => m.IdTipoTurno == id);
            if (tipoTurno == null)
            {
                return NotFound();
            }

            return View(tipoTurno);
        }

        // POST: TipoTurnoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tipoTurno = await _context.TipoTurnos.FindAsync(id);
            if (tipoTurno != null)
            {
                _context.TipoTurnos.Remove(tipoTurno);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipoTurnoExists(int id)
        {
            return _context.TipoTurnos.Any(e => e.IdTipoTurno == id);
        }
    }
}
