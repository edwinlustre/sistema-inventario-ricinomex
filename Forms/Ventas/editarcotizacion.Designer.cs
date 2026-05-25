namespace appInventario.Form_Ventas
{
    partial class editarcotizacion
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(editarcotizacion));
            this.label9 = new System.Windows.Forms.Label();
            this.txtprecio = new System.Windows.Forms.TextBox();
            this.label25 = new System.Windows.Forms.Label();
            this.txtnumprod = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.btnactualizar = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.comboBoxCapacidad = new System.Windows.Forms.ComboBox();
            this.label22 = new System.Windows.Forms.Label();
            this.comboBoxProducto = new System.Windows.Forms.ComboBox();
            this.label23 = new System.Windows.Forms.Label();
            this.txtempresa = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.comboBoxAceite = new System.Windows.Forms.ComboBox();
            this.label18 = new System.Windows.Forms.Label();
            this.btnguardar = new System.Windows.Forms.Button();
            this.txtlitros = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.dateTimePickerCompra = new System.Windows.Forms.DateTimePicker();
            this.label13 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnagregar = new System.Windows.Forms.Button();
            this.btneliminar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label9
            // 
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Malgun Gothic", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(62)))), ((int)(((byte)(89)))));
            this.label9.Location = new System.Drawing.Point(38, 30);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(259, 32);
            this.label9.TabIndex = 16;
            this.label9.Text = "Edicion de cotizacion";
            this.label9.Click += new System.EventHandler(this.label9_Click);
            // 
            // txtprecio
            // 
            this.txtprecio.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtprecio.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtprecio.Location = new System.Drawing.Point(705, 258);
            this.txtprecio.Name = "txtprecio";
            this.txtprecio.Size = new System.Drawing.Size(155, 26);
            this.txtprecio.TabIndex = 7;
            // 
            // label25
            // 
            this.label25.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label25.AutoSize = true;
            this.label25.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label25.Location = new System.Drawing.Point(701, 228);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(186, 20);
            this.label25.TabIndex = 133;
            this.label25.Text = "Ingresa el precio unitario:";
            // 
            // txtnumprod
            // 
            this.txtnumprod.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtnumprod.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtnumprod.Location = new System.Drawing.Point(166, 258);
            this.txtnumprod.Name = "txtnumprod";
            this.txtnumprod.Size = new System.Drawing.Size(155, 26);
            this.txtnumprod.TabIndex = 3;
            // 
            // label24
            // 
            this.label24.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label24.AutoSize = true;
            this.label24.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label24.Location = new System.Drawing.Point(162, 228);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(148, 20);
            this.label24.TabIndex = 131;
            this.label24.Text = "Ingresa la cantidad:";
            // 
            // btnactualizar
            // 
            this.btnactualizar.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnactualizar.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnactualizar.Location = new System.Drawing.Point(381, 306);
            this.btnactualizar.Name = "btnactualizar";
            this.btnactualizar.Size = new System.Drawing.Size(147, 29);
            this.btnactualizar.TabIndex = 9;
            this.btnactualizar.Text = "Actualizar producto";
            this.btnactualizar.UseVisualStyleBackColor = true;
            this.btnactualizar.Click += new System.EventHandler(this.btnactualizar_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(121, 354);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(860, 227);
            this.dataGridView1.TabIndex = 12;
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            // 
            // comboBoxCapacidad
            // 
            this.comboBoxCapacidad.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.comboBoxCapacidad.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCapacidad.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxCapacidad.FormattingEnabled = true;
            this.comboBoxCapacidad.Items.AddRange(new object[] {
            "Seleccionar",
            "Kg",
            "Litros"});
            this.comboBoxCapacidad.Location = new System.Drawing.Point(705, 48);
            this.comboBoxCapacidad.Name = "comboBoxCapacidad";
            this.comboBoxCapacidad.Size = new System.Drawing.Size(231, 28);
            this.comboBoxCapacidad.TabIndex = 4;
            // 
            // label22
            // 
            this.label22.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label22.Location = new System.Drawing.Point(701, 18);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(184, 20);
            this.label22.TabIndex = 127;
            this.label22.Text = "Selecciona la capacidad:";
            // 
            // comboBoxProducto
            // 
            this.comboBoxProducto.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.comboBoxProducto.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxProducto.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxProducto.FormattingEnabled = true;
            this.comboBoxProducto.Items.AddRange(new object[] {
            "Seleccionar",
            "Tote",
            "Tambor",
            "Garrafa 70L",
            "Garrafa 50L",
            "Garrafa 20L",
            "Garrafa 10L",
            "Garrafa 5L",
            "Botella"});
            this.comboBoxProducto.Location = new System.Drawing.Point(166, 188);
            this.comboBoxProducto.Name = "comboBoxProducto";
            this.comboBoxProducto.Size = new System.Drawing.Size(231, 28);
            this.comboBoxProducto.TabIndex = 2;
            // 
            // label23
            // 
            this.label23.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label23.Location = new System.Drawing.Point(162, 158);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(174, 20);
            this.label23.TabIndex = 126;
            this.label23.Text = "Selecciona el producto:";
            // 
            // txtempresa
            // 
            this.txtempresa.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtempresa.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtempresa.Location = new System.Drawing.Point(166, 48);
            this.txtempresa.Name = "txtempresa";
            this.txtempresa.Size = new System.Drawing.Size(231, 26);
            this.txtempresa.TabIndex = 0;
            // 
            // label21
            // 
            this.label21.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.Location = new System.Drawing.Point(162, 18);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(148, 20);
            this.label21.TabIndex = 123;
            this.label21.Text = "Empresa / Persona:";
            // 
            // comboBoxAceite
            // 
            this.comboBoxAceite.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.comboBoxAceite.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAceite.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxAceite.FormattingEnabled = true;
            this.comboBoxAceite.Items.AddRange(new object[] {
            "Seleccionar",
            "USP",
            "Industrial",
            "Otro"});
            this.comboBoxAceite.Location = new System.Drawing.Point(705, 188);
            this.comboBoxAceite.Name = "comboBoxAceite";
            this.comboBoxAceite.Size = new System.Drawing.Size(231, 28);
            this.comboBoxAceite.TabIndex = 6;
            // 
            // label18
            // 
            this.label18.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label18.Location = new System.Drawing.Point(701, 158);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(112, 20);
            this.label18.TabIndex = 120;
            this.label18.Text = "Tipo de aceite:";
            // 
            // btnguardar
            // 
            this.btnguardar.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnguardar.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnguardar.Location = new System.Drawing.Point(734, 306);
            this.btnguardar.Name = "btnguardar";
            this.btnguardar.Size = new System.Drawing.Size(147, 29);
            this.btnguardar.TabIndex = 11;
            this.btnguardar.Text = "Guardar cambios";
            this.btnguardar.UseVisualStyleBackColor = true;
            this.btnguardar.Click += new System.EventHandler(this.btnguardar_Click);
            // 
            // txtlitros
            // 
            this.txtlitros.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtlitros.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtlitros.Location = new System.Drawing.Point(705, 120);
            this.txtlitros.Name = "txtlitros";
            this.txtlitros.Size = new System.Drawing.Size(155, 26);
            this.txtlitros.TabIndex = 5;
            // 
            // label14
            // 
            this.label14.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label14.Location = new System.Drawing.Point(701, 88);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(163, 20);
            this.label14.TabIndex = 119;
            this.label14.Text = "Ingresa los litros/kilos:";
            // 
            // dateTimePickerCompra
            // 
            this.dateTimePickerCompra.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.dateTimePickerCompra.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePickerCompra.Location = new System.Drawing.Point(166, 118);
            this.dateTimePickerCompra.Name = "dateTimePickerCompra";
            this.dateTimePickerCompra.Size = new System.Drawing.Size(232, 26);
            this.dateTimePickerCompra.TabIndex = 1;
            // 
            // label13
            // 
            this.label13.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label13.Location = new System.Drawing.Point(162, 88);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(137, 20);
            this.label13.TabIndex = 118;
            this.label13.Text = "Fecha de compra:";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.btnagregar);
            this.panel1.Controls.Add(this.btneliminar);
            this.panel1.Controls.Add(this.dataGridView1);
            this.panel1.Controls.Add(this.txtprecio);
            this.panel1.Controls.Add(this.label13);
            this.panel1.Controls.Add(this.label25);
            this.panel1.Controls.Add(this.dateTimePickerCompra);
            this.panel1.Controls.Add(this.txtnumprod);
            this.panel1.Controls.Add(this.label14);
            this.panel1.Controls.Add(this.label24);
            this.panel1.Controls.Add(this.txtlitros);
            this.panel1.Controls.Add(this.btnactualizar);
            this.panel1.Controls.Add(this.btnguardar);
            this.panel1.Controls.Add(this.label18);
            this.panel1.Controls.Add(this.comboBoxCapacidad);
            this.panel1.Controls.Add(this.comboBoxAceite);
            this.panel1.Controls.Add(this.label22);
            this.panel1.Controls.Add(this.label21);
            this.panel1.Controls.Add(this.comboBoxProducto);
            this.panel1.Controls.Add(this.txtempresa);
            this.panel1.Controls.Add(this.label23);
            this.panel1.Location = new System.Drawing.Point(2, 81);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1102, 604);
            this.panel1.TabIndex = 134;
            // 
            // btnagregar
            // 
            this.btnagregar.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnagregar.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnagregar.Location = new System.Drawing.Point(221, 306);
            this.btnagregar.Name = "btnagregar";
            this.btnagregar.Size = new System.Drawing.Size(104, 29);
            this.btnagregar.TabIndex = 8;
            this.btnagregar.Text = "Agregar";
            this.btnagregar.UseVisualStyleBackColor = true;
            this.btnagregar.Click += new System.EventHandler(this.btnagregar_Click);
            // 
            // btneliminar
            // 
            this.btneliminar.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btneliminar.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btneliminar.Location = new System.Drawing.Point(582, 306);
            this.btneliminar.Name = "btneliminar";
            this.btneliminar.Size = new System.Drawing.Size(104, 29);
            this.btneliminar.TabIndex = 10;
            this.btneliminar.Text = "Eliminar";
            this.btneliminar.UseVisualStyleBackColor = true;
            this.btneliminar.Click += new System.EventHandler(this.btneliminar_Click);
            // 
            // editarcotizacion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(1104, 681);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "editarcotizacion";
            this.Text = "Edicion de cotizaciones";
            this.Load += new System.EventHandler(this.editarcotizacion_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtprecio;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.TextBox txtnumprod;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Button btnactualizar;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ComboBox comboBoxCapacidad;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.ComboBox comboBoxProducto;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.TextBox txtempresa;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.ComboBox comboBoxAceite;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Button btnguardar;
        private System.Windows.Forms.TextBox txtlitros;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.DateTimePicker dateTimePickerCompra;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btneliminar;
        private System.Windows.Forms.Button btnagregar;
    }
}