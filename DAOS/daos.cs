using appInventario.DB;
using appInventario.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace appInventario.DAOS
{
    public class daos
    {
        private conexiondb db;


        public daos()
        {
            //Credenciales para iniciar sesión
            db = new conexiondb("IP", "usuario", "base", "password");
        }

        // Función para verificar si el usuario existe y obtener su área, nombre y apellido paterno
        public (string area, string nombre, string apellidoPat) obtenerDatosUsuario(string usuario, string contraseña)
        {
            string area = null;
            string nombre = null;
            string apellidoPat = null;

            string query = "SELECT area, nombre, apellidopat, contrasena FROM registro_usuarios WHERE BINARY usuario = @usuario";

            // Usar using para asegurar que la conexión se cierra adecuadamente
            using (var connection = db.GetConnection())
            {
                try
                {
                    connection.Open(); // Abrir la conexión aquí

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@usuario", usuario);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Obtener la contraseña encriptada desde la base de datos
                                string contraseñaEncriptadaBD = reader["contrasena"].ToString();

                                // Encriptar la contraseña ingresada por el usuario
                                var encriptador = new EncriptadoContraseñas();
                                string contraseñaEncriptadaUsuario = encriptador.HashPassword(contraseña);

                                // Comparar las contraseñas encriptadas
                                if (contraseñaEncriptadaBD == contraseñaEncriptadaUsuario)
                                {
                                    area = reader["area"].ToString();
                                    nombre = reader["nombre"].ToString();
                                    apellidoPat = reader["apellidopat"].ToString();
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al verificar el usuario: " + ex.Message, "Advertencia");
                }
                // No es necesario llamar a CloseConnection(), ya que using se encarga de cerrar la conexión
            }

            return (area, nombre, apellidoPat);  // Regresa el área, nombre y apellido
        }

        //Cambiar contraseña

        public bool CambiarContraseña(string usuario, string nuevaContraseña)
        {
            bool exito = false;

            var encriptador = new EncriptadoContraseñas();
            string contraseñaEncriptadaUsuario = encriptador.HashPassword(nuevaContraseña);

            string query = "UPDATE registro_usuarios SET contrasena = @nuevaContrasena WHERE usuario = @usuario";

            try
            {
                using (var connection = db.GetConnection())
                {
                    connection.Open();  // Abrir la conexión aquí

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@nuevaContrasena", contraseñaEncriptadaUsuario);
                        command.Parameters.AddWithValue("@usuario", usuario);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            exito = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cambiar la contraseña: " + ex.Message, "Error");
            }
            // No es necesario llamar a CloseConnection(), ya que using se encarga de cerrar la conexión

            return exito;
        }



        /* --- Aqui es donde empieza el CRUD del productor --- */

        public void guardarProductor(Productor productor)
        {
            string query = "INSERT INTO registro_productores (nombre, apellidopat, apellidomat, curp, municipio, localidad, tipo, hectareas, fecha_registro, telefono) " +
                           "VALUES (@nombre, @apellidopat, @apellidomat, @curp, @municipio, @localidad, @tipo, @hectareas, @fecha, @telefono)";

            try
            {
                using (var connection = db.GetConnection())
                {
                    connection.Open();  // Abrir la conexión aquí

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@nombre", productor.Nombre);
                        command.Parameters.AddWithValue("@apellidopat", productor.Apellidopat);
                        command.Parameters.AddWithValue("@apellidomat", productor.Apellidomat);
                        command.Parameters.AddWithValue("@curp", productor.Curp);
                        command.Parameters.AddWithValue("@municipio", productor.Municipio);
                        command.Parameters.AddWithValue("@localidad", productor.Localidad);
                        command.Parameters.AddWithValue("@tipo", productor.Tipo);
                        command.Parameters.AddWithValue("@hectareas", productor.Hectareas);
                        command.Parameters.AddWithValue("@fecha", productor.Fecha);
                        command.Parameters.AddWithValue("@telefono", productor.Telefono);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al guardar el productor: " + ex.Message);
            }
            // No es necesario llamar a CloseConnection(), ya que using se encarga de cerrar la conexión
        }

        public List<Productor> BuscarProductorPorNombreYApellidos(string nombreOApellidos)
        {
            var productores = new List<Productor>();

            // Dividir los términos de búsqueda en partes
            var partes = nombreOApellidos.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (partes.Length == 0)
            {
                throw new Exception("Debe ingresar al menos un nombre o apellido.");
            }

            // Construir la consulta SQL de manera dinámica
            var query = "SELECT * FROM registro_productores WHERE";
            var condiciones = new List<string>();
            var parametros = new List<MySqlParameter>();

            for (int i = 0; i < partes.Length; i++)
            {
                var parametroNombre = "@param" + i;
                condiciones.Add($"(nombre LIKE {parametroNombre} OR apellidopat LIKE {parametroNombre} OR apellidomat LIKE {parametroNombre})");
                parametros.Add(new MySqlParameter(parametroNombre, "%" + partes[i] + "%"));
            }

            query += string.Join(" AND ", condiciones);

            try
            {
                using (var connection = db.GetConnection())
                {
                    connection.Open();  // Abrir la conexión aquí

                    using (var command = new MySqlCommand(query, connection))
                    {
                        // Agregar los parámetros al comando
                        command.Parameters.AddRange(parametros.ToArray());

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Asegurarse de que el campo ID existe en el lector
                                int id = reader["ID"] != DBNull.Value ? int.Parse(reader["ID"].ToString()) : 0;

                                productores.Add(new Productor(
                                    id,
                                    reader["nombre"].ToString(),
                                    reader["apellidopat"].ToString(),
                                    reader["apellidomat"].ToString(),
                                    reader["curp"].ToString(),
                                    reader["municipio"].ToString(),
                                    reader["localidad"].ToString(),
                                    reader["tipo"].ToString(),
                                    reader["hectareas"].ToString(),
                                    reader["fecha_registro"].ToString(),
                                    reader["telefono"].ToString()
                                ));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar el productor: " + ex.Message, "Error");
            }
            // No es necesario llamar a CloseConnection(), ya que using se encarga de cerrar la conexión

            return productores;
        }

        public DataTable ObtenerTodosProductores()
        {
            var dataTable = new DataTable();
            string query = "SELECT * FROM registro_productores";

            try
            {
                using (var connection = db.GetConnection())
                {
                    connection.Open();  // Abrir la conexión aquí

                    using (var command = new MySqlCommand(query, connection))
                    {
                        using (var adapter = new MySqlDataAdapter(command))
                        {
                            // Llenar el DataTable con los resultados
                            adapter.Fill(dataTable);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener los productores: " + ex.Message, "Error");
            }
            // No es necesario llamar a CloseConnection(), ya que using se encarga de cerrar la conexión

            return dataTable;
        }

        public void actualizarProductor(Productor productor, int id)
        {
            string query = "UPDATE registro_productores SET " +
                           "nombre = @nombre, " +
                           "apellidopat = @apellidopat, " +
                           "apellidomat = @apellidomat, " +
                           "curp = @curp, " +
                           "telefono = @telefono, " +
                           "municipio = @municipio, " +
                           "localidad = @localidad, " +
                           "tipo = @tipo, " +
                           "hectareas = @hectareas, " +
                           "fecha_registro = @fecha " +
                           "WHERE id = @id";

            try
            {
                using (var connection = db.GetConnection())
                {
                    connection.Open();  // Abrir la conexión aquí

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@nombre", productor.Nombre);
                        command.Parameters.AddWithValue("@apellidopat", productor.Apellidopat);
                        command.Parameters.AddWithValue("@apellidomat", productor.Apellidomat);
                        command.Parameters.AddWithValue("@curp", productor.Curp);
                        command.Parameters.AddWithValue("@telefono", productor.Telefono);
                        command.Parameters.AddWithValue("@municipio", productor.Municipio);
                        command.Parameters.AddWithValue("@localidad", productor.Localidad);
                        command.Parameters.AddWithValue("@tipo", productor.Tipo);
                        command.Parameters.AddWithValue("@hectareas", productor.Hectareas);
                        command.Parameters.AddWithValue("@fecha", productor.Fecha);
                        command.Parameters.AddWithValue("@id", id);

                        int rowsAffected = command.ExecuteNonQuery();

                        // Opcional: Mensaje para confirmar el número de filas actualizadas
                        // MessageBox.Show($"Filas actualizadas: {rowsAffected}", "Resultado de la actualización");
                    }
                }
            }
            catch (Exception ex)
            {
                // Mostrar cualquier error que ocurra
                MessageBox.Show("Error al actualizar el productor: " + ex.Message, "Error");
            }
            // No es necesario llamar a CloseConnection(), ya que using se encarga de cerrar la conexión
        }

        public void EliminarProductorPorId(int id)
        {
            string query = "DELETE FROM registro_productores WHERE ID = @id";

            try
            {
                using (var connection = db.GetConnection())
                {
                    connection.Open();  // Abrir la conexión aquí

                    using (var command = new MySqlCommand(query, connection))
                    {
                        // Agregar el parámetro ID al comando
                        command.Parameters.AddWithValue("@id", id);

                        // Ejecutar la consulta
                        int filasAfectadas = command.ExecuteNonQuery();

                        if (filasAfectadas == 0)
                        {
                            MessageBox.Show("No se encontró ningún productor con el ID especificado.", "Información");
                        }
                        else
                        {
                            MessageBox.Show("Productor eliminado exitosamente.", "Éxito");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar el productor: " + ex.Message, "Error");
            }
            // No es necesario llamar a CloseConnection(), ya que using se encarga de cerrar la conexión
        }


        /* --- Aqui es donde empieza compra de grano --- */

        public int RegistrarCompraGrano(int idProductor, string nombre, string apellidopat, string apellidomat, string hectareas, string telefono, string localidad, string grano, string precio, string importe, string fecha, string kg)
        {
            int nuevoId = 0;
            string queryRegistro = "INSERT INTO compra_grano (id_productor, nombre, apellidopat, apellidomat, hectareas, num_telefono, localidad, grano, precio, importe, fecha_compra, kg) " +
                                   "VALUES (@id_productor, @nombre, @apellidopat, @apellidomat, @hectareas, @telefono, @localidad, @grano, @precio, @importe, @fecha, @kg);" +
                                   "SELECT LAST_INSERT_ID();";

            try
            {
                using (var connection = db.GetConnection())
                {
                    connection.Open();  // Abrir la conexión aquí

                    using (var command = new MySqlCommand(queryRegistro, connection))
                    {
                        command.Parameters.AddWithValue("@id_productor", idProductor);
                        command.Parameters.AddWithValue("@nombre", nombre);
                        command.Parameters.AddWithValue("@apellidopat", apellidopat);
                        command.Parameters.AddWithValue("@apellidomat", apellidomat);
                        command.Parameters.AddWithValue("@hectareas", hectareas);
                        command.Parameters.AddWithValue("@telefono", telefono);
                        command.Parameters.AddWithValue("@localidad", localidad);
                        command.Parameters.AddWithValue("@grano", grano);
                        command.Parameters.AddWithValue("@precio", precio);
                        command.Parameters.AddWithValue("@importe", importe);
                        command.Parameters.AddWithValue("@fecha", fecha);
                        command.Parameters.AddWithValue("@kg", kg);

                        // Ejecutar la consulta y obtener el ID generado
                        nuevoId = Convert.ToInt32(command.ExecuteScalar());
                    }
                }

                MessageBox.Show("Registro de compra exitoso.", "Éxito");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar la compra: " + ex.Message, "Error");
            }

            return nuevoId;
        }

        public DataTable SearchProducers(string searchTerm)
        {
            // Dividir el término de búsqueda en palabras
            string[] searchTerms = searchTerm.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            // Construir la consulta SQL
            StringBuilder queryBuilder = new StringBuilder("SELECT * FROM registro_productores WHERE ");

            // Crear condiciones para cada término de búsqueda
            for (int i = 0; i < searchTerms.Length; i++)
            {
                if (i > 0) queryBuilder.Append(" AND ");
                queryBuilder.Append("(");
                queryBuilder.Append("nombre LIKE @searchTerm" + i);
                queryBuilder.Append(" OR apellidopat LIKE @searchTerm" + i);
                queryBuilder.Append(" OR apellidomat LIKE @searchTerm" + i);
                queryBuilder.Append(")");
            }

            string query = queryBuilder.ToString();
            DataTable resultsTable = new DataTable();

            try
            {
                using (var connection = db.GetConnection())
                {
                    connection.Open();  // Abrir la conexión aquí

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        // Añadir parámetros para cada término de búsqueda
                        for (int i = 0; i < searchTerms.Length; i++)
                        {
                            command.Parameters.AddWithValue("@searchTerm" + i, "%" + searchTerms[i] + "%");
                        }

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            adapter.Fill(resultsTable);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar productores: " + ex.Message, "Error");
            }

            // No es necesario llamar a CloseConnection(), ya que using se encarga de cerrar la conexión

            return resultsTable;
        }


        /* --- Aqui es donde empieza la parte de compra de costales --- */

        public void RegistrarCompraCostales(int idProductor, string nombre, string apellidopat, string apellidomat, string localidad, string telefono, int cantidadCostales, string kg, string grano, string fecha, string estado)
        {
            try
            {
                // Usar using para manejar la conexión automáticamente
                using (var connection = db.GetConnection())
                {
                    connection.Open();  // Abrir la conexión aquí

                    string query = "INSERT INTO compra_costales (id_productor, nombre, apellidopat, apellidomat, localidad, telefono, cantidad_costales, kg, grano, fecha_entrada, estado_pago) " +
                                   "VALUES (@id_productor, @nombre, @apellidopat, @apellidomat, @localidad, @telefono, @cantidad_costales, @kg, @grano, @fecha, @estado)";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        // Agregar los parámetros al comando
                        command.Parameters.AddWithValue("@id_productor", idProductor);
                        command.Parameters.AddWithValue("@nombre", nombre);
                        command.Parameters.AddWithValue("@apellidopat", apellidopat);
                        command.Parameters.AddWithValue("@apellidomat", apellidomat);
                        command.Parameters.AddWithValue("@localidad", localidad);
                        command.Parameters.AddWithValue("@telefono", telefono);
                        command.Parameters.AddWithValue("@cantidad_costales", cantidadCostales);
                        command.Parameters.AddWithValue("@kg", kg);
                        command.Parameters.AddWithValue("@grano", grano);
                        command.Parameters.AddWithValue("@fecha", fecha);
                        command.Parameters.AddWithValue("@estado", estado);

                        // Ejecutar la consulta
                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Registro de compra exitoso.", "Éxito");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar la compra: " + ex.Message, "Error");
            }
        }

        public int ObtenerUltimoIdInsertado()
        {
            try
            {
                using (var connection = db.GetConnection())
                {
                    connection.Open();  // Abrir la conexión aquí

                    string query = "SELECT LAST_INSERT_ID();";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        return Convert.ToInt32(command.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener el último ID insertado: " + ex.Message, "Error");
                return -1; // Indicar un error
            }
        }


        public void ActualizarFolio(int id, string folio)
        {
            try
            {
                using (var connection = db.GetConnection())
                {
                    connection.Open();  // Abrir la conexión aquí

                    string query = "UPDATE compra_costales SET folio = @folio WHERE id = @id";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@folio", folio);
                        command.Parameters.AddWithValue("@id", id);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar el folio: " + ex.Message, "Error");
            }
        }

        public DataTable obtenerCompraCostales()
        {
            DataTable dataTable = new DataTable();
            string query = "SELECT * FROM compra_costales";

            try
            {
                using (var connection = db.GetConnection())
                {
                    connection.Open();  // Abrir la conexión aquí

                    using (var command = new MySqlCommand(query, connection))
                    {
                        using (var adapter = new MySqlDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener los datos: " + ex.Message, "Error");
            }

            return dataTable;
        }


        public void UpdatePaymentStatus(int id, string precio, string importe, string estadoPago)
        {
            string query = "UPDATE compra_costales SET precio = @precio, importe = @importe, estado_pago = @estado_pago WHERE id = @id";

            try
            {
                using (var connection = db.GetConnection())
                {
                    connection.Open();  // Abrir la conexión aquí

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@precio", precio);
                        command.Parameters.AddWithValue("@importe", importe);
                        command.Parameters.AddWithValue("@estado_pago", estadoPago);
                        command.Parameters.AddWithValue("@id", id);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar el estado de pago: " + ex.Message, "Error");
            }
        }


        /* --- Aqui es donde empieza la parte de compra de semilla --- */

        public int RegistrarCompraSemilla(int idProductor, string nombre, string apellidopat, string apellidomat, string hectareas, string telefono, string localidad, string grano, string precio, string importe, string fecha, string kg)
        {
            int nuevoId = 0;

            try
            {
                using (var connection = db.GetConnection())
                {
                    connection.Open();  // Abrir la conexión aquí

                    string queryRegistro = "INSERT INTO compra_semillas (id_productor, nombre, apellidopat, apellidomat, hectareas, telefono, localidad, grano, precio, importe, fecha_compra, kg) " +
                                           "VALUES (@id_productor, @nombre, @apellidopat, @apellidomat, @hectareas, @telefono, @localidad, @grano, @precio, @importe, @fecha, @kg); " +
                                           "SELECT LAST_INSERT_ID();";

                    using (var command = new MySqlCommand(queryRegistro, connection))
                    {
                        command.Parameters.AddWithValue("@id_productor", idProductor);
                        command.Parameters.AddWithValue("@nombre", nombre);
                        command.Parameters.AddWithValue("@apellidopat", apellidopat);
                        command.Parameters.AddWithValue("@apellidomat", apellidomat);
                        command.Parameters.AddWithValue("@hectareas", hectareas);
                        command.Parameters.AddWithValue("@telefono", telefono);
                        command.Parameters.AddWithValue("@localidad", localidad);
                        command.Parameters.AddWithValue("@grano", grano);
                        command.Parameters.AddWithValue("@precio", precio);
                        command.Parameters.AddWithValue("@importe", importe);
                        command.Parameters.AddWithValue("@fecha", fecha);
                        command.Parameters.AddWithValue("@kg", kg);

                        // Ejecutar la consulta y obtener el ID generado
                        nuevoId = Convert.ToInt32(command.ExecuteScalar());
                    }
                }

                MessageBox.Show("Registro de compra exitoso.", "Éxito");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar la compra: " + ex.Message, "Error");
            }

            return nuevoId; // Devolver el ID auto incremental generado
        }

        /* --- Aqui es donde se vizualiza la compra de grano o semilla --- */

        public DataTable ObtenerCompraGrano()
        {
            DataTable dataTable = new DataTable();

            try
            {
                using (var connection = db.GetConnection())
                {
                    connection.Open();  // Abrir la conexión aquí

                    string query = "SELECT * FROM compra_grano";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        using (var adapter = new MySqlDataAdapter(command))
                        {
                            // Llenar el DataTable con los resultados
                            adapter.Fill(dataTable);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener las compras de grano: " + ex.Message, "Error");
            }

            return dataTable;
        }


        public DataTable ObtenerCompraSemilla()
        {
            DataTable dataTable = new DataTable();

            try
            {
                using (var connection = db.GetConnection())
                {
                    connection.Open();  // Abrir la conexión aquí

                    string query = "SELECT * FROM compra_semillas";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        using (var adapter = new MySqlDataAdapter(command))
                        {
                            // Llenar el DataTable con los resultados
                            adapter.Fill(dataTable);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener las compras de semilla: " + ex.Message, "Error");
            }

            return dataTable;
        }


        // Método para obtener los datos de la compra
        public Dictionary<string, string> obtenerDatosCompra(int id)
        {
            var datos = new Dictionary<string, string>();

            try
            {
                using (var connection = db.GetConnection())
                {
                    connection.Open();  // Abrir la conexión aquí

                    string query = "SELECT nombre, apellidopat, apellidomat, telefono, localidad, grano, fecha_entrada, kg " +
                                   "FROM compra_costales WHERE id = @id";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                datos["nombre"] = reader["nombre"].ToString();
                                datos["apellidopat"] = reader["apellidopat"].ToString();
                                datos["apellidomat"] = reader["apellidomat"].ToString();
                                datos["telefono"] = reader["telefono"].ToString();
                                datos["localidad"] = reader["localidad"].ToString();
                                datos["grano"] = reader["grano"].ToString();
                                datos["fecha_entrada"] = reader["fecha_entrada"].ToString();
                                datos["kg"] = reader["kg"].ToString();
                            }
                            else
                            {
                                MessageBox.Show("No se encontraron datos para el ID proporcionado.", "Información");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener los datos de la compra: " + ex.Message, "Error");
            }

            return datos;
        }

        public string ObtenerHectareaProductor(int id)
        {
            string hectareas = null;

            try
            {
                using (var connection = db.GetConnection())
                {
                    connection.Open();  // Abrir la conexión aquí

                    string query = "SELECT hectareas FROM registro_productores WHERE id = @id";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                hectareas = reader["hectareas"].ToString();
                            }
                            else
                            {
                                MessageBox.Show("No se encontraron datos para el ID proporcionado.", "Información");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener las hectáreas: " + ex.Message, "Error");
            }

            return hectareas;
        }


    }
}
