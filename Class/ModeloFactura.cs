using System.Collections.Generic;
using System;

public class ModeloFactura
{
    public string Cliente { get; set; }
    public string Documento { get; set; }
    public string NumeroExpediente { get; set; }
    public string AnioExpediente { get; set; }
    public List<ModeloOficio> Oficios { get; set; }
    public string TotalFactura { get; set; }
    public DateTime FechaLiquidacion { get; set; }
    public string NumeroLiquidacion { get; set; }
}

public class ModeloOficio
{
    public string Descripcion { get; set; }
    public string Destino { get; set; }
    public string DistanciaKm { get; set; }
    public string Monto { get; set; }
}
