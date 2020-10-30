namespace Autobuses.Administracion
{
    partial class Permisos
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Permisos));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labeldescripcion = new System.Windows.Forms.Label();
            this.labelcategoria = new System.Windows.Forms.Label();
            this.labelnombre = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.categoria = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textdescripcion = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtprivilegio = new System.Windows.Forms.TextBox();
            this.buttonagregar = new System.Windows.Forms.Button();
            this.buttonborrar = new System.Windows.Forms.Button();
            this.dataGridViewpermisos = new System.Windows.Forms.DataGridView();
            this.No = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.permiso = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Categ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Descripcion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ROL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.asignado = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.PRIV = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.p = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBoxRole = new System.Windows.Forms.ComboBox();
            this.buttonguardar = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label14 = new System.Windows.Forms.Label();
            this.toolStripButton5 = new System.Windows.Forms.Button();
            this.serch = new System.Windows.Forms.TextBox();
            this.label31 = new System.Windows.Forms.Label();
            this.PanelBarraTitulo = new System.Windows.Forms.Panel();
            this.titulo = new System.Windows.Forms.Label();
            this.btnNormal = new System.Windows.Forms.Button();
            this.pictureBox8 = new System.Windows.Forms.PictureBox();
            this.label33 = new System.Windows.Forms.Label();
            this.btnMinimizar = new System.Windows.Forms.Button();
            this.btnMaximizar = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelcargo = new System.Windows.Forms.Label();
            this.labelapellido = new System.Windows.Forms.Label();
            this.labelname = new System.Windows.Forms.Label();
            this.lbFecha = new System.Windows.Forms.Label();
            this.pictureBoxfoto = new System.Windows.Forms.PictureBox();
            this.lblHora = new System.Windows.Forms.Label();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewpermisos)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.PanelBarraTitulo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxfoto)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSize = true;
            this.groupBox1.Controls.Add(this.labeldescripcion);
            this.groupBox1.Controls.Add(this.labelcategoria);
            this.groupBox1.Controls.Add(this.labelnombre);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.categoria);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textdescripcion);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtprivilegio);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(13, 178);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(685, 154);
            this.groupBox1.TabIndex = 29;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Nuevo Permiso";
            // 
            // labeldescripcion
            // 
            this.labeldescripcion.AutoSize = true;
            this.labeldescripcion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labeldescripcion.ForeColor = System.Drawing.Color.Red;
            this.labeldescripcion.Location = new System.Drawing.Point(625, 86);
            this.labeldescripcion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labeldescripcion.Name = "labeldescripcion";
            this.labeldescripcion.Size = new System.Drawing.Size(13, 17);
            this.labeldescripcion.TabIndex = 259;
            this.labeldescripcion.Text = "*";
            // 
            // labelcategoria
            // 
            this.labelcategoria.AutoSize = true;
            this.labelcategoria.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelcategoria.ForeColor = System.Drawing.Color.Red;
            this.labelcategoria.Location = new System.Drawing.Point(589, 38);
            this.labelcategoria.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelcategoria.Name = "labelcategoria";
            this.labelcategoria.Size = new System.Drawing.Size(13, 17);
            this.labelcategoria.TabIndex = 258;
            this.labelcategoria.Text = "*";
            // 
            // labelnombre
            // 
            this.labelnombre.AutoSize = true;
            this.labelnombre.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelnombre.ForeColor = System.Drawing.Color.Red;
            this.labelnombre.Location = new System.Drawing.Point(291, 36);
            this.labelnombre.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelnombre.Name = "labelnombre";
            this.labelnombre.Size = new System.Drawing.Size(13, 17);
            this.labelnombre.TabIndex = 257;
            this.labelnombre.Text = "*";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(324, 41);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 18);
            this.label3.TabIndex = 7;
            this.label3.Text = "Categoria";
            // 
            // categoria
            // 
            this.categoria.Location = new System.Drawing.Point(413, 36);
            this.categoria.Margin = new System.Windows.Forms.Padding(4);
            this.categoria.Name = "categoria";
            this.categoria.Size = new System.Drawing.Size(169, 24);
            this.categoria.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 82);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 18);
            this.label1.TabIndex = 5;
            this.label1.Text = "Descripción";
            // 
            // textdescripcion
            // 
            this.textdescripcion.Location = new System.Drawing.Point(115, 82);
            this.textdescripcion.Margin = new System.Windows.Forms.Padding(4);
            this.textdescripcion.Name = "textdescripcion";
            this.textdescripcion.Size = new System.Drawing.Size(504, 24);
            this.textdescripcion.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 41);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 18);
            this.label2.TabIndex = 3;
            this.label2.Text = "Nombre";
            // 
            // txtprivilegio
            // 
            this.txtprivilegio.Location = new System.Drawing.Point(115, 36);
            this.txtprivilegio.Margin = new System.Windows.Forms.Padding(4);
            this.txtprivilegio.Name = "txtprivilegio";
            this.txtprivilegio.Size = new System.Drawing.Size(169, 24);
            this.txtprivilegio.TabIndex = 2;
            // 
            // buttonagregar
            // 
            this.buttonagregar.AutoSize = true;
            this.buttonagregar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(45)))), ((int)(((byte)(53)))));
            this.buttonagregar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonagregar.ForeColor = System.Drawing.Color.White;
            this.buttonagregar.Location = new System.Drawing.Point(1435, 79);
            this.buttonagregar.Margin = new System.Windows.Forms.Padding(4);
            this.buttonagregar.Name = "buttonagregar";
            this.buttonagregar.Size = new System.Drawing.Size(160, 37);
            this.buttonagregar.TabIndex = 161;
            this.buttonagregar.Text = "Agregar";
            this.buttonagregar.UseVisualStyleBackColor = false;
            this.buttonagregar.Click += new System.EventHandler(this.insert);
            // 
            // buttonborrar
            // 
            this.buttonborrar.AutoSize = true;
            this.buttonborrar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(45)))), ((int)(((byte)(53)))));
            this.buttonborrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonborrar.ForeColor = System.Drawing.Color.White;
            this.buttonborrar.Location = new System.Drawing.Point(1435, 178);
            this.buttonborrar.Margin = new System.Windows.Forms.Padding(4);
            this.buttonborrar.Name = "buttonborrar";
            this.buttonborrar.Size = new System.Drawing.Size(160, 41);
            this.buttonborrar.TabIndex = 181;
            this.buttonborrar.Text = "Borrar";
            this.buttonborrar.UseVisualStyleBackColor = false;
            this.buttonborrar.Click += new System.EventHandler(this.Borrar);
            // 
            // dataGridViewpermisos
            // 
            this.dataGridViewpermisos.AllowUserToAddRows = false;
            this.dataGridViewpermisos.AllowUserToDeleteRows = false;
            this.dataGridViewpermisos.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(69)))), ((int)(((byte)(76)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            this.dataGridViewpermisos.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewpermisos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewpermisos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewpermisos.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridViewpermisos.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(69)))), ((int)(((byte)(76)))));
            this.dataGridViewpermisos.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dataGridViewpermisos.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewpermisos.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewpermisos.ColumnHeadersHeight = 40;
            this.dataGridViewpermisos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridViewpermisos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.No,
            this.permiso,
            this.Categ,
            this.Descripcion,
            this.ROL,
            this.asignado,
            this.PRIV,
            this.ID,
            this.p});
            this.dataGridViewpermisos.Cursor = System.Windows.Forms.Cursors.SizeNWSE;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(61)))), ((int)(((byte)(69)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewpermisos.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewpermisos.EnableHeadersVisualStyles = false;
            this.dataGridViewpermisos.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(69)))), ((int)(((byte)(76)))));
            this.dataGridViewpermisos.Location = new System.Drawing.Point(0, 345);
            this.dataGridViewpermisos.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridViewpermisos.MultiSelect = false;
            this.dataGridViewpermisos.Name = "dataGridViewpermisos";
            this.dataGridViewpermisos.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(61)))), ((int)(((byte)(69)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewpermisos.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridViewpermisos.RowHeadersVisible = false;
            this.dataGridViewpermisos.RowHeadersWidth = 51;
            this.dataGridViewpermisos.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridViewpermisos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewpermisos.Size = new System.Drawing.Size(1664, 386);
            this.dataGridViewpermisos.TabIndex = 241;
            this.dataGridViewpermisos.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.CellContentClick);
            this.dataGridViewpermisos.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Check);
            // 
            // No
            // 
            this.No.HeaderText = "No.";
            this.No.MinimumWidth = 6;
            this.No.Name = "No";
            this.No.ReadOnly = true;
            // 
            // permiso
            // 
            this.permiso.HeaderText = "Permisos";
            this.permiso.MinimumWidth = 6;
            this.permiso.Name = "permiso";
            this.permiso.ReadOnly = true;
            // 
            // Categ
            // 
            this.Categ.HeaderText = "Categoria";
            this.Categ.MinimumWidth = 6;
            this.Categ.Name = "Categ";
            this.Categ.ReadOnly = true;
            // 
            // Descripcion
            // 
            this.Descripcion.HeaderText = "Descripcion";
            this.Descripcion.MinimumWidth = 6;
            this.Descripcion.Name = "Descripcion";
            this.Descripcion.ReadOnly = true;
            // 
            // ROL
            // 
            this.ROL.HeaderText = "ROL";
            this.ROL.MinimumWidth = 6;
            this.ROL.Name = "ROL";
            this.ROL.ReadOnly = true;
            // 
            // asignado
            // 
            this.asignado.HeaderText = "Asignado";
            this.asignado.MinimumWidth = 6;
            this.asignado.Name = "asignado";
            this.asignado.ReadOnly = true;
            // 
            // PRIV
            // 
            this.PRIV.HeaderText = "PRIV";
            this.PRIV.MinimumWidth = 6;
            this.PRIV.Name = "PRIV";
            this.PRIV.ReadOnly = true;
            this.PRIV.Visible = false;
            // 
            // ID
            // 
            this.ID.HeaderText = "ID";
            this.ID.MinimumWidth = 6;
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            this.ID.Visible = false;
            // 
            // p
            // 
            this.p.HeaderText = "P";
            this.p.MinimumWidth = 6;
            this.p.Name = "p";
            this.p.ReadOnly = true;
            this.p.Visible = false;
            // 
            // groupBox2
            // 
            this.groupBox2.AutoSize = true;
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.comboBoxRole);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.ForeColor = System.Drawing.Color.White;
            this.groupBox2.Location = new System.Drawing.Point(725, 178);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(644, 154);
            this.groupBox2.TabIndex = 242;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Role";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(49, 49);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(37, 17);
            this.label6.TabIndex = 21;
            this.label6.Text = "Role";
            // 
            // comboBoxRole
            // 
            this.comboBoxRole.FormattingEnabled = true;
            this.comboBoxRole.Location = new System.Drawing.Point(107, 49);
            this.comboBoxRole.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxRole.Name = "comboBoxRole";
            this.comboBoxRole.Size = new System.Drawing.Size(337, 26);
            this.comboBoxRole.TabIndex = 13;
            this.comboBoxRole.SelectedIndexChanged += new System.EventHandler(this.ComboBoxRole_SelectedIndexChanged);
            // 
            // buttonguardar
            // 
            this.buttonguardar.AutoSize = true;
            this.buttonguardar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(45)))), ((int)(((byte)(53)))));
            this.buttonguardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonguardar.ForeColor = System.Drawing.Color.White;
            this.buttonguardar.Location = new System.Drawing.Point(1435, 126);
            this.buttonguardar.Margin = new System.Windows.Forms.Padding(4);
            this.buttonguardar.Name = "buttonguardar";
            this.buttonguardar.Size = new System.Drawing.Size(160, 37);
            this.buttonguardar.TabIndex = 243;
            this.buttonguardar.Text = "Guardar";
            this.buttonguardar.UseVisualStyleBackColor = false;
            this.buttonguardar.Click += new System.EventHandler(this.updat);
            // 
            // button5
            // 
            this.button5.AutoSize = true;
            this.button5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(45)))), ((int)(((byte)(53)))));
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button5.ForeColor = System.Drawing.Color.White;
            this.button5.Location = new System.Drawing.Point(1435, 241);
            this.button5.Margin = new System.Windows.Forms.Padding(4);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(160, 37);
            this.button5.TabIndex = 245;
            this.button5.Text = "Limpiar";
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.clear);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorker1_DoWork);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label14);
            this.groupBox5.Controls.Add(this.toolStripButton5);
            this.groupBox5.Controls.Add(this.serch);
            this.groupBox5.Controls.Add(this.label31);
            this.groupBox5.ForeColor = System.Drawing.Color.White;
            this.groupBox5.Location = new System.Drawing.Point(13, 78);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox5.Size = new System.Drawing.Size(1356, 80);
            this.groupBox5.TabIndex = 289;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Busqueda";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(31, 39);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(58, 17);
            this.label14.TabIndex = 22;
            this.label14.Text = "Nombre";
            // 
            // toolStripButton5
            // 
            this.toolStripButton5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.toolStripButton5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(45)))), ((int)(((byte)(53)))));
            this.toolStripButton5.Cursor = System.Windows.Forms.Cursors.Default;
            this.toolStripButton5.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.toolStripButton5.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(69)))), ((int)(((byte)(76)))));
            this.toolStripButton5.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(45)))), ((int)(((byte)(53)))));
            this.toolStripButton5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.toolStripButton5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripButton5.ForeColor = System.Drawing.Color.Silver;
            this.toolStripButton5.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolStripButton5.Location = new System.Drawing.Point(1173, 30);
            this.toolStripButton5.Margin = new System.Windows.Forms.Padding(4);
            this.toolStripButton5.Name = "toolStripButton5";
            this.toolStripButton5.Size = new System.Drawing.Size(149, 34);
            this.toolStripButton5.TabIndex = 21;
            this.toolStripButton5.Text = "Buscar";
            this.toolStripButton5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolStripButton5.UseVisualStyleBackColor = false;
            this.toolStripButton5.Click += new System.EventHandler(this.buscar);
            // 
            // serch
            // 
            this.serch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.serch.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serch.Location = new System.Drawing.Point(109, 33);
            this.serch.Margin = new System.Windows.Forms.Padding(4);
            this.serch.Name = "serch";
            this.serch.Size = new System.Drawing.Size(1016, 26);
            this.serch.TabIndex = 19;
            this.serch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.bucartext);
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label31.ForeColor = System.Drawing.Color.LightGray;
            this.label31.Location = new System.Drawing.Point(29, 100);
            this.label31.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(82, 20);
            this.label31.TabIndex = 18;
            this.label31.Text = "Apellidos:";
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
            this.PanelBarraTitulo.Size = new System.Drawing.Size(1664, 53);
            this.PanelBarraTitulo.TabIndex = 290;
            this.PanelBarraTitulo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PanelBarraTitulo_MouseDown);
            // 
            // titulo
            // 
            this.titulo.AutoSize = true;
            this.titulo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titulo.ForeColor = System.Drawing.Color.White;
            this.titulo.Location = new System.Drawing.Point(751, 14);
            this.titulo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.titulo.Name = "titulo";
            this.titulo.Size = new System.Drawing.Size(156, 25);
            this.titulo.TabIndex = 8;
            this.titulo.Text = "ATAH SYSTEM";
            // 
            // btnNormal
            // 
            this.btnNormal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNormal.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNormal.FlatAppearance.BorderSize = 0;
            this.btnNormal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNormal.Image = global::Autobuses.Properties.Resources.Normal;
            this.btnNormal.Location = new System.Drawing.Point(1525, -2);
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
            this.label33.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label33.ForeColor = System.Drawing.Color.White;
            this.label33.Location = new System.Drawing.Point(45, 18);
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
            this.btnMinimizar.Location = new System.Drawing.Point(1464, 0);
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
            this.btnMaximizar.Location = new System.Drawing.Point(1525, 0);
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
            this.button2.Location = new System.Drawing.Point(1587, 2);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(52, 48);
            this.button2.TabIndex = 0;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(61)))), ((int)(((byte)(69)))));
            this.panel1.Controls.Add(this.labelcargo);
            this.panel1.Controls.Add(this.labelapellido);
            this.panel1.Controls.Add(this.labelname);
            this.panel1.Controls.Add(this.lbFecha);
            this.panel1.Controls.Add(this.pictureBoxfoto);
            this.panel1.Controls.Add(this.lblHora);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 782);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1664, 123);
            this.panel1.TabIndex = 296;
            // 
            // labelcargo
            // 
            this.labelcargo.AutoSize = true;
            this.labelcargo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelcargo.ForeColor = System.Drawing.Color.LightGray;
            this.labelcargo.Location = new System.Drawing.Point(109, 66);
            this.labelcargo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelcargo.Name = "labelcargo";
            this.labelcargo.Size = new System.Drawing.Size(54, 20);
            this.labelcargo.TabIndex = 7;
            this.labelcargo.Text = "Cargo";
            // 
            // labelapellido
            // 
            this.labelapellido.AutoSize = true;
            this.labelapellido.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelapellido.ForeColor = System.Drawing.Color.LightGray;
            this.labelapellido.Location = new System.Drawing.Point(109, 44);
            this.labelapellido.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelapellido.Name = "labelapellido";
            this.labelapellido.Size = new System.Drawing.Size(118, 20);
            this.labelapellido.TabIndex = 6;
            this.labelapellido.Text = "Apellidos User";
            // 
            // labelname
            // 
            this.labelname.AutoSize = true;
            this.labelname.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelname.ForeColor = System.Drawing.Color.LightGray;
            this.labelname.Location = new System.Drawing.Point(109, 22);
            this.labelname.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelname.Name = "labelname";
            this.labelname.Size = new System.Drawing.Size(82, 20);
            this.labelname.TabIndex = 5;
            this.labelname.Text = "Nombres ";
            // 
            // lbFecha
            // 
            this.lbFecha.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbFecha.AutoSize = true;
            this.lbFecha.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbFecha.ForeColor = System.Drawing.Color.White;
            this.lbFecha.Location = new System.Drawing.Point(1315, 87);
            this.lbFecha.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbFecha.Name = "lbFecha";
            this.lbFecha.Size = new System.Drawing.Size(275, 25);
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
            this.lblHora.Location = new System.Drawing.Point(1320, 14);
            this.lblHora.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblHora.Name = "lblHora";
            this.lblHora.Size = new System.Drawing.Size(259, 67);
            this.lblHora.TabIndex = 1;
            this.lblHora.Text = "21:49:45";
            // 
            // timer2
            // 
            this.timer2.Tick += new System.EventHandler(this.Timer2_Tick);
            // 
            // button1
            // 
            this.button1.AutoSize = true;
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(45)))), ((int)(((byte)(53)))));
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(1435, 295);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(160, 37);
            this.button1.TabIndex = 297;
            this.button1.Text = "Excel";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.ToolStripExportExcel_Click);
            // 
            // Permisos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(69)))), ((int)(((byte)(76)))));
            this.ClientSize = new System.Drawing.Size(1664, 905);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.PanelBarraTitulo);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.buttonguardar);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.dataGridViewpermisos);
            this.Controls.Add(this.buttonborrar);
            this.Controls.Add(this.buttonagregar);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Permisos";
            this.Text = "Permisos";
            this.Load += new System.EventHandler(this.Permisos_Load);
            this.Shown += new System.EventHandler(this.Permisos_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewpermisos)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.PanelBarraTitulo.ResumeLayout(false);
            this.PanelBarraTitulo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxfoto)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtprivilegio;
        private System.Windows.Forms.Button buttonagregar;
        private System.Windows.Forms.Button buttonborrar;
        private System.Windows.Forms.DataGridView dataGridViewpermisos;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox comboBoxRole;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button buttonguardar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textdescripcion;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox categoria;
        private System.Windows.Forms.DataGridViewTextBoxColumn No;
        private System.Windows.Forms.DataGridViewTextBoxColumn permiso;
        private System.Windows.Forms.DataGridViewTextBoxColumn Categ;
        private System.Windows.Forms.DataGridViewTextBoxColumn Descripcion;
        private System.Windows.Forms.DataGridViewTextBoxColumn ROL;
        private System.Windows.Forms.DataGridViewCheckBoxColumn asignado;
        private System.Windows.Forms.DataGridViewTextBoxColumn PRIV;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn p;
        private System.Windows.Forms.Label labeldescripcion;
        private System.Windows.Forms.Label labelcategoria;
        private System.Windows.Forms.Label labelnombre;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button toolStripButton5;
        public System.Windows.Forms.TextBox serch;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Panel PanelBarraTitulo;
        private System.Windows.Forms.Button btnNormal;
        private System.Windows.Forms.PictureBox pictureBox8;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Button btnMinimizar;
        private System.Windows.Forms.Button btnMaximizar;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelcargo;
        private System.Windows.Forms.Label labelapellido;
        private System.Windows.Forms.Label labelname;
        private System.Windows.Forms.Label lbFecha;
        private System.Windows.Forms.PictureBox pictureBoxfoto;
        private System.Windows.Forms.Label lblHora;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label titulo;
    }
}