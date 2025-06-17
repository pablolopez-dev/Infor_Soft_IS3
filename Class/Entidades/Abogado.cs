namespace Infor_Soft_WPF.Class.Entidades
{
    public class Abogado
    {
        public int Id { get; set; }  // id_abogado
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public string DocumentoTitulo { get; set; } // ← nuevo


        public string NombreCompleto => $"{Nombre} {Apellido}";
    }
}
