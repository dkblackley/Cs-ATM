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
    public partial class ATMForm : Form
    {
        public ATMForm()
        {
            InitializeComponent();
        }

        private void ATMForm_Load(object sender, EventArgs e)
        {
            Button Enter = new Button();
            //Enter.Click += new EventHandler(this.DisplayE);
            Enter.Text = "Enter";
            Enter.Location = new Point(100, 100);
            Enter.Show();

            Button Quit = new Button();
            Quit.Click += new EventHandler(this.quitApp);
            Quit.Text = "Quit";
            Controls.Add(Quit);
            Quit.Location = new Point(100, 200);
            Controls.Add(Enter);


            InitializeComponent();
        }

        void quitApp(object sender, EventArgs e)
        {

            Application.Exit();
        }
    }
}
