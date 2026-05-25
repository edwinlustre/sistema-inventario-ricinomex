using appInventario.DAOS;
using appInventario.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace appInventario.Forms
{
    public partial class actualizar : Form
    {
        private Compras form;
        private daos Daos;
        private carga Carga;

        public actualizar(Compras form)
        {
            InitializeComponent();
            Daos = new daos();
            Carga = new carga();


            this.form = form;
            this.FormClosing += new FormClosingEventHandler(actualizar_FormClosing);
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter; // O EditOnKeystroke
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;


        }

        private void actualizar_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            form?.Show();
        }

        // Nuevo método para actualizar el productor
        private bool ActualizarProductor(DataGridViewRow row)
        {
            // Crear un objeto Productor con los datos editados
            Productor productorEditado = new Productor(
                row.Cells["nombre"].Value.ToString(),
                row.Cells["apellidopat"].Value.ToString(),
                row.Cells["apellidomat"].Value.ToString(),
                row.Cells["curp"].Value.ToString(),
                row.Cells["municipio"].Value.ToString(),
                row.Cells["localidad"].Value.ToString(),
                row.Cells["tipo"].Value.ToString(),
                row.Cells["hectareas"].Value.ToString(),
                row.Cells["fecha_registro"].Value.ToString(),  // Asegúrate de que el nombre de la columna es correcto
                row.Cells["telefono"].Value.ToString()
            );

            try
            {
                int id = int.Parse(row.Cells["id"].Value.ToString());
                Daos.actualizarProductor(productorEditado, id);
                return true; // Indica que la actualización fue exitosa
            }
            catch (Exception ex)
            {
                // Muestra un mensaje de error si hay una excepción
                MessageBox.Show("Error al actualizar el productor: " + ex.Message, "Error");
                return false; // Indica que hubo un error
            }
        }

        private void CargarTodosProductores()
        {
            // Crear y mostrar el formulario de carga
            Carga.StartPosition = FormStartPosition.CenterScreen;
            Carga.Show();

            // Ejecutar la carga de datos en un hilo separado
            Task.Run(() =>
            {
                try
                {
                    // Actualizar el formulario de carga para mostrar "Cargando" y progreso del 30%
                    this.Invoke((MethodInvoker)delegate
                    {
                        Carga.UpdateStatus("Cargando, por favor espere...", 30);
                    });

                    // Llamar al método que devuelve un DataTable con todos los registros
                    DataTable dt = Daos.ObtenerTodosProductores();

                    // Actualizar el DataGridView en el hilo principal
                    this.Invoke((MethodInvoker)delegate
                    {
                        dataGridView1.DataSource = dt;

                        // Ajustar los títulos de las columnas
                        dataGridView1.Columns["id"].HeaderText = "ID";
                        dataGridView1.Columns["nombre"].HeaderText = "Nombre";
                        dataGridView1.Columns["apellidopat"].HeaderText = "Apellido Paterno";
                        dataGridView1.Columns["apellidomat"].HeaderText = "Apellido Materno";
                        dataGridView1.Columns["curp"].HeaderText = "CURP";
                        dataGridView1.Columns["telefono"].HeaderText = "Teléfono";
                        dataGridView1.Columns["municipio"].HeaderText = "Municipio";
                        dataGridView1.Columns["localidad"].HeaderText = "Localidad";
                        dataGridView1.Columns["tipo"].HeaderText = "Tipo";
                        dataGridView1.Columns["hectareas"].HeaderText = "Hectáreas";
                        dataGridView1.Columns["fecha_registro"].HeaderText = "Fecha de Registro";

                        // Actualizar el formulario de carga para mostrar el progreso completo
                        Carga.UpdateStatus("Carga completada.", 100);

                        // Cerrar el formulario de carga
                        Carga.Hide();
                    });
                }
                catch (Exception ex)
                {
                    // Mostrar el error en el hilo principal
                    this.Invoke((MethodInvoker)delegate
                    {
                        MessageBox.Show("Error al cargar los datos: " + ex.Message, "Error");
                        Carga.Hide(); // Cerrar el formulario de carga incluso en caso de error
                    });
                }
            });
        }


        private void actualizar_Load(object sender, EventArgs e)
        {
        }


        private void btnbuscar_Click_1(object sender, EventArgs e)
        {
            string busqueda = txtbusqueda.Text.Trim();

            if (string.IsNullOrEmpty(busqueda))
            {
                CargarTodosProductores();
            }
            else
            {

                List<Productor> productores = Daos.BuscarProductorPorNombreYApellidos(busqueda);

                if (productores.Count > 0)
                {
                    dataGridView1.DataSource = productores;
                }
                else
                {
                    MessageBox.Show("No se encontraron productores con esa búsqueda.", "Información");
                }
            }
        }

        private void btnactualizar_Click_1(object sender, EventArgs e)
        {
            bool actualizacionExitosa = true;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) continue;
                if (row.Cells["nombre"].Value != null && row.Cells["nombre"].Value.ToString() != "")
                {
                    if (!ActualizarProductor(row))
                    {
                        actualizacionExitosa = false;
                    }
                }
            }
            txtbusqueda.Text = "";

            if (actualizacionExitosa)
            {
                MessageBox.Show("El productor fue actualizado exitosamente.", "Éxito");
            }
            else
            {
                MessageBox.Show("Hubo errores al actualizar al productor.", "Error");
            }
            txtbusqueda.Text = "";
        }

        private void txtbusqueda_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            // Verificar si la tecla presionada es Enter
            if (e.KeyChar == (char)Keys.Enter)
            {
                // Suprimir el sonido "ding"
                e.Handled = true;
                btnbuscar.PerformClick();
            }
        }

        private void btneliminar_Click(object sender, EventArgs e)
        {
            // Verificar que haya una fila seleccionada en el DataGridView
            if (dataGridView1.CurrentRow != null)
            {
                // Obtener el ID del productor seleccionado
                int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ID"].Value);

                // Confirmar la eliminación
                var confirmResult = MessageBox.Show("¿Está seguro de que desea eliminar este productor?", "Confirmar eliminación", MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    // Llamar al método para eliminar el productor
                    try
                    {
                        Daos.EliminarProductorPorId(id);

                        // Volver a cargar los datos después de eliminar
                        CargarTodosProductores();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al intentar eliminar el productor: " + ex.Message, "Error");
                    }
                }
            }
            else
            {
                MessageBox.Show("Seleccione un productor para eliminar.", "Advertencia");
            }
        }

        private void actualizar_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                CargarTodosProductores();
                dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
                dataGridView1.DefaultCellStyle.Font = new Font("Arial", 9);
            }
        }
    }
}
