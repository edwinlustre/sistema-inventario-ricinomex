namespace appInventario.Models
{
    public class Productos
    {
        //int tote, tamborm, tamborp, g70, g50, g20, g10, g5, b1 = 0;
        public int Tote { get; set; }

        public int Tamborm { get; set; }

        public int Tamborp { get; set; }

        public int G70 { get; set; }

        public int G50 { get; set; }

        public int G20 { get; set; }

        public int G10 { get; set; }

        public int G5 { get; set; }

        public int B1 { get; set; }

        public Productos(int tote, int tamborm, int tamborp, int g70, int g50, int g20, int g10, int g5, int b1)
        {
            Tote = tote;
            Tamborm = tamborm;
            Tamborp = tamborp;
            G70 = g70;
            G50 = g50;
            G20 = g20;
            G10 = g10;
            G5 = g5;
            B1 = b1;

        }

        public Productos()
        {

        }



    }
}
