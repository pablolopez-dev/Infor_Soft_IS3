using System;
using System.Security.Cryptography;
using System.Text;
using Infor_Soft_WPF.Class.BD;
using System.Windows;
using MySql.Data.MySqlClient;

namespace Infor_Soft_WPF.Class.Login_Register
{
    public class RegisterUser
    {
        private string connectionString = "server=localhost;user=root;password=;database=inforsoft;port=3306";

        public int RegistrarUsuario(string usuario, int matricula, string nombre, string correo, string contraseña)
        {
            int nuevoId = -1;

            var db = new BD_CONN();
            var conn = db.GetConnection();

            try
            {
                db.OpenConnection();

                // Verificar si ya existe
                string verificarQuery = "SELECT COUNT(*) FROM usuarios WHERE usuario = @usuario";
                using (var cmd = new MySqlCommand(verificarQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@usuario", usuario);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                    if (count > 0)
                        return -1; // Usuario ya existe
                }

                // Insertar nuevo usuario
                string insertarQuery = "INSERT INTO usuarios (usuario, matricula, nombre, correo, contraseña) VALUES (@usuario, @matricula, @nombre, @correo, @contraseña)";
                using (var cmd = new MySqlCommand(insertarQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@nombre", nombre);
                    cmd.Parameters.AddWithValue("@usuario", usuario);
                    cmd.Parameters.AddWithValue("@matricula", matricula);
                    cmd.Parameters.AddWithValue("@correo", correo);
                    cmd.Parameters.AddWithValue("@contraseña", GenerarHashSHA256(contraseña));
                    cmd.ExecuteNonQuery();

                    // Obtener el ID insertado
                    cmd.CommandText = "SELECT LAST_INSERT_ID()";
                    nuevoId = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar: " + ex.Message);
            }
            finally
            {
                db.CloseConnection();
            }

            return nuevoId;
        }

        private string GenerarHashSHA256(string texto)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(texto));
                StringBuilder builder = new StringBuilder();

                foreach (byte b in bytes)
                    builder.Append(b.ToString("x2"));

                return builder.ToString();
            }
        }


        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);

                StringBuilder builder = new StringBuilder();
                foreach (byte b in hash)
                    builder.Append(b.ToString("x2"));

                return builder.ToString();
            }
        }
    }
}
