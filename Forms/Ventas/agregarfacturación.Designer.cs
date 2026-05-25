namespace appInventario.Form_Ventas
{
    partial class agregarfacturación
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(agregarfacturación));
            this.label10 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.comboBoxSolicitud = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxPaqueteria = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtcodigopostal = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtrfc = new System.Windows.Forms.TextBox();
            this.txtrazon = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.comboBoxMetodo = new System.Windows.Forms.ComboBox();
            this.comboBoxForma = new System.Windows.Forms.ComboBox();
            this.comboBoxCFDI = new System.Windows.Forms.ComboBox();
            this.comboBoxRegimen = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btnañadir = new System.Windows.Forms.Button();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label10
            // 
            this.label10.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Malgun Gothic", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(62)))), ((int)(((byte)(89)))));
            this.label10.Location = new System.Drawing.Point(44, 30);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(175, 32);
            this.label10.TabIndex = 0;
            this.label10.Text = "Realizar venta";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.comboBoxSolicitud);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.comboBoxPaqueteria);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.txtcodigopostal);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.txtrfc);
            this.panel2.Controls.Add(this.txtrazon);
            this.panel2.Controls.Add(this.label11);
            this.panel2.Controls.Add(this.label15);
            this.panel2.Controls.Add(this.comboBoxMetodo);
            this.panel2.Controls.Add(this.comboBoxForma);
            this.panel2.Controls.Add(this.comboBoxCFDI);
            this.panel2.Controls.Add(this.comboBoxRegimen);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.btnañadir);
            this.panel2.Location = new System.Drawing.Point(0, 81);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(885, 481);
            this.panel2.TabIndex = 7;
            // 
            // comboBoxSolicitud
            // 
            this.comboBoxSolicitud.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.comboBoxSolicitud.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSolicitud.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxSolicitud.FormattingEnabled = true;
            this.comboBoxSolicitud.Items.AddRange(new object[] {
            "Si",
            "No"});
            this.comboBoxSolicitud.Location = new System.Drawing.Point(477, 36);
            this.comboBoxSolicitud.Name = "comboBoxSolicitud";
            this.comboBoxSolicitud.Size = new System.Drawing.Size(69, 28);
            this.comboBoxSolicitud.TabIndex = 0;
            this.comboBoxSolicitud.SelectedIndexChanged += new System.EventHandler(this.comboBoxSolicitud_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label2.Location = new System.Drawing.Point(334, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(137, 20);
            this.label2.TabIndex = 62;
            this.label2.Text = "¿Solicita Factura?";
            // 
            // comboBoxPaqueteria
            // 
            this.comboBoxPaqueteria.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.comboBoxPaqueteria.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPaqueteria.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxPaqueteria.FormattingEnabled = true;
            this.comboBoxPaqueteria.Items.AddRange(new object[] {
            "Seleccionar",
            "DHL",
            "FedEx",
            "Tres Guerras"});
            this.comboBoxPaqueteria.Location = new System.Drawing.Point(106, 131);
            this.comboBoxPaqueteria.Name = "comboBoxPaqueteria";
            this.comboBoxPaqueteria.Size = new System.Drawing.Size(198, 28);
            this.comboBoxPaqueteria.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label1.Location = new System.Drawing.Point(102, 101);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(226, 20);
            this.label1.TabIndex = 61;
            this.label1.Text = "Selecciona una forma de envío";
            // 
            // txtcodigopostal
            // 
            this.txtcodigopostal.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtcodigopostal.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtcodigopostal.Location = new System.Drawing.Point(106, 341);
            this.txtcodigopostal.Name = "txtcodigopostal";
            this.txtcodigopostal.Size = new System.Drawing.Size(198, 26);
            this.txtcodigopostal.TabIndex = 4;
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label8.Location = new System.Drawing.Point(102, 311);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(111, 20);
            this.label8.TabIndex = 59;
            this.label8.Text = "Codigo Postal:";
            // 
            // txtrfc
            // 
            this.txtrfc.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtrfc.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtrfc.Location = new System.Drawing.Point(106, 271);
            this.txtrfc.Name = "txtrfc";
            this.txtrfc.Size = new System.Drawing.Size(198, 26);
            this.txtrfc.TabIndex = 3;
            // 
            // txtrazon
            // 
            this.txtrazon.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtrazon.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtrazon.Location = new System.Drawing.Point(106, 201);
            this.txtrazon.Name = "txtrazon";
            this.txtrazon.Size = new System.Drawing.Size(198, 26);
            this.txtrazon.TabIndex = 2;
            // 
            // label11
            // 
            this.label11.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label11.Location = new System.Drawing.Point(102, 241);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(46, 20);
            this.label11.TabIndex = 58;
            this.label11.Text = "RFC:";
            // 
            // label15
            // 
            this.label15.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label15.Location = new System.Drawing.Point(102, 171);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(104, 20);
            this.label15.TabIndex = 57;
            this.label15.Text = "Razon social:";
            // 
            // comboBoxMetodo
            // 
            this.comboBoxMetodo.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.comboBoxMetodo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMetodo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxMetodo.FormattingEnabled = true;
            this.comboBoxMetodo.Items.AddRange(new object[] {
            "Seleccionar",
            "Pago en Una sola Exhibición (PUE)",
            "Pago en Parcialidades o Diferido (PPD)."});
            this.comboBoxMetodo.Location = new System.Drawing.Point(563, 271);
            this.comboBoxMetodo.Name = "comboBoxMetodo";
            this.comboBoxMetodo.Size = new System.Drawing.Size(198, 28);
            this.comboBoxMetodo.TabIndex = 7;
            // 
            // comboBoxForma
            // 
            this.comboBoxForma.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.comboBoxForma.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxForma.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxForma.FormattingEnabled = true;
            this.comboBoxForma.Items.AddRange(new object[] {
            "Seleccionar",
            "Efectivo",
            "Cheque nominativo",
            "Transferencia electrónica de fondos",
            "Tarjeta de crédito",
            "Monedero electrónico",
            "Dinero electrónico",
            "Vales de despensa",
            "Dación en pago",
            "Pago por subrogación",
            "Pago por consignación",
            "Condonación",
            "Compensación",
            "Novación",
            "Confusión",
            "Remisión de deuda",
            "Prescripción o caducidad",
            "A satisfacción del acreedor",
            "Tarjeta de débito",
            "Tarjeta de servicios",
            "Aplicación de anticipos",
            "Por definir"});
            this.comboBoxForma.Location = new System.Drawing.Point(563, 341);
            this.comboBoxForma.Name = "comboBoxForma";
            this.comboBoxForma.Size = new System.Drawing.Size(198, 28);
            this.comboBoxForma.TabIndex = 8;
            // 
            // comboBoxCFDI
            // 
            this.comboBoxCFDI.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.comboBoxCFDI.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCFDI.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxCFDI.FormattingEnabled = true;
            this.comboBoxCFDI.Items.AddRange(new object[] {
            "Seleccionar",
            "Adquisición de mercancías",
            "Devoluciones, descuentos o bonificaciones",
            "Gastos en general",
            "Construcciones",
            "Mobiliario y equipo de oficina por inversiones",
            "Equipo de transporte",
            "Equipo de cómputo y accesorios",
            "Dados, troqueles, moldes, matrices y herramental",
            "Comunicaciones telefónicas",
            "Comunicaciones satelitales",
            "Otra maquinaria y equipo",
            "Honorarios médicos, dentales y gastos hospitalarios",
            "Gastos médicos por incapacidad o discapacidad",
            "Gastos funerales",
            "Donativos",
            "Intereses reales efectivamente pagados por créditos hipotecarios (casa habitación" +
                ")",
            "Aportaciones voluntarias al SAR",
            "Primas por seguros de gastos médicos",
            "Gastos de transportación escolar obligatoria",
            "Depósitos en cuentas para el ahorro, primas que tengan como base planes de pensio" +
                "nes",
            "Pagos por servicios educativos (colegiaturas)",
            "Pagos",
            "Nómina",
            "Sin Efectos Fiscales"});
            this.comboBoxCFDI.Location = new System.Drawing.Point(563, 201);
            this.comboBoxCFDI.Name = "comboBoxCFDI";
            this.comboBoxCFDI.Size = new System.Drawing.Size(198, 28);
            this.comboBoxCFDI.TabIndex = 6;
            // 
            // comboBoxRegimen
            // 
            this.comboBoxRegimen.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.comboBoxRegimen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxRegimen.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxRegimen.FormattingEnabled = true;
            this.comboBoxRegimen.Items.AddRange(new object[] {
            "Seleccionar",
            "Régimen Simplificado de Confianza",
            "Sueldos y salarios e ingresos asimilados a salarios",
            "Régimen de Actividades Empresariales y Profesionales",
            "Régimen de Incorporación Fiscal",
            "Enajenación de bienes",
            "Régimen de Actividades Empresariales con ingresos a través de Plataformas Tecnoló" +
                "gicas",
            "Régimen de Arrendamiento",
            "Intereses",
            "Obtención de premios",
            "Dividendos",
            "Demás ingresos",
            "Sin Obligaciones Fiscales"});
            this.comboBoxRegimen.Location = new System.Drawing.Point(563, 131);
            this.comboBoxRegimen.Name = "comboBoxRegimen";
            this.comboBoxRegimen.Size = new System.Drawing.Size(198, 28);
            this.comboBoxRegimen.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label4.Location = new System.Drawing.Point(559, 311);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(121, 20);
            this.label4.TabIndex = 53;
            this.label4.Text = "Forma de pago:";
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label5.Location = new System.Drawing.Point(559, 241);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(129, 20);
            this.label5.TabIndex = 52;
            this.label5.Text = "Metodo de pago:";
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label6.Location = new System.Drawing.Point(559, 101);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(117, 20);
            this.label6.TabIndex = 51;
            this.label6.Text = "Regimen fiscal:";
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label7.Location = new System.Drawing.Point(559, 171);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(106, 20);
            this.label7.TabIndex = 50;
            this.label7.Text = "Uso de CFDI:";
            // 
            // btnañadir
            // 
            this.btnañadir.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnañadir.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnañadir.Location = new System.Drawing.Point(385, 397);
            this.btnañadir.Name = "btnañadir";
            this.btnañadir.Size = new System.Drawing.Size(116, 29);
            this.btnañadir.TabIndex = 9;
            this.btnañadir.Text = "Añadir";
            this.btnañadir.UseVisualStyleBackColor = true;
            this.btnañadir.Click += new System.EventHandler(this.btnañadir_Click);
            // 
            // agregarfacturación
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(884, 561);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.panel2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "agregarfacturación";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Realizar venta";
            this.Load += new System.EventHandler(this.agregarfacturación_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnañadir;
        private System.Windows.Forms.ComboBox comboBoxMetodo;
        private System.Windows.Forms.ComboBox comboBoxForma;
        private System.Windows.Forms.ComboBox comboBoxCFDI;
        private System.Windows.Forms.ComboBox comboBoxRegimen;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBoxPaqueteria;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtcodigopostal;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtrfc;
        private System.Windows.Forms.TextBox txtrazon;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox comboBoxSolicitud;
        private System.Windows.Forms.Label label2;
    }
}