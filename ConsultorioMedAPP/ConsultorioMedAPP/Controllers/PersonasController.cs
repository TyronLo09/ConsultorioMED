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

        // Helper: formatea cédula 9 dígitos => 1-1836-0977
        private string FormatCedula(int cedula)
        {
            var s = cedula.ToString().PadLeft(9, '0');
            return $"{s[0]}-{s.Substring(1, 4)}-{s.Substring(5, 4)}";
        }

        // GET: Personas
        public async Task<IActionResult> Index(string genero, int? cantidad)
        {
            // Cargar datos base
            var personas = _context.Personas
                .Include(p => p.IdGeneroNavigation)
                .Include(p => p.Correos)
                .Include(p => p.Telefonos)
                .AsQueryable();

            // ✅ Filtro por género (solo si se selecciona uno)
            if (!string.IsNullOrEmpty(genero))
            {
                personas = personas.Where(p => p.IdGeneroNavigation.Descripcion == genero);
            }

            // ✅ Filtro por cantidad (mostrar solo n personas)
            if (cantidad.HasValue && cantidad > 0)
            {
                personas = personas.Take(cantidad.Value);
            }

            // Obtener lista final
            var lista = await personas.ToListAsync();

            // ✅ Enviar valores actuales del filtro a la vista (para mantener el estado)
            ViewBag.GeneroSeleccionado = genero;
            ViewBag.CantidadSeleccionada = cantidad;

            return View(lista);
        }

        // GET: Personas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var persona = await _context.Personas
                .Include(p => p.IdGeneroNavigation)
                .Include(p => p.Correos)
                .Include(p => p.Telefonos)
                .FirstOrDefaultAsync(m => m.IdCedula == id);

            if (persona == null) return NotFound();

            ViewBag.FormattedCedula = FormatCedula(persona.IdCedula);

            return View(persona);
        }

        // GET: Personas/Create
        public IActionResult Create()
        {
            ViewBag.IdGenero = new SelectList(_context.Generos.Where(g => g.Activo == true), "IdGenero", "Descripcion");
            ViewBag.IdTipoCorreo = new SelectList(_context.TipoCorreos.Where(t => t.Activo == true), "IdTipoCorreo", "Descripcion");
            ViewBag.IdTipoTelefono = new SelectList(_context.TipoTelefonos.Where(t => t.Activo == true), "IdTipoTelefono", "Descripcion");

            return View(); // <-- asegúrate de que sea exactamente esto
        }



        // POST: Personas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
     int IdCedula, string Nombre, string Apellido1, string Apellido2,
     DateOnly FechaNacimiento, int IdGenero, bool Activo,
     int? TipoCorreo, string DirecCorreo,
     int? TipoTelefono, string Telefono)
        {
            try
            {
                // 🔹 Validaciones manuales
                string error = string.Empty;

                // 1) cédula 9 dígitos
                if (IdCedula.ToString().Length != 9)
                    error += "La cédula debe tener exactamente 9 dígitos. ";

                // 2) fecha nacimiento no futura
                var hoy = DateOnly.FromDateTime(DateTime.Today);
                if (FechaNacimiento > hoy)
                    error += "La fecha de nacimiento no puede ser futura. ";

                // 3) no duplicados de cédula
                bool existe = await _context.Personas.AnyAsync(p => p.IdCedula == IdCedula);
                if (existe)
                    error += "Ya existe una persona con esa cédula. ";

                // Si hay errores, se muestran directamente
                if (!string.IsNullOrEmpty(error))
                {
                    ViewBag.Error = error;
                    ViewData["IdGenero"] = new SelectList(_context.Generos.Where(g => g.Activo == true), "IdGenero", "Descripcion", IdGenero);
                    ViewData["IdTipoCorreo"] = new SelectList(_context.TipoCorreos.Where(t => t.Activo == true), "IdTipoCorreo", "Descripcion");
                    ViewData["IdTipoTelefono"] = new SelectList(_context.TipoTelefonos.Where(t => t.Activo == true), "IdTipoTelefono", "Descripcion");
                    return View();
                }

                // 🔹 Crear objeto Persona manualmente
                var persona = new Persona
                {
                    IdCedula = IdCedula,
                    Nombre = Nombre?.Trim(),
                    Apellido1 = Apellido1?.Trim(),
                    Apellido2 = Apellido2?.Trim(),
                    FechaNacimiento = FechaNacimiento,
                    IdGenero = IdGenero,
                    Activo = Activo
                };

                // 🔹 Asociar correo (si se proporcionó)
                if (!string.IsNullOrWhiteSpace(DirecCorreo) && TipoCorreo.HasValue)
                {
                    persona.Correos = new List<Correo>
            {
                new Correo
                {
                    DirecCorreo = DirecCorreo.Trim(),
                    IdTipoCorreo = TipoCorreo.Value,
                    Activo = true
                }
            };
                }

                // 🔹 Asociar teléfono (si se proporcionó)
                if (!string.IsNullOrWhiteSpace(Telefono) && TipoTelefono.HasValue)
                {
                    persona.Telefonos = new List<Telefono>
            {
                new Telefono
                {
                    Numero = Telefono.Trim(),
                    IdTipoTelefono = TipoTelefono.Value,
                    Activo = true
                }
            };
                }

                // 🔹 Guardar todo
                _context.Personas.Add(persona);
                await _context.SaveChangesAsync();

                TempData["Mensaje"] = "✅ Persona creada correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // 🔹 Captura de error
                ViewBag.Error = "❌ Error al crear la persona: " + ex.Message;

                // recargar combos si hay fallo
                ViewData["IdGenero"] = new SelectList(_context.Generos.Where(g => g.Activo == true), "IdGenero", "Descripcion");
                ViewData["IdTipoCorreo"] = new SelectList(_context.TipoCorreos.Where(t => t.Activo == true), "IdTipoCorreo", "Descripcion");
                ViewData["IdTipoTelefono"] = new SelectList(_context.TipoTelefonos.Where(t => t.Activo == true), "IdTipoTelefono", "Descripcion");

                return View();
            }
        }


        // GET: Personas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var persona = await _context.Personas
                .Include(p => p.Correos)
                .Include(p => p.Telefonos)
                .FirstOrDefaultAsync(p => p.IdCedula == id);

            if (persona == null) return NotFound();

            ViewData["IdGenero"] = new SelectList(_context.Generos.Where(g => g.Activo == true), "IdGenero", "Descripcion", persona.IdGenero);
            ViewData["IdTipoCorreo"] = new SelectList(_context.TipoCorreos.Where(t => t.Activo == true), "IdTipoCorreo", "Descripcion");
            ViewData["IdTipoTelefono"] = new SelectList(_context.TipoTelefonos.Where(t => t.Activo == true), "IdTipoTelefono", "Descripcion");

            // tomar el primer correo/telefono si existen para mostrar en inputs
            ViewBag.PrimerCorreo = persona.Correos.FirstOrDefault()?.DirecCorreo ?? string.Empty;
            ViewBag.PrimerTipoCorreo = persona.Correos.FirstOrDefault()?.IdTipoCorreo ?? (int?)null;
            ViewBag.PrimerTelefono = persona.Telefonos.FirstOrDefault()?.Numero ?? string.Empty;
            ViewBag.PrimerTipoTelefono = persona.Telefonos.FirstOrDefault()?.IdTipoTelefono ?? (int?)null;

            return View(persona);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IFormCollection form)
        {
            try
            {
                // 🔹 Buscar persona existente
                var personaExistente = await _context.Personas.FindAsync(id);
                if (personaExistente == null)
                    return NotFound();

                // 🔹 Obtener datos manualmente del formulario
                string nombre = form["Nombre"];
                string apellido1 = form["Apellido1"];
                string apellido2 = form["Apellido2"];
                string fechaNacStr = form["FechaNacimiento"];
                string idGeneroStr = form["IdGenero"];
                string activoStr = form["Activo"];
                string direcCorreo = form["DirecCorreo"];
                string tipoCorreoStr = form["TipoCorreo"];
                string telefono = form["Telefono"];
                string tipoTelefonoStr = form["TipoTelefono"];

                // 🔹 Conversión segura de tipos
                DateOnly fechaNacimiento = DateOnly.Parse(fechaNacStr);
                int idGenero = int.Parse(idGeneroStr);
                bool activo = !string.IsNullOrEmpty(activoStr) && activoStr.Contains("true");

                int? tipoCorreo = !string.IsNullOrEmpty(tipoCorreoStr) ? int.Parse(tipoCorreoStr) : (int?)null;
                int? tipoTelefono = !string.IsNullOrEmpty(tipoTelefonoStr) ? int.Parse(tipoTelefonoStr) : (int?)null;

                // 🔹 Validaciones simples
                if (fechaNacimiento > DateOnly.FromDateTime(DateTime.Today))
                    throw new Exception("La fecha de nacimiento no puede ser futura.");

                // 🔹 Actualizar persona
                personaExistente.Nombre = nombre;
                personaExistente.Apellido1 = apellido1;
                personaExistente.Apellido2 = apellido2;
                personaExistente.FechaNacimiento = fechaNacimiento;
                personaExistente.IdGenero = idGenero;
                personaExistente.Activo = activo;

                // 🔹 Actualizar o crear correo
                var correoExistente = await _context.Correos.FirstOrDefaultAsync(c => c.IdCedula == id);
                if (!string.IsNullOrWhiteSpace(direcCorreo) && tipoCorreo.HasValue)
                {
                    if (correoExistente != null)
                    {
                        correoExistente.DirecCorreo = direcCorreo.Trim();
                        correoExistente.IdTipoCorreo = tipoCorreo.Value;
                    }
                    else
                    {
                        var nuevoCorreo = new Correo
                        {
                            DirecCorreo = direcCorreo.Trim(),
                            IdTipoCorreo = tipoCorreo.Value,
                            IdCedula = personaExistente.IdCedula,
                            Activo = true
                        };
                        _context.Correos.Add(nuevoCorreo);
                    }
                }

                // 🔹 Actualizar o crear teléfono
                var telExistente = await _context.Telefonos.FirstOrDefaultAsync(t => t.IdCedula == id);
                if (!string.IsNullOrWhiteSpace(telefono) && tipoTelefono.HasValue)
                {
                    if (telExistente != null)
                    {
                        telExistente.Numero = telefono.Trim();
                        telExistente.IdTipoTelefono = tipoTelefono.Value;
                    }
                    else
                    {
                        var nuevoTel = new Telefono
                        {
                            Numero = telefono.Trim(),
                            IdTipoTelefono = tipoTelefono.Value,
                            IdCedula = personaExistente.IdCedula,
                            Activo = true
                        };
                        _context.Telefonos.Add(nuevoTel);
                    }
                }

                // 🔹 Guardar cambios
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // 🔹 Manejo de error: mostrar mensaje y recargar combos
                ViewBag.ErrorMensaje = ex.Message;
                ViewData["IdGenero"] = new SelectList(_context.Generos.Where(g => g.Activo == true), "IdGenero", "Descripcion");
                ViewData["IdTipoCorreo"] = new SelectList(_context.TipoCorreos.Where(t => t.Activo == true), "IdTipoCorreo", "Descripcion");
                ViewData["IdTipoTelefono"] = new SelectList(_context.TipoTelefonos.Where(t => t.Activo == true), "IdTipoTelefono", "Descripcion");

                // 🔹 Cargar nuevamente los datos existentes de la persona
                var persona = await _context.Personas
                    .Include(p => p.Correos)
                    .Include(p => p.Telefonos)
                    .FirstOrDefaultAsync(p => p.IdCedula == id);

                return View(persona);
            }
        }

        // GET: Personas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var persona = await _context.Personas
                .Include(p => p.IdGeneroNavigation)
                .FirstOrDefaultAsync(m => m.IdCedula == id);

            if (persona == null) return NotFound();

            ViewBag.FormattedCedula = FormatCedula(persona.IdCedula);
            return View(persona);
        }

        // POST: Personas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var persona = await _context.Personas
                .Include(p => p.Correos)
                .Include(p => p.Telefonos)
                .FirstOrDefaultAsync(p => p.IdCedula == id);

            if (persona != null)
            {
                // eliminar correos y teléfonos asociados (si no existe cascade)
                if (persona.Correos != null)
                    _context.Correos.RemoveRange(persona.Correos);
                if (persona.Telefonos != null)
                    _context.Telefonos.RemoveRange(persona.Telefonos);

                _context.Personas.Remove(persona);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool PersonaExists(int id)
        {
            return _context.Personas.Any(e => e.IdCedula == id);
        }
    }
}
