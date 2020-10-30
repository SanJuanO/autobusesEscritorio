using Autobuses.Reportes;
using Autobuses.Utilerias;
    using ConnectDB;
    using Gma.QrCodeNet.Encoding;
    using Gma.QrCodeNet.Encoding.Windows.Render;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Json.Net;
using MyAttendance;
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
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;
using Font = System.Drawing.Font;

namespace Autobuses.Planeacion
{
    public partial class pagoguias : Form
    {
        public database db;
        ResultSet res = null;
        Bitmap imagen;
        private string _clase = "guiapagos";
        private int n = 0;
        private string dia;
        private string folio;
        private string folioguia;
        private double importeguia;
        private string sociousuario;
        private double gastosguia;
        private string tarjetasguia;
        private double ivaguia;
        private double totalguia;
        private string pkguia;
        private string pksocio;
        private string status;
        private bool contrausuario=false;
        private bool contrasocio = false;
        private double totaldispo = 0.0;
        private int cantidaddispo = 0;
        private int contando1 = 0;
        private int contando2 = 0;
        private string contraseña = "";
        private double totaldetalle = 0.0;
        private double tarjetas = 0;
        private int cantidaddetalle = 0;
        private string origen;
        private string destino;
        private string fecha;
        private string autobus;
        private bool cualquierhuella = false;
        private string boleto;
        private double importe;
        private double dsalida;
        private double comtaq;
        private double compban;
        private string nombredetermindo;
        private double aportacion;
        private double diesel;
        private double caseta;
        private double tarjeta;
        private double vseden;
        private double iva;
        private string socior;
        private double gastos;

        private double total;
        private string foliobuscar = "";
        private string _linea;
        private string sucursal;
        private string validador;
        private int espacio = 220;
        private string chofer;
        private string linea;
        private int tamaño;
        private string hora;
        private int nombreid = 0;
        private List<string> asiento = new List<string>();
        private List<string> pasajero = new List<string>();
        private List<string> destinopasajero = new List<string>();
        private List<string> foliopasajero = new List<string>();
        private List<string> tarifapasajero = new List<string>();
        private List<string> preciopasajero = new List<string>();
        private List<string> compbang = new List<string>();
        private List<string> aportaciong = new List<string>();

        private List<string> nombre = new List<string>();
        private string fechainicio;
        private string fechatermino;
        private string sucursalbusqueda = LoginInfo.Sucursal;
        private string recibedinero;
        private string fechaseleccionada;
        private string _socio;
        private string _autobus;
        private string _status;
        private string _conductor;
        private double importetext = 0.0;
        private double gastostext = 0.0;
        private double tarjetastext = 0.0;
        private double ivatext = 0.0;
        private double totaltext = 0.0;
        private string pagadooactivo = "";
        private string pk_guia;

        private int cantidadfolios = 0;
        private double imported = 0.0;
        private double gastosd = 0.0;
        private double tarjetasd = 0.0;
        private double ivad = 0.0;
        private double totald = 0.0;
        private double compbancotext = 0.0;
        private double aportaciontext = 0.0;
        private int dias = 0;
        private string fechadispo;

        byte[] fingerPrint;
        

        List<string> usuariochofer = new List<string>();
        private List<string> foliosporpagar = new List<string>();
        private List<string> pkporpagar = new List<string>();

        public PrinterSettings PrinterSettings { get; private set; }

        public pagoguias()
        {

            InitializeComponent();
            verificationUserControl1.Hide();
            verificationUserControl2.Hide();
            labelusu.Visible = false;
            labelsocio.Visible = false;

            this.Show();

        }



        private void permisos()
        {
            if (LoginInfo.privilegios.Any(x => x == "Generar cualquier pago"))
            {
                cualquierhuella = true;

            }

        }


