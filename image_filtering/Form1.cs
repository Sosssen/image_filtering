﻿using System;
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
        private Bitmap image = null;

        private static int histogramSize = 256;
        private int[] histogramRed = new int[histogramSize];
        private int[] histogramGreen = new int[histogramSize];
        private int[] histogramBlue = new int[histogramSize];

        private Pen pen = new Pen(Color.Black, 1);
        private Pen penRed = new Pen(Color.Red, 1);
        private SolidBrush sbRed = new SolidBrush(Color.Red);
        private int radius = 50;
        private HashSet<Point> circles = new HashSet<Point>();

        private int moving = 0; // 0 - not moving, 1 - moving LPM, 2 - moving PPM
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
            // aktualny obraz, będzie modyfikowany
            image = new Bitmap(new Bitmap(filename), 512, 512);
            
            // bitmapa do wyświetlenia
            drawArea = new DirectBitmap(image.Width, image.Height);
            Canvas.Width = image.Width;
            Canvas.Height = image.Height;
            Canvas.Image = drawArea.Bitmap;
            
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

            Canvas.Image = drawAreaMask.Bitmap;

            RedrawImage();
            CountHistogram();
        }

        public void RedrawImage()
        {
            using (Graphics g = Graphics.FromImage(drawArea.Bitmap))
            {
                g.DrawImage(image, 0, 0);
                foreach (var point in circles)
                {
                    //g.DrawEllipse(penRed, point.X - radius, point.Y - radius, 2 * radius, 2 * radius);
                    //g.FillEllipse(sbRed, point.X - radius, point.Y - radius, 2 * radius, 2 * radius);
                    midPointCircleDraw(drawArea, point.X, point.Y, radius, Color.Red);
                }
            }

            Canvas.Image = drawArea.Bitmap;
            Canvas.Invalidate();
            Canvas.Update();
        }

        public void midPointCircleDraw(DirectBitmap bitmap, int x_centre, int y_centre, int r, Color color)
        {

            int x = r, y = 0;

            // Printing the initial point on the
            // axes after translation

            // bitmap.SetPixel(-x + x_centre, y + y_centre, color);
            //Console.Write("(" + (x + x_centre)
            //        + ", " + (y + y_centre) + ")");

            drawLine(x + x_centre, y + y_centre, -x + x_centre, y + y_centre);

            // When radius is zero only a single
            // point will be printed
            // bitmap.SetPixel(x + x_centre, -y + y_centre, color);
            //Console.Write("(" + (x + x_centre)
            //    + ", " + (-y + y_centre) + ")");

            drawLine(y + x_centre, x + y_centre, y + x_centre, x + y_centre);
            //Console.Write("(" + (y + x_centre)
            //    + ", " + (x + y_centre) + ")");

            drawLine(y + x_centre, -x + y_centre, y + x_centre, -x + y_centre);
            int P = 1 - r;
            while (x > y)
            {

                y++;

                // Mid-point is inside or on the perimeter
                if (P <= 0)
                    P = P + 2 * y + 1;

                // Mid-point is outside the perimeter
                else
                {
                    x--;
                    P = P + 2 * y - 2 * x + 1;
                }

                // All the perimeter points have already
                // been printed
                if (x < y)
                    break;

                // Printing the generated point and its
                // reflection in the other octants after
                // translation
                // bitmap.SetPixel(x + x_centre, y + y_centre, color);
                //Console.Write("(" + (x + x_centre)
                //        + ", " + (y + y_centre) + ")");

                // bitmap.SetPixel(-x + x_centre, y + y_centre, color);
                //Console.Write("(" + (-x + x_centre)
                //        + ", " + (y + y_centre) + ")");
                drawLine(x + x_centre, y + y_centre, -x + x_centre, y + y_centre);

                // bitmap.SetPixel(x + x_centre, -y + y_centre, color);
                //Console.Write("(" + (x + x_centre) +
                //        ", " + (-y + y_centre) + ")");

                // bitmap.SetPixel(-x + x_centre, -y + y_centre, color);
                //Console.WriteLine("(" + (-x + x_centre)
                //        + ", " + (-y + y_centre) + ")");
                drawLine(x + x_centre, -y + y_centre, -x + x_centre, -y + y_centre);
                // If the generated point is on the
                // line x = y then the perimeter points
                // have already been printed
                if (x != y)
                {
                    // bitmap.SetPixel(y + x_centre, x + y_centre, color);
                    //Console.Write("(" + (y + x_centre)
                    //    + ", " + (x + y_centre) + ")");

                    // bitmap.SetPixel(-y + x_centre, x + y_centre, color);
                    //Console.Write("(" + (-y + x_centre)
                    //    + ", " + (x + y_centre) + ")");

                    drawLine(y + x_centre, x + y_centre, -y + x_centre, x + y_centre);

                    // bitmap.SetPixel(y + x_centre, -x + y_centre, color);
                    //Console.Write("(" + (y + x_centre)
                    //    + ", " + (-x + y_centre) + ")");

                    // bitmap.SetPixel(-y + x_centre, -x + y_centre, color);

                    drawLine(y + x_centre, -x + y_centre, -y + x_centre, -x + y_centre);
                    //Console.WriteLine("(" + (-y + x_centre)
                    //    + ", " + (-x + y_centre) + ")");
                }
            }
        }
        private void drawLine(int x0, int y0, int x1, int y1)
        {
            if (Math.Abs(y1 - y0) < Math.Abs(x1 - x0))
            {
                if (x0 > x1)
                {
                    drawLineLow(x1, y1, x0, y0);
                }
                else
                {
                    drawLineLow(x0, y0, x1, y1);
                }
            }
            else
            {
                if (y0 > y1)
                {
                    drawLineHigh(x1, y1, x0, y0);
                }
                else
                {
                    drawLineHigh(x0, y0, x1, y1);
                }
            }
        }

        private void drawLineLow(int x0, int y0, int x1, int y1)
        {
            int dx = x1 - x0;
            int dy = y1 - y0;
            int sy = 1;
            if (dy < 0)
            {
                sy = -1;
                dy = -dy;
            }
            int D = 2 * dy - dx;
            int y = y0;

            for (int x = x0; x <= x1; x++)
            {
                if (x >= 0 && y >= 0 && x < drawArea.Width && y < drawArea.Height) drawArea.SetPixel(x, y, Color.Red);
                if (D > 0)
                {
                    y += sy;
                    D += 2 * (dy - dx);
                }
                else
                {
                    D += 2 * dy;
                }
            }
        }

        private void drawLineHigh(int x0, int y0, int x1, int y1)
        {
            int dx = x1 - x0;
            int dy = y1 - y0;
            int sx = 1;
            if (dx < 0)
            {
                sx = -1;
                dx = -dx;
            }
            int D = 2 * dx - dy;
            int x = x0;

            for (int y = y0; y <= y1; y++)
            {
                if (x >= 0 && y >= 0 && x < drawArea.Width && y < drawArea.Height) drawArea.SetPixel(x, y, Color.Red);
                if (D > 0)
                {
                    x += sx;
                    D += 2 * (dx - dy);
                }
                else
                {
                    D += 2 * dx;
                }
            }
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

            if (moving == 1)
            {
                circles.Add(new Point(e.X, e.Y));
                RedrawImage();
            }
        }

        private void Canvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                moving = 1;
                circles.Add(new Point(e.X, e.Y));
                RedrawImage();
            }
        }

        private void Canvas_MouseUp(object sender, MouseEventArgs e)
        {
            moving = 0;
            using (Graphics g = Graphics.FromImage(drawAreaMask.Bitmap))
            {
                foreach (var point in circles)
                {
                    g.DrawEllipse(penRed, point.X - radius, point.Y - radius, 2 * radius, 2 * radius);
                    g.FillEllipse(sbRed, point.X - radius, point.Y - radius, 2 * radius, 2 * radius);
                }
            }
            using (Graphics g = Graphics.FromImage(image))
            {
                foreach (var point in circles)
                {
                    g.DrawEllipse(penRed, point.X - radius, point.Y - radius, 2 * radius, 2 * radius);
                    g.FillEllipse(sbRed, point.X - radius, point.Y - radius, 2 * radius, 2 * radius);
                }
            }
            circles.Clear();
            RedrawImage();
            CountHistogram();
            
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
