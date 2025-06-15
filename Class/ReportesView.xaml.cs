using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics.X86;
using System.Windows;
using System.Windows.Controls;
using Word = Microsoft.Office.Interop.Word;


namespace Infor_Soft_WPF.View
{
    public partial class Window1 : Window
    {
        private Dictionary<OpcionReporte, Func<string>> plantillaMap;

        private readonly Dictionary<string, List<string>> subopcionesDict = new()
        {
            { "Providencia", new() { "Con traslado", "Sin traslado", "No encontrado"} },
            { "A.I.", new() { "Adherido Porton", "Adherido Puerta", "No Adherido", "No encontrado", "Recibido sin especificar Mujer", "Recibido sin especificar Hombre" } },
            { "S.D.", new() { "Con Aviso", "Sin Aviso", "No encontrado"} },
            { "Oficio Comisivo", new() { "Con traslado", "Sin traslado", "No encontrado"} }
        };

        private readonly List<string> subSubOpciones = new()
        {
            "Adherido Porton", "Adherido Puerta", "No Adherido",
            "Recibido sin especificar Mujer", "Recibido sin especificar Hombre"
        };

        private readonly Dictionary<string, List<string>> condicionesSubSub = new()
        {
            { "Providencia", new() { "Con traslado", "Sin traslado", "No encontrado" } },
            { "A.I.", new() }, // sin subopciones
            { "S.D.", new() { "Con Aviso", "Sin Aviso", "No encontrado" } },
            { "Oficio Comisivo", new() { "Con traslado", "Sin traslado", "No encontrado" } }
        };

        public Window1()
        {
            InitializeComponent();
            InicializarPlantillas();
            listaSubSubopciones.SelectionChanged += listaSubSubopciones_SelectionChanged;
        }


