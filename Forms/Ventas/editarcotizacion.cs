using appInventario.DAOS;
using appInventario.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace appInventario.Form_Ventas
{
    public partial class editarcotizacion : Form
    {

        private daosventas Daos;
        private string nombrevendedor = null;
        private string correovendedor = null;
        private string numerovendedor = null;
        private string empresa = null;
        private string nombrec = null;
        private string apellidopatc = null;
        private string apellidomatc = null;
        private string telefono = null;
        private string correo = null;

        public editarcotizacion()
        {
            Daos = new daosventas();
            InitializeComponent();

            comboBoxProducto.SelectedIndex = 0;
            comboBoxCapacidad.SelectedIndex = 0;
            comboBoxAceite.SelectedIndex = 0;

            txtempresa.Enabled = false;
            dataGridView1.ReadOnly = true;

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.FormClosing += editarcotizacion_FormClosing;
        }

        private void editarcotizacion_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            // Limpiar el DataGridView cuando se cierra el formulario
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();

            this.Hide();
        }
        private void editarcotizacion_Load(object sender, EventArgs e)
        {
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Arial", 10, FontStyle.Bold);
            dataGridView1.DefaultCellStyle.Font = new System.Drawing.Font("Arial", 9);
        }

        public void MostrarCotizaciones(int id)
        {
            // Obtener la cotización por ID
            DataTable dtCotizacion = Daos.ObtenerCotizacionPorId(id);

            // Crear una tabla temporal para mostrar los datos separados
            DataTable dtProductos = new DataTable();
            dtProductos.Columns.Add("ID");
            dtProductos.Columns.Add("Producto");
            dtProductos.Columns.Add("Capacidad");
            dtProductos.Columns.Add("Cantidad");
            dtProductos.Columns.Add("Litros / Kilos");
            dtProductos.Columns.Add("Tipo");
            dtProductos.Columns.Add("Precio");

            foreach (DataRow row in dtCotizacion.Rows)
            {
                txtempresa.Text = row["empresa"].ToString();
                string[] aceitesGarrafa = row["aceite_garrafa"].ToString().Split(',');
                string[] garrafas = row["garrafas"].ToString().Split(',');
                string[] numGarrafas = row["num_garrafas"].ToString().Split(',');

                string[] aceitesTote = row["aceite_tote"].ToString().Split(',');
                string[] totes = row["totes"].ToString().Split(',');
                string[] numTotes = row["num_totes"].ToString().Split(',');

                string[] aceitesTambor = row["aceite_tambor"].ToString().Split(',');
                string[] tambores = row["tambores"].ToString().Split(',');
                string[] numTambores = row["num_tambores"].ToString().Split(',');

                int maxLength = Math.Max(aceitesGarrafa.Length, Math.Max(aceitesTote.Length, aceitesTambor.Length));

                for (int i = 0; i < maxLength; i++)
                {
                    if (i < aceitesGarrafa.Length && !string.IsNullOrWhiteSpace(aceitesGarrafa[i]) &&
                        i < garrafas.Length && !string.IsNullOrWhiteSpace(garrafas[i]) &&
                        i < numGarrafas.Length && !string.IsNullOrWhiteSpace(numGarrafas[i]))
                    {
                        DataRow newRow = dtProductos.NewRow();
                        newRow["ID"] = row["id"];
                        newRow["Tipo"] = aceitesGarrafa[i];
                        newRow["Producto"] = "Garrafa";
                        if (garrafas[i].Length > 1)
                        {
                            newRow["Capacidad"] = garrafas[i].Substring(0, garrafas[i].Length - 1);
                            string tipo = garrafas[i].Last().ToString();
                            if (tipo == "L")
                            {
                                newRow["Litros / Kilos"] = "Litros";
                            }
                            else if (tipo == "K")
                            {
                                newRow["Litros / Kilos"] = "Kg";
                            }
                        }
                        else
                        {
                            newRow["Capacidad"] = garrafas[i];
                            newRow["Litros / Kilos"] = "";
                        }
                        newRow["Cantidad"] = numGarrafas[i];
                        newRow["Precio"] = ""; // Columna de precio vacía
                        dtProductos.Rows.Add(newRow);
                    }

                    if (i < aceitesTote.Length && !string.IsNullOrWhiteSpace(aceitesTote[i]) &&
                        i < totes.Length && !string.IsNullOrWhiteSpace(totes[i]) &&
                        i < numTotes.Length && !string.IsNullOrWhiteSpace(numTotes[i]))
                    {
                        DataRow newRow = dtProductos.NewRow();
                        newRow["ID"] = row["id"];
                        newRow["Tipo"] = aceitesTote[i];
                        newRow["Producto"] = "Tote";
                        if (totes[i].Length > 1)
                        {
                            newRow["Capacidad"] = totes[i].Substring(0, totes[i].Length - 1);
                            string tipo = totes[i].Last().ToString();
                            if (tipo == "L")
                            {
                                newRow["Litros / Kilos"] = "Litros";
                            }
                            else if (tipo == "K")
                            {
                                newRow["Litros / Kilos"] = "Kg";
                            }
                        }
                        else
                        {
                            newRow["Capacidad"] = totes[i];
                            newRow["Litros / Kilos"] = "";
                        }
                        newRow["Cantidad"] = numTotes[i];
                        newRow["Precio"] = ""; // Columna de precio vacía
                        dtProductos.Rows.Add(newRow);
                    }

                    if (i < aceitesTambor.Length && !string.IsNullOrWhiteSpace(aceitesTambor[i]) &&
                        i < tambores.Length && !string.IsNullOrWhiteSpace(tambores[i]) &&
                        i < numTambores.Length && !string.IsNullOrWhiteSpace(numTambores[i]))
                    {
                        DataRow newRow = dtProductos.NewRow();
                        newRow["ID"] = row["id"];
                        newRow["Tipo"] = aceitesTambor[i];
                        newRow["Producto"] = "Tambor";
                        if (tambores[i].Length > 1)
                        {
                            newRow["Capacidad"] = tambores[i].Substring(0, tambores[i].Length - 1);
                            string tipo = tambores[i].Last().ToString();
                            if (tipo == "L")
                            {
                                newRow["Litros / Kilos"] = "Litros";
                            }
                            else if (tipo == "K")
                            {
                                newRow["Litros / Kilos"] = "Kg";
                            }
                        }
                        else
                        {
                            newRow["Capacidad"] = tambores[i];
                            newRow["Litros / Kilos"] = "";
                        }
                        newRow["Cantidad"] = numTambores[i];
                        newRow["Precio"] = ""; // Columna de precio vacía
                        dtProductos.Rows.Add(newRow);
                    }
                }
            }

            dataGridView1.DataSource = dtProductos;

            // Configurar columnas (puedes estilizar aquí si es necesario)
            dataGridView1.Columns["ID"].HeaderText = "ID";
            dataGridView1.Columns["Producto"].HeaderText = "Producto";
            dataGridView1.Columns["Capacidad"].HeaderText = "Capacidad";
            dataGridView1.Columns["Cantidad"].HeaderText = "Cantidad";
            dataGridView1.Columns["Litros / Kilos"].HeaderText = "Litros / Kilos";
            dataGridView1.Columns["Tipo"].HeaderText = "Tipo";
            dataGridView1.Columns["Precio"].HeaderText = "Precio";
        }

        public void ObtenerDatosCliente(int idCotizacion)
        {

            // Obtener datos de la cotización
            DataTable dtCotizacion = Daos.ObtenerCotizacionPorId(idCotizacion);

            if (dtCotizacion.Rows.Count > 0)
            {
                DataRow cotizacionRow = dtCotizacion.Rows[0];
                int idEmpresa = Convert.ToInt32(cotizacionRow["id_empresa"]);
                // Obtener datos del cliente
                DataTable dtCliente = Daos.ObtenerClientePorIdEmpresa(idEmpresa);

                if (dtCliente.Rows.Count > 0)
                {
                    DataRow clienteRow = dtCliente.Rows[0];
                    empresa = clienteRow["empresa"].ToString();
                    nombrec = clienteRow["nombre"].ToString();
                    apellidopatc = clienteRow["apellidopat"].ToString();
                    apellidomatc = clienteRow["apellidomat"].ToString();
                    telefono = clienteRow["telefono"].ToString();
                    correo = clienteRow["correo"].ToString();
                }
            }

        }

        private void btneliminar_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    if (!row.IsNewRow) // Evitar la eliminación de la fila nueva vacía
                    {
                        dataGridView1.Rows.Remove(row);
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona una fila para eliminar.");
            }
        }

        private void btnactualizar_Click(object sender, EventArgs e)
        {
            // Validar que los campos no estén vacíos y que los índices seleccionados sean válidos
            if (comboBoxProducto.SelectedIndex == 0 || comboBoxCapacidad.SelectedIndex == 0 ||
                string.IsNullOrWhiteSpace(txtlitros.Text) || comboBoxAceite.SelectedIndex == 0 ||
                string.IsNullOrWhiteSpace(txtnumprod.Text) || string.IsNullOrWhiteSpace(txtprecio.Text) || string.IsNullOrWhiteSpace(txtempresa.Text))
            {
                MessageBox.Show("Por favor, complete todos los campos antes de añadir.");
                return;
            }

            // Recoger datos de los controles
            string producto = comboBoxProducto.Text; // Obtener el nombre completo del producto
            string capacidadSeleccionada = comboBoxCapacidad.Text;
            string litroskilos = txtlitros.Text.Trim();
            string tipo = comboBoxAceite.Text;
            string cantidad = txtnumprod.Text;
            string precioTexto = txtprecio.Text.Trim();

            // Validar que el precio sea un número válido
            decimal precio;
            if (!decimal.TryParse(precioTexto, out precio) || precio < 0)
            {
                MessageBox.Show("El precio debe ser un número válido mayor o igual a 0.");
                return;
            }

            // Validar que txtlitros sea un número válido
            decimal cantidadDecimal;
            if (!decimal.TryParse(litroskilos, out cantidadDecimal) || cantidadDecimal <= 0)
            {
                MessageBox.Show("La cantidad debe ser un número válido mayor a 0.");
                return;
            }

            // Obtener el índice de la fila seleccionada
            int filaSeleccionada = dataGridView1.CurrentCell.RowIndex;

            // Actualizar la fila existente
            dataGridView1.Rows[filaSeleccionada].Cells["Producto"].Value = producto;
            dataGridView1.Rows[filaSeleccionada].Cells["Capacidad"].Value = litroskilos;
            dataGridView1.Rows[filaSeleccionada].Cells["Cantidad"].Value = cantidad;
            dataGridView1.Rows[filaSeleccionada].Cells["Litros / Kilos"].Value = capacidadSeleccionada;
            dataGridView1.Rows[filaSeleccionada].Cells["Tipo"].Value = tipo;
            dataGridView1.Rows[filaSeleccionada].Cells["Precio"].Value = precio;

            // Limpiar los controles para el siguiente registro
            comboBoxProducto.SelectedIndex = 0;
            comboBoxCapacidad.SelectedIndex = 0;
            txtlitros.Clear();
            txtprecio.Clear();
            comboBoxAceite.SelectedIndex = 0;
            txtnumprod.Clear();

        }

        private void btnagregar_Click(object sender, EventArgs e)
        {
            // Validar que los campos no estén vacíos y que los índices seleccionados sean válidos
            if (comboBoxProducto.SelectedIndex == 0 || comboBoxCapacidad.SelectedIndex == 0 ||
                string.IsNullOrWhiteSpace(txtlitros.Text) || comboBoxAceite.SelectedIndex == 0 ||
                string.IsNullOrWhiteSpace(txtnumprod.Text) || string.IsNullOrWhiteSpace(txtprecio.Text) || string.IsNullOrWhiteSpace(txtempresa.Text))
            {
                MessageBox.Show("Por favor, complete todos los campos antes de añadir.");
                return;
            }

            // Recoger datos de los controles
            string producto = comboBoxProducto.Text;
            string capacidadSeleccionada = comboBoxCapacidad.Text;
            string litroskilos = txtlitros.Text.Trim();
            string tipo = comboBoxAceite.Text;
            string cantidad = txtnumprod.Text;
            string precioTexto = txtprecio.Text.Trim();

            // Validar que el precio sea un número válido
            decimal precio;
            if (!decimal.TryParse(precioTexto, out precio) || precio < 0)
            {
                MessageBox.Show("El precio debe ser un número válido mayor o igual a 0.");
                return;
            }

            // Validar que txtlitros sea un número válido
            decimal cantidadDecimal;
            if (!decimal.TryParse(litroskilos, out cantidadDecimal) || cantidadDecimal <= 0)
            {
                MessageBox.Show("La cantidad debe ser un número válido mayor a 0.");
                return;
            }

            // Obtener el DataTable enlazado al DataGridView
            DataTable dataTable = (DataTable)dataGridView1.DataSource;

            // Obtener el ID de la última fila en el DataGridView
            int ultimoId = Convert.ToInt32(dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells["id"].Value);

            // Crear una nueva fila en el DataTable
            DataRow nuevaFila = dataTable.NewRow();
            nuevaFila["id"] = ultimoId;
            nuevaFila["Producto"] = producto;
            nuevaFila["Capacidad"] = litroskilos;
            nuevaFila["Litros / Kilos"] = capacidadSeleccionada;
            nuevaFila["Tipo"] = tipo;
            nuevaFila["Cantidad"] = cantidad;
            nuevaFila["Precio"] = precio;

            // Agregar la nueva fila al DataTable
            dataTable.Rows.Add(nuevaFila);
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Asegurarse de que no es el encabezado
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // Asignar valores de celdas a los TextBox
                txtlitros.Text = row.Cells["Capacidad"].Value.ToString();
                txtnumprod.Text = row.Cells["Cantidad"].Value.ToString();

                // Obtener valores de celdas
                string litrosokg = row.Cells["Litros / Kilos"].Value.ToString();
                string capacidad = row.Cells["Capacidad"].Value.ToString();
                string producto1 = row.Cells["Producto"].Value.ToString();
                // Dividir la cadena en palabras separadas por espacios y tomar la primera
                string producto = producto1.Split(' ')[0];
                string tipo = row.Cells["Tipo"].Value.ToString();

                comboBoxProducto.SelectedIndex = 0; // No seleccionar ningún ítem si no hay coincidencia

                // Ajustar el valor del ComboBox de capacidad para que coincida con la capacidad
                if (comboBoxCapacidad.Items.Contains(litrosokg))
                {
                    comboBoxCapacidad.SelectedItem = litrosokg;
                }
                else
                {
                    comboBoxCapacidad.SelectedIndex = 0; // No seleccionar ningún ítem si no hay coincidencia
                }

                if (comboBoxAceite.Items.Contains(tipo))
                {
                    comboBoxAceite.SelectedItem = tipo;
                }
                else
                {
                    comboBoxAceite.SelectedIndex = 0; // No seleccionar ningún ítem si no hay coincidencia
                }
            }
        }

        private async void btnguardar_Click(object sender, EventArgs e)
        {
            bool registro = false;
            try
            {
                // Preguntar si se va a añadir envío
                decimal envio = 0;
                DialogResult agregarEnvio = MessageBox.Show("¿Desea agregar un costo de envío?", "Confirmar Envío", MessageBoxButtons.YesNo);
                if (agregarEnvio == DialogResult.Yes)
                {
                    string inputEnvio = Prompt.ShowDialog("Ingrese el costo de envío:", "Costo de Envío");
                    if (!decimal.TryParse(inputEnvio, out envio))
                    {
                        MessageBox.Show("El valor ingresado para el envío no es válido.");
                        return;
                    }
                }

                // Preguntar si el precio incluye IVA
                bool incluyeIVA = MessageBox.Show("¿El precio incluye IVA?", "Confirmar IVA", MessageBoxButtons.YesNo) == DialogResult.Yes;

                // Inicializar el subtotal
                decimal subtotal = 0;

                // Inicializar variables para acumular datos
                string garrafas = "";
                string numGarrafas = "";
                string totes = "";
                string numTotes = "";
                string tambores = "";
                string numTambores = "";
                string id = "";

                // Variables para tipos de aceite
                string aceiteGarrafa = "";
                string aceiteTote = "";
                string aceiteTambor = "";
                string empresa = txtempresa.Text.Trim();

                // Recorrer las filas del DataGridView para calcular el subtotal y acumular valores
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        // Obtener el número de productos y el precio
                        string numeroProductosTexto = row.Cells["Cantidad"].Value?.ToString() ?? "0";
                        string precioTexto = row.Cells["Precio"].Value?.ToString() ?? "0";

                        // Validar y convertir los valores a decimal
                        if (!decimal.TryParse(numeroProductosTexto, out decimal numeroProductos))
                        {
                            MessageBox.Show($"El valor de 'Cantidad' '{numeroProductosTexto}' no es válido.");
                            return;
                        }

                        if (!decimal.TryParse(precioTexto, out decimal precio))
                        {
                            MessageBox.Show($"El valor de 'Precio' '{precioTexto}' no es válido.");
                            return;
                        }

                        // Calcular el subtotal para esa fila
                        subtotal += numeroProductos * precio;

                        // Obtener valores de la fila
                        id = row.Cells["id"].Value?.ToString() ?? string.Empty;
                        string producto = row.Cells["Producto"].Value?.ToString() ?? string.Empty;
                        string capacidadTexto = row.Cells["Capacidad"].Value?.ToString() ?? "0";
                        string LorKTexto = row.Cells["Litros / Kilos"].Value?.ToString() ?? "0";
                        string cantidadTexto = row.Cells["Cantidad"].Value?.ToString() ?? "0";
                        string tipoAceite = row.Cells["Tipo"].Value?.ToString() ?? string.Empty;


                        // Determinar la unidad de medida
                        string unidad = "";
                        if (LorKTexto.ToLower().Contains("litros"))
                        {
                            unidad = "L";
                        }
                        else if (LorKTexto.ToLower().Contains("kilos") || LorKTexto.ToLower().Contains("kg"))
                        {
                            unidad = "K";
                        }

                        // Formatear la cantidad con la unidad
                        string cantidadConUnidad = capacidadTexto + unidad;

                        // Acumular los valores según el tipo de producto
                        if (producto.StartsWith("Garrafa"))
                        {
                            garrafas += (garrafas == "" ? "" : ",") + cantidadConUnidad;
                            numGarrafas += (numGarrafas == "" ? "" : ",") + cantidadTexto;
                            aceiteGarrafa += (aceiteGarrafa == "" ? "" : ",") + tipoAceite;
                        }
                        else if (producto == "Tote")
                        {
                            totes += (totes == "" ? "" : ",") + cantidadConUnidad;
                            numTotes += (numTotes == "" ? "" : ",") + cantidadTexto;
                            aceiteTote += (aceiteTote == "" ? "" : ",") + tipoAceite;
                        }
                        else if (producto == "Tambor")
                        {
                            tambores += (tambores == "" ? "" : ",") + cantidadConUnidad;
                            numTambores += (numTambores == "" ? "" : ",") + cantidadTexto;
                            aceiteTambor += (aceiteTambor == "" ? "" : ",") + tipoAceite;
                        }
                    }
                }

                // Calcular IVA
                decimal iva = incluyeIVA ? (subtotal + envio) * 0.16m : 0;
                decimal total = subtotal + iva + envio;

                // Guardar en la base de datos (solo una vez)
                string fechacompra = dateTimePickerCompra.Value.ToString("dd-MM-yyyy");

                ObtenerDatosVendedor();

                // Mostrar la ventana de condiciones
                string condiciones = string.Empty;
                using (var ventanaCondiciones = new agregarcondiciones())
                {
                    if (ventanaCondiciones.ShowDialog() == DialogResult.OK)
                    {
                        condiciones = ventanaCondiciones.Condiciones;
                    }
                }

                registro = Daos.EmpresaExiste(empresa);

                // Mostrar la ventana de carga y generar el PDF
                using (var loadingForm = new carga())
                {
                    loadingForm.Show();
                    await Task.Run(() =>
                    {
                        try
                        {
                            // Registrar la compra en la base de datos
                            loadingForm.UpdateStatus("Guardando datos...", 30);
                            Daos.actualizarCotizacion(id, empresa, fechacompra, aceiteGarrafa, garrafas, numGarrafas,
                                               aceiteTote, totes, numTotes, aceiteTambor, tambores, numTambores, envio,
                                               subtotal, iva, total);

                            // Crear el PDF
                            loadingForm.UpdateStatus("Generando PDF...", 60);
                            string pdfPath = null;
                            if (registro == false)
                            {
                                empresa = null;
                                nombrec = nombrec + " " + apellidopatc + " " + apellidomatc;
                                pdfPath = CreatePdf(id, empresa, nombrec, telefono, correo, nombrevendedor, numerovendedor, correovendedor, aceiteGarrafa, garrafas, numGarrafas, aceiteTote, totes,
                                                    numTotes, aceiteTambor, tambores, numTambores, envio, subtotal, iva, total, fechacompra, condiciones);

                            }
                            else
                            {
                                nombrec = nombrec + " " + apellidopatc + " " + apellidomatc;
                                pdfPath = CreatePdf(id, empresa, nombrec, telefono, correo, nombrevendedor, numerovendedor, correovendedor, aceiteGarrafa, garrafas, numGarrafas, aceiteTote, totes,
                                                      numTotes, aceiteTambor, tambores, numTambores, envio, subtotal, iva, total, fechacompra, condiciones);
                            }


                            loadingForm.UpdateStatus("Guardando PDF...", 100);
                            SavePdfToUserSelectedPath(pdfPath);
                            txtempresa.Clear();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error: " + ex.Message, "Error");
                        }
                    });
                    loadingForm.Close();
                }
                MessageBox.Show("Cotizacion realizada.", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error inesperado: " + ex.Message);
            }

        }

        private void ObtenerDatosVendedor()
        {
            using (var seleccionarVendedorForm = new seleccionarvendedor())
            {
                if (seleccionarVendedorForm.ShowDialog() == DialogResult.OK)
                {
                    // Recuperar los datos del vendedor
                    nombrevendedor = seleccionarVendedorForm.NombreVendedor;
                    numerovendedor = seleccionarVendedorForm.TelefonoVendedor;
                    correovendedor = seleccionarVendedorForm.CorreoVendedor;

                    // Verifica si los datos son válidos
                    if (!string.IsNullOrWhiteSpace(nombrevendedor) &&
                        !string.IsNullOrWhiteSpace(numerovendedor) &&
                        !string.IsNullOrWhiteSpace(correovendedor))
                    {
                        // Usar los datos del vendedor
                        //MessageBox.Show($"Nombre: {nombrevendedor}, Teléfono: {numerovendedor}, Correo: {correovendedor}");
                    }
                    else
                    {
                        MessageBox.Show("No se seleccionó un vendedor o no se completaron todos los datos.");
                    }
                }
                else
                {
                    MessageBox.Show("No se seleccionó un vendedor.");
                }
            }
        }

        /* --- Funcion para crear PDFS de cotizaciones --- */
        public string CreatePdf(string folio, string empresa, string nombrec, string telefono, string correo, string nombreVendedor, string numeroVendedor, string correoVendedor, string aceiteGarrafa, string garrafas, string numGarrafas,
               string aceiteTote, string totes, string numTotes, string aceiteTambor, string tambores,
               string numTambores, decimal envio, decimal subtotal, decimal iva, decimal total, string fecha, string condiciones)
        {
            string tempPdfPath = Path.Combine(Path.GetTempPath(), "Cotizacion_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".pdf");

            try
            {
                using (var document = new iTextSharp.text.Document())
                {
                    PdfWriter.GetInstance(document, new FileStream(tempPdfPath, FileMode.Create));
                    document.Open();

                    // Agregar logotipo
                    using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("appInventario.Resources.ricinomex.png"))
                    {
                        if (stream != null)
                        {
                            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(stream);
                            img.ScaleToFit(150f, 150f);
                            img.Alignment = Element.ALIGN_LEFT;
                            document.Add(img);
                        }
                        else
                        {
                            MessageBox.Show("No se encontró la imagen de la empresa.");
                        }
                    }
                    // Definir el tamaño de fuente deseado
                    iTextSharp.text.Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA, 10); // Aquí usas iTextSharp.text.Font

                    // Agregar información de la empresa
                    Paragraph headerParagraph = new Paragraph();
                    headerParagraph.Add(new Phrase("\n\n", headerFont));
                    headerParagraph.Add(new Phrase("RICINOMEX S. de R.L. de C.V.\n", headerFont));
                    headerParagraph.Add(new Phrase("Calle Higuerilla SN, Monte del Toro,\n", headerFont));
                    headerParagraph.Add(new Phrase("C.P. 71502, Heroica Ciudad de Ejutla de Crespo, Oaxaca\n\n", headerFont));
                    // Crear una fuente en negrita para el título
                    iTextSharp.text.Font boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);

                    // Agregar "Datos del Vendedor" en negrita
                    headerParagraph.Add(new Phrase("Datos del Vendedor:\n", boldFont));

                    // Agregar la información de la empresa al documento
                    document.Add(headerParagraph);

                    // Crear tabla para datos del vendedor y la información adicional
                    PdfPTable infoTable = new PdfPTable(2);
                    infoTable.WidthPercentage = 100;
                    infoTable.SetWidths(new float[] { 60f, 60f });

                    // Aquí se usa la información del vendedor seleccionada
                    infoTable.AddCell(new PdfPCell(new Phrase("Nombre: " + nombreVendedor, FontFactory.GetFont(FontFactory.HELVETICA, 10))) { Border = iTextSharp.text.Rectangle.NO_BORDER });
                    infoTable.AddCell(new PdfPCell(new Phrase("Fecha: " + fecha, FontFactory.GetFont(FontFactory.HELVETICA, 10))) { Border = iTextSharp.text.Rectangle.NO_BORDER });

                    infoTable.AddCell(new PdfPCell(new Phrase("Teléfono: " + numeroVendedor, FontFactory.GetFont(FontFactory.HELVETICA, 10))) { Border = iTextSharp.text.Rectangle.NO_BORDER });
                    infoTable.AddCell(new PdfPCell(new Phrase("N° de cotización: ARM-" + folio, FontFactory.GetFont(FontFactory.HELVETICA, 10))) { Border = iTextSharp.text.Rectangle.NO_BORDER });

                    infoTable.AddCell(new PdfPCell(new Phrase("Correo: " + correoVendedor, FontFactory.GetFont(FontFactory.HELVETICA, 10))) { Border = iTextSharp.text.Rectangle.NO_BORDER });
                    DateTime vigenciaFecha = DateTime.Now.AddDays(15);
                    infoTable.AddCell(new PdfPCell(new Phrase("Vigencia: " + vigenciaFecha.ToString("dd-MM-yyyy"), FontFactory.GetFont(FontFactory.HELVETICA, 10))) { Border = iTextSharp.text.Rectangle.NO_BORDER });

                    // Agregar "Datos del cliente:" y los campos relacionados
                    infoTable.AddCell(new PdfPCell(new Phrase("Datos del cliente:", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10)))
                    {
                        Border = iTextSharp.text.Rectangle.NO_BORDER,
                        Colspan = 2,
                        HorizontalAlignment = Element.ALIGN_LEFT // Alinear a la izquierda
                    });

                    // Agregar siempre el nombre del cliente
                    infoTable.AddCell(new PdfPCell(new Phrase("Nombre: " + nombrec, FontFactory.GetFont(FontFactory.HELVETICA, 10)))
                    {
                        Border = iTextSharp.text.Rectangle.NO_BORDER,
                        HorizontalAlignment = Element.ALIGN_LEFT // Alinear a la izquierda
                    });

                    // Agregar empresa solo si tiene un valor válido
                    if (!string.IsNullOrEmpty(empresa?.Trim()))
                    {
                        infoTable.AddCell(new PdfPCell(new Phrase("Empresa: " + empresa, FontFactory.GetFont(FontFactory.HELVETICA, 10)))
                        {
                            Border = iTextSharp.text.Rectangle.NO_BORDER,
                            HorizontalAlignment = Element.ALIGN_LEFT // Alinear a la izquierda
                        });
                    }
                    else
                    {
                        // Si la empresa es nula o vacía, puedes agregar una celda vacía o dejarlo así
                        infoTable.AddCell(new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.HELVETICA, 10)))
                        {
                            Border = iTextSharp.text.Rectangle.NO_BORDER,
                            HorizontalAlignment = Element.ALIGN_LEFT // Alinear a la izquierda
                        });
                    }

                    // Agregar telefono solo si tiene un valor válido
                    if (!string.IsNullOrEmpty(telefono?.Trim()))
                    {
                        infoTable.AddCell(new PdfPCell(new Phrase("Telefono: " + telefono, FontFactory.GetFont(FontFactory.HELVETICA, 10)))
                        {
                            Border = iTextSharp.text.Rectangle.NO_BORDER,
                            HorizontalAlignment = Element.ALIGN_LEFT // Alinear a la izquierda
                        });
                    }
                    else
                    {
                        // Si el telefono es nulo o vacío | celda vacia
                        infoTable.AddCell(new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.HELVETICA, 10)))
                        {
                            Border = iTextSharp.text.Rectangle.NO_BORDER,
                            HorizontalAlignment = Element.ALIGN_LEFT // Alinear a la izquierda
                        });
                    }

                    // Agregar correo solo si tiene un valor válido
                    if (!string.IsNullOrEmpty(correo?.Trim()))
                    {
                        infoTable.AddCell(new PdfPCell(new Phrase("Correo: " + correo, FontFactory.GetFont(FontFactory.HELVETICA, 10)))
                        {
                            Border = iTextSharp.text.Rectangle.NO_BORDER,
                            HorizontalAlignment = Element.ALIGN_LEFT // Alinear a la izquierda
                        });
                    }
                    else
                    {
                        // Si el correo es nulo o vacio | celda vacía
                        infoTable.AddCell(new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.HELVETICA, 10)))
                        {
                            Border = iTextSharp.text.Rectangle.NO_BORDER,
                            HorizontalAlignment = Element.ALIGN_LEFT // Alinear a la izquierda
                        });
                    }

                    document.Add(infoTable);


                    // Agregar saltos de línea antes de la tabla
                    document.Add(new Paragraph("\n\n"));

                    // Crear la tabla para productos
                    PdfPTable productTable = new PdfPTable(5); // 5 columnas
                    productTable.WidthPercentage = 100;
                    productTable.SetWidths(new float[] { 1f, 2f, 3f, 1f, 1f });

                    // Agregar cabecera
                    AddTableHeader(productTable);

                    // Agregar filas de productos
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (!row.IsNewRow)
                        {
                            string primeraPalabra = row.Cells["Producto"].Value?.ToString() ?? string.Empty;
                            // Obtener la primera palabra
                            string producto = primeraPalabra.Split(' ')[0];
                            string cantidadTexto = row.Cells["Cantidad"].Value?.ToString() ?? "0";
                            string precioTexto = row.Cells["Precio"].Value?.ToString() ?? "0";
                            string tipoAceite = row.Cells["Tipo"].Value?.ToString() ?? string.Empty;
                            string capacidadTexto = row.Cells["Capacidad"].Value?.ToString() ?? "0";
                            string LorKNum = row.Cells["Litros / Kilos"].Value?.ToString() ?? "0";

                            decimal precio = decimal.TryParse(precioTexto, out precio) ? precio : 0;
                            decimal cantidad = decimal.TryParse(cantidadTexto, out cantidad) ? cantidad : 0;

                            string descripcion = $"{producto.ToUpper()} DE ACEITE DE RICINO GRADO {tipoAceite.ToUpper()} EN PRESENTACION DE {capacidadTexto} {LorKNum.ToUpper()}";

                            // Agregar fila
                            productTable.AddCell(new PdfPCell(new Phrase(cantidad.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 10))) { HorizontalAlignment = Element.ALIGN_CENTER });
                            productTable.AddCell(new PdfPCell(new Phrase(producto.ToUpper(), FontFactory.GetFont(FontFactory.HELVETICA, 10))) { HorizontalAlignment = Element.ALIGN_CENTER });
                            productTable.AddCell(new PdfPCell(new Phrase(descripcion, FontFactory.GetFont(FontFactory.HELVETICA, 10))) { HorizontalAlignment = Element.ALIGN_LEFT });
                            productTable.AddCell(new PdfPCell(new Phrase(precio.ToString("C"), FontFactory.GetFont(FontFactory.HELVETICA, 10))) { HorizontalAlignment = Element.ALIGN_RIGHT });
                            productTable.AddCell(new PdfPCell(new Phrase((cantidad * precio).ToString("C"), FontFactory.GetFont(FontFactory.HELVETICA, 10))) { HorizontalAlignment = Element.ALIGN_RIGHT });
                        }
                    }

                    // Agregar filas de envio, subtotal, IVA, y total debajo de la tabla

                    productTable.AddCell(new PdfPCell(new Phrase("Subtotal", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10))) { Colspan = 4, HorizontalAlignment = Element.ALIGN_RIGHT });
                    productTable.AddCell(new PdfPCell(new Phrase(subtotal.ToString("C"), FontFactory.GetFont(FontFactory.HELVETICA, 10))) { HorizontalAlignment = Element.ALIGN_RIGHT });

                    if (envio != 0)
                    {
                        productTable.AddCell(new PdfPCell(new Phrase("Envio", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10))) { Colspan = 4, HorizontalAlignment = Element.ALIGN_RIGHT });
                        productTable.AddCell(new PdfPCell(new Phrase(envio.ToString("C"), FontFactory.GetFont(FontFactory.HELVETICA, 10))) { HorizontalAlignment = Element.ALIGN_RIGHT });

                    }

                    productTable.AddCell(new PdfPCell(new Phrase("IVA (16%)", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10))) { Colspan = 4, HorizontalAlignment = Element.ALIGN_RIGHT });
                    productTable.AddCell(new PdfPCell(new Phrase(iva.ToString("C"), FontFactory.GetFont(FontFactory.HELVETICA, 10))) { HorizontalAlignment = Element.ALIGN_RIGHT });

                    productTable.AddCell(new PdfPCell(new Phrase("Total", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10))) { Colspan = 4, HorizontalAlignment = Element.ALIGN_RIGHT });
                    productTable.AddCell(new PdfPCell(new Phrase(total.ToString("C"), FontFactory.GetFont(FontFactory.HELVETICA, 10))) { HorizontalAlignment = Element.ALIGN_RIGHT });

                    // Agregar la tabla al documento
                    document.Add(productTable);


                    // Agregar condiciones de venta
                    Paragraph conditionsParagraph = new Paragraph
                    {
                        SpacingBefore = 20f,
                        SpacingAfter = 20f
                    };

                    // Ajusta el tamaño de la fuente de las condiciones
                    conditionsParagraph.Add(new Phrase("\nCONDICIONES DE VENTA\n", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10)));
                    conditionsParagraph.Add(new Phrase("- Los precios son en función del volumen de compra, en caso de requerir un volumen distinto al de la presente, solicitar cotización.\n", FontFactory.GetFont(FontFactory.HELVETICA, 9)));
                    conditionsParagraph.Add(new Phrase("- Precios en MXN\n", FontFactory.GetFont(FontFactory.HELVETICA, 9)));
                    conditionsParagraph.Add(new Phrase("- Condiciones de entrega: Gastos de envío a cargo del cliente.\n", FontFactory.GetFont(FontFactory.HELVETICA, 9)));
                    conditionsParagraph.Add(new Phrase("- Plazo de pago: Antes de la recolección.\n", FontFactory.GetFont(FontFactory.HELVETICA, 9)));
                    // Verificar si hay condiciones personalizadas antes de añadirlas
                    if (!string.IsNullOrEmpty(condiciones))
                    {
                        conditionsParagraph.Add(new Phrase(condiciones, FontFactory.GetFont(FontFactory.HELVETICA, 9)));
                    }
                    document.Add(conditionsParagraph);


                    document.Close();
                }

                return tempPdfPath;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al crear el PDF: " + ex.Message, "Error");
                return string.Empty;
            }
        }

        private void AddTableHeader(PdfPTable table)
        {
            var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);
            var cell = new PdfPCell(new Phrase("NÚMERO DE PRODUCTOS", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY, HorizontalAlignment = Element.ALIGN_CENTER };
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("UNIDAD", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY, HorizontalAlignment = Element.ALIGN_CENTER };
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("DESCRIPCIÓN", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY, HorizontalAlignment = Element.ALIGN_CENTER };
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("PRECIO UNITARIO", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY, HorizontalAlignment = Element.ALIGN_CENTER };
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("PRECIO", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY, HorizontalAlignment = Element.ALIGN_CENTER };
            table.AddCell(cell);
        }

        public static class Prompt
        {
            public static string ShowDialog(string text, string caption)
            {
                Form prompt = new Form()
                {
                    Width = 300,
                    Height = 150,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    Text = caption,
                    StartPosition = FormStartPosition.CenterScreen
                };
                Label textLabel = new Label() { Left = 20, Top = 20, Text = text };
                TextBox textBox = new TextBox() { Left = 20, Top = 50, Width = 250 };
                Button confirmation = new Button() { Text = "Aceptar", Left = 200, Width = 70, Top = 70, DialogResult = DialogResult.OK };
                confirmation.Click += (sender, e) => { prompt.Close(); };
                prompt.Controls.Add(textBox);
                prompt.Controls.Add(confirmation);
                prompt.Controls.Add(textLabel);
                prompt.AcceptButton = confirmation;

                return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
            }
        }


        public void SavePdfToUserSelectedPath(string tempPdfPath)
        {
            // Llamar a SaveFileDialog en el hilo principal
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => SavePdfToUserSelectedPath(tempPdfPath)));
                return;
            }

            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Archivo PDF (*.pdf)|*.pdf|Todos los archivos (*.*)|*.*";
                saveFileDialog.Title = "Guardar archivo PDF";
                saveFileDialog.FileName = "Cotizacion_ARM" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".pdf";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        System.IO.File.Copy(tempPdfPath, saveFileDialog.FileName, true);
                        MessageBox.Show("PDF generado y guardado exitosamente.", "Éxito");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al guardar el PDF: " + ex.Message, "Error");
                    }
                    finally
                    {
                        // Eliminar el archivo temporal después de guardarlo
                        if (System.IO.File.Exists(tempPdfPath))
                        {
                            System.IO.File.Delete(tempPdfPath);
                        }
                    }
                }
            }
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }
    }
}