        private void DataGridViewguias_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try {
                int n = e.RowIndex;

                if (n != -1)
                {

                    pk_guia = (string)dataGridViewguias.Rows[n].Cells[28].Value;
                    iradetalle();
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
        private void iradetalle()
        {
            try
            {


                Form form = null;
                if (CheckOpened("Detalleguia"))
                {
                    form.StartPosition = FormStartPosition.CenterScreen;
                    form.Show();

                }
                else
                {

                    form = new Detallguia(pk_guia);
                    AddOwnedForm(form);
                    form.StartPosition = FormStartPosition.CenterScreen;
                    form.Show();
                    Utilerias.LOG.acciones("ingreso a detalleguia ");


                }




            }
            catch (Exception err)
            {
                string error = err.Message;
                // MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "guiaclick";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }




        public void getDatosAdicionaleslinea()
        {
            try
            {
                comboBoxlinea.Items.Clear();

                string sql = "SELECT LINEA,PK1 FROM LINEAS WHERE BORRADO=0";
                db.PreparedSQL(sql);
                res = db.getTable();

                while (res.Next())
                {
                    ComboboxItem item = new ComboboxItem();
                    item.Text = res.Get("LINEA");
                    item.Value = res.Get("PK1");

                    comboBoxlinea.Items.Add(item);

                }
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "getDatosAdicionales";
                Utilerias.LOG.write(_clase, funcion, error);


            }

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
                    item.Text = res.Get("NOMBRE") +" "+ res.Get("APELLIDOS");
                    item.Value = res.Get("PK");
                    combosocio.Items.Add(item);

                }
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "getDatosAdicionales";
                Utilerias.LOG.write(_clase, funcion, error);


            }

        }

        private void DateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                fechatermino = dateTimePicker2.Value.ToString("yyyy-MM-dd");
                if (textBoxinicio.Text != "")
                {
                    dataGridViewguias.Rows.Clear();
                    obtenerguias();
                }
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "datetimepicker2";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }
        private void buscar(object sender, EventArgs e)
        {
            try
            {
                dataGridViewguias.Rows.Clear();
                obtenerguias();
                if (textBoxinicio.Text == "")
                {
                    button1.Enabled = false;
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




        private void obtenerdetalledeboletos()
        {
            try
            {

                string sql = "SELECT * FROM VENDIDOS WHERE PKGUIA=@PKGUIA";


                db.PreparedSQL(sql);

                db.command.Parameters.AddWithValue("@PKGUIA", pk_guia);


                int n = 0;

                res = db.getTable();

                while (res.Next())
                {

                    asiento.Add(res.Get("ASIENTO"));
                    pasajero.Add(res.Get("PASAJERO"));
                    destinopasajero.Add(res.Get("DESTINO"));
                    foliopasajero.Add(res.Get("FOLIO"));
                    tarifapasajero.Add(res.Get("TARIFA"));
                    preciopasajero.Add(res.Get("PRECIO"));

                }
                if (db.execute())
                {
                }

            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "obtenerdetalledeboletos";
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
                Font fBody10 = new Font("Lucida", 10, FontStyle.Bold);
                Font fBody12 = new Font("Lucida", 12, FontStyle.Bold);
                Font fBody18 = new Font("Lucida", 18, FontStyle.Bold);

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
                g.DrawString("SUCURSAL: " + LoginInfo.Sucursal, fBody10, sb, 0, espacio);
                espacio = espacio + 20;
                g.DrawString("Linea: " + _linea, fBody10, sb, 0, espacio);

                espacio = espacio + 20;

                g.DrawString("Folio: " + folio, fBody10, sb, 2, espacio);
                g.DrawString("Guias: " + cantidadfolios.ToString(), fBody10, sb, 150, espacio);


                espacio = espacio + 20;
                g.DrawString("Usuario: " + LoginInfo.NombreID+" "+LoginInfo.ApellidoID, fBody10, sb, 0, espacio);
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
                espacio = espacio + 20;

                for (int i = 0; i < asiento.Count(); i++)
                {


                    g.DrawString(asiento[i], fBody5, sb, 0, espacio);
                    g.DrawString(pasajero[i], fBody5, sb, 45, espacio);
                    g.DrawString(destinopasajero[i], fBody5, sb, 85, espacio);
                    g.DrawString(foliopasajero[i], fBody5, sb, 130, espacio);
                    g.DrawString(tarifapasajero[i], fBody5, sb, 185, espacio);
                    g.DrawString(preciopasajero[i], fBody5, sb, 225, espacio);

                    espacio = espacio + 20;
                }

                espacio = espacio + 10;
                g.DrawString("--------------------------------------------------------------------------------------", fBody7, sb, 0, espacio - 20);
                espacio = espacio + 10;
                g.DrawImage(imagen, 20, espacio);

                g.DrawString("Importe: " +Utilerias.Utilerias.formatCurrency( importetext), fBody7, sb, 170, espacio);
                espacio = espacio + 20;
                g.DrawString("Gastos: "  + Utilerias.Utilerias.formatCurrency(gastostext), fBody7, sb, 170, espacio);
                espacio = espacio + 20;
                g.DrawString("Tarjetas: "  + Utilerias.Utilerias.formatCurrency(tarjetastext), fBody7, sb, 170, espacio);
                espacio = espacio + 20;
                g.DrawString("Aportaciones: " + Utilerias.Utilerias.formatCurrency(aportaciontext), fBody7, sb, 170, espacio);
                espacio = espacio + 20;
                g.DrawString("Comp. Banco: " + Utilerias.Utilerias.formatCurrency(compbancotext), fBody7, sb, 170, espacio);
                espacio = espacio + 20;
                g.DrawString("IVA: " + Utilerias.Utilerias.formatCurrency(ivatext), fBody7, sb, 170, espacio);
                espacio = espacio + 20;
                g.DrawString("Total: " + Utilerias.Utilerias.formatCurrency(totaltext), fBody7, sb, 170, espacio);
                

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
            
                tamaño = 780 + ((cantidadfolios) * 20);
                PrintDocument pd = new PrintDocument();
                PaperSize ps = new PaperSize("", 420, tamaño);
                destino = folio;
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
                pd.Print();
                 ticket0 = new CrearTicket();
                ticket0.TextoIzquierda("");
                ticket0.TextoIzquierda("");
                ticket0.TextoIzquierda("");
                ticket0.TextoIzquierda("");
                ticket0.CortaTicket();
                ticket0.ImprimirTicket(Settings1.Default.impresora);
                pago();
                Utilerias.LOG.acciones("imprimir pago guia " + folio);
               
                asiento.Clear();
                pasajero.Clear();
                destinopasajero.Clear();
                foliopasajero.Clear();
                tarifapasajero.Clear();
                preciopasajero.Clear();
                aportaciong.Clear();
                compbang.Clear();
                _ = notificarAsync();



            
            }

            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Seleccione de nuevo una impresora.");
                string funcion = "imprimir";
                Utilerias.LOG.write(_clase, funcion, error);


            }

        }

       private void pago()
        {
            try
            {
                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, false);
                iTextSharp.text.Font font = new iTextSharp.text.Font(bfTimes, 12, 2, BaseColor.BLACK);
                iTextSharp.text.Font font2 = new iTextSharp.text.Font(bfTimes, 9, 2, BaseColor.BLACK);

                iTextSharp.text.Font arial = FontFactory.GetFont("Arial", 12, BaseColor.BLACK);
                iTextSharp.text.Font arial2 = FontFactory.GetFont("Arial", 9, BaseColor.BLACK);
                iTextSharp.text.Document doc = new iTextSharp.text.Document();
                string rutatemporal = Path.GetTempPath();
                PdfWriter title = PdfWriter.GetInstance(doc, new FileStream(rutatemporal + "/PagoGuia.pdf", FileMode.Create));
                doc.Open();

                Bitmap imagensplash = new Bitmap(Autobuses.Properties.Resources.logotickets);

                iTextSharp.text.Image PatientSign = iTextSharp.text.Image.GetInstance(imagensplash, System.Drawing.Imaging.ImageFormat.Png);
                PatientSign.Alignment = Element.ALIGN_LEFT;



                    PdfPTable table = new PdfPTable(3);
                    table.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    table.WidthPercentage = 100;

                    PdfPCell cell = new PdfPCell();

                    cell = new PdfPCell(PatientSign);
                    cell.BorderColor = BaseColor.WHITE;
                    table.AddCell(cell);
                Paragraph p = new Paragraph();
                p.Add(new Chunk("Sucursal: ", font));
                p.Add(new Chunk("\n"));
                p.Add(new Chunk(LoginInfo.Sucursal, arial));

                p.Add(new Chunk("\n"));
                p.Add(new Chunk("\n"));
                p.Add(new Chunk("Linea: ", font));
                p.Add(new Chunk("\n"));
                p.Add(new Chunk(_linea, arial));

                cell = new PdfPCell(p);
                cell.BorderColor = BaseColor.WHITE;
                table.AddCell(cell);
                Paragraph d = new Paragraph();
                d.Add(new Chunk("Fecha: " ,font));
                d.Add(new Chunk( DateTime.Now.ToString("dd/MM/yyyy"),arial));

                d.Add(new Chunk("\n"));
                d.Add(new Chunk("\n"));
                d.Add(new Chunk("Hora: ",font));
                d.Add(new Chunk( DateTime.Now.ToString("HH: mm tt"),arial));

                d.Add(new Chunk("\n"));
                d.Add(new Chunk("\n"));
                d.Add(new Chunk("Folio: ",font));
                d.Add(new Chunk( destino,arial));
                d.Alignment = Element.ALIGN_RIGHT;




                cell = new PdfPCell(d);
                cell.BorderColor = BaseColor.WHITE;
                table.AddCell(cell);




                doc.Add(table);

                doc.Add(new Paragraph("\n"));


                PdfPTable table2 = new PdfPTable(3);
                table2.WidthPercentage = 100;

                PdfPCell cell2 = new PdfPCell();
                Paragraph a2 = new Paragraph();
                a2.Add(new Chunk("Usuario: ", font));
                a2.Add(new Chunk("\n"));
                a2.Add(new Chunk(LoginInfo.NombreID + " " + LoginInfo.ApellidoID, arial));
                a2.Add(new Chunk("\n"));
                a2.Add(new Chunk("\n"));
                a2.Add(new Chunk("Socio: ", font));
                a2.Add(new Chunk("\n"));
                a2.Add(new Chunk(_socio, arial));
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
                p2.Add(new Chunk(fechainicio + " al " + fechatermino, arial));



                cell2 = new PdfPCell(p2);
                cell2.BorderColor = BaseColor.WHITE;
                table2.AddCell(cell2);
                Paragraph d2 = new Paragraph();
                BarcodeLib.Barcode Codigobarras = new BarcodeLib.Barcode();
                iTextSharp.text.Image qrr = iTextSharp.text.Image.GetInstance(Codigobarras.Encode(BarcodeLib.TYPE.CODE128, destino, Color.Black, Color.White, 170, 30), System.Drawing.Imaging.ImageFormat.Png);
                qrr.Alignment = Element.ALIGN_RIGHT;

                cell2 = new PdfPCell(qrr);
                cell2.BorderColor = BaseColor.WHITE;
                table2.AddCell(cell2);




                doc.Add(table2);

                doc.Add(new Paragraph("\n"));

                PdfPTable table3 = new PdfPTable(8);
                table3.WidthPercentage = 100;

                PdfPCell cell3 = new PdfPCell();


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
                d4.Add(new Chunk("Importe: ", arial));
                d5.Add(new Chunk(Utilerias.Utilerias.formatCurrency(importetext), arial));
                d4.Add(new Chunk("\n"));
                d5.Add(new Chunk("\n"));
                d4.Add(new Chunk("Gastos: ", arial));
                d5.Add(new Chunk(Utilerias.Utilerias.formatCurrency(gastostext), arial));
                d4.Add(new Chunk("\n"));
                d5.Add(new Chunk("\n"));
                d4.Add(new Chunk("Aportaciónes:", arial));
                d5.Add(new Chunk(Utilerias.Utilerias.formatCurrency(aportaciontext), arial));
                d4.Add(new Chunk("\n"));
                d5.Add(new Chunk("\n"));
                d4.Add(new Chunk("Comp. Banco:", arial));
                d5.Add(new Chunk(Utilerias.Utilerias.formatCurrency(compbancotext), arial));
                d4.Add(new Chunk("\n"));
                d5.Add(new Chunk("\n"));
                d4.Add(new Chunk("Tarjetas:", arial));
                d5.Add(new Chunk(Utilerias.Utilerias.formatCurrency(tarjetastext), arial));
                d4.Add(new Chunk("\n"));
                d5.Add(new Chunk("\n"));
                d4.Add(new Chunk("Iva:", arial));
                d5.Add(new Chunk(Utilerias.Utilerias.formatCurrency(ivatext), arial));
                d4.Add(new Chunk("\n"));
                d5.Add(new Chunk("\n"));
                d4.Add(new Chunk("Total:", arial));
                d5.Add(new Chunk(Utilerias.Utilerias.formatCurrency(totaltext), arial));
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

                float bo = doc.Bottom;

                Paragraph parrafo = new Paragraph("El presente documento representa el pago de las guias mencionadas, el cual, debera ser firmado por la persona quién recibe el pago, aceptando a los términos y condiciones de ATAH.", arial);
                parrafo.SpacingBefore = 120;
                doc.Add(parrafo);

                doc.Add(new Paragraph("\n"));
                doc.Add(new Paragraph("\n"));
                doc.Add(new Paragraph("_____________________________________", arial));
                doc.Add(new Paragraph("         Nombre y firma", arial));
                doc.Add(new Paragraph("\n"));
                doc.Add(new Paragraph("\n"));
                doc.AddTitle("Pago de Guia");

                doc.Close();

                MemoryStream ms = new MemoryStream();
                PdfWriter writerPdf = PdfWriter.GetInstance(doc, ms);
                byte[] bytes = ms.ToArray();
                System.Diagnostics.Process.Start(rutatemporal + "/PagoGuia.pdf");




            }
            catch (Exception err)
            {
                string error = err.Message;
                string funcion = "Pago";
                Utilerias.LOG.write(_clase, funcion, error);

                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
            }

        }
        private void Reimprimir_Click(object sender, EventArgs e)
        {
            imprimirr();
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

                //Program.Form.DesactivarMenu();

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

        public String GenerateRandom()
        {
            string foliotemp = "";
            try
            {
                string sql = "SELECT dbo.f_Get_Folio_Pago() as FOLIO";
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
                string funcion = "infoboletos";
                Utilerias.LOG.write(_clase, funcion, error);


            }
            return foliotemp;
        }
        private void act()
        {
            try
            {
                
                if (foliobuscar != "")
                {
                    int count = 1;
                    string sql = "SELECT * FROM GUIA";

                    sql += " WHERE  FOLIO=@FOLIO ";
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@FOLIO", foliobuscar);


                    res = db.getTable();

                    if (res.Next())
                    {
                        n = dataGridViewguias.Rows.Add();


                        dataGridViewguias.Rows[n].Cells[0].Value = res.Get("FECHA");
                        dataGridViewguias.Rows[n].Cells[1].Value = res.Get("FOLIO");
                        dataGridViewguias.Rows[n].Cells[2].Value = res.Get("STATUS");
                        status = res.Get("STATUS");
                        dataGridViewguias.Rows[n].Cells[3].Value = res.Get("ORIGEN");
                        dataGridViewguias.Rows[n].Cells[4].Value = res.Get("DESTINO");
                        string fech = res.Get("HORA");
                        dataGridViewguias.Rows[n].Cells[5].Value = res.Get("FECHA") + " " + fech;
                        dataGridViewguias.Rows[n].Cells[6].Value = res.Get("AUTOBUS");
                        dataGridViewguias.Rows[n].Cells[7].Value = res.Get("BOLETOS");
                        dataGridViewguias.Rows[n].Cells[8].Value = res.Get("IMPORTE");
                        dataGridViewguias.Rows[n].Cells[9].Value = res.Get("COMTAQ");
                        dataGridViewguias.Rows[n].Cells[10].Value = res.Get("COMPBAN");
                        dataGridViewguias.Rows[n].Cells[11].Value = res.Get("APORTACION");
                        dataGridViewguias.Rows[n].Cells[12].Value = res.Get("DIESEL");
                        dataGridViewguias.Rows[n].Cells[13].Value = res.Get("CASETA");
                        dataGridViewguias.Rows[n].Cells[14].Value = res.Get("TSALIDA");
                        dataGridViewguias.Rows[n].Cells[15].Value = res.Get("TTURNO");
                        dataGridViewguias.Rows[n].Cells[16].Value = res.Get("TPASO");
                        dataGridViewguias.Rows[n].Cells[17].Value = res.Get("VSEDENA");
                        dataGridViewguias.Rows[n].Cells[18].Value = res.Get("IVA");
                        dataGridViewguias.Rows[n].Cells[19].Value = res.Get("ANTICIPO");
                        dataGridViewguias.Rows[n].Cells[20].Value = res.Get("TOTAL");
                        dataGridViewguias.Rows[n].Cells[21].Value = res.Get("SUCURSAL");
                        dataGridViewguias.Rows[n].Cells[22].Value = res.Get("validador");
                        dataGridViewguias.Rows[n].Cells[23].Value = res.Get("CHOFER");
                        dataGridViewguias.Rows[n].Cells[24].Value = res.Get("LINEA");
                        dataGridViewguias.Rows[n].Cells[25].Value = res.Get("HORA");
                        dataGridViewguias.Rows[n].Cells[26].Value = res.Get("SOCIO");




                        count++;
                    }

                    db.execute();

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
        private void Buscarfolio_Click(object sender, EventArgs e) {
            act();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
           
            int cantidad = dataGridViewguias.Rows.Count;
            if (cantidad != 0)
            {
                Form mensaje = new Mensaje("¿Está seguro de pagar la guia?", false);

                DialogResult resut = mensaje.ShowDialog();
                if (resut == DialogResult.OK)
                {

                    verificationUserControl1.Show();
                    labelusu.Visible = true;
                    verificationUserControl1.Samples.Clear();
                    verificationUserControl1.IsVerificationComplete = false;
                    verificationUserControl1.img = null;
             
                    ValidateUser();
                    verificationUserControl1.Start();

                    verificationUserControl1.Focus();
                }
            }
            else
            {
                MessageBox.Show("No hay Guias por pagar");
                btnpagar.Enabled = false;
                btnpagar.BackColor = Color.White;

                Limpiar_Click(sender, e);
            }


        }

        void ValidateUser()
        {
            fingerPrint = null;

            fingerPrint = db.selectByteArray("SELECT HUELLA FROM USUARIOS WHERE USUARIO='" + LoginInfo.UserID + "'");
            verificationUserControl1.Samples.Add(new DPFP.Template(new MemoryStream(fingerPrint)), null);
        }
        void ValidateUsersocio()
        {
            try  {
                fingerPrint = null;

                nombre.Clear();
            string sql = "SELECT NOMBRE FROM SOCIOS WHERE ACTIVO=1 AND BORRADO=0  AND PK=@PK UNION " +
                "SELECT ALTERNONOMBRE FROM SOCIOS WHERE ACTIVO = 1 AND BORRADO = 0 AND PK =@PK AND NOT HUELLA1ALTERNO IS NULL";
            db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@PK", pksocio);
                res = db.getTable();
            while (res.Next())
            {
                nombre.Add(res.Get("NOMBRE"));
            }
            for (int i = 0; i < nombre.Count(); i++)
            {


                string cadenaDeConexion = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;
                using (SqlConnection cn = new SqlConnection(cadenaDeConexion))
                {
                        if (i == 0)
                        {
                            cn.Open();

                            SqlCommand cmd = new SqlCommand("SELECT HUELLA FROM SOCIOS WHERE ACTIVO=1 AND BORRADO=0  AND PK=" + pksocio,cn);
                            fingerPrint = (byte[])cmd.ExecuteScalar();
                            verificationUserControl2.Samples.Add(new DPFP.Template(new MemoryStream(fingerPrint)), null);
                        }
                        else {
                            cn.Open();

                            SqlCommand cmd2 = new SqlCommand("SELECT HUELLAALTERNO FROM SOCIOS WHERE ACTIVO=1 AND BORRADO=0  AND PK=" + pksocio,cn);
                            fingerPrint = (byte[])cmd2.ExecuteScalar();
                            verificationUserControl2.Samples.Add(new DPFP.Template(new MemoryStream(fingerPrint)), null);

                        }

                    }

            }


                }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "socioobtener";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }
        private void VerificationUserControl1_VerificationStatusChanged(object sender, StatusChangedEventArgs e)
        {
            if (verificationUserControl1.InvokeRequired)
            {
                bool band = false;
                bool band2 = false;

                verificationUserControl1.Invoke(new Action(() =>
                {
                    band = verificationUserControl1.Validate();
                    band2 = verificationUserControl1.IsVerificationComplete;
                }));
                if (band && band2)
                {
                    verificationUserControl1.Invoke(new Action(() => {

                        labelusu.Visible = false;
                        verificationUserControl1.Samples.Clear();
                        verificationUserControl1.IsVerificationComplete = false;
                        verificationUserControl1.img = null;

                        panelcontraseña.Visible = false;
                        verificationUserControl1.FingerPrintPicture.Image = null;
                        verificationUserControl1.Stop();

                        ValidateUsersocio();
                        Form mensaje = new Mensaje("Usuario validado", true);
                         mensaje.ShowDialog();
                        contando1 = 0;

                        timer2.Start();

                    }));

                }
            }
            else
            {
                if (verificationUserControl1.IsVerificationComplete && verificationUserControl1.Validate())
                {

                    verificationUserControl1.Samples.Clear();
                    verificationUserControl1.IsVerificationComplete = false;
                    verificationUserControl1.img = null;
                    verificationUserControl1.Stop();
           
                    Form mensaje = new Mensaje("Usuario validado", true);
                     mensaje.ShowDialog();
               

                }
            }
        }

        private void VerificationUserControl1_KeyUp(object sender, KeyEventArgs e)
        {

            if (e.KeyData == Keys.Escape)
            {
                VerificationUserControl verificacion = (VerificationUserControl)sender;
                verificacion.Hide();
                verificacion.Samples.Clear();
                verificacion.IsVerificationComplete = false;
                verificacion.img = null;
                verificacion.Init();
                verificacion.limpiarhuella();
                verificacion.FingerPrintPicture.Image = null;
                //this.Close();
            }

        }
        private void VerificationUserControl2_VerificationStatusChanged(object sender, StatusChangedEventArgs e)
        {
            if (verificationUserControl2.InvokeRequired)
            {
                bool band = false;
                bool band2 = false;

                verificationUserControl2.Invoke(new Action(() =>
                {
                    band = verificationUserControl2.Validate();
                    band2 = verificationUserControl2.IsVerificationComplete;
                }));
                if (band && band2)
                {
                    verificationUserControl2.Invoke(new Action(() => {

                        verificationUserControl2.Hide();
                        nombreid = verificationUserControl2.nombre();

                        verificationUserControl2.Samples.Clear();
                        verificationUserControl2.IsVerificationComplete = false;
                        verificationUserControl2.img = null;
                        verificationUserControl2.Stop();

                        verificationUserControl2.FingerPrintPicture.Image = null;


                        labelsocio.Visible = false;
                        contando2 = 0;
                        panelcontraseña.Visible = false;
                       

                      
                        Form mensaje = new Mensaje("Socio validado", true);
                         mensaje.ShowDialog();

                        guiapagada();
                    }));

                }
            }
            else
            {
                if (verificationUserControl2.IsVerificationComplete && verificationUserControl1.Validate())
                {
                    nombreid = verificationUserControl2.nombre();

                    labelsocio.Visible = false;

                    verificationUserControl2.Hide();
                    verificationUserControl2.Samples.Clear();
                    verificationUserControl2.IsVerificationComplete = false;
                    verificationUserControl2.img = null;
                 
                    verificationUserControl2.FingerPrintPicture.Image = null;
                    Form mensaje = new Mensaje("Socio validado", true);
                    mensaje.ShowDialog();
                    guiapagada();

                }
            }
        }

        private void VerificationUserControl2_KeyUp(object sender, KeyEventArgs e)
        {

            if (e.KeyData == Keys.Escape)
            {
                VerificationUserControl verificacion = (VerificationUserControl)sender;
                verificacion.Hide();
                verificacion.Samples.Clear();
                verificacion.IsVerificationComplete = false;
                verificacion.img = null;
                verificacion.Init();
                verificacion.limpiarhuella();
                verificacion.FingerPrintPicture.Image = null;
                //this.Close();

            }

        }


        void ValidateUserGUIA()
        {

            try
            {
                if (_status != "" && _socio != "" && _conductor != "" && _autobus != "")
                {
                    string sql;
                    if (_conductor != "")
                    {
                        sql = "SELECT SOCIO_PK FROM AUTOBUSES WHERE PK_CHOFRE=@CONDUCTOR ";

                        db.PreparedSQL(sql);
                        db.command.Parameters.AddWithValue("@CONDUCTOR", _conductor);
                    }
                    if (_autobus != "")
                    {
                        sql = "SELECT SOCIO_PK FROM AUTOBUSES WHERE ECO=@ECO ";

                        db.PreparedSQL(sql);
                        db.command.Parameters.AddWithValue("@ECO", _autobus);
                    }


                    res = db.getTable();
                    if (res.Next())
                    {
                        _socio = res.Get("SOCIO_PK");
                    }
                    btnpagar.Enabled = true;
                    btnpagar.BackColor= Color.FromArgb(38, 45, 56);
                }
                if (_status != "" && _socio == "")
                {
                    btnpagar.Enabled = false;
                    btnpagar.BackColor = Color.White;

                }
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "CREAR";
                Utilerias.LOG.write("validarusuario", funcion, error);

            }
        }



        private void guiapagada()
        {
            try
            {
                if (nombreid != 100)
                {
                    nombredetermindo = nombre[nombreid];
                }
             

                else
                {

                    nombredetermindo = "super contraseña";
                }
                string usuario = LoginInfo.NombreID + " " + LoginInfo.ApellidoID;

                if (contrausuario == true)
                {
                    usuario = "super contraseña";
                }
                cantidadfolios = dataGridViewguias.Rows.Count;
                folio = GenerateRandom();

                if (cantidadfolios > 0)
                {
                    
                    pksocio = (string)(combosocio.SelectedItem as ComboboxItem).Value;
                    string sql = "INSERT INTO GUIASPAGADAS(FECHAINICIO,FECHATERMINO,CANTIDADEGUIAS,IMPORTE,GASTOS,TARJETAS,IVA,TOTAL,USUARIO,SOCIO, FOLIO,SUCURSAL,FECHA,LINEA,PKSOCIO,COBRADOR,APORTACIONES,COMPBAN,CONTRAUSUARIO,CONTRASOCIO)" +
                        " VALUES(@FECHAINICIO,@FECHATERMINO,@CANTIDADEGUIAS,@IMPORTE,@GASTOS,@TARJETAS,@IVA,@TOTAL,@USUARIO,@SOCIO,@FOLIO,@SUCURSAL,@FECHA,@LINEA,@PKSOCIO,@COBRADOR,@APORTACIONES,@COMPBAN,@CONTRAUSUARIO,@CONTRASOCIO)";
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@FECHAINICIO", fechainicio);
                    db.command.Parameters.AddWithValue("@FECHATERMINO", fechatermino);
                    db.command.Parameters.AddWithValue("@CANTIDADEGUIAS", cantidadfolios);
                    db.command.Parameters.AddWithValue("@IMPORTE", importetext);
                    db.command.Parameters.AddWithValue("@GASTOS", gastostext);
                    db.command.Parameters.AddWithValue("@TARJETAS", tarjetastext);
                    db.command.Parameters.AddWithValue("@IVA", ivatext);
                    db.command.Parameters.AddWithValue("@TOTAL", totaltext);
                    db.command.Parameters.AddWithValue("@USUARIO", usuario);
                    db.command.Parameters.AddWithValue("@SOCIO", _socio);
                    db.command.Parameters.AddWithValue("@PKSOCIO", pksocio);
                    db.command.Parameters.AddWithValue("@COBRADOR",nombredetermindo);
                    db.command.Parameters.AddWithValue("@FOLIO", folio);
                    db.command.Parameters.AddWithValue("@SUCURSAL", LoginInfo.Sucursal);
                    db.command.Parameters.AddWithValue("@FECHA", DateTime.Now.ToString("dd/MM/yyyy"));
                    db.command.Parameters.AddWithValue("@LINEA", _linea);
                    db.command.Parameters.AddWithValue("@APORTACIONES", aportaciontext);
                    db.command.Parameters.AddWithValue("@COMPBAN", compbancotext);
                    db.command.Parameters.AddWithValue("@CONTRAUSUARIO", contrausuario);
                    db.command.Parameters.AddWithValue("@CONTRASOCIO", contrasocio);


                    pkguia = db.executeId();

                    if (!string.IsNullOrEmpty(pkguia))
                    {

                        sql = "UPDATE GUIA SET STATUS=@STATUS, USUARIOPAGADO=@USUARIOPAGADO, PKGUIAPAGADA=@PKGUIAPAGADA WHERE PK in (" + string.Join(",", pkporpagar) + ")";
                        db.PreparedSQL(sql);
                        db.command.Parameters.AddWithValue("@STATUS", "PAGADA");
                        db.command.Parameters.AddWithValue("@USUARIOPAGADO", LoginInfo.PkUsuario);
                        db.command.Parameters.AddWithValue("@PKGUIAPAGADA", pkguia);
                        db.execute();

                        guiasobtener();
                        imprimirr();
             
                        limp();


                    }
                    else
                    {
                        Form mensaje = new Mensaje("No se genero el pago intente mas tarde", true);
                         mensaje.ShowDialog();
                    }
                }
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "n";
                Utilerias.LOG.write(_clase, funcion, error);

            }

        }
        async Task RunAsync(NotificationDataModel data)
        {

            var myContent = JsonNet.Serialize(data);
            var stringContent = new StringContent(myContent, UnicodeEncoding.UTF8, "application/json");

            HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri("https://localhost:44333/api/SendPushNotificationPartners/");
            client.BaseAddress = new Uri("https://appi-atah.azurewebsites.net/api/SendPushNotificationPartners/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {
                var responsevar = "";
                HttpResponseMessage response = await client.PostAsync("https://appi-atah.azurewebsites.net/api/SendPushNotificationPartners/", stringContent);
                //HttpResponseMessage response = await client.PostAsync("https://localhost:44333/api/SendPushNotificationPartners/", stringContent);
                if (response.IsSuccessStatusCode)
                {
                    responsevar = await response.Content.ReadAsStringAsync();
                }
                Respuesta res = JsonNet.Deserialize<Respuesta>(responsevar);

                //  MessageBox.Show(res.mensaje);
            }
            catch (Exception e)
            {
                string error = e.Message;
                MessageBox.Show("Error con el servicio intente màs tarde");
            }

        }

        private void sociouser()
        {
            try
            {
                string sql = "SELECT USUARIO FROM SOCIOS  WHERE PK=@PK";
                db.PreparedSQL(sql);

                db.command.Parameters.AddWithValue("@PK", pksocio);
                res = db.getTable();
                if (res.Next())
                {
                    sociousuario = res.Get("USUARIO");
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
                string funcion = "sociouser";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }
        private async Task notificarAsync()
        {
            List<PARTNERS> partners = new List<PARTNERS>();
            PARTNERS p;

            p = new PARTNERS();
            sociouser();
            p.USUARIO = sociousuario;
            partners.Add(p);
            //string notificacion = "\"NOTIFICATION\":{ \"TITLE\":\"" + tbTitle.Text + "\",\"MESSAGE\":\"" + tbMessage.Text + "\"}";
            NOTIFICATION noti = new NOTIFICATION();
            noti.TITLE = "Nuevo Pago de Guia Generado";
            noti.MESSAGE = "Pago con folio: " + folio + ",  cobrado por: " + nombredetermindo + ", con un total de pago de : " + Utilerias.Utilerias.formatCurrency(total);
            noti.DATA = "PK_GUIA_PAGADA:" + pkguia;
            NotificationDataModel data = new NotificationDataModel();
            data.PARTNERS = partners;
            data.NOTIFICATION = noti;
            await RunAsync(data);
        }

        private void guiasobtener()
        {
        
            double tar = 0.0;
            importeguia = 0.0;
            gastosguia = 0.0;
            ivaguia = 0.0;
            totalguia = 0.0;
            double aportacionguia = 0.0;
            double compbanguia = 0.0;

            for (int i =0;i<dataGridViewguias.Rows.Count;i++)
            {
                folioguia = dataGridViewguias.Rows[i].Cells[1].Value.ToString();
                importeguia = Convert.ToDouble(dataGridViewguias.Rows[i].Cells["importedname"].Value);
                gastosguia = Convert.ToDouble(dataGridViewguias.Rows[i].Cells["gastosdname"].Value);
                tar = Convert.ToDouble(dataGridViewguias.Rows[i].Cells["tarjetasdname"].Value);
               
                ivaguia = Convert.ToDouble(dataGridViewguias.Rows[i].Cells["ivadname"].Value);
                totalguia = Convert.ToDouble(dataGridViewguias.Rows[i].Cells["totaldname"].Value);

                aportacionguia = Convert.ToDouble(dataGridViewguias.Rows[i].Cells["aportaciodname"].Value);
                compbanguia = Convert.ToDouble(dataGridViewguias.Rows[i].Cells["combandname"].Value);

                asiento.Add(folioguia);
                pasajero.Add(Utilerias.Utilerias.formatCurrency( importeguia));
                destinopasajero.Add(Utilerias.Utilerias.formatCurrency(gastosguia));
                foliopasajero.Add(Utilerias.Utilerias.formatCurrency(tar));
                tarifapasajero.Add(Utilerias.Utilerias.formatCurrency(ivaguia));
                preciopasajero.Add(Utilerias.Utilerias.formatCurrency(totalguia));

                compbang.Add(Utilerias.Utilerias.formatCurrency(compbanguia));
                aportaciong.Add(Utilerias.Utilerias.formatCurrency(aportacionguia));

            }
        }


        private void Limpiar_Click(object sender, EventArgs e)
        {
            limp();
        }
        private void limp()
        {
            combosocio.Enabled = false;
            dispototal.Text = "";
            disguia.Text = "";
            disptotal.Text = "";
            _linea = "";
            pksocio = "";
            _socio = "";
            textBoxcompbanco.Text = "";
            textBoxaportacion.Text="";
            dispototal.Text = "";
            dispoguias.Text = "";
            textBoximport.Text = "";
            textBoxgastos.Text = "";
            textBoxtarjetas.Text = "";
            textBoxiva.Text = "";
            textBoxtotal.Text = "";
            textBoxguias.Text = "";
            combosocio.Text = "";
            combosocio.Items.Clear();
            button1.Enabled = false;
            button1.BackColor = Color.White;
            comboBoxlinea.Text = "";
            sucursalbusqueda = "";
            dataGridViewguias.Rows.Clear();
            comboBoxlinea.Items.Clear();
           
            verificationUserControl1.Visible = false;
            verificationUserControl2.Visible = false;
            btnpagar.Enabled = false;
            btnpagar.BackColor = Color.White;
            contrausuario = false;
            contrasocio = false;
          imported = 0.0;
         gastosd = 0.0;
         tarjetasd = 0.0;
         ivad = 0.0;
            compban = 0.0;
            aportacion = 0.0;
            contando2 = 0;
            contando1 = 0;
            labelsocio.Visible = false;
            labelusu.Visible = false;

        totald = 0.0;

        getDatosAdicionaleslinea();
            getDatosAdicionalessocios();

        }

        private void Reporte_Guias_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;

        }

        private void inicio()
        
        {
            string sql = "SELECT top(1) CONVERT(VARCHAR(10),FECHA,120) AS FECHA FROM VTOTAL_GUIA_DETALLE where pksocio=@pksocio  and linea=@linea order by FECHA asc";
            db.PreparedSQL(sql);
            db.command.Parameters.AddWithValue("@pksocio", pksocio);
           
            db.command.Parameters.AddWithValue("@linea", _linea);

            res = db.getTable();

            if (res.Next())
            {
                fechainicio = res.Get("FECHA");
                textBoxinicio.Text = fechainicio;
                button1.Enabled = true;
                button1.BackColor = Color.FromArgb(38, 45, 56);
                dataGridViewguias.Rows.Clear();
                obtenerguias();
            }
            else
            {
                textBoxinicio.Text = "";
                dataGridViewguias.Rows.Clear();
                obtenerguias();
            }
        }
        private void obtenerguias()
        {
            try
            {
                textBoximport.Text = "$" + 0;
                textBoxgastos.Text = "$" + 0;
                textBoxtarjetas.Text = "$" + 0;
                textBoxiva.Text = "$" + 0;
                textBoxtotal.Text = "$" + 0;
                textBoxguias.Text = "$" + 0;
                disguia.Text = "$" + 0;
                dispototal.Text = "$" + 0;
                dispoguias.Text = "$" + 0;
                disptotal.Text = "$" + 0;
                string sql = "SELECT CONVERT(REAL,IMPORTE) AS IMPORTE,CONVERT(REAL,COMPBAN) AS COMPBAN,CONVERT(REAL,TSALIDA) AS TSALIDA,CONVERT(REAL,TTURNO) AS TTURNO,CONVERT(REAL,TPASO) AS TPASO," +
                    "CONVERT(REAL,IVA) AS IVA,CONVERT(REAL,ANTICIPO) AS ANTICIPO,CONVERT(REAL,TOTAL) AS TOTAL,FECHA,FOLIO,STATUS,ORIGEN,DESTINO,AUTOBUS,BOLETOS,COMTAQ," +
                    "CONVERT(REAL,APORTACION) AS APORTACION,DIESEL,CASETA,VSEDENA,HORA,VALIDADOR,CHOFER,LINEA,SOCIO,PK FROM GUIA WHERE PK in (SELECT PK FROM VTOTAL_GUIA_DETALLE where" +
                    " pksocio=@pksocio and Convert(varchar(10),FECHA,120)<=@FECHATERMINO) ORDER BY PK";
                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@pksocio", pksocio);
                db.command.Parameters.AddWithValue("@FECHATERMINO", fechatermino);
                db.command.Parameters.AddWithValue("@linea", _linea);

                res = db.getTable();
                foliosporpagar.Clear();
                pkporpagar.Clear();
                while (res.Next())
                {
                    n = dataGridViewguias.Rows.Add();

                    importe= (double.TryParse(res.Get("IMPORTE"), out double aux1)) ? res.GetDouble("IMPORTE") : 0.0;
                    dataGridViewguias.Rows[n].Cells["importedname"].Value = importe;
                    dataGridViewguias.Rows[n].Cells["imp"].Value = Utilerias.Utilerias.formatCurrency(importe);
                    
                    compban = (double.TryParse(res.Get("COMPBAN"), out double aux2)) ? res.GetDouble("COMPBAN") : 0.0;
                    dataGridViewguias.Rows[n].Cells["compbanco"].Value=Utilerias.Utilerias.formatCurrency(compban);
                    dataGridViewguias.Rows[n].Cells["combandname"].Value = compban;

                    aportacion = (double.TryParse(res.Get("APORTACION"), out double aux22)) ? res.GetDouble("APORTACION") : 0.0;
                    dataGridViewguias.Rows[n].Cells["Apor"].Value = Utilerias.Utilerias.formatCurrency(aportacion);
                    dataGridViewguias.Rows[n].Cells["aportaciodname"].Value = aportacion;



                    dataGridViewguias.Rows[n].Cells[14].Value = (double.TryParse(res.Get("TSALIDA"), out double aux3)) ? res.GetDouble("TSALIDA") : 0.0;         
                    dataGridViewguias.Rows[n].Cells[15].Value = (double.TryParse(res.Get("TTURNO"), out double aux4)) ? res.GetDouble("TTURNO") : 0.0;
                    dataGridViewguias.Rows[n].Cells[16].Value = (double.TryParse(res.Get("TPASO"), out double aux5)) ? res.GetDouble("TPASO") : 0.0;

                     tarjetas = (double.TryParse(res.Get("TSALIDA"), out double aux6)) ? res.GetDouble("TSALIDA") : 0.0;
                    tarjetas += (double.TryParse(res.Get("TTURNO"), out double aux7)) ? res.GetDouble("TTURNO") : 0.0;
                    tarjetas += (double.TryParse(res.Get("TPASO"), out double aux8)) ? res.GetDouble("TPASO") : 0.0;
                    dataGridViewguias.Rows[n].Cells["tarjetaname"].Value = Utilerias.Utilerias.formatCurrency(tarjetas);
                    dataGridViewguias.Rows[n].Cells["tarjetasdname"].Value =tarjetas;

                    iva = (double.TryParse(res.Get("IVA"), out double aux9)) ? res.GetDouble("IVA") : 0.0;
                    dataGridViewguias.Rows[n].Cells[19].Value = Utilerias.Utilerias.formatCurrency(iva);
                    dataGridViewguias.Rows[n].Cells["ivadname"].Value = iva;

                    gastos = (double.TryParse(res.Get("ANTICIPO"), out double aux10)) ? res.GetDouble("ANTICIPO") : 0.0;
                    dataGridViewguias.Rows[n].Cells["ANTI"].Value = Utilerias.Utilerias.formatCurrency(gastos);
                    dataGridViewguias.Rows[n].Cells["gastosdname"].Value = gastos;
                    
                    total = (double.TryParse(res.Get("TOTAL"), out double aux11)) ? res.GetDouble("TOTAL") : 0.0;
                    dataGridViewguias.Rows[n].Cells["totalname"].Value = Utilerias.Utilerias.formatCurrency(total);
                    dataGridViewguias.Rows[n].Cells["totaldname"].Value = total;

    
                    dataGridViewguias.Rows[n].Cells[0].Value = res.Get("FECHA");
                    dataGridViewguias.Rows[n].Cells[1].Value = res.Get("FOLIO");
                    dataGridViewguias.Rows[n].Cells[2].Value = res.Get("STATUS");
                    dataGridViewguias.Rows[n].Cells[3].Value = res.Get("ORIGEN");
                    dataGridViewguias.Rows[n].Cells[4].Value = res.Get("DESTINO");
                    string fech = res.Get("HORA");
                    //dataGridViewguias.Rows[n].Cells[5].Value = res.Get("FECHA") + " " + fech;
                    dataGridViewguias.Rows[n].Cells[5].Value = res.Get("AUTOBUS");
                    dataGridViewguias.Rows[n].Cells[6].Value = res.Get("BOLETOS");
                    dataGridViewguias.Rows[n].Cells[9].Value = res.Get("COMTAQ");
                    dataGridViewguias.Rows[n].Cells[12].Value = res.Get("DIESEL");
                    dataGridViewguias.Rows[n].Cells[13].Value = res.Get("CASETA");
              
                    dataGridViewguias.Rows[n].Cells[18].Value = res.Get("VSEDENA");
                    
                    dataGridViewguias.Rows[n].Cells[22].Value = res.Get("HORA");
                    dataGridViewguias.Rows[n].Cells[23].Value = res.Get("VALIDADOR");
                    dataGridViewguias.Rows[n].Cells[24].Value = res.Get("CHOFER");
                    dataGridViewguias.Rows[n].Cells[25].Value = res.Get("LINEA");
                    dataGridViewguias.Rows[n].Cells[26].Value = res.Get("HORA");
                    dataGridViewguias.Rows[n].Cells[27].Value = res.Get("SOCIO");
                    dataGridViewguias.Rows[n].Cells[28].Value = res.Get("PK");
            
                }


                int cantidad = dataGridViewguias.Rows.Count;
                importetext = 0.0;
                gastostext = 0.0;
                tarjetastext = 0.0;
                ivatext = 0.0;
                totaltext = 0.0;
                compbancotext = 0.0;
                aportaciontext = 0.0;
                bool pagarsepuede = false;

                if (cantidad > 0)
                {
                    for (int i = 0; i < cantidad; i++)
                    {
                        importetext += Convert.ToDouble(dataGridViewguias.Rows[i].Cells["importedname"].Value);
                        gastostext += Convert.ToDouble(dataGridViewguias.Rows[i].Cells["gastosdname"].Value);
                        tarjetastext += Convert.ToDouble(dataGridViewguias.Rows[i].Cells[14].Value);
                        tarjetastext += Convert.ToDouble(dataGridViewguias.Rows[i].Cells[15].Value);
                        tarjetastext += Convert.ToDouble(dataGridViewguias.Rows[i].Cells[16].Value);
                        ivatext += Convert.ToDouble(dataGridViewguias.Rows[i].Cells["ivadname"].Value);
                        totaltext += Convert.ToDouble(dataGridViewguias.Rows[i].Cells["totaldname"].Value);
                        compbancotext += Convert.ToDouble(dataGridViewguias.Rows[i].Cells["combandname"].Value);
                        aportaciontext += Convert.ToDouble(dataGridViewguias.Rows[i].Cells["aportaciodname"].Value);
                        pagadooactivo = (string)dataGridViewguias.Rows[i].Cells[2].Value;
                        foliosporpagar.Add(dataGridViewguias.Rows[i].Cells[1].Value.ToString());
                        pkporpagar.Add(dataGridViewguias.Rows[i].Cells["guia_pk"].Value.ToString());


                    }
                }

                btnpagar.Enabled = true;
                btnpagar.BackColor = Color.FromArgb(38, 45, 56);


                textBoximport.Text = Utilerias.Utilerias.formatCurrency( importetext);
                textBoxgastos.Text = Utilerias.Utilerias.formatCurrency(gastostext);
                textBoxtarjetas.Text = Utilerias.Utilerias.formatCurrency(tarjetastext);
                textBoxiva.Text = Utilerias.Utilerias.formatCurrency(ivatext);
                textBoxtotal.Text = Utilerias.Utilerias.formatCurrency(totaltext);
                textBoxaportacion.Text = Utilerias.Utilerias.formatCurrency(aportaciontext);
                textBoxcompbanco.Text = Utilerias.Utilerias.formatCurrency(compbancotext);
                textBoxguias.Text = cantidad.ToString();
                dispo();

            }
            catch (Exception err)
            {
               string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "obtenerguias";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }
        private void dispo()
        {
            string sql = "SELECT SUM(CONVERT(REAL,TOTAL)) AS TOTAL,SUM(CANTIDAD_GUIAS) AS CANTIDAD_GUIAS FROM VTOTAL_GUIA where linea=@linea AND PKSOCIO=@pksocio ";
            db.PreparedSQL(sql);
            db.command.Parameters.AddWithValue("@pksocio", pksocio);
           
            db.command.Parameters.AddWithValue("@linea", _linea);

            res = db.getTable();

            cantidaddispo = 0;
            totaldispo = 0;
            if (res.Next())
            {
            
                cantidaddispo +=res.GetInt("CANTIDAD_GUIAS");
                totaldispo +=Math.Round((double.TryParse(res.Get("TOTAL"), out double aux7)) ? res.GetDouble("TOTAL") : 0.0,2);
               
            }
            dispototal.Text = Utilerias.Utilerias.formatCurrency(totaldispo);
            dispoguias.Text = cantidaddispo.ToString();


            string sql2 = "SELECT count(*) as CANTIDAD, sum(CONVERT(FLOAT,TOTAL)) AS TOTAL FROM VTOTAL_GUIA_DETALLE where linea=@linea and pksocio=@pksocio and Convert(varchar(10),FECHA,120)<=@FECHA";
            db.PreparedSQL(sql2);
            db.command.Parameters.AddWithValue("@pksocio", pksocio);
            db.command.Parameters.AddWithValue("@FECHA", fechadispo);
            db.command.Parameters.AddWithValue("@linea", _linea);

            res = db.getTable();
            cantidaddetalle = 0;
            totaldetalle = 0;
            if (res.Next())
            {
                cantidaddetalle = res.GetInt("CANTIDAD");
                totaldetalle = (Double.TryParse(res.Get("TOTAL"), out double aux7)) ? res.GetDouble("TOTAL") : 0.0; 

            }
            disptotal.Text = Utilerias.Utilerias.formatCurrency(totaldetalle);
            disguia.Text = cantidaddetalle.ToString();
        }

        private void conf()
        {
            string sql = "select VALOR from VARIABLES where NOMBRE='dias'";
            db.PreparedSQL(sql);
           

            res = db.getTable();

            if (res.Next())
            {
                dias = res.GetInt("VALOR");
             
            }
        }
        private void Reporte_Guias_Shown(object sender, EventArgs e)
            {
                db = new database();

            conf();
                combosocio.DropDownStyle = ComboBoxStyle.DropDownList;

            comboBoxlinea.DropDownStyle = ComboBoxStyle.DropDownList;

            dataGridViewguias.EnableHeadersVisualStyles = false;
            dateTimePicker2.MaxDate = DateTime.Now.AddDays(-dias);

            fechatermino = DateTime.Now.AddDays(-dias).ToString("yyyy-MM-dd");
            fechadispo = DateTime.Now.AddDays(-dias).ToString("yyyy-MM-dd");
            getDatosAdicionalessocios();
                getDatosAdicionaleslinea();

            titulo.Text = "Reporte y Pagos de Guias";
                permisos();
                btnpagar.Enabled = false;
            btnpagar.BackColor = Color.White;
            comboBoxlinea.DropDownStyle = ComboBoxStyle.DropDownList;

            button1.Enabled = false;
            button1.BackColor = Color.White;
            combosocio.Enabled = false;
               
            //Estas lineas eliminan los parpadeos del formulario o controles en la interfaz grafica (Pero no en un 100%)
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.DoubleBuffered = true;
            timer1.Interval = 1;

            labelnombre.Text = LoginInfo.NombreID;
            labelapellido.Text = LoginInfo.ApellidoID;
            MemoryStream ms = new MemoryStream((byte[])LoginInfo.imagenfoto);
            Bitmap bmp = new Bitmap(ms);
            pictureBoxfoto.Image = bmp;
            labelcargo.Text = LoginInfo.rol;
            timer1.Start();
            supercontra();
        }

            private void ComboBoxlinea_SelectedIndexChanged(object sender, EventArgs e)
            {
                
                combosocio.Enabled = true;
                
                _linea = (comboBoxlinea.SelectedItem.ToString());

        }

            private void ToolStripExportExcel_Click(object sender, EventArgs e)
            {
                Utilerias.Utilerias.ExportarDataGridViewExcel(dataGridViewguias);

            }

            private void BtnMinimizar_Click(object sender, EventArgs e)
            {
                      this.WindowState = FormWindowState.Minimized;
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


        private void Timer1_Tick(object sender, EventArgs e)
            {
                lblHora.Text = DateTime.Now.ToString("hh:mm:ss");
                lbFecha.Text = DateTime.Now.ToString("D");

            }

        private void Combosocio_SelectedIndexChanged(object sender, EventArgs e)
        {
            pksocio = (combosocio.SelectedItem != null) ? (combosocio.SelectedItem as ComboboxItem).Value.ToString() : ""; 
            _socio = (combosocio.SelectedItem != null) ? combosocio.SelectedItem.ToString():"";
            inicio();

        }

        private void Button4_Click(object sender, EventArgs e)
        {

            try
            {

                iTextSharp.text.Document doc = new iTextSharp.text.Document();
                PdfWriter title = PdfWriter.GetInstance(doc, new FileStream("PagoGuia.pdf", FileMode.Create));
                doc.Open();

                iTextSharp.text.Image PatientSign = iTextSharp.text.Image.GetInstance(Autobuses.Properties.Resources.llavess, System.Drawing.Imaging.ImageFormat.Png);
                PatientSign.Alignment = Element.ALIGN_LEFT;
                PatientSign.ScaleToFit(150f, 100f);
                doc.Add(new Paragraph("\n"));



                PdfPTable table = new PdfPTable(3);
                table.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                table.WidthPercentage = 100;

                PdfPCell cell = new PdfPCell();

                cell = new PdfPCell(PatientSign);
                cell.BorderColor = BaseColor.WHITE;
                table.AddCell(cell);
                Paragraph p = new Paragraph();
                p.Add(new Chunk("Sucursal: " + LoginInfo.Sucursal));
                p.Add(new Chunk("\n"));
                p.Add(new Chunk("\n"));
                p.Add(new Chunk("Linea: " + _linea));
                p.Add(new Chunk("\n"));
                p.Add(new Chunk("\n"));
                p.Add(new Chunk("Telefono: " + "555555555"));

                cell = new PdfPCell(p);
                cell.BorderColor = BaseColor.WHITE;
                table.AddCell(cell);
                Paragraph d = new Paragraph();
                d.Add(new Chunk("Fecha: " + DateTime.Now.ToString("dd/MM/yyyy")));
                d.Add(new Chunk("\n"));
                d.Add(new Chunk("\n"));
                d.Add(new Chunk("Hora: " + DateTime.Now.ToString("HH: mm tt")));
                d.Add(new Chunk("\n"));
                d.Add(new Chunk("\n"));
                d.Add(new Chunk("Folio: " + destino));
                d.Add(new Chunk("\n"));
                d.Add(new Chunk("Cantidadfolios de guias:" + cantidadfolios));
                d.Add(new Chunk("\n"));
                d.Alignment = Element.ALIGN_RIGHT;




                cell = new PdfPCell(d);
                cell.BorderColor = BaseColor.WHITE;
                table.AddCell(cell);




                doc.Add(table);

                doc.Add(new Paragraph("\n"));


                PdfPTable table2 = new PdfPTable(3);
                table2.WidthPercentage = 100;

                PdfPCell cell2 = new PdfPCell();
                Paragraph a2 = new Paragraph();
                a2.Add(new Chunk("Usuario: "));
                a2.Add(new Chunk("\n"));
                a2.Add(new Chunk(LoginInfo.NombreID + " " + LoginInfo.ApellidoID));
                a2.Add(new Chunk("\n"));
                a2.Add(new Chunk("\n"));
                a2.Add(new Chunk("Socio: "));
                a2.Add(new Chunk("\n"));
                a2.Add(new Chunk(_socio));
                a2.Add(new Chunk("\n"));
             
                cell2 = new PdfPCell(a2);
                cell2.BorderColor = BaseColor.WHITE;
                table2.AddCell(cell2);
                Paragraph p2 = new Paragraph();
                p2.Add(new Chunk("Rango de fechas de pago:"));
                p2.Add(new Chunk("\n"));
                p2.Add(new Chunk("\n"));
                p2.Add(new Chunk(fechainicio + " al " + fechatermino));




                cell2 = new PdfPCell(p2);
                cell2.BorderColor = BaseColor.WHITE;
                table2.AddCell(cell2);
                Paragraph d2 = new Paragraph();
                BarcodeLib.Barcode Codigobarras = new BarcodeLib.Barcode();
                iTextSharp.text.Image qrr = iTextSharp.text.Image.GetInstance(Codigobarras.Encode(BarcodeLib.TYPE.CODE128, destino, Color.Black, Color.White, 170, 30), System.Drawing.Imaging.ImageFormat.Png);
                qrr.Alignment = Element.ALIGN_RIGHT;

                cell2 = new PdfPCell(qrr);
                cell2.BorderColor = BaseColor.WHITE;
                table2.AddCell(cell2);




                doc.Add(table2);

                doc.Add(new Paragraph("\n"));

                PdfPTable table3 = new PdfPTable(6);
                table3.WidthPercentage = 100;

                PdfPCell cell3 = new PdfPCell();


                Paragraph folio = new Paragraph();
                Paragraph importe = new Paragraph();
                Paragraph gastos = new Paragraph();
                Paragraph aportacion = new Paragraph();
                Paragraph ComBanco = new Paragraph();

                Paragraph tarjetas = new Paragraph();
                Paragraph iva = new Paragraph();
                Paragraph total = new Paragraph();
                folio.Add(new Chunk("Folio "));
                importe.Add(new Chunk("Importe "));
                gastos.Add(new Chunk("Gastos "));
                aportacion.Add(new Chunk("Aportación "));
                ComBanco.Add(new Chunk("Com. Banco "));
                tarjetas.Add(new Chunk("Tarjetas "));
                iva.Add(new Chunk("Iva "));
                total.Add(new Chunk("Total "));

                cell3 = new PdfPCell(folio);
                table3.AddCell(cell3);
                cell3 = new PdfPCell(importe);
                table3.AddCell(cell3);
                cell3 = new PdfPCell(gastos);
                table3.AddCell(cell3);
                cell3 = new PdfPCell(aportacion);
                table3.AddCell(cell3);
                cell3 = new PdfPCell(ComBanco);
                table3.AddCell(cell3);
                cell3 = new PdfPCell(tarjetas);
                table3.AddCell(cell3);
                cell3 = new PdfPCell(iva);
                table3.AddCell(cell3);
                cell3 = new PdfPCell(total);
                table3.AddCell(cell3);
                doc.Add(table3);

                PdfPTable table33 = new PdfPTable(6);
                table33.WidthPercentage = 100;
                PdfPCell cell33 = new PdfPCell();

                Paragraph folio3 = new Paragraph();
                Paragraph importe3 = new Paragraph();
                Paragraph gastos3 = new Paragraph();
                Paragraph tarjetas3 = new Paragraph();
                Paragraph aportacion3 = new Paragraph();

                Paragraph compban3 = new Paragraph();
                Paragraph iva3 = new Paragraph();
                Paragraph total3 = new Paragraph();
                for (int i = 0; i < asiento.Count(); i++)
                {

                    folio3.Add(new Chunk(asiento[i]));
                    importe3.Add(new Chunk( pasajero[i]));
                    gastos3.Add(new Chunk( destinopasajero[i]));
                    aportacion3.Add(new Chunk(aportaciong[i]));
                    compban3.Add(new Chunk(compbang[i]));
                    tarjetas3.Add(new Chunk( foliopasajero[i]));
                    iva3.Add(new Chunk( tarifapasajero[i]));
                    total3.Add(new Chunk( preciopasajero[i]));
                    folio3.Add(new Chunk("\n "));
                    importe3.Add(new Chunk("\n "));
                    gastos3.Add(new Chunk("\n "));
                    aportacion3.Add(new Chunk("\n "));
                    compban3.Add(new Chunk("\n "));
                    tarjetas3.Add(new Chunk("\n "));
                    iva3.Add(new Chunk("\n "));
                    total3.Add(new Chunk("\n "));
                    folio3.Add(new Chunk("\n "));
                    importe3.Add(new Chunk("\n "));
                    gastos3.Add(new Chunk("\n "));
                    aportacion3.Add(new Chunk("\n "));
                    compban3.Add(new Chunk("\n "));


                    tarjetas3.Add(new Chunk("\n "));
                    iva3.Add(new Chunk("\n "));
                    total3.Add(new Chunk("\n "));
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
                cell33 = new PdfPCell(aportacion3);
                cell33.BorderColor = BaseColor.WHITE;
                table33.AddCell(cell33);
                cell33 = new PdfPCell(compban3);
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



                doc.Add(table33);
                doc.Add(new Paragraph("\n"));
                doc.Add(new Paragraph("\n"));
                PdfPTable table4 = new PdfPTable(3);
                table4.WidthPercentage = 100;

                PdfPCell cell4 = new PdfPCell();
                cell4.BorderColor = BaseColor.WHITE;

                cell4 = new PdfPCell(new Paragraph("\n"));
                cell4.BorderColor = BaseColor.WHITE;
                table4.AddCell(cell4);
                cell4 = new PdfPCell(new Paragraph("\n"));
                cell4.BorderColor = BaseColor.WHITE;
                table4.AddCell(cell4);

                Paragraph d4 = new Paragraph();
                d4.Add(new Chunk("Importe:      $" + importetext.ToString()));
                d4.Add(new Chunk("\n"));
                d4.Add(new Chunk("\n"));
                d4.Add(new Chunk("Gastos:       $" + gastostext.ToString()));
                d4.Add(new Chunk("\n"));
                d4.Add(new Chunk("\n"));
                d4.Add(new Chunk("Tarjetas:     $" + tarjetastext.ToString()));
                d4.Add(new Chunk("\n"));
                d4.Add(new Chunk("\n"));
                d4.Add(new Chunk("Iva:          $" + ivatext.ToString()));
                d4.Add(new Chunk("\n"));
                d4.Add(new Chunk("\n"));
                d4.Add(new Chunk("Total:        $" + totaltext.ToString()));
                d4.Add(new Chunk("\n"));


                d4.Alignment = Element.ALIGN_RIGHT;
                cell4 = new PdfPCell(d4);
                cell4.BorderColor = BaseColor.WHITE;
                table4.AddCell(cell4);
                doc.Add(table4);
                doc.Add(new Paragraph("\n"));

                doc.Add(new Paragraph("El presente documento representa el pago de la guia el cual  debera ser firmado por la persona quien recibe el pago"));


                doc.Add(new Paragraph("\n"));
                doc.Add(new Paragraph("\n"));
                doc.Add(new Paragraph("_____________________________________________"));
                doc.Add(new Paragraph("               Firma del Socio               "));
                doc.Add(new Paragraph("\n"));
                doc.Add(new Paragraph("\n"));




                MemoryStream ms = new MemoryStream();
                PdfWriter writerPdf = PdfWriter.GetInstance(doc, ms);
                byte[] bytes = ms.ToArray();
                System.Diagnostics.Process.Start("PagoGuia.pdf");




            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
            }

        }

        private void GroupBox2_Enter(object sender, EventArgs e)
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

        private void dataGridViewguias_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void verificationUserControl1_BackColorChanged(object sender, EventArgs e)
        {
            contando1 += 1;
            if (contando1 >= 5)
            {
                panelcontraseña.Visible = true;
                textBoxcontraseña.Focus();
            }

        }

        private void verificationUserControl2_BackColorChanged(object sender, EventArgs e)
        {
            contando2 += 1;
            if (contando2 >= 5)
            {
                panelcontraseña.Visible = true;
                textBoxcontraseña.Focus();
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
        private void button2_Click_1(object sender, EventArgs e)
        {
            if (textBoxcontraseña.Text == contraseña)
            {
                contraseñaerror.Visible = false;

                if (contando1 >= 5)
                {
                    labelusu.Visible = false;
                  
                    verificationUserControl1.Samples.Clear();
                    verificationUserControl1.IsVerificationComplete = false;
                    verificationUserControl1.img = null;
                    textBoxcontraseña.Text = "";
                    panelcontraseña.Visible = false;
                    verificationUserControl1.FingerPrintPicture.Image = null;
                    verificationUserControl1.Stop();
                    contrausuario = true;
                    ValidateUsersocio();
                    timer2.Start();
                    Form mensaje = new Mensaje("Usuario validado con contraseñaa", true);
                    mensaje.ShowDialog();
                    contando1 = 0;

                }
                if (contando2 >= 5)
                {
                    verificationUserControl2.Hide();
                    labelsocio.Visible = false;
                    contrasocio = true;
                    textBoxcontraseña.Text = "";

                    verificationUserControl2.Samples.Clear();
                    verificationUserControl2.IsVerificationComplete = false;
                    verificationUserControl2.img = null;
                   
                    verificationUserControl2.FingerPrintPicture.Image = null;
                    verificationUserControl2.Stop();
                    Form mensaje = new Mensaje("Socio validado con contraseña", true);
                     mensaje.ShowDialog();
                    contando2 = 0;
                    panelcontraseña.Visible = false;
                    nombreid = 100;
                    guiapagada();
                }
            }
            else
            {
                contraseñaerror.Visible = true;
            }

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Stop();
            labelsocio.Visible = true;

            
            verificationUserControl1.Hide();
            verificationUserControl2.Show();
            verificationUserControl2.Start();
        }

        private void textBoxcontraseña_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar == (char)Keys.Enter))
            {
                if (textBoxcontraseña.Text == contraseña)
                {
                    contraseñaerror.Visible = false;

                    if (contando1 >= 5)
                    {
                        labelusu.Visible = false;

                        verificationUserControl1.Samples.Clear();
                        verificationUserControl1.IsVerificationComplete = false;
                        verificationUserControl1.img = null;
                        textBoxcontraseña.Text = "";
                        panelcontraseña.Visible = false;
                        verificationUserControl1.FingerPrintPicture.Image = null;
                        verificationUserControl1.Stop();
                        contrausuario = true;
                        ValidateUsersocio();
                        timer2.Start();
                        Form mensaje = new Mensaje("Usuario validado con contraseñaa", true);
                        mensaje.ShowDialog();
                        contando1 = 0;

                    }
                    if (contando2 >= 5)
                    {
                        verificationUserControl2.Hide();
                        labelsocio.Visible = false;
                        contrasocio = true;
                        textBoxcontraseña.Text = "";

                        verificationUserControl2.Samples.Clear();
                        verificationUserControl2.IsVerificationComplete = false;
                        verificationUserControl2.img = null;

                        verificationUserControl2.FingerPrintPicture.Image = null;
                        verificationUserControl2.Stop();
                        Form mensaje = new Mensaje("Socio validado con contraseña", true);
                        mensaje.ShowDialog();
                        contando2 = 0;
                        panelcontraseña.Visible = false;
                        nombreid = 100;
                        guiapagada();
                    }
                }
                else
                {
                    contraseñaerror.Visible = true;
                }
            }

            }
    }
    }
