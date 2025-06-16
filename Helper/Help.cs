using Word = Microsoft.Office.Interop.Word;
using System.Collections.Generic;

namespace Infor_Soft_WPF.Helpers
{
    public static class WordDocumentHelper
    {
        public static void CrearDocumento(string contenido, List<string> palabrasEnNegrita = null)
        {
            var wordApp = new Word.Application();
            var documento = wordApp.Documents.Add();

            Word.Paragraph parrafo = documento.Content.Paragraphs.Add();
            parrafo.Range.Text = contenido;

            // ✅ Justificar el texto
            parrafo.Alignment = Word.WdParagraphAlignment.wdAlignParagraphJustify;

            // ✅ Aplicar negrita a palabras específicas
            if (palabrasEnNegrita != null)
            {
                foreach (var palabra in palabrasEnNegrita)
                {
                    Word.Find find = parrafo.Range.Find;
                    find.ClearFormatting();
                    find.Text = palabra;
                    find.Replacement.ClearFormatting();
                    find.Replacement.Font.Bold = 1;
                    find.Execute(Replace: Word.WdReplace.wdReplaceAll);
                }
            }

            parrafo.Range.InsertParagraphAfter();

            // Mostrar el documento
            wordApp.Visible = true;
        }
    }
}
