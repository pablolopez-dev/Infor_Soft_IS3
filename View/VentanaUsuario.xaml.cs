using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Infor_Soft_WPF
{
    public partial class VentanaUsuario : Window
    {
        public VentanaUsuario()
        {
            InitializeComponent();
            CargarInformes();
        }

        private void CargarInformes()
        {
            dgInformes.ItemsSource = new List<Informe>
            {
                new Informe { Id = 1, Titulo = "Informe de Seguridad", Fecha = "2025-05-01" },
                new Informe { Id = 2, Titulo = "Revisión Legal", Fecha = "2025-05-12" },
                new Informe { Id = 3, Titulo = "Auditoría Interna", Fecha = "2025-06-02" }
            };
        }

        private void dgInformes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var informeSeleccionado = dgInformes.SelectedItem as Informe;
            if (informeSeleccionado != null)
            {
                lstAbogados.ItemsSource = ObtenerAbogadosPorInforme(informeSeleccionado.Id);
            }
        }

        private List<Abogado> ObtenerAbogadosPorInforme(int informeId)
        {
            // Simulación. En un sistema real, estos datos vendrían de una BD.
            return new List<Abogado>
            {
                new Abogado { Nombre = "Dr. Hugo Torres", Especialidad = "Derecho Penal" },
                new Abogado { Nombre = "Lic. María Gómez", Especialidad = "Derecho Administrativo" }
            };
        }

        private void VerInforme_Click(object sender, RoutedEventArgs e)
        {
            var informe = dgInformes.SelectedItem as Informe;
            if (informe != null)
                MessageBox.Show($"Mostrando detalles del informe: {informe.Titulo}");
        }

        private void Cerrar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

    public class Informe
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Fecha { get; set; }
    }

    public class Abogado
    {
        public string Nombre { get; set; }
        public string Especialidad { get; set; }
    }
}
