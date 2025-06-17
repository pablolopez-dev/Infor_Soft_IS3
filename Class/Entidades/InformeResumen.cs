using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infor_Soft_WPF.Class.Entidades
{
    public class InformeResumen
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Usuario { get; set; }
        public DateTime FechaCreacion { get; set; }
        public TimeSpan HoraCreacion { get; set; }
    }

}
