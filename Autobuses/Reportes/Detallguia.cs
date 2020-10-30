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
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Autobuses.Reportes
{
    public partial class Detallguia : Form
    {
        public database db;
        Bitmap imagen;

        ResultSet res = null;
        private string pk_guia;
        private string _clase = "reporteguia";
        private int n = 0;
        private string dia;
        private int boletos = 0;
        private string folio;
        private string status;
        private string origen;
        private string destino;
        private string fecha;
        private string autobus;
        private string boleto;
        private string importe;
        private string dsalida;
        private int ideboleto = 0;

        private string comtaq;
        private string compban;
        private string aportacion;
        private string diesel;
        private string caseta;
        private string ttpaso;
        private string tturno;
        private string tsalida;
        private bool tick = false;
        private bool tick2 = false;
        PrintDocument pd = new PrintDocument();
        PrintDocument pd2 = new PrintDocument();
        PrintDocument pdc = new PrintDocument();
        PrintDocument pdc2 = new PrintDocument();
        private string vseden;
        private string iva;
        private string socior;
        private string anticipo;
        private string total;
        private string foliobuscar = "";
        private string sucursal;
        private string validador;
        private int espacio = 220;
        private string chofer;
        private string linea;
        private int tamaño;
        private string hora;
        private List<string> asiento = new List<string>();
        private List<string> pasajero = new List<string>();
        private List<string> destinopasajero = new List<string>();
        private List<string> foliopasajero = new List<string>();
        private List<string> tarifapasajero = new List<string>();
        private List<string> preciopasajero = new List<string>();
        public Detallguia(string pkguia)
        {

            InitializeComponent();

                            pk_guia = pkguia;

            this.Show();
        }



        private void valores()
        {
            try
            {
                int n = 0;

                    folio = (string)dataGridViewguias.Rows[n].Cells[1].Value;
                    foliobuscar = folio;
                    status = (string)dataGridViewguias.Rows[n].Cells[2].Value;
                    origen = (string)dataGridViewguias.Rows[n].Cells[3].Value;
                    destino = (string)dataGridViewguias.Rows[n].Cells[4].Value;
                    fecha = (string)dataGridViewguias.Rows[n].Cells[5].Value;
                    autobus = (string)dataGridViewguias.Rows[n].Cells[6].Value;
                    boleto = (string)dataGridViewguias.Rows[n].Cells[7].Value;
                    importe =Utilerias.Utilerias.formatCurrency( Convert.ToDouble(dataGridViewguias.Rows[n].Cells[8].Value));
                    dsalida = Utilerias.Utilerias.formatCurrency(Convert.ToDouble(dataGridViewguias.Rows[n].Cells[9].Value));
                    comtaq = Utilerias.Utilerias.formatCurrency(Convert.ToDouble(dataGridViewguias.Rows[n].Cells[9].Value));
                    compban = Utilerias.Utilerias.formatCurrency(Convert.ToDouble(dataGridViewguias.Rows[n].Cells[10].Value));
                    aportacion = Utilerias.Utilerias.formatCurrency(Convert.ToDouble(dataGridViewguias.Rows[n].Cells[12].Value));
                    diesel = Utilerias.Utilerias.formatCurrency(Convert.ToDouble(dataGridViewguias.Rows[n].Cells[12].Value));
                    caseta = Utilerias.Utilerias.formatCurrency(Convert.ToDouble(dataGridViewguias.Rows[n].Cells[13].Value));
                    tsalida = Utilerias.Utilerias.formatCurrency(Convert.ToDouble(dataGridViewguias.Rows[n].Cells[14].Value));
                tturno = Utilerias.Utilerias.formatCurrency(Convert.ToDouble(dataGridViewguias.Rows[n].Cells[15].Value));
                ttpaso= Utilerias.Utilerias.formatCurrency(Convert.ToDouble(dataGridViewguias.Rows[n].Cells[16].Value));

                vseden = (string)dataGridViewguias.Rows[n].Cells[17].Value;
                    iva = Utilerias.Utilerias.formatCurrency(Convert.ToDouble(dataGridViewguias.Rows[n].Cells[18].Value));
                    anticipo = Utilerias.Utilerias.formatCurrency(Convert.ToDouble(dataGridViewguias.Rows[n].Cells[19].Value));
                    total = Utilerias.Utilerias.formatCurrency(Convert.ToDouble(dataGridViewguias.Rows[n].Cells[20].Value));
                    sucursal = (string)dataGridViewguias.Rows[n].Cells[21   ].Value;
                    validador = (string)dataGridViewguias.Rows[n].Cells[22].Value;
                    chofer = (string)dataGridViewguias.Rows[n].Cells[23].Value;
                    linea = (string)dataGridViewguias.Rows[n].Cells[24].Value;
                    hora = (string)dataGridViewguias.Rows[n].Cells[25].Value;
                    socior = (string)dataGridViewguias.Rows[n].Cells[26].Value;
                    //pk_guia = (string)dataGridViewguias.Rows[n].Cells[27].Value;

                

            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "datagridviewguia";
                Utilerias.LOG.write(_clase, funcion, error);


            }

        }

        private void act()
        {
            try
            {
             
                    int count = 1;
                    string sql = "SELECT FOLIO,STATUS,ORIGEN,DESTINO,FECHA,AUTOBUS,BOLETOS,CONVERT(FLOAT,IMPORTE) as IMPORTE,CONVERT(FLOAT, COMTAQ) as COMTAQ,CONVERT(FLOAT, COMPBAN) as COMPBAN,CONVERT(FLOAT, APORTACION) as APORTACION,DIESEL,CASETA,TSALIDA,TTURNO,TPASO,VSEDENA,CONVERT(FLOAT, IVA) as IVA,CONVERT(FLOAT, ANTICIPO) as ANTICIPO,CONVERT(FLOAT, TOTAL) as TOTAL,SUCURSAL,validador,CHOFER,LINEA,HORA,SOCIO FROM GUIA";

                    sql += " WHERE  PK=@PK order by BOLETOS ";
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@PK", pk_guia);
                boletos = 0;

                    res = db.getTable();
                int n = 0;
                    if (res.Next())
                    {
                        n = dataGridViewguias.Rows.Add();


                    dataGridViewguias.Rows[n].Cells[0].Value = res.Get("FECHA");
                   textBoxfech.Text= res.Get("FECHA");
                    dataGridViewguias.Rows[n].Cells[1].Value = res.Get("FOLIO");
                    textBoxfol.Text= res.Get("FOLIO");
                    dataGridViewguias.Rows[n].Cells[2].Value = res.Get("STATUS");
                        status = res.Get("STATUS");
                    
                    dataGridViewguias.Rows[n].Cells[3].Value = res.Get("ORIGEN");
                    textBoxorig.Text= res.Get("ORIGEN");
                    dataGridViewguias.Rows[n].Cells[4].Value = res.Get("DESTINO");
                    textBoxdest.Text= res.Get("DESTINO");
                    string fech = res.Get("HORA");
                    dataGridViewguias.Rows[n].Cells[5].Value = res.Get("FECHA");

                    dataGridViewguias.Rows[n].Cells[6].Value = res.Get("AUTOBUS");
                    textBoxaut.Text = res.Get("AUTOBUS");

                    dataGridViewguias.Rows[n].Cells[7].Value = res.Get("BOLETOS");
                    textBoxbol.Text = res.Get("BOLETOS");

                    dataGridViewguias.Rows[n].Cells[8].Value = res.GetDouble("IMPORTE");
                    textBoximp.Text = textBoxsalid.Text = Utilerias.Utilerias.formatCurrency(res.GetDouble("IMPORTE"));


                    dataGridViewguias.Rows[n].Cells[9].Value = res.GetDouble("COMTAQ");
                    dataGridViewguias.Rows[n].Cells[10].Value = (double.TryParse(res.Get("COMPBAN"), out double aux1C)) ? res.GetDouble("COMPBAN") : 0.0;
                    textBoxcomban.Text = Utilerias.Utilerias.formatCurrency((double.TryParse(res.Get("COMPBAN"), out double aux1A)) ? res.GetDouble("COMPBAN") : 0.0);

                    dataGridViewguias.Rows[n].Cells[11].Value = res.GetDouble("APORTACION");
                    textBoxaportaciones.Text = Utilerias.Utilerias.formatCurrency(res.GetDouble("APORTACION"));
                    
                    dataGridViewguias.Rows[n].Cells[12].Value = res.GetDouble("DIESEL");
                    dataGridViewguias.Rows[n].Cells[13].Value = res.GetDouble("CASETA");
                    dataGridViewguias.Rows[n].Cells[14].Value = res.GetDouble("TSALIDA");
                    textBoxsalid.Text = Utilerias.Utilerias.formatCurrency(res.GetDouble("TSALIDA"));
                    dataGridViewguias.Rows[n].Cells[15].Value = res.GetDouble("TTURNO");
                    textBoxturno.Text = Utilerias.Utilerias.formatCurrency(res.GetDouble("TTURNO"));
                    dataGridViewguias.Rows[n].Cells[16].Value = (double.TryParse(res.Get("TPASO"), out double aux1)) ? res.GetDouble("TPASO") : 0.0;
                    textBoxpaso.Text = Utilerias.Utilerias.formatCurrency((double.TryParse(res.Get("TPASO"), out double aux11)) ? res.GetDouble("TPASO") : 0.0);
                    dataGridViewguias.Rows[n].Cells[17].Value = res.Get("VSEDENA");
                    dataGridViewguias.Rows[n].Cells[18].Value = (double.TryParse(res.Get("IVA"), out double aux3)) ? res.GetDouble("IVA") : 0.0 ;
                    textBoxiva.Text = Utilerias.Utilerias.formatCurrency((double.TryParse(res.Get("IVA"), out double aux2) ? res.GetDouble("IVA") : 0.0));
                    dataGridViewguias.Rows[n].Cells[19].Value = (double.TryParse(res.Get("ANTICIPO"), out double aux1A2)) ? res.GetDouble("ANTICIPO") : 0.0;
                    textboxanticipo.Text = Utilerias.Utilerias.formatCurrency((double.TryParse(res.Get("ANTICIPO"), out double aux1A22)) ? res.GetDouble("ANTICIPO") : 0.0);
                    dataGridViewguias.Rows[n].Cells[20].Value = (double.TryParse(res.Get("TOTAL"), out double aux5)) ? res.GetDouble("TOTAL") : 0.0;
                    textBoxtotal.Text = Utilerias.Utilerias.formatCurrency((double.TryParse(res.Get("TOTAL"), out double aux1T)) ? res.GetDouble("TOTAL") : 0.0);
                    dataGridViewguias.Rows[n].Cells[21].Value = res.Get("SUCURSAL");
                    textBoxsuc.Text = res.Get("SUCURSAL");
                    dataGridViewguias.Rows[n].Cells[22].Value = res.Get("validador");
                    textBoxvalidador.Text = res.Get("validador");
                    dataGridViewguias.Rows[n].Cells[23].Value = res.Get("CHOFER");
                    textBoxconducto.Text = res.Get("CHOFER");
                    dataGridViewguias.Rows[n].Cells[24].Value = res.Get("LINEA");
                    
                    dataGridViewguias.Rows[n].Cells[25].Value = res.Get("HORA");
                    textBoxsal.Text = res.Get("HORA");
                    dataGridViewguias.Rows[n].Cells[26].Value = res.Get("SOCIO");
                    count++;
                    boletos+=1;
                    }

                    db.execute();

                

            }

            catch (Exception err)
             {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "getRows";
                Utilerias.LOG.write(_clase, funcion, error);


            }

        }


        void llenarticket(object sender, PrintPageEventArgs e)
        {
            try
            {
                codigoqr(folio);

                Graphics g = e.Graphics;
                // g.DrawRectangle(Pens.Black, 3, 5, 340, 700);
                // g.DrawImage(imagensplash, 10, 0);
                Font fBody7 = new Font("Lucida", 7, FontStyle.Bold);
                Font fBody5 = new Font("Lucida", 5, FontStyle.Bold);

                Font fBody9 = new Font("Lucida", 9, FontStyle.Bold);
                Font fBody = new Font("Lucida", 8, FontStyle.Bold);
                Font fBody10 = new Font("Lucida", 9, FontStyle.Bold);
                Font fBody12 = new Font("Lucida", 12, FontStyle.Bold);
                Font fBody18 = new Font("Lucida", 18, FontStyle.Bold);

                Color customColor = Color.FromArgb(255, Color.Black);
                SolidBrush sb = new SolidBrush(customColor);
                espacio = 0;
                g.DrawString("Fecha :", fBody, sb, 2, espacio);
                g.DrawString(DateTime.Now.ToShortDateString(), fBody, sb, 48, espacio);
                g.DrawString("Hora :", fBody, sb, 170, 0);
                g.DrawString(DateTime.Now.ToShortTimeString(), fBody, sb, 210, espacio);
                espacio = espacio + 25;
                g.DrawString("GUIA", fBody18, sb, 0, 25);
                espacio = espacio + 10;
         
                espacio = espacio + 20;
                g.DrawString("SUCURSAL: " + sucursal, fBody12, sb, 0, espacio);
                espacio = espacio + 20;
                g.DrawString("Folio: "+folio, fBody10, sb, 2, espacio);
                g.DrawString("Autobus: "+autobus, fBody9, sb, 180, espacio);
                espacio = espacio + 20;
                g.DrawString(" Boletos: " + boleto, fBody10, sb, 180, espacio);
                g.DrawString("Salida: "+hora.ToString(), fBody10, sb, 0, espacio);
                espacio = espacio + 20;
                g.DrawString("Origen: "+origen, fBody10, sb, 0, espacio);
                g.DrawString("Status: " + status, fBody10, sb, 180, espacio);
                espacio = espacio + 20;
                g.DrawString("Destino: "+destino, fBody10, sb, 0, espacio);
                espacio = espacio + 20;
                g.DrawString(" Chofer: "+chofer, fBody10, sb, 0, espacio);
                espacio = espacio + 20;
                g.DrawString(" validador: "+validador, fBody10, sb, 0, espacio);
                espacio = espacio + 20;
                g.DrawString("Socio: "+socior, fBody10, sb, 0, espacio);
                espacio = espacio + 30;
                g.DrawRectangle(Pens.Black, 0, espacio, 268, 12);
                g.DrawString("ASIENTO", fBody5, sb, 0, espacio);
                g.DrawString("NOMBRE", fBody5, sb, 40, espacio);
                g.DrawString("DESTINO", fBody5, sb, 90, espacio);
                g.DrawString("BOLETO", fBody5, sb, 145, espacio);
                g.DrawString("TARIFA", fBody5, sb, 190, espacio);
                g.DrawString("PRECIO", fBody5, sb, 235, espacio);
                espacio = espacio + 20;

                for (int i = 0; i < asiento.Count(); i++)
                {

                    string nom = pasajero[i].ToString().Substring(0, 12);
                    g.DrawString(asiento[i], fBody5, sb, 10, espacio);
                    g.DrawString(nom, fBody5, sb, 30, espacio);
                    g.DrawString(destinopasajero[i], fBody5, sb, 80, espacio);
                    g.DrawString(foliopasajero[i], fBody5, sb, 135, espacio);
                    g.DrawString(tarifapasajero[i], fBody5, sb, 185, espacio);
                    g.DrawString(preciopasajero[i], fBody5, sb, 235, espacio);
                    if (espacio > 1180)
                    {
                        ideboleto = i;
                        g.Dispose();
                        tick = true;
                        continuacion();
                        return;
                    }
                    espacio = espacio + 20;

                }


                if (espacio > 970)
                {
                    g.Dispose();
                    continuacion2();
                    return;
                }

                g.DrawString("Importe", fBody7, sb, 170, espacio);
                g.DrawString( importe, fBody7, sb, 230, espacio);
                g.DrawImage(imagen, 30, espacio);
                espacio = espacio + 20;
              
            
                g.DrawString("Com. Banco", fBody7, sb, 170, espacio);
                g.DrawString(compban, fBody7, sb, 230, espacio);
                //g.DrawString("Boletos:", fBody7, sb, 0, espacio);
                //g.DrawString(boleto, fBody7, sb, 40, espacio);

                espacio = espacio + 20;
               
                g.DrawString("Aportación", fBody7, sb, 170, espacio);
                g.DrawString(aportacion, fBody7, sb, 230, espacio);
               espacio = espacio + 20;
                //g.DrawString("Diesel", fBody7, sb, 170, espacio);
                //g.DrawString("$" + diesel, fBody7, sb, 230, espacio);
                //espacio = espacio + 20;
                //g.DrawString("Casetas", fBody7, sb, 170, espacio);
                //g.DrawString("$" + caseta, fBody7, sb, 230, espacio);
                //espacio = espacio + 20;
                g.DrawString("T. turno", fBody7, sb, 170, espacio);
                g.DrawString( tturno, fBody7, sb, 230, espacio);
                espacio = espacio + 20;
                g.DrawString("T. paso", fBody7, sb, 170, espacio);
                g.DrawString( ttpaso, fBody7, sb, 230, espacio);
                espacio = espacio + 20;
                g.DrawString("T. Salida", fBody7, sb, 170, espacio);
                g.DrawString( tsalida, fBody7, sb, 230, espacio);
                espacio = espacio + 20;
                g.DrawString("IVA", fBody7, sb, 170, espacio);
                g.DrawString( iva, fBody7, sb, 230, espacio);
                //espacio = espacio + 20;
                //g.DrawString("V.SEDENA", fBody7, sb, 170, espacio);
                //g.DrawString("$" + vseden, fBody7, sb, 230, espacio);
                espacio = espacio + 20;
                g.DrawString("Anticipo", fBody7, sb, 170, espacio);
                g.DrawString(anticipo, fBody7, sb, 230, espacio);
                espacio = espacio + 20;
                g.DrawString("Total", fBody7, sb, 170, espacio);
                g.DrawString( total, fBody7, sb, 230, espacio);
                espacio = espacio + 10;
                
                g.Dispose();
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error llenar ticket, intente de nuevo.");
                string funcion = "llenarticket";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }
        private void continuacion()
        {


            pdc.DefaultPageSettings.PaperSize = new PaperSize("Custom", 315, tamaño);
            pdc.DefaultPageSettings.PaperSize.RawKind = 119;
            pdc.PrinterSettings.DefaultPageSettings.PaperSize.RawKind = 119;
            pdc.DefaultPageSettings.PrinterSettings.PrintRange = PrintRange.AllPages;

            pdc.DefaultPageSettings.Landscape = false;


            pdc.PrintPage += new PrintPageEventHandler(llenarticketcontinuacion);


            pdc.PrinterSettings.PrinterName = Settings1.Default.impresora;
        }
        private void continuacion2()
        {

            tick2 = true;
            pdc2.DefaultPageSettings.PaperSize = new PaperSize("Custom", 315, tamaño - (ideboleto * 20));
            pdc2.DefaultPageSettings.PaperSize.RawKind = 119;
            pdc2.PrinterSettings.DefaultPageSettings.PaperSize.RawKind = 119;
            pdc2.DefaultPageSettings.PrinterSettings.PrintRange = PrintRange.AllPages;

            pdc2.DefaultPageSettings.Landscape = false;


            pdc2.PrintPage += new PrintPageEventHandler(llenarticketcontinuacion2);


            pdc2.PrinterSettings.PrinterName = Settings1.Default.impresora;
        }
        void llenarticketcontinuacion(object sender, PrintPageEventArgs e)
        {
            try
            {

                Graphics g = e.Graphics;
                // g.DrawRectangle(Pens.Black, 3, 5, 340, 700);
                // g.DrawImage(imagensplash, 10, 0);
                Font fBody7 = new Font("Lucida", 7, FontStyle.Bold);
                Font fBody5 = new Font("Lucida", 5, FontStyle.Bold);

                Font fBody9 = new Font("Lucida", 9, FontStyle.Bold);
                Font fBody = new Font("Lucida", 8, FontStyle.Bold);
                Font fBody10 = new Font("Lucida", 10, FontStyle.Bold);
                Font fBody12 = new Font("Lucida", 12, FontStyle.Bold);
                Font fBody18 = new Font("Lucida", 18, FontStyle.Bold);
                Color customColor = Color.FromArgb(255, Color.Black);
                SolidBrush sb = new SolidBrush(customColor);

                espacio = 0;

                espacio = espacio + 20;
                for (int i = ideboleto; i < asiento.Count(); i++)
                {
                    string nom = pasajero[i].ToString().Substring(0, 12);
                    g.DrawString(asiento[i], fBody5, sb, 10, espacio);
                    g.DrawString(nom, fBody5, sb, 30, espacio);
                    g.DrawString(destinopasajero[i], fBody5, sb, 80, espacio);
                    g.DrawString(foliopasajero[i], fBody5, sb, 135, espacio);
                    g.DrawString(tarifapasajero[i], fBody5, sb, 185, espacio);
                    g.DrawString(preciopasajero[i], fBody5, sb, 235, espacio);
                    espacio = espacio + 20;

                }

                g.DrawString("Importe", fBody7, sb, 170, espacio);
                g.DrawString(importe, fBody7, sb, 230, espacio);
                g.DrawImage(imagen, 30, espacio);
                espacio = espacio + 20;

                // g.DrawString("Salida", fBody7, sb, 170, espacio);
                // g.DrawString("$" + dsalida, fBody7, sb, 230, espacio);
                //espacio = espacio + 20;
                //g.DrawString("Com. Taq", fBody7, sb, 170, espacio);
                //g.DrawString("$" + comtaq, fBody7, sb, 230, espacio);
                //g.DrawString("Boletos:", fBody7, sb, 0, espacio);
                //g.DrawString(boleto, fBody7, sb, 40, espacio);
                g.DrawString("Com. Banco", fBody7, sb, 170, espacio);
                g.DrawString( compban, fBody7, sb, 230, espacio);
                //g.DrawString("Boletos:", fBody7, sb, 0, espacio);
                //g.DrawString(boleto, fBody7, sb, 40, espacio);

                espacio = espacio + 20;

                g.DrawString("Aportación", fBody7, sb, 170, espacio);
                g.DrawString(aportacion, fBody7, sb, 230, espacio);
                //espacio = espacio + 20;
                //g.DrawString("Diesel", fBody7, sb, 170, espacio);
                //g.DrawString("$" + diesel, fBody7, sb, 230, espacio);
                //espacio = espacio + 20;
                //g.DrawString("Casetas", fBody7, sb, 170, espacio);
                //g.DrawString("$" + caseta, fBody7, sb, 230, espacio);
                //espacio = espacio + 20;
                espacio = espacio + 20;
                g.DrawString("T. turno", fBody7, sb, 170, espacio);
                g.DrawString(tturno, fBody7, sb, 230, espacio);
                espacio = espacio + 20;
                g.DrawString("T. paso", fBody7, sb, 170, espacio);
                g.DrawString(ttpaso, fBody7, sb, 230, espacio);
                espacio = espacio + 20;
                g.DrawString("T. Salida", fBody7, sb, 170, espacio);
                g.DrawString(tsalida, fBody7, sb, 230, espacio);
                espacio = espacio + 20;
                g.DrawString("IVA", fBody7, sb, 170, espacio);
                g.DrawString(iva, fBody7, sb, 230, espacio);
                //espacio = espacio + 20;
                //g.DrawString("V.SEDENA", fBody7, sb, 170, espacio);
                //g.DrawString("$" + vseden, fBody7, sb, 230, espacio);
                espacio = espacio + 20;
                g.DrawString("Anticipo", fBody7, sb, 170, espacio);
                g.DrawString(anticipo, fBody7, sb, 230, espacio);
                espacio = espacio + 20;
                g.DrawString("Total", fBody7, sb, 170, espacio);
                g.DrawString(total, fBody7, sb, 230, espacio);
                espacio = espacio + 10;

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

        void llenarticketcontinuacion2(object sender, PrintPageEventArgs e)
        {
            try
            {

                Graphics g = e.Graphics;
                // g.DrawRectangle(Pens.Black, 3, 5, 340, 700);
                // g.DrawImage(imagensplash, 10, 0);
                Font fBody7 = new Font("Lucida", 7, FontStyle.Bold);
                Font fBody5 = new Font("Lucida", 5, FontStyle.Bold);

                Font fBody9 = new Font("Lucida", 9, FontStyle.Bold);
                Font fBody = new Font("Lucida", 8, FontStyle.Bold);
                Font fBody10 = new Font("Lucida", 10, FontStyle.Bold);
                Font fBody12 = new Font("Lucida", 12, FontStyle.Bold);
                Font fBody18 = new Font("Lucida", 18, FontStyle.Bold);
                Color customColor = Color.FromArgb(255, Color.Black);
                SolidBrush sb = new SolidBrush(customColor);

                espacio = 0;
                g.DrawString("Detalles", fBody10, sb, 0, espacio);
                espacio = espacio + 25;

                g.DrawString("Importe", fBody7, sb, 170, espacio);
                g.DrawString(importe, fBody7, sb, 230, espacio);
                g.DrawImage(imagen, 30, espacio);
                espacio = espacio + 20;

                // g.DrawString("Salida", fBody7, sb, 170, espacio);
                // g.DrawString("$" + dsalida, fBody7, sb, 230, espacio);
                //espacio = espacio + 20;
                //g.DrawString("Com. Taq", fBody7, sb, 170, espacio);
                //g.DrawString("$" + comtaq, fBody7, sb, 230, espacio);
                //g.DrawString("Boletos:", fBody7, sb, 0, espacio);
                //g.DrawString(boleto, fBody7, sb, 40, espacio);
                g.DrawString("Com. Banco", fBody7, sb, 170, espacio);
                g.DrawString("$" + compban, fBody7, sb, 230, espacio);
                //g.DrawString("Boletos:", fBody7, sb, 0, espacio);
                //g.DrawString(boleto, fBody7, sb, 40, espacio);

                espacio = espacio + 20;

                g.DrawString("Aportación", fBody7, sb, 170, espacio);
                g.DrawString(aportacion, fBody7, sb, 230, espacio);
                //espacio = espacio + 20;
                //g.DrawString("Diesel", fBody7, sb, 170, espacio);
                //g.DrawString("$" + diesel, fBody7, sb, 230, espacio);
                //espacio = espacio + 20;
                //g.DrawString("Casetas", fBody7, sb, 170, espacio);
                //g.DrawString("$" + caseta, fBody7, sb, 230, espacio);
                //espacio = espacio + 20;
                g.DrawString("T. turno", fBody7, sb, 170, espacio);
                g.DrawString(tturno, fBody7, sb, 230, espacio);
                espacio = espacio + 20;
                g.DrawString("T. paso", fBody7, sb, 170, espacio);
                g.DrawString(ttpaso, fBody7, sb, 230, espacio);
                espacio = espacio + 20;
                g.DrawString("T. Salida", fBody7, sb, 170, espacio);
                g.DrawString(tsalida, fBody7, sb, 230, espacio);
                espacio = espacio + 20;
                g.DrawString("IVA", fBody7, sb, 170, espacio);
                g.DrawString(iva, fBody7, sb, 230, espacio);
                //espacio = espacio + 20;
                //g.DrawString("V.SEDENA", fBody7, sb, 170, espacio);
                //g.DrawString("$" + vseden, fBody7, sb, 230, espacio);
                espacio = espacio + 20;
                g.DrawString("Anticipo", fBody7, sb, 170, espacio);
                g.DrawString(anticipo, fBody7, sb, 230, espacio);
                espacio = espacio + 20;
                g.DrawString("Total", fBody7, sb, 170, espacio);
                g.DrawString(total, fBody7, sb, 230, espacio);
                espacio = espacio + 10;

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

     

        private void imprimirr()
        {
            try
                {

                pd = new PrintDocument();
                pd2 = new PrintDocument();
                pdc = new PrintDocument();
                pdc2 = new PrintDocument();
                tamaño = 50 + (int.Parse(boleto) * 20);
                pd.DefaultPageSettings.PaperSize = new PaperSize("Custom", 315, tamaño);
                pd.DefaultPageSettings.PaperSize.RawKind = 119;
                pd.PrinterSettings.DefaultPageSettings.PaperSize.RawKind = 119;
                pd.DefaultPageSettings.PrinterSettings.PrintRange = PrintRange.AllPages;

                pd.DefaultPageSettings.Landscape = false;


                pd.PrintPage += new PrintPageEventHandler(llenarticket);


                pd.PrinterSettings.PrinterName = Settings1.Default.impresora;


                pd.Print();
               
                if (tick == false && tick2 == false)
                {
                    CrearTicket ticket0 = new CrearTicket();
                    ticket0.TextoIzquierda("");
                    ticket0.TextoIzquierda("");
                    ticket0.TextoIzquierda("");
                    ticket0.TextoIzquierda("");
                    ticket0.CortaTicket();
                    ticket0.ImprimirTicket(Settings1.Default.impresora);
                }
                if (tick == true)
                {
                    pdc.Print();
                    CrearTicket ticket1 = new CrearTicket();
                    ticket1.TextoIzquierda("");
                    ticket1.TextoIzquierda("");
                    ticket1.TextoIzquierda("");
                    ticket1.TextoIzquierda("");
                    ticket1.CortaTicket();
                    ticket1.ImprimirTicket(Settings1.Default.impresora);
                }
                if (tick2 == true)
                {
                    pdc2.Print();
                    CrearTicket ticket1 = new CrearTicket();
                    ticket1.TextoIzquierda("");
                    ticket1.TextoIzquierda("");
                    ticket1.TextoIzquierda("");
                    ticket1.TextoIzquierda("");
                    ticket1.CortaTicket();
                    ticket1.ImprimirTicket(Settings1.Default.impresora);
                }
       

                Utilerias.LOG.acciones("reimprimio guia " + folio);

                asiento.Clear();
                pasajero.Clear();
                destinopasajero.Clear();
                foliopasajero.Clear();
                tarifapasajero.Clear();
                preciopasajero.Clear();
            }

            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Seleccione de nuevo una impresora.");
                string funcion = "imprimir";
                Utilerias.LOG.write(_clase, funcion, error);


            }

        }
        private void llenardatosdeboletosvista()
        {
            try
            {
                string resultado = fecha.Substring(0, 10);
                string sql = "SELECT * FROM VENDIDOS WHERE PKGUIA=@PKGUIA AND FECHA=@FECHA";


                db.PreparedSQL(sql);

                db.command.Parameters.AddWithValue("@PKGUIA", pk_guia);
                db.command.Parameters.AddWithValue("@FECHA", resultado);

                res = db.getTable();

                while (res.Next())
                {
                    n = datagridviewbol.Rows.Add();

                    datagridviewbol.Rows[n].Cells[0].Value = res.Get("ASIENTO");
                    datagridviewbol.Rows[n].Cells[1].Value = res.Get("PASAJERO");
                    datagridviewbol.Rows[n].Cells[2].Value = res.Get("DESTINOBOLETO");
                    datagridviewbol.Rows[n].Cells[3].Value = res.Get("FOLIO");
                    string tarifatempo = res.Get("TARIFA");
                    tarifatempo += "         ";
                    datagridviewbol.Rows[n].Cells[4].Value = tarifatempo.Substring(0,8);
                    datagridviewbol.Rows[n].Cells[5].Value = res.Get("PRECIO");
                    asiento.Add(res.Get("ASIENTO"));
                    pasajero.Add(res.Get("PASAJERO"));
                    destinopasajero.Add(res.Get("DESTINO"));
                    foliopasajero.Add(res.Get("FOLIO"));
                    tarifapasajero.Add(tarifatempo.Substring(0, 8));
                    preciopasajero.Add(res.Get("PRECIO"));
                }

            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "llenardatosdeboletos";
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
                MessageBox.Show("Ocurrio un Error con qr, intente de nuevo.");
                string funcion = "codigoqr";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }

     

        private void Buttonimp_Click(object sender, EventArgs e)
        {
            imprimirr();

        }

        private void Detallguia_Load(object sender, EventArgs e)
        {

        }

        private void Detallguia_Shown(object sender, EventArgs e)
        {
            db = new database();

            //pk_guia = pkguia;
            act();
           valores();

            llenardatosdeboletosvista();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

      
        private void BtnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

        }

        private void BtnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
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

    }


}
