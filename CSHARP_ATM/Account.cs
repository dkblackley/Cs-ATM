using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CSHARP_ATM
{
    class Account
    {
        //the attributes for the account
        private int balance;
        private int pin;
        private int accountNum;

        //multithreading attributes
        private Semaphore withdrawing;
        private Barrier threadSyncer;
        private bool synced = false;
        private bool dataRace;
        
        //used to make logs
        private logWriter logs;

        // a constructor that takes initial values for each of the attributes (balance, pin, accountNumber)
        public Account(int balance, int pin, int accountNum, bool dataRace, int ATMsToMake)
        {
            this.balance = balance;
            this.pin = pin;
            this.accountNum = accountNum;
            this.withdrawing = new Semaphore(1, 1);
            this.threadSyncer = new Barrier(ATMsToMake);
            this.dataRace = dataRace;
            //stop mutliple threads creating a file
            withdrawing.WaitOne();
            this.logs = new logWriter();
            withdrawing.Release();
            
        }

        //getter and setter functions for balance
        public int getBalance()
        {
            return balance;
        }

        public void setBalance(int newBalance)
        {
            //uses a semaphore so that both threads don't try to overwrite this number at the same time, though Ideally decrement balance should be called
            //this is just used as a precaution
            withdrawing.WaitOne();
            this.balance = newBalance;
            withdrawing.Release();
        }

        public void setpin(int p)
        {
            //in event of account needing to be locked
            this.pin = p;

        }

        public bool lockedAcc()
        {
            if (this.pin == -1)
            {
                return true;

            }
            else
            {

                return false;
            }

        }



        //get whether this account has to have threads runnning in sync (for showing data race)
        public bool getSynced()
        {
            return this.synced;
        }

        //setter for sync variable
        public void setSynced(bool newSync)
        {
            this.synced = newSync;
        }

        /*
         *   This funciton allows us to decrement the balance of an account
         *   it perfomes a simple check to ensure the balance is greater than
         *   the amount being removed. If datarace condition is enabled then it
         *   will not use a semaphore and deliberately allow the program to mess up
         *   
         *   param : amount the amount to decrement from the accounts balance
         *   returns:
         *   true if the transactions if possible
         *   false if there are insufficent funds in the account
         */
        public Boolean decrementBalance(int amount)
        {
            //always use a semaphore for writing to the log file, to avoid any errors
            withdrawing.WaitOne();
            logs.logTransaction(this.accountNum, amount);
            withdrawing.Release();

            if (dataRace) //if we want a data race to occur
            {
                if (this.getSynced())   // if we want the threads to be synced
                {
                    threadSyncer.SignalAndWait();   // wait for the other thread
                }

                if (this.balance > amount)
                {
                    //roundabout way of setting balance, to prove data race is occuring
                    int temp = balance;
                    temp -= amount;
                    balance = temp;
                    return true;
                }
                else
                {
                    return false;
                }
                
            }
            else
            {
                return decrementNoRace(amount);
            }
        }


        /*
         *   This funciton allows us to decrement the balance of an account
         *   it perfomes a simple check to ensure the balance is greater than
         *   the amount being withdrawn
         *   
         *   Also makes sure that no data race will occur by only allowing one thread to access the
         *   balance at any one time
         *   
         *   param : amount the amount to decrement from the accounts balance
         *   returns:
         *   true if the transactions if possible
         *   false if there are insufficent funds in the account
         */
        public Boolean decrementNoRace(int amount)
        {
            //if the account is to wait for another thread
            if (this.getSynced())
            {
                //pause the thread until there are two
                threadSyncer.SignalAndWait();

            }

            //make sure that there are not multiple threads trying to access the balance by using a semaphore
            withdrawing.WaitOne();
            if (this.balance > amount)
            {
                //roundabout way of setting balance, to prove data race hase been fixed
                int temp;
                temp = balance;
                temp -= amount;
                balance = temp;

                //free up semaphore to allow second thread through
                withdrawing.Release();

                return true;
            }
            else
            {
                //free up semaphore
                withdrawing.Release();

                return false;
            }
        }


        /*
         * This funciton check the account pin against the argument passed to it
         *
         * param : pinEntered the pin to be checked
         * returns:
         * true if they match
         * false if they do not
         */
        public Boolean checkPin(int pinEntered)
        {
            if (pinEntered == pin)
            {
                withdrawing.WaitOne();
                logs.logAccess(this.accountNum);
                withdrawing.Release();

                return true;
            }
            else
            {

                return false;
            }
        }
        //getter for the account number
        public int getAccountNum()
        {
            return accountNum;
        }

    }
    /* 
     *      This is our main ATM class that preforms the actions outlined in the assigment hand out
     *      
     *      the constutor contains the main funcitonality.
     */
}
