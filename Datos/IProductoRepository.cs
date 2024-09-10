using NegocioApp.Dominio;

namespace NegocioApp.Data
{
    public interface IProductoRepository
    {
        List<Producto> GetAll();
        Producto GetById(int id);
        bool Save(Producto oProducto);
        bool Delete(int id);
    }
}
