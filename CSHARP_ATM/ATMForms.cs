using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSHARP_ATM
{
    public partial class ATMForms : Form
    {
        public ATMForms()
        {
           



            //format form for the ATM
            this.BackColor = Color.SlateGray;
            
            //quit button to close ATM
            Button Quit = new Button();
            Quit.Click += new EventHandler(this.quitApp);
            Quit.Text = "Quit";
            Controls.Add(Quit);
            Quit.ForeColor = Color.White;
            Quit.Location = new Point(100, 200);


            InitializeComponent();
        }

        private void ATMForms_Load(object sender, EventArgs e)
        {

            //card slot picture
            Image slot = Image.FromFile(@"slot.png");
            PictureBox pb = new PictureBox();
          //resize to appropiate dimensions
            pb.Image = (Image)(new Bitmap(slot, new Size(130, 95)));
            pb.SizeMode = PictureBoxSizeMode.AutoSize;
           
            pb.Location = new Point(400, 200);
            this.Controls.Add(pb);
            pb.Show();

        }

      

        void quitApp(object sender, EventArgs e)
        {

            this.Close();
        }
    }
}
