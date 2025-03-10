using Checador.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Checador.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ChecadorDB _contextDB;

        public HomeController(ILogger<HomeController> logger, ChecadorDB contexDB)
        {
            _logger = logger;
            _contextDB = contexDB;
        }

        public IActionResult Index()
        {
            Initialize();
            //var MiCookie = HttpContext.Request.Cookies["MiCookie"];

            //if (MiCookie != null) 
            //{
            //    List<Usuario> usuarios = _contextDB.Usuario.ToList();
            //    foreach (var item in usuarios)
            //    {
            //        if (MiCookie == item.Correo)
            //        {
            //            ViewBag.Nombre = item.Nombre;
            //            ViewBag.Tipo = item.TipoUsuario;
            //        }
            //    }
            //}
            return View();
        }

        [HttpGet]
        public IActionResult Login(){
            Initialize();
            return View();
        }

        //[HttpPost]
        //public IActionResult Login(Usuario usuario){
        //    UsuarioModel usuarioModel = new UsuarioModel(_contextDB);
        //    usuarioModel.Correo = usuario.Correo;
        //    usuarioModel.Contrasena = usuario.Contrasena;

        //    if (usuarioModel.Login())
        //    {
        //        CookieOptions options = new CookieOptions();
        //        options.Expires = DateTime.Now.AddDays(365);
        //        options.IsEssential = true;
        //        options.Path = "/";
        //        HttpContext.Response.Cookies.Append("MiCookie", usuario.Correo, options);
        //        return RedirectToAction("Index");
        //    }

        //    return View(usuario);
        //}

        public IActionResult Privacy()
        {
            return View();
        }

        public void Initialize()
        {
            _contextDB.Database.EnsureCreated();

            if (_contextDB.Usuario.Any())
            {
                return;
            }

            var insertarEmpleado = new Empleado[]
            {
                new Empleado {Nombre = "Rich", TipoEmpleado = "Administrador", Activo = true, IdSite = 1, Hash = "1554SA5S", DireccionImagen = "h", Sexo = "Masculino"}
            };

            var insertarUsuario = new Usuario[]
            {
                new Usuario {Contrasena = "1234", Correo = "ricardo_138@outlook.com",  HashEmpleado = "1554SA5S"}
            };

            var insertarSite = new Site[]
            {
                new Site {Nombre = "Valle de chalco", Direccion = "Calle Poniente 4D",Activo = true}
            };

            foreach (var u in insertarUsuario)
                _contextDB.Usuario.Add(u);
            _contextDB.SaveChanges();

            foreach (var u in insertarSite)
                _contextDB.Sites.Add(u);
            _contextDB.SaveChanges();

            foreach (var u in insertarEmpleado)
                _contextDB.Empleados.Add(u);
            _contextDB.SaveChanges();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
