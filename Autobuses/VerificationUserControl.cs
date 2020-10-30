using Autobuses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MyAttendance
{
    public partial class VerificationUserControl : UserControl, IComponent, DPFP.Capture.EventHandler
    {
        public Boolean i = true;
        public Byte[] img;
        bool verified = false;
        int nom = 0;
        int resultado;
        public TextBox nueva;
        public VerificationUserControl()
        {
            InitializeComponent();
            this.Load += new EventHandler(FingerPrintVerificationUserControl_Load);
            this.HandleDestroyed += new EventHandler(FingerPrintVerificationUserControl_HandleDestroyed);

        }

        private bool isVerificationComplete = false;
        public bool IsVerificationComplete
        {
            get { return this.isVerificationComplete; }
            set
            {
                if (value != this.isVerificationComplete)
                {
                    this.isVerificationComplete = value;
                    if (this.VerificationStatusChanged != null)
                    {
                        this.VerificationStatusChanged(this, new StatusChangedEventArgs(value));
                    }
                }
            }

        }

        public object VerifiedObject
        {
            get;
            private set;
        }


        private DPFP.Capture.Capture Capturer;
        public Dictionary<DPFP.Template, object> Samples = new Dictionary<DPFP.Template, object>();
        private DPFP.Verification.Verification Verificator;

        public event StatusChangedEventHandler VerificationStatusChanged;

        delegate void Function();

        #region Form Event Handlers:
        private void FingerPrintVerificationUserControl_Load(object sender, EventArgs e)
        {
            Init();
            Start();
        }

        public void FingerPrintVerificationUserControl_HandleDestroyed(object sender, EventArgs e)
        {
            Stop();
        }

        #endregion

        #region FingerPrint Handlers

        protected virtual void Process(DPFP.Sample Sample)
        {

            DrawPicture(FingerPrintUtility.ConvertSampleToBitmap(Sample));

            try
            {
                nom = 0;
                DPFP.FeatureSet features = FingerPrintUtility.ExtractFeatures(Sample, DPFP.Processing.DataPurpose.Verification);
                SetPanelColor(Color.FromArgb(64, 69, 76));
                SetPrompt("Detectando...");
                if (features != null)
                {
                    // Compare the feature set with our template
                    verified = false;
                    foreach (DPFP.Template template in this.Samples.Keys)
                    {

                        DPFP.Verification.Verification.Result result = new DPFP.Verification.Verification.Result();
                        Verificator.Verify(features, template, ref result);
                        if (result.Verified)
                        {
                            this.VerifiedObject = Samples[template];
                            verified = true;
                            SetPrompt("Verificado");
                            resultado = nom;
                            Stop();



                        }
                        nom++;
                    }
                    this.IsVerificationComplete = verified;
                    if (!verified)
                    {
                        SetPrompt("Incorrecta");
                    }
                }
                else
                {
                    SetPrompt("Fallando");
                }
            }
            catch (Exception e)
            {
                SetPrompt("Error!");

            }
        }

        public virtual void Init()
        {
            try
            {
                Capturer = new DPFP.Capture.Capture();
                Verificator = new DPFP.Verification.Verification();
                if (null != Capturer)
                {
                    Capturer.EventHandler = this;
                }
                else
                {
                    MessageBox.Show("No se pudo iniciar la captura!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch
            {
                MessageBox.Show("Error al iniciar la captura!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Start()
        {
            if (null != Capturer)
            {
                try
                {
                    Capturer.StartCapture();
                    SetPrompt("Detectando");
                }
                catch
                {
                    SetPrompt("Lector ocupado");
                }
            }
        }

        public void Stop()
        {
            if (null != Capturer)
            {
                try
                {
                    Capturer.StopCapture();
                }
                catch
                {
                    SetPrompt("Termino de captura!");
                }
            }
        }

        public void OnComplete(object Capture, string ReaderSerialNumber, DPFP.Sample Sample)
        {
            SetPrompt("Captura completa.");
            Process(Sample);


        }

        public void OnFingerGone(object Capture, string ReaderSerialNumber)
        {
            SetPrompt("Incorrecta");
            SetPanelColor(Color.FromArgb(64, 69, 76));
        }

        public void OnFingerTouch(object Capture, string ReaderSerialNumber)
        {

            SetPrompt("Detectando");
            SetPanelColor(Color.Red);
        }

        public void OnReaderConnect(object Capture, string ReaderSerialNumber)
        {
            SetPrompt("Conectado");
        }

        public void OnReaderDisconnect(object Capture, string ReaderSerialNumber)
        {
            SetPrompt("Desconectado");
        }

        public void OnSampleQuality(object Capture, string ReaderSerialNumber, DPFP.Capture.CaptureFeedback CaptureFeedback)
        {
            if (CaptureFeedback == DPFP.Capture.CaptureFeedback.Good)
                SetPrompt("Good scan.");
            else
                SetPrompt("Poor scan.");
        }

        #endregion



        private void SetPrompt(string prompt)
        {
            this.Invoke(new Function(delegate ()
            {
                Prompt.Text = prompt;

            }));
        }

        private void SetPanelColor(Color color)
        {
            this.Invoke(new Function(delegate ()
            {
                this.BackColor = color;
            }));
        }

        private void DrawPicture(Bitmap bitmap)
        {
            this.Invoke(new Function(delegate ()
            {
                FingerPrintPicture.Image = new Bitmap(bitmap, FingerPrintPicture.Size);	// fit the image into the picture box
            }));
        }

        private void ClearFPSamples_Click(object sender, EventArgs e)
        {
            this.IsVerificationComplete = false;
            SetPrompt("Otener mas huella.");
            FingerPrintPicture.Image = null;
            Start();
        }
        public bool verificando()
        {
            return verified;
        }
        public void limpiarhuella()
        {
            FingerPrintPicture.Image = null;
            verified = false;
            Start();
        }
        public int nombre()
        {

            return resultado;
        }

        private void FingerPrintPicture_Click(object sender, EventArgs e)
        {

        }
    }
}
