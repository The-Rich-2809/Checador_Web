using MySql.Data.MySqlClient;
using System.Data;

namespace Admin_Checador_Web.Models
{
    public class Admin_SQL
    {
        public DataTable Mostrar_Empleados()
        {
            DataTable dt = new DataTable();

            string sql = "SELECT * From empleados";
            MySqlConnection conexionBD = Conexion.conexion();
            conexionBD.Open();

            try
            {
                MySqlCommand comando = new MySqlCommand(sql, conexionBD);
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

            string sql = "SELECT * From sites";
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
    }
}
