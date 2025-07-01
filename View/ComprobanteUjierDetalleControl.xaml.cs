using Infor_Soft_WPF.Helpers;
using QuestPDF.Fluent;
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

namespace Infor_Soft_WPF.View
{
    /// <summary>
    /// Lógica de interacción para ComprobanteUjierDetalleControl.xaml
    /// </summary>
    
    public partial class ComprobanteUjierDetalleControl : UserControl
    {

        private string _usuarioActual;
        private int _idUsuarioActual; // Cambiar a int (no string)
        public ComprobanteUjierDetalleControl(string usuarioLogueado, int idUsuarioLogueado)
        {
            InitializeComponent();
            _usuarioActual = usuarioLogueado;
            _idUsuarioActual = idUsuarioLogueado; 
            DataContext = this;
            CargarDatosUsuario();  // << Aquí cargas nombre y juzgado
        }

        private void Cerrar_Click(object sender, RoutedEventArgs e)
        {
            // Busca la ventana que contiene este control y la cierra
            Window ventana = Window.GetWindow(this);
            ventana?.Close();
        }

        private void Imprimir_Click(object sender, RoutedEventArgs e)
        {
            // Ejecuta el proceso de impresión
            Window ventana = Window.GetWindow(this);
            if (ventana != null)
            {
                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    // Imprime el contenido del control
                    printDialog.PrintVisual(this, "Comprobante Ujier");
                }
            }


        }

        private void GenerarPdf_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is ComprobanteUjierModel model)
            {
                var helper = new PdfComprobanteHelper();
                helper.GenerarPdf(model);
            }
            else
            {
                MessageBox.Show("No hay datos para exportar.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }



        private void ExportarComprobanteAPdf(ComprobanteUjierModel model)
        {
            // Aquí llamas a tu lógica para generar PDF
            var pdfHelper = new PdfComprobanteHelper();
            pdfHelper.GenerarPdf(model);
        }


        private void CargarDatosUsuario()
        {
            try
            {
                // Cadena conexión
                string connectionString = "server=localhost;user=root;password=;database=inforsoft;port=3306";

                using (var conn = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
                {
                    conn.Open();

                    // Consulta que une usuarios con matricula para traer nombre y juzgado
                    string query = @"
                SELECT u.nombre, m.juzgado_de_paz
                FROM usuarios u
                INNER JOIN matricula m ON u.matricula = m.matricula_id
                WHERE u.id_usuario = @idUsuario;";

                    using (var cmd = new MySql.Data.MySqlClient.MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@idUsuario", _idUsuarioActual);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string nombre = reader["nombre"]?.ToString() ?? "";
                                string juzgado = reader["juzgado_de_paz"]?.ToString() ?? "";

                                txtNombreUsuarioSidebar.Text = $"{nombre}";
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



    }
}
