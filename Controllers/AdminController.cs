﻿using Checador_Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;
using OfficeOpenXml;

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
                if (Datos.IdEncargado == row.Field<int>("IdEncargado"))
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
        public IActionResult Sites()
        {
            DataTable data = admin.Mostrar_Sites();
            ViewBag.Sites = data;  
            return View();
        }

        [HttpGet]
        public IActionResult CrearSite()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CrearSite(string Nombre, string Direccion, string Correo, string Contrasena)
        {
            if (admin.CrearSite(Nombre, Direccion, Correo, Contrasena))
                return RedirectToAction("Sites");
            return View();
        }

        [HttpGet]
        public IActionResult EditarSite(int id)
        {
            DataTable dt = admin.GetSite(id);
            ViewBag.Sites = dt;
            return View();
        }

        [HttpPost]
        public IActionResult EditarSite(int IdSite, string Nombre, string Direccion, string Correo)
        {
            if (admin.EditSite(IdSite, Nombre, Direccion, Correo))
                return RedirectToAction("Sites");
            return View();
        }

        [HttpGet]
        public IActionResult EliminarSite(int id)
        {
            DataTable dt = admin.GetSite(id);
            ViewBag.Sites = dt;
            Datos.IdSite = id;
            return View();
        }

        [HttpPost]
        public IActionResult EliminarSite()
        {
            if (admin.EliminarSite(Datos.IdSite))
                return RedirectToAction("Sites");
            return View();
        }

        [HttpGet]
        public IActionResult Asistencias()
        {
            ViewBag.Asistencias = admin.Asistencias();
            ViewBag.Empleados = admin.Mostrar_Empleados();
            ViewBag.Sites = admin.Mostrar_Sites();
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
        [HttpPost]
        public IActionResult ExportarExcel()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Para evitar problemas de licencia en EPPlus

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Asistencias");

                // Agregar encabezados
                worksheet.Cells[1, 1].Value = "ID";
                worksheet.Cells[1, 2].Value = "Nombre";
                worksheet.Cells[1, 3].Value = "Site";
                worksheet.Cells[1, 4].Value = "Encargado";
                worksheet.Cells[1, 5].Value = "Fecha";
                worksheet.Cells[1, 6].Value = "Hora de Entrada";
                worksheet.Cells[1, 7].Value = "Hora de Salida";
                worksheet.Cells[1, 8].Value = "Estado";

                // Obtener datos
                var asistencias = (admin.Asistencias() as DataTable)?.Rows.Cast<DataRow>()
                    .GroupBy(a => new { EmpleadoID = a[1].ToString(), Fecha = Convert.ToDateTime(a[4]).Date })
                    .Select(grupo => new
                    {
                        EmpleadoID = grupo.Key.EmpleadoID,
                        Fecha = grupo.Key.Fecha,
                        Entrada = grupo.Min(a => Convert.ToDateTime(a[4])), // La primera hora de entrada
                        Salida = grupo.Max(a => Convert.ToDateTime(a[4]))  // La última hora de salida
                    });

                var empleados = (admin.Mostrar_Empleados() as DataTable)?.Rows.Cast<DataRow>();
                var sitios = (admin.Mostrar_Sites() as DataTable)?.Rows.Cast<DataRow>();
                //var encargados = (ViewBag.Encargados as DataTable)?.Rows.Cast<DataRow>();

                int row = 2;
                foreach (var asistencia in asistencias)
                {
                    var empleado = empleados?.FirstOrDefault(e => e[0].ToString() == asistencia.EmpleadoID);
                    var sitio = sitios?.FirstOrDefault(s => s[0].ToString() == empleado?[5]?.ToString());
                    //var encargado = encargados?.FirstOrDefault(enc => enc[0].ToString() == empleado?[6]?.ToString());

                    // Verifica si empleado no es null antes de acceder a sus datos
                    string nombreEmpleado = empleado != null ? $"{empleado[1]} {empleado[2]} {empleado[3]}" : "Desconocido";
                    string nombreSitio = sitio?[1]?.ToString() ?? "N/A";
                    //string nombreEncargado = encargado?[1]?.ToString() ?? "N/A";

                    worksheet.Cells[row, 1].Value = asistencia.EmpleadoID;
                    worksheet.Cells[row, 2].Value = nombreEmpleado;
                    worksheet.Cells[row, 3].Value = nombreSitio;
                    //worksheet.Cells[row, 4].Value = nombreEncargado;
                    worksheet.Cells[row, 5].Value = asistencia.Fecha.ToString("yyyy-MM-dd");
                    worksheet.Cells[row, 6].Value = asistencia.Entrada.ToString("HH:mm");
                    worksheet.Cells[row, 7].Value = asistencia.Salida.ToString("HH:mm");
                    worksheet.Cells[row, 8].Value = "Presente";

                    row++;
                }


                // Ajustar tamaño de columnas
                worksheet.Cells.AutoFitColumns();

                // Guardar archivo
                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Asistencias.xlsx");
            }
        }
        public IActionResult CerrarSesion()
        {
            HttpContext.Response.Cookies.Delete("Checador_Intervalo");
            return RedirectToAction("Index", "Home");
        }
    }
}
