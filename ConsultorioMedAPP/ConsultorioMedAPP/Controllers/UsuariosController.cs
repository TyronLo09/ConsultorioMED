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
    public class UsuariosController : Controller
    {
        private readonly ConsultorioMedDBContext _context;

        public UsuariosController(ConsultorioMedDBContext context)
        {
            _context = context;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            var consultorioMedDBContext = _context.Usuarios.Include(u => u.PersonasIdCedulaNavigation).Include(u => u.RolUsuarioIdRolUsuarioNavigation);
            return View(await consultorioMedDBContext.ToListAsync());
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.PersonasIdCedulaNavigation)
                .Include(u => u.RolUsuarioIdRolUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.IdUsuario == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            ViewData["PersonasIdCedula"] = new SelectList(_context.Personas, "IdCedula", "IdCedula");
            ViewData["RolUsuarioIdRolUsuario"] = new SelectList(_context.RolUsuarios, "IdRolUsuario", "IdRolUsuario");
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Usuario usuario)
        {
            try
            {
                if (usuario != null)
                {
                    // Asignar fechas automáticamente si es necesario
                    usuario.FechaCreacion = DateTime.Now;
                    usuario.UltimoAcceso = DateTime.Now; // O null si prefieres

                    _context.Add(usuario);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewData["Error"] = "Los datos del usuario no pueden estar vacíos";
                }
            }
            catch (Exception ex)
            {
                ViewData["Error"] = "Error al crear el usuario: " + ex.Message;
            }

            ViewData["PersonasIdCedula"] = new SelectList(_context.Personas, "IdCedula", "IdCedula", usuario?.PersonasIdCedula);
            ViewData["RolUsuarioIdRolUsuario"] = new SelectList(_context.RolUsuarios, "IdRolUsuario", "IdRolUsuario", usuario?.RolUsuarioIdRolUsuario);
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            ViewData["PersonasIdCedula"] = new SelectList(_context.Personas, "IdCedula", "IdCedula", usuario.PersonasIdCedula);
            ViewData["RolUsuarioIdRolUsuario"] = new SelectList(_context.RolUsuarios, "IdRolUsuario", "IdRolUsuario", usuario.RolUsuarioIdRolUsuario);
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Usuario usuario)
        {
            if (id != usuario.IdUsuario)
            {
                return NotFound();
            }

            try
            {
                var usuarioExistente = await _context.Usuarios.FindAsync(id);
                if (usuarioExistente == null)
                {
                    return NotFound();
                }

                // Actualizar propiedades
                usuarioExistente.PersonasIdCedula = usuario.PersonasIdCedula;
                usuarioExistente.Contraseña = usuario.Contraseña;
                usuarioExistente.RolUsuarioIdRolUsuario = usuario.RolUsuarioIdRolUsuario;
                usuarioExistente.UltimoAcceso = usuario.UltimoAcceso;

                // Mantener la fecha de creación original
                // usuarioExistente.FechaCreacion no se modifica

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Usuarios.Any(e => e.IdUsuario == usuario.IdUsuario))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                ViewData["Error"] = "Error al actualizar: " + ex.Message;
            }

            ViewData["PersonasIdCedula"] = new SelectList(_context.Personas, "IdCedula", "IdCedula", usuario.PersonasIdCedula);
            ViewData["RolUsuarioIdRolUsuario"] = new SelectList(_context.RolUsuarios, "IdRolUsuario", "IdRolUsuario", usuario.RolUsuarioIdRolUsuario);
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.PersonasIdCedulaNavigation)
                .Include(u => u.RolUsuarioIdRolUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.IdUsuario == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.IdUsuario == id);
        }
    }
}
