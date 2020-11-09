using Autobuses.Utilerias;
using ConnectDB;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Printing;
using Autobuses;
using System.Globalization;
using System.Data.SqlClient;
using System.Configuration;
using System.Reflection;

namespace Autobuses.Planeacion
{
    public partial class Ventas : Form
    {

        public database db;
        ResultSet res = null;
        string _clase = "ventas";
        private string _fecha;
        List<String> etiquetass = new List<String>();
        List<int> filas = new List<int>();
        List<int> columnas = new List<int>();
        List<string> vendidos = new List<string>();
        List<string> apartado = new List<string>();
        public string _folio;
        private string pkvendido;
        public string _linea;
        public string _origen;
        private string chofer;
        public string _destino;
        public string _hora;
        public string subtt;
        public string totaltt;
        public string ivatt;
        public string PKCONDUCTOR;
        private string ERROR = "";
        private string RESULTADO = "";
        private int pkcorrida = 0;
        private string _destinoboleto_;
        private string _tarifa;
        private int contando=0;
        private string _precio;
        private int IVA=0;
        private bool permitircambio = false;
        private string _asiento;
        private float valor = 0;
        private bool primero = false;
        private string _pasaje;
        private string _pasajero;
        private string VENDTEMPO="";
        private int _fila;
        private int _columna;
        private string _statustemporal;
        private string _npiso;
        private double _total;
        private string st;
        private string pklinea;
        private string asi;
        private string _pasaje2;
        private string vendedoractual = LoginInfo.NombreID + " " + LoginInfo.ApellidoID;
        private int dato = 0;
        private bool todoslosorigenes = false;
        private string pis;
        private bool pie;
        private string tipoautobus;
        public string ECO;
        public bool verdetalle = false;
        private string totalm="";
        private int valoriva;

        private string _llegada;
        private bool cancel = false;
        private string rutapasajero;
        private int nfila;
        private Boolean val = false;
        byte[] fingerPrint;
        List<string> iList = new List<string>();
        List<int> pk_ruta = new List<int>();
        private string validar;
        private string validar2;
        private string pk;
        private float totalcal;
        private float efectivocal;
        private float cambiocal;


        private string formadepago;
        private bool ventavalidar = false;
        private string foliotarjeta;
        private string digitostarjeta;
        private string[,] matrizpiso1;
        private string[,] matrizpiso2;
        private string[,] matrizstatus;
        private string[,] asientovendido;
        private string[,] matrizstatus2;
        private string[,] matriztipo;
        private string[,] matrizn;
        private string[,] matrizn2;

        private string[,] matriztipo2;
        private string[,] matrizvendedor;
        private string[,] matrizvendedor2;
        private string nombre = "";
        private string contraseña = "";
        private string fechaa = DateTime.Now.ToString("yyyy-MM-dd");
        private bool puedecancelar = false;
        private bool puedegenerarguia = false;

        private Bitmap imagen;
        private Bitmap imagenansientopantalla = new Bitmap(Autobuses.Properties.Resources.asientopantall1);
        private Bitmap pantalla = new Bitmap(Autobuses.Properties.Resources.pantalla);

        private Bitmap imagenansiento = new Bitmap(Autobuses.Properties.Resources.asientobase);
        private Bitmap imagenrojo = new Bitmap(Autobuses.Properties.Resources.rojo);
        private Bitmap imagenbloqueo = new Bitmap(Autobuses.Properties.Resources.asientobase);

        private Bitmap imagenamarrillo = new Bitmap(Autobuses.Properties.Resources.amarrillo);
        private Bitmap imagenazul = new Bitmap(Autobuses.Properties.Resources.asientobase);
        private Bitmap imagenpasillo = new Bitmap(Autobuses.Properties.Resources.pasillo);
        private Bitmap imagenbaño = new Bitmap(Autobuses.Properties.Resources.BAN1);
        private Bitmap imagenescalera = new Bitmap(Autobuses.Properties.Resources.ESCALERAS);
        private Bitmap imagensplash = new Bitmap(Autobuses.Properties.Resources.logotickets);
        private Bitmap niño = new Bitmap(Autobuses.Properties.Resources.niño);
        private Bitmap estudiantes = new Bitmap(Autobuses.Properties.Resources.estudiantes);
        private Bitmap maestro = new Bitmap(Autobuses.Properties.Resources.maestro);

        private Bitmap inapam = new Bitmap(Autobuses.Properties.Resources.inapam);
        private Bitmap pasedecortesia = new Bitmap(Autobuses.Properties.Resources.pasedecortecia);
        private Bitmap imagabordo = new Bitmap(Autobuses.Properties.Resources.abord1);

        private string etiqueta;
        private float sub;
        private float ivasub;
        private float tot;
        PointF firstLocation = new PointF(1f, 4f);
        PointF secondlocation = new PointF(10f, 20f);
        Font arialFont = new Font("Arial", 9);
        Font arialFont2 = new Font("Arial", 9);

        public Ventas()
        {
            InitializeComponent();
            CultureInfo ci = new CultureInfo("es-MX");
            CultureInfo.DefaultThreadCurrentCulture = ci;
            CultureInfo.DefaultThreadCurrentUICulture = ci;
            db = new database();
            groupBoxhuella.Visible = false;
            this.Show();
        }

        private void borrarerrores()
        {
            labellinea.Visible = false;
            labelorigen.Visible = false;
            labeldestino.Visible = false;
            labelsalida.Visible = false;
            labeldestinobol.Visible = false;
            labeltarifa.Visible = false;
            labelfecha.Visible = false;
            labelnombre.Visible = false;

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

        private void asientoconpantalla(Color col,string asignacion,int fila, int columna,string tipo,bool pi)
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
            PointF firstLocation = new PointF(1f, 4f);
            using (Graphics graphics = Graphics.FromImage(asientoimg))
            {
                using (Font arialFont = new Font("Arial", 12))
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
                agregados.Rows.Clear();

                autobus.Rows.Clear();
                PISOS2.Rows.Clear();

                PISOS2.ClearSelection();
                agregados.ClearSelection();


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
                        matrizn[fila, columna] = "1";
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
                                asientoconpantalla(col,asignacion,fila,columna,tipo,true);
                            }
                        }
                        if (tipo == "asiento")
                        {

                            Bitmap asientoimg = new Bitmap(imagenansiento);
                            if (asignacion != "")
                            {
                                for (int Xcount =5; Xcount < 15; Xcount++)
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
                                PointF firstLocation = new PointF(1f, 4f);
                                using (Graphics graphics = Graphics.FromImage(asientoimg))
                                {
                                    using (Font arialFont = new Font("Arial", 12))
                                    {
                                        graphics.DrawString(firstText, arialFont, Brushes.White, firstLocation);
                                    }

                                }
                            }
                            autobus.Rows[fila].Cells[columna].Value = asientoimg;
                            matrizpiso1[fila, columna] = asignacion;
                            matrizstatus[fila, columna] = "";
                            matriztipo[fila, columna] = tipo;

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
                        if (tipo == "Pantalla")
                        {

                            autobus.Rows[fila].Cells[columna].Value = pantalla;
                            matrizpiso1[fila, columna] = asignacion;


                        }

                    }
                    if (pi == "2")
                    {
                        c.Visible = true;
                        d.Visible = true;
                        PISOS2.Visible = true;
                        matrizn2[fila, columna] = "2";
                        if (fila == fil2)
                        {
                            PISOS2.Rows.Add();
                            fil2 = fil2 + 1;
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
                                PointF firstLocation = new PointF(1f, 4f);
                                using (Graphics graphics = Graphics.FromImage(asientoimg))
                                {
                                    using (Font arialFont = new Font("Arial", 12))
                                    {
                                        graphics.DrawString(firstText, arialFont, Brushes.White, firstLocation);
                                    }

                                }
                            }
                            PISOS2.Rows[fila].Cells[columna].Value = asientoimg;
                            matrizpiso2[fila, columna] = asignacion;
                            matrizstatus2[fila, columna] = "";
                            matriztipo2[fila, columna] = tipo;

                        }
                        if (tipo == "Asiento con pantalla")
                        {

                            if (asignacion != "")
                            {
                                Color col = Color.Green;
                                asientoconpantalla(col, asignacion, fila, columna, tipo, false);
                            }
                  
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
                        if (tipo == "Pantalla")
                        {

                            PISOS2.Rows[fila].Cells[columna].Value = pantalla;
                            matrizpiso2[fila, columna] = asignacion;


                        }
                        if (tipo == "baño")
                        {

                            PISOS2.Rows[fila].Cells[columna].Value = imagenbaño;
                            matrizpiso2[fila, columna] = asignacion;


                        }

                    }

                }

