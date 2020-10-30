using ConnectDB;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autobuses.Utilerias;
using System.Globalization;

namespace Autobuses
{
    public partial class Corte_de_Caja : Form
    {


        public database db;
        ResultSet res = null;
        private int bcemitidos=0;
        private double biemitidos;
        private string contraseña = "";
        private int bccancelado = 0;
        private double bicancelados = 0.0;
        private double bcventa;
        private double biventa;
        private string bccancfdt = "0";
        private double bicancfdt = 0.0;
        private string bcree = "0";
        private double biree = 0.0;
        private int contando = 0;
        private string gemitidas = "0";
        private string gcanceladas = "0";
        private double gimporte = 0.0;
        private double gsalida = 0.0;
        private double gcomisiontaq = 0.0;
        private double gcomisionbanco = 0.0;
        private double gaportacion = 0.0;
        private double gdiesel = 0.0;
        private double gcaseta = 0.0;
        private double gtarjeta = 0.0;
        private double giva = 0.0;
        private double ganticipo = 0.0;
        private double gtotal = 0.0;
        string _clase = "corte de caja";
        private double cventa = 0.0;
        private double ccancfueradt = 0.0;
        private double canticipos = 0.0;
        private double ctarjetas = 0.0;
        private double ccoutassalidas = 0.0;
        private double ccomisiones = 0.0;
        private double ctarjeta = 0.0;
        private double caportaciones = 0.0;
        private double cdiesel = 0.0;
        private double ccaseta = 0.0;
        private double tsalida = 0.0;
        private double tturno = 0.0;
        private double tpaso = 0.0;
        private double ctotalae = 0;
        private double cefectivo = 0;
        private double ctarjetacred = 0;
        private double ctarjetad = 0;
        private double cvales = 0;
        private int tamaño;
        private int espacio;
        private string folio;
        private Bitmap imagen;
        private string sucursal = LoginInfo.Sucursal;
        private string fechai = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        private string fechaf;
        byte[] fingerPrint;
        List<string> iList = new List<string>();
        string user = "";
        private string usuarioconsulta="";
        private double efectivo = 0.0;
        private double tdebito = 0.0;
        private double tcredito = 0.0;
        private bool cualquiercorte = false;

        private Bitmap imagensplash = new Bitmap(Autobuses.Properties.Resources.logotickets);

        public Corte_de_Caja()
        {
            InitializeComponent();
            db = new database();
            CultureInfo.GetCultureInfo("es-MX");
            timer1.Interval = 1;
            timer1.Start();
        }
        private bool CheckOpened(string name)
        {

            FormCollection fc = Application.OpenForms;
            foreach (Form frm in fc)
            {
                if (frm.Text == name)
                {
                    return true;
                }
            }
            return false;
        }
        public void checkInternetAvaible()
        {
            if (!Utilerias.Utilerias.CheckForInternetConnection())
            {
                MessageBox.Show("Error al conectarse a internet");
                return;

            }
        }

        private void permisos()
        {

            if (LoginInfo.privilegios.Any(x => x == "Generar cualquier corte"))
            {
               cualquiercorte = true;

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
                    verificationUserControl1.Samples.Add(new DPFP.Template(new MemoryStream(fingerPrint)), null);
                }
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "validaruser";
                Utilerias.LOG.write(_clase, funcion, error);


            }


        }

