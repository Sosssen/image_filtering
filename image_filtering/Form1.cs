using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace image_filtering
{
    public partial class Form1 : Form
    {
        private DirectBitmap db = null;
        private Bitmap image = null;

        private static int histogramSize = 256;
        private int[] histogramRed = new int[histogramSize];
        private int[] histogramGreen = new int[histogramSize];
        private int[] histogramBlue = new int[histogramSize];
        public Form1()
        {
            InitializeComponent();

            Configuration();
        }

        public void Configuration()
        {
            Rectangle screen = Screen.PrimaryScreen.WorkingArea;
            int w = (int)(screen.Width / 1.25);
            int h = (int)(screen.Height / 1.25);
            this.Size = new Size(w, h);

            redChart.ChartAreas[0].AxisX.Minimum = 0;
            redChart.ChartAreas[0].AxisX.Maximum = 255;
            greenChart.ChartAreas[0].AxisX.Minimum = 0;
            greenChart.ChartAreas[0].AxisX.Maximum = 255;
            blueChart.ChartAreas[0].AxisX.Minimum = 0;
            blueChart.ChartAreas[0].AxisX.Maximum = 255;

            string filename = @".\lib\lenna.png";
            LoadImage(filename);
        }

        public void LoadImage(string filename)
        {
            image = new Bitmap(new Bitmap(filename), 512, 512);
            db = new DirectBitmap(image.Width, image.Height);
            using (Graphics g = Graphics.FromImage(db.Bitmap))
            {
                g.DrawImage(image, 0, 0);
            }
            Canvas.Width = image.Width;
            Canvas.Height = image.Height;
            Canvas.Image = db.Bitmap;
            Canvas.Invalidate();
            Canvas.Update();

            CountHistogram();
        }

        public void CountHistogram()
        {
            Array.Clear(histogramRed, 0, histogramSize);
            Array.Clear(histogramGreen, 0, histogramSize);
            Array.Clear(histogramBlue, 0, histogramSize);

            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    Color color = db.GetPixel(i, j);
                    histogramRed[color.R]++;
                    histogramGreen[color.G]++;
                    histogramBlue[color.B]++;
                }
            }
            redChart.Series["red"].Points.Clear();
            greenChart.Series["green"].Points.Clear();
            blueChart.Series["blue"].Points.Clear();
            for (int i = 0; i < histogramSize; i++)
            {
                redChart.Series["red"].Points.Add(new DataPoint(i, histogramRed[i]));
                greenChart.Series["green"].Points.Add(new DataPoint(i, histogramGreen[i]));
                blueChart.Series["blue"].Points.Add(new DataPoint(i, histogramBlue[i]));
            }
        }
    }

    public class DirectBitmap : IDisposable
    {
        public Bitmap Bitmap { get; private set; }
        public Int32[] Bits { get; private set; }
        public bool Disposed { get; private set; }
        public int Height { get; private set; }
        public int Width { get; private set; }

        protected System.Runtime.InteropServices.GCHandle BitsHandle { get; private set; }

        public DirectBitmap(int width, int height)
        {
            Width = width;
            Height = height;
            Bits = new Int32[width * height];
            BitsHandle = System.Runtime.InteropServices.GCHandle.Alloc(Bits, System.Runtime.InteropServices.GCHandleType.Pinned);
            Bitmap = new Bitmap(width, height, width * 4, System.Drawing.Imaging.PixelFormat.Format32bppPArgb, BitsHandle.AddrOfPinnedObject());
        }

        public void SetPixel(int x, int y, Color colour)
        {
            int index = x + (y * Width);
            int col = colour.ToArgb();

            Bits[index] = col;
        }

        public Color GetPixel(int x, int y)
        {
            int index = x + (y * Width);
            int col = Bits[index];
            Color result = Color.FromArgb(col);

            return result;
        }

        public void Dispose()
        {
            if (Disposed) return;
            Disposed = true;
            Bitmap.Dispose();
            BitsHandle.Free();
        }
    }
}