                getRowsVendidos();

            }
            catch (Exception err)
            {
                string error = err.Message;
                if (Utilerias.Utilerias.CheckForInternetConnection())
                {
                    Form mensaje = new Mensaje("Sin internet", true);

                    mensaje.ShowDialog();
                }
                else
                    MessageBox.Show("Ocurrio un Error, al obtener información.");
                string funcion = "llenarautobus";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }

        private void Limpiar_Click(object sender, EventArgs e)
        {
            limpiar();
            limpiarpreventa();
            limpiarformpago();
            formadepago = "Efectivo";

            foliotarjeta = " ";
            digitostarjeta = " ";
            groupBoxformadepago.Visible = false;
            buttonborrar.Enabled = false;
            buttonreimprimir.Enabled = false;
        }

        private void limpiar()
        {
            try

            {

                groupBoxbvendidos.Visible = false;
                pie = true;
                verdetalle = false;
                textBoxECO.Text = "";
                textBoxruta.Text = "";
                buttonactualizar.BackColor = Color.White;
                buttonactualizar.Enabled = false;
                buttonborrar.Enabled = false;
                buttonborrar.BackColor = Color.White;
                buttonreimprimir.Enabled = false;
                buttonreimprimir.BackColor = Color.White;
                buttonvender.Enabled = false;
                buttonvender.BackColor = Color.White;
                buttonbloquear.Enabled = false;
                buttonbloquear.BackColor = Color.White;
                buttonguia.Enabled = false;
                buttonguia.BackColor = Color.White;
                textBoxtarifa.Text = "";
                textBoxnombre.Text = "";
                datagridvendidos.Rows.Clear();
                agregados.Rows.Clear();
                a.Visible = false;
                b.Visible = false;
                autobus.Visible = false;
                c.Visible = false;
                d.Visible = false;
                PISOS2.Visible = false;
                textBoxtotal.Text = "";
                comboBoxorigen.Items.Clear();
                comboBoxdestino.Items.Clear();
                comboBoxsalida.Items.Clear();
                comboBoxdestinobol.Items.Clear();
                comboBoxtarifa.Items.Clear();
                comboBoxlinea.Text = "";
                comboBoxorigen.Text = "";
                comboBoxdestino.Text = "";
                comboBoxsalida.Text = "";
                comboBoxdestinobol.Text = "";
                comboBoxtarifa.Text = "";
                _tarifa = null;
                _destinoboleto_ = null;
                _hora = null;
                _destino = null;
                _origen = null;
                _linea = null;
                textBoxruta.Text = "";
                textBoxECO.Text = "";


                llenarcomboboxlinea();
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
                    MessageBox.Show("Ocurrio un Error limpiando , intente de nuevo.");
                string funcion = "limpiar";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }

        private void DateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                limpiar();
                _fecha = dateTimePicker1.Value.ToString("dd/MM/yyyy");
                fechaa = dateTimePicker1.Value.ToString("yyyy-MM-dd");

                llenarcomboboxlinea();




            }
            catch (Exception err)
            {
                string error = err.Message;
                checkInternetAvaible();
                MessageBox.Show("Ocurrio un Error, al obtener información.");
                string funcion = "datatimepicker1";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }

        private void llenarcomboboxlinea()
        {
            try
            {
                comboBoxlinea.Items.Clear();
                buttonactualizar.BackColor = Color.White;
                buttonactualizar.Enabled = false;
                buttonlimpiarautobus.BackColor = Color.White;
                buttonlimpiarautobus.Enabled = false;


                if (val == false)
                {
                    string sql = "SELECT LINEA,PK_LINEA FROM VCORRIDAS_DIA_1 WHERE FECHA=convert(date,@FECHA,104)  GROUP BY LINEA, PK_LINEA";
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@FECHA", _fecha);
                }
                res = db.getTable();

                while (res.Next())
                {
                    ComboboxItem item = new ComboboxItem();
                    item.Text = res.Get("LINEA");
                    item.Value = res.Get("PK_LINEA");
                    comboBoxlinea.Items.Add(item);

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
                else MessageBox.Show("Ocurrio un Error al obtener información.");
                string funcion = "llenarcomboboxlinea";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }
        private void ComboBoxlinea_SelectedIndexChanged(object sender, EventArgs e)
        {
            try

            {
                groupBoxbvendidos.Visible = false;

                buttonvender.Enabled = false;
                buttonvender.BackColor = Color.White;
                buttonbloquear.Enabled = false;
                buttonbloquear.BackColor = Color.White;
                buttonguia.Enabled = false;
                buttonguia.BackColor = Color.White;
                verdetalle = false;
                textBoxnombre.Text = "";
                buttonactualizar.BackColor = Color.White;
                buttonactualizar.Enabled = false;
                buttonlimpiarautobus.BackColor = Color.White;
                buttonlimpiarautobus.Enabled = false;
                limpiarpreventa();
                comboBoxorigen.Items.Clear();
                comboBoxdestino.Items.Clear();
                comboBoxsalida.Items.Clear();
                comboBoxdestinobol.Items.Clear();
                comboBoxtarifa.Items.Clear();
                comboBoxorigen.Text = "";
                comboBoxdestino.Text = "";
                comboBoxsalida.Text = "";
                comboBoxdestinobol.Text = "";
                comboBoxtarifa.Text = "";
                textBoxtarifa.Text = "";
                textBoxECO.Text = "";
                _tarifa = null;
                _destinoboleto_ = null;
                _hora = null;
                _destino = null;
                _origen = null;
                _linea = null;
                a.Visible = false;
                b.Visible = false;
                autobus.Visible = false;
                c.Visible = false;
                d.Visible = false;
                PISOS2.Visible = false;
                _linea = comboBoxlinea.SelectedItem.ToString();
                pklinea = (comboBoxlinea.SelectedItem as ComboboxItem).Value.ToString();


                if (todoslosorigenes == true)
                {
                    string sql2 = "SELECT DESTINO FROM DESTINOS WHERE BORRADO=0 ORDER BY DESTINO";
                    db.PreparedSQL(sql2);


                    res = db.getTable();

                    while (res.Next())


                    {
                        ComboboxItem item = new ComboboxItem();
                        item.Text = res.Get("DESTINO");
                        comboBoxorigen.Items.Add(item);

                    }
                }
                else
                {
                    ComboboxItem item = new ComboboxItem();
                    item.Text = LoginInfo.Sucursal;

                    comboBoxorigen.Items.Add(item);

                }
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error,configuracion de lineas es incorrecto, intente de nuevo.");
                string funcion = "comboboxlinea";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }
        private void ComboBoxorigen_SelectedIndexChanged(object sender, EventArgs e)
        {
            try



            {
                groupBoxbvendidos.Visible = false;

                buttonvender.Enabled = false;
                buttonvender.BackColor = Color.White;
                buttonbloquear.Enabled = false;
                buttonbloquear.BackColor = Color.White;
                buttonguia.Enabled = false;
                buttonguia.BackColor = Color.White;
                textBoxruta.Text = "";
                verdetalle = false;
                textBoxnombre.Text = "";
                buttonactualizar.BackColor = Color.White;
                buttonactualizar.Enabled = false;
                buttonlimpiarautobus.BackColor = Color.White;
                buttonlimpiarautobus.Enabled = false;
                limpiarpreventa();
                _origen = comboBoxorigen.SelectedItem.ToString();
                comboBoxdestino.Items.Clear();
                comboBoxsalida.Items.Clear();
                comboBoxdestinobol.Items.Clear();
                comboBoxtarifa.Items.Clear();
                textBoxECO.Text = "";
                comboBoxdestino.Text = "";
                comboBoxsalida.Text = "";
                comboBoxdestinobol.Text = "";
                comboBoxtarifa.Text = "";
                textBoxtarifa.Text = "";
                _tarifa = null;
                _destinoboleto_ = null;
                _hora = null;
                a.Visible = false;
                b.Visible = false;
                autobus.Visible = false;
                c.Visible = false;
                d.Visible = false;
                PISOS2.Visible = false;

                string var = DateTime.Now.AddMinutes(-30).ToString("yyyy-MM-dd HH:mm");
                //string sql2 = "SELECT DESTINO FROM VCORRIDAS_DIA_3 WHERE LINEA = @LINEA AND BLOQUEADO = 0" +
                //                               " and FECHA = convert(date, @FECHA, 104)  and completo = 1 and pk_ruta in " +
                //                               "(select PK_RUTA FROM VCORRIDAS_DIA_3 WHERE SALIDA>@SALIDA AND COMPLETO = 1 AND ORIGEN =@ORIGEN AND " +
                //                               "FECHA = convert(date, @FECHA, 104)  AND LINEA =@LINEA) GROUP BY DESTINO";

                string sql2 = "SELECT DESTINO_COMPLETO AS DESTINO,PK_DESTINO_COMPLETO FROM VCORRIDAS_DIA_3 WHERE BLOQUEADO = 0 AND GUIA=0 AND SALIDA_C>@SALIDA AND LINEA =@LINEA AND ORIGEN=@ORIGEN AND FECHA = convert(date, @FECHA, 104)" +
                    " GROUP BY DESTINO_COMPLETO,PK_DESTINO_COMPLETO ORDER BY DESTINO_COMPLETO";
                db.PreparedSQL(sql2);

                db.command.Parameters.AddWithValue("@SALIDA", DateTime.Now.AddMinutes(-30).ToString("yyyy-MM-dd HH:mm"));
                db.command.Parameters.AddWithValue("@ORIGEN", _origen);
                db.command.Parameters.AddWithValue("@LINEA", _linea);
                db.command.Parameters.AddWithValue("@FECHA", _fecha);

                res = db.getTable();

                while (res.Next())


                {
                    ComboboxItem item = new ComboboxItem();
                    item.Text = res.Get("DESTINO");
                    comboBoxdestino.Items.Add(item);

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
                    MessageBox.Show("Ocurrio un Error, configuración de corridas es incorrecto.");
                string funcion = "comboboxorigen";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }

        private void ComboBoxdestino_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
     {
                groupBoxbvendidos.Visible = false;

                buttonvender.Enabled = false;
                buttonvender.BackColor = Color.White;
                buttonbloquear.Enabled = false;
                buttonbloquear.BackColor = Color.White;
                buttonguia.Enabled = false;
                buttonguia.BackColor = Color.White;
                verdetalle = false;
                limpiarpreventa();
                if (comboBoxdestino.SelectedItem != null)
                {
                    _destino = (comboBoxdestino.SelectedItem as ComboboxItem).ToString();
                }
                textBoxruta.Text = "";
                textBoxnombre.Text = "";
                comboBoxsalida.Items.Clear();
                comboBoxdestinobol.Items.Clear();
                comboBoxtarifa.Items.Clear();
                buttonactualizar.BackColor = Color.White;
                buttonactualizar.Enabled = false;
                buttonlimpiarautobus.BackColor = Color.White;
                buttonlimpiarautobus.Enabled = false;
                comboBoxsalida.Text = "";
                textBoxECO.Text = "";
                comboBoxdestinobol.Text = "";
                comboBoxtarifa.Text = "";
                _tarifa = null;
                _destinoboleto_ = null;
                _hora = null;
                a.Visible = false;
                b.Visible = false;
                autobus.Visible = false;
                c.Visible = false;
                d.Visible = false;
                PISOS2.Visible = false;
                textBoxtarifa.Text = "";
                string DAT = DateTime.Now.AddMinutes(-30).ToString("yyyy-MM-dd HH:mm");



                string sql = "SELECT  CONVERT(VARCHAR(8), SALIDA ) AS SALIDA,SALIDA_C FROM VCORRIDAS_DIA_3  WHERE SALIDA_C>@SALIDA AND ORIGEN=@ORIGEN   AND fecha =@FECHA and" +
                    " DESTINO_COMPLETO=@DESTINO AND LINEA = @LINEA  AND BLOQUEADO = 0 AND GUIA = 0 GROUP BY SALIDA, SALIDA_C ORDER BY SALIDA ASC";


                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@ORIGEN", _origen);
                db.command.Parameters.AddWithValue("@DESTINO", _destino);
                db.command.Parameters.AddWithValue("@LINEA", _linea);
                db.command.Parameters.AddWithValue("@FECHA", fechaa);
                db.command.Parameters.AddWithValue("@SALIDA", DateTime.Now.AddMinutes(-30).ToString("yyyy-MM-dd HH:mm"));


                res = db.getTable();

                while (res.Next())
                {
                    ComboboxItem item = new ComboboxItem();
                    item.Text = res.GetDateTime("SALIDA").ToString("HH:mm");
                    item.Value = res.GetDateTime("SALIDA_C").ToString("yyyy-MM-dd HH:mm");
                    comboBoxsalida.Items.Add(item);

                    //MessageBox.Show((comboBoxRole.SelectedItem as ComboboxItem).Value.ToString());
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
                    MessageBox.Show("Ocurrio un Error, configuración de corridas es incorrecto.");
                string funcion = "comboboxdestino";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }

        private void llegada()
        {
            try


            {
                _hora = comboBoxsalida.SelectedItem.ToString();

                if (val == false)
                {
                    string sql = "SELECT ( LLEGADA_C ) AS LLEGADA,PK,AUTOBUS, RUTA FROM VCORRIDAS_DIA_3 WHERE fecha =@FECHA and " +
                        "DESTINO =@DESTINO AND ORIGEN =@ORIGEN AND LINEA = @LINEA AND SALIDA=@SALIDA AND BLOQUEADO = 0 AND GUIA = 0 and pk_ruta = (" +
                        "SELECT PK_RUTA FROM VCORRIDAS_DIA_3  WHERE fecha =@FECHA and" +
                        " DESTINO =@DESTINO AND ORIGEN =@ORIGEN AND LINEA = @LINEA AND SALIDA=@SALIDA AND BLOQUEADO = 0 AND GUIA = 0 group by PK_RUTA)";
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@ORIGEN", _origen);
                    db.command.Parameters.AddWithValue("@DESTINO", _destinoboleto_);
                    db.command.Parameters.AddWithValue("@LINEA", _linea);
                    db.command.Parameters.AddWithValue("@SALIDA", _hora);
                    db.command.Parameters.AddWithValue("@FECHA", fechaa);

                }
                res = db.getTable();

                if (res.Next())
                {
                    _llegada = res.GetDateTime("LLEGADA").ToString("yyyy-MM-dd HH:mm");
                    pkcorrida = res.GetInt("PK");


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
                    MessageBox.Show("Ocurrio un Error los horarios son incorrecto, configure bien el rolado.");
                string funcion = "llegada";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }

        private void obtenerpkparasaberguia()
        {
            try
            {

                string sql = "SELECT PK FROM VCORRIDAS_DIA_1 WHERE DESTINO=@DESTINO AND ORIGEN=@ORIGEN AND LINEA=@LINEA AND SALIDA=@SALIDA AND FECHA=convert(date,@FECHA,104) ORDER BY SALIDA ASC ";
                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@ORIGEN", _origen);
                db.command.Parameters.AddWithValue("@DESTINO", _destino);
                db.command.Parameters.AddWithValue("@LINEA", _linea);
                db.command.Parameters.AddWithValue("@SALIDA", _hora);
                db.command.Parameters.AddWithValue("@FECHA", _fecha);




                res = db.getTable();

                if (res.Next())
                {
                    pk = res.Get("PK");

                    GUIACOMPROBACION();
                    limpiarpreventa();
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
                    MessageBox.Show("Ocurrio un Error, la corrida no es correcta.");
                string funcion = "obtenerpk";
                Utilerias.LOG.write(_clase, funcion, error);


            }

        }
        private void ruta()
        {
            try
            {


                string sql = "SELECT  CONVERT(VARCHAR(8), LLEGADA ) AS LLEGADA,PK,AUTOBUS, RUTA FROM VCORRIDAS_DIA_3 WHERE fecha =@FECHA and " +
                   "DESTINO =@DESTINO AND ORIGEN =@ORIGEN AND LINEA = @LINEA AND SALIDA_C=@SALIDA AND BLOQUEADO = 0 AND GUIA = 0 and pk_ruta = (" +
                   "SELECT PK_RUTA FROM VCORRIDAS_DIA_3  WHERE FECHA =@FECHA and" +
                   " DESTINO =@DESTINO AND ORIGEN =@ORIGEN AND LINEA = @LINEA AND SALIDA_C=@SALIDA AND BLOQUEADO = 0 AND GUIA = 0 AND ESCALA=1   group by PK_RUTA)";
                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@ORIGEN", _origen);
                db.command.Parameters.AddWithValue("@DESTINO", _destino);
                db.command.Parameters.AddWithValue("@LINEA", _linea);
                db.command.Parameters.AddWithValue("@SALIDA", _hora);
                db.command.Parameters.AddWithValue("@FECHA", fechaa);


                res = db.getTable();

                if (res.Next())

                {

                    textBoxruta.Text = res.Get("RUTA");
                    pk = res.Get("PK");
                    ECO = res.Get("AUTOBUS");
                    textBoxECO.Text = ECO;
                    obtenerchofer();
                    sabercambio();
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
                    MessageBox.Show("Ocurrio un Error, rolado no tiene la ruta correcta.");
                string funcion = "ruta";
                Utilerias.LOG.write(_clase, funcion, error);
            }


        }
        private void sabercambio()
        {
            try
            {
                string sql = "SELECT TOP(1) VE.PK FROM VENDIDOS VE " +
                 "INNER JOIN VCORRIDAS_DIA_1 CO ON(CO.PK= VE.PKCORRIDA AND NOT CO.AUTOBUS = VE.ECO) " +
                 "WHERE  VE.STATUS='VENDIDO' AND VE.FECHA=@FECHA AND  VE.RUTA=@RUTA";
                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@FECHA", DateTime.Now.ToString("dd/MM/yyyy"));
                db.command.Parameters.AddWithValue("@RUTA", textBoxruta.Text);
                res = db.getTable();
                if (res.Next())
                {
                    Form mensaje = new Mensaje("Quedan asientos pendientes por asignar, debido al cambio de autobus", true);
                    mensaje.ShowDialog();
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
                    MessageBox.Show("Ocurrio un Error, rolado no tiene la ruta correcta.");
                string funcion = "sabercambio";
                Utilerias.LOG.write(_clase, funcion, error);
            }

        }
        private void obtenerchofer()
        {
            try
            {
                string sql = "SELECT PK_CHOFER, CHOFER FROM VAUTOBUSES WHERE ECO=@ECO";
                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@ECO", ECO);
                res = db.getTable();

                if (res.Next())
                {
                    chofer = res.Get("CHOFER");
                    PKCONDUCTOR = res.Get("PK_CHOFER");

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
                    MessageBox.Show("Ocurrio un Error, rolado no tiene la ruta correcta.");
                string funcion = "obtenerchofer";
                Utilerias.LOG.write(_clase, funcion, error);
            }
        }

        private void ComboBoxsalida_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                groupBoxbvendidos.Visible = false;
                
                buttonvender.Enabled = false;
                buttonvender.BackColor = Color.White;
                buttonbloquear.Enabled = false;
                buttonbloquear.BackColor = Color.White;
                buttonguia.Enabled = false;
                buttonguia.BackColor = Color.White;
                textBoxruta.Text = "";
                textBoxnombre.Text = "";
                buttonactualizar.BackColor = Color.White;
                buttonactualizar.Enabled = false;
                buttonlimpiarautobus.BackColor = Color.White;
                buttonlimpiarautobus.Enabled = false;
                buttonguia.Enabled = true;
                buttonguia.BackColor = Color.BlueViolet;
                limpiarpreventa();
                if (comboBoxsalida.SelectedItem != null)
                {
                    _hora = (comboBoxsalida.SelectedItem as ComboboxItem).Value.ToString();
                }
                comboBoxdestinobol.Items.Clear();
                comboBoxtarifa.Items.Clear();
                comboBoxdestinobol.Text = "";
                comboBoxtarifa.Text = "";
                _tarifa = null;
                _destinoboleto_ = null;
                ruta();


                a.Visible = false;
                b.Visible = false;
                autobus.Visible = false;
                c.Visible = false;
                d.Visible = false;
                PISOS2.Visible = false;
                textBoxtarifa.Text = "";



                comboBoxdestinobol.Items.Clear();

                string sql = "SELECT  DESTINO FROM VCORRIDAS_DIA_3 where LINEA =@LINEA AND FECHA=@FECHA AND ORIGEN=@ORIGEN " +
                    "AND BLOQUEADO=0 AND GUIA=0 AND SALIDA_C=@SALIDA and (ESCALA=1 OR COMPLETO=1) AND PK_RUTA in (SELECT PK_RUTA FROM VCORRIDAS_DIA_3 WHERE PK=@PK )";
                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@ORIGEN", _origen);
                db.command.Parameters.AddWithValue("@LINEA", _linea);
                db.command.Parameters.AddWithValue("@FECHA", fechaa);
                db.command.Parameters.AddWithValue("@SALIDA", _hora);
                db.command.Parameters.AddWithValue("@PK", pk);



                res = db.getTable();

                while (res.Next())
                {
                    ComboboxItem item = new ComboboxItem();
                    item.Text = res.Get("DESTINO");
                    comboBoxdestinobol.Items.Add(item);
                    //  textBoxruta.Text = res.Get("RUTA");

                }
                comboBoxdestinobol.SelectedIndex = 0;


                obtenerpkparasaberguia();
                if (validar == "True")
                {
                    Form mensaje = new Mensaje("La guia ya se cerro", true);

                    mensaje.ShowDialog();
                    limpiar();
                    agregados.Rows.Clear();

                }
                if (validar2 == "True")
                {
                    Form mensaje = new Mensaje("La guia ya se bloqueo", true);

                    mensaje.ShowDialog(); limpiar();
                    agregados.Rows.Clear();

                }
                verdetalle = true;
                llegada();
                llenarautobus();

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
                    MessageBox.Show("Cambio de autobus");
                string funcion = "comboboxsalida";
                Utilerias.LOG.write(_clase, funcion, error);


            }


        }
        private void tar()
        {
            try

            {

                groupBoxbvendidos.Visible = true;
                buttonlimpiarautobus.BackColor = Color.DodgerBlue;
                buttonlimpiarautobus.Enabled = true;
                buttonactualizar.BackColor = Color.FromArgb(128, 64, 0);
                buttonactualizar.Enabled = true;
                buttonvender.Enabled = true;
                buttonvender.BackColor = Color.FromArgb(54, 43, 221);

                autobus.Enabled = true;
                PISOS2.Enabled = true;
                comboBoxtarifa.Items.Clear();
                comboBoxtarifa.Text = "";
                textBoxtarifa.Text = "";
                _tarifa = null;

                if (comboBoxdestinobol.SelectedItem != null)
                {
                    _destinoboleto_ = (comboBoxdestinobol.SelectedItem as ComboboxItem).ToString();
                }
                string sql = "SELECT PASAJE FROM VLISTAPRECIO WHERE ACTIVO=1 AND DESTINO=@DESTINO AND ORIGEN=@ORIGEN AND LINEA=@LINEA GROUP BY PASAJE";
                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@DESTINO", _destinoboleto_);
                db.command.Parameters.AddWithValue("@ORIGEN", _origen);
                db.command.Parameters.AddWithValue("@LINEa", _linea);

                res = db.getTable();

                while (res.Next())
                {
                    ComboboxItem item = new ComboboxItem();
                    item.Text = res.Get("PASAJE");
                    comboBoxtarifa.Items.Add(item);

                }
                if (comboBoxtarifa.Items.Count > 0)
                {
                    comboBoxtarifa.SelectedIndex = 0;

                    textBoxnombre.Text = "Publico en General";
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
                    MessageBox.Show("Ocurrio un Error, verifique la configuración en corridas.");
                string funcion = "comboboxdestinobol";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }

        private void ComboBoxdestinobol_SelectedIndexChanged(object sender, EventArgs e)
        {
            try

                {
             
                groupBoxbvendidos.Visible = true;
                buttonlimpiarautobus.BackColor = Color.DodgerBlue;
                buttonlimpiarautobus.Enabled = true;
                buttonactualizar.BackColor = Color.FromArgb(128, 64, 0);
                buttonactualizar.Enabled = true;
                buttonvender.Enabled = true;
                buttonvender.BackColor = Color.FromArgb(54, 43, 221);
              
                autobus.Enabled = true;
                PISOS2.Enabled = true;
                comboBoxtarifa.Items.Clear();
                comboBoxtarifa.Text = "";
                textBoxtarifa.Text = "";
                _tarifa = null;

                if (comboBoxdestinobol.SelectedItem != null)
                {
                    _destinoboleto_ = (comboBoxdestinobol.SelectedItem as ComboboxItem).ToString();
                }
                string sql = "SELECT PASAJE FROM VLISTAPRECIO WHERE ACTIVO=1 AND DESTINO=@DESTINO AND ORIGEN=@ORIGEN AND LINEA=@LINEA GROUP BY PASAJE";
                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@DESTINO", _destinoboleto_);
                db.command.Parameters.AddWithValue("@ORIGEN", _origen);
                db.command.Parameters.AddWithValue("@LINEa", _linea);

                res = db.getTable();

                while (res.Next())
                {
                    ComboboxItem item = new ComboboxItem();
                    item.Text = res.Get("PASAJE");
                    comboBoxtarifa.Items.Add(item);

                }
                if (comboBoxtarifa.Items.Count > 0)
                {
                    comboBoxtarifa.SelectedIndex = 0;
               
                    textBoxnombre.Text = "Publico en General";
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
                    MessageBox.Show("Ocurrio un Error, verifique la configuración en corridas.");
                string funcion = "comboboxdestinobol";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }


        private void ComboBoxtarifa_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                _tarifa = "";
                _destinoboleto_ = comboBoxdestinobol.SelectedItem.ToString();
                textBoxtarifa.Text = "";
                autobus.Enabled = true;
                PISOS2.Enabled = true;
                _pasaje = comboBoxtarifa.SelectedItem.ToString();
                string sql = "SELECT PRECIOCONDESCUENTO FROM VLISTAPRECIO WHERE ACTIVO=1 AND PASAJE=@PASAJE AND  DESTINO=@DESTINO AND ORIGEN=@ORIGEN AND LINEA=@LINEA GROUP BY PRECIOCONDESCUENTO";
                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@PASAJE", _pasaje);
                db.command.Parameters.AddWithValue("@DESTINO", _destinoboleto_);
                db.command.Parameters.AddWithValue("@ORIGEN", _origen);
                db.command.Parameters.AddWithValue("@LINEA", _linea);
                res = db.getTable();

                if (res.Next())
                {
                    double tempo=0.0;
                    tempo = res.GetDouble("PRECIOCONDESCUENTO");
                    _tarifa = (Convert.ToInt32(Math.Floor(tempo))).ToString();

                    textBoxtarifa.Text = "$" + _tarifa;
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
                    MessageBox.Show("Ocurrio un Error, verifique la configuracion de corridas por que no se puede obtener los precios.");
                string funcion = "comboboxtarifa";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }


        private void limpiarpreventa()

        {
            try
            {
                pie = true;
                if (_hora != null && ECO != null)
                {
                    agregados.Rows.Clear();

                    string sql3 = "DELETE FROM VENDIDOS  WHERE  (SALIDA  BETWEEN  SALIDA AND CONVERT (datetime,@LLEGADA ) ) AND " +
                    "(LLEGADA  BETWEEN  CONVERT(datetime, @SALIDA) AND LLEGADA) AND LINEA=@LINEA  AND FECHA=@FECHA AND ECO=@ECO AND VENDEDOR=@VENDEDOR AND (STATUS='PREVENTA' OR STATUS='BLOQUEADO') ";

                    db.PreparedSQL(sql3);
                    db.command.Parameters.AddWithValue("@VENDEDOR", vendedoractual);
                    db.command.Parameters.AddWithValue("@SALIDA", _hora);
                    db.command.Parameters.AddWithValue("@LINEA", _linea);
                    db.command.Parameters.AddWithValue("@LLEGADA", _llegada);
                    db.command.Parameters.AddWithValue("@FECHA", _fecha);
                    db.command.Parameters.AddWithValue("@ECO", ECO);
                    db.execute();
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
                    MessageBox.Show("Ocurrio un Error, verifique la configuracion de corridas por que no se puede obtener los precios.");
                string funcion = "limpiarpreventa";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }


        public void getRowsVendidos()
        {
            try


            {

                groupBoxbvendidos.Visible = true;
                _hora = (comboBoxsalida.SelectedItem as ComboboxItem).Value.ToString();
                datagridvendidos.Rows.Clear();
                string sql = "SELECT * FROM VISTAVENDIDOSCOLOR  WHERE  (SALIDA  BETWEEN  SALIDA AND CONVERT (datetime,@LLEGADA ) ) AND" +
                    " (LLEGADA  BETWEEN  CONVERT(datetime, @SALIDA) AND LLEGADA) AND LINEA =@LINEA  AND FECHA=@FECHA AND ECO=@ECO ORDER BY FECHAC ASC";
                db.PreparedSQL(sql);


                db.command.Parameters.AddWithValue("@SALIDA", _hora);
                db.command.Parameters.AddWithValue("@LINEA", _linea);
                db.command.Parameters.AddWithValue("@LLEGADA", _llegada);
                db.command.Parameters.AddWithValue("@FECHA", _fecha);
                db.command.Parameters.AddWithValue("@ECO", ECO);


                textBoxboletosvendidos.Text = "";

                int n = 0;
                int bvendidos = 0;

                res = db.getTable();


                while (res.Next())
                {
                    n = datagridvendidos.Rows.Add();
                    datagridvendidos.Rows[n].Cells[0].Value = res.Get("FOLIO");
                    datagridvendidos.Rows[n].Cells[1].Value = res.Get("LINEA");
                    datagridvendidos.Rows[n].Cells[2].Value = res.Get("ORIGEN");
                    datagridvendidos.Rows[n].Cells[3].Value = res.Get("FECHA");
                    datagridvendidos.Rows[n].Cells[4].Value = res.Get("DESTINO");
                    datagridvendidos.Rows[n].Cells[5].Value = res.Get("SALIDA");
                    datagridvendidos.Rows[n].Cells[6].Value = res.Get("DESTINOBOLETO");
                    datagridvendidos.Rows[n].Cells[7].Value = res.Get("TARIFA");
                    datagridvendidos.Rows[n].Cells[8].Value = Utilerias.Utilerias.formatCurrency(res.GetDouble("PRECIO"));
                    datagridvendidos.Rows[n].Cells[9].Value = res.Get("ASIENTO");
                    datagridvendidos.Rows[n].Cells[10].Value = res.Get("PASAJERO");
                    datagridvendidos.Rows[n].Cells[11].Value = res.Get("STATUS");
                    datagridvendidos.Rows[n].Cells[12].Value = res.Get("FILA");
                    datagridvendidos.Rows[n].Cells[13].Value = res.Get("COLUMNA");
                    datagridvendidos.Rows[n].Cells[14].Value = res.Get("VENDEDOR");
                    datagridvendidos.Rows[n].Cells[15].Value = res.Get("SUCURSAL");
                    datagridvendidos.Rows[n].Cells[16].Value = res.Get("ECO");
                    datagridvendidos.Rows[n].Cells[17].Value = res.Get("LLEGADA");
                    datagridvendidos.Rows[n].Cells[18].Value = res.Get("SALIDA");
                    datagridvendidos.Rows[n].Cells[19].Value = res.Get("PISOS");
                    datagridvendidos.Rows[n].Cells[20].Value = res.Get("FORMADEPAGO");
                    datagridvendidos.Rows[n].Cells[21].Value = res.Get("RUTA");
                    datagridvendidos.Rows[n].Cells[22].Value = res.Get("CONDUCTOR");
                    datagridvendidos.Rows[n].Cells[23].Value = res.Get("MONTO");
                    datagridvendidos.Rows[n].Cells["pkvendidosname"].Value = res.Get("PK");

                    string tipopasaje = res.Get("TARIFA");
                    string st = res.Get("STATUS");
                    string color = res.Get("COLOR");
                    string foli = res.Get("FOLIO");
                    string ven = res.Get("VENDEDOR");
                    double prec = res.GetDouble("PRECIO");
                    int _fila = res.GetInt("FILA");
                    int _columna = res.GetInt("COLUMNA");
                    string asi = res.Get("ASIENTO");
                    string destinoboleto = res.Get("CLAVE");
                    if (destinoboleto.Length > 3)
                    {
                        destinoboleto = destinoboleto.Substring(0, 3);
                    }
                    string pis = res.Get("PISOS");
                    string abord = res.Get("SUCURSAL");
                    string tip = res.Get("TIPO");

                    Color col = Color.FromName(color);
                    if (pis == "1")
                    {

                        if (st == "PREVENTA" && ven != vendedoractual)
                        {
                            bvendidos += 1;
                            textBoxboletosvendidos.Text = bvendidos.ToString();
                            if (tip == "asiento")
                                asientopintar(col, asi, ven, st, true,_fila,_columna, destinoboleto);
                            else
                                asientopantalla(col, asi, ven, st, true,_fila,_columna, destinoboleto);

                        }
                        if (st == "BLOQUEADO" && ven != vendedoractual)
                        {
                            bvendidos += 1;
                            textBoxboletosvendidos.Text = bvendidos.ToString();
                            if (tip == "asiento")
                                asientopintarbloqueado(col, asi, ven, st, true, _fila, _columna, destinoboleto);
                            else
                                asientopantalla(col, asi, ven, st, true, _fila, _columna, destinoboleto);

                        }
                        if (st == "VENDIDO")
                        {bvendidos += 1;
                        textBoxboletosvendidos.Text = bvendidos.ToString();
                        
                            if (tip == "asiento")
                               
                                asientopintar(col, asi, ven, st, true, _fila, _columna, destinoboleto);
                            if (tip == "Asiento con pantalla")
                                asientopantalla(col, asi, ven, st, true, _fila, _columna, destinoboleto);

                        }


                        if (st == "CANCELADO")
                        {
                            if (tip == "asiento")
                                asientopintar(Color.Green, asi, ven, st, true, _fila, _columna, destinoboleto);
                            if (tip == "Asiento con pantalla")
                                asientopantalla(Color.Green, asi, ven, st, true, _fila, _columna, destinoboleto);

                        }

                        if (st == "PREVENTA" && ven == vendedoractual)
                        {

                            matrizvendedor[_fila, _columna] = ven;
                            matrizstatus[_fila, _columna] = st;

                        }

                    }

                    if (pis == "2")
                    {
                        if (st == "PREVENTA" && ven != vendedoractual)
                        {
                            bvendidos += 1;
                            textBoxboletosvendidos.Text = bvendidos.ToString();
                            if (tip == "asiento")
                                asientopintar(col, asi, ven, st, false,_fila,_columna, destinoboleto);
                            if (tip == "Asiento con pantalla")
                                asientopantalla(col, asi, ven, st, false,_fila,_columna, destinoboleto);
                        }
                        if (st == "BLOQUEADO" && ven != vendedoractual)
                        {
                            bvendidos += 1;
                            textBoxboletosvendidos.Text = bvendidos.ToString();
                            if (tip == "asiento")
                                asientopintar(col, asi, ven, st, false, _fila, _columna, destinoboleto);
                            else
                                asientopantalla(col, asi, ven, st, false, _fila, _columna, destinoboleto);

                        }

                     
                        if (st == "VENDIDO")
                        {
                            bvendidos += 1;
                            textBoxboletosvendidos.Text = bvendidos.ToString();
                            if (tip == "asiento")
                                asientopintar(col, asi, ven, st, false, _fila, _columna, destinoboleto);
                            if (tip == "Asiento con pantalla")
                                asientopantalla(col, asi, ven, st, false, _fila, _columna, destinoboleto);
                        }


                        if (st == "CANCELADO")
                        {
                            if (tip == "asiento")
                                asientopintar(Color.Green, asi, ven, st, false, _fila, _columna, destinoboleto);
                            if (tip == "Asiento con pantalla")
                                asientopantalla(Color.Green, asi, ven, st, false, _fila, _columna, destinoboleto);
                        }
                    }



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
                    MessageBox.Show("Ocurrio un Error, verifique autobuses o que la corrida tenga autobus");


                string funcion = "getRowsvendidos";
                Utilerias.LOG.write(_clase, funcion, error);


            }

        }
        private void asientopantalla(Color col, string asi, string ven, string st,bool donde,int fila, int columna,string destinoboleto)
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
            PointF firstLocation = new PointF(1f, 4f);
            using (Graphics graphics = Graphics.FromImage(asientoimg))
            {
                using (Font arialFont = new Font("Arial", 12))
                {
                    graphics.DrawString(firstText, arialFont, Brushes.White, firstLocation);
                }
                using (Font arialFont = new Font("Arial", 12))
                {
                    graphics.DrawString(destinoboleto, arialFont, Brushes.White, secondlocation);
                }
            }

            if (donde)
            {
                autobus.Rows[fila].Cells[columna].Value = asientoimg;
                matrizstatus[fila, columna] = st;
                matrizvendedor[fila, columna] = ven;
            }
            else
            {
                PISOS2.Rows[fila].Cells[columna].Value = asientoimg;
                matrizstatus2[fila, columna] = st;
                matrizvendedor2[fila, columna] = ven;
            }
        }

        private void asientopintar(Color col, string asi, string ven, string st, bool donde,int fila,int columna,string destinoboleto)
        {
            try
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


                using (Graphics graphics = Graphics.FromImage(asientoimg))
                {
                    using (Font arialFont = new Font("Arial",12 ))
                    {
                        graphics.DrawString(asi, arialFont, Brushes.White, firstLocation);
                    }
                    if (st == "CANCELADO")
                    {
                        using (Font arialFont = new Font("Arial", 12))
                        {
                            graphics.DrawString("", arialFont, Brushes.White, secondlocation);
                        }
                    }
                    else
                    {
                        using (Font arialFont = new Font("Arial", 12))
                        {
                            graphics.DrawString(destinoboleto, arialFont, Brushes.White, secondlocation);
                        }
                    }
                }
                if (donde)
                {
                    autobus.Rows[fila].Cells[columna].Value = asientoimg;
                    matrizstatus[fila, columna] = st;
                    matrizvendedor[fila, columna] = ven;
                }
                else
                {
                    PISOS2.Rows[fila].Cells[columna].Value = asientoimg;
                    matrizstatus2[fila, columna] = st;
                    matrizvendedor2[fila, columna] = ven;
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
                    MessageBox.Show("Ocurrio un Error, verifique autobuses o que la corrida tenga autobus");


                string funcion = "getRowsvendidos";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }
        private void asientopintarbloqueado(Color col, string asi, string ven, string st, bool donde, int fila, int columna, string destinoboleto)
        {
            try
            {

                Bitmap asientoimg = new Bitmap(imagenbloqueo);

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


                using (Graphics graphics = Graphics.FromImage(asientoimg))
                {
                    using (Font arialFont = new Font("Arial", 12))
                    {
                        graphics.DrawString(asi, arialFont, Brushes.White, firstLocation);
                    }
                    if (st == "CANCELADO")
                    {
                        using (Font arialFont = new Font("Arial", 12))
                        {
                            graphics.DrawString("", arialFont, Brushes.White, secondlocation);
                        }
                    }
                    else
                    {
                        using (Font arialFont = new Font("Arial", 12))
                        {
                            graphics.DrawString(destinoboleto, arialFont, Brushes.White, secondlocation);
                        }
                    }
                }
                if (donde)
                {
                    autobus.Rows[fila].Cells[columna].Value = asientoimg;
                    matrizstatus[fila, columna] = st;
                    matrizvendedor[fila, columna] = ven;
                }
                else
                {
                    PISOS2.Rows[fila].Cells[columna].Value = asientoimg;
                    matrizstatus2[fila, columna] = st;
                    matrizvendedor2[fila, columna] = ven;
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
                    MessageBox.Show("Ocurrio un Error, verifique autobuses o que la corrida tenga autobus");


                string funcion = "getRowsvendidos";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }


        public void getRowsVendidos2()
        {
            try


            {
        

                //datagridvendidos.Rows.Clear();
                string sql = "SELECT TARIFA,STATUS,VENDEDOR,FILA,COLUMNA,ASIENTO,PISOS,SUCURSAL,COLOR,TIPO,CLAVE FROM VISTAVENDIDOSCOLOR  WHERE  (SALIDA  BETWEEN  SALIDA AND CONVERT (datetime,@LLEGADA ) ) AND" +
                    " (LLEGADA  BETWEEN  CONVERT(datetime, @SALIDA) AND LLEGADA) AND LINEA=@LINEA  AND FECHA=@FECHA AND ECO=@ECO ORDER BY FECHAC ASC";
                db.PreparedSQL(sql);


                db.command.Parameters.AddWithValue("@SALIDA", _hora);
                db.command.Parameters.AddWithValue("@LINEA", _linea);
                db.command.Parameters.AddWithValue("@LLEGADA", _llegada);
                db.command.Parameters.AddWithValue("@FECHA", _fecha);
                db.command.Parameters.AddWithValue("@ECO", ECO);


                res = db.getTable();

                int bvendidos = 0;

                while (res.Next())
                {

                    string tipopasaje = res.Get("TARIFA");
                    string st = res.Get("STATUS");
                    string ven = res.Get("VENDEDOR");
                    int _fila = res.GetInt("FILA");
                    int _columna = res.GetInt("COLUMNA");
                        string color = res.Get("COLOR");
                    string asi = res.Get("ASIENTO");
                    string pis = res.Get("PISOS");
                    string tip = res.Get("TIPO");
                    string destinoboleto = res.Get("CLAVE");
                    if (destinoboleto.Length > 3)
                    {
                        destinoboleto = destinoboleto.Substring(0, 3);
                    }
                    string abord = res.Get("SUCURSAL");
                    Color col = Color.FromName(color);
                    if (pis == "1")
                    {
                        if (st == "PREVENTA" && ven != vendedoractual)
                        {
                            bvendidos += 1;
                            textBoxboletosvendidos.Text = bvendidos.ToString();
                            if (tip=="asiento")
                            asientopintar(col, asi, ven, st,true,_fila,_columna, destinoboleto);
                            else
                                asientopantalla(col, asi, ven, st,true,_fila,_columna,destinoboleto);


                        }
                        if (st == "BLOQUEADO" && ven != vendedoractual)
                        {
                            bvendidos += 1;
                            textBoxboletosvendidos.Text = bvendidos.ToString();
                            if (tip == "asiento")
                                asientopintarbloqueado(col, asi, ven, st, true, _fila, _columna, destinoboleto);
                            else
                                asientopantalla(col, asi, ven, st, true, _fila, _columna, destinoboleto);

                        }


                     

                        if (st == "VENDIDO")
                        {
                            bvendidos += 1;
                            textBoxboletosvendidos.Text = bvendidos.ToString();
                            if (tip == "asiento")
                                asientopintar(col, asi, ven, st, true, _fila, _columna, destinoboleto);
                            if (tip == "Asiento con pantalla")
                                asientopantalla(col, asi, ven, st, true, _fila, _columna,destinoboleto);
                        }



                        if (st == "CANCELADO")
                        {

                            if (tip == "asiento")
                                asientopintar(Color.Green, asi, ven, st, true, _fila, _columna, destinoboleto);
                            if (tip == "Asiento con pantalla")
                                asientopantalla(Color.Green, asi, ven, st, true, _fila, _columna,destinoboleto);
                        }
                        if (st == "PREVENTA" && ven == vendedoractual)
                        {
                            matrizstatus[_fila, _columna] = st;
                            matrizvendedor[_fila, _columna] = ven;

                        }

                    }

                    if (pis == "2")
                    {
                    
                        if (st == "PREVENTA" && ven == vendedoractual)
                        {
                            bvendidos += 1;
                            textBoxboletosvendidos.Text = bvendidos.ToString();
                            matrizstatus2[_fila, _columna] = st;
                            matrizvendedor2[_fila, _columna] = ven;
                        }
                        if (st == "PREVENTA" && ven != vendedoractual)
                        {
                            bvendidos += 1;
                            textBoxboletosvendidos.Text = bvendidos.ToString();
                            if (tip == "asiento")
                                asientopintarbloqueado(col, asi, ven, st, false,_fila,_columna, destinoboleto);
                            if (tip == "Asiento con pantalla")
                                asientopantalla(col, asi, ven, st, false,_fila,_columna,destinoboleto);
                        }
                        if (st == "BLOQUEADO" && ven != vendedoractual)
                        {
                            bvendidos += 1;
                            textBoxboletosvendidos.Text = bvendidos.ToString();
                            if (tip == "asiento")
                                asientopintarbloqueado(col, asi, ven, st, false, _fila, _columna, destinoboleto);
                            else
                                asientopantalla(col, asi, ven, st, false, _fila, _columna, destinoboleto);

                        }
                      
                        if (st == "VENDIDO")
                        {
                            bvendidos += 1;
                            textBoxboletosvendidos.Text = bvendidos.ToString();
                            if (tip == "asiento")
                                asientopintar(col, asi, ven, st, false, _fila, _columna, destinoboleto);
                            if (tip == "Asiento con pantalla")
                                asientopantalla(col, asi, ven, st, false,_fila, _columna,destinoboleto);
                        }
                        if (st == "CANCELADO")
                        {
                            if (tip == "asiento")
                                asientopintar(Color.Green, asi, ven, st, false, _fila, _columna, destinoboleto);
                            if (tip == "Asiento con pantalla")
                                asientopantalla(Color.Green, asi, ven, st, false,_fila, _columna,destinoboleto);
                        }
                    }



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
                    MessageBox.Show("Ocurrio un Error, verifique autobuses o que la corrida tenga autobus");

                string funcion = "getRowsvendidos2";

                Utilerias.LOG.write(_clase, funcion, error);


            }

        }

        private void GUIACOMPROBACION()
        {
            try
            {
                validar = "0";
                validar2 = "0";
                string fechaaa = DateTime.Now.ToString("dd/MM/yyyy");
                string sql = "SELECT GUIA, BLOQUEADO FROM CORRIDAS_DIA  WHERE PK=@PK";


                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@PK", pk);

                int n = 0;


                res = db.getTable();
                if (res.Next())
                {
                    validar = res.Get("GUIA");
                    validar2 = res.Get("BLOQUEADO");

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
                    MessageBox.Show("Ocurrio un Error en guia comprobacion, intente de nuevo.");
                string funcion = "guiacomprobacion";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }

        private void Buttonactualizar_Click(object sender, EventArgs e)
        {
            buttonvender.Enabled = false;
            buttonguia.Enabled = false;
            buttonactualizar.Enabled = false;
            buttonlimpiarautobus.Enabled = false;
            buttonlimpiar.Enabled = false;
            buttonborrar.Enabled = false;
            buttonbloquear.Enabled = false;

            textBoxefectivocal.Text = "";
            textBoxcambiocal.Text = "";
            textBoxtotalcal.Text = "";
       
            GUIACOMPROBACION();
            if (validar == "True")
            {
                validar = "False";
                Form mensaje = new Mensaje("La guia ya se cerro", true);

                mensaje.ShowDialog();
                limpiar();
                agregados.Rows.Clear();

            }
             else if (validar2 == "True")
            {
                validar2 = "False";
                Form mensaje = new Mensaje("La guia esta bloqueada", true);

                mensaje.ShowDialog();
                limpiar();
                agregados.Rows.Clear();

            }
            else
            {


                if (agregados.RowCount == 0)
                {

                    Form mensaje = new Mensaje("¿Quieres vender boletos a pie?", false);

                    DialogResult resut = mensaje.ShowDialog();
                    if (resut == DialogResult.OK)
                    {
                        bool yano = false;
                        bool yanol = false;
                        bool yanob = false;
                        autobus.Enabled = false;
                        PISOS2.Enabled = false;
                        
                    
                        string corte = "0";
                        rutapasajero = textBoxruta.Text;
                        _pasajero = textBoxnombre.Text;
                        _fila =100;
                        _npiso = "1";
                        _columna = 100;
                        string   tip = "PIE";
                        etiqueta = "PIE";
                        string horacorta = comboBoxsalida.SelectedItem.ToString();
                        executaProcedimientopreventa( _llegada, _hora, _linea, ECO, _fecha, etiqueta, _tarifa, _origen,
            _destino, _destinoboleto_, horacorta, tot.ToString(), _pasajero, _statustemporal, _fila.ToString(), _columna.ToString(),
            vendedoractual,
                              _npiso, corte, pkcorrida.ToString(), rutapasajero, chofer, tip);


                        textBoxefectivocal.Visible = true;
                        textBoxcambiocal.Visible = true;

                     if (ERROR == "Ya no puedes seleccionar tipo de pasaje")
                    {
                        yano = true;
                    }
                    else if (ERROR == "La guia ya se genero")
                    {
                        yanol = true;
                    }
                    else if (ERROR == "Corrida bloqueada")
                    {
                        yanob = true;
                    }
                        else if ((ERROR == "Boleto vendido " ))
                        {

                            pie = true;
                            buttonbloquear.Enabled = true;
                            buttonbloquear.BackColor = Color.DarkGoldenrod;

                            tot = float.Parse(_tarifa);
                            sub = tot * 100 / IVA;

                            ivasub = tot - sub;
                           
                            rutapasajero = textBoxruta.Text;



                            agregados.Rows.Add(_folio, _linea, _origen, _fecha, _destino, _hora, _destinoboleto_, _pasaje, tot, etiqueta, _pasajero, Utilerias.Utilerias.formatCurrency(sub), Utilerias.Utilerias.formatCurrency(ivasub), Utilerias.Utilerias.formatCurrency(tot), _statustemporal, _fila, _columna, ECO, _llegada, _hora, _npiso, pk, pkvendido);

                          

                            totalsumatori();
                            groupBoxformadepago.Visible = true;
                            autobus.Enabled = false;
                            PISOS2.Enabled = false;
                            calculocambio();

                            folio.Visible = false;
                            foliotext.Visible = false;
                            cuatro.Visible = false;
                            cuatrotext.Visible = false;
                            textBoxefectivocal.Focus();
                            autobus.Enabled = false;
                            PISOS2.Enabled = false;
                            label17.Visible = true;
                            label18.Visible = true;
                            textBoxefectivocal.Visible = true;
                            textBoxcambiocal.Visible = true;
                            Utilerias.LOG.acciones("preventa de  un boleto " + _folio);


                        }

                        if (yano == true)
                        {
                            Form mensaje1 = new Mensaje("Tarifa agotada " + _pasaje, true);
                            mensaje1.ShowDialog();
                        }
                        if (yanol == true)
                        {
                            Form mensaje2 = new Mensaje("La guia ya se genero", true);
                            mensaje2.ShowDialog();
                            limpiar();
                        }
                        if (yanol == true)
                        {
                            Form mensaje3 = new Mensaje(ERROR, true);
                            mensaje3.ShowDialog();
                            limpiar();
                        }
                    }

                }
                else
                {


                    groupBoxformadepago.Visible = true;
                    autobus.Enabled = false;
                    PISOS2.Enabled = false;
                    calculocambio();

                    folio.Visible = false;
                    foliotext.Visible = false;
                    cuatro.Visible = false;
                    cuatrotext.Visible = false;
                    textBoxefectivocal.Focus();
                    autobus.Enabled = false;
                    PISOS2.Enabled = false;
                    label17.Visible = true;
                    label18.Visible = true;
                    textBoxefectivocal.Visible = true;
                    textBoxcambiocal.Visible = true;

                }
            }
        }
        private void calculocambio()
        {
            try
            {
                textBoxefectivocal.Text = "";
                textBoxcambiocal.Text = "";
                textBoxtotalcal.Text = "";
             

                    totalcal = valor;
                    textBoxtotalcal.Text = Utilerias.Utilerias.formatCurrency(valor);

             

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
                    MessageBox.Show("Ocurrio un Error en vender.");
                string funcion = "calculocambio";
                Utilerias.LOG.write(_clase, funcion, error);



            }

        }
        private void ventapie()
        {
            try
            {
                for (int i = 0; i < agregados.Rows.Count; i++)
                {



                    pie = false;

                    _asiento = agregados.Rows[i].Cells[9].Value.ToString();
                    totalm = agregados.Rows[i].Cells["totalname"].Value.ToString();
                    _folio = agregados.Rows[i].Cells["folioname"].Value.ToString();
                    _pasaje = agregados.Rows[i].Cells["tarifaname"].Value.ToString();
                    nombre = agregados.Rows[i].Cells["pasajeroname"].Value.ToString();
                    string _p = agregados.Rows[i].Cells[20].Value.ToString();
                    string _pk = agregados.Rows[i].Cells[21].Value.ToString();
                    string _fila = agregados.Rows[i].Cells[15].Value.ToString();
                    string _columna = agregados.Rows[i].Cells[16].Value.ToString();
                    pkvendido = agregados.Rows[i].Cells["pkvendido1"].Value.ToString();





                    codigoqr(_folio);



                    string status = "VENDIDO";

                    string sql = "UPDATE  VENDIDOS SET STATUS=@STATUS,FOLIO=@FOLIO, MONTO=@MONTO, FORMADEPAGO=@FORMADEPAGO,FOLIOTARJETA=@FOLIOTARJETA,DIGITOSTARJETA=@DIGITOSTARJETA WHERE PK=@PK ";

                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@FOLIO", _folio);
                    db.command.Parameters.AddWithValue("@PK", pkvendido);

                    db.command.Parameters.AddWithValue("@STATUS", status);
                    db.command.Parameters.AddWithValue("@FORMADEPAGO", formadepago);
                    db.command.Parameters.AddWithValue("@DIGITOSTARJETA", digitostarjeta);
                    db.command.Parameters.AddWithValue("@FOLIOTARJETA", foliotarjeta);
                    db.command.Parameters.AddWithValue("@MONTO", totalm);


                    if (db.execute())
                    {

                        Utilerias.LOG.acciones("vendio un boleto" + _folio);
                        imprimir();
                        _total = 0;
                    

                    }
                }

                getRowsVendidos();


                columnas.Clear();
                filas.Clear();
                etiquetass.Clear();
                agregados.Rows.Clear();
                textBoxtotal.Text = "$0";
                vender = false;
                textBoxnombre.Text = "Publico en General";
                textBoxcomando.Focus();
                textBoxcomando.Text = "";
                if (agregados.Rows.Count < 1)
                {
                    buttonbloquear.Enabled = false;
                    buttonbloquear.BackColor = Color.White;
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
                    MessageBox.Show("Ocurrio un Error en vender.");
                string funcion = "buttonactualizar";
                Utilerias.LOG.write(_clase, funcion, error);


            }

        }

        private void Cancelt_Click(object sender, EventArgs e)
        {
            groupBoxformadepago.Visible = false;
            autobus.Enabled = true;
            PISOS2.Enabled = true;
            tdebito.Text = "Tarjeta de Debito";
            tcredito.Text = "Tarjeta de Credito";
            autobus.Enabled = true;
            PISOS2.Enabled = true;


        }

        private void Efectivo_Click(object sender, EventArgs e)
        {
            groupBoxformadepago.Visible = false;
            progressBar1.Visible = true;
            progressBar1.Value = 90;
            timerefectivo.Start();
            

        }

        private void txtCaracter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back) && (e.KeyChar != (char)Keys.Enter) && (e.KeyChar != (char)Keys.Delete))
            {
                if ((e.KeyChar == (char)Keys.T) || (e.KeyChar == 116))
                {
                    comboBoxtarifa.Focus();
                    timer1.Start();
                }
                else if ((e.KeyChar == (char)Keys.D) || (e.KeyChar == 100))
                {
                    comboBoxdestinobol.Focus();
                    timer1.Start();
                }
               else if ((e.KeyChar == (char)Keys.N) || (e.KeyChar == 110))
                {
                    textBoxnombre.Focus();
                    timer1.Start();
                }
                else
                {
                    Form mensaje = new Mensaje("Solo numeros", true);

                    mensaje.ShowDialog();
                    e.Handled = true;
                    return;
                }
            }
        }
        private void Debito_Click(object sender, EventArgs e)
        {
            foliotext.Focus();
            label17.Visible = false;
            label18.Visible = false;
            textBoxefectivocal.Visible = false;
            textBoxcambiocal.Visible = false;
            if (ventavalidar == true)
            {

                foliotarjeta = foliotext.Text;
                digitostarjeta = cuatrotext.Text;
                if (!string.IsNullOrEmpty(foliotarjeta) && !string.IsNullOrEmpty(digitostarjeta))
                {
                    groupBoxformadepago.Visible = false;
                    progressBar1.Visible = true;
                    progressBar1.Value = 90;
                    timerdebito.Start();

                }
                else
                {
                    Form mensaje = new Mensaje("Ingrese el folio y los ultimo 4 numero de la tarjeta", true);

                    mensaje.ShowDialog();
                }

            }
            else
            {
                folio.Visible = true;
                foliotext.Visible = true;
                cuatro.Visible = true;
                cuatrotext.Visible = true;
                Efectivo.Enabled = false;
                tcredito.Enabled = false;
                cuatrotext.Enabled = true;
                foliotext.Enabled = true;
                tdebito.Text = "Aceptar";
                formadepago = "T. Debito";
                ventavalidar = true;
                foliotext.Text = "";
                cuatrotext.Text = "";
                foliotext.Focus();
            }
            foliotext.Focus();
        }

        private void Credito_Click(object sender, EventArgs e)
        {
            foliotext.Focus();
            label17.Visible = false;
            label18.Visible = false;
            textBoxefectivocal.Visible = false;
            textBoxcambiocal.Visible = false;
            if (ventavalidar == true)
            {
                foliotarjeta = foliotext.Text;
                digitostarjeta = cuatrotext.Text;
                if (cuatrotext.TextLength == 4)
                {

                    if (!string.IsNullOrEmpty(foliotarjeta) && !string.IsNullOrEmpty(digitostarjeta))
                    {

                        groupBoxformadepago.Visible = false;
                        progressBar1.Visible = true;
                        progressBar1.Value = 90;

                        timercredito.Start();
                    }
                    else
                    {
                        Form mensaje = new Mensaje("Ingrese el folio y los ultimo 4 numero de la tarjeta", true);

                        mensaje.ShowDialog();
                    }
                }
                else
                {
                    Form mensaje = new Mensaje("Deben de ser 4 digitos", true);

                    mensaje.ShowDialog();
                }
            }
            else
            {
                folio.Visible = true;
                foliotext.Visible = true;
                cuatro.Visible = true;
                cuatrotext.Visible = true;
                Efectivo.Enabled = false;
                tdebito.Enabled = false;
                cuatrotext.Enabled = true;
                foliotext.Enabled = true;
                tcredito.Text = "Aceptar";
                formadepago = "T. Credito";
                ventavalidar = true;
                foliotext.Text = "";
                cuatrotext.Text = "";
                foliotext.Focus();
            }
            foliotext.Focus();
        }
        private void variables()
        {
            try
            {
                string sql = "SELECT VALOR FROM VARIABLES  WHERE NOMBRE='IVA'";
                db.PreparedSQL(sql);
         

                res = db.getTable();
                if (res.Next())
                {
                    IVA = res.GetInt("VALOR");
                    IVA = 100 + IVA;
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
                string funcion = "tarjetas";
                Utilerias.LOG.write(_clase, funcion, error);


            }


        }

        private void aceptarventa()
        {
            try
            {
                for (int i = 0; i < agregados.Rows.Count; i++)
                {





                    Bitmap rojo = new Bitmap(imagenrojo);   
                    _asiento = agregados.Rows[i].Cells[9].Value.ToString();

                     totalm= agregados.Rows[i].Cells["totalname"].Value.ToString();
                    _folio = agregados.Rows[i].Cells["folioname"].Value.ToString();
                    _pasaje2 = agregados.Rows[i].Cells["tarifaname"].Value.ToString();
                    _destinoboleto_= agregados.Rows[i].Cells["destinoboletoname"].Value.ToString();

                    nombre = agregados.Rows[i].Cells["pasajeroname"].Value.ToString();
                    string _p = agregados.Rows[i].Cells[20].Value.ToString();
                    string _pk = agregados.Rows[i].Cells[21].Value.ToString();
                    string _fila = agregados.Rows[i].Cells[15].Value.ToString();
                    string _columna = agregados.Rows[i].Cells[16].Value.ToString();
                    pkvendido= agregados.Rows[i].Cells["pkvendido1"].Value.ToString();
                    subtt = agregados.Rows[i].Cells["subtotalname"].Value.ToString();
                    ivatt = agregados.Rows[i].Cells["ivaname"].Value.ToString();
                    totaltt = agregados.Rows[i].Cells["totalname"].Value.ToString();




                    codigoqr(_folio);



                    string status = "VENDIDO";

                    string sql = "UPDATE  VENDIDOS SET STATUS=@STATUS,FOLIO=@FOLIO, MONTO=@MONTO, FORMADEPAGO=@FORMADEPAGO,FOLIOTARJETA=@FOLIOTARJETA,DIGITOSTARJETA=@DIGITOSTARJETA WHERE PK=@PK ";

                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@FOLIO", _folio);
                    db.command.Parameters.AddWithValue("@PK", pkvendido);

                    db.command.Parameters.AddWithValue("@STATUS", status);
                    db.command.Parameters.AddWithValue("@FORMADEPAGO", formadepago);
                    db.command.Parameters.AddWithValue("@DIGITOSTARJETA", digitostarjeta);
                    db.command.Parameters.AddWithValue("@FOLIOTARJETA", foliotarjeta);
                    db.command.Parameters.AddWithValue("@MONTO", totalm);


                    if (db.execute())
                    {
                       
                    Utilerias.LOG.acciones("vendio un boleto" + _folio);
                        imprimir();
                        _total = 0;

                        string firstText = _asiento;
                        PointF firstLocation = new PointF(1f, 4f);
                        using (Graphics graphics = Graphics.FromImage(rojo))
                        {
                            using (Font arialFont = new Font("Arial", 12))
                            {
                                graphics.DrawString(firstText, arialFont, Brushes.White, firstLocation);
                            }
                        }

                        if (_fila == "100")
                        {

                        }
                        else
                        {
                            if (_p == "1")
                            {
                                autobus.Rows[int.Parse(_fila)].Cells[int.Parse(_columna)].Value = rojo;
                            }
                            if (_p == "2")
                            {
                                PISOS2.Rows[int.Parse(_fila)].Cells[int.Parse(_columna)].Value = rojo;
                            }
                        }

                    }
                }

                getRowsVendidos();


                columnas.Clear();
                filas.Clear();
                etiquetass.Clear();
                agregados.Rows.Clear();
                textBoxtotal.Text = "$0";
                vender = false;
                textBoxnombre.Text = "Publico en General";
                textBoxcomando.Focus();
                textBoxcomando.Text = "";
                if (agregados.Rows.Count < 1)
                {
                    buttonbloquear.Enabled = false;
                    buttonbloquear.BackColor = Color.White;
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
                    MessageBox.Show("Ocurrio un Error en vender.");
                string funcion = "buttonactualizar";
                Utilerias.LOG.write(_clase, funcion, error);


            }

        }
        public void checkInternetAvaible()
        {
            if (!Utilerias.Utilerias.CheckForInternetConnection())
            {
                Form mensaje = new Mensaje("Error al conectarse al internet", true);

                mensaje.ShowDialog();
                return;
            }
        }
        public String GenerateRandom()
        {
            {
                string sPassword = "";
                try
                {
                     Random randomGenerate = new System.Random();

                    sPassword = Convert.ToString(randomGenerate.Next(00000001, 99999999));


                    //sPassword= sPassword.Substring(8);
                }
                catch (Exception err)
                {
                    string error = err.Message;
                    MessageBox.Show("Ocurrio un Error sumatoria, intente de nuevo.");
                    string funcion = "GenerateRandom";
                    Utilerias.LOG.write(_clase, funcion, error);


                }
                return sPassword;
            }
        }

        private Boolean validaragregar()
        {
            Boolean validado = true;
            try
            {
                if (comboBoxlinea.SelectedItem != null)
                {
                    _linea = comboBoxlinea.SelectedItem.ToString();

                    // _linea = (comboBoxlinea.SelectedItem as ComboboxItem).Value.ToString();

                }

                if (comboBoxorigen.SelectedItem != null)
                {
                    // _origen = (comboBoxorigen.SelectedItem as ComboboxItem).Value.ToString();

                    _origen = comboBoxorigen.SelectedItem.ToString();
                }
                if (comboBoxdestino.SelectedItem != null)
                {
                    // _destino = (comboBoxdestino.SelectedItem as ComboboxItem).Value.ToString();

                    _destino = comboBoxdestino.SelectedItem.ToString();
                }
                //if (comboBoxsalida.SelectedItem != null)
                //{
                //    //_salida = (comboBoxsalida.SelectedItem as ComboboxItem).Value.ToString();

                //    _fecha = comboBoxsalida.SelectedItem.ToString();
                //}
                if (comboBoxdestinobol.SelectedItem != null)
                {
                    // _salida = (comboBoxsalida.SelectedItem as ComboboxItem).Value.ToString();

                    _destinoboleto_ = comboBoxdestinobol.SelectedItem.ToString();
                }
                if (comboBoxtarifa.SelectedItem != null)
                {
                    // _tarifa = (comboBoxtarifa.SelectedItem as ComboboxItem).Value.ToString();

                    _pasaje = comboBoxtarifa.SelectedItem.ToString();
                }

                _pasajero = textBoxnombre.Text;

                if (_pasajero == "")
                {
                    _pasajero = "Publico en general";
                }
                if (string.IsNullOrEmpty(_linea))
                {
                    labellinea.Visible = true;
                    validado = false;

                }
                if (string.IsNullOrEmpty(_origen))
                {
                    labelorigen.Visible = true;
                    validado = false;

                }
                if (string.IsNullOrEmpty(_destino))
                {
                    labeldestino.Visible = true;
                    validado = false;

                }
                if (string.IsNullOrEmpty(_fecha))
                {
                    labelfecha.Visible = true;
                    validado = false;

                }

                if (string.IsNullOrEmpty(_destinoboleto_))
                {
                    labeldestinobol.Visible = true;
                    validado = false;

                }
                if (string.IsNullOrEmpty(_pasaje))
                {
                    labeltarifa.Visible = true;
                    validado = false;

                }
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error codigo qr, intente de nuevo.");
                string funcion = "validaragregar";
                Utilerias.LOG.write(_clase, funcion, error);


            }


            return validado;
        }

        private void codigoqr(string folio)
        {
            try
            {


                string valor = folio;
                QrEncoder qrEncoder = new QrEncoder(ErrorCorrectionLevel.H);
                QrCode qrCode = new QrCode();
                qrEncoder.TryEncode(valor, out qrCode);

                GraphicsRenderer renderer = new GraphicsRenderer(new FixedCodeSize(400, QuietZoneModules.Zero), Brushes.Black, Brushes.White);

                MemoryStream ms = new MemoryStream();

                renderer.WriteToStream(qrCode.Matrix, ImageFormat.Png, ms);
                var imageTemporal = new Bitmap(ms);
                imagen = new Bitmap(imageTemporal, new Size(new Point(100, 100)));
                // imagen.Save("imagen.png", ImageFormat.Png);

                // Guardar en el disco duro la imagen (Carpeta del proyecto)

            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un con codigo qr modificado, intente de nuevo.");
                string funcion = "codigoqr";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }
        private void codigobarra()
        {
            BarcodeLib.Barcode Codigobarras = new BarcodeLib.Barcode();
            Codigobarras.IncludeLabel = true;

        }

        void llenarticket(object sender, PrintPageEventArgs e)
        {
            try
            {
                BarcodeLib.Barcode Codigobarras = new BarcodeLib.Barcode();
                Codigobarras.IncludeLabel = true;
                Graphics g = e.Graphics;
                Graphics g2 = e.Graphics;

                // Create solid brush.
                SolidBrush blueBrush = new SolidBrush(Color.Black);

                // Create rectangle for region.
                Rectangle fillRect = new Rectangle(0, 21, 270, 26);

                // Create region for fill.
                Region fillRegion = new Region(fillRect);

                // Fill region to screen.
                // g.DrawRectangle(Pens.Black, 3, 7, 340, 700);
                //g.DrawImage(imagensplash, 0, 0);
                Font fBody9 = new Font("Agency FB", 9, FontStyle.Regular);
                Font fBody7 = new Font("Agency FB", 7, FontStyle.Regular);

                Font fBody = new Font("Agency FB", 8, FontStyle.Regular);
                Font fBody10 = new Font("Agency FB", 8, FontStyle.Italic);
                Font fBody12 = new Font("Agency FB", 9, FontStyle.Bold | FontStyle.Italic);
                Font fBody18 = new Font("Agency FB", 12, FontStyle.Bold | FontStyle.Italic);
                Font fBody5 = new Font("Agency FB", 6, FontStyle.Regular);
                Font fBody188 = new Font("Agency FB", 18, FontStyle.Bold | FontStyle.Italic);
                Font fBody16= new Font("Agency FB", 16, FontStyle.Bold | FontStyle.Italic);
                Font fBody15 = new Font("Agency FB", 16, FontStyle.Bold | FontStyle.Italic);

                Font fBody14fecha = new Font("Agency FB", 14, FontStyle.Bold | FontStyle.Italic);

                int espacio = 0;
                Color customColor = Color.FromArgb(255, Color.Black);
                Color customColor2 = Color.FromArgb(255, Color.White);
                g2.FillRegion(blueBrush, fillRegion);

                SolidBrush sb = new SolidBrush(customColor);
                SolidBrush sb2 = new SolidBrush(customColor2);

                g.DrawString("Fecha: " + DateTime.Now.ToShortDateString(), fBody14fecha, sb, 0, espacio);
                g.DrawString("Hora: " + DateTime.Now.ToShortTimeString(), fBody14fecha, sb, 175, espacio);
                espacio = espacio + 21;
                g.DrawString("Visitanos en ATAH.ONLINE", fBody188, sb2, 30, espacio);
                espacio = espacio + 30;
                g.DrawString("RUTA:", fBody10, sb, 0, espacio);
                g.DrawString("SERVICIO:", fBody10, sb, 80, espacio);
                g.DrawString("FOLIO:", fBody10, sb, 190, espacio);

                espacio = espacio + 15;
                g.DrawString(textBoxruta.Text, fBody18, sb, 0, espacio);
                g.DrawString(_linea, fBody18, sb, 80, espacio);
                g.DrawString(_folio, fBody18, sb, 190, espacio);

                espacio = espacio + 20;
                g.DrawString("ORIGEN:", fBody10, sb, 0, espacio);
                g.DrawString("DESTINO:", fBody10, sb, 100, espacio);

                espacio = espacio + 15;
                g.DrawString(_origen, fBody14fecha, sb, 0, espacio);
                g.DrawString(_destinoboleto_, fBody14fecha, sb, 100, espacio);

                espacio = espacio + 20;
                g.DrawString("SALIDA:", fBody10, sb, 0, espacio);
                g.DrawString("LLEGADA APROX.:", fBody10, sb, 150, espacio);

                espacio = espacio + 15;
                g.DrawString(_hora, fBody14fecha, sb, 0, espacio);
                g.DrawString(_llegada, fBody14fecha, sb, 150, espacio);

                espacio = espacio + 25;
                g.DrawString("AUTOBUS:", fBody10, sb, 0, espacio);
                g.DrawString("ASIENTO:", fBody10, sb, 100, espacio);
                g.DrawString("TIPO DE PASAJE:", fBody10, sb, 170, espacio);

                espacio = espacio + 15;
                g.DrawString(ECO, fBody14fecha, sb, 0, espacio);
                g.DrawString(_asiento, fBody14fecha, sb, 100, espacio);
                g.DrawString(_pasaje2, fBody16, sb, 170, espacio);

                espacio = espacio + 25;

                g.DrawString("NOMBRE:", fBody10, sb, 0, espacio);



                Font fBody13 = new Font("Agency FB", 13, FontStyle.Regular | FontStyle.Italic);

                espacio = espacio + 15;
                g.DrawString("SUBTOTAL:", fBody12, sb, 160, espacio);
                g.DrawString(subtt, fBody13, sb, 220, espacio);

                nombre = _pasajero;
                g.DrawString(nombre, fBody14fecha, sb, 0, espacio);
                g.DrawString("IVA:", fBody12, sb, 185, espacio+19);
                g.DrawString(ivatt, fBody13, sb, 219, espacio+17);
                espacio = espacio + 25;
                g.DrawString("TIPO DE PAGO:", fBody10, sb, 00, espacio);
                g.DrawString("TOTAL:", fBody14fecha, sb, 155, espacio+10);
                g.DrawString(totaltt, fBody14fecha, sb, 200, espacio+10);
                espacio = espacio + 15;
       
                g.DrawString(formadepago, fBody14fecha, sb, 00, espacio);
                espacio = espacio + 25;


                g.DrawImage(Codigobarras.Encode(BarcodeLib.TYPE.CODE128, _folio, Color.Black, Color.White, 250, 50), 0, espacio);
                espacio = espacio + 55;



                g.DrawImage(imagen, 2, espacio);
                espacio = espacio + 5;

                Font fBody8blod = new Font("Agency FB", 8, FontStyle.Bold);


                g.DrawString("PRESENTE IDENTIFICACIÓN ORIGINAL Y ", fBody9, sb, 120, espacio);
                espacio = espacio + 15;
                g.DrawString("VIGENTE AL MOMENTO DE ABORDAR ", fBody9, sb, 125, espacio);
                espacio = espacio + 15;
         
                g.DrawString("Linea de atención quejas y sugerencias: ", fBody, sb, 125, espacio);
                espacio = espacio + 20;
                g.DrawString("‎01 800 836 0726", fBody8blod, sb, 160, espacio);
                Font fbody5bold = new Font("Agency FB", 5, FontStyle.Bold);

                espacio = espacio + 15;
                g.DrawString("Consulta términos y condiciónes, facturación en:", fBody, sb, 110, espacio);
                espacio = espacio + 12;
                g.DrawString("   www.atah.online", fBody8blod, sb, 145, espacio);
                espacio = espacio + 23;
                int espaciotempo = 5;
                g.DrawString("---------------------------------------------------------------------------------------------------- ", fBody12, sb, espaciotempo, espacio);
                espacio = espacio + 15;
                g.DrawString("LA PRESENTACION DE ESTE BOLETO ES INDISPENSABLE PARA VIAJAR.", fBody7, sb, espaciotempo, espacio);
                espacio = espacio + 12;
                g.DrawString("DOCUMENTE SU EQUIPAJE(UNO O VARIOS) DEL PASAJERO, SE ACEPTARA SIN COSTO,", fBody7, sb, espaciotempo, espacio);
                espacio = espacio + 12;
                g.DrawString("HASTA 25 KG.", fBody7, sb, espaciotempo, espacio);
                espacio = espacio + 12;
                g.DrawString("PARA ABORDAR EL AUTOBUS, FAVOR DE PRESENTARSE 5 MINUTOS ANTES DE LA HORA", fBody7, sb, espaciotempo, espacio);
                espacio = espacio + 12;
                g.DrawString("DE SALIDA INDICADA EN EL BOLETO.", fBody7, sb, espaciotempo, espacio);
                espacio = espacio + 12;
                g.DrawString("SE PODRA CANCELAR EL BOLETO HASTA 60 MINUTOS ANTES DE LA SALIDA DE LA CORRIDA", fBody7, sb, espaciotempo, espacio);
                espacio = espacio + 12;
                g.DrawString("QUE AMPARE, SIN COSTO ALGUNO PARA EL PASAJERO, EN LA MISMA FORMA EN LA QUE  ", fBody7, sb, espaciotempo, espacio);
                espacio = espacio + 12;
                g.DrawString("SE REALIZO EL PAGO.", fBody7, sb, espaciotempo, espacio);
                espacio = espacio + 12;
                g.DrawString("EN CASO DE PERDIDA DE EQUIPAJE REGISTRADO O DOCUMENTADO LA EMPRESA", fBody7, sb, espaciotempo, espacio);
                espacio = espacio + 12;
                g.DrawString("PAGARA HASTA 20 DIAS SMGV POR PASAJERO, INDEPENDIENTEMENTE DEL NUMERO ", fBody7, sb, espaciotempo, espacio);
                espacio = espacio + 12;
                g.DrawString("DE PIEZAS, CONTRA ENTREGA DE LA CONTRASEÑA DE DOCUMENTACION DEL EQUIPAJE.", fBody7, sb, espaciotempo, espacio);
                espacio = espacio + 12;
                g.DrawString("INCLUYE SEGURO DE VIAJERO, RESPONSABILIDAD CIVIL 3160 DSMVDF, POR PASAJERO ", fBody7, sb, espaciotempo, espacio);
                espacio = espacio + 12;
                g.DrawString("Y 3,500,00.00 (TRES MILLONES QUINIENTOS MIL PESOS CERO CENTAVOSMONEDA NACIONAL),", fBody7, sb, espaciotempo, espacio);
                espacio = espacio + 12;
                g.DrawString("POR RESPONSABILIDAD CIVIL. ", fBody7, sb, espaciotempo, espacio);
                espacio = espacio + 12;
                g.DrawString("CONFORME A LAS DISPOSICIONES FISCALES VIGENTES, ART. 1 FRACCION II Y ART. 14 ", fBody7, sb, espaciotempo, espacio);
                espacio = espacio + 12;
                g.DrawString("FRACCION II DE LA LEY DEL IVA, A PARTIR DE 01 DE ENERO DEL 2014 ", fBody7, sb, espaciotempo, espacio);
                espacio = espacio + 12;
               
                g.DrawString("EL SERVICIO DE TRANSPORTE GRAVA IVA.", fBody7, sb, espaciotempo, espacio);
                espacio = espacio + 12;

                g.DrawString("COMPROBANTE VALIDO PARA FACTURACIÓN. ", fBody7, sb, espaciotempo, espacio);
    
                g.Dispose();
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Impresora no conectada.");
                string funcion = "llenarticket";
                Utilerias.LOG.write(_clase, funcion, error);


            }



        }
        //void llenarticket(object sender, PrintPageEventArgs e)
        //{
        //    try
        //    {
        //        BarcodeLib.Barcode Codigobarras = new BarcodeLib.Barcode();
        //        Codigobarras.IncludeLabel = true;
        //        Graphics g = e.Graphics;
        //        Graphics g2 = e.Graphics;

        //        // Create solid brush.
        //        SolidBrush blueBrush = new SolidBrush(Color.Black);

        //        // Create rectangle for region.
        //        Rectangle fillRect = new Rectangle(0, 12, 270, 26);

        //        // Create region for fill.
        //        Region fillRegion = new Region(fillRect);

        //        // Fill region to screen.
        //        // g.DrawRectangle(Pens.Black, 3, 7, 340, 700);
        //        //g.DrawImage(imagensplash, 0, 0);
        //        Font fBody9 = new Font("Agency FB", 9, FontStyle.Regular);
        //        Font fBody7 = new Font("Agency FB", 7, FontStyle.Regular);

        //        Font fBody = new Font("Agency FB", 8, FontStyle.Regular);
        //        Font fBody10 = new Font("Agency FB", 10, FontStyle.Italic);
        //        Font fBody12 = new Font("Agency FB", 12, FontStyle.Bold | FontStyle.Italic);
        //        Font fBody18 = new Font("Agency FB", 18, FontStyle.Bold | FontStyle.Italic);
        //        Font fBody5 = new Font("Agency FB", 6, FontStyle.Regular);

        //        int espacio = 0;
        //        Color customColor = Color.FromArgb(255, Color.Black);
        //        Color customColor2 = Color.FromArgb(255, Color.White);
        //        g2.FillRegion(blueBrush, fillRegion);

        //        SolidBrush sb = new SolidBrush(customColor);
        //        SolidBrush sb2 = new SolidBrush(customColor2);

        //        g.DrawString("Fecha: " + DateTime.Now.ToShortDateString(), fBody, sb, 0, espacio);
        //        g.DrawString("Hora: " + DateTime.Now.ToShortTimeString(), fBody, sb, 190, espacio);
        //        espacio = espacio + 10;
        //        g.DrawString("Visitanos en ATAH.COM", fBody18, sb2, 30, espacio);
        //        espacio = espacio + 40;
        //        g.DrawString("VALIDO PARA:", fBody12, sb, 80, espacio);
        //        espacio = espacio + 15;
        //        g.DrawString("MARCA:", fBody10, sb, 0, espacio);
        //        g.DrawString("SERVICIO:", fBody10, sb, 160, espacio);

        //        espacio = espacio + 15;
        //        g.DrawString("ATAH", fBody12, sb, 0, espacio);
        //        g.DrawString(_linea, fBody12, sb, 160, espacio);

        //        espacio = espacio + 25;
        //        g.DrawString("ORIGEN:", fBody10, sb, 0, espacio);
        //        g.DrawString("DESTINO:", fBody10, sb, 160, espacio);

        //        espacio = espacio + 15;
        //        g.DrawString(_origen, fBody12, sb, 0, espacio);
        //        g.DrawString(_destinoboleto_, fBody12, sb, 160, espacio);

        //        espacio = espacio + 25;
        //        g.DrawString("SALIDA:", fBody10, sb, 0, espacio);
        //        g.DrawString("ASIENTO:", fBody10, sb, 160, espacio);

        //        espacio = espacio + 15;
        //        g.DrawString(_hora, fBody12, sb, 0, espacio);
        //        g.DrawString(_asiento, fBody12, sb, 160, espacio);

        //        espacio = espacio + 25;
        //        g.DrawString("AUTOBUS:", fBody10, sb, 0, espacio);
        //        g.DrawString("RUTA:", fBody10, sb, 160, espacio);

        //        espacio = espacio + 15;
        //        g.DrawString(ECO, fBody12, sb, 0, espacio);
        //        g.DrawString(textBoxruta.Text, fBody12, sb, 160, espacio);

        //        espacio = espacio + 25;

        //        g.DrawString("LLEGADA:", fBody10, sb, 0, espacio);
        //        g.DrawString("PRECIO TOTAL:", fBody10, sb, 160, espacio);

        //        espacio = espacio + 15;
        //        g.DrawString(_llegada, fBody12, sb, 0, espacio);
        //        g.DrawString("$" + tot.ToString(), fBody12, sb, 160, espacio);

        //        espacio = espacio + 25;
        //        g.DrawString("FOLIO:", fBody10, sb, 0, espacio);
        //        g.DrawString("TIPO DE PAGO:", fBody10, sb, 160, espacio);

        //        espacio = espacio + 15;
        //        g.DrawString(_folio, fBody12, sb, 0, espacio);
        //        g.DrawString(formadepago, fBody12, sb, 160, espacio);

        //        espacio = espacio + 25;
        //        g.DrawString("NOMBRE:", fBody10, sb, 0, espacio);
        //        g.DrawString("CONDUCTOR:", fBody10, sb, 160, espacio);

        //        espacio = espacio + 15;
        //        g.DrawString(textBoxnombre.Text, fBody12, sb, 0, espacio);
        //        g.DrawString(chofer, fBody12, sb, 160, espacio);

        //        espacio = espacio + 25;

        //        g.DrawString("TIPO DE PASAJE:", fBody10, sb, 100, espacio);
        //        espacio = espacio + 15;
        //        g.DrawString(_pasaje, fBody12, sb, 100, espacio);
        //        espacio = espacio + 25;


        //        g.DrawImage(Codigobarras.Encode(BarcodeLib.TYPE.CODE128, _folio, Color.Black, Color.White, 250, 50), 0, espacio);
        //        espacio = espacio + 55;
        //        g.DrawString("CONSERVE SU BOLETO, ES SU SEGURO DE VIAJERO", fBody, sb, 20, espacio);
        //        espacio = espacio + 15;
        //        g.DrawString("Su boleto es su seguro de viajero, Válido para la fecha y hora indicada.", fBody7, sb, 0, espacio);
        //        espacio = espacio + 9;
        //        g.DrawString("Autotransportes tlaxala, apizaco, huamantla, S.A. DE C.V. no es la transportista", fBody7, sb, 0, espacio);
        //        espacio = espacio + 9;
        //        g.DrawString("Ni presta el servicio por lo que no existe ninguna obligacion en común con la ", fBody7, sb, 0, espacio);
        //        espacio = espacio + 9;
        //        g.DrawString("transportista. Mención sólo para efectos fiscales en terminos del Art. 72 de la  ", fBody7, sb, 0, espacio);
        //        espacio = espacio + 9;
        //        g.DrawString("leydel ISR. Cualquier derecho u obligacion relacionados. directamente con la  ", fBody7, sb, 0, espacio);

        //        espacio = espacio + 9;
        //        g.DrawString("de este servicio incluyendo pagos e indemnizaciones se regirán y resolverán ", fBody7, sb, 0, espacio);
        //        espacio = espacio + 9;
        //        g.DrawString("conforme a la legislacion aplicable y con los tribunales competentes del .", fBody7, sb, 0, espacio);
        //        espacio = espacio + 9;
        //        g.DrawString("fuero común, renunciando a cualquier otra Ley o juridicción o competencia,", fBody7, sb, 0, espacio);
        //        espacio = espacio + 9;
        //        g.DrawString("nacional o extranjera que pudiese corresponder por domicilio de las partes, ", fBody7, sb, 0, espacio);
        //        espacio = espacio + 9;
        //        g.DrawString("nacionalidad o por otra causa. El transportista no responderaá por culpa o  ", fBody7, sb, 0, espacio);
        //        espacio = espacio + 9;
        //        g.DrawString("negligencia de la victima, caso fortuito ni fuera mayor ni por culpa de terceros. ", fBody7, sb, 0, espacio);
        //        espacio = espacio + 9;
        //        g.DrawString("dom. y Admon. que tiene la transportista para todo los efectos.Boleto cancelable ", fBody7, sb, 0, espacio);
        //        espacio = espacio + 9;

        //        g.DrawString(" hasta 60 min. antes de la hora de la salida del viaje, comprado en ATAH.COM y ", fBody7, sb, 0, espacio);
        //        espacio = espacio + 9;
        //        g.DrawString("ATAH movil con promocion o compra anticipada (debido a tarifa preferente a  ", fBody7, sb, 0, espacio);
        //        espacio = espacio + 9;
        //        g.DrawString("la compra y 30 min. antes de la hora de salida del viaje por  ", fBody7, sb, 0, espacio);
        //        espacio = espacio + 9;
        //        g.DrawString("fecha/hora/origen/destino, pagando la diferencia a tarifa disponible vigente.", fBody7, sb, 0, espacio);
        //        espacio = espacio + 9;
        //        g.DrawString(" Hasta 25 kg. de equipaje sin costo. ", fBody7, sb, 0, espacio);
        //        espacio = espacio + 9;

        //        g.DrawString("En caso de pérdida se paga hasta 50 UMAS por pasajero y contra entrega  ", fBody7, sb, 0, espacio);
        //        espacio = espacio + 9;

        //        g.DrawString("de contraseña del equipaje o articulos olvidados. Dudas, quejas o ", fBody7, sb, 0, espacio);
        //        espacio = espacio + 9;
        //        g.DrawString("sujerencias escribe a atah.com, facturacion con tus datos fiscales en ", fBody7, sb, 0, espacio);
        //        espacio = espacio + 9;
        //        g.DrawString("ATAH.com (exepto boletos manual) y en taquilla de las principales terminales.", fBody7, sb, 0, espacio);
        //        espacio = espacio + 9;
        //        g.DrawString("Terminos y condiciones en Atah.com/terminosycondiciones.", fBody7, sb, 0, espacio);
        //        espacio = espacio + 25;

        //        g.DrawImage(imagen, 75, espacio);
        //        espacio = espacio + 110;

        //        g.DrawString("PRESENTE IDENTIFICACIÓN ORIGENAL Y ", fBody12, sb, 10, espacio);
        //        espacio = espacio + 15;
        //        g.DrawString("VIGENTE AL MOMENTO DE ABORDAR ", fBody12, sb, 15, espacio);
        //        espacio = espacio + 20;
        //        g.DrawString("SERVICIO DE ASISTENCIA TOTAL ", fBody10, sb, 45, espacio);
        //        espacio = espacio + 20;
        //        g.DrawString("La transportista no es responsable de este servicio", fBody, sb, 20, espacio);
        //        espacio = espacio + 15;
        //        g.DrawString("Atención a clientes: 01 800 0230 232", fBody, sb, 40, espacio);
        //        espacio = espacio + 15;
        //        g.DrawString("Consulta terminos y condiciónes en:", fBody, sb, 50, espacio);
        //        espacio = espacio + 15;
        //        g.DrawString("Atah.com/terminosycondicones/", fBody, sb, 55, espacio);

        //        g.Dispose();
        //    }
        //    catch (Exception err)
        //    {
        //        string error = err.Message;
        //        MessageBox.Show("Impresora no conectada.");
        //        string funcion = "llenarticket";
        //        Utilerias.LOG.write(_clase, funcion, error);


        //    }



        //}

        private void Buttonimprimir_Click(object sender, EventArgs e)
        {
            
            try
            {



                PrintDocument pd = new PrintDocument();
                PaperSize ps = new PaperSize("", 420, 900);
                pd.PrintPage += new PrintPageEventHandler(llenarticket2);
                pd.PrintController = new StandardPrintController();

                pd.DefaultPageSettings.Margins.Left = 0;
                pd.DefaultPageSettings.Margins.Right = 0;
                pd.DefaultPageSettings.Margins.Top = 0;
                pd.DefaultPageSettings.Margins.Bottom = 0;
                pd.DefaultPageSettings.PaperSize = ps;


                pd.PrinterSettings.PrinterName = Settings1.Default.impresora;
                pd.Print();
                CrearTicket ticket = new CrearTicket();
                ticket.TextoIzquierda("");
          
                ticket.CortaTicket();
                ticket.ImprimirTicket(Settings1.Default.impresora);

            }

            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("No se detecto impresora.");
                string funcion = "buttonimprimir";
                Utilerias.LOG.write(_clase, funcion, error);


            }

        }

        private void imprimir()
        {
            try
            {
                if (Settings1.Default.impresora == "Ninguna")
                {
                    Form mensaje = new Mensaje("Configura la impresora por favor", true);

                    mensaje.ShowDialog();
                }
                else
                {

                    PrintDocument pd = new PrintDocument();
                    PaperSize ps = new PaperSize("", 420, 850);
                    pd.PrintPage += new PrintPageEventHandler(llenarticket);
                    pd.PrintController = new StandardPrintController();

                    pd.DefaultPageSettings.Margins.Left = 0;
                    pd.DefaultPageSettings.Margins.Right = 0;
                    pd.DefaultPageSettings.Margins.Top = 0;
                    pd.DefaultPageSettings.Margins.Bottom = 0;
                    pd.DefaultPageSettings.PaperSize = ps;
                    pd.PrinterSettings.PrinterName = Settings1.Default.impresora;

                 

                    pd.Print();
                    CrearTicket ticket = new CrearTicket();
                    ticket.TextoIzquierda("");
                    ticket.TextoIzquierda("");
                    ticket.TextoIzquierda("");
                    ticket.TextoIzquierda("");
                    ticket.TextoIzquierda("");
                    ticket.TextoIzquierda("");

                    ticket.CortaTicket();
                    ticket.ImprimirTicket(Settings1.Default.impresora);
                }
            }

            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Impresora no conectada.");
                string funcion = "imprimir";
                Utilerias.LOG.write(_clase, funcion, error);


            }

        }





        private void toolStripCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private bool CheckOpened(string name)
        {

            Form form = null;

            FormCollection fc = Application.OpenForms;

            foreach (Form frm in fc)
            {
                if (frm.Text == name)
                {
                    form = frm;
                    return true;
                }
            }
            return false;
        }

        Form form;
        private bool vender;


        private void limpiarpreventatotal()

        {
            try
            {
                pie = true;
                if (_hora != null && ECO != null)
                {
                    agregados.Rows.Clear();

                    string sql3 = "DELETE FROM VENDIDOS  WHERE  (SALIDA  BETWEEN  SALIDA AND CONVERT (datetime,@LLEGADA ) ) AND " +
                    "(LLEGADA  BETWEEN  CONVERT(datetime, @SALIDA) AND LLEGADA) AND LINEA=@LINEA  AND FECHA=@FECHA AND ECO=@ECO  AND (STATUS='PREVENTA' OR STATUS='BLOQUEADO') ";

                    db.PreparedSQL(sql3);
                    db.command.Parameters.AddWithValue("@SALIDA", _hora);
                    db.command.Parameters.AddWithValue("@LINEA", _linea);
                    db.command.Parameters.AddWithValue("@LLEGADA", _llegada);
                    db.command.Parameters.AddWithValue("@FECHA", _fecha);
                    db.command.Parameters.AddWithValue("@ECO", ECO);
                    db.execute();
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
                    MessageBox.Show("Ocurrio un Error, verifique la configuracion de corridas por que no se puede obtener los precios.");
                string funcion = "limpiarpreventa";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }


        private void GUIA_Click(object sender, EventArgs e)
        {
            try
            {
                Form mensaje2 = new Mensaje("¿Está seguro de generar la guia? se bloquera la venta para esta corrida", false);

                DialogResult resut = mensaje2.ShowDialog();
                if (resut == DialogResult.OK)
                {
                    limpiarpreventatotal();
                    foliotarjeta = " ";
                    digitostarjeta = " ";
                    autobus.Enabled = true;
                    PISOS2.Enabled = true;
                    groupBoxformadepago.Visible = false;
                    if (puedegenerarguia == true)
                    {

                        if (CheckOpened("GUIA"))
                        {
                            form.StartPosition = FormStartPosition.CenterScreen;
                            form.Show();

                        }
                        else
                        {
                            verificationUserControl2.Start();
                            verificationUserControl2.Stop();

                            verificationUserControl2.Hide();
                            form = new GUIA(_destino, _origen, _hora, _linea, ECO, int.Parse(pk), chofer);
                            AddOwnedForm(form);
                            form.StartPosition = FormStartPosition.CenterScreen;
                            form.Show();
                            limpiar();
                            Utilerias.LOG.acciones("ingreso a guia ");


                        }

                    }
                    else
                    {
                        Form mensaje = new Mensaje("No tienes permiso para generar guia", true);

                        mensaje.ShowDialog();
                    }
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
                string funcion = "guiaclick";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }
        public void closeguia()
        {
            form.Close();
        }

        private void totalsumatori()
        {
            try
            {
                 valor = 0;
                for (int i = 0; i < agregados.RowCount; i++)
                {
                    char[] MyChar = { '$',' ' };

                    string NewString = agregados.Rows[i].Cells[8].Value.ToString().TrimStart(MyChar);

                    valor += float.Parse(NewString);

                }
                textBoxtotal.Text = Utilerias.Utilerias.formatCurrency(valor);
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "totalsumatori";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }
        // Check for possible trouble states of a print job using its properties
       
        private void Datagridvendidos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
           

            int n= e.RowIndex;
            int c = e.ColumnIndex;
            if (n != -1)
            {
                string tipo = datagridvendidos.Rows[n].Cells["STATUS"].Value.ToString();
                if (tipo == "CANCELADO" || tipo == "PREVENTA")
                {

                    buttonborrar.Enabled = false;
                    buttonborrar.BackColor = Color.White;
                    buttonborrar.ForeColor = Color.Black;
                    buttonreimprimir.Enabled = false;
                    buttonreimprimir.BackColor = Color.White;
                    buttonreimprimir.ForeColor = Color.Black;



                }
                else
                {
                    buttonborrar.Enabled = true;
                    buttonborrar.BackColor = Color.FromArgb(163, 17, 18);
                    nfila = e.RowIndex;

              

                }
            }

        }


        private void Buttonborrar_Click(object sender, EventArgs e)
        {
            try
            {
                string foli = (string)datagridvendidos.Rows[nfila].Cells[0].Value;
                textBoxmotivo.Focus();
                GUIACOMPROBACION();
                if (validar == "True")
                {
                    Form mensaje = new Mensaje("La guia ya se cerro", true);

                    mensaje.ShowDialog();
                    limpiar();
                    agregados.Rows.Clear();

                }
                if (validar2 == "True")
                {
                    Form mensaje = new Mensaje("La guia esta bloqueada", true);

                    mensaje.ShowDialog();
                    limpiar();
                    agregados.Rows.Clear();

                }


                if (puedecancelar == true)
                {
                    textBoxcontraseña.Visible = false;
                    verificationUserControl2.Visible = true;
                    textBoxmotivo.Text = "";
                    groupBoxhuella.Visible = true;
                    verificationUserControl2.Visible = true;
                    verificationUserControl2.limpiarhuella();

                    ValidateUser();
                    textBoxmotivo.Focus();

                }
                else
                {
                    Form mensaje = new Mensaje("No puedes cancelar el boleto", true);

                    mensaje.ShowDialog();
                }

            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "buttonborrar";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }

        private void cancelacionmotivo()
        {
        
       

        }
        public void cerrarguia()
        {
            if (form.InvokeRequired)
            {
                form.Invoke(new Action(() =>
                {
                    form.Close();
                }
                ));
            }
            else
            {
                form.Close();

            }
        }

        private void permisos()
        {

                if (LoginInfo.privilegios.Any(x => x == "Cancelar Boletos"))
                {
                    puedecancelar = true;

                }
            if (LoginInfo.privilegios.Any(x => x == "Generar guia"))
            {
                puedegenerarguia = true;

            }
            if (LoginInfo.privilegios.Any(x => x == "Todos los origenes"))
            {
                todoslosorigenes = true;

            }


        }

        private void cancelarentabla()
        {
            try
                {

                string foli = (string)datagridvendidos.Rows[nfila].Cells["pkvendidosname"].Value;

                string sql = "UPDATE VENDIDOS SET STATUS=@STATUS,MOTIVOCANCELADO=@MOTIVOCANCELADO,CANCELADO=@VENDEDOR,CORTECANCELADO=0  WHERE PK=@PK";


                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@VENDEDOR", vendedoractual);
                db.command.Parameters.AddWithValue("@STATUS", "CANCELADO");
                db.command.Parameters.AddWithValue("@PK", foli);
                db.command.Parameters.AddWithValue("@MOTIVOCANCELADO", textBoxmotivo.Text);


                if (db.execute())
                {
                    limpiarpreventa();
                    Utilerias.LOG.acciones("cancelo  de preventa un boleto " + foli);

                    getRowsVendidos();
                    groupBoxhuella.Visible = false;
                    verificationUserControl2.limpiarhuella();
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
                    MessageBox.Show("Ocurrio un Error al cancelar venta.");

                string funcion = "cancelarentabla";
                Utilerias.LOG.write(_clase, funcion, error);


            }
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




        private void preventa(bool cantidadepisos)
        {
            try
                {
                string tipo = "";
                if (cantidadepisos)
                 tipo = matriztipo[_fila, _columna];
                else
                 tipo = matriztipo2[_fila, _columna];
                int corte = 0;
                string horacorta = comboBoxsalida.SelectedItem.ToString();
                string sql = "INSERT INTO VENDIDOS(FOLIO,LINEA,ORIGEN,FECHA,DESTINO,DESTINOBOLETO,TARIFA,HORA,PRECIO,ASIENTO,PASAJERO,STATUS,FILA,COLUMNA,VENDEDOR,SUCURSAL,ECO,LLEGADA,SALIDA,PISOS,CORTE,PKCORRIDA,RUTA,CONDUCTOR,TIPO)" +
                                   " VALUES(@FOLIO,@LINEA,@ORIGEN,@FECHA,@DESTINO,@DESTINOBOLETO,@TARIFA,@HORA,@PRECIO,@ASIENTO,@PASAJERO,@STATUS,@FILA,@COLUMNA,@VENDEDOR,@SUCURSAL,@ECO,@LLEGADA,@SALIDA,@PISOS,@CORTE,@PKCORRIDA,@RUTA,@CONDUCTOR,@TIPO)";

                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@FOLIO", _folio);
                db.command.Parameters.AddWithValue("@LINEA", _linea);
                db.command.Parameters.AddWithValue("@ORIGEN", _origen);
                db.command.Parameters.AddWithValue("@FECHA", _fecha);
                db.command.Parameters.AddWithValue("@DESTINO", _destino);
                db.command.Parameters.AddWithValue("@DESTINOBOLETO", _destinoboleto_);
                db.command.Parameters.AddWithValue("@TARIFA", _pasaje);
                db.command.Parameters.AddWithValue("@PRECIO",tot);
                db.command.Parameters.AddWithValue("@ASIENTO", etiqueta);
                db.command.Parameters.AddWithValue("@HORA", horacorta);
                db.command.Parameters.AddWithValue("@PASAJERO", _pasajero);
                db.command.Parameters.AddWithValue("@STATUS", _statustemporal);
                db.command.Parameters.AddWithValue("@FILA", _fila);
                db.command.Parameters.AddWithValue("@COLUMNA", _columna);
                db.command.Parameters.AddWithValue("@VENDEDOR", LoginInfo.UserID);
                db.command.Parameters.AddWithValue("@SUCURSAL", _origen);
                db.command.Parameters.AddWithValue("@ECO", ECO);
                db.command.Parameters.AddWithValue("@LLEGADA", _llegada);
                db.command.Parameters.AddWithValue("@SALIDA", _hora);
                db.command.Parameters.AddWithValue("@PISOS", _npiso);
                db.command.Parameters.AddWithValue("@CORTE", 0);
                db.command.Parameters.AddWithValue("@PKCORRIDA", pk);
                db.command.Parameters.AddWithValue("@RUTA", rutapasajero);
                db.command.Parameters.AddWithValue("@CONDUCTOR", chofer);
                db.command.Parameters.AddWithValue("@TIPO", tipo);

                string PKVENDIDO = db.executeId();

                if (!string.IsNullOrEmpty(PKVENDIDO))
                {
                     string sql2 = "UPDATE  VENDIDOS SET FOLIO=@FOLIO WHERE PK=@PK ";

                    db.PreparedSQL(sql2);
                    string folionuevo = PKVENDIDO;
                    if (PKVENDIDO.Length < 10)

                    {

                        folionuevo = PKVENDIDO.PadLeft(8, '0');

                    }
                    db.command.Parameters.AddWithValue("@PK", PKVENDIDO);

                    db.command.Parameters.AddWithValue("@FOLIO", folionuevo);
                    db.execute();
                    _folio = folionuevo;

                }
                columnas.Clear();
                filas.Clear();
                etiquetass.Clear();

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
                    MessageBox.Show("Ocurrio un Error al apartar lugar.");
                checkInternetAvaible();
                string funcion = "preventa";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            formadepago = "Efectivo";
            buttonactualizar.Enabled = false;
            foliotarjeta = " ";
            digitostarjeta = " ";
            groupBoxformadepago.Visible = false;
            limpiarpreventa();
            agregados.Rows.Clear();
            llenarautobus();
            sabercambio();
            buttonactualizar.Enabled = true;

        }


        private void Ventas_FormClosing(object sender, FormClosingEventArgs e)
        {
            limpiarpreventa();
        }
        private void verificartarifa(){

            try
            {

                string sql = "if (SELECT PERMITIDOS FROM TIPODEPASAJE WHERE PASAJE=@TARIFA and PKLINEA=@LINEA AND NOT PERMITIDOS=0)" +
                    "<= (select COUNT(PK) from vendidos where PKCORRIDA=@PK and tarifa = @TARIFA) " +
                    "BEGIN select DATO = 1 END ELSE " +
                    "BEGIN select DATO = 0 END";
                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@TARIFA", _pasaje);
                db.command.Parameters.AddWithValue("@PK", pk);
                db.command.Parameters.AddWithValue("@LINEA", pklinea);


                res = db.getTable();
                if (res.Next())
                {
                     dato= res.GetInt("DATO");
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
                    MessageBox.Show("Ocurrio un Error al apartar lugar.");
                checkInternetAvaible();
                string funcion = "preventa";
                Utilerias.LOG.write(_clase, funcion, error);


            }

        }

        public void executaProcedimientopreventa(string llegadp, string salidap, string lineap, string ecop, string fechap,
          string etiquetap, string tarifap, string origenp, string destinop, string destinoboletop, string horacortap, string totalp,
          string pasajerop, string statusp, string filap, string columnap, string usuariop,
         string npisosp, string cortep, string pkcorridap, string rutap, string choferp, string tipop)
        {
            try
            {
                string sql = "SP_VENTA_2";
                List<Parametros> lista = new List<Parametros>();
                Parametros pa = new Parametros();
                pa.nombreParametro = "@LLEGADA";
                pa.tipoParametro = SqlDbType.DateTime;
                pa.direccion = ParameterDirection.Input;
                pa.value = llegadp;
                lista.Add(pa);

                pa = new Parametros();
                pa.nombreParametro = "@SALIDA";
                pa.tipoParametro = SqlDbType.DateTime;
                pa.direccion = ParameterDirection.Input;
                pa.value = salidap;
                lista.Add(pa);

                pa = new Parametros();
                pa.nombreParametro = "@LINEA";
                pa.tipoParametro = SqlDbType.VarChar;
                pa.direccion = ParameterDirection.Input;
                pa.value = lineap;
                lista.Add(pa);

                pa = new Parametros();
                pa.nombreParametro = "@ECO";
                pa.tipoParametro = SqlDbType.BigInt;
                pa.direccion = ParameterDirection.Input;
                pa.value = ecop;
                lista.Add(pa);

                pa = new Parametros();
                pa.nombreParametro = "@FECHA";
                pa.tipoParametro = SqlDbType.VarChar;
                pa.direccion = ParameterDirection.Input;
                pa.value = fechap;
                lista.Add(pa);
                pa = new Parametros();
                pa.nombreParametro = "@ASIENTO";
                pa.tipoParametro = SqlDbType.VarChar;
                pa.direccion = ParameterDirection.Input;
                pa.value = etiquetap;
                lista.Add(pa);
                pa = new Parametros();
                pa.nombreParametro = "@TARIFA";
                pa.tipoParametro = SqlDbType.VarChar;
                pa.direccion = ParameterDirection.Input;
                pa.value = _pasaje;
                lista.Add(pa);
                pa = new Parametros();
                pa.nombreParametro = "@ORIGEN";
                pa.tipoParametro = SqlDbType.VarChar;
                pa.direccion = ParameterDirection.Input;
                pa.value = origenp;
                lista.Add(pa);
                pa = new Parametros();
                pa.nombreParametro = "@DESTINO";
                pa.tipoParametro = SqlDbType.VarChar;
                pa.direccion = ParameterDirection.Input;
                pa.value = destinop;
                lista.Add(pa); pa = new Parametros();
                pa.nombreParametro = "@DESTINOBOLETO";
                pa.tipoParametro = SqlDbType.VarChar;
                pa.direccion = ParameterDirection.Input;
                pa.value = destinoboletop;
                lista.Add(pa);
                pa = new Parametros(); 
                pa.nombreParametro = "@HORA";
                pa.tipoParametro = SqlDbType.VarChar;
                pa.direccion = ParameterDirection.Input;
                pa.value = horacortap;
                lista.Add(pa);
                pa = new Parametros();
                pa.nombreParametro = "@PRECIO";
                pa.tipoParametro = SqlDbType.Real;
                pa.direccion = ParameterDirection.Input;
                pa.value = _tarifa;
                lista.Add(pa);
                pa = new Parametros();
                pa.nombreParametro = "@PASAJERO";
                pa.tipoParametro = SqlDbType.VarChar;
                pa.direccion = ParameterDirection.Input;
                pa.value = pasajerop;
                lista.Add(pa);
                pa = new Parametros();
                pa.nombreParametro = "@STATUS";
                pa.tipoParametro = SqlDbType.VarChar;
                pa.direccion = ParameterDirection.Input;
                pa.value = statusp;
                lista.Add(pa);
                pa = new Parametros();
                pa.nombreParametro = "@FILA";
                pa.tipoParametro = SqlDbType.Int;
                pa.direccion = ParameterDirection.Input;
                pa.value = filap;
                lista.Add(pa);
                pa = new Parametros();
                pa.nombreParametro = "@PK_USUARIO";
                pa.tipoParametro = SqlDbType.Int;
                pa.direccion = ParameterDirection.Input;
                pa.value = LoginInfo.PkUsuario;
                lista.Add(pa);
                pa = new Parametros();
                pa.nombreParametro = "@PK_CONDUCTOR";
                pa.tipoParametro = SqlDbType.Int;
                pa.direccion = ParameterDirection.Input;
                pa.value = PKCONDUCTOR;
                lista.Add(pa);
                pa = new Parametros();
                pa.nombreParametro = "@COLUMNA";
                pa.tipoParametro = SqlDbType.Int;
                pa.direccion = ParameterDirection.Input;
                pa.value = columnap;
                lista.Add(pa);
                pa = new Parametros();
                pa.nombreParametro = "@VENDEDOR";
                pa.tipoParametro = SqlDbType.VarChar;
                pa.direccion = ParameterDirection.Input;
                pa.value = usuariop;
                lista.Add(pa);
                pa = new Parametros();
                pa.nombreParametro = "@SUCURSAL";
                pa.tipoParametro = SqlDbType.VarChar;
                pa.direccion = ParameterDirection.Input;
                pa.value = origenp;
                lista.Add(pa);
                pa = new Parametros();
                pa.nombreParametro = "@PISOS";
                pa.tipoParametro = SqlDbType.VarChar;
                pa.direccion = ParameterDirection.Input;
                pa.value = npisosp;
                lista.Add(pa);
                pa = new Parametros();
                pa.nombreParametro = "@CORTE";
                pa.tipoParametro = SqlDbType.Int;
                pa.direccion = ParameterDirection.Input;
                pa.value = cortep;
                lista.Add(pa);
                pa = new Parametros();
                pa.nombreParametro = "@PK_CORRIDA_DIA";
                pa.tipoParametro = SqlDbType.Int;
                pa.direccion = ParameterDirection.Input;
                pa.value = pkcorridap;
                lista.Add(pa);
                pa = new Parametros();
                pa.nombreParametro = "@RUTA";
                pa.tipoParametro = SqlDbType.VarChar;
                pa.direccion = ParameterDirection.Input;
                pa.value = rutap;
                lista.Add(pa);
                pa = new Parametros();
                pa.nombreParametro = "@CONDUCTOR";
                pa.tipoParametro = SqlDbType.VarChar;
                pa.direccion = ParameterDirection.Input;
                pa.value = choferp;
                lista.Add(pa);
                pa = new Parametros();
                pa.nombreParametro = "@TIPO";
                pa.tipoParametro = SqlDbType.VarChar;
                pa.direccion = ParameterDirection.Input;
                pa.value = tipop;
                lista.Add(pa); 
                pa = new Parametros();
                pa.nombreParametro = "@RESULTADO";
                pa.tipoParametro = SqlDbType.VarChar;
                pa.direccion = ParameterDirection.Output;
                pa.value = "0";
                lista.Add(pa);
                pa = new Parametros();
                pa.nombreParametro = "@ERROR";
                pa.tipoParametro = SqlDbType.VarChar;
                pa.direccion = ParameterDirection.Output;
                pa.longitudParametro = 50;
                pa.value = "0";
                lista.Add(pa);
                pa = new Parametros();
                pa.nombreParametro = "@FOLIO";
                pa.tipoParametro = SqlDbType.VarChar;
                pa.direccion = ParameterDirection.Output;
                pa.longitudParametro = 50;
                pa.value = "0";
                lista.Add(pa);


                pa = new Parametros();
                pa.nombreParametro = "@PK";
                pa.tipoParametro = SqlDbType.BigInt;
                pa.direccion = ParameterDirection.Output;
                pa.value = "0";
                lista.Add(pa);
                pa = new Parametros();
                pa.nombreParametro = "@VEND";
                pa.tipoParametro = SqlDbType.VarChar;
                pa.direccion = ParameterDirection.Output;
                pa.longitudParametro = 50;
                pa.value = "0";
                lista.Add(pa);

                db.ExecuteStoreProcedure2(sql, lista);
                 RESULTADO = db.command.Parameters["@RESULTADO"].Value.ToString();
                ERROR = db.command.Parameters["@ERROR"].Value.ToString();
                VENDTEMPO = db.command.Parameters["@VEND"].Value.ToString();

                pkvendido = ( db.command.Parameters["@PK"].Value.ToString());
                _folio = (db.command.Parameters["@FOLIO"].Value.ToString());

            }
            catch (Exception err)
            {
                string error = err.Message;
                if (Utilerias.Utilerias.CheckForInternetConnection())
                {
                    Form mensaje = new Mensaje("Sin internet", true);
                    mensaje.ShowDialog();
                }
                else
                    MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "Autobus_CellMouseUp";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }

        private void Autobus_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
             {
                bool yano = false;
                bool yanol = false;
                bool yanob = false;


                foreach (DataGridViewCell cel in autobus.SelectedCells)
                        {
                            int n = cel.RowIndex;
                            int c = cel.ColumnIndex;
                            if (n >= 0 && c >= 0)
                            {
                        etiqueta = matrizpiso1[n, c];


                        if (etiqueta != "")
                                {

                            string statusselec = "";
                            string vended = "";
                            statusselec = matrizstatus[n, c];
                            vended = matrizvendedor[n, c];
                            string tip = matriztipo[n, c];
                            string corte = "0";
                            rutapasajero = textBoxruta.Text;
                            _pasajero = textBoxnombre.Text;
                            _fila = n;
                            _npiso = "1";   
                            _columna = c;
                                            _destinoboleto_ = comboBoxdestinobol.Text;
                            string horacorta = comboBoxsalida.SelectedItem.ToString();
                            executaProcedimientopreventa( _llegada, _hora, _linea, ECO, _fecha, etiqueta, _tarifa, _origen,
            _destino, _destinoboleto_, horacorta, tot.ToString(), _pasajero, _statustemporal, _fila.ToString(), _columna.ToString(),
            vendedoractual,
                              _npiso, corte, pkcorrida.ToString(), rutapasajero, chofer, tip);


                            if (ERROR == "ASIENTO OCUPADO" && vendedoractual == VENDTEMPO && (statusselec == "PREVENTA" || statusselec == "BLOQUEADO"))
                            {


                                string sql3;
                                sql3 = "DELETE FROM VENDIDOS  WHERE ASIENTO=@ASIENTO AND (SALIDA  BETWEEN  SALIDA AND CONVERT (datetime,@LLEGADA ) ) AND" +
                            "(LLEGADA  BETWEEN  CONVERT(datetime, @SALIDA) AND LLEGADA) AND LINEA=@LINEA  AND FECHA=@FECHA AND ECO=@ECO AND VENDEDOR=@VENDEDOR AND (STATUS='PREVENTA' OR STATUS='BLOQUEADO')";


                                db.PreparedSQL(sql3);
                                db.command.Parameters.AddWithValue("@ASIENTO", etiqueta);
                                db.command.Parameters.AddWithValue("@SALIDA", _hora);
                                db.command.Parameters.AddWithValue("@LINEA", _linea);
                                db.command.Parameters.AddWithValue("@LLEGADA", _llegada);
                                db.command.Parameters.AddWithValue("@FECHA", _fecha);
                                db.command.Parameters.AddWithValue("@ECO", ECO);
                                db.command.Parameters.AddWithValue("@VENDEDOR", vendedoractual);

                                if (db.execute())
                                {

                                    Utilerias.LOG.acciones("quito  de preventa un boleto " + _folio);
                                    if (tip == "asiento")
                                    {
                                        Bitmap asientoimg = new Bitmap(imagenansiento);
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
                                        string texto = etiqueta;

                                        string firstText = texto;
                                        PointF firstLocation = new PointF(1f, 4f);
                                        using (Graphics graphics = Graphics.FromImage(asientoimg))
                                        {
                                            using (Font arialFont = new Font("Arial", 12))
                                            {
                                                graphics.DrawString(firstText, arialFont, Brushes.White, firstLocation);
                                            }
                                        }
                                        autobus.Rows[n].Cells[c].Value = asientoimg;
                                        matrizstatus[n, c] = "";
                                        matrizvendedor[n, c] = "";

                                    }
                                    if (tip == "Asiento con pantalla")
                                    {

                                        asientoconpantalla(Color.Green, etiqueta, n, c, tip, true);
                                    }

                                    for (int i = 0; i < agregados.RowCount; i++)
                                    {


                                        if (agregados.Rows[i].Cells["asientoname"].Value.ToString() == etiqueta)
                                        {
                                            agregados.Rows.RemoveAt(i);
                                            totalsumatori();
                                        }
                                    }



                                }
                                if (agregados.Rows.Count < 1)
                                {
                                    buttonbloquear.Enabled = false;
                                    buttonbloquear.BackColor = Color.White;
                                }
                                if (_npiso == "1")
                                {
                                    matrizstatus[n, c] = "";
                                }
                                if (_npiso == "2")
                                {
                                    matrizstatus2[n, c] = "";
                                }
                            }
                            else if (ERROR == "HUBO UN CAMBIO DE AUTOBUS")
                            {
                                limpiar();
                                limpiarpreventa();
                                limpiarformpago();
                                formadepago = "Efectivo";

                                foliotarjeta = " ";
                                digitostarjeta = " ";
                                groupBoxformadepago.Visible = false;
                                buttonborrar.Enabled = false;
                                buttonreimprimir.Enabled = false;
                                return;
                            }

                            else if (ERROR == "ASIENTO OCUPADO" && vendedoractual != VENDTEMPO && statusselec != "VENDIDO")
                            {
                                if (agregados.Rows.Count < 1)
                                {
                                    buttonbloquear.Enabled = false;
                                    buttonbloquear.BackColor = Color.White;
                                }
                                getRowsVendidos2();

                            }

                            else if (ERROR == "Ya no puedes seleccionar tipo de pasaje")
                            {
                                yano = true;
                            }
                            else if (ERROR == "La guia ya se genero")
                            {
                                yanol = true;
                            }
                            else if (ERROR == "Corrida bloqueada")
                            {
                                yanob = true;
                            }
                            else if ((ERROR == "Boleto vendido " && statusselec == "") || (ERROR == "Boleto vendido " && statusselec == "CANCELADO"))
                            {



                                buttonbloquear.Enabled = true;
                                buttonbloquear.BackColor = Color.DarkGoldenrod;

                                tot = float.Parse(_tarifa);
                                sub = tot * 100 / IVA;

                                ivasub = tot - sub;
                                _fila = n;
                                _columna = c;
                                _npiso = "1";
                                rutapasajero = textBoxruta.Text;

                                matrizstatus[n, c] = "PREVENTA";


                                agregados.Rows.Add(_folio, _linea, _origen, _fecha, _destino, _hora, _destinoboleto_, _pasaje, tot, etiqueta, _pasajero, Utilerias.Utilerias.formatCurrency(sub), Utilerias.Utilerias.formatCurrency(ivasub), Utilerias.Utilerias.formatCurrency(tot), _statustemporal, _fila, _columna, ECO, _llegada, _hora, _npiso, pk, pkvendido);

                                if (tip == "asiento")
                                {
                                    Bitmap asientoimg = new Bitmap(imagenazul);
                                    for (int Xcount = 5; Xcount < 15; Xcount++)
                                    {
                                        for (int Ycount = 0; Ycount < 43; Ycount++)
                                        {
                                            asientoimg.SetPixel(Xcount, Ycount, Color.LightSkyBlue);
                                        }
                                    }
                                    for (int Xcount = 35; Xcount < 45; Xcount++)
                                    {
                                        for (int Ycount = 0; Ycount < 43; Ycount++)
                                        {
                                            asientoimg.SetPixel(Xcount, Ycount, Color.LightSkyBlue);
                                        }
                                    }
                                    for (int Xcount = 1; Xcount < 49; Xcount++)
                                    {
                                        for (int Ycount = 0; Ycount < 22; Ycount++)
                                        {
                                            asientoimg.SetPixel(Xcount, Ycount, Color.LightSkyBlue);
                                        }
                                    }
                                    for (int Xcount = 15; Xcount < 35; Xcount++)
                                    {
                                        for (int Ycount = 0; Ycount < 40; Ycount++)
                                        {
                                            asientoimg.SetPixel(Xcount, Ycount, Color.LightSkyBlue);
                                        }
                                    }
                                    string texto = etiqueta;

                                    string firstText = texto;
                                    PointF firstLocation = new PointF(1f, 4f);
                                    using (Graphics graphics = Graphics.FromImage(asientoimg))
                                    {
                                        using (Font arialFont = new Font("Arial", 12))
                                        {
                                            graphics.DrawString(firstText, arialFont, Brushes.White, firstLocation);
                                        }
                                    }
                                    autobus.Rows[n].Cells[c].Value = asientoimg;

                                }
                                if (tip == "Asiento con pantalla")
                                {
                                    asientoconpantalla(Color.LightSkyBlue, etiqueta, n, c, tip, true);

                                }
                                matrizvendedor[n, c] = vendedoractual;

                                totalsumatori();
                                Utilerias.LOG.acciones("preventa de  un boleto " + _folio);


                            }
                                  }
                     

                    }
                         }
                if (yano == true)
                {
                    Form mensaje = new Mensaje("Tarifa agotada "+_pasaje, true);
                    mensaje.ShowDialog();
                }
                if (yanol == true)
                {
                    Form mensaje = new Mensaje("La guia ya se genero", true);
                    mensaje.ShowDialog();
                    limpiar();
                }
                if (yanol == true)
                {
                    Form mensaje = new Mensaje(ERROR, true);
                    mensaje.ShowDialog();
                    limpiar();
                }




            }
            catch (Exception err)
            {
                string error = err.Message;
                if (Utilerias.Utilerias.CheckForInternetConnection())
                {
                    Form mensaje = new Mensaje("Sin internet", true);
                    mensaje.ShowDialog();
                }
                else
                    MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "Autobus_CellMouseUp";
                Utilerias.LOG.write(_clase, funcion, error);


            }

        }


        private void PISOS2_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                bool yano = false;
                bool yanol = false;
                bool yanob = false;


                foreach (DataGridViewCell cel in PISOS2.SelectedCells)
                {
                    int n = cel.RowIndex;
                    int c = cel.ColumnIndex;
                    if (n >= 0 && c >= 0)
                    {
                        etiqueta = matrizpiso2[n, c];


                        if (etiqueta != "")
                        {

                            string statusselec = "";
                            string vended = "";
                            statusselec = matrizstatus2[n, c];
                            vended = matrizvendedor2[n, c];
                            string tip = matriztipo2[n, c];
                            string corte = "0";
                            rutapasajero = textBoxruta.Text;
                            _pasajero = textBoxnombre.Text;
                            _fila = n;
                            _npiso = "2";
                            _columna = c;
                            string horacorta = comboBoxsalida.SelectedItem.ToString();
                            _destinoboleto_ = comboBoxdestinobol.Text;

                            executaProcedimientopreventa(_llegada, _hora, _linea, ECO, _fecha, etiqueta, _tarifa, _origen,
            _destino, _destinoboleto_, horacorta, tot.ToString(), _pasajero, _statustemporal, _fila.ToString(), _columna.ToString(),
            vendedoractual,
                              _npiso, corte, pkcorrida.ToString(), rutapasajero, chofer, tip);


                            if (ERROR=="ASIENTO OCUPADO" && vendedoractual == VENDTEMPO && (statusselec == "PREVENTA" || statusselec == "BLOQUEADO"))
                            {

                                 
                                string sql3;
                                sql3 = "DELETE FROM VENDIDOS  WHERE ASIENTO=@ASIENTO AND (SALIDA  BETWEEN  SALIDA AND CONVERT (datetime,@LLEGADA ) ) AND" +
                            "(LLEGADA  BETWEEN  CONVERT(datetime, @SALIDA) AND LLEGADA) AND LINEA=@LINEA  AND FECHA=@FECHA AND ECO=@ECO AND VENDEDOR=@VENDEDOR AND (STATUS='PREVENTA' OR STATUS='BLOQUEADO')";


                                db.PreparedSQL(sql3);
                                db.command.Parameters.AddWithValue("@ASIENTO", etiqueta);
                                db.command.Parameters.AddWithValue("@SALIDA", _hora);
                                db.command.Parameters.AddWithValue("@LINEA", _linea);
                                db.command.Parameters.AddWithValue("@LLEGADA", _llegada);
                                db.command.Parameters.AddWithValue("@FECHA", _fecha);
                                db.command.Parameters.AddWithValue("@ECO", ECO);
                                db.command.Parameters.AddWithValue("@VENDEDOR", vendedoractual);

                                if (db.execute())
                                {

                                    Utilerias.LOG.acciones("quito  de preventa un boleto " + _folio);
                                    if (tip == "asiento")
                                    {
                                        Bitmap asientoimg = new Bitmap(imagenansiento);
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
                                        string texto = etiqueta;

                                        string firstText = texto;
                                        PointF firstLocation = new PointF(1f, 4f);
                                        using (Graphics graphics = Graphics.FromImage(asientoimg))
                                        {
                                            using (Font arialFont = new Font("Arial", 12))
                                            {
                                                graphics.DrawString(firstText, arialFont, Brushes.White, firstLocation);
                                            }
                                        }
                                        PISOS2.Rows[n].Cells[c].Value = asientoimg;
                                        matrizstatus2[n, c] = "";
                                        matrizvendedor2[n, c] = "";

                                    }
                                    if (tip == "Asiento con pantalla")
                                    {

                                        asientoconpantalla(Color.Green, etiqueta, n, c, tip, true);
                                    }

                                    for (int i = 0; i < agregados.RowCount; i++)
                                    {


                                        if (agregados.Rows[i].Cells["asientoname"].Value.ToString() == etiqueta)
                                        {
                                            agregados.Rows.RemoveAt(i);
                                            totalsumatori();
                                        }
                                    }



                                }
                                if (agregados.Rows.Count < 1)
                                {
                                    buttonbloquear.Enabled = false;
                                    buttonbloquear.BackColor = Color.White;
                                }
                                if (_npiso == "1")
                                {
                                    matrizstatus[n, c] = "";
                                }
                                if (_npiso == "2")
                                {
                                    matrizstatus2[n, c] = "";
                                }
                            }

                            else if (ERROR=="ASIENTO OCUPADO" && vendedoractual != VENDTEMPO && statusselec != "VENDIDO")
                            {
                                getRowsVendidos2();
                                if (agregados.Rows.Count < 1)
                                {
                                    buttonbloquear.Enabled = false;
                                    buttonbloquear.BackColor = Color.White;
                                }
                            }
                            else if (ERROR == "HUBO UN CAMBIO DE AUTOBUS")
                            {
                                limpiar();
                                limpiarpreventa();
                                limpiarformpago();
                                formadepago = "Efectivo";

                                foliotarjeta = " ";
                                digitostarjeta = " ";
                                groupBoxformadepago.Visible = false;
                                buttonborrar.Enabled = false;
                                buttonreimprimir.Enabled = false;
                                return;
                            }
                            else if (ERROR=="Ya no puedes seleccionar tipo de pasaje")
                            {
                                yano = true;
                            }
                            else if (ERROR=="La guia ya se genero")
                            {
                                yanol = true;
                            }
                            else if (ERROR == "Corrida bloqueada")
                            {
                                yanob = true;
                            }
                            else if ((ERROR=="Boleto vendido " && statusselec == "") || (ERROR=="Boleto vendido " && statusselec == "CANCELADO"))
                            {





                                tot = float.Parse(_tarifa);
                                sub = tot * 100 / IVA;

                                ivasub = tot - sub;
                                _fila = n;
                                _columna = c;
                                rutapasajero = textBoxruta.Text;

                                matrizstatus2[n, c] = "PREVENTA";

                                buttonbloquear.Enabled = true;
                                buttonbloquear.BackColor = Color.DarkGoldenrod;

                                agregados.Rows.Add(_folio, _linea, _origen, _fecha, _destino, _hora, _destinoboleto_, _pasaje, tot, etiqueta, _pasajero, Utilerias.Utilerias.formatCurrency(sub), Utilerias.Utilerias.formatCurrency(ivasub), Utilerias.Utilerias.formatCurrency(tot), _statustemporal, _fila, _columna, ECO, _llegada, _hora, _npiso, pk,pkvendido);

                                if (tip == "asiento")
                                {
                                    Bitmap asientoimg = new Bitmap(imagenazul);
                                    for (int Xcount = 5; Xcount < 15; Xcount++)
                                    {
                                        for (int Ycount = 0; Ycount < 43; Ycount++)
                                        {
                                            asientoimg.SetPixel(Xcount, Ycount, Color.LightSkyBlue);
                                        }
                                    }
                                    for (int Xcount = 35; Xcount < 45; Xcount++)
                                    {
                                        for (int Ycount = 0; Ycount < 43; Ycount++)
                                        {
                                            asientoimg.SetPixel(Xcount, Ycount, Color.LightSkyBlue);
                                        }
                                    }
                                    for (int Xcount = 1; Xcount < 49; Xcount++)
                                    {
                                        for (int Ycount = 0; Ycount < 22; Ycount++)
                                        {
                                            asientoimg.SetPixel(Xcount, Ycount, Color.LightSkyBlue);
                                        }
                                    }
                                    for (int Xcount = 15; Xcount < 35; Xcount++)
                                    {
                                        for (int Ycount = 0; Ycount < 40; Ycount++)
                                        {
                                            asientoimg.SetPixel(Xcount, Ycount, Color.LightSkyBlue);
                                        }
                                    }
                                    string texto = etiqueta;

                                    string firstText = texto;
                                    PointF firstLocation = new PointF(1f, 4f);
                                    using (Graphics graphics = Graphics.FromImage(asientoimg))
                                    {
                                        using (Font arialFont = new Font("Arial", 12))
                                        {
                                            graphics.DrawString(firstText, arialFont, Brushes.White, firstLocation);
                                        }
                                    }
                                    PISOS2.Rows[n].Cells[c].Value = asientoimg;

                                }
                                if (tip == "Asiento con pantalla")
                                {
                                    asientoconpantalla(Color.LightSkyBlue, etiqueta, n, c, tip, true);

                                }
                                matrizvendedor2[n, c] = vendedoractual;

                                totalsumatori();
                                Utilerias.LOG.acciones("preventa de  un boleto " + _folio);

                                if (agregados.Rows.Count < 1)
                                {
                                    buttonbloquear.Enabled = false;
                                    buttonbloquear.BackColor = Color.White;
                                }
                            }
                        }


                    }
                }
                if (yano == true)
                {
                    Form mensaje = new Mensaje("Tarifa agotada", true);
                    mensaje.ShowDialog();
                }
                if (yanol == true)
                {
                    Form mensaje = new Mensaje("Ya no puedes seleccionar tipo de pasaje", true);
                    mensaje.ShowDialog();
                }
                if (yanob == true)
                {
                    Form mensaje = new Mensaje(ERROR, true);
                    mensaje.ShowDialog();
                }



            }
            catch (Exception err)
            {
                string error = err.Message;
                if (Utilerias.Utilerias.CheckForInternetConnection())
                {
                    Form mensaje = new Mensaje("Sin internet", true);
                    mensaje.ShowDialog();
                }
                else
                    MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "Autobus_CellMouseUp";
                Utilerias.LOG.write(_clase, funcion, error);


            }


        }

        private void actualizar()
        {
            try
            {
                verificartarifa();

                GUIACOMPROBACION();
                if (validar == "True")
                {
                    Form mensaje = new Mensaje("La guia ya se cerro", true);

                    mensaje.ShowDialog(); limpiar();
                    agregados.Rows.Clear();

                }
                if (validar2 == "True")
                {
                    Form mensaje = new Mensaje("La guia ya se bloqueo", true);

                    mensaje.ShowDialog();
                    limpiar();
                    agregados.Rows.Clear();

                }
                if (dato == 1)
                {
                    Form mensaje = new Mensaje("Ya no puedes vender mas boletos con este tipo de tarifa", true);

                    mensaje.ShowDialog();
                }
                else
                {


                    if (agregados.RowCount == 0)
                    {
                        Form mensaje = new Mensaje("Seleccione almenos un boleto", true);

                        mensaje.ShowDialog();
                    }
                    else
                    {

                        Form mensaje = new Mensaje("La guia ya se cerro", true);

                        DialogResult confirmResult = mensaje.ShowDialog();

                        getRowsVendidos();
                        if (confirmResult == DialogResult.Yes)
                        {

                            string sql = "UPDATE  VENDIDOS SET STATUS=@STATUS WHERE  ";

                            db.PreparedSQL(sql);
                            db.command.Parameters.AddWithValue("@FOLIO", _folio);
                            db.command.Parameters.AddWithValue("@STATUS", "AUTOBUSCANCELADO");


                            if (db.execute())
                            {
                                Utilerias.LOG.acciones("vendio un boleto" + _folio);

                                _total = 0;
                            }
                        }
                    }
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
                    MessageBox.Show("Ocurrio un Error al actualizar");
                string funcion = "actualizar";
                Utilerias.LOG.write(_clase, funcion, error);


            }

        }




        private void Ventas_FormClosed(object sender, FormClosedEventArgs e)
        {
            limpiarpreventa();

        }
        private void DoubleBuffered(DataGridView dgv, bool setting)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, setting, null);
        }

        private void Ventas_Shown(object sender, EventArgs e)
        {
            agregados.EnableHeadersVisualStyles = false;
            datagridvendidos.EnableHeadersVisualStyles = false;
            comboBoxlinea.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxdestino.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxsalida.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxdestinobol.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxtarifa.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxorigen.DropDownStyle = ComboBoxStyle.DropDownList;

            agregados.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            agregados.MultiSelect = false;
            filas.Clear();
            DoubleBuffered(autobus, true);
            DoubleBuffered(PISOS2, true);
            DoubleBuffered(agregados, true);
            DoubleBuffered(datagridvendidos, true);


            columnas.Clear();
            vendidos.Clear();
            apartado.Clear();
            etiquetass.Clear();
            dateTimePicker1.Value = DateTime.Now;
            _fecha = DateTime.Now.ToString("dd/MM/yyyy");
            llenarcomboboxlinea();
            borrarerrores();

            datagridvendidos.Visible = false;
            button4.BackColor = Color.DarkBlue;
            permisos();
            groupBoxformadepago.Visible = false;
            autobus.Enabled = true;
            PISOS2.Enabled = true;
            matrizpiso1 = new string[18, 5];

            matrizpiso2 = new string[18, 5];
            matrizstatus = new string[18, 5];
            matrizstatus2 = new string[18, 5];
            matrizvendedor = new string[18, 5];
            matrizvendedor2 = new string[18, 5];
            matrizn = new string[18, 5];
            matrizn2 = new string[18, 5];

            matriztipo = new string[18, 5];
            matriztipo2 = new string[18, 5];
            buttonlimpiarautobus.BackColor = Color.White;
            buttonlimpiarautobus.Enabled = false;
            buttonborrar.Enabled = false;
            buttonborrar.BackColor = Color.White;
            buttonreimprimir.Enabled = false;
            buttonreimprimir.BackColor = Color.White;
            buttonvender.Enabled = false;
            buttonvender.BackColor = Color.White;
            buttonbloquear.Enabled = false;
            buttonbloquear.BackColor = Color.White;
            buttonborrar.Enabled = false;
            buttonborrar.BackColor = Color.White;
            buttonactualizar.BackColor = Color.White;
            buttonactualizar.Enabled = false;
            //Estas lineas eliminan los parpadeos del formulario o controles en la interfaz grafica (Pero no en un 100%)
            this.SetStyle(ControlStyles.ResizeRedraw, true);

            label12.Text = LoginInfo.NombreID;
            labelapellido.Text = LoginInfo.ApellidoID;
            MemoryStream ms = new MemoryStream((byte[])LoginInfo.imagenfoto);
            Bitmap bmp = new Bitmap(ms);
            pictureBoxfoto.Image = bmp;
            labelcargo.Text = LoginInfo.rol;

            timer2.Start();

            buttonborrar.Enabled = false;
            buttonborrar.BackColor = Color.White;
            buttonreimprimir.Enabled = false;
            buttonreimprimir.BackColor = Color.White;
            buttonvender.Enabled = false;
            buttonvender.BackColor = Color.White;
            buttonbloquear.Enabled = false;
            buttonbloquear.BackColor = Color.White;
            buttonguia.Enabled = false;
            buttonguia.BackColor = Color.White;
            timer3.Start();
            cuatrotext.MaxLength = 4;
            variables();
            supercontra();
            if (Settings1.Default.impresora == "Ninguna")
            {
                Form mensaje = new Mensaje("Configura la impresora por favor", true);

                mensaje.ShowDialog();
            }
        }
        private void botonesinicio()
        {
            buttonborrar.Enabled = false;
            buttonborrar.BackColor = Color.White;
            buttonreimprimir.Enabled = false;
            buttonreimprimir.BackColor = Color.White;
            buttonvender.Enabled = false;
            buttonvender.BackColor = Color.White;
            buttonbloquear.Enabled = false;
            buttonbloquear.BackColor = Color.White;
            buttonguia.Enabled = false;
            buttonguia.BackColor = Color.White;
            buttonlimpiar.Enabled = false;
            buttonlimpiar.BackColor = Color.White;
            buttonlimpiarautobus.BackColor = Color.White;
            buttonlimpiarautobus.Enabled = false;
            buttonactualizar.BackColor = Color.White;
            buttonactualizar.Enabled = false;

        }
        private void buttonactivar()
        {
            buttonborrar.Enabled = true;
            buttonborrar.BackColor = Color.FromArgb(163, 17, 18);
 
            buttonvender.Enabled = true;
            buttonvender.BackColor = Color.FromArgb(54, 43, 221);
            buttonbloquear.Enabled = false;
            buttonbloquear.BackColor = Color.DarkGoldenrod;
            buttonguia.Enabled = true;
            buttonguia.BackColor = Color.BlueViolet;
            buttonlimpiar.BackColor = Color.FromArgb(28, 160, 0);
            buttonlimpiar.Enabled = true;
            buttonlimpiarautobus.BackColor = Color.DodgerBlue;
            buttonlimpiarautobus.Enabled = true;
            buttonactualizar.BackColor = Color.FromArgb(128, 64, 0);
            buttonactualizar.Enabled = true;
        }

        private void TabControl1_Click(object sender, EventArgs e)
        {
            getRowsVendidos();

        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Location = new Point(Cursor.Position.X + e.X, Cursor.Position.Y + e.Y);
            }
        }
        private void Button1_Click_1(object sender, EventArgs e)
        {
            formadepago = "Efectivo";

            foliotarjeta = " ";
            digitostarjeta = " ";
            autobus.Enabled = true;
            PISOS2.Enabled = true;
            groupBoxformadepago.Visible = false;
            limpiarpreventa();
            getRowsVendidos();
            agregados.Rows.Clear();
            llenarautobus();
            limpiarformpago();
            buttonborrar.Enabled = false;
            buttonreimprimir.Enabled = false;
            textBoxtotal.Text = "";
        }

        private void Button6_Click(object sender, EventArgs e)
        {

            buttonvender.Enabled = true;
            buttonguia.Enabled = true;
            buttonactualizar.Enabled = true;
            buttonlimpiarautobus.Enabled = true;
            buttonlimpiar.Enabled = true;
            buttonbloquear.Enabled = true;
            groupBoxformadepago.Visible = false;
            autobus.Enabled = true;
            PISOS2.Enabled = true;
            limpiarformpago();
        }

        private void limpiarformpago()
        {
            pie = true;
            ventavalidar = false;
            tcredito.Enabled = true;
            tdebito.Enabled = true;
            Efectivo.Enabled = true;
            Efectivo.Text = "Efectivo";
            tcredito.Text = "Tarjeta de Credito";
            tdebito.Text = "Tarjeta de Debito";

        }
        private void Button4_Click(object sender, EventArgs e)
        {
            button3.BackColor = Color.FromArgb(38, 45, 53);

            button4.BackColor = Color.DarkBlue;
            datagridvendidos.Visible = false;
            label7.Visible = true;
            textBoxtotal.Visible = true;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            button3.BackColor = Color.DarkBlue;
            button4.BackColor = Color.FromArgb(38, 45, 53);
            if (datagridvendidos.Rows.Count > 0)
            {
                datagridvendidos.CurrentRow.Selected = false;
            }
            datagridvendidos.Visible = true;
            label7.Visible = false;
            textBoxtotal.Visible = false;
        }

        private void BtnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

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
        private void Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Timer2_Tick(object sender, EventArgs e)
        {
            lblHora.Text = DateTime.Now.ToString("hh:mm:ss");
            lbFecha.Text = DateTime.Now.ToString("D");
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

        private void PanelBarraTitulo_MouseDown(object sender, MouseEventArgs e)
        {
            moverForm();
        }

        private void g(object sender, PaintEventArgs e)
        {
            ReleaseCapture();
            SendMessage(groupBoxformadepago.Handle, 0x112, 0xf012, 0); ReleaseCapture();
        }

        private void Panel2_MouseMove(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(groupBoxformadepago.Handle, 0x112, 0xf012, 0); ReleaseCapture();
        }

        void llenarticket2(object sender, PrintPageEventArgs e)
        {
            try
            {
                codigoqr(datagridvendidos.Rows[nfila].Cells[0].Value.ToString());
                BarcodeLib.Barcode Codigobarras = new BarcodeLib.Barcode();
                Codigobarras.IncludeLabel = true;
                Graphics g = e.Graphics;
                Graphics g2 = e.Graphics;

                // Create solid brush.
                SolidBrush blueBrush = new SolidBrush(Color.Black);

                // Create rectangle for region.
                Rectangle fillRect = new Rectangle(0, 12, 270, 26);

                // Create region for fill.
                Region fillRegion = new Region(fillRect);

                // Fill region to screen.
                // g.DrawRectangle(Pens.Black, 3, 7, 340, 700);
                //g.DrawImage(imagensplash, 0, 0);


                Font fBody9 = new Font("Agency FB", 9, FontStyle.Regular);
                Font fBody7 = new Font("Agency FB", 7, FontStyle.Regular);

                Font fBody = new Font("Agency FB", 8, FontStyle.Regular);
                Font fBody10 = new Font("Agency FB", 8, FontStyle.Italic);
                Font fBody12 = new Font("Agency FB", 9, FontStyle.Bold | FontStyle.Italic);
                Font fBody18 = new Font("Agency FB", 12, FontStyle.Bold | FontStyle.Italic);
                Font fBody5 = new Font("Agency FB", 6, FontStyle.Regular);
                Font fBody188 = new Font("Agency FB", 18, FontStyle.Bold | FontStyle.Italic);

                int espacio = 0;
                Color customColor = Color.FromArgb(255, Color.Black);
                Color customColor2 = Color.FromArgb(255, Color.White);
                g2.FillRegion(blueBrush, fillRegion);

                SolidBrush sb = new SolidBrush(customColor);
                SolidBrush sb2 = new SolidBrush(customColor2);

                g.DrawString("Fecha: " + DateTime.Now.ToShortDateString(), fBody, sb, 0, espacio);
                g.DrawString("Hora: " + DateTime.Now.ToShortTimeString(), fBody, sb, 190, espacio);
                espacio = espacio + 10;
                g.DrawString("Visitanos en ATAH.ONLINE", fBody188, sb2, 30, espacio);
                espacio = espacio + 30;
                g.DrawString("Esta es una reimpresión del boleto original", fBody18, sb, 0, espacio);
                espacio = espacio + 30;
                g.DrawString("MARCA:", fBody10, sb, 0, espacio);
                g.DrawString("SERVICIO:", fBody10, sb, 100, espacio);
                g.DrawString("FOLIO:", fBody10, sb, 190, espacio);

                espacio = espacio + 15;
                g.DrawString("ATAH", fBody10, sb, 0, espacio);
                g.DrawString(datagridvendidos.Rows[nfila].Cells[1].Value.ToString(), fBody10, sb, 100, espacio);
                g.DrawString(datagridvendidos.Rows[nfila].Cells[0].Value.ToString(), fBody10, sb, 190, espacio);

                espacio = espacio + 20;
                g.DrawString("ORIGEN:", fBody10, sb, 0, espacio);
                g.DrawString("DESTINO:", fBody10, sb, 100, espacio);
                g.DrawString("RUTA:", fBody10, sb, 190, espacio);

                espacio = espacio + 15;
                g.DrawString(datagridvendidos.Rows[nfila].Cells[2].Value.ToString(), fBody12, sb, 0, espacio);
                g.DrawString(datagridvendidos.Rows[nfila].Cells[6].Value.ToString(), fBody12, sb, 100, espacio);
                g.DrawString(datagridvendidos.Rows[nfila].Cells[21].Value.ToString(), fBody12, sb, 190, espacio);


                espacio = espacio + 20;
                g.DrawString("SALIDA:", fBody10, sb, 0, espacio);
                g.DrawString("LLEGADA:", fBody10, sb, 150, espacio);

                espacio = espacio + 15;
                g.DrawString(datagridvendidos.Rows[nfila].Cells[5].Value.ToString(), fBody12, sb, 0, espacio);
                g.DrawString(datagridvendidos.Rows[nfila].Cells["lleg"].Value.ToString(), fBody12, sb, 150, espacio);

                espacio = espacio + 25;
                g.DrawString("AUTOBUS:", fBody10, sb, 0, espacio);
                g.DrawString("ASIENTO:", fBody10, sb, 170, espacio);

                espacio = espacio + 15;

                g.DrawString(datagridvendidos.Rows[nfila].Cells[16].Value.ToString(), fBody18, sb, 0, espacio);
                g.DrawString(datagridvendidos.Rows[nfila].Cells[9].Value.ToString(), fBody18, sb, 170, espacio);

                espacio = espacio + 25;

                g.DrawString("NOMBRE:", fBody10, sb, 0, espacio);
                g.DrawString("TIPO DE PASAJE:", fBody10, sb, 170, espacio);

                espacio = espacio + 15;
                g.DrawString(datagridvendidos.Rows[nfila].Cells[10].Value.ToString(), fBody12, sb, 0, espacio);
                g.DrawString(datagridvendidos.Rows[nfila].Cells[7].Value.ToString(), fBody18, sb, 170, espacio);

                espacio = espacio + 20;
                g.DrawString("PRECIO TOTAL:", fBody10, sb, 0, espacio);
                g.DrawString("TIPO DE PAGO:", fBody10, sb, 170, espacio);

                espacio = espacio + 15;
                g.DrawString(datagridvendidos.Rows[nfila].Cells[23].Value.ToString(), fBody12, sb, 0, espacio);
                g.DrawString(datagridvendidos.Rows[nfila].Cells[20].Value.ToString(), fBody12, sb, 170, espacio);
                espacio = espacio + 25;


                g.DrawImage(Codigobarras.Encode(BarcodeLib.TYPE.CODE128, _folio, Color.Black, Color.White, 250, 50), 0, espacio);
                espacio = espacio + 55;



                g.DrawImage(imagen, 2, espacio);


                g.DrawString("PRESENTE IDENTIFICACIÓN ORIGINAL Y ", fBody9, sb, 110, espacio);
                espacio = espacio + 15;
                g.DrawString("VIGENTE AL MOMENTO DE ABORDAR ", fBody9, sb, 115, espacio);
                espacio = espacio + 15;
                g.DrawString("SERVICIO DE ASISTENCIA TOTAL ", fBody7, sb, 135, espacio);
                espacio = espacio + 15;
                g.DrawString("La transportista no es responsable de este servicio", fBody5, sb, 120, espacio);
                espacio = espacio + 12;
                g.DrawString("Atención a clientes: ‎01 800 836 0726", fBody5, sb, 140, espacio);
                espacio = espacio + 12;
                g.DrawString("Consulta terminos y condiciónes en:", fBody5, sb, 140, espacio);
                espacio = espacio + 12;
                g.DrawString("              www.atah.online", fBody5, sb, 145, espacio);
                espacio = espacio + 20;


                g.Dispose();
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Impresora no conectada.");
                string funcion = "llenarticket";
                Utilerias.LOG.write(_clase, funcion, error);


            }



        }


        //void llenarticket2(object sender, PrintPageEventArgs e)
        //{
        //    try
        //    {
        //        codigoqr(datagridvendidos.Rows[nfila].Cells[0].Value.ToString());
        //        BarcodeLib.Barcode Codigobarras = new BarcodeLib.Barcode();
        //        Codigobarras.IncludeLabel = true;
        //        Graphics g = e.Graphics;
        //        Graphics g2 = e.Graphics;

        //        // Create solid brush.
        //        SolidBrush blueBrush = new SolidBrush(Color.Black);

        //        // Create rectangle for region.
        //        Rectangle fillRect = new Rectangle(0, 12, 270, 26);

        //        // Create region for fill.
        //        Region fillRegion = new Region(fillRect);

        //        // Fill region to screen.
        //        // g.DrawRectangle(Pens.Black, 3, 7, 340, 700);
        //        //g.DrawImage(imagensplash, 0, 0);
        //        Font fBody9 = new Font("Agency FB", 9, FontStyle.Regular);
        //        Font fBody7 = new Font("Agency FB", 7, FontStyle.Regular);

        //        Font fBody = new Font("Agency FB", 8, FontStyle.Regular);
        //        Font fBody10 = new Font("Agency FB", 10, FontStyle.Italic);
        //        Font fBody12 = new Font("Agency FB", 12, FontStyle.Bold | FontStyle.Italic);
        //        Font fBody18 = new Font("Agency FB", 18, FontStyle.Bold | FontStyle.Italic);
        //        Font fBody5 = new Font("Agency FB", 6, FontStyle.Regular);

        //        int espacio = 0;
        //        Color customColor = Color.FromArgb(255, Color.Black);
        //        Color customColor2 = Color.FromArgb(255, Color.White);
        //        g2.FillRegion(blueBrush, fillRegion);

        //        SolidBrush sb = new SolidBrush(customColor);
        //        SolidBrush sb2 = new SolidBrush(customColor2);

        //        g.DrawString("Fecha: " + DateTime.Now.ToShortDateString(), fBody, sb, 0, espacio);
        //        g.DrawString("Hora: " + DateTime.Now.ToShortTimeString(), fBody, sb, 190, espacio);
        //        espacio = espacio + 10;
        //        g.DrawString("Visitanos en ATAH.COM", fBody18, sb2, 30, espacio);
        //        espacio = espacio + 40;
        //        g.DrawString("VALIDO PARA:", fBody12, sb, 80, espacio);
        //        espacio = espacio + 15;
        //        g.DrawString("MARCA:", fBody10, sb, 0, espacio);
        //        g.DrawString("SERVICIO:", fBody10, sb, 160, espacio);

        //        espacio = espacio + 15;
        //        g.DrawString("ATAH", fBody12, sb, 0, espacio);
        //        g.DrawString(datagridvendidos.Rows[nfila].Cells[1].Value.ToString(), fBody12, sb, 160, espacio);

        //        espacio = espacio + 25;
        //        g.DrawString("ORIGEN:", fBody10, sb, 0, espacio);
        //        g.DrawString("DESTINO:", fBody10, sb, 160, espacio);

        //        espacio = espacio + 15;
        //        g.DrawString(datagridvendidos.Rows[nfila].Cells[2].Value.ToString(), fBody12, sb, 0, espacio);
        //        g.DrawString(datagridvendidos.Rows[nfila].Cells[6].Value.ToString(), fBody12, sb, 160, espacio);

        //        espacio = espacio + 25;
        //        g.DrawString("FECHA DE SALIDA:", fBody10, sb, 0, espacio);
        //        g.DrawString("ASIENTO:", fBody10, sb, 160, espacio);

        //        espacio = espacio + 15;
        //        g.DrawString(DateTime.Now.ToString("dd/MM/yyyy"), fBody12, sb, 0, espacio);
        //        g.DrawString(datagridvendidos.Rows[nfila].Cells[9].Value.ToString(), fBody12, sb, 160, espacio);

        //        espacio = espacio + 25;
        //        g.DrawString("AUTOBUS:", fBody10, sb, 0, espacio);
        //        g.DrawString("RUTA:", fBody10, sb, 160, espacio);

        //        espacio = espacio + 15;
        //        g.DrawString(datagridvendidos.Rows[nfila].Cells[16].Value.ToString(), fBody12, sb, 0, espacio);
        //        g.DrawString(datagridvendidos.Rows[nfila].Cells[21].Value.ToString(), fBody12, sb, 160, espacio);

        //        espacio = espacio + 25;

        //        g.DrawString("HORA DE SALIDA:", fBody10, sb, 0, espacio);
        //        g.DrawString("PRECIO TOTAL:", fBody10, sb, 160, espacio);

        //        espacio = espacio + 15;
        //        g.DrawString(datagridvendidos.Rows[nfila].Cells[5].Value.ToString(), fBody12, sb, 0, espacio);
        //        g.DrawString(datagridvendidos.Rows[nfila].Cells[23].Value.ToString(), fBody12, sb, 160, espacio);

        //        espacio = espacio + 25;
        //        g.DrawString("FOLIO:", fBody10, sb, 0, espacio);
        //        g.DrawString("TIPO DE PAGO:", fBody10, sb, 160, espacio);

        //        espacio = espacio + 15;
        //        g.DrawString(datagridvendidos.Rows[nfila].Cells[0].Value.ToString(), fBody12, sb, 0, espacio);
        //        g.DrawString(datagridvendidos.Rows[nfila].Cells[20].Value.ToString(), fBody12, sb, 160, espacio);

        //        espacio = espacio + 25;
        //        g.DrawString("NOMBRE:", fBody10, sb, 0, espacio);
        //        g.DrawString("CONDUCTOR:", fBody10, sb, 160, espacio);

        //        espacio = espacio + 15;
        //        g.DrawString(datagridvendidos.Rows[nfila].Cells[10].Value.ToString(), fBody12, sb, 0, espacio);
        //        g.DrawString(datagridvendidos.Rows[nfila].Cells[22].Value.ToString(), fBody12, sb, 160, espacio);

        //        espacio = espacio + 25;
        //        g.DrawString("TIPO DE PASAJE:", fBody10, sb, 100, espacio);
        //        espacio = espacio + 15;
        //        g.DrawString(datagridvendidos.Rows[nfila].Cells[7].Value.ToString(), fBody12, sb, 100, espacio);
        //        espacio = espacio + 25;

        //        g.DrawImage(Codigobarras.Encode(BarcodeLib.TYPE.CODE128, datagridvendidos.Rows[nfila].Cells[0].Value.ToString(), Color.Black, Color.White, 250, 50), 0, espacio);
        //        espacio = espacio + 55;
        //        g.DrawString("CONSERVE SU BOLETO, ES SU SEGURO DE VIAJERO", fBody, sb, 20, espacio);
        //        espacio = espacio + 15;
        //        g.DrawString("Su boleto es su seguro de viajero, Válido para la fecha y hora indicada.", fBody7, sb, 0, espacio);
        //        espacio = espacio + 9;
        //        g.DrawString("Autotransportes tlaxala, apizaco, huamantla, S.A. DE C.V. no es la transportista", fBody7, sb, 0, espacio);
        //        espacio = espacio + 9;
        //        g.DrawString("Ni presta el servicio por lo que no existe ninguna obligacion en común con la ", fBody7, sb, 0, espacio);
        //        espacio = espacio + 9;
        //        g.DrawString("transportista. Mención sólo para efectos fiscales en terminos del Art. 72 de la  ", fBody7, sb, 0, espacio);
        //        espacio = espacio + 9;
        //        g.DrawString("leydel ISR. Cualquier derecho u obligacion relacionados. directamente con la  ", fBody7, sb, 0, espacio);

        //        espacio = espacio + 9;
        //        g.DrawString("de este servicio incluyendo pagos e indemnizaciones se regirán y resolverán ", fBody7, sb, 0, espacio);
        //        espacio = espacio + 9;
        //        g.DrawString("conforme a la legislacion aplicable y con los tribunales competentes del .", fBody7, sb, 0, espacio);
        //        espacio = espacio + 9;
        //        g.DrawString("fuero común, renunciando a cualquier otra Ley o juridicción o competencia,", fBody7, sb, 0, espacio);
        //        espacio = espacio + 9;
        //        g.DrawString("nacional o extranjera que pudiese corresponder por domicilio de las partes, ", fBody7, sb, 0, espacio);
        //        espacio = espacio + 9;
        //        g.DrawString("nacionalidad o por otra causa. El transportista no responderaá por culpa o  ", fBody7, sb, 0, espacio);
        //        espacio = espacio + 9;
        //        g.DrawString("negligencia de la victima, caso fortuito ni fuera mayor ni por culpa de terceros. ", fBody7, sb, 0, espacio);
        //        espacio = espacio + 9;
        //        g.DrawString("dom. y Admon. que tiene la transportista para todo los efectos.Boleto cancelable ", fBody7, sb, 0, espacio);
        //        espacio = espacio + 9;

        //        g.DrawString(" hasta 60 min. antes de la hora de la salida del viaje, comprado en ATAH.COM y ", fBody7, sb, 0, espacio);
        //        espacio = espacio + 9;
        //        g.DrawString("ATAH movil con promocion o compra anticipada (debido a tarifa preferente a  ", fBody7, sb, 0, espacio);
        //        espacio = espacio + 9;
        //        g.DrawString("la compra y 30 min. antes de la hora de salida del viaje por  ", fBody7, sb, 0, espacio);
        //        espacio = espacio + 9;
        //        g.DrawString("fecha/hora/origen/destino, pagando la diferencia a tarifa disponible vigente.", fBody7, sb, 0, espacio);
        //        espacio = espacio + 9;
        //        g.DrawString(" Hasta 25 kg. de equipaje sin costo. ", fBody7, sb, 0, espacio);
        //        espacio = espacio + 9;

        //        g.DrawString("En caso de pérdida se paga hasta 50 UMAS por pasajero y contra entrega  ", fBody7, sb, 0, espacio);
        //        espacio = espacio + 9;

        //        g.DrawString("de contraseña del equipaje o articulos olvidados. Dudas, quejas o ", fBody7, sb, 0, espacio);
        //        espacio = espacio + 9;
        //        g.DrawString("sujerencias escribe a atah.com, facturacion con tus datos fiscales en ", fBody7, sb, 0, espacio);
        //        espacio = espacio + 9;
        //        g.DrawString("ATAH.com (exepto boletos manual) y en taquilla de las principales terminales.", fBody7, sb, 0, espacio);
        //        espacio = espacio + 9;
        //        g.DrawString("Terminos y condiciones en Atah.com/terminosycondiciones.", fBody7, sb, 0, espacio);
        //        espacio = espacio + 25;

        //        g.DrawImage(imagen, 75, espacio);
        //        espacio = espacio + 110;

        //        g.DrawString("PRESENTE IDENTIFICACIÓN ORIGENAL Y ", fBody12, sb, 10, espacio);
        //        espacio = espacio + 15;
        //        g.DrawString("VIGENTE AL MOMENTO DE ABORDAR ", fBody12, sb, 15, espacio);
        //        espacio = espacio + 20;
        //        g.DrawString("SERVICIO DE ASISTENCIA TOTAL ", fBody10, sb, 45, espacio);
        //        espacio = espacio + 20;
        //        g.DrawString("La transportista no es responsable de este servicio", fBody, sb, 20, espacio);
        //        espacio = espacio + 15;
        //        g.DrawString("Atención a clientes: 01 800 0230 232", fBody, sb, 40, espacio);
        //        espacio = espacio + 15;
        //        g.DrawString("Consulta terminos y condiciónes en:", fBody, sb, 50, espacio);
        //        espacio = espacio + 15;
        //        g.DrawString("Atah.com/terminosycondicones/", fBody, sb, 55, espacio);

        //        g.Dispose();
        //    }
        //    catch (Exception err)
        //    {
        //        string error = err.Message;
        //        MessageBox.Show("Impresora no conectada.");
        //        string funcion = "llenarticket";
        //        Utilerias.LOG.write(_clase, funcion, error);


        //    }



        //}


        private void Ventas_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;

        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            groupBoxhuella.Visible = false;
        }

        private void button1_Click_3(object sender, EventArgs e)
        {
            labelmotivo.Visible = true;
            labelhuell.Visible = true;
            bool huella = verificationUserControl2.verificando();

            //if (huella == true && textBoxmotivo.Text != ""  || textBoxcontraseña.Text==contraseña && textBoxmotivo.Text != "")
                if (true)

                {

                    cancelarentabla();
                verificationUserControl2.limpiarhuella();
                labelmotivo.Visible = false;
                labelhuell.Visible = false;
                cancel = false;
                buttonborrar.Enabled = false;
                buttonreimprimir.Enabled = false;
                buttonborrar.BackColor = Color.White;
                buttonreimprimir.BackColor = Color.White;
                contando = 0;

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

        private void timer3_Tick(object sender, EventArgs e)
        {
            timer3.Stop();
            buttonborrar.Visible = true;
            buttonvender.Visible = true;
            buttonguia.Visible = true;
            buttonlimpiar.Visible = true;
            buttonlimpiarautobus.Visible = true;
            buttonactualizar.Visible = true;
            buttonbloquear.Visible = true;
            paneltablas.Visible = true;
            groupBoxdatos1.Visible = true;
            groupBoxdatos2.Visible = true;
            button4.Visible = true;
            button3.Visible = true;
            comboBoxlinea.Focus();
        }


        private void detalledelviaje()
        {

            if (CheckOpened("asientosocupadosboleto"))
            {
                form.StartPosition = FormStartPosition.CenterScreen;
                form.Show();

            }
            else
            {

                form = new Reportes.asientosocupadosboleto(int.Parse(pk), _origen, _linea, fechaa, _hora, ECO);
                AddOwnedForm(form);
                form.StartPosition = FormStartPosition.CenterScreen;
                form.Show();
                Utilerias.LOG.acciones("ingreso a asientosocupados ");


            }



        }



        private void Ventas_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F7 && verdetalle == true)
            {
                detalledelviaje();
            }
            if (e.KeyData == Keys.F5 && verdetalle == true)
            {
                detallegastos();
            }


        }
        private void detallegastos()
        {
            if (CheckOpened("GastosAutobus"))
            {
                form.StartPosition = FormStartPosition.CenterScreen;
                form.Show();

            }
            else
            {

                form = new Reportes.GastosAutobus(_fecha, ECO);
                AddOwnedForm(form);
                form.StartPosition = FormStartPosition.CenterScreen;
                form.Show();
                Utilerias.LOG.acciones("ingreso a asientosocupados ");


            }

        }
        private void button5_Click(object sender, EventArgs e)
        {
            detalledelviaje();
        }

        private void groupBoxdatos2_Enter(object sender, EventArgs e)
        {

        }

        private void datagridvendidos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void spacekey(object sender, KeyEventArgs e)
                 {

        }

        private void space(object sender, KeyEventArgs e)
        {

        }

        private void labeldestinobol_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            groupBoxhuella.Visible = false;
            verificationUserControl2.Stop();
            verificationUserControl2.Visible = false;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void textBoxboletosvendidos_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonborrar_MouseLeave(object sender, EventArgs e)
        {
            if (puedecancelar == true)
            {   
                verificationUserControl2.limpiarhuella();
            }
        }

        private void textBoxefectivocal_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            
            {

                if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back) && (e.KeyChar != 46) && (e.KeyChar != (char)Keys.Enter))
                {
                    permitircambio = false;
                    if ((e.KeyChar == '\u001b'))
                    {
                        return;
                    }
                    else
                    {
                        MessageBox.Show("Solo se permiten numeros", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        e.Handled = true;
                        return;
                    }
                }
                
                else
                {
                    permitircambio = true;

                    if ((e.KeyChar == (char)Keys.Enter))
                    {
                        Efectivo_Click(sender, e);
                    }

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
                    MessageBox.Show("Ocurrio un Error en vender.");
                string funcion = "textboxefectivocal";
                Utilerias.LOG.write(_clase, funcion, error);



            }

        }

        private void textBoxefectivocal_TextChanged(object sender, EventArgs e)
        {
            if (textBoxefectivocal.Text == "")
            {
                efectivocal = 0;

                textBoxcambiocal.Text = "$0";
            }
            else
            {
                efectivocal = float.Parse(textBoxefectivocal.Text);
                cambiocal = efectivocal - totalcal;
                textBoxcambiocal.Text = Utilerias.Utilerias.formatCurrency(cambiocal);
            }
    
        }

        private void verificationUserControl2_BackColorChanged(object sender, EventArgs e)
        {
            contando += 1;
            if (contando >= 5)
            {
                textBoxcontraseña.Text = "";
                textBoxcontraseña.Visible = true;
                textBoxcontraseña.Focus();
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            try
           
                {
                bool yano = false;
                bool yanol = false;
                bool yanob = false;

                int n = 100;
                int c = 100;
                if (e.KeyCode == Keys.Enter)
                {
                    string comando = textBoxcomando.Text;
                    bool donde = false;
                    bool incorrecto = false;

                    
                   
                        for (int i = 0; i < matrizpiso1.GetLength(0); i++)
                        {
                            for (int j = 0; j < matrizpiso1.GetLength(1); j++)
                            {
                                if (matrizpiso1[i, j] == comando)
                                {
                                    etiqueta = comando;
                                    n = i;
                                    c = j;
                                    _npiso = "1";
                                    donde = true;
                                    incorrecto = true;
                                }
                            }
                        }
                    
                    if (donde == false)
                    {
                        for (int i = 0; i < matrizpiso2.GetLength(0); i++)
                        {
                            for (int j = 0; j < matrizpiso2.GetLength(1); j++)
                            {
                                if (matrizpiso2[i, j] == comando)
                                {
                                    etiqueta = comando;
                                    n = i;
                                    c = j;
                                    _npiso = "2";
                                    incorrecto = true;
                                }
                            }
                        }
                    }

                    if ( n !=100)
                    {

                        string horacorta = comboBoxsalida.SelectedItem.ToString();
                        string corte = "0";
                        string statusselec = "";
                        string vended = "";
                        string tip = "";
                        if (_npiso == "1")
                        {
                            statusselec = matrizstatus[n, c];
                            vended = matrizvendedor[n, c];
                             tip = matriztipo[n, c];
                        }
                        if (_npiso == "2")
                        {
                            statusselec = matrizstatus2[n, c];
                            vended = matrizvendedor2[n, c];
                             tip = matriztipo2[n, c];
                        }
                    
                     
                        rutapasajero = textBoxruta.Text;
               
                        _pasajero = textBoxnombre.Text;
                        _destinoboleto_ = comboBoxdestinobol.Text;

                        executaProcedimientopreventa( _llegada, _hora, _linea, ECO, _fecha, etiqueta, _tarifa, _origen,
        _destino, _destinoboleto_, horacorta, tot.ToString(), _pasajero, _statustemporal, n.ToString(), c.ToString(),
        vendedoractual,
                          _npiso, corte, pkcorrida.ToString(), rutapasajero, chofer, tip);


                        if (ERROR=="ASIENTO OCUPADO" && vendedoractual == VENDTEMPO && statusselec != "VENDIDO")
                        {


                            string sql3;
                            sql3 = "DELETE FROM VENDIDOS  WHERE ASIENTO=@ASIENTO AND (SALIDA  BETWEEN  SALIDA AND CONVERT (datetime,@LLEGADA ) ) AND" +
                        "(LLEGADA  BETWEEN  CONVERT(datetime, @SALIDA) AND LLEGADA) AND LINEA=@LINEA  AND FECHA=@FECHA AND ECO=@ECO AND VENDEDOR=@VENDEDOR AND (STATUS='PREVENTA' OR STATUS='BLOQUEADO')";


                            db.PreparedSQL(sql3);
                            db.command.Parameters.AddWithValue("@ASIENTO", etiqueta);
                            db.command.Parameters.AddWithValue("@SALIDA", _hora);
                            db.command.Parameters.AddWithValue("@LINEA", _linea);
                            db.command.Parameters.AddWithValue("@LLEGADA", _llegada);
                            db.command.Parameters.AddWithValue("@FECHA", _fecha);
                            db.command.Parameters.AddWithValue("@ECO", ECO);
                            db.command.Parameters.AddWithValue("@VENDEDOR", vendedoractual);

                            if (db.execute())
                            {

                                Utilerias.LOG.acciones("quito  de preventa un boleto " + _folio);
                                if (tip == "asiento")
                                {
                                    Bitmap asientoimg = new Bitmap(imagenansiento);
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
                                    string texto = etiqueta;

                                    string firstText = texto;
                                    PointF firstLocation = new PointF(1f, 4f);
                                    using (Graphics graphics = Graphics.FromImage(asientoimg))
                                    {
                                        using (Font arialFont = new Font("Arial", 12))
                                        {
                                            graphics.DrawString(firstText, arialFont, Brushes.White, firstLocation);
                                        }
                                    }
                                    if (_npiso == "1")
                                    {
                                        autobus.Rows[n].Cells[c].Value = asientoimg;
                                        matrizstatus[n, c] = "";
                                        matrizvendedor[n, c] = "";
                                    }
                                    if (_npiso == "2")
                                    {
                                        PISOS2.Rows[n].Cells[c].Value = asientoimg;
                                        matrizstatus2[n, c] = "";
                                        matrizvendedor2[n, c] = "";
                                    }
                                    

                                }
                                if (tip == "Asiento con pantalla")
                                {

                                    asientoconpantalla(Color.Green, etiqueta, n, c, tip, true);
                                }

                                for (int i = 0; i < agregados.RowCount; i++)
                                {


                                    if (agregados.Rows[i].Cells["asientoname"].Value.ToString() == etiqueta)
                                    {
                                        agregados.Rows.RemoveAt(i);
                                        totalsumatori();
                                    }
                                }



                            }
                            if (agregados.Rows.Count < 1)
                            {
                                buttonbloquear.Enabled = false;
                                buttonbloquear.BackColor = Color.White;
                            }
                            if (_npiso == "1")
                            {
                                matrizstatus[n, c] = "";
                            }
                            if (_npiso == "2")
                            {
                                matrizstatus2[n, c] = "";
                            }
                        }

                        else if (ERROR == "HUBO UN CAMBIO DE AUTOBUS")
                        {
                            limpiar();
                            limpiarpreventa();
                            limpiarformpago();
                            formadepago = "Efectivo";

                            foliotarjeta = " ";
                            digitostarjeta = " ";
                            groupBoxformadepago.Visible = false;
                            buttonborrar.Enabled = false;
                            buttonreimprimir.Enabled = false;
                            return;
                        }
                        else if (ERROR=="ASIENTO OCUPADO" && vendedoractual != VENDTEMPO && statusselec != "VENDIDO")
                        {
                            getRowsVendidos2();
                            if (agregados.Rows.Count < 1)
                            {
                                buttonbloquear.Enabled = false;
                                buttonbloquear.BackColor = Color.White;
                            }
                        }
                        else if (ERROR=="Ya no puedes seleccionar tipo de pasaje")
                        {
                            yano = true;
                        }
                        else if (ERROR=="La guia ya se genero")
                        {
                            yanol = true;
                        }
                        else if (ERROR == "Corrida bloqueada")
                        {
                            yanob = true;
                        }
                        else if ((ERROR=="Boleto vendido " && statusselec == "") || (ERROR=="Boleto vendido " && statusselec == "CANCELADO"))
                        {





                            tot = float.Parse(_tarifa);
                            sub = tot * 100 / IVA;

                            ivasub = tot - sub;
                            _fila = n;
                            _columna = c;
                            rutapasajero = textBoxruta.Text;
                              if (_npiso == "1")
                            {
                                matrizstatus[n, c] = "PREVENTA";

                            }

                            if (_npiso == "2")
                            {
                                matrizstatus2[n, c] = "PREVENTA";

                            }


                            if (tip == "asiento")
                            {
                                Bitmap asientoimg = new Bitmap(imagenazul);
                                for (int Xcount = 5; Xcount < 15; Xcount++)
                                {
                                    for (int Ycount = 0; Ycount < 43; Ycount++)
                                    {
                                        asientoimg.SetPixel(Xcount, Ycount, Color.LightSkyBlue);
                                    }
                                }
                                for (int Xcount = 35; Xcount < 45; Xcount++)
                                {
                                    for (int Ycount = 0; Ycount < 43; Ycount++)
                                    {
                                        asientoimg.SetPixel(Xcount, Ycount, Color.LightSkyBlue);
                                    }
                                }
                                for (int Xcount = 1; Xcount < 49; Xcount++)
                                {
                                    for (int Ycount = 0; Ycount < 22; Ycount++)
                                    {
                                        asientoimg.SetPixel(Xcount, Ycount, Color.LightSkyBlue);
                                    }
                                }
                                for (int Xcount = 15; Xcount < 35; Xcount++)
                                {
                                    for (int Ycount = 0; Ycount < 40; Ycount++)
                                    {
                                        asientoimg.SetPixel(Xcount, Ycount, Color.LightSkyBlue);
                                    }
                                }
                                string texto = etiqueta;

                                string firstText = texto;
                                PointF firstLocation = new PointF(1f, 4f);
                                using (Graphics graphics = Graphics.FromImage(asientoimg))
                                {
                                    using (Font arialFont = new Font("Arial", 12))
                                    {
                                        graphics.DrawString(firstText, arialFont, Brushes.White, firstLocation);
                                    }
                                }
                                if (_npiso == "1")
                                {
                                    autobus.Rows[n].Cells[c].Value = asientoimg;

                                }
                                if (_npiso == "2")
                                {
                                    PISOS2.Rows[n].Cells[c].Value = asientoimg;

                                }

                            }
                            if (tip == "Asiento con pantalla")
                            {
                                asientoconpantalla(Color.LightSkyBlue, etiqueta, n, c, tip, true);

                            }
                            if (_npiso == "1")
                            {
                                matrizvendedor[n, c] = vendedoractual;

                            }
                            if (_npiso == "2")
                            {
                                matrizvendedor2[n, c] = vendedoractual;

                            }


                            agregados.Rows.Add(_folio, _linea, _origen, _fecha, _destino, _hora, _destinoboleto_, _pasaje, tot, etiqueta, _pasajero, Utilerias.Utilerias.formatCurrency(sub), Utilerias.Utilerias.formatCurrency(ivasub), Utilerias.Utilerias.formatCurrency(tot), _statustemporal, _fila, _columna, ECO, _llegada, _hora, _npiso, pk,pkvendido);
                            totalsumatori();
                            Utilerias.LOG.acciones("preventa de  un boleto " + _folio);
                            Buttonactualizar_Click(sender, e);
                            if (agregados.Rows.Count < 1)
                            {
                                buttonbloquear.Enabled = false;
                                buttonbloquear.BackColor = Color.White;
                            }

                        }
                    }
                    textBoxcomando.Text = "";

                }
                if (yano == true)
                {
                    Form mensaje = new Mensaje("Tarifa agotada", true);
                    mensaje.ShowDialog();
                }
                if (yanol == true)
                {
                    Form mensaje = new Mensaje("Ya no puedes seleccionar tipo de pasaje", true);
                    mensaje.ShowDialog();
                }
                if (yanob == true)
                {
                    Form mensaje = new Mensaje(ERROR, true);
                    mensaje.ShowDialog();
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
                    MessageBox.Show("Ocurrio un Error en vender.");
                string funcion = "rapido";
                Utilerias.LOG.write(_clase, funcion, error);



            }

        }

        private void buttonbloquear_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < agregados.Rows.Count; i++)
                {





                    Bitmap bloqueo = new Bitmap(imagenbloqueo);
                    _asiento = agregados.Rows[i].Cells[9].Value.ToString();
                    totalm = agregados.Rows[i].Cells["totalname"].Value.ToString();
                    _folio = agregados.Rows[i].Cells[0].Value.ToString();
                    _pasaje = agregados.Rows[i].Cells["tarifaname"].Value.ToString();
                    nombre = agregados.Rows[i].Cells["pasajeroname"].Value.ToString();
                    string _p = agregados.Rows[i].Cells[20].Value.ToString();
                    string pisot= agregados.Rows[i].Cells["pisosname"].Value.ToString();
                    string _pk = agregados.Rows[i].Cells[21].Value.ToString();
                    string _fila = agregados.Rows[i].Cells[15].Value.ToString();
                    string _columna = agregados.Rows[i].Cells[16].Value.ToString();

                    codigoqr(_folio);
                    if (pisot == "1")
                    {
                        matrizstatus[int.Parse( _fila),int.Parse( _columna)]="BLOQUEADO";

                    }
                    if (pisot == "2")
                    {
                        matrizstatus2[int.Parse(_fila), int.Parse(_columna)] = "BLOQUEADO";

                    }

                    string status = "BLOQUEADO";

                    string sql = "UPDATE  VENDIDOS SET STATUS=@STATUS WHERE FOLIO=@FOLIO ";

                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@FOLIO", _folio);
                    db.command.Parameters.AddWithValue("@STATUS", status);


                    if (db.execute())
                    {
                        Utilerias.LOG.acciones("vendio un boleto" + _folio);
                        _total = 0;

                        string firstText = _asiento;
                        PointF firstLocation = new PointF(1f, 4f);
                        using (Graphics graphics = Graphics.FromImage(bloqueo))
                        {
                            using (Font arialFont = new Font("Arial", 12))
                            {
                                graphics.DrawString(firstText, arialFont, Brushes.White, firstLocation);
                            }
                        }


                        if (_p == "1")
                        {
                            autobus.Rows[int.Parse(_fila)].Cells[int.Parse(_columna)].Value = bloqueo;
                        }
                        if (_p == "2")
                        {
                            PISOS2.Rows[int.Parse(_fila)].Cells[int.Parse(_columna)].Value = bloqueo;
                        }

                    }
                }

                getRowsVendidos2();



                columnas.Clear();
                filas.Clear();
                etiquetass.Clear();
                agregados.Rows.Clear();
                textBoxtotal.Text = "$0";
                vender = false;
                textBoxnombre.Text = "Publico en General";
                buttonbloquear.Enabled = false;
                buttonbloquear.BackColor = Color.White;
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
                    MessageBox.Show("Ocurrio un Error en vender.");
                string funcion = "buttonactualizar";
                Utilerias.LOG.write(_clase, funcion, error);


            }

        }

        private void autobus_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if ((e.ColumnIndex == this.autobus.Columns[0].Index)
           && e.Value != null)
                {
                    DataGridViewCell cell =
                        this.autobus.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    if (e.Value.Equals("*"))
                    {
                        cell.ToolTipText = "very bad";
                    }
                    else if (e.Value.Equals("**"))
                    {
                        cell.ToolTipText = "bad";
                    }
                    else if (e.Value.Equals("***"))
                    {
                        cell.ToolTipText = "good";
                    }
                    else if (e.Value.Equals("****"))
                    {
                        cell.ToolTipText = "very good";
                    }
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
                    MessageBox.Show("Ocurrio un Error en vender.");
                string funcion = "buttonactualizar";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }

        private void autobus_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            try { 
            int col = e.ColumnIndex;
            int row = e.RowIndex;
                string origenm = "";
                string destinom = "";
                string tarifanom = "";
                string pasajenom = "";
                string formanom = "";

                string venn = "";

                string asiento = matrizpiso1[row, col];
            if (asiento != "")
            {

                    for (int i=0;i<datagridvendidos.Rows.Count;i++)
                    {
                        string Valor = datagridvendidos.Rows[i].Cells["asientoinfo"].Value.ToString();

                        if (Valor == asiento)
                    {

                         origenm= datagridvendidos.Rows[i].Cells["origenven"].Value.ToString();
                         destinom = datagridvendidos.Rows[i].Cells["destinoven"].Value.ToString();
                            tarifanom = datagridvendidos.Rows[i].Cells["tarifaven"].Value.ToString();
                            pasajenom = datagridvendidos.Rows[i].Cells["pasajeroven"].Value.ToString();
                            formanom = datagridvendidos.Rows[i].Cells["formapagoname"].Value.ToString();
                             venn= datagridvendidos.Rows[i].Cells["VENDEDOR"].Value.ToString();


                            DataGridViewCell cell = autobus.Rows[row].Cells[col];
                            cell.ToolTipText = "Asiento: " + asiento + ", " + origenm + "-" + destinom + ", "
                                + "Tarifa: " + tarifanom + ", " + "Usuario: " + pasajenom + ", " + "Forma de pago: " + formanom
                                + ", " + "Vendedor: " +venn;
                        }
                }
       

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
                    MessageBox.Show("Ocurrio un Error en vender.");
                string funcion = "autobus_CellMouseMove";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }

        private void PISOS2_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                int col = e.ColumnIndex;
                int row = e.RowIndex;
                string origenm = "";
                string destinom = "";
                string tarifanom = "";
                string pasajenom = "";
                string formanom = "";
                string venn = "";

                string asiento = matrizpiso2[row, col];
                if (asiento != "")
                {

                    for (int i = 0; i < datagridvendidos.Rows.Count; i++)
                    {
                        string Valor = datagridvendidos.Rows[i].Cells["asientoinfo"].Value.ToString();

                        if (Valor == asiento)
                        {

                            origenm = datagridvendidos.Rows[i].Cells["origenven"].Value.ToString();
                            destinom = datagridvendidos.Rows[i].Cells["destinoven"].Value.ToString();
                            tarifanom = datagridvendidos.Rows[i].Cells["tarifaven"].Value.ToString();
                            pasajenom = datagridvendidos.Rows[i].Cells["pasajeroven"].Value.ToString();
                            formanom = datagridvendidos.Rows[i].Cells["formapagoname"].Value.ToString();
                            venn = datagridvendidos.Rows[i].Cells["VENDEDOR"].Value.ToString();


                            DataGridViewCell cell = PISOS2.Rows[row].Cells[col];
                            cell.ToolTipText = "Asiento: " + asiento + ", " + origenm + "-" + destinom + ", "
                                + "Tarifa: " + tarifanom + ", " + "Usuario: " + pasajenom + ", " + "Forma de pago: " + formanom
                                + ", " + "Vendedor: " + venn;
                        }
                    }


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
                    MessageBox.Show("Ocurrio un Error en vender.");
                string funcion = "PISOS2_CellMouseMove";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }

        private void textBoxnombre_KeyPress(object sender, KeyPressEventArgs e)
        {
          
        }

        private void comboBoxtarifa_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar != (char)Keys.R))
            {
                textBoxcomando.Focus();
                textBoxcomando.Text = "";

            }
        }

        private void comboBoxdestinobol_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar != (char)Keys.R))
            {
                textBoxcomando.Focus();
                textBoxcomando.Text = "";

            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            textBoxcomando.Text = "";
        }

        private void textBoxmotivo_KeyPress(object sender, KeyPressEventArgs e)
        {

            if ((e.KeyChar == (char)Keys.Enter))
            {
                labelmotivo.Visible = true;
                labelhuell.Visible = true;
                bool huella = verificationUserControl2.verificando();

                if (huella == true && textBoxmotivo.Text != "" || textBoxcontraseña.Text == contraseña && textBoxmotivo.Text != "")
                {

                    cancelarentabla();
                    verificationUserControl2.limpiarhuella();
                    labelmotivo.Visible = false;
                    labelhuell.Visible = false;
                    cancel = false;
                    buttonborrar.Enabled = false;
                    buttonreimprimir.Enabled = false;
                    buttonborrar.BackColor = Color.White;
                    buttonreimprimir.BackColor = Color.White;
                    contando = 0;

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

        private void textBoxcontraseña_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar == (char)Keys.Enter))
            {
                labelmotivo.Visible = true;
                labelhuell.Visible = true;
                bool huella = verificationUserControl2.verificando();

                if (huella == true && textBoxmotivo.Text != "" || textBoxcontraseña.Text == contraseña && textBoxmotivo.Text != "")
                {

                    cancelarentabla();
                    verificationUserControl2.limpiarhuella();
                    labelmotivo.Visible = false;
                    labelhuell.Visible = false;
                    cancel = false;
                    buttonborrar.Enabled = false;
                    buttonreimprimir.Enabled = false;
                    buttonborrar.BackColor = Color.White;
                    buttonreimprimir.BackColor = Color.White;
                    contando = 0;

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

        private void timerocultarformadepago_Tick(object sender, EventArgs e)
        {
     
            timerefectivo.Stop();
            formadepago = "Efectivo";

            foliotarjeta = " ";
            digitostarjeta = " ";
            autobus.Enabled = true;
            PISOS2.Enabled = true;
            if (pie == false)

                ventapie();
            else
                aceptarventa();

            progressBar1.Visible = false;
            progressBar1.Value = 0;

            buttonvender.Enabled = true;
            buttonguia.Enabled = true;
            buttonactualizar.Enabled = true;
            buttonlimpiarautobus.Enabled = true;
            buttonlimpiar.Enabled = true;
         

        }

        private void timerdebito_Tick(object sender, EventArgs e)
        {
      
            timerdebito.Stop();
            tdebito.Text = "Tarjeta de Debito";
            autobus.Enabled = true;
            PISOS2.Enabled = true;
            if (pie == false)
                ventapie();
            else
                aceptarventa();

            ventavalidar = false;
            Efectivo.Enabled = true;
            tdebito.Enabled = true;
            tcredito.Enabled = true;

            progressBar1.Visible = false;
            progressBar1.Value = 0;

            buttonvender.Enabled = true;
            buttonguia.Enabled = true;
            buttonactualizar.Enabled = true;
            buttonlimpiarautobus.Enabled = true;
            buttonlimpiar.Enabled = true;

        }

        private void timercredito_Tick(object sender, EventArgs e)
        {
          
            timercredito.Stop();
            tcredito.Text = "Tarjeta de Credito";
            autobus.Enabled = true;
            PISOS2.Enabled = true;
            if (pie == false)
                ventapie();
            else
                aceptarventa();

            ventavalidar = false;
            Efectivo.Enabled = true;
            tdebito.Enabled = true;
            tcredito.Enabled = true;

            foliotext.Focus();

            progressBar1.Visible = false;
            progressBar1.Value = 0;
            buttonvender.Enabled = true;
            buttonguia.Enabled = true;
            buttonactualizar.Enabled = true;
            buttonlimpiarautobus.Enabled = true;
            buttonlimpiar.Enabled = true;
        
        }

        private void backgroundWorkerventa_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void textBoxefectivocal_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyData==Keys.Escape)
            {
                buttonvender.Enabled = true;
                buttonguia.Enabled = true;
                buttonactualizar.Enabled = true;
                buttonlimpiarautobus.Enabled = true;
                buttonlimpiar.Enabled = true;
                buttonbloquear.Enabled = true;
                groupBoxformadepago.Visible = false;
                autobus.Enabled = true;
                PISOS2.Enabled = true;
                limpiarformpago();

            }
        }

        private void comboBoxorigen_SelectedValueChanged(object sender, EventArgs e)
        {

        }
    }
}