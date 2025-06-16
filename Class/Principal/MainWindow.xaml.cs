using Infor_Soft_WPF.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Infor_Soft_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _usuarioActual;
        private int _idUsuarioActual; // Cambiar a int (no string)

        public MainWindow(string usuarioLogueado, int idUsuarioLogueado)
        {
            InitializeComponent();
            _usuarioActual = usuarioLogueado;
            _idUsuarioActual = idUsuarioLogueado;  // Falta punto y coma aquí
        }

        private void Reportes_Click(object sender, RoutedEventArgs e)
        {
            Window1 ventana = new Window1(_usuarioActual, _idUsuarioActual); // Corregir paréntesis y tipo
            ventana.Show();
            this.Close();
        }


        private void CerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            // Lógica para cerrar sesión
            LoginView login = new LoginView();
            login.Show();
            this.Close(); // Cierra la ventana actual
        }

        private void Minimizar_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Maximizar_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = (this.WindowState == WindowState.Normal) ? WindowState.Maximized : WindowState.Normal;
        }

        private void Cerrar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
