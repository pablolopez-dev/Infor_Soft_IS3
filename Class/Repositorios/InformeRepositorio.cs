using Infor_Soft_WPF.Class.BD;
using MySql.Data.MySqlClient;
using System;
using System.IO;

namespace Infor_Soft_WPF.Class.Repositorios
{
    public class InformeRepositorio
    {
        public bool GuardarInforme(string nombreArchivo, byte[] contenidoArchivo, int idUsuario, string usuario, DateTime fecha, TimeSpan hora, out string error)
        {
            error = null;

            try
            {
                using (var db = new BD_CONN())
                {
                    var conn = db.GetConnection();

                    db.OpenConnection(); // <-- Abre la conexión antes de ejecutar el comando

                    string query = @"INSERT INTO informes 
                             (titulo_docu, informe_blob, id_usuario, usuario, fecha_creacion, hora_creacion) 
                             VALUES (@nombre, @informe, @id_usuario, @usuario, @fecha, @hora)";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@nombre", nombreArchivo);
                        cmd.Parameters.AddWithValue("@informe", contenidoArchivo);
                        cmd.Parameters.AddWithValue("@id_usuario", idUsuario);
                        cmd.Parameters.AddWithValue("@usuario", usuario);
                        cmd.Parameters.AddWithValue("@fecha", fecha);
                        cmd.Parameters.AddWithValue("@hora", hora);

                        cmd.ExecuteNonQuery();
                    }

                    db.CloseConnection();
                }

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

    }
}
