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
        private DirectBitmap drawArea = null;
        private DirectBitmap imageCopy = null;
        private DirectBitmap drawAreaMask = null;
        private DirectBitmap workingBitmap = null;
        private Bitmap image = null;

        private static int histogramSize = 256;
        private int[] histogramRed = new int[histogramSize];
        private int[] histogramGreen = new int[histogramSize];
        private int[] histogramBlue = new int[histogramSize];

        private Pen pen = new Pen(Color.Black, 1);
        private int radius = 100;
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
            
            // bitmapa do wyświetlenia
            drawArea = new DirectBitmap(image.Width, image.Height);
            Canvas.Width = image.Width;
            Canvas.Height = image.Height;
            Canvas.Image = drawArea.Bitmap;
            RedrawImage();
            
            // kopia załadowanego zdjęcia
            imageCopy = new DirectBitmap(image.Width, image.Height);
            using (Graphics g = Graphics.FromImage(imageCopy.Bitmap))
            {
                g.DrawImage(image, 0, 0);
            }

            // maska używana po skończeniu rysowania
            drawAreaMask = new DirectBitmap(image.Width, image.Height);
            using (Graphics g = Graphics.FromImage(drawAreaMask.Bitmap))
            {
                g.Clear(Color.White);
            }

            // bitmapa robocza - przy rysowaniu tutaj zapisujemy zamalowany obszar
            workingBitmap = new DirectBitmap(image.Width, image.Height);
            using (Graphics g = Graphics.FromImage(workingBitmap.Bitmap))
            {
                g.Clear(Color.White);
            }

            CountHistogram();
        }

        public void RedrawImage()
        {
            using (Graphics g = Graphics.FromImage(drawArea.Bitmap))
            {
                g.DrawImage(image, 0, 0);
            }

            Canvas.Invalidate();
            Canvas.Update();
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
                    Color color = drawArea.GetPixel(i, j);
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

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            RedrawImage();
            using (Graphics g = Graphics.FromImage(drawArea.Bitmap))
            {
                g.DrawEllipse(pen, e.X - radius, e.Y - radius, 2 * radius, 2 * radius);
            }
            Canvas.Invalidate();
            Canvas.Update();
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