        private void fecha()
        {
            try

            {

                string sql = "SELECT TOP 1 FECHAC FROM VENDIDOS WHERE CORTE='0' AND STATUS='PREVENTA' AND NOT STATUS='BLOQUEADO' AND  VENDEDOR=@USUARIO ORDER BY FECHAC asc";
                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@USUARIO", usuarioconsulta);

                res = db.getTable();

                if (res.Next())
                {
                    fechai = res.Get("FECHAC");
                    if (fechai.Length > 19)
                    {
                        fechai = fechai.Substring(0, 19);
                    }

                    textBoxini.Text = fechai;
                 
                }
                else
                {
                    fechai = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                    textBoxini.Text = fechai;
              
                }
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "fecha";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }
        public void boletosinfo()
        {
            try

            {

                string sql = "SELECT (sum(CAST(PRECIO as DECIMAL(9,2)))) as total, count(precio) as cantidad FROM VENDIDOS" +
                    " WHERE  CORTE='0' AND VENDEDOR=@VENDEDOR AND (STATUS='VENDIDO' OR STATUS='CANCELADO')";
                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@VENDEDOR", usuarioconsulta);
                int n = 0;
                res = db.getTable();

                if (res.Next())
                {

                    biemitidos = (double.TryParse(res.Get("total"),out double aux1))?res.GetDouble("total"):0.0;
                    bcemitidos = (int.TryParse(res.Get("cantidad"),out int aux2))?res.GetInt("cantidad"):0;
                    boletoscalculo();

                }


            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "boletosinfo";
                Utilerias.LOG.write(_clase, funcion, error);


            }

        }

        public void boletosinfocancelado()
        {
            try

            {

                string sql = "SELECT (sum(CAST(PRECIO as DECIMAL(9,2)))) as total, count(precio) as cantidad FROM VENDIDOS " +
                    "WHERE CORTECANCELADO='0' AND CANCELADO=@VENDEDOR AND STATUS='CANCELADO'";
                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@VENDEDOR", usuarioconsulta);
                int n = 0;
                res = db.getTable();

                if (res.Next())
                {


                    bicancelados = (double.TryParse(res.Get("total"), out double aux1)) ? res.GetDouble("total") : 0.0;
                    bccancelado = (int.TryParse(res.Get("cantidad"), out int aux2)) ? res.GetInt("cantidad") : 0;

                }
               
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "boletosinfocancelado";
                Utilerias.LOG.write(_clase, funcion, error);


            }

        }
        public void pagoefectivo()
        {
            try

            {

                string sql = "SELECT (sum(CAST(PRECIO as DECIMAL(9,2)))) as total FROM VENDIDOS WHERE " +
                    "CORTE='0' AND VENDEDOR=@VENDEDOR AND FORMADEPAGO='EFECTIVO' AND STATUS='VENDIDO'";

                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@VENDEDOR", usuarioconsulta);
                int n = 0;
                res = db.getTable();

                if (res.Next())
                {


                    efectivo = (double.TryParse(res.Get("total"), out double aux1)) ? res.GetDouble("total") : 0.0;

                }

            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "pagoefectivo";
                Utilerias.LOG.write(_clase, funcion, error);


            }

        }

        public void TARJETADEBITO()
        {
            try

            {

                string sql = "SELECT (sum(CAST(PRECIO as DECIMAL(9,2)))) as total FROM VENDIDOS WHERE " +
                    "CORTE='0' AND VENDEDOR=@VENDEDOR AND FORMADEPAGO='T. DEBITO' AND STATUS='VENDIDO' ";

                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@VENDEDOR", usuarioconsulta);
                int n = 0;
                res = db.getTable();

                if (res.Next())
                {


                    tdebito = (double.TryParse(res.Get("total"), out double aux1)) ? res.GetDouble("total") : 0.0;

                }

            }
            catch (Exception err)
            {
                string error = err.Message;
              //  MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "tarjetadebito";
                Utilerias.LOG.write(_clase, funcion, error);


            }

        }
        public void TARJETACREDITO()
        {
            try

            {

                string sql = "SELECT (sum(CAST(PRECIO as DECIMAL(9,2)))) as total FROM VENDIDOS WHERE " +
                    "CORTE='0' AND VENDEDOR=@VENDEDOR AND FORMADEPAGO='T. CREDITO' AND STATUS='VENDIDO'";

                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@VENDEDOR", usuarioconsulta);
                int n = 0;
                res = db.getTable();

                if (res.Next())
                {


                    tcredito = (double.TryParse(res.Get("total"), out double aux1)) ? res.GetDouble("total") : 0.0;

                }

            }
            catch (Exception err)
            {
                string error = err.Message;
               // MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "tarjetacredito";
                Utilerias.LOG.write(_clase, funcion, error);


            }


        }
        public void getRows()
        {
            try
            {
            

               string sql = "   SELECT COUNT(*) AS CANTIDAD," +
"sum(CAST(IMPORTE as DECIMAL(9, 2))) as IMPORTE," +
"sum(CAST(COMTAQ as DECIMAL(9, 2))) as COMTAQ ," +
"sum(CAST(COMPBAN as DECIMAL(9, 2))) as COMPBAN ," +
"sum(CAST(APORTACION as DECIMAL(9, 2))) as APORTACION," +
"sum(CAST(DIESEL as DECIMAL(9, 2))) as DIESEL," +
"sum(CAST(CASETA as DECIMAL(9, 2))) as CASETA," +
"sum(CAST(IVA as DECIMAL(9, 2))) as IVA," +
"sum(CAST(ANTICIPO as DECIMAL(9, 2))) as ANTICIPO," +
"sum(CAST(TOTAL as DECIMAL(9, 2))) as TOTAL, " +
"sum(CAST(TSALIDA as DECIMAL(9, 2))) as TSALIDA, " +
"sum(CAST(TTURNO as DECIMAL(9, 2))) as TTURNO, " +
"sum(CAST(TPASO as DECIMAL(9, 2))) as TPASO " +
"FROM GUIA " +
"WHERE CORTE =0 AND VALIDADOR=@VALIDADOR";
                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@VALIDADOR", usuarioconsulta);



                res = db.getTable();

                if (res.Next())
                {

                    gemitidas = res.Get("CANTIDAD");
                    gcanceladas = "0";   //res.Get("IMPORTE");
                    gimporte = (double.TryParse(res.Get("IMPORTE"), out double aux1)) ? res.GetDouble("IMPORTE") : 0.0;
                    gcomisiontaq = (double.TryParse(res.Get("COMTAQ"), out double aux2)) ? res.GetDouble("COMTAQ") : 0.0;
                    gcomisionbanco = (double.TryParse(res.Get("COMPBAN"), out double aux3)) ? res.GetDouble("COMPBAN") : 0.0;
                    gaportacion = (double.TryParse(res.Get("APORTACION"), out double aux4)) ? res.GetDouble("APORTACION") : 0.0;
                    gdiesel = (double.TryParse(res.Get("DIESEL"), out double aux5)) ? res.GetDouble("DIESEL") : 0.0;
                    gcaseta = (double.TryParse(res.Get("CASETA"), out double aux6)) ? res.GetDouble("CASETA") : 0.0;
                    giva = (double.TryParse(res.Get("IVA"), out double aux7)) ? res.GetDouble("IVA") : 0.0;
                    ganticipo = (double.TryParse(res.Get("ANTICIPO"), out double aux8)) ? res.GetDouble("ANTICIPO") : 0.0;
                    gtotal = (double.TryParse(res.Get("TOTAL"),out double aux9))?res.GetDouble("TOTAL"):0.0;
                    tturno = (double.TryParse(res.Get("TTURNO"), out double aux10)) ? res.GetDouble("TTURNO") : 0.0;
                    tsalida = (double.TryParse(res.Get("TSALIDA"), out double aux11)) ? res.GetDouble("TSALIDA") : 0.0;
                    tpaso = (double.TryParse(res.Get("TPASO"), out double aux12)) ? res.GetDouble("TPASO") : 0.0;

                   textBoxgemitidas.Text= gemitidas;
                   textBoxgcanceladas.Text= gcanceladas;
                   textBoxgimporte.Text=  Utilerias.Utilerias.formatCurrency( gimporte);
                   textBoxgcomisiontaq.Text= Utilerias.Utilerias.formatCurrency(gcomisionbanco);
                 //  textBoxgcomisionbanco.Text = gcomisionbanco;
                   textBoxgaportacion.Text= Utilerias.Utilerias.formatCurrency(gaportacion);
                   //textBoxgdiesel.Text= gdiesel;
                   //textBoxgcasetas.Text= gcaseta;
                   textBoxgiva.Text= Utilerias.Utilerias.formatCurrency(giva);
                   textBoxganticipos.Text= Utilerias.Utilerias.formatCurrency(ganticipo);
                   textBoxgtotal.Text= Utilerias.Utilerias.formatCurrency(gtotal);
                    textBoxturno.Text = Utilerias.Utilerias.formatCurrency(tturno);
                    textBoxsalida.Text = Utilerias.Utilerias.formatCurrency(tsalida);
                    textBoxpaso.Text = Utilerias.Utilerias.formatCurrency(tpaso);

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
        private void caja()
        {


            try
            {
                cventa = biemitidos;
                ccancfueradt = bicancelados;
                canticipos = ganticipo;
                ctarjeta = gtarjeta;
                ccoutassalidas = gsalida;
                ccomisiones = gcomisiontaq;
                caportaciones = gaportacion;
                //ccomisiones = gcomisionbanco;
                cdiesel = gdiesel;
                ccaseta = gcaseta;
                ctotalae = Convert.ToDouble(cventa) - Convert.ToDouble(bicancelados) - Convert.ToDouble(canticipos) - Convert.ToDouble(ctarjeta)
                    - Convert.ToDouble(cdiesel) - Convert.ToDouble(ccaseta);
                cefectivo = Convert.ToDouble(cventa) - Convert.ToDouble(ccancfueradt) - Convert.ToDouble(canticipos)
                    - Convert.ToDouble(ccoutassalidas) - Convert.ToDouble(cdiesel) - Convert.ToDouble(ccaseta)
                    - Convert.ToDouble(tcredito) - Convert.ToDouble(tdebito);


                textBoxcventa.Text = Utilerias.Utilerias.formatCurrency(cventa);
                textBoxccancfueradt.Text = Utilerias.Utilerias.formatCurrency(ccancfueradt);
                textBoxcanticipos.Text = Utilerias.Utilerias.formatCurrency(canticipos);
                textBoxctarjeta.Text = Utilerias.Utilerias.formatCurrency(ctarjeta);
                textBoxccomision.Text = Utilerias.Utilerias.formatCurrency(ccomisiones);
                textBoxcaportacion.Text = Utilerias.Utilerias.formatCurrency(caportaciones);
                textBoxccomision.Text = Utilerias.Utilerias.formatCurrency(caportaciones);
            
                textBoxtotalaentr.Text = Utilerias.Utilerias.formatCurrency(ctotalae);
                textBoxcefectivo.Text = Utilerias.Utilerias.formatCurrency(cefectivo);
                textBoxctarjetascred.Text = Utilerias.Utilerias.formatCurrency(tcredito);
                textBoxctarjetadeb.Text = Utilerias.Utilerias.formatCurrency(tdebito);
                double tvales = 0.0;
                textBoxvales.Text = Utilerias.Utilerias.formatCurrency(tvales);
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "caja";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }
        private void boletoscalculo()

        {
            try
            {
                textBoxbcemitidos.Text = bcemitidos.ToString();
                textBoxbiemitidos.Text = Utilerias.Utilerias.formatCurrency(biemitidos);
                textBoxbccancelados.Text = bccancelado.ToString();
                textBoxbicancelados.Text = Utilerias.Utilerias.formatCurrency(bicancelados);
                bcventa = Convert.ToDouble(bcemitidos - bccancelado);

                textBoxbcventa.Text = bcventa.ToString();
                biventa = Convert.ToDouble(biemitidos - bicancelados);

                textBoxbiventa.Text = Utilerias.Utilerias.formatCurrency(biventa);
               
                textBoxbcree.Text = bcree;
                textboxbiree.Text = Utilerias.Utilerias.formatCurrency(biree);
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "boletoscalculo";
                Utilerias.LOG.write(_clase, funcion, error);


            }
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
                imagen.Save("imagen.png", ImageFormat.Png);

                // Guardar en el disco duro la imagen (Carpeta del proyecto)

            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "codigoqr";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }

        public String GenerateRandom()
        {
            string foliotemp = "";
            try
            {
                string sql = "SELECT dbo.f_Get_Folio_Corte() as FOLIO";


                db.PreparedSQL(sql);
                res = db.getTable();
                if (res.Next())
                {
                    foliotemp = res.Get("FOLIO");


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
                string funcion = "generaterandom";
                Utilerias.LOG.write(_clase, funcion, error);


            }
            return foliotemp;
        }


        void llenarticket(object sender, PrintPageEventArgs e)
        {
            try
                 {
                Graphics g = e.Graphics;
                Font fBody7 = new Font("Lucida", 7, FontStyle.Bold);
                Font fBody5 = new Font("Lucida", 5, FontStyle.Bold);
                Font fBody9 = new Font("Lucida", 9, FontStyle.Bold);
                Font fBody = new Font("Lucida", 8, FontStyle.Bold);
                Font fBody10 = new Font("Lucida", 10, FontStyle.Bold);
                Font fBody12 = new Font("Lucida", 12, FontStyle.Bold);
                Font fBody18 = new Font("Lucida", 18, FontStyle.Bold);
                Color customColor = Color.FromArgb(255, Color.Black);
                SolidBrush sb = new SolidBrush(customColor);
                g.DrawImage(imagensplash, 0, 0);
                espacio = 0;
                g.DrawString("Fecha :", fBody7, sb, 180, espacio);
                g.DrawString(DateTime.Now.ToShortDateString(), fBody7, sb, 215, espacio);
                g.DrawString("Hora :", fBody7, sb, 180, espacio + 20);
                g.DrawString(DateTime.Now.ToShortTimeString(), fBody7, sb, 210, espacio + 20);
                g.DrawString("SUCURSAL: " + sucursal, fBody, sb, 140, espacio+40);
                espacio = espacio + 70;
                g.DrawString("Folio: " + folio, fBody, sb, 0, espacio);
                espacio = espacio + 20;
                g.DrawString("Cajero: " + usuarioconsulta, fBody10, sb, 0, espacio);
                espacio = espacio + 20;
                g.DrawString("Apartir de: " + fechai, fBody10, sb, 0, espacio);
                espacio = espacio + 20;
                g.DrawString("Hasta: " + fechaf, fBody10, sb, 0, espacio);
                espacio = espacio + 20;

                g.DrawRectangle(Pens.Black, 0, espacio, 267, 80);
                g.DrawRectangle(Pens.Black, 0, espacio, 267, 20);

                espacio = espacio + 2;
                g.DrawString("Boletos", fBody10, sb, 0, espacio);
                g.DrawString("Cantidad", fBody10, sb, 90, espacio);
                g.DrawString("Importe", fBody10, sb, 180, espacio);
                espacio = espacio + 23;
                g.DrawString("Emitidos", fBody10, sb, 0, espacio);
                g.DrawString(bcemitidos.ToString(), fBody, sb, 110, espacio);
                g.DrawString(Utilerias.Utilerias.formatCurrency(biemitidos), fBody, sb, 170, espacio);
                espacio = espacio + 20;
                g.DrawString("Cancelados", fBody10, sb, 0, espacio);
                g.DrawString(bccancelado.ToString(), fBody, sb, 110, espacio);
                g.DrawString(Utilerias.Utilerias.formatCurrency(bicancelados), fBody, sb, 170, espacio);
                espacio = espacio + 20;
                //g.DrawString("Canc. Fuera de turno", fBody10, sb, 0, espacio);
                //g.DrawString(bccancfdt, fBody, sb, 110, espacio);
                //g.DrawString(Utilerias.Utilerias.formatCurrency(bicancfdt), fBody, sb, 170, espacio);
                //espacio = espacio + 20;
                    //g.DrawString("ReExpedidos", fBody10, sb, 0, espacio);
                    //g.DrawString(bcree, fBody, sb, 110, espacio);
                    //g.DrawString(Utilerias.Utilerias.formatCurrency(biree), fBody, sb, 170, espacio);
                    //espacio = espacio + 20;
                g.DrawString("Venta", fBody10, sb, 0, espacio);
                g.DrawString(bcventa.ToString(), fBody, sb, 110, espacio);
                g.DrawString(Utilerias.Utilerias.formatCurrency(biventa), fBody, sb, 170, espacio);
                espacio = espacio + 30;
                int es = espacio;
                g.DrawRectangle(Pens.Black, 0, espacio, 130, 260);
                g.DrawRectangle(Pens.Black, 0, espacio, 130, 20);

                espacio = espacio + 2;
               
                g.DrawString("Guias", fBody12, sb, 40, espacio);
                espacio = espacio + 23;
                g.DrawString("Emitidas", fBody7, sb, 0, espacio);
                g.DrawString( gemitidas, fBody7, sb, 60, espacio);
                espacio = espacio + 20;
                g.DrawString("Canc.", fBody7, sb, 0, espacio);
                g.DrawString(gcanceladas, fBody7, sb,60, espacio);
                espacio = espacio + 20;
                g.DrawString("Importe", fBody7, sb, 0, espacio);
                g.DrawString(Utilerias.Utilerias.formatCurrency(gimporte), fBody7, sb, 60, espacio);
                espacio = espacio + 20;
                g.DrawString("Comisión", fBody7, sb, 0, espacio);
                g.DrawString(Utilerias.Utilerias.formatCurrency(gcomisiontaq), fBody7, sb, 60, espacio);
                espacio = espacio + 20;
                g.DrawString("Com. Banco", fBody7, sb, 0, espacio);
                g.DrawString("$" + gcomisionbanco, fBody7, sb, 60, espacio);
                espacio = espacio + 20;
                g.DrawString("Aportación", fBody7, sb, 0, espacio);
                g.DrawString(Utilerias.Utilerias.formatCurrency(gaportacion), fBody7, sb, 60, espacio);
                espacio = espacio + 20;
                //g.DrawString("Diesel", fBody7, sb, 0, espacio);
                //g.DrawString("$" + gdiesel, fBody7, sb, 80, espacio);
                //espacio = espacio + 20;
                //g.DrawString("Casetas", fBody7, sb, 0, espacio);
                //g.DrawString("$" + gcaseta, fBody7, sb, 80, espacio);
                //espacio = espacio + 20;
                g.DrawString("T. Turno", fBody7, sb, 0, espacio);
                g.DrawString(Utilerias.Utilerias.formatCurrency(tturno), fBody7, sb, 60, espacio);
                espacio = espacio + 20;
                g.DrawString("T. Salida", fBody7, sb, 0, espacio);
                g.DrawString(Utilerias.Utilerias.formatCurrency(tsalida), fBody7, sb, 60, espacio);
                espacio = espacio + 20;
                g.DrawString("T. Paso", fBody7, sb, 0, espacio);
                g.DrawString(Utilerias.Utilerias.formatCurrency(tpaso), fBody7, sb, 60, espacio);
                espacio = espacio + 20;
                g.DrawString("IVA", fBody7, sb, 0, espacio);
                g.DrawString(Utilerias.Utilerias.formatCurrency(giva), fBody7, sb, 60, espacio);
                espacio = espacio + 20;
                g.DrawString("Anticipos", fBody7, sb, 0, espacio);
                g.DrawString(Utilerias.Utilerias.formatCurrency(ganticipo), fBody7, sb, 60, espacio);
                espacio = espacio + 20;
                g.DrawString("Total", fBody7, sb, 0, espacio);
                g.DrawString(Utilerias.Utilerias.formatCurrency(gtotal), fBody7, sb, 60, espacio);

                g.DrawRectangle(Pens.Black, 135, es, 135, 260);
                g.DrawRectangle(Pens.Black, 135, es, 135, 20);
                espacio = es + 2;
                g.DrawString("Caja", fBody12, sb, 180, espacio);
                espacio = espacio + 23;
                g.DrawString("Venta", fBody7, sb, 140, espacio);
                g.DrawString(Utilerias.Utilerias.formatCurrency(cventa), fBody7, sb, 192, espacio);
                espacio = espacio + 20;
                g.DrawString("Canc.", fBody7, sb, 140, espacio);
                g.DrawString(Utilerias.Utilerias.formatCurrency(ccancfueradt), fBody7, sb, 192, espacio);
                espacio = espacio + 20;
                g.DrawString("Gastos", fBody7, sb, 140, espacio);
                g.DrawString(Utilerias.Utilerias.formatCurrency(canticipos), fBody7, sb, 192, espacio);
                espacio = espacio + 20;
                g.DrawString("Tarjetas", fBody7, sb, 140, espacio);
                g.DrawString(Utilerias.Utilerias.formatCurrency(ctarjeta), fBody7, sb, 192, espacio);
                espacio = espacio + 20;
                //g.DrawString("Salidas", fBody7, sb, 140, espacio);
                //g.DrawString("$" + ccoutassalidas, fBody7, sb, 200, espacio);
                //espacio = espacio + 20;
                g.DrawString("Comisión", fBody7, sb, 140, espacio);
                g.DrawString(Utilerias.Utilerias.formatCurrency(ccomisiones), fBody7, sb, 192, espacio);
                espacio = espacio + 20;
                g.DrawString("Aportación", fBody7, sb, 140, espacio);
                g.DrawString(Utilerias.Utilerias.formatCurrency(caportaciones), fBody7, sb, 192, espacio);
                espacio = espacio + 20;
                //g.DrawString("Diesel", fBody7, sb, 140, espacio);
                //g.DrawString("$" + cdiesel, fBody7, sb, 200, espacio);
                //espacio = espacio + 20;
                //g.DrawString("Casetas", fBody7, sb, 140, espacio);
                //g.DrawString("$" + ccaseta, fBody7, sb, 200, espacio);
                //espacio = espacio + 20;
                g.DrawString("Total", fBody9, sb, 140, espacio);
                g.DrawString(Utilerias.Utilerias.formatCurrency(ctotalae), fBody9, sb, 192, espacio);
                espacio = espacio + 20;
                g.DrawString("Efectivo", fBody7, sb, 140, espacio);
                g.DrawString(Utilerias.Utilerias.formatCurrency(efectivo), fBody7, sb, 192, espacio);
                espacio = espacio + 20;
                g.DrawString("T. Crédito", fBody7, sb, 140, espacio);
                g.DrawString(Utilerias.Utilerias.formatCurrency(tcredito), fBody7, sb, 192, espacio);
                espacio = espacio + 20;
                g.DrawString("T. Debito", fBody7, sb, 140, espacio);
                g.DrawString(Utilerias.Utilerias.formatCurrency(tdebito), fBody7, sb, 192, espacio);
                espacio = espacio + 60;

                //g.DrawRectangle(Pens.Black, 0, espacio, 275, 120);
                //g.DrawRectangle(Pens.Black, 0, espacio, 275, 20);

                //espacio = espacio + 2;
                //g.DrawString(sucursal, fBody12, sb, 100, espacio);
                //espacio = espacio + 23;
                //g.DrawString(fechaf, fBody, sb, 0, espacio);
                //g.DrawString("Folio: " + folio, fBody7, sb, 180, espacio);
                //espacio = espacio + 20;
                //g.DrawString("Tarjetas", fBody10, sb, 0, espacio);
                //g.DrawString(ctarjeta, fBody, sb, 180, espacio);
                //espacio = espacio + 20;
                //g.DrawString("Aportación", fBody10, sb, 0, espacio);
                //g.DrawString(caportaciones, fBody, sb, 180, espacio);
                //espacio = espacio + 20;
                //g.DrawString("Diesel", fBody10, sb, 0, espacio);
                //g.DrawString(cdiesel, fBody, sb, 180, espacio);
                //espacio = espacio + 20;
                //g.DrawString("Casetas", fBody10, sb, 0, espacio);
                //g.DrawString(ccaseta, fBody, sb, 180, espacio);

                g.Dispose();
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "llenarticket";
                Utilerias.LOG.write(_clase, funcion, error);

            }
        }

  private void subir()
        {
            try
            {

          
                string sql = "INSERT INTO CORTEDECAJA" +
                    " (APARTIR,HASTA,BCEMITIDOS,BIEMITIDOS,BCCANCELADOS,BICANCELADOS,BCCANCFUERADT,BICANCFUERADT,BCREEXPEDIDOS," +
                    "BIREEXPEDIDOS,BCVENTA,BIVENTA,GEMITIDOS,GCANCELADAS,GIMPORT,GCOMISION,GCOMISIONBANCO,GAPORTACION,GDIESEL,GCASETAS," +
                    "GIVA,GANTICIPO,GTOTAL,CVENTA,CCANCFUERADTURNO,CANTICIPOS,CTARJETAS,CCOUTASSALIDA,CCOMISION,CAPORTACION,CDIESEL,CCASETA,CTOTALaENTREGAR," +
                    "CEFECTIVO,CTARJETACREDITO,CTARJETADEBITO,FOLIO,SUCURSAL,USUARIO,CVALES,GTTURNO,GTPASO,GTSALIDA,PKUSUARIO) " +
                    "VALUES" +
                    "(@APARTIR,@HASTA,@BCEMITIDOS,@BIEMITIDOS,@BCCANCELADOS,@BICANCELADOS,@BCCANCFUERADT,@BICANCFUERADT,@BCREEXPEDIDOS," +
                    "@BIREEXPEDIDOS,@BCVENTA,@BIVENTA,@GEMITIDOS,@GCANCELADAS,@GIMPORT,@GCOMISION,@GCOMISIONBANCO,@GAPORTACION,@GDIESEL,@GCASETAS," +
                    "@GIVA,@GANTICIPO,@GTOTAL,@CVENTA,@CCANCFUERADTURNO,@CANTICIPOS,@CTARJETAS,@CCOUTASSALIDA,@CCOMISION,@CAPORTACION,@CDIESEL,@CCASETA," +
                    "@CTOTALaENTREGAR,@CEFECTIVO,@CTARJETACREDITO,@CTARJETADEBITO,@FOLIO,@SUCURSAL,@USUARIO,@CVALES,@GTTURNO,@GTPASO,@GTSALIDA,@pkusuario)";
                    db.PreparedSQL(sql);

                db.command.Parameters.AddWithValue("@APARTIR", fechai);
                    db.command.Parameters.AddWithValue("@HASTA", fechaf);
                    db.command.Parameters.AddWithValue("@BCEMITIDOS", bcemitidos);
                    db.command.Parameters.AddWithValue("@BIEMITIDOS", biemitidos);
                    db.command.Parameters.AddWithValue("@BCCANCELADOS", bccancelado);
                db.command.Parameters.AddWithValue("@BICANCELADOS", bicancelados);
                db.command.Parameters.AddWithValue("@BCCANCFUERADT", bccancfdt);
                    db.command.Parameters.AddWithValue("@BICANCFUERADT", bicancfdt);
                    db.command.Parameters.AddWithValue("@BCREEXPEDIDOS", bcree);
                    db.command.Parameters.AddWithValue("@BIREEXPEDIDOS", biree);
                    db.command.Parameters.AddWithValue("@BCVENTA", bcventa);
                    db.command.Parameters.AddWithValue("@BIVENTA", biventa);
                    db.command.Parameters.AddWithValue("@GEMITIDOS", gemitidas);
                    db.command.Parameters.AddWithValue("@GCANCELADAS", gcanceladas);
                    db.command.Parameters.AddWithValue("@GIMPORT", gimporte);
                    db.command.Parameters.AddWithValue("@GCOMISION", gcomisiontaq);
                    db.command.Parameters.AddWithValue("@GCOMISIONBANCO", gcomisionbanco);
                db.command.Parameters.AddWithValue("@GAPORTACION", gaportacion);
                db.command.Parameters.AddWithValue("@GDIESEL", gdiesel);
                db.command.Parameters.AddWithValue("@GCASETAS", gcanceladas);
                db.command.Parameters.AddWithValue("@GTPASO", tpaso);
                db.command.Parameters.AddWithValue("@GTSALIDA", tsalida);
                db.command.Parameters.AddWithValue("@GTTURNO", tturno);
                db.command.Parameters.AddWithValue("@GIVA", giva);
                db.command.Parameters.AddWithValue("@GANTICIPO", ganticipo);
                db.command.Parameters.AddWithValue("@GTOTAL", gtotal);
                db.command.Parameters.AddWithValue("@CVENTA", cventa);
                db.command.Parameters.AddWithValue("@CCANCFUERADTURNO",ccancfueradt);
                db.command.Parameters.AddWithValue("@CANTICIPOS", canticipos);
                db.command.Parameters.AddWithValue("@CTARJETAS", ctarjeta);
                db.command.Parameters.AddWithValue("@CCOUTASSALIDA", ccoutassalidas);
                db.command.Parameters.AddWithValue("@CCOMISION", ccomisiones);
                db.command.Parameters.AddWithValue("@CAPORTACION", caportaciones);
                db.command.Parameters.AddWithValue("@CDIESEL", cdiesel);
                db.command.Parameters.AddWithValue("@CCASETA", ccaseta);
                db.command.Parameters.AddWithValue("@CTOTALAENTREGAR", ctotalae);
                db.command.Parameters.AddWithValue("@CEFECTIVO", efectivo);
                db.command.Parameters.AddWithValue("@CTARJETACREDITO", tcredito);
                db.command.Parameters.AddWithValue("@CTARJETADEBITO", tdebito);
                db.command.Parameters.AddWithValue("@FOLIO", folio);
                db.command.Parameters.AddWithValue("@pkusuario", LoginInfo.PkUsuario);

                db.command.Parameters.AddWithValue("@SUCURSAL", sucursal);
                db.command.Parameters.AddWithValue("@USUARIO", usuarioconsulta);
                db.command.Parameters.AddWithValue("@CVALES", 0);


                if (db.execute())
                    {
                    Form mensaje = new Mensaje("Corte Realizado", true);

                    mensaje.ShowDialog();
                }
              

                

            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "subir";
                Utilerias.LOG.write(_clase, funcion, error);

            }

        }

        private void updateguiayvendidos(object sender, EventArgs e)
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

                    bool huella = verificationUserControl1.verificando();
                   if (huella == true)
                
                    {
                        corte();

                    }
                    else
                    {
                        Form mensaje = new Mensaje("Verifique la huella", true);

                        mensaje.ShowDialog();
                    }
                }
            }

            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un con la impresora seleccionada, intente de nuevo.");
                    string funcion = "updateguiayvendidos";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }

        private void Label4_Click(object sender, EventArgs e)
        {

        }
        private void usuarios()
        {

        }
        public void getDatosAdicionalesusu()
        {
            try
            {
                comboBoxven.Items.Clear();

                string sql = "SELECT NOMBRE,APELLIDOS,USUARIO FROM USUARIOS WHERE BORRADO=0";
                db.PreparedSQL(sql);
                res = db.getTable();

                while (res.Next())
                {
                    ComboboxItem item = new ComboboxItem();
                    item.Text = res.Get("NOMBRE") + " " + res.Get("APELLIDOS");
                    item.Value = res.Get("NOMBRE") + " " + res.Get("APELLIDOS");

                    comboBoxven.Items.Add(item);

                }
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "getDatosAdicionalesusu";
                Utilerias.LOG.write(_clase, funcion, error);


            }

        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();

            permisos();
            if (cualquiercorte)
            {
                getDatosAdicionalesusu();
                 comboBoxven.DropDownStyle = ComboBoxStyle.DropDownList;
                groupBoxcombo.Visible = true;
            }
            else
            {
                usuarioconsulta = LoginInfo.NombreID+" "+LoginInfo.ApellidoID;
                TARJETACREDITO();
                pagoefectivo();
                TARJETADEBITO();
                boletosinfocancelado();
                boletosinfo();
                ValidateUser();
                getRows();
                fecha();
                caja();
                boletosinfocancelado();
                supercontra();
            }

            //Estas lineas eliminan los parpadeos del formulario o controles en la interfaz grafica (Pero no en un 100%)
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.DoubleBuffered = true;
            textBoxusuario.Text = usuarioconsulta;

            labelnombre.Text = LoginInfo.NombreID;
            labelapellido.Text = LoginInfo.ApellidoID;
            MemoryStream ms = new MemoryStream((byte[])LoginInfo.imagenfoto);
            Bitmap bmp = new Bitmap(ms);
            pictureBoxfoto.Image = bmp;
            labelcargo.Text = LoginInfo.rol;
            timer2.Start();

         
        }

        private void Timer2_Tick(object sender, EventArgs e)
        {
            lblHora.Text = DateTime.Now.ToString("hh:mm:ss");
            lbFecha.Text = DateTime.Now.ToString("D");
            fechaf = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            textBoxfin.Text = fechaf;
        }

        private void Button3_Click(object sender, EventArgs e)
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
        private void Button2_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void label26_Click(object sender, EventArgs e)
        {

        }

        private void label39_Click(object sender, EventArgs e)
        {

        }

        private void textBoxtotalaentr_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxvales_TextChanged(object sender, EventArgs e)
        {

        }

        private void Corte_de_Caja_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;

        }

        private void limpiarhuella(object sender, EventArgs e)
        {
            verificationUserControl1.Stop();
            verificationUserControl1.limpiarhuella();
        }

        private void textBoxini_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxven_SelectedIndexChanged(object sender, EventArgs e)
        {
            usuarioconsulta = (comboBoxven.SelectedItem as ComboboxItem).Value.ToString();
            textBoxusuario.Text = (comboBoxven.SelectedItem as ComboboxItem).Text;

            TARJETACREDITO();
            pagoefectivo();
            TARJETADEBITO();
            boletosinfocancelado();
            boletosinfo();
            ValidateUser();
            getRows();
            fecha();
            caja();
            boletosinfocancelado();

        }

        private void Corte_de_Caja_VisibleChanged(object sender, EventArgs e)
        {
            
        }

        private void Corte_de_Caja_MouseLeave(object sender, EventArgs e)
        {
            verificationUserControl1.limpiarhuella();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            verificationUserControl1.Start();
            verificationUserControl1.limpiarhuella();
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            timer3.Stop();
            checkInternetAvaible();
            if (CheckOpened("Ventas"))
            {
                Form mensaje = new Mensaje("Taquilla abierta, cierre la vista para hacer el corte de caja", true);
                mensaje.ShowDialog();
                this.Close();
            }
        }

        private void verificationUserControl1_VerificationStatusChanged(object sender, StatusChangedEventArgs e)
        {
       
        }

        private void buttoncontraseña_Click(object sender, EventArgs e)
        {

        }
        private void corte()
        {
            folio = GenerateRandom();

            Utilerias.LOG.acciones("genero corte de caja " + folio);

            string sql = "UPDATE VENDIDOS SET CORTE=@CORTE WHERE CORTE=0 AND VENDEDOR=@VALIDADOR AND NOT STATUS='PREVENTA' AND NOT STATUS='BLOQUEADO' ";


            db.PreparedSQL(sql);
            db.command.Parameters.AddWithValue("@CORTE", 1);

            db.command.Parameters.AddWithValue("@VALIDADOR", usuarioconsulta);


            db.execute();
            string sql3 = "UPDATE VENDIDOS SET CORTECANCELADO=@CORTECANCELADO WHERE CORTECANCELADO=0 AND CANCELADO=@VALIDADOR";


            db.PreparedSQL(sql3);
            db.command.Parameters.AddWithValue("@CORTECANCELADO", 1);

            db.command.Parameters.AddWithValue("@VALIDADOR", usuarioconsulta);


            db.execute();

            string sql2 = "UPDATE GUIA SET CORTE=@CORTE WHERE CORTE=0 AND VALIDADOR=@VALIDADOR";


            db.PreparedSQL(sql2);
            db.command.Parameters.AddWithValue("@CORTE", 1);
            db.command.Parameters.AddWithValue("@VALIDADOR", usuarioconsulta);


            db.execute();


            tamaño = 1000;

            PrintDocument pd = new PrintDocument();
            pd.PrintPage += new PrintPageEventHandler(llenarticket);
            PaperSize ps = new PaperSize("", 420, tamaño);

            pd.PrintController = new StandardPrintController();

            pd.DefaultPageSettings.Margins.Left = 0;
            pd.DefaultPageSettings.Margins.Right = 0;
            pd.DefaultPageSettings.Margins.Top = 0;
            pd.DefaultPageSettings.Margins.Bottom = 0;
            pd.DefaultPageSettings.PaperSize = ps;
            pd.PrinterSettings.PrinterName = Settings1.Default.impresora;
            subir();

            pd.Print();
            CrearTicket ticket0 = new CrearTicket();
            ticket0.TextoIzquierda("");
            ticket0.TextoIzquierda("");
            ticket0.TextoIzquierda("");
            ticket0.TextoIzquierda("");
            ticket0.CortaTicket();
            ticket0.ImprimirTicket(Settings1.Default.impresora);
            this.Close();
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            if (contraseña == textBoxcontraseña.Text)
            {
                contraseñaerror.Visible = false;
                corte();
               
                this.Close();
            }
            else
            {
                contraseñaerror.Visible = true;
            }


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

        private void verificationUserControl1_TabIndexChanged(object sender, EventArgs e)
        {

        }

        private void verificationUserControl1_BackColorChanged(object sender, EventArgs e)
        {
            contando += 1;
            if (contando >= 5)
            {
                panelcontraseña.Visible = true;
                textBoxcontraseña.Focus();
            }
        }

        private void groupBox6_Enter(object sender, EventArgs e)
        {

        }
    }
}
