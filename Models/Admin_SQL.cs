using MySql.Data.MySqlClient;
using System.Data;

namespace Checador_Web.Models
{
    public class Admin_SQL
    {
        public DataTable Mostrar_Empleados()
        {
            DataTable dt = new DataTable();
            string sql;

            if (Datos.AccesoSite == "0")
                sql = "SELECT * From empleados";
            else
                sql = "SELECT * From empleados where idSite = @AccesoSite";

            MySqlConnection conexionBD = Conexion.conexion();
            conexionBD.Open();

            try
            {
                MySqlCommand comando = new MySqlCommand(sql, conexionBD);
                comando.Parameters.AddWithValue("AccesoSite", Datos.AccesoSite);
                dt.Load(comando.ExecuteReader());

            }
            catch (MySqlException ex)
            {
                Datos.Mensaje ="Error al buscar " + ex.Message;
            }
            finally
            {
                conexionBD.Close();
            }
            return dt;
        }

        public DataTable Mostrar_EmpleadosId(int id)
        {
            DataTable dt = new DataTable();
            string sql = "SELECT * From empleados where idEmpleado = @idEmpleado";

            MySqlConnection conexionBD = Conexion.conexion();
            conexionBD.Open();

            try
            {
                MySqlCommand comando = new MySqlCommand(sql, conexionBD);
                comando.Parameters.AddWithValue("idEmpleado", id);
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

        public DataTable Mostrar_Asistencias()
        {
            DataTable dt = new DataTable();

            string sql = "SELECT * From registros";
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

        public DataTable Mostrar_Sites()
        {
            DataTable dt = new DataTable();

            string sql;

            if (Datos.AccesoSite == "0")
                sql = "SELECT * From sites";
            else
                sql = "SELECT * From sites where idsite = @AccesoSite";

            MySqlConnection conexionBD = Conexion.conexion();
            conexionBD.Open();

            try
            {
                MySqlCommand comando = new MySqlCommand(sql, conexionBD);
                comando.Parameters.AddWithValue("AccesoSite", Datos.AccesoSite);
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
    }
}
