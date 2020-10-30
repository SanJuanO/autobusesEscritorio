namespace Autobuses
{
    partial class Login
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
            this.LoginUsuario = new System.Windows.Forms.TextBox();
            this.LoginPassword = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnIngresar = new System.Windows.Forms.Button();
            this.groupBoxingresar = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.labelsucursal = new System.Windows.Forms.Label();
            this.labelnombre = new System.Windows.Forms.Label();
            this.verificationUserControl = new MyAttendance.VerificationUserControl();
            this.button1 = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label4 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.backgroundWorkerLogin = new System.ComponentModel.BackgroundWorker();
            this.PanelBarraTitulo = new System.Windows.Forms.Panel();
            this.button3 = new System.Windows.Forms.Button();
            this.pictureBox8 = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBoxusuario = new System.Windows.Forms.GroupBox();
            this.error = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonusuario = new System.Windows.Forms.Button();
            this.groupBoxingresar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.PanelBarraTitulo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).BeginInit();
            this.groupBoxusuario.SuspendLayout();
            this.SuspendLayout();
            // 
            // LoginUsuario
            // 
            this.LoginUsuario.Location = new System.Drawing.Point(35, 64);
            this.LoginUsuario.Margin = new System.Windows.Forms.Padding(4);
            this.LoginUsuario.Name = "LoginUsuario";
            this.LoginUsuario.Size = new System.Drawing.Size(195, 22);
            this.LoginUsuario.TabIndex = 1;
            this.LoginUsuario.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LoginUsuario_KeyDown_1);
            this.LoginUsuario.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.LoginUsuario_PreviewKeyDown);
            // 
            // LoginPassword
            // 
            this.LoginPassword.Location = new System.Drawing.Point(297, 231);
            this.LoginPassword.Margin = new System.Windows.Forms.Padding(4);
            this.LoginPassword.Name = "LoginPassword";
            this.LoginPassword.PasswordChar = '*';
            this.LoginPassword.Size = new System.Drawing.Size(195, 22);
            this.LoginPassword.TabIndex = 2;
            this.LoginPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LoginPassword_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Snow;
            this.label1.Location = new System.Drawing.Point(35, 43);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Usuario:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(295, 211);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Password:";
            // 
            // btnIngresar
            // 
            this.btnIngresar.BackColor = System.Drawing.SystemColors.HotTrack;
            this.btnIngresar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIngresar.Location = new System.Drawing.Point(97, 300);
            this.btnIngresar.Margin = new System.Windows.Forms.Padding(4);
            this.btnIngresar.Name = "btnIngresar";
            this.btnIngresar.Size = new System.Drawing.Size(133, 43);
            this.btnIngresar.TabIndex = 3;
            this.btnIngresar.Text = "Ingresar";
            this.btnIngresar.UseVisualStyleBackColor = false;
            this.btnIngresar.Click += new System.EventHandler(this.btnIngresar_Click);
            // 
            // groupBoxingresar
            // 
            this.groupBoxingresar.CausesValidation = false;
            this.groupBoxingresar.Controls.Add(this.pictureBox1);
            this.groupBoxingresar.Controls.Add(this.labelsucursal);
            this.groupBoxingresar.Controls.Add(this.labelnombre);
            this.groupBoxingresar.Controls.Add(this.verificationUserControl);
            this.groupBoxingresar.Controls.Add(this.button1);
            this.groupBoxingresar.Controls.Add(this.progressBar1);
            this.groupBoxingresar.Controls.Add(this.LoginPassword);
            this.groupBoxingresar.Controls.Add(this.label2);
            this.groupBoxingresar.Controls.Add(this.btnIngresar);
            this.groupBoxingresar.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxingresar.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBoxingresar.Location = new System.Drawing.Point(33, 75);
            this.groupBoxingresar.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxingresar.Name = "groupBoxingresar";
            this.groupBoxingresar.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxingresar.Size = new System.Drawing.Size(525, 368);
            this.groupBoxingresar.TabIndex = 0;
            this.groupBoxingresar.TabStop = false;
            this.groupBoxingresar.Text = "Login";
            this.groupBoxingresar.Visible = false;
            this.groupBoxingresar.BackColorChanged += new System.EventHandler(this.GroupBox1_BackColorChanged);
            this.groupBoxingresar.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(63, 33);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(150, 175);
            this.pictureBox1.TabIndex = 11;
            this.pictureBox1.TabStop = false;
            // 
            // labelsucursal
            // 
            this.labelsucursal.AutoSize = true;
            this.labelsucursal.Location = new System.Drawing.Point(37, 260);
            this.labelsucursal.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelsucursal.Name = "labelsucursal";
            this.labelsucursal.Size = new System.Drawing.Size(71, 17);
            this.labelsucursal.TabIndex = 10;
            this.labelsucursal.Text = "Sucursal";
            // 
            // labelnombre
            // 
            this.labelnombre.AutoSize = true;
            this.labelnombre.Location = new System.Drawing.Point(37, 231);
            this.labelnombre.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelnombre.Name = "labelnombre";
            this.labelnombre.Size = new System.Drawing.Size(64, 17);
            this.labelnombre.TabIndex = 9;
            this.labelnombre.Text = "Nombre";
            // 
            // verificationUserControl
            // 
            this.verificationUserControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(69)))), ((int)(((byte)(76)))));
            this.verificationUserControl.IsVerificationComplete = false;
            this.verificationUserControl.Location = new System.Drawing.Point(348, 33);
            this.verificationUserControl.Margin = new System.Windows.Forms.Padding(1);
            this.verificationUserControl.Name = "verificationUserControl";
            this.verificationUserControl.Size = new System.Drawing.Size(126, 154);
            this.verificationUserControl.TabIndex = 8;
            this.verificationUserControl.Load += new System.EventHandler(this.VerificationUserControl_Load);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.GrayText;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(291, 300);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(133, 43);
            this.button1.TabIndex = 7;
            this.button1.Text = "Cancelar";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.progressBar1.Location = new System.Drawing.Point(2, 349);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(523, 19);
            this.progressBar1.Step = 100;
            this.progressBar1.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(225, 447);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(149, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Powered By GESDES @2019";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // timer2
            // 
            this.timer2.Enabled = true;
            this.timer2.Interval = 1000;
            this.timer2.Tick += new System.EventHandler(this.Timer2_Tick);
            // 
            // backgroundWorkerLogin
            // 
            this.backgroundWorkerLogin.WorkerReportsProgress = true;
            this.backgroundWorkerLogin.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorker1_DoWork);
            this.backgroundWorkerLogin.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.BackgroundWorkerLogin_ProgressChanged);
            // 
            // PanelBarraTitulo
            // 
            this.PanelBarraTitulo.BackColor = System.Drawing.SystemColors.HotTrack;
            this.PanelBarraTitulo.Controls.Add(this.button3);
            this.PanelBarraTitulo.Controls.Add(this.pictureBox8);
            this.PanelBarraTitulo.Controls.Add(this.label3);
            this.PanelBarraTitulo.Dock = System.Windows.Forms.DockStyle.Top;
            this.PanelBarraTitulo.Location = new System.Drawing.Point(0, 0);
            this.PanelBarraTitulo.Margin = new System.Windows.Forms.Padding(4);
            this.PanelBarraTitulo.Name = "PanelBarraTitulo";
            this.PanelBarraTitulo.Size = new System.Drawing.Size(591, 53);
            this.PanelBarraTitulo.TabIndex = 8;
            this.PanelBarraTitulo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PanelBarraTitulo_MouseDown);
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button3.FlatAppearance.BorderSize = 0;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Image = global::Autobuses.Properties.Resources.Close;
            this.button3.Location = new System.Drawing.Point(535, 5);
            this.button3.Margin = new System.Windows.Forms.Padding(4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(52, 48);
            this.button3.TabIndex = 6;
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // pictureBox8
            // 
            this.pictureBox8.Image = global::Autobuses.Properties.Resources.satellite_dish;
            this.pictureBox8.Location = new System.Drawing.Point(12, 11);
            this.pictureBox8.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox8.Name = "pictureBox8";
            this.pictureBox8.Size = new System.Drawing.Size(37, 34);
            this.pictureBox8.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox8.TabIndex = 5;
            this.pictureBox8.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(45, 18);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(126, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "ATAH SYSTEM";
            // 
            // groupBoxusuario
            // 
            this.groupBoxusuario.Controls.Add(this.error);
            this.groupBoxusuario.Controls.Add(this.label5);
            this.groupBoxusuario.Controls.Add(this.buttonusuario);
            this.groupBoxusuario.Controls.Add(this.label1);
            this.groupBoxusuario.Controls.Add(this.LoginUsuario);
            this.groupBoxusuario.Location = new System.Drawing.Point(33, 77);
            this.groupBoxusuario.Name = "groupBoxusuario";
            this.groupBoxusuario.Size = new System.Drawing.Size(286, 206);
            this.groupBoxusuario.TabIndex = 12;
            this.groupBoxusuario.TabStop = false;
            // 
            // error
            // 
            this.error.AutoSize = true;
            this.error.ForeColor = System.Drawing.Color.DarkRed;
            this.error.Location = new System.Drawing.Point(37, 90);
            this.error.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.error.Name = "error";
            this.error.Size = new System.Drawing.Size(0, 17);
            this.error.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(60, 190);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(149, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Powered By GESDES @2019";
            // 
            // buttonusuario
            // 
            this.buttonusuario.BackColor = System.Drawing.SystemColors.HotTrack;
            this.buttonusuario.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonusuario.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonusuario.Location = new System.Drawing.Point(69, 118);
            this.buttonusuario.Margin = new System.Windows.Forms.Padding(4);
            this.buttonusuario.Name = "buttonusuario";
            this.buttonusuario.Size = new System.Drawing.Size(133, 43);
            this.buttonusuario.TabIndex = 4;
            this.buttonusuario.Text = "Ingresar";
            this.buttonusuario.UseVisualStyleBackColor = false;
            this.buttonusuario.Click += new System.EventHandler(this.buttonusuario_Click);
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(69)))), ((int)(((byte)(76)))));
            this.ClientSize = new System.Drawing.Size(591, 482);
            this.ControlBox = false;
            this.Controls.Add(this.groupBoxusuario);
            this.Controls.Add(this.PanelBarraTitulo);
            this.Controls.Add(this.groupBoxingresar);
            this.Controls.Add(this.label4);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.Load += new System.EventHandler(this.Login_Load);
            this.Shown += new System.EventHandler(this.Login_Shown);
            this.groupBoxingresar.ResumeLayout(false);
            this.groupBoxingresar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.PanelBarraTitulo.ResumeLayout(false);
            this.PanelBarraTitulo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).EndInit();
            this.groupBoxusuario.ResumeLayout(false);
            this.groupBoxusuario.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox LoginUsuario;
        private System.Windows.Forms.TextBox LoginPassword;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnIngresar;
        private System.Windows.Forms.GroupBox groupBoxingresar;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;
        private System.ComponentModel.BackgroundWorker backgroundWorkerLogin;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel PanelBarraTitulo;
        private System.Windows.Forms.PictureBox pictureBox8;
        private System.Windows.Forms.Label label3;
        private MyAttendance.VerificationUserControl verificationUserControl;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label labelsucursal;
        private System.Windows.Forms.Label labelnombre;
        private System.Windows.Forms.GroupBox groupBoxusuario;
        private System.Windows.Forms.Button buttonusuario;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label error;
        private System.Windows.Forms.Button button3;
    }
}