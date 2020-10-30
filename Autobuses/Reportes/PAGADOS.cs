using Autobuses.Utilerias;
using ConnectDB;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using iTextSharp.text;
using iTextSharp.text.pdf;
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
using Font = iTextSharp.text.Font;

namespace Autobuses.Reportes
{
    public partial class PAGADOS : Form
    {
        public database db;
        ResultSet res = null;
        Bitmap imagen;
        private string _clase="pagados";
        private string fechainicio;
        private string fechatermino;
        private string _socio;
        private string foliotext;
        private string sucursaltext;
        private string importetext = "0";
        private string gastostext = "0";
        private string tarjetastext = "0";
        private string ivatext = "0";
        private string aportaciontext = "0";
        private string compbantext = "0";
        private string totaltext = "0";
        private string pagadooactivo = "";
        private string pk_guia;
        private string nombredetermindo="";
        private string _socioseleccionado;
        private string cantidadboletosguia;
        private int espacio;
        private string folio;
        private string linea;

        private string usuariopagador;

        private string folioguia;
        private double importeguia;
        private double gastosguia;
        private double compbanguia;
        private double aportacionguia;

        private double tarjetasguia;
        private double ivaguia;
        private double totalguia;
        private string pkguia;
        private string status;

        private List<string> asiento = new List<string>();
        private List<string> pasajero = new List<string>();
        private List<string> destinopasajero = new List<string>();
        private List<string> foliopasajero = new List<string>();
        private List<string> tarifapasajero = new List<string>();
        private List<string> preciopasajero = new List<string>();

        private List<string> compbang = new List<string>();
        private List<string> aportaciong = new List<string>();
        private int cantidadfolios = 0;
        private double imported = 0.0;
        private double gastosd = 0.0;
        private double tarjetasd = 0.0;
        private double ivad = 0.0;
        private double totald = 0.0;




        public PAGADOS()
        {
            InitializeComponent();
            this.Show();
            reimprimir.Enabled = false;


        }
        public void getDatosAdicionalessocios()
        {
            try
            {
                combosocio.Items.Clear();

                string sql = "SELECT PK,NOMBRE,APELLIDOS FROM SOCIOS WHERE BORRADO=0 ORDER BY NOMBRE";
                db.PreparedSQL(sql);
                res = db.getTable();

                while (res.Next())
                {
                    ComboboxItem item = new ComboboxItem();
                    item.Text = res.Get("NOMBRE") + " " + res.Get("APELLIDOS");
                    item.Value = res.Get("PK");
                    combosocio.Items.Add(item);

                }
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "getDatosAdicionalesSOCIOS";
                Utilerias.LOG.write(_clase, funcion, error);


            }

        }

