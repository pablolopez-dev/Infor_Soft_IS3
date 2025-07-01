    using Infor_Soft_WPF.Class.BD;
    using Infor_Soft_WPF.Class.Entidades;
    using MySql.Data.MySqlClient;
    using System.Collections.Generic;

    namespace Infor_Soft_WPF.Class.Repositorios
    {
        public class AbogadoRepositorio
        {
            public List<Abogado> ObtenerAbogados()
            {
                var lista = new List<Abogado>();

                using (var db = new BD_CONN())
                {
                    db.OpenConnection();
                    var conn = db.GetConnection();

                    string query = "SELECT id_abogado, nombre, apellido, telefono FROM abogados ORDER BY nombre ASC";

                    using (var cmd = new MySqlCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Abogado
                            {
                                Id = reader.GetInt32("id_abogado"),
                                Nombre = reader.GetString("nombre"),
                                Apellido = reader.IsDBNull(reader.GetOrdinal("apellido")) ? "" : reader.GetString("apellido"),
                                Telefono = reader.IsDBNull(reader.GetOrdinal("telefono")) ? "" : reader.GetString("telefono")
                            });
                        }
                    }

                    db.CloseConnection();
                }

                return lista;
            }

            public void AgregarAbogado(string nombre, string apellido, string telefono)
            {
                using (var db = new BD_CONN())
                {
                    db.OpenConnection();
                    var conn = db.GetConnection();

                    string query = "INSERT INTO abogados (nombre, apellido, telefono) VALUES (@nombre, @apellido, @telefono)";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@nombre", nombre);
                        cmd.Parameters.AddWithValue("@apellido", apellido);
                        cmd.Parameters.AddWithValue("@telefono", telefono);
                        cmd.ExecuteNonQuery();
                    }

                    db.CloseConnection();
                }
            }
        }
    }
