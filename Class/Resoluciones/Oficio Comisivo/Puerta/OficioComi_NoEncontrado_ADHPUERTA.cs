using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Infor_Soft_WPF
{
    public static class OficioComi_NoEncontrado_ADHPUERTA
    {
        public static string GenerarInforme(string dia, string mes, string anio, string hora, string minuto, string nombreCompleto, string direccion)
        {
            return $"EN CIUDAD DEL ESTE, ALTO PARANÁ, REPÚBLICA DEL PARAGUAY, a los {dia} días del mes de {mes} del año {anio}, siendo las {hora} horas y {minuto} minutos, me constituí en el domicilio del/la señor/a {nombreCompleto}, SITO, {direccion}. - a fin de notificar la providencia que antecede. Una vez en dicho lugar, empiezo a buscar arduamente preguntando a los vecinos del lugar, y nadie tiene conocimiento del demandado/a. No se pudo dar cumplimiento a mi cometido por la dirección imprecisa. En esta circunstancia di por terminado. Es mi informe. Conste.";

        }
    }

}