        private void buscar(object sender, EventArgs e)
        {
            try
            {
                dataGridViewguias.Rows.Clear();
                _socio = "";
            


                if ((combosocio.SelectedItem) != null)
                {
                    _socio = combosocio.SelectedItem.ToString();
                    getRows( _socio, fechainicio);

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

        public void getRows(string socio = "", string ini = "")
        {
            try
            {
                string pksocio = (string)(combosocio.SelectedItem as ComboboxItem).Value;

                int count = 1;
                string sql = "SELECT * FROM GUIASPAGADAS WHERE PKSOCIO=@SOCIO AND FECHA=@FECHA";
                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@SOCIO", pksocio);
                db.command.Parameters.AddWithValue("@FECHA", ini);


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

                    double  compban = (double.TryParse(res.Get("COMPBAN"), out double aux8)) ? res.GetDouble("COMPBAN") : 0.0;
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
        private void DateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                fechainicio = dateTimePicker1.Value.ToString("dd/MM/yyyy");
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "datetimerpicker1";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }

        private void DataGridViewguias_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int n = e.RowIndex;

                if (n != -1)
                {


                    int cantidad = dataGridViewguias.Rows.Count;
               
                    bool pagarsepuede = false;
                    pkguia = (string)dataGridViewguias.Rows[n].Cells["pkname"].Value;
                    sucursaltext = (string)dataGridViewguias.Rows[n].Cells["sucursalname"].Value;
                    foliotext= (string)dataGridViewguias.Rows[n].Cells["folioname"].Value;
                    linea = (string)dataGridViewguias.Rows[n].Cells["lineaname"].Value;
                    usuariopagador = (string)dataGridViewguias.Rows[n].Cells["usuarioname"].Value;
                    nombredetermindo = (string)dataGridViewguias.Rows[n].Cells["cobradoname"].Value;

                    _socioseleccionado = (string) dataGridViewguias.Rows[n].Cells["socioname"].Value ;
                           cantidadboletosguia= (string)dataGridViewguias.Rows[n].Cells["cantidaddeguiasname"].Value;         
                            importetext = (dataGridViewguias.Rows[n].Cells["importename"].Value.ToString());
                            gastostext = (dataGridViewguias.Rows[n].Cells["gastosname"].Value.ToString());
                            tarjetastext = (dataGridViewguias.Rows[n].Cells["tarjetaname"].Value.ToString());
                            ivatext = (dataGridViewguias.Rows[n].Cells["ivaname"].Value.ToString());
                            totaltext =(dataGridViewguias.Rows[n].Cells["totalname"].Value.ToString());
                    aportaciontext = (dataGridViewguias.Rows[n].Cells["aportacionesname"].Value.ToString());
                    compbantext = (dataGridViewguias.Rows[n].Cells["compbanname"].Value.ToString());


                    textBoximport.Text = importetext;
                    textBoxgastos.Text =  gastostext;
                    textBoxtarjetas.Text =  tarjetastext;
                    textBoxiva.Text =  ivatext;
                    textBoxtotal.Text = totaltext;
                   textBoxaportacion.Text = aportaciontext;
                    textBoxcompban.Text = compbantext;
                    textBoxguias.Text = cantidad.ToString();
                    reimprimir.Enabled = true;
                }


            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "datagridviewguia";
                Utilerias.LOG.write(_clase, funcion, error);


            }

        }
        private void limp()
        {
            textBoxtarjetas.Text = "";
            textBoximport.Text = "";
            textBoxiva.Text = "";
            textBoxtotal.Text = "";
            textBoxguias.Text = "";
            textBoxgastos.Text = "";
            reimprimir.Enabled = false;

            dataGridViewguias.Rows.Clear();
        }
        private void Limpiar_Click(object sender, EventArgs e)
        {
            limp();
        }

        private void PAGADOS_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.Dock = DockStyle.Fill;
        }

        private void PAGADOS_Shown(object sender, EventArgs e)
        {
            db = new database();

            combosocio.DropDownStyle = ComboBoxStyle.DropDownList;

            dataGridViewguias.EnableHeadersVisualStyles = false;
            fechainicio = DateTime.Now.ToString("dd/MM/yyyy");
            fechatermino = DateTime.Now.ToString("dd/MM/yyyy");
            getDatosAdicionalessocios();
            //Estas lineas eliminan los parpadeos del formulario o controles en la interfaz grafica (Pero no en un 100%)
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.DoubleBuffered = true;
            titulo.Text = "Reportes de Pago de Guias";
            labelnombre.Text = LoginInfo.NombreID;
            labelapellido.Text = LoginInfo.ApellidoID;
            MemoryStream ms = new MemoryStream((byte[])LoginInfo.imagenfoto);
            Bitmap bmp = new Bitmap(ms);
            labelcargo.Text = LoginInfo.rol;
            pictureBoxfoto.Image = bmp;
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
                codigoqr(foliotext);

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
                g.DrawString("Pago de Guias", fBody18, sb, 50, espacio);
                espacio = espacio + 30;
                g.DrawString("SUCURSAL: " + sucursaltext, fBody10, sb, 0, espacio);
                espacio = espacio + 20;

                g.DrawString("Linea: " + linea, fBody10, sb, 0, espacio);

                espacio = espacio + 20;

                g.DrawString("Folio: " + foliotext, fBody10, sb, 2, espacio);
                g.DrawString("Guias: " + cantidadfolios.ToString(), fBody10, sb, 150, espacio);


                espacio = espacio + 20;
                g.DrawString("Usuario: " + usuariopagador, fBody10, sb, 0, espacio);
                espacio = espacio + 20;

                g.DrawString("Socio: " + _socio, fBody10, sb, 0, espacio);
                espacio = espacio + 20;
                g.DrawString("Quién cobro: " + nombredetermindo, fBody10, sb, 0, espacio);
                espacio = espacio + 20;
                g.DrawString("Rango de fechas: ", fBody10, sb, 70, espacio);
                espacio = espacio + 20;
                g.DrawString("Inicio: " + fechainicio, fBody, sb, 0, espacio);
                g.DrawString("Termino: " + fechatermino, fBody, sb, 140, espacio);
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

                for (int i = 0; i < asiento.Count(); i++)
                {


                    g.DrawString(asiento[i], fBody5, sb, 0, espacio);
                    g.DrawString( pasajero[i], fBody5, sb, 45, espacio);
                    g.DrawString(destinopasajero[i], fBody5, sb, 85, espacio);
                    g.DrawString(foliopasajero[i], fBody5, sb, 130, espacio);
                    g.DrawString( tarifapasajero[i], fBody5, sb, 185, espacio);
                    g.DrawString( preciopasajero[i], fBody5, sb, 225, espacio);

                    espacio = espacio + 20;
                }

                espacio = espacio + 10;
                g.DrawString("--------------------------------------------------------------------------------------", fBody7, sb, 0, espacio - 20);
                espacio = espacio + 10;
                g.DrawImage(imagen, 20, espacio);

                g.DrawString("Importe: "+  importetext, fBody7, sb, 170, espacio);
                espacio = espacio + 20;
                g.DrawString("Gastos: " +gastostext, fBody7, sb, 170, espacio);
                espacio = espacio + 20;
                g.DrawString("Tarjetas: " + tarjetastext, fBody7, sb, 170, espacio);
                espacio = espacio + 20;
                g.DrawString("Aportaciónes: " + aportaciontext, fBody7, sb, 170, espacio);
                espacio = espacio + 20;
                g.DrawString("Comp. Banco: " + compbantext, fBody7, sb, 170, espacio);
                espacio = espacio + 20;
                g.DrawString("IVA: " + ivatext, fBody7, sb, 170, espacio);
                espacio = espacio + 20;
                g.DrawString("Total: " +  totaltext, fBody7, sb, 170, espacio);

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

                int tamaño = 780 + ((cantidadfolios) * 20);
                PrintDocument pd = new PrintDocument();
                PaperSize ps = new PaperSize("", 420, tamaño);
                pd.PrintPage += new PrintPageEventHandler(llenarticket);

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
               
                Utilerias.LOG.acciones("imprimir pago guia " + foliotext);

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
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "imprimir";
                Utilerias.LOG.write(_clase, funcion, error);


            }

        }


