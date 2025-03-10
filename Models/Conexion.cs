using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin_Checador_Web.Models
{
    internal class Conexion
    {
        public static MySqlConnection conexion()
        {
            string cadenaConexion = "";
            cadenaConexion = "datasource=localhost; username=root; password=300920; database=checador";
            //cadenaConexion = "datasource=db4free.net; username=dbintervalo; password=Intervalo2024; database=dbchecador";

            try
            {
                MySqlConnection conexionBD = new MySqlConnection(cadenaConexion);

                return conexionBD;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return null;
            }
        }
    }
}
