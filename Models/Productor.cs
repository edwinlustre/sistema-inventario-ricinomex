namespace appInventario.Models
{
    public class Productor
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellidopat { get; set; }
        public string Apellidomat { get; set; }
        public string Curp { get; set; }
        public string Municipio { get; set; }
        public string Localidad { get; set; }
        public string Tipo { get; set; }
        public string Hectareas { get; set; }
        public string Fecha { get; set; }
        public string Telefono { get; set; }

        // Constructor con todos los parámetros
        public Productor(int id, string nombre, string apellidopat, string apellidomat, string curp, string municipio, string localidad, string tipo, string hectareas, string fecha, string telefono)
        {
            Id = id;  // Asignación correcta
            Nombre = nombre;
            Apellidopat = apellidopat;
            Apellidomat = apellidomat;
            Curp = curp;
            Municipio = municipio;
            Localidad = localidad;
            Tipo = tipo;
            Hectareas = hectareas;
            Fecha = fecha;
            Telefono = telefono;
        }

        // Constructor sin el ID
        public Productor(string nombre, string apellidopat, string apellidomat, string curp, string municipio, string localidad, string tipo, string hectareas, string fecha, string telefono)
        {
            Nombre = nombre;
            Apellidopat = apellidopat;
            Apellidomat = apellidomat;
            Curp = curp;
            Municipio = municipio;
            Localidad = localidad;
            Tipo = tipo;
            Hectareas = hectareas;
            Fecha = fecha;
            Telefono = telefono;
        }
    }
}
