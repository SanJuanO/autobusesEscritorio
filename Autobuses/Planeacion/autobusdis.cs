using ConnectDB;
using Essy.Tools.InputBox;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Autobuses.Planeacion
{
    public partial class autobusdis : Form
    {
        string cantidad;

        public database db;
        ResultSet res = null;

        string asiento = "asiento";
        string escalera = "escalera";
        string baño = "baño";
        string nada = "";
        string NOMBREDEAUTOBUS;
        string _clase = "DISEÑO";

        int FILA;
        int COLUMNA;
        string pk;
        private int n = 0;
        string TIPO;
        string pkid;
        string cantidaddefila;
        string cantidadpisos;
        private Bitmap imagenansiento = new Bitmap(Properties.Resources.asiento);
        private Bitmap imagenansientopantalla = new Bitmap(Properties.Resources.asientopantall1);
        private Bitmap imagenpantalla = new Bitmap(Properties.Resources.pantalla);
        private Bitmap imagenrojo = new Bitmap(Properties.Resources.rojo);
        private Bitmap imagenamarrillo = new Bitmap(Properties.Resources.amarrillo);
        private Bitmap imagenazul = new Bitmap(Properties.Resources.AZUL);
        private Bitmap imagenpasillo = new Bitmap(Properties.Resources.pasillo);
        private Bitmap imagenbaño = new Bitmap(Properties.Resources.BAN1);
        private Bitmap imagenescalera = new Bitmap(Properties.Resources.ESCALERAS);
        private bool permiso = false;
        public autobusdis()
        {
            InitializeComponent();
            this.Show();
            labelnombre.Text = LoginInfo.NombreID;
            labelapellido.Text = LoginInfo.ApellidoID;
            MemoryStream ms = new MemoryStream((byte[])LoginInfo.imagenfoto);
            Bitmap bmp = new Bitmap(ms);
            pictureBoxfoto.Image = bmp;
            labelcargo.Text = LoginInfo.rol;
            timer2.Start();
            label33.Text = "Diseño de Autobuses";


        }

        private void permisos()
        {
            buttonAGREGAR.Visible = false;
            buttonborrar.Visible = false;
            buttonborrar.Visible = false;

            if (LoginInfo.privilegios.Any(x => x == "agregar diseño de autobuses"))
            {
                buttonAGREGAR.Visible = true;

            }
            if (LoginInfo.privilegios.Any(x => x == "Borrar diseño de autobus"))
            {
                buttonborrar.Visible = true;
         

            }
            if (LoginInfo.privilegios.Any(x => x == "Modificar diseño"))
            {
                permiso = true;

            }
        }
        public void getRows()
        {
            try
            {
                dataAUTOBUSES.Rows.Clear();
                string sql = "SELECT * FROM TIPOAUTOBUS";

                db.PreparedSQL(sql);


                res = db.getTable();
                while (res.Next())
                {
                    n = dataAUTOBUSES.Rows.Add();


                    dataAUTOBUSES.Rows[n].Cells[0].Value = res.Get("PK");
                    dataAUTOBUSES.Rows[n].Cells[1].Value = res.Get("Descripcion");
                    dataAUTOBUSES.Rows[n].Cells[2].Value = res.Get("CANTIDADEFILAS");
                    dataAUTOBUSES.Rows[n].Cells[3].Value = res.Get("PISOS");



                }
                if (db.execute())
                {

                }


            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "getRows";
                Utilerias.LOG.write(_clase, funcion, error);


            }

        }


        private void insertt(object sender, EventArgs e)
        {
            try
            {

                if (validar())
                {


                    autobus.Rows.Clear();

                    int PISO = int.Parse(pisos.SelectedItem.ToString());

                    string sql = "INSERT INTO TIPOAUTOBUS(Descripcion,CANTIDADEFILAS,PISOS) " +
                                             "VALUES(@Descripcion,@CANTIDADEFILAS,@PISOS)";

                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@Descripcion", NOMBREDEAUTOBUS);
                    db.command.Parameters.AddWithValue("@CANTIDADEFILAS", cantidad);
                    db.command.Parameters.AddWithValue("@PISOS", PISO);

                    if (db.execute())
                    {
                        Utilerias.LOG.acciones("agrego un autobus " + NOMBREDEAUTOBUS);

                        Form mensaje = new Mensaje("Creado", true);
                        mensaje.ShowDialog();
                    }

                    sql = "SELECT * FROM TIPOAUTOBUS WHERE Descripcion=@Descripcion";

                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@Descripcion", NOMBREDEAUTOBUS);

                    res = db.getTable();

                    while (res.Next())
                    {
                        pk = res.Get("PK");
                    }
                    if (db.execute())
                    {

                    }
              

                    for (int i = 0; i <= int.Parse(cantidad) ; i++)
                    {

                        for (int m = 0; m < 5; m++)
                        {



                            //  Bitmap bitmap = new Bitmap("C:/Users/wksadmin/Downloads/asiento.png");



                            if (m != 2)
                            {
                                sql = "INSERT INTO DESCTIPOAUTOBUS(PKTIPOAUTOBUS,FILA,COLUMNA,OBJETO,ETIQUETA,PISOS) VALUES(@PKTIPOAUTOBUS,@FILA,@COLUMNA,@OBJETO,@ETIQUETA,@PISOS)";
                                db.PreparedSQL(sql);
                                // autobus.Rows[i].Cells[m].Value = img;
                                string etiqueta = "";
                                TIPO = asiento;
                                FILA = i;
                                COLUMNA = m;
                                db.command.Parameters.AddWithValue("@PKTIPOAUTOBUS", pk);
                                db.command.Parameters.AddWithValue("@FILA", FILA);
                                db.command.Parameters.AddWithValue("@COLUMNA", COLUMNA);
                                db.command.Parameters.AddWithValue("@PISOS", "1");
                                db.command.Parameters.AddWithValue("@OBJETO", TIPO);
                                db.command.Parameters.AddWithValue("@ETIQUETA", etiqueta);
                                if (db.execute())
                                { }
                            }

                            if (m == 2)
                            {
                                sql = "INSERT INTO DESCTIPOAUTOBUS(PKTIPOAUTOBUS,FILA,COLUMNA,OBJETO,ETIQUETA,PISOS) VALUES(@PKTIPOAUTOBUS,@FILA,@COLUMNA,@OBJETO,@ETIQUETA,@PISOS)";
                                db.PreparedSQL(sql);
                                //    autobus.Rows[i].Cells[m].Value = nadaimg;
                                string etiqueta = "";

                                string NADA = "";
                                FILA = i;
                                COLUMNA = m;
                                db.command.Parameters.AddWithValue("@PKTIPOAUTOBUS", pk);
                                db.command.Parameters.AddWithValue("@FILA", FILA);
                                db.command.Parameters.AddWithValue("@COLUMNA", COLUMNA);
                                db.command.Parameters.AddWithValue("@OBJETO", NADA);
                                db.command.Parameters.AddWithValue("@ETIQUETA", etiqueta);
                                db.command.Parameters.AddWithValue("@PISOS", 1);
                                db.execute();
                              
                            }
                            if (PISO == 2)
                            {
                                if (m != 2)
                                {
                                    sql = "INSERT INTO DESCTIPOAUTOBUS(PKTIPOAUTOBUS,FILA,COLUMNA,OBJETO,ETIQUETA,PISOS) VALUES(@PKTIPOAUTOBUS,@FILA,@COLUMNA,@OBJETO,@ETIQUETA,@PISOS)";
                                    db.PreparedSQL(sql);
                                    // autobus.Rows[i].Cells[m].Value = img;
                                    string etiqueta = "";
                                    TIPO = asiento;
                                    FILA = i;
                                    COLUMNA = m;
                                    db.command.Parameters.AddWithValue("@PKTIPOAUTOBUS", pk);
                                    db.command.Parameters.AddWithValue("@FILA", FILA);
                                    db.command.Parameters.AddWithValue("@COLUMNA", COLUMNA);
                                    db.command.Parameters.AddWithValue("@PISOS", PISO);
                                    db.command.Parameters.AddWithValue("@OBJETO", TIPO);
                                    db.command.Parameters.AddWithValue("@ETIQUETA", etiqueta);
                                    db.execute();
                                }

                                if (m == 2)
                                {
                                    sql = "INSERT INTO DESCTIPOAUTOBUS(PKTIPOAUTOBUS,FILA,COLUMNA,OBJETO,ETIQUETA,PISOS) VALUES(@PKTIPOAUTOBUS,@FILA,@COLUMNA,@OBJETO,@ETIQUETA,@PISOS)";
                                    db.PreparedSQL(sql);
                                    //    autobus.Rows[i].Cells[m].Value = nadaimg;
                                    string etiqueta = "";

                                    string NADA = "";
                                    FILA = i;
                                    COLUMNA = m;
                                    db.command.Parameters.AddWithValue("@PKTIPOAUTOBUS", pk);
                                    db.command.Parameters.AddWithValue("@FILA", FILA);
                                    db.command.Parameters.AddWithValue("@COLUMNA", COLUMNA);
                                    db.command.Parameters.AddWithValue("@OBJETO", NADA);
                                    db.command.Parameters.AddWithValue("@ETIQUETA", etiqueta);
                                    db.command.Parameters.AddWithValue("@PISOS", PISO);
                                    db.execute();
                                }

                            }



                        }

                    }
                    getRows();

                }
                else
                {
                    Form mensaje = new Mensaje("Ingresa un entero", true);

                   mensaje.ShowDialog();
                                }

            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "insertt";
                Utilerias.LOG.write(_clase, funcion, error);


            }

        }

        private Boolean validar()
        {

            cantidad = comboBoxfilas.SelectedItem.ToString();

            Boolean validar = true;
            int ejem = 0;
            NOMBREDEAUTOBUS = textBoxnombre.Text;

            if (NOMBREDEAUTOBUS == "")
            {
                labelnombre.Visible = true;
                validar = false;
            }
            if (!int.TryParse(cantidad, out ejem))
            {
                labelcantidad.Visible = true;
                validar = false;

            }

            return validar;
        }

        private void modificarautobus(object sender, DataGridViewCellEventArgs e)
        {
            try
            
            {

                if (permiso == true)
                {
                    int n = e.RowIndex;
                    int c = e.ColumnIndex;
                    string tipo = comboBoxtipo.SelectedItem.ToString();



                    if (tipo == "Asiento")
                    {
                        string texto = InputBox.ShowInputBox("asignacion de asiento");

                        string firstText = texto;
                        PointF firstLocation = new PointF(8f, 8f);
                        Bitmap asientoimg = new Bitmap(imagenansiento);
                        using (Graphics graphics = Graphics.FromImage(asientoimg))
                        {
                            using (Font arialFont = new Font("Arial", 10))
                            {
                                graphics.DrawString(firstText, arialFont, Brushes.Black, firstLocation);
                            }
                        }
                        autobus.Rows[n].Cells[c].Value = asientoimg;
                        TIPO = asiento;
                        string sql = "UPDATE DESCTIPOAUTOBUS SET OBJETO=@OBJETO, ETIQUETA=@ETIQUETA WHERE PKTIPOAUTOBUS=@PKTIPOAUTOBUS AND FILA=@FILA AND COLUMNA=@COLUMNA AND PISOS=@PISO";


                        db.PreparedSQL(sql);
                        db.command.Parameters.AddWithValue("@PKTIPOAUTOBUS", pkid);
                        db.command.Parameters.AddWithValue("@OBJETO", TIPO);
                        db.command.Parameters.AddWithValue("@ETIQUETA", texto);
                        db.command.Parameters.AddWithValue("@FILA", n);
                        db.command.Parameters.AddWithValue("@COLUMNA", c);
                        db.command.Parameters.AddWithValue("@PISO", "1");

                        if (db.execute())
                        {
                            Utilerias.LOG.acciones("modifico un autobus " + pkid);

                            Form mensaje = new Mensaje("Modificado", true);
 mensaje.ShowDialog();
                                                 }

                    }
                    if (tipo == "Asiento con pantalla")
                    {
                        string texto = InputBox.ShowInputBox("asignacion de asiento");

                        string firstText = texto;
                        PointF firstLocation = new PointF(8f, 8f);
                        Bitmap asientoimg = new Bitmap(imagenansientopantalla);
                        using (Graphics graphics = Graphics.FromImage(asientoimg))
                        {
                            using (Font arialFont = new Font("Arial", 10))
                            {
                                graphics.DrawString(firstText, arialFont, Brushes.Black, firstLocation);
                            }
                        }
                        autobus.Rows[n].Cells[c].Value = asientoimg;
                        TIPO = tipo;
                        string sql = "UPDATE DESCTIPOAUTOBUS SET OBJETO=@OBJETO, ETIQUETA=@ETIQUETA WHERE PKTIPOAUTOBUS=@PKTIPOAUTOBUS AND FILA=@FILA AND COLUMNA=@COLUMNA AND PISOS=@PISO";


                        db.PreparedSQL(sql);
                        db.command.Parameters.AddWithValue("@PKTIPOAUTOBUS", pkid);
                        db.command.Parameters.AddWithValue("@OBJETO", TIPO);
                        db.command.Parameters.AddWithValue("@ETIQUETA", texto);
                        db.command.Parameters.AddWithValue("@FILA", n);
                        db.command.Parameters.AddWithValue("@COLUMNA", c);
                        db.command.Parameters.AddWithValue("@PISO", "1");

                        if (db.execute())
                        {
                            Utilerias.LOG.acciones("modifico un autobus " + pkid);

                            Form mensaje = new Mensaje("Modificado", true);
                            mensaje.ShowDialog();
                        }

                    }
                    if (tipo == "Pantalla")
                    {
                        autobus.Rows[n].Cells[c].Value = imagenpantalla;
                        TIPO = tipo;
                        string sql = "UPDATE DESCTIPOAUTOBUS SET OBJETO=@OBJETO, ETIQUETA=@ETIQUETA WHERE PKTIPOAUTOBUS=@PKTIPOAUTOBUS AND FILA=@FILA AND COLUMNA=@COLUMNA AND PISOS=@PISO";


                        db.PreparedSQL(sql);
                        db.command.Parameters.AddWithValue("@PKTIPOAUTOBUS", pkid);
                        db.command.Parameters.AddWithValue("@OBJETO", TIPO);
                        db.command.Parameters.AddWithValue("@ETIQUETA", "");
                        db.command.Parameters.AddWithValue("@FILA", n);
                        db.command.Parameters.AddWithValue("@COLUMNA", c);
                        db.command.Parameters.AddWithValue("@PISO", 1);

                        if (db.execute())
                        {
                            Utilerias.LOG.acciones("modifico un autobus " + pkid);

                            Form mensaje = new Mensaje("Modificado", true);
                            mensaje.ShowDialog();
                        }
                    }
                    if (tipo == "Baño")
                    {
                        autobus.Rows[n].Cells[c].Value = imagenbaño;
                        TIPO = baño;
                        string sql = "UPDATE DESCTIPOAUTOBUS SET OBJETO=@OBJETO, ETIQUETA=@ETIQUETA WHERE PKTIPOAUTOBUS=@PKTIPOAUTOBUS AND FILA=@FILA AND COLUMNA=@COLUMNA AND PISOS=@PISO";


                        db.PreparedSQL(sql);
                        db.command.Parameters.AddWithValue("@PKTIPOAUTOBUS", pkid);
                        db.command.Parameters.AddWithValue("@OBJETO", TIPO);
                        db.command.Parameters.AddWithValue("@ETIQUETA", "");
                        db.command.Parameters.AddWithValue("@FILA", n);
                        db.command.Parameters.AddWithValue("@COLUMNA", c);
                        db.command.Parameters.AddWithValue("@PISO", "1");

                        if (db.execute())
                        {
                            Utilerias.LOG.acciones("modifico un autobus " + pkid);

                            Form mensaje = new Mensaje("Modificado", true);
                            mensaje.ShowDialog();
                        }
                    }
                    if (tipo == "Escalera")
                    {
                        autobus.Rows[n].Cells[c].Value = imagenescalera;
                        TIPO = escalera;
                        string sql = "UPDATE DESCTIPOAUTOBUS SET OBJETO=@OBJETO, ETIQUETA=@ETIQUETA WHERE PKTIPOAUTOBUS=@PKTIPOAUTOBUS AND FILA=@FILA AND COLUMNA=@COLUMNA AND PISOS=@PISO";


                        db.PreparedSQL(sql);
                        db.command.Parameters.AddWithValue("@PKTIPOAUTOBUS", pkid);
                        db.command.Parameters.AddWithValue("@OBJETO", TIPO);
                        db.command.Parameters.AddWithValue("@ETIQUETA", "");
                        db.command.Parameters.AddWithValue("@FILA", n);
                        db.command.Parameters.AddWithValue("@COLUMNA", c);
                        db.command.Parameters.AddWithValue("@PISO", "1");

                        if (db.execute())
                        {
                            Utilerias.LOG.acciones("modifico un autobus " + pkid);

                            Form mensaje = new Mensaje("Modificado", true);
                            mensaje.ShowDialog();
                        }
                    }
                    if (tipo == "Nada")
                    {

                        string sql = "UPDATE DESCTIPOAUTOBUS SET OBJETO=@OBJETO, ETIQUETA=@ETIQUETA WHERE PKTIPOAUTOBUS=@PKTIPOAUTOBUS AND FILA=@FILA AND COLUMNA=@COLUMNA AND PISOS=@PISO";
                        autobus.Rows[n].Cells[c].Value = imagenpasillo;
                        TIPO = "";

                        db.PreparedSQL(sql);
                        db.command.Parameters.AddWithValue("@PKTIPOAUTOBUS", pkid);
                        db.command.Parameters.AddWithValue("@OBJETO", TIPO);
                        db.command.Parameters.AddWithValue("@ETIQUETA", "");
                        db.command.Parameters.AddWithValue("@FILA", n);
                        db.command.Parameters.AddWithValue("@COLUMNA", c);
                        db.command.Parameters.AddWithValue("@PISO", "1");

                        if (db.execute())
                        {
                            Utilerias.LOG.acciones("modifico un autobus " + pkid);

                            Form mensaje = new Mensaje("Modificado", true);
                            mensaje.ShowDialog();
                        }
                    }


                }

            }


            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "modificarautobus";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }


        private void autobus2piso(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                if (permiso == true)
                {


                    int n = e.RowIndex;
                    int c = e.ColumnIndex;

                    string tipo = comboBoxtipo.SelectedItem.ToString();

                    PISO2.Visible = true;




                    if (tipo == "Asiento")
                    {
                        string texto = InputBox.ShowInputBox("asignacion de asiento");

                        string firstText = texto;
                        PointF firstLocation = new PointF(8f, 8f);
                        Bitmap asientoimg = new Bitmap(imagenansiento);

                        using (Graphics graphics = Graphics.FromImage(asientoimg))
                        {
                            using (Font arialFont = new Font("Arial", 10))
                            {
                                graphics.DrawString(firstText, arialFont, Brushes.Black, firstLocation);
                            }
                        }
                        PISO2.Rows[n].Cells[c].Value = asientoimg;
                        TIPO = asiento;
                        string sql = "UPDATE DESCTIPOAUTOBUS SET OBJETO=@OBJETO, ETIQUETA=@ETIQUETA WHERE PKTIPOAUTOBUS=@PKTIPOAUTOBUS AND FILA=@FILA AND COLUMNA=@COLUMNA AND PISOS=@PISO";


                        db.PreparedSQL(sql);
                        db.command.Parameters.AddWithValue("@PKTIPOAUTOBUS", pkid);
                        db.command.Parameters.AddWithValue("@OBJETO", TIPO);
                        db.command.Parameters.AddWithValue("@ETIQUETA", texto);
                        db.command.Parameters.AddWithValue("@FILA", n);
                        db.command.Parameters.AddWithValue("@COLUMNA", c);
                        db.command.Parameters.AddWithValue("@PISO", cantidadpisos);

                        if (db.execute())
                        {
                            Utilerias.LOG.acciones("modifico un autobus " + pkid);

                            Form mensaje = new Mensaje("Modificado", true);
                            mensaje.ShowDialog();
                        }

                    }
                    if (tipo == "Asiento con pantalla")
                    {
                        string texto = InputBox.ShowInputBox("asignacion de asiento");

                        string firstText = texto;
                        PointF firstLocation = new PointF(8f, 8f);
                        Bitmap asientoimg = new Bitmap(imagenansientopantalla);
                        using (Graphics graphics = Graphics.FromImage(asientoimg))
                        {
                            using (Font arialFont = new Font("Arial", 10))
                            {
                                graphics.DrawString(firstText, arialFont, Brushes.Black, firstLocation);
                            }
                        }
                        PISO2.Rows[n].Cells[c].Value = asientoimg;
                        TIPO = tipo;
                        string sql = "UPDATE DESCTIPOAUTOBUS SET OBJETO=@OBJETO, ETIQUETA=@ETIQUETA WHERE PKTIPOAUTOBUS=@PKTIPOAUTOBUS AND FILA=@FILA AND COLUMNA=@COLUMNA AND PISOS=@PISO";


                        db.PreparedSQL(sql);
                        db.command.Parameters.AddWithValue("@PKTIPOAUTOBUS", pkid);
                        db.command.Parameters.AddWithValue("@OBJETO", TIPO);
                        db.command.Parameters.AddWithValue("@ETIQUETA", texto);
                        db.command.Parameters.AddWithValue("@FILA", n);
                        db.command.Parameters.AddWithValue("@COLUMNA", c);
                        db.command.Parameters.AddWithValue("@PISO", "2");

                        if (db.execute())
                        {
                            Form mensaje = new Mensaje("Modificado", true);
                            mensaje.ShowDialog();
                        }


                    }
                    if (tipo == "Pantalla")
                    {
                        PISO2.Rows[n].Cells[c].Value = imagenpantalla;
                        TIPO = tipo;
                        string sql = "UPDATE DESCTIPOAUTOBUS SET OBJETO=@OBJETO, ETIQUETA=@ETIQUETA WHERE PKTIPOAUTOBUS=@PKTIPOAUTOBUS AND FILA=@FILA AND COLUMNA=@COLUMNA AND PISOS=@PISO";


                        db.PreparedSQL(sql);
                        db.command.Parameters.AddWithValue("@PKTIPOAUTOBUS", pkid);
                        db.command.Parameters.AddWithValue("@OBJETO", TIPO);
                        db.command.Parameters.AddWithValue("@ETIQUETA", "");
                        db.command.Parameters.AddWithValue("@FILA", n);
                        db.command.Parameters.AddWithValue("@COLUMNA", c);
                        db.command.Parameters.AddWithValue("@PISO", cantidadpisos);

                        if (db.execute())
                        {
                            Utilerias.LOG.acciones("modifico un autobus " + pkid);

                            Form mensaje = new Mensaje("Modificado", true);
                            mensaje.ShowDialog();
                        }
                    }
                    if (tipo == "Baño")
                    {
                        PISO2.Rows[n].Cells[c].Value = imagenbaño;
                        TIPO = baño;
                        string sql = "UPDATE DESCTIPOAUTOBUS SET OBJETO=@OBJETO, ETIQUETA=@ETIQUETA WHERE PKTIPOAUTOBUS=@PKTIPOAUTOBUS AND FILA=@FILA AND COLUMNA=@COLUMNA AND PISOS=@PISO";


                        db.PreparedSQL(sql);
                        db.command.Parameters.AddWithValue("@PKTIPOAUTOBUS", pkid);
                        db.command.Parameters.AddWithValue("@OBJETO", TIPO);
                        db.command.Parameters.AddWithValue("@ETIQUETA", "");
                        db.command.Parameters.AddWithValue("@FILA", n);
                        db.command.Parameters.AddWithValue("@COLUMNA", c);
                        db.command.Parameters.AddWithValue("@PISO", cantidadpisos);

                        if (db.execute())
                        {
                            Utilerias.LOG.acciones("modifico un autobus " + pkid);

                            Form mensaje = new Mensaje("Modificado", true);
                            mensaje.ShowDialog();
                        }
                    }
                    if (tipo == "Escalera")
                    {
                        PISO2.Rows[n].Cells[c].Value = imagenescalera;
                        TIPO = escalera;
                        string sql = "UPDATE DESCTIPOAUTOBUS SET OBJETO=@OBJETO, ETIQUETA=@ETIQUETA WHERE PKTIPOAUTOBUS=@PKTIPOAUTOBUS AND FILA=@FILA AND COLUMNA=@COLUMNA AND PISOS=@PISO";


                        db.PreparedSQL(sql);
                        db.command.Parameters.AddWithValue("@PKTIPOAUTOBUS", pkid);
                        db.command.Parameters.AddWithValue("@OBJETO", TIPO);
                        db.command.Parameters.AddWithValue("@ETIQUETA", "");
                        db.command.Parameters.AddWithValue("@FILA", n);
                        db.command.Parameters.AddWithValue("@COLUMNA", c);
                        db.command.Parameters.AddWithValue("@PISO", cantidadpisos);

                        if (db.execute())
                        {
                            Utilerias.LOG.acciones("modifico un autobus " + pkid);

                            Form mensaje = new Mensaje("Modificado", true);
                            mensaje.ShowDialog();
                        }
                    }
                    if (tipo == "Nada")
                    {
                        PISO2.Rows[n].Cells[c].Value = imagenpasillo;
                        TIPO = nada;
                        string sql = "UPDATE DESCTIPOAUTOBUS SET OBJETO=@OBJETO, ETIQUETA=@ETIQUETA WHERE PKTIPOAUTOBUS=@PKTIPOAUTOBUS AND FILA=@FILA AND COLUMNA=@COLUMNA AND PISOS=@PISOS";


                        db.PreparedSQL(sql);
                        db.command.Parameters.AddWithValue("@PKTIPOAUTOBUS", pkid);
                        db.command.Parameters.AddWithValue("@OBJETO", TIPO);
                        db.command.Parameters.AddWithValue("@ETIQUETA", "");
                        db.command.Parameters.AddWithValue("@FILA", n);
                        db.command.Parameters.AddWithValue("@COLUMNA", c);
                        db.command.Parameters.AddWithValue("@PISOS", cantidadpisos);

                        if (db.execute())
                        {
                            Utilerias.LOG.acciones("modifico un autobus " + pkid);

                            Form mensaje = new Mensaje("Modificado", true);
                            mensaje.ShowDialog();
                        }
                    }



                }
            }

            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "autobus2piso";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }
        private void contenidoautobus(object sender, DataGridViewCellEventArgs e)
        {
            try
            {


                buttonAGREGAR.BackColor = Color.White;
                buttonborrar.BackColor = Color.FromArgb(38, 45, 53);
                buttonborrar.Enabled = true;
                buttonAGREGAR.Enabled = false;

                pictureBox1.Visible = true;
                pictureBox11.Visible = true;
                pictureBox2.Visible = false;
                pictureBox22.Visible = false;
                autobus.Rows.Clear();
                autobus.Refresh();

                PISO2.Refresh();
                autobus.Visible = true;
                int n = e.RowIndex;
                int c = e.ColumnIndex;
                pkid = dataAUTOBUSES.Rows[n].Cells[0].Value.ToString();
                cantidaddefila = dataAUTOBUSES.Rows[n].Cells[2].Value.ToString();
                cantidadpisos = dataAUTOBUSES.Rows[n].Cells[3].Value.ToString();

                if (cantidadpisos == "1")
                {
                    PISO2.Visible = false;
                }
                if (cantidadpisos == "2")
                {
                    pictureBox2.Visible = true;
                    pictureBox22.Visible = true;
                    PISO2.Visible = true;
                    PISO2.Rows.Clear();


                    for (int i = 0; i <= int.Parse(cantidaddefila); i++)
                    {

                        PISO2.Rows.Add();

                    }


                    string sql2 = "SELECT * FROM DESCTIPOAUTOBUS WHERE PKTIPOAUTOBUS=@PKTIPOAUTOBUS AND PISOS=@PISO order by fila";
                    db.PreparedSQL(sql2);
                    db.command.Parameters.AddWithValue("@PKTIPOAUTOBUS", pkid);
                    db.command.Parameters.AddWithValue("@PISO", cantidadpisos);

                    res = db.getTable();

                    while (res.Next())
                    {

                        int fila = res.GetInt("FILA");
                        int columna = res.GetInt("COLUMNA");
                        string tipo = res.Get("OBJETO");
                        string asignacion = res.Get("ETIQUETA");


                        if (tipo == "asiento")
                        {
                            Bitmap asientoimg = new Bitmap(imagenansiento);

                            if (asignacion != "")
                            {
                                string firstText = asignacion;
                                PointF firstLocation = new PointF(8f, 8f);
                                using (Graphics graphics = Graphics.FromImage(asientoimg))
                                {
                                    using (Font arialFont = new Font("Arial", 10))
                                    {
                                        graphics.DrawString(firstText, arialFont, Brushes.Black, firstLocation);
                                    }

                                }
                            }
                            PISO2.Rows[fila].Cells[columna].Value = asientoimg;

                        }
                        if (tipo == "Asiento con pantalla")
                        {
                            Bitmap asientoimg = new Bitmap(imagenansientopantalla);

                            if (asignacion != "")
                            {
                                string firstText = asignacion;
                                PointF firstLocation = new PointF(8f, 8f);
                                using (Graphics graphics = Graphics.FromImage(asientoimg))
                                {
                                    using (Font arialFont = new Font("Arial", 10))
                                    {
                                        graphics.DrawString(firstText, arialFont, Brushes.Black, firstLocation);
                                    }

                                }
                            }
                            PISO2.Rows[fila].Cells[columna].Value = asientoimg;

                        }
                        if (tipo == "Pantalla")
                        {

                            PISO2.Rows[fila].Cells[columna].Value = imagenpantalla;

                        }
                        if (tipo == "")
                        {

                            PISO2.Rows[fila].Cells[columna].Value = imagenpasillo;

                        }
                        if (tipo == "escalera")
                        {

                            PISO2.Rows[fila].Cells[columna].Value = imagenescalera;

                        }
                        if (tipo == "baño")
                        {

                            PISO2.Rows[fila].Cells[columna].Value = imagenbaño;

                        }

                    }

                }


                for (int i = 0; i <= int.Parse(cantidaddefila); i++)
                {

                    autobus.Rows.Add();

                }


                string sql = "SELECT * FROM DESCTIPOAUTOBUS WHERE PKTIPOAUTOBUS=@PKTIPOAUTOBUS AND PISOS=@PISO";
                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@PKTIPOAUTOBUS", pkid);
                db.command.Parameters.AddWithValue("@PISO", "1");

                res = db.getTable();

                while (res.Next())
                {
                    int fila = res.GetInt("FILA");
                    int columna = res.GetInt("COLUMNA");
                    string tipo = res.Get("OBJETO");
                    string asignacion = res.Get("ETIQUETA");


                    if (tipo == "asiento")
                    {
                        Bitmap asientoimg = new Bitmap(imagenansiento);

                        if (asignacion != "")
                        {
                            string firstText = asignacion;
                            PointF firstLocation = new PointF(8f, 8f);
                            using (Graphics graphics = Graphics.FromImage(asientoimg))
                            {
                                using (Font arialFont = new Font("Arial", 10))
                                {
                                    graphics.DrawString(firstText, arialFont, Brushes.Black, firstLocation);
                                }

                            }
                        }
                        autobus.Rows[fila].Cells[columna].Value = asientoimg;

                    }
                    if (tipo == "Asiento con pantalla")
                    {
                        Bitmap asientoimg = new Bitmap(imagenansientopantalla);

                        if (asignacion != "")
                        {
                            string firstText = asignacion;
                            PointF firstLocation = new PointF(8f, 8f);
                            using (Graphics graphics = Graphics.FromImage(asientoimg))
                            {
                                using (Font arialFont = new Font("Arial", 10))
                                {
                                    graphics.DrawString(firstText, arialFont, Brushes.Black, firstLocation);
                                }

                            }
                        }
                        autobus.Rows[fila].Cells[columna].Value = asientoimg;

                    }
                    if (tipo == "Pantalla")
                    {

                        autobus.Rows[fila].Cells[columna].Value = imagenpantalla;

                    }
                    if (tipo == "")
                    {

                        autobus.Rows[fila].Cells[columna].Value = imagenpasillo;

                    }
                    if (tipo == "escalera")
                    {

                        autobus.Rows[fila].Cells[columna].Value = imagenescalera;

                    }
                    if (tipo == "baño")
                    {

                        autobus.Rows[fila].Cells[columna].Value = imagenbaño;

                    }

                }
            
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "contenidoautobus";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }

        private void llenarcomobotipo()
        {

            try
            {
                comboBoxfilas.Items.Add("1");
                comboBoxfilas.Items.Add("2");
                comboBoxfilas.Items.Add("3");
                comboBoxfilas.Items.Add("4");
                comboBoxfilas.Items.Add("5");
                comboBoxfilas.Items.Add("6");
                comboBoxfilas.Items.Add("7");
                comboBoxfilas.Items.Add("8");
                comboBoxfilas.Items.Add("9");
                comboBoxfilas.Items.Add("10");
                comboBoxfilas.Items.Add("11");
                comboBoxfilas.Items.Add("12");
                comboBoxfilas.Items.Add("13");
                comboBoxfilas.Items.Add("14");
                comboBoxfilas.Items.Add("15");
                comboBoxfilas.Items.Add("16");
                comboBoxfilas.Items.Add("17");

                comboBoxtipo.Items.Add("Asiento");
                comboBoxtipo.Items.Add("Baño");
                comboBoxtipo.Items.Add("Escalera");
                comboBoxtipo.Items.Add("Nada");
                comboBoxtipo.Items.Add("Asiento con pantalla");
                comboBoxtipo.Items.Add("Pantalla");
                comboBoxtipo.SelectedItem = "Asiento";
                pisos.Items.Add("1");
                pisos.Items.Add("2");


            }

            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "llenarcombotipo";
                Utilerias.LOG.write(_clase, funcion, error);


            }



        }

        private void Buttonlimpiar_Click(object sender, EventArgs e)
        {
            PISO2.Rows.Clear();
            autobus.Rows.Clear();
            comboBoxtipo.Items.Clear();

            getRows();
            comboBoxfilas.Items.Clear();
            pisos.Items.Clear();
            pisos.Text = "";
            comboBoxtipo.Text = "";
            llenarcomobotipo();
            buttonAGREGAR.BackColor = Color.FromArgb(38, 45, 53);

            buttonborrar.BackColor = Color.White;
            buttonborrar.Enabled = false;
            buttonAGREGAR.Enabled = true;
            pictureBox1.Visible = false;
            pictureBox11.Visible = false;
            pictureBox2.Visible = false;
            pictureBox22.Visible = false;
            autobus.Visible = false;
            PISO2.Visible = false;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            try
            {
                Form mensaje = new Mensaje("¿Está seguro de eliminar el autobus?", false);

                DialogResult resut = mensaje.ShowDialog();
                if (resut == DialogResult.OK)
                { 
                    string aut = dataAUTOBUSES.Rows[n].Cells[1].Value.ToString();
                    string sql;
                    sql = "DELETE FROM TIPOAUTOBUS  WHERE PK = @PK";
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@PK", pkid);

                    if (db.execute())
                    {
                        Utilerias.LOG.acciones("BORRO UN AUTOBUS  " + aut);


                    }


                    sql = "DELETE FROM DESCTIPOAUTOBUS  WHERE PKTIPOAUTOBUS = @PK";
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@PK", pkid);

                    if (db.execute())
                    {

                        getRows();
                        PISO2.Rows.Clear();
                        PISO2.Visible = false;
                        autobus.Rows.Clear();
                        buttonborrar.Enabled = false;
                        buttonAGREGAR.Enabled = true;
                        pictureBox1.Visible = false;
                        pictureBox11.Visible = false;
                        pictureBox2.Visible = false;
                        pictureBox22.Visible = false;
                    }
                    else
                    {
                        Form mensajej = new Mensaje("No se pudo eliminar el registro", true);

                        mensajej.ShowDialog();
                                     }
                }

            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "eliminar";
                Utilerias.LOG.write(_clase, funcion, error);

            }


        }
        private void toolStripCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripSessionClose_Click(object sender, EventArgs e)
        {
            try
            {

                for (int i = Application.OpenForms.Count - 1; i >= 0; i--)
                {
                    if (Application.OpenForms[i].Name != "Main")
                        Application.OpenForms[i].Close();
                }

              //  Program.Form.DesactivarMenu();

                Form form = new Login();
                form.MdiParent = this.MdiParent;
                form.StartPosition = FormStartPosition.CenterScreen;
                form.Show();
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "closestrip";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }

        private void Autobusdis_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;

        }

        private void Autobusdis_Shown(object sender, EventArgs e)
        {
            db = new database();

            labelcantidad.Visible = false;
            labelnombre.Visible = false;
            llenarcomobotipo();
            dataAUTOBUSES.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkBlue;
            dataAUTOBUSES.EnableHeadersVisualStyles = false;
            getRows();
            PISO2.Visible = false;
            buttonborrar.Enabled = false;
            autobus.Visible = false;
            pictureBox1.Visible = false;
            pictureBox11.Visible = false;
            pictureBox2.Visible = false;
            pictureBox22.Visible = false;
            comboBoxfilas.DropDownStyle = ComboBoxStyle.DropDownList;

            comboBoxtipo.DropDownStyle = ComboBoxStyle.DropDownList;

            pisos.DropDownStyle = ComboBoxStyle.DropDownList;

            buttonborrar.BackColor = Color.White;
            permisos();
            DoubleBufferedd(autobus, true);
            DoubleBufferedd(PISO2, true);

        }
        private void DoubleBufferedd(DataGridView dgv, bool setting)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, setting, null);
        }

        private void ToolStripExportExcel_Click(object sender, EventArgs e)
        {
            Utilerias.Utilerias.ExportarDataGridViewExcel(dataAUTOBUSES);

        }

        private void BtnMinimizar_Click(object sender, EventArgs e)
        {

        }

        private void BtnNormal_Click(object sender, EventArgs e)
        {

            this.WindowState = FormWindowState.Normal;

            btnNormal.Visible = false;
            btnMaximizar.Visible = true;
        }

        private void BtnMaximizar_Click(object sender, EventArgs e)
        {

            this.WindowState = FormWindowState.Maximized;


            btnMaximizar.Visible = false;
            btnNormal.Visible = true;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Timer2_Tick(object sender, EventArgs e)
        {

            lblHora.Text = DateTime.Now.ToString("hh:mm:ss");
            lbFecha.Text = DateTime.Now.ToString("D");
        }

        private void PanelBarraTitulo_MouseDown(object sender, MouseEventArgs e)
        {
            moverForm();
        }

        //
        // Declaraciones del API de Windows (y constantes usadas para mover el form)
        //
        const int WM_SYSCOMMAND = 0x112;
        const int MOUSE_MOVE = 0xF012;
        //
        // Declaraciones del API
        [System.Runtime.InteropServices.DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        //
        [System.Runtime.InteropServices.DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);
        //
        // función privada usada para mover el formulario actual
        private void moverForm()
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0); ReleaseCapture();

        }

        private void PISO2_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
        }
    }


}
