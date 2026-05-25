using appInventario.DAOS;
using System;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace appInventario.Form_Produccion
{
    public partial class inventariograno : Form
    {
        private daosproduccion Daos;

        public inventariograno()
        {
            InitializeComponent();
            Daos = new daosproduccion();
            this.FormClosing += inventariograno_FormClosing;
        }

        private void inventariograno_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void inventariograno_Load(object sender, EventArgs e)
        {

        }

        private void inventariograno_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                // Configurar el DateTimePicker para mostrar solo F y año
                dateTimePicker1.Format = DateTimePickerFormat.Custom;
                dateTimePicker1.CustomFormat = "MM/yyyy";
                dateTimePicker1.ShowUpDown = true; // Para mostrar solo mes y año

                int mesActual = dateTimePicker1.Value.Month;
                int añoActual = dateTimePicker1.Value.Year;

                insertarTitulo(mesActual, añoActual);

                // Cargar los datos iniciales del gráfico (mes y año actuales)
                LlenarGrafico(dateTimePicker1.Value.Month, dateTimePicker1.Value.Year);

                // Asignar el evento de cambio de valor al DateTimePicker
                dateTimePicker1.ValueChanged += DateTimePicker1_ValueChanged;

                /* --- ANUAL --- */

                // Configurar el DateTimePicker para mostrar solo F y año
                dateTimePicker2.Format = DateTimePickerFormat.Custom;
                dateTimePicker2.CustomFormat = "yyyy";
                dateTimePicker2.ShowUpDown = true; // Para mostrar solo mes y año

                int añoActual2 = dateTimePicker2.Value.Year;

                insertarTituloAnual(añoActual2);

                // Cargar los datos iniciales del gráfico (mes y año actuales)
                LlenarGraficoAnual(dateTimePicker2.Value.Year);

                // Asignar el evento de cambio de valor al DateTimePicker
                dateTimePicker2.ValueChanged += DateTimePicker2_ValueChanged;
            }
        }

        private void insertarTitulo(int mesActual, int añoActual)
        {
            switch (mesActual)
            {
                case 01:
                    labelgrano.Text = "Inventario de grano de Enero " + añoActual;
                    break;
                case 02:
                    labelgrano.Text = "Inventario de grano de Febrero " + añoActual;
                    break;
                case 03:
                    labelgrano.Text = "Inventario de grano de Marzo " + añoActual;
                    break;
                case 04:
                    labelgrano.Text = "Inventario de grano de Abril " + añoActual;
                    break;
                case 05:
                    labelgrano.Text = "Inventario de grano de Mayo " + añoActual;
                    break;
                case 06:
                    labelgrano.Text = "Inventario de grano de Junio " + añoActual;
                    break;
                case 07:
                    labelgrano.Text = "Inventario de grano de Julio " + añoActual;
                    break;
                case 08:
                    labelgrano.Text = "Inventario de grano de Agosto " + añoActual;
                    break;
                case 09:
                    labelgrano.Text = "Inventario de grano de Septiembre " + añoActual;
                    break;
                case 10:
                    labelgrano.Text = "Inventario de grano de Octubre " + añoActual;
                    break;
                case 11:
                    labelgrano.Text = "Inventario de grano de Noviembre " + añoActual;
                    break;
                case 12:
                    labelgrano.Text = "Inventario de grano de Diciembre " + añoActual;
                    break;
            }
        }

        private void DateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            int mesActual = dateTimePicker1.Value.Month;
            int añoActual = dateTimePicker1.Value.Year;

            insertarTitulo(mesActual, añoActual);

            // Llenar el gráfico con los datos del mes seleccionado
            LlenarGrafico(mesActual, añoActual);
        }

        private void LlenarGrafico(int mes, int año)
        {
            // Obtener el total de kilos por mes
            int totalKg = Daos.obtenerTotalKgPorMes(mes, año);

            // Limpiar los puntos de datos previos
            chart1.Series["Kilos"].Points.Clear();

            // Añadir datos al gráfico
            chart1.Series["Kilos"].Points.AddXY($"{mes}/{año}", totalKg);

            // Configuración adicional (opcional)
            chart1.Series["Kilos"].IsValueShownAsLabel = true;
            chart1.Series["Kilos"].ChartType = SeriesChartType.RangeColumn; // Cambia a Pie para gráfico de pastel
        }

        /* --- ANUAL ---*/

        private void insertarTituloAnual(int añoActual)
        {
            labelgranoanual.Text = "Inventario de grano del " + añoActual;
        }

        private void DateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            int añoActual = dateTimePicker2.Value.Year;

            insertarTituloAnual(añoActual);

            // Llenar el gráfico con los datos del mes seleccionado
            LlenarGraficoAnual(añoActual);
        }

        private void LlenarGraficoAnual(int año)
        {
            // Obtener los kilos por cada mes
            int[] kilosPorMes = Daos.obtenerTotalKgPorMes(año);

            // Limpiar los puntos de datos previos
            chart2.Series["Kilos"].Points.Clear();

            // Añadir datos al gráfico, un punto por cada mes
            for (int i = 0; i < 12; i++)
            {
                // Convertir el número de mes a nombre del mes
                string nombreMes = new DateTime(año, i + 1, 1).ToString("MMMM");
                chart2.Series["Kilos"].Points.AddXY(nombreMes, kilosPorMes[i]);
            }

            // Configuración adicional (opcional)
            chart2.Series["Kilos"].IsValueShownAsLabel = true;
            chart2.Series["Kilos"].ChartType = SeriesChartType.RangeColumn; // Cambia a Pie para gráfico de pastel
        }

    }
}
