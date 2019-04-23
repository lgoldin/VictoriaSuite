namespace Kairos.Forms
{
    partial class FrmCapturaDatosArduino
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.buttonComenzar = new System.Windows.Forms.Button();
            this.buttonFinalizar = new System.Windows.Forms.Button();
            this.buttonExplorar = new System.Windows.Forms.Button();
            this.textBoxExplorar = new System.Windows.Forms.TextBox();
            this.labelEstado = new System.Windows.Forms.Label();
            this.labelLectura = new System.Windows.Forms.Label();
            this.errorProviderExplorar = new System.Windows.Forms.ErrorProvider(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxPuerto = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.rbExistingFile = new System.Windows.Forms.RadioButton();
            this.rbNewFile = new System.Windows.Forms.RadioButton();
            this.timerMensaje = new System.Windows.Forms.Timer(this.components);
            this.pnlMensaje = new System.Windows.Forms.Panel();
            this.lblMensaje = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderExplorar)).BeginInit();
            this.pnlMensaje.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonComenzar
            // 
            this.buttonComenzar.AutoSize = true;
            this.buttonComenzar.BackColor = System.Drawing.Color.Black;
            this.buttonComenzar.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonComenzar.Font = new System.Drawing.Font("Book Antiqua", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonComenzar.ForeColor = System.Drawing.Color.White;
            this.buttonComenzar.Location = new System.Drawing.Point(0, 0);
            this.buttonComenzar.Name = "buttonComenzar";
            this.buttonComenzar.Size = new System.Drawing.Size(92, 244);
            this.buttonComenzar.TabIndex = 2;
            this.buttonComenzar.Text = "Comenzar";
            this.buttonComenzar.UseVisualStyleBackColor = false;
            this.buttonComenzar.Click += new System.EventHandler(this.buttonComenzar_Click);
            // 
            // buttonFinalizar
            // 
            this.buttonFinalizar.AutoSize = true;
            this.buttonFinalizar.BackColor = System.Drawing.Color.Black;
            this.buttonFinalizar.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonFinalizar.Font = new System.Drawing.Font("Book Antiqua", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonFinalizar.ForeColor = System.Drawing.Color.White;
            this.buttonFinalizar.Location = new System.Drawing.Point(373, 0);
            this.buttonFinalizar.Name = "buttonFinalizar";
            this.buttonFinalizar.Size = new System.Drawing.Size(82, 244);
            this.buttonFinalizar.TabIndex = 3;
            this.buttonFinalizar.Text = "Finalizar";
            this.buttonFinalizar.UseVisualStyleBackColor = false;
            this.buttonFinalizar.Click += new System.EventHandler(this.buttonFinalizar_Click);
            // 
            // buttonExplorar
            // 
            this.buttonExplorar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.buttonExplorar.Enabled = false;
            this.buttonExplorar.Font = new System.Drawing.Font("Book Antiqua", 10F);
            this.buttonExplorar.ForeColor = System.Drawing.Color.White;
            this.buttonExplorar.Location = new System.Drawing.Point(279, 123);
            this.buttonExplorar.Name = "buttonExplorar";
            this.buttonExplorar.Size = new System.Drawing.Size(78, 32);
            this.buttonExplorar.TabIndex = 4;
            this.buttonExplorar.Text = "Explorar";
            this.buttonExplorar.UseVisualStyleBackColor = false;
            this.buttonExplorar.Click += new System.EventHandler(this.buttonExplorar_Click);
            // 
            // textBoxExplorar
            // 
            this.textBoxExplorar.Enabled = false;
            this.textBoxExplorar.Location = new System.Drawing.Point(114, 132);
            this.textBoxExplorar.Name = "textBoxExplorar";
            this.textBoxExplorar.Size = new System.Drawing.Size(159, 20);
            this.textBoxExplorar.TabIndex = 5;
            // 
            // labelEstado
            // 
            this.labelEstado.AutoSize = true;
            this.labelEstado.Font = new System.Drawing.Font("Book Antiqua", 11F);
            this.labelEstado.Location = new System.Drawing.Point(98, 183);
            this.labelEstado.Name = "labelEstado";
            this.labelEstado.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelEstado.Size = new System.Drawing.Size(272, 20);
            this.labelEstado.TabIndex = 6;
            this.labelEstado.Text = "Presione comenzar para capturar datos";
            this.labelEstado.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelLectura
            // 
            this.labelLectura.AutoSize = true;
            this.labelLectura.Location = new System.Drawing.Point(213, 170);
            this.labelLectura.Name = "labelLectura";
            this.labelLectura.Size = new System.Drawing.Size(0, 13);
            this.labelLectura.TabIndex = 7;
            // 
            // errorProviderExplorar
            // 
            this.errorProviderExplorar.ContainerControl = this;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Book Antiqua", 11F);
            this.label1.Location = new System.Drawing.Point(120, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 20);
            this.label1.TabIndex = 11;
            this.label1.Text = "Puerto Serial";
            // 
            // comboBoxPuerto
            // 
            this.comboBoxPuerto.FormattingEnabled = true;
            this.comboBoxPuerto.Location = new System.Drawing.Point(216, 43);
            this.comboBoxPuerto.Name = "comboBoxPuerto";
            this.comboBoxPuerto.Size = new System.Drawing.Size(121, 21);
            this.comboBoxPuerto.TabIndex = 10;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Book Antiqua", 11F);
            this.label2.Location = new System.Drawing.Point(120, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(184, 20);
            this.label2.TabIndex = 12;
            this.label2.Text = "Deteccion de Puerto Serial";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Book Antiqua", 11F);
            this.label3.Location = new System.Drawing.Point(119, 77);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(129, 20);
            this.label3.TabIndex = 15;
            this.label3.Text = "Archivo a Escribir";
            // 
            // rbExistingFile
            // 
            this.rbExistingFile.AutoSize = true;
            this.rbExistingFile.Location = new System.Drawing.Point(251, 100);
            this.rbExistingFile.Name = "rbExistingFile";
            this.rbExistingFile.Size = new System.Drawing.Size(68, 17);
            this.rbExistingFile.TabIndex = 17;
            this.rbExistingFile.TabStop = true;
            this.rbExistingFile.Text = "Existente";
            this.rbExistingFile.UseVisualStyleBackColor = true;
            this.rbExistingFile.CheckedChanged += new System.EventHandler(this.rbExistingFile_CheckedChanged);
            // 
            // rbNewFile
            // 
            this.rbNewFile.AutoSize = true;
            this.rbNewFile.Location = new System.Drawing.Point(123, 100);
            this.rbNewFile.Name = "rbNewFile";
            this.rbNewFile.Size = new System.Drawing.Size(57, 17);
            this.rbNewFile.TabIndex = 16;
            this.rbNewFile.TabStop = true;
            this.rbNewFile.Text = "Nuevo";
            this.rbNewFile.UseVisualStyleBackColor = true;
            this.rbNewFile.CheckedChanged += new System.EventHandler(this.rbNewFile_CheckedChanged);
            // 
            // timerMensaje
            // 
            this.timerMensaje.Interval = 2000;
            this.timerMensaje.Tick += new System.EventHandler(this.timerMensaje_Tick);
            // 
            // pnlMensaje
            // 
            this.pnlMensaje.Controls.Add(this.lblMensaje);
            this.pnlMensaje.Location = new System.Drawing.Point(90, 2);
            this.pnlMensaje.Name = "pnlMensaje";
            this.pnlMensaje.Size = new System.Drawing.Size(286, 19);
            this.pnlMensaje.TabIndex = 24;
            // 
            // lblMensaje
            // 
            this.lblMensaje.Font = new System.Drawing.Font("Book Antiqua", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMensaje.Location = new System.Drawing.Point(9, 2);
            this.lblMensaje.Name = "lblMensaje";
            this.lblMensaje.Size = new System.Drawing.Size(274, 17);
            this.lblMensaje.TabIndex = 22;
            this.lblMensaje.Text = "Mensaje";
            this.lblMensaje.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblMensaje.Visible = false;
            // 
            // FrmCapturaDatosArduino
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.ClientSize = new System.Drawing.Size(455, 244);
            this.Controls.Add(this.pnlMensaje);
            this.Controls.Add(this.rbExistingFile);
            this.Controls.Add(this.rbNewFile);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxPuerto);
            this.Controls.Add(this.labelLectura);
            this.Controls.Add(this.labelEstado);
            this.Controls.Add(this.textBoxExplorar);
            this.Controls.Add(this.buttonExplorar);
            this.Controls.Add(this.buttonFinalizar);
            this.Controls.Add(this.buttonComenzar);
            this.Name = "FrmCapturaDatosArduino";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Kairos - Captura de datos";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderExplorar)).EndInit();
            this.pnlMensaje.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonComenzar;
        private System.Windows.Forms.Button buttonFinalizar;
        private System.Windows.Forms.Button buttonExplorar;
        private System.Windows.Forms.TextBox textBoxExplorar;
        private System.Windows.Forms.Label labelEstado;
        private System.Windows.Forms.Label labelLectura;
        private System.Windows.Forms.ErrorProvider errorProviderExplorar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxPuerto;
        private System.Windows.Forms.RadioButton rbExistingFile;
        private System.Windows.Forms.RadioButton rbNewFile;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Timer timerMensaje;
        private System.Windows.Forms.Label lblMensaje;
        private System.Windows.Forms.Panel pnlMensaje;
    }
}

