using Infor_Soft_WPF.Class.Entidades;
using Infor_Soft_WPF.Class.Repositorios;
using System.Collections.Generic;
using System.Windows;

namespace Infor_Soft_WPF.View
{
    public partial class SeleccionarAbogadoWindow : Window
    {
        public int IdAbogadoSeleccionado { get; private set; }
        public string TituloDocumento { get; private set; } 


        public Abogado AbogadoSeleccionado { get; private set; }

        public SeleccionarAbogadoWindow()
        {
            InitializeComponent();
            CargarAbogados();
        }

        private void CargarAbogados()
        {
            var repo = new AbogadoRepositorio();
            List<Abogado> abogados = repo.ObtenerAbogados();
            cmbAbogados.ItemsSource = abogados;
        }

        private void BtnNuevoAbogado_Click(object sender, RoutedEventArgs e)
        {
            var nuevoAbogadoWindow = new WindowNuevoAbogado();
            nuevoAbogadoWindow.Owner = this;

            if (nuevoAbogadoWindow.ShowDialog() == true)
            {
                CargarAbogados(); // Refrescar la lista
            }
        }

        private void BtnAceptar_Click(object sender, RoutedEventArgs e)
        {
            if (cmbAbogados.SelectedItem == null)
            {
                MessageBox.Show("Debe seleccionar un abogado.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtdocu.Text))
            {
                MessageBox.Show("Debe ingresar un título para el documento.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            IdAbogadoSeleccionado = (int)cmbAbogados.SelectedValue;
            TituloDocumento = txtdocu.Text.Trim();
            // ✅ Aquí estás asignando el objeto completo correctamente:
            AbogadoSeleccionado = (Abogado)cmbAbogados.SelectedItem;
            this.DialogResult = true;
            this.Close();
        }

    }
}
