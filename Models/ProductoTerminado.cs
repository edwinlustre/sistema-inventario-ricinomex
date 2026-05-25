namespace appInventario.Models
{
    public class ProductoTerminado
    {
        public string fechaIngreso { get; set; }
        public string Cantidad { get; set; }

        public ProductoTerminado(string fechaingreso, string cantidad)
        {
            fechaIngreso = fechaingreso;
            Cantidad = cantidad;
        }


    }
}
