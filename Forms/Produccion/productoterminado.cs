using appInventario.DAOS;
using System;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace appInventario.Form_Produccion
{
    public partial class productoterminado : Form
    {
        private daosproduccion Daos;
        public productoterminado()
        {
            InitializeComponent();
            Daos = new daosproduccion();

            this.FormClosing += productoterminado_FormClosing;
        }

        private void productoterminado_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void productoterminado_Load(object sender, EventArgs e)
        {

        }

        private void productoterminado_VisibleChanged(object sender, EventArgs e)
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

                dateTimePicker2.Format = DateTimePickerFormat.Custom;
                dateTimePicker2.CustomFormat = "yyyy";
                dateTimePicker2.ShowUpDown = true; // Para mostrar solo mes y año

                int añoTotal = dateTimePicker2.Value.Year;

                insertarTituloAnual(añoActual);

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
                    labelgrano.Text = "Inventario de producto terminado de Enero " + añoActual;
                    break;
                case 02:
                    labelgrano.Text = "Inventario de producto terminado de Febrero " + añoActual;
                    break;
                case 03:
                    labelgrano.Text = "Inventario de producto terminado de Marzo " + añoActual;
                    break;
                case 04:
                    labelgrano.Text = "Inventario de producto terminado de Abril " + añoActual;
                    break;
                case 05:
                    labelgrano.Text = "Inventario de producto terminado de Mayo " + añoActual;
                    break;
                case 06:
                    labelgrano.Text = "Inventario de producto terminado de Junio " + añoActual;
                    break;
                case 07:
                    labelgrano.Text = "Inventario de producto terminado de Julio " + añoActual;
                    break;
                case 08:
                    labelgrano.Text = "Inventario de producto terminado de Agosto " + añoActual;
                    break;
                case 09:
                    labelgrano.Text = "Inventario de producto terminado de Septiembre " + añoActual;
                    break;
                case 10:
                    labelgrano.Text = "Inventario de producto terminado de Octubre " + añoActual;
                    break;
                case 11:
                    labelgrano.Text = "Inventario de producto terminado de Noviembre " + añoActual;
                    break;
                case 12:
                    labelgrano.Text = "Inventario de producto terminado de Diciembre " + añoActual;
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

            // Calcular y llenar el gráfico con los datos del mes pasado
            //DateTime mesPasado = dateTimePicker1.Value.AddMonths(-1);
            //LlenarGrafico(mesPasado.Month, mesPasado.Year);
        }

        private void LlenarGrafico(int mes, int año)
        {
            // Obtener el total de kilos por mes
            int totalKg = Daos.obtenerProductoTerminado(mes, año);

            // Limpiar los puntos de datos previos
            chart1.Series["Litros"].Points.Clear();

            // Añadir datos al gráfico
            chart1.Series["Litros"].Points.AddXY($"{mes}/{año}", totalKg);

            // Configuración adicional (opcional)
            chart1.Series["Litros"].IsValueShownAsLabel = true;
            chart1.Series["Litros"].ChartType = SeriesChartType.RangeColumn; // Cambia a Pie para gráfico de pastel
        }

        /* --- ANUAL --- */

        private void insertarTituloAnual(int añoActual)
        {
            labelanual.Text = "Inventario de producto terminado del año " + añoActual;
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
            // Obtener los productos terminados por mes
            int[] productoPorMes = Daos.obtenerProductoTerminadoPorMes(año);

            // Limpiar los puntos de datos previos
            chart2.Series["Litros"].Points.Clear();

            // Añadir datos al gráfico por cada mes
            for (int i = 0; i < 12; i++)
            {
                string mesNombre = new DateTime(año, i + 1, 1).ToString("MMMM"); // Obtiene el nombre del mes
                chart2.Series["Litros"].Points.AddXY(mesNombre, productoPorMes[i]);
            }

            // Configuración adicional (opcional)
            chart2.Series["Litros"].IsValueShownAsLabel = true;
            chart2.Series["Litros"].ChartType = SeriesChartType.RangeColumn; // Cambia a Pie para gráfico de pastel
        }



    }
}
