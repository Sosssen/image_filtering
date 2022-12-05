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
        private DirectBitmap imageBackup = null;
        private Bitmap image = null;

        private int[] antifilterArr = new int[256];
        private int[] negationArr = new int[256];
        private int[] brightnessArr = new int[256];
        private int brightnessConst = 30;
        private int[] gammaArr = new int[256];
        private double gammaConst = 2.0;
        private int[] contrastArr = new int[256];
        private int contrastConst = 40;
        private int[] ownArr = new int[256];
        

        private static int histogramSize = 256;
        private int[] histogramRed = new int[histogramSize];
        private int[] histogramGreen = new int[histogramSize];
        private int[] histogramBlue = new int[histogramSize];

        private Pen pen = new Pen(Color.Black, 1);
        private Pen penRed = new Pen(Color.Red, 1);
        private SolidBrush sbRed = new SolidBrush(Color.Red);
        private int radius = 30;
        private HashSet<Point> circles = new HashSet<Point>();

        private int moving = 0; // 0 - not moving, 1 - moving LPM, 2 - moving PPM

        private MyPoint[] bezierPoints = new MyPoint[4];
        private bool movingBezier = false;
        private int bezierIndex = 0;
        private int bezierRadius = 5;
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

            redChart.ChartAreas[0].AxisX.Minimum = -1;
            redChart.ChartAreas[0].AxisX.Maximum = 256;
            redChart.ChartAreas[0].AxisX.LabelStyle.Enabled = false;
            redChart.ChartAreas[0].AxisY.LabelStyle.Enabled = false;
            greenChart.ChartAreas[0].AxisX.Minimum = -1;
            greenChart.ChartAreas[0].AxisX.Maximum = 256;
            greenChart.ChartAreas[0].AxisX.LabelStyle.Enabled = false;
            greenChart.ChartAreas[0].AxisY.LabelStyle.Enabled = false;
            blueChart.ChartAreas[0].AxisX.Minimum = -1;
            blueChart.ChartAreas[0].AxisX.Maximum = 256;
            blueChart.ChartAreas[0].AxisX.LabelStyle.Enabled = false;
            blueChart.ChartAreas[0].AxisY.LabelStyle.Enabled = false;

            filterChart.ChartAreas[0].AxisX2.Enabled = AxisEnabled.True;
            filterChart.ChartAreas[0].AxisY2.Enabled = AxisEnabled.True;
            filterChart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            filterChart.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            filterChart.ChartAreas[0].AxisX2.MajorGrid.Enabled = false;
            filterChart.ChartAreas[0].AxisY2.MajorGrid.Enabled = false;
            filterChart.ChartAreas[0].AxisX.Minimum = 0;
            filterChart.ChartAreas[0].AxisX.Maximum = 255;
            filterChart.ChartAreas[0].AxisY.Minimum = 0;
            filterChart.ChartAreas[0].AxisY.Maximum = 255;
            filterChart.ChartAreas[0].AxisX2.Minimum = 0;
            filterChart.ChartAreas[0].AxisX2.Maximum = 255;
            filterChart.ChartAreas[0].AxisY2.Minimum = 0;
            filterChart.ChartAreas[0].AxisY2.Maximum = 255;
            filterChart.ChartAreas[0].AxisX.LabelStyle.Enabled = false;
            filterChart.ChartAreas[0].AxisY.LabelStyle.Enabled = false;
            filterChart.ChartAreas[0].AxisX2.LabelStyle.Enabled = false;
            filterChart.ChartAreas[0].AxisY2.LabelStyle.Enabled = false;
            filterChart.ChartAreas[0].CursorX.LineWidth = 0;
            filterChart.ChartAreas[0].CursorY.LineWidth = 0;



            InitializeFilterArrays();

            DrawFilterChart();

            string filename = @".\lib\landscape.png";
            LoadImage(filename);
        }

        public void InitializeFilterArrays()
        {
            for (int i = 0; i < antifilterArr.Length; i++)
            {
                antifilterArr[i] = i;
            }

            for (int i = 0; i < negationArr.Length; i++)
            {
                negationArr[i] = 255 - i;
            }

            for (int i = 0; i < brightnessConst; i++)
            {
                brightnessArr[i] = 0;
            }

            for (int i = brightnessConst; i < brightnessArr.Length; i++)
            {
                brightnessArr[i] = i - brightnessConst;
            }

            for (int i = 0; i < gammaArr.Length; i++)
            {
                double tmp = (double)i / 255.0;
                tmp = Math.Pow(tmp, gammaConst);
                gammaArr[i] = (int)(tmp * 255);
            }

            for (int i = 0; i < contrastConst; i++)
            {
                contrastArr[i] = 0;
                contrastArr[contrastArr.Length - i - 1] = 255;
            }
            double h = 255.0 / (255.0 - 2 * contrastConst);
            double currVal = 0;
            for (int i = contrastConst; i < contrastArr.Length - contrastConst; i++)
            {
                currVal += h;
                contrastArr[i] = Math.Min((int)currVal, 255);
            }

            bezierPoints[0] = new MyPoint(0, 0);
            bezierPoints[1] = new MyPoint(50, 200);
            bezierPoints[2] = new MyPoint(200, 50);
            bezierPoints[3] = new MyPoint(255, 255);

            CountBezier();


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

            imageBackup = new DirectBitmap(image.Width, image.Height);
            using (Graphics g = Graphics.FromImage(imageBackup.Bitmap))
            {
                g.DrawImage(image, 0, 0);
            }

            RedrawImage();
            CountHistogram();
        }

        public void RedrawImage()
        {
            using (Graphics g = Graphics.FromImage(drawArea.Bitmap))
            {
                g.DrawImage(imageCopy.Bitmap, 0, 0);
                foreach (var point in circles)
                {
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

            DrawLine(bitmap, x + x_centre, y + y_centre, -x + x_centre, y + y_centre);

            DrawLine(bitmap, y + x_centre, x + y_centre, y + x_centre, x + y_centre);

            DrawLine(bitmap, y + x_centre, -x + y_centre, y + x_centre, -x + y_centre);
            int P = 1 - r;
            while (x > y)
            {

                y++;

                if (P <= 0)
                    P = P + 2 * y + 1;

                else
                {
                    x--;
                    P = P + 2 * y - 2 * x + 1;
                }

                if (x < y)
                    break;

                DrawLine(bitmap, x + x_centre, y + y_centre, -x + x_centre, y + y_centre);

                DrawLine(bitmap, x + x_centre, -y + y_centre, -x + x_centre, -y + y_centre);

                if (x != y)
                {
                    DrawLine(bitmap, y + x_centre, x + y_centre, -y + x_centre, x + y_centre);

                    DrawLine(bitmap, y + x_centre, -x + y_centre, -y + x_centre, -x + y_centre);
                }
            }
        }
        private void DrawLine(DirectBitmap bitmap, int x0, int y0, int x1, int y1)
        {
            if (Math.Abs(y1 - y0) < Math.Abs(x1 - x0))
            {
                if (x0 > x1)
                {
                    DrawLineLow(bitmap, x1, y1, x0, y0);
                }
                else
                {
                    DrawLineLow(bitmap, x0, y0, x1, y1);
                }
            }
            else
            {
                if (y0 > y1)
                {
                    DrawLineHigh(bitmap, x1, y1, x0, y0);
                }
                else
                {
                    DrawLineHigh(bitmap, x0, y0, x1, y1);
                }
            }
        }

        private void DrawLineLow(DirectBitmap bitmap, int x0, int y0, int x1, int y1)
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
                if (x >= 0 && y >= 0 && x < bitmap.Width && y < bitmap.Height)
                {
                    // Color oldColor = imageCopy.GetPixel(x, y);
                    // Color newColor = Color.FromArgb(255 - oldColor.R, 255 - oldColor.G, 255 - oldColor.B);
                    Color color = GetColor(x, y);
                    bitmap.SetPixel(x, y, color);
                }
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

        private void DrawLineHigh(DirectBitmap bitmap, int x0, int y0, int x1, int y1)
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
                if (x >= 0 && y >= 0 && x < bitmap.Width && y < bitmap.Height)
                {
                    Color color = GetColor(x, y);
                    bitmap.SetPixel(x, y, color);
                }
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

        public Color GetColor(int x, int y)
        {
            Color old = imageCopy.GetPixel(x, y);
            if (eraseRadioButton.Checked)
            {
                return imageBackup.GetPixel(x, y);
            }
            else if (antifilterRadioButton.Checked)
            {
                return Color.FromArgb(antifilterArr[old.R], antifilterArr[old.G], antifilterArr[old.B]);
            }
            else if (negationRadioButton.Checked)
            {
                return Color.FromArgb(negationArr[old.R], negationArr[old.G], negationArr[old.B]);
            }
            else if (brightnessRadioButton.Checked)
            {
                return Color.FromArgb(brightnessArr[old.R], brightnessArr[old.G], brightnessArr[old.B]);
            }
            else if (gammaRadioButton.Checked)
            {
                return Color.FromArgb(gammaArr[old.R], gammaArr[old.G], gammaArr[old.B]);
            }
            else if (contrastRadioButton.Checked)
            {
                return Color.FromArgb(contrastArr[old.R], contrastArr[old.G], contrastArr[old.B]);
            }
            else if (ownRadioButton.Checked)
            {
                return Color.FromArgb(ownArr[old.R], ownArr[old.G], ownArr[old.B]);
            }
            else return Color.White;
        }

        public void CountHistogram()
        {
            Array.Clear(histogramRed, 0, histogramSize);
            Array.Clear(histogramGreen, 0, histogramSize);
            Array.Clear(histogramBlue, 0, histogramSize);

            for (int i = 0; i < drawArea.Height; i++)
            {
                for (int j = 0; j < drawArea.Width; j++)
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

        public void DrawFilterChart()
        {
            filterChart.Series["filter"].Points.Clear();
            filterChart.Series["bezierPoints"].Points.Clear();

            if (eraseRadioButton.Checked)
            {
                filterChart.Series["filter"].Points.Add(new DataPoint(-1, -1));
            }
            else if(antifilterRadioButton.Checked)
            {
                for (int i = 0; i < antifilterArr.Length; i++)
                {
                    filterChart.Series["filter"].Points.Add(new DataPoint(i, antifilterArr[i]));
                }
            }
            else if (negationRadioButton.Checked)
            {
                for (int i = 0; i < negationArr.Length; i++)
                {
                    filterChart.Series["filter"].Points.Add(new DataPoint(i, negationArr[i]));
                }
            }
            else if (brightnessRadioButton.Checked)
            {
                for (int i = 0; i < brightnessArr.Length; i++)
                {
                    filterChart.Series["filter"].Points.Add(new DataPoint(i, brightnessArr[i]));
                }
            }
            else if (gammaRadioButton.Checked)
            {
                for (int i = 0; i < gammaArr.Length; i++)
                {
                    filterChart.Series["filter"].Points.Add(new DataPoint(i, gammaArr[i]));
                }
            }
            else if (contrastRadioButton.Checked)
            {
                for (int i = 0; i < contrastArr.Length; i++)
                {
                    filterChart.Series["filter"].Points.Add(new DataPoint(i, contrastArr[i]));
                }
            }
            else if (ownRadioButton.Checked)
            {
                for (int i = 0; i < ownArr.Length; i++)
                {
                    filterChart.Series["filter"].Points.Add(new DataPoint(i, ownArr[i]));
                }

                foreach (var point in bezierPoints)
                {
                    filterChart.Series["bezierPoints"].Points.Add(new DataPoint(point.x, point.y));
                }
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

            using(Graphics g = Graphics.FromImage(imageCopy.Bitmap))
            {
                g.DrawImage(drawArea.Bitmap, 0, 0);
            }

            circles.Clear();
            RedrawImage();
            CountHistogram();
            
        }

        private void eraseRadioButton_Click(object sender, EventArgs e)
        {
            DrawFilterChart();
        }

        private void negationRadioButton_Click(object sender, EventArgs e)
        {
            DrawFilterChart();
        }

        private void brightnessRadioButton_Click(object sender, EventArgs e)
        {
            DrawFilterChart();
        }

        private void gammaRadioButton_Click(object sender, EventArgs e)
        {
            DrawFilterChart();
        }

        private void contrastRadioButton_Click(object sender, EventArgs e)
        {
            DrawFilterChart();
        }

        private void antifilterRadioButton_Click(object sender, EventArgs e)
        {
            DrawFilterChart();
        }

        private void ownRadioButton_Click(object sender, EventArgs e)
        {
            DrawFilterChart();
        }

        private void filterChart_MouseDown(object sender, MouseEventArgs e)
        {
            filterChart.ChartAreas[0].CursorX.Position = double.NaN;
            filterChart.ChartAreas[0].CursorY.Position = double.NaN;
            filterChart.ChartAreas[0].CursorX.SetCursorPixelPosition(new Point(e.X, e.Y), false);
            filterChart.ChartAreas[0].CursorY.SetCursorPixelPosition(new Point(e.X, e.Y), false);
            double px = filterChart.ChartAreas[0].CursorX.Position;
            double py = filterChart.ChartAreas[0].CursorY.Position;
            for (int i = 0; i < bezierPoints.Length; i++)
            {
                var point = bezierPoints[i];
                if ((point.x - px) * (point.x - px) + (point.y - py) * (point.y - py) < bezierRadius * bezierRadius)
                {
                    movingBezier = true;
                    bezierIndex = i;
                }
            }
        }

        private void filterChart_MouseMove(object sender, MouseEventArgs e)
        {
            if (movingBezier)
            {
                // filterChart.ChartAreas[0].CursorX.Position = double.NaN;
                // filterChart.ChartAreas[0].CursorY.Position = double.NaN;
                filterChart.ChartAreas[0].CursorX.SetCursorPixelPosition(new Point(e.X, e.Y), false);
                filterChart.ChartAreas[0].CursorY.SetCursorPixelPosition(new Point(e.X, e.Y), false);
                int px = (int)filterChart.ChartAreas[0].CursorX.Position;
                int py = (int)filterChart.ChartAreas[0].CursorY.Position;

                if (px < 0 || bezierIndex == 0) px = 0;
                if (px > 255 || bezierIndex == 3) px = 255;
                if (py < 0) py = 0;
                if (py > 255) py = 255;

                bezierPoints[bezierIndex].x = px;
                bezierPoints[bezierIndex].y = py;
                filterChart.Series["bezierPoints"].Points.Clear();
                filterChart.Series["filter"].Points.Clear();

                CountBezier();

                for (int i = 0; i < ownArr.Length; i++)
                {
                    filterChart.Series["filter"].Points.Add(new DataPoint(i, ownArr[i]));
                }

                for (int i = 0; i < bezierPoints.Length; i++)
                {
                    filterChart.Series["bezierPoints"].Points.Add(new DataPoint(bezierPoints[i].x, bezierPoints[i].y));
                }

                
            }
        }

        public void CountBezier()
        {
            MyPoint V0 = bezierPoints[0];
            MyPoint V1 = bezierPoints[1];
            MyPoint V2 = bezierPoints[2];
            MyPoint V3 = bezierPoints[3];

            for (int i = 0; i < ownArr.Length; i++)
            {
                ownArr[i] = -1;
            }

            int A0x = V0.x;
            int A0y = V0.y;
            int A1x = 3 * (V1.x - V0.x);
            int A1y = 3 * (V1.y - V0.y);
            int A2x = 3 * (V2.x - 2 * V1.x + V0.x);
            int A2y = 3 * (V2.y - 2 * V1.y + V0.y);
            int A3x = V3.x - 3 * V2.x + 3 * V1.x - V0.x;
            int A3y = V3.y - 3 * V2.y + 3 * V1.y - V0.y;

            for (double t = 0.0; t <= 1.0; t += 0.001)
            {
                double tx = A0x + t * (A1x + t * (A2x + t * A3x));
                double ty = A0y + t * (A1y + t * (A2y + t * A3y));
                // tx *= 255;
                // ty *= 255;
                // Debug.WriteLine($"{tx}, {ty}");
                if (ty < 0) ty = 0;
                if (ty > 255) ty = 255;
                ownArr[(int)Math.Round(tx, 0)] = (int)Math.Round(ty, 0);
            }

            for (int i = 0; i < ownArr.Length; i++)
            {
                if (ownArr[i] == -1)
                {
                    if (i == 0) ownArr[i] = ownArr[i + 1];
                    else if (i == 255) ownArr[i] = ownArr[i - 1];
                    else ownArr[i] = (ownArr[i - 1] + ownArr[i + 1]) / 2;
                }
            }
        }

        private void filterChart_MouseUp(object sender, MouseEventArgs e)
        {
            movingBezier = false;
        }

        private void fillButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < drawArea.Height; i++)
            {
                for (int j = 0; j < drawArea.Width; j++)
                {
                    imageCopy.SetPixel(i, j, GetColor(i, j));
                }
            }
            RedrawImage();
            CountHistogram();
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = Environment.CurrentDirectory + @".\lib";
            DialogResult result = ofd.ShowDialog();
            if (result == DialogResult.OK)
            {
                string filename = ofd.FileName;
                LoadImage(filename);
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

    public class MyPoint
    {
        public int x;
        public int y;

        public MyPoint(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
