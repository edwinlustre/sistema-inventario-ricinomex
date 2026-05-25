using appInventario.DAOS;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace appInventario.Forms
{
    public partial class resultadoscompracostales : Form
    {
        private Compras form;
        private daos Daos;
        private confirmacioncompra Confirmacion;
        public string autenticacion;
        private string tipo = "Costal";

        public resultadoscompracostales(Compras form)
        {
            this.form = form;
            this.FormClosing += resultadoscompracostales_FormClosing;

            InitializeComponent();
            Daos = new daos(); // Asegúrate de que Daos esté inicializado
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        public resultadoscompracostales()
        {
            InitializeComponent();
            Daos = new daos(); // Inicializa Daos aquí también
        }

        private void resultadoscompracostales_FormClosing(object sender, FormClosingEventArgs e)
        {
            // No limpiar el DataGridView aquí
            e.Cancel = true; // Evitar que se cierre, solo ocultar
            this.Hide();
            form?.Show(); // Mostrar el formulario principal nuevamente
        }

        private void resultadoscompracostales_Load(object sender, EventArgs e)
        {
            LoadPurchases();
        }

        public void LoadPurchases()
        {
            try
            {
                // Obtiene los datos de la base de datos
                DataTable dt = Daos.obtenerCompraCostales();
                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show("No se encontraron datos.", "Información");
                    return;
                }

                // Configura el DataGridView
                dataGridView1.DataSource = dt;
                dataGridView1.ReadOnly = true;
                dataGridView1.AllowUserToAddRows = false;

                // Elimina la columna de botones si existe
                if (dataGridView1.Columns.Contains("pagar"))
                {
                    dataGridView1.Columns.Remove("pagar");
                }

                // Asegúrate de que la columna "pagar" exista y esté visible
                DataGridViewButtonColumn payButtonColumn = new DataGridViewButtonColumn
                {
                    Name = "pagar",
                    HeaderText = "Acción",
                    Text = "Pagar",
                    UseColumnTextForButtonValue = true,
                    Width = 100
                };
                dataGridView1.Columns.Add(payButtonColumn);

                // Aplica formato a las celdas y configura el botón
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    string estadoPago = row.Cells["estado_pago"].Value?.ToString();
                    string fechaEntradaStr = row.Cells["fecha_entrada"].Value?.ToString();

                    if (string.IsNullOrEmpty(fechaEntradaStr))
                    {
                        // Manejo de errores de fecha vacía
                        continue;
                    }

                    if (DateTime.TryParseExact(fechaEntradaStr, "dd-MM-yyyy", null, DateTimeStyles.None, out DateTime fechaEntrada))
                    {
                        TimeSpan diferencia = DateTime.Now - fechaEntrada;

                        // Configura el estado de pago y formato de celdas
                        if (estadoPago == "Pendiente" && diferencia.TotalDays > 6)
                        {
                            // Verificar que las columnas "precio" e "importe" no estén vacías
                            var precioCell = row.Cells["precio"].Value;
                            var importeCell = row.Cells["importe"].Value;

                            bool precioTieneValor = precioCell != null && !string.IsNullOrEmpty(precioCell.ToString());
                            bool importeTieneValor = importeCell != null && !string.IsNullOrEmpty(importeCell.ToString());

                            if (precioTieneValor && importeTieneValor)
                            {
                                row.Cells["estado_pago"].Style.BackColor = Color.Red; // Cambia el color a lo que prefieras
                                row.Cells["estado_pago"].Value = "Pagar";
                            }
                            else
                            {
                                row.Cells["estado_pago"].Style.BackColor = Color.Red;
                                row.Cells["estado_pago"].Value = "Pendiente";
                            }
                        }
                        else if (estadoPago == "Pendiente")
                        {
                            row.Cells["estado_pago"].Style.BackColor = Color.Yellow;
                        }
                        else if (estadoPago == "Pagar" && diferencia.TotalDays > 6)
                        {
                            row.Cells["estado_pago"].Style.BackColor = Color.Red;
                            row.Cells["estado_pago"].Value = "Pagar";
                        }
                        else if (estadoPago == "Pagar")
                        {
                            row.Cells["estado_pago"].Style.BackColor = Color.Yellow;
                        }
                        else if (estadoPago == "Pagado")
                        {
                            row.Cells["estado_pago"].Style.BackColor = Color.Green;

                            // Oculta el botón en lugar de cambiar su valor
                            row.Cells["pagar"] = new DataGridViewTextBoxCell(); // Usa una celda de texto vacía
                        }
                        else
                        {
                            // Muestra el botón si el estado es "Pendiente" o "Vencido"
                            row.Cells["pagar"].Value = "Pendiente";
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Error al convertir la fecha de entrada: {fechaEntradaStr} en la fila {row.Index}", "Error");
                    }
                }
                dataGridView1.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Arial", 10, FontStyle.Bold);
                dataGridView1.DefaultCellStyle.Font = new System.Drawing.Font("Arial", 9);
                dataGridView1.Columns["id"].HeaderText = "ID";
                dataGridView1.Columns["id_productor"].HeaderText = "ID Productor";
                dataGridView1.Columns["folio"].HeaderText = "Folio";
                dataGridView1.Columns["nombre"].HeaderText = "Nombre";
                dataGridView1.Columns["apellidopat"].HeaderText = "Apellido Paterno";
                dataGridView1.Columns["apellidomat"].HeaderText = "Apellido Materno";
                dataGridView1.Columns["fecha_entrada"].HeaderText = "Fecha de Entrada";
                dataGridView1.Columns["localidad"].HeaderText = "Localidad";
                dataGridView1.Columns["telefono"].HeaderText = "Telefono";
                dataGridView1.Columns["cantidad_costales"].HeaderText = "Cantidad de costales";
                dataGridView1.Columns["kg"].HeaderText = "KG";
                dataGridView1.Columns["grano"].HeaderText = "Grano";
                dataGridView1.Columns["precio"].HeaderText = "Precio";
                dataGridView1.Columns["importe"].HeaderText = "Importe";
                dataGridView1.Columns["estado_pago"].HeaderText = "Estado de Pago";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos: " + ex.Message, "Error");
            }
        }

        private async void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["pagar"].Index && e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                string estadoPago = row.Cells["estado_pago"].Value?.ToString();

                // Verifica que el estado no sea ya "Pagado" antes de permitir la acción
                if (estadoPago == "Pagado" && autenticacion == "Compras")
                {
                    MessageBox.Show("Este producto ya está pagado.", "Información");
                    return;
                }

                // Verifica que el estado sea "Pendiente" y que solo el administrador pueda modificar datos
                else if (estadoPago == "Pendiente" && autenticacion == "Administrador")
                {
                    string kg = row.Cells["kg"].Value?.ToString();
                    int id = Convert.ToInt32(row.Cells["id"].Value);
                    int idp = Convert.ToInt32(row.Cells["id_productor"].Value);

                    // Mostrar la ventana de confirmación como un formulario modal
                    Confirmacion = new confirmacioncompra(kg, id, idp);
                    var result = Confirmacion.ShowDialog();

                    // Después de cerrar la ventana de confirmación, puedes continuar con el código
                    if (result == DialogResult.OK)
                    {
                        LoadPurchases();
                    }
                }

                // Verifica que el estado sea "Pendiente" y si el prefil compras presiona el boton pagar
                else if (estadoPago == "Pendiente" && autenticacion == "Compras")
                {
                    MessageBox.Show("Aun no se ha colocado la información para pagar, intentalo más tarde.", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                // Verifica que el estado sea "Pagar" y si el prefil Administrador presiona el boton pagar
                else if (estadoPago == "Pagar" && autenticacion == "Administrador")
                {
                    MessageBox.Show("El precio ya ha sido colocado", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                // Verifica que el estado sea "Pagar" y que solo el perfil Compras pueda pagar
                else if (estadoPago == "Pagar" && autenticacion == "Compras")
                {
                    // Preguntar al usuario si desea proceder con el pago
                    DialogResult result = MessageBox.Show("¿Deseas realizar el pago ahora?", "Confirmar pago", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    // Si el usuario elige "Yes", proceder con el pago
                    if (result == DialogResult.Yes)
                    {

                        int id = Int32.Parse(row.Cells["id"].Value?.ToString());
                        int idp = Int32.Parse(row.Cells["id_productor"].Value?.ToString());
                        string nombre = row.Cells["nombre"].Value?.ToString();
                        string apellidopat = row.Cells["apellidopat"].Value?.ToString();
                        string apellidomat = row.Cells["apellidomat"].Value?.ToString();
                        string hectareas = Daos.ObtenerHectareaProductor(idp);
                        string telefono = row.Cells["telefono"].Value?.ToString();
                        string localidad = row.Cells["localidad"].Value?.ToString();
                        string grano = row.Cells["grano"].Value?.ToString();
                        string precio = row.Cells["precio"].Value?.ToString();
                        string importe = row.Cells["importe"].Value?.ToString();
                        string fecha = row.Cells["fecha_entrada"].Value?.ToString();
                        string kg = row.Cells["kg"].Value?.ToString();

                        using (var loadingForm = new carga())
                        {
                            loadingForm.Show();
                            await Task.Run(() =>
                            {
                                try
                                {
                                    // Registrar la compra en la base de datos
                                    loadingForm.UpdateStatus("Guardando datos...", 30);
                                    // Actualiza el estado de pago
                                    pagoFinal(precio, importe, id, "Pagado");

                                    // Crear el PDF
                                    loadingForm.UpdateStatus("Generando PDF...", 60);
                                    // Llamar al método CreatePdf pasando los datos obtenidos
                                    string pdfPath = CreatePdf(id, nombre, apellidopat, apellidomat, hectareas, telefono, localidad, grano, precio, importe, fecha, kg, tipo);

                                    // Mostrar el diálogo para seleccionar la ubicación del archivo PDF
                                    loadingForm.UpdateStatus("Guardando PDF...", 90);
                                    SavePdfToUserSelectedPath(pdfPath);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Error al registrar la compra: " + ex.Message, "Error");
                                }
                            });
                            loadingForm.Close();
                            LoadPurchases();
                        }
                    }
                    else
                    {
                        // Si el usuario elige "No", no se realiza el pago
                        MessageBox.Show("El pago ha sido cancelado.", "Pago cancelado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        /* --- Funciones para crear PDFS --- */
        public string CreatePdf(int idauto, string nombre_, string apellidopat_, string apellidomat_, string hectareas_, string telefono, string localidad_, string grano, string precio, string importe, string fecha, string kg, string tipo)
        {
            string pdfPath = Path.Combine(Path.GetTempPath(), "Compra_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".pdf");

            try
            {
                using (FileStream fs = new FileStream(pdfPath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    using (Document doc = new Document(PageSize.A4))
                    {
                        PdfWriter.GetInstance(doc, fs);
                        doc.Open();

                        // Crear la tabla para agrupar los elementos en una fila
                        PdfPTable headerTable = new PdfPTable(3)
                        {
                            WidthPercentage = 100
                        };
                        headerTable.SetWidths(new float[] { 1f, 1f, 1f });

                        // Texto "Nota de Remisión de Compra de Higuerilla" con fondo negro y texto blanco
                        PdfPCell titleCell = new PdfPCell(new Phrase("Nota de Remisión de Compra de Higuerilla", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18, BaseColor.WHITE)))
                        {
                            BackgroundColor = BaseColor.BLACK,
                            Border = iTextSharp.text.Rectangle.NO_BORDER,
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            VerticalAlignment = Element.ALIGN_MIDDLE
                        };
                        headerTable.AddCell(titleCell);

                        // Logotipo centrado
                        using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("appInventario.Resources.ricinomex.png"))
                        {
                            if (stream != null)
                            {
                                iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(stream);
                                img.ScaleToFit(100f, 100f);
                                img.Alignment = Element.ALIGN_CENTER;

                                PdfPCell logoCell = new PdfPCell(img)
                                {
                                    Border = iTextSharp.text.Rectangle.NO_BORDER,
                                    HorizontalAlignment = Element.ALIGN_CENTER,
                                    VerticalAlignment = Element.ALIGN_MIDDLE
                                };
                                headerTable.AddCell(logoCell);
                            }
                            else
                            {
                                MessageBox.Show("No se encontró la imagen de la empresa.");
                                headerTable.AddCell(""); // Agrega una celda vacía en caso de que no haya logotipo
                            }
                        }

                        // Folio
                        string year = DateTime.Now.Year.ToString();
                        string folio = $"{year}-{tipo.Substring(0, 1).ToUpper()}{idauto}";
                        PdfPCell folioCell = new PdfPCell(new Phrase($"Folio: {folio}", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14, BaseColor.RED)))
                        {
                            Border = iTextSharp.text.Rectangle.NO_BORDER,
                            HorizontalAlignment = Element.ALIGN_RIGHT,
                            VerticalAlignment = Element.ALIGN_MIDDLE
                        };
                        headerTable.AddCell(folioCell);

                        // Añadir la tabla de encabezado al documento
                        doc.Add(headerTable);

                        // Espacio después del encabezado
                        doc.Add(new Paragraph("\n"));

                        // Dirección y fecha
                        string address = "Calle Higuerilla, Monte del Toro, Heroica Ciudad de Ejutla de Crespo, Oaxaca";
                        Paragraph dateParagraph = new Paragraph($"{address}, a {DateTime.Now:dd 'de' MMMM 'de' yyyy}", FontFactory.GetFont(FontFactory.HELVETICA, 12))
                        {
                            Alignment = Element.ALIGN_CENTER
                        };
                        doc.Add(dateParagraph);

                        // Espacio antes de la tabla de datos
                        doc.Add(new Paragraph("\n"));

                        // Tabla para los datos de la compra
                        PdfPTable dataTable = new PdfPTable(2)
                        {
                            WidthPercentage = 100
                        };

                        PdfPCell nombreCell = new PdfPCell(new Phrase($"Nombre: {nombre_} {apellidopat_} {apellidomat_}", FontFactory.GetFont(FontFactory.HELVETICA, 12)))
                        {
                            Border = iTextSharp.text.Rectangle.BOX
                        };
                        PdfPCell localidadCell = new PdfPCell(new Phrase($"Poblacion: {localidad_}", FontFactory.GetFont(FontFactory.HELVETICA, 12)))
                        {
                            Border = iTextSharp.text.Rectangle.BOX
                        };
                        PdfPCell telefonoCell = new PdfPCell(new Phrase($"Teléfono: {telefono}", FontFactory.GetFont(FontFactory.HELVETICA, 12)))
                        {
                            Border = iTextSharp.text.Rectangle.BOX
                        };
                        PdfPCell hectareasCell = null;
                        if (!String.IsNullOrEmpty(hectareas_))
                        {
                            hectareasCell = new PdfPCell(new Phrase($"Hectáreas Sembradas: {hectareas_}", FontFactory.GetFont(FontFactory.HELVETICA, 12)))
                            {
                                Border = iTextSharp.text.Rectangle.BOX
                            };
                        }

                        dataTable.AddCell(nombreCell);
                        dataTable.AddCell(localidadCell);
                        dataTable.AddCell(telefonoCell);
                        if (!String.IsNullOrEmpty(hectareas_))
                        {
                            dataTable.AddCell(hectareasCell);
                        }
                        doc.Add(dataTable);

                        PdfPTable dataDescripcion = new PdfPTable(1)
                        {
                            WidthPercentage = 100
                        };

                        // Determinar la descripción basada en el tipo
                        string descripcion;
                        switch (tipo)
                        {
                            case "Grano":
                                descripcion = "Grano de higuerilla";
                                break;
                            case "Semilla":
                                descripcion = "Semilla de higuerilla";
                                break;
                            case "Costal":
                                descripcion = "Costal(es) de higuerilla";
                                break;
                            default:
                                descripcion = "Descripción no disponible";
                                break;
                        }

                        PdfPCell descripcionCell = new PdfPCell(new Phrase($"Descripción: {descripcion}", FontFactory.GetFont(FontFactory.HELVETICA, 12)))
                        {
                            Border = iTextSharp.text.Rectangle.BOX
                        };

                        dataDescripcion.AddCell(descripcionCell); // Añadir descripción a la tabla

                        // Añadir la tabla de descripción al documento
                        doc.Add(dataDescripcion);

                        // Tabla para precio, importe y cantidad
                        PdfPTable priceTable = new PdfPTable(3)
                        {
                            WidthPercentage = 100
                        };

                        PdfPCell precioCell = new PdfPCell(new Phrase($"Precio Kg: {precio}", FontFactory.GetFont(FontFactory.HELVETICA, 12, BaseColor.BLACK)))
                        {
                            Border = iTextSharp.text.Rectangle.BOX
                        };
                        PdfPCell importeCell = new PdfPCell(new Phrase($"Importe Total: {importe}", FontFactory.GetFont(FontFactory.HELVETICA, 12, BaseColor.RED)))
                        {
                            Border = iTextSharp.text.Rectangle.BOX
                        };
                        PdfPCell cantidadCell = new PdfPCell(new Phrase($"Cantidad Kg: {kg}", FontFactory.GetFont(FontFactory.HELVETICA, 12, BaseColor.RED)))
                        {
                            Border = iTextSharp.text.Rectangle.BOX
                        };

                        priceTable.AddCell(precioCell);
                        priceTable.AddCell(importeCell);
                        priceTable.AddCell(cantidadCell);

                        doc.Add(priceTable);

                        // Espacio antes de las firmas
                        doc.Add(new Paragraph("\n\n\n"));

                        // Tabla para las firmas con espacio en blanco para firma
                        PdfPTable signatureTable = new PdfPTable(3)
                        {
                            WidthPercentage = 100
                        };

                        PdfPCell signatureCell1 = new PdfPCell(new Phrase("Recibí de conformidad mi pago", FontFactory.GetFont(FontFactory.HELVETICA, 12)))
                        {
                            Border = iTextSharp.text.Rectangle.BOX,
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            VerticalAlignment = Element.ALIGN_MIDDLE
                        };
                        PdfPCell signatureCell2 = new PdfPCell(new Phrase("Compró", FontFactory.GetFont(FontFactory.HELVETICA, 12)))
                        {
                            Border = iTextSharp.text.Rectangle.BOX,
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            VerticalAlignment = Element.ALIGN_MIDDLE
                        };
                        PdfPCell signatureCell3 = new PdfPCell(new Phrase("Supervisa Agronomía", FontFactory.GetFont(FontFactory.HELVETICA, 12)))
                        {
                            Border = iTextSharp.text.Rectangle.BOX,
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            VerticalAlignment = Element.ALIGN_MIDDLE
                        };

                        signatureTable.AddCell(signatureCell1);
                        signatureTable.AddCell(signatureCell2);
                        signatureTable.AddCell(signatureCell3);

                        // Espacio para firmas en blanco
                        PdfPCell blankCell1 = new PdfPCell(new Phrase("\n\n", FontFactory.GetFont(FontFactory.HELVETICA, 12)))
                        {
                            Border = iTextSharp.text.Rectangle.BOX,
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            VerticalAlignment = Element.ALIGN_MIDDLE
                        };
                        PdfPCell blankCell2 = new PdfPCell(new Phrase("\n\n", FontFactory.GetFont(FontFactory.HELVETICA, 12)))
                        {
                            Border = iTextSharp.text.Rectangle.BOX,
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            VerticalAlignment = Element.ALIGN_MIDDLE
                        };
                        PdfPCell blankCell3 = new PdfPCell(new Phrase("\n\n", FontFactory.GetFont(FontFactory.HELVETICA, 12)))
                        {
                            Border = iTextSharp.text.Rectangle.BOX,
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            VerticalAlignment = Element.ALIGN_MIDDLE
                        };

                        signatureTable.AddCell(blankCell1);
                        signatureTable.AddCell(blankCell2);
                        signatureTable.AddCell(blankCell3);

                        doc.Add(signatureTable);

                        // Copia en la misma página
                        doc.Add(new Paragraph("\n\n\n\n"));
                        doc.Add(headerTable); // Reutilizar el mismo encabezado para la copia
                        doc.Add(new Paragraph("\n"));
                        doc.Add(dateParagraph);
                        doc.Add(new Paragraph("\n"));
                        doc.Add(dataTable);
                        doc.Add(dataDescripcion); // Reutilizar la tabla de descripción
                        doc.Add(priceTable);
                        doc.Add(new Paragraph("\n\n\n"));
                        doc.Add(signatureTable);

                        doc.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al crear el PDF: " + ex.Message);
            }
            return pdfPath;
        }

        private void SavePdfToUserSelectedPath(string tempPdfPath)
        {
            // Llamar a SaveFileDialog en el hilo principal
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => SavePdfToUserSelectedPath(tempPdfPath)));
                return;
            }

            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*";
                saveFileDialog.Title = "Guardar archivo PDF";
                saveFileDialog.FileName = "Compra_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".pdf";

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

        public void pagoFinal(string precio, string importe, int id, string estatus)
        {
            Daos.UpdatePaymentStatus(id, precio, importe, estatus);
        }
    }
}
