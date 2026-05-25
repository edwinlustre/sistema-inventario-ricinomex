using appInventario.DAOS;
using appInventario.Models;
using System;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace appInventario.Form_Produccion
{
    public partial class inventarioproducto : Form
    {
        private daosproduccion Daos;

        public inventarioproducto()
        {
            InitializeComponent();
            Daos = new daosproduccion();
            this.FormClosing += new FormClosingEventHandler(inventarioproducto_FormClosing);
        }

        private void inventarioproducto_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void inventarioproducto_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                muestraDeGraficosPlanta();
                muestraDeGraficosAlmacen();
            }

        }

        private void muestraDeGraficosPlanta()
        {
            Productos productos = Daos.contarProductosDisponibles("Planta");
            actualizarGrafico(chart1, productos);
        }

        private void muestraDeGraficosAlmacen()
        {
            Productos productos = Daos.contarProductosDisponibles("Almacen");
            actualizarGrafico(chart2, productos);
        }

        private void actualizarGrafico(Chart chart, Productos productos)
        {
            // Limpiar los puntos de datos previos
            chart.Series["Disponible"].Points.Clear();

            // Añadir los datos al gráfico
            chart.Series["Disponible"].Points.AddXY("Tote", productos.Tote);
            chart.Series["Disponible"].Points.AddXY("Tambor Metálico", productos.Tamborm);
            chart.Series["Disponible"].Points.AddXY("Tambor Plástico", productos.Tamborp);
            chart.Series["Disponible"].Points.AddXY("Garrafa 70L", productos.G70);
            chart.Series["Disponible"].Points.AddXY("Garrafa 50L", productos.G50);
            chart.Series["Disponible"].Points.AddXY("Garrafa 20L", productos.G20);
            chart.Series["Disponible"].Points.AddXY("Garrafa 10L", productos.G10);
            chart.Series["Disponible"].Points.AddXY("Garrafa 5L", productos.G5);
            chart.Series["Disponible"].Points.AddXY("Botella", productos.B1);

            // Configuración adicional
            chart.Series["Disponible"].IsValueShownAsLabel = true;
            chart.Series["Disponible"].ChartType = SeriesChartType.RangeColumn; // O cualquier tipo que prefieras
        }

        private void inventarioproducto_Load(object sender, EventArgs e)
        {

        }
    }
}
