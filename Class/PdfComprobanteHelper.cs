using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using Infor_Soft_WPF.View;
using MigraDoc.DocumentObjectModel.Shapes;

namespace Infor_Soft_WPF.Helpers
{
    public class PdfComprobanteHelper
    {
        public void GenerarPdf(ComprobanteUjierModel model)
        {
            var doc = new Document();
            var section = doc.AddSection();

            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string logoPath = Path.Combine(baseDir, "Imagenes", "logo_pj.png");
            string compromisoPath = Path.Combine(baseDir, "Imagenes", "Compromiso.jpg");

            if (File.Exists(logoPath))
            {
                var logo = section.AddImage(logoPath);
                logo.LockAspectRatio = true;
                logo.Width = "4cm";
                logo.Left = ShapePosition.Left;
            }

            if (File.Exists(compromisoPath))
            {
                var compromiso = section.AddImage(compromisoPath);
                compromiso.LockAspectRatio = true;
                compromiso.Width = "4cm";
                compromiso.Left = ShapePosition.Right;
            }

            section.AddParagraph().AddLineBreak();

            // --- Título ---
            var titulo = section.AddParagraph("Factura Ujier");
            titulo.Format.Font.Size = 18;
            titulo.Format.Font.Bold = true;
            titulo.Format.SpaceAfter = "1cm";
            titulo.Format.Font.Color = Colors.DarkOrange;
            titulo.Format.Alignment = ParagraphAlignment.Center;

            // --- Datos del Cliente en recuadro ---
            var clienteTable = section.AddTable();
            clienteTable.Borders.Width = 1;
            clienteTable.AddColumn("16cm");

            var clienteRow = clienteTable.AddRow();
            var clienteCell = clienteRow.Cells[0];
            clienteCell.AddParagraph("Datos del Cliente").Format.Font.Bold = true;
            clienteCell.AddParagraph($"Nombre: {model.Cliente}");
            clienteCell.AddParagraph($"Documento: {model.Documento}");
            clienteCell.Format.SpaceAfter = "0.5cm";

            // --- Datos del Expediente en recuadro ---
            var expedienteTable = section.AddTable();
            expedienteTable.Borders.Width = 1;
            expedienteTable.AddColumn("16cm");

            var expedienteRow = expedienteTable.AddRow();
            var expedienteCell = expedienteRow.Cells[0];
            expedienteCell.AddParagraph("Datos del Expediente").Format.Font.Bold = true;
            expedienteCell.AddParagraph($"Número: {model.NumeroExpediente}");
            expedienteCell.AddParagraph($"Año: {model.AnioExpediente}");
            expedienteCell.Format.SpaceAfter = "0.5cm";

            // --- Tabla de Oficios con bordes ---
            section.AddParagraph("Detalle de Oficios Comisivos").Format.Font.Bold = true;

            var table = section.AddTable();
            table.Borders.Width = 0.75;
            table.AddColumn("6cm");
            table.AddColumn("4cm");
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

            section.AddParagraph().Format.SpaceAfter = "0.5cm";

            // --- Totales en recuadro ---
            var totalTable = section.AddTable();
            totalTable.Borders.Width = 1;
            totalTable.AddColumn("16cm");

            var totalRow = totalTable.AddRow();
            var totalCell = totalRow.Cells[0];
            totalCell.AddParagraph("Total Factura: " + model.Total).Format.Font.Bold = true;
            totalCell.Format.SpaceAfter = "0.5cm";

            // --- Liquidación en recuadro ---
            var liquidacionTable = section.AddTable();
            liquidacionTable.Borders.Width = 1;
            liquidacionTable.AddColumn("16cm");

            var liquidacionRow = liquidacionTable.AddRow();
            var liquidacionCell = liquidacionRow.Cells[0];
            liquidacionCell.AddParagraph($"Fecha de Liquidación: {model.FechaLiquidacion:dd/MM/yyyy}");
            liquidacionCell.AddParagraph($"N° Liquidación: {model.NumeroLiquidacion}");

            // --- Guardar PDF con diálogo para elegir ruta ---
            var dlg = new SaveFileDialog
            {
                Filter = "PDF Files (*.pdf)|*.pdf",
                FileName = "ComprobanteUjier.pdf",
                InitialDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads")
            };

            if (dlg.ShowDialog() == true)
            {
                try
                {
                    var renderer = new PdfDocumentRenderer(true) { Document = doc };
                    renderer.RenderDocument();
                    renderer.PdfDocument.Save(dlg.FileName);

                    MessageBox.Show($"PDF guardado correctamente en:\n{dlg.FileName}", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (UnauthorizedAccessException)
                {
                    MessageBox.Show("No se tiene permiso para guardar en la ruta seleccionada.", "Error de acceso", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al guardar el PDF: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
