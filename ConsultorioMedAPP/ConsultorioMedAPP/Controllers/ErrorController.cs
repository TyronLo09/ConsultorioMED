using Microsoft.AspNetCore.Mvc;

namespace ConsultorioMedAPP.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/404")]
        public IActionResult NotFound404()
        {
            Response.StatusCode = 404;
            return View("NotFound");
        }

        [Route("Error/500")]
        public IActionResult ServerError()
        {
            Response.StatusCode = 500;
            return View("ServerError");
        }
    }
}