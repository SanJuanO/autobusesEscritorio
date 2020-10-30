namespace Autobuses
{
    partial class RegistrationUserControl
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
            this.SamplesNeeded = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.FingerPrintPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // Prompt
            // 
            this.Prompt.Location = new System.Drawing.Point(43, 224);
            this.Prompt.Margin = new System.Windows.Forms.Padding(0);
            this.Prompt.Name = "Prompt";
            this.Prompt.Size = new System.Drawing.Size(181, 46);
            this.Prompt.TabIndex = 21;
            this.Prompt.Text = "Estado";
            this.Prompt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FingerPrintPicture
            // 
            this.FingerPrintPicture.BackColor = System.Drawing.SystemColors.Window;
            this.FingerPrintPicture.Location = new System.Drawing.Point(43, 15);
            this.FingerPrintPicture.Margin = new System.Windows.Forms.Padding(0);
            this.FingerPrintPicture.Name = "FingerPrintPicture";
            this.FingerPrintPicture.Size = new System.Drawing.Size(181, 209);
            this.FingerPrintPicture.TabIndex = 18;
            this.FingerPrintPicture.TabStop = false;
            // 
            // SamplesNeeded
            // 
            this.SamplesNeeded.AutoSize = true;
            this.SamplesNeeded.Location = new System.Drawing.Point(51, 286);
            this.SamplesNeeded.Margin = new System.Windows.Forms.Padding(0);
            this.SamplesNeeded.Name = "SamplesNeeded";
            this.SamplesNeeded.Size = new System.Drawing.Size(52, 17);
            this.SamplesNeeded.TabIndex = 22;
            this.SamplesNeeded.Text = "Estado";
            this.SamplesNeeded.Click += new System.EventHandler(this.SamplesNeeded_Click);
            // 
            // RegistrationUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(69)))), ((int)(((byte)(76)))));
            this.Controls.Add(this.SamplesNeeded);
            this.Controls.Add(this.Prompt);
            this.Controls.Add(this.FingerPrintPicture);
            this.ForeColor = System.Drawing.Color.White;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "RegistrationUserControl";
            this.Size = new System.Drawing.Size(272, 329);
            ((System.ComponentModel.ISupportInitialize)(this.FingerPrintPicture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Prompt;
        private System.Windows.Forms.PictureBox FingerPrintPicture;
        private System.Windows.Forms.Label SamplesNeeded;
    }
}
