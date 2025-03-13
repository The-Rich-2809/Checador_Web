using Checador_Web.Models;
using MySql.Data.MySqlClient;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Checador_Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;


namespace Checador_Web.Models
{
    public class Login_SQL
    {
        public bool Ingresar(string Correo, string Contrasena)
        {
            MySqlDataReader reader = null;
            string sql = "SELECT * From empleadosadmin WHERE Correo = @Correo AND Contrasena = @Contrasena LIMIT 1";
            MySqlConnection conexionBD = Conexion.conexion();
            conexionBD.Open();

            try
            {
                MySqlCommand comando = new MySqlCommand(sql, conexionBD);
                comando.Parameters.AddWithValue("Correo", Correo);
                comando.Parameters.AddWithValue("Contrasena", Contrasena);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Datos.AccesoSite = reader.GetString(4);
                        Datos.idEmpleadoTabla = reader.GetInt32(1);
                    }

                    return true;
                }
            }
            catch (MySqlException ex)
            {
                Datos.Mensaje = "Error al buscar " + ex.Message;
            }
            finally
            {
                conexionBD.Close();
            }
            return false;


        }
        public DataTable Mostrar_EmpleadosAdmin()
        {
            DataTable dt = new DataTable();

            string sql = "SELECT * From empleadosAdmin";
            MySqlConnection conexionBD = Conexion.conexion();
            conexionBD.Open();

            try
            {
                MySqlCommand comando = new MySqlCommand(sql, conexionBD);
                dt.Load(comando.ExecuteReader());

            }
            catch (MySqlException ex)
            {
                Datos.Mensaje = "Error al buscar " + ex.Message;
            }
            finally
            {
                conexionBD.Close();
            }
            return dt;
        }

        public bool ComprobarCookie(string miCookie)
        {

            DataTable data = Mostrar_EmpleadosAdmin();
            foreach (DataRow row in data.Rows)
            {
                if (miCookie == row.Field<string>("Correo"))
                {
                    Datos.idEmpleadoTabla = row.Field<Int32>("idEmpleado");
                    Datos.AccesoSite = row.Field<string>("AccesoSite");
                    return true;
                }
            }
            return false;
        }
    }
}
