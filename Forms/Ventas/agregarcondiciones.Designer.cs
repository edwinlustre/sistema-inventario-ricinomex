namespace appInventario.Form_Ventas
{
    partial class agregarcondiciones
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(agregarcondiciones));
            this.label10 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBoxCondiciones = new System.Windows.Forms.ComboBox();
            this.txtcondicionesextra = new System.Windows.Forms.TextBox();
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
            this.label10.Location = new System.Drawing.Point(38, 30);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(250, 32);
            this.label10.TabIndex = 0;
            this.label10.Text = "Agregar condiciones";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.comboBoxCondiciones);
            this.panel2.Controls.Add(this.txtcondicionesextra);
            this.panel2.Controls.Add(this.btnañadir);
            this.panel2.Location = new System.Drawing.Point(0, 65);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(785, 397);
            this.panel2.TabIndex = 8;
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.label6.Location = new System.Drawing.Point(59, 21);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(121, 24);
            this.label6.TabIndex = 11;
            this.label6.Text = "Condiciones:";
            // 
            // comboBoxCondiciones
            // 
            this.comboBoxCondiciones.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.comboBoxCondiciones.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCondiciones.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxCondiciones.FormattingEnabled = true;
            this.comboBoxCondiciones.Items.AddRange(new object[] {
            "Seleccionar",
            "- Condiciones de entrega: En domicilio del cliente.",
            "- Facturación: Antes de la recolección.",
            "- Plazo de pago: Antes de la recolección.",
            "- Cualquier modificación al pedido original puede generar ajustes en el precio y " +
                "en los plazos de entrega."});
            this.comboBoxCondiciones.Location = new System.Drawing.Point(186, 20);
            this.comboBoxCondiciones.Name = "comboBoxCondiciones";
            this.comboBoxCondiciones.Size = new System.Drawing.Size(321, 28);
            this.comboBoxCondiciones.TabIndex = 0;
            this.comboBoxCondiciones.SelectedIndexChanged += new System.EventHandler(this.comboBoxCondiciones_SelectedIndexChanged);
            // 
            // txtcondicionesextra
            // 
            this.txtcondicionesextra.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtcondicionesextra.Location = new System.Drawing.Point(44, 61);
            this.txtcondicionesextra.Multiline = true;
            this.txtcondicionesextra.Name = "txtcondicionesextra";
            this.txtcondicionesextra.Size = new System.Drawing.Size(698, 261);
            this.txtcondicionesextra.TabIndex = 1;
            // 
            // btnañadir
            // 
            this.btnañadir.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnañadir.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnañadir.Location = new System.Drawing.Point(335, 341);
            this.btnañadir.Name = "btnañadir";
            this.btnañadir.Size = new System.Drawing.Size(116, 29);
            this.btnañadir.TabIndex = 2;
            this.btnañadir.Text = "Añadir";
            this.btnañadir.UseVisualStyleBackColor = true;
            this.btnañadir.Click += new System.EventHandler(this.btnañadir_Click);
            // 
            // agregarcondiciones
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(784, 461);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.panel2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "agregarcondiciones";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Agregar condiciones";
            this.Load += new System.EventHandler(this.agregarcondiciones_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnañadir;
        private System.Windows.Forms.TextBox txtcondicionesextra;
        private System.Windows.Forms.ComboBox comboBoxCondiciones;
        private System.Windows.Forms.Label label6;
    }
}