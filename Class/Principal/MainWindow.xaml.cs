using Infor_Soft_WPF.Class.Repositorios;
using Infor_Soft_WPF.View;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using MySql.Data.MySqlClient;
using Infor_Soft_WPF.Views;

namespace Infor_Soft_WPF
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private bool isSidebarVisible = true;
        private DispatcherTimer sidebarTimer;
        private double sidebarAnimationFrom;
        private double sidebarAnimationTo;
        private DateTime sidebarAnimationStart;
        private const double SidebarAnimationDurationMs = 300;

        private DispatcherTimer _graficoTimer;

        private string _usuarioActual;
        private int _idUsuarioActual;

        public SeriesCollection SeriesCollection { get; set; }
        public List<string> Labels { get; set; }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainWindow(string usuarioLogueado, int idUsuarioLogueado)
        {
            InitializeComponent();

            _usuarioActual = usuarioLogueado;
            _idUsuarioActual = idUsuarioLogueado;

            DataContext = this;

            CargarDatosUsuario();

            // Cargar gráfico la primera vez
            CargarGrafico();

            // Configurar actualizaciones en tiempo real
            IniciarActualizacionGrafico();
        }

        private void GraficoTimer_Tick(object sender, EventArgs e)
        {
            CargarGrafico();
        }


        private void IniciarActualizacionGrafico()
        {
            _graficoTimer = new DispatcherTimer();
            _graficoTimer.Interval = TimeSpan.FromSeconds(10);
            _graficoTimer.Tick += GraficoTimer_Tick;
            _graficoTimer.Start();
        }

        private void CargarGrafico()
        {
            try
            {
                var repo = new InformeRepositorio();
                var datos = repo.ObtenerCantidadInformesPorUsuario();

                Labels = datos.Keys.ToList();
                var valores = new ChartValues<int>(datos.Values);

                SeriesCollection = new SeriesCollection
                {
                    new ColumnSeries
                    {
                        Title = "Informes por Usuario",
                        Values = valores,
                        Fill = new SolidColorBrush(Color.FromRgb(255, 193, 7)), // Amarillo
                        Stroke = new SolidColorBrush(Color.FromRgb(184, 134, 11)),
                        StrokeThickness = 2,
                        MaxColumnWidth = 50,
                        DataLabels = true
                    }
                };

                OnPropertyChanged(nameof(SeriesCollection));
                OnPropertyChanged(nameof(Labels));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando gráfico: " + ex.Message);
            }
        }

        private void CargarDatosUsuario()
        {
            try
            {
                string connectionString = "server=localhost;user=root;password=;database=inforsoft;port=3306";

                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                        SELECT u.nombre, m.juzgado_de_paz
                        FROM usuarios u
                        INNER JOIN matricula m ON u.matricula = m.matricula_id
                        WHERE u.id_usuario = @idUsuario;";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@idUsuario", _idUsuarioActual);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string nombre = reader["nombre"]?.ToString() ?? "";
                                string juzgado = reader["juzgado_de_paz"]?.ToString() ?? "";

                                txtNombreUsuarioSidebar.Text = $"Bienvenido: {nombre}";
                                txtJuzgadoSidebar.Text = $"{juzgado}";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando datos del usuario: " + ex.Message);
            }
        }

        private void Reportes_Click(object sender, RoutedEventArgs e)
        {
            Window1 ventana = new Window1(_usuarioActual, _idUsuarioActual);
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
            LoginView login = new LoginView();
            login.Show();
            this.Close();
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

        private void Cobertura_Click(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Navigate(new CoberturaView());
        }

        private void AbrirFactura_Click(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Content = new ComprobanteUjierView(_usuarioActual, _idUsuarioActual);
        }

        private void AnimateSidebar(double from, double to)
        {
            var animation = new System.Windows.Media.Animation.DoubleAnimation
            {
                From = from,
                To = to,
                Duration = new Duration(TimeSpan.FromMilliseconds(300)),
                EasingFunction = new System.Windows.Media.Animation.CubicEase
                {
                    EasingMode = System.Windows.Media.Animation.EasingMode.EaseInOut
                }
            };

            SidebarColumn.BeginAnimation(ColumnDefinition.WidthProperty, animation);
        }

        private void SidebarTimer_Tick(object sender, EventArgs e)
        {
            double elapsedMs = (DateTime.Now - sidebarAnimationStart).TotalMilliseconds;
            double progress = Math.Min(elapsedMs / SidebarAnimationDurationMs, 1.0);

            progress = EaseInOutCubic(progress);

            double current = sidebarAnimationFrom + (sidebarAnimationTo - sidebarAnimationFrom) * progress;
            SidebarColumn.Width = new GridLength(current);

            if (progress >= 1.0)
            {
                sidebarTimer.Stop();
            }
        }

        private double EaseInOutCubic(double t)
        {
            return t < 0.5
                ? 4 * t * t * t
                : 1 - Math.Pow(-2 * t + 2, 3) / 2;
        }

        private void ToggleSidebar_Click(object sender, RoutedEventArgs e)
        {
            sidebarAnimationFrom = SidebarColumn.ActualWidth;
            sidebarAnimationTo = isSidebarVisible ? 0 : 250;
            sidebarAnimationStart = DateTime.Now;

            if (sidebarTimer == null)
            {
                sidebarTimer = new DispatcherTimer();
                sidebarTimer.Interval = TimeSpan.FromMilliseconds(16);
                sidebarTimer.Tick += SidebarTimer_Tick;
            }

            sidebarTimer.Start();
            isSidebarVisible = !isSidebarVisible;
        }
    }
}
