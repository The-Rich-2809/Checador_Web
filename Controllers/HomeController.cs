using Checador_Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;

namespace Checador_Web.Controllers
{
    public class HomeController : Controller
    {
        Login_SQL login = new Login_SQL();
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var miCookie = HttpContext.Request.Cookies["Checador_Intervalo"];

            DataTable data = login.Mostrar_EmpleadosAdmin();
            foreach (DataRow row in data.Rows)
            {
                if (miCookie == row.Field<string>("Correo"))
                {
                    Datos.idEmpleadoTabla = row.Field<Int32>("idEmpleado");
                    Datos.AccesoSite = row.Field<string>("AccesoSite");

                    return RedirectToAction("Lobby", "Admin");
                }
            }

            return View();
        }
        [HttpPost]
        public IActionResult Index(string Correo, string Contrasena)
        {
            if (login.Ingresar(Correo, Contrasena))
            {
                CookieOptions options = new CookieOptions();
                options.Expires = DateTime.Now.AddDays(365);
                options.IsEssential = true;
                options.Path = "/";
                HttpContext.Response.Cookies.Append("Checador_Intervalo", Correo, options);

                return RedirectToAction("Lobby", "Admin");
            }
            else
            {
                ViewBag.ErrorMessage = "Usuario y/o Contraseña incorrecta";
                return View();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
