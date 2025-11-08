using ConsultorioMedAPP.Models;
using ConsultorioMedAPP.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            // Si ya está autenticado, redirigir según su rol
            if (SessionHelper.EstaAutenticado(HttpContext.Session))
            {
                return RedirigirSegunRol();
            }

            return View();
        }

        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(int cedula, string contrasena)
        {
            // Si ya está autenticado, redirigir
            if (SessionHelper.EstaAutenticado(HttpContext.Session))
            {
                return RedirigirSegunRol();
            }

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

            return RedirigirSegunRol(usuario.RolUsuarioIdRolUsuario);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }

        private IActionResult RedirigirSegunRol(int? rolUsuarioId = null)
        {
            // Si no se proporciona el rol, obtenerlo de la sesión
            if (rolUsuarioId == null)
            {
                var rol = SessionHelper.ObtenerRol(HttpContext.Session);
                // Convertir rol a ID según tu lógica, o mantener la lógica actual
            }

            // Usar el ID del rol para redirigir
            switch (rolUsuarioId ?? ObtenerRolIdDeSesion())
            {
                case 1:
                    return RedirectToAction("Index", "Home");

                case 2:
                    return RedirectToAction("Index", "Home");

                default:
                    return RedirectToAction("Index", "Home");
            }
        }

        private int ObtenerRolIdDeSesion()
        {
            // Aquí puedes mapear el nombre del rol a su ID
            var rol = SessionHelper.ObtenerRol(HttpContext.Session);
            return rol switch
            {
                "Administrador" => 1,
                "Recepcion" => 2,
                _ => 3
            };
        }
    }
}