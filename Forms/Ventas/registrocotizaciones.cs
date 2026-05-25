using appInventario.DAOS;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace appInventario.Form_Ventas
{
    public partial class registrocotizaciones : Form
    {
        private daosventas Daos;
        private editarcotizacion edicion;

        public registrocotizaciones()
        {
            InitializeComponent();
            Daos = new daosventas();
            edicion = new editarcotizacion();

            // Llenar el DataGridView con las cotizaciones
            CargarCotizaciones();

            // Evitar que el usuario pueda agregar filas
            dataGridView1.AllowUserToAddRows = false;
            // Evitar que el usuario pueda eliminar filas
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            this.FormClosing += registrocotizaciones_FormClosing;
        }

        private void registrocotizaciones_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            // Limpiar el DataGridView cuando se cierra el formulario
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();

            // Opcional: Puedes ocultar el formulario en lugar de cerrarlo si prefieres reutilizarlo
            this.Hide();
        }

        private void CargarCotizaciones()
        {
            DataTable cotizaciones = Daos.ObtenerCotizaciones();
            dataGridView1.DataSource = cotizaciones;

            // Establecer los encabezados de las columnas del DataGridView
            dataGridView1.Columns["id"].HeaderText = "ID";
            dataGridView1.Columns["id_empresa"].HeaderText = "ID Cliente";
            dataGridView1.Columns["empresa"].HeaderText = "Empresa / Cliente";
            dataGridView1.Columns["fecha_compra"].HeaderText = "Fecha de Compra";
            dataGridView1.Columns["aceite_garrafa"].HeaderText = "Tipo en Garrafa";
            dataGridView1.Columns["aceite_tote"].HeaderText = "Tipo en Tote";
            dataGridView1.Columns["aceite_tambor"].HeaderText = "Tipo en Tambor";
            dataGridView1.Columns["garrafas"].HeaderText = "Presentacion de Garrafas";
            dataGridView1.Columns["num_garrafas"].HeaderText = "Número de Garrafas";
            dataGridView1.Columns["totes"].HeaderText = "Presentacion de Totes";
            dataGridView1.Columns["num_totes"].HeaderText = "Número de Totes";
            dataGridView1.Columns["tambores"].HeaderText = "Presentacion de Tambores";
            dataGridView1.Columns["num_tambores"].HeaderText = "Número de Tambores";
            dataGridView1.Columns["envio"].HeaderText = "Envio";
            dataGridView1.Columns["subtotal"].HeaderText = "Subtotal";
            dataGridView1.Columns["iva"].HeaderText = "IVA";
            dataGridView1.Columns["total"].HeaderText = "Total";

        }

        private void registrocotizaciones_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                // Llenar el DataGridView con las cotizaciones
                CargarCotizaciones();
                dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
                dataGridView1.DefaultCellStyle.Font = new Font("Arial", 9);
            }
        }

        private void registrocotizaciones_Load(object sender, EventArgs e)
        {

        }

        private void btnactualizar_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int idCotizacion = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["id"].Value);
                edicion.MostrarCotizaciones(idCotizacion);
                edicion.ObtenerDatosCliente(idCotizacion);
                edicion.Show();
            }
        }

        private void btnventa_Click(object sender, EventArgs e)
        {
            // Verificar si hay una fila seleccionada
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Obtener los valores de la fila seleccionada
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                int idCotizacion = Convert.ToInt32(selectedRow.Cells["id"].Value);
                int idEmpresa = Convert.ToInt32(selectedRow.Cells["id_empresa"].Value);
                string fechaCompra = selectedRow.Cells["fecha_compra"].Value.ToString();
                string aceiteGarrafa = selectedRow.Cells["aceite_garrafa"].Value.ToString();
                string garrafas = selectedRow.Cells["garrafas"].Value.ToString();
                string numGarrafas = selectedRow.Cells["num_garrafas"].Value.ToString();
                string aceiteTote = selectedRow.Cells["aceite_tote"].Value.ToString();
                string totes = selectedRow.Cells["totes"].Value.ToString();
                string numTotes = selectedRow.Cells["num_totes"].Value.ToString();
                string aceiteTambor = selectedRow.Cells["aceite_tambor"].Value.ToString();
                string tambores = selectedRow.Cells["tambores"].Value.ToString();
                string numTambores = selectedRow.Cells["num_tambores"].Value.ToString();

                // Mostrar la ventana de facturación y esperar a que el usuario ingrese los datos
                agregarfacturación facturacion = new agregarfacturación(idEmpresa);
                if (facturacion.ShowDialog() == DialogResult.OK)
                {
                    // Recuperar los datos ingresados en la ventana de facturación
                    string paqueteria = facturacion.Paqueteria;
                    string razon = facturacion.Razon;
                    string rfc = facturacion.RFC;
                    string codigopostal = facturacion.CodigoPostal;
                    string regimen = facturacion.Regimen;
                    string cfdi = facturacion.CFDI;
                    string metodo = facturacion.MetodoPago;
                    string forma = facturacion.FormaPago;

                    // Llamar al método del DAO para realizar la operación
                    bool resultado = Daos.InsertarHistorialCompraYVentaNoConcretada(idCotizacion, idEmpresa, fechaCompra, aceiteGarrafa, garrafas, numGarrafas, aceiteTote, totes, numTotes, aceiteTambor, tambores, numTambores, paqueteria, razon, rfc, codigopostal, regimen, cfdi, metodo, forma);

                    if (resultado)
                    {
                        MessageBox.Show("Venta registrada correctamente.", "Mensaje");
                    }
                    else
                    {
                        MessageBox.Show("Hubo un problema al realizar la venta.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione una cotización.");
            }
        }

        private void txtbusqueda_TextChanged(object sender, EventArgs e)
        {
            // Obtener el texto ingresado en el TextBox
            string filterText = txtbusqueda.Text.Trim();

            // Verificar si el TextBox está vacío
            if (string.IsNullOrEmpty(filterText))
            {
                // Si el TextBox está vacío, recargar toda la tabla
                CargarCotizaciones();
            }
            else
            {
                // Filtrar los datos del DataGridView utilizando el texto ingresado
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = string.Format(
                    "Convert(id, 'System.String') LIKE '%{0}%' OR " +
                    "empresa LIKE '%{0}%' OR " +
                    "Convert(id_empresa, 'System.String') LIKE '%{0}%' OR " +
                    "fecha_compra LIKE '%{0}%' OR " +
                    "aceite_garrafa LIKE '%{0}%' OR " +
                    "aceite_tote LIKE '%{0}%' OR " +
                    "aceite_tambor LIKE '%{0}%' OR " +
                    "Convert(garrafas, 'System.String') LIKE '%{0}%' OR " +
                    "Convert(num_garrafas, 'System.String') LIKE '%{0}%' OR " +
                    "totes LIKE '%{0}%' OR " +
                    "Convert(num_totes, 'System.String') LIKE '%{0}%' OR " +
                    "tambores LIKE '%{0}%' OR " +
                    "Convert(num_tambores, 'System.String') LIKE '%{0}%' OR " +
                    "Convert(subtotal, 'System.String') LIKE '%{0}%' OR " +
                    "Convert(iva, 'System.String') LIKE '%{0}%' OR " +
                    "Convert(total, 'System.String') LIKE '%{0}%'",
                    filterText);
            }
        }
    }
}
