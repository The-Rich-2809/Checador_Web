using Checador.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Checador.Controllers
{
    public class AdminController : Controller
    {
        private readonly ChecadorDB _contextDB;
        static int idEmpleado = 0;

        public AdminController(ChecadorDB contexDB)
        {
            _contextDB = contexDB;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Empleados()
        {
            var listaEmpleados = _contextDB.Empleados.ToList();
            var listaSites = _contextDB.Sites.ToList();
            var viewModel = new Tablas
            {
                Empleados = listaEmpleados,
                Sites = listaSites
            };

            return View(viewModel);
        }
        public IActionResult addEmpleados()
        {
            var listaSites = _contextDB.Sites.ToList();
            return View(listaSites);
        }
        [HttpPost]
        public IActionResult addEmpleados(Empleado empleado, Usuario usuario)
        {
            empleado.Hash = GenerarSHA256Base64(empleado.Nombre + empleado.IdSite);
            empleado.DireccionImagen = "h";
            _contextDB.Empleados.Add(empleado);
            _contextDB.SaveChanges();

            if (usuario.Correo != null)
            {
                usuario.HashEmpleado = empleado.Hash;
                _contextDB.Usuario.Add(usuario);
                _contextDB.SaveChanges();
            }
            return RedirectToAction("Empleados");
        }

        public IActionResult editEmpleados(int Id)
        {
            int idEmpleado = Id;
            var empleado = _contextDB.Empleados.FirstOrDefault(p => p.Id == Id);
            var usuario = _contextDB.Usuario.FirstOrDefault(p => p.HashEmpleado == empleado.Hash);
            if(usuario != null)
            {
                ViewBag.Correo = usuario.Correo;
                ViewBag.Contrasena = usuario.Contrasena;
            }

            var listasites = _contextDB.Sites.ToList();
            ViewBag.Sites = listasites;
            ViewBag.TipoEmpleado = empleado.TipoEmpleado;   
            return View(empleado);
        }
        [HttpPost]
        public IActionResult editEmpleados(Empleado empleado, Usuario usuario)
        {
            var empleadoExistente = _contextDB.Empleados.FirstOrDefault(p => p.Id == empleado.Id);
            string hash = empleadoExistente.Hash;
            if (empleadoExistente != null)
            {
                empleadoExistente.Nombre = empleado.Nombre;
                empleadoExistente.TipoEmpleado = empleado.TipoEmpleado;
                empleadoExistente.IdSite = empleado.IdSite;
                empleadoExistente.DireccionImagen = empleado.DireccionImagen;
                empleadoExistente.Hash = GenerarSHA256Base64(empleado.Nombre + empleado.IdSite);
                hash = empleadoExistente.Hash;
                _contextDB.SaveChanges();
            }
            if (usuario.Correo != null)
            {
                var usuarioExistente = _contextDB.Usuario.FirstOrDefault(p => p.HashEmpleado == hash);
                usuarioExistente.Correo = usuario.Correo;
                usuarioExistente.Contrasena = usuario.Contrasena;
                _contextDB.SaveChanges();
            }

            return View();
        }

        public IActionResult Sites()
        {
            var listasites = _contextDB.Sites.ToList();
            return View(listasites);
        }

        static string GenerarSHA256Base64(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hashBytes);
            }
        }


    }
}
