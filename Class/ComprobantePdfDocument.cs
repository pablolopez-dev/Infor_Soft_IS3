using Infor_Soft_WPF.View;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;

public class ComprobantePdfDocument : IDocument
{
    private readonly ComprobanteUjierModel _model;

    public ComprobantePdfDocument(ComprobanteUjierModel model)
    {
        _model = model;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Margin(30);
            page.Size(PageSizes.A4);
            page.PageColor(Colors.White);
            page.DefaultTextStyle(x => x.FontSize(12));

            page.Header()
                .Text("Comprobante Ujier")
                .FontSize(20)
                .Bold()
                .FontColor(Colors.Brown.Medium);

            page.Content()
                .PaddingVertical(10)
                .Column(column =>
                {
                    // Datos Cliente
                    column.Item().Text($"Cliente: {_model.Cliente}").Bold();
                    column.Item().Text($"Documento: {_model.Documento}");
                    column.Item().Text($"Expediente: {_model.NumeroExpediente} / {_model.AnioExpediente}");
                    column.Item().Text($"Fecha Liquidación: {_model.FechaLiquidacion:dd/MM/yyyy}");
                    column.Item().Text($"N° Liquidación: {_model.NumeroLiquidacion}");

                    column.Item().PaddingVertical(10).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                    // Tabla Oficios
                    column.Item().Table(table =>
                    {
                        // Columnas
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(3);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(1);
                            columns.RelativeColumn(1);
                        });

                        // Header
                        table.Header(header =>
                        {
                            header.Cell().Text("Descripción").FontSize(14).Bold();
                            header.Cell().Text("Destino").FontSize(14).Bold();
                            header.Cell().Text("Distancia (Km)").FontSize(14).Bold();
                            header.Cell().Text("Monto").FontSize(14).Bold();
                        });

                        // Rows
                        foreach (var oficio in _model.Oficios)
                        {
                            table.Cell().Text(oficio.Descripcion);
                            table.Cell().Text(oficio.Destino);
                            table.Cell().Text(oficio.DistanciaKm);
                            table.Cell().Text(oficio.Monto);
                        }
                    });

                    column.Item().PaddingTop(10).AlignRight().Text($"Total: {_model.Total}").Bold().FontSize(14);
                });

            page.Footer()
                .AlignCenter()
                .Text(x =>
                {
                    x.Span("Generado con Infor Soft - ");
                    x.CurrentPageNumber();
                    x.Span(" / ");
                    x.TotalPages();
                });
        });
    }
}
