using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace CSHARP_ATM
{
    class Prog
    {

        private Account[] ac = new Account[3];
        private Thread[] ATMThreads;
        private ThreadStart[] ATMStart;

        /*
         * This function initilises the 3 accounts 
         * and instanciates the ATM class passing a referance to the account information
         * Also creates as man threads as the user has requested for ATMs
         * 
         */
        public Prog(int ATMsToMake)
        {
            //prompts the user, asking whether or not they want a data race to occur
            bool dataRace = getdataRaceOption();
            
            ac[0] = new Account(300, 1111, 111111, dataRace, ATMsToMake);
            ac[1] = new Account(750, 2222, 222222, dataRace, ATMsToMake);
            ac[2] = new Account(3000, 3333, 333333, dataRace, ATMsToMake);

            //initialise threads
            ATMThreads = new Thread[ATMsToMake];
            ATMStart = new ThreadStart[ATMsToMake];

            for (int i = 0; i < ATMsToMake; i++)
            {
                ATMStart[i] = new ThreadStart(threadBegin);
                ATMThreads[i] = new Thread(ATMStart[i]);

                //begin the threads running
                ATMThreads[i].Start();
            }

        }
        /*
         * Returns true or false based on whether the user wants to enable data race
         */
        private bool getdataRaceOption()
        {
            DialogResult result;

            result = MessageBox.Show("Would you like to enable the data Race?", "Data Race Condition", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                return true;
            }

            return false;

        }

        //used by threads, instantiates a new ATM object
        public void threadBegin()
        {
            ATMForms form = new ATMForms();

            ATM atm = new ATM(ac, form, 1);
        }

     //   static void Main(string[] args)
       // {
       //     new Program();
       // }
    }
    /*
     *   The Account class encapusulates all features of a simple bank account
     */
}
