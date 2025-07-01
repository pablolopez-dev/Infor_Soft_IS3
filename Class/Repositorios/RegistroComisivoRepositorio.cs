using MySql.Data.MySqlClient;
using System;
using System.IO;

namespace Infor_Soft_WPF.Class.Repositorios
{
    public class RegistroComisivoRepositorio
    {
        private static string CadenaConexion => "server=localhost;user=root;password=;database=inforsoft;";

        public bool GuardarRegistroComisivo(byte[] archivoBlob, int idUsuario, string autosCaratulados, out string error)
        {
            error = null;
            try
            {
                using (var conexion = new MySqlConnection(CadenaConexion))
                {
                    conexion.Open();
                    string query = @"
                        INSERT INTO registros_comisivos (registro_blob, id_usuario, autos_caratulados) 
                        VALUES (@blob, @idUsuario, @autosCaratulados)";

                    using (var cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@blob", archivoBlob);
                        cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                        cmd.Parameters.AddWithValue("@autosCaratulados", autosCaratulados);

                        cmd.ExecuteNonQuery();
                    }
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
