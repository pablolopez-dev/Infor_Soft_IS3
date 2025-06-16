using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using Infor_Soft_WPF.Class.BD;
using Infor_Soft_WPF.Class.Login_Register;
using MySql.Data.MySqlClient;

namespace Infor_Soft_WPF.View
{
    public partial class LoginView : Window
    {


        public LoginView()
        {
            InitializeComponent();

            this.MouseDown += Window_MouseDown;
            btnMinimize.Click += btnMinimize_Click;
            btnClose.Click += btnClose_Click;
            btnLogin.Click += btnLogin_Click;
        }

        // Permite mover la ventana arrastrándola con el mouse
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        // Minimiza la ventana
        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        // Cierra la ventana
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // Evento para el botón de inicio de sesión
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUser.Text.Trim();
            string password = txtPass.Password;

            if (ValidarUsuario(username, password, out int idUsuario))
            {
                MainWindow dashboard = new MainWindow(username, idUsuario); // ✅ PASÁS usuario e ID
                dashboard.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Usuario o contraseña incorrectos", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        // Método para generar hash SHA256
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

        // Método para validar usuario y contraseña contra la base de datos
        private bool ValidarUsuario(string username, string password, out int idUsuario)
        {
            bool valido = false;
            idUsuario = -1;

            var db = new BD_CONN();
            var conn = db.GetConnection();

            try
            {
                db.OpenConnection();
                string query = "SELECT id_usuario, contraseña FROM usuarios WHERE usuario=@user";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@user", username);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string hashEnBD = reader["contraseña"].ToString();
                            string hashIngresado = GenerarHashSHA256(password);

                            if (hashEnBD == hashIngresado)
                            {
                                valido = true;
                                idUsuario = Convert.ToInt32(reader["id_usuario"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al validar el usuario: " + ex.Message);
            }
            finally
            {
                db.CloseConnection();
            }

            return valido;
        }


        private void txtUser_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            // Puedes implementar lógica si quieres reaccionar a cambios de texto
        }

        private void btnIrARegistro_Click(object sender, RoutedEventArgs e)
        {
            // Abrir el registro
            Page1 registro = new Page1();
            registro.Show();

                // Cerrar la ventana de inicio de sesión
                this.Close();
        }


    }
}
