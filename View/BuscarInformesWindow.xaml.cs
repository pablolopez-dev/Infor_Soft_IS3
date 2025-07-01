using Infor_Soft_WPF.Class.Entidades;
using Infor_Soft_WPF.Class.Repositorios;
using System.Collections.Generic;
using System.Windows;
using Microsoft.Win32;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;

namespace Infor_Soft_WPF.View
{
    public partial class BuscarInformesWindow : Window
    {
        private AbogadoRepositorio _abogadoRepo = new AbogadoRepositorio();
        private InformeRepositorio _informeRepo = new InformeRepositorio();
        private List<InformeResumen> informesTotales = new List<InformeResumen>();

        public BuscarInformesWindow()
        {
            InitializeComponent();
            CargarAbogados();
        }

        private void CargarAbogados()
        {
            var lista = _abogadoRepo.ObtenerAbogados();
            cmbAbogados.ItemsSource = lista;
        }

        private void cmbAbogados_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CargarInformes();
        }

        private void CargarInformes()
        {
            if (cmbAbogados.SelectedItem is Abogado abogado)
            {
                informesTotales = _informeRepo.ObtenerInformesPorAbogado(abogado.Id);
                dgInformes.ItemsSource = informesTotales;
            }
            else
            {
                informesTotales.Clear();
                dgInformes.ItemsSource = null;
            }
        }

       

        private void BtnVerInforme_Click(object sender, RoutedEventArgs e)
        {
            if (dgInformes.SelectedItem is InformeResumen informe)
            {
                var bytes = _informeRepo.ObtenerInformePorId(informe.Id, out string titulo);
                if (bytes != null)
                {
                    string tempPath = Path.Combine(Path.GetTempPath(), titulo);
                    File.WriteAllBytes(tempPath, bytes);
                    Process.Start(new ProcessStartInfo(tempPath) { UseShellExecute = true });
                }
                else
                {
                    MessageBox.Show("No se pudo recuperar el archivo.");
                }
            }
            else
            {
                MessageBox.Show("Seleccione un informe.");
            }
        }

        private void BtnDescargarInforme_Click(object sender, RoutedEventArgs e)
        {
            if (dgInformes.SelectedItem is InformeResumen informe)
            {
                SaveFileDialog saveDialog = new SaveFileDialog
                {
                    FileName = informe.Titulo,
                    Filter = "Word/PDF files|*.docx;*.pdf|All files|*.*"
                };

                if (saveDialog.ShowDialog() == true)
                {
                    var bytes = _informeRepo.ObtenerInformePorId(informe.Id, out _);
                    if (bytes != null)
                    {
                        File.WriteAllBytes(saveDialog.FileName, bytes);
                        MessageBox.Show("Archivo descargado correctamente.");
                    }
                    else
                    {
                        MessageBox.Show("No se pudo recuperar el archivo.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Seleccione un informe.");
            }
        }

        private void BtnEliminarInforme_Click(object sender, RoutedEventArgs e)
        {
            if (dgInformes.SelectedItem is InformeResumen informe)
            {
                var confirmacion = MessageBox.Show($"¿Está seguro que desea eliminar el informe \"{informe.Titulo}\"?",
                                "Confirmar Eliminación",
                                MessageBoxButton.YesNo,
                                MessageBoxImage.Warning);

                if (confirmacion == MessageBoxResult.Yes)
                {
                    if (_informeRepo.EliminarInforme(informe.Id))
                    {
                        MessageBox.Show("Informe eliminado exitosamente.");
                        CargarInformes(); // Recarga
                    }
                    else
                    {
                        MessageBox.Show("Error al eliminar el informe.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Seleccione un informe para eliminar.");
            }
        }
    }
}
