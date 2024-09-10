namespace NegocioApp.Dominio
{
    public class Factura
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string Client { get; set; }

        public int Expiration { get; set; }

        private List<DetalleFactura> details;

        public List<DetalleFactura> GetDetails()
        {
            return details;
        }

        public Factura()
        {
            details = new List<DetalleFactura>();
        }

        public void AddDetail(DetalleFactura detail)
        {
            if (detail != null)
                details.Add(detail);
        }

        public void Remove(int index)
        {
            details.RemoveAt(index);
        }

        public float Total()
        {
            float total = 0;
            foreach (var detail in details)
                total += detail.SubTotal();

            return total;
        }

    }
}