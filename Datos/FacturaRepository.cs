using FacturasApp.Datos.Utils;
using NegocioApp.Dominio;
using System.Data;
using System.Data.SqlClient;

namespace NegocioApp.Data
{
    public class FacturaRepository : IFacturaRepository
    {
        public List<Factura> GetAll()
        {
            List<Factura> lst = new List<Factura>();
            Factura? oBudget = null;
            var helper = DataHelper.GetInstance();
            var t = helper.ExecuteSPQuery("SP_RECUPERAR_PRESUPUESTOS", null);
            foreach (DataRow row in t.Rows)
            {
                if (oBudget == null || oBudget.Id != Convert.ToUInt32(row["id"].ToString()))
                {

                    oBudget = new Factura();
                    oBudget.Client = row["cliente"].ToString();
                    oBudget.Date = Convert.ToDateTime(row["fecha"].ToString());
                    oBudget.Expiration = Convert.ToInt32(row["vigencia"].ToString());
                    oBudget.Id = Convert.ToInt32(row["id"].ToString());
                    oBudget.AddDetail(ReadDetail(row));
                    lst.Add(oBudget);
                }
                else
                {
                    oBudget.AddDetail(ReadDetail(row));
                }
            }
            return lst;
        }

        private DetalleFactura ReadDetail(DataRow row)
        {
            DetalleFactura detail = new DetalleFactura();
            detail.Producto = new Producto
            {
                Codigo = Convert.ToInt32(row["codigo"].ToString()),
                Nombre = row["n_producto"].ToString(),
                Stock = Convert.ToInt32(row["stock"].ToString()),
                Activo = Convert.ToBoolean(row["esta_activo"].ToString())
            };
            detail.Price = Convert.ToSingle(row["precio"].ToString());
            detail.Count = Convert.ToInt32(row["cantidad"].ToString());
            return detail;
        }

        public Factura? GetById(int id)
        {
            Factura? oBudget = null;
            var helper = DataHelper.GetInstance();
            var parameter = new ParameterSQL("@id", id);
            var parameters = new List<ParameterSQL>();
            parameters.Add(parameter);

            var t = helper.ExecuteSPQuery("SP_RECUPERAR_PRESUPUESTO_POR_ID", parameters);
            foreach (DataRow row in t.Rows)
            {
                if (oBudget == null)
                {
                    oBudget = new Factura();
                    oBudget.Client = row["cliente"].ToString();
                    oBudget.Date = Convert.ToDateTime(row["fecha"].ToString());
                    oBudget.Expiration = Convert.ToInt32(row["vigencia"].ToString());
                    oBudget.Id = Convert.ToInt32(row["id"].ToString());
                    oBudget.AddDetail(ReadDetail(row));
                }
                else
                {
                    oBudget.AddDetail(ReadDetail(row));
                }
            }
            return oBudget;

        }

        public bool Save(Factura oFactura)
        {
            bool result = true;
            SqlTransaction? t = null;
            SqlConnection? cnn = null;

            try
            {
                cnn = DataHelper.GetInstance().GetConnection();
                cnn.Open();
                t = cnn.BeginTransaction();

                var cmd = new SqlCommand("SP_INSERTAR_MAESTRO", cnn, t);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@cliente", oFactura.Client);
                cmd.Parameters.AddWithValue("@vigencia", oFactura.Expiration);

                SqlParameter param = new SqlParameter("@id", SqlDbType.Int);
                param.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(param);
                cmd.ExecuteNonQuery();

                int budgetId = (int)param.Value;

                int nroDetail = 1;
                foreach (var detail in oFactura.GetDetails())
                {
                    var cmdDetail = new SqlCommand("SP_INSERTAR_DETALLE", cnn, t);
                    cmdDetail.CommandType = CommandType.StoredProcedure;
                    cmdDetail.Parameters.AddWithValue("@presupuesto", budgetId);
                    cmdDetail.Parameters.AddWithValue("@id_detalle", nroDetail);
                    cmdDetail.Parameters.AddWithValue("@producto", detail.Producto.Codigo);
                    cmdDetail.Parameters.AddWithValue("@cantidad", detail.Count);
                    cmdDetail.Parameters.AddWithValue("@precio", detail.Price);
                    cmdDetail.ExecuteNonQuery();
                    nroDetail++;
                }

                t.Commit();
            }
            catch (SqlException)
            {
                if (t != null)
                {
                    t.Rollback();
                }
                result = false;
            }
            finally
            {
                if (cnn != null && cnn.State == ConnectionState.Open)
                {
                    cnn.Close();
                }
            }
            return result;
        }
    }
}
