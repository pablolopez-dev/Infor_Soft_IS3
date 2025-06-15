using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Infor_Soft_WPF
{
    public static class Sin_Especificar_Hombre_tras
    {
        public static string GenerarInforme(string dia, string mes, string anio, string hora, string minuto, string nombreCompleto, string direccion)
        {
            return $"EN CIUDAD DEL ESTE, ALTO PARANÁ, REPÚBLICA DEL PARAGUAY, a los {dia} días del mes de {mes} del año {anio}, " +
                   $"siendo las {hora} horas con {minuto} minutos, para dar cumplimiento a la cedula de aviso, me constituí nuevamente en el domicilio del/la señor/a " +
                   $"{nombreCompleto}, SITO, {direccion} - a fin de notificar la providencia que antecede. Una vez en dicho fui recibido por una persona de sexo masculino, " +
                   $"quien se niega a identificarse, a quien instruí mi cometido dándole integra lectura del contenido de la presente cédula de notificación con sus respectivas copias de traslado, " +
                   $"invitando a firmar conmigo, No lo hizo. Pero comprometiendose en hacer entrega de la misma al destinatario en la brevedad posible. En esta circunstancia di por terminado. " +
                   $"Es mi informe. Conste. -----------------";
        }
    }

}