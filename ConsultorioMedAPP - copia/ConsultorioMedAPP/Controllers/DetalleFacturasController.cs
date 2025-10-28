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
    public class DetalleFacturasController : Controller
    {
        private readonly ConsultorioMedDBContext _context;

        public DetalleFacturasController(ConsultorioMedDBContext context)
        {
            _context = context;
        }

        // GET: DetalleFacturas
        public async Task<IActionResult> Index()
        {
            var consultorioMedDBContext = _context.DetalleFacturas.Include(d => d.FacturaIdFacturaNavigation).Include(d => d.IdCitaCitaNavigation);
            return View(await consultorioMedDBContext.ToListAsync());
        }

        // GET: DetalleFacturas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalleFactura = await _context.DetalleFacturas
                .Include(d => d.FacturaIdFacturaNavigation)
                .Include(d => d.IdCitaCitaNavigation)
                .FirstOrDefaultAsync(m => m.IdDetalleFactura == id);
            if (detalleFactura == null)
            {
                return NotFound();
            }

            return View(detalleFactura);
        }

        // GET: DetalleFacturas/Create
        public IActionResult Create()
        {
            ViewData["FacturaIdFactura"] = new SelectList(_context.Facturas, "IdFactura", "IdFactura");
            ViewData["IdCitaCita"] = new SelectList(_context.Cita, "IdCita", "IdCita");
            return View();
        }

        // POST: DetalleFacturas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDetalleFactura,FacturaIdFactura,Descripcion,IdCitaCita,Subtotal,Activo")] DetalleFactura detalleFactura)
        {
            if (ModelState.IsValid)
            {
                _context.Add(detalleFactura);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FacturaIdFactura"] = new SelectList(_context.Facturas, "IdFactura", "IdFactura", detalleFactura.FacturaIdFactura);
            ViewData["IdCitaCita"] = new SelectList(_context.Cita, "IdCita", "IdCita", detalleFactura.IdCitaCita);
            return View(detalleFactura);
        }

        // GET: DetalleFacturas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalleFactura = await _context.DetalleFacturas.FindAsync(id);
            if (detalleFactura == null)
            {
                return NotFound();
            }
            ViewData["FacturaIdFactura"] = new SelectList(_context.Facturas, "IdFactura", "IdFactura", detalleFactura.FacturaIdFactura);
            ViewData["IdCitaCita"] = new SelectList(_context.Cita, "IdCita", "IdCita", detalleFactura.IdCitaCita);
            return View(detalleFactura);
        }

        // POST: DetalleFacturas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdDetalleFactura,FacturaIdFactura,Descripcion,IdCitaCita,Subtotal,Activo")] DetalleFactura detalleFactura)
        {
            if (id != detalleFactura.IdDetalleFactura)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(detalleFactura);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DetalleFacturaExists(detalleFactura.IdDetalleFactura))
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
            ViewData["FacturaIdFactura"] = new SelectList(_context.Facturas, "IdFactura", "IdFactura", detalleFactura.FacturaIdFactura);
            ViewData["IdCitaCita"] = new SelectList(_context.Cita, "IdCita", "IdCita", detalleFactura.IdCitaCita);
            return View(detalleFactura);
        }

        // GET: DetalleFacturas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalleFactura = await _context.DetalleFacturas
                .Include(d => d.FacturaIdFacturaNavigation)
                .Include(d => d.IdCitaCitaNavigation)
                .FirstOrDefaultAsync(m => m.IdDetalleFactura == id);
            if (detalleFactura == null)
            {
                return NotFound();
            }

            return View(detalleFactura);
        }

        // POST: DetalleFacturas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var detalleFactura = await _context.DetalleFacturas.FindAsync(id);
            if (detalleFactura != null)
            {
                _context.DetalleFacturas.Remove(detalleFactura);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DetalleFacturaExists(int id)
        {
            return _context.DetalleFacturas.Any(e => e.IdDetalleFactura == id);
        }
    }
}
