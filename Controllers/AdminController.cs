using Checador_Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Checador_Web.Controllers
{
    public class AdminController : Controller
    {
        Admin_SQL admin = new Admin_SQL();
        public IActionResult Lobby()
        {
            DataTable data = admin.Mostrar_Empleados();
            foreach (DataRow row in data.Rows)
            {
                if(Datos.IdEncargado == row.Field<int>("IdEncargado"))
                {
                    ViewBag.Nombre = row.Field<string>("Nombre");
                    break;
                }
            }
            return View();
        }
        [HttpGet]
        public IActionResult Empleados() 
        {
            DataTable data = admin.Mostrar_Empleados();
            DataTable sites = admin.Mostrar_Sites();
            ViewBag.Empleados = data;
            ViewBag.Sites = sites;
            return View();
        }
        [HttpGet]
        public IActionResult Agregar_Empleados()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Asistencias()
        {
            return View();
        }
        [HttpGet]
        public JsonResult ObtenerAsistencias()
        {
            DateTime dateTime = new DateTime();
            int FechaEmpleado = 0;
            DataTable dt_asitencias = admin.Mostrar_Asistencias();
            dt_asitencias.DefaultView.Sort = "registro ASC";

            DataTable dt_empleados = admin.Mostrar_Empleados();
            DataTable dt_sites = admin.Mostrar_Sites();

            foreach(DataRow row_empleados in dt_empleados.Rows)
            {
                foreach(DataRow row_asisitencia in dt_asitencias.Rows)
                {
                    if (row_empleados.Field<int>("idEmpleado") == row_asisitencia.Field<int>("idEmpleado"))
                    {
                        if(row_asisitencia.Field<DateTime>("registro").Date != dateTime.Date)
                        {
                            FechaEmpleado++;
                            dateTime = row_asisitencia.Field<DateTime>("registro");
                        }
                    }
                }

            }

            var asistencias = new List<object>
            {
                new { ID = 1, Nombre = "Juan Pérez", Site = "CDMX", TipoEmpleado = "Tiempo Completo", Encargado = "José Martínez", Fecha = "2025-03-03", Entrada = "08:00", Salida = "17:00", Estado = "Presente" },
                new { ID = 2, Nombre = "María López", Site = "Guadalajara", TipoEmpleado = "Medio Tiempo", Encargado = "Laura Gómez", Fecha = "2025-03-03", Entrada = "08:30", Salida = "17:30", Estado = "Tarde" }
            };

            return Json(new { data = asistencias });
        }
        public IActionResult CerrarSesion()
        {
            HttpContext.Response.Cookies.Delete("Checador_Intervalo");
            return RedirectToAction("Index", "Home");
        }
    }
}
