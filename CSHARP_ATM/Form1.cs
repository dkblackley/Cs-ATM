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
    public partial class Form1 : Form
    {
        TextBox display;
        RichTextBox logBox;

        public Form1()
        {
           //initialise UI objects for the control screen
           
            //quit button for exiting application

            Button Quit = new Button();
            Quit.Click += new EventHandler(this.quitApp);
            Quit.Text = "Quit";
            Quit.Location = new Point(100, 200);
            Controls.Add(Quit);

            //button to start the ATM process
            Button Start = new Button();
            Start.Click += new EventHandler(this.Startapp);
            Start.Text = "Start";
            Start.Location = new Point(100, 300);
            Controls.Add(Start);

            //button to display logs
            Button displayLogs = new Button();
            displayLogs.Click += new EventHandler(this.readLogs);
            displayLogs.Text = "Display logs";
            displayLogs.AutoSize = true;
            displayLogs.Location = new Point(100, 247);
            Controls.Add(displayLogs);

            //prompt user for number of ATMS needed
            Label prompt = new Label();
            prompt.Location = new Point(300, 100);
            prompt.AutoSize = true;
            prompt.Text = "How many ATMs would you like to run?";
            prompt.Show();
            Controls.Add(prompt);

            //input for user to add number of ATMs
            display = new TextBox();
            display.Location = new Point(300, 130);
            display.Text = "2";
            display.Show();
            Controls.Add(display);

            //textbox for showing logs
            logBox = new RichTextBox();
            logBox.Location = new Point(300, 250);
            logBox.Text = "";
            logBox.Size = new Size(400, 150);
            logBox.Multiline = true;
            logBox.Show();
            Controls.Add(logBox);


            InitializeComponent();
           // initialise UI 
        }

        private void Form1_Load(object sender, EventArgs e)
        {
          
        }

        void DisplayE(object sender, EventArgs e)
        {
           
           
        }

        void quitApp(object sender, EventArgs e)
        {

            Application.Exit();
        }
        /*
         * Calls the logwriter and reads in then displays log data
         */
        public void readLogs(object sender, EventArgs e)
        {
            logWriter readLogs = new logWriter();

            logBox.Text = readLogs.returnData();

        }

        void Startapp(object sender,EventArgs e)
        {
            //read in number of ATMS needed
            int numOfATMS = Convert.ToInt32(display.Text);

            //check if number of ATMs is appropiate
            if (numOfATMS > 6 || numOfATMS < 1)
            {
                MessageBox.Show("Please enter between 1 and 6 ATMs", "Please adjust number of ATMs!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } else
            {
                //run program
                Prog pr = new Prog(numOfATMS);
            }

            
        }

    }
}
