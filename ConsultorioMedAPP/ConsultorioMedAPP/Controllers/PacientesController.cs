using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ConsultorioMedAPP.Models;

namespace ConsultorioMedAPP.Controllers
{
    public class PacientesController : Controller
    {
        private readonly ConsultorioMedDBContext _context;

        public PacientesController(ConsultorioMedDBContext context)
        {
            _context = context;
        }

        // --- INDEX ---
        public async Task<IActionResult> Index(string busqueda)
        {
            var query = _context.Pacientes
                .Include(p => p.IdCedulaNavigation)
                .Include(p => p.SeguroPacienteIdSeguroNavigation)
                    .ThenInclude(s => s.IdTipoSeguroNavigation)
                .Include(p => p.AntecedentesMedicos)
                    .ThenInclude(a => a.IdTipoEnfermedadNavigation)
                .AsQueryable();

            if (!string.IsNullOrEmpty(busqueda))
            {
                ViewData["BusquedaSel"] = busqueda;
                string busquedaNormalizada = busqueda.ToLower();

                query = query.Where(p =>
                    p.IdCedula.ToString().Contains(busquedaNormalizada) ||
                    p.IdCedulaNavigation.Nombre.ToLower().Contains(busquedaNormalizada) ||
                    p.IdCedulaNavigation.Apellido1.ToLower().Contains(busquedaNormalizada) ||
                    p.IdCedulaNavigation.Apellido2.ToLower().Contains(busquedaNormalizada)
                );
            }

            var pacientes = await query.OrderBy(p => p.IdCedula).ToListAsync();

            var vistaPacientes = pacientes.Select(p =>
            {
                var persona = p.IdCedulaNavigation;
                var seguro = p.SeguroPacienteIdSeguroNavigation;
                var antecedente = p.AntecedentesMedicos.FirstOrDefault();

                // Obtener el porcentaje directamente de la tabla TipoSeguro
                string tipoSeguroDescripcion = seguro?.IdTipoSeguroNavigation?.Descripcion ?? "Sin Seguro";
                decimal? porcentaje = seguro?.IdTipoSeguroNavigation?.Porcentaje;

                return new
                {
                    Cedula = persona.IdCedula,
                    Nombre = persona.Nombre,
                    Apellido1 = persona.Apellido1,
                    Apellido2 = persona.Apellido2,
                    Estado = p.Estado,
                    TipoSeguro = tipoSeguroDescripcion,
                    PorcentajeSeguro = porcentaje.HasValue ? $"{porcentaje.Value}%" : "0%",
                    TipoEnfermedadDescripcion = antecedente?.IdTipoEnfermedadNavigation?.Descripcion ?? "Sin registro",
                    EsCronico = (antecedente?.Cronico ?? false) ? "Sí" : "No",
                    AntecedentesDescripcion = antecedente?.Descripcion ?? "Sin antecedentes registrados",
                    IdSeguro = seguro?.IdSeguro
                };
            }).ToList();

            ViewBag.Pacientes = vistaPacientes;
            return View();
        }

        // --- DETAILS ---
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var paciente = await _context.Pacientes
                .Include(p => p.IdCedulaNavigation)
                .Include(p => p.SeguroPacienteIdSeguroNavigation)
                    .ThenInclude(s => s.IdTipoSeguroNavigation)
                .Include(p => p.AntecedentesMedicos)
                    .ThenInclude(a => a.IdTipoEnfermedadNavigation)
                .FirstOrDefaultAsync(m => m.IdCedula == id);

            if (paciente == null) return NotFound();

            ViewData["Antecedente"] = paciente.AntecedentesMedicos.FirstOrDefault();
            return View(paciente);
        }

        // --- CREATE GET ---
        public IActionResult Create()
        {
            RecargarSelectLists(null, null, null);
            return View();
        }

