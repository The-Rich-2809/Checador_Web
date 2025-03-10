using MySql.Data.MySqlClient;
using System.Data;

namespace Checador_Web.Models
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

        public DataTable GetSite(int id)
        {
            DataTable dataTable = new DataTable();

            string sql = "SELECT * FROM sites WHERE idsite = @idSite;";
            MySqlConnection conexionBD = Conexion.conexion();
            conexionBD.Open();

            try
            {
                MySqlCommand comando = new MySqlCommand(sql, conexionBD);
                comando.Parameters.AddWithValue("idsite", id);
                dataTable.Load(comando.ExecuteReader());

            }
            catch (MySqlException ex)
            {
                Datos.Mensaje = "Error al buscar " + ex.Message;
            }
            finally
            {
                conexionBD.Close();
            }

            return dataTable;
        }

        public bool CrearSite(string nombre, string Direccion, string Correo, string Contrasena)
        {
            string sql = "INSERT INTO sites (Nombre, Direccion, Correo, Contrasena) VALUES (@Nombre, @Direccion, @Correo, @Contrasena);";
            MySqlConnection conexionBD = Conexion.conexion();
            conexionBD.Open();

            try
            {
                MySqlCommand comando = new MySqlCommand(sql, conexionBD);
                comando.Parameters.AddWithValue("@Nombre", nombre);
                comando.Parameters.AddWithValue("@Direccion", Direccion);
                comando.Parameters.AddWithValue("@Correo", Correo);
                comando.Parameters.AddWithValue("@Contrasena", Contrasena);
                comando.ExecuteNonQuery();
                conexionBD.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                conexionBD.Close();
                return false;
            }
        }


        public bool EditSite(int IdSite, string Nombre, string Direccion, string Correo)
        {
            string sql = "UPDATE sites SET Nombre = @Nombre, Direccion = @Direccion, Correo =@Correo WHERE idsite = @IdSite;";
            MySqlConnection conexionBD = Conexion.conexion();
            conexionBD.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(sql, conexionBD);
                comando.Parameters.AddWithValue("Nombre", Nombre);
                comando.Parameters.AddWithValue("Direccion", Direccion);
                comando.Parameters.AddWithValue("Correo", Correo);
                comando.Parameters.AddWithValue("IdSite", IdSite);
                comando.ExecuteNonQuery();
                conexionBD.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                conexionBD.Close();
                return false;

            }
        }

        public bool EliminarSite(int IdSite)
        {
            string sqlEmpleadosAdmin = @"
            DELETE FROM empleadosadmin 
            WHERE idEmpleado IN (SELECT idEmpleado FROM empleados WHERE idSite = @IdSite);";

            string sqlEmpleados = "DELETE FROM empleados WHERE idSite = @IdSite;";
            string sqlSites = "DELETE FROM sites WHERE idSite = @IdSite;";

            MySqlConnection conexionBD = Conexion.conexion();
            conexionBD.Open();

            MySqlTransaction transaccion = conexionBD.BeginTransaction(); // Iniciar transacción

            try
            {
                MySqlCommand comando = new MySqlCommand(sqlEmpleadosAdmin, conexionBD, transaccion);
                comando.Parameters.AddWithValue("@IdSite", IdSite);
                comando.ExecuteNonQuery();

                comando.CommandText = sqlEmpleados;
                comando.ExecuteNonQuery();

                comando.CommandText = sqlSites;
                comando.ExecuteNonQuery();

                transaccion.Commit(); // Confirmar los cambios
                conexionBD.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                transaccion.Rollback(); // Revertir cambios en caso de error
                conexionBD.Close();
                return false;
            }
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