        private void guiasobtener()
        {
            string sql = "SELECT FOLIO,IMPORTE,ANTICIPO,TSALIDA,TTURNO,TPASO,IVA,TOTAL,APORTACION,COMPBAN FROM GUIA WHERE PKGUIAPAGADA=@PKGUIAPAGADA";

            db.PreparedSQL(sql);

            db.command.Parameters.AddWithValue("@PKGUIAPAGADA", pkguia);
            res = db.getTable();
            cantidadfolios = 0;
            double tar = 0.0;
            while (res.Next())
            {
                folioguia = res.Get("FOLIO");
                importeguia = (double.TryParse(res.Get("IMPORTE"), out double aux1)) ? res.GetDouble("IMPORTE") : 0.0;
                gastosguia = (double.TryParse(res.Get("ANTICIPO"), out double aux2)) ? res.GetDouble("ANTICIPO") : 0.0;
                aportacionguia = (double.TryParse(res.Get("APORTACION"), out double aux3)) ? res.GetDouble("APORTACION") : 0.0;
                compbanguia = (double.TryParse(res.Get("COMPBAN"), out double aux4)) ? res.GetDouble("COMPBAN") : 0.0;


                tar += (double.TryParse(res.Get("TSALIDA"), out double aux11)) ? res.GetDouble("TSALIDA") : 0.0;
                tar += (double.TryParse(res.Get("TTURNO"), out double aux21)) ? res.GetDouble("TTURNO") : 0.0;
                tar += (double.TryParse(res.Get("TPASO"), out double aux13)) ? res.GetDouble("TPASO") : 0.0;
                ivaguia = (double.TryParse(res.Get("IVA"), out double aux5)) ? res.GetDouble("IVA") : 0.0;
                totalguia = (double.TryParse(res.Get("TOTAL"), out double aux6)) ? res.GetDouble("TOTAL") : 0.0;

                asiento.Add( folioguia);
                pasajero.Add(Utilerias.Utilerias.formatCurrency(importeguia));
                destinopasajero.Add(Utilerias.Utilerias.formatCurrency(gastosguia));
                foliopasajero.Add(Utilerias.Utilerias.formatCurrency( tar));
                tarifapasajero.Add(Utilerias.Utilerias.formatCurrency(ivaguia));
                compbang.Add(Utilerias.Utilerias.formatCurrency(compbanguia));
                aportaciong.Add(Utilerias.Utilerias.formatCurrency(aportacionguia));
                preciopasajero.Add(Utilerias.Utilerias.formatCurrency(totalguia));
                cantidadfolios++;

            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            guiasobtener();
            pago();

            imprimirr();
            reimprimir.Enabled = false;
        }

        private void pago()
        {
            try
            {
                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, false);
                Font font = new Font(bfTimes, 12, 2, BaseColor.BLACK);
                Font font2 = new Font(bfTimes, 9, 2, BaseColor.BLACK);

                Font arial = FontFactory.GetFont("Arial", 12,BaseColor.BLACK);
                Font arial2 = FontFactory.GetFont("Arial", 9, BaseColor.BLACK);


                iTextSharp.text.Document doc = new iTextSharp.text.Document();
                string rutatemporal = Path.GetTempPath();
                PdfWriter title = PdfWriter.GetInstance(doc, new FileStream(rutatemporal + "/PagoGuiacopia.pdf", FileMode.Create));
                doc.Open();
              Bitmap imagensplash = new Bitmap(Autobuses.Properties.Resources.logotickets);

        iTextSharp.text.Image PatientSign = iTextSharp.text.Image.GetInstance(imagensplash, System.Drawing.Imaging.ImageFormat.Png);
                PatientSign.Alignment = Element.ALIGN_LEFT;
                
                doc.Add(new Paragraph("\n"));



                PdfPTable table = new PdfPTable(3);
                table.HorizontalAlignment = Element.ALIGN_CENTER;
              
                table.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                table.WidthPercentage = 100;

                PdfPCell cell = new PdfPCell();

                cell = new PdfPCell(PatientSign);
                cell.BorderColor = BaseColor.WHITE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                Paragraph p = new Paragraph();
                p.Add(new Chunk("Sucursal: ",font));
                p.Add(new Chunk("\n"));
                p.Add(new Chunk( LoginInfo.Sucursal, arial));

                p.Add(new Chunk("\n"));
                p.Add(new Chunk("\n"));
                p.Add(new Chunk("Linea: ", font));
                p.Add(new Chunk("\n"));
                p.Add(new Chunk(linea, arial));

                cell = new PdfPCell(p);
                cell.BorderColor = BaseColor.WHITE;
                table.AddCell(cell);
                Paragraph d = new Paragraph();
                
                d.Add(new Chunk("Fecha: " , font));
                d.Add(new Chunk(DateTime.Now.ToString("dd/MM/yyyy"), arial));

                d.Add(new Chunk("\n"));
                d.Add(new Chunk("\n"));
                d.Add(new Chunk("Hora: " , font));
                d.Add(new Chunk(DateTime.Now.ToString("HH: mm tt"), arial));

                d.Add(new Chunk("\n"));
                d.Add(new Chunk("\n"));
                d.Add(new Chunk("Folio: " , font));
                d.Add(new Chunk(foliotext, arial));

                d.Alignment = Element.ALIGN_RIGHT;




                cell = new PdfPCell(d);
                cell.BorderColor = BaseColor.WHITE;
                table.AddCell(cell);



                doc.Add(table);

                doc.Add(new Paragraph("\n"));


                PdfPTable table2 = new PdfPTable(3);
                table2.WidthPercentage = 100;
                table2.HorizontalAlignment = Element.ALIGN_CENTER;


                PdfPCell cell2 = new PdfPCell();
                cell2.HorizontalAlignment = Element.ALIGN_CENTER;

                Paragraph a2 = new Paragraph();
                a2.Add(new Chunk("Usuario: ", font));
                a2.Add(new Chunk("\n"));
                a2.Add(new Chunk(LoginInfo.NombreID + " " + LoginInfo.ApellidoID, arial));
                a2.Add(new Chunk("\n"));
                a2.Add(new Chunk("\n"));
                a2.Add(new Chunk("Socio: ", font));
                a2.Add(new Chunk("\n"));
                a2.Add(new Chunk(_socio,arial));
                a2.Add(new Chunk("\n"));
                a2.Add(new Chunk("\n"));
                a2.Add(new Chunk("Quién Cobro: ", font));
                a2.Add(new Chunk("\n"));
                a2.Add(new Chunk(nombredetermindo, arial));
                a2.Add(new Chunk("\n"));
               
                cell2 = new PdfPCell(a2);
                cell2.BorderColor = BaseColor.WHITE;
                table2.AddCell(cell2);
                Paragraph p2 = new Paragraph();
                p2.Add(new Chunk("Cantidad de guias: ", font));
                p2.Add(new Chunk("\n"));
                p2.Add(new Chunk(cantidadfolios.ToString(), arial));
                p2.Add(new Chunk("\n"));
                p2.Add(new Chunk("\n"));
                p2.Add(new Chunk("Rango  de pago:", font));
                p2.Add(new Chunk("\n"));
                p2.Add(new Chunk(fechainicio+"-"+fechatermino, arial));
            




                cell2 = new PdfPCell(p2);
                cell2.BorderColor = BaseColor.WHITE;
                table2.AddCell(cell2);
                Paragraph d2 = new Paragraph();
                BarcodeLib.Barcode Codigobarras = new BarcodeLib.Barcode();
                iTextSharp.text.Image qrr = iTextSharp.text.Image.GetInstance(Codigobarras.Encode(BarcodeLib.TYPE.CODE128, foliotext, Color.Black, Color.White, 170, 30), System.Drawing.Imaging.ImageFormat.Png);
                qrr.Alignment = Element.ALIGN_RIGHT;

                cell2 = new PdfPCell(qrr);
                cell2.BorderColor = BaseColor.WHITE;
                table2.AddCell(cell2);




                doc.Add(table2);

                doc.Add(new Paragraph("\n"));

                PdfPTable table3 = new PdfPTable(8);
                table3.WidthPercentage = 100;
                table3.HorizontalAlignment = Element.ALIGN_CENTER;

                PdfPCell cell3 = new PdfPCell();
                cell3.HorizontalAlignment = Element.ALIGN_CENTER;


                Paragraph folio = new Paragraph();
                Paragraph importe = new Paragraph();
                Paragraph gastos = new Paragraph();
                Paragraph aport = new Paragraph();
                Paragraph com = new Paragraph();

                Paragraph tarjetas = new Paragraph();
                Paragraph iva = new Paragraph();
                Paragraph total = new Paragraph();
                folio.Add(new Chunk("Folio", font2));
                folio.Add(new Chunk("\n", font2));
                importe.Add(new Chunk("Importe", font2));
                importe.Add(new Chunk("\n", font2));
                gastos.Add(new Chunk("Gastos", font2));
                gastos.Add(new Chunk("\n", font2));
                aport.Add(new Chunk("Aportación", font2));
                aport.Add(new Chunk("\n", font2));
                com.Add(new Chunk("Com. Banco", font2));
                com.Add(new Chunk("\n", font2));
                tarjetas.Add(new Chunk("Tarjetas", font2));
                tarjetas.Add(new Chunk("\n", font2));
                iva.Add(new Chunk("Iva", font2));
                iva.Add(new Chunk("\n", font2));
                total.Add(new Chunk("Total", font2));
                total.Add(new Chunk("\n", font2));

                cell3 = new PdfPCell(folio);
                table3.AddCell(cell3);
                cell3 = new PdfPCell(importe);
                table3.AddCell(cell3);
                cell3 = new PdfPCell(gastos);
                table3.AddCell(cell3);
                cell3 = new PdfPCell(aport);
                table3.AddCell(cell3);
                cell3 = new PdfPCell(com);
                table3.AddCell(cell3);
                cell3 = new PdfPCell(tarjetas);
                table3.AddCell(cell3);
                cell3 = new PdfPCell(iva);
                table3.AddCell(cell3);
                cell3 = new PdfPCell(total);
                table3.AddCell(cell3);
                doc.Add(table3);

                PdfPTable table33 = new PdfPTable(8);
                table33.WidthPercentage = 100;
                PdfPCell cell33 = new PdfPCell();

                Paragraph folio3 = new Paragraph();
                Paragraph importe3 = new Paragraph();
                Paragraph gastos3 = new Paragraph();
                Paragraph aport3 = new Paragraph();
                Paragraph com3 = new Paragraph();
                Paragraph tarjetas3 = new Paragraph();
                Paragraph iva3 = new Paragraph();
                Paragraph total3 = new Paragraph();
                for (int i = 0; i < asiento.Count(); i++)
                {

                    folio3.Add(new Chunk(asiento[i], arial2));
                    folio3.Add(new Chunk("\n", arial2));
                    importe3.Add(new Chunk(pasajero[i], arial2));
                    importe3.Add(new Chunk("\n", arial2));
                    gastos3.Add(new Chunk(destinopasajero[i], arial2));
                    gastos3.Add(new Chunk("\n", arial2));
                    aport3.Add(new Chunk(aportaciong[i], arial2));
                    aport3.Add(new Chunk("\n", arial2));
                    com3.Add(new Chunk(compbang[i], arial2));
                    com3.Add(new Chunk("\n", arial2));
                    tarjetas3.Add(new Chunk(foliopasajero[i], arial2));
                    tarjetas3.Add(new Chunk("\n", arial2));
                    iva3.Add(new Chunk(tarifapasajero[i], arial2));
                    iva3.Add(new Chunk("\n", arial2));
                    total3.Add(new Chunk(preciopasajero[i], arial2));
                    total3.Add(new Chunk("\n", arial2));
                }
                cell33 = new PdfPCell(folio3);
                    cell33.BorderColor = BaseColor.WHITE;
                    table33.AddCell(cell33);

                    cell33 = new PdfPCell(importe3);
                    cell33.BorderColor = BaseColor.WHITE;
                    table33.AddCell(cell33);

                    cell33 = new PdfPCell(gastos3);
                    cell33.BorderColor = BaseColor.WHITE;
                    table33.AddCell(cell33);

                    cell33 = new PdfPCell(aport3);
                    cell33.BorderColor = BaseColor.WHITE;
                    table33.AddCell(cell33);

                    cell33 = new PdfPCell(com3);
                    cell33.BorderColor = BaseColor.WHITE;

                    table33.AddCell(cell33);
                    cell33 = new PdfPCell(tarjetas3);
                    cell33.BorderColor = BaseColor.WHITE;
                    table33.AddCell(cell33);

                    cell33 = new PdfPCell(iva3);
                    cell33.BorderColor = BaseColor.WHITE;
                    table33.AddCell(cell33);

                    cell33 = new PdfPCell(total3);
                    cell33.BorderColor = BaseColor.WHITE;
                    table33.AddCell(cell33);
                    cell33 = new PdfPCell();


                
                doc.Add(table33);
                doc.Add(new Paragraph("\n"));
                doc.Add(new Paragraph("\n"));
                PdfPTable table4 = new PdfPTable(6);
                table4.WidthPercentage = 100;

                PdfPCell cell4 = new PdfPCell();
                PdfPCell cell6 = new PdfPCell();
             

                cell4.BorderColor = BaseColor.WHITE;
                cell6.BorderColor = BaseColor.WHITE;

                cell4 = new PdfPCell(new Paragraph("\n"));
                cell4.BorderColor = BaseColor.WHITE;
                table4.AddCell(cell4);
                cell4 = new PdfPCell(new Paragraph("\n"));
                cell4.BorderColor = BaseColor.WHITE;
                table4.AddCell(cell4);
                cell4 = new PdfPCell(new Paragraph("\n"));
                cell4.BorderColor = BaseColor.WHITE;
                table4.AddCell(cell4);
                cell4 = new PdfPCell(new Paragraph("\n"));
                cell4.BorderColor = BaseColor.WHITE;
                table4.AddCell(cell4);

                Paragraph d4 = new Paragraph();
                Paragraph d5 = new Paragraph();
                d4.Add(new Chunk("Importe: ",arial));
                d5.Add(new Chunk( importetext, arial));
                d4.Add(new Chunk("\n"));
                d5.Add(new Chunk("\n"));
                d4.Add(new Chunk("Gastos: " ,arial));
                d5.Add(new Chunk(gastostext, arial));
                d4.Add(new Chunk("\n"));
                d5.Add(new Chunk("\n"));
                d4.Add(new Chunk("Aportaciónes:" ,arial));
                d5.Add(new Chunk(aportaciontext, arial));
                d4.Add(new Chunk("\n"));
                d5.Add(new Chunk("\n"));
                d4.Add(new Chunk("Comp. Banco:",arial));
                d5.Add(new Chunk(compbantext, arial));
                d4.Add(new Chunk("\n"));
                d5.Add(new Chunk("\n"));
                d4.Add(new Chunk("Tarjetas:",arial));
                d5.Add(new Chunk(tarjetastext, arial));
                d4.Add(new Chunk("\n"));
                d5.Add(new Chunk("\n"));
                d4.Add(new Chunk("Iva:" ,arial));
                d5.Add(new Chunk(ivatext, arial));
                d4.Add(new Chunk("\n"));
                d5.Add(new Chunk("\n"));
                d4.Add(new Chunk("Total:" ,arial));
                d5.Add(new Chunk(totaltext, arial));
                d4.Add(new Chunk("\n"));
                d5.Add(new Chunk("\n"));


                cell4 = new PdfPCell(d4);
                cell4.BorderColor = BaseColor.WHITE;
                table4.AddCell(cell4);

                cell6 = new PdfPCell(d5);
                cell6.BorderColor = BaseColor.WHITE;
                table4.AddCell(cell6);

                doc.Add(table4);
                doc.Add(new Paragraph("\n"));

               float bo= doc.Bottom;
                
              Paragraph parrafo = new Paragraph("El presente documento representa el pago de las guias mencionadas, el cual, debera ser firmado por la persona quién recibe el pago, aceptando a los términos y condiciones de ATAH.",arial);
                parrafo.SpacingBefore =120;
                doc.Add(parrafo);

                doc.Add(new Paragraph("\n"));
                doc.Add(new Paragraph("\n"));
                doc.Add(new Paragraph("_____________________________________",arial));
                doc.Add(new Paragraph("         Nombre y firma",arial));
                doc.Add(new Paragraph("\n"));
                doc.Add(new Paragraph("\n"));
                doc.AddTitle("Pago de Guia");
          
                doc.Close();

                MemoryStream ms = new MemoryStream();
                PdfWriter writerPdf = PdfWriter.GetInstance(doc, ms);
                byte[] bytes = ms.ToArray();
                System.Diagnostics.Process.Start(rutatemporal + "/PagoGuiacopia.pdf");





            }
            catch (Exception err)
            {
                string error = err.Message;
                string funcion = "Pago";
                Utilerias.LOG.write(_clase, funcion, error);

                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
            }

        }

        private void ToolStripSessionClose_Click(object sender, EventArgs e)
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

        private void ToolStripCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ToolStripExportExcel_Click(object sender, EventArgs e)
        {
            Utilerias.Utilerias.ExportarDataGridViewExcel(dataGridViewguias);

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

        private void Button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {

            lblHora.Text = DateTime.Now.ToString("hh:mm:ss");
            lbFecha.Text = DateTime.Now.ToString("D");
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
