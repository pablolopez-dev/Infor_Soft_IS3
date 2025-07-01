using Infor_Soft_WPF.Class.BD;
using Infor_Soft_WPF.Class.Entidades;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

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

        public Dictionary<string, int> ObtenerCantidadInformesPorUsuario()
        {
            var resultados = new Dictionary<string, int>();

            using (var conexion = Conexion.ObtenerConexion())
            {
                conexion.Open();

                string query = @"
            SELECT u.nombre AS usuario, COUNT(i.id_informe) AS cantidad
            FROM informes i
            INNER JOIN usuarios u ON u.id_usuario = i.id_usuario
            GROUP BY u.nombre
            ORDER BY cantidad DESC";

                using (var cmd = new MySqlCommand(query, conexion))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string usuario = reader.GetString("usuario");
                            int cantidad = reader.GetInt32("cantidad");
                            resultados[usuario] = cantidad;
                        }
                    }
                }
            }

            return resultados;
        }


        public bool GuardarInforme(string nombreArchivo, byte[] contenidoArchivo, int idUsuario, string usuario, DateTime fecha, TimeSpan hora, int idAbogado, out string error)
        {
            error = null;

            try
            {
                using (var db = new BD_CONN())
                {
                    db.OpenConnection();
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

                string query = @"
                    SELECT i.id_informe, i.titulo_docu, i.autos_caratulados, 
                           i.fecha_creacion, i.hora_creacion, 
                           u.nombre AS usuario_nombre,
                           a.nombre AS abogado_nombre
                    FROM informes i
                    INNER JOIN usuarios u ON u.id_usuario = i.id_usuario
                    INNER JOIN abogados a ON a.id_abogado = i.id_abogado
                    WHERE i.id_abogado = @idAbogado
                    ORDER BY i.fecha_creacion DESC";

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
                                AutosCaratulados = reader.IsDBNull(reader.GetOrdinal("autos_caratulados"))
                                    ? "" : reader.GetString("autos_caratulados"),
                                Usuario = reader.GetString("usuario_nombre"),
                                AbogadoNombre = reader.GetString("abogado_nombre"),
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

        // NUEVO: Método para buscar informes por nombre del abogado (con LIKE)
        public List<InformeResumen> BuscarInformesPorNombreAbogado(string nombreAbogado)
        {
            var lista = new List<InformeResumen>();

            using (var db = new BD_CONN())
            {
                db.OpenConnection();
                var conn = db.GetConnection();

                string query = @"
                    SELECT i.id_informe, i.titulo_docu, i.autos_caratulados, 
                           i.fecha_creacion, i.hora_creacion, 
                           u.nombre AS usuario_nombre,
                           a.nombre AS abogado_nombre
                    FROM informes i
                    INNER JOIN usuarios u ON u.id_usuario = i.id_usuario
                    INNER JOIN abogados a ON a.id_abogado = i.id_abogado
                    INNER JOIN registros_comisivos r a ON r.id_usuario = i.id_usuario
                    WHERE a.nombre LIKE @nombreAbogado
                    ORDER BY i.fecha_creacion DESC";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nombreAbogado", "%" + nombreAbogado + "%");

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new InformeResumen
                            {
                                Id = reader.GetInt32("id_informe"),
                                Titulo = reader.GetString("titulo_docu"),
                                AutosCaratulados = reader.IsDBNull(reader.GetOrdinal("autos_caratulados"))
                                    ? "" : reader.GetString("autos_caratulados"),
                                Usuario = reader.GetString("usuario_nombre"),
                                AbogadoNombre = reader.GetString("abogado_nombre"),
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
                    ORDER BY dia DESC", conexion);

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
                    ORDER BY mes DESC", conexion);

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string mes = reader["mes"].ToString();
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
                    ORDER BY anio DESC", conexion);

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
