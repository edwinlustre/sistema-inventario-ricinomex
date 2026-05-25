using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace appInventario.Form_Ventas
{
    public partial class resultadosbusqueda : Form
    {
        public int? SelectedId { get; private set; }
        public string SelectedEmpresa { get; private set; }
        public string SelectedNombre { get; private set; }
        public string SelectedApellidoPat { get; private set; }
        public string SelectedApellidoMat { get; private set; }
        public string SelectedTelefono { get; private set; }
        public string SelectedCorreo { get; private set; }


        public resultadosbusqueda()
        {
            InitializeComponent();

            // Elimina esta línea o configúrala a false para permitir la edición
            dataGridView1.ReadOnly = true;

            // Evitar que el usuario pueda agregar filas
            dataGridView1.AllowUserToAddRows = false;

            // Evitar que el usuario pueda eliminar filas
            dataGridView1.AllowUserToDeleteRows = false;

            // Evitar que el usuario pueda reordenar columnas
            dataGridView1.AllowUserToOrderColumns = false;

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void resultadosbusqueda_Load(object sender, EventArgs e)
        {
            dataGridView1.ReadOnly = true; // Deshabilitar la edición
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.CellDoubleClick += DataGridView1_CellDoubleClick; // Manejar la selección de fila
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
            dataGridView1.DefaultCellStyle.Font = new Font("Arial", 9);
        }

        public void CargarEmpresas(DataTable dataTable)
        {
            dataGridView1.DataSource = dataTable;

            // Asignar los nombres personalizados a las columnas
            if (dataGridView1.Columns["id"] != null)
            {
                dataGridView1.Columns["id"].HeaderText = "ID";
            }

            if (dataGridView1.Columns["empresa"] != null)
            {
                dataGridView1.Columns["empresa"].HeaderText = "Empresa";
            }

            if (dataGridView1.Columns["nombre"] != null)
            {
                dataGridView1.Columns["nombre"].HeaderText = "Nombre";
            }

            if (dataGridView1.Columns["apellidopat"] != null)
            {
                dataGridView1.Columns["apellidopat"].HeaderText = "Apellido Paterno";
            }

            if (dataGridView1.Columns["apellidomat"] != null)
            {
                dataGridView1.Columns["apellidomat"].HeaderText = "Apellido Materno";
            }

            if (dataGridView1.Columns["telefono"] != null)
            {
                dataGridView1.Columns["telefono"].HeaderText = "Teléfono";
            }

            if (dataGridView1.Columns["correo"] != null)
            {
                dataGridView1.Columns["correo"].HeaderText = "Correo";
            }

            if (dataGridView1.Columns["razon_social"] != null)
            {
                dataGridView1.Columns["razon_social"].HeaderText = "Razón Social";
            }

            if (dataGridView1.Columns["rfc"] != null)
            {
                dataGridView1.Columns["rfc"].HeaderText = "RFC";
            }

            if (dataGridView1.Columns["codigo_postal"] != null)
            {
                dataGridView1.Columns["codigo_postal"].HeaderText = "Código Postal";
            }

            if (dataGridView1.Columns["regimen_fiscal"] != null)
            {
                dataGridView1.Columns["regimen_fiscal"].HeaderText = "Régimen Fiscal";
            }

            if (dataGridView1.Columns["cfdi"] != null)
            {
                dataGridView1.Columns["cfdi"].HeaderText = "CFDI";
            }

            if (dataGridView1.Columns["metodo_pago"] != null)
            {
                dataGridView1.Columns["metodo_pago"].HeaderText = "Método de Pago";
            }

            if (dataGridView1.Columns["forma_pago"] != null)
            {
                dataGridView1.Columns["forma_pago"].HeaderText = "Forma de Pago";
            }

            if (dataGridView1.Columns["domicilio_entrega"] != null)
            {
                dataGridView1.Columns["domicilio_entrega"].HeaderText = "Domicilio de Entrega";
            }
        }

        private void DataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Asumiendo que la primera columna es 'id' y la segunda es 'empresa'
                SelectedId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["id"].Value);
                SelectedEmpresa = dataGridView1.Rows[e.RowIndex].Cells["empresa"].Value.ToString();
                SelectedNombre = dataGridView1.Rows[e.RowIndex].Cells["nombre"].Value.ToString();
                SelectedApellidoPat = dataGridView1.Rows[e.RowIndex].Cells["apellidopat"].Value.ToString();
                SelectedApellidoMat = dataGridView1.Rows[e.RowIndex].Cells["apellidomat"].Value.ToString();
                SelectedTelefono = dataGridView1.Rows[e.RowIndex].Cells["telefono"].Value.ToString();
                SelectedCorreo = dataGridView1.Rows[e.RowIndex].Cells["correo"].Value.ToString();
                this.DialogResult = DialogResult.OK; // Cierra el formulario con éxito
                this.Close();
            }
        }
    }
}
