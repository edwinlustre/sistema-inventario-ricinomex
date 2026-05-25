using appInventario.DAOS;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace appInventario.Form_Ventas
{
    public partial class registrarvendedor : Form
    {
        private daosventas Daos;

        public registrarvendedor()
        {
            Daos = new daosventas();
            InitializeComponent();

            // Opcional: Ajustar el ancho de las columnas automáticamente
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            this.FormClosing += registrarvendedor_FormClosing;
        }

        private void registrarvendedor_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        public void MostrarVendedores()
        {
            // Obtener los datos de los vendedores
            DataTable dtVendedores = Daos.ObtenerVendedores();

            // Asignar los datos al DataGridView
            dataGridView1.DataSource = dtVendedores;

            // Establecer los encabezados de las columnas del DataGridView
            dataGridView1.Columns["id"].HeaderText = "ID";
            dataGridView1.Columns["nombre"].HeaderText = "Nombre";
            dataGridView1.Columns["apellidopat"].HeaderText = "Apellido Paterno";
            dataGridView1.Columns["apellidomat"].HeaderText = "Apellido Materno";
            dataGridView1.Columns["numero"].HeaderText = "Número de Telefono";
            dataGridView1.Columns["correo"].HeaderText = "Correo";
        }

        private void registrarvendedor_Load(object sender, EventArgs e)
        {
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold); // Cambia "Arial" por el tipo de letra que prefieras, "12" por el tamaño, y "Bold" para negrita.
            dataGridView1.DefaultCellStyle.Font = new Font("Arial", 9); // Cambia "Arial" por el tipo de letra que prefieras, y "12" por el tamaño.
        }

        private void registrarvendedor_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                MostrarVendedores();
            }
        }

        private void btnactualizar_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Obtener la fila seleccionada
                DataGridViewRow filaSeleccionada = dataGridView1.SelectedRows[0];

                // Obtener los valores actualizados de la fila
                int id = Convert.ToInt32(filaSeleccionada.Cells["id"].Value);
                string nombre = filaSeleccionada.Cells["nombre"].Value?.ToString();
                string apellidopat = filaSeleccionada.Cells["apellidopat"].Value?.ToString();
                string apellidomat = filaSeleccionada.Cells["apellidomat"].Value?.ToString();
                string numero = filaSeleccionada.Cells["numero"].Value?.ToString();
                string correo = filaSeleccionada.Cells["correo"].Value?.ToString();

                // Actualizar la base de datos
                bool actualizado = Daos.ActualizarVendedor(id, nombre, apellidopat, apellidomat, numero, correo);

                if (actualizado)
                {
                    // Actualizar el DataGridView
                    MostrarVendedores();
                    MessageBox.Show("Datos actualizados correctamente.");
                }
                else
                {
                    MessageBox.Show("Error al actualizar los datos.");
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione una fila para actualizar.");
            }
        }

        private void btneliminar_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Confirmar la eliminación
                DialogResult confirmResult = MessageBox.Show("¿Está seguro que desea eliminar este registro?",
                                                             "Confirmar Eliminación",
                                                             MessageBoxButtons.YesNo);

                if (confirmResult == DialogResult.Yes)
                {
                    // Obtener la fila seleccionada
                    DataGridViewRow filaSeleccionada = dataGridView1.SelectedRows[0];

                    // Obtener el ID del registro
                    int id = Convert.ToInt32(filaSeleccionada.Cells["id"].Value);

                    // Eliminar el registro de la base de datos
                    bool eliminado = Daos.EliminarVendedor(id);

                    if (eliminado)
                    {
                        // Actualizar el DataGridView
                        MostrarVendedores();
                        MessageBox.Show("Registro eliminado correctamente.");
                    }
                    else
                    {
                        MessageBox.Show("Error al eliminar el registro.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione una fila para eliminar.");
            }
        }
    }
}
