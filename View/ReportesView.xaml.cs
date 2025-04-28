using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Infor_Soft_WPF.View
{
    public partial class Window1 : Window
    {
        private readonly Dictionary<string, List<string>> subopcionesDict = new()
        {
            { "Providencia", new() { "Con traslado", "Sin Traslado", "No encontrado", "No Adherido" } },
            { "A.I.", new() { "Adherido Porton", "Adherido Puerta", "No Adherido", "No encontrado", "Recibido sin especificar Mujer", "Recibido sin especificar Hombre" } },
            { "S.D.", new() { "Con Aviso", "Sin Aviso", "No encontrado", "No Adherido" } },
            { "Oficio Comisivo", new() { "Con traslado", "Sin Traslado", "No encontrado", "No Adherido" } }
        };

        private readonly List<string> subSubOpciones = new()
        {
            "Adherido Porton", "Adherido Puerta", "No Adherido",
            "Recibido sin especificar Mujer", "Recibido sin especificar Hombre"
        };

        private readonly Dictionary<string, List<string>> condicionesSubSub = new()
        {  { "Providencia", new() { "Con traslado", "Sin Traslado", "No encontrado" } },
    { "A.I.", new() }, // sin subopciones
    { "S.D.", new() { "Con Aviso", "Sin Aviso", "No encontrado" } },
    { "Oficio Comisivo", new() { "Con traslado", "Sin Traslado", "No encontrado" } }
        };

        public Window1()
        {
            InitializeComponent();
        }

        private void comboTipo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedTipo = (comboTipo.SelectedItem as ComboBoxItem)?.Content as string;

            listaSubopciones.Items.Clear();
            listaSubSubopciones.ItemsSource = null;
            listaSubSubopciones.Visibility = Visibility.Collapsed;

            panelAdherido.Visibility = Visibility.Collapsed;
            panelCamposNormales.Visibility = Visibility.Collapsed;
            panelCamposAdicionales.Visibility = Visibility.Collapsed;

            if (!string.IsNullOrEmpty(selectedTipo))
            {
                if (subopcionesDict.ContainsKey(selectedTipo))
                {
                    foreach (var sub in subopcionesDict[selectedTipo])
                    {
                        listaSubopciones.Items.Add(sub);
                    }

                    // Si el tipo es A.I., mostrar directamente dropdown de adheridos
                    if (selectedTipo == "A.I.")
                    {
                        panelAdherido.Visibility = Visibility.Visible;
                        panelCamposNormales.Visibility = Visibility.Visible;
                        comboAdherido.SelectedIndex = -1; // Limpiar selección previa
                    }
                }
            }
        }



        private void listaSubopciones_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MostrarSubSubOpciones();
            VerificarMostrarCampos();
        }
        private void comboAdherido_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string opcion = (comboAdherido.SelectedItem as ComboBoxItem)?.Content as string;

            if (opcion == "No Adherido")
            {
                panelCamposNormales.Visibility = Visibility.Visible;
                panelCamposAdicionales.Visibility = Visibility.Visible;
            }
            else
            {
                panelCamposNormales.Visibility = Visibility.Visible;
                panelCamposAdicionales.Visibility = Visibility.Collapsed;
            }
        }


        private void MostrarSubSubOpciones()
        {
            var tipo = (comboTipo.SelectedItem as ComboBoxItem)?.Content.ToString();
            var subopcion = listaSubopciones.SelectedItem?.ToString();

            if (tipo != null && subopcion != null)
            {
                if (condicionesSubSub.ContainsKey(tipo) && condicionesSubSub[tipo].Contains(subopcion))
                {
                    listaSubSubopciones.ItemsSource = subSubOpciones;
                    listaSubSubopciones.Visibility = Visibility.Visible;
                }
                else
                {
                    listaSubSubopciones.ItemsSource = null;
                    listaSubSubopciones.Visibility = Visibility.Collapsed;
                }

                if (tipo != "A.I." && listaSubSubopciones.Visibility == Visibility.Visible)
                {
                    panelAdherido.Visibility = Visibility.Visible;
                }
            }
        }


        private void VerificarMostrarCampos()
        {
            string tipo = (comboTipo.SelectedItem as ComboBoxItem)?.Content?.ToString();
            string subopcion = listaSubopciones.SelectedItem?.ToString();

            if (tipo == "A.I.")
            {
                // Ya gestionado antes
                return;
            }

            if (subopcion == "No encontrado")
            {
                panelCamposNormales.Visibility = Visibility.Visible;
                panelCamposAdicionales.Visibility = Visibility.Collapsed;
                panelAdherido.Visibility = Visibility.Collapsed;
            }
            else if (condicionesSubSub.ContainsKey(tipo) && condicionesSubSub[tipo].Contains(subopcion))
            {
                panelAdherido.Visibility = Visibility.Visible;
                panelCamposNormales.Visibility = Visibility.Collapsed;
                panelCamposAdicionales.Visibility = Visibility.Collapsed;
            }
            else
            {
                OcultarCampos();
            }
        }


        private void OcultarCampos()
        {
            panelCamposNormales.Visibility = Visibility.Collapsed;
            panelCamposAdicionales.Visibility = Visibility.Collapsed;
        }

        private void GenerarReporte_Click(object sender, RoutedEventArgs e)
        {
            var tipo = (comboTipo.SelectedItem as ComboBoxItem)?.Content.ToString();
            var subopcion = listaSubopciones.SelectedItem?.ToString();

            if (tipo != null && subopcion != null)
            {
                MessageBox.Show($"Generando reporte:\nTipo: {tipo}\nSubopción: {subopcion}", "Reporte", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Por favor seleccione un tipo y una subopción.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
