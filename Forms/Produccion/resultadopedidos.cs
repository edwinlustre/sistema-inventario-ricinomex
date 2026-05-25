using appInventario.DAOS;
using appInventario.Forms;
using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace appInventario.Form_Produccion
{
    public partial class resultadopedidos : Form
    {

        private daosproduccion Daos;
        private carga Carga;

        public resultadopedidos()
        {
            Daos = new daosproduccion();
            Carga = new carga();
            InitializeComponent();

            this.FormClosing += resultadospedidos_FormClosing;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void resultadospedidos_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;

            // Opcional: Puedes ocultar el formulario en lugar de cerrarlo si prefieres reutilizarlo
            this.Hide();
        }


        private void CargarTodosPedidos()
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
                    DataTable dt = Daos.obtenerTodosPedidos();

                    if (dt == null || dt.Rows.Count == 0)
                    {
                        MessageBox.Show("No se encontraron datos.", "Información");
                        return;
                    }

                    // Configura el DataGridView
                    this.Invoke((MethodInvoker)delegate
                    {
                        dataGridView1.DataSource = dt;
                        dataGridView1.ReadOnly = true;
                        dataGridView1.AllowUserToAddRows = false;

                        // Elimina la columna de botones si existe
                        if (dataGridView1.Columns.Contains("Actualizar"))
                        {
                            dataGridView1.Columns.Remove("Actualizar");
                        }

                        // Asegúrate de que la columna "Actualizar" exista y esté visible
                        DataGridViewButtonColumn actualizarButtonColumn = new DataGridViewButtonColumn
                        {
                            Name = "Actualizar",
                            HeaderText = "Acción",
                            Text = "Actualizar",
                            UseColumnTextForButtonValue = true,
                            Width = 100
                        };
                        dataGridView1.Columns.Add(actualizarButtonColumn);

                        // Ajustar los títulos de las columnas
                        dataGridView1.Columns["id"].HeaderText = "ID Pedido";
                        dataGridView1.Columns["fecha_ingreso"].HeaderText = "Fecha de Ingreso";
                        dataGridView1.Columns["kilos"].HeaderText = "Kilos";
                        dataGridView1.Columns["producto"].HeaderText = "Producto";
                        dataGridView1.Columns["presentacion"].HeaderText = "Presentación";
                        dataGridView1.Columns["fecha_entrega"].HeaderText = "Fecha de Entrega";
                        dataGridView1.Columns["estado"].HeaderText = "Estado";

                        // Aplica formato a las celdas y configura el botón
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            string estadoPago = row.Cells["estado"].Value?.ToString();
                            string fechaEntregaStr = row.Cells["fecha_entrega"].Value?.ToString();

                            if (string.IsNullOrEmpty(fechaEntregaStr))
                            {
                                // Manejo de errores de fecha vacía
                                continue;
                            }

                            if (DateTime.TryParseExact(fechaEntregaStr, "dd-MM-yyyy", null, DateTimeStyles.None, out DateTime fechaEntrega))
                            {
                                // Calcular la diferencia de días
                                int diasTranscurridos = (DateTime.Now - fechaEntrega).Days;

                                // Actualizar el estado si han pasado más de 7 días
                                if (diasTranscurridos > 7 && estadoPago == "Actual")
                                {
                                    estadoPago = "Demorado";
                                    row.Cells["estado"].Value = estadoPago;
                                    Daos.UpdateShopStatus(Convert.ToInt32(row.Cells["id"].Value), estadoPago);
                                    row.Cells["estado"].Style.ForeColor = Color.White;
                                }

                                // Configura el estado de pago y formato de celdas
                                if (estadoPago == "Actual")
                                {
                                    row.Cells["estado"].Style.BackColor = Color.Blue;
                                    row.Cells["estado"].Style.ForeColor = Color.White;
                                }
                                else if (estadoPago == "Demorado")
                                {
                                    // Aplicar color rojo a la celda cuando el estado es "Demorado"
                                    row.Cells["estado"].Style.BackColor = Color.Red;
                                }
                                else if (estadoPago == "Entregado")
                                {
                                    row.Cells["estado"].Style.BackColor = Color.Green;

                                    // Oculta el botón en lugar de cambiar su valor
                                    row.Cells["Actualizar"] = new DataGridViewTextBoxCell(); // Usa una celda de texto vacía
                                }
                            }
                            else
                            {
                                MessageBox.Show($"Error al convertir la fecha de entrada: {fechaEntregaStr} en la fila {row.Index}", "Error");
                            }
                        }

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


        private void resultadopedidos_Load(object sender, EventArgs e)
        {
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold); // Cambia "Arial" por el tipo de letra que prefieras, "12" por el tamaño, y "Bold" para negrita.
            dataGridView1.DefaultCellStyle.Font = new Font("Arial", 9); // Cambia "Arial" por el tipo de letra que prefieras, y "12" por el tamaño.
        }

        private void resultadopedidos_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                CargarTodosPedidos();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["Actualizar"].Index && e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                int id = Convert.ToInt32(row.Cells["id"].Value);
                string estadoPago = row.Cells["estado"].Value?.ToString();

                if (estadoPago == "Entregado")
                {
                    MessageBox.Show("Este ítem ya está entregado.", "Información");
                    return;
                }

                // Permitir actualizar de "Actual" o "Demorado" a "Entregado"
                string nuevoEstado = (estadoPago == "Actual" || estadoPago == "Demorado") ? "Entregado" : null;

                if (nuevoEstado != null)
                {
                    try
                    {
                        Daos.UpdateShopStatus(id, nuevoEstado);
                        MessageBox.Show("Estado actualizado a " + nuevoEstado, "Mensaje");
                        CargarTodosPedidos(); // Recargar la lista de pedidos después de la actualización
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al actualizar el estado: " + ex.Message, "Error");
                    }
                }
            }
        }
    }
}
