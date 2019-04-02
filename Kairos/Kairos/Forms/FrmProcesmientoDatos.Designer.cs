namespace Kairos.Forms
{
    partial class FrmProcesmientoDatos
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmProcesmientoDatos));
            this.pnlEventos = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblMedia = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblCantidad = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.dgwEventos = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.rbFecha = new System.Windows.Forms.RadioButton();
            this.lblVista = new System.Windows.Forms.Label();
            this.rbIntervalos = new System.Windows.Forms.RadioButton();
            this.lblEventos = new System.Windows.Forms.Label();
            this.pnlModificable = new System.Windows.Forms.Panel();
            this.nudAgregarPorIntervalo = new System.Windows.Forms.NumericUpDown();
            this.cbAgregarPorIntervalo = new System.Windows.Forms.ComboBox();
            this.rbAgregarPorIntervalo = new System.Windows.Forms.RadioButton();
            this.rbAgregarPorFechaYHora = new System.Windows.Forms.RadioButton();
            this.txtIntervalo2 = new System.Windows.Forms.TextBox();
            this.txtIntervalo = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chlFiltros = new System.Windows.Forms.CheckedListBox();
            this.btnLimpiar = new System.Windows.Forms.Button();
            this.cmbTipoFiltro = new System.Windows.Forms.ComboBox();
            this.lblTipoFiltro = new System.Windows.Forms.Label();
            this.dtp2 = new System.Windows.Forms.DateTimePicker();
            this.dtp1 = new System.Windows.Forms.DateTimePicker();
            this.btnAceptar = new System.Windows.Forms.Button();
            this.lblAccion2 = new System.Windows.Forms.Label();
            this.lblAccion1 = new System.Windows.Forms.Label();
            this.lblTituloAccion = new System.Windows.Forms.Label();
            this.pnlAccionesFinales = new System.Windows.Forms.Panel();
            this.btnCalcularFDP = new System.Windows.Forms.Button();
            this.pnlTitulo = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlMensaje = new System.Windows.Forms.Panel();
            this.lblMensaje = new System.Windows.Forms.Label();
            this.timerMensaje = new System.Windows.Forms.Timer(this.components);
            this.btnAgregarRegistro = new System.Windows.Forms.Button();
            this.btnModificarRegistro = new System.Windows.Forms.Button();
            this.btnVolver = new System.Windows.Forms.Button();
            this.btnBorrarSeleccionados = new System.Windows.Forms.Button();
            this.btnFiltrar = new System.Windows.Forms.Button();
            this.btnSeleccionarTodos = new System.Windows.Forms.Button();
            this.pnlAcciones = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnlSegmentacion = new System.Windows.Forms.Panel();
            this.rbSegundo = new System.Windows.Forms.RadioButton();
            this.rbDia = new System.Windows.Forms.RadioButton();
            this.rbMinuto = new System.Windows.Forms.RadioButton();
            this.rbHora = new System.Windows.Forms.RadioButton();
            this.rbDtConstante = new System.Windows.Forms.RadioButton();
            this.rbEventoAEvento = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.lblTituloProyecto = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.pnlEventos.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgwEventos)).BeginInit();
            this.panel2.SuspendLayout();
            this.pnlModificable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudAgregarPorIntervalo)).BeginInit();
            this.pnlAccionesFinales.SuspendLayout();
            this.pnlTitulo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.pnlMensaje.SuspendLayout();
            this.pnlAcciones.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnlSegmentacion.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlEventos
            // 
            this.pnlEventos.Controls.Add(this.panel3);
            this.pnlEventos.Controls.Add(this.dgwEventos);
            this.pnlEventos.Controls.Add(this.panel2);
            this.pnlEventos.Controls.Add(this.lblEventos);
            this.pnlEventos.Location = new System.Drawing.Point(317, 161);
            this.pnlEventos.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlEventos.Name = "pnlEventos";
            this.pnlEventos.Size = new System.Drawing.Size(517, 558);
            this.pnlEventos.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.lblMedia);
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.lblCantidad);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Location = new System.Drawing.Point(0, 514);
            this.panel3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(517, 43);
            this.panel3.TabIndex = 64;
            // 
            // lblMedia
            // 
            this.lblMedia.AutoSize = true;
            this.lblMedia.Font = new System.Drawing.Font("Book Antiqua", 12F);
            this.lblMedia.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblMedia.Location = new System.Drawing.Point(289, 9);
            this.lblMedia.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMedia.Name = "lblMedia";
            this.lblMedia.Size = new System.Drawing.Size(20, 24);
            this.lblMedia.TabIndex = 64;
            this.lblMedia.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Book Antiqua", 12F);
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label5.Location = new System.Drawing.Point(217, 9);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 24);
            this.label5.TabIndex = 62;
            this.label5.Text = "Media:";
            // 
            // lblCantidad
            // 
            this.lblCantidad.AutoSize = true;
            this.lblCantidad.Font = new System.Drawing.Font("Book Antiqua", 12F);
            this.lblCantidad.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblCantidad.Location = new System.Drawing.Point(119, 9);
            this.lblCantidad.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCantidad.Name = "lblCantidad";
            this.lblCantidad.Size = new System.Drawing.Size(20, 24);
            this.lblCantidad.TabIndex = 63;
            this.lblCantidad.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Book Antiqua", 12F);
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label4.Location = new System.Drawing.Point(16, 9);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(98, 24);
            this.label4.TabIndex = 57;
            this.label4.Text = "Cantidad:";
            // 
            // dgwEventos
            // 
            this.dgwEventos.AllowUserToAddRows = false;
            this.dgwEventos.AllowUserToDeleteRows = false;
            this.dgwEventos.BackgroundColor = System.Drawing.Color.White;
            this.dgwEventos.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            this.dgwEventos.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgwEventos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgwEventos.Location = new System.Drawing.Point(21, 41);
            this.dgwEventos.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgwEventos.Name = "dgwEventos";
            this.dgwEventos.ReadOnly = true;
            this.dgwEventos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgwEventos.Size = new System.Drawing.Size(459, 466);
            this.dgwEventos.TabIndex = 23;
            this.dgwEventos.SelectionChanged += new System.EventHandler(this.dgwEventos_SelectionChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.rbFecha);
            this.panel2.Controls.Add(this.lblVista);
            this.panel2.Controls.Add(this.rbIntervalos);
            this.panel2.Location = new System.Drawing.Point(191, 2);
            this.panel2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(289, 41);
            this.panel2.TabIndex = 61;
            // 
            // rbFecha
            // 
            this.rbFecha.AutoSize = true;
            this.rbFecha.Checked = true;
            this.rbFecha.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rbFecha.Font = new System.Drawing.Font("Book Antiqua", 10F);
            this.rbFecha.Location = new System.Drawing.Point(75, 10);
            this.rbFecha.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rbFecha.Name = "rbFecha";
            this.rbFecha.Size = new System.Drawing.Size(75, 26);
            this.rbFecha.TabIndex = 59;
            this.rbFecha.TabStop = true;
            this.rbFecha.Text = "Fecha";
            this.rbFecha.UseVisualStyleBackColor = true;
            this.rbFecha.CheckedChanged += new System.EventHandler(this.rbFecha_CheckedChanged);
            // 
            // lblVista
            // 
            this.lblVista.AutoSize = true;
            this.lblVista.Font = new System.Drawing.Font("Book Antiqua", 12F);
            this.lblVista.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblVista.Location = new System.Drawing.Point(3, 10);
            this.lblVista.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblVista.Name = "lblVista";
            this.lblVista.Size = new System.Drawing.Size(55, 24);
            this.lblVista.TabIndex = 55;
            this.lblVista.Text = "Vista";
            // 
            // rbIntervalos
            // 
            this.rbIntervalos.AutoSize = true;
            this.rbIntervalos.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rbIntervalos.Font = new System.Drawing.Font("Book Antiqua", 10F);
            this.rbIntervalos.Location = new System.Drawing.Point(167, 10);
            this.rbIntervalos.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rbIntervalos.Name = "rbIntervalos";
            this.rbIntervalos.Size = new System.Drawing.Size(108, 26);
            this.rbIntervalos.TabIndex = 60;
            this.rbIntervalos.Text = "Intervalos";
            this.rbIntervalos.UseVisualStyleBackColor = true;
            this.rbIntervalos.CheckedChanged += new System.EventHandler(this.rbIntervalos_CheckedChanged);
            // 
            // lblEventos
            // 
            this.lblEventos.AutoSize = true;
            this.lblEventos.Font = new System.Drawing.Font("Book Antiqua", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEventos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblEventos.Location = new System.Drawing.Point(16, 11);
            this.lblEventos.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblEventos.Name = "lblEventos";
            this.lblEventos.Size = new System.Drawing.Size(99, 28);
            this.lblEventos.TabIndex = 24;
            this.lblEventos.Text = "Eventos";
            // 
            // pnlModificable
            // 
            this.pnlModificable.Controls.Add(this.nudAgregarPorIntervalo);
            this.pnlModificable.Controls.Add(this.cbAgregarPorIntervalo);
            this.pnlModificable.Controls.Add(this.rbAgregarPorIntervalo);
            this.pnlModificable.Controls.Add(this.rbAgregarPorFechaYHora);
            this.pnlModificable.Controls.Add(this.txtIntervalo2);
            this.pnlModificable.Controls.Add(this.txtIntervalo);
            this.pnlModificable.Controls.Add(this.label2);
            this.pnlModificable.Controls.Add(this.chlFiltros);
            this.pnlModificable.Controls.Add(this.btnLimpiar);
            this.pnlModificable.Controls.Add(this.cmbTipoFiltro);
            this.pnlModificable.Controls.Add(this.lblTipoFiltro);
            this.pnlModificable.Controls.Add(this.dtp2);
            this.pnlModificable.Controls.Add(this.dtp1);
            this.pnlModificable.Controls.Add(this.btnAceptar);
            this.pnlModificable.Controls.Add(this.lblAccion2);
            this.pnlModificable.Controls.Add(this.lblAccion1);
            this.pnlModificable.Controls.Add(this.lblTituloAccion);
            this.pnlModificable.Location = new System.Drawing.Point(888, 130);
            this.pnlModificable.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlModificable.Name = "pnlModificable";
            this.pnlModificable.Size = new System.Drawing.Size(376, 416);
            this.pnlModificable.TabIndex = 2;
            this.pnlModificable.Visible = false;
            // 
            // nudAgregarPorIntervalo
            // 
            this.nudAgregarPorIntervalo.Location = new System.Drawing.Point(127, 207);
            this.nudAgregarPorIntervalo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.nudAgregarPorIntervalo.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudAgregarPorIntervalo.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudAgregarPorIntervalo.Name = "nudAgregarPorIntervalo";
            this.nudAgregarPorIntervalo.Size = new System.Drawing.Size(69, 22);
            this.nudAgregarPorIntervalo.TabIndex = 65;
            this.nudAgregarPorIntervalo.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudAgregarPorIntervalo.Visible = false;
            // 
            // cbAgregarPorIntervalo
            // 
            this.cbAgregarPorIntervalo.Cursor = System.Windows.Forms.Cursors.Default;
            this.cbAgregarPorIntervalo.FormattingEnabled = true;
            this.cbAgregarPorIntervalo.Items.AddRange(new object[] {
            "Segundos",
            "Minutos",
            "Horas",
            "Dias"});
            this.cbAgregarPorIntervalo.Location = new System.Drawing.Point(204, 207);
            this.cbAgregarPorIntervalo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbAgregarPorIntervalo.Name = "cbAgregarPorIntervalo";
            this.cbAgregarPorIntervalo.Size = new System.Drawing.Size(116, 24);
            this.cbAgregarPorIntervalo.TabIndex = 64;
            this.cbAgregarPorIntervalo.Text = "Seleccione";
            this.cbAgregarPorIntervalo.Visible = false;
            // 
            // rbAgregarPorIntervalo
            // 
            this.rbAgregarPorIntervalo.AutoSize = true;
            this.rbAgregarPorIntervalo.Checked = true;
            this.rbAgregarPorIntervalo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rbAgregarPorIntervalo.Font = new System.Drawing.Font("Book Antiqua", 10F);
            this.rbAgregarPorIntervalo.Location = new System.Drawing.Point(15, 207);
            this.rbAgregarPorIntervalo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rbAgregarPorIntervalo.Name = "rbAgregarPorIntervalo";
            this.rbAgregarPorIntervalo.Size = new System.Drawing.Size(101, 26);
            this.rbAgregarPorIntervalo.TabIndex = 62;
            this.rbAgregarPorIntervalo.TabStop = true;
            this.rbAgregarPorIntervalo.Text = "Intervalo";
            this.rbAgregarPorIntervalo.UseVisualStyleBackColor = true;
            this.rbAgregarPorIntervalo.CheckedChanged += new System.EventHandler(this.rbAgregarPorIntervalo_CheckedChanged);
            // 
            // rbAgregarPorFechaYHora
            // 
            this.rbAgregarPorFechaYHora.AutoSize = true;
            this.rbAgregarPorFechaYHora.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rbAgregarPorFechaYHora.Font = new System.Drawing.Font("Book Antiqua", 10F);
            this.rbAgregarPorFechaYHora.Location = new System.Drawing.Point(12, 59);
            this.rbAgregarPorFechaYHora.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rbAgregarPorFechaYHora.Name = "rbAgregarPorFechaYHora";
            this.rbAgregarPorFechaYHora.Size = new System.Drawing.Size(127, 26);
            this.rbAgregarPorFechaYHora.TabIndex = 61;
            this.rbAgregarPorFechaYHora.Text = "Fecha y hora";
            this.rbAgregarPorFechaYHora.UseVisualStyleBackColor = true;
            this.rbAgregarPorFechaYHora.CheckedChanged += new System.EventHandler(this.rbAgregarPorFechaYHora_CheckedChanged);
            // 
            // txtIntervalo2
            // 
            this.txtIntervalo2.Location = new System.Drawing.Point(168, 143);
            this.txtIntervalo2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtIntervalo2.Name = "txtIntervalo2";
            this.txtIntervalo2.Size = new System.Drawing.Size(51, 22);
            this.txtIntervalo2.TabIndex = 56;
            this.txtIntervalo2.Text = "0";
            this.txtIntervalo2.Visible = false;
            // 
            // txtIntervalo
            // 
            this.txtIntervalo.Location = new System.Drawing.Point(168, 89);
            this.txtIntervalo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtIntervalo.Name = "txtIntervalo";
            this.txtIntervalo.Size = new System.Drawing.Size(51, 22);
            this.txtIntervalo.TabIndex = 55;
            this.txtIntervalo.Text = "0";
            this.txtIntervalo.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Book Antiqua", 12F);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label2.Location = new System.Drawing.Point(9, 276);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(162, 24);
            this.label2.TabIndex = 54;
            this.label2.Text = "Filtros Aplicados";
            // 
            // chlFiltros
            // 
            this.chlFiltros.FormattingEnabled = true;
            this.chlFiltros.Location = new System.Drawing.Point(15, 305);
            this.chlFiltros.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chlFiltros.Name = "chlFiltros";
            this.chlFiltros.Size = new System.Drawing.Size(305, 55);
            this.chlFiltros.TabIndex = 53;
            this.chlFiltros.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.chlFiltros_ItemCheck);
            // 
            // btnLimpiar
            // 
            this.btnLimpiar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnLimpiar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLimpiar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(118)))), ((int)(((byte)(118)))));
            this.btnLimpiar.FlatAppearance.BorderSize = 2;
            this.btnLimpiar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(108)))), ((int)(((byte)(108)))));
            this.btnLimpiar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLimpiar.Font = new System.Drawing.Font("Book Antiqua", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLimpiar.ForeColor = System.Drawing.Color.Black;
            this.btnLimpiar.Location = new System.Drawing.Point(220, 369);
            this.btnLimpiar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(100, 38);
            this.btnLimpiar.TabIndex = 52;
            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.UseVisualStyleBackColor = false;
            this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click);
            // 
            // cmbTipoFiltro
            // 
            this.cmbTipoFiltro.FormattingEnabled = true;
            this.cmbTipoFiltro.Location = new System.Drawing.Point(149, 38);
            this.cmbTipoFiltro.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbTipoFiltro.Name = "cmbTipoFiltro";
            this.cmbTipoFiltro.Size = new System.Drawing.Size(171, 24);
            this.cmbTipoFiltro.TabIndex = 51;
            this.cmbTipoFiltro.Text = "Seleccione";
            this.cmbTipoFiltro.SelectedIndexChanged += new System.EventHandler(this.cmbTipoFiltro_SelectedIndexChanged);
            // 
            // lblTipoFiltro
            // 
            this.lblTipoFiltro.AutoSize = true;
            this.lblTipoFiltro.Font = new System.Drawing.Font("Book Antiqua", 12F);
            this.lblTipoFiltro.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblTipoFiltro.Location = new System.Drawing.Point(9, 36);
            this.lblTipoFiltro.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTipoFiltro.Name = "lblTipoFiltro";
            this.lblTipoFiltro.Size = new System.Drawing.Size(128, 24);
            this.lblTipoFiltro.TabIndex = 50;
            this.lblTipoFiltro.Text = "Tipo de filtro";
            // 
            // dtp2
            // 
            this.dtp2.CustomFormat = "HH:mm:ss";
            this.dtp2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp2.Location = new System.Drawing.Point(55, 171);
            this.dtp2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dtp2.Name = "dtp2";
            this.dtp2.ShowUpDown = true;
            this.dtp2.Size = new System.Drawing.Size(265, 22);
            this.dtp2.TabIndex = 49;
            // 
            // dtp1
            // 
            this.dtp1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtp1.Location = new System.Drawing.Point(55, 114);
            this.dtp1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dtp1.Name = "dtp1";
            this.dtp1.Size = new System.Drawing.Size(265, 22);
            this.dtp1.TabIndex = 48;
            // 
            // btnAceptar
            // 
            this.btnAceptar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(57)))), ((int)(((byte)(63)))));
            this.btnAceptar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAceptar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(118)))), ((int)(((byte)(118)))));
            this.btnAceptar.FlatAppearance.BorderSize = 2;
            this.btnAceptar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(108)))), ((int)(((byte)(108)))));
            this.btnAceptar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAceptar.Font = new System.Drawing.Font("Book Antiqua", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAceptar.ForeColor = System.Drawing.Color.White;
            this.btnAceptar.Location = new System.Drawing.Point(220, 241);
            this.btnAceptar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(101, 38);
            this.btnAceptar.TabIndex = 47;
            this.btnAceptar.Text = "Aceptar";
            this.btnAceptar.UseVisualStyleBackColor = false;
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            // 
            // lblAccion2
            // 
            this.lblAccion2.AutoSize = true;
            this.lblAccion2.Font = new System.Drawing.Font("Book Antiqua", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAccion2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblAccion2.Location = new System.Drawing.Point(8, 143);
            this.lblAccion2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAccion2.Name = "lblAccion2";
            this.lblAccion2.Size = new System.Drawing.Size(81, 24);
            this.lblAccion2.TabIndex = 46;
            this.lblAccion2.Text = "Horario";
            // 
            // lblAccion1
            // 
            this.lblAccion1.AutoSize = true;
            this.lblAccion1.Font = new System.Drawing.Font("Book Antiqua", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAccion1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblAccion1.Location = new System.Drawing.Point(8, 86);
            this.lblAccion1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAccion1.Name = "lblAccion1";
            this.lblAccion1.Size = new System.Drawing.Size(62, 24);
            this.lblAccion1.TabIndex = 45;
            this.lblAccion1.Text = "Fecha";
            // 
            // lblTituloAccion
            // 
            this.lblTituloAccion.AutoSize = true;
            this.lblTituloAccion.Font = new System.Drawing.Font("Book Antiqua", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTituloAccion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblTituloAccion.Location = new System.Drawing.Point(8, 4);
            this.lblTituloAccion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTituloAccion.Name = "lblTituloAccion";
            this.lblTituloAccion.Size = new System.Drawing.Size(171, 28);
            this.lblTituloAccion.TabIndex = 44;
            this.lblTituloAccion.Text = "Tiltulo Accion";
            // 
            // pnlAccionesFinales
            // 
            this.pnlAccionesFinales.Controls.Add(this.btnCalcularFDP);
            this.pnlAccionesFinales.Location = new System.Drawing.Point(888, 663);
            this.pnlAccionesFinales.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlAccionesFinales.Name = "pnlAccionesFinales";
            this.pnlAccionesFinales.Size = new System.Drawing.Size(376, 55);
            this.pnlAccionesFinales.TabIndex = 3;
            // 
            // btnCalcularFDP
            // 
            this.btnCalcularFDP.BackColor = System.Drawing.Color.Black;
            this.btnCalcularFDP.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCalcularFDP.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnCalcularFDP.FlatAppearance.BorderSize = 2;
            this.btnCalcularFDP.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnCalcularFDP.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCalcularFDP.Font = new System.Drawing.Font("Book Antiqua", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCalcularFDP.ForeColor = System.Drawing.Color.White;
            this.btnCalcularFDP.Location = new System.Drawing.Point(71, 4);
            this.btnCalcularFDP.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCalcularFDP.Name = "btnCalcularFDP";
            this.btnCalcularFDP.Size = new System.Drawing.Size(235, 44);
            this.btnCalcularFDP.TabIndex = 16;
            this.btnCalcularFDP.Text = "Calcular FDP";
            this.btnCalcularFDP.UseVisualStyleBackColor = false;
            this.btnCalcularFDP.Click += new System.EventHandler(this.btnCalcularFDP_Click);
            // 
            // pnlTitulo
            // 
            this.pnlTitulo.BackColor = System.Drawing.Color.White;
            this.pnlTitulo.Controls.Add(this.pictureBox1);
            this.pnlTitulo.Controls.Add(this.label1);
            this.pnlTitulo.Location = new System.Drawing.Point(1, 0);
            this.pnlTitulo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlTitulo.Name = "pnlTitulo";
            this.pnlTitulo.Size = new System.Drawing.Size(1279, 100);
            this.pnlTitulo.TabIndex = 21;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(1100, 0);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(180, 100);
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
            this.pnlMensaje.Location = new System.Drawing.Point(275, 100);
            this.pnlMensaje.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlMensaje.Name = "pnlMensaje";
            this.pnlMensaje.Size = new System.Drawing.Size(1005, 23);
            this.pnlMensaje.TabIndex = 22;
            // 
            // lblMensaje
            // 
            this.lblMensaje.Font = new System.Drawing.Font("Book Antiqua", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMensaje.Location = new System.Drawing.Point(12, 0);
            this.lblMensaje.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMensaje.Name = "lblMensaje";
            this.lblMensaje.Size = new System.Drawing.Size(1000, 23);
            this.lblMensaje.TabIndex = 22;
            this.lblMensaje.Text = "Mensaje";
            this.lblMensaje.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblMensaje.Visible = false;
            // 
            // timerMensaje
            // 
            this.timerMensaje.Interval = 2000;
            this.timerMensaje.Tick += new System.EventHandler(this.timerMensaje_Tick);
            // 
            // btnAgregarRegistro
            // 
            this.btnAgregarRegistro.BackColor = System.Drawing.Color.Black;
            this.btnAgregarRegistro.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAgregarRegistro.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnAgregarRegistro.FlatAppearance.CheckedBackColor = System.Drawing.Color.White;
            this.btnAgregarRegistro.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnAgregarRegistro.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAgregarRegistro.Font = new System.Drawing.Font("Book Antiqua", 13F);
            this.btnAgregarRegistro.ForeColor = System.Drawing.Color.White;
            this.btnAgregarRegistro.Location = new System.Drawing.Point(0, 0);
            this.btnAgregarRegistro.Margin = new System.Windows.Forms.Padding(0);
            this.btnAgregarRegistro.Name = "btnAgregarRegistro";
            this.btnAgregarRegistro.Size = new System.Drawing.Size(280, 101);
            this.btnAgregarRegistro.TabIndex = 14;
            this.btnAgregarRegistro.Text = "Agregar Registro";
            this.btnAgregarRegistro.UseVisualStyleBackColor = false;
            this.btnAgregarRegistro.Click += new System.EventHandler(this.btnAgregarRegistro_Click);
            // 
            // btnModificarRegistro
            // 
            this.btnModificarRegistro.BackColor = System.Drawing.Color.Black;
            this.btnModificarRegistro.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnModificarRegistro.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnModificarRegistro.FlatAppearance.CheckedBackColor = System.Drawing.Color.White;
            this.btnModificarRegistro.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnModificarRegistro.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnModificarRegistro.Font = new System.Drawing.Font("Book Antiqua", 13F);
            this.btnModificarRegistro.ForeColor = System.Drawing.Color.White;
            this.btnModificarRegistro.Location = new System.Drawing.Point(0, 101);
            this.btnModificarRegistro.Margin = new System.Windows.Forms.Padding(0);
            this.btnModificarRegistro.Name = "btnModificarRegistro";
            this.btnModificarRegistro.Size = new System.Drawing.Size(280, 101);
            this.btnModificarRegistro.TabIndex = 14;
            this.btnModificarRegistro.Text = "Modificar Registro";
            this.btnModificarRegistro.UseVisualStyleBackColor = false;
            this.btnModificarRegistro.Click += new System.EventHandler(this.btnModificarRegistro_Click);
            // 
            // btnVolver
            // 
            this.btnVolver.BackColor = System.Drawing.Color.Black;
            this.btnVolver.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnVolver.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnVolver.FlatAppearance.CheckedBackColor = System.Drawing.Color.White;
            this.btnVolver.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnVolver.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVolver.Font = new System.Drawing.Font("Book Antiqua", 13F);
            this.btnVolver.ForeColor = System.Drawing.Color.White;
            this.btnVolver.Location = new System.Drawing.Point(0, 650);
            this.btnVolver.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnVolver.Name = "btnVolver";
            this.btnVolver.Size = new System.Drawing.Size(280, 70);
            this.btnVolver.TabIndex = 6;
            this.btnVolver.Text = "Volver";
            this.btnVolver.UseVisualStyleBackColor = false;
            this.btnVolver.Click += new System.EventHandler(this.btnVolver_Click);
            // 
            // btnBorrarSeleccionados
            // 
            this.btnBorrarSeleccionados.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(134)))), ((int)(((byte)(0)))), ((int)(((byte)(3)))));
            this.btnBorrarSeleccionados.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBorrarSeleccionados.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnBorrarSeleccionados.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(79)))), ((int)(((byte)(79)))));
            this.btnBorrarSeleccionados.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBorrarSeleccionados.Font = new System.Drawing.Font("Book Antiqua", 13F);
            this.btnBorrarSeleccionados.ForeColor = System.Drawing.Color.White;
            this.btnBorrarSeleccionados.Location = new System.Drawing.Point(0, 404);
            this.btnBorrarSeleccionados.Margin = new System.Windows.Forms.Padding(0);
            this.btnBorrarSeleccionados.Name = "btnBorrarSeleccionados";
            this.btnBorrarSeleccionados.Size = new System.Drawing.Size(280, 101);
            this.btnBorrarSeleccionados.TabIndex = 14;
            this.btnBorrarSeleccionados.Text = "Borrar Seleccionados";
            this.btnBorrarSeleccionados.UseVisualStyleBackColor = false;
            this.btnBorrarSeleccionados.Click += new System.EventHandler(this.btnBorrarSeleccionados_Click);
            // 
            // btnFiltrar
            // 
            this.btnFiltrar.BackColor = System.Drawing.Color.Black;
            this.btnFiltrar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFiltrar.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnFiltrar.FlatAppearance.CheckedBackColor = System.Drawing.Color.White;
            this.btnFiltrar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnFiltrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFiltrar.Font = new System.Drawing.Font("Book Antiqua", 13F);
            this.btnFiltrar.ForeColor = System.Drawing.Color.White;
            this.btnFiltrar.Location = new System.Drawing.Point(0, 303);
            this.btnFiltrar.Margin = new System.Windows.Forms.Padding(0);
            this.btnFiltrar.Name = "btnFiltrar";
            this.btnFiltrar.Size = new System.Drawing.Size(280, 101);
            this.btnFiltrar.TabIndex = 14;
            this.btnFiltrar.Text = "Filtrar";
            this.btnFiltrar.UseVisualStyleBackColor = false;
            this.btnFiltrar.Click += new System.EventHandler(this.btnFiltrar_Click);
            // 
            // btnSeleccionarTodos
            // 
            this.btnSeleccionarTodos.BackColor = System.Drawing.Color.Black;
            this.btnSeleccionarTodos.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSeleccionarTodos.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnSeleccionarTodos.FlatAppearance.CheckedBackColor = System.Drawing.Color.White;
            this.btnSeleccionarTodos.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnSeleccionarTodos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSeleccionarTodos.Font = new System.Drawing.Font("Book Antiqua", 13F);
            this.btnSeleccionarTodos.ForeColor = System.Drawing.Color.White;
            this.btnSeleccionarTodos.Location = new System.Drawing.Point(0, 202);
            this.btnSeleccionarTodos.Margin = new System.Windows.Forms.Padding(0);
            this.btnSeleccionarTodos.Name = "btnSeleccionarTodos";
            this.btnSeleccionarTodos.Size = new System.Drawing.Size(280, 101);
            this.btnSeleccionarTodos.TabIndex = 14;
            this.btnSeleccionarTodos.Text = "Seleccionar Todos";
            this.btnSeleccionarTodos.UseVisualStyleBackColor = false;
            this.btnSeleccionarTodos.Click += new System.EventHandler(this.btnSeleccionarTodos_Click);
            // 
            // pnlAcciones
            // 
            this.pnlAcciones.BackColor = System.Drawing.Color.Black;
            this.pnlAcciones.Controls.Add(this.btnAgregarRegistro);
            this.pnlAcciones.Controls.Add(this.btnModificarRegistro);
            this.pnlAcciones.Controls.Add(this.btnSeleccionarTodos);
            this.pnlAcciones.Controls.Add(this.btnFiltrar);
            this.pnlAcciones.Controls.Add(this.btnBorrarSeleccionados);
            this.pnlAcciones.Location = new System.Drawing.Point(0, 100);
            this.pnlAcciones.Margin = new System.Windows.Forms.Padding(0);
            this.pnlAcciones.Name = "pnlAcciones";
            this.pnlAcciones.Size = new System.Drawing.Size(280, 550);
            this.pnlAcciones.TabIndex = 22;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pnlSegmentacion);
            this.panel1.Controls.Add(this.rbDtConstante);
            this.panel1.Controls.Add(this.rbEventoAEvento);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Location = new System.Drawing.Point(888, 554);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(376, 100);
            this.panel1.TabIndex = 23;
            // 
            // pnlSegmentacion
            // 
            this.pnlSegmentacion.Controls.Add(this.rbSegundo);
            this.pnlSegmentacion.Controls.Add(this.rbDia);
            this.pnlSegmentacion.Controls.Add(this.rbMinuto);
            this.pnlSegmentacion.Controls.Add(this.rbHora);
            this.pnlSegmentacion.Location = new System.Drawing.Point(0, 68);
            this.pnlSegmentacion.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlSegmentacion.Name = "pnlSegmentacion";
            this.pnlSegmentacion.Size = new System.Drawing.Size(376, 32);
            this.pnlSegmentacion.TabIndex = 58;
            this.pnlSegmentacion.Visible = false;
            // 
            // rbSegundo
            // 
            this.rbSegundo.AutoSize = true;
            this.rbSegundo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rbSegundo.Location = new System.Drawing.Point(264, 4);
            this.rbSegundo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rbSegundo.Name = "rbSegundo";
            this.rbSegundo.Size = new System.Drawing.Size(86, 21);
            this.rbSegundo.TabIndex = 3;
            this.rbSegundo.Text = "Segundo";
            this.rbSegundo.UseVisualStyleBackColor = true;
            // 
            // rbDia
            // 
            this.rbDia.AutoSize = true;
            this.rbDia.Checked = true;
            this.rbDia.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rbDia.Location = new System.Drawing.Point(15, 4);
            this.rbDia.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rbDia.Name = "rbDia";
            this.rbDia.Size = new System.Drawing.Size(50, 21);
            this.rbDia.TabIndex = 2;
            this.rbDia.TabStop = true;
            this.rbDia.Text = "Dia";
            this.rbDia.UseVisualStyleBackColor = true;
            // 
            // rbMinuto
            // 
            this.rbMinuto.AutoSize = true;
            this.rbMinuto.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rbMinuto.Location = new System.Drawing.Point(180, 4);
            this.rbMinuto.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rbMinuto.Name = "rbMinuto";
            this.rbMinuto.Size = new System.Drawing.Size(71, 21);
            this.rbMinuto.TabIndex = 1;
            this.rbMinuto.Text = "Minuto";
            this.rbMinuto.UseVisualStyleBackColor = true;
            // 
            // rbHora
            // 
            this.rbHora.AutoSize = true;
            this.rbHora.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rbHora.Location = new System.Drawing.Point(93, 4);
            this.rbHora.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rbHora.Name = "rbHora";
            this.rbHora.Size = new System.Drawing.Size(60, 21);
            this.rbHora.TabIndex = 0;
            this.rbHora.Text = "Hora";
            this.rbHora.UseVisualStyleBackColor = true;
            // 
            // rbDtConstante
            // 
            this.rbDtConstante.AutoSize = true;
            this.rbDtConstante.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rbDtConstante.Font = new System.Drawing.Font("Book Antiqua", 10F);
            this.rbDtConstante.Location = new System.Drawing.Point(195, 41);
            this.rbDtConstante.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rbDtConstante.Name = "rbDtConstante";
            this.rbDtConstante.Size = new System.Drawing.Size(125, 26);
            this.rbDtConstante.TabIndex = 57;
            this.rbDtConstante.TabStop = true;
            this.rbDtConstante.Text = "Δt constante";
            this.rbDtConstante.UseVisualStyleBackColor = true;
            this.rbDtConstante.CheckedChanged += new System.EventHandler(this.rbDtConstante_CheckedChanged);
            // 
            // rbEventoAEvento
            // 
            this.rbEventoAEvento.AutoSize = true;
            this.rbEventoAEvento.Checked = true;
            this.rbEventoAEvento.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rbEventoAEvento.Font = new System.Drawing.Font("Book Antiqua", 10F);
            this.rbEventoAEvento.Location = new System.Drawing.Point(15, 41);
            this.rbEventoAEvento.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rbEventoAEvento.Name = "rbEventoAEvento";
            this.rbEventoAEvento.Size = new System.Drawing.Size(152, 26);
            this.rbEventoAEvento.TabIndex = 56;
            this.rbEventoAEvento.TabStop = true;
            this.rbEventoAEvento.Text = "Evento a evento";
            this.rbEventoAEvento.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Book Antiqua", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label3.Location = new System.Drawing.Point(9, 9);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(151, 28);
            this.label3.TabIndex = 55;
            this.label3.Text = "Metodologia";
            // 
            // lblTituloProyecto
            // 
            this.lblTituloProyecto.AutoSize = true;
            this.lblTituloProyecto.Font = new System.Drawing.Font("Book Antiqua", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTituloProyecto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblTituloProyecto.Location = new System.Drawing.Point(333, 132);
            this.lblTituloProyecto.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTituloProyecto.Name = "lblTituloProyecto";
            this.lblTituloProyecto.Size = new System.Drawing.Size(180, 28);
            this.lblTituloProyecto.TabIndex = 64;
            this.lblTituloProyecto.Text = "Titulo Proyecto";
            // 
            // FrmProcesmientoDatos
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.ClientSize = new System.Drawing.Size(1280, 720);
            this.Controls.Add(this.lblTituloProyecto);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnlTitulo);
            this.Controls.Add(this.pnlAcciones);
            this.Controls.Add(this.pnlMensaje);
            this.Controls.Add(this.pnlAccionesFinales);
            this.Controls.Add(this.pnlModificable);
            this.Controls.Add(this.pnlEventos);
            this.Controls.Add(this.btnVolver);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.Name = "FrmProcesmientoDatos";
            this.Opacity = 0.98D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.pnlEventos.ResumeLayout(false);
            this.pnlEventos.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgwEventos)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.pnlModificable.ResumeLayout(false);
            this.pnlModificable.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudAgregarPorIntervalo)).EndInit();
            this.pnlAccionesFinales.ResumeLayout(false);
            this.pnlTitulo.ResumeLayout(false);
            this.pnlTitulo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.pnlMensaje.ResumeLayout(false);
            this.pnlAcciones.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnlSegmentacion.ResumeLayout(false);
            this.pnlSegmentacion.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel pnlEventos;
        private System.Windows.Forms.Panel pnlModificable;
        private System.Windows.Forms.Panel pnlAccionesFinales;
        private System.Windows.Forms.Panel pnlTitulo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlMensaje;
        private System.Windows.Forms.DataGridView dgwEventos;
        private System.Windows.Forms.Label lblEventos;
        private System.Windows.Forms.Button btnCalcularFDP;
        private System.Windows.Forms.DateTimePicker dtp2;
        private System.Windows.Forms.DateTimePicker dtp1;
        private System.Windows.Forms.Button btnAceptar;
        private System.Windows.Forms.Label lblAccion2;
        private System.Windows.Forms.Label lblAccion1;
        private System.Windows.Forms.Label lblTituloAccion;
        private System.Windows.Forms.ComboBox cmbTipoFiltro;
        private System.Windows.Forms.Label lblTipoFiltro;
        private System.Windows.Forms.Button btnLimpiar;
        private System.Windows.Forms.Timer timerMensaje;
        private System.Windows.Forms.Label lblMensaje;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.CheckedListBox chlFiltros;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnAgregarRegistro;
        private System.Windows.Forms.Button btnModificarRegistro;
        private System.Windows.Forms.Button btnVolver;
        private System.Windows.Forms.Button btnBorrarSeleccionados;
        private System.Windows.Forms.Button btnFiltrar;
        private System.Windows.Forms.Button btnSeleccionarTodos;
        private System.Windows.Forms.FlowLayoutPanel pnlAcciones;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rbDtConstante;
        private System.Windows.Forms.RadioButton rbEventoAEvento;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel pnlSegmentacion;
        private System.Windows.Forms.RadioButton rbDia;
        private System.Windows.Forms.RadioButton rbMinuto;
        private System.Windows.Forms.RadioButton rbHora;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton rbFecha;
        private System.Windows.Forms.Label lblVista;
        private System.Windows.Forms.RadioButton rbIntervalos;
        private System.Windows.Forms.TextBox txtIntervalo;
        private System.Windows.Forms.TextBox txtIntervalo2;
        private System.Windows.Forms.RadioButton rbSegundo;
        private System.Windows.Forms.Label lblTituloProyecto;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lblMedia;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblCantidad;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbAgregarPorIntervalo;
        private System.Windows.Forms.RadioButton rbAgregarPorIntervalo;
        private System.Windows.Forms.RadioButton rbAgregarPorFechaYHora;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.NumericUpDown nudAgregarPorIntervalo;
    }
}