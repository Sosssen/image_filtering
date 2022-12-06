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
    public partial class IF : Form
    {
        private int mode = 0; // 0 - fill whole image, 1 - fill with brush, 2 - fill with polygon

        private DirectBitmap drawArea = null;
        private DirectBitmap imageCopy = null;
        private DirectBitmap imageBackup = null;
        private Bitmap image = null;

        private int[] antifilterArr = new int[256];
        private int[] negationArr = new int[256];
        private int[] brightnessArr = new int[256];
        private int brightnessConst = 30;
        private int[] gammaArr = new int[256];
        private double gammaConst = 0.5;
        private int[] contrastArr = new int[256];
        private int contrastConst = 40;
        private int contrastConst2 = 0;
        private int contrastConst3 = 255;
        private int[] ownArr = new int[256];
        

        private static int histogramSize = 256;
        private int[] histogramRed = new int[histogramSize];
        private int[] histogramGreen = new int[histogramSize];
        private int[] histogramBlue = new int[histogramSize];

        private Pen pen = new Pen(Color.Black, 1);
        private SolidBrush sbBlack = new SolidBrush(Color.Black);
        private Pen penRed = new Pen(Color.Red, 1);
        private SolidBrush sbRed = new SolidBrush(Color.Red);
        private int radius = 30;
        private HashSet<Point> circles = new HashSet<Point>();

        private int moving = 0; // 0 - not moving, 1 - moving LPM, 2 - moving PPM

        private MyPoint[] bezierPoints = new MyPoint[4];
        private bool movingBezier = false;
        private int bezierIndex = 0;
        private int bezierRadius = 5;

        private List<MyPoint> points = new List<MyPoint>();
        private int pointRadius = 3;
        public IF()
        {
            InitializeComponent();

            Configuration();
        }

        public void Configuration()
        {
            this.Text = "Image Filtering";
            this.Icon = Properties.Resources.icon_if;

            Rectangle screen = Screen.PrimaryScreen.WorkingArea;
            int w = (int)(screen.Width / 1.15);
            int h = (int)(screen.Height / 1.15);
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
            filterChart.Series["filter"].BorderWidth = 3;
            filterChart.Series["bezierPoints"].Color = Color.Red;
            filterChart.Series["dashedLine"].BorderDashStyle = ChartDashStyle.Dot;

            brightnessUpDown.Value = brightnessConst;
            gammaUpDown.Value = Convert.ToDecimal(gammaConst);
            contrastUpDown.Value = contrastConst;
            contrastUpDown2.Value = contrastConst2;
            contrastUpDown3.Value = contrastConst3;
            contrastUpDown2.Maximum = contrastUpDown3.Value - 1;
            contrastUpDown3.Minimum = contrastUpDown2.Value + 1;
            brushUpDown.Value = radius;

            InitializeFilterArrays();

            DrawFilterChart();

            string filename = @".\lib\lenna.png";
            LoadImage(filename);
        }

        public void InitializeAntifilterArray()
        {
            for (int i = 0; i < antifilterArr.Length; i++)
            {
                antifilterArr[i] = i;
            }
        }

        public void InitializeNegationArray()
        {
            for (int i = 0; i < negationArr.Length; i++)
            {
                negationArr[i] = 255 - i;
            }
        }

        public void InitializeBrightnessArray()
        {
            if (brightnessConst >= 0)
            {
                for (int i = 0; i < 255 - brightnessConst; i++)
                {
                    brightnessArr[i] = i + brightnessConst;
                }

                for (int i = 255 - brightnessConst; i < brightnessArr.Length; i++)
                {
                    brightnessArr[i] = 255;
                }

                
            }
            else
            {

                for (int i = 0; i < -brightnessConst; i++)
                {
                    brightnessArr[i] = 0;
                }

                for (int i = -brightnessConst; i < brightnessArr.Length; i++)
                {
                    brightnessArr[i] = i + brightnessConst;
                }

            }
        }

        public void InitializeGammaArray()
        {
            for (int i = 0; i < gammaArr.Length; i++)
            {
                double tmp = (double)i / 255.0;
                tmp = Math.Pow(tmp, gammaConst);
                gammaArr[i] = (int)(tmp * 255);
            }
        }

        public void InitializeContrastArray()
        {
            for (int i = 0; i < contrastConst; i++)
            {
                contrastArr[i] = contrastConst2;
                contrastArr[contrastArr.Length - i - 1] = contrastConst3;
            }
            double h = (contrastConst3 - contrastConst2) / (255.0 - 2.0 * contrastConst);
            double currVal = contrastConst2;
            for (int i = contrastConst; i < contrastArr.Length - contrastConst; i++)
            {
                currVal += h;
                contrastArr[i] = Math.Min((int)currVal, 255);
            }
        }

        public void InitializeFilterArrays()
        {
            InitializeAntifilterArray();

            InitializeNegationArray();

            InitializeBrightnessArray();

            InitializeGammaArray();

            InitializeContrastArray();

            bezierPoints[0] = new MyPoint(0, 0);
            bezierPoints[1] = new MyPoint(50, 200);
            bezierPoints[2] = new MyPoint(200, 50);
            bezierPoints[3] = new MyPoint(255, 255);

            InitializeOwnArray();
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

        public void RedrawImage(int x = -1, int y = -1)
        {
            using (Graphics g = Graphics.FromImage(drawArea.Bitmap))
            {
                g.DrawImage(imageCopy.Bitmap, 0, 0);
                foreach (var point in circles)
                {
                    midPointCircleDraw(drawArea, point.X, point.Y, radius, Color.Red);
                }
            }

            if (mode == 1 && x != -1 && y != -1)
            {
                using (Graphics g = Graphics.FromImage(drawArea.Bitmap))
                {
                    g.DrawEllipse(pen, x - radius, y - radius, 2 * radius, 2 * radius);
                }
            }
            else if (mode == 2 && x != -1 && y != -1)
            {
                using (Graphics g = Graphics.FromImage(drawArea.Bitmap))
                {
                    for (int i = 0; i < points.Count - 1; i++)
                    {
                        g.DrawLine(pen, points[i].x, points[i].y, points[i + 1].x, points[i + 1].y);
                    }
                    if (points.Count > 0)
                    {
                        g.DrawLine(pen, points[points.Count - 1].x, points[points.Count - 1].y, x, y);
                    }
                    foreach (var point in points)
                    {
                        g.DrawEllipse(pen, point.x - pointRadius, point.y - pointRadius, 2 * pointRadius, 2 * pointRadius);
                        g.FillEllipse(sbBlack, point.x - 3, point.y - 3, 2 * 3, 2 * 3);
                    }
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
                    Color color = imageCopy.GetPixel(i, j);
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
            filterChart.Series["dashedLine"].Points.Clear();
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
                    filterChart.Series["dashedLine"].Points.Add(new DataPoint(point.x, point.y));
                    filterChart.Series["bezierPoints"].Points.Add(new DataPoint(point.x, point.y));
                }
            }
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (mode == 1)
            {
                if (moving == 1)
                {
                    circles.Add(new Point(e.X, e.Y));
                }
                RedrawImage(e.X, e.Y);
            }
            else if (mode == 2)
            {
                RedrawImage(e.X, e.Y);
            }
        }

        private void Canvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (mode == 1)
                {
                    moving = 1;
                    circles.Add(new Point(e.X, e.Y));

                    RedrawImage(e.X, e.Y);
                }
                else if (mode == 2)
                {
                    foreach (var point in points)
                    {
                        if ((point.x - e.X) * (point.x - e.X) + (point.y - e.Y) * (point.y - e.Y) <= pointRadius * pointRadius)
                        {
                            if (point != points[0])
                            {
                                MessageBox.Show("You can connect only with first node!", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                DrawRectangle(points);
                                points.Clear();
                                RedrawImage(-1, -1);
                            }
                            return;
                        }
                    }
                    points.Add(new MyPoint(e.X, e.Y));
                    RedrawImage(e.X, e.Y);
                }
            }
        }

        public void DrawRectangle(List<MyPoint> points)
        {
            Polygon polygon = new Polygon();
            polygon.points = points;
            polygon.edges = new List<Edge>();
            for (int i = 0; i < points.Count; i++)
            {
                polygon.edges.Add(new Edge(points[i], points[(i + 1) % points.Count]));
            }
            ScanlineFill(polygon);
        }

        private void Canvas_MouseUp(object sender, MouseEventArgs e)
        {
            moving = 0;

            RedrawImage();

            using (Graphics g = Graphics.FromImage(imageCopy.Bitmap))
            {
                g.DrawImage(drawArea.Bitmap, 0, 0);
            }

            // TODO: dont count rectangle points in histograms
            circles.Clear();
            RedrawImage(e.X, e.Y);
            CountHistogram();
            
        }

        private void eraseRadioButton_Click(object sender, EventArgs e)
        {
            brightnessUpDown.Enabled = false;
            gammaUpDown.Enabled = false;
            contrastUpDown.Enabled = false;
            contrastUpDown2.Enabled = false;
            contrastUpDown3.Enabled = false;
            DrawFilterChart();
        }

        private void negationRadioButton_Click(object sender, EventArgs e)
        {
            brightnessUpDown.Enabled = false;
            gammaUpDown.Enabled = false;
            contrastUpDown.Enabled = false;
            contrastUpDown2.Enabled = false;
            contrastUpDown3.Enabled = false;
            DrawFilterChart();
        }

        private void brightnessRadioButton_Click(object sender, EventArgs e)
        {
            brightnessUpDown.Enabled = true;
            gammaUpDown.Enabled = false;
            contrastUpDown.Enabled = false;
            contrastUpDown2.Enabled = false;
            contrastUpDown3.Enabled = false;
            DrawFilterChart();
        }

        private void gammaRadioButton_Click(object sender, EventArgs e)
        {
            brightnessUpDown.Enabled = false;
            gammaUpDown.Enabled = true;
            contrastUpDown.Enabled = false;
            contrastUpDown2.Enabled = false;
            contrastUpDown3.Enabled = false;
            DrawFilterChart();
        }

        private void contrastRadioButton_Click(object sender, EventArgs e)
        {
            brightnessUpDown.Enabled = false;
            gammaUpDown.Enabled = false;
            contrastUpDown.Enabled = true;
            contrastUpDown2.Enabled = true;
            contrastUpDown3.Enabled = true;
            DrawFilterChart();
        }

        private void antifilterRadioButton_Click(object sender, EventArgs e)
        {
            brightnessUpDown.Enabled = false;
            gammaUpDown.Enabled = false;
            contrastUpDown.Enabled = false;
            contrastUpDown2.Enabled = false;
            contrastUpDown3.Enabled = false;
            DrawFilterChart();
        }

        private void ownRadioButton_Click(object sender, EventArgs e)
        {
            brightnessUpDown.Enabled = false;
            gammaUpDown.Enabled = false;
            contrastUpDown.Enabled = false;
            contrastUpDown2.Enabled = false;
            contrastUpDown3.Enabled = false;
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
                if ((point.x - px) * (point.x - px) + (point.y - py) * (point.y - py) <= bezierRadius * bezierRadius)
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
                filterChart.Series["dashedLine"].Points.Clear();
                filterChart.Series["filter"].Points.Clear();

                InitializeOwnArray();

                for (int i = 0; i < ownArr.Length; i++)
                {
                    filterChart.Series["filter"].Points.Add(new DataPoint(i, ownArr[i]));
                }

                for (int i = 0; i < bezierPoints.Length; i++)
                {
                    filterChart.Series["dashedLine"].Points.Add(new DataPoint(bezierPoints[i].x, bezierPoints[i].y));
                    filterChart.Series["bezierPoints"].Points.Add(new DataPoint(bezierPoints[i].x, bezierPoints[i].y));
                }

                
            }
        }

        public void InitializeOwnArray()
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
            brushUpDown.Enabled = false;
            mode = 0;

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

        private void brushButton_Click(object sender, EventArgs e)
        {
            brushUpDown.Enabled = true;
            mode = 1;
            RedrawImage();
        }

        private void rectangleButton_Click(object sender, EventArgs e)
        {
            brushUpDown.Enabled = false;
            points.Clear();
            mode = 2;
            RedrawImage();
        }

        private static readonly int maxVer = 100;
        private static int maxHt = 1000;
        private static int minY;
        private static int maxY;

        public class EdgeBucket
        {
            public int ymax;
            public double xofymin;
            public double slopeinverse;
        }

        public class EdgeTableTuple
        {
            public int countEdgeBucket;
            public EdgeBucket[] buckets = new EdgeBucket[maxVer];
        }

        private static EdgeTableTuple[] edgeTable;
        public static EdgeTableTuple ActiveEdgeTuple = new EdgeTableTuple();

        public static EdgeTableTuple[] EdgeTable { get => edgeTable; set => edgeTable = value; }

        public static void InitEdgeTable()
        {
            maxHt = maxY - minY + 1;
            EdgeTable = new EdgeTableTuple[maxHt];
            for (int i = 0; i < maxHt; i++)
            {
                EdgeTable[i] = new EdgeTableTuple();
                EdgeTable[i].countEdgeBucket = 0;
                for (int j = 0; j < maxVer; j++)
                {
                    EdgeTable[i].buckets[j] = new EdgeBucket();
                }
            }
            for (int i = 0; i < maxVer; i++)
            {
                ActiveEdgeTuple.buckets[i] = new EdgeBucket();
            }
            ActiveEdgeTuple.countEdgeBucket = 0;
        }

        static void InsertionSort(EdgeTableTuple ett)
        {
            int i, j;
            EdgeBucket temp = new EdgeBucket();

            for (i = 1; i < ett.countEdgeBucket; i++)
            {
                temp.ymax = ett.buckets[i].ymax;
                temp.xofymin = ett.buckets[i].xofymin;
                temp.slopeinverse = ett.buckets[i].slopeinverse;
                j = i - 1;
                while ((j >= 0) && (temp.xofymin < ett.buckets[j].xofymin))
                {
                    ett.buckets[j + 1].ymax = ett.buckets[j].ymax;
                    ett.buckets[j + 1].xofymin = ett.buckets[j].xofymin;
                    ett.buckets[j + 1].slopeinverse = ett.buckets[j].slopeinverse;
                    j--;
                }
                ett.buckets[j + 1].ymax = temp.ymax;
                ett.buckets[j + 1].xofymin = temp.xofymin;
                ett.buckets[j + 1].slopeinverse = temp.slopeinverse;
            }
        }

        public static void StoreEdgeInTuple(EdgeTableTuple receiver, int ym, int xm, double slopInv)
        {
            receiver.buckets[receiver.countEdgeBucket].ymax = ym;
            receiver.buckets[receiver.countEdgeBucket].xofymin = (double)xm;
            receiver.buckets[receiver.countEdgeBucket].slopeinverse = slopInv;

            InsertionSort(receiver);

            receiver.countEdgeBucket++;
        }

        public static void StoreEdgeInTable(int x1, int y1, int x2, int y2)
        {
            double m, minv;
            int ymaxTS, xwithyminTS, scanline;

            if (x2 == x1)
            {
                minv = 0.0;
            }
            else
            {
                m = ((double)(y2 - y1)) / ((double)(x2 - x1));
                if (y2 == y1) return;

                minv = (double)(1.0 / m);
            }

            if (y1 > y2)
            {
                scanline = y2;
                ymaxTS = y1;
                xwithyminTS = x2;
            }
            else
            {
                scanline = y1;
                ymaxTS = y2;
                xwithyminTS = x1;
            }

            StoreEdgeInTuple(EdgeTable[scanline - minY], ymaxTS, xwithyminTS, minv);
        }

        public static void RemoveEdgeByYmax(EdgeTableTuple Tup, int yy)
        {
            int i, j;
            for (i = 0; i < Tup.countEdgeBucket; i++)
            {
                if (Tup.buckets[i].ymax == yy)
                {
                    for (j = i; j < Tup.countEdgeBucket - 1; j++)
                    {
                        Tup.buckets[j].ymax = Tup.buckets[j + 1].ymax;
                        Tup.buckets[j].xofymin = Tup.buckets[j + 1].xofymin;
                        Tup.buckets[j].slopeinverse = Tup.buckets[j + 1].slopeinverse;
                    }
                    Tup.countEdgeBucket--;
                    i--;
                }
            }
        }

        public static void Updatexbyslopeinv(EdgeTableTuple Tup)
        {
            int i;

            for (i = 0; i < Tup.countEdgeBucket; i++)
            {
                Tup.buckets[i].xofymin = Tup.buckets[i].xofymin + Tup.buckets[i].slopeinverse;
            }
        }

        public void ScanlineFill(Polygon polygon)
        {
            minY = int.MaxValue;
            maxY = int.MinValue;

            foreach (var edge in polygon.edges)
            {
                if (edge.src.y < minY) minY = (int)edge.src.y;
                if (edge.src.y > maxY) maxY = (int)edge.src.y;
                if (edge.dst.y < minY) minY = (int)edge.dst.y;
                if (edge.dst.y > maxY) maxY = (int)edge.dst.y;
            }

            InitEdgeTable();

            foreach (var edge in polygon.edges)
            {
                StoreEdgeInTable((int)edge.src.x, (int)edge.src.y, (int)edge.dst.x, (int)edge.dst.y);
            }

            int i, j, x1, ymax1, x2, ymax2, coordCount;
            for (i = 0; i < maxHt; i++)
            {
                for (j = 0; j < EdgeTable[i].countEdgeBucket; j++)
                {
                    StoreEdgeInTuple(ActiveEdgeTuple, EdgeTable[i].buckets[j].ymax, (int)EdgeTable[i].buckets[j].xofymin, EdgeTable[i].buckets[j].slopeinverse);
                }

                RemoveEdgeByYmax(ActiveEdgeTuple, i + minY);

                InsertionSort(ActiveEdgeTuple);

                j = 0;
                coordCount = 0;
                x1 = 0;
                x2 = 0;
                ymax1 = 0;
                ymax2 = 0;
                while (j < ActiveEdgeTuple.countEdgeBucket)
                {
                    if (coordCount % 2 == 0)
                    {
                        x1 = (int)(ActiveEdgeTuple.buckets[j].xofymin);
                        ymax1 = ActiveEdgeTuple.buckets[j].ymax;
                        if (x1 == x2)
                        {
                            if (((x1 == ymax1) && (x2 != ymax2)) || ((x1 != ymax1) && (x2 == ymax2)))
                            {
                                x2 = x1;
                                ymax2 = ymax1;
                            }
                            else
                            {
                                coordCount++;
                            }
                        }
                        else
                        {
                            coordCount++;
                        }
                    }
                    else
                    {
                        x2 = (int)ActiveEdgeTuple.buckets[j].xofymin;
                        ymax2 = ActiveEdgeTuple.buckets[j].ymax;

                        int FillFlag = 0;

                        if (x1 == x2)
                        {
                            if (((x1 == ymax1) && (x2 != ymax2)) || ((x1 != ymax1) && (x2 == ymax2)))
                            {
                                x1 = x2;
                                ymax1 = ymax2;
                            }
                            else
                            {
                                coordCount++;
                                FillFlag = 1;
                            }
                        }
                        else
                        {
                            coordCount++;
                            FillFlag = 1;
                        }

                        if (FillFlag == 1)
                        {
                            for (int k = x1; k <= x2; k++)
                            {
                                imageCopy.SetPixel(k, i + minY, GetColor(k, i + minY));
                            }
                        }
                    }
                    j++;
                }
                Updatexbyslopeinv(ActiveEdgeTuple);
            }
        }

        private void brightnessUpDown_ValueChanged(object sender, EventArgs e)
        {
            brightnessConst = (int)brightnessUpDown.Value;
            InitializeBrightnessArray();
            DrawFilterChart();
        }

        private void gammaUpDown_ValueChanged(object sender, EventArgs e)
        {
            gammaConst = (double)gammaUpDown.Value;
            InitializeGammaArray();
            DrawFilterChart();
        }

        private void contrastUpDown_ValueChanged(object sender, EventArgs e)
        {
            contrastConst = (int)contrastUpDown.Value;
            InitializeContrastArray();
            DrawFilterChart();
        }

        private void contrastUpDown2_ValueChanged(object sender, EventArgs e)
        {
            contrastUpDown3.Minimum = contrastUpDown2.Value + 1;
            contrastConst2 = (int)contrastUpDown2.Value;
            InitializeContrastArray();
            DrawFilterChart();
        }

        private void contrastUpDown3_ValueChanged(object sender, EventArgs e)
        {
            contrastUpDown2.Maximum = contrastUpDown3.Value - 1;
            contrastConst3 = (int)contrastUpDown3.Value;
            InitializeContrastArray();
            DrawFilterChart();
        }

        private void brushUpDown_ValueChanged(object sender, EventArgs e)
        {
            radius = (int)brushUpDown.Value;
        }

        private void Canvas_MouseLeave(object sender, EventArgs e)
        {
            RedrawImage(-1, -1);
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

    public class Polygon
    {
        public List<MyPoint> points;
        public List<Edge> edges;
        public int size;

        public Polygon()
        {
            this.points = null;
            this.edges = null;
            this.size = 0;
        }

        public Polygon(List<MyPoint> points, List<Edge> edges, int size)
        {
            this.points = points;
            this.edges = edges;
            this.size = size;
        }
    }

    public class Edge
    {
        public MyPoint src;
        public MyPoint dst;

        public Edge(MyPoint src, MyPoint dst)
        {
            this.src = src;
            this.dst = dst;
        }
    }
}
