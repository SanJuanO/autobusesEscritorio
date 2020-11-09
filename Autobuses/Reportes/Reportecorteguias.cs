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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Autobuses.Reportes
{
    public partial class Reportecorteguias : Form
    {
        private Bitmap imagen;
        private int tamaño;
        private int espacio;
        private string folio;
        public database db;
        ResultSet res = null;
        string fechaini = "";
        string fechafin = "";
        string usuariobusqueda = "";
        string _clase = "reportecorteguia";
        string PKSELECCIONADO = "";
        string[] gfolios;
        private int pagosrealizados;

        string[] gcantidadguia;
        double[] gimportes;
        double[] ggastos;
        double[] gtarjetas;
        double[] gcompbancos;
        double[] gaportaciones;
        double[] giva;
        double[] gtotal;
        int scantidadguia = 0;
        double simporte = 0.0;
        double sgastos = 0.0;
        double starjetas = 0.0;
        double scompbancos = 0.0;
        double saportaciones = 0.0;
        double siva = 0.0;
        double stotal = 0.0;
        public Reportecorteguias()
        {
            InitializeComponent();
            db = new database();
            comboBoxven.DropDownStyle = ComboBoxStyle.DropDownList;
            timer1.Start();
            //Estas lineas eliminan los parpadeos del formulario o controles en la interfaz grafica (Pero no en un 100%)
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.DoubleBuffered = true;
            titulo.Text = "Reporte de Corte de Caja";
            labelnombre.Text = LoginInfo.NombreID;
            labelapellido.Text = LoginInfo.ApellidoID;
            MemoryStream ms = new MemoryStream((byte[])LoginInfo.imagenfoto);
            Bitmap bmp = new Bitmap(ms);
            pictureBoxfoto.Image = bmp;
            labelcargo.Text = LoginInfo.rol;
            fechaini = dateTimePicker1.Value.ToString("dd/MM/yyyy");
            fechafin = dateTimePicker1.Value.ToString("dd/MM/yyyy");
            getDatosAdicionalesusu();
          
        }

        public void getDatosAdicionalesusu()
        {
            try
            {
                comboBoxven.Items.Clear();

                string sql = "SELECT NOMBRE,APELLIDOS,PK FROM USUARIOS WHERE BORRADO=0";
                db.PreparedSQL(sql);
                res = db.getTable();

                while (res.Next())
                {
                    ComboboxItem item = new ComboboxItem();
                    item.Text = res.Get("NOMBRE") + " " + res.Get("APELLIDOS");
                    item.Value = res.Get("PK");

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
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                fechaini = dateTimePicker1.Value.ToString("dd/MM/yyyy");
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "datetimepicker1";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                fechafin = dateTimePicker2.Value.ToString("dd/MM/yyyy");
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "datetimepicker1";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {

                //if (comboBoxven.SelectedItem == null)
                //{
                //    Form mensaje = new Mensaje("Seleccione un vendedor", true);

                //    mensaje.ShowDialog();
                //}
                //else
                //{
                 //   dataGridViewguias.Rows.Clear();


                //if ((comboBoxven.SelectedItem) != null)
                //{
                dataGridViewcorte.Rows.Clear();
                limpiardatos();
                     //   usuariobusqueda = comboBoxven.SelectedItem.ToString();
                        infoguias();
                   // }


                //}
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "buttunbuscar";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }




        public void getRows()
        {
            try
            {

                int count = 1;
                string sql = "select * from GUIASPAGADAS WHERE CORTEPK=@PK";
                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@PK", PKSELECCIONADO);


                res = db.getTable();
                int n;

                var i = 0;
             


         

                while (res.Next())
                {
                    gfolios = new string[res.Count];

                    gcantidadguia = new string[res.Count];
                    gimportes = new double[res.Count];
                    ggastos = new double[res.Count];
                    gtarjetas = new double[res.Count];
                    gcompbancos = new double[res.Count];
                    gaportaciones = new double[res.Count];
                    giva = new double[res.Count];
                    gtotal = new double[res.Count];
                    n = dataGridViewguias.Rows.Add();

                    gfolios[i] = res.Get("FOLIO");

                    gcantidadguia[i] = res.Get("CANTIDADEGUIAS");
                    gimportes[i] = (double.TryParse(res.Get("IMPORTE"), out double aux11)) ? res.GetDouble("IMPORTE") : 0.0;
                    ggastos[i] = (double.TryParse(res.Get("GASTOS"), out double aux22)) ? res.GetDouble("GASTOS") : 0.0;
                    gcompbancos[i] = (double.TryParse(res.Get("COMPBAN"), out double aux33)) ? res.GetDouble("COMPBAN") : 0.0;
                    giva[i] = (double.TryParse(res.Get("IVA"), out double aux44)) ? res.GetDouble("IVA") : 0.0;
                    gtotal[i] = (double.TryParse(res.Get("TOTAL"), out double aux55)) ? res.GetDouble("TOTAL") : 0.0;
                    gtarjetas[i] = (double.TryParse(res.Get("TARJETAS"), out double aux66)) ? res.GetDouble("TARJETAS") : 0.0;
                    gaportaciones[i] = (double.TryParse(res.Get("APORTACIONES"), out double aux77)) ? res.GetDouble("APORTACIONES") : 0.0;


                    i++;

              

                    double importe = (double.TryParse(res.Get("IMPORTE"), out double aux1)) ? res.GetDouble("IMPORTE") : 0.0;
                    dataGridViewguias.Rows[n].Cells["importenamed"].Value = importe;
                    dataGridViewguias.Rows[n].Cells["importename"].Value = Utilerias.Utilerias.formatCurrency(importe);

                    dataGridViewguias.Rows[n].Cells["dataGridViewTextBoxColumn1"].Value = res.Get("FOLIO");
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

                    dataGridViewguias.Rows[n].Cells["dataGridViewTextBoxColumn2"].Value = res.Get("SUCURSAL");
                    dataGridViewguias.Rows[n].Cells["pkname"].Value = res.Get("PK");
                    dataGridViewguias.Rows[n].Cells["lineaname"].Value = res.Get("LINEA");
                    dataGridViewguias.Rows[n].Cells["cobradoname"].Value = res.Get("COBRADOR");
                    dataGridViewguias.Rows[n].Cells["dataGridViewTextBoxColumn3"].Value = res.Get("USUARIO");





                    count++;
                }

                for (int u = 0; u < res.Count; u++)
                {
                    scantidadguia = int.Parse(gcantidadguia[u]);
                    simporte = simporte + gimportes[u];
                    sgastos = sgastos + ggastos[u];
                    saportaciones = saportaciones + gaportaciones[u];
                    scompbancos = scompbancos + gcompbancos[u];
                    starjetas = starjetas + gtarjetas[u];
                    siva = siva + giva[u];
                    stotal = stotal + gtotal[u];




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
        public void infoguias()
        {
            try

            {

                string sql = "select * from CORTEGUIA WHERE fechac>= convert(date,@fechaini,104) and FECHAC<= convert(date,@fechafin,104)";
                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@fechafin", fechafin);
                db.command.Parameters.AddWithValue("@fechaini", fechaini);

                int n = 0;
                res = db.getTable();

                if (res.Next())
                {
                    n = dataGridViewcorte.Rows.Add();

                    dataGridViewcorte.Rows[n].Cells["FOLIONAME"].Value = res.Get("FOLIO");
                    dataGridViewcorte.Rows[n].Cells["SUCURSALNAME"].Value = res.Get("EMITIDOS");
                    dataGridViewcorte.Rows[n].Cells["USUARIONAME"].Value = res.Get("USUARIO");
                    dataGridViewcorte.Rows[n].Cells["FECHANAME"].Value = res.Get("FECHAC");
                    dataGridViewcorte.Rows[n].Cells["pk"].Value = res.Get("PK");
                    n++;
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
        private void dataGridViewcorte_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

              var  n = e.RowIndex;
                if (n == -1)
                {

                }
                else
                {
                    dataGridViewguias.Rows.Clear();
                    string sql = "select * from CORTEGUIA WHERE PK=@PK";
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@PK", dataGridViewcorte.Rows[n].Cells["pk"].Value);
                    res = db.getTable();
                    var i = 0;
                   
                    if (res.Next())
                    {
                        textBoxini.Text= res.Get("APARTIR");
                        textBoxfin.Text = res.Get("HASTA");
                        textBoxusuario.Text = res.Get("USUARIO");

                        textBoxfolio.Text = res.Get("FOLIO");
                        PKSELECCIONADO = res.Get("PK");
                        textguias.Text = res.Get("EMITIDOS");
                        textimporte.Text = Utilerias.Utilerias.formatCurrency(double.TryParse(res.Get("IMPORT"), out double aux1) ? res.GetDouble("IMPORT") : 0.0);
                        textgastos.Text = Utilerias.Utilerias.formatCurrency(double.TryParse(res.Get("GASTOS"), out double aux2) ? res.GetDouble("GASTOS") : 0.0);
                        textbanco.Text = Utilerias.Utilerias.formatCurrency((double.TryParse(res.Get("COMPROBANTEBANCOS"), out double aux3)) ? res.GetDouble("COMPROBANTEBANCOS") : 0.0);
                        textiva.Text = Utilerias.Utilerias.formatCurrency((double.TryParse(res.Get("IVA"), out double aux4)) ? res.GetDouble("IVA") : 0.0);
                        texttotal.Text = Utilerias.Utilerias.formatCurrency((double.TryParse(res.Get("TOTAL"), out double aux5)) ? res.GetDouble("TOTAL") : 0.0);
                        texttarjetas.Text = Utilerias.Utilerias.formatCurrency((double.TryParse(res.Get("TARJETAS"), out double aux6)) ? res.GetDouble("TARJETAS") : 0.0);
                        textaportaciones.Text = Utilerias.Utilerias.formatCurrency((double.TryParse(res.Get("APORTACION"), out double aux7)) ? res.GetDouble("APORTACION") : 0.0);

                        getRows();  
                    }
               

        
                }

            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "llenardataviewcorte";
                Utilerias.LOG.write(_clase, funcion, error);


            }




        }

        private void limpiardatos()
        {
            dataGridViewguias.Rows.Clear();
            textBoxfolio.Text = "";
            textBoxini.Text = "";
            textBoxfin.Text = "";
            textBoxusuario.Text = "";
            textguias.Text = "";
            textimporte.Text = "";
            textbanco.Text = "";
            textgastos.Text = "";
            textaportaciones.Text = "";
            texttotal.Text = "";
            textiva.Text = "";
            texttarjetas.Text = "";
        }
        private void limpiar_Click(object sender, EventArgs e)
        {
            limpiardatos();
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblHora.Text = DateTime.Now.ToString("hh:mm:ss");
            lbFecha.Text = DateTime.Now.ToString("D");
        }

        private void reimprimir()
        {
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

        void llenarticket(object sender, PrintPageEventArgs e)
        {
            try
            {
                codigoqr(textBoxfolio.Text);

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



                g.DrawString("Folio: " + textBoxfolio.Text, fBody10, sb, 2, espacio);
                g.DrawString("Cantidad de Pagos: " + dataGridViewguias.Rows.Count.ToString(), fBody10, sb, 150, espacio);


                espacio = espacio + 20;
                g.DrawString("Usuario: " + LoginInfo.NombreID + " " + LoginInfo.ApellidoID, fBody10, sb, 0, espacio);
                espacio = espacio + 20;

                g.DrawString("Rango de fechas: ", fBody10, sb, 80, espacio);
                espacio = espacio + 20;
                g.DrawString(textBoxini.Text, fBody, sb, 0, espacio);
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

                for (int i = 0; i < gimportes.Count(); i++)
                {


                    g.DrawString(gfolios[i], fBody5, sb, 0, espacio);
                    g.DrawString(Utilerias.Utilerias.formatCurrency(gimportes[i]), fBody5, sb, 45, espacio);
                    g.DrawString(Utilerias.Utilerias.formatCurrency(ggastos[i]), fBody5, sb, 85, espacio);
                    g.DrawString(Utilerias.Utilerias.formatCurrency(gtarjetas[i]), fBody5, sb, 130, espacio);
                    g.DrawString(Utilerias.Utilerias.formatCurrency(giva[i]), fBody5, sb, 185, espacio);
                    g.DrawString(Utilerias.Utilerias.formatCurrency(gtotal[i]), fBody5, sb, 225, espacio);

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

        private void Reimprimir_Click(object sender, EventArgs e)
        {
            if (dataGridViewguias.Rows.Count > 0)
            {
                reimprimir();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}