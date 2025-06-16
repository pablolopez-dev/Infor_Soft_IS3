using Word = Microsoft.Office.Interop.Word;
using System.Collections.Generic;
using Microsoft.Office.Interop.Word;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Infor_Soft_WPF.Helpers
{
    public static class WordDocumentHelper
    {
        private static Word.Application wordApp;
        private static Word.Document documento;
        private static bool seGeneroPrimeraPagina = false;

        public static string UltimaRutaGenerada { get; private set; }
        public static string RutaTemporalCopia { get; private set; }

        public static void CrearDocumento(string contenido, List<string> palabrasEnNegrita = null, bool mismoDocumento = false)
        {
            string ruta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Reporte.docx");
            UltimaRutaGenerada = ruta;

            if (!mismoDocumento)
            {
                wordApp = new Word.Application();
                documento = wordApp.Documents.Add();
                seGeneroPrimeraPagina = false;
            }
            else
            {
                if (wordApp == null || documento == null)
                {
                    wordApp = new Word.Application();
                    documento = wordApp.Documents.Add();
                    seGeneroPrimeraPagina = false;
                }

                if (seGeneroPrimeraPagina)
                {
                    object collapseEnd = Word.WdCollapseDirection.wdCollapseEnd;
                    Word.Range rangeFinal = documento.Content;
                    rangeFinal.Collapse(ref collapseEnd);
                    rangeFinal.InsertBreak(Word.WdBreakType.wdPageBreak);
                }
            }

            Word.Range rango = documento.Content;
            rango.Collapse(Word.WdCollapseDirection.wdCollapseEnd);
            Word.Paragraph parrafo = documento.Content.Paragraphs.Add();
            parrafo.Range.Text = contenido;
            parrafo.Alignment = Word.WdParagraphAlignment.wdAlignParagraphJustify;

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

            documento.SaveAs2(ruta);

            if (!wordApp.Visible)
                wordApp.Visible = true;

            seGeneroPrimeraPagina = true;
        }

        public static string CrearCopiaParaBD()
        {
            if (!File.Exists(UltimaRutaGenerada))
                throw new FileNotFoundException("No se encuentra el documento generado.");

            string copiaTemporal = Path.Combine(Path.GetTempPath(), "Reporte_Temporal.docx");
            File.Copy(UltimaRutaGenerada, copiaTemporal, true);
            RutaTemporalCopia = copiaTemporal;
            return copiaTemporal;
        }

        public static void ReiniciarDocumento()
        {
            if (documento != null)
            {
                documento.Close(false);
                Marshal.ReleaseComObject(documento);
                documento = null;
            }

            if (wordApp != null)
            {
                wordApp.Quit();
                Marshal.ReleaseComObject(wordApp);
                wordApp = null;
            }

            seGeneroPrimeraPagina = false;

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
