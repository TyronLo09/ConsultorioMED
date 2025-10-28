using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ConsultorioMedAPP.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ConsultorioMedAPP.Controllers
{
    public class PersonasController : Controller
    {
        private readonly ConsultorioMedDBContext _context;

        public PersonasController(ConsultorioMedDBContext context)
        {
            _context = context;
        }

        // GET: Personas
        public async Task<IActionResult> Index()
        {
            var consultorioMedDBContext = _context.Personas
                .Include(p => p.IdGeneroNavigation)
                .Include(p => p.Correos)
                .Include(p => p.Telefonos);

            return View(await consultorioMedDBContext.ToListAsync());
        }

        // GET: Personas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var persona = await _context.Personas
                .Include(p => p.IdGeneroNavigation)
                .FirstOrDefaultAsync(m => m.IdCedula == id);
            if (persona == null)
            {
                return NotFound();
            }

            return View(persona);
        }

        // GET: Personas/Create
        public IActionResult Create()
        {
            ViewData["IdGenero"] = new SelectList(_context.Generos, "IdGenero", "IdGenero");
            return View();
        }

        // POST: Personas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCedula,Nombre,Apellido1,Apellido2,FechaNacimiento,IdGenero,Activo")] Persona persona)
        {
            try {
                if(persona != null)
             _context.Add(persona);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch {
                Exception ex;
            }
            
            ViewData["IdGenero"] = new SelectList(_context.Generos, "IdGenero", "IdGenero", persona.IdGenero);
            return View(persona);
        }

        // GET: Personas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var persona = await _context.Personas.FindAsync(id);
            if (persona == null)
            {
                return NotFound();
            }
            ViewData["IdGenero"] = new SelectList(_context.Generos, "IdGenero", "IdGenero", persona.IdGenero);
            return View(persona);
        }

        // POST: Personas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: Personas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCedula,Nombre,Apellido1,Apellido2,FechaNacimiento,IdGenero,Activo")] Persona persona)
        {
            if (id != persona.IdCedula)
            {
                return NotFound();
            }

            try
            {
                // Cargar la entidad existente
                var personaExistente = await _context.Personas.FindAsync(id);
                if (personaExistente == null)
                {
                    return NotFound();
                }

                // Validación manual básica
                if (string.IsNullOrWhiteSpace(persona.Nombre))
                {
                    ViewData["Error"] = "El nombre es requerido";
                    ViewData["IdGenero"] = new SelectList(_context.Generos, "IdGenero", "IdGenero", persona.IdGenero);
                    return View(persona);
                }

                // Actualizar propiedades (manteniendo la fecha de creación original)
                personaExistente.Nombre = persona.Nombre;
                personaExistente.Apellido1 = persona.Apellido1;
                personaExistente.Apellido2 = persona.Apellido2;
                personaExistente.FechaNacimiento = persona.FechaNacimiento;
                personaExistente.IdGenero = persona.IdGenero;
                personaExistente.Activo = persona.Activo;

                // La fecha de creación se mantiene igual (no se modifica)
                // Si necesitas fecha de modificación, agrega una propiedad FechaModificacion
                // personaExistente.FechaModificacion = DateTime.Now;

                // Guardar cambios
                int filasAfectadas = await _context.SaveChangesAsync();

                if (filasAfectadas > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewData["Error"] = "No se realizaron cambios en la base de datos";
                    ViewData["IdGenero"] = new SelectList(_context.Generos, "IdGenero", "IdGenero", persona.IdGenero);
                    return View(persona);
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!PersonaExists(persona.IdCedula))
                {
                    return NotFound();
                }
                else
                {
                    ViewData["Error"] = "Error de concurrencia: " + ex.Message;
                    ViewData["IdGenero"] = new SelectList(_context.Generos, "IdGenero", "IdGenero", persona.IdGenero);
                    return View(persona);
                }
            }
            catch (Exception ex)
            {
                ViewData["Error"] = "Error al actualizar: " + ex.Message;
                ViewData["IdGenero"] = new SelectList(_context.Generos, "IdGenero", "IdGenero", persona.IdGenero);
                return View(persona);
            }
        }
        // GET: Personas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var persona = await _context.Personas
                .Include(p => p.IdGeneroNavigation)
                .FirstOrDefaultAsync(m => m.IdCedula == id);
            if (persona == null)
            {
                return NotFound();
            }

            return View(persona);
        }

        // POST: Personas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var persona = await _context.Personas.FindAsync(id);
            if (persona != null)
            {
                _context.Personas.Remove(persona);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonaExists(int id)
        {
            return _context.Personas.Any(e => e.IdCedula == id);
        }
    }
}
