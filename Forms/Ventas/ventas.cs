using appInventario.DAOS;
using appInventario.Forms;
using appInventario.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace appInventario.Form_Ventas
{
    public partial class ventas : Form
    {
        //Instancias
        private IFormToShow originalForm;
        private daosventas Daos;
        private registrodeempresas registroemp;
        private resultadosbusqueda busqueda;
        private registrocotizaciones registroc;
        private historialcompras historial;
        private ventasnoconcretadas ventasnc;
        private ventasconcretadas ventasc;
        private registrarvendedor vendedor;

        public string usuarioventa;
        public string nombrevendedor = null;
        public string correovendedor = null;
        public string numerovendedor = null;
        private string empresa = null;
        private string nombrec = null;
        private string apellidopatc = null;
        private string apellidomatc = null;
        private string telefono = null;
        private string correo = null;

        public int? EmpresaIdSeleccionada { get; private set; }
        public string EmpresaSeleccionada { get; private set; }

        public ventas(IFormToShow form)
        {
            /*Instancias de componentes*/
            InitializeComponent();
            Daos = new daosventas();
            registroemp = new registrodeempresas();
            busqueda = new resultadosbusqueda();
            registroc = new registrocotizaciones();
            historial = new historialcompras();
            ventasnc = new ventasnoconcretadas();
            ventasc = new ventasconcretadas();
            vendedor = new registrarvendedor();

            //Inicializacion
            txtempresa2.ReadOnly = true;
            comboBoxAceite.SelectedIndex = 0;
            comboBoxProducto.SelectedIndex = 0;
            comboBoxCapacidad.SelectedIndex = 0;

            // Limpiar columnas existentes si las hay
            dataGridView1.Columns.Clear();

            // Definir columnas
            dataGridView1.Columns.Add("Producto", "Producto");
            dataGridView1.Columns.Add("Capacidad", "Capacidad");
            dataGridView1.Columns.Add("Cantidad", "Cantidad");
            dataGridView1.Columns.Add("Litros / Kilos", "Litros / Kilos");
            dataGridView1.Columns.Add("Tipo", "Tipo");
            dataGridView1.Columns.Add("Precio", "Precio");
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            this.originalForm = form;
            this.FormClosing += new FormClosingEventHandler(ventas_FormClosing);
        }

        public ventas()
        {
            InitializeComponent();
        }

        private void ventas_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            originalForm?.ShowForm(); // Usa el método de la interfaz
        }

        private void ventas_Load(object sender, EventArgs e)
        {
            // Inicializa el TextBox con un placeholder
            InicializarTextBoxConPlaceholder(txtbuscar, " Buscar al cliente");
        }

        private void btnagregar_Click(object sender, EventArgs e)
        {
            string empresa = txtempresa.Text.Trim();
            string nombre = txtnombre.Text.Trim();
            string apellidopat = txtapellidopat.Text.Trim();
            string apellidomat = txtapellidomat.Text.Trim();
            string telefono = txttelefono.Text.Trim();
            string correo = txtcorreo.Text.Trim();
            string domicilio = txtdomicilio.Text.Trim();
            string razons = txtrazon.Text.Trim();
            string rfc = txtrfc.Text.Trim();
            string codigo_postal = txtcodigopostal.Text.Trim();

            // Verificar si los campos obligatorios están llenos
            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(apellidopat) ||
                string.IsNullOrEmpty(telefono))
            {
                MessageBox.Show("Por favor, complete todos los campos obligatorios.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Intentar registrar la empresa junto con la facturación
            bool resultado = Daos.registrarEmpresa(empresa, nombre, apellidopat, apellidomat, telefono, correo, domicilio, razons, rfc, codigo_postal);

            // Verificar si el registro fue exitoso
            if (resultado)
            {
                MessageBox.Show("El registro ha sido exitoso.", "Registro Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Limpiar los campos del formulario después de registrar
                txtempresa.Clear();
                txtnombre.Clear();
                txtapellidopat.Clear();
                txtapellidomat.Clear();
                txttelefono.Clear();
                txtcorreo.Clear();
                txtdomicilio.Clear();
                txtrazon.Clear();
                txtrfc.Clear();
                txtcodigopostal.Clear();
            }
            else
            {
                MessageBox.Show("Hubo un error al registrar la empresa.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnguardar_Click(object sender, EventArgs e)
        {
            try
            {

                // Verificar si el DataGridView está vacío
                if (dataGridView1.Rows.Count == 0 || (dataGridView1.Rows.Count == 1 && dataGridView1.Rows[0].IsNewRow))
                {
                    MessageBox.Show("No hay datos ingresados para generar la cotización.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

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

                // Variables para tipos de aceite
                string aceiteGarrafa = "";
                string aceiteTote = "";
                string aceiteTambor = "";

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
                        string producto = row.Cells["Producto"].Value?.ToString() ?? string.Empty;
                        string capacidadTexto = row.Cells["Litros / Kilos"].Value?.ToString() ?? "0";
                        string LorKTexto = row.Cells["Capacidad"].Value?.ToString()?.ToUpper() ?? string.Empty;  // Convertir a mayúsculas
                        string cantidadTexto = row.Cells["Cantidad"].Value?.ToString() ?? "0";
                        string tipoAceite = row.Cells["Tipo"].Value?.ToString() ?? string.Empty;

                        // Determinar la unidad de medida
                        string unidad = "";
                        if (LorKTexto.Contains("L"))
                        {
                            unidad = "L";
                        }
                        else if (LorKTexto.Contains("K"))
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
                string empresa = EmpresaSeleccionada ?? string.Empty;
                string persona = null;
                int idEmpresa = EmpresaIdSeleccionada.GetValueOrDefault();

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

                // Mostrar la ventana de carga y generar el PDF
                using (var loadingForm = new carga())
                {
                    loadingForm.Show();
                    await Task.Run(() =>
                    {
                        try
                        {
                            int idauto = 0;
                            // Registrar la compra en la base de datos
                            loadingForm.UpdateStatus("Guardando datos...", 30);

                            if (String.IsNullOrEmpty(empresa))
                            {
                                persona = txtempresa2.Text.Trim();
                                idauto = Daos.guardarCotizacion(idEmpresa, persona, fechacompra, aceiteGarrafa, garrafas, numGarrafas,
                                                   aceiteTote, totes, numTotes, aceiteTambor, tambores, numTambores, envio,
                                                   subtotal, iva, total);
                            }
                            else
                            {
                                idauto = Daos.guardarCotizacion(idEmpresa, empresa, fechacompra, aceiteGarrafa, garrafas, numGarrafas,
                                                   aceiteTote, totes, numTotes, aceiteTambor, tambores, numTambores, envio,
                                                   subtotal, iva, total);
                            }
                            // Crear el PDF
                            loadingForm.UpdateStatus("Generando PDF...", 60);
                            string pdfPath = CreatePdf(idauto, empresa, nombrec, apellidopatc, apellidomatc, telefono, correo, nombrevendedor, numerovendedor, correovendedor, aceiteGarrafa, garrafas, numGarrafas, aceiteTote, totes,
                                                       numTotes, aceiteTambor, tambores, numTambores, envio, subtotal, iva, total, fechacompra, condiciones);

                            loadingForm.UpdateStatus("Guardando PDF...", 100);
                            SavePdfToUserSelectedPath(pdfPath);
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
            txtempresa2.Clear();
            dataGridView1.Rows.Clear();
            nombrevendedor = null;
            numerovendedor = null;
            correovendedor = null;
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
        public string CreatePdf(int folio, string empresa, string nombrec, string apellidopatc, string apellidomatc, string telefono, string correo, string nombreVendedor, string numeroVendedor, string correoVendedor, string aceiteGarrafa, string garrafas, string numGarrafas,
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

                    infoTable.AddCell(new PdfPCell(new Phrase("Nombre: " + nombrec + " " + apellidopatc + " " + apellidomatc, FontFactory.GetFont(FontFactory.HELVETICA, 10)))
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

                            string descripcion = $"{producto.ToUpper()} DE ACEITE DE RICINO GRADO {tipoAceite.ToUpper()} EN PRESENTACION DE {LorKNum} {capacidadTexto.ToUpper()}";

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

        private void btnbuscarcliente_Click(object sender, EventArgs e)
        {
            string terminoBusqueda = txtbuscar.Text.Trim();

            resultadosbusqueda busqueda = new resultadosbusqueda();

            if (string.IsNullOrEmpty(terminoBusqueda))
            {
                busqueda.CargarEmpresas(Daos.mostrarEmpresas());
            }
            else
            {
                busqueda.CargarEmpresas(Daos.BuscarEmpresas(terminoBusqueda));
            }

            if (busqueda.ShowDialog() == DialogResult.OK)
            {
                EmpresaIdSeleccionada = busqueda.SelectedId;
                EmpresaSeleccionada = busqueda.SelectedEmpresa;
                nombrec = busqueda.SelectedNombre;
                apellidopatc = busqueda.SelectedApellidoPat;
                apellidomatc = busqueda.SelectedApellidoMat;
                telefono = busqueda.SelectedTelefono;
                correo = busqueda.SelectedCorreo;

                if (String.IsNullOrEmpty(EmpresaSeleccionada))
                {
                    txtempresa2.Text = nombrec + " " + apellidopatc + " " + apellidomatc;
                }
                else
                {
                    txtempresa2.Text = EmpresaSeleccionada;
                }
                empresa = EmpresaSeleccionada;
                nombrec = busqueda.SelectedNombre;
                apellidopatc = busqueda.SelectedApellidoPat;
                apellidomatc = busqueda.SelectedApellidoMat;
            }
            txtbuscar.Clear();
        }

        private void txtbuscar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;  // Evitar el sonido de la tecla Enter
                e.SuppressKeyPress = true;  // Evitar el efecto de la tecla Enter

                // Simular el clic en el botón de búsqueda
                btnbuscarcliente.PerformClick();
            }
        }

        private void btnañadir_Click(object sender, EventArgs e)
        {
            // Validar que los campos no estén vacíos y que los índices seleccionados sean válidos
            if (comboBoxProducto.SelectedIndex == 0 || comboBoxCapacidad.SelectedIndex == 0 ||
                string.IsNullOrWhiteSpace(txtlitros.Text) || comboBoxAceite.SelectedIndex == 0 ||
                string.IsNullOrWhiteSpace(txtnumprod.Text) || string.IsNullOrWhiteSpace(txtprecio.Text) || string.IsNullOrWhiteSpace(txtempresa2.Text))
            {
                MessageBox.Show("Por favor, complete todos los campos antes de añadir.");
                return;
            }

            // Recoger datos de los controles
            string producto = comboBoxProducto.Text; // Obtener el nombre completo del producto
            string capacidadSeleccionada = comboBoxCapacidad.Text;
            string cantidad = txtlitros.Text.Trim();
            string tipo = comboBoxAceite.Text;
            string numprod = txtnumprod.Text;
            string precioTexto = txtprecio.Text.Trim();

            // Validar que el precio sea un número válido
            decimal precio;
            if (!decimal.TryParse(precioTexto, out precio) || precio < 0)
            {
                MessageBox.Show("El precio debe ser un número válido mayor o igual a 0.");
                return;
            }

            // Crear una fila con los datos
            dataGridView1.Rows.Add(producto, capacidadSeleccionada, numprod, cantidad, tipo, precio);

            // Limpiar los controles para el siguiente registro
            comboBoxProducto.SelectedIndex = 0;
            comboBoxCapacidad.SelectedIndex = 0;
            txtlitros.Clear();
            txtprecio.Clear();
            comboBoxAceite.SelectedIndex = 0;
            txtnumprod.Clear();
            dateTimePickerCompra.Value = DateTime.Now;


        }

        private void btnguardarv_Click(object sender, EventArgs e)
        {
            // Obtener los valores de los TextBox
            string nombrev = txtnombrev.Text.Trim();
            string apellidopatv = txtapellidopatv.Text.Trim();
            string apellidomatv = txtapellidomatv.Text.Trim();
            string telefonov = txttelefonov.Text.Trim();
            string correov = txtcorreov.Text.Trim();

            // Verificar si los campos están llenos
            if (string.IsNullOrEmpty(nombrev) || string.IsNullOrEmpty(apellidopatv) || string.IsNullOrEmpty(telefonov) || string.IsNullOrEmpty(correov))
            {
                MessageBox.Show("Por favor, complete todos los campos obligatorios.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Llamar al método del DAO para guardar los datos
            bool resultado = Daos.InsertarVendedor(nombrev, apellidopatv, apellidomatv, telefonov, correov);

            if (resultado)
            {
                MessageBox.Show("Vendedor registrado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Limpiar los TextBox después de la inserción
                txtnombrev.Clear();
                txtapellidopatv.Clear();
                txtapellidomatv.Clear();
                txttelefonov.Clear();
                txtcorreov.Clear();
            }
            else
            {
                MessageBox.Show("Hubo un problema al registrar el vendedor.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Funciones del panel lateral
        private void linkLabel1_LinkClicked(object sender, EventArgs e)
        {
            registroemp.Show();
        }

        private void linkLabel2_LinkClicked(object sender, EventArgs e)
        {
            registroc.Show();
        }

        private void historialCompra_LinkClicked(object sender, EventArgs e)
        {
            historial.Show();
        }

        private void linkLabel4_LinkClicked(object sender, EventArgs e)
        {
            ventasnc.Show();
        }

        private void ventasConcretadas_LinkClicked(object sender, EventArgs e)
        {
            ventasc.Show();
        }

        private void registroVendedores_LinkClicked(object sender, EventArgs e)
        {
            vendedor.Show();
        }

        private void ventas_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            // Verifica si hay formularios hijos abiertos
            foreach (Form form in Application.OpenForms)
            {
                if (form != this) // Omitir la ventana principal
                {
                    // Cancelar el cierre de la ventana principal
                    e.Cancel = true;
                    return;
                }
            }
        }

        //Icono menu | Funciones
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

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            linkLabel1_LinkClicked(sender, e);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            linkLabel2_LinkClicked(sender, e);
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            historialCompra_LinkClicked(sender, e);
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            linkLabel4_LinkClicked(sender, e);
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            ventasConcretadas_LinkClicked(sender, e);
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            registroVendedores_LinkClicked(sender, e);
        }

        // Método para inicializar el TextBox con el placeholder
        private void InicializarTextBoxConPlaceholder(TextBox txt, string placeholder)
        {
            txt.Text = placeholder;
            txt.ForeColor = Color.Gray;

            txt.Enter += (sender, e) =>
            {
                // Si el texto del TextBox es el placeholder, lo limpiamos
                if (txt.Text == placeholder)
                {
                    txt.Text = "";
                    txt.ForeColor = Color.Black; // Cambiamos el color a negro cuando se ingresa texto
                }
            };

            txt.Leave += (sender, e) =>
            {
                // Si el texto está vacío, volvemos a poner el placeholder
                if (string.IsNullOrWhiteSpace(txt.Text))
                {
                    txt.Text = placeholder;
                    txt.ForeColor = Color.Gray; // El color vuelve a gris cuando mostramos el placeholder
                }
            };
        }
        private void pictureBox2_MouseEnter(object sender, EventArgs e)
        {
            pictureBox2.Cursor = Cursors.Hand; // Cambiar a la mano apuntando
        }

        private void pictureBox3_MouseEnter(object sender, EventArgs e)
        {
            pictureBox3.Cursor = Cursors.Hand; // Cambiar a la mano apuntando
        }

        private void pictureBox4_MouseEnter(object sender, EventArgs e)
        {
            pictureBox4.Cursor = Cursors.Hand; // Cambiar a la mano apuntando
        }

        private void pictureBox5_MouseEnter(object sender, EventArgs e)
        {
            pictureBox5.Cursor = Cursors.Hand; // Cambiar a la mano apuntando
        }

        private void pictureBox6_MouseEnter(object sender, EventArgs e)
        {
            pictureBox6.Cursor = Cursors.Hand; // Cambiar a la mano apuntando
        }

        private void pictureBox7_MouseEnter(object sender, EventArgs e)
        {
            pictureBox7.Cursor = Cursors.Hand; // Cambiar a la mano apuntando
        }

        private void pictureBox2_MouseLeave(object sender, EventArgs e)
        {
            pictureBox2.Cursor = Cursors.Default; // Volver al cursor por defecto
        }

        private void pictureBox3_MouseLeave(object sender, EventArgs e)
        {
            pictureBox3.Cursor = Cursors.Default; // Volver al cursor por defecto
        }

        private void pictureBox4_MouseLeave(object sender, EventArgs e)
        {
            pictureBox4.Cursor = Cursors.Default; // Volver al cursor por defecto
        }

        private void pictureBox5_MouseLeave(object sender, EventArgs e)
        {
            pictureBox5.Cursor = Cursors.Default; // Volver al cursor por defecto
        }

        private void pictureBox6_MouseLeave(object sender, EventArgs e)
        {
            pictureBox6.Cursor = Cursors.Default; // Volver al cursor por defecto
        }

        private void pictureBox7_MouseLeave(object sender, EventArgs e)
        {
            pictureBox7.Cursor = Cursors.Default; // Volver al cursor por defecto
        }
    }
}

