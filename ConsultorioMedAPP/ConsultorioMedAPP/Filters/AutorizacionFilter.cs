using ConsultorioMedAPP.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ConsultorioMedAPP.Filters
{
    public class AutorizacionFilter : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var nombreControlador = context.RouteData.Values["controller"]?.ToString();

            if (nombreControlador?.Equals("Login", StringComparison.OrdinalIgnoreCase) == true ||
            nombreControlador?.Equals("Error", StringComparison.OrdinalIgnoreCase) == true)
            {
                return;
            }

            var cedula = context.HttpContext.Session.GetInt32("Cedula");

            if (!SessionHelper.EstaAutenticado(context.HttpContext.Session))
            {
                context.Result = new RedirectToActionResult("Index", "Login", null);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        { }
    }
}