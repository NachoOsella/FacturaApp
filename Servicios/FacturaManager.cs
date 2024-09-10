using NegocioApp.Data;
using NegocioApp.Dominio;


namespace NegocioApp.Services
{
    public class FacturaManager
    {
        private IFacturaRepository _repository;

        public FacturaManager()
        {
            _repository = new FacturaRepository();
        }

        public List<Factura> GetBudgets()
        {
            return _repository.GetAll();
        }

        public Factura? GetBudgetsById(int id)
        {
            return _repository.GetById(id);
        }


    }
}