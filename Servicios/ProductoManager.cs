using NegocioApp.Data;
using NegocioApp.Dominio;

namespace NegocioApp.Services
{
    public class ProductoManager
    {
        private IProductoRepository _repositorio;

        public ProductoManager()
        {
            _repositorio = new ProductoRepositoryAdo();
        }

        public List<Producto> GetProducts()
        {
            return _repositorio.GetAll();
        }

        public Producto GetProductByCodigo(int cod)
        {
            return _repositorio.GetById(cod);
        }

        public bool SaveProduct(Producto oProducto)
        {
            return _repositorio.Save(oProducto);
        }
        public bool DeleteProduct(int cod)
        {
            return _repositorio.Delete(cod);
        }
    }
}
