namespace Kairos.Forms
{
    partial class FrmAjusteFunciones
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea25 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend25 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea26 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend26 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series13 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAjusteFunciones));
            this.pnlEventos = new System.Windows.Forms.Panel();
            this.chrtInversa = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chrtFuncion = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblGraficoFuncionInversa = new System.Windows.Forms.Label();
            this.lblFuncionInversa = new System.Windows.Forms.TextBox();
            this.lblFuncion = new System.Windows.Forms.Label();
            this.pnlTitulo = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlMensaje = new System.Windows.Forms.Panel();
            this.lblMensaje = new System.Windows.Forms.Label();
            this.timerMensaje = new System.Windows.Forms.Timer(this.components);
            this.btnFuncionWeibull05 = new System.Windows.Forms.Button();
            this.btnVolver = new System.Windows.Forms.Button();
            this.pnlFunciones = new System.Windows.Forms.FlowLayoutPanel();
            this.btnFuncionBinomial = new System.Windows.Forms.Button();
            this.btnFuncionExponencial = new System.Windows.Forms.Button();
            this.btnFuncionLogistica = new System.Windows.Forms.Button();
            this.btnFuncionLognormal = new System.Windows.Forms.Button();
            this.btnFuncionLogLogistica = new System.Windows.Forms.Button();
            this.btnFuncionNormal = new System.Windows.Forms.Button();
            this.btnFuncionWeibull15 = new System.Windows.Forms.Button();
            this.btnFuncionWeibull3 = new System.Windows.Forms.Button();
            this.btnFuncionPoisson = new System.Windows.Forms.Button();
            this.btnFuncionUniforme = new System.Windows.Forms.Button();
            this.btnFuncionWeibull5 = new System.Windows.Forms.Button();
            this.pnlValoresAleatorios = new System.Windows.Forms.Panel();
            this.lbxGenerados = new System.Windows.Forms.ListBox();
            this.btnGenerarValoresAleatorios = new System.Windows.Forms.Button();
            this.lblTituloCantidadValoresGenerados = new System.Windows.Forms.Label();
            this.nudCantidadGenerados = new System.Windows.Forms.NumericUpDown();
            this.lblGenerarValoresAleatorios = new System.Windows.Forms.Label();
            this.lblTituloFunciones = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnExportarResultados = new System.Windows.Forms.Button();
            this.lblGraficoFuncion = new System.Windows.Forms.Label();
            this.pnlEventos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chrtInversa)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chrtFuncion)).BeginInit();
            this.pnlTitulo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.pnlMensaje.SuspendLayout();
            this.pnlFunciones.SuspendLayout();
            this.pnlValoresAleatorios.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCantidadGenerados)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlEventos
            // 
            this.pnlEventos.Controls.Add(this.lblFuncionInversa);
            this.pnlEventos.Controls.Add(this.chrtInversa);
            this.pnlEventos.Controls.Add(this.chrtFuncion);
            this.pnlEventos.Controls.Add(this.lblFuncion);
            this.pnlEventos.Controls.Add(this.lblGraficoFuncionInversa);
            this.pnlEventos.Controls.Add(this.lblGraficoFuncion);
            this.pnlEventos.Location = new System.Drawing.Point(280, 120);
            this.pnlEventos.Margin = new System.Windows.Forms.Padding(0);
            this.pnlEventos.Name = "pnlEventos";
            this.pnlEventos.Size = new System.Drawing.Size(720, 600);
            this.pnlEventos.TabIndex = 1;
            // 
            // chrtInversa
            // 
            this.chrtInversa.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.Center;
            this.chrtInversa.BackSecondaryColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            chartArea25.AxisX.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea25.Name = "ChartArea2";
            this.chrtInversa.ChartAreas.Add(chartArea25);
            legend25.Name = "Legend2";
            this.chrtInversa.Legends.Add(legend25);
            this.chrtInversa.Location = new System.Drawing.Point(0, 400);
            this.chrtInversa.Margin = new System.Windows.Forms.Padding(0);
            this.chrtInversa.Name = "chrtInversa";
            this.chrtInversa.Size = new System.Drawing.Size(720, 200);
            this.chrtInversa.TabIndex = 27;
            this.chrtInversa.Text = "chart2";
            // 
            // chrtFuncion
            // 
            this.chrtFuncion.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.Center;
            this.chrtFuncion.BackSecondaryColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.chrtFuncion.BorderSkin.BackColor = System.Drawing.Color.Empty;
            chartArea26.Name = "ChartArea1";
            this.chrtFuncion.ChartAreas.Add(chartArea26);
            legend26.Name = "Legend1";
            this.chrtFuncion.Legends.Add(legend26);
            this.chrtFuncion.Location = new System.Drawing.Point(0, 55);
            this.chrtFuncion.Margin = new System.Windows.Forms.Padding(0);
            this.chrtFuncion.Name = "chrtFuncion";
            this.chrtFuncion.RightToLeft = System.Windows.Forms.RightToLeft.No;
            series13.ChartArea = "ChartArea1";
            series13.Legend = "Legend1";
            series13.Name = "Series1";
            this.chrtFuncion.Series.Add(series13);
            this.chrtFuncion.Size = new System.Drawing.Size(720, 295);
            this.chrtFuncion.TabIndex = 26;
            this.chrtFuncion.Text = "chart1";
            // 
            // lblGraficoFuncionInversa
            // 
            this.lblGraficoFuncionInversa.Font = new System.Drawing.Font("Book Antiqua", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGraficoFuncionInversa.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblGraficoFuncionInversa.Location = new System.Drawing.Point(100, 345);
            this.lblGraficoFuncionInversa.Margin = new System.Windows.Forms.Padding(0);
            this.lblGraficoFuncionInversa.Name = "lblGraficoFuncionInversa";
            this.lblGraficoFuncionInversa.Size = new System.Drawing.Size(500, 25);
            this.lblGraficoFuncionInversa.TabIndex = 25;
            this.lblGraficoFuncionInversa.Text = "Función inversa";
            this.lblGraficoFuncionInversa.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblFuncionInversa
            // 
            this.lblFuncionInversa.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.lblFuncionInversa.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblFuncionInversa.Font = new System.Drawing.Font("Book Antiqua", 11.25F);
            this.lblFuncionInversa.Location = new System.Drawing.Point(0, 375);
            this.lblFuncionInversa.Margin = new System.Windows.Forms.Padding(0);
            this.lblFuncionInversa.Name = "lblFuncionInversa";
            this.lblFuncionInversa.ReadOnly = true;
            this.lblFuncionInversa.Size = new System.Drawing.Size(720, 24);
            this.lblFuncionInversa.TabIndex = 56;
            this.lblFuncionInversa.TabStop = false;
            this.lblFuncionInversa.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblFuncion
            // 
            this.lblFuncion.Font = new System.Drawing.Font("Book Antiqua", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFuncion.Location = new System.Drawing.Point(0, 30);
            this.lblFuncion.Margin = new System.Windows.Forms.Padding(0);
            this.lblFuncion.Name = "lblFuncion";
            this.lblFuncion.Size = new System.Drawing.Size(720, 25);
            this.lblFuncion.TabIndex = 50;
            this.lblFuncion.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // pnlTitulo
            // 
            this.pnlTitulo.BackColor = System.Drawing.Color.White;
            this.pnlTitulo.Controls.Add(this.pictureBox1);
            this.pnlTitulo.Controls.Add(this.label1);
            this.pnlTitulo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTitulo.Location = new System.Drawing.Point(0, 0);
            this.pnlTitulo.Margin = new System.Windows.Forms.Padding(0);
            this.pnlTitulo.Name = "pnlTitulo";
            this.pnlTitulo.Size = new System.Drawing.Size(1280, 100);
            this.pnlTitulo.TabIndex = 21;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Right;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(1130, 0);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(150, 100);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 10;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Vivaldi", 48F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(487, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(254, 95);
            this.label1.TabIndex = 9;
            this.label1.Text = "Kairós";
            // 
            // pnlMensaje
            // 
            this.pnlMensaje.Controls.Add(this.lblMensaje);
            this.pnlMensaje.Location = new System.Drawing.Point(280, 100);
            this.pnlMensaje.Margin = new System.Windows.Forms.Padding(0);
            this.pnlMensaje.Name = "pnlMensaje";
            this.pnlMensaje.Size = new System.Drawing.Size(1000, 20);
            this.pnlMensaje.TabIndex = 22;
            // 
            // lblMensaje
            // 
            this.lblMensaje.BackColor = System.Drawing.Color.Transparent;
            this.lblMensaje.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMensaje.Font = new System.Drawing.Font("Book Antiqua", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMensaje.Location = new System.Drawing.Point(0, 0);
            this.lblMensaje.Margin = new System.Windows.Forms.Padding(0);
            this.lblMensaje.Name = "lblMensaje";
            this.lblMensaje.Size = new System.Drawing.Size(1000, 20);
            this.lblMensaje.TabIndex = 22;
            this.lblMensaje.Text = "Mensaje";
            this.lblMensaje.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblMensaje.Visible = false;
            // 
            // timerMensaje
            // 
            this.timerMensaje.Interval = 3000;
            this.timerMensaje.Tick += new System.EventHandler(this.timerMensaje_Tick);
            // 
            // btnFuncionWeibull05
            // 
            this.btnFuncionWeibull05.BackColor = System.Drawing.Color.Black;
            this.btnFuncionWeibull05.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFuncionWeibull05.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnFuncionWeibull05.FlatAppearance.CheckedBackColor = System.Drawing.Color.White;
            this.btnFuncionWeibull05.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnFuncionWeibull05.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFuncionWeibull05.Font = new System.Drawing.Font("Book Antiqua", 13F);
            this.btnFuncionWeibull05.ForeColor = System.Drawing.Color.White;
            this.btnFuncionWeibull05.Location = new System.Drawing.Point(0, 0);
            this.btnFuncionWeibull05.Margin = new System.Windows.Forms.Padding(0);
            this.btnFuncionWeibull05.Name = "btnFuncionWeibull05";
            this.btnFuncionWeibull05.Size = new System.Drawing.Size(280, 41);
            this.btnFuncionWeibull05.TabIndex = 14;
            this.btnFuncionWeibull05.Text = "Weibull 0.5";
            this.btnFuncionWeibull05.UseVisualStyleBackColor = false;
            this.btnFuncionWeibull05.Click += new System.EventHandler(this.btnFuncionWeibull0_5_Click);
            // 
            // btnVolver
            // 
            this.btnVolver.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(134)))), ((int)(((byte)(0)))), ((int)(((byte)(3)))));
            this.btnVolver.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnVolver.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnVolver.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(79)))), ((int)(((byte)(79)))));
            this.btnVolver.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVolver.Font = new System.Drawing.Font("Book Antiqua", 13F);
            this.btnVolver.ForeColor = System.Drawing.Color.White;
            this.btnVolver.Location = new System.Drawing.Point(0, 661);
            this.btnVolver.Margin = new System.Windows.Forms.Padding(0);
            this.btnVolver.Name = "btnVolver";
            this.btnVolver.Size = new System.Drawing.Size(280, 59);
            this.btnVolver.TabIndex = 14;
            this.btnVolver.Text = "Volver";
            this.btnVolver.UseVisualStyleBackColor = false;
            this.btnVolver.Click += new System.EventHandler(this.btnVolver_Click);
            // 
            // pnlFunciones
            // 
            this.pnlFunciones.BackColor = System.Drawing.Color.Black;
            this.pnlFunciones.Controls.Add(this.btnFuncionWeibull05);
            this.pnlFunciones.Controls.Add(this.btnFuncionBinomial);
            this.pnlFunciones.Controls.Add(this.btnFuncionExponencial);
            this.pnlFunciones.Controls.Add(this.btnFuncionLogistica);
            this.pnlFunciones.Controls.Add(this.btnFuncionLognormal);
            this.pnlFunciones.Controls.Add(this.btnFuncionLogLogistica);
            this.pnlFunciones.Controls.Add(this.btnFuncionNormal);
            this.pnlFunciones.Controls.Add(this.btnFuncionWeibull15);
            this.pnlFunciones.Controls.Add(this.btnFuncionWeibull3);
            this.pnlFunciones.Controls.Add(this.btnFuncionPoisson);
            this.pnlFunciones.Controls.Add(this.btnFuncionUniforme);
            this.pnlFunciones.Controls.Add(this.btnFuncionWeibull5);
            this.pnlFunciones.Location = new System.Drawing.Point(0, 135);
            this.pnlFunciones.Margin = new System.Windows.Forms.Padding(0);
            this.pnlFunciones.Name = "pnlFunciones";
            this.pnlFunciones.Size = new System.Drawing.Size(280, 526);
            this.pnlFunciones.TabIndex = 22;
            // 
            // btnFuncionBinomial
            // 
            this.btnFuncionBinomial.BackColor = System.Drawing.Color.Black;
            this.btnFuncionBinomial.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFuncionBinomial.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnFuncionBinomial.FlatAppearance.CheckedBackColor = System.Drawing.Color.White;
            this.btnFuncionBinomial.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnFuncionBinomial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFuncionBinomial.Font = new System.Drawing.Font("Book Antiqua", 13F);
            this.btnFuncionBinomial.ForeColor = System.Drawing.Color.White;
            this.btnFuncionBinomial.Location = new System.Drawing.Point(0, 41);
            this.btnFuncionBinomial.Margin = new System.Windows.Forms.Padding(0);
            this.btnFuncionBinomial.Name = "btnFuncionBinomial";
            this.btnFuncionBinomial.Size = new System.Drawing.Size(280, 41);
            this.btnFuncionBinomial.TabIndex = 15;
            this.btnFuncionBinomial.Text = "Binomial";
            this.btnFuncionBinomial.UseVisualStyleBackColor = false;
            this.btnFuncionBinomial.Click += new System.EventHandler(this.btnFuncionBinomial_Click);
            // 
            // btnFuncionExponencial
            // 
            this.btnFuncionExponencial.BackColor = System.Drawing.Color.Black;
            this.btnFuncionExponencial.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFuncionExponencial.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnFuncionExponencial.FlatAppearance.CheckedBackColor = System.Drawing.Color.White;
            this.btnFuncionExponencial.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnFuncionExponencial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFuncionExponencial.Font = new System.Drawing.Font("Book Antiqua", 13F);
            this.btnFuncionExponencial.ForeColor = System.Drawing.Color.White;
            this.btnFuncionExponencial.Location = new System.Drawing.Point(0, 82);
            this.btnFuncionExponencial.Margin = new System.Windows.Forms.Padding(0);
            this.btnFuncionExponencial.Name = "btnFuncionExponencial";
            this.btnFuncionExponencial.Size = new System.Drawing.Size(280, 41);
            this.btnFuncionExponencial.TabIndex = 16;
            this.btnFuncionExponencial.Text = "Exponencial";
            this.btnFuncionExponencial.UseVisualStyleBackColor = false;
            this.btnFuncionExponencial.Click += new System.EventHandler(this.btnFuncionExponencial_Click);
            // 
            // btnFuncionLogistica
            // 
            this.btnFuncionLogistica.BackColor = System.Drawing.Color.Black;
            this.btnFuncionLogistica.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFuncionLogistica.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnFuncionLogistica.FlatAppearance.CheckedBackColor = System.Drawing.Color.White;
            this.btnFuncionLogistica.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnFuncionLogistica.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFuncionLogistica.Font = new System.Drawing.Font("Book Antiqua", 13F);
            this.btnFuncionLogistica.ForeColor = System.Drawing.Color.White;
            this.btnFuncionLogistica.Location = new System.Drawing.Point(0, 123);
            this.btnFuncionLogistica.Margin = new System.Windows.Forms.Padding(0);
            this.btnFuncionLogistica.Name = "btnFuncionLogistica";
            this.btnFuncionLogistica.Size = new System.Drawing.Size(280, 41);
            this.btnFuncionLogistica.TabIndex = 17;
            this.btnFuncionLogistica.Text = "Logística";
            this.btnFuncionLogistica.UseVisualStyleBackColor = false;
            this.btnFuncionLogistica.Click += new System.EventHandler(this.btnFuncionLogistica_Click);
            // 
            // btnFuncionLognormal
            // 
            this.btnFuncionLognormal.BackColor = System.Drawing.Color.Black;
            this.btnFuncionLognormal.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFuncionLognormal.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnFuncionLognormal.FlatAppearance.CheckedBackColor = System.Drawing.Color.White;
            this.btnFuncionLognormal.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnFuncionLognormal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFuncionLognormal.Font = new System.Drawing.Font("Book Antiqua", 13F);
            this.btnFuncionLognormal.ForeColor = System.Drawing.Color.White;
            this.btnFuncionLognormal.Location = new System.Drawing.Point(0, 164);
            this.btnFuncionLognormal.Margin = new System.Windows.Forms.Padding(0);
            this.btnFuncionLognormal.Name = "btnFuncionLognormal";
            this.btnFuncionLognormal.Size = new System.Drawing.Size(280, 41);
            this.btnFuncionLognormal.TabIndex = 18;
            this.btnFuncionLognormal.Text = "Log-Normal";
            this.btnFuncionLognormal.UseVisualStyleBackColor = false;
            this.btnFuncionLognormal.Click += new System.EventHandler(this.btnFuncionLognormal_Click);
            // 
            // btnFuncionLogLogistica
            // 
            this.btnFuncionLogLogistica.BackColor = System.Drawing.Color.Black;
            this.btnFuncionLogLogistica.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFuncionLogLogistica.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnFuncionLogLogistica.FlatAppearance.CheckedBackColor = System.Drawing.Color.White;
            this.btnFuncionLogLogistica.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnFuncionLogLogistica.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFuncionLogLogistica.Font = new System.Drawing.Font("Book Antiqua", 13F);
            this.btnFuncionLogLogistica.ForeColor = System.Drawing.Color.White;
            this.btnFuncionLogLogistica.Location = new System.Drawing.Point(0, 205);
            this.btnFuncionLogLogistica.Margin = new System.Windows.Forms.Padding(0);
            this.btnFuncionLogLogistica.Name = "btnFuncionLogLogistica";
            this.btnFuncionLogLogistica.Size = new System.Drawing.Size(280, 41);
            this.btnFuncionLogLogistica.TabIndex = 19;
            this.btnFuncionLogLogistica.Text = "Log-Logística";
            this.btnFuncionLogLogistica.UseVisualStyleBackColor = false;
            this.btnFuncionLogLogistica.Click += new System.EventHandler(this.btnFuncionLogLogistica_Click);
            // 
            // btnFuncionNormal
            // 
            this.btnFuncionNormal.BackColor = System.Drawing.Color.Black;
            this.btnFuncionNormal.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFuncionNormal.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnFuncionNormal.FlatAppearance.CheckedBackColor = System.Drawing.Color.White;
            this.btnFuncionNormal.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnFuncionNormal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFuncionNormal.Font = new System.Drawing.Font("Book Antiqua", 13F);
            this.btnFuncionNormal.ForeColor = System.Drawing.Color.White;
            this.btnFuncionNormal.Location = new System.Drawing.Point(0, 246);
            this.btnFuncionNormal.Margin = new System.Windows.Forms.Padding(0);
            this.btnFuncionNormal.Name = "btnFuncionNormal";
            this.btnFuncionNormal.Size = new System.Drawing.Size(280, 41);
            this.btnFuncionNormal.TabIndex = 20;
            this.btnFuncionNormal.Text = "Normal";
            this.btnFuncionNormal.UseVisualStyleBackColor = false;
            this.btnFuncionNormal.Click += new System.EventHandler(this.btnFuncionNormal_Click);
            // 
            // btnFuncionWeibull15
            // 
            this.btnFuncionWeibull15.BackColor = System.Drawing.Color.Black;
            this.btnFuncionWeibull15.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFuncionWeibull15.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnFuncionWeibull15.FlatAppearance.CheckedBackColor = System.Drawing.Color.White;
            this.btnFuncionWeibull15.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnFuncionWeibull15.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFuncionWeibull15.Font = new System.Drawing.Font("Book Antiqua", 13F);
            this.btnFuncionWeibull15.ForeColor = System.Drawing.Color.White;
            this.btnFuncionWeibull15.Location = new System.Drawing.Point(0, 287);
            this.btnFuncionWeibull15.Margin = new System.Windows.Forms.Padding(0);
            this.btnFuncionWeibull15.Name = "btnFuncionWeibull15";
            this.btnFuncionWeibull15.Size = new System.Drawing.Size(280, 41);
            this.btnFuncionWeibull15.TabIndex = 21;
            this.btnFuncionWeibull15.Text = "Weibull 1.5";
            this.btnFuncionWeibull15.UseVisualStyleBackColor = false;
            this.btnFuncionWeibull15.Click += new System.EventHandler(this.btnFuncionWeibull1_5_Click);
            // 
            // btnFuncionWeibull3
            // 
            this.btnFuncionWeibull3.BackColor = System.Drawing.Color.Black;
            this.btnFuncionWeibull3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFuncionWeibull3.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnFuncionWeibull3.FlatAppearance.CheckedBackColor = System.Drawing.Color.White;
            this.btnFuncionWeibull3.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnFuncionWeibull3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFuncionWeibull3.Font = new System.Drawing.Font("Book Antiqua", 13F);
            this.btnFuncionWeibull3.ForeColor = System.Drawing.Color.White;
            this.btnFuncionWeibull3.Location = new System.Drawing.Point(0, 328);
            this.btnFuncionWeibull3.Margin = new System.Windows.Forms.Padding(0);
            this.btnFuncionWeibull3.Name = "btnFuncionWeibull3";
            this.btnFuncionWeibull3.Size = new System.Drawing.Size(280, 41);
            this.btnFuncionWeibull3.TabIndex = 22;
            this.btnFuncionWeibull3.Text = "Weibull 3";
            this.btnFuncionWeibull3.UseVisualStyleBackColor = false;
            this.btnFuncionWeibull3.Click += new System.EventHandler(this.btnFuncionWeibull3_Click);
            // 
            // btnFuncionPoisson
            // 
            this.btnFuncionPoisson.BackColor = System.Drawing.Color.Black;
            this.btnFuncionPoisson.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFuncionPoisson.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnFuncionPoisson.FlatAppearance.CheckedBackColor = System.Drawing.Color.White;
            this.btnFuncionPoisson.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnFuncionPoisson.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFuncionPoisson.Font = new System.Drawing.Font("Book Antiqua", 13F);
            this.btnFuncionPoisson.ForeColor = System.Drawing.Color.White;
            this.btnFuncionPoisson.Location = new System.Drawing.Point(0, 369);
            this.btnFuncionPoisson.Margin = new System.Windows.Forms.Padding(0);
            this.btnFuncionPoisson.Name = "btnFuncionPoisson";
            this.btnFuncionPoisson.Size = new System.Drawing.Size(280, 41);
            this.btnFuncionPoisson.TabIndex = 23;
            this.btnFuncionPoisson.Text = "Poisson";
            this.btnFuncionPoisson.UseVisualStyleBackColor = false;
            this.btnFuncionPoisson.Click += new System.EventHandler(this.btnFuncionPoisson_Click);
            // 
            // btnFuncionUniforme
            // 
            this.btnFuncionUniforme.BackColor = System.Drawing.Color.Black;
            this.btnFuncionUniforme.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFuncionUniforme.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnFuncionUniforme.FlatAppearance.CheckedBackColor = System.Drawing.Color.White;
            this.btnFuncionUniforme.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnFuncionUniforme.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFuncionUniforme.Font = new System.Drawing.Font("Book Antiqua", 13F);
            this.btnFuncionUniforme.ForeColor = System.Drawing.Color.White;
            this.btnFuncionUniforme.Location = new System.Drawing.Point(0, 410);
            this.btnFuncionUniforme.Margin = new System.Windows.Forms.Padding(0);
            this.btnFuncionUniforme.Name = "btnFuncionUniforme";
            this.btnFuncionUniforme.Size = new System.Drawing.Size(280, 41);
            this.btnFuncionUniforme.TabIndex = 24;
            this.btnFuncionUniforme.Text = "Uniforme";
            this.btnFuncionUniforme.UseVisualStyleBackColor = false;
            this.btnFuncionUniforme.Click += new System.EventHandler(this.btnFuncionUniforme_Click);
            // 
            // btnFuncionWeibull5
            // 
            this.btnFuncionWeibull5.BackColor = System.Drawing.Color.Black;
            this.btnFuncionWeibull5.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFuncionWeibull5.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnFuncionWeibull5.FlatAppearance.CheckedBackColor = System.Drawing.Color.White;
            this.btnFuncionWeibull5.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnFuncionWeibull5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFuncionWeibull5.Font = new System.Drawing.Font("Book Antiqua", 13F);
            this.btnFuncionWeibull5.ForeColor = System.Drawing.Color.White;
            this.btnFuncionWeibull5.Location = new System.Drawing.Point(0, 451);
            this.btnFuncionWeibull5.Margin = new System.Windows.Forms.Padding(0);
            this.btnFuncionWeibull5.Name = "btnFuncionWeibull5";
            this.btnFuncionWeibull5.Size = new System.Drawing.Size(280, 41);
            this.btnFuncionWeibull5.TabIndex = 25;
            this.btnFuncionWeibull5.Text = "Weibull 5";
            this.btnFuncionWeibull5.UseVisualStyleBackColor = false;
            this.btnFuncionWeibull5.Click += new System.EventHandler(this.btnFuncionWeibull5_Click);
            // 
            // pnlValoresAleatorios
            // 
            this.pnlValoresAleatorios.Controls.Add(this.lbxGenerados);
            this.pnlValoresAleatorios.Controls.Add(this.btnGenerarValoresAleatorios);
            this.pnlValoresAleatorios.Controls.Add(this.lblTituloCantidadValoresGenerados);
            this.pnlValoresAleatorios.Controls.Add(this.nudCantidadGenerados);
            this.pnlValoresAleatorios.Controls.Add(this.lblGenerarValoresAleatorios);
            this.pnlValoresAleatorios.Location = new System.Drawing.Point(1000, 120);
            this.pnlValoresAleatorios.Margin = new System.Windows.Forms.Padding(0);
            this.pnlValoresAleatorios.Name = "pnlValoresAleatorios";
            this.pnlValoresAleatorios.Size = new System.Drawing.Size(280, 500);
            this.pnlValoresAleatorios.TabIndex = 23;
            // 
            // lbxGenerados
            // 
            this.lbxGenerados.FormattingEnabled = true;
            this.lbxGenerados.ItemHeight = 16;
            this.lbxGenerados.Location = new System.Drawing.Point(0, 160);
            this.lbxGenerados.Margin = new System.Windows.Forms.Padding(0);
            this.lbxGenerados.Name = "lbxGenerados";
            this.lbxGenerados.ScrollAlwaysVisible = true;
            this.lbxGenerados.Size = new System.Drawing.Size(280, 340);
            this.lbxGenerados.TabIndex = 59;
            // 
            // btnGenerarValoresAleatorios
            // 
            this.btnGenerarValoresAleatorios.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnGenerarValoresAleatorios.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGenerarValoresAleatorios.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(118)))), ((int)(((byte)(118)))));
            this.btnGenerarValoresAleatorios.FlatAppearance.BorderSize = 2;
            this.btnGenerarValoresAleatorios.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(108)))), ((int)(((byte)(108)))));
            this.btnGenerarValoresAleatorios.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenerarValoresAleatorios.Font = new System.Drawing.Font("Book Antiqua", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerarValoresAleatorios.ForeColor = System.Drawing.Color.Black;
            this.btnGenerarValoresAleatorios.Location = new System.Drawing.Point(0, 110);
            this.btnGenerarValoresAleatorios.Margin = new System.Windows.Forms.Padding(0);
            this.btnGenerarValoresAleatorios.Name = "btnGenerarValoresAleatorios";
            this.btnGenerarValoresAleatorios.Size = new System.Drawing.Size(280, 50);
            this.btnGenerarValoresAleatorios.TabIndex = 58;
            this.btnGenerarValoresAleatorios.Text = "Generar";
            this.btnGenerarValoresAleatorios.UseVisualStyleBackColor = false;
            this.btnGenerarValoresAleatorios.Click += new System.EventHandler(this.btnGenerarValoresAleatorios_Click);
            // 
            // lblTituloCantidadValoresGenerados
            // 
            this.lblTituloCantidadValoresGenerados.Font = new System.Drawing.Font("Book Antiqua", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTituloCantidadValoresGenerados.Location = new System.Drawing.Point(0, 71);
            this.lblTituloCantidadValoresGenerados.Margin = new System.Windows.Forms.Padding(0);
            this.lblTituloCantidadValoresGenerados.Name = "lblTituloCantidadValoresGenerados";
            this.lblTituloCantidadValoresGenerados.Size = new System.Drawing.Size(140, 22);
            this.lblTituloCantidadValoresGenerados.TabIndex = 57;
            this.lblTituloCantidadValoresGenerados.Text = "Cantidad";
            this.lblTituloCantidadValoresGenerados.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // nudCantidadGenerados
            // 
            this.nudCantidadGenerados.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.nudCantidadGenerados.Location = new System.Drawing.Point(140, 71);
            this.nudCantidadGenerados.Margin = new System.Windows.Forms.Padding(0);
            this.nudCantidadGenerados.Maximum = new decimal(new int[] {
            7000,
            0,
            0,
            0});
            this.nudCantidadGenerados.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudCantidadGenerados.Name = "nudCantidadGenerados";
            this.nudCantidadGenerados.Size = new System.Drawing.Size(120, 22);
            this.nudCantidadGenerados.TabIndex = 56;
            this.nudCantidadGenerados.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblGenerarValoresAleatorios
            // 
            this.lblGenerarValoresAleatorios.Font = new System.Drawing.Font("Book Antiqua", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGenerarValoresAleatorios.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblGenerarValoresAleatorios.Location = new System.Drawing.Point(0, 0);
            this.lblGenerarValoresAleatorios.Margin = new System.Windows.Forms.Padding(0);
            this.lblGenerarValoresAleatorios.Name = "lblGenerarValoresAleatorios";
            this.lblGenerarValoresAleatorios.Size = new System.Drawing.Size(280, 60);
            this.lblGenerarValoresAleatorios.TabIndex = 55;
            this.lblGenerarValoresAleatorios.Text = "Generar intervalos aleatorios";
            this.lblGenerarValoresAleatorios.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTituloFunciones
            // 
            this.lblTituloFunciones.Font = new System.Drawing.Font("Book Antiqua", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTituloFunciones.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblTituloFunciones.Location = new System.Drawing.Point(0, 100);
            this.lblTituloFunciones.Margin = new System.Windows.Forms.Padding(0);
            this.lblTituloFunciones.Name = "lblTituloFunciones";
            this.lblTituloFunciones.Size = new System.Drawing.Size(280, 35);
            this.lblTituloFunciones.TabIndex = 25;
            this.lblTituloFunciones.Text = "Ranking Mejor Ajuste";
            this.lblTituloFunciones.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnExportarResultados);
            this.panel1.Location = new System.Drawing.Point(1000, 620);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(280, 100);
            this.panel1.TabIndex = 26;
            // 
            // btnExportarResultados
            // 
            this.btnExportarResultados.BackColor = System.Drawing.Color.Black;
            this.btnExportarResultados.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExportarResultados.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnExportarResultados.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnExportarResultados.FlatAppearance.BorderSize = 2;
            this.btnExportarResultados.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnExportarResultados.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportarResultados.Font = new System.Drawing.Font("Book Antiqua", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExportarResultados.ForeColor = System.Drawing.Color.White;
            this.btnExportarResultados.Location = new System.Drawing.Point(0, 0);
            this.btnExportarResultados.Margin = new System.Windows.Forms.Padding(0);
            this.btnExportarResultados.Name = "btnExportarResultados";
            this.btnExportarResultados.Size = new System.Drawing.Size(280, 100);
            this.btnExportarResultados.TabIndex = 17;
            this.btnExportarResultados.Text = "Exportar Resultados";
            this.btnExportarResultados.UseVisualStyleBackColor = false;
            this.btnExportarResultados.Click += new System.EventHandler(this.btnExportarResultados_Click);
            // 
            // lblGraficoFuncion
            // 
            this.lblGraficoFuncion.Font = new System.Drawing.Font("Book Antiqua", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGraficoFuncion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblGraficoFuncion.Location = new System.Drawing.Point(100, 0);
            this.lblGraficoFuncion.Margin = new System.Windows.Forms.Padding(0);
            this.lblGraficoFuncion.Name = "lblGraficoFuncion";
            this.lblGraficoFuncion.Size = new System.Drawing.Size(500, 30);
            this.lblGraficoFuncion.TabIndex = 24;
            this.lblGraficoFuncion.Text = "Función de Densidad de Probabilidad";
            this.lblGraficoFuncion.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // FrmAjusteFunciones
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.ClientSize = new System.Drawing.Size(1280, 720);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblTituloFunciones);
            this.Controls.Add(this.pnlValoresAleatorios);
            this.Controls.Add(this.btnVolver);
            this.Controls.Add(this.pnlTitulo);
            this.Controls.Add(this.pnlFunciones);
            this.Controls.Add(this.pnlMensaje);
            this.Controls.Add(this.pnlEventos);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "FrmAjusteFunciones";
            this.Opacity = 0.98D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.FrmAjusteFunciones_Load);
            this.pnlEventos.ResumeLayout(false);
            this.pnlEventos.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chrtInversa)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chrtFuncion)).EndInit();
            this.pnlTitulo.ResumeLayout(false);
            this.pnlTitulo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.pnlMensaje.ResumeLayout(false);
            this.pnlFunciones.ResumeLayout(false);
            this.pnlValoresAleatorios.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudCantidadGenerados)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel pnlEventos;
        private System.Windows.Forms.Panel pnlTitulo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlMensaje;
        private System.Windows.Forms.Label lblFuncion;
        private System.Windows.Forms.Timer timerMensaje;
        private System.Windows.Forms.Label lblMensaje;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnFuncionWeibull05;
        private System.Windows.Forms.Button btnVolver;
        private System.Windows.Forms.FlowLayoutPanel pnlFunciones;
        private System.Windows.Forms.Panel pnlValoresAleatorios;
        private System.Windows.Forms.Label lblGenerarValoresAleatorios;
        private System.Windows.Forms.Button btnFuncionBinomial;
        private System.Windows.Forms.Button btnFuncionExponencial;
        private System.Windows.Forms.Button btnFuncionLogistica;
        private System.Windows.Forms.Button btnFuncionLognormal;
        private System.Windows.Forms.Button btnFuncionLogLogistica;
        private System.Windows.Forms.Button btnFuncionNormal;
        private System.Windows.Forms.Button btnFuncionWeibull15;
        private System.Windows.Forms.Button btnFuncionWeibull3;
        private System.Windows.Forms.Button btnFuncionPoisson;
        private System.Windows.Forms.Button btnFuncionUniforme;
        private System.Windows.Forms.Button btnFuncionWeibull5;
        private System.Windows.Forms.Label lblTituloFunciones;
        private System.Windows.Forms.DataVisualization.Charting.Chart chrtInversa;
        private System.Windows.Forms.DataVisualization.Charting.Chart chrtFuncion;
        private System.Windows.Forms.Label lblGraficoFuncionInversa;
        private System.Windows.Forms.ListBox lbxGenerados;
        private System.Windows.Forms.Button btnGenerarValoresAleatorios;
        private System.Windows.Forms.Label lblTituloCantidadValoresGenerados;
        private System.Windows.Forms.NumericUpDown nudCantidadGenerados;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnExportarResultados;
        private System.Windows.Forms.TextBox lblFuncionInversa;
        private System.Windows.Forms.Label lblGraficoFuncion;
    }
}