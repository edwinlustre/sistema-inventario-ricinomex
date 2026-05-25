using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace appInventario.Forms
{
    public partial class resultadosbusqueda : Form
    {
        private Compras form;
        private DataGridViewRow selectedRow; // Fila seleccionada
        private ProcessDataAction dataProcessor; // Acción para procesar los datos

        public resultadosbusqueda(Compras form)
        {
            InitializeComponent();
            this.form = form;
            this.FormClosing += resultadosbusqueda_FormClosing;

            // Establecer el DataGridView como de solo lectura
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
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold); // Cambia "Arial" por el tipo de letra que prefieras, "12" por el tamaño, y "Bold" para negrita.
            dataGridView1.DefaultCellStyle.Font = new Font("Arial", 9); // Cambia "Arial" por el tipo de letra que prefieras, y "12" por el tamaño.
        }

        public void LoadResults(DataTable resultsTable)
        {
            dataGridView1.DataSource = resultsTable;
            // Renombrar los encabezados de las columnas
            dataGridView1.Columns["nombre"].HeaderText = "Nombre";
            dataGridView1.Columns["apellidopat"].HeaderText = "Apellido Paterno";
            dataGridView1.Columns["apellidomat"].HeaderText = "Apellido Materno";
            dataGridView1.Columns["curp"].HeaderText = "CURP";
            dataGridView1.Columns["telefono"].HeaderText = "Teléfono";
            dataGridView1.Columns["municipio"].HeaderText = "Municipio";
            dataGridView1.Columns["localidad"].HeaderText = "Localidad";
            dataGridView1.Columns["tipo"].HeaderText = "Tipo";
            dataGridView1.Columns["hectareas"].HeaderText = "Hectáreas";
            dataGridView1.Columns["fecha_registro"].HeaderText = "Fecha de registro";
            dataGridView1.Columns["ID"].HeaderText = "ID";
        }

        private void resultadosbusqueda_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            // Limpiar el DataGridView cuando se cierra el formulario
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();

            // Opcional: Puedes ocultar el formulario en lugar de cerrarlo si prefieres reutilizarlo
            this.Hide();
            form?.Show(); // Asegúrate de que el formulario principal se muestre nuevamente
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Asegúrate de que la selección no sea el encabezado de columna
            if (e.RowIndex >= 0)
            {
                // Almacenar la fila seleccionada
                selectedRow = dataGridView1.Rows[e.RowIndex];

                if (dataProcessor != null)
                {
                    // Extraer los valores de la fila almacenada
                    string nombre = selectedRow.Cells["nombre"].Value.ToString();
                    string apellidoPaterno = selectedRow.Cells["apellidopat"].Value.ToString();
                    string apellidoMaterno = selectedRow.Cells["apellidomat"].Value.ToString();
                    string telefono = selectedRow.Cells["telefono"].Value.ToString();
                    string localidad = selectedRow.Cells["localidad"].Value.ToString();
                    string hectareas = selectedRow.Cells["hectareas"].Value.ToString();
                    int id = Convert.ToInt32(selectedRow.Cells["ID"].Value);

                    // Ejecutar la acción pasada como parámetro
                    dataProcessor(nombre, apellidoPaterno, apellidoMaterno, localidad, telefono, hectareas, id);

                    // Cerrar el formulario de resultados
                    this.Hide();
                    form.Show(); // Asegúrate de que el formulario principal se muestre nuevamente
                }
                else
                {
                    MessageBox.Show("No se ha definido ninguna acción para procesar los datos.", "Error");
                }
            }
        }

        public delegate void ProcessDataAction(string nombre, string apellidopat, string apellidomat, string telefono, string localidad, string hectareas, int id);

        public void SetDataProcessor(ProcessDataAction action)
        {
            dataProcessor = action;
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }


    }
}

