using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;


    class BalanceOfCodes
    {
        private double balance;
    //Note: ReadDataset Object will be responsible of concatenating the scores of a holistic bin or each analytic bin separately
    //Form will call this constructor to calculate balance of scores, then will call GetBalance() to get the contents of List<double> balance;
    //Form will call this function only once for the holistic bin. 
    //Form will call the function up to the number of analytic bins to have the balance of each bin
    //Programmer: Marisol Mercado Santiago (marisolvsh@gmail.com)
    //Automated Analysis of Constructed Response Research Group
    //Reference: https://stats.stackexchange.com/questions/239973/a-general-measure-of-data-set-imbalance
    public BalanceOfCodes(double k, double n, List<double> c, List<string> distinctLevels)
        {
            double H = 0;
            double temp;
            double logTemp;

            //k=number of distinct levels, 
            //n=number of responses in the dataset, 
            //c=count of responses for each level
            for (int i = 0; i < distinctLevels.Count; i++)
            {
                temp = c[i]/n;
                logTemp = Math.Log(temp);
                H += -(temp * logTemp);  
            }

            balance = (H / Math.Log(k));
            if (Double.IsNaN(balance))
            { balance = 0; }
        }

        public double GetBalance()
        { return balance;}
        
    }

