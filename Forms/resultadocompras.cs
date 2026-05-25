using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace appInventario.Forms
{
    public partial class resultadocompras : Form
    {
        private Compras form;
        private BindingSource bindingSource;
        private DataTable originalTable;

        public resultadocompras(Compras form)
        {
            InitializeComponent();
            this.form = form;
            this.FormClosing += resultadocompras_FormClosing;

            // Instanciar el BindingSource
            bindingSource = new BindingSource();

            // Establecer el DataGridView como de solo lectura
            dataGridView1.ReadOnly = true;

            // Evitar que el usuario pueda agregar filas
            dataGridView1.AllowUserToAddRows = false;

            // Evitar que el usuario pueda eliminar filas
            dataGridView1.AllowUserToDeleteRows = false;

            // Evitar que el usuario pueda reordenar columnas
            dataGridView1.AllowUserToOrderColumns = false;

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Agregar evento al TextBox de búsqueda
            txtbusqueda.TextChanged += txtbusqueda_TextChanged;
            txtbusqueda.KeyPress += txtbusqueda_KeyPress; // Para detectar Enter

        }

        public void LoadResults(DataTable resultsTable)
        {
            // Asignar la tabla de resultados al BindingSource
            bindingSource.DataSource = resultsTable;

            // Guardar una copia de la tabla original
            originalTable = resultsTable.Copy();

            // Enlazar el BindingSource al DataGridView
            dataGridView1.DataSource = bindingSource;

            dataGridView1.DataSource = resultsTable;

            // Renombrar los encabezados de las columnas
            /*dataGridView1.Columns["id"].HeaderText = "ID";
            dataGridView1.Columns["id_productor"].HeaderText = "ID Productor";
            dataGridView1.Columns["nombre"].HeaderText = "Nombre";
            dataGridView1.Columns["apellidopat"].HeaderText = "Apellido Paterno";
            dataGridView1.Columns["apellidomat"].HeaderText = "Apellido Materno";
            dataGridView1.Columns["fecha_compra"].HeaderText = "Fecha de Compra";
            dataGridView1.Columns["hectareas"].HeaderText = "Hectáreas";
            dataGridView1.Columns["telefono"].HeaderText = "Teléfono";
            dataGridView1.Columns["localidad"].HeaderText = "Localidad";
            dataGridView1.Columns["grano"].HeaderText = "Grano";
            dataGridView1.Columns["kg"].HeaderText = "Kg";
            dataGridView1.Columns["precio"].HeaderText = "Precio";
            dataGridView1.Columns["importe"].HeaderText = "Importe";*/

        }

        private void resultadocompras_Load(object sender, EventArgs e)
        {
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
            dataGridView1.DefaultCellStyle.Font = new Font("Arial", 9);
        }

        private void txtbusqueda_TextChanged(object sender, EventArgs e)
        {
            string filterText = txtbusqueda.Text.Trim();

            if (!string.IsNullOrEmpty(filterText))
            {
                // Dividir la cadena de búsqueda en palabras
                string[] searchTerms = filterText.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                // Construir el filtro dinámico
                string filter = string.Join(" AND ", searchTerms.Select(term =>
                    $"(nombre LIKE '%{term}%' OR apellidopat LIKE '%{term}%' OR apellidomat LIKE '%{term}%')"
                ));

                // Aplicar el filtro al BindingSource
                bindingSource.Filter = filter;
            }
            else
            {
                // Quitar el filtro si el TextBox está vacío
                bindingSource.RemoveFilter();
            }
        }


        private void txtbusqueda_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true; // Prevenir el sonido de "ding" al presionar Enter

                if (string.IsNullOrEmpty(txtbusqueda.Text.Trim()))
                {
                    // Si el TextBox está vacío, restaurar la tabla original
                    bindingSource.DataSource = originalTable;
                }
            }
        }

        private void resultadocompras_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            // Limpiar el DataGridView cuando se cierra el formulario
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();

            // Opcional: Puedes ocultar el formulario en lugar de cerrarlo si prefieres reutilizarlo
            this.Hide();
            form?.Show(); // Asegúrate de que el formulario principal se muestre nuevamente
        }

        private void btnbuscar_Click(object sender, EventArgs e)
        {

        }
    }
}
