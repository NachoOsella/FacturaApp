namespace NegocioApp.Dominio
{
    public class DetalleFactura
    {
        public Producto Producto { get; set; }
        public int Count { get; set; }
        public float Price { get; set; }

        public float SubTotal()
        {
            return Price * Count;
        }

    }
}
