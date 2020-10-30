namespace Autobuses.Reportes
{
    partial class Reporte_de_tarjetas
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Reporte_de_tarjetas));
            this.btnAdd = new System.Windows.Forms.Button();
            this.dataGridViewguias = new System.Windows.Forms.DataGridView();
            this.salidaname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.origenname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.econame = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tarjetaturno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.costoturnoname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tarjetapasoname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.costopasoname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tarjetasalidaname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.costoname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.comboBoxsucursal = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.dateTimePickerinicio = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxlinea = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dateTimePickerfinal = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.textgeneral = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.textsalida = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textpaso = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.texturno = new System.Windows.Forms.TextBox();
            this.molnto = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelcargo = new System.Windows.Forms.Label();
            this.labelapellido = new System.Windows.Forms.Label();
            this.labelnombre = new System.Windows.Forms.Label();
            this.lbFecha = new System.Windows.Forms.Label();
            this.pictureBoxfoto = new System.Windows.Forms.PictureBox();
            this.lblHora = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.PanelBarraTitulo = new System.Windows.Forms.Panel();
            this.titulo = new System.Windows.Forms.Label();
            this.btnNormal = new System.Windows.Forms.Button();
            this.pictureBox8 = new System.Windows.Forms.PictureBox();
            this.label33 = new System.Windows.Forms.Label();
            this.btnMinimizar = new System.Windows.Forms.Button();
            this.btnMaximizar = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewguias)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxfoto)).BeginInit();
            this.PanelBarraTitulo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(45)))), ((int)(((byte)(53)))));
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Location = new System.Drawing.Point(386, 246);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(160, 37);
            this.btnAdd.TabIndex = 940;
            this.btnAdd.Text = "Buscar";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // dataGridViewguias
            // 
            this.dataGridViewguias.AllowUserToAddRows = false;
            this.dataGridViewguias.AllowUserToDeleteRows = false;
            this.dataGridViewguias.AllowUserToResizeColumns = false;
            this.dataGridViewguias.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewguias.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewguias.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridViewguias.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(69)))), ((int)(((byte)(76)))));
            this.dataGridViewguias.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dataGridViewguias.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewguias.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewguias.ColumnHeadersHeight = 40;
            this.dataGridViewguias.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.salidaname,
            this.origenname,
            this.econame,
            this.tarjetaturno,
            this.costoturnoname,
            this.tarjetapasoname,
            this.costopasoname,
            this.tarjetasalidaname,
            this.costoname});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(61)))), ((int)(((byte)(69)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewguias.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewguias.EnableHeadersVisualStyles = false;
            this.dataGridViewguias.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(69)))), ((int)(((byte)(76)))));
            this.dataGridViewguias.Location = new System.Drawing.Point(0, 298);
            this.dataGridViewguias.MultiSelect = false;
            this.dataGridViewguias.Name = "dataGridViewguias";
            this.dataGridViewguias.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(61)))), ((int)(((byte)(69)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewguias.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewguias.RowHeadersVisible = false;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(61)))), ((int)(((byte)(69)))));
            this.dataGridViewguias.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridViewguias.RowTemplate.Height = 24;
            this.dataGridViewguias.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewguias.Size = new System.Drawing.Size(1262, 354);
            this.dataGridViewguias.TabIndex = 0;
            this.dataGridViewguias.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridView1_CellContentClick);
            // 
            // salidaname
            // 
            this.salidaname.HeaderText = "Salida";
            this.salidaname.Name = "salidaname";
            this.salidaname.ReadOnly = true;
            // 
            // origenname
            // 
            this.origenname.HeaderText = "Origen";
            this.origenname.Name = "origenname";
            this.origenname.ReadOnly = true;
            // 
            // econame
            // 
            this.econame.HeaderText = "Eco";
            this.econame.Name = "econame";
            this.econame.ReadOnly = true;
            // 
            // tarjetaturno
            // 
            this.tarjetaturno.HeaderText = "T. Turno";
            this.tarjetaturno.Name = "tarjetaturno";
            this.tarjetaturno.ReadOnly = true;
            // 
            // costoturnoname
            // 
            this.costoturnoname.HeaderText = "Costo T. T.";
            this.costoturnoname.Name = "costoturnoname";
            this.costoturnoname.ReadOnly = true;
            // 
            // tarjetapasoname
            // 
            this.tarjetapasoname.HeaderText = "T. Paso";
            this.tarjetapasoname.Name = "tarjetapasoname";
            this.tarjetapasoname.ReadOnly = true;
            // 
            // costopasoname
            // 
            this.costopasoname.HeaderText = "Costo T. P.";
            this.costopasoname.Name = "costopasoname";
            this.costopasoname.ReadOnly = true;
            // 
            // tarjetasalidaname
            // 
            this.tarjetasalidaname.HeaderText = "T. Salida";
            this.tarjetasalidaname.Name = "tarjetasalidaname";
            this.tarjetasalidaname.ReadOnly = true;
            // 
            // costoname
            // 
            this.costoname.HeaderText = "Costo T. S.";
            this.costoname.Name = "costoname";
            this.costoname.ReadOnly = true;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // comboBoxsucursal
            // 
            this.comboBoxsucursal.Location = new System.Drawing.Point(96, 95);
            this.comboBoxsucursal.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxsucursal.Name = "comboBoxsucursal";
            this.comboBoxsucursal.Size = new System.Drawing.Size(236, 30);
            this.comboBoxsucursal.TabIndex = 942;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(24, 100);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 20);
            this.label5.TabIndex = 941;
            this.label5.Text = "Sucursal";
            // 
            // dateTimePickerinicio
            // 
            this.dateTimePickerinicio.Location = new System.Drawing.Point(96, 35);
            this.dateTimePickerinicio.MinDate = new System.DateTime(2019, 7, 29, 0, 0, 0, 0);
            this.dateTimePickerinicio.Name = "dateTimePickerinicio";
            this.dateTimePickerinicio.Size = new System.Drawing.Size(236, 28);
            this.dateTimePickerinicio.TabIndex = 946;
            this.dateTimePickerinicio.ValueChanged += new System.EventHandler(this.DateTimePickerinicio_ValueChanged_1);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(7, 41);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 20);
            this.label2.TabIndex = 945;
            this.label2.Text = "Fecha Inicio";
            // 
            // comboBoxlinea
            // 
            this.comboBoxlinea.Location = new System.Drawing.Point(421, 97);
            this.comboBoxlinea.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxlinea.Name = "comboBoxlinea";
            this.comboBoxlinea.Size = new System.Drawing.Size(248, 30);
            this.comboBoxlinea.TabIndex = 948;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(380, 106);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 20);
            this.label3.TabIndex = 947;
            this.label3.Text = "Linea";
            // 
            // dateTimePickerfinal
            // 
            this.dateTimePickerfinal.Location = new System.Drawing.Point(428, 36);
            this.dateTimePickerfinal.MinDate = new System.DateTime(2019, 7, 29, 0, 0, 0, 0);
            this.dateTimePickerfinal.Name = "dateTimePickerfinal";
            this.dateTimePickerfinal.Size = new System.Drawing.Size(241, 28);
            this.dateTimePickerfinal.TabIndex = 950;
            this.dateTimePickerfinal.ValueChanged += new System.EventHandler(this.DateTimePicker1_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(339, 39);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(116, 20);
            this.label4.TabIndex = 949;
            this.label4.Text = "Fecha termino";
            // 
            // textgeneral
            // 
            this.textgeneral.Enabled = false;
            this.textgeneral.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textgeneral.Location = new System.Drawing.Point(29, 172);
            this.textgeneral.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textgeneral.Name = "textgeneral";
            this.textgeneral.ReadOnly = true;
            this.textgeneral.Size = new System.Drawing.Size(272, 28);
            this.textgeneral.TabIndex = 965;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(26, 155);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(120, 22);
            this.label11.TabIndex = 964;
            this.label11.Text = "Total General";
            // 
            // textsalida
            // 
            this.textsalida.Enabled = false;
            this.textsalida.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textsalida.Location = new System.Drawing.Point(29, 127);
            this.textsalida.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textsalida.Name = "textsalida";
            this.textsalida.ReadOnly = true;
            this.textsalida.Size = new System.Drawing.Size(272, 28);
            this.textsalida.TabIndex = 959;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(26, 110);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(128, 22);
            this.label9.TabIndex = 958;
            this.label9.Text = "Total T. Salida";
            // 
            // textpaso
            // 
            this.textpaso.Enabled = false;
            this.textpaso.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textpaso.Location = new System.Drawing.Point(29, 80);
            this.textpaso.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textpaso.Name = "textpaso";
            this.textpaso.ReadOnly = true;
            this.textpaso.Size = new System.Drawing.Size(272, 28);
            this.textpaso.TabIndex = 957;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(29, 63);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(119, 22);
            this.label7.TabIndex = 956;
            this.label7.Text = "Total T. Paso";
            // 
            // texturno
            // 
            this.texturno.Enabled = false;
            this.texturno.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.texturno.Location = new System.Drawing.Point(29, 34);
            this.texturno.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.texturno.Name = "texturno";
            this.texturno.ReadOnly = true;
            this.texturno.Size = new System.Drawing.Size(272, 28);
            this.texturno.TabIndex = 955;
            // 
            // molnto
            // 
            this.molnto.AutoSize = true;
            this.molnto.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.molnto.Location = new System.Drawing.Point(29, 17);
            this.molnto.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.molnto.Name = "molnto";
            this.molnto.Size = new System.Drawing.Size(126, 22);
            this.molnto.TabIndex = 954;
            this.molnto.Text = "Total T. Turno";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(61)))), ((int)(((byte)(69)))));
            this.panel1.Controls.Add(this.labelcargo);
            this.panel1.Controls.Add(this.labelapellido);
            this.panel1.Controls.Add(this.labelnombre);
            this.panel1.Controls.Add(this.lbFecha);
            this.panel1.Controls.Add(this.pictureBoxfoto);
            this.panel1.Controls.Add(this.lblHora);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 668);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1262, 123);
            this.panel1.TabIndex = 966;
            // 
            // labelcargo
            // 
            this.labelcargo.AutoSize = true;
            this.labelcargo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelcargo.ForeColor = System.Drawing.Color.LightGray;
            this.labelcargo.Location = new System.Drawing.Point(110, 66);
            this.labelcargo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelcargo.Name = "labelcargo";
            this.labelcargo.Size = new System.Drawing.Size(66, 25);
            this.labelcargo.TabIndex = 7;
            this.labelcargo.Text = "Cargo";
            // 
            // labelapellido
            // 
            this.labelapellido.AutoSize = true;
            this.labelapellido.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelapellido.ForeColor = System.Drawing.Color.LightGray;
            this.labelapellido.Location = new System.Drawing.Point(110, 44);
            this.labelapellido.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelapellido.Name = "labelapellido";
            this.labelapellido.Size = new System.Drawing.Size(138, 25);
            this.labelapellido.TabIndex = 6;
            this.labelapellido.Text = "Apellidos User";
            // 
            // labelnombre
            // 
            this.labelnombre.AutoSize = true;
            this.labelnombre.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelnombre.ForeColor = System.Drawing.Color.LightGray;
            this.labelnombre.Location = new System.Drawing.Point(110, 22);
            this.labelnombre.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelnombre.Name = "labelnombre";
            this.labelnombre.Size = new System.Drawing.Size(96, 25);
            this.labelnombre.TabIndex = 5;
            this.labelnombre.Text = "Nombres ";
            // 
            // lbFecha
            // 
            this.lbFecha.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbFecha.AutoSize = true;
            this.lbFecha.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbFecha.ForeColor = System.Drawing.Color.White;
            this.lbFecha.Location = new System.Drawing.Point(912, 87);
            this.lbFecha.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbFecha.Name = "lbFecha";
            this.lbFecha.Size = new System.Drawing.Size(336, 29);
            this.lbFecha.TabIndex = 4;
            this.lbFecha.Text = "Lunes, 26 de septiembre 2018";
            // 
            // pictureBoxfoto
            // 
            this.pictureBoxfoto.Location = new System.Drawing.Point(15, 4);
            this.pictureBoxfoto.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBoxfoto.Name = "pictureBoxfoto";
            this.pictureBoxfoto.Size = new System.Drawing.Size(87, 106);
            this.pictureBoxfoto.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxfoto.TabIndex = 3;
            this.pictureBoxfoto.TabStop = false;
            // 
            // lblHora
            // 
            this.lblHora.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblHora.AutoSize = true;
            this.lblHora.Font = new System.Drawing.Font("Microsoft Sans Serif", 35.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHora.ForeColor = System.Drawing.Color.LightGray;
            this.lblHora.Location = new System.Drawing.Point(918, 14);
            this.lblHora.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblHora.Name = "lblHora";
            this.lblHora.Size = new System.Drawing.Size(309, 81);
            this.lblHora.TabIndex = 1;
            this.lblHora.Text = "21:49:45";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(45)))), ((int)(((byte)(53)))));
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(572, 246);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(160, 37);
            this.button1.TabIndex = 968;
            this.button1.Text = "Excel";
            this.button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.ToolStripExportExcel_Click);
            // 
            // PanelBarraTitulo
            // 
            this.PanelBarraTitulo.BackColor = System.Drawing.SystemColors.HotTrack;
            this.PanelBarraTitulo.Controls.Add(this.titulo);
            this.PanelBarraTitulo.Controls.Add(this.btnNormal);
            this.PanelBarraTitulo.Controls.Add(this.pictureBox8);
            this.PanelBarraTitulo.Controls.Add(this.label33);
            this.PanelBarraTitulo.Controls.Add(this.btnMinimizar);
            this.PanelBarraTitulo.Controls.Add(this.btnMaximizar);
            this.PanelBarraTitulo.Controls.Add(this.button2);
            this.PanelBarraTitulo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PanelBarraTitulo.Dock = System.Windows.Forms.DockStyle.Top;
            this.PanelBarraTitulo.Location = new System.Drawing.Point(0, 0);
            this.PanelBarraTitulo.Margin = new System.Windows.Forms.Padding(4);
            this.PanelBarraTitulo.Name = "PanelBarraTitulo";
            this.PanelBarraTitulo.Size = new System.Drawing.Size(1262, 53);
            this.PanelBarraTitulo.TabIndex = 969;
            this.PanelBarraTitulo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PanelBarraTitulo_MouseDown);
            // 
            // titulo
            // 
            this.titulo.AutoSize = true;
            this.titulo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titulo.ForeColor = System.Drawing.Color.White;
            this.titulo.Location = new System.Drawing.Point(570, 16);
            this.titulo.Name = "titulo";
            this.titulo.Size = new System.Drawing.Size(182, 29);
            this.titulo.TabIndex = 10;
            this.titulo.Text = "ATAH SYSTEM";
            // 
            // btnNormal
            // 
            this.btnNormal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNormal.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNormal.FlatAppearance.BorderSize = 0;
            this.btnNormal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNormal.Image = global::Autobuses.Properties.Resources.Normal;
            this.btnNormal.Location = new System.Drawing.Point(1130, 0);
            this.btnNormal.Margin = new System.Windows.Forms.Padding(4);
            this.btnNormal.Name = "btnNormal";
            this.btnNormal.Size = new System.Drawing.Size(57, 53);
            this.btnNormal.TabIndex = 6;
            this.btnNormal.UseVisualStyleBackColor = true;
            this.btnNormal.Visible = false;
            this.btnNormal.Click += new System.EventHandler(this.BtnNormal_Click);
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
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label33.ForeColor = System.Drawing.Color.White;
            this.label33.Location = new System.Drawing.Point(46, 18);
            this.label33.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(156, 25);
            this.label33.TabIndex = 4;
            this.label33.Text = "ATAH SYSTEM";
            // 
            // btnMinimizar
            // 
            this.btnMinimizar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMinimizar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMinimizar.FlatAppearance.BorderSize = 0;
            this.btnMinimizar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinimizar.Image = global::Autobuses.Properties.Resources.Minimize;
            this.btnMinimizar.Location = new System.Drawing.Point(1062, 0);
            this.btnMinimizar.Margin = new System.Windows.Forms.Padding(4);
            this.btnMinimizar.Name = "btnMinimizar";
            this.btnMinimizar.Size = new System.Drawing.Size(57, 53);
            this.btnMinimizar.TabIndex = 2;
            this.btnMinimizar.UseVisualStyleBackColor = true;
            this.btnMinimizar.Click += new System.EventHandler(this.BtnMinimizar_Click);
            // 
            // btnMaximizar
            // 
            this.btnMaximizar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMaximizar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMaximizar.FlatAppearance.BorderSize = 0;
            this.btnMaximizar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMaximizar.Image = global::Autobuses.Properties.Resources.maximize3;
            this.btnMaximizar.Location = new System.Drawing.Point(1123, 0);
            this.btnMaximizar.Margin = new System.Windows.Forms.Padding(4);
            this.btnMaximizar.Name = "btnMaximizar";
            this.btnMaximizar.Size = new System.Drawing.Size(57, 53);
            this.btnMaximizar.TabIndex = 1;
            this.btnMaximizar.UseVisualStyleBackColor = true;
            this.btnMaximizar.Click += new System.EventHandler(this.BtnMaximizar_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Image = global::Autobuses.Properties.Resources.Close;
            this.button2.Location = new System.Drawing.Point(1185, 2);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(52, 48);
            this.button2.TabIndex = 0;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // timer2
            // 
            this.timer2.Tick += new System.EventHandler(this.Timer2_Tick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.comboBoxsucursal);
            this.groupBox1.Controls.Add(this.dateTimePickerinicio);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.comboBoxlinea);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.dateTimePickerfinal);
            this.groupBox1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox1.Location = new System.Drawing.Point(32, 71);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(701, 140);
            this.groupBox1.TabIndex = 970;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filtrar";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.molnto);
            this.groupBox2.Controls.Add(this.texturno);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.textpaso);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.textgeneral);
            this.groupBox2.Controls.Add(this.textsalida);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox2.Location = new System.Drawing.Point(757, 75);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(379, 208);
            this.groupBox2.TabIndex = 951;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Detalle";
            this.groupBox2.Enter += new System.EventHandler(this.GroupBox2_Enter);
            // 
            // Reporte_de_tarjetas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(69)))), ((int)(((byte)(76)))));
            this.ClientSize = new System.Drawing.Size(1262, 791);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.PanelBarraTitulo);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.dataGridViewguias);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Reporte_de_tarjetas";
            this.Text = "Reporte_de_tarjetas";
            this.Load += new System.EventHandler(this.Reporte_de_tarjetas_Load);
            this.Shown += new System.EventHandler(this.Reporte_de_tarjetas_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewguias)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxfoto)).EndInit();
            this.PanelBarraTitulo.ResumeLayout(false);
            this.PanelBarraTitulo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.DataGridView dataGridViewguias;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ComboBox comboBoxsucursal;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dateTimePickerinicio;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxlinea;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dateTimePickerfinal;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridViewTextBoxColumn salidaname;
        private System.Windows.Forms.DataGridViewTextBoxColumn origenname;
        private System.Windows.Forms.DataGridViewTextBoxColumn econame;
        private System.Windows.Forms.DataGridViewTextBoxColumn tarjetaturno;
        private System.Windows.Forms.DataGridViewTextBoxColumn costoturnoname;
        private System.Windows.Forms.DataGridViewTextBoxColumn tarjetapasoname;
        private System.Windows.Forms.DataGridViewTextBoxColumn costopasoname;
        private System.Windows.Forms.DataGridViewTextBoxColumn tarjetasalidaname;
        private System.Windows.Forms.DataGridViewTextBoxColumn costoname;
        private System.Windows.Forms.TextBox textgeneral;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textpaso;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox texturno;
        private System.Windows.Forms.Label molnto;
        private System.Windows.Forms.TextBox textsalida;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelcargo;
        private System.Windows.Forms.Label labelapellido;
        private System.Windows.Forms.Label labelnombre;
        private System.Windows.Forms.Label lbFecha;
        private System.Windows.Forms.PictureBox pictureBoxfoto;
        private System.Windows.Forms.Label lblHora;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel PanelBarraTitulo;
        private System.Windows.Forms.Button btnNormal;
        private System.Windows.Forms.PictureBox pictureBox8;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Button btnMinimizar;
        private System.Windows.Forms.Button btnMaximizar;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label titulo;
    }
}