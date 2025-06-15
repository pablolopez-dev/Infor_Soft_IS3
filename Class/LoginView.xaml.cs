using System.Windows;
using System.Windows.Input;

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
            string username = txtUser.Text;
            string password = txtPass.Password;

            if (username == "admin" && password == "1234")  // Ejemplo de validación simple
            {

                //Abrir el dashboard
                MainWindow dashboard = new MainWindow();
             
                dashboard.Show();  // Mostrar la ventana ReportesView

                // Cerrar la ventana de inicio de sesión
                this.Close();
            }
            else
            {
                MessageBox.Show("Usuario o contraseña incorrectos", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txtUser_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }
    }
}
