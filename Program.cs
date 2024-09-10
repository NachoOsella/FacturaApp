using NegocioApp.Dominio;
using NegocioApp.Services;

ProductoManager manager = new ProductoManager();

// Crear nuevo producto:
var nuevoProducto = new Producto()
{
    Nombre = "NUEVO PRODUCTO",
    Stock = 1500,
    Activo = true
};
if (manager.SaveProduct(nuevoProducto))
    Console.WriteLine("¡PRODUCTO CREADO EXITOSAMENTE!");

// Listar todos los productos de la tienda:
List<Producto> productos = manager.GetProducts();
if (productos.Count == 0)
{
    Console.WriteLine("Sin productos en la base de datos");
}
else
{
    foreach (var producto in productos)
    {
        Console.WriteLine(producto);
    }
}

// Eliminar producto con código = 2:
if (manager.DeleteProduct(2))
    Console.WriteLine("¡PRODUCTO ELIMINADO EXITOSAMENTE!");

FacturaManager facturaManager = new FacturaManager();
var presupuestos = facturaManager.GetBudgets();
var presupuesto01 = facturaManager.GetBudgetsById(1);