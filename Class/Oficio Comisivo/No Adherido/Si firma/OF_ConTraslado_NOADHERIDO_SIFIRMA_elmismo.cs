using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Infor_Soft_WPF
{
    public static class OF_ConTraslado_NOADHERIDO_SI_FIRMA_ELMISMO
    {
        public static string GenerarInforme(string dia, string mes, string anio, string hora, string minuto, string nombreCompleto, string direccion, string recibio, string parentesco)
        {
            return $"EN CIUDAD DEL ESTE, ALTO PARANÁ, REPÚBLICA DEL PARAGUAY, a los {dia} días del mes de {mes} del año {anio}, " +
                   $"siendo las {hora} horas con {minuto} minutos, para dar cumplimiento a la cedula de aviso, me constituí nuevamente en el domicilio del/la señor/a " +
                   $"{nombreCompleto}, SITO, {direccion} - a fin de notificar el oficio comisivo que antecede. Una vez en dicho lugar fui recibida por una persona quien dijo ser {recibio} ({parentesco}), a quien instruí mi cometido, dándole integra lectura del contenido de la presente cedula de notificación con sus respectivas copias de traslado, enterada le hice entrega de su duplicado e invitado a firmar conmigo, ASI LO HIZO. Con lo que di por terminado el acto. Es mi informe. Conste. -------------------------------";
        }
    }

}