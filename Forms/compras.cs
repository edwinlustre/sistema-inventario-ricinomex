using appInventario.DAOS;
using appInventario.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace appInventario.Forms
{
    public partial class Compras : Form
    {
        private actualizar act;
        private resultadosbusqueda resultsForm;
        private resultadoscompracostales rescompra;
        private int idproductor;
        private daos Daos;
        private daosadministrador DaosA;
        private carga Carga;
        public string autenticacion;
        private IFormToShow originalForm;

        public Compras(IFormToShow form)
        {
            InitializeComponent();
            this.originalForm = form;
            this.FormClosing += new FormClosingEventHandler(compras_FormClosing);
            dateTimePicker.Format = DateTimePickerFormat.Custom;
            dateTimePicker.CustomFormat = "dd-MM-yyyy";

            //Instancias
            Daos = new daos();
            DaosA = new daosadministrador();
            act = new actualizar(this);
            Carga = new carga();
            rescompra = new resultadoscompracostales(this);
            resultsForm = new resultadosbusqueda(this);

            //Inicializacion
            comboBoxTipo.SelectedIndex = 0;
            comboBoxGrano.SelectedIndex = 0;
            comboBoxGrano2.SelectedIndex = 0;
            comboBoxGrano3.SelectedIndex = 0;

            //Calcula el importe
            txtkg1.TextChanged += new EventHandler(precioSemilla_TextChanged);
            txtprecio1.TextChanged += new EventHandler(precioSemilla_TextChanged);
            txtkg2.TextChanged += new EventHandler(precioGrano_TextChanged);
            txtprecio2.TextChanged += new EventHandler(precioGrano_TextChanged);
            txtprecio1.Enabled = false;
            txtprecio2.Enabled = false;
            dateTimePicker.Enabled = false;
            dateTimePickerGrano.Enabled = false;
            dateTimePickerSemilla.Enabled = false;
            dateTimePickerCostal.Enabled = false;

        }

        public Compras()
        {
            InitializeComponent();
        }

        public void InicializarAutenticacion(string area)
        {
            this.autenticacion = area;
        }

        private void precioSemilla_TextChanged(object sender, EventArgs e)
        {
            // Obtener los valores de los TextBox
            string text1 = txtkg1.Text.Trim();
            string text2 = txtprecio1.Text.Trim();

            // Verificar si ambos TextBox contienen datos
            if (!string.IsNullOrEmpty(text1) && !string.IsNullOrEmpty(text2))
            {
                if (decimal.TryParse(text1, out decimal value1) && decimal.TryParse(text2, out decimal value2))
                {
                    // Realizar la multiplicación y redondear a 2 decimales
                    decimal result = Math.Round(value1 * value2, 2);

                    // Mostrar el resultado en txtimporte1
                    txtimporte1.Text = result.ToString();
                }
                else
                {
                    // Mostrar un mensaje de error o manejar el caso de entradas no válidas
                    txtimporte1.Text = "Entrada no válida";
                }
            }
            else
            {
                // Limpiar el resultado si uno de los TextBox está vacío
                txtimporte1.Text = string.Empty;
            }
        }

        private void precioGrano_TextChanged(object sender, EventArgs e)
        {
            // Obtener los valores de los TextBox
            string text1 = txtkg2.Text.Trim();
            string text2 = txtprecio2.Text.Trim();

            // Verificar si ambos TextBox contienen datos
            if (!string.IsNullOrEmpty(text1) && !string.IsNullOrEmpty(text2))
            {
                if (decimal.TryParse(text1, out decimal value1) && decimal.TryParse(text2, out decimal value2))
                {
                    // Realizar la multiplicación y redondear a 2 decimales
                    decimal result = Math.Round(value1 * value2, 2);

                    // Mostrar el resultado en txtimporte1
                    txtimporte2.Text = result.ToString();
                }
                else
                {
                    // Mostrar un mensaje de error o manejar el caso de entradas no válidas
                    txtimporte2.Text = "Entrada no válida";
                }
            }
            else
            {
                // Limpiar el resultado si uno de los TextBox está vacío
                txtimporte2.Text = string.Empty;
            }
        }

        private void compras_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            GC.Collect();
            originalForm?.ShowForm(); // Usa el método de la interfaz
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
                            Border = Rectangle.NO_BORDER,
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
                                    Border = Rectangle.NO_BORDER,
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
                            Border = Rectangle.NO_BORDER,
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
                            Border = Rectangle.BOX
                        };
                        PdfPCell localidadCell = new PdfPCell(new Phrase($"Poblacion: {localidad_}", FontFactory.GetFont(FontFactory.HELVETICA, 12)))
                        {
                            Border = Rectangle.BOX
                        };
                        PdfPCell telefonoCell = new PdfPCell(new Phrase($"Teléfono: {telefono}", FontFactory.GetFont(FontFactory.HELVETICA, 12)))
                        {
                            Border = Rectangle.BOX
                        };
                        PdfPCell hectareasCell = null;
                        if (!String.IsNullOrEmpty(hectareas_))
                        {
                            hectareasCell = new PdfPCell(new Phrase($"Hectáreas Sembradas: {hectareas_}", FontFactory.GetFont(FontFactory.HELVETICA, 12)))
                            {
                                Border = Rectangle.BOX
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
                            Border = Rectangle.BOX
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
                            Border = Rectangle.BOX
                        };
                        PdfPCell importeCell = new PdfPCell(new Phrase($"Importe Total: {importe}", FontFactory.GetFont(FontFactory.HELVETICA, 12, BaseColor.RED)))
                        {
                            Border = Rectangle.BOX
                        };
                        PdfPCell cantidadCell = new PdfPCell(new Phrase($"Cantidad Kg: {kg}", FontFactory.GetFont(FontFactory.HELVETICA, 12, BaseColor.RED)))
                        {
                            Border = Rectangle.BOX
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
                            Border = Rectangle.BOX,
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            VerticalAlignment = Element.ALIGN_MIDDLE
                        };
                        PdfPCell signatureCell2 = new PdfPCell(new Phrase("Compró", FontFactory.GetFont(FontFactory.HELVETICA, 12)))
                        {
                            Border = Rectangle.BOX,
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            VerticalAlignment = Element.ALIGN_MIDDLE
                        };
                        PdfPCell signatureCell3 = new PdfPCell(new Phrase("Supervisa Agronomía", FontFactory.GetFont(FontFactory.HELVETICA, 12)))
                        {
                            Border = Rectangle.BOX,
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            VerticalAlignment = Element.ALIGN_MIDDLE
                        };

                        signatureTable.AddCell(signatureCell1);
                        signatureTable.AddCell(signatureCell2);
                        signatureTable.AddCell(signatureCell3);

                        // Espacio para firmas en blanco
                        PdfPCell blankCell1 = new PdfPCell(new Phrase("\n\n", FontFactory.GetFont(FontFactory.HELVETICA, 12)))
                        {
                            Border = Rectangle.BOX,
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            VerticalAlignment = Element.ALIGN_MIDDLE
                        };
                        PdfPCell blankCell2 = new PdfPCell(new Phrase("\n\n", FontFactory.GetFont(FontFactory.HELVETICA, 12)))
                        {
                            Border = Rectangle.BOX,
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            VerticalAlignment = Element.ALIGN_MIDDLE
                        };
                        PdfPCell blankCell3 = new PdfPCell(new Phrase("\n\n", FontFactory.GetFont(FontFactory.HELVETICA, 12)))
                        {
                            Border = Rectangle.BOX,
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

        /* ---  Compra de Grano --- */

        private async void btnregistrar_Click(object sender, EventArgs e)
        {
            int id = idproductor;
            string nombre_ = txtnombre1.Text.Trim();
            string apellidopat_ = txtapellidopat1.Text.Trim();
            string apellidomat_ = txtapellidomat1.Text.Trim();
            string hectareas_ = txthectareas1.Text.Trim();
            string telefono = txttelefono1.Text.Trim();
            string localidad_ = txtlocalidad1.Text.Trim();
            string grano = comboBoxGrano.Text;
            string precio = txtprecio1.Text.Trim();
            string importe = txtimporte1.Text.Trim();
            string fecha = dateTimePickerGrano.Value.ToString("dd-MM-yyyy"); // Formato de fecha ajustado para SQL
            string kg = txtkg1.Text.Trim();
            string tipo = "Grano";

            if (string.IsNullOrEmpty(nombre_) || string.IsNullOrEmpty(apellidopat_) || comboBoxGrano.SelectedIndex == 0 ||
                string.IsNullOrEmpty(hectareas_) || string.IsNullOrEmpty(telefono) || string.IsNullOrEmpty(localidad_) ||
                string.IsNullOrEmpty(grano) || string.IsNullOrEmpty(precio) || string.IsNullOrEmpty(importe) || string.IsNullOrEmpty(fecha))
            {
                MessageBox.Show("Por favor, completa todos los campos.", "Advertencia");
                return;
            }

            using (var loadingForm = new carga())
            {
                loadingForm.Show();
                await Task.Run(() =>
                {
                    try
                    {
                        // Registrar la compra en la base de datos
                        loadingForm.UpdateStatus("Guardando datos...", 30);
                        int idauto = Daos.RegistrarCompraGrano(id, nombre_, apellidopat_, apellidomat_, hectareas_, telefono, localidad_, grano, precio, importe, fecha, kg);

                        // Crear el PDF
                        loadingForm.UpdateStatus("Generando PDF...", 60);
                        string pdfPath = CreatePdf(idauto, nombre_, apellidopat_, apellidomat_, hectareas_, telefono, localidad_, grano, precio, importe, fecha, kg, tipo);

                        // Mostrar el diálogo para seleccionar la ubicación del archivo PDF
                        loadingForm.UpdateStatus("Guardando PDF...", 90);
                        SavePdfToUserSelectedPath(pdfPath);

                    }
                    catch (PdfException pdfEx)
                    {
                        MessageBox.Show("Error al generar el PDF: " + pdfEx.Message, "Error");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al registrar la compra: " + ex.Message, "Error");
                    }
                });
                loadingForm.Close();
            }

            // Limpiar los campos después de registrar
            txtnombre1.Clear();
            txtapellidopat1.Clear();
            txtapellidomat1.Clear();
            txthectareas1.Clear();
            txttelefono1.Clear();
            txtlocalidad1.Clear();
            comboBoxGrano.SelectedIndex = 0;
            txtimporte1.Clear();
            txtkg1.Clear();
            dateTimePickerGrano.Value = DateTime.Now;
        }

        private void txtbuscarproductorg_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Verificar si la tecla presionada es Enter
            if (e.KeyChar == (char)Keys.Enter)
            {
                // Suprimir el sonido "ding"
                e.Handled = true;
                btnbuscarproductorg.PerformClick();
            }
        }

        private void btnbuscarproductorg_Click(object sender, EventArgs e)
        {
            // Crear una instancia de la ventana de carga
            Carga.StartPosition = FormStartPosition.CenterScreen;
            Carga.Show();

            // Ejecutar la búsqueda en un hilo separado para evitar el bloqueo de la UI
            Task.Run(() =>
            {
                DataTable resultsTable;
                string searchTerm = txtbuscarproductorg.Text.Trim();

                try
                {
                    // Actualizar el formulario de carga para mostrar "Buscando..." y progreso del 30%
                    this.Invoke((MethodInvoker)delegate
                    {
                        Carga.UpdateStatus("Buscando productores, por favor espere...", 30);
                    });

                    // Llamar al método de búsqueda en DAO dependiendo si el término de búsqueda está vacío
                    if (string.IsNullOrEmpty(searchTerm))
                    {
                        // Obtener todos los productores
                        resultsTable = Daos.ObtenerTodosProductores();
                    }
                    else
                    {
                        // Buscar productores específicos
                        resultsTable = Daos.SearchProducers(searchTerm);
                    }

                    // Configurar el delegado para procesar los datos seleccionados
                    this.Invoke((MethodInvoker)delegate
                    {
                        // Crear una instancia de resultadosbusqueda
                        resultadosbusqueda resultsForm = new resultadosbusqueda(this);

                        // Cargar los resultados en el formulario
                        resultsForm.LoadResults(resultsTable);

                        // Configurar el delegado para procesar los datos seleccionados
                        resultsForm.SetDataProcessor((nombre, apellidopat, apellidomat, localidad, telefono, hectareas, id) =>
                        {
                            txtnombre1.Text = nombre;
                            txtapellidopat1.Text = apellidopat;
                            txtapellidomat1.Text = apellidomat;
                            txttelefono1.Text = telefono;
                            txtlocalidad1.Text = localidad;
                            txthectareas1.Text = hectareas;
                            idproductor = id;
                        });

                        // Mostrar el formulario de resultados
                        resultsForm.Show();
                    });

                    // Actualizar el formulario de carga para mostrar el progreso completo
                    this.Invoke((MethodInvoker)delegate
                    {
                        Carga.UpdateStatus("Búsqueda completada.", 100);
                        Carga.Hide(); // Cerrar el formulario de carga
                    });
                }
                catch (Exception ex)
                {
                    // Mostrar el error en el hilo principal
                    this.Invoke((MethodInvoker)delegate
                    {
                        MessageBox.Show("Error al buscar los productores: " + ex.Message, "Error");
                        Carga.Hide(); // Cerrar el formulario de carga incluso en caso de error
                    });
                }
            });
        }

        private void Compras_Load(object sender, EventArgs e)
        {
            // Inicializa el TextBox con un placeholder
            InicializarTextBoxConPlaceholder(txtbuscarproductorg, " Buscar al productor");
            InicializarTextBoxConPlaceholder(txtbuscarproductors, " Buscar al productor");
            InicializarTextBoxConPlaceholder(txtbuscarproductorc, " Buscar al productor");
        }

        /* --- Registro del Productor --- */
        private void btnaceptar_Click(object sender, EventArgs e)
        {
            string nombre = txtnombre.Text.Trim();
            string apellidopat = txtapellidopat.Text.Trim();
            string apellidomat = txtapellidomat.Text.Trim();
            string curp = txtcurp.Text.Trim();
            string telefono = txttelefono.Text.Trim();
            string municipio = txtmunicipio.Text.Trim();
            string localidad = txtlocalidad.Text.Trim();
            string tipo = comboBoxTipo.Text.Trim();
            string hectareas = txthectareas.Text.Trim();
            string fecha = dateTimePicker.Value.ToString("dd-MM-yyyy");

            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(apellidopat) || string.IsNullOrEmpty(telefono) || string.IsNullOrEmpty(municipio) ||
                string.IsNullOrEmpty(localidad) || comboBoxTipo.SelectedIndex == 0 || string.IsNullOrEmpty(hectareas) ||
                string.IsNullOrEmpty(fecha))
            {
                MessageBox.Show("Por favor, completa todos los campos.", "Advertencia");
                return;
            }

            Productor productor = new Productor(nombre, apellidopat, apellidomat, curp, municipio, localidad, tipo, hectareas, fecha, telefono);


            // Llamar a DAO para guardar en la base de datos
            try
            {
                Daos.guardarProductor(productor);
                MessageBox.Show("Productor guardado exitosamente.", "Éxito");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar el productor: " + ex.Message, "Error");
            }

            txtnombre.Clear();
            txtapellidopat.Clear();
            txtapellidomat.Clear();
            txtcurp.Clear();
            txttelefono.Clear();
            txtmunicipio.Clear();
            txtlocalidad.Clear();
            comboBoxTipo.SelectedIndex = 0;
            txthectareas.Clear();
            dateTimePicker.Value = DateTime.Now;
        }

        private void btnactualizar_Click(object sender, EventArgs e)
        {
            this.Hide();
            act.Show();
        }

        /* --Compra de costales ---*/

        private void button2_Click(object sender, EventArgs e)
        {
            Carga.StartPosition = FormStartPosition.CenterScreen;
            Carga.Show();

            // Obtener el término de búsqueda del TextBox
            string searchTerm = txtbuscarproductorc.Text.Trim();

            // Ejecutar la búsqueda en un hilo separado para evitar el bloqueo de la UI
            Task.Run(() =>
            {
                DataTable resultsTable;

                try
                {
                    // Actualizar el formulario de carga para mostrar "Buscando..." y progreso del 30%
                    this.Invoke((MethodInvoker)delegate
                    {
                        Carga.UpdateStatus("Buscando productores, por favor espere...", 30);
                    });

                    // Llamar al método de búsqueda en DAO dependiendo si el término de búsqueda está vacío
                    if (string.IsNullOrEmpty(searchTerm))
                    {
                        // Obtener todos los productores
                        resultsTable = Daos.ObtenerTodosProductores();
                    }
                    else
                    {
                        // Buscar productores específicos
                        resultsTable = Daos.SearchProducers(searchTerm);
                    }

                    // Configurar el delegado para procesar los datos seleccionados
                    this.Invoke((MethodInvoker)delegate
                    {
                        // Crear una instancia de resultadosbusqueda
                        resultadosbusqueda resultsForm = new resultadosbusqueda(this);

                        // Cargar los resultados en el formulario
                        resultsForm.LoadResults(resultsTable);

                        // Configurar el delegado para procesar los datos seleccionados
                        resultsForm.SetDataProcessor((nombre, apellidopat, apellidomat, localidad, telefono, hectareas, id) =>
                        {
                            txtnombre3.Text = nombre;
                            txtapellidopat3.Text = apellidopat;
                            txtapellidomat3.Text = apellidomat;
                            txttelefono3.Text = telefono;
                            txtlocalidad3.Text = localidad;
                            idproductor = id;
                        });

                        // Mostrar el formulario de resultados
                        resultsForm.Show();
                    });

                    // Actualizar el formulario de carga para mostrar el progreso completo y cerrar
                    this.Invoke((MethodInvoker)delegate
                    {
                        Carga.UpdateStatus("Búsqueda completada.", 100);
                        Carga.Hide(); // Cerrar el formulario de carga
                    });
                }
                catch (Exception ex)
                {
                    // Mostrar el error en el hilo principal
                    this.Invoke((MethodInvoker)delegate
                    {
                        MessageBox.Show("Error al buscar los productores: " + ex.Message, "Error");
                        Carga.Hide(); // Cerrar el formulario de carga incluso en caso de error
                    });
                }
            });

            // Limpiar el TextBox después de la búsqueda
            txtbuscarproductorc.Clear();

        }

        private void txtbuscarproductorc_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Verificar si la tecla presionada es Enter
            if (e.KeyChar == (char)Keys.Enter)
            {
                // Suprimir el sonido "ding"
                e.Handled = true;
                btnbuscarproductorc.PerformClick();
            }
        }
        private async void txtregistrar3_Click(object sender, EventArgs e)
        {
            int id = idproductor;
            string nombre = txtnombre3.Text.Trim();
            string apellidopat = txtapellidopat3.Text.Trim();
            string apellidomat = txtapellidomat3.Text.Trim();
            string localidad = txtlocalidad3.Text.Trim();
            string telefono = txttelefono3.Text.Trim();
            string cantidad_costal = txtcantidad3.Text.Trim();
            string kg = txtkg3.Text.Trim();
            string grano = comboBoxGrano3.Text.Trim();
            string fecha = dateTimePickerCostal.Value.ToString("dd-MM-yyyy"); // Formato de fecha ajustado para SQL
            string estado = "Pendiente";

            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(apellidopat) || comboBoxGrano3.SelectedIndex == 0 ||
                string.IsNullOrEmpty(cantidad_costal) || string.IsNullOrEmpty(telefono) || string.IsNullOrEmpty(localidad) ||
                string.IsNullOrEmpty(grano) || string.IsNullOrEmpty(fecha))
            {
                MessageBox.Show("Por favor, completa todos los campos.", "Advertencia");
                return;
            }

            if (!int.TryParse(cantidad_costal, out int cantidadCostal))
            {
                MessageBox.Show("La cantidad de costales debe ser un número válido.", "Advertencia");
                return;
            }

            using (var loadingForm = new carga())
            {
                loadingForm.Show();
                await Task.Run(() =>
                {
                    try
                    {
                        loadingForm.UpdateStatus("Guardando datos...", 30);
                        // Registrar la compra
                        Daos.RegistrarCompraCostales(id, nombre, apellidopat, apellidomat, localidad, telefono, cantidadCostal, kg, grano, fecha, estado);

                        // Obtener el último ID insertado
                        int lastInsertedId = Daos.ObtenerUltimoIdInsertado();
                        loadingForm.UpdateStatus("Recuperando información...", 50);
                        if (lastInsertedId > 0)
                        {
                            // Crear el folio
                            string folio = $"F-{lastInsertedId}";
                            MessageBox.Show("Folio Generado: " + folio, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            loadingForm.UpdateStatus("Terminando...", 80);
                            // Actualizar el registro con el folio generado
                            Daos.ActualizarFolio(lastInsertedId, folio);
                            loadingForm.UpdateStatus("Completado", 100);
                        }
                        else
                        {
                            MessageBox.Show("No se pudo obtener el último ID insertado para generar el folio.", "Error");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al registrar la compra o generar el folio: " + ex.Message, "Error");
                    }
                });
                loadingForm.Close();
            }

            // Limpiar los campos después de registrar
            txtnombre3.Clear();
            txtapellidopat3.Clear();
            txtapellidomat3.Clear();
            txtlocalidad3.Clear();
            txttelefono3.Clear();
            txtcantidad3.Clear();
            txtkg3.Clear();
            comboBoxGrano3.SelectedIndex = 0;
            dateTimePickerCostal.Value = DateTime.Now;
        }

        private void estadosdePago_LinkClicked(object sender, EventArgs e)
        {
            rescompra.Show();
            rescompra.autenticacion = autenticacion;
            rescompra.LoadPurchases(); //Detalles
        }

        /* --- Para registrar las Semillas --- */
        private async void btnregistrar4_Click(object sender, EventArgs e)
        {
            int id = idproductor;
            string nombre = txtnombre2.Text.Trim();
            string apellidopat = txtapellidopat2.Text.Trim();
            string apellidomat = txtapellidomat2.Text.Trim();
            string localidad = txtlocalidad2.Text.Trim();
            string telefono = txttelefono2.Text.Trim();
            string hectareas = txthectareas2.Text.Trim();
            string kg = txtkg2.Text.Trim();
            string grano = comboBoxGrano2.Text.Trim();
            string precio = txtprecio2.Text.Trim();
            string importe = txtimporte2.Text.Trim();
            string fecha = dateTimePickerSemilla.Value.ToString("dd-MM-yyyy"); // Formato de fecha ajustado para SQL
            string tipo = "Semilla";

            if (comboBoxGrano2.SelectedIndex == 0 ||
               string.IsNullOrEmpty(hectareas) || string.IsNullOrEmpty(localidad) ||
               string.IsNullOrEmpty(grano) || string.IsNullOrEmpty(precio) || string.IsNullOrEmpty(importe) || string.IsNullOrEmpty(fecha))
            {
                MessageBox.Show("Por favor, completa todos los campos.", "Advertencia");
                return;
            }

            using (var loadingForm = new carga())
            {
                loadingForm.Show();
                await Task.Run(() =>
                {
                    try
                    {
                        // Registrar la compra en la base de datos
                        loadingForm.UpdateStatus("Guardando datos...", 30);
                        int idauto = Daos.RegistrarCompraSemilla(id, nombre, apellidopat, apellidomat, hectareas, telefono, localidad, grano, precio, importe, fecha, kg);

                        // Crear el PDF
                        loadingForm.UpdateStatus("Generando PDF...", 60);
                        string pdfPath = CreatePdf(idauto, nombre, apellidopat, apellidomat, hectareas, telefono, localidad, grano, precio, importe, fecha, kg, tipo);

                        // Mostrar el diálogo para seleccionar la ubicación del archivo PDF
                        loadingForm.UpdateStatus("Guardando PDF...", 90);
                        SavePdfToUserSelectedPath(pdfPath);

                    }
                    catch (PdfException pdfEx)
                    {
                        MessageBox.Show("Error al generar el PDF: " + pdfEx.Message, "Error");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al registrar la compra: " + ex.Message, "Error");
                    }
                });
                loadingForm.Close();
            }

            // Limpiar los campos después de registrar
            txtnombre2.Clear();
            txtapellidopat2.Clear();
            txtapellidomat2.Clear();
            txthectareas2.Clear();
            txttelefono2.Clear();
            txtlocalidad2.Clear();
            comboBoxGrano2.SelectedIndex = 0;
            txtimporte2.Clear();
            txtkg2.Clear();
            dateTimePickerSemilla.Value = DateTime.Now;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Carga.StartPosition = FormStartPosition.CenterScreen;
            Carga.Show();

            // Obtener el término de búsqueda del TextBox
            string searchTerm = txtbuscarproductors.Text.Trim();

            // Ejecutar la búsqueda en un hilo separado para evitar el bloqueo de la UI
            Task.Run(() =>
            {
                DataTable resultsTable;

                try
                {
                    // Actualizar el formulario de carga para mostrar "Buscando..." y progreso del 30%
                    this.Invoke((MethodInvoker)delegate
                    {
                        Carga.UpdateStatus("Buscando productores, por favor espere...", 30);
                    });

                    // Llamar al método de búsqueda en DAO dependiendo si el término de búsqueda está vacío
                    if (string.IsNullOrEmpty(searchTerm))
                    {
                        // Obtener todos los productores
                        resultsTable = Daos.ObtenerTodosProductores();
                    }
                    else
                    {
                        // Buscar productores específicos
                        resultsTable = Daos.SearchProducers(searchTerm);
                    }

                    // Configurar el delegado para procesar los datos seleccionados
                    this.Invoke((MethodInvoker)delegate
                    {
                        // Crear una instancia de resultadosbusqueda
                        resultadosbusqueda resultsForm = new resultadosbusqueda(this);

                        // Cargar los resultados en el formulario
                        resultsForm.LoadResults(resultsTable);

                        // Configurar el delegado para procesar los datos seleccionados
                        resultsForm.SetDataProcessor((nombre, apellidopat, apellidomat, localidad, telefono, hectareas, id) =>
                        {
                            txtnombre2.Text = nombre;
                            txtapellidopat2.Text = apellidopat;
                            txtapellidomat2.Text = apellidomat;
                            txttelefono2.Text = telefono;
                            txtlocalidad2.Text = localidad;
                            txthectareas2.Text = hectareas;
                            idproductor = id;
                        });

                        // Mostrar el formulario de resultados
                        resultsForm.Show();
                    });

                    // Actualizar el formulario de carga para mostrar el progreso completo y cerrar
                    this.Invoke((MethodInvoker)delegate
                    {
                        Carga.UpdateStatus("Búsqueda completada.", 100);
                        Carga.Hide(); // Cerrar el formulario de carga
                    });
                }
                catch (Exception ex)
                {
                    // Mostrar el error en el hilo principal
                    this.Invoke((MethodInvoker)delegate
                    {
                        MessageBox.Show("Error al buscar los productores: " + ex.Message, "Error");
                        Carga.Hide(); // Cerrar el formulario de carga incluso en caso de error
                    });
                }
            });

            // Limpiar el TextBox después de la búsqueda
            txtbuscarproductors.Clear();
        }

        private void txtbuscarproductors_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Verificar si la tecla presionada es Enter
            if (e.KeyChar == (char)Keys.Enter)
            {
                // Suprimir el sonido "ding"
                e.Handled = true;
                btnbuscarproductors.PerformClick();
            }
        }

        /* --- Ver compras --- */

        private void btnvercomprasg_Click(object sender, EventArgs e)
        {

        }

        private void compraGrano_LinkClicked(object sender, EventArgs e)
        {
            // Crear y mostrar la ventana de carga
            Carga.StartPosition = FormStartPosition.CenterScreen;
            Carga.Show();

            // Ejecutar la búsqueda en un hilo separado para evitar el bloqueo de la UI
            Task.Run(() =>
            {
                DataTable resultsTable;

                try
                {
                    // Actualizar el formulario de carga para mostrar "Cargando..." y progreso del 50%
                    this.Invoke((MethodInvoker)delegate
                    {
                        Carga.UpdateStatus("Cargando datos de compras, por favor espere...", 50);
                    });

                    // Obtener todos los datos de compras
                    resultsTable = Daos.ObtenerCompraGrano();

                    // Configurar el delegado para procesar los datos seleccionados
                    this.Invoke((MethodInvoker)delegate
                    {
                        // Crear una instancia de resultadocompras
                        resultadocompras resultsForm = new resultadocompras(this);

                        // Cargar los resultados en el formulario
                        resultsForm.LoadResults(resultsTable);

                        // Mostrar el formulario de resultados
                        resultsForm.Show();
                    });

                    // Actualizar el formulario de carga para mostrar el progreso completo
                    this.Invoke((MethodInvoker)delegate
                    {
                        Carga.UpdateStatus("Datos cargados exitosamente.", 100);
                        Carga.Hide(); // Cerrar el formulario de carga
                    });
                }
                catch (Exception ex)
                {
                    // Mostrar el error en el hilo principal
                    this.Invoke((MethodInvoker)delegate
                    {
                        MessageBox.Show("Error al cargar los datos de compras: " + ex.Message, "Error");
                        Carga.Hide(); // Cerrar el formulario de carga incluso en caso de error
                    });
                }
            });
        }

        private void btnvercomprass_Click(object sender, EventArgs e)
        {

        }

        private void compraSemilla_LinkClicked(object sender, EventArgs e)
        {
            // Crear y mostrar la ventana de carga
            Carga.StartPosition = FormStartPosition.CenterScreen;
            Carga.Show();

            // Ejecutar la búsqueda en un hilo separado para evitar el bloqueo de la UI
            Task.Run(() =>
            {
                DataTable resultsTable;

                try
                {
                    // Actualizar el formulario de carga para mostrar "Cargando..." y progreso del 50%
                    this.Invoke((MethodInvoker)delegate
                    {
                        Carga.UpdateStatus("Cargando datos de compras, por favor espere...", 50);
                    });

                    // Obtener todos los datos de compras de semillas
                    resultsTable = Daos.ObtenerCompraSemilla();

                    // Configurar el delegado para procesar los datos seleccionados
                    this.Invoke((MethodInvoker)delegate
                    {
                        // Crear una instancia de resultadocompras
                        resultadocompras resultsForm = new resultadocompras(this);

                        // Cargar los resultados en el formulario
                        resultsForm.LoadResults(resultsTable);

                        // Mostrar el formulario de resultados
                        resultsForm.Show();
                    });

                    // Actualizar el formulario de carga para mostrar el progreso completo
                    this.Invoke((MethodInvoker)delegate
                    {
                        Carga.UpdateStatus("Datos cargados exitosamente.", 100);
                        Carga.Hide(); // Cerrar el formulario de carga
                    });
                }
                catch (Exception ex)
                {
                    // Mostrar el error en el hilo principal
                    this.Invoke((MethodInvoker)delegate
                    {
                        MessageBox.Show("Error al cargar los datos de compras: " + ex.Message, "Error");
                        Carga.Hide(); // Cerrar el formulario de carga incluso en caso de error
                    });
                }
            });
        }

        private void txtkg1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender is System.Windows.Forms.TextBox textBox)
            {
                // Permitir solo dígitos, el carácter de control de retroceso y el punto decimal
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
                {
                    e.Handled = true;
                }

                // Permitir solo un punto decimal
                if (e.KeyChar == '.' && textBox.Text.IndexOf('.') > -1)
                {
                    e.Handled = true;
                }
            }
        }

        private void txtprecio1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender is System.Windows.Forms.TextBox textBox)
            {
                // Permitir solo dígitos, el carácter de control de retroceso y el punto decimal
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
                {
                    e.Handled = true;
                }

                // Permitir solo un punto decimal
                if (e.KeyChar == '.' && textBox.Text.IndexOf('.') > -1)
                {
                    e.Handled = true;
                }
            }
        }

        private void txtkg2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender is System.Windows.Forms.TextBox textBox)
            {
                // Permitir solo dígitos, el carácter de control de retroceso y el punto decimal
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
                {
                    e.Handled = true;
                }

                // Permitir solo un punto decimal
                if (e.KeyChar == '.' && textBox.Text.IndexOf('.') > -1)
                {
                    e.Handled = true;
                }
            }
        }

        private void txtprecio2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender is System.Windows.Forms.TextBox textBox)
            {
                // Permitir solo dígitos, el carácter de control de retroceso y el punto decimal
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
                {
                    e.Handled = true;
                }

                // Permitir solo un punto decimal
                if (e.KeyChar == '.' && textBox.Text.IndexOf('.') > -1)
                {
                    e.Handled = true;
                }
            }
        }

        private void txtkg3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender is System.Windows.Forms.TextBox textBox)
            {
                // Permitir solo dígitos, el carácter de control de retroceso y el punto decimal
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
                {
                    e.Handled = true;
                }

                // Permitir solo un punto decimal
                if (e.KeyChar == '.' && textBox.Text.IndexOf('.') > -1)
                {
                    e.Handled = true;
                }
            }
        }

        private void txtcantidad3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender is System.Windows.Forms.TextBox textBox)
            {
                // Permitir solo dígitos, el carácter de control de retroceso y el punto decimal
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
                {
                    e.Handled = true;
                }

                // Permitir solo un punto decimal
                if (e.KeyChar == '.' && textBox.Text.IndexOf('.') > -1)
                {
                    e.Handled = true;
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
                panelfondo.Size = new System.Drawing.Size(panelfondo.Width + panellateral.Width, panelfondo.Height);
                panelfondo.Location = new System.Drawing.Point(panelfondo.Location.X - panellateral.Width, panelfondo.Location.Y);
                tabControl1.Size = new System.Drawing.Size(tabControl1.Width + panellateral.Width, tabControl1.Height);
                tabControl1.Location = new System.Drawing.Point(tabControl1.Location.X - panellateral.Width, tabControl1.Location.Y);
            }
            else
            {
                // Mostrar panel1 y contraer panel2
                panellateral.Visible = true;
                panelfondo.Size = new System.Drawing.Size(panelfondo.Width - panellateral.Width, panelfondo.Height);
                panelfondo.Location = new System.Drawing.Point(panelfondo.Location.X + panellateral.Width, panelfondo.Location.Y);
                tabControl1.Size = new System.Drawing.Size(tabControl1.Width - panellateral.Width, tabControl1.Height);
                tabControl1.Location = new System.Drawing.Point(tabControl1.Location.X + panellateral.Width, tabControl1.Location.Y);
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

        private void Compras_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                txtprecio1.Text = DaosA.ObtenerPrecioGrano();
                txtprecio2.Text = DaosA.ObtenerPrecioSemilla();
            }
        }

        // Método para inicializar el TextBox con el placeholder
        private void InicializarTextBoxConPlaceholder(TextBox txt, string placeholder)
        {
            txt.Text = placeholder;
            txt.ForeColor = System.Drawing.Color.Gray;

            txt.Enter += (sender, e) =>
            {
                // Si el texto del TextBox es el placeholder, lo limpiamos
                if (txt.Text == placeholder)
                {
                    txt.Text = "";
                    txt.ForeColor = System.Drawing.Color.Black; // Cambiamos el color a negro cuando se ingresa texto
                }
            };

            txt.Leave += (sender, e) =>
            {
                // Si el texto está vacío, volvemos a poner el placeholder
                if (string.IsNullOrWhiteSpace(txt.Text))
                {
                    txt.Text = placeholder;
                    txt.ForeColor = System.Drawing.Color.Gray; // El color vuelve a gris cuando mostramos el placeholder
                }
            };
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            compraGrano_LinkClicked(sender, e);
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            compraSemilla_LinkClicked(sender, e);
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            estadosdePago_LinkClicked(sender, e);
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
