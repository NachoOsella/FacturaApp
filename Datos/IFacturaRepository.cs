using NegocioApp.Dominio;

namespace NegocioApp.Data
{
    public interface IFacturaRepository
    {
        bool Save(Factura oFactura);
        List<Factura> GetAll();
        Factura? GetById(int id);

    }
}
