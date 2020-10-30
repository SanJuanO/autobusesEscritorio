using Autobuses.Utilerias;
using ConnectDB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Autobuses.Reportes
{
    public partial class Reporte_Corte_de_Caja : Form
    {

        public database db;
        ResultSet res = null;
        private string _clase = "reporte corte de caja";
        private int n = 0;
        private string fecha;
        private string sucursalbusqueda = "";
        private string usuariobusqueda = "";
        private string bcemitidos;
        private double biemitidos;
        private string bccancelado = "0";
        private double bicancelados = 0.0;
        private string bcventa="0";
        private double biventa;
        private string bccancfdt = "0";
        private double bicancfdt =0.0;
        private string bcree ="0";
        private double biree = 0.0;
        private double efectivo;
        private double tdebito;
        private double tcredito;
        private string gemitidas;
        private double gcanceladas;
        private double gimporte=0.0;
        private double gsalida;
        private double gcomisiontaq;
        private double gcomisionbanco;
        private double gaportacion;
        private string gdiesel;
        private string gcaseta;
        private double gtarjeta;
        private double giva;
        private double ganticipo;
        private double gtotal;
        private double cventa;
        private double ccancfueradt;
        private double canticipos;
        private double ctarjetas;
        private double ccoutassalidas;
        private double ccomisiones;
        private double ctarjeta;
        private double caportaciones;
        private string cdiesel;
        private string ccaseta;
        private double ctotalae;
        private double cefectivo;
        private double ctarjetacred;
        private double ctarjetad;
        private double cvales;
             private double tsalida = 0.0;
        private double tturno = 0.0;
        private double tpaso = 0.0;
        private int tamaño;
        private int espacio;
        private string folio;
        private Bitmap imagen;
        private string sucursal = LoginInfo.Sucursal;
        private string fechai;
        private string fechaf;
        private string usercaj;
        private Bitmap imagensplash = new Bitmap(Autobuses.Properties.Resources.logotickets);

        public Reporte_Corte_de_Caja()
        {
            InitializeComponent();
            this.Show();
        }

        private void permisos()
        {
       
            Reimprimir.Visible = false;

            if (LoginInfo.privilegios.Any(x => x == "reimprimir corte de caja"))
            {
                Reimprimir.Visible = true;

            }
        }

        public void getRows(string suc = "", string usu = "", string fech = "")
        {
            try
            {
                dataGridViewcorte.Rows.Clear();
                string sql = "SELECT * FROM CORTEDECAJA";

                if (suc == "" && usu!="")
                {
                    string pkuser = (comboBoxven.SelectedItem as ComboboxItem).Value.ToString();

                    sql += " WHERE  PKUSUARIO=@PKUSUARIO AND HASTA like @HASTA";
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@PKUSUARIO", pkuser);
                    db.command.Parameters.AddWithValue("@HASTA", fech + "%");
                }
                if (suc != "" && usu == "")
                {

                    sql += " WHERE  SUCURSAL=@SUCURSAL AND HASTA like @HASTA";
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@SUCURSAL", sucursalbusqueda);
                    db.command.Parameters.AddWithValue("@HASTA", fech + "%");
                }
                if(suc == "" && usu == "")
                {
                    sql += " WHERE  SUCURSAL=@SUCURSAL AND PKUSUARIO=@PKUSUARIO AND HASTA=@HASTA";
                    db.PreparedSQL(sql);
                    string pkuser1 = (comboBoxven.SelectedItem as ComboboxItem).Value.ToString();

                    db.command.Parameters.AddWithValue("@PKUSUARIO", pkuser1);
                    db.command.Parameters.AddWithValue("@SUCURSAL", suc);
                    db.command.Parameters.AddWithValue("@HASTA", fech + "%");
                }
            



                    res = db.getTable();

                    while (res.Next())
                    {
                        n = dataGridViewcorte.Rows.Add();


                        dataGridViewcorte.Rows[n].Cells["FOLIONAME"].Value = res.Get("FOLIO");
                        dataGridViewcorte.Rows[n].Cells["SUCURSALNAME"].Value = res.Get("SUCURSAL");
                        dataGridViewcorte.Rows[n].Cells["USUARIONAME"].Value = res.Get("USUARIO");
                    dataGridViewcorte.Rows[n].Cells["FECHANAME"].Value = fecha;

                    dataGridViewcorte.Rows[n].Cells["APARTIR"].Value = res.Get("APARTIR");
                    dataGridViewcorte.Rows[n].Cells["HASTA"].Value = res.Get("HASTA");
                    dataGridViewcorte.Rows[n].Cells["VENTA"].Value = res.Get("CVENTA");
                    dataGridViewcorte.Rows[n].Cells["CANCELADOS"].Value = res.Get("BICANCELADOS");
                    dataGridViewcorte.Rows[n].Cells["TOTAL"].Value = res.Get("CTOTALAENTREGAR");
                    dataGridViewcorte.Rows[n].Cells["EFECTIVOC"].Value = res.Get("CEFECTIVO");
                    dataGridViewcorte.Rows[n].Cells["TDEBITOC"].Value = res.Get("CTARJETADEBITO");
                    dataGridViewcorte.Rows[n].Cells["TCREDITOC"].Value = res.Get("CTARJETACREDITO");


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

        private void DateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                fecha = dateTimePicker1.Value.ToString("dd/MM/yyyy");
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "datetimepicker1";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }
        public void getDatosAdicionalessuc()
        {
            try
            {
                comboBoxsuc.Items.Clear();

                string sql = "SELECT SUCURSAL,ID FROM SUCURSALES";
                db.PreparedSQL(sql);
                res = db.getTable();

                while (res.Next())
                {
                    ComboboxItem item = new ComboboxItem();
                    item.Text = res.Get("SUCURSAL");
                    item.Value = res.Get("ID");
                    comboBoxsuc.Items.Add(item);

                }
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "getDatosAdicionalessuc";
                Utilerias.LOG.write(_clase, funcion, error);


            }

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
                    item.Text = res.Get("NOMBRE")+" " +res.Get("APELLIDOS") ;
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

        private void buttunbuscar(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxsuc.SelectedItem == null && comboBoxven.SelectedItem == null)
                {
                    Form mensaje = new Mensaje("Seleccione un vendedor o sucursal", true);

                    mensaje.ShowDialog();
                }
                else
                {
                    dataGridViewcorte.Rows.Clear();
                    if ((comboBoxsuc.SelectedItem) != null)
                    {

                        sucursalbusqueda = comboBoxsuc.SelectedItem.ToString();
                        getRows(sucursalbusqueda, usuariobusqueda, fecha);
                    }

                    if ((comboBoxven.SelectedItem) != null)
                    {

                        usuariobusqueda = comboBoxven.SelectedItem.ToString();
                        getRows(sucursalbusqueda, usuariobusqueda, fecha);
                    }


                }
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "buttunbuscar";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }


                private void llenardataviewcorte(object sender, DataGridViewCellEventArgs e)
                {
                    try
                    {

                        n = e.RowIndex;
                if (n == -1)
                {

                }
                else {

                    Reimprimir.Enabled = true;
                    button3.Enabled = true;
                    button3.BackColor = Color.FromArgb(38, 45, 53);
                    Reimprimir.BackColor = Color.FromArgb(38, 45, 53);
                    folio = (string)dataGridViewcorte.Rows[n].Cells["FOLIONAME"].Value;
                    usercaj = (string)dataGridViewcorte.Rows[n].Cells["USUARIONAME"].Value;
                    string sql = "SELECT * FROM CORTEDECAJA WHERE FOLIO=@FOLIO";
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@FOLIO", folio);

                    res = db.getTable();

                        if (res.Next())
                    {
                        fechai = res.Get("APARTIR");
                        fechaf = res.Get("HASTA");
                        bcemitidos = res.Get("BCEMITIDOS");
                        biemitidos = (double.TryParse(res.Get("BIEMITIDOS"), out double aux1)) ? res.GetDouble("BIEMITIDOS") : 0.0;
                        bccancelado = res.Get("BCCANCELADOS");
                        bicancelados = (double.TryParse(res.Get("BICANCELADOS"), out double aux2)) ? res.GetDouble("BICANCELADOS") : 0.0;
                        bccancfdt =res.Get("BCCANCFUERADT");
                        bcree = res.Get("BCREEXPEDIDOS") ;
                        biree = (double.TryParse(res.Get("BIREEXPEDIDOS"), out double aux4)) ? res.GetDouble("BIREEXPEDIDOS") : 0.0;
                        bcventa =res.Get("BCVENTA");
                        biventa = (double.TryParse(res.Get("BIVENTA"), out double aux5)) ? res.GetDouble("BIVENTA") : 0.0;
                        gemitidas = res.Get("GEMITIDOS");
                        gcanceladas = (double.TryParse(res.Get("GCANCELADAS"), out double aux66)) ? res.GetDouble("GCANCELADAS") : 0.0; 
                        gimporte = (double.TryParse(res.Get("GIMPORT"), out double aux6)) ? res.GetDouble("GIMPORT") : 0.0; 
                        //gsalida = res.Get("GCUOTASALIDA");
                        gcomisiontaq = (double.TryParse(res.Get("GCOMISION"), out double aux7)) ? res.GetDouble("GCOMISION") : 0.0;  res.Get("GCOMISION");
                        gcomisionbanco = (double.TryParse(res.Get("GCOMISIONBANCO"), out double aux77)) ? res.GetDouble("GCOMISIONBANCO") : 0.0;
                        gaportacion = (double.TryParse(res.Get("GAPORTACION"), out double aux8)) ? res.GetDouble("GAPORTACION") : 0.0; 
                        gdiesel = res.Get("GDIESEL");
                        gcaseta = res.Get("GCASETAS");
                        //gtarjeta = res.Get("GTARJETAS");
                        giva = (double.TryParse(res.Get("GIVA"), out double aux9)) ? res.GetDouble("GIVA") : 0.0; 
                        ganticipo = (double.TryParse(res.Get("GANTICIPO"), out double aux10)) ? res.GetDouble("GANTICIPO") : 0.0; 
                        gtotal = (double.TryParse(res.Get("GTOTAL"), out double aux11)) ? res.GetDouble("GTOTAL") : 0.0;
                        cventa = (double.TryParse(res.Get("CVENTA"), out double aux20)) ? res.GetDouble("CVENTA") : 0.0; 
                        ccancfueradt = (double.TryParse(res.Get("CCANCFUERADTURNO"), out double aux21)) ? res.GetDouble("CCANCFUERADTURNO") : 0.0;
                        canticipos = (double.TryParse(res.Get("CANTICIPOS"), out double aux22)) ? res.GetDouble("CANTICIPOS") : 0.0;
                        ctarjeta = (double.TryParse(res.Get("CTARJETAS"), out double aux23)) ? res.GetDouble("CTARJETAS") : 0.0; 
                        ccoutassalidas = (double.TryParse(res.Get("CCOUTASSALIDA"), out double aux283)) ? res.GetDouble("CCOUTASSALIDA") : 0.0;
                        ccomisiones = (double.TryParse(res.Get("CCOMISION"), out double aux13)) ? res.GetDouble("CCOMISION") : 0.0;
                        caportaciones = (double.TryParse(res.Get("CAPORTACION"), out double aux12)) ? res.GetDouble("CAPORTACION") : 0.0; 
                        cdiesel = res.Get("CDIESEL");
                        ccaseta = res.Get("CCASETA");
                        ctotalae = (double.TryParse(res.Get("CTOTALAENTREGAR"), out double aux103)) ? res.GetDouble("CTOTALAENTREGAR") : 0.0; 
                        cefectivo = (double.TryParse(res.Get("CEFECTIVO"), out double aux14)) ? res.GetDouble("CEFECTIVO") : 0.0;
                        ctarjetacred = (double.TryParse(res.Get("CTARJETACREDITO"), out double aux15)) ? res.GetDouble("CTARJETACREDITO") : 0.0; 
                        ctarjetad = (double.TryParse(res.Get("CTARJETADEBITO"), out double aux16)) ? res.GetDouble("CTARJETADEBITO") : 0.0; 
                        folio = res.Get("FOLIO");
                        cvales = (double.TryParse(res.Get("CVALES"), out double aux107)) ? res.GetDouble("CVALES") : 0.0;
                        tturno = (double.TryParse(res.Get("GTTURNO"), out double aux17)) ? res.GetDouble("GTTURNO") : 0.0; 
                        tpaso = (double.TryParse(res.Get("GTPASO"), out double aux18)) ? res.GetDouble("GTPASO") : 0.0;
                        tsalida =(double.TryParse(res.Get("GTSALIDA"), out double aux19)) ? res.GetDouble("GTSALIDA") : 0.0; 
                    }

                    if (db.execute())
                    {
                        textBoxturno.Text=
                        textBoxusuario.Text = usercaj;
                        textBoxini.Text = fechai;
                        textBoxfin.Text = fechaf;
                        textBoxgemitidas.Text = gemitidas;
                        textBoxgcanceladas.Text = Utilerias.Utilerias.formatCurrency(gcanceladas);
                        textBoxgimporte.Text = Utilerias.Utilerias.formatCurrency(gimporte);
                        //textBoxgsalida.Text = gsalida;
                        textBoxgcomisiontaq.Text = Utilerias.Utilerias.formatCurrency(gcomisiontaq);
                        //textBoxgcomisionbanco.Text = Utilerias.Utilerias.formatCurrency(gcomisionbanco);
                        textBoxgaportacion.Text = Utilerias.Utilerias.formatCurrency(gaportacion) ;
                       // textBoxgdiesel.Text = gdiesel;
                        //textBoxgcasetas.Text = gcaseta;
                        textBoxgiva.Text = Utilerias.Utilerias.formatCurrency(giva) ;
                        textBoxganticipos.Text = Utilerias.Utilerias.formatCurrency(ganticipo) ;
                        textBoxgtotal.Text = Utilerias.Utilerias.formatCurrency(gtotal) ;
                        textBoxcventa.Text = Utilerias.Utilerias.formatCurrency(cventa) ;
                        textBoxccancfueradt.Text = Utilerias.Utilerias.formatCurrency(ccancfueradt) ;
                        textBoxcanticipos.Text = Utilerias.Utilerias.formatCurrency(canticipos) ;
                        textBoxctarjeta.Text = Utilerias.Utilerias.formatCurrency(ctarjeta);
                       // textBoxcoutassalida.Text = ccoutassalidas;
                        textBoxccomision.Text = Utilerias.Utilerias.formatCurrency(ccomisiones);
                        textBoxcaportacion.Text= Utilerias.Utilerias.formatCurrency(caportaciones)  ;
                        textBoxccomision.Text = Utilerias.Utilerias.formatCurrency(caportaciones) ;
                        //textBoxcdiesel.Text = cdiesel;
                        //textBoxccasetas.Text = ccaseta;
                        textBoxtotalaentr.Text = Utilerias.Utilerias.formatCurrency(ctotalae) ;
                        textBoxcefectivo.Text = Utilerias.Utilerias.formatCurrency(cefectivo) ;
                        textBoxctarjetascred.Text = Utilerias.Utilerias.formatCurrency(ctarjetacred) ;
                        textBoxctarjetadeb.Text = Utilerias.Utilerias.formatCurrency(ctarjetad) ;
                        textBoxvales.Text = Utilerias.Utilerias.formatCurrency(cvales);
                        textBoxbcemitidos.Text = bcemitidos ;
                        textBoxbiemitidos.Text = Utilerias.Utilerias.formatCurrency(biemitidos) ;
                        textBoxbccancelados.Text = bccancelado;
                        textBoxbicancelados.Text = Utilerias.Utilerias.formatCurrency(bicancelados) ;
                        textBoxbcventa.Text = (bcventa) ;
                        textBoxbiventa.Text = Utilerias.Utilerias.formatCurrency(biventa) ;

                        textBoxturno.Text = Utilerias.Utilerias.formatCurrency(tturno); 
                        textBoxsalida.Text = Utilerias.Utilerias.formatCurrency(tsalida); 
                        textBoxpaso.Text = Utilerias.Utilerias.formatCurrency(tpaso); 

                        textBoxbcree.Text = (bcree) ;
                        textboxbiree.Text = Utilerias.Utilerias.formatCurrency(biree) ;
                        textBoxvales.Text = Utilerias.Utilerias.formatCurrency(cvales);
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
                        string funcion = "closestrip";
                        Utilerias.LOG.write(_clase, funcion, error);


                    }
                }


                private void Reimprimir_Click(object sender, EventArgs e)
                {
                    try
                    {


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

                pd.Print();
                CrearTicket ticket0 = new CrearTicket();
                ticket0.TextoIzquierda("");
                ticket0.TextoIzquierda("");
                ticket0.TextoIzquierda("");
                ticket0.TextoIzquierda("");
                ticket0.CortaTicket();
                ticket0.ImprimirTicket(Settings1.Default.impresora);
                Utilerias.LOG.acciones("reimprimio corte de caja " + folio);
                Reimprimir.Enabled = false;
            }

            catch (Exception err)
                    {
                        string error = err.Message;
                        MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                        string funcion = "reemprimir";
                        Utilerias.LOG.write(_clase, funcion, error);


                    }
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
                g.DrawString("SUCURSAL: " + sucursal, fBody, sb, 140, espacio + 40);
                espacio = espacio + 70;
                g.DrawString("Folio: " + folio, fBody, sb, 0, espacio);
                espacio = espacio + 20;
                g.DrawString("Cajero: " + LoginInfo.UserID, fBody10, sb, 0, espacio);
                espacio = espacio + 20;
                g.DrawString("Apartir de: " + LoginInfo.ingreso, fBody10, sb, 0, espacio);
                espacio = espacio + 20;
                g.DrawString("Hasta: " + fechaf, fBody10, sb, 0, espacio);
                espacio = espacio + 20;

                g.DrawRectangle(Pens.Black, 0, espacio, 268, 80);
                g.DrawRectangle(Pens.Black, 0, espacio, 268, 20);

                espacio = espacio + 2;
                g.DrawString("Boletos", fBody10, sb, 0, espacio);
                g.DrawString("Cantidad", fBody10, sb, 90, espacio);
                g.DrawString("Importe", fBody10, sb, 170, espacio);
                espacio = espacio + 23;
                g.DrawString("Emitidos", fBody10, sb, 0, espacio);
                g.DrawString(bcemitidos, fBody, sb, 110, espacio);
                g.DrawString(Utilerias.Utilerias.formatCurrency(biemitidos), fBody, sb, 170, espacio);
                espacio = espacio + 20;
                g.DrawString("Cancelados", fBody10, sb, 0, espacio);
                g.DrawString(bccancelado, fBody, sb, 110, espacio);
                g.DrawString(Utilerias.Utilerias.formatCurrency(bicancelados), fBody, sb, 170, espacio);
                espacio = espacio + 20;

                g.DrawString("Venta", fBody10, sb, 0, espacio);
                g.DrawString(bcventa.ToString(), fBody, sb, 110, espacio);
                g.DrawString(Utilerias.Utilerias.formatCurrency( biventa), fBody, sb, 170, espacio);
                espacio = espacio + 30;
                int es = espacio;
                g.DrawRectangle(Pens.Black, 0, espacio, 130, 260);
                g.DrawRectangle(Pens.Black, 0, espacio, 130, 20);

                espacio = espacio + 2;

                g.DrawString("Guias", fBody12, sb, 40, espacio);
                espacio = espacio + 23;
                g.DrawString("Emitidas", fBody7, sb, 0, espacio);
                g.DrawString(gemitidas, fBody7, sb, 65, espacio);
                espacio = espacio + 20;
                g.DrawString("Canc.", fBody7, sb, 0, espacio);
                g.DrawString(Utilerias.Utilerias.formatCurrency(gcanceladas), fBody7, sb, 65, espacio);
                espacio = espacio + 20;
                g.DrawString("Importe", fBody7, sb, 0, espacio);
                g.DrawString(Utilerias.Utilerias.formatCurrency( gimporte), fBody7, sb, 65, espacio);
                espacio = espacio + 20;
                g.DrawString("Comisión", fBody7, sb, 0, espacio);
                g.DrawString(Utilerias.Utilerias.formatCurrency(gcomisiontaq), fBody7, sb, 65, espacio);
                espacio = espacio + 20;
                //g.DrawString("Comisión Banco", fBody7, sb, 0, espacio);
                //g.DrawString("$" + gcomisionbanco, fBody7, sb, 65, espacio);
                //espacio = espacio + 20;
                g.DrawString("Aportación", fBody7, sb, 0, espacio);
                g.DrawString(Utilerias.Utilerias.formatCurrency(gaportacion), fBody7, sb, 65, espacio);
                espacio = espacio + 20;
                //g.DrawString("Diesel", fBody7, sb, 0, espacio);
                //g.DrawString("$" + gdiesel, fBody7, sb, 65, espacio);
                //espacio = espacio + 20;
                //g.DrawString("Casetas", fBody7, sb, 0, espacio);
                //g.DrawString("$" + gcaseta, fBody7, sb, 65, espacio);
                //espacio = espacio + 20;
                g.DrawString("T. Turno", fBody7, sb, 0, espacio);
                g.DrawString(Utilerias.Utilerias.formatCurrency(tturno), fBody7, sb, 65, espacio);
                espacio = espacio + 20;
                g.DrawString("T. Salida", fBody7, sb, 0, espacio);
                g.DrawString(Utilerias.Utilerias.formatCurrency(tsalida), fBody7, sb, 65, espacio);
                espacio = espacio + 20;
                g.DrawString("T. Paso", fBody7, sb, 0, espacio);
                g.DrawString(Utilerias.Utilerias.formatCurrency(tpaso), fBody7, sb, 65, espacio);
                espacio = espacio + 20;
                g.DrawString("IVA", fBody7, sb, 0, espacio);
                g.DrawString(Utilerias.Utilerias.formatCurrency(giva), fBody7, sb, 65, espacio);
                espacio = espacio + 20;
                g.DrawString("Anticipos", fBody7, sb, 0, espacio);
                g.DrawString(Utilerias.Utilerias.formatCurrency(ganticipo), fBody7, sb, 65, espacio);
                espacio = espacio + 20;
                g.DrawString("Total", fBody7, sb, 0, espacio);
                g.DrawString(Utilerias.Utilerias.formatCurrency(gtotal), fBody7, sb, 65, espacio);

                g.DrawRectangle(Pens.Black, 135, es, 135, 260);
                g.DrawRectangle(Pens.Black, 135, es, 135, 20);
                espacio = es + 2;
                g.DrawString("Caja", fBody12, sb, 180, espacio);
                espacio = espacio + 23;
                g.DrawString("Venta", fBody7, sb, 140, espacio);
                g.DrawString(Utilerias.Utilerias.formatCurrency(cventa), fBody7, sb, 200, espacio);
                espacio = espacio + 20;
                g.DrawString("Canc.", fBody7, sb, 140, espacio);
                g.DrawString(Utilerias.Utilerias.formatCurrency(ccancfueradt), fBody7, sb, 200, espacio);
                espacio = espacio + 20;
                //g.DrawString("Anticipos", fBody7, sb, 140, espacio);
                //g.DrawString("$" + canticipos, fBody7, sb, 200, espacio);
                //espacio = espacio + 20;
                g.DrawString("Gastos", fBody7, sb, 140, espacio);
                g.DrawString(Utilerias.Utilerias.formatCurrency(ganticipo), fBody7, sb, 200, espacio);
                espacio = espacio + 20;
                g.DrawString("Tarjetas", fBody7, sb, 140, espacio);
                g.DrawString(Utilerias.Utilerias.formatCurrency(ctarjeta), fBody7, sb, 200, espacio);
                espacio = espacio + 20;
                g.DrawString("Comisión", fBody7, sb, 140, espacio);
                g.DrawString(Utilerias.Utilerias.formatCurrency(ccomisiones), fBody7, sb, 200, espacio);
                espacio = espacio + 20;
                g.DrawString("Aportación", fBody7, sb, 140, espacio);
                g.DrawString(Utilerias.Utilerias.formatCurrency(caportaciones), fBody7, sb, 200, espacio);
                espacio = espacio + 20;
                //g.DrawString("Diesel", fBody7, sb, 140, espacio);
                //g.DrawString("$" + cdiesel, fBody7, sb, 200, espacio);
                //espacio = espacio + 20;
                //g.DrawString("Casetas", fBody7, sb, 140, espacio);
                //g.DrawString("$" + ccaseta, fBody7, sb, 200, espacio);
                //espacio = espacio + 20;
                g.DrawString("Total", fBody9, sb, 140, espacio);
                g.DrawString(Utilerias.Utilerias.formatCurrency(ctotalae), fBody9, sb, 200, espacio);
                espacio = espacio + 20;
                g.DrawString("Efectivo", fBody7, sb, 140, espacio);
                g.DrawString(Utilerias.Utilerias.formatCurrency(cefectivo), fBody7, sb, 200, espacio);
                espacio = espacio + 20;
                g.DrawString("T. Crédito", fBody7, sb, 140, espacio);
                g.DrawString(Utilerias.Utilerias.formatCurrency(ctarjetacred), fBody7, sb, 200, espacio);
                espacio = espacio + 20;
                g.DrawString("T. Debito", fBody7, sb, 140, espacio);
                g.DrawString(Utilerias.Utilerias.formatCurrency(ctarjetad), fBody7, sb, 200, espacio);
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

        private void Limpiar_Click(object sender, EventArgs e)
        {
            comboBoxsuc.Text = null;
            comboBoxven.Text = null;
            sucursalbusqueda = "";
            dataGridViewcorte.Rows.Clear();
            usuariobusqueda = "";
            Reimprimir.Enabled = false;
            Reimprimir.BackColor = Color.White;
            limpiartext();
            button3.Enabled = false;
        }

       private void limpiartext()
        {
            textBoxusuario.Text = "";
            textBoxini.Text = "";
            textBoxfin.Text = "";
            textBoxgemitidas.Text = "";
            textBoxgcanceladas.Text = "";
            textBoxgimporte.Text = "";
            //textBoxgsalida.Text = gsalida;
            textBoxgcomisiontaq.Text = "";
          //  textBoxgcomisionbanco.Text = "";
            textBoxgaportacion.Text = "";
            //textBoxgdiesel.Text = "";
            //textBoxgcasetas.Text = "";
            textBoxgiva.Text = "";
            textBoxganticipos.Text = "";
            textBoxgtotal.Text = "";
            textBoxcventa.Text = "";
            textBoxccancfueradt.Text = "";
            textBoxcanticipos.Text = "";
            //textBoxctarjeta.Text = ctarjeta;
           // textBoxcoutassalida.Text = "";
            textBoxccomision.Text = "";
            textBoxcaportacion.Text = "";
            textBoxccomision.Text = "";
           // textBoxcdiesel.Text = "";
            //textBoxccasetas.Text = "";
            textBoxtotalaentr.Text = "";
            textBoxcefectivo.Text = "";
            textBoxctarjetascred.Text = "";
            textBoxctarjetadeb.Text = "";
            textBoxvales.Text = "";
            textBoxbcemitidos.Text = "";
            textBoxbiemitidos.Text = "";
            textBoxbccancelados.Text = "";
            textBoxbicancelados.Text = "";
            textBoxbcventa.Text = "";
            textBoxbiventa.Text = "";


            textBoxbcree.Text = "";
            textboxbiree.Text = "";
            textBoxvales.Text = "";
        }
        private void Reporte_Corte_de_Caja_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;

        }

        private void Reporte_Corte_de_Caja_Shown(object sender, EventArgs e)
        {
            db = new database();
            comboBoxven.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxsuc.DropDownStyle = ComboBoxStyle.DropDownList;
            dataGridViewcorte.EnableHeadersVisualStyles = false;
            timer1.Start();
            timer1.Interval = 1;
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
            timer2.Start();
            fecha = dateTimePicker1.Value.ToString("dd/MM/yyyy");
            button3.Enabled = false;
            button3.BackColor = Color.White;

        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            fecha = DateTime.Now.ToString("dd/MM/yyyy");
            getDatosAdicionalessuc();
            getDatosAdicionalesusu();
            Reimprimir.Enabled = false;
            Reimprimir.BackColor = Color.White;
            permisos();
            timer1.Stop();
        }

        private void GroupBox4_Enter(object sender, EventArgs e)
        {

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



        private void GroupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void Timer2_Tick(object sender, EventArgs e)
        {
            lblHora.Text = DateTime.Now.ToString("hh:mm:ss");
            lbFecha.Text = DateTime.Now.ToString("D");
        }

        private void GroupBox5_Enter(object sender, EventArgs e)
        {

        }


        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void PanelBarraTitulo_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void groupBox4_Enter_1(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter_1(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox5_Enter_1(object sender, EventArgs e)
        {

        }

        private void comboBoxven_SelectedIndexChanged(object sender, EventArgs e)
        {

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


        private void button3_Click(object sender, EventArgs e)
        {
            if (CheckOpened("asientosocupadosboleto"))
            {
                form.StartPosition = FormStartPosition.CenterScreen;
                form.Show();

            }
            else
            {

                form = new Reportes.detallecortecaja(textBoxusuario.Text, fechai, fechaf);
                AddOwnedForm(form);
                form.StartPosition = FormStartPosition.CenterScreen;
                form.Show();
                Utilerias.LOG.acciones("ingreso a detallecorte ");


            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
      
            Utilerias.Utilerias.ExportarDataGridViewExcel(dataGridViewcorte, new List<string> {""});
          

        }
    }
}
