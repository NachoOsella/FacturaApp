using FacturasApp.Datos.Utils;
using NegocioApp.Dominio;
using System.Data;
using System.Data.SqlClient;

namespace NegocioApp.Data
{
    public class ProductoRepositoryAdo : IProductoRepository
    {
        private SqlConnection _connection;

        public ProductoRepositoryAdo()
        {
            _connection = new SqlConnection(Properties.Resources.cnnString);
        }


        public bool Delete(int id)
        {
            var parameters = new List<ParameterSQL>();
            parameters.Add(new ParameterSQL("@codigo", id));
            int rows = DataHelper.GetInstance().ExecuteSPDML("SP_REGISTRAR_BAJA_PRODUCTO", parameters);
            return rows == 1;
        }

        public List<Producto> GetAll()
        {
            List<Producto> lst = new List<Producto>();
            var helper = DataHelper.GetInstance();
            var t = helper.ExecuteSPQuery("SP_RECUPERAR_PRODUCTOS", null);
            foreach (DataRow row in t.Rows)
            {
                int id = Convert.ToInt32(row["codigo"]);
                string nombre = row["n_producto"].ToString();
                int stock = Convert.ToInt32(row["stock"]);
                bool activo = Convert.ToBoolean(row["esta_activo"]);

                Producto oProducto = new Producto()
                {
                    Codigo = id,
                    Nombre = nombre,
                    Stock = stock,
                    Activo = activo
                };
                lst.Add(oProducto);
            }
            return lst;
        }

        public Producto GetById(int id)
        {
            var parameters = new List<ParameterSQL>();
            parameters.Add(new ParameterSQL("@codigo", id));
            DataTable t = DataHelper.GetInstance().ExecuteSPQuery("SP_RECUPERAR_PRODUCTO_POR_CODIGO", parameters);

            if (t != null && t.Rows.Count == 1)
            {
                DataRow row = t.Rows[0];
                int codigo = Convert.ToInt32(row["codigo"]);
                string nombre = row["n_producto"].ToString();
                int stock = Convert.ToInt32(row["stock"]);
                bool activo = Convert.ToBoolean(row["esta_activo"]);

                Producto oProducto = new Producto()
                {
                    Codigo = codigo,
                    Nombre = nombre,
                    Stock = stock,
                    Activo = activo
                };
                return oProducto;

            }
            return null;
        }

        public bool Save(Producto oProducto)
        {
            bool result = true;
            string query = "SP_GUARDAR_PRODUCTO";

            try
            {
                if (oProducto != null)
                {
                    _connection.Open();
                    var cmd = new SqlCommand(query, _connection);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@codigo", oProducto.Codigo);
                    cmd.Parameters.AddWithValue("@nombre", oProducto.Nombre);
                    cmd.Parameters.AddWithValue("@stock", oProducto.Stock);
                    result = cmd.ExecuteNonQuery() == 1;
                }
            }
            catch (SqlException sqlEx)
            {
                result = false;
            }
            finally
            {
                if (_connection != null && _connection.State == System.Data.ConnectionState.Open)
                {
                    _connection.Close();
                }
            }
            return result;
        }
    }
}
