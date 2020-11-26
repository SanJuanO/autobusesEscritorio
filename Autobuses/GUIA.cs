using Autobuses.Planeacion;
using Autobuses.Utilerias;
using ConnectDB;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using Json.Net;
using LibPrintTicket;
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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Autobuses
{
    public partial class GUIA : Form
    {
        public database db;
        ResultSet res = null;
        private bool liberarguia = false;
        private string destino;
        private string origen;
        private string linea;
        private string hora;
        private string hora2;
        private double pkgastos = 0;
        private string folio;
        private string _clase;
        private double suma =0.0;
        private double iva =0.0;
        private double anticipo = 0.0;
        private double sumaconiva = 0;
        private double total = 0;
        private int cuantosdoble = 0;
        private string validadorr;
        private string desti;
        private string salida;
        private float com = 0;
        private float aportaciontotal=0 ;
        private string orig;
        private bool tick = false;
        private string contraseña="";
        private bool validarcontraseña = false;
        private float comisiontotal=0;
        private string hor;
        private string pkguia;
        private string lin;
   
        private string chofervalidador="";
        private string ec;
        private float diselruta = 0;
        private string impor;
        private string socio;
        private string disel;
        private string caset;
        private string ivaa;
        private string tota;
        private string vseden;
        private string anticipp;
        private string tot;
        private double gastomaximo = 0.0;
        private string usuario;
        private string chofer;
        private double txtimp = 0.0;
        private float sumatargeta = 0;

        private double txtsal = 0;
        private double txtcomptaq = 0;
        private double txtcomban = 0;
        private double txtaport = 0;
        private double txtdies = 0;
        private double txtcase = 0;
        private double txtiva = 0;
        private double txtvsed = 0;
        private double txtantic = 0;
        private double txttotal = 0.0;
        private double tarjetasr = 0;
        private double casetasr = 0;
        private double sueldor = 0;
        private double temporalimp = 0;
        private double sumar = 0;
        private Bitmap imagen;
        private string sucursal;
        private int boletos=0;
        private int espacio = 220;
        private int tamaño=0;
        private int ideboleto = 0;
        private string ruta;
        PrintDocument pd = new PrintDocument();
        PrintDocument pd2 = new PrintDocument();
        PrintDocument pdc = new PrintDocument();
        PrintDocument pdc2 = new PrintDocument();
        private bool tick2 = false;
        private bool completoes;
        private int mayor;
        private string eco;
        private string validar = "";
        private string validar2 = "";
        private float turnocosto = 0;
        private float aportaciones=0;
        private float salidacosto = 0;
        private float pasocosto = 0;
        private string fechaaa;
        private string cuantosturnos;
        private string sociousuario;
        private int IVA;
        private float comision;
        private int cantidadpagostarjeta = 0;

        private int cuantassalidas;
        private int cuantasdepaso;
        private string SOCIOPK;
        private string AUTOBUSPK;
        private string CHOFERPK;
        private string fechayears;
        private bool turn;
        private bool saliddd;
        private bool cualquierhuella = false;
        private int contando1 = 0;
            private  int contando2 = 0;
        private bool contusuario = false;
        private bool contchofer = false;
        byte[] fingerPrint;
        private string pkruta;

        private bool passo;
        private Bitmap imagensplash = new Bitmap(Autobuses.Properties.Resources.logotickets);
        private List<string> pkss = new List<string>();
        private string foliovendidos;
        private bool COBRARTARJETAS;
        private bool gastosruta;

        private void permisos()
        {

            if (LoginInfo.privilegios.Any(x => x == "Generar cualquier guia"))
            {
                cualquierhuella = true;

            }
      
            if (LoginInfo.privilegios.Any(x => x == "Guia con gastos de ruta"))
            {
                gastosruta = true;

            }
        }


        public GUIA(string _destino, string _origen, string _hora, string _linea, string _eco, int may, string chof)
        {
            InitializeComponent();
            CultureInfo ci = new CultureInfo("es-MX");
            CultureInfo.DefaultThreadCurrentCulture = ci;
            CultureInfo.DefaultThreadCurrentUICulture = ci;
            destino = _destino;
            hora = _hora;
            origen = _origen;
            linea = _linea;
            eco = _eco;
            mayor = may;
            pkruta = may.ToString();
            chofer = chof;
            verificationUserControl1.Hide();
            verificationUserControl2.Hide();
            this.Show();


        }

        private void siexiste()
        {
            string sql = "SELECT COUNT(PK1) AS CANTIDAD FROM TARJETAS_STATUS  WHERE FECHA=@FECHA AND ECO=@ECO ";
            db.PreparedSQL(sql);
            db.command.Parameters.AddWithValue("@FECHA", fechaaa);
            db.command.Parameters.AddWithValue("@ECO", eco);
            string existencia = "";
            res = db.getTable();
            if (res.Next())
            {
                existencia = res.Get("CANTIDAD");



                // cantidad();
            }

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
                sql = "SELECT CONVERT(FLOAT,VALOR) AS VALOR FROM VARIABLES  WHERE NOMBRE='COMISIONBANCO'";
                db.PreparedSQL(sql);


                res = db.getTable();
                if (res.Next())
                {
                 comision = res.GetFloat("VALOR");
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

        private void tarjetas_status()
        {
            try
            {
                string folioturno = GenerateRandom();

                if (saliddd == false)
                {
                    salidacosto = 0;
                }
                if (turn == false)
                {
                    turnocosto = 0;
                }
                if (passo == false)
                {
                    pasocosto = 0;
                }
                string sql = "INSERT INTO TARJETAS_STATUS" +
                    "(ECO,TARJETATURNO,COSTOTURNO,TARJETAPASO,COSTOPASO,TARJETASALIDA,COSTOSALIDA,HORASALIDA,FECHA,USUARIO,LINEA,FOLIO,ORIGEN)" +
                    "VALUES(@ECO,'TURNO',@COSTOTURNO,'PASO',@COSTOPASO,'SALIDA',@COSTOSALIDA,@HORASALIDA,@FECHA,@USUARIO,@LINEA,@FOLIO,@ORIGEN)";

                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@ECO", eco);
                db.command.Parameters.AddWithValue("@COSTOTURNO", turnocosto);
                db.command.Parameters.AddWithValue("@COSTOPASO", pasocosto);
                db.command.Parameters.AddWithValue("@COSTOSALIDA", salidacosto);
                db.command.Parameters.AddWithValue("@FECHA", DateTime.Now.ToString("dd/MM/yyyy"));
                db.command.Parameters.AddWithValue("@LINEA", linea);
                db.command.Parameters.AddWithValue("@USUARIO", LoginInfo.UserID);
                db.command.Parameters.AddWithValue("@HORASALIDA", hora);
                db.command.Parameters.AddWithValue("@FOLIO", folioturno);
                db.command.Parameters.AddWithValue("@ORIGEN", origen);


                db.execute();


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
                string funcion = "tarjetasstatus";
                Utilerias.LOG.write(_clase, funcion, error);


            }


        }
        private void imprimirlargo()
        {
            //Creamos una instancia d ela clase CrearTicket
           Ticket ticket = new Ticket();
            ticket.MaxChar = 45;
            ticket.FontName = "Arial";
            ticket.FontSize = 7;
            ticket.HeaderImage = imagensplash;//Por ejemplo
            ticket.AddHeaderLine("SUCURSAL:" + sucursal);
            ticket.AddHeaderLine("Folio:");
            ticket.AddHeaderLine("Autobus:" + eco);
            ticket.AddHeaderLine("Socio:" + socio);//Es el mio por si me quieren contactar ...
            ticket.AddHeaderLine("Conductor:" + chofer);
            ticket.AddHeaderLine("Usuario:" + LoginInfo.NombreID + LoginInfo.ApellidoID);
            ticket.AddHeaderLine("Salida:" + hora);
            ticket.AddHeaderLine("Origen:" + origen);
            ticket.AddHeaderLine("Destino:" + destino);
            ticket.AddHeaderLine("");
            ticket.AddHeaderLine("");

         
            //De aqui en adelante pueden formar su ticket a su gusto... Les muestro un ejemplo
            ticket.AddHeaderLine("ASIENTO "+ "NOMBRE " + "DESTINO " + "BOLETO " + "TARIFA " + "PRECIO");

            foreach (DataGridViewRow fila in datagridvendidos.Rows)//dgvLista es el nombre del datagridview
            {
                string asi = fila.Cells["asientoname"].Value.ToString();
                string nom = fila.Cells["nombrename"].Value.ToString().Substring(0, 7);
                string dat = fila.Cells["destinoname"].Value.ToString().Substring(0, 7);
                string bol = fila.Cells["Boletoname"].Value.ToString();
                    string tar = fila.Cells["tarifaname"].Value.ToString();
                string pre = fila.Cells["preciosname"].Value.ToString();
                ticket.AddSubHeaderLine(asi+nom+ dat+ bol+ tar+ pre);
           }

         
            ticket.AddTotal("         IMPORTE......",Utilerias.Utilerias.formatCurrency( txtimp));
            ticket.AddTotal("         Tarjetas.....", Utilerias.Utilerias.formatCurrency(tarjetasr));
            ticket.AddTotal("         Anticipo.....", Utilerias.Utilerias.formatCurrency(anticipo));
            ticket.AddTotal("         Com. Banco...", Utilerias.Utilerias.formatCurrency(com));
            ticket.AddTotal("         Aportaciones.", Utilerias.Utilerias.formatCurrency(aportaciones));
            ticket.AddTotal("         Iva..........", Utilerias.Utilerias.formatCurrency(iva));
            ticket.AddTotal("         Total........", Utilerias.Utilerias.formatCurrency(txttotal));

            ticket.PrintTicket("EPSON TM-T20II"); //Nombre de la impresora de tickets﻿




        }
        private void cantidad()
        {
            try
            {
                string sql = "SELECT COUNT(PK) AS C FROM VCORRIDAS_DIA_1 WHERE FECHA=@FECHA AND AUTOBUS=@ECO AND ESCALA=1 OR COMPLETO=1";
                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@FECHA", fechayears);
                db.command.Parameters.AddWithValue("@ECO", eco);

                res = db.getTable();
                if (res.Next())
                {
                    cuantassalidas = res.GetInt("C");
                }


                sql = "SELECT COUNT(PK) AS C FROM VCORRIDAS_DIA_1 WHERE FECHA=@FECHA AND AUTOBUS=@ECO AND ESCALA=0 AND COMPLETO=0";
                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@FECHA", fechayears);
                db.command.Parameters.AddWithValue("@ECO", eco);

                res = db.getTable();
                if (res.Next())
                {
                    cuantasdepaso = res.GetInt("C");
                }

                tarjetas_status();
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
                string funcion = "cantidad";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }
        private void tarjetas()
        {
            try
            {
                string sql = "SELECT COSTO FROM TARJETAS  WHERE LINEA=@LINEA AND TARJETA='TURNO' AND ORIGEN=@ORIGEN AND DESTINO=@DESTINO";
                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@LINEA", linea);
                db.command.Parameters.AddWithValue("@ORIGEN", origen);
                db.command.Parameters.AddWithValue("@DESTINO", destino);


                res = db.getTable();
                if (res.Next())
                {
                    turnocosto = res.GetInt("COSTO");
                }
                string sqlA = "SELECT VALOR FROM VARIABLES  WHERE NOMBRE='APORTACIONES' ";
                db.PreparedSQL(sqlA);
             

                res = db.getTable();
                if (res.Next())
                {
                    aportaciones = res.GetInt("VALOR");
                    aportaciones = aportaciones * boletos;
                    textBoxaportaciones.Text =Utilerias.Utilerias.formatCurrency( aportaciones);
                }
                sql = "SELECT COSTO FROM TARJETAS  WHERE LINEA=@LINEA AND TARJETA='SALIDA' AND ORIGEN=@ORIGEN AND DESTINO=@DESTINO";
                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@LINEA", linea);
                db.command.Parameters.AddWithValue("@ORIGEN", origen);
                db.command.Parameters.AddWithValue("@DESTINO", destino);


                res = db.getTable();
                if (res.Next())
                {
                    salidacosto = res.GetInt("COSTO");
                }

          

                sql = "SELECT COSTO FROM TARJETAS  WHERE LINEA=@LINEA AND TARJETA='PASO' AND ORIGEN=@ORIGEN AND DESTINO=@DESTINO<";
                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@LINEA", linea);
                db.command.Parameters.AddWithValue("@ORIGEN", origen);
                db.command.Parameters.AddWithValue("@DESTINO", destino);


                res = db.getTable();
                if (res.Next())
                {
                    pasocosto = res.GetInt("COSTO");
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

        private void GUIACOMPROBACION()
        {
            try
            {
                fechaaa = DateTime.Now.ToString("dd/MM/yyyy");
                string sql = "SELECT GUIA, BLOQUEADO  FROM CORRIDAS_DIA  WHERE PK=@PK";


                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@PK", mayor);

                int n = 0;


                res = db.getTable();
                if (res.Next())
                {
                    validar = res.Get("GUIA");
                    validar2 = res.Get("BLOQUEADO");

                    if (validar == "True" || validar2 == "True")
                    {
                        MessageBox.Show("La guia ya se genero");
                        this.Close();
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
                string funcion = "guiacomprobacion";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }

        private void sumatoria()
        {
            try
            {
                const int columna = 5;
                foreach (DataGridViewRow row in datagridvendidos.Rows)
                {
                    suma += float.Parse(row.Cells[columna].Value.ToString());
                }

                txttotal = (suma * 100 / 116);
                txtimp = suma;
                iva = suma - txttotal;
                if (cantidadpagostarjeta > 0)
                {
                     com = ((comision) * comisiontotal);
                    com = com / 100;
                    com = (com * IVA)/100;
                    txttotal = (txttotal - com);
                    comisionname.Text = Utilerias.Utilerias.formatCurrency(com);
 }
                else
                {
                    comisionname.Text = Utilerias.Utilerias.formatCurrency(com);

                }


                textBoxiva.Text = Utilerias.Utilerias.formatCurrency(iva);
                textBoximp.Text = Utilerias.Utilerias.formatCurrency(txtimp);
                textBoxtotal.Text = Utilerias.Utilerias.formatCurrency(txttotal);

            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error sumatoria, intente de nuevo.");
                string funcion = "sumatoria";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }



        public String GenerateRandom()
        {
            string foliotemp = "";
            try
            {
                string sql = "SELECT dbo.f_Get_Folio_Guia() as FOLIO";


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
                string funcion = "generarandom";
                Utilerias.LOG.write(_clase, funcion, error);


            }
            return foliotemp;
        }


        private void infoboletos()
        {
            try
            {
                datagridvendidos.Rows.Clear();
                fechaaa = DateTime.Now.ToString("dd/MM/yyyy");

                string sql = "SELECT ESCANEADO, ASIENTO,PASAJERO,DESTINOBOLETO,FOLIO, TARIFA, PRECIO" +
                    " FROM VENDIDOS WHERE ORIGEN=@ORIGEN AND FECHA=@FECHA AND ECO=@ECO AND SALIDA=@SALIDA  AND STATUS='VENDIDO'";


                db.PreparedSQL(sql);

                db.command.Parameters.AddWithValue("@ORIGEN", origen);
                db.command.Parameters.AddWithValue("@FECHA", fechaaa);
                db.command.Parameters.AddWithValue("@ECO", eco);
                db.command.Parameters.AddWithValue("@SALIDA", hora);

                int n = 0;


                res = db.getTable();
                pkss.Clear();
                int es = 0;
                while (res.Next())
                {
                    n = datagridvendidos.Rows.Add();

                   
                   bool escaneadotemporal= res.GetBool("ESCANEADO");
                    datagridvendidos.Rows[n].Cells[0].Value = res.Get("ASIENTO");
                    datagridvendidos.Rows[n].Cells[1].Value = res.Get("PASAJERO");
                    string tarifatempo = res.Get("TARIFA");
                    tarifatempo += "         ";
                    datagridvendidos.Rows[n].Cells[2].Value = (tarifatempo.Substring(0,8));
                    datagridvendidos.Rows[n].Cells[3].Value = res.Get("FOLIO");
                    datagridvendidos.Rows[n].Cells[4].Value = res.Get("TARIFA");
                    datagridvendidos.Rows[n].Cells[5].Value = res.Get("PRECIO");
                    if (escaneadotemporal)
                    {
                        es++;
                    }

                }
                textboxescaneado.Text=es.ToString();

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
        }
        private void llenardatosdeboletos()
        {
            try
            {
                string sqln = "SELECT PK,FOLIO,ASIENTO,PASAJERO,DESTINOBOLETO,TARIFA,PRECIO,FECHA,SUCURSAL,ESCANEADO, PKCORRIDA, CONVERT(VARCHAR(8), SALIDA ) AS SALIDA,FORMADEPAGO FROM VENDIDOS WHERE STATUS='VENDIDO' AND FECHA=@FECHA AND SALIDA=@SALIDA AND LINEA=@LINEA AND  " +
                    "DESTINO=@DESTINO AND ORIGEN=@ORIGEN AND  PKGUIA IS NULL ORDER BY ASIENTO";
                string fechatemp = hora.Substring(0,10);
                DateTime fec = DateTime.Parse(fechatemp);
                 fechatemp = ((fec.ToString()).Substring(0,10)).Substring(0,10);

                db.PreparedSQL(sqln);

                db.command.Parameters.AddWithValue("@DESTINO", destino);
                db.command.Parameters.AddWithValue("@ORIGEN", origen);
                db.command.Parameters.AddWithValue("@SALIDA", hora);

                db.command.Parameters.AddWithValue("@LINEA", linea);
               
                db.command.Parameters.AddWithValue("@FECHA",fechatemp);

                int n = 0;

                cantidadpagostarjeta = 0;

                res = db.getTable();
                int es = 0;

                string formadepago = "";
                while (res.Next())
                {
                    n = datagridvendidos.Rows.Add();
                    pkruta = res.Get("PKCORRIDA");

                    datagridvendidos.Rows[n].Cells[0].Value = res.Get("ASIENTO");
                    datagridvendidos.Rows[n].Cells[1].Value = res.Get("PASAJERO");
                    datagridvendidos.Rows[n].Cells[2].Value = res.Get("DESTINOBOLETO");
                    datagridvendidos.Rows[n].Cells[3].Value = res.Get("FOLIO");
                    foliovendidos += (string.IsNullOrEmpty(foliovendidos) ? "'" + res.Get("PK") + "'" : ",'" + res.Get("PK") + "'");

                    datagridvendidos.Rows[n].Cells[4].Value = res.Get("TARIFA");
                    datagridvendidos.Rows[n].Cells[5].Value = res.GetFloat("PRECIO");
                    datagridvendidos.Rows[n].Cells["precioname"].Value = Utilerias.Utilerias.formatCurrency(res.GetFloat("PRECIO"));
                    formadepago = res.Get("FORMADEPAGO");
                    salida = res.Get("FECHA");
                    hora2 = res.Get("SALIDA");
                    sucursal = res.Get("SUCURSAL");

                    if (formadepago != "Efectivo")
                    {
                        cantidadpagostarjeta+=1;
                        comisiontotal += (res.GetFloat("PRECIO"));
                    }

                    bool escaneadotemporal = res.GetBool("ESCANEADO");

                    if (escaneadotemporal)
                    {
                        es++;
                    }



                }
                boletos = datagridvendidos.Rows.Count;
                textboxescaneado.Text = es.ToString();


                n = n + 1;
                textBoxboletos.Text = boletos.ToString();
               

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
                string funcion = "llenadodeboletos";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }






        private void botonimprimir(object sender, EventArgs e)
        {
            try
            {
                buttonimp.Enabled = false;
                txttotal = txttotal - anticipo;
                consultarconductoresdesocio();
                checkBoxpaso.Enabled = false;
                checkBoxsalida.Enabled = false;
                checkboxturno.Enabled = false;
                textBoxanticipo.Enabled = false;
                //verificationUserControl1.Hide();
                //verificationUserControl2.Hide();
                //verificationUserControl1.Show();
                //verificationUserControl2.Show();
                //labelusu.Visible = true;
                //labelsocio.Visible = true;
                //verificationUserControl1.Samples.Clear();
                //verificationUserControl1.IsVerificationComplete = false;
                //verificationUserControl1.img = null;
                //verificationUserControl2.Samples.Clear();
                //verificationUserControl2.IsVerificationComplete = false;
                //verificationUserControl2.img = null;
               // ValidateUser();
                //verificationUserControl1.limpiarhuella();
                choferdefault();
                //huella cambio
                panelconductor.Visible = true;
                panelcontraseña.Visible = false;
                //verificationUserControl1.Focus();
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error boton imprimir, intente de nuevo.");
                string funcion = "botonimprimir";
                Utilerias.LOG.write(_clase, funcion, error);

            }
        }
        void ValidateUser()
        {
            fingerPrint = db.selectByteArray("SELECT HUELLA FROM USUARIOS WHERE USUARIO='" + LoginInfo.UserID + "'");
            verificationUserControl1.Samples.Add(new DPFP.Template(new MemoryStream(fingerPrint)), null);
        }
        void Validateconductor(string pkconductor)
        {
            fingerPrint = db.selectByteArray("SELECT HUELLA FROM CHOFERES WHERE PK='" +pkconductor + "'");
            verificationUserControl2.Samples.Add(new DPFP.Template(new MemoryStream(fingerPrint)), null);
        }
        void ValidateUsersocio()
        {
            try
            {
                if (linea =="SUPRA") {
                    for (int i = 0; i< LoginInfo.fingerPrintchoferessupra.Count ; i++)
                    {
                        verificationUserControl2.Samples.Add(new DPFP.Template(new MemoryStream(LoginInfo.fingerPrintchoferessupra[i])), null);

                    }
                }
                if (linea == "Ejecutivo Centro")
                {
                    for (int i = 0;i< LoginInfo.fingerPrintchoferesejecutivo.Count ; i++)
                    {
                        verificationUserControl2.Samples.Add(new DPFP.Template(new MemoryStream(LoginInfo.fingerPrintchoferesejecutivo[i])), null);

                    }
                }

            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error boton imprimir, intente de nuevo.");
                string funcion = "validateusersocio";
                Utilerias.LOG.write(_clase, funcion, error);

            }
        }
        private void VerificationUserControl1_VerificationStatusChanged(object sender, StatusChangedEventArgs e)
        {
            try
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

                        verificationUserControl1.Invoke(new Action(() =>
                        {
                            contando1 = 0;
                            panelcontraseña.Visible = false;

                            verificationUserControl1.Stop();
                            verificationUserControl1.Hide();
                            labelusu.Visible = false;
                            verificationUserControl1.Samples.Clear();
                            verificationUserControl1.IsVerificationComplete = false;
                            verificationUserControl1.img = null;
                            //verificationUserControl1.Init();
                            verificationUserControl2.limpiarhuella();

                            verificationUserControl1.FingerPrintPicture.Image = null;

                            panelconductor.Visible = true;
                            choferdefault();
                            verificationUserControl2.Focus();

                        }));

                    }
                }
                else
                {
                    if (verificationUserControl1.IsVerificationComplete && verificationUserControl1.Validate())
                    {
                        verificationUserControl1.Hide();
                        verificationUserControl1.Hide();
                        labelusu.Visible = false;
                        verificationUserControl1.Samples.Clear();
                        verificationUserControl1.IsVerificationComplete = false;
                        verificationUserControl1.img = null;
                        //verificationUserControl1.Init();
                        verificationUserControl2.Start();
                        verificationUserControl1.FingerPrintPicture.Image = null;

                        panelconductor.Visible = true;

                    }
                }
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error qr, intente de nuevo.");
                string funcion = "verifcationusercontrol";
                Utilerias.LOG.write(_clase, funcion, error);


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
                verificacion.Start();
                verificacion.FingerPrintPicture.Image = null;
                //this.Close();
            }

        }
        private void VerificationUserControl2_VerificationStatusChanged(object sender, StatusChangedEventArgs e)
        {
            try
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
                        verificationUserControl2.Invoke(new Action(() =>
                        {

                            verificationUserControl2.Hide();
                            labelsocio.Visible = false;
                            verificationUserControl2.Samples.Clear();
                            verificationUserControl2.IsVerificationComplete = false;
                            verificationUserControl2.img = null;
                            verificationUserControl2.Init();
                            //verificationUserControl2.Start();
                            verificationUserControl2.FingerPrintPicture.Image = null;
                           

                            imprimirr();
                            

                        }));

                    }
                }
                else
                {
                    if (verificationUserControl2.IsVerificationComplete && verificationUserControl1.Validate())
                    {
                     
                        verificationUserControl2.Hide();
                        verificationUserControl2.Samples.Clear();
                        verificationUserControl2.IsVerificationComplete = false;
                        verificationUserControl2.img = null;
                        verificationUserControl2.Init();
                        verificationUserControl2.Start();
                        verificationUserControl2.FingerPrintPicture.Image = null;
                  
                        imprimirr();
                    }
                }
            }     
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error qr, intente de nuevo.");
                string funcion = "verificationusercontrol2";
                Utilerias.LOG.write(_clase, funcion, error);


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
                verificacion.Start();
                verificacion.FingerPrintPicture.Image = null;
                //this.Close();
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
                MessageBox.Show("Ocurrio un Error qr, intente de nuevo.");
                string funcion = "codigoqr";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }


        void llenarticket(object sender, PrintPageEventArgs e)
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
                g.DrawImage(imagensplash, 0, 0);
                espacio = 0;
                g.DrawString("Fecha :", fBody7, sb, 180, espacio);
                g.DrawString(DateTime.Now.ToShortDateString(), fBody7, sb, 215, espacio);
                g.DrawString("Hora :", fBody7, sb, 180, espacio + 20);
                g.DrawString(DateTime.Now.ToShortTimeString(), fBody7, sb, 210, espacio + 20);
                espacio = espacio + 55;
                g.DrawString("GUIA", fBody18, sb, 150, espacio);
                g.DrawString("Folio: " + folio, fBody, sb, 0, espacio);
                espacio = espacio + 20;
                g.DrawString("Autobus: " + eco, fBody, sb, 0, espacio);
                espacio = espacio + 20;
                g.DrawString("Salida: " + hora, fBody10, sb, 0, espacio);
                espacio = espacio + 20;
                g.DrawString("Origen: " + origen, fBody10, sb, 0, espacio);
                espacio = espacio + 20;

                g.DrawString("Destino: " + destino, fBody10, sb, 0, espacio);
                espacio = espacio + 20;
                g.DrawString("Chofer: " + chofer, fBody10, sb, 0, espacio);
                espacio = espacio + 20;
                g.DrawString("Socio: " + socio, fBody10, sb, 0, espacio);
                espacio = espacio + 20;
                g.DrawString("Usuario: " + LoginInfo.NombreID + " " + LoginInfo.ApellidoID, fBody10, sb, 0, espacio);
                espacio = espacio + 20;
                g.DrawRectangle(Pens.Black, 0, espacio, 268, 12);
                g.DrawString("ASIENTO", fBody5, sb, 0, espacio);
                g.DrawString("NOMBRE", fBody5, sb, 40, espacio);
                g.DrawString("DESTINO", fBody5, sb, 90, espacio);
                g.DrawString("BOLETO", fBody5, sb, 145, espacio);
                g.DrawString("TARIFA", fBody5, sb, 190, espacio);
                g.DrawString("PRECIO", fBody5, sb, 235, espacio);
                espacio = espacio + 20;

                for (int i = 0; i < datagridvendidos.RowCount; i++)
                {

                    string nom = datagridvendidos.Rows[i].Cells[1].Value.ToString().Substring(0, 12);
                    g.DrawString(datagridvendidos.Rows[i].Cells[0].Value.ToString(), fBody5, sb, 10, espacio);
                    g.DrawString(nom, fBody5, sb, 30, espacio);
                    g.DrawString(datagridvendidos.Rows[i].Cells[2].Value.ToString(), fBody5, sb, 90, espacio);
                    g.DrawString(datagridvendidos.Rows[i].Cells[3].Value.ToString(), fBody5, sb, 145, espacio);
                    g.DrawString(datagridvendidos.Rows[i].Cells[4].Value.ToString(), fBody5, sb, 190, espacio);
                    g.DrawString(datagridvendidos.Rows[i].Cells["precioname"].Value.ToString(), fBody5, sb, 235, espacio);

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

                espacio = espacio + 10;

                g.DrawString("Importe:", fBody7, sb, 150, espacio);
                g.DrawString(Utilerias.Utilerias.formatCurrency(txtimp), fBody7, sb, 210, espacio);
                g.DrawString("Boletos: " + boletos, fBody7, sb, 0, espacio);
                g.DrawString("Abordo: " + textboxescaneado.Text, fBody7, sb, 60, espacio);

                //g.DrawString("Usuario: "+LoginInfo.NombreID, fBody7, sb, 0, espacio);
                //g.DrawString("Com. Taq", fBody7, sb, 150, espacio);
                // g.DrawString("$0.00", fBody7, sb, 210, espacio);
                espacio = espacio + 15;

                g.DrawString("Com. Banco", fBody7, sb, 150, espacio);
                g.DrawString(Utilerias.Utilerias.formatCurrency(com), fBody7, sb, 210, espacio);

                // g.DrawString("Socio: " + socio, fBody7, sb, 0, espacio);
                //g.DrawString("Aportación", fBody7, sb, 150, espacio);
                //g.DrawString("$0.00", fBody7, sb, 210, espacio);
                espacio = espacio + 15;
                g.DrawImage(imagen, 10, espacio);
                if (LoginInfo.privilegios.Any(x => x == "Aportaciones"))
                {
                    g.DrawString("Aportaciones", fBody7, sb, 150, espacio);
                    g.DrawString("-" + Utilerias.Utilerias.formatCurrency(aportaciones), fBody7, sb, 210, espacio);
                    espacio = espacio + 15;

                }
                if (gastosruta)
                {
                    g.DrawString("Diesel", fBody7, sb, 150, espacio);
                    g.DrawString("$" + diselruta, fBody7, sb, 210, espacio);
                    espacio = espacio + 15;
                    g.DrawString("Casetas", fBody7, sb, 150, espacio);
                    g.DrawString("$" + casetasr, fBody7, sb, 210, espacio);
                    espacio = espacio + 15;

                    g.DrawString("Tarjetas", fBody7, sb, 150, espacio);
                    g.DrawString("-$" + tarjetasr, fBody7, sb, 210, espacio);
                    espacio = espacio + 15;
                }
                else
                {
                    g.DrawString("T. salida", fBody7, sb, 150, espacio);
                    g.DrawString("-" + Utilerias.Utilerias.formatCurrency(salidacosto), fBody7, sb, 210, espacio);
                    espacio = espacio + 15;
                    g.DrawString("T. turno", fBody7, sb, 150, espacio);
                    g.DrawString("-" + Utilerias.Utilerias.formatCurrency(turnocosto), fBody7, sb, 210, espacio);
                    espacio = espacio + 15;
                    g.DrawString("T. de Paso", fBody7, sb, 150, espacio);
                    g.DrawString("-" + Utilerias.Utilerias.formatCurrency(pasocosto), fBody7, sb, 210, espacio);
                    espacio = espacio + 15;

                }
                g.DrawString("IVA", fBody7, sb, 150, espacio);
                g.DrawString("-" + Utilerias.Utilerias.formatCurrency(iva), fBody7, sb, 210, espacio);

                //g.DrawString("V.SEDENA", fBody7, sb, 150, espacio);
                //g.DrawString("$" + vseden, fBody7, sb, 210, espacio);
                espacio = espacio + 15;
                g.DrawString("Gastos:", fBody7, sb, 150, espacio);
                g.DrawString("-" + Utilerias.Utilerias.formatCurrency(anticipo), fBody7, sb, 210, espacio);
                espacio = espacio + 15;
                g.DrawString("Total", fBody7, sb, 150, espacio);

                g.DrawString(Utilerias.Utilerias.formatCurrency(txttotal), fBody7, sb, 210, espacio);
           
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

            tick2=true;
            pdc2.DefaultPageSettings.PaperSize = new PaperSize("Custom", 315, tamaño-(ideboleto*20));
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
                for (int i = ideboleto; i < datagridvendidos.RowCount; i++)
                {

                    string nom = datagridvendidos.Rows[i].Cells[1].Value.ToString().Substring(0, 12);
                    g.DrawString(datagridvendidos.Rows[i].Cells[0].Value.ToString(), fBody5, sb, 10, espacio);
                    g.DrawString(nom, fBody5, sb, 30, espacio);
                    g.DrawString(datagridvendidos.Rows[i].Cells[2].Value.ToString(), fBody5, sb, 90, espacio);
                    g.DrawString(datagridvendidos.Rows[i].Cells[3].Value.ToString(), fBody5, sb, 145, espacio);
                    g.DrawString(datagridvendidos.Rows[i].Cells[4].Value.ToString(), fBody5, sb, 190, espacio);
                    g.DrawString(datagridvendidos.Rows[i].Cells["precioname"].Value.ToString(), fBody5, sb, 235, espacio);

                    espacio = espacio + 20;
                  

                }

              
                espacio = espacio + 10;

                g.DrawString("Importe:", fBody7, sb, 150, espacio);
                g.DrawString(Utilerias.Utilerias.formatCurrency(txtimp), fBody7, sb, 210, espacio);
                g.DrawString("Boletos: " + boletos, fBody7, sb, 0, espacio);
                //g.DrawString("Usuario: "+LoginInfo.NombreID, fBody7, sb, 0, espacio);
                //g.DrawString("Com. Taq", fBody7, sb, 150, espacio);
                // g.DrawString("$0.00", fBody7, sb, 210, espacio);
                espacio = espacio + 15;

                g.DrawString("Com. Banco", fBody7, sb, 150, espacio);
                g.DrawString(Utilerias.Utilerias.formatCurrency(com), fBody7, sb, 210, espacio);

                // g.DrawString("Socio: " + socio, fBody7, sb, 0, espacio);
                //g.DrawString("Aportación", fBody7, sb, 150, espacio);
                //g.DrawString("$0.00", fBody7, sb, 210, espacio);
                espacio = espacio + 15;
                g.DrawImage(imagen, 10, espacio);
                if (LoginInfo.privilegios.Any(x => x == "Aportaciones"))
                {
                    g.DrawString("Aportaciones", fBody7, sb, 150, espacio);
                    g.DrawString("-" +Utilerias.Utilerias.formatCurrency(aportaciones), fBody7, sb, 210, espacio);
                    espacio = espacio + 15;

                }
                if (gastosruta)
                {
                    g.DrawString("Diesel", fBody7, sb, 150, espacio);
                    g.DrawString("$" + diselruta, fBody7, sb, 210, espacio);
                    espacio = espacio + 15;
                    g.DrawString("Casetas", fBody7, sb, 150, espacio);
                    g.DrawString("$" + casetasr, fBody7, sb, 210, espacio);
                    espacio = espacio + 15;

                    g.DrawString("Tarjetas", fBody7, sb, 150, espacio);
                    g.DrawString("-$" + tarjetasr, fBody7, sb, 210, espacio);
                    espacio = espacio + 15;
                }
                else
                {
                    g.DrawString("T. salida", fBody7, sb, 150, espacio);
                    g.DrawString("-" + Utilerias.Utilerias.formatCurrency(salidacosto), fBody7, sb, 210, espacio);
                    espacio = espacio + 15;
                    g.DrawString("T. turno", fBody7, sb, 150, espacio);
                    g.DrawString("-" + Utilerias.Utilerias.formatCurrency(turnocosto), fBody7, sb, 210, espacio);
                    espacio = espacio + 15;
                    g.DrawString("T. de Paso", fBody7, sb, 150, espacio);
                    g.DrawString("-" + Utilerias.Utilerias.formatCurrency(pasocosto), fBody7, sb, 210, espacio);
                    espacio = espacio + 15;

                }
                g.DrawString("IVA", fBody7, sb, 150, espacio);
                g.DrawString("-" + Utilerias.Utilerias.formatCurrency(iva), fBody7, sb, 210, espacio);

                //g.DrawString("V.SEDENA", fBody7, sb, 150, espacio);
                //g.DrawString("$" + vseden, fBody7, sb, 210, espacio);
                espacio = espacio + 15;
                g.DrawString("Gastos:", fBody7, sb, 150, espacio);
                g.DrawString("-" + Utilerias.Utilerias.formatCurrency(anticipo), fBody7, sb, 210, espacio);
                espacio = espacio + 15;
                g.DrawString("Total", fBody7, sb, 150, espacio);

                g.DrawString(Utilerias.Utilerias.formatCurrency(txttotal), fBody7, sb, 210, espacio);

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

                g.DrawString("Importe:", fBody7, sb, 150, espacio);
                g.DrawString(Utilerias.Utilerias.formatCurrency(txtimp), fBody7, sb, 210, espacio);
                g.DrawString("Boletos: " + boletos, fBody7, sb, 0, espacio);
                //g.DrawString("Usuario: "+LoginInfo.NombreID, fBody7, sb, 0, espacio);
                //g.DrawString("Com. Taq", fBody7, sb, 150, espacio);
                // g.DrawString("$0.00", fBody7, sb, 210, espacio);
                espacio = espacio + 15;

                g.DrawString("Com. Banco", fBody7, sb, 150, espacio);
                g.DrawString(Utilerias.Utilerias.formatCurrency(com), fBody7, sb, 210, espacio);

                // g.DrawString("Socio: " + socio, fBody7, sb, 0, espacio);
                //g.DrawString("Aportación", fBody7, sb, 150, espacio);
                //g.DrawString("$0.00", fBody7, sb, 210, espacio);
                espacio = espacio + 15;
                g.DrawImage(imagen, 10, espacio);
                if (LoginInfo.privilegios.Any(x => x == "Aportaciones"))
                {
                    g.DrawString("Aportaciones", fBody7, sb, 150, espacio);
                    g.DrawString("-" +Utilerias.Utilerias.formatCurrency( aportaciones), fBody7, sb, 210, espacio);
                    espacio = espacio + 15;

                }
                if (gastosruta)
                {
                    g.DrawString("Diesel", fBody7, sb, 150, espacio);
                    g.DrawString("$" + diselruta, fBody7, sb, 210, espacio);
                    espacio = espacio + 15;
                    g.DrawString("Casetas", fBody7, sb, 150, espacio);
                    g.DrawString("$" + casetasr, fBody7, sb, 210, espacio);
                    espacio = espacio + 15;

                    g.DrawString("Tarjetas", fBody7, sb, 150, espacio);
                    g.DrawString("-$" + tarjetasr, fBody7, sb, 210, espacio);
                    espacio = espacio + 15;
                }
                else
                {
                    g.DrawString("T. salida", fBody7, sb, 150, espacio);
                    g.DrawString("-" + Utilerias.Utilerias.formatCurrency(salidacosto), fBody7, sb, 210, espacio);
                    espacio = espacio + 15;
                    g.DrawString("T. turno", fBody7, sb, 150, espacio);
                    g.DrawString("-" + Utilerias.Utilerias.formatCurrency(turnocosto), fBody7, sb, 210, espacio);
                    espacio = espacio + 15;
                    g.DrawString("T. de Paso", fBody7, sb, 150, espacio);
                    g.DrawString("-" + Utilerias.Utilerias.formatCurrency(pasocosto), fBody7, sb, 210, espacio);
                    espacio = espacio + 15;

                }
                g.DrawString("IVA", fBody7, sb, 150, espacio);
                g.DrawString("-" + Utilerias.Utilerias.formatCurrency(iva), fBody7, sb, 210, espacio);

                //g.DrawString("V.SEDENA", fBody7, sb, 150, espacio);
                //g.DrawString("$" + vseden, fBody7, sb, 210, espacio);
                espacio = espacio + 15;
                g.DrawString("Gastos:", fBody7, sb, 150, espacio);
                g.DrawString("-" + Utilerias.Utilerias.formatCurrency(anticipo), fBody7, sb, 210, espacio);
                espacio = espacio + 15;
                g.DrawString("Total", fBody7, sb, 150, espacio);

                g.DrawString(Utilerias.Utilerias.formatCurrency(txttotal), fBody7, sb, 210, espacio);

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

        void llenarticketinferior(object sender, PrintPageEventArgs e)
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
                g.DrawString("Fecha :", fBody7, sb, 180, espacio);
                g.DrawString(DateTime.Now.ToShortDateString(), fBody7, sb, 215, espacio);
                g.DrawString("Hora :", fBody7, sb, 180, espacio + 20);
                g.DrawString(DateTime.Now.ToShortTimeString(), fBody7, sb, 210, espacio + 20);
                espacio = espacio + 10;
                g.DrawString("GUIA " , fBody10, sb, 0, espacio);
                espacio = espacio + 25;
                g.DrawString("SUCURSAL: " + sucursal, fBody10, sb, 0, espacio);
                espacio = espacio + 25;
                g.DrawString("Folio:" + folio, fBody7, sb, 0, espacio);
                g.DrawString("Boletos: " + boletos, fBody, sb, 150, espacio);
                espacio = espacio + 15;
                g.DrawString("Autobus: " + eco, fBody7, sb, 0, espacio);
                espacio = espacio + 15;
                g.DrawString("Socio: " + socio, fBody7, sb, 0, espacio);
                espacio = espacio + 15;
                g.DrawString("Chofer: " + chofer, fBody7, sb, 0, espacio);
                espacio = espacio + 15;
                g.DrawString("Usuario: " + LoginInfo.NombreID + " " + LoginInfo.ApellidoID, fBody7, sb, 0, espacio);
                espacio = espacio + 15;
                g.DrawString("Salida: " + salida + " " + hora, fBody7, sb, 0, espacio);
                //g.DrawString("Com. Taq", fBody7, sb, 150, espacio);
                //g.DrawString("$0.00", fBody7, sb, 210, espacio);
                espacio = espacio + 15;
                g.DrawString("Origen: " + origen, fBody7, sb, 0, espacio);
                
                espacio = espacio + 15;
                g.DrawString("Destino: " + destino, fBody7, sb, 0, espacio);
                espacio = espacio + 25;
                //g.DrawString("Diesel", fBody7, sb, 150, espacio);
                //g.DrawString("$" + disel, fBody7, sb, 210, espacio);
                g.DrawImage(imagen, 10, espacio);
                //g.DrawString("Casetas", fBody7, sb, 150, espacio);
                //g.DrawString("$" + caset, fBody7, sb, 210, espacio);
                g.DrawString("Importe", fBody7, sb, 150, espacio);
                g.DrawString(Utilerias.Utilerias.formatCurrency(txtimp), fBody7, sb, 210, espacio);
                espacio = espacio + 15;
                g.DrawString("Aportación", fBody7, sb, 150, espacio);

                g.DrawString("$0.00", fBody7, sb, 210, espacio);
                espacio = espacio + 15;

                g.DrawString("Com. Banco", fBody7, sb, 150, espacio);
                g.DrawString(Utilerias.Utilerias.formatCurrency(com), fBody7, sb, 210, espacio);
                espacio = espacio + 15;

                if (gastosruta)
                {
                    g.DrawString("Diesel", fBody7, sb, 150, espacio);
                    g.DrawString("$" + diselruta, fBody7, sb, 210, espacio);
                    espacio = espacio + 15;
                    g.DrawString("Casetas", fBody7, sb, 150, espacio);
                    g.DrawString("$" + casetasr, fBody7, sb, 210, espacio);
                    espacio = espacio + 15;
                    g.DrawString("Tarjetas", fBody7, sb, 150, espacio);
                    g.DrawString("-$" + tarjetasr, fBody7, sb, 210, espacio);
                    espacio = espacio + 15;
                }
                else
                {
                    g.DrawString("T. salida", fBody7, sb, 150, espacio);
                    g.DrawString("-" + Utilerias.Utilerias.formatCurrency(salidacosto), fBody7, sb, 210, espacio);
                    espacio = espacio + 15;
                    g.DrawString("T. turno", fBody7, sb, 150, espacio);
                    g.DrawString("-" + Utilerias.Utilerias.formatCurrency(turnocosto), fBody7, sb, 210, espacio);
                    espacio = espacio + 15;
                    g.DrawString("T. de Paso", fBody7, sb, 150, espacio);
                    g.DrawString("-" + Utilerias.Utilerias.formatCurrency( pasocosto), fBody7, sb, 210, espacio);
                    espacio = espacio + 15;

                }
                if (LoginInfo.privilegios.Any(x => x == "Aportaciones"))
                {
                    g.DrawString("Aportaciones", fBody7, sb, 150, espacio);
                    g.DrawString("-" +Utilerias.Utilerias.formatCurrency(aportaciones), fBody7, sb, 210, espacio);
                    espacio = espacio + 15;

                }
                g.DrawString("IVA", fBody7, sb, 150, espacio);
                g.DrawString("-" + Utilerias.Utilerias.formatCurrency(iva), fBody7, sb, 210, espacio);

                //g.DrawString("V.SEDENA", fBody7, sb, 150, espacio);
                //g.DrawString("$" + vseden, fBody7, sb, 210, espacio);
                espacio = espacio + 15;
                g.DrawString("Gastos:", fBody7, sb, 150, espacio);
                g.DrawString("-" + Utilerias.Utilerias.formatCurrency( anticipo), fBody7, sb, 210, espacio);
                espacio = espacio + 15;
                g.DrawString("Total", fBody7, sb, 150, espacio);
                g.DrawString(Utilerias.Utilerias.formatCurrency(txttotal), fBody7, sb, 210, espacio);


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

        private void consutarsocio()
        {
            try
            {
                string sql = "SELECT SOCIO FROM VAUTOBUSES  WHERE ECO=@ECO";
                db.PreparedSQL(sql);

                db.command.Parameters.AddWithValue("@ECO", eco);
                res = db.getTable();
                if (res.Next())
                {
                    socio = res.Get("SOCIO");
                }

                if (socio == "")
                {
                    consutarsocio2();
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
                string funcion = "consultarsocio";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }
        private void consutarsocio2()
        {
            try
            {
                string sql = "SELECT SOCIO_TRABAJA FROM VAUTOBUSES  WHERE ECO=@ECO";
                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@ECO", ec);
                res = db.getTable();
                if (res.Next())

                {
                    socio = res.Get("SOCIO");
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
                string funcion = "consultarsocio2";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }
        async Task RunAsync(NotificationDataModel data)
        {

            //var myContent = JsonNet.Serialize(data);
            //var stringContent = new StringContent(myContent, UnicodeEncoding.UTF8, "application/json");

            //HttpClient client = new HttpClient();
            ////client.BaseAddress = new Uri("https://localhost:44333/api/SendPushNotificationPartners/");
            //// client.BaseAddress = new Uri("https://appi-atah.azurewebsites.net/api/SendPushNotificationPartners/");

            //client.BaseAddress = new Uri("https://appis.atah.online/api/SendPushNotificationPartners/");


            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //try
            //{
            //    var responsevar = "";
            //    HttpResponseMessage response = await client.PostAsync("https://appis.atah.online/api/SendPushNotificationPartners/", stringContent);

            //    // HttpResponseMessage response = await client.PostAsync("https://appi-atah.azurewebsites.net/api/SendPushNotificationPartners/", stringContent);
            //    //HttpResponseMessage response = await client.PostAsync("https://localhost:44333/api/SendPushNotificationPartners/", stringContent);
            //    if (response.IsSuccessStatusCode)
            //    {
            //        responsevar = await response.Content.ReadAsStringAsync();
            //    }
            //    Respuesta res = JsonNet.Deserialize<Respuesta>(responsevar);

            //    //  MessageBox.Show(res.mensaje);
            //}
            //catch (Exception e)
            //{
            //    string error = e.Message;
            //    MessageBox.Show("Error con el servicio intente màs tarde");
            //}

        }
        
        private async Task notificarAsync()
        {
            List<PARTNERS> partners = new List<PARTNERS>();
            PARTNERS p;

            p = new PARTNERS();
            p.USUARIO =sociousuario;
            partners.Add(p);
            //string notificacion = "\"NOTIFICATION\":{ \"TITLE\":\"" + tbTitle.Text + "\",\"MESSAGE\":\"" + tbMessage.Text + "\"}";
            NOTIFICATION noti = new NOTIFICATION();
            noti.TITLE = "Nueva guia Generada";
            noti.MESSAGE = "Guia con folio: "+folio+" del autobus: "+eco+", del conductor: "+chofer+", con gastos:"+Utilerias.Utilerias.formatCurrency(anticipo) + " y un total: "+ Utilerias.Utilerias.formatCurrency(txttotal);
            noti.DATA = "PK_GUIA:" + pkguia;
            NotificationDataModel data = new NotificationDataModel();
            data.PARTNERS = partners;
            data.NOTIFICATION = noti;
            await RunAsync(data);
        }
    
        private void obtenerpks()
        {
            try
            {        
                string sql = "SELECT AU.PK_CHOFER,AU.SOCIO_PK,S.USUARIO  FROM AUTOBUSES AS AU" +
                    " INNER JOIN SOCIOS AS S ON AU.SOCIO_PK = S.PK WHERE AU.ECO=@ECO";
                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@ECO", eco);
                res = db.getTable();
                if (res.Next())

                {
                    SOCIOPK = res.Get("SOCIO_PK");
                   // CHOFERPK = res.Get("PK_CHOFER");
                    sociousuario = res.Get("USUARIO");

                }
                string sql2 = "SELECT   PK1 FROM AUTOBUSES  WHERE ECO=@ECO";
                db.PreparedSQL(sql2);
                db.command.Parameters.AddWithValue("@ECO", eco);
                res = db.getTable();
                if (res.Next())

                {
                    AUTOBUSPK = res.Get("PK1");
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
                string funcion = "obtenerpks";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }
        private void ocupado()
        {
            try
            {

                string pkcorida="";
                string pkorigen="";
                string fecha="";
                
                string sql = "SELECT PK_CORRIDA_RUTA,PK_ORIGEN,FECHA FROM VCORRIDAS_DIA_3 WHERE PK=@PK";

                db.PreparedSQL(sql);


                db.command.Parameters.AddWithValue("@PK", pkruta);

                res = db.getTable();
                while (res.Next())

                {
                    pkcorida = res.Get("PK_CORRIDA_RUTA");
                    pkorigen = res.Get("PK_ORIGEN");
                    fecha = res.Get("FECHA");







                    string sql2 = "UPDATE CORRIDAS_DIA SET GUIA=@GUIA WHERE PK_CORRIDA_RUTA=@PKCORRIDA AND PK_ORIGEN=@ORIGEN AND FECHA=@FECHA ";
                    db.PreparedSQL(sql2);


                    db.command.Parameters.AddWithValue("@PKCORRIDA", pkcorida);
                    db.command.Parameters.AddWithValue("@ORIGEN", pkorigen);
                    db.command.Parameters.AddWithValue("@FECHA", fecha);

                    db.command.Parameters.AddWithValue("@GUIA", 1);

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
                    MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "ocupados";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }

        private void getDatosAdicionales()
        {
            try
            {

                string sql = "SELECT ANTICIPO from GUIA WHERE  PKCORRIDA in " +
                    "(SELECT  PK FROM VCORRIDAS_DIA_3 where LINEA =@LINEA AND FECHA =@FECHA " +
                    "AND AUTOBUS =@ECO AND PK_CORRIDA_RUTA in (SELECT PK_CORRIDA_RUTA FROM VCORRIDAS_DIA_3 WHERE PK =@PK)) ";
                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@ECO", eco);
                db.command.Parameters.AddWithValue("@LINEA", linea);
                db.command.Parameters.AddWithValue("@FECHA", DateTime.Now.ToString("yyyy-MM-dd"));
                db.command.Parameters.AddWithValue("@PK", mayor);

                pkgastos = 0;
                res = db.getTable();

                while (res.Next())
                {
                    pkgastos+=res.GetDouble("ANTICIPO");
                }
                //textBoxgastos.Text = "$" + pkgastos.ToString();
                
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "getDatosAdicionales";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }
        private void guardartabla()
        {

            try
           {
             
                salida = DateTime.Now.ToString("dd/MM/yyyy");
                string horatemp;
                DateTime ini = DateTime.Parse(hora);
                horatemp = ini.ToString("HH:mm");
                string sql = "";
               
                
                    sql = "INSERT INTO GUIA (FOLIO,AUTOBUS,FECHA,ORIGEN,DESTINO,IMPORTE,COMTAQ,COMPBAN,APORTACION,DIESEL,CASETA,IVA,VSEDENA,ANTICIPO,TOTAL,STATUS,validador,CHOFER,SUCURSAL,BOLETOS,LINEA,HORA,TSALIDA,TTURNO,TPASO,PKCORRIDA,SOCIO,PKSOCIO,PKCHOFER,PKAUTOBUS,PASSWORDATAH,PASSWORDUSER,PKUSUARIO)" +
                             " VALUES(@FOLIO,@AUTOBUS,@FECHA,@ORIGEN,@DESTINO,@IMPORTE,@COMTAQ,@COMPBAN,@APORTACION,@DIESEL,@CASETA,@IVA,@VSEDENA,@ANTICIPO,@TOTAL,@STATUS,@VALIDADOR,@CHOFER,@SUCURSAL,@BOLETOS,@LINEA,@HORA,@TSALIDA,@TTURNO,@TPASO,@PKCORRIDA,@SOCIO,@PKSOCIO,@PKCHOFER,@PKAUTOBUS,@CONDC,@USEC,@PKUSER)";
                
                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@FOLIO", folio);
       
                db.command.Parameters.AddWithValue("@AUTOBUS", eco);
                db.command.Parameters.AddWithValue("@FECHA", DateTime.Now.ToString("dd/MM/yyyy"));
                db.command.Parameters.AddWithValue("@ORIGEN", origen);
                db.command.Parameters.AddWithValue("@DESTINO", destino);
                db.command.Parameters.AddWithValue("@IMPORTE", Math.Round(txtimp, 2));
                db.command.Parameters.AddWithValue("@DSALIDA", 0);
                db.command.Parameters.AddWithValue("@COMTAQ", 0);
                db.command.Parameters.AddWithValue("@CONDC", contchofer);
                db.command.Parameters.AddWithValue("@USEC", contusuario);
                db.command.Parameters.AddWithValue("@PKUSER", LoginInfo.PkUsuario);
                db.command.Parameters.AddWithValue("@COMPBAN", com);
                if (checkBoxaportacion.Checked == true)
                {
                    db.command.Parameters.AddWithValue("@APORTACION", aportaciones);
                }
                if (checkBoxaportacion.Checked == false)
                {

                    db.command.Parameters.AddWithValue("@APORTACION", 0);
                }
                db.command.Parameters.AddWithValue("@DIESEL", diselruta);
                db.command.Parameters.AddWithValue("@CASETA", casetasr);
                db.command.Parameters.AddWithValue("@TARJETA", tarjetasr);
                db.command.Parameters.AddWithValue("@IVA", Math.Round(iva, 2));
                db.command.Parameters.AddWithValue("@VSEDENA", 0);
                db.command.Parameters.AddWithValue("@ANTICIPO", Math.Round(anticipo, 2));

                db.command.Parameters.AddWithValue("@TOTAL", Math.Round(txttotal, 2));
                db.command.Parameters.AddWithValue("@STATUS", "Activa");
                db.command.Parameters.AddWithValue("@VALIDADOR", LoginInfo.NombreID+" "+LoginInfo.ApellidoID);
                db.command.Parameters.AddWithValue("@CHOFER", chofer);

                db.command.Parameters.AddWithValue("@BOLETOS", boletos);
                db.command.Parameters.AddWithValue("@LINEA", linea);
                db.command.Parameters.AddWithValue("@HORA", horatemp);
                db.command.Parameters.AddWithValue("@SUCURSAL", LoginInfo.Sucursal);
                if (gastosruta)
                {
                    db.command.Parameters.AddWithValue("@TSALIDA", salidacosto);
                    db.command.Parameters.AddWithValue("@TTURNO", tarjetasr);
                    db.command.Parameters.AddWithValue("@TPASO", pasocosto);
                }
                else
                {
                    db.command.Parameters.AddWithValue("@TSALIDA", salidacosto);
                    db.command.Parameters.AddWithValue("@TTURNO", turnocosto);
                    db.command.Parameters.AddWithValue("@TPASO", pasocosto);
                }
                db.command.Parameters.AddWithValue("@PKCORRIDA", mayor);
                db.command.Parameters.AddWithValue("@SOCIO", socio);
                db.command.Parameters.AddWithValue("@PKSOCIO", SOCIOPK);
                db.command.Parameters.AddWithValue("@PKCHOFER", CHOFERPK);
                db.command.Parameters.AddWithValue("@PKAUTOBUS", AUTOBUSPK);

                 pkguia = db.executeId();

                if (!string.IsNullOrEmpty(pkguia))
                {
                    if (foliovendidos != null) { 
                    sql = "UPDATE vendidos SET PKGUIA=@PKGUIA  WHERE PK in(" + foliovendidos + ")";
                    db.PreparedSQL(sql);
                    db.command.Parameters.AddWithValue("@PKGUIA", pkguia);
                    db.execute();
                        liberarguia = true;
                }
                    buttonimp.Enabled = false;
                   
                    _ = notificarAsync();
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
                string funcion = "guardartabla";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }
        private void imprimirr()
        {
            try
            {

               
                obtenerpks();
                tarjetas_status();
                folio = GenerateRandom();
                codigoqr(folio);
                if (textBoxanticipo.Text == null)
                {
                    txtantic = 0;
                }
                tamaño = 50 + (boletos * 20);
                pd.DefaultPageSettings.PaperSize = new PaperSize("Custom", 315, tamaño);
                pd.DefaultPageSettings.PaperSize.RawKind = 119;
                pd.PrinterSettings.DefaultPageSettings.PaperSize.RawKind = 119;
                pd.DefaultPageSettings.PrinterSettings.PrintRange = PrintRange.AllPages;

                pd.DefaultPageSettings.Landscape = false;

                
                pd.PrintPage += new PrintPageEventHandler(llenarticket);
           
              
                pd.PrinterSettings.PrinterName = Settings1.Default.impresora;
              

                pd2.DefaultPageSettings.PaperSize = new PaperSize("Custom", 315, tamaño);
                pd2.DefaultPageSettings.PaperSize.RawKind = 119;
                pd2.PrinterSettings.DefaultPageSettings.PaperSize.RawKind = 119;
                pd2.DefaultPageSettings.PrinterSettings.PrintRange = PrintRange.AllPages;

                pd2.DefaultPageSettings.Landscape = false;


                pd2.PrintPage += new PrintPageEventHandler(llenarticketinferior);


                pd2.PrinterSettings.PrinterName = Settings1.Default.impresora;
                guardartabla();
                pd.Print();
                if (tick == false && tick2==false)
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
                { pdc.Print();
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
                pd2.Print();
                CrearTicket ticket = new CrearTicket();
                ticket.TextoIzquierda("");
                ticket.TextoIzquierda("");
                ticket.TextoIzquierda("");
                ticket.TextoIzquierda("");
                ticket.CortaTicket();
                ticket.ImprimirTicket(Settings1.Default.impresora);
                

                Utilerias.LOG.acciones("se imprimio la guia " + folio);
                panelcontraseña.Visible = false;
                panelconductor.Visible = false;
                buttonreimprimir.Visible = true;
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
                {
                    Form mensaje = new Mensaje("Configure la imresora", true);

                    mensaje.ShowDialog();
                }
                string funcion = "imprimirr";
                Utilerias.LOG.write(_clase, funcion, error);


            }

        }
        private void txtCaracter_KeyPress(object sender, KeyPressEventArgs e)
        {
            try { 
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                MessageBox.Show("Solo se permiten numeros", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "completo";
                Utilerias.LOG.write(_clase, funcion, error);

            }
        }
        private void TextBoxanticipo_KeyDown(object sender, KeyEventArgs e)
        {
            

        }
    
        private void Checkboxturno_CheckedChanged(object sender, EventArgs e)
        {
          
            if (checkboxturno.Checked == true)
            {
                textBoxturno.ReadOnly = true;
                turnocosto = float.Parse(textBoxturno.Text);
                turn = true;
                txttotal = txttotal - turnocosto;
                textBoxtotal.Text = Utilerias.Utilerias.formatCurrency(txttotal);
                if(anticipo != 0)
                {
                    textBoxtotal.Text = Utilerias.Utilerias.formatCurrency((txttotal-anticipo));
                }
            }
            if (checkboxturno.Checked == false)
            {
                textBoxturno.ReadOnly = false;
               
                txttotal = txttotal+turnocosto;
                textBoxturno.Text = "0";
                turnocosto = 0;
                turn = false;
                textBoxtotal.Text = Utilerias.Utilerias.formatCurrency(txttotal);
                if (anticipo != 0)
                {
                    textBoxtotal.Text = Utilerias.Utilerias.formatCurrency((txttotal - anticipo));
                }
            }
        }

        private void CheckBoxsalida_CheckedChanged(object sender, EventArgs e)
        {
        
            if (checkBoxsalida.Checked == true)
            {
                textBoxsalid.ReadOnly = true;
                salidacosto = float.Parse(textBoxsalid.Text);

                saliddd = true;
               txttotal = (txttotal - salidacosto);
                textBoxtotal.Text = Utilerias.Utilerias.formatCurrency( txttotal);
                if (anticipo != 0)
                {
                    textBoxtotal.Text = Utilerias.Utilerias.formatCurrency((txttotal - anticipo));
                }
            }
            if (checkBoxsalida.Checked == false)
            {
                textBoxsalid.ReadOnly = false;
            
                saliddd = false;
               txttotal = (txttotal+salidacosto);
                salidacosto = 0;
                textBoxsalid.Text = "0";
                textBoxtotal.Text = Utilerias.Utilerias.formatCurrency(  txttotal);
                if (anticipo != 0)
                {
                    textBoxtotal.Text = Utilerias.Utilerias.formatCurrency((txttotal - anticipo));
                }
            }
        }

        private void CheckBoxpaso_CheckedChanged(object sender, EventArgs e)
        {
  
            if (checkBoxpaso.Checked == true)
            {
                passo = true;
                textBoxpaso.ReadOnly = true;
                pasocosto = float.Parse(textBoxpaso.Text);

                txttotal = (txttotal - pasocosto);
                textBoxtotal.Text = Utilerias.Utilerias.formatCurrency(txttotal);
                if (anticipo != 0)
                {
                    textBoxtotal.Text = Utilerias.Utilerias.formatCurrency((txttotal - anticipo));
                }
            }
            if (checkBoxpaso.Checked == false)
            {
          
                textBoxpaso.ReadOnly = false;
                passo = false;
              txttotal = (txttotal+pasocosto);
                pasocosto = 0;
                textBoxpaso.Text = "0";
                textBoxtotal.Text = Utilerias.Utilerias.formatCurrency(txttotal);
                if (anticipo != 0)
                {
                    textBoxtotal.Text = Utilerias.Utilerias.formatCurrency((txttotal - anticipo));
                }
            }
        }

        private void GUIA_Load(object sender, EventArgs e)
        {

        }

        private void GUIA_Shown(object sender, EventArgs e)
        {
            db = new database();
            verificationUserControl1.Stop();
            verificationUserControl2.Stop();

            fechaaa = DateTime.Now.ToString("dd/MM/yyyy");
            fechayears = DateTime.Now.ToString("yyyy-MM-dd");
            
            GUIACOMPROBACION();
            ocupado();
            textBoxanticipo.Text = "0";
            textBoxsuc.Text = LoginInfo.Sucursal;
            textBoxorig.Text = origen;
            textBoxdest.Text = destino;
            textBoxsal.Text = hora;
            textBoxaut.Text = eco;
            permisos();
            labelusu.Visible = false;
            labelsocio.Visible = false;
            llenardatosdeboletos();

            comboBoxconductoruser.DropDownStyle = ComboBoxStyle.DropDownList;
             tarjetas();
            variables();


                sumatoria();
            
                textBoxturno.Text = turnocosto.ToString();
                textBoxsalid.Text =salidacosto.ToString();
            textBoxpaso.Text = pasocosto.ToString() ;
                verificarsucursal();
       
            if (gastosruta)
            {
                textBoxanticipo.Enabled = false;
                groupBoxruta.Visible = true;
                completo();
                
            }
            if (LoginInfo.privilegios.Any(x => x == "Aportaciones"))
            {
                groupBoxaportaciones.Visible = true;
                checkBoxaportacion.Checked = true;

            }
            supercontra();
        }

        private void completo()
        {
            try
            {
                string sql = "SELECT COMPLETO,RUTA FROM VCORRIDAS_DIA_3 WHERE PK=@PK";
                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@PK", pkruta);

                res = db.getTable();
                if (res.Next())
                {
                    completoes = res.GetBool("COMPLETO");
                    ruta = res.Get("RUTA");

                }
                if (completoes)
                {
                  

                    string sql2 = "SELECT PK,TARJETAS,CASETAS,SUELDO from RUTAS WHERE RUTA=@RUTA";
                    db.PreparedSQL(sql2);
                    db.command.Parameters.AddWithValue("@RUTA", ruta);
                    int pktemporuta=0;
                    res = db.getTable();
                    if (res.Next())
                    {
                        tarjetasr = res.GetInt("TARJETAS");
                        pktemporuta = res.GetInt("PK");
                         casetasr = res.GetInt("CASETAS");
                        sueldor = res.GetInt("SUELDO");

                    }
                    string sql22 = "select(( convert(float,(select kms from RUTAS_DESTINOS  WHERE COMPLETO=1 and pk=42)))" +
                        "/" +
                        "(SELECT convert(float, (SELECT VALOR FROM VARIABLES WHERE NOMBRE = 'RENDIMIENTO'))))" +
                        "*" +
                        "((SELECT convert(float, (SELECT VALOR FROM VARIABLES WHERE NOMBRE = 'GASOLINA'))))" +
                        "AS DISEL";
                    db.PreparedSQL(sql22);
                    db.command.Parameters.AddWithValue("@PK", pktemporuta);
                    res = db.getTable();
                    if (res.Next())
                    {
                        diselruta = res.GetInt("DISEL");

                    }
                    //suma = tarjetasr + casetasr + sueldor + diselruta;
                    suma = txttotal - suma;
                    textBoxdisel.Text = diselruta.ToString();
                    textBoxtarjetas.Text = tarjetasr.ToString();
                    textBoxcasetas.Text = casetasr.ToString();
                    textBoxtotal.Text = "$" +suma.ToString();


                }
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "completo";
                Utilerias.LOG.write(_clase, funcion, error);

            }
        }

        private void verificarsucursal()
        {

            string sql = "SELECT COBRARTARJETAS  FROM SUCURSALES WHERE SUCURSAL=@SUCURSAL";
            db.PreparedSQL(sql);
            db.command.Parameters.AddWithValue("@SUCURSAL", origen);
     
            res = db.getTable();
            if (res.Next())
            {
                COBRARTARJETAS = res.GetBool("COBRARTARJETAS");

                if (COBRARTARJETAS == true)
                {

                    checkboxturno.Checked = true;
                    checkBoxsalida.Checked = true;
                    checkBoxpaso.Checked = true;
                    checkBoxpaso.Enabled = false;
                    checkBoxsalida.Enabled = false;
                    checkboxturno.Enabled = false;


                }
            }

        }
        
        private void TextBoxanticipo_KeyUp(object sender, KeyEventArgs e)
        {
            }

        private void BtnCerrar_Click(object sender, EventArgs e)
        {
            if(!liberarguia)
            {
                liberar();
             
            }
            this.Close();
        }
        private void liberar()
        {
            try
            {
         string pkcorida = "";
         string pkorigen = "";
         string fecha = "";


        string sql = "SELECT PK_CORRIDA_RUTA,PK_ORIGEN,FECHA FROM VCORRIDAS_DIA_3 WHERE PK=@PK";

                db.PreparedSQL(sql);


                db.command.Parameters.AddWithValue("@PK", pkruta);

                res = db.getTable();
                while (res.Next())

                {
                    pkcorida = res.Get("PK_CORRIDA_RUTA");
                    pkorigen = res.Get("PK_ORIGEN");
                    fecha = res.Get("FECHA");







                    string sql2 = "UPDATE CORRIDAS_DIA SET GUIA=@GUIA WHERE PK_CORRIDA_RUTA=@PKCORRIDA AND PK_ORIGEN=@ORIGEN AND FECHA=@FECHA ";
                    db.PreparedSQL(sql2);


                    db.command.Parameters.AddWithValue("@PKCORRIDA", pkcorida);
                    db.command.Parameters.AddWithValue("@ORIGEN", pkorigen);
                    db.command.Parameters.AddWithValue("@FECHA", fecha);

                    db.command.Parameters.AddWithValue("@GUIA", 0);

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
                    MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "ocupados";
                Utilerias.LOG.write(_clase, funcion, error);


            }
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

        private void textBoxanticipo_KeyUp_1(object sender, KeyEventArgs e)
        {

            try

          {

           

                
              
            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "eliminar";
                Utilerias.LOG.write(_clase, funcion, error);

            }
        }

        private void cambio()
        {
            try

            {


                if (double.TryParse(textBoxanticipo.Text, out double resultado) && Convert.ToDouble(textBoxanticipo.Text) <= gastomaximo)
                {



                    if (textBoxanticipo.Text != "")
                    {
                        textBoxtotal.Text = "$" + ((Math.Round(txttotal - Convert.ToDouble(textBoxanticipo.Text), 2))).ToString();
                        tot = ((Math.Round(txttotal - Convert.ToDouble(textBoxanticipo.Text), 2))).ToString();
                        txtantic = (Math.Round(Convert.ToDouble(tot), 2));
                    }

                }
                else
                {
                    textBoxanticipo.Text = "0";
                    textBoxtotal.Text = "$" + gastomaximo.ToString();

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

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
           

            if (checkBoxaportacion.Checked == true)
            {

               txttotal = (txttotal - aportaciones-anticipo);
                textBoxtotal.Text = Utilerias.Utilerias.formatCurrency( txttotal);
                gastomaximo = txttotal;
                checkBoxaportacion.Enabled = false;
            }
        
        }

        private void groupBoxaportaciones_Enter(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void labelsocio_Click(object sender, EventArgs e)
        {

        }

        private void buttoncontraseña_Click(object sender, EventArgs e)
        {

        }


        private void contraseñaadmin_Click(object sender, EventArgs e)
        {
            panelconductor.Visible = false;
            panelcontraseña.Visible = true;
       
            textBoxcontraseña.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (contraseña == textBoxcontraseña.Text)
            {
                if (contando1 > 5)
                {
                    contusuario = true;
                    contando1 = 0;
                    verificationUserControl1.Stop();
                    verificationUserControl1.Hide();
                    labelusu.Visible = false;
                    verificationUserControl1.Samples.Clear();
                    verificationUserControl1.IsVerificationComplete = false;
                    verificationUserControl1.img = null;
                    //verificationUserControl1.Init();
                    verificationUserControl2.limpiarhuella();
                    panelcontraseña.Visible = false;
                    textBoxcontraseña.Text = "";
                    verificationUserControl1.FingerPrintPicture.Image = null;

                    panelconductor.Visible = true;
                    choferdefault();
                    verificationUserControl2.Focus();
                }
                if (contando2 > 5)
                {
                    contando2 = 0;
                    contchofer=true;
                    panelcontraseña.Visible = false;
                    textBoxcontraseña.Text = "";

                    contraseñaerror.Visible = false;

                validarcontraseña = true;
                imprimirr();
            } 
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

        private void comboBoxconductoruser_SelectedIndexChanged(object sender, EventArgs e)
        {
             string pkconductor = (comboBoxconductoruser.SelectedItem != null) ? (comboBoxconductoruser.SelectedItem as ComboboxItem).Value.ToString() : "";
            buscarusuarioconductor(pkconductor);
        }
        private void buscarusuarioconductor(string pkconductor)
        {
            try
            {
                string sql = "SELECT PK, NOMBRE,APELLIDOS,IMAG FROM CHOFERES WHERE ACTIVO=1 AND PK=@PK";


                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@PK", pkconductor);
                res = db.getTable();
                if (res.Next())
                {
                    labelnombreconductor.Text = res.Get("NOMBRE") + " " + res.Get("APELLIDOS");
                    byte[] imagen = (!string.IsNullOrEmpty(res.Get("IMAG"))) ? Convert.FromBase64String(res.Get("IMAG")) : null;
                    CHOFERPK = res.Get("PK");

                    pictureBox1.Image = (imagen != null) ? Image.Bytes_A_Imagen((byte[])imagen) : null;
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                 //   groupBoxhuellaconductor.Visible = true;
                    chofer = labelnombreconductor.Text;
                   // Validateconductor(pkconductor);
                  //  error.Visible = false;



                }
                else
                {
                    error.Visible = true;
                    error.Text = "Usuario incorrecto";
                }
                this.CenterToScreen();

            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "ingresar";
                Utilerias.LOG.write(_clase, funcion, error);

            }

        }

        public void getDatosAdicionaleschoferes()
        {
            try
            {
                comboBoxconductoruser.Items.Clear();

                string sql = "SELECT PK, NOMBRE,APELLIDOS FROM CHOFERES WHERE BORRADO=0 ORDER BY NOMBRE";
                db.PreparedSQL(sql);
                res = db.getTable();

                while (res.Next())
                {
                    ComboboxItem item = new ComboboxItem();
                    item.Text = res.Get("NOMBRE") + " " + res.Get("APELLIDOS");
                    item.Value = res.Get("PK");
                    comboBoxconductoruser.Items.Add(item);

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
        private void consultarconductoresdesocio()
        {
            try
            {
                string sql = "SELECT PK_CHOFER, CHOFER,SOCIO FROM VAUTOBUSES " +
                    "WHERE SOCIO_PK IN (SELECT SOCIO_PK FROM VAUTOBUSES  WHERE ECO=@ECO)";
                db.PreparedSQL(sql);

                db.command.Parameters.AddWithValue("@ECO", eco);
                res = db.getTable();
                while (res.Next())
                {
                    socio = res.Get("SOCIO");
                    ComboboxItem item = new ComboboxItem();
                    item.Text = res.Get("CHOFER");
                    item.Value = res.Get("PK_CHOFER");
                    comboBoxconductoruser.Items.Add(item);
                }
                string sql2 = "SELECT PK, NOMBRE FROM CHOFERES WHERE PK=140";
                db.PreparedSQL(sql2);
                res = db.getTable();
                while (res.Next())
                {
                    ComboboxItem item = new ComboboxItem();
                    item.Text = res.Get("NOMBRE");
                    item.Value = res.Get("PK");
                    comboBoxconductoruser.Items.Add(item);
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
                string funcion = "consultarconductoresdesocio";
                Utilerias.LOG.write(_clase, funcion, error);


            }
        }

        private void choferdefault()
        {
            try
            {
                comboBoxconductoruser.Focus();
                string sql = "SELECT PK, NOMBRE,APELLIDOS,IMAG FROM CHOFERES WHERE " +
                    "PK IN (SELECT PK_CHOFER FROM VAUTOBUSES  WHERE ECO=@ECO) ";


                db.PreparedSQL(sql);
                db.command.Parameters.AddWithValue("@ECO", eco);
                res = db.getTable();
                if (res.Next())
                {
                    labelnombreconductor.Text = res.Get("NOMBRE") + " " + res.Get("APELLIDOS");
                    byte[] imagen = (!string.IsNullOrEmpty(res.Get("IMAG"))) ? Convert.FromBase64String(res.Get("IMAG")) : null;
                    CHOFERPK = res.Get("PK");

                    pictureBox1.Image = (imagen != null) ? Image.Bytes_A_Imagen((byte[])imagen) : null;
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                 //   groupBoxhuellaconductor.Visible = true;
                    chofer = labelnombreconductor.Text;
                   // Validateconductor(res.Get("PK"));
                    //error.Visible = false;



                }
                else
                {
                    error.Visible = true;
                    error.Text = "Usuario incorrecto";
                }
                this.CenterToScreen();

            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "ingresar";
                Utilerias.LOG.write(_clase, funcion, error);

            }

        }

        private void textBoxanticipo_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
                {

                    if (e.KeyChar == 46)
                    {


                    }
                    else
                    {
                        MessageBox.Show("Solo se permiten numeros", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        e.Handled = true;
                        return;
                    }
                }
              

            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "ingresar";
                Utilerias.LOG.write(_clase, funcion, error);

            }
        }

        private void textBoxanticipo_KeyUp_2(object sender, KeyEventArgs e)
        {
            try
            {
                double busqueda=0.00;

                if (textBoxanticipo.Text == "")
               {
                    anticipo = 0.0;
                }
                else
                {

                    string tempo = textBoxanticipo.Text;

                    Console.WriteLine(tempo);

                    int posicion = 0;
                    // string obtenerprimeros= textBoxanticipo.Text.Substring(0, posicion);

                    //string ultimos= textBoxanticipo.Text.Substring( posicion,textBoxanticipo.Text.Length);
                    //string res = obtenerprimeros + ultimos;
                    if ( tempo.Contains("."))
                    {
                        Console.WriteLine(tempo);



                        if (double.TryParse(tempo, out anticipo))
                        {

                            //si ingresa es un valor nro valido

                        }
                    }
                    else
                    {
                        anticipo = float.Parse(tempo);
                    }
                }
                double temporal= (txttotal - anticipo);
                textBoxtotal.Text = Utilerias.Utilerias.formatCurrency(temporal);
            



            }
            catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "ingresar";
                Utilerias.LOG.write(_clase, funcion, error);

            }
        }

        private void textBoxcontraseña_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter )
            {
                if (contraseña == textBoxcontraseña.Text)
                {
                    if (contando1 > 5)
                    {
                        contusuario = true;
                        contando1 = 0;
                        verificationUserControl1.Stop();
                        verificationUserControl1.Hide();
                        labelusu.Visible = false;
                        verificationUserControl1.Samples.Clear();
                        verificationUserControl1.IsVerificationComplete = false;
                        verificationUserControl1.img = null;
                        //verificationUserControl1.Init();
                        verificationUserControl2.limpiarhuella();
                        panelcontraseña.Visible = false;
                        textBoxcontraseña.Text = "";
                        verificationUserControl1.FingerPrintPicture.Image = null;

                        panelconductor.Visible = true;
                        choferdefault();
                        verificationUserControl2.Focus();
                    }
                    if (contando2 > 5)
                    {
                        contando2 = 0;
                        contchofer = true;
                        panelcontraseña.Visible = false;
                        textBoxcontraseña.Text = "";

                        contraseñaerror.Visible = false;

                        validarcontraseña = true;
                        imprimirr();
                    }
                }
                else
                {
                    contraseñaerror.Visible = true;
                }


            }
            }

        private void buttonreimprimir_Click(object sender, EventArgs e)
        {
            try
            {
                pd = new PrintDocument();
                pd2 = new PrintDocument();
                 pdc = new PrintDocument();
                 pdc2 = new PrintDocument();
                pd.DefaultPageSettings.PaperSize = new PaperSize("Custom", 315, tamaño);
                pd.DefaultPageSettings.PaperSize.RawKind = 119;
                pd.PrinterSettings.DefaultPageSettings.PaperSize.RawKind = 119;
                pd.DefaultPageSettings.PrinterSettings.PrintRange = PrintRange.AllPages;

                pd.DefaultPageSettings.Landscape = false;


                pd.PrintPage += new PrintPageEventHandler(llenarticket);


                pd.PrinterSettings.PrinterName = Settings1.Default.impresora;


                pd2.DefaultPageSettings.PaperSize = new PaperSize("Custom", 315, tamaño);
                pd2.DefaultPageSettings.PaperSize.RawKind = 119;
                pd2.PrinterSettings.DefaultPageSettings.PaperSize.RawKind = 119;
                pd2.DefaultPageSettings.PrinterSettings.PrintRange = PrintRange.AllPages;

                pd2.DefaultPageSettings.Landscape = false;


                pd2.PrintPage += new PrintPageEventHandler(llenarticketinferior);


                pd2.PrinterSettings.PrinterName = Settings1.Default.impresora;
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
                pd2.Print();
                CrearTicket ticket = new CrearTicket();
                ticket.TextoIzquierda("");
                ticket.TextoIzquierda("");
                ticket.TextoIzquierda("");
                ticket.TextoIzquierda("");
                ticket.CortaTicket();
                ticket.ImprimirTicket(Settings1.Default.impresora);


                Utilerias.LOG.acciones("se imprimio la guia " + folio);

                panelconductor.Visible = false;
                buttonreimprimir.Visible = true;
            }
              catch (Exception err)
            {
                string error = err.Message;
                MessageBox.Show("Ocurrio un Error, intente de nuevo.");
                string funcion = "ingresar";
                Utilerias.LOG.write(_clase, funcion, error);

            }
        }

        private void textBoxcasetas_TextChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
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

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            labelsocio.Visible = false;
            panelconductor.Visible = false;
            imprimirr();
        }
    }
    }

