using appInventario.DAOS;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace appInventario.Form_Ventas
{
    public partial class historialcompras : Form
    {
        private daosventas Daos;
        private DataTable comprasOriginal; // Para almacenar la tabla original

        public historialcompras()
        {
            Daos = new daosventas();
            InitializeComponent();

            // Evitar que el usuario pueda agregar filas
            dataGridView1.AllowUserToAddRows = false;
            // Evitar que el usuario pueda eliminar filas
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            this.FormClosing += historialcompras_FormClosing;
        }

        private void historialcompras_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            // Limpiar el DataGridView cuando se cierra el formulario
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();

            // Opcional: Puedes ocultar el formulario en lugar de cerrarlo si prefieres reutilizarlo
            this.Hide();
        }

        private void CargarHistorial()
        {
            DataTable cotizaciones = Daos.ObtenerHistorialCompra();

            // Agregar columna para el nombre de la empresa
            cotizaciones.Columns.Add("NombreEmpresa", typeof(string));

            // Llenar la nueva columna con los nombres de las empresas
            foreach (DataRow row in cotizaciones.Rows)
            {
                int idEmpresa = Convert.ToInt32(row["id_empresa"]);
                string nombreEmpresa = Daos.ObtenerNombreEmpresa(idEmpresa);
                row["NombreEmpresa"] = nombreEmpresa;
            }

            // Asignar el DataTable al DataGridView
            dataGridView1.DataSource = cotizaciones;

            // Establecer los encabezados de las columnas del DataGridView
            dataGridView1.Columns["id"].HeaderText = "ID";
            dataGridView1.Columns["id_empresa"].HeaderText = "ID Empresa";
            dataGridView1.Columns["NombreEmpresa"].HeaderText = "Nombre de la Empresa"; // Nueva columna
            dataGridView1.Columns["fecha_compra"].HeaderText = "Fecha de Compra";
            dataGridView1.Columns["aceite_garrafa"].HeaderText = "Tipo de Aceite en Garrafa";
            dataGridView1.Columns["garrafas"].HeaderText = "Presentación de Garrafas";
            dataGridView1.Columns["num_garrafas"].HeaderText = "Número de Garrafas";
            dataGridView1.Columns["aceite_tote"].HeaderText = "Tipo de Aceite en Tote";
            dataGridView1.Columns["totes"].HeaderText = "Presentación de Totes";
            dataGridView1.Columns["num_totes"].HeaderText = "Número de Totes";
            dataGridView1.Columns["aceite_tambor"].HeaderText = "Tipo de Aceite en Tambor";
            dataGridView1.Columns["tambores"].HeaderText = "Presentación de Tambores";
            dataGridView1.Columns["num_tambores"].HeaderText = "Número de Tambores";
            dataGridView1.Columns["factura"].HeaderText = "Estatus de la Factura";

            // Reordenar columnas
            dataGridView1.Columns["id_empresa"].DisplayIndex = 1;
            dataGridView1.Columns["NombreEmpresa"].DisplayIndex = 2;
        }

        private void historialcompras_Load(object sender, EventArgs e)
        {

        }

        private void historialcompras_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                CargarHistorial();
                comprasOriginal = (DataTable)dataGridView1.DataSource;
                dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
                dataGridView1.DefaultCellStyle.Font = new Font("Arial", 9);
            }
        }

        private void txtbusqueda_TextChanged(object sender, EventArgs e)
        {
            string filtro = txtbusqueda.Text.Trim();

            if (string.IsNullOrEmpty(filtro))
            {
                // Restaurar la tabla completa si el TextBox está vacío
                dataGridView1.DataSource = comprasOriginal;
            }
            else
            {
                try
                {
                    // Verificar si el filtro contiene caracteres especiales potencialmente problemáticos
                    if (!System.Text.RegularExpressions.Regex.IsMatch(filtro, @"^[a-zA-Z0-9\s-]*$"))
                    {
                        MessageBox.Show("La búsqueda contiene caracteres no permitidos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Filtrar la tabla basada en el texto ingresado
                    DataView vistaFiltrada = new DataView(comprasOriginal);
                    vistaFiltrada.RowFilter = $"NombreEmpresa LIKE '%{filtro}%' OR " +
                                              $"fecha_compra LIKE '%{filtro}%' OR " +
                                              $"factura LIKE '%{filtro}%'";
                    dataGridView1.DataSource = vistaFiltrada;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ocurrió un error al filtrar los datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    dataGridView1.DataSource = comprasOriginal; // Restaurar la tabla original en caso de error
                }
            }
        }
    }
}