        private void InicializarPlantillas()
        {
            plantillaMap = new Dictionary<OpcionReporte, Func<string>>
            {

          //-------------------------- //---------------------------------------------//-----------------------------------//----------------------------------------

                //PARA LA RESOLUCION PROVIDENCIAS -- PORTON

                //Adherido Porton
                [new OpcionReporte("Providencia", "Con traslado", "Adherido Porton")] = () =>
                    Infor_Soft_WPF.Provi_ConTraslado_ADHPORTON.GenerarInforme(
                        fechaTextBox.Text, mesTextBox.Text, anoTextBox.Text,
                        horasTextBox.Text, minutosTextBox.Text,
                        demandadoTextBox.Text, domicilioTextBox.Text
                    ),
              
                //Adherido Porton
                [new OpcionReporte("Providencia", "Sin traslado", "Adherido Porton")] = () =>
                    Infor_Soft_WPF.Provi_SinTraslado_ADHPORTON.GenerarInforme(
                        fechaTextBox.Text, mesTextBox.Text, anoTextBox.Text,
                        horasTextBox.Text, minutosTextBox.Text,
                        demandadoTextBox.Text, domicilioTextBox.Text
                    ),

               
                //No encontrado
                [new OpcionReporte("Providencia", "No encontrado")] = () =>
                    Infor_Soft_WPF.Provi_NoEncontrado_ADHPORTON.GenerarInforme(
                        fechaTextBox.Text, mesTextBox.Text, anoTextBox.Text,
                        horasTextBox.Text, minutosTextBox.Text,
                        demandadoTextBox.Text, domicilioTextBox.Text
                    ),






                //PARA LA RESOLUCION PROVIDENCIAS -- PUERTA

                //Adherido PUERTA
                [new OpcionReporte("Providencia", "Con traslado", "Adherido Puerta")] = () =>
                    Infor_Soft_WPF.Provi_ConTraslado_ADHPUERTA.GenerarInforme(
                        fechaTextBox.Text, mesTextBox.Text, anoTextBox.Text,
                        horasTextBox.Text, minutosTextBox.Text,
                        demandadoTextBox.Text, domicilioTextBox.Text
                    ),

                //Adherido PUERTA
                [new OpcionReporte("Providencia", "Sin traslado", "Adherido Puerta")] = () =>
                    Infor_Soft_WPF.Provi_SinTraslado_ADHPUERTA.GenerarInforme(
                        fechaTextBox.Text, mesTextBox.Text, anoTextBox.Text,
                        horasTextBox.Text, minutosTextBox.Text,
                        demandadoTextBox.Text, domicilioTextBox.Text
                    ),


                //No encontrado
                [new OpcionReporte("Providencia", "No encontrado")] = () =>
                    Infor_Soft_WPF.Provi_NoEncontrado_ADHPUERTA.GenerarInforme(
                        fechaTextBox.Text, mesTextBox.Text, anoTextBox.Text,
                        horasTextBox.Text, minutosTextBox.Text,
                        demandadoTextBox.Text, domicilioTextBox.Text
                    ),





                //PARA LA RESOLUCION PROVIDENCIAS -- Sin Especificar Hombre

                
                [new OpcionReporte("Providencia", "Con traslado", "Recibido sin especificar Hombre")] = () =>
                    Infor_Soft_WPF.Sin_Especificar_Hombre_tras.GenerarInforme(
                        fechaTextBox.Text, mesTextBox.Text, anoTextBox.Text,
                        horasTextBox.Text, minutosTextBox.Text,
                        demandadoTextBox.Text, domicilioTextBox.Text
                    ),

                [new OpcionReporte("Providencia", "Sin traslado", "Recibido sin especificar Hombre")] = () =>
                    Infor_Soft_WPF.Sin_Especificar_Hombre_Sin_tras.GenerarInforme(
                        fechaTextBox.Text, mesTextBox.Text, anoTextBox.Text,
                        horasTextBox.Text, minutosTextBox.Text,
                        demandadoTextBox.Text, domicilioTextBox.Text
                    ),


                //No encontrado
                [new OpcionReporte("Providencia", "No encontrado")] = () =>
                    Infor_Soft_WPF.Provi_NoEncontrado_ADHPUERTA.GenerarInforme(
                        fechaTextBox.Text, mesTextBox.Text, anoTextBox.Text,
                        horasTextBox.Text, minutosTextBox.Text,
                        demandadoTextBox.Text, domicilioTextBox.Text
                    ),




                //PARA LA RESOLUCION PROVIDENCIAS -- Sin Especificar Mujer


                [new OpcionReporte("Providencia", "Con traslado", "Recibido sin especificar Mujer")] = () =>
                    Infor_Soft_WPF.Sin_Especificar_Mujer_tras.GenerarInforme(
                        fechaTextBox.Text, mesTextBox.Text, anoTextBox.Text,
                        horasTextBox.Text, minutosTextBox.Text,
                        demandadoTextBox.Text, domicilioTextBox.Text
                    ),

                [new OpcionReporte("Providencia", "Sin traslado", "Recibido sin especificar Mujer")] = () =>
                    Infor_Soft_WPF.Sin_Especificar_Mujer_Sin_tras.GenerarInforme(
                        fechaTextBox.Text, mesTextBox.Text, anoTextBox.Text,
                        horasTextBox.Text, minutosTextBox.Text,
                        demandadoTextBox.Text, domicilioTextBox.Text
                    ),


                //No encontrado
                [new OpcionReporte("Providencia", "No encontrado")] = () =>
                    Infor_Soft_WPF.Provi_NoEncontrado_ADHPUERTA.GenerarInforme(
                        fechaTextBox.Text, mesTextBox.Text, anoTextBox.Text,
                        horasTextBox.Text, minutosTextBox.Text,
                        demandadoTextBox.Text, domicilioTextBox.Text
                    ),




                // No Adherido - Sí firmó (NO él mismo)
                [new OpcionReporte("Providencia", "Con traslado", "No Adherido - Sí firmó")] = () =>
                    Infor_Soft_WPF.Provi_ConTraslado_NOADHERIDO_SI_FIRMA.GenerarInforme(
                        fechaTextBox.Text, mesTextBox.Text, anoTextBox.Text,
                        horasTextBox.Text, minutosTextBox.Text,
                        demandadoTextBox.Text, domicilioTextBox.Text,
                        recibidoTextBox.Text, parentescoTextBox.Text
                    ),

                // No Adherido - Sí firmó (ÉL MISMO)
                [new OpcionReporte("Providencia", "Con traslado", "No Adherido - Sí firmó (él mismo)")] = () =>
                    Infor_Soft_WPF.Provi_ConTraslado_NOADHERIDO_SI_FIRMA_ELMISMO.GenerarInforme(
                        fechaTextBox.Text, mesTextBox.Text, anoTextBox.Text,
                        horasTextBox.Text, minutosTextBox.Text,
                        demandadoTextBox.Text, domicilioTextBox.Text,
                        recibidoTextBox.Text, parentescoTextBox.Text
                    ),

                // No Adherido - No firmó (NO él mismo)
                [new OpcionReporte("Providencia", "Con traslado", "No Adherido - No firmó")] = () =>
                    Infor_Soft_WPF.Provi_ConTraslado_NOADHERIDO_NOFIRMA.GenerarInforme(
                        fechaTextBox.Text, mesTextBox.Text, anoTextBox.Text,
                        horasTextBox.Text, minutosTextBox.Text,
                        demandadoTextBox.Text, domicilioTextBox.Text,
                        recibidoTextBox.Text, parentescoTextBox.Text
                    ),

                // No Adherido - No firmó (ÉL MISMO)
                [new OpcionReporte("Providencia", "Con traslado", "No Adherido - No firmó (él mismo)")] = () =>
                    Infor_Soft_WPF.Provi_ConTraslado_NOADHERIDO_NOFIRMA_elmismo.GenerarInforme(
                        fechaTextBox.Text, mesTextBox.Text, anoTextBox.Text,
                        horasTextBox.Text, minutosTextBox.Text,
                        demandadoTextBox.Text, domicilioTextBox.Text,
                        recibidoTextBox.Text, parentescoTextBox.Text
                    ),







                            //-------------------------- //---------------------------------------------//-----------------------------------//----------------------------------------

                            //PARA LA RESOLUCION Oficio Comisivo -- PORTON

                            //Adherido Porton
                            [new OpcionReporte("Oficio Comisivo", "Con traslado", "Adherido Porton")] = () =>
                    Infor_Soft_WPF.OficioComi_ConTraslado_ADHPORTON.GenerarInforme(
                        fechaTextBox.Text, mesTextBox.Text, anoTextBox.Text,
                        horasTextBox.Text, minutosTextBox.Text,
                        demandadoTextBox.Text, domicilioTextBox.Text
                    ),

                //Adherido Porton
                [new OpcionReporte("Oficio Comisivo", "Sin traslado", "Adherido Porton")] = () =>
                    Infor_Soft_WPF.OficioComi_SinTraslado_ADHPORTON.GenerarInforme(
                        fechaTextBox.Text, mesTextBox.Text, anoTextBox.Text,
                        horasTextBox.Text, minutosTextBox.Text,
                        demandadoTextBox.Text, domicilioTextBox.Text
                    ),


                //No encontrado
                [new OpcionReporte("Oficio Comisivo", "No encontrado")] = () =>
                    Infor_Soft_WPF.OficioComi_NoEncontrado_ADHPORTON.GenerarInforme(
                        fechaTextBox.Text, mesTextBox.Text, anoTextBox.Text,
                        horasTextBox.Text, minutosTextBox.Text,
                        demandadoTextBox.Text, domicilioTextBox.Text
                    ),

                //PARA LA RESOLUCION Oficio Comisivo -- PUERTA

                //Adherido PUERTA
                [new OpcionReporte("Oficio Comisivo", "Con traslado", "Adherido Puerta")] = () =>
                    Infor_Soft_WPF.OficioComi_ConTraslado_ADHPUERTA.GenerarInforme(
                        fechaTextBox.Text, mesTextBox.Text, anoTextBox.Text,
                        horasTextBox.Text, minutosTextBox.Text,
                        demandadoTextBox.Text, domicilioTextBox.Text
                    ),

                //Adherido PUERTA
                [new OpcionReporte("Oficio Comisivo", "Sin traslado", "Adherido puerta")] = () =>
                    Infor_Soft_WPF.OficioComi_SinTraslado_ADHPUERTA.GenerarInforme(
                        fechaTextBox.Text, mesTextBox.Text, anoTextBox.Text,
                        horasTextBox.Text, minutosTextBox.Text,
                        demandadoTextBox.Text, domicilioTextBox.Text
                    ),


                //No encontrado
                [new OpcionReporte("Oficio Comisivo", "No encontrado")] = () =>
                    Infor_Soft_WPF.OficioComi_NoEncontrado_ADHPUERTA.GenerarInforme(
                        fechaTextBox.Text, mesTextBox.Text, anoTextBox.Text,
                        horasTextBox.Text, minutosTextBox.Text,
                        demandadoTextBox.Text, domicilioTextBox.Text
                    ),


                //PARA LA RESOLUCION OFICIO COMISIVO -- Sin Especificar HOMBRE


                [new OpcionReporte("Oficio Comisivo", "Con traslado", "Recibido sin especificar Hombre")] = () =>
                    Infor_Soft_WPF.Sin_Especificar_Hombre_tras_OF.GenerarInforme(
                        fechaTextBox.Text, mesTextBox.Text, anoTextBox.Text,
                        horasTextBox.Text, minutosTextBox.Text,
                        demandadoTextBox.Text, domicilioTextBox.Text
                    ),

                [new OpcionReporte("Oficio Comisivo", "Sin traslado", "Recibido sin especificar Hombre")] = () =>
                    Infor_Soft_WPF.Sin_Especificar_Hombre_Sin_tras_OF.GenerarInforme(
                        fechaTextBox.Text, mesTextBox.Text, anoTextBox.Text,
                        horasTextBox.Text, minutosTextBox.Text,
                        demandadoTextBox.Text, domicilioTextBox.Text
                    ),


                //No encontrado
                [new OpcionReporte("Oficio Comisivo", "No encontrado")] = () =>
                    Infor_Soft_WPF.Provi_NoEncontrado_ADHPUERTA.GenerarInforme(
                        fechaTextBox.Text, mesTextBox.Text, anoTextBox.Text,
                        horasTextBox.Text, minutosTextBox.Text,
                        demandadoTextBox.Text, domicilioTextBox.Text
                    ),













                //PARA LA RESOLUCION OFICIO COMISIVO -- Sin Especificar Mujer


                [new OpcionReporte("Oficio Comisivo", "Con traslado", "Recibido sin especificar Mujer")] = () =>
                    Infor_Soft_WPF.Sin_Especificar_Mujer_tras_OF.GenerarInforme(
                        fechaTextBox.Text, mesTextBox.Text, anoTextBox.Text,
                        horasTextBox.Text, minutosTextBox.Text,
                        demandadoTextBox.Text, domicilioTextBox.Text
                    ),

                [new OpcionReporte("Oficio Comisivo", "Sin traslado", "Recibido sin especificar Mujer")] = () =>
                    Infor_Soft_WPF.Sin_Especificar_Mujer_Sin_tras_OF.GenerarInforme(
                        fechaTextBox.Text, mesTextBox.Text, anoTextBox.Text,
                        horasTextBox.Text, minutosTextBox.Text,
                        demandadoTextBox.Text, domicilioTextBox.Text
                    ),


                //No encontrado
                [new OpcionReporte("Oficio Comisivo", "No encontrado")] = () =>
                    Infor_Soft_WPF.Provi_NoEncontrado_ADHPUERTA.GenerarInforme(
                        fechaTextBox.Text, mesTextBox.Text, anoTextBox.Text,
                        horasTextBox.Text, minutosTextBox.Text,
                        demandadoTextBox.Text, domicilioTextBox.Text
                    ),




            };
        }

