using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging; // Para trabajar con imágenes
using System.Drawing.Drawing2D; // Para trabajar con Imágenes
using System.Runtime.InteropServices;
using System.Drawing;
using System.Windows.Forms; // Para usar API

namespace WindowsFormsApplication1
{
    public class WebCam
    {

        // Conserva esta cabecera
        // Empeorxdevil 2007

        class SaveImage
        {

            // Conserva esta cabecera
            // Emperorxdevil 2007

            /// <summary>
            /// Imagen que contiene la clase
            /// </summary>
            public Image Tmp; //  referencia a la imagen del Picturebox Asociado
            Form Ref; // referencia que necesitare a este formulario

            Form Opciones;
            GroupBox Gr;
            RadioButton R1;
            RadioButton R2;
            RadioButton R3;
            TextBox Porciento;
            TextBox Larg;        // Objetos necesarios para formar el diálogo
            TextBox Anch;
            Label L1;
            Label L2;
            Button Acepta;
            Button Cancelar;

            SaveFileDialog Sv;
            DialogResult rs;


            /// <summary>
            /// Constructor de clase SaveImage
            /// </summary>
            /// <param name="imagen">Imagen que se va a guardar</param>
            /// <param name="Interfaz"> Si se mostrará Interfaz</param>
            public SaveImage(Image imagen, bool Interfaz)
            {

                Tmp = imagen; // Asigno la imagen...              
                Opciones = new Form();
                Ref = Opciones;
                Opciones.Width = 333;     // Configuracion
                Opciones.Height = 198;
                Opciones.Text = "Parámetros de Imagen";
                Opciones.ControlBox = false;
                Opciones.ShowInTaskbar = false;
                Opciones.FormBorderStyle = FormBorderStyle.FixedDialog;

                Gr = new GroupBox();
                Gr.Width = 288;
                Gr.Height = 105;
                Gr.Text = "Configuración";
                Gr.Location = new Point(18, 15);
                Porciento = new TextBox();
                Porciento.Enabled = false;                   // Configuracion
                Porciento.Text = "";
                Porciento.Width = 37;
                Porciento.Height = 20;
                Porciento.MaxLength = 3;
                Porciento.TextAlign = HorizontalAlignment.Center;
                Porciento.Location = new Point(147, 47);
                R1 = new RadioButton();
                R1.Text = "Guardar con Dimensiones Actuales";
                R1.Width = 191;
                R1.Height = 17;
                Gr.Controls.Add(R1);
                R1.Location = new Point(18, 25);
                R2 = new RadioButton();
                R2.CheckedChanged += new EventHandler(R2_CheckedChanged);
                R2.Text = "Reducir tamaño al";
                R2.Width = 115;             // Configuracion
                R2.Height = 17;
                R2.Location = new Point(18, 48);
                Gr.Controls.Add(R2);
                Gr.Controls.Add(Porciento);
                L1 = new Label();
                L1.Text = "%";
                L1.AutoSize = true;
                L1.Location = new Point(195, 50);
                Gr.Controls.Add(L1);
                R3 = new RadioButton();
                R3.CheckedChanged += new EventHandler(R3_CheckedChanged);
                R3.Text = "Cambiar tamaño a"; // Configuracion
                R3.Width = 113;
                R3.Height = 17;
                R3.Location = new Point(18, 71);
                Gr.Controls.Add(R3);
                L2 = new Label();
                L2.Text = "X";
                L2.AutoSize = true;
                L2.Location = new Point(195, 75);
                Gr.Controls.Add(L2);
                Larg = new TextBox();
                Larg.Enabled = false;
                Larg.Width = 37;
                Larg.Height = 20;
                Larg.MaxLength = 4;
                Larg.TextAlign = HorizontalAlignment.Center;
                Larg.Location = new Point(147, 71);
                Gr.Controls.Add(Larg);

                Anch = new TextBox();
                Anch.Enabled = false;
                Anch.Width = 37;
                Anch.Height = 20;
                Anch.MaxLength = 4;
                Anch.TextAlign = HorizontalAlignment.Center; // Configuracion
                Anch.Location = new Point(218, 70);
                Gr.Controls.Add(Anch);

                Acepta = new Button();
                Acepta.Text = "Aceptar";
                Acepta.Width = 59;
                Acepta.Height = 26;
                Acepta.Location = new Point(247, 133);
                Acepta.Click += new EventHandler(Acepta_Click);

                Cancelar = new Button();
                Cancelar.Text = "Cancelar";
                Cancelar.Width = 59;
                Cancelar.Height = 26;
                Cancelar.Location = new Point(18, 133);
                Cancelar.Click += new EventHandler(Cancelar_Click);
                Opciones.Controls.Add(Gr);
                Opciones.Controls.Add(Acepta);
                Opciones.Controls.Add(Cancelar);
                Opciones.StartPosition = FormStartPosition.CenterScreen;
                Opciones.ShowDialog();

            }


