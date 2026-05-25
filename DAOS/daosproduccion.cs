using appInventario.DB;
using appInventario.Models;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace appInventario.DAOS
{
    public class daosproduccion
    {
        private conexiondb db;

        public daosproduccion()
        {
            //Credenciales para iniciar sesión
            db = new conexiondb("IP", "usuario", "base", "password");
        }

        public void inventarioProducto(string producto, string capacidad, string guardado, string contenido)
        {
            try
            {
                string query = "INSERT INTO inventario_producto (producto, capacidad, guardado, contenido) VALUES (@producto, @capacidad, @guardado, @contenido)";

                using (var connection = db.GetConnection())
                {
                    connection.Open(); // Abrir la conexión aquí

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@producto", producto);
                        command.Parameters.AddWithValue("@capacidad", capacidad);
                        command.Parameters.AddWithValue("@guardado", guardado);
                        command.Parameters.AddWithValue("@contenido", contenido);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar el producto: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void registroPedido(string fecha_ingreso, string kilos, string producto, string presentacion, string fecha_entrega, string estado)
        {
            try
            {
                string query = "INSERT INTO registro_pedido (fecha_ingreso, kilos, producto, presentacion, fecha_entrega, estado) VALUES (@fecha_ingreso, @kilos, @producto, @presentacion, @fecha_entrega, @estado)";

                using (var connection = db.GetConnection())
                {
                    connection.Open(); // Abrir la conexión aquí

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@fecha_ingreso", fecha_ingreso);
                        command.Parameters.AddWithValue("@kilos", kilos);
                        command.Parameters.AddWithValue("@producto", producto);
                        command.Parameters.AddWithValue("@presentacion", presentacion);
                        command.Parameters.AddWithValue("@fecha_entrega", fecha_entrega);
                        command.Parameters.AddWithValue("@estado", estado);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar el pedido: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        //Obtener disponibilidad

        // Método para verificar disponibilidad
        public bool disponibilidad(string producto, string guardado)
        {
            bool disponibilidad = false;

            try
            {
                string query = "SELECT COUNT(*) FROM inventario_producto WHERE producto LIKE @producto AND guardado LIKE @guardado";

                using (var connection = db.GetConnection())
                {
                    connection.Open(); // Abrir la conexión aquí

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@producto", producto);
                        command.Parameters.AddWithValue("@guardado", guardado);

                        object result = command.ExecuteScalar();
                        int total = result != DBNull.Value ? Convert.ToInt32(result) : 0;

                        // Si hay al menos un producto disponible, indicamos que hay disponibilidad
                        disponibilidad = total > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener la disponibilidad de productos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return disponibilidad;
        }

        // Método para eliminar un solo producto que coincida
        public void actualizarPresentacion(string producto, string guardado, int unidadesNecesarias)
        {
            try
            {
                string query = "DELETE FROM inventario_producto WHERE producto = @producto AND guardado = @guardado LIMIT @limite";

                using (var connection = db.GetConnection())
                {
                    connection.Open(); // Abrir la conexión aquí

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@producto", producto);
                        command.Parameters.AddWithValue("@guardado", guardado);
                        command.Parameters.AddWithValue("@limite", unidadesNecesarias);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            //MessageBox.Show($"Se eliminaron {rowsAffected} unidades de {producto} del inventario.");
                        }
                        else
                        {
                            MessageBox.Show("No se encontró suficiente inventario para eliminar la cantidad solicitada.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar el envase del inventario: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public DataTable obtenerTodosPedidos()
        {
            DataTable dataTable = new DataTable();

            try
            {
                string query = "SELECT * FROM registro_pedido";

                using (var connection = db.GetConnection())
                {
                    connection.Open(); // Abrir la conexión aquí

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
                MessageBox.Show("Error al obtener los pedidos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return dataTable;
        }

        public int obtenerTotalKgPorMes(int mes, int año)
        {
            int totalKg = 0;

            try
            {
                // Construir el patrón de búsqueda de fecha con un comodín
                string fechaInicio = $"%{mes:D2}-{año}%";  // Formato %MM-YYYY%

                string query = "SELECT SUM(kg) AS kg FROM compra_grano WHERE fecha_compra LIKE @fechaInicio";

                using (var connection = db.GetConnection())
                {
                    connection.Open(); // Abrir la conexión aquí

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@fechaInicio", fechaInicio);

                        object result = command.ExecuteScalar();
                        totalKg = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener el total de kilos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return totalKg;
        }

        public int[] obtenerTotalKgPorMes(int año)
        {
            int[] kilosPorMes = new int[12];

            try
            {
                // Modificamos la consulta para convertir la cadena en fecha y extraer mes y año
                string query = "SELECT MONTH(STR_TO_DATE(fecha_compra, '%d-%m-%Y')) AS mes, SUM(kg) AS kg_total " +
                               "FROM compra_grano " +
                               "WHERE YEAR(STR_TO_DATE(fecha_compra, '%d-%m-%Y')) = @año " +
                               "GROUP BY MONTH(STR_TO_DATE(fecha_compra, '%d-%m-%Y'))";

                using (var connection = db.GetConnection())
                {
                    connection.Open(); // Abrir la conexión aquí

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@año", año);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int mes = reader.GetInt32("mes");
                                int kg = reader.GetInt32("kg_total");
                                kilosPorMes[mes - 1] = kg; // Guardar el total en el array (mes - 1 porque el array es 0-based)
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener los kilos por mes: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return kilosPorMes;
        }

        public void UpdateShopStatus(int id, string estado)
        {
            string query = "UPDATE registro_pedido SET estado = @estado WHERE id = @id";

            try
            {
                using (var connection = db.GetConnection())
                {
                    connection.Open(); // Abrir la conexión aquí

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@estado", estado);
                        command.Parameters.AddWithValue("@id", id);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar el estado de pago: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public Productos contarProductosDisponibles(string lugarAlmacenamiento)
        {
            Productos productos = new Productos();

            try
            {
                using (var connection = db.GetConnection())
                {
                    connection.Open(); // Abrir la conexión aquí

                    string query = "SELECT producto, COUNT(*) AS Total " +
                                   "FROM inventario_producto " +
                                   "WHERE producto IN ('Tote', 'Tambor metálico', 'Tambor plástico', 'Garrafa 70L', 'Garrafa 50L', 'Garrafa 20L', 'Garrafa 10L', 'Garrafa 5L', 'Botella 1L') " +
                                   "AND guardado = @LugarAlmacenamiento " +
                                   "GROUP BY producto";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@LugarAlmacenamiento", lugarAlmacenamiento);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string tipoContenedor = reader.GetString("producto");
                                int total = reader.GetInt32("Total");

                                // Asignar valores basados en el tipo de contenedor
                                switch (tipoContenedor)
                                {
                                    case "Tote": productos.Tote = total; break;
                                    case "Tambor metálico": productos.Tamborm = total; break;
                                    case "Tambor plástico": productos.Tamborp = total; break;
                                    case "Garrafa 70L": productos.G70 = total; break;
                                    case "Garrafa 50L": productos.G50 = total; break;
                                    case "Garrafa 20L": productos.G20 = total; break;
                                    case "Garrafa 10L": productos.G10 = total; break;
                                    case "Garrafa 5L": productos.G5 = total; break;
                                    case "Botella 1L": productos.B1 = total; break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al contar los contenedores: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return productos;
        }

        public int obtenerProductoTerminado(int mes, int año)
        {
            int totalT = 0;

            try
            {
                using (var connection = db.GetConnection())
                {
                    connection.Open(); // Abrir la conexión

                    // Construir el patrón de búsqueda de fecha con un comodín
                    string fechaInicio = $"%-{mes.ToString("D2")}-{año}%";  // Formato %MM-YYYY%

                    string query = "SELECT SUM(cantidad) AS cantidad FROM producto_terminado WHERE fecha LIKE @fechaInicio";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@fechaInicio", fechaInicio);

                        object result = command.ExecuteScalar();
                        totalT = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener el total de producto terminado: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return totalT;
        }

        public int[] obtenerProductoTerminadoPorMes(int año)
        {
            int[] productoPorMes = new int[12];

            try
            {
                using (var connection = db.GetConnection())
                {
                    connection.Open(); // Abrir la conexión

                    // Consulta para obtener la suma de cantidades agrupadas por mes
                    string query = "SELECT MONTH(STR_TO_DATE(fecha, '%d-%m-%Y')) AS mes, SUM(cantidad) AS cantidad_total " +
                                   "FROM producto_terminado " +
                                   "WHERE YEAR(STR_TO_DATE(fecha, '%d-%m-%Y')) = @año " +
                                   "GROUP BY MONTH(STR_TO_DATE(fecha, '%d-%m-%Y'))";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@año", año);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int mes = reader.GetInt32("mes");
                                int cantidad = reader.GetInt32("cantidad_total");
                                productoPorMes[mes - 1] = cantidad; // Guardar el total en el array (mes - 1 porque el array es 0-based)
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener los productos terminados por mes: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return productoPorMes;
        }

        public void RegistrarProductoTerminado(ProductoTerminado producto)
        {
            try
            {
                using (var connection = db.GetConnection())
                {
                    connection.Open(); // Abrir la conexión

                    string query = "INSERT INTO producto_terminado (fecha, cantidad) VALUES (@fecha, @cantidad)";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@fecha", producto.fechaIngreso);
                        command.Parameters.AddWithValue("@cantidad", producto.Cantidad);

                        command.ExecuteNonQuery(); // Ejecutar la consulta
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar el producto: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
