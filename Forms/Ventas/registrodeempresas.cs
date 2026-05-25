using appInventario.DAOS;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace appInventario.Form_Ventas
{
    public partial class registrodeempresas : Form
    {
        private daosventas Daos;
        public registrodeempresas()
        {
            InitializeComponent();
            Daos = new daosventas();
            this.FormClosing += registrodeempresas_FormClosing;

            dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter; // Permitir la edición al seleccionar la celda

            // Elimina esta línea o configúrala a false para permitir la edición
            // dataGridView1.ReadOnly = true;

            // Evitar que el usuario pueda agregar filas
            dataGridView1.AllowUserToAddRows = false;

            // Evitar que el usuario pueda eliminar filas
            dataGridView1.AllowUserToDeleteRows = false;

            // Evitar que el usuario pueda reordenar columnas
            dataGridView1.AllowUserToOrderColumns = false;

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void registrodeempresas_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void registrodeempresas_Load(object sender, EventArgs e)
        {
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold); // Cambia "Arial" por el tipo de letra que prefieras, "12" por el tamaño, y "Bold" para negrita.
            dataGridView1.DefaultCellStyle.Font = new Font("Arial", 9); // Cambia "Arial" por el tipo de letra que prefieras, y "12" por el tamaño.
        }

        // Método para cargar las empresas en el DataGridView con ventana de carga
        private void CargarEmpresas()
        {
            DataTable dataTable = Daos.mostrarEmpresas();
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

            // Ajustar columnas y estilos del DataGridView
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.DefaultCellStyle.ForeColor = Color.Black;
        }

        private void registrodeempresas_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                CargarEmpresas();
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnactualizar_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                // Obtener los valores de la fila seleccionada
                int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["id"].Value);
                string empresa = dataGridView1.CurrentRow.Cells["empresa"].Value.ToString();
                string nombre = dataGridView1.CurrentRow.Cells["nombre"].Value.ToString();
                string apellidopat = dataGridView1.CurrentRow.Cells["apellidopat"].Value.ToString();
                string apellidomat = dataGridView1.CurrentRow.Cells["apellidomat"].Value.ToString();
                string telefono = dataGridView1.CurrentRow.Cells["telefono"].Value.ToString();
                string correo = dataGridView1.CurrentRow.Cells["correo"].Value.ToString();
                string razonSocial = dataGridView1.CurrentRow.Cells["razon_social"].Value.ToString();
                string rfc = dataGridView1.CurrentRow.Cells["rfc"].Value.ToString();
                string codigoPostal = dataGridView1.CurrentRow.Cells["codigo_postal"].Value.ToString();
                string domicilioEntrega = dataGridView1.CurrentRow.Cells["domicilio_entrega"].Value.ToString();

                // Llamar al método para actualizar la empresa
                bool actualizado = Daos.actualizarEmpresa(id, empresa, nombre, apellidopat, apellidomat, telefono, correo,
                                                          razonSocial, rfc, codigoPostal, domicilioEntrega);

                if (actualizado)
                {
                    MessageBox.Show("Cliente actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarEmpresas(); // Recargar los datos
                }
                else
                {
                    MessageBox.Show("Error al actualizar el cliente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btneliminar_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["id"].Value);

                DialogResult confirmResult = MessageBox.Show("¿Estás seguro de que deseas eliminar esta empresa?",
                                                             "Confirmar eliminación",
                                                             MessageBoxButtons.YesNo,
                                                             MessageBoxIcon.Warning);

                if (confirmResult == DialogResult.Yes)
                {
                    bool eliminado = Daos.eliminarEmpresa(id);

                    if (eliminado)
                    {
                        MessageBox.Show("Cliente eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarEmpresas(); // Recargar los datos
                    }
                    else
                    {
                        MessageBox.Show("Error al eliminar la empresa.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnbuscar_Click(object sender, EventArgs e)
        {
            string terminoBusqueda = txtbusqueda.Text.Trim();

            if (string.IsNullOrEmpty(terminoBusqueda))
            {
                // Si el cuadro de búsqueda está vacío, recargar todas las empresas
                CargarEmpresas();
            }
            else
            {
                // Buscar empresas que coincidan con el término de búsqueda
                DataTable resultadoBusqueda = Daos.BuscarEmpresas(terminoBusqueda);
                dataGridView1.DataSource = resultadoBusqueda;
            }
        }

        private void txtbusqueda_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;  // Evitar el sonido de la tecla Enter
                e.SuppressKeyPress = true;  // Evitar el efecto de la tecla Enter

                // Simular el clic en el botón de búsqueda
                btnbuscar.PerformClick();
            }
        }

        private void txtbusqueda_TextChanged(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }
    }
}
