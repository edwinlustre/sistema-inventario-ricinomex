using appInventario.DB;
using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace appInventario.DAOS
{
    public class daosadministrador
    {
        private conexiondb db;

        public daosadministrador()
        {
            //Credenciales para iniciar sesión
            db = new conexiondb("IP", "usuario", "base", "password");
        }

        public void ActualizarOInsertarPrecio(string tipo, string precio)
        {
            try
            {
                using (var connection = db.GetConnection())
                {
                    connection.Open(); // Abrir la conexión

                    // Verificar si el tipo ya existe en la tabla
                    string selectQuery = "SELECT COUNT(*) FROM registro_precios WHERE tipo = @tipo";
                    using (var command = new MySqlCommand(selectQuery, connection))
                    {
                        command.Parameters.AddWithValue("@tipo", tipo);
                        int count = Convert.ToInt32(command.ExecuteScalar());

                        // Si existe, actualiza el precio; si no, inserta una nueva fila
                        if (count > 0)
                        {
                            string updateQuery = "UPDATE registro_precios SET precio = @precio WHERE tipo = @tipo";
                            using (var updateCommand = new MySqlCommand(updateQuery, connection))
                            {
                                updateCommand.Parameters.AddWithValue("@precio", precio);
                                updateCommand.Parameters.AddWithValue("@tipo", tipo);
                                updateCommand.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            string insertQuery = "INSERT INTO registro_precios (tipo, precio) VALUES (@tipo, @precio)";
                            using (var insertCommand = new MySqlCommand(insertQuery, connection))
                            {
                                insertCommand.Parameters.AddWithValue("@precio", precio);
                                insertCommand.Parameters.AddWithValue("@tipo", tipo);
                                insertCommand.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar o insertar el precio: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public string ObtenerPrecioGrano()
        {
            return ObtenerPrecioPorTipo("Grano");
        }

        public string ObtenerPrecioSemilla()
        {
            return ObtenerPrecioPorTipo("Semilla");
        }

        public string ObtenerPrecioCostal()
        {
            return ObtenerPrecioPorTipo("Costal");
        }

        private string ObtenerPrecioPorTipo(string tipo)
        {
            try
            {
                using (var connection = db.GetConnection())
                {
                    connection.Open(); // Abrir la conexión

                    string query = "SELECT precio FROM registro_precios WHERE tipo = @tipo";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@tipo", tipo);
                        var result = command.ExecuteScalar();
                        return result?.ToString() ?? string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener el precio: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return string.Empty;
            }
        }

        public void VerificarOCrearRegistros()
        {
            try
            {
                string[] items = { "Grano", "Semilla", "Costal" };

                using (var connection = db.GetConnection())
                {
                    connection.Open(); // Abrir la conexión

                    foreach (string item in items)
                    {
                        // Verificar si el tipo ya existe en la tabla
                        string queryCheck = "SELECT COUNT(*) FROM registro_precios WHERE tipo = @tipo";
                        using (var commandCheck = new MySqlCommand(queryCheck, connection))
                        {
                            commandCheck.Parameters.AddWithValue("@tipo", item);
                            int count = Convert.ToInt32(commandCheck.ExecuteScalar());

                            if (count == 0)
                            {
                                // Si no existe, insertar el registro con precio 0
                                string queryInsert = "INSERT INTO registro_precios (tipo, precio) VALUES (@tipo, 0)";
                                using (var commandInsert = new MySqlCommand(queryInsert, connection))
                                {
                                    commandInsert.Parameters.AddWithValue("@tipo", item);
                                    commandInsert.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al verificar o crear los registros: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