        private void comboTipo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedTipo = (comboTipo.SelectedItem as ComboBoxItem)?.Content as string;

            listaSubopciones.Items.Clear();
            listaSubopciones.Visibility = Visibility.Collapsed;

            listaSubSubopciones.ItemsSource = null;
            listaSubSubopciones.Visibility = Visibility.Collapsed;

            radioPanelAdherido.Visibility = Visibility.Collapsed;
            panelCamposNormales.Visibility = Visibility.Collapsed;
            panelCamposAdicionales.Visibility = Visibility.Collapsed;

            if (!string.IsNullOrEmpty(selectedTipo))
            {
                if (selectedTipo == "A.I.")
                {
                    radioPanelAdherido.Visibility = Visibility.Visible;
                }
                else if (subopcionesDict.ContainsKey(selectedTipo))
                {
                    foreach (var sub in subopcionesDict[selectedTipo])
                    {
                        listaSubopciones.Items.Add(sub);
                    }

                    listaSubopciones.Visibility = Visibility.Visible;
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
            string tipo = (comboTipo.SelectedItem as ComboBoxItem)?.Content?.ToString();
            string subopcion = listaSubopciones.SelectedItem?.ToString();

            if (!string.IsNullOrEmpty(tipo) && !string.IsNullOrEmpty(subopcion))
            {
                // Evitar mostrar sub-subopciones si se selecciona "No encontrado"
                if (subopcion == "No encontrado")
                {
                    listaSubSubopciones.ItemsSource = null;
                    listaSubSubopciones.Visibility = Visibility.Collapsed;
                    return;
                }

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
            }
        }


        private void listaSubSubopciones_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string seleccion = listaSubSubopciones.SelectedItem?.ToString();

            if (!string.IsNullOrEmpty(seleccion))
            {
                if (seleccion == "No Adherido")
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
            else
            {
                panelCamposNormales.Visibility = Visibility.Collapsed;
                panelCamposAdicionales.Visibility = Visibility.Collapsed;
            }
        }

        private void VerificarMostrarCampos()
        {
            string tipo = (comboTipo.SelectedItem as ComboBoxItem)?.Content?.ToString();
            string subopcion = listaSubopciones.SelectedItem?.ToString();

            if (tipo == "A.I.")
                return;

            if (subopcion == "No encontrado")
            {
                panelCamposNormales.Visibility = Visibility.Visible;
                panelCamposAdicionales.Visibility = Visibility.Collapsed;
            }
            else if (condicionesSubSub.ContainsKey(tipo) && condicionesSubSub[tipo].Contains(subopcion))
            {
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



        private void btnGenerarReporte_Click(object sender, RoutedEventArgs e)
        {
            string tipo = (comboTipo.SelectedItem as ComboBoxItem)?.Content?.ToString();
            string subopcion = listaSubopciones.SelectedItem?.ToString();
            string subSubopcion = listaSubSubopciones.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(tipo))
            {
                MessageBox.Show("Por favor seleccione un tipo.",
                                "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

           


            OpcionReporte clave;
            if (tipo == "Providencia" && subopcion == "No encontrado")
            {
                clave = new OpcionReporte(tipo, subopcion); // solo tipo y subopción
            }
            else if (tipo == "Resolución A.I")
            {
                if (string.IsNullOrEmpty(subSubopcion))
                {
                    MessageBox.Show("Por favor seleccione una sub-subopción.",
                                    "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                clave = new OpcionReporte(tipo, null, subSubopcion);
            }
            else
            {
                if (string.IsNullOrEmpty(subopcion) || string.IsNullOrEmpty(subSubopcion))
                {
                    MessageBox.Show("Por favor seleccione una subopción y sub-subopción.",
                                    "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                clave = new OpcionReporte(tipo, subopcion, subSubopcion);
            }


            if (tipo == "Providencia" && subopcion == "Con traslado" && subSubopcion == "No Adherido")
            {
                if (string.IsNullOrWhiteSpace(recibidoTextBox.Text) || string.IsNullOrWhiteSpace(parentescoTextBox.Text))
                {
                    MessageBox.Show("Por favor complete los campos de 'Recibido por' y 'Parentesco'.",
                                    "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                bool esElMismo = demandadoTextBox.Text.Trim().Equals(recibidoTextBox.Text.Trim(), StringComparison.OrdinalIgnoreCase);

                if (firmaComboBox.Text == "Sí firmó")
                {
                    clave = esElMismo
                        ? new OpcionReporte(tipo, subopcion, "No Adherido - Sí firmó (él mismo)")
                        : new OpcionReporte(tipo, subopcion, "No Adherido - Sí firmó");
                }
                else if (firmaComboBox.Text == "No firmó")
                {
                    clave = esElMismo
                        ? new OpcionReporte(tipo, subopcion, "No Adherido - No firmó (él mismo)")
                        : new OpcionReporte(tipo, subopcion, "No Adherido - No firmó");
                }
                else
                {
                    MessageBox.Show("Por favor seleccione una opción de firma válida.",
                                    "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }











            if (plantillaMap.TryGetValue(clave, out var generador))
            {
                string reporte = generador();
                Infor_Soft_WPF.Helpers.WordDocumentHelper.CrearDocumento(reporte);
            }
            else
            {
                MessageBox.Show("La combinación seleccionada no tiene una plantilla definida.",
                                "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
