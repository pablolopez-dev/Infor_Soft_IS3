using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infor_Soft_WPF.Class
{
    public class OficioComisivo
    {
        public string Descripcion { get; set; }
        public string Destino { get; set; }
        public string DistanciaKm { get; set; }
        public string Monto { get; set; }
    }

    public class FacturaPreviewModel
    {
        public string Cliente { get; set; }
        public string Documento { get; set; }
        public string NumeroExpediente { get; set; }
        public string AnioExpediente { get; set; }
        public List<OficioComisivo> Oficios { get; set; } = new List<OficioComisivo>();
        public string TotalFactura { get; set; }
        public DateTime FechaLiquidacion { get; set; }
        public string NumeroLiquidacion { get; set; }
    }

}
