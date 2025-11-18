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

        // ----------------------------------------------------------------------------------
        // --- READ: INDEX (Listado de Pacientes con Filtro y Límite) 
        // ----------------------------------------------------------------------------------
        public async Task<IActionResult> Index(string busqueda, int? cantidad)
        {
            // Establecer un límite por defecto si no se proporciona o es inválido
            int limite = (cantidad.HasValue && cantidad.Value > 0) ? cantidad.Value : 50;

            // Guardamos los valores en ViewData para mantenerlos en el formulario
            ViewData["BusquedaSel"] = busqueda;
            ViewData["CantidadSel"] = limite;

            // 1. INICIAR LA CONSULTA (IQueryable)
            var query = _context.Pacientes
                .Include(p => p.IdCedulaNavigation)
                .Include(p => p.SeguroPacienteIdSeguroNavigation)
                    .ThenInclude(s => s.IdTipoSeguroNavigation)
                .Include(p => p.AntecedentesMedicos)
                    .ThenInclude(a => a.IdTipoEnfermedadNavigation)
                .OrderBy(p => p.IdCedula)
                .AsQueryable();

            // 2. APLICAR EL FILTRO DE BÚSQUEDA
            if (!string.IsNullOrEmpty(busqueda))
            {
                string busquedaNormalizada = busqueda.ToLower();

                query = query.Where(p =>
                    // Búsqueda por Cédula (convertida a string)
                    p.IdCedula.ToString().Contains(busquedaNormalizada) ||
                    // Búsqueda por Nombre, Apellido1 o Apellido2
                    p.IdCedulaNavigation.Nombre.ToLower().Contains(busquedaNormalizada) ||
                    p.IdCedulaNavigation.Apellido1.ToLower().Contains(busquedaNormalizada) ||
                    p.IdCedulaNavigation.Apellido2.ToLower().Contains(busquedaNormalizada)
                );
            }

            // 3. APLICAR EL LÍMITE DE CANTIDAD A MOSTRAR
            query = query.Take(limite);


            // 4. EJECUTAR LA CONSULTA
            var pacientes = await query.ToListAsync();

            // 5. Lógica para crear el modelo de vista unificada (modelo anónimo)
            var vistaPacientes = pacientes.Select(p =>
            {
                var persona = p.IdCedulaNavigation;
                var seguro = p.SeguroPacienteIdSeguroNavigation;
                var antecedente = p.AntecedentesMedicos.FirstOrDefault();

                string tipoSeguroDescripcion = seguro?.IdTipoSeguroNavigation?.Descripcion ?? "Sin Seguro";

                // Lógica de porcentaje 
                decimal porcentaje = 0m;
                const decimal PORCENTAJE_NORMAL = 0.10m;
                const decimal PORCENTAJE_ADULTO_MAYOR = 0.25m;
                const decimal PORCENTAJE_AMBOS = 0.35m;

                var seguroTexto = (tipoSeguroDescripcion ?? "").Trim().ToLower();

                if (seguroTexto.Contains("ambos"))
                {
                    porcentaje = PORCENTAJE_AMBOS;
                }
                else if (seguroTexto.Contains("adulto"))
                {
                    porcentaje = PORCENTAJE_ADULTO_MAYOR;
                }
                else if (seguroTexto.Contains("normal"))
                {
                    porcentaje = PORCENTAJE_NORMAL;
                }

                // Primer objeto anónimo: usamos bool para EsCronico y Estado
                return new
                {
                    Cedula = persona.IdCedula,
                    Nombre = persona.Nombre,
                    Apellido1 = persona.Apellido1,
                    Apellido2 = persona.Apellido2,
                    Estado = p.Estado,
                    TipoSeguro = tipoSeguroDescripcion,
                    PorcentajeSeguro = $"{porcentaje * 100}%",
                    TipoEnfermedadDescripcion = antecedente?.IdTipoEnfermedadNavigation?.Descripcion ?? "Sin registro",
                    EsCronico = antecedente?.Cronico ?? false,
                    AntecedentesDescripcion = antecedente?.Descripcion ?? "Sin antecedentes registrados",
                    IdSeguro = seguro?.IdSeguro
                };
            })
            // Segundo Select: Convertimos EsCronico de bool a "Sí" / "No" para la vista
            .Select(p => new {
                p.Cedula,
                p.Nombre,
                p.Apellido1,
                p.Apellido2,
                p.Estado,
                p.TipoSeguro,
                p.PorcentajeSeguro,
                p.TipoEnfermedadDescripcion,
                EsCronico = (p.EsCronico ? "Sí" : "No"),
                p.AntecedentesDescripcion,
                p.IdSeguro
            })
            .ToList();

            ViewBag.Pacientes = vistaPacientes;
            return View();
        }

        // ----------------------------------------------------------------------------------
        // --- READ: DETAILS (Detalles) ---
        // ----------------------------------------------------------------------------------
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

        // ----------------------------------------------------------------------------------
        // --- CREATE: GET (Preparar formulario) ---
        // ----------------------------------------------------------------------------------
        public IActionResult Create()
        {
            RecargarSelectLists(null, null, null);
            return View();
        }

        // ----------------------------------------------------------------------------------
        // --- CREATE: POST (Crear Paciente, Seguro y Antecedente) ---
        // Se corrige el manejo del parámetro 'Estado' a string, por si viene "Activo"/"Inactivo"
        // ----------------------------------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Se cambia bool Estado a string EstadoSeleccionado
        public async Task<IActionResult> Create(int IdCedula, int IdTipoSeguro, int IdTipoEnfermedad, string DescripcionAntecedente, bool Cronico, string EstadoSeleccionado)
        {
            using var trans = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. Validar que la Persona exista y no sea Paciente
                var persona = await _context.Personas.FindAsync(IdCedula);
                if (persona == null)
                {
                    ModelState.AddModelError("IdCedula", "La Cédula seleccionada no existe en la tabla de Personas.");
                }
                else if (await _context.Pacientes.AnyAsync(p => p.IdCedula == IdCedula))
                {
                    ModelState.AddModelError("IdCedula", "La Persona con esta Cédula ya está registrada como Paciente.");
                }

                if (!ModelState.IsValid)
                {
                    RecargarSelectLists(IdCedula, IdTipoSeguro, IdTipoEnfermedad);
                    return View();
                }

                // 2. Determinar el estado booleano
                // Asumimos que la vista envía "true" o "false", o "Activo"
                bool estadoActivo = EstadoSeleccionado != null && (EstadoSeleccionado.Equals("true", StringComparison.OrdinalIgnoreCase) || EstadoSeleccionado.Equals("Activo", StringComparison.OrdinalIgnoreCase));

                // 3. Crear el registro de Seguro
                var seguro = new Seguro
                {
                    IdTipoSeguro = IdTipoSeguro,
                    IdCedula = IdCedula,
                    FechaCreacion = DateTime.Now,
                    Activo = true
                };
                _context.Seguros.Add(seguro);
                await _context.SaveChangesAsync();

                // 4. Crear el registro de Paciente
                var paciente = new Paciente
                {
                    IdCedula = IdCedula,
                    SeguroPacienteIdSeguro = seguro.IdSeguro,
                    FechaRegistro = DateTime.Now,
                    Estado = estadoActivo // <-- AQUI SE USA EL VALOR CONVERTIDO
                };
                _context.Pacientes.Add(paciente);

                // 5. Crear el Antecedente Médico
                var antecedente = new AntecedentesMedico
                {
                    IdCedula = IdCedula,
                    IdTipoEnfermedad = IdTipoEnfermedad,
                    Descripcion = DescripcionAntecedente ?? "Sin descripción",
                    Cronico = Cronico,
                    Activo = true
                };
                _context.AntecedentesMedicos.Add(antecedente);

                await _context.SaveChangesAsync();
                await trans.CommitAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                ModelState.AddModelError("", "Error al crear paciente: " + ex.InnerException?.Message ?? ex.Message);
                RecargarSelectLists(IdCedula, IdTipoSeguro, IdTipoEnfermedad);
                return View();
            }
        }

        // ----------------------------------------------------------------------------------
        // --- UPDATE: GET (Cargar datos para edición) ---
        // ----------------------------------------------------------------------------------
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

            RecargarSelectLists(
                paciente.IdCedula,
                seguro?.IdTipoSeguro,
                antecedente?.IdTipoEnfermedad
            );

            ViewData["Antecedente"] = antecedente;
            ViewData["Persona"] = persona;
            ViewData["SeguroId"] = seguro?.IdSeguro;

            return View(paciente);
        }

        // ----------------------------------------------------------------------------------
        // --- UPDATE: POST (Guardar cambios) ---
        // ----------------------------------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Paciente paciente, int IdTipoSeguro, int IdTipoEnfermedad, string DescripcionAntecedente, bool Cronico, int IdSeguro)
        {
            if (id != paciente.IdCedula) return NotFound();

            using var trans = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. Actualizar Paciente (Estado)
                var pacienteExistente = await _context.Pacientes.FindAsync(id);
                if (pacienteExistente != null)
                {
                    pacienteExistente.Estado = paciente.Estado;
                    _context.Update(pacienteExistente);
                }

                // 2. Actualizar el TipoSeguro del Seguro
                var seguroExistente = await _context.Seguros.FindAsync(IdSeguro);
                if (seguroExistente != null)
                {
                    seguroExistente.IdTipoSeguro = IdTipoSeguro;
                    _context.Update(seguroExistente);
                }

                // 3. Actualizar Antecedentes
                var antecedenteExistente = await _context.AntecedentesMedicos.FirstOrDefaultAsync(a => a.IdCedula == id);
                if (antecedenteExistente != null)
                {
                    antecedenteExistente.IdTipoEnfermedad = IdTipoEnfermedad;
                    antecedenteExistente.Descripcion = DescripcionAntecedente ?? "Sin descripción";
                    antecedenteExistente.Cronico = Cronico;
                    _context.Update(antecedenteExistente);
                }

                // 4. GUARDAR CAMBIOS
                await _context.SaveChangesAsync();
                await trans.CommitAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                ModelState.AddModelError("", "Error al actualizar el paciente: " + ex.InnerException?.Message ?? ex.Message);

                // Volvemos a cargar los datos necesarios para la vista en caso de error
                var antecedente = await _context.AntecedentesMedicos.FirstOrDefaultAsync(a => a.IdCedula == id);
                var seguro = await _context.Seguros.FindAsync(IdSeguro);
                var persona = await _context.Personas.FindAsync(id);

                RecargarSelectLists(id, IdTipoSeguro, IdTipoEnfermedad);
                ViewData["Antecedente"] = antecedente;
                ViewData["Persona"] = persona;
                ViewData["SeguroId"] = IdSeguro;

                return View(paciente);
            }
        }

        // ----------------------------------------------------------------------------------
        // --- DELETE: GET y POST ---
        // ----------------------------------------------------------------------------------
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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var paciente = await _context.Pacientes
                .Include(p => p.AntecedentesMedicos)
                .FirstOrDefaultAsync(p => p.IdCedula == id);

            if (paciente != null)
            {
                using var trans = await _context.Database.BeginTransactionAsync();
                try
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
                        if (seguro != null) _context.Seguros.Remove(seguro);
                    }

                    await _context.SaveChangesAsync();
                    await trans.CommitAsync();
                }
                catch (Exception)
                {
                    await trans.RollbackAsync();
                }
            }

            return RedirectToAction(nameof(Index));
        }


        // ----------------------------------------------------------------------------------
        // --- Método Auxiliar para Recargar Listas (CREATE/EDIT) ---
        // ----------------------------------------------------------------------------------
        private void RecargarSelectLists(int? idCedulaSeleccionada, int? idTipoSeguroSeleccionado, int? idTipoEnfermedadSeleccionado)
        {
            // Cédulas de Personas que NO son Pacientes (Para Create)
            var personasNoPacientes = _context.Personas
                .Where(p => !_context.Pacientes.Any(pa => pa.IdCedula == p.IdCedula))
                .OrderBy(p => p.IdCedula)
                .Select(p => new {
                    p.IdCedula,
                    NombreCompleto = $"{p.IdCedula} - {p.Nombre} {p.Apellido1}"
                });
            ViewData["IdCedula"] = new SelectList(personasNoPacientes.AsEnumerable(), "IdCedula", "NombreCompleto", idCedulaSeleccionada);

            // SelectList para TipoSeguro
            ViewData["IdTipoSeguro"] = new SelectList(
                _context.TipoSeguros.OrderBy(t => t.Descripcion).ToList(),
                "IdTipoSeguro",
                "Descripcion",
                idTipoSeguroSeleccionado
            );

            // SelectList para TipoEnfermedad
            ViewData["IdTipoEnfermedad"] = new SelectList(
                _context.TipoEnfermedads.OrderBy(t => t.Descripcion).ToList(),
                "IdTipoEnfermedad",
                "Descripcion",
                idTipoEnfermedadSeleccionado
            );
        }

        // ----------------------------------------------------------------------------------
        // --- AJAX: Obtener datos de la Persona por Cédula (PARA LA VISTA CREATE) ---
        // ----------------------------------------------------------------------------------
        [HttpGet]
        public async Task<IActionResult> GetPersonaData(int idCedula)
        {
            var persona = await _context.Personas
                .FirstOrDefaultAsync(p => p.IdCedula == idCedula);

            if (persona == null)
            {
                return NotFound();
            }

            // Convertimos la fecha a string
            string fechaNacimientoStr = persona.FechaNacimiento.ToString("dd/MM/yyyy");

            // Devolvemos solo los datos relevantes para rellenar la vista
            return Json(new
            {
                nombre = persona.Nombre,
                apellido1 = persona.Apellido1,
                apellido2 = persona.Apellido2,
                fechaNacimiento = fechaNacimientoStr
            });
        }
    }
}