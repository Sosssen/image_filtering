
namespace image_filtering
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint1 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(0D, 0D);
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint2 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(10D, 10D);
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint3 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(0D, 0D);
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint4 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(10D, 10D);
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint5 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(0D, 0D);
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint6 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(10D, 10D);
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint7 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(0D, 0D);
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint8 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(10D, 10D);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.Canvas = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.blueChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.redChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.greenChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.eraseRadioButton = new System.Windows.Forms.RadioButton();
            this.ownRadioButton = new System.Windows.Forms.RadioButton();
            this.contrastRadioButton = new System.Windows.Forms.RadioButton();
            this.gammaRadioButton = new System.Windows.Forms.RadioButton();
            this.negationRadioButton = new System.Windows.Forms.RadioButton();
            this.antifilterRadioButton = new System.Windows.Forms.RadioButton();
            this.brightnessRadioButton = new System.Windows.Forms.RadioButton();
            this.brightnessUpDown = new System.Windows.Forms.NumericUpDown();
            this.filterChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.loadButton = new System.Windows.Forms.Button();
            this.fillButton = new System.Windows.Forms.Button();
            this.brushButton = new System.Windows.Forms.Button();
            this.rectangleButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Canvas)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.blueChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.redChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.greenChart)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.brightnessUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filterChart)).BeginInit();
            this.tableLayoutPanel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Controls.Add(this.Canvas, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1574, 1129);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // Canvas
            // 
            this.Canvas.Location = new System.Drawing.Point(3, 3);
            this.Canvas.Name = "Canvas";
            this.Canvas.Size = new System.Drawing.Size(781, 1123);
            this.Canvas.TabIndex = 0;
            this.Canvas.TabStop = false;
            this.Canvas.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseDown);
            this.Canvas.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseMove);
            this.Canvas.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseUp);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.blueChart, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.redChart, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.greenChart, 0, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(1261, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 4;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(310, 1123);
            this.tableLayoutPanel3.TabIndex = 2;
            // 
            // blueChart
            // 
            chartArea1.Name = "ChartArea1";
            this.blueChart.ChartAreas.Add(chartArea1);
            this.blueChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.blueChart.Location = new System.Drawing.Point(3, 563);
            this.blueChart.Name = "blueChart";
            this.blueChart.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
            this.blueChart.PaletteCustomColors = new System.Drawing.Color[] {
        System.Drawing.Color.Blue};
            series1.ChartArea = "ChartArea1";
            series1.Name = "blue";
            series1.Points.Add(dataPoint1);
            series1.Points.Add(dataPoint2);
            this.blueChart.Series.Add(series1);
            this.blueChart.Size = new System.Drawing.Size(304, 274);
            this.blueChart.TabIndex = 2;
            this.blueChart.Text = "chart1";
            // 
            // redChart
            // 
            chartArea2.Name = "ChartArea1";
            this.redChart.ChartAreas.Add(chartArea2);
            this.redChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.redChart.Location = new System.Drawing.Point(3, 3);
            this.redChart.Name = "redChart";
            this.redChart.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
            this.redChart.PaletteCustomColors = new System.Drawing.Color[] {
        System.Drawing.Color.Red};
            series2.ChartArea = "ChartArea1";
            series2.Name = "red";
            series2.Points.Add(dataPoint3);
            series2.Points.Add(dataPoint4);
            this.redChart.Series.Add(series2);
            this.redChart.Size = new System.Drawing.Size(304, 274);
            this.redChart.TabIndex = 0;
            this.redChart.Text = "chart1";
            // 
            // greenChart
            // 
            chartArea3.Name = "ChartArea1";
            this.greenChart.ChartAreas.Add(chartArea3);
            this.greenChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.greenChart.Location = new System.Drawing.Point(3, 283);
            this.greenChart.Name = "greenChart";
            this.greenChart.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
            this.greenChart.PaletteCustomColors = new System.Drawing.Color[] {
        System.Drawing.Color.Lime};
            series3.ChartArea = "ChartArea1";
            series3.Name = "green";
            series3.Points.Add(dataPoint5);
            series3.Points.Add(dataPoint6);
            this.greenChart.Series.Add(series3);
            this.greenChart.Size = new System.Drawing.Size(304, 274);
            this.greenChart.TabIndex = 1;
            this.greenChart.Text = "chart1";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel4, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.filterChart, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel5, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(947, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(308, 1123);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel4.Controls.Add(this.eraseRadioButton, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.ownRadioButton, 0, 6);
            this.tableLayoutPanel4.Controls.Add(this.contrastRadioButton, 0, 5);
            this.tableLayoutPanel4.Controls.Add(this.gammaRadioButton, 0, 4);
            this.tableLayoutPanel4.Controls.Add(this.negationRadioButton, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.antifilterRadioButton, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.brightnessRadioButton, 0, 3);
            this.tableLayoutPanel4.Controls.Add(this.brightnessUpDown, 1, 3);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 377);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 7;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(302, 368);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // eraseRadioButton
            // 
            this.eraseRadioButton.AutoSize = true;
            this.eraseRadioButton.Location = new System.Drawing.Point(3, 3);
            this.eraseRadioButton.Name = "eraseRadioButton";
            this.eraseRadioButton.Size = new System.Drawing.Size(144, 29);
            this.eraseRadioButton.TabIndex = 0;
            this.eraseRadioButton.Text = "erase filter";
            this.eraseRadioButton.UseVisualStyleBackColor = true;
            this.eraseRadioButton.Click += new System.EventHandler(this.eraseRadioButton_Click);
            // 
            // ownRadioButton
            // 
            this.ownRadioButton.AutoSize = true;
            this.ownRadioButton.Location = new System.Drawing.Point(3, 315);
            this.ownRadioButton.Name = "ownRadioButton";
            this.ownRadioButton.Size = new System.Drawing.Size(164, 29);
            this.ownRadioButton.TabIndex = 5;
            this.ownRadioButton.Text = "own function";
            this.ownRadioButton.UseVisualStyleBackColor = true;
            this.ownRadioButton.Click += new System.EventHandler(this.ownRadioButton_Click);
            // 
            // contrastRadioButton
            // 
            this.contrastRadioButton.AutoSize = true;
            this.contrastRadioButton.Location = new System.Drawing.Point(3, 263);
            this.contrastRadioButton.Name = "contrastRadioButton";
            this.contrastRadioButton.Size = new System.Drawing.Size(197, 29);
            this.contrastRadioButton.TabIndex = 4;
            this.contrastRadioButton.Text = "change contrast";
            this.contrastRadioButton.UseVisualStyleBackColor = true;
            this.contrastRadioButton.Click += new System.EventHandler(this.contrastRadioButton_Click);
            // 
            // gammaRadioButton
            // 
            this.gammaRadioButton.AutoSize = true;
            this.gammaRadioButton.Location = new System.Drawing.Point(3, 211);
            this.gammaRadioButton.Name = "gammaRadioButton";
            this.gammaRadioButton.Size = new System.Drawing.Size(190, 29);
            this.gammaRadioButton.TabIndex = 3;
            this.gammaRadioButton.Text = "change gamma";
            this.gammaRadioButton.UseVisualStyleBackColor = true;
            this.gammaRadioButton.Click += new System.EventHandler(this.gammaRadioButton_Click);
            // 
            // negationRadioButton
            // 
            this.negationRadioButton.AutoSize = true;
            this.negationRadioButton.Location = new System.Drawing.Point(3, 107);
            this.negationRadioButton.Name = "negationRadioButton";
            this.negationRadioButton.Size = new System.Drawing.Size(126, 29);
            this.negationRadioButton.TabIndex = 1;
            this.negationRadioButton.Text = "negation";
            this.negationRadioButton.UseVisualStyleBackColor = true;
            this.negationRadioButton.Click += new System.EventHandler(this.negationRadioButton_Click);
            // 
            // antifilterRadioButton
            // 
            this.antifilterRadioButton.AutoSize = true;
            this.antifilterRadioButton.Checked = true;
            this.antifilterRadioButton.Location = new System.Drawing.Point(3, 55);
            this.antifilterRadioButton.Name = "antifilterRadioButton";
            this.antifilterRadioButton.Size = new System.Drawing.Size(114, 29);
            this.antifilterRadioButton.TabIndex = 6;
            this.antifilterRadioButton.TabStop = true;
            this.antifilterRadioButton.Text = "nothing";
            this.antifilterRadioButton.UseVisualStyleBackColor = true;
            this.antifilterRadioButton.Click += new System.EventHandler(this.antifilterRadioButton_Click);
            // 
            // brightnessRadioButton
            // 
            this.brightnessRadioButton.AutoSize = true;
            this.brightnessRadioButton.Location = new System.Drawing.Point(3, 159);
            this.brightnessRadioButton.Name = "brightnessRadioButton";
            this.brightnessRadioButton.Size = new System.Drawing.Size(205, 29);
            this.brightnessRadioButton.TabIndex = 2;
            this.brightnessRadioButton.Text = "change brightness";
            this.brightnessRadioButton.UseVisualStyleBackColor = true;
            this.brightnessRadioButton.Click += new System.EventHandler(this.brightnessRadioButton_Click);
            // 
            // brightnessUpDown
            // 
            this.brightnessUpDown.Dock = System.Windows.Forms.DockStyle.Fill;
            this.brightnessUpDown.Location = new System.Drawing.Point(214, 159);
            this.brightnessUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.brightnessUpDown.Minimum = new decimal(new int[] {
            255,
            0,
            0,
            -2147483648});
            this.brightnessUpDown.Name = "brightnessUpDown";
            this.brightnessUpDown.Size = new System.Drawing.Size(85, 31);
            this.brightnessUpDown.TabIndex = 7;
            this.brightnessUpDown.ValueChanged += new System.EventHandler(this.brightnessUpDown_ValueChanged);
            // 
            // filterChart
            // 
            chartArea4.Name = "ChartArea1";
            this.filterChart.ChartAreas.Add(chartArea4);
            this.filterChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.filterChart.Location = new System.Drawing.Point(3, 751);
            this.filterChart.Name = "filterChart";
            this.filterChart.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
            this.filterChart.PaletteCustomColors = new System.Drawing.Color[] {
        System.Drawing.Color.Blue};
            series4.ChartArea = "ChartArea1";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series4.Name = "bezierPoints";
            series4.YValuesPerPoint = 2;
            series5.ChartArea = "ChartArea1";
            series5.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series5.Name = "filter";
            series5.Points.Add(dataPoint7);
            series5.Points.Add(dataPoint8);
            this.filterChart.Series.Add(series4);
            this.filterChart.Series.Add(series5);
            this.filterChart.Size = new System.Drawing.Size(302, 369);
            this.filterChart.TabIndex = 3;
            this.filterChart.Text = "chart1";
            this.filterChart.MouseDown += new System.Windows.Forms.MouseEventHandler(this.filterChart_MouseDown);
            this.filterChart.MouseMove += new System.Windows.Forms.MouseEventHandler(this.filterChart_MouseMove);
            this.filterChart.MouseUp += new System.Windows.Forms.MouseEventHandler(this.filterChart_MouseUp);
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Controls.Add(this.loadButton, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.fillButton, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.brushButton, 0, 2);
            this.tableLayoutPanel5.Controls.Add(this.rectangleButton, 0, 3);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 4;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(302, 368);
            this.tableLayoutPanel5.TabIndex = 4;
            // 
            // loadButton
            // 
            this.loadButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.loadButton.Location = new System.Drawing.Point(3, 3);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(296, 86);
            this.loadButton.TabIndex = 0;
            this.loadButton.Text = "load file";
            this.loadButton.UseVisualStyleBackColor = true;
            this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
            // 
            // fillButton
            // 
            this.fillButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fillButton.Location = new System.Drawing.Point(3, 95);
            this.fillButton.Name = "fillButton";
            this.fillButton.Size = new System.Drawing.Size(296, 86);
            this.fillButton.TabIndex = 1;
            this.fillButton.Text = "fill whole image";
            this.fillButton.UseVisualStyleBackColor = true;
            this.fillButton.Click += new System.EventHandler(this.fillButton_Click);
            // 
            // brushButton
            // 
            this.brushButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.brushButton.Location = new System.Drawing.Point(3, 187);
            this.brushButton.Name = "brushButton";
            this.brushButton.Size = new System.Drawing.Size(296, 86);
            this.brushButton.TabIndex = 2;
            this.brushButton.Text = "fill using brush";
            this.brushButton.UseVisualStyleBackColor = true;
            this.brushButton.Click += new System.EventHandler(this.brushButton_Click);
            // 
            // rectangleButton
            // 
            this.rectangleButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rectangleButton.Location = new System.Drawing.Point(3, 279);
            this.rectangleButton.Name = "rectangleButton";
            this.rectangleButton.Size = new System.Drawing.Size(296, 86);
            this.rectangleButton.TabIndex = 3;
            this.rectangleButton.Text = "fill using rectangle";
            this.rectangleButton.UseVisualStyleBackColor = true;
            this.rectangleButton.Click += new System.EventHandler(this.rectangleButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1574, 1129);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Canvas)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.blueChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.redChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.greenChart)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.brightnessUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filterChart)).EndInit();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.PictureBox Canvas;
        private System.Windows.Forms.DataVisualization.Charting.Chart redChart;
        private System.Windows.Forms.DataVisualization.Charting.Chart blueChart;
        private System.Windows.Forms.DataVisualization.Charting.Chart greenChart;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.RadioButton eraseRadioButton;
        private System.Windows.Forms.RadioButton negationRadioButton;
        private System.Windows.Forms.RadioButton brightnessRadioButton;
        private System.Windows.Forms.RadioButton gammaRadioButton;
        private System.Windows.Forms.RadioButton contrastRadioButton;
        private System.Windows.Forms.RadioButton ownRadioButton;
        private System.Windows.Forms.DataVisualization.Charting.Chart filterChart;
        private System.Windows.Forms.RadioButton antifilterRadioButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Button loadButton;
        private System.Windows.Forms.Button fillButton;
        private System.Windows.Forms.Button brushButton;
        private System.Windows.Forms.Button rectangleButton;
        private System.Windows.Forms.NumericUpDown brightnessUpDown;
    }
}

