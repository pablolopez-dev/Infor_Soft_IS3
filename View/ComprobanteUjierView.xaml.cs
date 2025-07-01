using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MigraDoc.DocumentObjectModel.Shapes;
using PdfSharp.Snippets.Drawing;

namespace Infor_Soft_WPF.View
{
    public partial class ComprobanteUjierView : UserControl
    {
        private List<Diligencia> diligencias = new List<Diligencia>();

        private string _usuarioActual;
        private int _idUsuarioActual; // Cambiar a int (no string)
        private string _juzgadoUsuario;


        public ComprobanteUjierView(string usuarioLogueado, int idUsuarioLogueado)
        {
            InitializeComponent();
            _usuarioActual = usuarioLogueado;
            _idUsuarioActual = idUsuarioLogueado;  // Falta punto y coma aquí
            DataContext = this;
            CargarDatosUsuario();  // << Aquí cargas nombre y juzgado
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
                                _juzgadoUsuario = juzgado;

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

        private void Agregar_Click(object sender, RoutedEventArgs e)
        {
            if (!decimal.TryParse(txtMonto.Text, out var monto) || monto <= 0 ||
                !double.TryParse(txtDistancia.Text, out var distancia) || distancia < 0)
            {
                MessageBox.Show("Por favor, verificá los valores de Monto y Distancia.", "Datos Inválidos", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDescripcion.Text) || string.IsNullOrWhiteSpace(txtDestino.Text))
            {
                MessageBox.Show("Completá la descripción y el destino.", "Campos Vacíos", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var nuevaDiligencia = new Diligencia
            {
                Descripcion = txtDescripcion.Text.Trim(),
                Destino = txtDestino.Text.Trim(),
                DistanciaKm = distancia,
                Monto = monto
            };

            diligencias.Add(nuevaDiligencia);
            dgDiligencias.Items.Add(nuevaDiligencia);

            txtDescripcion.Clear();
            txtDestino.Clear();
            txtDistancia.Clear();
            txtMonto.Clear();

            CalcularTotal();
        }

        private void CalcularTotal()
        {
            decimal total = diligencias.Sum(d => d.Monto);
            txtTotalMonto.Text = total.ToString("N0");
        }

        private void GenerarComprobante_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCliente.Text) || string.IsNullOrWhiteSpace(txtDocumento.Text))
            {
                MessageBox.Show("Completa los datos.", "Datos incompletos", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!diligencias.Any())
            {
                MessageBox.Show("Agrega información para generar el comprobante.", "Campos vacios", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var comprobanteModel = new ComprobanteUjierModel
            {
                Cliente = txtCliente.Text.Trim(),
                Documento = txtDocumento.Text.Trim(),
                NumeroExpediente = txtNumeroExpediente.Text.Trim(),
                AnioExpediente = txtAnioExpediente.Text.Trim(),
                Oficios = diligencias.Select(d => new OficioComisivo
                {
                    Descripcion = d.Descripcion,
                    Destino = d.Destino,
                    DistanciaKm = d.DistanciaKm.ToString("N2"),
                    Monto = d.Monto.ToString("N0")
                }).ToList(),
                Total = txtTotalMonto.Text,
                FechaLiquidacion = dpFechaLiquidacion.SelectedDate ?? DateTime.Now,
                NumeroLiquidacion = txtNumeroLiquidacion.Text.Trim()
            };

            var ventanaDetalle = new Window
            {
                Title = "Detalle del Comprobante",
                Content = new ComprobanteUjierDetalleControl(_usuarioActual, _idUsuarioActual)
                {
                    DataContext = comprobanteModel
                },
                Owner = Window.GetWindow(this),
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            ventanaDetalle.ShowDialog();
        }

        // ✅ Método fuera de cualquier otro método
        private void GenerarPdfComprobante(ComprobanteUjierModel model)
        {
            var doc = new Document();
            var section = doc.AddSection();

            // Márgenes reducidos para simular "marco"
            section.PageSetup.TopMargin = "1.5cm";
            section.PageSetup.BottomMargin = "1.5cm";
            section.PageSetup.LeftMargin = "1.5cm";
            section.PageSetup.RightMargin = "1.5cm";

            // --- TABLA DE BORDE PARA EL "MARCO" ---
            var borderTable = section.AddTable();
            borderTable.Borders.Width = 3;
            borderTable.Borders.Color = Colors.DarkOrange;
            borderTable.AddColumn("100%");
            var borderRow = borderTable.AddRow();
            var borderCell = borderRow.Cells[0];
            borderCell.Shading.Color = Colors.White;

            // --- Stack de contenido dentro del "marco" ---
            var innerSection = borderCell.Elements.AddParagraph();
            innerSection.Format.SpaceAfter = "0.5cm";

            // Título
            innerSection.AddFormattedText("Comprobante Ujier", TextFormat.Bold);
            innerSection.AddLineBreak();
            innerSection.Format.Font.Size = 18;
            innerSection.Format.Font.Color = Colors.DarkOrange;
            innerSection.Format.Alignment = ParagraphAlignment.Center;
            innerSection.AddLineBreak();

            // Datos del Usuario
            innerSection.AddFormattedText($"Usuario: {_usuarioActual}", TextFormat.Bold);
            innerSection.AddLineBreak();
            innerSection.AddFormattedText($"Juzgado: {_juzgadoUsuario}", TextFormat.Bold);
            innerSection.AddLineBreak();
            innerSection.AddLineBreak();

            // Datos del Cliente
            innerSection.AddFormattedText("Datos del Cliente", TextFormat.Bold);
            innerSection.AddLineBreak();
            innerSection.AddText($"Nombre: {model.Cliente}");
            innerSection.AddLineBreak();
            innerSection.AddText($"Documento: {model.Documento}");
            innerSection.AddLineBreak();
            innerSection.AddLineBreak();

            // Expediente
            innerSection.AddFormattedText("Datos del Expediente", TextFormat.Bold);
            innerSection.AddLineBreak();
            innerSection.AddText($"Número: {model.NumeroExpediente}");
            innerSection.AddLineBreak();
            innerSection.AddText($"Año: {model.AnioExpediente}");
            innerSection.AddLineBreak();
            innerSection.AddLineBreak();

            // Detalle de Oficios
            innerSection.AddFormattedText("Detalle de Oficios Comisivos", TextFormat.Bold);
            innerSection.AddLineBreak();

            // --- Tabla de Oficios ---
            var table = borderCell.Elements.AddTable();
            table.Borders.Width = 0.75;
            table.AddColumn("4cm");
            table.AddColumn("3cm");
            table.AddColumn("3cm");
            table.AddColumn("3cm");

            var header = table.AddRow();
            header.Shading.Color = Colors.LightGray;
            header.Cells[0].AddParagraph("Descripción");
            header.Cells[1].AddParagraph("Destino");
            header.Cells[2].AddParagraph("Distancia (Km)");
            header.Cells[3].AddParagraph("Monto");

            foreach (var oficio in model.Oficios)
            {
                var row = table.AddRow();
                row.Cells[0].AddParagraph(oficio.Descripcion);
                row.Cells[1].AddParagraph(oficio.Destino);
                row.Cells[2].AddParagraph(oficio.DistanciaKm);
                row.Cells[3].AddParagraph(oficio.Monto);
            }

            innerSection.AddLineBreak();
            innerSection.AddFormattedText($"Total Factura: {model.Total}", TextFormat.Bold);
            innerSection.AddLineBreak();
            innerSection.AddLineBreak();

            // Liquidación
            innerSection.AddText($"Fecha de Liquidación: {model.FechaLiquidacion:dd/MM/yyyy}");
            innerSection.AddLineBreak();
            innerSection.AddText($"N° Liquidación: {model.NumeroLiquidacion}");
            innerSection.AddLineBreak();

            // --- PIE DE PÁGINA ---
            var footer = section.Footers.Primary.AddParagraph();
            footer.AddText("hecho en inforsoft");
            footer.Format.Font.Size = 8;
            footer.Format.Font.Color = Colors.Gray;
            footer.Format.Alignment = ParagraphAlignment.Right;

            // --- Guardar PDF ---
            var dlg = new SaveFileDialog
            {
                Filter = "PDF Files (*.pdf)|*.pdf",
                FileName = "ComprobanteUjier.pdf"
            };

            if (dlg.ShowDialog() == true)
            {
                var renderer = new PdfDocumentRenderer(true);
                renderer.Document = doc;
                renderer.RenderDocument();
                renderer.PdfDocument.Save(dlg.FileName);
            }
        }




    }

    // Modelos
    public class Diligencia
    {
        public string Descripcion { get; set; }
        public string Destino { get; set; }
        public double DistanciaKm { get; set; }
        public decimal Monto { get; set; }
    }

    public class OficioComisivo
    {
        public string Descripcion { get; set; }
        public string Destino { get; set; }
        public string DistanciaKm { get; set; }
        public string Monto { get; set; }
    }

    public class ComprobanteUjierModel
    {
        public string Cliente { get; set; }
        public string Documento { get; set; }
        public string NumeroExpediente { get; set; }
        public string AnioExpediente { get; set; }
        public List<OficioComisivo> Oficios { get; set; }
        public string Total { get; set; }
        public DateTime FechaLiquidacion { get; set; }
        public string NumeroLiquidacion { get; set; }
    }
}
