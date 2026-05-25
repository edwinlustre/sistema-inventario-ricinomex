using appInventario.DB;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace appInventario.DAOS
{
    public class daosventas
    {
        private conexiondb db;
        public daosventas()
        {
            //Credenciales para iniciar sesión
            db = new conexiondb("IP", "usuario", "base", "password");
        }

        // Método para abrir la conexión
        public void OpenConnection()
        {
            try
            {
                db.OpenConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al abrir la conexión: " + ex.Message, "Mensaje");
            }
        }

        // Método para cerrar la conexión
        public void CloseConnection()
        {
            try
            {
                db.CloseConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cerrar la conexión: " + ex.Message, "Advertencia");
                // Maneja el error según sea necesario
            }
        }

        /*  --- CRUD DE EMPRESAS --- */
        //Función para registrar una nueva empresa
        public bool registrarEmpresa(string empresa, string nombre, string apellidopat, string apellidomat, string telefono, string correo, string domicilioEntrega, string razons, string rfc, string codigo_postal)
        {
            string query = "INSERT INTO registro_clientes (empresa, nombre, apellidopat, apellidomat, telefono, correo, razon_social, rfc, codigo_postal, domicilio_entrega) " +
                           "VALUES (@empresa, @nombre, @apellidopat, @apellidomat, @telefono, @correo, @razon_social, @rfc, @codigo_postal, @domicilio_entrega)";

            try
            {
                using (var connection = db.GetConnection())
                {
                    connection.Open(); // Abrir conexión

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        // Asignar parámetros
                        cmd.Parameters.AddWithValue("@empresa", empresa);
                        cmd.Parameters.AddWithValue("@nombre", nombre);
                        cmd.Parameters.AddWithValue("@apellidopat", apellidopat);
                        cmd.Parameters.AddWithValue("@apellidomat", apellidomat);
                        cmd.Parameters.AddWithValue("@telefono", telefono);
                        cmd.Parameters.AddWithValue("@correo", correo);
                        cmd.Parameters.AddWithValue("@razon_social", razons);
                        cmd.Parameters.AddWithValue("@rfc", rfc);
                        cmd.Parameters.AddWithValue("@codigo_postal", codigo_postal);
                        cmd.Parameters.AddWithValue("@domicilio_entrega", domicilioEntrega);

                        // Ejecutar la consulta
                        int result = cmd.ExecuteNonQuery();
                        return result > 0; // Retorna true si se inserta al menos una fila
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                MessageBox.Show("Error al registrar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // Función para actualizar una empresa existente
        public bool actualizarEmpresa(int id, string empresa, string nombre, string apellidopat, string apellidomat,
                              string telefono, string correo, string razonSocial, string rfc, string codigoPostal,
                              string domicilioEntrega)
        {
            string query = "UPDATE registro_clientes SET empresa = @empresa, nombre = @nombre, apellidopat = @apellidopat, " +
                           "apellidomat = @apellidomat, telefono = @telefono, correo = @correo, razon_social = @razonSocial, " +
                           "rfc = @rfc, codigo_postal = @codigoPostal, domicilio_entrega = @domicilioEntrega " +
                           "WHERE id = @id";

            try
            {
                using (var connection = db.GetConnection())
                {
                    connection.Open(); // Abrir conexión

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        // Asignar parámetros
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@empresa", empresa);
                        cmd.Parameters.AddWithValue("@nombre", nombre);
                        cmd.Parameters.AddWithValue("@apellidopat", apellidopat);
                        cmd.Parameters.AddWithValue("@apellidomat", apellidomat);
                        cmd.Parameters.AddWithValue("@telefono", telefono);
                        cmd.Parameters.AddWithValue("@correo", correo);
                        cmd.Parameters.AddWithValue("@razonSocial", razonSocial);
                        cmd.Parameters.AddWithValue("@rfc", rfc);
                        cmd.Parameters.AddWithValue("@codigoPostal", codigoPostal);
                        cmd.Parameters.AddWithValue("@domicilioEntrega", domicilioEntrega);

                        // Ejecutar la actualización
                        int result = cmd.ExecuteNonQuery();
                        return result > 0; // Retorna true si se actualizó al menos una fila
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                MessageBox.Show("Error al actualizar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // Función para obtener todas las empresas
        public DataTable mostrarEmpresas()
        {
            string query = "SELECT * FROM registro_clientes";

            try
            {
                using (var connection = db.GetConnection())
                {
                    connection.Open(); // Abrir la conexión solo dentro del bloque using

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);
                            return dataTable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                MessageBox.Show("Error al obtener datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        // Función para eliminar una empresa
        public bool eliminarEmpresa(int id)
        {
            string query = "DELETE FROM registro_clientes WHERE id = @id";

            try
            {
                using (var connection = db.GetConnection())
                {
                    connection.Open(); // Abrir la conexión dentro del bloque using

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@id", id);

                        int result = cmd.ExecuteNonQuery();
                        return result > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores con ícono de advertencia
                MessageBox.Show("Error al eliminar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        //Busqueda de empresas
        public DataTable BuscarEmpresas(string terminoBusqueda)
        {
            string[] palabrasBusqueda = terminoBusqueda.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            // Construir la cláusula WHERE dinámicamente para buscar en múltiples columnas con múltiples términos
            string query = "SELECT * FROM registro_clientes WHERE ";
            List<string> condiciones = new List<string>();

            // Utilizar índices para generar parámetros únicos en lugar de concatenar directamente las palabras
            for (int i = 0; i < palabrasBusqueda.Length; i++)
            {
                string parametro = "@termino" + i;
                condiciones.Add($"(empresa LIKE {parametro} OR " +
                                $"nombre LIKE {parametro} OR " +
                                $"apellidopat LIKE {parametro} OR " +
                                $"apellidomat LIKE {parametro} OR " +
                                $"telefono LIKE {parametro} OR " +
                                $"correo LIKE {parametro} OR " +
                                $"domicilio_entrega LIKE {parametro})");
            }

            // Unir todas las condiciones con AND para asegurarse de que todas las palabras coincidan
            query += string.Join(" AND ", condiciones);

            try
            {
                // Usar using para manejar la conexión
                using (var connection = db.GetConnection())
                {
                    connection.Open(); // Abrir la conexión explícitamente

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        // Agregar los parámetros de búsqueda con los valores correspondientes
                        for (int i = 0; i < palabrasBusqueda.Length; i++)
                        {
                            cmd.Parameters.AddWithValue("@termino" + i, "%" + palabrasBusqueda[i] + "%");
                        }

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);
                            return dataTable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar empresas: " + ex.Message);
                return null;
            }
        }

        /* -- CRUD COTIZACIONES --- */
        public int guardarCotizacion(int idEmpresa, string empresa, string fechacompra, string aceiteGarrafa,
 string garrafas, string numGarrafas, string aceiteTote, string totes, string numTotes,
 string aceiteTambor, string tambores, string numTambores, decimal envio, decimal subtotal, decimal iva, decimal total)
        {
            int nuevoId = 0;

            // Definir la consulta SQL para insertar
            string query = "INSERT INTO cotizacion (id_empresa, empresa, fecha_compra, aceite_garrafa, aceite_tote, aceite_tambor, " +
                            "garrafas, num_garrafas, totes, num_totes, tambores, num_tambores, envio, subtotal, iva, total) " +
                            "VALUES (@IdEmpresa, @Empresa, @FechaCompra, @AceiteGarrafa, @AceiteTote, @AceiteTambor, " +
                            "@Garrafas, @NumGarrafas, @Totes, @NumTotes, @Tambores, @NumTambores, @Envio, @Subtotal, @IVA, @Total); " +
                            "SELECT LAST_INSERT_ID();";  // Obtén el último ID insertado

            try
            {
                // Usar using para manejar la conexión
                using (var connection = db.GetConnection())
                {
                    connection.Open(); // Asegurar que la conexión está abierta

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        // Agregar parámetros
                        cmd.Parameters.AddWithValue("@IdEmpresa", idEmpresa);
                        cmd.Parameters.AddWithValue("@Empresa", empresa);
                        cmd.Parameters.AddWithValue("@FechaCompra", fechacompra);
                        cmd.Parameters.AddWithValue("@AceiteGarrafa", aceiteGarrafa);
                        cmd.Parameters.AddWithValue("@Garrafas", garrafas);
                        cmd.Parameters.AddWithValue("@NumGarrafas", numGarrafas);
                        cmd.Parameters.AddWithValue("@AceiteTote", aceiteTote);
                        cmd.Parameters.AddWithValue("@Totes", totes);
                        cmd.Parameters.AddWithValue("@NumTotes", numTotes);
                        cmd.Parameters.AddWithValue("@AceiteTambor", aceiteTambor);
                        cmd.Parameters.AddWithValue("@Tambores", tambores);
                        cmd.Parameters.AddWithValue("@NumTambores", numTambores);
                        cmd.Parameters.AddWithValue("@Envio", envio);
                        cmd.Parameters.AddWithValue("@Subtotal", subtotal);
                        cmd.Parameters.AddWithValue("@IVA", iva);
                        cmd.Parameters.AddWithValue("@Total", total);

                        // Ejecutar la consulta y obtener el ID generado
                        nuevoId = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar: " + ex.Message);
            }

            return nuevoId;
        }

        // Método para obtener todos los registros de la tabla 'cotizacion'
        public DataTable ObtenerCotizaciones()
        {
            DataTable dtCotizaciones = new DataTable();
            // Cadena de consulta SQL
            string query = "SELECT * FROM cotizacion";

            try
            {
                // Usar using para manejar la conexión y asegurar su cierre
                using (var connection = db.GetConnection())
                {
                    connection.Open(); // Abrir conexión

                    // Crear el comando SQL
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        // Crear el adaptador de datos
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            // Llenar el DataTable con los datos obtenidos
                            adapter.Fill(dtCotizaciones);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                MessageBox.Show("Error al obtener las cotizaciones: " + ex.Message);
            }

            return dtCotizaciones;
        }

        public DataTable ObtenerCotizacionPorId(int id)
        {
            DataTable dtCotizacion = new DataTable();
            string query = "SELECT id, id_empresa, empresa, aceite_garrafa, garrafas, num_garrafas, aceite_tote, totes, num_totes, aceite_tambor, tambores, num_tambores, subtotal FROM cotizacion WHERE id = @id";

            try
            {
                // Usar using para manejar la conexión y asegurar su cierre
                using (var connection = db.GetConnection())
                {
                    connection.Open(); // Abrir conexión

                    // Crear el comando SQL
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@id", id);

                        // Crear el adaptador de datos
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            // Llenar el DataTable con los datos obtenidos
                            adapter.Fill(dtCotizacion);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                MessageBox.Show("Error al obtener la cotización: " + ex.Message);
            }

            return dtCotizacion;
        }

        public DataTable ObtenerClientePorIdEmpresa(int id)
        {
            DataTable dtCliente = new DataTable();
            // Consulta SQL para obtener datos del cliente
            string query = "SELECT empresa, nombre, apellidopat, apellidomat, telefono, correo FROM registro_clientes WHERE id = @id";

            try
            {
                // Usar using para manejar la conexión y asegurar su cierre
                using (var connection = db.GetConnection())
                {
                    connection.Open(); // Abrir conexión

                    // Crear el comando SQL
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@id", id);

                        // Crear el adaptador de datos
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            // Llenar el DataTable con los datos obtenidos
                            adapter.Fill(dtCliente);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                MessageBox.Show("Error al obtener la información del cliente: " + ex.Message);
            }

            return dtCliente;
        }


        public void actualizarCotizacion(string id, string empresa, string fechaCompra, string aceiteGarrafa, string garrafas, string numGarrafas,
                         string aceiteTote, string totes, string numTotes, string aceiteTambor, string tambores, string numTambores, decimal envio,
                         decimal subtotal, decimal iva, decimal total)
        {
            // SQL para actualizar la cotización
            string sql = @"
            UPDATE cotizacion
            SET 
                fecha_compra = @FechaCompra,
                aceite_garrafa = @AceiteGarrafa,
                garrafas = @Garrafas,
                num_garrafas = @NumGarrafas,
                aceite_tote = @AceiteTote,
                totes = @Totes,
                num_totes = @NumTotes,
                aceite_tambor = @AceiteTambor,
                tambores = @Tambores,
                num_tambores = @NumTambores,
                envio = @Envio,
                subtotal = @Subtotal,
                iva = @IVA,
                total = @Total
            WHERE id = @Id";

            try
            {
                // Usar using para manejar la conexión y asegurar su cierre
                using (var connection = db.GetConnection())
                {
                    connection.Open(); // Abrir conexión

                    using (var command = new MySqlCommand(sql, connection))
                    {
                        // Agregar parámetros
                        command.Parameters.AddWithValue("@Id", id);
                        command.Parameters.AddWithValue("@FechaCompra", fechaCompra);
                        command.Parameters.AddWithValue("@AceiteGarrafa", aceiteGarrafa);
                        command.Parameters.AddWithValue("@Garrafas", garrafas);
                        command.Parameters.AddWithValue("@NumGarrafas", numGarrafas);
                        command.Parameters.AddWithValue("@AceiteTote", aceiteTote);
                        command.Parameters.AddWithValue("@Totes", totes);
                        command.Parameters.AddWithValue("@NumTotes", numTotes);
                        command.Parameters.AddWithValue("@AceiteTambor", aceiteTambor);
                        command.Parameters.AddWithValue("@Tambores", tambores);
                        command.Parameters.AddWithValue("@NumTambores", numTambores);
                        command.Parameters.AddWithValue("@Envio", envio);
                        command.Parameters.AddWithValue("@Subtotal", subtotal);
                        command.Parameters.AddWithValue("@IVA", iva);
                        command.Parameters.AddWithValue("@Total", total);

                        // Ejecutar el comando
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar la cotización: " + ex.Message);
            }
        }

        public bool EmpresaExiste(string nombreEmpresa)
        {
            bool existe = false;

            // Cadena de consulta SQL con parámetro
            string query = "SELECT COUNT(*) FROM registro_clientes WHERE empresa = @nombreEmpresa";

            try
            {
                // Usar using para manejar la conexión y asegurar su cierre
                using (var connection = db.GetConnection())
                {
                    connection.Open(); // Abrir conexión

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        // Agregar el parámetro
                        cmd.Parameters.AddWithValue("@nombreEmpresa", nombreEmpresa);

                        // Ejecutar el comando y obtener el resultado
                        int count = Convert.ToInt32(cmd.ExecuteScalar());

                        // Si el conteo es mayor que 0, significa que existe
                        existe = count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                MessageBox.Show("Error al verificar la existencia de la empresa: " + ex.Message);
            }

            return existe;
        }

        //Actualizacion de cotizacion a venta no realizada
        public bool InsertarHistorialCompraYVentaNoConcretada(int idCotizacion, int idEmpresa, string fechaCompra, string aceiteGarrafa, string garrafas, string numGarrafas,
                                                      string aceiteTote, string totes, string numTotes, string aceiteTambor, string tambores, string numTambores,
                                                      string paqueteria, string razon, string rfc, string codigopostal, string regimen, string cfdi, string metodo, string forma)
        {
            bool exito = false;

            // SQL para insertar en historial_compra
            string insertarHistorialSQL = @"
        INSERT INTO historial_compra 
        (id_empresa, fecha_compra, aceite_garrafa, garrafas, num_garrafas, aceite_tote, totes, num_totes, aceite_tambor, tambores, num_tambores, factura)
        VALUES
        (@idEmpresa, @fechaCompra, @aceiteGarrafa, @garrafas, @numGarrafas, @aceiteTote, @totes, @numTotes, @aceiteTambor, @tambores, @numTambores, '');
    ";

            // SQL para insertar en venta_noconcretada
            string insertarVentaNoConcretadaSQL = @"
        INSERT INTO ventas_noconcretadas 
        (id_historial, id_cotizacion, estatus_factura, estatus_pago, estatus_envio, forma_envio, razon, rfc, codigo_postal, regimen, cfdi, metodo_pago, forma_pago)
        VALUES 
        (@idHistorial, @idCotizacion, '', '', '', @formaEnvio, @razon, @rfc, @codigoPostal, @regimen, @cfdi, @metodoPago, @formaPago);
    ";

            try
            {
                // Abrir la conexión
                using (var connection = db.GetConnection())
                {
                    connection.Open(); // Abrir conexión

                    using (var cmd = new MySqlCommand())
                    {
                        cmd.Connection = connection;
                        using (var transaction = connection.BeginTransaction())
                        {
                            try
                            {
                                // Insertar en historial_compra
                                cmd.CommandText = insertarHistorialSQL;
                                cmd.Parameters.AddWithValue("@idEmpresa", idEmpresa);
                                cmd.Parameters.AddWithValue("@fechaCompra", fechaCompra);
                                cmd.Parameters.AddWithValue("@aceiteGarrafa", aceiteGarrafa);
                                cmd.Parameters.AddWithValue("@garrafas", garrafas);
                                cmd.Parameters.AddWithValue("@numGarrafas", numGarrafas);
                                cmd.Parameters.AddWithValue("@aceiteTote", aceiteTote);
                                cmd.Parameters.AddWithValue("@totes", totes);
                                cmd.Parameters.AddWithValue("@numTotes", numTotes);
                                cmd.Parameters.AddWithValue("@aceiteTambor", aceiteTambor);
                                cmd.Parameters.AddWithValue("@tambores", tambores);
                                cmd.Parameters.AddWithValue("@numTambores", numTambores);
                                cmd.ExecuteNonQuery();

                                // Obtener el ID generado en historial_compra
                                long idHistorial = cmd.LastInsertedId;

                                // Insertar en venta_noconcretada
                                cmd.CommandText = insertarVentaNoConcretadaSQL;
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("@idHistorial", idHistorial);
                                cmd.Parameters.AddWithValue("@idCotizacion", idCotizacion);
                                cmd.Parameters.AddWithValue("@formaEnvio", paqueteria);
                                cmd.Parameters.AddWithValue("@razon", razon);
                                cmd.Parameters.AddWithValue("@rfc", rfc);
                                cmd.Parameters.AddWithValue("@codigoPostal", codigopostal);
                                cmd.Parameters.AddWithValue("@regimen", regimen);
                                cmd.Parameters.AddWithValue("@cfdi", cfdi);
                                cmd.Parameters.AddWithValue("@metodoPago", metodo);
                                cmd.Parameters.AddWithValue("@formaPago", forma);
                                cmd.ExecuteNonQuery();

                                // Confirmar la transacción
                                transaction.Commit();
                                exito = true;
                            }
                            catch (Exception ex)
                            {
                                // Revertir la transacción en caso de error
                                transaction.Rollback();
                                MessageBox.Show("Error al realizar la operación: " + ex.Message);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores en caso de problemas con la conexión
                MessageBox.Show("Error al conectar con la base de datos: " + ex.Message);
            }

            return exito;
        }

        // Método para obtener todos los registros de la tabla 'historial_compra'
        public DataTable ObtenerHistorialCompra()
        {
            DataTable dtCotizaciones = new DataTable();
            // Cadena de consulta SQL
            string query = "SELECT * FROM historial_compra";

            try
            {
                // Abrir la conexión
                using (var connection = db.GetConnection())
                {
                    connection.Open(); // Abrir conexión

                    // Crear el comando SQL
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        // Crear el adaptador de datos
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            // Llenar el DataTable con los datos obtenidos
                            adapter.Fill(dtCotizaciones);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                MessageBox.Show("Error al obtener el historial: " + ex.Message);
            }

            return dtCotizaciones;
        }

        // Método para obtener todos los registros de la tabla 'ventas no concretadas'
        public DataTable ObtenerVentasNoConcretadas()
        {
            DataTable dtVentasNoConcretadas = new DataTable();
            // Cadena de consulta SQL
            string query = "SELECT * FROM ventas_noconcretadas";

            try
            {
                // Abrir la conexión
                using (var connection = db.GetConnection())
                {
                    connection.Open(); // Abrir conexión

                    // Crear el comando SQL
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        // Crear el adaptador de datos
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            // Llenar el DataTable con los datos obtenidos
                            adapter.Fill(dtVentasNoConcretadas);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                MessageBox.Show("Error al obtener las ventas no concretadas: " + ex.Message);
            }

            return dtVentasNoConcretadas;
        }

        // Método para actualizar algunos datos de 'ventas no concretadas'
        public void updateVentaNoConcretada(string id, string idHistorial, string linkfactura, string linkpago, string linkenvio, string formaEnvio)
        {
            string sqlUpdateVentasNoConcretadas = @"
            UPDATE ventas_noconcretadas
            SET estatus_factura = @estatusFactura,
                estatus_pago = @estatusPago,
                estatus_envio = @estatusEnvio,
                forma_envio = @formaEnvio
            WHERE id = @id";

            string sqlUpdateHistorialCompra = @"
            UPDATE historial_compra
            SET factura = @estatusFactura
            WHERE id = @idHistorial";

            try
            {
                // Abrir la conexión
                using (var connection = db.GetConnection())
                {
                    connection.Open(); // Abrir conexión

                    using (var transaction = connection.BeginTransaction())
                    using (var cmd = new MySqlCommand())
                    {
                        cmd.Connection = connection;
                        cmd.Transaction = transaction;

                        try
                        {
                            // Actualizar ventas_noconcretadas
                            cmd.CommandText = sqlUpdateVentasNoConcretadas;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@id", id);
                            cmd.Parameters.AddWithValue("@estatusFactura", linkfactura);
                            cmd.Parameters.AddWithValue("@estatusPago", linkpago);
                            cmd.Parameters.AddWithValue("@estatusEnvio", linkenvio);
                            cmd.Parameters.AddWithValue("@formaEnvio", formaEnvio);
                            cmd.ExecuteNonQuery();

                            // Actualizar historial_compra
                            cmd.CommandText = sqlUpdateHistorialCompra;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@estatusFactura", linkfactura);
                            cmd.Parameters.AddWithValue("@idHistorial", idHistorial);
                            cmd.ExecuteNonQuery();

                            // Confirmar la transacción
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            // Revertir la transacción en caso de error
                            transaction.Rollback();
                            MessageBox.Show("Error al actualizar en la base de datos: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al abrir la conexión: " + ex.Message);
            }
        }

        // Método para compartir datos de facturacion con 'historial de compra'
        public void actualizarFacturaVentasNC(string estatusFactura, int idHistorial)
        {
            // Consulta SQL para actualizar el estatus de la factura
            string queryActualizarFactura = "UPDATE historial_compra SET factura = @factura WHERE id = @idHistorial";

            try
            {
                // Crear y abrir la conexión
                using (var connection = db.GetConnection())
                {
                    connection.Open(); // Abrir la conexión

                    using (var cmd = new MySqlCommand(queryActualizarFactura, connection))
                    {
                        // Agregar los parámetros
                        cmd.Parameters.AddWithValue("@factura", estatusFactura);
                        cmd.Parameters.AddWithValue("@idHistorial", idHistorial);

                        // Ejecutar el comando
                        int filasActualizadas = cmd.ExecuteNonQuery();

                        if (filasActualizadas > 0)
                        {
                            MessageBox.Show("Estatus de factura actualizado correctamente.");
                        }
                        else
                        {
                            MessageBox.Show("No se encontró el registro en historial_compra.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                MessageBox.Show("Error al actualizar el estatus de la factura: " + ex.Message);
            }
        }

        // Método para obtener todos los registros de la tabla 'ventas no concretadas'
        public DataTable ObtenerVentasConcretadas()
        {
            DataTable dtCotizaciones = new DataTable();
            string query = "SELECT * FROM ventas_concretadas";

            try
            {
                using (var connection = db.GetConnection())
                {
                    connection.Open(); // Abrir la conexión

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        using (var adapter = new MySqlDataAdapter(cmd))
                        {
                            adapter.Fill(dtCotizaciones);
                        }
                    }

                    // Reemplazar valores nulos con cadenas vacías
                    foreach (DataRow row in dtCotizaciones.Rows)
                    {
                        foreach (DataColumn col in dtCotizaciones.Columns)
                        {
                            if (row[col] == DBNull.Value || row[col] == null)
                            {
                                row[col] = string.Empty;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener las ventas concretadas: " + ex.Message);
            }

            return dtCotizaciones;
        }

        /* --- CRUD VENTAS CONCRETADAS --- */
        public bool ventaConcretada(int id, DataGridViewRow filaSeleccionada)
        {
            bool eliminado = false;

            string queryEliminarVentaNoConcretada = "DELETE FROM ventas_noconcretadas WHERE id = @id";

            try
            {
                using (var connection = db.GetConnection())
                {
                    connection.Open();

                    using (var cmd = new MySqlCommand(queryEliminarVentaNoConcretada, connection))
                    {
                        cmd.Parameters.AddWithValue("@id", id);

                        int filasEliminadas = cmd.ExecuteNonQuery();

                        if (filasEliminadas > 0)
                        {
                            eliminado = true;
                            MessageBox.Show("Venta eliminada correctamente.");
                        }
                        else
                        {
                            MessageBox.Show("No se pudo eliminar la venta.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar la venta: " + ex.Message);
            }

            return eliminado;
        }

        public void insertarVentaConcretada(int idHistorial, int idCotizacion, string estatusFactura)
        {
            // Insertar un nuevo registro en ventas_concretadas con valores predeterminados
            string queryInsertarVenta = @"
        INSERT INTO ventas_concretadas (id_historial, id_cotizacion, comprobante_pago, orden_compra, factura) 
        VALUES (@idHistorial, @idCotizacion, '', '', @estatusFactura)";

            try
            {
                using (var connection = db.GetConnection())
                {
                    connection.Open();

                    using (var cmd = new MySqlCommand(queryInsertarVenta, connection))
                    {
                        // Agregar parámetros
                        cmd.Parameters.AddWithValue("@idHistorial", idHistorial);
                        cmd.Parameters.AddWithValue("@idCotizacion", idCotizacion);
                        cmd.Parameters.AddWithValue("@estatusFactura", estatusFactura);

                        // Ejecutar el comando
                        int filasInsertadas = cmd.ExecuteNonQuery();

                        if (filasInsertadas > 0)
                        {
                            MessageBox.Show("Venta registrada como concretada correctamente.");
                        }
                        else
                        {
                            MessageBox.Show("No se pudo registrar la venta como concretada.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                MessageBox.Show("Error al registrar la venta como concretada: " + ex.Message);
            }
        }

        public string ObtenerNombreEmpresa(int idEmpresa)
        {
            string nombreEmpresa = string.Empty;

            // SQL para obtener el nombre de la empresa o concatenar nombre completo
            string sqlQuery = @"
            SELECT 
                CASE 
                    WHEN empresa IS NULL OR empresa = '' 
                    THEN CONCAT(nombre, ' ', apellidopat, ' ', apellidomat)
                    ELSE empresa 
                END AS nombreCompleto
            FROM registro_clientes 
            WHERE id = @idEmpresa";

            try
            {
                using (var connection = db.GetConnection())
                {
                    connection.Open();

                    using (var cmd = new MySqlCommand(sqlQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@idEmpresa", idEmpresa);
                        object result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            nombreEmpresa = result.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener el nombre de la empresa: " + ex.Message);
            }

            return nombreEmpresa;
        }

        public string ObtenerNombreEmpresaPorHistorial(int idHistorial)
        {
            string nombreEmpresa = string.Empty;

            // Consulta para obtener el id_empresa desde historial_compra
            string sqlSelectIdEmpresa = "SELECT id_empresa FROM historial_compra WHERE id = @idHistorial";

            // Consulta para obtener el nombre de la empresa o concatenar el nombre completo si el campo empresa está vacío o es NULL
            string sqlSelectNombreEmpresa = @"
        SELECT 
            CASE 
                WHEN empresa IS NULL OR empresa = '' 
                THEN CONCAT(nombre, ' ', apellidopat, ' ', apellidomat)
                ELSE empresa 
            END AS nombreCompleto
        FROM registro_clientes 
        WHERE id = @idEmpresa";

            try
            {
                using (var connection = db.GetConnection())
                {
                    connection.Open();

                    // Obtener id_empresa desde historial_compra
                    int idEmpresa;
                    using (var cmd = new MySqlCommand(sqlSelectIdEmpresa, connection))
                    {
                        cmd.Parameters.AddWithValue("@idHistorial", idHistorial);
                        object result = cmd.ExecuteScalar();

                        if (result == null)
                        {
                            return nombreEmpresa; // No se encontró id_empresa
                        }

                        idEmpresa = Convert.ToInt32(result);
                    }

                    // Obtener el nombre de la empresa o concatenar el nombre completo si el campo empresa está vacío
                    using (var cmd = new MySqlCommand(sqlSelectNombreEmpresa, connection))
                    {
                        cmd.Parameters.AddWithValue("@idEmpresa", idEmpresa);
                        object result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            nombreEmpresa = result.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener el nombre de la empresa: " + ex.Message);
            }

            return nombreEmpresa;
        }

        public void CargarDatosClientePorId(int idCliente, ref TextBox txtrazon, ref TextBox txtrfc, ref TextBox txtcodigopostal)
        {
            // Definir la consulta SQL
            string query = @"
                SELECT razon_social, rfc, codigo_postal 
                FROM registro_clientes 
                WHERE id = @idCliente;
            ";

            try
            {
                using (var connection = db.GetConnection())
                {
                    connection.Open();
                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        // Asignar el parámetro de la consulta
                        cmd.Parameters.AddWithValue("@idCliente", idCliente);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Asignar los valores a los TextBox correspondientes
                                txtrazon.Text = reader["razon_social"].ToString();
                                txtrfc.Text = reader["rfc"].ToString();
                                txtcodigopostal.Text = reader["codigo_postal"].ToString();
                            }
                            else
                            {
                                MessageBox.Show("No se encontró un cliente con el ID especificado.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos del cliente: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /* --- CRUD VENDEDOR --- */
        public bool InsertarVendedor(string nombre, string apellidopat, string apellidomat, string numero, string correo)
        {
            // SQL para insertar en registro_vendedor
            string query = @"
                INSERT INTO registro_vendedor (nombre, apellidopat, apellidomat, numero, correo)
                VALUES (@nombre, @apellidopat, @apellidomat, @numero, @correo);
            ";

            try
            {
                using (var connection = db.GetConnection())
                {
                    connection.Open();
                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        // Asignar los parámetros de la consulta
                        cmd.Parameters.AddWithValue("@nombre", nombre);
                        cmd.Parameters.AddWithValue("@apellidopat", apellidopat);
                        cmd.Parameters.AddWithValue("@apellidomat", apellidomat);
                        cmd.Parameters.AddWithValue("@numero", numero);
                        cmd.Parameters.AddWithValue("@correo", correo);

                        // Ejecutar la consulta
                        cmd.ExecuteNonQuery();
                    }
                }

                return true; // Retornar verdadero si la inserción fue exitosa
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al insertar el vendedor: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false; // Retornar falso si ocurrió un error
            }
        }

        public DataTable ObtenerVendedores()
        {
            // SQL para seleccionar todos los vendedores
            string query = "SELECT * FROM registro_vendedor";

            DataTable dt = new DataTable();

            try
            {
                using (var connection = db.GetConnection())
                {
                    connection.Open();
                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        using (var adapter = new MySqlDataAdapter(cmd))
                        {
                            // Llenar el DataTable con los resultados de la consulta
                            adapter.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener los datos de los vendedores: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return dt;
        }

        public bool ActualizarVendedor(int id, string nombre, string apellidopat, string apellidomat, string numero, string correo)
        {
            // SQL para actualizar los datos del vendedor
            string query = @"
                UPDATE registro_vendedor
                SET nombre = @nombre, apellidopat = @apellidopat, apellidomat = @apellidomat, numero = @numero, correo = @correo
                WHERE id = @id;
            ";

            try
            {
                // Abrir la conexión a la base de datos
                using (var connection = db.GetConnection())
                {
                    connection.Open();
                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        // Asignar los parámetros de la consulta
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@nombre", nombre);
                        cmd.Parameters.AddWithValue("@apellidopat", apellidopat);
                        cmd.Parameters.AddWithValue("@apellidomat", apellidomat);
                        cmd.Parameters.AddWithValue("@numero", numero);
                        cmd.Parameters.AddWithValue("@correo", correo);

                        // Ejecutar la consulta y retornar si se actualizaron filas
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar el vendedor: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public bool EliminarVendedor(int id)
        {
            // SQL para eliminar un vendedor
            string query = "DELETE FROM registro_vendedor WHERE id = @id;";

            try
            {
                // Abrir la conexión a la base de datos
                using (var connection = db.GetConnection())
                {
                    connection.Open();
                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        // Asignar el parámetro de la consulta
                        cmd.Parameters.AddWithValue("@id", id);

                        // Ejecutar la consulta y retornar si se eliminaron filas
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar el vendedor: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public void ActualizarVentaC(string id, string comprobantePago, string ordenCompra, string factura)
        {
            // SQL para actualizar los datos
            string query = @"
            UPDATE ventas_concretadas 
            SET comprobante_pago = @comprobantePago, 
                orden_compra = @ordenCompra, 
                factura = @factura 
            WHERE id = @id";

            try
            {
                // Abrir conexión a la base de datos
                using (var connection = db.GetConnection())
                {
                    connection.Open();
                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        // Asignar los parámetros de la consulta
                        cmd.Parameters.AddWithValue("@comprobantePago", comprobantePago);
                        cmd.Parameters.AddWithValue("@ordenCompra", ordenCompra);
                        cmd.Parameters.AddWithValue("@factura", factura);
                        cmd.Parameters.AddWithValue("@id", id);

                        // Ejecutar la consulta
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Datos actualizados correctamente.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar los datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void ActualizarFactura(string id, string factura)
        {
            // SQL para actualizar los datos
            string query = @"
            UPDATE historial_compra 
            SET factura = @factura 
            WHERE id = @id";

            try
            {
                // Abrir conexión a la base de datos
                using (var connection = db.GetConnection())
                {
                    connection.Open();
                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        // Asignar los parámetros de la consulta
                        cmd.Parameters.AddWithValue("@factura", factura);
                        cmd.Parameters.AddWithValue("@id", id);

                        // Ejecutar la consulta
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Datos de facturación actualizados correctamente.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar los datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
