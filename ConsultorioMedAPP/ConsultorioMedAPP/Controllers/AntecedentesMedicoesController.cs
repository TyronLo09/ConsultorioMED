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
    public class AntecedentesMedicoesController : Controller
    {
        private readonly ConsultorioMedDBContext _context;

        public AntecedentesMedicoesController(ConsultorioMedDBContext context)
        {
            _context = context;
        }

        // GET: AntecedentesMedicoes
        public async Task<IActionResult> Index()
        {
            var consultorioMedDBContext = _context.AntecedentesMedicos.Include(a => a.IdCedulaNavigation).Include(a => a.IdTipoEnfermedadNavigation);
            return View(await consultorioMedDBContext.ToListAsync());
        }

        // GET: AntecedentesMedicoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var antecedentesMedico = await _context.AntecedentesMedicos
                .Include(a => a.IdCedulaNavigation)
                .Include(a => a.IdTipoEnfermedadNavigation)
                .FirstOrDefaultAsync(m => m.IdAntecedentesMedicos == id);
            if (antecedentesMedico == null)
            {
                return NotFound();
            }

            return View(antecedentesMedico);
        }

        // GET: AntecedentesMedicoes/Create
        public IActionResult Create()
        {
            ViewData["IdCedula"] = new SelectList(_context.Pacientes, "IdCedula", "IdCedula");
            ViewData["IdTipoEnfermedad"] = new SelectList(_context.TipoEnfermedads, "IdTipoEnfermedad", "IdTipoEnfermedad");
            return View();
        }

        // POST: AntecedentesMedicoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AntecedentesMedico antecedentesMedico)
        {
            try
            {
                if (antecedentesMedico != null)
                {
                    _context.Add(antecedentesMedico);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewData["Error"] = "Los datos del antecedente médico no pueden estar vacíos";
                }
            }
            catch (Exception ex)
            {
                ViewData["Error"] = "Error al crear el antecedente médico: " + ex.Message;
            }

            ViewData["IdCedula"] = new SelectList(_context.Pacientes, "IdCedula", "IdCedula", antecedentesMedico?.IdCedula);
            ViewData["IdTipoEnfermedad"] = new SelectList(_context.TipoEnfermedads, "IdTipoEnfermedad", "IdTipoEnfermedad", antecedentesMedico?.IdTipoEnfermedad);
            return View(antecedentesMedico);
        }

        // GET: AntecedentesMedicoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var antecedentesMedico = await _context.AntecedentesMedicos.FindAsync(id);
            if (antecedentesMedico == null)
            {
                return NotFound();
            }
            ViewData["IdCedula"] = new SelectList(_context.Pacientes, "IdCedula", "IdCedula", antecedentesMedico.IdCedula);
            ViewData["IdTipoEnfermedad"] = new SelectList(_context.TipoEnfermedads, "IdTipoEnfermedad", "IdTipoEnfermedad", antecedentesMedico.IdTipoEnfermedad);
            return View(antecedentesMedico);
        }

        // POST: AntecedentesMedicoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AntecedentesMedico antecedentesMedico)
        {
            if (id != antecedentesMedico.IdAntecedentesMedicos)
            {
                return NotFound();
            }

            try
            {
                // Cargar la entidad existente
                var antecedenteExistente = await _context.AntecedentesMedicos.FindAsync(id);
                if (antecedenteExistente == null)
                {
                    return NotFound();
                }

                // Actualizar solo las propiedades necesarias
                antecedenteExistente.IdTipoEnfermedad = antecedentesMedico.IdTipoEnfermedad;
                antecedenteExistente.IdCedula = antecedentesMedico.IdCedula;
                antecedenteExistente.Descripcion = antecedentesMedico.Descripcion;
                antecedenteExistente.Cronico = antecedentesMedico.Cronico;
                antecedenteExistente.Activo = antecedentesMedico.Activo;

                // Guardar cambios
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Verificar directamente si el antecedente médico existe
                var existeAntecedente = await _context.AntecedentesMedicos.AnyAsync(a => a.IdAntecedentesMedicos == antecedentesMedico.IdAntecedentesMedicos);
                if (!existeAntecedente)
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
                ViewData["Error"] = "Error al actualizar el antecedente médico: " + ex.Message;
            }

            ViewData["IdCedula"] = new SelectList(_context.Pacientes, "IdCedula", "IdCedula", antecedentesMedico.IdCedula);
            ViewData["IdTipoEnfermedad"] = new SelectList(_context.TipoEnfermedads, "IdTipoEnfermedad", "IdTipoEnfermedad", antecedentesMedico.IdTipoEnfermedad);
            return View(antecedentesMedico);
        }
        // GET: AntecedentesMedicoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var antecedentesMedico = await _context.AntecedentesMedicos
                .Include(a => a.IdCedulaNavigation)
                .Include(a => a.IdTipoEnfermedadNavigation)
                .FirstOrDefaultAsync(m => m.IdAntecedentesMedicos == id);
            if (antecedentesMedico == null)
            {
                return NotFound();
            }

            return View(antecedentesMedico);
        }

        // POST: AntecedentesMedicoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var antecedentesMedico = await _context.AntecedentesMedicos.FindAsync(id);
            if (antecedentesMedico != null)
            {
                _context.AntecedentesMedicos.Remove(antecedentesMedico);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AntecedentesMedicoExists(int id)
        {
            return _context.AntecedentesMedicos.Any(e => e.IdAntecedentesMedicos == id);
        }
    }
}