            /// <summary>
            /// Constructor de la clase SaveImage sin Interfaz
            /// </summary>
            /// <param name="imagen">Imagen a Guardar</param>
            public SaveImage(Image imagen)
            {
                Tmp = imagen; // Asigno la imagen...  
            }

            void R3_CheckedChanged(object sender, EventArgs e)
            {
                if (this.R3.Checked) { this.Anch.Enabled = true; this.Larg.Enabled = true; }
                else { this.Anch.Enabled = false; this.Larg.Enabled = false; }
            }
            void R2_CheckedChanged(object sender, EventArgs e)
            {
                if (this.R2.Checked) { this.Porciento.Enabled = true; }
                else { this.Porciento.Enabled = false; }
            }
            void Acepta_Click(object sender, EventArgs e)
            {
                try
                {
                    if (R1.Checked || R2.Checked || R3.Checked)
                    {

                        if (R2.Checked) { Tmp = Resize(Tmp, Convert.ToInt32(Porciento.Text)); }

                        if (R3.Checked) { Tmp = Resize(Tmp, Convert.ToInt32(Larg.Text), Convert.ToInt32(Anch.Text)); }

                        Sv = new SaveFileDialog(); // Creo un diálogo para salvar la imagen 
                        Sv.Title = "Guardar imagen como..."; // Lo configuro
                        Sv.Filter = "Archivo JPG|*.jpg|Archivo PNG|*.png|Archivo GIF|*.gif|Mapa de Bits|*.bmp";
                        rs = Sv.ShowDialog(); // Lo muestro modal


                        if (rs == DialogResult.OK)
                        {

                            int x = Sv.FilterIndex; // Ontengo la extensión seleccionada  

                            switch (x)
                            {
                                // Salvo la imagen en el formato deseado
                                case 1:
                                    Tmp.Save(Sv.FileName, ImageFormat.Jpeg);
                                    break;
                                case 2:
                                    Tmp.Save(Sv.FileName, ImageFormat.Png);
                                    break;
                                case 3:
                                    Tmp.Save(Sv.FileName, ImageFormat.Gif);
                                    break;
                                case 4:
                                    Tmp.Save(Sv.FileName, ImageFormat.Bmp);
                                    break;
                            }
                            Ref.Dispose(); // Cierro el formulario        
                        }
                    }
                    else { MessageBox.Show("Al menos debe selecionar una opción", "xdevilWebCapture", MessageBoxButtons.OK, MessageBoxIcon.Warning); }

                }

                catch
                {
                    MessageBox.Show("Introduzca valores válidos", "xdevilWebCapture", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

            }
            void Cancelar_Click(object sender, EventArgs e)
            {
                Ref.Dispose(); // Cierro el formulario
            }



            /// <summary>
            /// Escala la imagen capturada al % especificado
            /// </summary>
            /// <param name="foto"> Imagen que se quiere escalar </param>
            /// <param name="TantoPorCiento"> Porcentaje de reducción</param>
            /// <returns>Imagen escalada</returns>
            public Image Resize(Image foto, int TantoPorCiento)
            {

                float Tporciento = ((float)TantoPorCiento / 100); // Obtengo el coeficiente de dimension                    
                int ImgOrAncho = foto.Width;
                int ImgOrAlto = foto.Height; // Obtengo las dimensiones originales de la foto

                int OrigX = 0;
                int OrigY = 0;
                int ResX = 0;  // Variables referencia para saber donde empiezo a contar px
                int ResY = 0;

                int ResAncho = (int)(ImgOrAncho * Tporciento);
                int ResAlto = (int)(ImgOrAlto * Tporciento); // Obtengo las dimensiones al % especificado    

                Bitmap RszIm = new Bitmap(ResAncho, ResAlto, PixelFormat.Format24bppRgb); // Creo una imagen con esas dimensiones y bpp
                RszIm.SetResolution(foto.HorizontalResolution, foto.VerticalResolution); // Le doy la misma res. que la original

                Graphics Gfoto = Graphics.FromImage(RszIm); // Creo una intancia de Graphics para manejar la imagen nueva
                Gfoto.InterpolationMode = InterpolationMode.HighQualityBicubic; // Especifico la calidad del algoritmo de sampleo
                // De la foto original, obtengo la redimensionada (mediante un rectángulo)
                Gfoto.DrawImage(foto, new Rectangle(ResX, ResY, ResAncho, ResAlto), new Rectangle(OrigX, OrigY, ImgOrAncho, ImgOrAlto), GraphicsUnit.Pixel);
                Gfoto.Dispose(); // Ya no me hace falta esto, asi que lo descargo

                return (RszIm); // Devuelvo la imagen redimensionada
            }
            /// <summary>
            /// Redimensiona la imagen en pixeles
            /// </summary>
            /// <param name="foto"> Imagen a redimensionar</param>
            /// <param name="ancho">Ancho de la imagen</param>
            /// <param name="alto">Alto de la imagen</param>
            /// <returns>Imagen redimensionada</returns>
            public Image Resize(Image foto, int ancho, int alto)
            {

                int ImgORAncho = foto.Width;
                int ImgOrAlto = foto.Height; // Obtengo las dimensiones de la foto

                int OrigX = 0;
                int OrigY = 0;
                int ResX = 0;  // Varables referencia para saber donde contar px
                int ResY = 0;

                float Porciento = 0;
                float PorcientoAncho = 0; // Porcentajes de sampleo
                float PorcientoAlto = 0;

                PorcientoAncho = ((float)ancho / (float)ImgORAncho);
                PorcientoAlto = ((float)alto / (float)ImgOrAlto); //Calculo el % que puedo resamplear

                if (PorcientoAlto < PorcientoAncho)
                {
                    Porciento = PorcientoAlto;
                }
                else
                { // Para resamplear bien                 
                    Porciento = PorcientoAncho;
                }


                int AnchuraFinal = (int)(ImgORAncho * Porciento);
                int AlturaFinal;  // Calculo las nuevas dimensiones                

                if (ancho > alto)
                {
                    AlturaFinal = (int)(ImgOrAlto * Porciento);
                }
                else
                {
                    AlturaFinal = AnchuraFinal;
                } // Para proporcionar la imagen 

                Bitmap RszIm = new Bitmap(ancho, alto, PixelFormat.Format24bppRgb);
                RszIm.SetResolution(foto.HorizontalResolution, foto.VerticalResolution);

                Graphics Gfoto = Graphics.FromImage(RszIm);
                Gfoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
                Gfoto.DrawImage(foto, new Rectangle(ResX, ResY, AnchuraFinal, AlturaFinal), new Rectangle(OrigX, OrigY, ImgORAncho, ImgOrAlto), GraphicsUnit.Pixel);
                Gfoto.Dispose();
                return (RszIm);

            }

        } /// Clase que guarda las imágenes

        #region Funciones API

        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(int hWnd, uint Msg, int wParam, int lParam);

        [DllImport("avicap32.dll", EntryPoint = "capCreateCaptureWindowA")]
        public static extern int capCreateCaptureWindowA(string lpszWindowName, int dwStyle, int X, int Y, int nWidth, int nHeight, int hwndParent, int nID);

        [DllImport("user32.dll", EntryPoint = "EmptyClipboard")]
        public static extern int EmptyClipboard();

        //[DllImport("avicap32.dll", EntryPoint = "capGetDriverDescriptionA")]
        //public static extern bool capGetDriverDescriptionA(int wdriver,  string lpszName, int cbName, string lpszVer, int cbVer);

        [DllImport("avicap32.dll", EntryPoint = "capGetDriverDescriptionA")]
        public static extern bool capGetDriverDescriptionA(short wDriverIndex, [MarshalAs(UnmanagedType.VBByRefStr)]ref String lpszName,
       int cbName, [MarshalAs(UnmanagedType.VBByRefStr)] ref String lpszVer, int cbVer);

        #endregion

        #region Constantes API

        const int WM_USER = 1024;
        const int WM_CAP_CONNECT = 1034;
        const int WM_CAP_DISCONNECT = 1035;
        const int WM_CAP_GET_FRAME = 1084;  // Constantes necesarias
        const int WM_CAP_COPY = 1054;
        const int WM_CAP_START = WM_USER;
        const int WM_CAP_SET_PREVIEW = WM_CAP_START + 50;
        const int WM_CAP_SET_PREVIEWRATE = WM_USER + 52;

        #endregion

        private int Frecuencia = 1; // Frecuencia de captura de imagen   
        private int CamHandle = 0; // Handle del dispositivo


        Timer reloj; // Timer
        ComboBox Cbx; // Para referencia al Combobox
        PictureBox Pbx; // Para referencia al Picturebox


        /// <summary>
        /// Crea una instancia de la clase WebCam de Emperorxdevil
        /// </summary>
        /// <param name="
        /// ">Intervalo de tiempo en el que se capturarán las imágenes</param>
        /// <param name="pct">PictureBox en el cual se mostrará lo capturado por la webcam</param>
        /// <param name="cb"> Listbox en el que se mostrarán las Cams detectadas</param>
        public WebCam(int frecuencia, PictureBox pct, ComboBox cb)
        {

            Frecuencia = frecuencia;
            Pbx = pct;              // Inicio las variables necesarias
            reloj = new Timer(); // Creo el timer
            reloj.Tick += new EventHandler(reloj_Tick); // Le asigno el controlador   
            reloj.Interval = Frecuencia; // Asigno la velocidad de captura
            Cbx = cb; //Configuro la referencia

        }

        private void reloj_Tick(object sender, EventArgs e)
        {

            SendMessage(CamHandle, WM_CAP_GET_FRAME, 80, 0); // Capturo la imagen
            SendMessage(CamHandle, WM_CAP_COPY, 80, 0); // La copio al portapeles
            Pbx.Image = Clipboard.GetImage(); // La mando al Picturebox
            EmptyClipboard(); // Vacío el portapapeles

        }
        /// <summary>
        /// Listará las cámaras web encontradas en el Combobox asociado
        /// </summary>
        public void Listar()
        {

            bool valor;
            string nombre = "".PadRight(100);
            string vers = "".PadRight(100); ;
            short i = 0;
            do
            {
                valor = capGetDriverDescriptionA(i, ref nombre, 100, ref vers, 100);
                if (valor)
                    Cbx.Items.Add(i + " " + nombre);
                i++;
            } while (!valor == false);
        }

        public int ReturnDriver()
        {
            String dName = "".PadRight(100);
            String dVersion = "".PadRight(100);
            int driverindex = 0;
            for (short i = 0; i < 9; i++)
            {
                //if (capGetDriverDescriptionA(i, drivername, 500, driverversion, 500))
                if (capGetDriverDescriptionA(i, ref dName, 100, ref dVersion, 100))
                {
                    driverindex = i;
                }
            }
            return driverindex;
        }

        /// <summary>
        /// La WebCam se Encenderá y mostrará lo capturado en el PictureBox referenciado
        /// </summary>
        /// <param name="Dispositivo">Nº de dispositivo del Combobox</param>         


        public void EncenderCamara(int Dispositivo)
        {

            try
            {

                StopCaptura(); // Antes de nada "reseteo" el dispositivo  
                CamHandle = capCreateCaptureWindowA("XdevilCapture" + Dispositivo.ToString(), 0, 0, 0, 0, 0, Pbx.Handle.ToInt32(), Dispositivo); // Obtengo el Handle de la cam
                SendMessage(CamHandle, WM_CAP_CONNECT, 0, 0); // Enciendo la cam
                SendMessage(CamHandle, WM_CAP_SET_PREVIEWRATE, 80, 0); // Establezco los frames de captura /seg
                SendMessage(CamHandle, WM_CAP_SET_PREVIEW, 0, 0); // Empiezo a capturar
                reloj.Start(); // Inicio la captura      
               
            }

            catch (Exception e)
            {
                MessageBox.Show("No se puede Iniciar la WebCam", "Error");
            }

        }
        /// <summary>
        /// Detiene la captura de imágenes y apaga la WebCam
        /// </summary>
        public void StopCaptura()
        {


            try
            {
                reloj.Stop(); // Paro el reloj que captura
                SendMessage(CamHandle, WM_CAP_DISCONNECT, 0, 0); // Apago la cam
                EmptyClipboard(); // Vacío el portapapeles
            }

            catch (Exception e) { } // No hago nada.. pa k ?

        }
        /// <summary>
        /// Muestra diferentes parametros configurables de la imagen que se va a guardar
        /// </summary>
        public void GuardarImagen()
        {

            SaveImage s = new SaveImage(Pbx.Image, true); // Creo la clase que guarda las fotos
        }


    }
}