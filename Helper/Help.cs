using Word = Microsoft.Office.Interop.Word;

namespace Infor_Soft_WPF.Helpers
{
    public static class WordDocumentHelper
    {
        public static void CrearDocumento(string contenido)
        {
            var wordApp = new Word.Application();
            var documento = wordApp.Documents.Add();

            Word.Paragraph parrafo = documento.Content.Paragraphs.Add();
            parrafo.Range.Text = contenido;
            parrafo.Range.InsertParagraphAfter();

            wordApp.Visible = true;

            // Opcional: guardar automáticamente
            /*
            string path = @"C:\TuRuta\reporte.docx";
            documento.SaveAs2(path);
            wordApp.Quit();
            */
        }
    }
}
