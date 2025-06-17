using Infor_Soft_WPF.Class.BD;
using Infor_Soft_WPF.Class.Entidades;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;

namespace Infor_Soft_WPF.Class.Repositorios
{
    public class InformeRepositorio
    {

        public static class Conexion
        {
            public static string Cadena()
            {
                return "server=localhost;user=root;password=;database=inforsoft;";
            }

            public static MySqlConnection ObtenerConexion()
            {
                return new MySqlConnection(Cadena());
            }
        }

        public bool GuardarInforme(string nombreArchivo, byte[] contenidoArchivo, int idUsuario, string usuario, DateTime fecha, TimeSpan hora, int idAbogado, out string error)
        {
            error = null;


            try
            {
                using (var db = new BD_CONN())
                {
                    db.OpenConnection(); // ← Esto abre la conexión correctamente

                    var conn = db.GetConnection();

                    string query = @"INSERT INTO informes 
                             (titulo_docu, informe_blob, id_usuario, usuario, fecha_creacion, hora_creacion, id_abogado) 
                             VALUES (@nombre, @informe, @id_usuario, @usuario, @fecha, @hora, @id_abogado)";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@nombre", nombreArchivo);
                        cmd.Parameters.AddWithValue("@informe", contenidoArchivo);
                        cmd.Parameters.AddWithValue("@id_usuario", idUsuario);
                        cmd.Parameters.AddWithValue("@usuario", usuario);
                        cmd.Parameters.AddWithValue("@fecha", fecha);
                        cmd.Parameters.AddWithValue("@hora", hora);
                        cmd.Parameters.AddWithValue("@id_abogado", idAbogado);

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

        public List<InformeResumen> ObtenerInformesPorAbogado(int idAbogado)
        {
            var lista = new List<InformeResumen>();

            using (var db = new BD_CONN())
            {
                db.OpenConnection();
                var conn = db.GetConnection();

                string query = @"SELECT id_informe, titulo_docu, usuario, fecha_creacion, hora_creacion 
                         FROM informes
                         WHERE id_abogado = @idAbogado
                         ORDER BY fecha_creacion DESC, hora_creacion DESC";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idAbogado", idAbogado);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new InformeResumen
                            {
                                Id = reader.GetInt32("id_informe"),
                                Titulo = reader.GetString("titulo_docu"),
                                Usuario = reader.GetString("usuario"),
                                FechaCreacion = reader.GetDateTime("fecha_creacion"),
                                HoraCreacion = reader.GetTimeSpan("hora_creacion")
                            });
                        }
                    }
                }

                db.CloseConnection();
            }

            return lista;
        }

        public bool EliminarInforme(int idInforme)
        {
            using (var db = new BD_CONN())
            {
                db.OpenConnection();
                var conn = db.GetConnection();

                string query = "DELETE FROM informes WHERE id_informe = @id";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", idInforme);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }


        public byte[] ObtenerInformePorId(int idInforme, out string titulo)
        {
            titulo = "informe";
            using (var db = new BD_CONN())
            {
                db.OpenConnection();
                var conn = db.GetConnection();

                string query = @"SELECT titulo_docu, informe_blob FROM informes WHERE id_informe = @id";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", idInforme);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            titulo = reader.GetString("titulo_docu");
                            return (byte[])reader["informe_blob"];
                        }
                    }
                }

                db.CloseConnection();
            }

            return null;
        }

        public Dictionary<string, int> ObtenerCantidadInformesPorDia()
        {
            var resultados = new Dictionary<string, int>();
            using (var conexion = Conexion.ObtenerConexion())
            {
                conexion.Open();
                var cmd = new MySqlCommand(@"
            SELECT DATE(fecha_creacion) as dia, COUNT(*) as cantidad
            FROM informes
            GROUP BY dia
            ORDER BY dia DESC
        ", conexion);

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string fecha = Convert.ToDateTime(reader["dia"]).ToString("dd/MM/yyyy");
                    int cantidad = Convert.ToInt32(reader["cantidad"]);
                    resultados[fecha] = cantidad;
                }
            }
            return resultados;
        }

        public Dictionary<string, int> ObtenerCantidadInformesPorMes()
        {
            var resultados = new Dictionary<string, int>();
            using (var conexion = Conexion.ObtenerConexion())
            {
                conexion.Open();
                var cmd = new MySqlCommand(@"
            SELECT DATE_FORMAT(fecha_creacion, '%Y-%m') as mes, COUNT(*) as cantidad
            FROM informes
            GROUP BY mes
            ORDER BY mes DESC
        ", conexion);

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string mes = reader["mes"].ToString(); // ejemplo: "2025-06"
                    resultados[mes] = Convert.ToInt32(reader["cantidad"]);
                }
            }
            return resultados;
        }

        public Dictionary<string, int> ObtenerCantidadInformesPorAño()
        {
            var resultados = new Dictionary<string, int>();
            using (var conexion = Conexion.ObtenerConexion())
            {
                conexion.Open();
                var cmd = new MySqlCommand(@"
            SELECT YEAR(fecha_creacion) as anio, COUNT(*) as cantidad
            FROM informes
            GROUP BY anio
            ORDER BY anio DESC
        ", conexion);

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string anio = reader["anio"].ToString();
                    resultados[anio] = Convert.ToInt32(reader["cantidad"]);
                }
            }
            return resultados;
        }


    }
}
