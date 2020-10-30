using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Autobuses
{
    public partial class loading : Form
    {
        
        public loading()
        {
            InitializeComponent();
            imagen.Load("cargadorcolores.gif");
            imagen.SizeMode = PictureBoxSizeMode.StretchImage;
            imagen.Location = new Point(this.Width / 2 - imagen.Width / 2, this.Height / 2 - imagen.Height / 2);
        }

        private void Loading_Load(object sender, EventArgs e)
        {
            imagen.Load("cargadorcolores.gif");
            imagen.SizeMode = PictureBoxSizeMode.StretchImage;

            imagen.Location = new Point(this.Width / 2 - imagen.Width / 2, this.Height / 2 - imagen.Height / 2);
        }
    }
}