        // --- CREATE POST ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int IdCedula, int IdTipoSeguro, int IdTipoEnfermedad, string DescripcionAntecedente, bool Cronico, bool Estado)
        {
            // Validar que la Persona exista
            var persona = await _context.Personas.FindAsync(IdCedula);
            if (persona == null)
            {
                TempData["Error"] = "La Cédula seleccionada no existe en la tabla de Personas.";
                RecargarSelectLists(IdCedula, IdTipoSeguro, IdTipoEnfermedad);
                return View();
            }

            // Validar que no sea ya un Paciente
            var pacienteExiste = await _context.Pacientes.AnyAsync(p => p.IdCedula == IdCedula);
            if (pacienteExiste)
            {
                TempData["Error"] = "La Persona con esta Cédula ya está registrada como Paciente.";
                RecargarSelectLists(IdCedula, IdTipoSeguro, IdTipoEnfermedad);
                return View();
            }

            var strategy = _context.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                using var trans = await _context.Database.BeginTransactionAsync();
                try
                {
                    // Crear el Seguro
                    var seguro = new Seguro
                    {
                        IdTipoSeguro = IdTipoSeguro,
                        IdCedula = IdCedula,
                        FechaCreacion = DateTime.Now,
                        Activo = true
                    };
                    _context.Seguros.Add(seguro);
                    await _context.SaveChangesAsync();

                    // Crear el Paciente
                    var paciente = new Paciente
                    {
                        IdCedula = IdCedula,
                        SeguroPacienteIdSeguro = seguro.IdSeguro,
                        FechaRegistro = DateTime.Now,
                        Estado = Estado
                    };
                    _context.Pacientes.Add(paciente);

                    // Crear el Antecedente
                    var antecedente = new AntecedentesMedico
                    {
                        IdCedula = IdCedula,
                        IdTipoEnfermedad = IdTipoEnfermedad,
                        Descripcion = string.IsNullOrWhiteSpace(DescripcionAntecedente) ? "Sin descripción" : DescripcionAntecedente,
                        Cronico = Cronico,
                        Activo = true
                    };
                    _context.AntecedentesMedicos.Add(antecedente);

                    await _context.SaveChangesAsync();
                    await trans.CommitAsync();
                }
                catch (Exception ex)
                {
                    await trans.RollbackAsync();
                    throw;
                }
            });

            TempData["Success"] = "Paciente creado exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        // --- EDIT GET ---
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var paciente = await _context.Pacientes
                .Include(p => p.IdCedulaNavigation)
                .Include(p => p.SeguroPacienteIdSeguroNavigation)
                    .ThenInclude(s => s.IdTipoSeguroNavigation)
                .Include(p => p.AntecedentesMedicos)
                    .ThenInclude(a => a.IdTipoEnfermedadNavigation)
                .FirstOrDefaultAsync(p => p.IdCedula == id);

            if (paciente == null) return NotFound();

            var antecedente = paciente.AntecedentesMedicos.FirstOrDefault();
            var seguro = paciente.SeguroPacienteIdSeguroNavigation;
            var persona = paciente.IdCedulaNavigation;

            RecargarSelectLists(paciente.IdCedula, seguro?.IdTipoSeguro, antecedente?.IdTipoEnfermedad);

            ViewData["Antecedente"] = antecedente;
            ViewData["Persona"] = persona;
            ViewData["SeguroId"] = seguro?.IdSeguro;
            ViewData["EstadoActual"] = paciente.Estado;

            return View(paciente);
        }

        // --- EDIT POST ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int IdTipoSeguro, int IdTipoEnfermedad, string DescripcionAntecedente, bool Cronico, bool Estado, int IdSeguro)
        {
            var strategy = _context.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                using var trans = await _context.Database.BeginTransactionAsync();
                try
                {
                    // Actualizar Paciente
                    var pacienteExistente = await _context.Pacientes.FindAsync(id);
                    if (pacienteExistente != null)
                    {
                        pacienteExistente.Estado = Estado;
                        _context.Pacientes.Update(pacienteExistente);
                    }

                    // Actualizar Seguro
                    var seguroExistente = await _context.Seguros.FindAsync(IdSeguro);
                    if (seguroExistente != null)
                    {
                        seguroExistente.IdTipoSeguro = IdTipoSeguro;
                        _context.Seguros.Update(seguroExistente);
                    }

                    // Actualizar Antecedente
                    var antecedenteExistente = await _context.AntecedentesMedicos
                        .FirstOrDefaultAsync(a => a.IdCedula == id);

                    if (antecedenteExistente != null)
                    {
                        antecedenteExistente.IdTipoEnfermedad = IdTipoEnfermedad;
                        antecedenteExistente.Descripcion = string.IsNullOrWhiteSpace(DescripcionAntecedente) ? "Sin descripción" : DescripcionAntecedente;
                        antecedenteExistente.Cronico = Cronico;
                        _context.AntecedentesMedicos.Update(antecedenteExistente);
                    }
                    else
                    {
                        // Si no existe, crear uno nuevo
                        var nuevoAntecedente = new AntecedentesMedico
                        {
                            IdCedula = id,
                            IdTipoEnfermedad = IdTipoEnfermedad,
                            Descripcion = string.IsNullOrWhiteSpace(DescripcionAntecedente) ? "Sin descripción" : DescripcionAntecedente,
                            Cronico = Cronico,
                            Activo = true
                        };
                        _context.AntecedentesMedicos.Add(nuevoAntecedente);
                    }

                    await _context.SaveChangesAsync();
                    await trans.CommitAsync();
                }
                catch (Exception ex)
                {
                    await trans.RollbackAsync();
                    throw;
                }
            });

            TempData["Success"] = "Paciente actualizado exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        // --- DELETE GET ---
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var paciente = await _context.Pacientes
                .Include(p => p.IdCedulaNavigation)
                .Include(p => p.SeguroPacienteIdSeguroNavigation)
                    .ThenInclude(s => s.IdTipoSeguroNavigation)
                .FirstOrDefaultAsync(m => m.IdCedula == id);

            if (paciente == null) return NotFound();
            return View(paciente);
        }

        // --- DELETE POST ---
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var strategy = _context.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                using var trans = await _context.Database.BeginTransactionAsync();
                try
                {
                    var paciente = await _context.Pacientes
                        .Include(p => p.AntecedentesMedicos)
                        .FirstOrDefaultAsync(p => p.IdCedula == id);

                    if (paciente != null)
                    {
                        var seguroId = paciente.SeguroPacienteIdSeguro;

                        // Eliminar Antecedentes
                        if (paciente.AntecedentesMedicos.Any())
                        {
                            _context.AntecedentesMedicos.RemoveRange(paciente.AntecedentesMedicos);
                        }

                        // Eliminar Paciente
                        _context.Pacientes.Remove(paciente);

                        // Eliminar Seguro
                        if (seguroId.HasValue)
                        {
                            var seguro = await _context.Seguros.FindAsync(seguroId.Value);
                            if (seguro != null)
                            {
                                _context.Seguros.Remove(seguro);
                            }
                        }

                        await _context.SaveChangesAsync();
                        await trans.CommitAsync();
                    }
                }
                catch (Exception ex)
                {
                    await trans.RollbackAsync();
                    throw;
                }
            });

            TempData["Success"] = "Paciente eliminado exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        // --- MÉTODO AUXILIAR ---
        private void RecargarSelectLists(int? idCedulaSeleccionada, int? idTipoSeguroSeleccionado, int? idTipoEnfermedadSeleccionado)
        {
            var personasNoPacientes = _context.Personas
                .Where(p => !_context.Pacientes.Any(pa => pa.IdCedula == p.IdCedula))
                .OrderBy(p => p.IdCedula)
                .Select(p => new
                {
                    p.IdCedula,
                    NombreCompleto = $"{p.IdCedula} - {p.Nombre} {p.Apellido1}"
                });

            ViewData["IdCedula"] = new SelectList(personasNoPacientes.AsEnumerable(), "IdCedula", "NombreCompleto", idCedulaSeleccionada);

            ViewData["IdTipoSeguro"] = new SelectList(
                _context.TipoSeguros.OrderBy(t => t.Descripcion).ToList(),
                "IdTipoSeguro",
                "Descripcion",
                idTipoSeguroSeleccionado
            );

            ViewData["IdTipoEnfermedad"] = new SelectList(
                _context.TipoEnfermedads.OrderBy(t => t.Descripcion).ToList(),
                "IdTipoEnfermedad",
                "Descripcion",
                idTipoEnfermedadSeleccionado
            );
        }

        // --- AJAX ---
        [HttpGet]
        public async Task<IActionResult> GetPersonaData(int idCedula)
        {
            var persona = await _context.Personas.FirstOrDefaultAsync(p => p.IdCedula == idCedula);

            if (persona == null)
            {
                return NotFound();
            }

            return Json(new
            {
                nombre = persona.Nombre,
                apellido1 = persona.Apellido1,
                apellido2 = persona.Apellido2,
                fechaNacimiento = persona.FechaNacimiento.ToString("dd/MM/yyyy")
            });
        }
    }
}