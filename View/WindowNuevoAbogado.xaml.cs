using Infor_Soft_WPF.Class.Repositorios;
using System.Windows;

namespace Infor_Soft_WPF.View
{
    public partial class WindowNuevoAbogado : Window
    {
        public WindowNuevoAbogado()
        {
            InitializeComponent();
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            string nombre = txtNombre.Text.Trim();
            string apellido = txtApellido.Text.Trim();
            string telefono = txtTelefono.Text.Trim();

            if (string.IsNullOrWhiteSpace(nombre))
            {
                MessageBox.Show("El nombre es obligatorio.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var repo = new AbogadoRepositorio();
            repo.AgregarAbogado(nombre, apellido, telefono);

            MessageBox.Show("Abogado guardado exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            this.DialogResult = true;
            this.Close();
        }
    }
}
