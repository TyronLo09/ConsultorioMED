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
    public class DiagnosticoesController : Controller
    {
        private readonly ConsultorioMedDBContext _context;

        public DiagnosticoesController(ConsultorioMedDBContext context)
        {
            _context = context;
        }

        // GET: Diagnosticoes
        public async Task<IActionResult> Index()
        {
            var consultorioMedDBContext = _context.Diagnosticos.Include(d => d.CitaIdCitaNavigation);
            return View(await consultorioMedDBContext.ToListAsync());
        }

        // GET: Diagnosticoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var diagnostico = await _context.Diagnosticos
                .Include(d => d.CitaIdCitaNavigation)
                .FirstOrDefaultAsync(m => m.IdDiagnostico == id);
            if (diagnostico == null)
            {
                return NotFound();
            }

            return View(diagnostico);
        }

        // GET: Diagnosticoes/Create
        public IActionResult Create()
        {
            ViewData["CitaIdCita"] = new SelectList(_context.Cita, "IdCita", "IdCita");
            return View();
        }

        // POST: Diagnosticoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDiagnostico,CitaIdCita,Descripcion,CodigoDiagnostico,FechaDiagnostico,Recomendaciones,FechaCreacion,Estado")] Diagnostico diagnostico)
        {
            if (ModelState.IsValid)
            {
                _context.Add(diagnostico);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CitaIdCita"] = new SelectList(_context.Cita, "IdCita", "IdCita", diagnostico.CitaIdCita);
            return View(diagnostico);
        }

        // GET: Diagnosticoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var diagnostico = await _context.Diagnosticos.FindAsync(id);
            if (diagnostico == null)
            {
                return NotFound();
            }
            ViewData["CitaIdCita"] = new SelectList(_context.Cita, "IdCita", "IdCita", diagnostico.CitaIdCita);
            return View(diagnostico);
        }

        // POST: Diagnosticoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdDiagnostico,CitaIdCita,Descripcion,CodigoDiagnostico,FechaDiagnostico,Recomendaciones,FechaCreacion,Estado")] Diagnostico diagnostico)
        {
            if (id != diagnostico.IdDiagnostico)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(diagnostico);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiagnosticoExists(diagnostico.IdDiagnostico))
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
            ViewData["CitaIdCita"] = new SelectList(_context.Cita, "IdCita", "IdCita", diagnostico.CitaIdCita);
            return View(diagnostico);
        }

        // GET: Diagnosticoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var diagnostico = await _context.Diagnosticos
                .Include(d => d.CitaIdCitaNavigation)
                .FirstOrDefaultAsync(m => m.IdDiagnostico == id);
            if (diagnostico == null)
            {
                return NotFound();
            }

            return View(diagnostico);
        }

        // POST: Diagnosticoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var diagnostico = await _context.Diagnosticos.FindAsync(id);
            if (diagnostico != null)
            {
                _context.Diagnosticos.Remove(diagnostico);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DiagnosticoExists(int id)
        {
            return _context.Diagnosticos.Any(e => e.IdDiagnostico == id);
        }
    }
}
