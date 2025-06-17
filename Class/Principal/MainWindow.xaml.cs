using Infor_Soft_WPF.Class.Repositorios;
using Infor_Soft_WPF.View;
using LiveCharts;
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
using LiveCharts;
using LiveCharts.Wpf;
using System.Windows;
using System.Collections.Generic;
using System;
using System.Windows.Controls;
using Infor_Soft_WPF.Class.Repositorios;

using System.Linq;

namespace Infor_Soft_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _usuarioActual;
        private int _idUsuarioActual; // Cambiar a int (no string)
        public SeriesCollection SeriesCollection { get; set; }
        public ChartValues<int> Valores { get; set; }
        public List<string> Labels { get; set; }



        public MainWindow(string usuarioLogueado, int idUsuarioLogueado)
        {
            InitializeComponent();
            _usuarioActual = usuarioLogueado;
            _idUsuarioActual = idUsuarioLogueado;  // Falta punto y coma aquí
            DataContext = this;

            CargarGrafico("día");

        }

        private void Reportes_Click(object sender, RoutedEventArgs e)
        {
            Window1 ventana = new Window1(_usuarioActual, _idUsuarioActual); // Corregir paréntesis y tipo
            ventana.Show();
            this.Close();
        }

        private void AbrirBuscarInformes(object sender, RoutedEventArgs e)
        {
            var ventana = new BuscarInformesWindow();
            ventana.Owner = this;
            ventana.ShowDialog();
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

        private void cmbFiltro_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbFiltro.SelectedItem is ComboBoxItem selectedItem)
            {
                string filtro = selectedItem.Content.ToString();
                if (filtro.Contains("Día")) CargarGrafico("día");
                else if (filtro.Contains("Mes")) CargarGrafico("mes");
                else if (filtro.Contains("Año")) CargarGrafico("año");
            }
        }


        private void CargarGrafico(string filtro)
        {
            var repo = new InformeRepositorio();
            Dictionary<string, int> datos = filtro switch
            {
                "mes" => repo.ObtenerCantidadInformesPorMes(),
                "año" => repo.ObtenerCantidadInformesPorAño(),
                _ => repo.ObtenerCantidadInformesPorDia()
            };

            Labels = datos.Keys.ToList();
            Valores = new ChartValues<int>(datos.Values);

            DataContext = null; // Refrescar el binding
            DataContext = this;
        }


    }
}
