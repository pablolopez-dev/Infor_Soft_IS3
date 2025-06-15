using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Infor_Soft_WPF
{
    public static class OficioComi_SinTraslado_ADHPORTON
    {
        public static string GenerarInforme(string dia, string mes, string anio, string hora, string minuto, string nombreCompleto, string direccion)
        {
            return $"EN CIUDAD DEL ESTE, ALTO PARANÁ, REPÚBLICA DEL PARAGUAY, a los {dia} días del mes de {mes} del año {anio}, " +
                   $"siendo las {hora} horas con {minuto} minutos, me constituí nuevamente en el domicilio del/la señor/a " +
                   $"{nombreCompleto}, SITO, {direccion} - a fin de notificar el oficio comisivo que antecede. Una vez en dicho lugar encontré el portón cerrado, " +
                   $"nadie me atendió y al no ser atendida por ninguna persona, procedo a dejar la cedula de notificación, con sus respectivas copias de traslado, " +
                   $"adherido por el portón del acceso principal al domicilio. Adjunto tomas fotográficas del acto realizado. En esta circunstancia di por terminado el acto. Es mi informe. Conste.";
        }
    }

}