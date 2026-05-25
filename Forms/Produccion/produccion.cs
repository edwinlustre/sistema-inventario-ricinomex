using appInventario.DAOS;
using appInventario.Form_Produccion;
using appInventario.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace appInventario.Forms
{
    public partial class produccion : Form
    {
        //Instancias
        private daosproduccion Daos;
        private resultadopedidos resultadop;
        private inventariograno inventariog;
        private inventarioproducto inventariop;
        private productoterminado productot;
        private ProductoTerminado pt;
        private carga Carga;
        private IFormToShow originalForm;

        public produccion(IFormToShow form)
        {
            //Inicializacion de instancias
            InitializeComponent();
            Daos = new daosproduccion();
            resultadop = new resultadopedidos();
            inventariog = new inventariograno();
            inventariop = new inventarioproducto();
            productot = new productoterminado();
            Carga = new carga();

            this.originalForm = form;
            this.FormClosing += new FormClosingEventHandler(produccion_FormClosing);
            label3.TextChanged += new EventHandler(textoCapacidad_TextChanged);
            comboBoxProducto.SelectedIndexChanged += comboBoxProducto_SelectedIndexChanged;
            comboBoxCapacidad.SelectedIndexChanged += comboBoxCapacidad_SelectedIndexChanged;

            //Inicializacion
            comboBoxProducto.SelectedIndex = 0;
            comboBoxCapacidad.SelectedIndex = 0;
            comboBoxGuardado.SelectedIndex = 0;
            comboBoxTipo.SelectedIndex = 0;
            comboBoxPresentacion.SelectedIndex = 0;
            comboBoxGuardadoR.SelectedIndex = 0;

            //Daos.VerificarEInsertarInventarioSiVacio();
        }

        private void textoCapacidad_TextChanged(object sender, EventArgs e)
        {
            if (comboBoxProducto.SelectedIndex == 1)
            {
                label3.Text = "Ingresa los kilos";
            }
            else if (comboBoxProducto.SelectedIndex == 2)
            {
                label3.Text = "Ingresa los litros";
            }
        }

        private void produccion_Load(object sender, EventArgs e)
        {

        }

        private void produccion_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            originalForm?.ShowForm(); // Usa el método de la interfaz
        }

        private Dictionary<string, (string kg, string litros)> capacidades = new Dictionary<string, (string kg, string litros)>
        {
            { "Tote", ("950", "1000") },
            { "Tambor metálico", ("190", "200") },
            { "Tambor plástico", ("190", "200") },
            { "Garrafa 70L", ("66.5", "70") },
            { "Garrafa 50L", ("47.5", "50") },
            { "Garrafa 20L", ("19", "20") },
            { "Garrafa 10L", ("9.5", "10") },
            { "Garrafa 5L", ("4.75", "5") },
            { "Botella 1L", ("0.95", "1") }
        };

        private async void btnagregar_Click(object sender, EventArgs e)
        {
            // Validar los datos de entrada
            string contenido = txtcontenido.Text.Trim();
            if (comboBoxProducto.SelectedIndex == 0 || comboBoxCapacidad.SelectedIndex == 0 || comboBoxGuardado.SelectedIndex == 0 || String.IsNullOrEmpty(contenido))
            {
                MessageBox.Show("Ingresa todos los datos", "Advertencia");
                return;
            }

            string producto = comboBoxProducto.Text;
            string capacidad = comboBoxCapacidad.Text;
            string guardado = comboBoxGuardado.Text;
            int cantidad = int.Parse(txtcantidad.Text.Trim());

            // Inicializar la ventana de carga
            using (var cargaForm = new carga())
            {
                cargaForm.StartPosition = FormStartPosition.CenterScreen;
                cargaForm.Show();

                // Dividir el progreso para cada inserción
                int progresoPorProducto = 100 / cantidad;

                await Task.Run(() =>
                {
                    try
                    {
                        for (int i = 0; i < cantidad; i++)
                        {
                            Daos.inventarioProducto(producto, capacidad, guardado, contenido);

                            // Actualizar el progreso en la interfaz
                            this.Invoke((MethodInvoker)delegate
                            {
                                cargaForm.UpdateStatus($"Agregando... {i + 1} de {cantidad}", progresoPorProducto * (i + 1));
                            });
                        }

                        // Mostrar el mensaje de éxito en el hilo principal
                        this.Invoke((MethodInvoker)delegate
                        {
                            MessageBox.Show("Producto guardado exitosamente.", "Éxito");
                        });
                    }
                    catch (Exception ex)
                    {
                        // Manejar el error en el hilo principal
                        this.Invoke((MethodInvoker)delegate
                        {
                            MessageBox.Show("Error al guardar los productos: " + ex.Message, "Error");
                        });
                    }
                });

                // Cerrar la ventana de carga después de completar la tarea
                cargaForm.Close();
            }

            // Limpiar los controles de entrada después de la inserción
            comboBoxProducto.SelectedIndex = 0;
            comboBoxCapacidad.SelectedIndex = 0;
            comboBoxGuardado.SelectedIndex = 0;
            txtcontenido.Clear();
            txtcantidad.Clear();
        }

        private int CalcularUnidadesNecesarias(string presentacion, double kilosSolicitados)
        {
            if (capacidades.ContainsKey(presentacion))
            {
                double kilosPorUnidad = Convert.ToDouble(capacidades[presentacion].kg);
                int unidadesNecesarias = (int)Math.Ceiling(kilosSolicitados / kilosPorUnidad);
                return unidadesNecesarias;
            }
            else
            {
                throw new Exception("Presentación no encontrada en el diccionario de capacidades.");
            }
        }

        private void btnregistro_Click(object sender, EventArgs e)
        {
            bool disponibilidad = false;
            string fechaingreso = dateTimePickerIngreso.Value.ToString("dd-MM-yyyy");
            string kilos = txtkilos.Text.Trim();
            string producto = comboBoxTipo.Text;
            string presentacion = comboBoxPresentacion.Text;
            string fechaentrega = dateTimePickerEntrega.Value.ToString("dd-MM-yyyy");
            string guardado = comboBoxGuardadoR.Text;
            string estado = "Actual";

            try
            {
                disponibilidad = Daos.disponibilidad(presentacion, guardado);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar el pedido: " + ex.Message, "Error");
            }

            if (string.IsNullOrEmpty(kilos) || comboBoxTipo.SelectedIndex == 0 || comboBoxPresentacion.SelectedIndex == 0)
            {
                MessageBox.Show("Por favor, completa todos los campos.", "Advertencia");
                return;
            }

            if (disponibilidad == false)
            {
                MessageBox.Show("El producto que has seleccionado no lo tienes en inventario o lugar", "Advertencia");
                return;
            }

            try
            {
                double kilosSolicitados = Convert.ToDouble(kilos);
                int unidadesNecesarias = CalcularUnidadesNecesarias(presentacion, kilosSolicitados);

                Daos.registroPedido(fechaingreso, kilos, producto, presentacion, fechaentrega, estado);
                Daos.actualizarPresentacion(presentacion, guardado, unidadesNecesarias);

                MessageBox.Show("Pedido guardado exitosamente.", "Éxito");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar el pedido: " + ex.Message, "Error");
            }

            txtkilos.Clear();
            comboBoxTipo.SelectedIndex = 0;
            comboBoxPresentacion.SelectedIndex = 0;
            comboBoxGuardadoR.SelectedIndex = 0;
            dateTimePickerIngreso.Value = DateTime.Now;
            dateTimePickerEntrega.Value = DateTime.Now;
            //txtcantidad.Clear();
        }

        private void btnguardar_Click(object sender, EventArgs e)
        {

            string fechaingreso = dateTimePickerSalida.Value.ToString("dd-MM-yyyy");
            string cantidad = txtlitrost.Text;

            pt = new ProductoTerminado(fechaingreso, cantidad);
            try
            {
                Daos.RegistrarProductoTerminado(pt);
                MessageBox.Show("Producto guardado exitosamente.", "Éxito");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar los datos: " + ex.Message, "Error");
            }
            txtlitrost.Clear();
            dateTimePickerSalida.Value = DateTime.Now;
        }

        private void linkLabel4_LinkClicked(object sender, EventArgs e)
        {
            resultadop.Show();
        }

        private void linkLabel1_LinkClicked(object sender, EventArgs e)
        {
            inventariog.Show();
        }

        private void linkLabel3_LinkClicked(object sender, EventArgs e)
        {
            inventariop.Show();
        }

        private void comboBoxProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActualizarCapacidad();
        }

        private void comboBoxCapacidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActualizarCapacidad();
        }

        private void linkLabel2_LinkClicked(object sender, EventArgs e)
        {
            productot.Show();
        }

        private void ActualizarCapacidad()
        {
            if (comboBoxProducto.SelectedIndex > 0 && comboBoxCapacidad.SelectedIndex > 0)
            {
                string producto = comboBoxProducto.Text;
                string tipoCapacidad = comboBoxCapacidad.Text;

                if (capacidades.TryGetValue(producto, out var capacidad))
                {
                    if (tipoCapacidad == "Kg")
                    {
                        txtcontenido.Text = capacidad.kg;
                    }
                    else if (tipoCapacidad == "Litros")
                    {
                        txtcontenido.Text = capacidad.litros;
                    }
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // Alternar la visibilidad de los paneles
            if (panellateral.Visible)
            {
                // Ocultar panel1 y expandir panel2
                panellateral.Visible = false;
                panelfondo.Size = new Size(panelfondo.Width + panellateral.Width, panelfondo.Height);
                panelfondo.Location = new Point(panelfondo.Location.X - panellateral.Width, panelfondo.Location.Y);
                tabControl1.Size = new Size(tabControl1.Width + panellateral.Width, tabControl1.Height);
                tabControl1.Location = new Point(tabControl1.Location.X - panellateral.Width, tabControl1.Location.Y);
            }
            else
            {
                // Mostrar panel1 y contraer panel2
                panellateral.Visible = true;
                panelfondo.Size = new Size(panelfondo.Width - panellateral.Width, panelfondo.Height);
                panelfondo.Location = new Point(panelfondo.Location.X + panellateral.Width, panelfondo.Location.Y);
                tabControl1.Size = new Size(tabControl1.Width - panellateral.Width, tabControl1.Height);
                tabControl1.Location = new Point(tabControl1.Location.X + panellateral.Width, tabControl1.Location.Y);
            }
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            pictureBox1.Cursor = Cursors.Hand; // Cambiar a la mano apuntando
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            pictureBox1.Cursor = Cursors.Default; // Volver al cursor por defecto
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            linkLabel1_LinkClicked(sender, e);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            linkLabel3_LinkClicked(sender, e);
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            linkLabel2_LinkClicked(sender, e);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            linkLabel4_LinkClicked(sender, e);
        }

        private void pictureBox5_MouseEnter(object sender, EventArgs e)
        {
            pictureBox5.Cursor = Cursors.Hand; // Cambiar a la mano apuntando
        }

        private void pictureBox2_MouseEnter(object sender, EventArgs e)
        {
            pictureBox2.Cursor = Cursors.Hand; // Cambiar a la mano apuntando
        }

        private void pictureBox4_MouseEnter(object sender, EventArgs e)
        {
            pictureBox4.Cursor = Cursors.Hand; // Cambiar a la mano apuntando
        }

        private void pictureBox3_MouseEnter(object sender, EventArgs e)
        {
            pictureBox3.Cursor = Cursors.Hand; // Cambiar a la mano apuntando
        }

        private void pictureBox5_MouseLeave(object sender, EventArgs e)
        {
            pictureBox5.Cursor = Cursors.Default; // Volver al cursor por defecto
        }

        private void pictureBox2_MouseLeave(object sender, EventArgs e)
        {
            pictureBox2.Cursor = Cursors.Default; // Volver al cursor por defecto
        }

        private void pictureBox4_MouseLeave(object sender, EventArgs e)
        {
            pictureBox4.Cursor = Cursors.Default; // Volver al cursor por defecto
        }

        private void pictureBox3_MouseLeave(object sender, EventArgs e)
        {
            pictureBox3.Cursor = Cursors.Default; // Volver al cursor por defecto
        }
    }
}
