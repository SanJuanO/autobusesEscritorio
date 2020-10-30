namespace MyAttendance
{
    partial class VerificationUserControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Prompt = new System.Windows.Forms.Label();
            this.FingerPrintPicture = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.FingerPrintPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // Prompt
            // 
            this.Prompt.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Prompt.AutoEllipsis = true;
            this.Prompt.AutoSize = true;
            this.Prompt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Prompt.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Prompt.ForeColor = System.Drawing.Color.White;
            this.Prompt.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.Prompt.Location = new System.Drawing.Point(-2, 118);
            this.Prompt.Margin = new System.Windows.Forms.Padding(0);
            this.Prompt.Name = "Prompt";
            this.Prompt.Size = new System.Drawing.Size(57, 20);
            this.Prompt.TabIndex = 21;
            this.Prompt.Text = "Status";
            this.Prompt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FingerPrintPicture
            // 
            this.FingerPrintPicture.BackColor = System.Drawing.Color.White;
            this.FingerPrintPicture.Location = new System.Drawing.Point(0, 0);
            this.FingerPrintPicture.Margin = new System.Windows.Forms.Padding(0);
            this.FingerPrintPicture.Name = "FingerPrintPicture";
            this.FingerPrintPicture.Size = new System.Drawing.Size(136, 118);
            this.FingerPrintPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.FingerPrintPicture.TabIndex = 20;
            this.FingerPrintPicture.TabStop = false;
            this.FingerPrintPicture.Click += new System.EventHandler(this.FingerPrintPicture_Click);
            // 
            // VerificationUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(45)))), ((int)(((byte)(53)))));
            this.Controls.Add(this.Prompt);
            this.Controls.Add(this.FingerPrintPicture);
            this.Margin = new System.Windows.Forms.Padding(1);
            this.Name = "VerificationUserControl";
            this.Size = new System.Drawing.Size(136, 187);
            ((System.ComponentModel.ISupportInitialize)(this.FingerPrintPicture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label Prompt;
        public System.Windows.Forms.PictureBox FingerPrintPicture;
    }
}
