using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Cms;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
                conexionBD.Close();

            }
            catch (MySqlException ex)
            {
                Datos.Mensaje ="Error al buscar " + ex.Message;
                conexionBD.Close();
            }
            return dt;
        }

        public DataTable GetEmpleado(int id)
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
                conexionBD.Close();

            }
            catch (MySqlException ex)
            {
                Datos.Mensaje = "Error al buscar " + ex.Message;
                conexionBD.Close();
            }
            return dt;
        }

        public bool EliminarEmpleado(string id)
        {
            string sql = "DELETE FROM empleados WHERE idEmpleado = @idEmpleado";
            MySqlConnection conexionBD = Conexion.conexion();
            conexionBD.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(sql, conexionBD);
                comando.Parameters.AddWithValue("idEmpleado", id);
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

        public bool EliminarEmpleadoSite(string id)
        {
            string sql = "DELETE FROM empleadosite WHERE idEmpleado = @idEmpleado";
            MySqlConnection conexionBD = Conexion.conexion();
            conexionBD.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(sql, conexionBD);
                comando.Parameters.AddWithValue("idEmpleado", id);
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
        public bool EliminarEmpleadoAdmin(string id)
        {
            string sql = "DELETE FROM empleadosadmin WHERE idEmpleado = @idEmpleado";
            MySqlConnection conexionBD = Conexion.conexion();
            conexionBD.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(sql, conexionBD);
                comando.Parameters.AddWithValue("idEmpleado", id);
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
        public DataTable GetSite()
        {
            DataTable dataTable = new DataTable();

            string sql = "SELECT * FROM sites";
            MySqlConnection conexionBD = Conexion.conexion();
            conexionBD.Open();

            try
            {
                MySqlCommand comando = new MySqlCommand(sql, conexionBD);
                dataTable.Load(comando.ExecuteReader());
                conexionBD.Close();

            }
            catch (MySqlException ex)
            {
                Datos.Mensaje = "Error al buscar " + ex.Message;
                conexionBD.Close();
            }

            return dataTable;
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
                conexionBD.Close();

            }
            catch (MySqlException ex)
            {
                Datos.Mensaje = "Error al buscar " + ex.Message;
                conexionBD.Close();
            }

            return dataTable;
        }

        public bool CrearSite(string nombre, string Direccion, string Correo, string Contrasena, TimeSpan entrada, TimeSpan salida)
        {
            string sql = "INSERT INTO sites (Nombre, Direccion, Correo, Contrasena, HoraEntrada, HoraSalida) VALUES (@Nombre, @Direccion, @Correo, @Contrasena, @entrada, @salida);";
            MySqlConnection conexionBD = Conexion.conexion();
            conexionBD.Open();

            try
            {
                MySqlCommand comando = new MySqlCommand(sql, conexionBD);
                comando.Parameters.AddWithValue("@Nombre", nombre);
                comando.Parameters.AddWithValue("@Direccion", Direccion);
                comando.Parameters.AddWithValue("@Correo", Correo);
                comando.Parameters.AddWithValue("@Contrasena", Contrasena);
                comando.Parameters.AddWithValue("@entrada", entrada);
                comando.Parameters.AddWithValue("@salida", salida);
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


        public bool EditSite(int IdSite, string Nombre, string Direccion, string Correo, string Contrasena, TimeSpan entrada, TimeSpan salida)
        {
            string sql = "UPDATE sites SET Nombre = @Nombre, Direccion = @Direccion, Correo =@Correo, Contrasena = @Contrasena, HoraEntrada = @entrada, HoraSalida = @salida WHERE idsite = @IdSite;";
            MySqlConnection conexionBD = Conexion.conexion();
            conexionBD.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(sql, conexionBD);
                comando.Parameters.AddWithValue("Nombre", Nombre);
                comando.Parameters.AddWithValue("Direccion", Direccion);
                comando.Parameters.AddWithValue("Correo", Correo);
                comando.Parameters.AddWithValue("IdSite", IdSite);
                comando.Parameters.AddWithValue("@entrada", entrada);
                comando.Parameters.AddWithValue("@salida", salida);
                comando.Parameters.AddWithValue("@Contrasena", Contrasena);
                comando.ExecuteNonQuery();
                conexionBD.Close();

                if (EditarEmpleadoHora(IdSite, entrada, salida))
                    return true;
                return false;
            }
            catch (MySqlException ex)
            {
                conexionBD.Close();
                return false;

            }
        }

        public bool EditarEmpleadoHora (int IdSite, TimeSpan entrada, TimeSpan salida)
        {
            string sql = "UPDATE empleados SET  HoraEntrada = @entrada, HoraSalida = @salida WHERE idsite = @IdSite;";
            MySqlConnection conexionBD = Conexion.conexion();
            conexionBD.Open();

            try
            {
                MySqlCommand comando = new MySqlCommand(sql, conexionBD);
                comando.Parameters.AddWithValue("IdSite", IdSite);
                comando.Parameters.AddWithValue("@entrada", entrada);
                comando.Parameters.AddWithValue("@salida", salida);
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

        public DataTable Asistencias() 
        {
            DataTable dt = new DataTable();
            string sql;

            if (Datos.AccesoSite == "0")
                sql = "SELECT * From registros";
            else
                sql = "SELECT * From registros where idSite = @AccesoSite";

            MySqlConnection conexionBD = Conexion.conexion();
            conexionBD.Open();

            try
            {
                MySqlCommand comando = new MySqlCommand(sql, conexionBD);
                comando.Parameters.AddWithValue("AccesoSite", Datos.AccesoSite);
                dt.Load(comando.ExecuteReader());
                conexionBD.Close();

            }
            catch (MySqlException ex)
            {
                Datos.Mensaje ="Error al buscar " + ex.Message;
                conexionBD.Close();
            }

            return dt;
        }
        public DataTable Mostrar_Asistencias()
        {
            DataTable dt = new DataTable();
            string sql;

            if (Datos.AccesoSite == "0")
                sql = "SELECT * From registros";
            else
                sql = "SELECT * From registros where idSite = @AccesoSite";

            MySqlConnection conexionBD = Conexion.conexion();
            conexionBD.Open();

            try
            {
                MySqlCommand comando = new MySqlCommand(sql, conexionBD);
                comando.Parameters.AddWithValue("AccesoSite", Datos.AccesoSite);
                dt.Load(comando.ExecuteReader());
                conexionBD.Close();

            }
            catch (MySqlException ex)
            {
                Datos.Mensaje = "Error al buscar " + ex.Message;
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
                conexionBD.Close();

            }
            catch (MySqlException ex)
            {
                Datos.Mensaje = "Error al buscar " + ex.Message;
                conexionBD.Close();
            }
            return dt;
        }
    }
}
