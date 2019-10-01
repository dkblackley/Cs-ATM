using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace CSHARP_ATM
{
    class ATM
    {

        //local referance to the array of accounts
        private Account[] ac;
        // private TextBox display = new TextBox();
        // DisplayNameAttribute.
        // display.Location = new Point(200,250);
        //this is a referance to the account that is being used

        private Account activeAccount = null;

        //create box to display messages
        public TextBox display = new TextBox();

        //create number pad buttons
        public Button[,] numpad = new Button[3,4];

        //set number of maximum pin tries
        public int pincount = 3;

        //button to select synchronised threads
        public Button syncThreads = new Button();

        //reference to form
        public ATMForms form;

        //next step button initialised
        public Button next = new Button();

        //create box to input numbers
        public TextBox dial = new TextBox();

        //boolean to see if process had ended and allow repeat
        public bool cli = false;

        //variable to see what state the atm is in
        String nxte = "";

        // the atm constructor takes an array of account objects as a referance
        public ATM(Account[] ac, ATMForms frm, int no)
        {
           

            this.ac = ac;
            this.form = frm;

            this.numpad = new Button[3,4];
            //set attributes, settings of UI objects
            dial.Location = new Point(200, 150);//*no+50, 150);
            next.Location = new Point(300, 150);//*no+50, 150);
            dial.ForeColor = Color.White;
            dial.ReadOnly = true;
            dial.BackColor = Color.Black;
            display.ReadOnly = true;
            display.BackColor = Color.Black;
            display.ForeColor = Color.LightGreen;

            //The following button is clicked to make the current account wait for another user to access
            //and withdraw from the same account
            syncThreads.Location = new Point(100, 300);
            this.syncThreads.Text = "Make thread withdraw simultaneously";
            syncThreads.Click += new EventHandler(syncClicked);
            syncThreads.AutoSize = true;
            syncThreads.ForeColor = Color.DarkRed;
            form.Controls.Add(syncThreads);

            //set up next button
            next.Text = "NEXT";
            next.Click += new EventHandler(nextclicked);
            next.Show();

            //add items to form
            form.Controls.Add(next);
            form.Controls.Add(dial);
            dial.Show();
          

            writer("hello from ATM");
            display.Multiline = true;
            Font dis = new Font("Calibri", 12.0f);
                 
            
            display.Location = new Point(200, 50);

            display.Font = dis;
            display.Size = new Size(400, 50);
            form.Controls.Add(display);
            display.Show();
     

            // set up numpad buttons
            for (int i = 0; i < 3; i++)
            {
               // count = 1;
               
                for (int t = 0; t < 4; t++)
                {

                    int startpos = 100 * no;
                    numpad[i,t] = new Button();

                    numpad[i,t].Click += new EventHandler(readnum);
                    //numpad[i,t].Text = Convert.ToString(count);
                    numpad[i,t].Location = new Point(200+(i * 50), 200+(20*t));
                    form.Controls.Add(numpad[i,t]);
                    
                    numpad[i,t].Show();
                }
            }

            //labelling numpad buttons
            for(int i=0; i<3; i++)
            {
                numpad[i, 0].Text = Convert.ToString(i+1);

            }
            for (int i = 0; i < 3; i++)
            {
                numpad[i, 1].Text = Convert.ToString(4 + i);

            }
            for (int i = 0; i < 3; i++)
            {
                numpad[i, 2].Text = Convert.ToString(7 + i);

            }

            numpad[1,3].Text = "0";
            numpad[0,3].Text = "CLEAR";

            //ask for account number and store result in acctiveAccount (null if no match found)

            display.Text = "Enter your account number...";
            nxte = "Account";
            //hide tha baility to synce accounts while there is no active account
            syncThreads.Hide();

            frm.ShowDialog();

        }

        private void syncClicked(object sender, EventArgs e)
        {
            if (activeAccount.getSynced()) {
                activeAccount.setSynced(false);
                syncThreads.ForeColor = Color.DarkRed;

            }
            else
            {
                activeAccount.setSynced(true);
                syncThreads.ForeColor = Color.DarkGreen;
            }

            
        }

        /*
         *    this method promts for the input of an account number
         *    the string input is then converted to an int
         *    a for loop is used to check the enterd account number
         *    against those held in the account array
         *    if a match is found a referance to the match is returned
         *    if the for loop completest with no match we return null
         * 
         */
        private Account findAccount()
        {

            String inp = dial.Text;
            if (inp != "")
            {
                int input = Convert.ToInt32(inp);
                for (int i = 0; i < this.ac.Length; i++)
                {
                    if (ac[i].getAccountNum() == input)
                    {

                        return ac[i];
                    }
                }
                return null;
            }
            return null;
        }
        /*
         * 
         *  this jsut promt the use to enter a pin number
         *  
         * returns the string entered converted to an int
         * 
         */
        private void promptForPin()
        {
            //check if chosen account is locked
            if (activeAccount.lockedAcc())
            {
                writer("Your account has been locked, please contact the bank.");
                pincount = 1;
                nxte = "root";
                return;

            }
            //prompt user to enter pin
            nxte = "pin";
            Console.WriteLine(nxte);
            writer("enter pin:");
        }

        /*
         * 
         *  give the use the options to do with the accoutn
         *  
         *  promt for input
         *  and defer to appropriate method based on input
         *  
         */
        private void dispOptions()
        {
            syncThreads.Show();
            writer("1> take out cash 2> balance 3> exit");
            nxte = "options";
        }

        /*
         * offer withdrawable amounts
         * 
         * based on input attempt to withraw the corosponding amount of money
         * 
         */
        private void dispWithdraw()
        {
            writer("1> 10 2> 20 3> 40 4> £100 5> £500");
            nxte = "withdraw";
            
        }
        /*
         *  display balance of activeAccount and await keypress
         *  
         */
        private void dispBalance()
        {
            if (this.activeAccount != null)
            {
                cli = false;
                writer(" your current balance is : " + activeAccount.getBalance()+ " (press next to continue)");
                dial.Text = "";
                //set state of machine to prepare for loop
                nxte = "restart";
            }
        }

        //function to output text to the display
        public void writer(String str)
        {
            display.Text = str;
        }


        
       //function to read input from numpad

        public void readnum(object sender, EventArgs e)
        {

            var button = sender as Button;
            if (button.Text == "CLEAR")
            {
                dial.Text = "";
                return;
            }

            if ((dial.Text.Length < 1) && ((nxte == "options")||(nxte=="withdraw")))
            {
                dial.Text += button.Text;
            }

            if ((dial.Text.Length < 4) && (nxte == "pin"))
            {
                dial.Text += button.Text;
            }


            if ((dial.Text.Length < 6) && (nxte == "Account"))
            {
                dial.Text += button.Text;
            }

         
        }
        //play beep sound effect

        private void playBeep()
        {
            SoundPlayer beep = new SoundPlayer(@"beep.wav"); // from http://soundbible.com/1251-Beep.html
            beep.Play();
        }





        //function to control actions when next is clicked

        private void nextclicked(object sender, EventArgs e)
        {
            //play beep sound effect
            playBeep();

            //set clicked variable to true
            cli = true;

            //if state of machine is asking for account number
            if (nxte == "Account")
            {
                //find account if it exists
                activeAccount = this.findAccount();


                //if it exists
                if (activeAccount != null)
                {
                    //check if account is locked
                    if (pincount == 1)
                    {

                        nxte = "root";
                        return;
                    }


                    //if the account is found check the pin 
                    else
                    {
                        //set tries of pin entering
                        pincount = 3;
                        promptForPin();
                    }
                }
                else
                {   //if the account number entered is not found let the user know!
                    writer("no matching account found.");
                  
                }

                //wipes all text
              
                dial.Text = "";

            }

            if (nxte == "pin")
            {
                //if state is asking for pin
                
                //check input
                String str = dial.Text;

               


                //if input is 4 characters long
                if (str.Length == 4)
                {
                    //check pin entered
                    int pinNumEntered = Convert.ToInt32(str);
                    if (activeAccount.checkPin(pinNumEntered))
                    {
                        //if the pin is a match give the options to do stuff to the account (take money out, view balance, exit)
                        dispOptions();

                    }


                    //if tries have been exceeded
                    else if (pincount == 1)
                    {
                        //lock user out of account
                        writer("Please contact bank to reactivate account");
                        activeAccount.setpin(-1);
                        
                    }

                    else
                    {   //if the account number entered is not found let the user know!
                        pincount = pincount - 1;
                        writer("Incorrect pin, you have "+pincount+" attempt(s) left. Re-enter and try again.");
                        
                    }

                  
                }


                dial.Text = "";
            }


            //if machine state is the option screen
            if (nxte == "options")
            {
                String strinput = dial.Text;
                if (strinput.Length == 1)
                {
                    //check if input is one character
                    int input = Convert.ToInt32(strinput);


                    //select correct options
                    if (input == 1)
                    {
                        dispWithdraw();
                    }
                    else if (input == 2)
                    {
                        dispBalance();
                    }
                    else if (input == 3)
                    {
                        //change state to quit
                        cli = false;
                        writer("Press next to quit");
                        nxte = "restart";
                        dial.Text = "";
                    }
                    else
                    {

                    }
                }

                dial.Text = "";
            }
            if (nxte == "withdraw")
            {
                //if state is withdraw screen
                String str = dial.Text;

                //if string is one character long
                if (str.Length == 1)
                {
                    int input = Convert.ToInt32(str);
                    //if option valid
                    if (input > 0 && input < 6)
                    {
                        displayUpdating();
                        //opiton one is entered by the user
                        if (input == 1)
                        {
                            //attempt to decrement account by 10 punds
                            if (activeAccount.decrementBalance(10))
                            {
                               
                                cli = false;
                                //bool is changed to false as it prepares for restart loop
                                //if this is possible display new balance and await key press
                                writer("new balance " + activeAccount.getBalance()+" (press next to continue)");
                                
                               
                                dial.Text = "";
                            }
                            else
                            {
                                cli = false;
                                //if this is not possible inform user and await key press
                                writer("insufficient funds"+" (press next to continue)");
                               
                                nxte = "restart";
                                dial.Text = "";
                            }
                        }
                        else if (input == 2)
                        {
                            if (activeAccount.decrementBalance(20))
                            {
                                
                                cli = false;
                                writer("new balance " + activeAccount.getBalance()+" (press next to continue)");
                             
                                nxte = "restart";
                                dial.Text = "";
                            }
                            else
                            {
                                cli = false;
                                writer("insufficient funds"+" (press next to continue)");
                               
                                nxte = "restart";
                                dial.Text = "";
                            }
                        }
                        else if (input == 3)
                        {
                            if (activeAccount.decrementBalance(40))
                            {
                                

                                cli = false;
                                writer("new balance " + activeAccount.getBalance()+" (press next to continue)");
                                
                                nxte = "restart";
                                dial.Text = "";
                            }
                            else
                            {
                                cli = false;
                                writer("insufficient funds"+" (press next to continue)");
                                
                                nxte = "restart";
                                dial.Text = "";

                            }
                        }

                        else if (input == 4)
                        {
                            if (activeAccount.decrementBalance(100))
                            {
                                
                                cli = false;
                                writer("new balance " + activeAccount.getBalance()+" (press next to continue)");
                               
                                nxte = "restart";
                                dial.Text = "";
                            }
                            else
                            {
                                cli = false;
                                writer("insufficient funds"+"(press next to continue)");
                                
                                nxte = "restart";
                                dial.Text = "";

                            }
                        }

                        else if (input == 5)
                        {
                            if (activeAccount.decrementBalance(500))
                            {
                               

                                cli = false;
                                writer("new balance " + activeAccount.getBalance()+" (press next to continue)");
                               
                                nxte = "restart";
                                dial.Text = "";
                            }
                            else
                            {
                                cli = false;
                                writer("insufficient funds"+" (press next to continue)");
                               
                                nxte = "restart";
                                dial.Text = "";

                            }
                        }
                    }
                }
            }

            if (display.Text.Contains("next")&&(cli==true))
            {
                //restart holding state
                
                nxte = "root";
            }

            if (nxte == "root")
            {
                //restart
                if (pincount == 1)
                {
                    //restart not enabled when machine goes into lockdown
                    //lock out of account
                    writer("Your account has been locked, please contact the bank.");
                    return;
                }

                //hide the sync thread option while no active account
                syncThreads.Hide();
                display.Text = "enter your account number...";
                nxte = "Account";
            }
        }
        //screen for update thread
        private void displayUpdating()
        {
            display.Text = "Updating Balance";
            display.Update();
         
        }
    }
}
        
    

