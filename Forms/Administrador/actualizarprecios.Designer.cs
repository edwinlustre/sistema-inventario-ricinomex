namespace appInventario.Form_Administrador
{
    partial class actualizarprecios
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(actualizarprecios));
            this.label9 = new System.Windows.Forms.Label();
            this.txtsemilla = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtgrano = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtcostal = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnactualizar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label9
            // 
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Malgun Gothic", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(62)))), ((int)(((byte)(89)))));
            this.label9.Location = new System.Drawing.Point(45, 32);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(209, 32);
            this.label9.TabIndex = 16;
            this.label9.Text = "Actualiza precios";
            // 
            // txtsemilla
            // 
            this.txtsemilla.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtsemilla.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtsemilla.Location = new System.Drawing.Point(315, 241);
            this.txtsemilla.Name = "txtsemilla";
            this.txtsemilla.Size = new System.Drawing.Size(175, 26);
            this.txtsemilla.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label5.Location = new System.Drawing.Point(311, 211);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(131, 20);
            this.label5.TabIndex = 12;
            this.label5.Text = "Precio de semilla:";
            // 
            // txtgrano
            // 
            this.txtgrano.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtgrano.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtgrano.Location = new System.Drawing.Point(315, 171);
            this.txtgrano.Name = "txtgrano";
            this.txtgrano.Size = new System.Drawing.Size(174, 26);
            this.txtgrano.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label4.Location = new System.Drawing.Point(311, 141);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(124, 20);
            this.label4.TabIndex = 11;
            this.label4.Text = "Precio de grano:";
            // 
            // txtcostal
            // 
            this.txtcostal.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtcostal.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtcostal.Location = new System.Drawing.Point(316, 311);
            this.txtcostal.Name = "txtcostal";
            this.txtcostal.Size = new System.Drawing.Size(174, 26);
            this.txtcostal.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label2.Location = new System.Drawing.Point(311, 281);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(125, 20);
            this.label2.TabIndex = 15;
            this.label2.Text = "Precio de costal:";
            // 
            // btnactualizar
            // 
            this.btnactualizar.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnactualizar.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnactualizar.Location = new System.Drawing.Point(339, 399);
            this.btnactualizar.Name = "btnactualizar";
            this.btnactualizar.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnactualizar.Size = new System.Drawing.Size(116, 29);
            this.btnactualizar.TabIndex = 3;
            this.btnactualizar.Text = "Actualizar";
            this.btnactualizar.UseVisualStyleBackColor = true;
            this.btnactualizar.Click += new System.EventHandler(this.btnactualizar_Click);
            // 
            // actualizarprecios
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(784, 491);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.btnactualizar);
            this.Controls.Add(this.txtcostal);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtsemilla);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtgrano);
            this.Controls.Add(this.label4);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "actualizarprecios";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Actualizar precios";
            this.VisibleChanged += new System.EventHandler(this.actualizarprecios_VisibleChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtsemilla;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtgrano;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtcostal;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnactualizar;
    }
}