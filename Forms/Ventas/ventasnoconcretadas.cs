using appInventario.DAOS;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace appInventario.Form_Ventas
{
    public partial class ventasnoconcretadas : Form
    {
        private daosventas Daos;
        private DataTable ventasncOriginal;

        public ventasnoconcretadas()
        {
            Daos = new daosventas();
            InitializeComponent();

            // Evitar que el usuario pueda agregar filas
            dataGridView1.AllowUserToAddRows = false;
            // Evitar que el usuario pueda eliminar filas
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            this.FormClosing += ventasnoconcretadas_FormClosing;
        }

        private void ventasnoconcretadas_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;

            // Opcional: Puedes ocultar el formulario en lugar de cerrarlo si prefieres reutilizarlo
            this.Hide();
        }

        private void CargarVentas()
        {
            try
            {
                // Obtener los datos de ventas no concretadas
                DataTable cotizaciones = Daos.ObtenerVentasNoConcretadas();

                // Verificar que se haya obtenido el DataTable correctamente
                if (cotizaciones == null)
                {
                    MessageBox.Show("No se pudieron obtener los datos de ventas.");
                    return;
                }

                // Agregar columna para el nombre de la empresa si no existe
                if (!cotizaciones.Columns.Contains("Nombre Empresa"))
                {
                    cotizaciones.Columns.Add("Nombre Empresa", typeof(string));
                }

                // Llenar la nueva columna con los nombres de las empresas
                foreach (DataRow row in cotizaciones.Rows)
                {
                    int idHistorial = Convert.ToInt32(row["id_historial"]);
                    string nombreEmpresa = Daos.ObtenerNombreEmpresaPorHistorial(idHistorial);
                    row["Nombre Empresa"] = nombreEmpresa ?? "Desconocido"; // Manejar posibles valores nulos
                }

                // Guardar una copia del DataTable original
                ventasncOriginal = cotizaciones.Copy();

                // Configurar el DataGridView
                dataGridView1.DataSource = cotizaciones;

                // Establecer los encabezados de las columnas del DataGridView
                dataGridView1.Columns["id"].HeaderText = "ID";
                dataGridView1.Columns["id_historial"].HeaderText = "ID de Historial";
                dataGridView1.Columns["id_cotizacion"].HeaderText = "ID de Cotizacion";
                dataGridView1.Columns["estatus_factura"].HeaderText = "Factura";
                dataGridView1.Columns["estatus_pago"].HeaderText = "Estatus del Pago";
                dataGridView1.Columns["estatus_envio"].HeaderText = "Estatus del Envío";
                dataGridView1.Columns["forma_envio"].HeaderText = "Forma de Envío";
                dataGridView1.Columns["razon"].HeaderText = "Razon";
                dataGridView1.Columns["rfc"].HeaderText = "RFC";
                dataGridView1.Columns["codigo_postal"].HeaderText = "Codigo Postal";
                dataGridView1.Columns["regimen"].HeaderText = "Regimen Fiscal";
                dataGridView1.Columns["cfdi"].HeaderText = "CFDI";
                dataGridView1.Columns["metodo_pago"].HeaderText = "Método de Pago";
                dataGridView1.Columns["forma_pago"].HeaderText = "Forma de Pago";

                // Ajustar el orden de las columnas para que "Nombre Empresa" aparezca después de "id_cotizacion"
                dataGridView1.Columns["id_cotizacion"].DisplayIndex = 1;
                dataGridView1.Columns["Nombre Empresa"].DisplayIndex = 2;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar las ventas: " + ex.Message);
            }
        }

        private void ventasnoconcretadas_Load(object sender, EventArgs e)
        {

        }

        private void ventasnoconcretadas_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                CargarVentas();
                dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
                dataGridView1.DefaultCellStyle.Font = new Font("Arial", 9);
            }
        }

        private void btneditar_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Obtener la fila seleccionada
                DataGridViewRow filaSeleccionada = dataGridView1.SelectedRows[0];

                // Crear instancia de la ventana hija
                edicionventa ventanaEdicion = new edicionventa();

                // Pasar datos a la ventana hija
                string id = filaSeleccionada.Cells["id_cotizacion"].Value.ToString();
                string formaEnvio = filaSeleccionada.Cells["forma_envio"].Value.ToString();
                string estatusFactura = filaSeleccionada.Cells["estatus_factura"].Value.ToString();
                string estatusPago = filaSeleccionada.Cells["estatus_pago"].Value.ToString();
                string estatusEnvio = filaSeleccionada.Cells["estatus_envio"].Value.ToString();

                ventanaEdicion.SetTextBoxValues(id, formaEnvio, estatusFactura, estatusPago, estatusEnvio);

                // Mostrar la ventana hija
                if (ventanaEdicion.ShowDialog() == DialogResult.OK)
                {
                    // Actualizar la fila en el DataGridView
                    string idhistorial = filaSeleccionada.Cells["id_historial"].Value.ToString();
                    filaSeleccionada.Cells["estatus_factura"].Value = ventanaEdicion.descargafactura;
                    filaSeleccionada.Cells["estatus_pago"].Value = ventanaEdicion.descargapago;
                    filaSeleccionada.Cells["estatus_envio"].Value = ventanaEdicion.descargaenvio;
                    filaSeleccionada.Cells["forma_envio"].Value = ventanaEdicion.comboBoxFormaEnvio.Text;

                    // Actualizar en la base de datos
                    Daos.updateVentaNoConcretada(filaSeleccionada.Cells["id"].Value.ToString(),
                                                 idhistorial,
                                                 ventanaEdicion.descargafactura,
                                                 ventanaEdicion.descargapago,
                                                 ventanaEdicion.descargaenvio,
                                                 ventanaEdicion.comboBoxFormaEnvio.Text
                                                 );
                }
                CargarVentas();
            }
            else
            {
                MessageBox.Show("Por favor, selecciona una fila para editar.");
            }
        }

        private void txtbusqueda_TextChanged(object sender, EventArgs e)
        {
            string filtro = txtbusqueda.Text.Trim();

            if (string.IsNullOrEmpty(filtro))
            {
                // Restaurar la tabla completa si el TextBox está vacío
                dataGridView1.DataSource = ventasncOriginal;
            }
            else
            {
                try
                {
                    // Verificar si el filtro contiene caracteres especiales potencialmente problemáticos
                    if (!System.Text.RegularExpressions.Regex.IsMatch(filtro, @"^[a-zA-Z0-9\s]*$"))
                    {
                        MessageBox.Show("La búsqueda contiene caracteres no permitidos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Filtrar la tabla basada en el texto ingresado
                    DataView vistaFiltrada = new DataView(ventasncOriginal);
                    vistaFiltrada.RowFilter = $"[Nombre Empresa] LIKE '%{filtro}%' OR " +
                                              $"estatus_factura LIKE '%{filtro}%' OR " +
                                              $"estatus_pago LIKE '%{filtro}%' OR " +
                                              $"estatus_envio LIKE '%{filtro}%' OR " +
                                              $"forma_envio LIKE '%{filtro}%'";
                    dataGridView1.DataSource = vistaFiltrada;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ocurrió un error al filtrar los datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    dataGridView1.DataSource = ventasncOriginal; // Restaurar la tabla original en caso de error
                }
            }
        }

        private void btnventar_Click(object sender, EventArgs e)
        {
            try
            {
                // Verificar si hay una fila seleccionada en el DataGridView
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    // Obtener la fila seleccionada
                    DataGridViewRow filaSeleccionada = dataGridView1.SelectedRows[0];

                    // Obtener los valores necesarios de la fila seleccionada
                    int id = Convert.ToInt32(filaSeleccionada.Cells["id"].Value);
                    int idHistorial = Convert.ToInt32(filaSeleccionada.Cells["id_historial"].Value);
                    int idCotizacion = Convert.ToInt32(filaSeleccionada.Cells["id_cotizacion"].Value);
                    string estatusFactura = filaSeleccionada.Cells["estatus_factura"].Value.ToString();

                    // Actualizar el estatus de la factura
                    Daos.actualizarFacturaVentasNC(estatusFactura, idHistorial);

                    // Insertar el id_historial en la tabla ventas_concretadas
                    Daos.insertarVentaConcretada(idHistorial, idCotizacion, estatusFactura);

                    // Eliminar la fila de la tabla y del DataGridView
                    bool eliminado = Daos.ventaConcretada(id, filaSeleccionada);
                    if (eliminado == true)
                    {
                        // Eliminar la fila del DataGridView
                        dataGridView1.Rows.Remove(filaSeleccionada);
                    }
                }
                else
                {
                    MessageBox.Show("Por favor, seleccione una fila para realizar la venta.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al realizar la venta: " + ex.Message);
            }
        }
    }
}
