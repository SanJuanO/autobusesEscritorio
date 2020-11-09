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
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Autobuses
{
    public partial class CorteGuia : Form
    {
        public database db;
        ResultSet res = null;


        int scantidadguia = 0;
        double simporte = 0.0;
        double sgastos = 0.0;
        double starjetas = 0.0;
        double scompbancos = 0.0;
        double saportaciones = 0.0;
        double siva = 0.0;
        double stotal = 0.0;
        string _clase = "corte de Guias";
        string fechainicio = "";
        string fechaf = "";
        string pkguia = "";
        private string fechai = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        private int tamaño;
        private int espacio;
        private string folio;
        private Bitmap imagen;
        string[] folios;
        private int pagosrealizados;

        string[] cantidadguia;
        double[] importes;
        double[] gastos;
        double[] tarjetas;
        double[] compbancos;
        double[] aportaciones;
        double[] iva;
        double[] total;

        public CorteGuia()
        {
            CultureInfo.GetCultureInfo("es-MX");

            db = new database();
            InitializeComponent();
            permisos();
            infoguias();
            string usuarioconsulta = LoginInfo.NombreID + " " + LoginInfo.ApellidoID;

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
            fecha();
            timer2.Start();

        }

        private void fecha()
        {
            try

            {

                string sql = "SELECT TOP 1 FECHA_C FROM guiaspagadas WHERE CORTE='0' AND  PK_USUARIO=@PKUSUARIO ORDER BY FECHA_C asc";
                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@PKUSUARIO", LoginInfo.PkUsuario);

                res = db.getTable();

                if (res.Next())
                {
                    fechai = res.Get("FECHA_C");
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
        public void infoguias()
        {
            try

            {

                string sql = "select * from GUIASPAGADAS WHERE PK_USUARIO=@VENDEDOR AND CORTE=0";
                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@VENDEDOR", LoginInfo.PkUsuario);
                int n = 0;
                res = db.getTable();
                var i = 0;
                folios = new string[res.Count];

                cantidadguia = new string[res.Count];
                 importes = new double[res.Count];
                 gastos = new double[res.Count];
                 tarjetas = new double[res.Count];
                 compbancos = new double[res.Count];
                 aportaciones = new double[res.Count];
                 iva = new double[res.Count];
                 total = new double[res.Count];
                while (res.Next())
                {
                    folios[i] = res.Get("FOLIO");

                    cantidadguia[i] = res.Get("CANTIDADEGUIAS");
                    importes[i] = (double.TryParse(res.Get("IMPORTE"), out double aux1)) ? res.GetDouble("IMPORTE") : 0.0;
                    gastos[i] = (double.TryParse(res.Get("GASTOS"), out double aux2)) ? res.GetDouble("GASTOS") : 0.0;
                    compbancos[i] = (double.TryParse(res.Get("COMPBAN"), out double aux3)) ? res.GetDouble("COMPBAN") : 0.0;
                    iva[i] = (double.TryParse(res.Get("IVA"), out double aux4)) ? res.GetDouble("IVA") : 0.0;
                    total[i] = (double.TryParse(res.Get("TOTAL"), out double aux5)) ? res.GetDouble("TOTAL") : 0.0;
                    tarjetas[i] = (double.TryParse(res.Get("TARJETAS"), out double aux6)) ? res.GetDouble("TARJETAS") : 0.0;
                    aportaciones[i] = (double.TryParse(res.Get("APORTACIONES"), out double aux7)) ? res.GetDouble("APORTACIONES") : 0.0;


                    i++;
                }
                pagosrealizados = res.Count;


                for (int u = 0; u < res.Count; u++)
                {
                    scantidadguia = int.Parse(cantidadguia[u]);
                    simporte = simporte + importes[u];
                    sgastos = sgastos + gastos[u];
                    saportaciones = saportaciones + aportaciones[u];
                    scompbancos = scompbancos + compbancos[u];
                    starjetas = starjetas + tarjetas[u];
                    siva = siva + iva[u];
                    stotal = stotal + total[u];




                }

                textguias.Text = pagosrealizados.ToString();
                textimporte.Text = Utilerias.Utilerias.formatCurrency( simporte);
                textgastos.Text = Utilerias.Utilerias.formatCurrency(sgastos);
                textaportaciones.Text = Utilerias.Utilerias.formatCurrency (saportaciones);
                textbanco.Text = Utilerias.Utilerias.formatCurrency(scompbancos);
                texttarjetas.Text = Utilerias.Utilerias.formatCurrency(starjetas);
                textiva.Text = Utilerias.Utilerias.formatCurrency( siva);
                texttotal.Text = Utilerias.Utilerias.formatCurrency( stotal);
                getRows();
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "boletosinfocancelado";
                Utilerias.LOG.write(_clase, funcion, error);


            }

        }


        private void Timer2_Tick(object sender, EventArgs e)
        {
            lblHora.Text = DateTime.Now.ToString("hh:mm:ss");
            lbFecha.Text = DateTime.Now.ToString("D");
            
            textBoxfin.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        }




        private void permisos()
        {

            if (LoginInfo.privilegios.Any(x => x == "Generar cualquier corte"))
            {


            }
        }



        public void getRows()
        {
            try
            {

                int count = 1;
                string sql = "select * from GUIASPAGADAS WHERE PK_USUARIO=@PK_USUARIO AND CORTE=0";
                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@PK_USUARIO", LoginInfo.PkUsuario);


                res = db.getTable();
                int n;


                while (res.Next())
                {
                    n = dataGridViewguias.Rows.Add();

                    double importe = (double.TryParse(res.Get("IMPORTE"), out double aux1)) ? res.GetDouble("IMPORTE") : 0.0;
                    dataGridViewguias.Rows[n].Cells["importenamed"].Value = importe;
                    dataGridViewguias.Rows[n].Cells["importename"].Value = Utilerias.Utilerias.formatCurrency(importe);

                    dataGridViewguias.Rows[n].Cells["folioname"].Value = res.Get("FOLIO");
                    dataGridViewguias.Rows[n].Cells["socioname"].Value = res.Get("SOCIO");
                    dataGridViewguias.Rows[n].Cells["cantidaddeguiasname"].Value = res.Get("CANTIDADEGUIAS");

                    double gastos = (double.TryParse(res.Get("GASTOS"), out double aux2)) ? res.GetDouble("GASTOS") : 0.0;
                    dataGridViewguias.Rows[n].Cells["gastosnamed"].Value = gastos;
                    dataGridViewguias.Rows[n].Cells["gastosname"].Value = Utilerias.Utilerias.formatCurrency(gastos);

                    double tarjetas = (double.TryParse(res.Get("TARJETAS"), out double aux3)) ? res.GetDouble("TARJETAS") : 0.0;
                    dataGridViewguias.Rows[n].Cells["tarjetasnamed"].Value = tarjetas;
                    dataGridViewguias.Rows[n].Cells["tarjetaname"].Value = Utilerias.Utilerias.formatCurrency(tarjetas);

                    double iva = (double.TryParse(res.Get("IVA"), out double aux4)) ? res.GetDouble("IVA") : 0.0;
                    dataGridViewguias.Rows[n].Cells["ivanamed"].Value = iva;
                    dataGridViewguias.Rows[n].Cells["ivaname"].Value = Utilerias.Utilerias.formatCurrency(iva);

                    double total = (double.TryParse(res.Get("TOTAL"), out double aux5)) ? res.GetDouble("TOTAL") : 0.0;
                    dataGridViewguias.Rows[n].Cells["totalnamed"].Value = total;
                    dataGridViewguias.Rows[n].Cells["totalname"].Value = Utilerias.Utilerias.formatCurrency(total);
                    double aportacion = (double.TryParse(res.Get("APORTACIONES"), out double aux6)) ? res.GetDouble("APORTACIONES") : 0.0;
                    dataGridViewguias.Rows[n].Cells["aportacionnamed"].Value = aportacion;
                    dataGridViewguias.Rows[n].Cells["aportacionesname"].Value = Utilerias.Utilerias.formatCurrency(aportacion);

                    double compban = (double.TryParse(res.Get("COMPBAN"), out double aux8)) ? res.GetDouble("COMPBAN") : 0.0;
                    dataGridViewguias.Rows[n].Cells["combannamed"].Value = compban;
                    dataGridViewguias.Rows[n].Cells["compbanname"].Value = Utilerias.Utilerias.formatCurrency(compban);

                    dataGridViewguias.Rows[n].Cells["sucursalname"].Value = res.Get("SUCURSAL");
                    dataGridViewguias.Rows[n].Cells["pkname"].Value = res.Get("PK");
                    dataGridViewguias.Rows[n].Cells["pkname"].Value = res.Get("PK");
                    dataGridViewguias.Rows[n].Cells["lineaname"].Value = res.Get("LINEA");
                    dataGridViewguias.Rows[n].Cells["cobradoname"].Value = res.Get("COBRADOR");
                    dataGridViewguias.Rows[n].Cells["usuarioname"].Value = res.Get("USUARIO");





                    count++;
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

        private void reimprimir_Click(object sender, EventArgs e)
        {
            if (importes.Length > 0)
            {
                corte();
            }
            else
            {
                Form mensaje = new Mensaje("No puedes realizar el corte", true);

                mensaje.ShowDialog();

            }
        }

        private void corte()
        {
            folio = GenerateRandom();

            Utilerias.LOG.acciones("genero corte de pago de guias " + folio);



      


            tamaño = 1000;
             subir();
            if (!string.IsNullOrEmpty(pkguia))
            {

                string sql2 = "UPDATE GUIASPAGADAS SET CORTE=@CORTE,CORTEPK=@CORTEPK WHERE CORTE=0 AND PK_USUARIO=@PK_USUARIO";


                db.PreparedSQL(sql2);
                db.command.Parameters.AddWithValue("@CORTE", 1);
                db.command.Parameters.AddWithValue("@PK_USUARIO", LoginInfo.PkUsuario);
                db.command.Parameters.AddWithValue("@CORTEPK", pkguia);


                db.execute();


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

                pd.Print();
                CrearTicket ticket0 = new CrearTicket();
                ticket0.TextoIzquierda("");
                ticket0.TextoIzquierda("");
                ticket0.TextoIzquierda("");
                ticket0.TextoIzquierda("");
                ticket0.CortaTicket();
                ticket0.ImprimirTicket(Settings1.Default.impresora);
        
                    Form mensaje = new Mensaje("Corte Realizado", true);

                    mensaje.ShowDialog();
                
                this.Close();
            }
            else
            {
                Form mensaje = new Mensaje("No se pudo relizar el corte", true);

                mensaje.ShowDialog();
            }

        }
        private void subir()
        {
            try
            {
                fechaf = textBoxfin.Text;
                fechai = textBoxini.Text;

                string sql = "INSERT INTO CORTEGUIA" +
                    " (APARTIR,HASTA,EMITIDOS,IMPORT,GASTOS,TARJETAS,COMPROBANTEBANCOS,APORTACION,IVA,TOTAL,FOLIO,USUARIO,PKUSUARIO) " +
                    "VALUES" +
                    "(@APARTIR,@HASTA,@EMITIDOS,@IMPOR,@GASTOS,@TARJETAS,@COMPROBANTEBANCOS,@APORTACION,@IVA,@TOTAL,@FOLIO,@USUARIO,@PKUSUARIO) ";
                db.PreparedSQL(sql);

                db.command.Parameters.AddWithValue("@APARTIR", fechai);
                db.command.Parameters.AddWithValue("@HASTA", fechaf);

                db.command.Parameters.AddWithValue("@EMITIDOS", pagosrealizados);
                db.command.Parameters.AddWithValue("@IMPOR", simporte);
                db.command.Parameters.AddWithValue("@GASTOS", sgastos);
                db.command.Parameters.AddWithValue("@TARJETAS", starjetas);
                db.command.Parameters.AddWithValue("@COMPROBANTEBANCOS", scompbancos);
                db.command.Parameters.AddWithValue("@APORTACION", saportaciones);
                db.command.Parameters.AddWithValue("@IVA", siva);
                db.command.Parameters.AddWithValue("@TOTAL", stotal);   
                db.command.Parameters.AddWithValue("@FOLIO", folio);

                db.command.Parameters.AddWithValue("@USUARIO", LoginInfo.NombreID+" " +LoginInfo.ApellidoID);
                db.command.Parameters.AddWithValue("@PKUSUARIO", LoginInfo.PkUsuario);

                pkguia = db.executeId();
               




            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "subir";
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
        private void BtnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }




        void llenarticket(object sender, PrintPageEventArgs e)
        {
            try
            {
                codigoqr(folio);

                Graphics g = e.Graphics;
                // g.DrawRectangle(Pens.Black, 3, 5, 340, 700);
                // g.DrawImage(imagensplash, 10, 0);
                System.Drawing.Font fBody7 = new System.Drawing.Font("Lucida", 7, FontStyle.Bold);
                System.Drawing.Font fBody5 = new System.Drawing.Font("Lucida", 5, FontStyle.Bold);

                System.Drawing.Font fBody9 = new System.Drawing.Font("Lucida", 9, FontStyle.Bold);
                System.Drawing.Font fBody = new System.Drawing.Font("Lucida", 8, FontStyle.Bold);
                System.Drawing.Font fBody10 = new System.Drawing.Font("Lucida", 8, FontStyle.Bold);
                System.Drawing.Font fBody12 = new System.Drawing.Font("Lucida", 12, FontStyle.Bold);
                System.Drawing.Font fBody18 = new System.Drawing.Font("Lucida", 18, FontStyle.Bold);

                Color customColor = Color.FromArgb(255, Color.Black);
                SolidBrush sb = new SolidBrush(customColor);
                espacio = 0;
                g.DrawString("Fecha :", fBody, sb, 2, espacio);
                g.DrawString(DateTime.Now.ToShortDateString(), fBody, sb, 48, espacio);
                g.DrawString("Hora :", fBody, sb, 170, espacio);
                g.DrawString(DateTime.Now.ToShortTimeString(), fBody, sb, 210, espacio);
                espacio = espacio + 15;
                g.DrawString("Corte de pago de guias", fBody18, sb, 20, espacio);
                espacio = espacio + 30;
                g.DrawString("SUCURSAL: " + LoginInfo.Sucursal, fBody10, sb, 0, espacio);
                espacio = espacio + 20;

               

                g.DrawString("Folio: " + folio, fBody10, sb, 2, espacio);
                g.DrawString("Cantidad de Pagos: " + pagosrealizados.ToString(), fBody10, sb, 150, espacio);


                espacio = espacio + 20;
                g.DrawString("Usuario: " + LoginInfo.NombreID+" "+LoginInfo.ApellidoID, fBody10, sb, 0, espacio);
                espacio = espacio + 20;

                g.DrawString("Rango de fechas: ", fBody10, sb, 80, espacio);
                espacio = espacio + 20;
                g.DrawString( textBoxini.Text, fBody, sb, 0, espacio);
                g.DrawString(textBoxfin.Text, fBody, sb, 140, espacio);
                espacio = espacio + 20;


                g.DrawString("-------------------------------------------------------------------------------------", fBody7, sb, 0, espacio);
                espacio = espacio + 10;

                g.DrawString("FOLIO", fBody5, sb, 0, espacio);
                g.DrawString("IMPORTE", fBody5, sb, 45, espacio);
                g.DrawString("GASTOS", fBody5, sb, 85, espacio);
                g.DrawString("TARJETAS", fBody5, sb, 130, espacio);
                g.DrawString("IVA", fBody5, sb, 185, espacio);
                g.DrawString("TOTAL", fBody5, sb, 225, espacio);
                espacio = espacio + 10;

                g.DrawString("-------------------------------------------------------------------------------------", fBody7, sb, 0, espacio);
                espacio = espacio + 10;

                for (int i = 0; i < importes.Count(); i++)
                {


                    g.DrawString(folios[i], fBody5, sb, 0, espacio);
                    g.DrawString(Utilerias.Utilerias.formatCurrency(importes[i]), fBody5, sb, 45, espacio);
                    g.DrawString(Utilerias.Utilerias.formatCurrency(gastos[i]), fBody5, sb, 85, espacio);
                    g.DrawString(Utilerias.Utilerias.formatCurrency(tarjetas[i]), fBody5, sb, 130, espacio);
                    g.DrawString(Utilerias.Utilerias.formatCurrency(iva[i]), fBody5, sb, 185, espacio);
                    g.DrawString(Utilerias.Utilerias.formatCurrency(total[i]), fBody5, sb, 225, espacio);

                    espacio = espacio + 20;
                }

                espacio = espacio + 10;
                g.DrawString("--------------------------------------------------------------------------------------", fBody7, sb, 0, espacio - 20);
                espacio = espacio + 10;
                g.DrawImage(imagen, 20, espacio);

                g.DrawString("Importe: " + Utilerias.Utilerias.formatCurrency(simporte), fBody7, sb, 150, espacio);
                espacio = espacio + 20;
                g.DrawString("Gastos: " + Utilerias.Utilerias.formatCurrency(sgastos), fBody7, sb, 150, espacio);
                espacio = espacio + 20;
                g.DrawString("Tarjetas: " + Utilerias.Utilerias.formatCurrency(starjetas), fBody7, sb, 150, espacio);
                espacio = espacio + 20;
                g.DrawString("Aportaciónes: " + Utilerias.Utilerias.formatCurrency(saportaciones), fBody7, sb, 150, espacio);
                espacio = espacio + 20;
                g.DrawString("Comp. Banco: " + Utilerias.Utilerias.formatCurrency(scompbancos), fBody7, sb, 150, espacio);
                espacio = espacio + 20;
                g.DrawString("IVA: " + Utilerias.Utilerias.formatCurrency(siva), fBody7, sb, 150, espacio);
                espacio = espacio + 20;
                g.DrawString("Total: " + Utilerias.Utilerias.formatCurrency(stotal), fBody7, sb, 150, espacio);

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
                //imagen.Save("imagen.png", ImageFormat.Png);

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

    }
}
