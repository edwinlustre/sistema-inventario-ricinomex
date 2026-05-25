using appInventario.DAOS;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace appInventario.Form_Ventas
{
    public partial class ventasconcretadas : Form
    {
        private daosventas Daos;
        private DataTable ventasc;

        public ventasconcretadas()
        {
            Daos = new daosventas();
            InitializeComponent();

            // Evitar que el usuario pueda agregar filas
            dataGridView1.AllowUserToAddRows = false;
            // Evitar que el usuario pueda eliminar filas
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            this.FormClosing += ventasconcretadas_FormClosing;
        }

        private void ventasconcretadas_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;

            // Opcional: Puedes ocultar el formulario en lugar de cerrarlo si prefieres reutilizarlo
            this.Hide();
        }

        private void CargarVentas()
        {
            try
            {
                // Obtener los datos de ventas concretadas
                ventasc = Daos.ObtenerVentasConcretadas();

                // Verificar que se haya obtenido el DataTable correctamente
                if (ventasc == null)
                {
                    MessageBox.Show("No se pudieron obtener los datos de ventas concretadas.");
                    return;
                }

                // Agregar columna para el nombre de la empresa si no existe
                if (!ventasc.Columns.Contains("Nombre Empresa"))
                {
                    ventasc.Columns.Add("Nombre Empresa", typeof(string));
                }

                // Llenar la nueva columna con los nombres de las empresas
                foreach (DataRow row in ventasc.Rows)
                {
                    int idHistorial = Convert.ToInt32(row["id_historial"]);
                    string nombreEmpresa = Daos.ObtenerNombreEmpresaPorHistorial(idHistorial);
                    row["Nombre Empresa"] = nombreEmpresa ?? "Desconocido"; // Manejar posibles valores nulos
                }

                // Configurar el DataGridView
                ConfigurarDataGridView(ventasc);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar las ventas concretadas: " + ex.Message);
            }
        }

        private void ConfigurarDataGridView(DataTable dataTable)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Columns.Clear();

            // Configurar el DataGridView
            dataGridView1.DataSource = dataTable;

            // Establecer los encabezados de las columnas del DataGridView
            dataGridView1.Columns["id"].HeaderText = "ID";
            dataGridView1.Columns["id_historial"].HeaderText = "ID de Historial";
            dataGridView1.Columns["id_cotizacion"].HeaderText = "ID de Cotizacion";
            dataGridView1.Columns["comprobante_pago"].HeaderText = "Comprobante de Pago";
            dataGridView1.Columns["orden_compra"].HeaderText = "Orden de Compra";
            dataGridView1.Columns["factura"].HeaderText = "Factura";

            // Reordenar columnas
            dataGridView1.Columns["id_historial"].DisplayIndex = 1;
            dataGridView1.Columns["Nombre Empresa"].DisplayIndex = 3;
        }


        private void ventasconcretadas_Load(object sender, EventArgs e)
        {
            CargarVentas();
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
            dataGridView1.DefaultCellStyle.Font = new Font("Arial", 9);
        }

        private void ventasconcretadas_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                CargarVentas();
            }
        }

        private void txtbusqueda_TextChanged(object sender, EventArgs e)
        {
            string filtro = txtbusqueda.Text.Trim();

            if (string.IsNullOrWhiteSpace(filtro))
            {
                // Si el filtro está vacío, mostrar toda la tabla
                ConfigurarDataGridView(ventasc);
            }
            else
            {
                try
                {
                    // Escapar caracteres especiales para evitar errores de sintaxis
                    filtro = filtro.Replace("'", "''"); // Manejar comillas simples

                    // Aplicar filtro al DataTable
                    string filtroQuery = $"[Nombre Empresa] LIKE '%{filtro}%' OR " +
                                         $"[comprobante_pago] LIKE '%{filtro}%' OR " +
                                         $"[orden_compra] LIKE '%{filtro}%' OR " +
                                         $"[factura] LIKE '%{filtro}%' OR " +
                                         $"CONVERT([id_historial], 'System.String') LIKE '%{filtro}%'";

                    DataView vistaFiltrada = new DataView(ventasc);
                    vistaFiltrada.RowFilter = filtroQuery;

                    // Actualizar el DataGridView con la vista filtrada
                    ConfigurarDataGridView(vistaFiltrada.ToTable());
                }
                catch (SyntaxErrorException ex)
                {
                    MessageBox.Show("Ocurrió un error al aplicar el filtro de búsqueda: " + ex.Message);
                }
            }
        }

        private void btneditar_Click(object sender, EventArgs e)
        {
            // Verificar que hay una fila seleccionada
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Obtener la fila seleccionada
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                // Extraer datos de la fila seleccionada
                string id = selectedRow.Cells["id"].Value.ToString();
                string idHistorial = selectedRow.Cells["id_historial"].Value.ToString();
                string idCotizacion = selectedRow.Cells["id_cotizacion"].Value.ToString();
                string comprobantePago = selectedRow.Cells["comprobante_pago"].Value.ToString();
                string ordenCompra = selectedRow.Cells["orden_compra"].Value.ToString();
                string factura = selectedRow.Cells["factura"].Value.ToString();

                // Crear una instancia de la ventana que contiene los TextBox
                edicionventac Edicion = new edicionventac();

                // Pasar los datos a los TextBox en la otra ventana
                Edicion.SetTextBoxValues(idCotizacion, comprobantePago, ordenCompra, factura);

                // Mostrar la otra ventana
                if (Edicion.ShowDialog() == DialogResult.OK) // ShowDialog hace que sea modal
                {
                    // Actualizar la fila en el DataGridView
                    selectedRow.Cells["comprobante_pago"].Value = Edicion.GetComprobantePago();
                    selectedRow.Cells["orden_compra"].Value = Edicion.GetOrdenCompra();
                    selectedRow.Cells["factura"].Value = Edicion.GetFactura();

                    // Actualizar la base de datos con los valores nuevos
                    Daos.ActualizarVentaC(id, Edicion.GetFactura(), Edicion.GetComprobantePago(), Edicion.GetOrdenCompra());
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona una fila antes de editar.");
            }
        }
    }
}
