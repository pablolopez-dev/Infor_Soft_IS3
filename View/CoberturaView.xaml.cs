using Infor_Soft_WPF.Class.Actividad;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Word = Microsoft.Office.Interop.Word;


namespace Infor_Soft_WPF.Views
{
    public partial class CoberturaView : UserControl
    {
        private ObservableCollection<ActividadCobertura> registros = new ObservableCollection<ActividadCobertura>();
        private int contador = 1;

        private int _idUsuarioActual;

        public CoberturaView()
        {
            InitializeComponent();
            dgActividades.ItemsSource = registros;
        }

        private void AgregarActividad_Click(object sender, RoutedEventArgs e)
        {
            if (cmbDestino.SelectedItem is ComboBoxItem item)
            {
                string destinoSeleccionado = item.Content.ToString();
                DateTime? fechaSeleccionada = dpFechaSolicitud.SelectedDate;

               

                if (fechaSeleccionada == null)
                {
                    MessageBox.Show("Debe seleccionar la Fecha de Notificación.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Intentamos extraer el monto del texto del destino
                string montoStr = "";
                var montoInicio = destinoSeleccionado.IndexOf('(');
                var montoFin = destinoSeleccionado.IndexOf(')');

                if (montoInicio != -1 && montoFin != -1)
                {
                    montoStr = destinoSeleccionado.Substring(montoInicio + 1, montoFin - montoInicio - 1);
                }

                registros.Add(new ActividadCobertura
                {
                    Numero = contador++,
                    Destino = destinoSeleccionado,
                    Monto = montoStr,
                    Fecha = fechaSeleccionada.Value.ToString("dd/MM/yyyy")
                });
            }
            else
            {
                MessageBox.Show("Seleccione un destino válido.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private bool ValidarFormulario()
        {
            var errores = new System.Collections.Generic.List<string>();

            if (registros.Count == 0)
                errores.Add("Debe cargar al menos una actividad.");

            if (errores.Any())
            {
                MessageBox.Show(string.Join("\n", errores), "Errores de Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }



        private void GuardarWordEnBaseDeDatos(string filePath, int idUsuario)
        {
            string connectionString = "server=localhost;user=root;password=;database=inforsoft;port=3306";

            byte[] fileBytes = File.ReadAllBytes(filePath);

            using (var conn = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
            {
                conn.Open();

                string query = @"INSERT INTO registros_comisivos (registro_blob, id_usuario) 
                         VALUES (@blob, @idUsuario)";

                using (var cmd = new MySql.Data.MySqlClient.MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@blob", fileBytes);
                    cmd.Parameters.AddWithValue("@idUsuario", idUsuario);

                    cmd.ExecuteNonQuery();
                }
            }
        }


        private void GenerarPDF_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidarFormulario())
                return;

            GenerarDocumentoWord();
        }

        private void GenerarDocumentoWord()
        {
            try
            {
                if (registros.Count == 0)
                {
                    MessageBox.Show("Debe cargar al menos una actividad para generar el Word.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var saveDialog = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "Documento Word|*.docx",
                    FileName = $"Registro_Oficios_Comisivos_{DateTime.Now:yyyyMMdd}.docx"
                };

                if (saveDialog.ShowDialog() != true)
                    return;

                string filePath = saveDialog.FileName;

                var wordApp = new Word.Application();
                var doc = wordApp.Documents.Add();
                // Establecer orientación horizontal en el documento y en la primera sección
                doc.PageSetup.Orientation = Word.WdOrientation.wdOrientLandscape;
                doc.Sections[1].PageSetup.Orientation = Word.WdOrientation.wdOrientLandscape;


                wordApp.Visible = false;

                // TÍTULO
                var titulo = doc.Content.Paragraphs.Add();
                titulo.Range.Text = "REGISTRO DE OFICIOS COMISIVOS";
                titulo.Range.Font.Size = 16;
                titulo.Range.Font.Bold = 1;
                titulo.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                titulo.Range.InsertParagraphAfter();

                // SALTO DE LÍNEA
                var p = doc.Content.Paragraphs.Add();
                p.Range.Text = "";
                p.Range.InsertParagraphAfter();

                // TABLA
                int rows = registros.Count + 1;
                int cols = 5;
                var tabla = doc.Tables.Add(doc.Bookmarks.get_Item("\\endofdoc").Range, rows, cols);

                tabla.Range.ParagraphFormat.SpaceAfter = 6;
                tabla.Borders.Enable = 1;

                // Ajustar anchos de columnas para landscape
                tabla.Columns[1].Width = 40;   // N°
                tabla.Columns[2].Width = 300;  // Autos Caratulados
                tabla.Columns[3].Width = 250;  // Destino
                tabla.Columns[4].Width = 100;  // Monto
                tabla.Columns[5].Width = 100;  // Fecha

                // ENCABEZADOS
                tabla.Cell(1, 1).Range.Text = "N°";
                tabla.Cell(1, 2).Range.Text = "Autos Caratulados";
                tabla.Cell(1, 3).Range.Text = "Destino";
                tabla.Cell(1, 4).Range.Text = "Monto";
                tabla.Cell(1, 5).Range.Text = "Fecha";

                for (int j = 1; j <= cols; j++)
                {
                    tabla.Cell(1, j).Range.Bold = 1;
                    tabla.Cell(1, j).Range.Shading.BackgroundPatternColor = Word.WdColor.wdColorGray20;
                    tabla.Cell(1, j).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                }


                // FILAS DE DATOS
                for (int i = 0; i < registros.Count; i++)
                {
                    var r = registros[i];

                    tabla.Cell(i + 2, 1).Range.Text = r.Numero.ToString();
                    tabla.Cell(i + 2, 2).Range.Text = r.AutosCaratulados;
                    tabla.Cell(i + 2, 3).Range.Text = r.Destino;
                    tabla.Cell(i + 2, 4).Range.Text = r.Monto;
                    tabla.Cell(i + 2, 5).Range.Text = r.Fecha;
                }

                doc.SaveAs2(filePath);
               // doc.Close();
                wordApp.Quit();

                MessageBox.Show($"Documento Word generado correctamente en:\n{filePath}", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                // Suponiendo que tengas _idUsuarioActual disponible en tu clase

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar el documento Word:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            

        }


    }
}
