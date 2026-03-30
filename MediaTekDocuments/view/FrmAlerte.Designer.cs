namespace MediaTekDocuments.view
{
    partial class FrmAlerte
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
            this.label1 = new System.Windows.Forms.Label();
            this.dgvAlerte = new System.Windows.Forms.DataGridView();
            this.btnAlerteOK = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAlerte)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(14, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(352, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Les abonnements suivants expirent dans moins de 30 jours : ";
            // 
            // dgvAlerte
            // 
            this.dgvAlerte.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAlerte.Location = new System.Drawing.Point(12, 34);
            this.dgvAlerte.Name = "dgvAlerte";
            this.dgvAlerte.Size = new System.Drawing.Size(360, 187);
            this.dgvAlerte.TabIndex = 1;
            // 
            // btnAlerteOK
            // 
            this.btnAlerteOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAlerteOK.Location = new System.Drawing.Point(142, 230);
            this.btnAlerteOK.Name = "btnAlerteOK";
            this.btnAlerteOK.Size = new System.Drawing.Size(96, 22);
            this.btnAlerteOK.TabIndex = 21;
            this.btnAlerteOK.Text = "OK";
            this.btnAlerteOK.UseVisualStyleBackColor = true;
            this.btnAlerteOK.Click += new System.EventHandler(this.btnAlerteOK_Click);
            // 
            // FrmAlerte
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 261);
            this.Controls.Add(this.btnAlerteOK);
            this.Controls.Add(this.dgvAlerte);
            this.Controls.Add(this.label1);
            this.Name = "FrmAlerte";
            this.Text = "FrmAlerte";
            ((System.ComponentModel.ISupportInitialize)(this.dgvAlerte)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvAlerte;
        private System.Windows.Forms.Button btnAlerteOK;
    }
}