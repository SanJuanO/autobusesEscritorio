using Autobuses.Utilerias;
using ConnectDB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Autobuses.Planeacion
{
    public partial class cambio_de_autobus : Form
    {
        public database db;
        ResultSet res = null;
        private string digitostarjeta;
        private string[,] matrizpiso1;
        private string[,] temp1;
        private string[,] temp2;
        byte[] fingerPrint;
        private int filatemp =100;
        private int columnatemp = 100;
        private string[,] pkpiso1;
        private string[,] pkpiso2;
        private string[,] matrizpiso2;
        private string[,] matrizstatus;
        private string[,] matrizstatus2;
        private string[,] matriztipo;
        private string[,] matriztipo2;
        private string[,] matriztipop2;
        private string[,] matriztipo2p2;
        private string[,] matrizvendedor;
        private string[,] matrizvendedor2;
        private string ECO;
        private string contraseña = "";
        private string pkcorrida;


        private string PKCONDUCTOR;
        private string  CONDUCTOR;

        private int contando = 0;
        private bool cancel = false;
        private string ECONUEVO;
        private int asientos;
        private string[,] etiqueta2;
        private string[,] etiqueta2piso2;

        List<int> fila = new List<int>();
        List<int> columna = new List<int>();




        private string tipoautobus;
        private string _clase = "cambiodeautobus";

        private string fechaa = DateTime.Now.ToString("yyyy-MM-dd");

        private Bitmap imagen;
        private Bitmap pantalla = new Bitmap(Autobuses.Properties.Resources.pantalla);

        private Bitmap imagenansiento = new Bitmap(Autobuses.Properties.Resources.asientobase);
        private Bitmap imagenrojo = new Bitmap(Autobuses.Properties.Resources.asientobase);
        private Bitmap imagenamarrillo = new Bitmap(Autobuses.Properties.Resources.amarrillo);
        private Bitmap imagenazul = new Bitmap(Autobuses.Properties.Resources.AZUL);

        private Bitmap imagenansientopantalla = new Bitmap(Autobuses.Properties.Resources.asientopantall1);

        private Bitmap imagenpasillo = new Bitmap(Autobuses.Properties.Resources.pasillo);
        private Bitmap imagenbaño = new Bitmap(Autobuses.Properties.Resources.BAN1);
        private Bitmap imagenescalera = new Bitmap(Autobuses.Properties.Resources.ESCALERAS);
        private Bitmap imagensplash = new Bitmap(Autobuses.Properties.Resources.logotickets);
        private Bitmap niño = new Bitmap(Autobuses.Properties.Resources.niño);
        private Bitmap estudiantes = new Bitmap(Autobuses.Properties.Resources.estudiantes);
        private Bitmap inapam = new Bitmap(Autobuses.Properties.Resources.inapam);
        private Bitmap pasedecortesia = new Bitmap(Autobuses.Properties.Resources.pasedecortecia);
        private Bitmap imagabordo = new Bitmap(Autobuses.Properties.Resources.abord1);
        public cambio_de_autobus()
        {

            InitializeComponent();
            this.Show();
        }

        private void Cambio_de_autobus_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;

        }
        private void supercontra()
        {
            try
            {
                string sql = "SELECT VALOR FROM VARIABLES WHERE NOMBRE='CONTRASEÑA'";


                db.PreparedSQL(sql);
                res = db.getTable();
                if (res.Next())
                {
                    contraseña = res.Get("VALOR");

                }

            }

            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "supercontra";
                Utilerias.LOG.write(_clase, funcion, error);

            }
        }

        private void llenarcomboboxeco()
        {
            try
            {
                comboviejo.Items.Clear();

                string sql = "SELECT VE.ECO,VE.PKCORRIDA,CO.PK,CO.AUTOBUS FROM VENDIDOS VE " +
                "INNER JOIN VCORRIDAS_DIA_1 CO ON(CO.PK= VE.PKCORRIDA AND NOT CO.AUTOBUS = VE.ECO) " +
                "WHERE  VE.STATUS='VENDIDO'  GROUP BY VE.ECO,VE.PKCORRIDA,CO.PK,CO.AUTOBUS ";
                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@FECHA", DateTime.Now.ToString("dd/MM/yyyy"));

                res = db.getTable();

                while (res.Next())
                {
                    ComboboxItem item = new ComboboxItem();
                    item.Text = res.Get("ECO");
                    item.Value = res.Get("PKCORRIDA");

                    comboviejo.Items.Add(item);

                    //MessageBox.Show((comboBoxRole.SelectedItem as ComboboxItem).Value.ToString());
                }


            }
            catch (Exception err)
            {
                string error = err.Message;
                checkInternetAvaible();
                MessageBox.Show("Ocurrio un Error al obtener información.");
                string funcion = "LLENARCOMBOECO";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }

        private void llenarcomboboxeco2()
        {
            try
            {
                comboviejo.Items.Clear();

                string sql = "select ECO from VENDIDOS where FECHA=@FECHA GROUP BY ECO";
                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@FECHA", DateTime.Now.ToString("dd/MM/yyyy"));

                res = db.getTable();

                while (res.Next())
                {
                    ComboboxItem item = new ComboboxItem();
                    item.Text = res.Get("ECO");
                    comboviejo.Items.Add(item);

                    //MessageBox.Show((comboBoxRole.SelectedItem as ComboboxItem).Value.ToString());
                }


            }
            catch (Exception err)
            {
                string error = err.Message;
                checkInternetAvaible();
                MessageBox.Show("Ocurrio un Error al obtener información.");
                string funcion = "LLENARCOMBOECO";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }

        private void asientoconpantalla2(Color col, string asignacion, int fila, int columna, string tipo, bool pi)
        {
            Bitmap asientoimg = new Bitmap(imagenansientopantalla);

            for (int Xcount = 5; Xcount < 15; Xcount++)
            {
                for (int Ycount = 9; Ycount < 43; Ycount++)
                {
                    asientoimg.SetPixel(Xcount, Ycount, col);
                }
            }
            for (int Xcount = 35; Xcount < 45; Xcount++)
            {
                for (int Ycount = 9; Ycount < 43; Ycount++)
                {
                    asientoimg.SetPixel(Xcount, Ycount, col);
                }
            }
            for (int Xcount = 1; Xcount < 49; Xcount++)
            {
                for (int Ycount = 9; Ycount < 22; Ycount++)
                {
                    asientoimg.SetPixel(Xcount, Ycount, col);
                }
            }
            for (int Xcount = 15; Xcount < 35; Xcount++)
            {
                for (int Ycount = 9; Ycount < 40; Ycount++)
                {
                    asientoimg.SetPixel(Xcount, Ycount, col);
                }
            }
            string firstText = asignacion;
            PointF firstLocation = new PointF(4f, 8f);
            using (Graphics graphics = Graphics.FromImage(asientoimg))
            {
                using (Font arialFont = new Font("Arial", 9))
                {
                    graphics.DrawString(firstText, arialFont, Brushes.White, firstLocation);
                }

            }
            if (pi)
            {
                autobus2.Rows[fila].Cells[columna].Value = asientoimg;
                etiqueta2[fila, columna] = asignacion;
                matriztipop2[fila, columna] = tipo;

            }
            else
            {
                PISOS22.Rows[fila].Cells[columna].Value = asientoimg;
                etiqueta2piso2[fila, columna] = asignacion;

                matriztipo2p2[fila, columna] = tipo;
            }
        }

        private void asientoconpantalla(Color col, string asignacion, int fila, int columna, string tipo, bool pi)
        {
            Bitmap asientoimg = new Bitmap(imagenansientopantalla);

            for (int Xcount = 5; Xcount < 15; Xcount++)
            {
                for (int Ycount = 9; Ycount < 43; Ycount++)
                {
                    asientoimg.SetPixel(Xcount, Ycount, col);
                }
            }
            for (int Xcount = 35; Xcount < 45; Xcount++)
            {
                for (int Ycount = 9; Ycount < 43; Ycount++)
                {
                    asientoimg.SetPixel(Xcount, Ycount, col);
                }
            }
            for (int Xcount = 1; Xcount < 49; Xcount++)
            {
                for (int Ycount = 9; Ycount < 22; Ycount++)
                {
                    asientoimg.SetPixel(Xcount, Ycount, col);
                }
            }
            for (int Xcount = 15; Xcount < 35; Xcount++)
            {
                for (int Ycount = 9; Ycount < 40; Ycount++)
                {
                    asientoimg.SetPixel(Xcount, Ycount, col);
                }
            }
            string firstText = asignacion;
            PointF firstLocation = new PointF(4f, 8f);
            using (Graphics graphics = Graphics.FromImage(asientoimg))
            {
                using (Font arialFont = new Font("Arial", 9))
                {
                    graphics.DrawString(firstText, arialFont, Brushes.White, firstLocation);
                }

            }
            if (pi)
            {
                autobus.Rows[fila].Cells[columna].Value = asientoimg;
                matrizpiso1[fila, columna] = asignacion;
                matriztipo[fila, columna] = tipo;
                matrizstatus[fila, columna] = "";

            }
            else
            {
                PISOS2.Rows[fila].Cells[columna].Value = asientoimg;
                matrizpiso2[fila, columna] = asignacion;
                matrizstatus2[fila, columna] = "";
                matriztipo2[fila, columna] = tipo;
            }
        }


        private void llenarautobus()
        {
            try



            {

                autobus.Rows.Clear();
                PISOS2.Rows.Clear();



                string sql = "SELECT * FROM DESCTIPOAUTOBUS WHERE PKTIPOAUTOBUS=(select TIPO_PK FROM AUTOBUSES WHERE ECO=@ECO) order by fila";
                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@ECO", ECO);


                res = db.getTable();
                int fil = 0;
                int fil2 = 0;
                while (res.Next())
                {
                    tipoautobus = res.Get("PKTIPOAUTOBUS");
                    string pi = res.Get("PISOS");
                    int fila = res.GetInt("FILA");
                    int columna = res.GetInt("COLUMNA");
                    string tipo = res.Get("OBJETO");
                    string asignacion = res.Get("ETIQUETA");

                    if (pi == "1")
                    {
                        a.Visible = true;
                        b.Visible = true;
                        autobus.Visible = true;

                        if (fila == fil)
                        {
                            autobus.Rows.Add();
                            fil = fil + 1;
                        }
                        if (tipo == "asiento con pantalla")
                        {

                            if (asignacion != "")
                            {
                                Color col = Color.Green;
                                asientoconpantalla(col, asignacion, fila, columna, tipo, true);
                            }
                        }
                        if (tipo == "asiento")
                        {

                            Bitmap asientoimg = new Bitmap(imagenansiento);
                            if (asignacion != "")
                            {
                                for (int Xcount = 5; Xcount < 15; Xcount++)
                                {
                                    for (int Ycount = 0; Ycount < 43; Ycount++)
                                    {
                                        asientoimg.SetPixel(Xcount, Ycount, Color.Green);
                                    }
                                }
                                for (int Xcount = 35; Xcount < 45; Xcount++)
                                {
                                    for (int Ycount = 0; Ycount < 43; Ycount++)
                                    {
                                        asientoimg.SetPixel(Xcount, Ycount, Color.Green);
                                    }
                                }
                                for (int Xcount = 1; Xcount < 49; Xcount++)
                                {
                                    for (int Ycount = 0; Ycount < 22; Ycount++)
                                    {
                                        asientoimg.SetPixel(Xcount, Ycount, Color.Green);
                                    }
                                }
                                for (int Xcount = 15; Xcount < 35; Xcount++)
                                {
                                    for (int Ycount = 0; Ycount < 40; Ycount++)
                                    {
                                        asientoimg.SetPixel(Xcount, Ycount, Color.Green);
                                    }
                                }
                                string firstText = asignacion;
                                PointF firstLocation = new PointF(4f, 8f);
                                using (Graphics graphics = Graphics.FromImage(asientoimg))
                                {
                                    using (Font arialFont = new Font("Arial", 9))
                                    {
                                        graphics.DrawString(firstText, arialFont, Brushes.White, firstLocation);
                                    }

                                }
                            }
                            autobus.Rows[fila].Cells[columna].Value = asientoimg;
                            matrizpiso1[fila, columna] = asignacion;
                            matrizstatus[fila, columna] = "";

                        }

                        if (tipo == "")
                        {

                            autobus.Rows[fila].Cells[columna].Value = imagenpasillo;
                            matrizpiso1[fila, columna] = asignacion;


                        }
                        if (tipo == "escalera")
                        {

                            autobus.Rows[fila].Cells[columna].Value = imagenescalera;
                            matrizpiso1[fila, columna] = asignacion;


                        }
                        if (tipo == "baño")
                        {

                            autobus.Rows[fila].Cells[columna].Value = imagenbaño;
                            matrizpiso1[fila, columna] = asignacion;


                        }

                    }
                    if (pi == "2")
                    {
                        c.Visible = true;
                        d.Visible = true;
                        PISOS2.Visible = true;
                        if (fila == fil2)
                        {
                            PISOS2.Rows.Add();
                            fil2 = fil2 + 1;
                        }

                        if (tipo == "asiento con pantalla")
                        {

                            if (asignacion != "")
                            {
                                Color col = Color.Green;
                                asientoconpantalla(col, asignacion, fila, columna, tipo, false);
                            }
                        }
                        if (tipo == "asiento")
                        {

                            Bitmap asientoimg = new Bitmap(imagenansiento);
                            if (asignacion != "")
                            {
                                for (int Xcount = 5; Xcount < 15; Xcount++)
                                {
                                    for (int Ycount = 0; Ycount < 43; Ycount++)
                                    {
                                        asientoimg.SetPixel(Xcount, Ycount, Color.Green);
                                    }
                                }
                                for (int Xcount = 35; Xcount < 45; Xcount++)
                                {
                                    for (int Ycount = 0; Ycount < 43; Ycount++)
                                    {
                                        asientoimg.SetPixel(Xcount, Ycount, Color.Green);
                                    }
                                }
                                for (int Xcount = 1; Xcount < 49; Xcount++)
                                {
                                    for (int Ycount = 0; Ycount < 22; Ycount++)
                                    {
                                        asientoimg.SetPixel(Xcount, Ycount, Color.Green);
                                    }
                                }
                                for (int Xcount = 15; Xcount < 35; Xcount++)
                                {
                                    for (int Ycount = 0; Ycount < 40; Ycount++)
                                    {
                                        asientoimg.SetPixel(Xcount, Ycount, Color.Green);
                                    }
                                }
                                string firstText = asignacion;
                                PointF firstLocation = new PointF(4f, 8f);
                                using (Graphics graphics = Graphics.FromImage(asientoimg))
                                {
                                    using (Font arialFont = new Font("Arial", 9))
                                    {
                                        graphics.DrawString(firstText, arialFont, Brushes.White, firstLocation);
                                    }

                                }
                            }



                            PISOS2.Rows[fila].Cells[columna].Value = asientoimg;
                            matrizpiso2[fila, columna] = asignacion;
                            matrizstatus2[fila, columna] = "";


                        }

                        if (tipo == "")
                        {

                            PISOS2.Rows[fila].Cells[columna].Value = imagenpasillo;
                            matrizpiso2[fila, columna] = asignacion;

                        }
                        if (tipo == "escalera")
                        {

                            PISOS2.Rows[fila].Cells[columna].Value = imagenescalera;
                            matrizpiso2[fila, columna] = asignacion;


                        }
                        if (tipo == "baño")
                        {

                            PISOS2.Rows[fila].Cells[columna].Value = imagenbaño;
                            matrizpiso2[fila, columna] = asignacion;


                        }

                    }

                }

                getRowsVendidos2();

            }
            catch (Exception err)
            {
                string error = err.Message;
                checkInternetAvaible();
                MessageBox.Show("Ocurrio un Error, al obtener información.");
                string funcion = "llenarautobus";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }

        private void asientopantalla(Color col, string asi, string ven, string st, bool donde, int filaa, int columnaa)
        {
            Bitmap asientoimg = new Bitmap(imagenansientopantalla);

            for (int Xcount = 5; Xcount < 15; Xcount++)
            {
                for (int Ycount = 9; Ycount < 43; Ycount++)
                {
                    asientoimg.SetPixel(Xcount, Ycount, col);
                }
            }
            for (int Xcount = 35; Xcount < 45; Xcount++)
            {
                for (int Ycount = 9; Ycount < 43; Ycount++)
                {
                    asientoimg.SetPixel(Xcount, Ycount, col);
                }
            }
            for (int Xcount = 1; Xcount < 49; Xcount++)
            {
                for (int Ycount = 9; Ycount < 22; Ycount++)
                {
                    asientoimg.SetPixel(Xcount, Ycount, col);
                }
            }
            for (int Xcount = 15; Xcount < 35; Xcount++)
            {
                for (int Ycount = 9; Ycount < 40; Ycount++)
                {
                    asientoimg.SetPixel(Xcount, Ycount, col);
                }
            }
            string texto = asi;
            string firstText = texto;
            PointF firstLocation = new PointF(4f, 8f);
            using (Graphics graphics = Graphics.FromImage(asientoimg))
            {
                using (Font arialFont = new Font("Arial", 9))
                {
                    graphics.DrawString(firstText, arialFont, Brushes.White, firstLocation);
                }
            }

            if (donde)
            {
                autobus.Rows[filaa].Cells[columnaa].Value = asientoimg;
                matrizstatus[filaa, columnaa] = st;
                matrizvendedor[filaa, columnaa] = ven;
                fila.Add(filaa);
                columna.Add(columnaa);
            }
            else
            {
                PISOS2.Rows[filaa].Cells[columnaa].Value = asientoimg;
                matrizstatus2[filaa, columnaa] = st;
                matrizvendedor2[filaa, columnaa] = ven;
                fila.Add(filaa);
                columna.Add(columnaa);
            }
        }

        private void asientopintar(Color col, string asi, string ven, string st, bool donde, int filaa, int columnaa)
        {
            Bitmap asientoimg = new Bitmap(imagenansiento);

            for (int Xcount = 5; Xcount < 15; Xcount++)
            {
                for (int Ycount = 0; Ycount < 43; Ycount++)
                {
                    asientoimg.SetPixel(Xcount, Ycount, col);
                }
            }
            for (int Xcount = 35; Xcount < 45; Xcount++)
            {
                for (int Ycount = 0; Ycount < 43; Ycount++)
                {
                    asientoimg.SetPixel(Xcount, Ycount, col);
                }
            }
            for (int Xcount = 1; Xcount < 49; Xcount++)
            {
                for (int Ycount = 0; Ycount < 22; Ycount++)
                {
                    asientoimg.SetPixel(Xcount, Ycount, col);
                }
            }
            for (int Xcount = 15; Xcount < 35; Xcount++)
            {
                for (int Ycount = 0; Ycount < 40; Ycount++)
                {
                    asientoimg.SetPixel(Xcount, Ycount, col);
                }
            }


            string texto = asi;
            string firstText = texto;
            PointF firstLocation = new PointF(4f, 8f);
            using (Graphics graphics = Graphics.FromImage(asientoimg))
            {
                using (Font arialFont = new Font("Arial", 9))
                {
                    graphics.DrawString(firstText, arialFont, Brushes.White, firstLocation);
                }
            }
            if (donde)
            {
                autobus.Rows[filaa].Cells[columnaa].Value = asientoimg;
                matrizstatus[filaa, columnaa] = st;
                matrizvendedor[filaa, columnaa] = ven;
                fila.Add(filaa);
                columna.Add(columnaa);
            }
            else
            {
                PISOS2.Rows[filaa].Cells[columnaa].Value = asientoimg;
                matrizstatus2[filaa, columnaa] = st;
                matrizvendedor2[filaa, columnaa] = ven;
                fila.Add(filaa);
                columna.Add(columnaa);
            }
        }


        public void getRowsVendidos2()
        {
            try


            {
                //datagridvendidos.Rows.Clear();
                string sql = "SELECT TARIFA,STATUS,VENDEDOR,FILA,COLUMNA,ASIENTO,PISOS,SUCURSAL,COLOR,TIPO " +
                    "FROM VISTAVENDIDOSCOLOR  WHERE  ECO=@ECO AND PKCORRIDA=@PKCORRIDA AND NOT ASIENTO='PIE' ORDER BY FECHAC ASC";
                db.PreparedSQL(sql);

                db.command.Parameters.AddWithValue("@ECO", ECO);
                //db.command.Parameters.AddWithValue("@FECHA", DateTime.Now.ToString("dd/MM/yyyy"));
                db.command.Parameters.AddWithValue("@PKCORRIDA", pkcorrida);


                res = db.getTable();


                while (res.Next())
                {

                    string tipopasaje = res.Get("TARIFA");
                    string st = res.Get("STATUS");
                    string ven = res.Get("VENDEDOR");
                    int _fila = res.GetInt("FILA");
                    int _columna = res.GetInt("COLUMNA");
                    string asi = res.Get("ASIENTO");
                    string pis = res.Get("PISOS");
                    string abord = res.Get("SUCURSAL");
                    string color = res.Get("COLOR");
                    string tip = res.Get("TIPO");
                    Color col = Color.FromName(color);
                    if(_fila == 100)
                    {

                    }
                   else if (pis == "1")
                    {


                        if (st == "VENDIDO")
                        {
                            if (tip == "asiento")
                                asientopintar(col, asi, ven, st, true, _fila, _columna);
                            if(tip=="asientopantalla")
                                asientopantalla(col, asi, ven, st, true, _fila, _columna);



                        }



                    }

                   else if (pis == "2")
                    {

                        if (st == "VENDIDO")
                        {
                            if (tip == "asiento")
                                asientopintar(col, asi, ven, st, false, _fila, _columna);
                            if (tip == "asientopantalla")
                                asientopantalla(col, asi, ven, st, false, _fila, _columna);


                        }
                    }



                }



            }
            catch (Exception err)
            {
                string error = err.Message;

                MessageBox.Show("Ocurrio un Error, verifique autobuses o que la corrida tenga autobus");
                checkInternetAvaible();
                string funcion = "getRowsvendidos";
                Utilerias.LOG.write(_clase, funcion, error);


            }

        }

        private void asientopantalla2(Color col, string asi, string ven, string st, bool donde, int filaa, int columnaa)
        {
            Bitmap asientoimg = new Bitmap(imagenansientopantalla);

            for (int Xcount = 5; Xcount < 15; Xcount++)
            {
                for (int Ycount = 9; Ycount < 43; Ycount++)
                {
                    asientoimg.SetPixel(Xcount, Ycount, col);
                }
            }
            for (int Xcount = 35; Xcount < 45; Xcount++)
            {
                for (int Ycount = 9; Ycount < 43; Ycount++)
                {
                    asientoimg.SetPixel(Xcount, Ycount, col);
                }
            }
            for (int Xcount = 1; Xcount < 49; Xcount++)
            {
                for (int Ycount = 9; Ycount < 22; Ycount++)
                {
                    asientoimg.SetPixel(Xcount, Ycount, col);
                }
            }
            for (int Xcount = 15; Xcount < 35; Xcount++)
            {
                for (int Ycount = 9; Ycount < 40; Ycount++)
                {
                    asientoimg.SetPixel(Xcount, Ycount, col);
                }
            }
            string texto = asi;
            string firstText = texto;
            PointF firstLocation = new PointF(4f, 8f);
            using (Graphics graphics = Graphics.FromImage(asientoimg))
            {
                using (Font arialFont = new Font("Arial", 9))
                {
                    graphics.DrawString(firstText, arialFont, Brushes.White, firstLocation);
                }
            }

            if (donde)
            {
                autobus2.Rows[filaa].Cells[columnaa].Value = asientoimg;

            }
            else
            {
                PISOS22.Rows[filaa].Cells[columnaa].Value = asientoimg;

            }
        }

        private void asientopintar2(Color col, string asi, string ven, string st, bool donde, int filaa, int columnaa)
        {
            Bitmap asientoimg = new Bitmap(imagenansiento);

            for (int Xcount = 5; Xcount < 15; Xcount++)
            {
                for (int Ycount = 0; Ycount < 43; Ycount++)
                {
                    asientoimg.SetPixel(Xcount, Ycount, col);
                }
            }
            for (int Xcount = 35; Xcount < 45; Xcount++)
            {
                for (int Ycount = 0; Ycount < 43; Ycount++)
                {
                    asientoimg.SetPixel(Xcount, Ycount, col);
                }
            }
            for (int Xcount = 1; Xcount < 49; Xcount++)
            {
                for (int Ycount = 0; Ycount < 22; Ycount++)
                {
                    asientoimg.SetPixel(Xcount, Ycount, col);
                }
            }
            for (int Xcount = 15; Xcount < 35; Xcount++)
            {
                for (int Ycount = 0; Ycount < 40; Ycount++)
                {
                    asientoimg.SetPixel(Xcount, Ycount, col);
                }
            }


            string texto = asi;
            string firstText = texto;
            PointF firstLocation = new PointF(4f, 8f);
            using (Graphics graphics = Graphics.FromImage(asientoimg))
            {
                using (Font arialFont = new Font("Arial", 9))
                {
                    graphics.DrawString(firstText, arialFont, Brushes.White, firstLocation);
                }
            }
            if (donde)
            {
                autobus2.Rows[filaa].Cells[columnaa].Value = asientoimg;


            }
            else
            {
                PISOS22.Rows[filaa].Cells[columnaa].Value = asientoimg;


            }
        }



        public void getRowsVendidos22()
        {
            try


            {
                //datagridvendidos.Rows.Clear();
                string sql = "SELECT TARIFA,STATUS,VENDEDOR,FILA,COLUMNA,ASIENTO,PISOS,SUCURSAL,COLOR,TIPO " +
                    "FROM VISTAVENDIDOSCOLOR  WHERE ECO=@ECO AND PKCORRIDA=@PKCORRIDA AND NOT ASIENTO='PIE' ORDER BY FECHAC ASC";
                db.PreparedSQL(sql);

                db.command.Parameters.AddWithValue("@ECO", ECONUEVO);
                db.command.Parameters.AddWithValue("@FECHA", DateTime.Now.ToString("dd/MM/yyyy"));
                db.command.Parameters.AddWithValue("@PKCORRIDA", pkcorrida);




                int n = 0;


                res = db.getTable();


                while (res.Next())
                {

                    string tipopasaje = res.Get("TARIFA");
                    string st = res.Get("STATUS");
                    string ven = res.Get("VENDEDOR");
                    int _fila = res.GetInt("FILA");
                    int _columna = res.GetInt("COLUMNA");
                    string asi = res.Get("ASIENTO");
                    string pis = res.Get("PISOS");
                    string abord = res.Get("SUCURSAL");
                    string color = res.Get("COLOR");
                    Color col = Color.FromName(color);
                    string tip = res.Get("TIPO");


                    if (pis == "1")
                    {


                        if (st == "VENDIDO")
                        {
                            if (tip == "asiento")
                                asientopintar2(col, asi, ven, st, true, _fila, _columna);
                            else
                                asientopantalla2(col, asi, ven, st, true, _fila, _columna);


                        }





                    }

                    if (pis == "2")
                    {

                        if (st == "VENDIDO")
                        {
                            if (tip == "asiento")
                                asientopintar2(col, asi, ven, st, false, _fila, _columna);
                            else
                                asientopantalla2(col, asi, ven, st, false, _fila, _columna);


                        }
                    }



                }

                db.execute();


            }
            catch (Exception err)
            {
                string error = err.Message;

                MessageBox.Show("Ocurrio un Error, verifique autobuses o que la corrida tenga autobus");
                checkInternetAvaible();
                string funcion = "getRowsvendidos";
                Utilerias.LOG.write(_clase, funcion, error);


            }

        }

        public void checkInternetAvaible()
        {
            if (!Utilerias.Utilerias.CheckForInternetConnection())
            {
                MessageBox.Show("Error al conectarse a internet");
                return;
            }
        }

        private void Cambio_de_autobus_Shown(object sender, EventArgs e)
        {
            db = new database();
            matrizpiso1 = new string[18, 5];
            comboviejo.DropDownStyle = ComboBoxStyle.DropDownList;

            matrizpiso2 = new string[18, 5];
            matrizstatus = new string[18, 5];
            temp1 = new string[18, 5];
            temp2 = new string[18, 5];
            matrizstatus2 = new string[18, 5];
            matrizvendedor = new string[18, 5];
            matrizvendedor2 = new string[18, 5];
            matriztipo = new string[18, 5];
            matriztipo2 = new string[18, 5];
            matriztipop2 = new string[18, 5];
            matriztipo2p2 = new string[18, 5];
            etiqueta2 = new string[18, 5];
            etiqueta2piso2 = new string[18, 5];
            llenarcomboboxeco();

            a.Visible = false;
            b.Visible = false;
            c.Visible = false;
            d.Visible = false;
            autobus.Visible = false;
            PISOS2.Visible = false;
            a2.Visible = false;
            b2.Visible = false;
            c2.Visible = false;
            d2.Visible = false;
            autobus2.Visible = false;
            PISOS22.Visible = false;
            textBoxasiento.Text = "";
            supercontra();
            ValidateUser();
        }

        void ValidateUser()
        {


            try
            {
                string cadenaDeConexion = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;
                using (SqlConnection cn = new SqlConnection(cadenaDeConexion))
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT HUELLA FROM USUARIOS WHERE USUARIO='" + LoginInfo.UserID + "'", cn);
                    fingerPrint = (byte[])cmd.ExecuteScalar();
                    verificationUserControl2.Samples.Add(new DPFP.Template(new MemoryStream(fingerPrint)), null);
                }
            }
            catch (Exception err)
            {
                string error = err.Message;

                if (!Utilerias.Utilerias.CheckForInternetConnection())
                {
                    Form mensaje = new Mensaje("Sin internet", true);
                    mensaje.ShowDialog();
                }
                else
                    MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "validaruser";
                Utilerias.LOG.write(_clase, funcion, error);


            }

        }


        private void Comboviejo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ECO = comboviejo.SelectedItem.ToString();
            a2.Visible = false;
            b2.Visible = false;
            c2.Visible = false;
            d2.Visible = false;
            autobus2.Visible = false;
            PISOS22.Visible = false;

            pkcorrida = (comboviejo.SelectedItem as ComboboxItem).Value.ToString();
            llenarautobus();

            string sql = "SELECT v.AUTOBUS, AU.PK_CHOFER,AU.CHOFER FROM VCORRIDAS_DIA_1 as v" +
                " INNER JOIN VAUTOBUSES AU ON(V.AUTOBUS = AU.ECO) WHERE V.PK = @PKCORRIDA";
            db.PreparedSQL(sql);
            db.command.Parameters.AddWithValue("@FECHA", DateTime.Now.ToString("yyyy-MM-dd"));
            db.command.Parameters.AddWithValue("@PKCORRIDA", pkcorrida);

            res = db.getTable();

            if (res.Next())
            {
                ComboboxItem item = new ComboboxItem();
                textboxnuevo.Text = res.Get("AUTOBUS");
                ECONUEVO = res.Get("AUTOBUS");
                PKCONDUCTOR = res.Get("PK_CHOFER");
                CONDUCTOR = res.Get("CHOFER");

            }
            if (ECO != ECONUEVO)
            {
                asientos = fila.Count;
                llenarautobus2();
                getRowsVendidos22();

            }
            else
                MessageBox.Show("Este autobus no a sido modificado");



        }


        private void llenarautobus2()
        {
            try
            {

                autobus2.Rows.Clear();
                PISOS22.Rows.Clear();



                string sql = "SELECT * FROM DESCTIPOAUTOBUS WHERE PKTIPOAUTOBUS=(select TIPO_PK FROM AUTOBUSES WHERE ECO=@ECO) order by fila";
                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@ECO", ECONUEVO);


                res = db.getTable();
                int fil = 0;
                int fil2 = 0;
                while (res.Next())
                {
                    tipoautobus = res.Get("PKTIPOAUTOBUS");
                    string pi = res.Get("PISOS");
                    int fila = res.GetInt("FILA");
                    int columna = res.GetInt("COLUMNA");
                    string tipo = res.Get("OBJETO");
                    string asignacion = res.Get("ETIQUETA");

                    if (pi == "1")
                    {
                        a2.Visible = true;
                        b2.Visible = true;
                        autobus2.Visible = true;

                        if (fila == fil)
                        {
                            autobus2.Rows.Add();
                            fil = fil + 1;
                        }
                        if (tipo == "asiento con pantalla")
                        {

                            if (asignacion != "")
                            {
                                Color col = Color.Green;
                                asientoconpantalla2(col, asignacion, fila, columna, tipo, true);
                            }
                        }
                        if (tipo == "asiento")
                        {

                            Bitmap asientoimg = new Bitmap(imagenansiento);
                            if (asignacion != "")
                            {
                                for (int Xcount = 5; Xcount < 15; Xcount++)
                                {
                                    for (int Ycount = 0; Ycount < 43; Ycount++)
                                    {
                                        asientoimg.SetPixel(Xcount, Ycount, Color.Green);
                                    }
                                }
                                for (int Xcount = 35; Xcount < 45; Xcount++)
                                {
                                    for (int Ycount = 0; Ycount < 43; Ycount++)
                                    {
                                        asientoimg.SetPixel(Xcount, Ycount, Color.Green);
                                    }
                                }
                                for (int Xcount = 1; Xcount < 49; Xcount++)
                                {
                                    for (int Ycount = 0; Ycount < 22; Ycount++)
                                    {
                                        asientoimg.SetPixel(Xcount, Ycount, Color.Green);
                                    }
                                }
                                for (int Xcount = 15; Xcount < 35; Xcount++)
                                {
                                    for (int Ycount = 0; Ycount < 40; Ycount++)
                                    {
                                        asientoimg.SetPixel(Xcount, Ycount, Color.Green);
                                    }
                                }
                                string firstText = asignacion;
                                PointF firstLocation = new PointF(4f, 8f);
                                using (Graphics graphics = Graphics.FromImage(asientoimg))
                                {
                                    using (Font arialFont = new Font("Arial", 9))
                                    {
                                        graphics.DrawString(firstText, arialFont, Brushes.White, firstLocation);
                                    }

                                }
                            }
                            autobus2.Rows[fila].Cells[columna].Value = asientoimg;
                            etiqueta2[fila, columna] = asignacion;
                            matriztipop2[fila, columna] = tipo;


                        }

                        if (tipo == "")
                        {

                            autobus2.Rows[fila].Cells[columna].Value = imagenpasillo;


                        }
                        if (tipo == "escalera")
                        {

                            autobus2.Rows[fila].Cells[columna].Value = imagenescalera;


                        }
                        if (tipo == "baño")
                        {

                            autobus2.Rows[fila].Cells[columna].Value = imagenbaño;


                        }

                    }
                    if (pi == "2")
                    {
                        c2.Visible = true;
                        d2.Visible = true;
                        PISOS22.Visible = true;
                        if (fila == fil2)
                        {
                            PISOS22.Rows.Add();
                            fil2 = fil2 + 1;
                        }

                        if (tipo == "asiento con pantalla")
                        {

                            if (asignacion != "")
                            {
                                Color col = Color.Green;
                                asientoconpantalla2(col, asignacion, fila, columna, tipo, false);
                            }
                        }
                        if (tipo == "asiento")
                        {

                            Bitmap asientoimg = new Bitmap(imagenansiento);
                            if (asignacion != "")
                            {
                                for (int Xcount = 5; Xcount < 15; Xcount++)
                                {
                                    for (int Ycount = 0; Ycount < 43; Ycount++)
                                    {
                                        asientoimg.SetPixel(Xcount, Ycount, Color.Green);
                                    }
                                }
                                for (int Xcount = 35; Xcount < 45; Xcount++)
                                {
                                    for (int Ycount = 0; Ycount < 43; Ycount++)
                                    {
                                        asientoimg.SetPixel(Xcount, Ycount, Color.Green);
                                    }
                                }
                                for (int Xcount = 1; Xcount < 49; Xcount++)
                                {
                                    for (int Ycount = 0; Ycount < 22; Ycount++)
                                    {
                                        asientoimg.SetPixel(Xcount, Ycount, Color.Green);
                                    }
                                }
                                for (int Xcount = 15; Xcount < 35; Xcount++)
                                {
                                    for (int Ycount = 0; Ycount < 40; Ycount++)
                                    {
                                        asientoimg.SetPixel(Xcount, Ycount, Color.Green);
                                    }
                                }
                                string firstText = asignacion;
                                PointF firstLocation = new PointF(4f, 8f);
                                using (Graphics graphics = Graphics.FromImage(asientoimg))
                                {
                                    using (Font arialFont = new Font("Arial", 9))
                                    {
                                        graphics.DrawString(firstText, arialFont, Brushes.White, firstLocation);
                                    }

                                }
                            }
                            PISOS22.Rows[fila].Cells[columna].Value = asientoimg;
                            etiqueta2piso2[fila, columna] = asignacion;
                            matriztipo2p2[fila, columna] = tipo;


                        }

                        if (tipo == "")
                        {

                            PISOS22.Rows[fila].Cells[columna].Value = imagenpasillo;

                        }

                        if (tipo == "Pantalla")
                        {

                            PISOS22.Rows[fila].Cells[columna].Value = pantalla;

                        }
                        if (tipo == "escalera")
                        {

                            PISOS22.Rows[fila].Cells[columna].Value = imagenescalera;


                        }
                        if (tipo == "baño")
                        {

                            PISOS22.Rows[fila].Cells[columna].Value = imagenbaño;


                        }

                    }

                }


            }
            catch (Exception err)
            {
                string error = err.Message;
                checkInternetAvaible();
                MessageBox.Show("Ocurrio un Error, al obtener información.");
                string funcion = "llenarautobus";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }


   
        private void Autobus2_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (fila.Count > 0 && textBoxasiento.Text != "")
                {

                    int n = e.RowIndex;
                    int c = e.ColumnIndex;

                    string sql2 = "select 1 as RESPUESTA from VENDIDOS where pisos='1' and eco=@ECO2 and fila=@FILA2 and COLUMNA=@COLUMNA2 and ASIENTO=@ETIQUETA AND PKCORRIDA=@PKCORRIDA";
                    db.PreparedSQL(sql2);

                    db.command.Parameters.AddWithValue("@FILA2", n);
                    db.command.Parameters.AddWithValue("@COLUMNA2", c);
                    db.command.Parameters.AddWithValue("@ECO2", ECONUEVO);
                    db.command.Parameters.AddWithValue("@PKCORRIDA", pkcorrida);

                    db.command.Parameters.AddWithValue("@ETIQUETA", etiqueta2[n, c]);
                    int respu = 0;
                    res = db.getTable();

                    if (res.Next())
                    {
                        respu = res.GetInt("RESPUESTA");
                    }

                    if (respu == 0)
                    {
                        string sql = "UPDATE  VENDIDOS SET PISOS='1', PK_CONDUCTOR=@PKCONDUCTOR,CONDUCTOR=@CONDUCTOR, ECO=@ECO2,FILA=@FILA2, CAMBIODEAUTOBUS=1,COLUMNA=@COLUMNA2,ASIENTO=@ETIQUETA,TIPO=@TIPO WHERE ECO=@ECO AND ASIENTO=@ASIENTO AND PKCORRIDA=@PKCORRIDA";

                        db.PreparedSQL(sql);
                        db.command.Parameters.AddWithValue("@ECO", ECO);

                        db.command.Parameters.AddWithValue("@PKCORRIDA", pkcorrida);
                        db.command.Parameters.AddWithValue("@ASIENTO", textBoxasiento.Text);
                        db.command.Parameters.AddWithValue("@TIPO", matriztipop2[n, c]);
                        db.command.Parameters.AddWithValue("@FILA2", n);
                        db.command.Parameters.AddWithValue("@COLUMNA2", c);
                        db.command.Parameters.AddWithValue("@ECO2", ECONUEVO);
                        db.command.Parameters.AddWithValue("@ETIQUETA", etiqueta2[n, c]);
                        db.command.Parameters.AddWithValue("@PKCONDUCTOR", PKCONDUCTOR);
                        db.command.Parameters.AddWithValue("@CONDUCTOR", CONDUCTOR);




                        if (db.execute())
                        {
                            Utilerias.LOG.acciones("modifico boleto " + LoginInfo.UserID);

                            fila.Clear();
                            columna.Clear();
                            getRowsVendidos22();
                            llenarautobus();
                            getRowsVendidos2();
                            textBoxasiento.Text = "";

                        }
                        else
                        {
                            Form mensaje = new Mensaje("Ya no se puede asignar mas asientos", true);
                            fila.Clear();
                            columna.Clear();
                            getRowsVendidos22();
                            llenarautobus();
                            getRowsVendidos2();
                            textBoxasiento.Text = "";
                            mensaje.ShowDialog();
                        }
                    }
                    else
                    {
                        Form mensaje = new Mensaje("Asiento ya asignado", true);
                        fila.Clear();
                        columna.Clear();
                        getRowsVendidos22();
                        llenarautobus();
                        getRowsVendidos2();
                        textBoxasiento.Text = "";
                        mensaje.ShowDialog();
                    }
                }
                else
                {
                    Form mensaje = new Mensaje("Seleccione un asiento para asignarlo en el nuevo autobus", true);
                    fila.Clear();
                    columna.Clear();
                    getRowsVendidos22();
                    llenarautobus();
                    getRowsVendidos2();
                    textBoxasiento.Text = "";
                    mensaje.ShowDialog();
                }

            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "toolStripSessionClose_Click";
                Utilerias.LOG.write(_clase, funcion, error);


            }

        }


        private void consulta()
        {

            string sql2 = "select 1 as RESPUESTA from VENDIDOS where ECO=@ECO AND PKCORRIDA=@PKCORRIDA";
            db.PreparedSQL(sql2);

            db.command.Parameters.AddWithValue("@ECO", ECO);
            db.command.Parameters.AddWithValue("@PKCORRIDA", pkcorrida);
            res = db.getTable();
            int respuesta = 0;

            if (res.Next())
            {
                respuesta = res.GetInt("RESPUESTA");
                if (respuesta == 0)
                    this.Close();
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

                // Program.Form.DesactivarMenu();

                Form form = new Login();
                form.MdiParent = this.MdiParent;
                form.StartPosition = FormStartPosition.CenterScreen;
                form.Show();
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "toolStripSessionClose_Click";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }

        private void PISOS22_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (fila.Count > 0 && textBoxasiento.Text != "")
                {

                    int n = e.RowIndex;
                    int c = e.ColumnIndex;

                    string sql2 = "select 1 as RESPUESTA from VENDIDOS where pisos='2' and eco=@ECO2 and fila=@FILA2 and COLUMNA=@COLUMNA2 and ASIENTO=@ETIQUETA AND PKCORRIDA=@PKCORRIDA";
                    db.PreparedSQL(sql2);

                    db.command.Parameters.AddWithValue("@FILA2", n);
                    db.command.Parameters.AddWithValue("@COLUMNA2", c);
                    db.command.Parameters.AddWithValue("@ECO2", ECONUEVO);
                    db.command.Parameters.AddWithValue("@PKCORRIDA", pkcorrida);

                    db.command.Parameters.AddWithValue("@ETIQUETA", etiqueta2piso2[n, c]);
                    int respu = 0;
                    res = db.getTable();

                    if (res.Next())
                    {
                        respu = res.GetInt("RESPUESTA");
                    }
                    if (respu == 0)
                    {
                        string sql = "UPDATE  VENDIDOS SET PISOS='2', ECO=@ECO2, PK_CONDUCTOR=@PKCONDUCTOR,CONDUCTOR=@CONDUCTOR, CAMBIODEAUTOBUS=1,TIPO=@TIPO,FILA=@FILA2,COLUMNA=@COLUMNA2,ASIENTO=@ETIQUETA WHERE ECO=@ECO AND ASIENTO=@ASIENTO AND PKCORRIDA=@PKCORRIDA ";

                        db.PreparedSQL(sql);
                        db.command.Parameters.AddWithValue("@ECO", ECO);

                        db.command.Parameters.AddWithValue("@PKCORRIDA", pkcorrida);
                        db.command.Parameters.AddWithValue("@ASIENTO", textBoxasiento.Text);

                        db.command.Parameters.AddWithValue("@TIPO", matriztipo2p2[n, c]);

                        db.command.Parameters.AddWithValue("@FILA2", n);
                        db.command.Parameters.AddWithValue("@COLUMNA2", c);
                        db.command.Parameters.AddWithValue("@ECO2", ECONUEVO);
                        db.command.Parameters.AddWithValue("@ETIQUETA", etiqueta2piso2[n, c]);
                        db.command.Parameters.AddWithValue("@PKCONDUCTOR", PKCONDUCTOR);
                        db.command.Parameters.AddWithValue("@CONDUCTOR", CONDUCTOR);


                        if (db.execute())
                        {
                            Utilerias.LOG.acciones("modifico boleto " + LoginInfo.UserID);

                            fila.Clear();
                            columna.Clear();
                            getRowsVendidos22();
                            llenarautobus();
                            getRowsVendidos2();
                            textBoxasiento.Text = "";


                        }
                        else
                        {
                            Form mensaje = new Mensaje("Ya no se puede asignar mas asientos", true);

                            mensaje.ShowDialog();
                        }
                    }
                    else
                    {
                        Form mensaje = new Mensaje("Asiento ya asignado", true);

                        mensaje.ShowDialog();
                    }
                }
                else
                {
                    Form mensaje = new Mensaje("Seleccione un asiento para asignarlo en el nuevo autobus", true);

                    mensaje.ShowDialog();
                }

            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "toolStripSessionClose_Click";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }


        private void BtnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void Timer1_Tick(object sender, EventArgs e)
        {

        }

        private void C2_Click(object sender, EventArgs e)
        {

        }

        private void B_Click(object sender, EventArgs e)
        {

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

        private void autobus_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int c = e.ColumnIndex;
            int n = e.RowIndex;
            if(filatemp==n && columnatemp == c)
            {
                filatemp = 100;
                textBoxasiento.Text = "";
                columnatemp = 100;
                
                getRowsVendidos2();

            }
            else if (matrizpiso1[n, c] != "" && textBoxasiento.Text == "" && matrizstatus[n, c] != "")
            {
                Bitmap asientoimg = new Bitmap(imagenazul);

                string firstText = matrizpiso1[n, c];
                PointF firstLocation = new PointF(4f, 8f);
                using (Graphics graphics = Graphics.FromImage(asientoimg))
                {
                    using (Font arialFont = new Font("Arial", 9))
                    {
                        graphics.DrawString(firstText, arialFont, Brushes.White, firstLocation);
                    }

                }
                autobus.Rows[n].Cells[c].Value = asientoimg;
                filatemp = n;
                columnatemp = c;
                textBoxasiento.Text = matrizpiso1[n, c];
                button3.Enabled = true;

            }

        }

        private void PISOS2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            int c = e.ColumnIndex;
            int n = e.RowIndex;
            if (filatemp == n && columnatemp == c)
            {
                getRowsVendidos2();
                filatemp = 100;
                textBoxasiento.Text = "";
                columnatemp = 100;
            }
            else if (matrizpiso2[n, c] != "" && textBoxasiento.Text == "" && matrizstatus2[n, c] != "")
            {
                Bitmap asientoimg = new Bitmap(imagenazul);

                string firstText = matrizpiso2[n, c];
                PointF firstLocation = new PointF(4f, 8f);
                using (Graphics graphics = Graphics.FromImage(asientoimg))
                {
                    using (Font arialFont = new Font("Arial", 9))
                    {
                        graphics.DrawString(firstText, arialFont, Brushes.White, firstLocation);
                    }

                }
                PISOS2.Rows[n].Cells[c].Value = asientoimg;
                filatemp = n;
                columnatemp = c;
                textBoxasiento.Text = matrizpiso2[n, c];
                button3.Enabled = true;

            }

        }


        private void cancelarboleto()
        {
            try


            {
                //datagridvendidos.Rows.Clear();
                string sql = "UPDATE VENDIDOS SET STATUS='CANCELADO', CANCELADO=@VENDEDOR WHERE  ECO=@ECO AND PKCORRIDA=@PKCORRIDA AND FILA=@FILA AND COLUMNA=@COLUMNA AND ASIENTO=@ASIENTO";
                db.PreparedSQL(sql);

                db.command.Parameters.AddWithValue("@ECO", ECO);
                db.command.Parameters.AddWithValue("@PKCORRIDA", pkcorrida);
                db.command.Parameters.AddWithValue("@FILA", filatemp);
                db.command.Parameters.AddWithValue("@COLUMNA", columnatemp);
                db.command.Parameters.AddWithValue("@VENDEDOR", LoginInfo.NombreID +" "+LoginInfo.ApellidoID);
                db.command.Parameters.AddWithValue("@ASIENTO", textBoxasiento.Text);




                if (db.execute())
                {
                    textBoxcontraseña.Text = "";
                    textBoxcontraseña.Visible = false;
                    labelmotivo.Visible = false;
                    labelhuell.Visible = false;
                    getRowsVendidos2();
                    contando = 0;

                }



            }
            catch (Exception err)
            {
                string error = err.Message;

                MessageBox.Show("Ocurrio un Error, verifique autobuses o que la corrida tenga autobus");
                checkInternetAvaible();
                string funcion = "getRowsvendidos";
                Utilerias.LOG.write(_clase, funcion, error);


            }

        }

  

        private void button1_Click(object sender, EventArgs e)
        {
            labelmotivo.Visible = true;
            labelhuell.Visible = true;
            bool huella = verificationUserControl2.verificando();

            if (huella == true && textBoxmotivo.Text != "" || textBoxcontraseña.Text == contraseña && textBoxmotivo.Text != "")
            {

                cancelarboleto();
                fila.Clear();
                columna.Clear();
                cancel = false;
                getRowsVendidos22();
                llenarautobus();
                getRowsVendidos2();
                button3.Enabled = false;
                textBoxasiento.Text = "";
                textBoxmotivo.Text = "";
                labelmotivo.Visible = false;
                labelhuell.Visible = false;
                verificationUserControl2.limpiarhuella();
                filatemp =100;
                columnatemp = 100;
                textBoxcontraseña.Text = "";
                groupBoxhuella.Visible = false;

            }
            else
            {
                if (huella == false && textBoxmotivo.Text != "")
                {
                    labelhuell.Visible = true;
                    labelmotivo.Visible = false;
                }
                if (cancel == true && textBoxmotivo.Text == "")
                {
                    labelmotivo.Visible = true;
                    labelhuell.Visible = false;
                }


            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

            button3.Enabled = false;
            groupBoxhuella.Visible = true;
                        textBoxmotivo.Focus();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            groupBoxhuella.Visible = false;
            textBoxcontraseña.Text = "";
            textBoxcontraseña.Visible = false;
            labelmotivo.Visible = false;
            labelhuell.Visible = false;
            contando = 0;
        }

        private void verificationUserControl2_BackColorChanged(object sender, EventArgs e)
        {
            contando += 1;
            if (contando >= 7)
            {
                contando = 0;
                textBoxcontraseña.Text = "";
                textBoxcontraseña.Visible = true;
                textBoxcontraseña.Focus();
            }
        }

        private void textBoxcontraseña_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar == (char)Keys.Enter))
            {
                labelmotivo.Visible = true;
                labelhuell.Visible = true;
                bool huella = verificationUserControl2.verificando();

                if (huella == true && textBoxmotivo.Text != "" || textBoxcontraseña.Text == contraseña && textBoxmotivo.Text != "")
                {

                    cancelarboleto();
                    fila.Clear();
                    columna.Clear();
                    cancel = false;
                    getRowsVendidos22();
                    llenarautobus();
                    getRowsVendidos2();
                    button3.Enabled = false;
                    textBoxasiento.Text = "";
                    textBoxmotivo.Text = "";
                    labelmotivo.Visible = false;
                    labelhuell.Visible = false;
                    verificationUserControl2.limpiarhuella();
                    filatemp = 100;
                    columnatemp = 100;
                    textBoxcontraseña.Text = "";
                    groupBoxhuella.Visible = false;

                }
                else
                {
                    if (huella == false && textBoxmotivo.Text != "")
                    {
                        labelhuell.Visible = true;
                        labelmotivo.Visible = false;
                    }
                    if (cancel == true && textBoxmotivo.Text == "")
                    {
                        labelmotivo.Visible = true;
                        labelhuell.Visible = false;
                    }


                }

            }
        }
    }

}
