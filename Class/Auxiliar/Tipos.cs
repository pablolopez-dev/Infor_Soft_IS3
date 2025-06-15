using System;

public class OpcionReporte
{
    public string Tipo { get; }
    public string Subopcion { get; }
    public string SubSubopcion { get; }

    public OpcionReporte(string tipo, string subopcion = null, string subSubopcion = null)
    {
        Tipo = tipo ?? "";
        Subopcion = subopcion ?? "";
        SubSubopcion = subSubopcion ?? "";
    }

    public override bool Equals(object obj)
    {
        return obj is OpcionReporte other &&
               string.Equals(Tipo, other.Tipo, StringComparison.OrdinalIgnoreCase) &&
               string.Equals(Subopcion, other.Subopcion, StringComparison.OrdinalIgnoreCase) &&
               string.Equals(SubSubopcion, other.SubSubopcion, StringComparison.OrdinalIgnoreCase);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(
            Tipo?.ToLowerInvariant() ?? "",
            Subopcion?.ToLowerInvariant() ?? "",
            SubSubopcion?.ToLowerInvariant() ?? ""
        );
    }
}
