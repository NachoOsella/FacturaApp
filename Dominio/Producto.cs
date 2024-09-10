namespace NegocioApp.Dominio
{
    public class Producto
    {
        public int Codigo { get; set; }
        public string Nombre { get; set; }
        public int Stock { get; set; }
        public bool Activo { get; set; }

        public override string? ToString()
        {
            return "[" + Codigo + "] " + Nombre;
        }
    }
}
