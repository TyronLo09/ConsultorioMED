using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ConsultorioMedAPP.Models;

namespace ConsultorioMedAPP.Controllers
{
    public class LoginController : Controller
    {
        private readonly ConsultorioMedDBContext _context;

        public LoginController(ConsultorioMedDBContext context)
        {
            _context = context;
        }

        // GET: Login
        public IActionResult Index()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(int cedula, string contrasena)
        {
            if (cedula == 0 || string.IsNullOrEmpty(contrasena))
            {
                ViewBag.Error = "Debe ingresar la cédula y la contraseña.";
                return View();
            }

            // Buscar el usuario por cédula y contraseña
            var usuario = await _context.Usuarios
                .Include(u => u.RolUsuarioIdRolUsuarioNavigation)
                .FirstOrDefaultAsync(u =>
                    u.PersonasIdCedula == cedula &&
                    u.Contraseña == contrasena);

            if (usuario == null)
            {
                ViewBag.Error = "Cédula o contraseña incorrecta.";
                return View();
            }

            // Guardar datos básicos en sesión
            HttpContext.Session.SetInt32("Cedula", usuario.PersonasIdCedula);
            HttpContext.Session.SetInt32("IdUsuario", usuario.IdUsuario);
            HttpContext.Session.SetString("Rol", usuario.RolUsuarioIdRolUsuarioNavigation.Descripcion ?? "Desconocido");

            // Actualizar fecha de último acceso
            usuario.UltimoAcceso = DateTime.Now;
            _context.Update(usuario);
            await _context.SaveChangesAsync();

            // Redirección según el rol
            switch (usuario.RolUsuarioIdRolUsuario)
            {
                case 1:
                    return RedirectToAction("Index", "HomeAdmin"); // menú del administrador
                case 2:
                    return RedirectToAction("Index", "HomeRecepcion"); // menú de recepción
                default:
                    return RedirectToAction("Index", "Home"); // menú general
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}
