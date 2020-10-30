using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DPFP.Error;
using System.IO;
using System.Data.SqlClient;
using Autobuses.Administracion;

namespace Autobuses
{
    public partial class RegistrationUserControl : UserControl, IComponent, DPFP.Capture.EventHandler
    {
        
        private bool isRegistrationComplete = false;
        private int count = 0;
        private Bitmap img1;
 

        public bool IsRegistrationComplete
        {
            get { return isRegistrationComplete; }
            private set
            {
                this.isRegistrationComplete = value;
                if (this.RegistrationCompletedStatusChanged != null)
                {
                    RegistrationCompletedStatusChanged(this, new StatusChangedEventArgs(value));
                }
            }
        }


        public event StatusChangedEventHandler RegistrationCompletedStatusChanged;

        public byte[] FingerPrint;

        public Bitmap img;
        private DPFP.Capture.Capture Capturer;
        private DPFP.Processing.Enrollment Enroller;

        public RegistrationUserControl()
        {
            InitializeComponent();
            this.Load += new EventHandler(FingerPrintRegistrationUserControl_Load);
            this.HandleDestroyed += new EventHandler(FingerPrintRegistrationUserControl_HandleDestroyed);
        }

        #region FingerPrint Handlers

        protected virtual void Process(DPFP.Sample Sample)
        {
            DrawPicture(FingerPrintUtility.ConvertSampleToBitmap(Sample));
            try
            {
                DPFP.FeatureSet features = FingerPrintUtility.ExtractFeatures(Sample, DPFP.Processing.DataPurpose.Enrollment);
                if (features != null)
                {
                    try
                    {
                        SetPrompt("Intento realizado.");
                        SetPanelColor(Color.FromArgb(64, 69, 76));
                        Enroller.AddFeatures(features);		// Add feature set to template.
                    }
                    catch (SDKException ex)
                    {
                        SetPrompt(ex.Message);
                    }
                    finally
                    {
                        UpdateSamplesNeeded();

                        // Check if template has been created.
                        switch (Enroller.TemplateStatus)
                        {
                            case DPFP.Processing.Enrollment.Status.Ready:	// report success and stop capturing
                                OnTemplateCollect(Enroller.Template);
                                SetPrompt("Terminado.");
                                Stop();
                                break;

                            case DPFP.Processing.Enrollment.Status.Failed:	// report failure and restart capturing
                                Enroller.Clear();
                                Stop();
                                UpdateSamplesNeeded();
                                OnTemplateCollect(null);
                                Start();
                                break;
                        }
                    }
                }
                else
                {
                    SetPrompt("Sin detectar huella.");
                    UpdateSamplesNeeded();
                }
            }
            catch (Exception)
            {
                SetPrompt("Sin detectar huella.");
                UpdateSamplesNeeded();
            }

        }

        protected virtual void Init()
        {
            try
            {
                Capturer = new DPFP.Capture.Capture();
                Enroller = new DPFP.Processing.Enrollment();
                this.TotalFeaturesNeeded = Enroller.FeaturesNeeded;

                if (null != Capturer)
                {
                    Capturer.EventHandler = this;
                }
                else
                {
                    MessageBox.Show("No se pudo capturar!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch
            {
                MessageBox.Show("No se puedo capturar!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected void Start()
        {
            if (null != Capturer)
            {
                try
                {
                    Capturer.StartCapture();
                    SetPrompt("Coloca el dedo en el lector.");
                }
                catch
                {
                    SetPrompt("No se pudo inicializar la captura!");
                }
            }
        }

        protected void Stop()
        {
            if (null != Capturer)
            {
                try
                {
                    Capturer.StopCapture();
                }
                catch
                {
                    SetPrompt("No se puede terminar la captura!");
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
            SetPrompt("Volver a colocar la huella.");
            SetPanelColor(Color.FromArgb(64, 69, 76));
        }

        public void OnFingerTouch(object Capture, string ReaderSerialNumber)
        {

            SetPrompt("Coloca tu dedo en el lector.");
            SetPanelColor(Color.Red);
        }

        public void OnReaderConnect(object Capture, string ReaderSerialNumber)
        {
            SetPrompt("Listo para capturar huella.");
        }

        public void OnReaderDisconnect(object Capture, string ReaderSerialNumber)
        {
            SetPrompt("Lector desconectado.");
        }

        public void OnSampleQuality(object Capture, string ReaderSerialNumber, DPFP.Capture.CaptureFeedback CaptureFeedback)
        {
            if (CaptureFeedback == DPFP.Capture.CaptureFeedback.Good)
                SetPrompt("Listo.");
            else
                SetPrompt("escaneo malo .");
        }

        private void OnTemplateCollect(DPFP.Template template)
        {
            if (template != null)
            {

                MemoryStream s = new MemoryStream();
                    template.Serialize(s);
                    this.FingerPrint = s.ToArray();
                    this.IsRegistrationComplete = true;
               
              //  MessageBox.Show("Agregado!");
     
            }
        }

        public byte [] datos()
        {
            byte[] enviar = null;
            enviar = this.FingerPrint;
            return enviar;
        }
        #endregion

        #region Form Event Handlers:
        private void FingerPrintRegistrationUserControl_Load(object sender, EventArgs e)
        {
            Init();
            Start();
            UpdateSamplesNeeded();
        }

        private void FingerPrintRegistrationUserControl_HandleDestroyed(object sender, EventArgs e)
        {
            Stop();
        }

        #endregion

        delegate void Function();

        private void UpdateSamplesNeeded()
        {
            this.Invoke(new Function(delegate ()
            {
                SamplesNeeded.Text = String.Format("Verificació  : {0}/{1}", this.TotalFeaturesNeeded - Enroller.FeaturesNeeded, this.TotalFeaturesNeeded);
            }));
        }

        private void SetPrompt(string prompt)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Function(delegate ()
                {
                    Prompt.Text = prompt;
                }));
            }
            else {
                Prompt.Text = prompt;
            }

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
                FingerPrintPicture.Image = new Bitmap(bitmap, FingerPrintPicture.Size); // fit the image into the picture box
                if (count == 0)
                {
                    img1 = new Bitmap(bitmap, FingerPrintPicture.Size);
                    count++;
                }
               
            }));
        }
        public Bitmap imagenregreso()
        {
            return img1;
        }
     
        private uint TotalFeaturesNeeded;

        private void ClearFPSamples_Click(object sender, EventArgs e)
        {
            this.Reset();
   
        }

        public void Reset()
        {
            Enroller.Clear();
            this.IsRegistrationComplete = false;
            this.FingerPrint = null;
            UpdateSamplesNeeded();
            SetPrompt("Give fingerprint samples again.");
            FingerPrintPicture.Image = null;
            Start();
        }

        private void SamplesNeeded_Click(object sender, EventArgs e)
        {

        }
    }
}