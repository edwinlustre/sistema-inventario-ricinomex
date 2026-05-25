namespace appInventario.Models
{
    public class Facturacion
    {
        public string Razon_social { get; set; }

        public string Rfc { get; set; }

        public string Codigo_postal { get; set; }

        public string Regimen_fiscal { get; set; }

        public string Cfdi { get; set; }

        public string Metodo_pago { get; set; }

        public string Forma_pago { get; set; }

        public Facturacion(string razon_social, string rfc, string codigo_postal, string regimen_fiscal, string cfdi, string metodo_pago, string forma_pago)
        {
            Razon_social = razon_social;
            Rfc = rfc;
            Codigo_postal = codigo_postal;
            Regimen_fiscal = regimen_fiscal;
            Cfdi = cfdi;
            Metodo_pago = metodo_pago;
            Forma_pago = forma_pago;

        }

    }
}
