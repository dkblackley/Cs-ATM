using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

/*
 * The following class is responsible for reading and writing from the file logs.txt
 * It seperates the data in the file by using a semi colon (;)
 */

namespace CSHARP_ATM
{
    class logWriter
    {

        DateTime now;
        StreamReader sr;
        StreamWriter sw;

        //constructor checks if file exists, if not then makes it.
        public logWriter()
        {
            if (!File.Exists("logs.txt"))
            {
                FileStream fs = File.Create("logs.txt");
                fs.Close();
            }
        }

        /*
         * The following function using the streamwriter to append to the logs.txt file
         * This method only appends the time that the account was accessed
         * Param : accnum The account number that was accessed
         */
        public void logAccess(int accnum)
        {

            try
            {
                sw = new StreamWriter("logs.txt", true);

                now = DateTime.Now; //get current date
                string date = now.ToString("F"); // convert to date and time
                string account = Convert.ToString(accnum);

                string dataToWrite = account + ";" + date;

                sw.WriteLine(dataToWrite);  // write data to file


                sw.Close();
            } 
            catch (Exception e) //catch any errors
            {

                MessageBox.Show("An Error Has occured reading And writing to log files. Logs will be cleared.", "ERROR IN LOGS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                File.Delete("logs.txt");
                File.Create("logs.txt");

            }
            
            
        }

        /*
         * The following method logs the time and transaction that occured
         * very similar to above method
         * Param : accnum The account number that was used
         * Param : money the amount of money taken out
         */
        public void logTransaction(int accnum, int money)
        {

            try
            {
                sw = new StreamWriter("logs.txt", true);

                string account = Convert.ToString(accnum);
                string amount = Convert.ToString(money);

                now = DateTime.Now;
                string date = now.ToString("F");

                string dataToWrite = account + ";" + amount + ";" + date;

                sw.WriteLine(dataToWrite);

                sw.Close();
            }
            catch (Exception e) 
            {

                MessageBox.Show("An Error Has occured reading And writing to log files. Logs will be cleared.", "ERROR IN LOGS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                File.Delete("logs.txt");
                File.Create("logs.txt");
            }
            
        }

        /*
         * The following method reads the log data from the file and returns it in a form that is easy to read
         * 
         * Return : The string of data that was read and converted
         */
        public String returnData()
        {

            string dataRead = null;

            try
            {
                sr = new StreamReader("logs.txt");
              
                string currentLine;

                while ((currentLine = sr.ReadLine()) != null) //read data in line by line
                {

                    int count = currentLine.TakeWhile(c => c == ';').Count(); // ';' is the character used to seperate the account numbers from dates etc.

                    //counts the number of ;'s in a file to determine whether it shows just the transaction date or account accessed
                    for (int n = 0; n < currentLine.Length; n++)
                    {
                        char c = currentLine[n];
                        if (c == ';')
                        {
                            count++;
                        }
                    }


                    if (count == 2) //if only two occurences of ; then it shows the transaction date
                    {
                        int i = 0;

                        dataRead += "Account; ";

                        for (int n = 0; n < currentLine.Length; n++)
                        {
                            char c = currentLine[n];

                            if (c == ';') //find where the ; occurs and read that data into the string
                            {
                                i++;

                                if (i == 1)
                                {
                                    dataRead += " Took out £";
                                }
                                else if (i == 2)
                                {
                                    dataRead += " at ";
                                }
                            }
                            else
                            {
                                dataRead += c;
                            }
                        }

                        dataRead += '\n'; //add a newline
                    }
                    else //else it only shows dte accessed
                    {
                        dataRead += "Account; ";

                        for (int n = 0; n < currentLine.Length; n++)
                        {
                            char c = currentLine[n];

                            if (c == ';')
                            {
                                dataRead += " Was Accessed at ";
                            }
                            else
                            {
                                dataRead += c;
                            }
                        }
                        dataRead += '\n';
                    }
                }

                //if logs.txt has no logs
                if (String.IsNullOrEmpty(dataRead))
                {
                    dataRead = "No log files found";
                }

                sr.Close();

            }
            catch (Exception e)
            {

                MessageBox.Show("An Error Has occured reading And writing to log files. Logs will be cleared.", "ERROR IN LOGS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                File.Delete("logs.txt");
                File.Create("logs.txt");
            }

            return dataRead;
        }
    }
}
