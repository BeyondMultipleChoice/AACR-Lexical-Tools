using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Balance_Calculator
{
    //Programmer: Marisol Mercado Santiago (marisolvsh@gmail.com)
    //Automated Analysis of Constructed Response Research Group

    //Acknowledgements: 
    //This material is based upon work supported by the National Science Foundation(Grants 1323162 and 1347740). 
    //Any opinions, findings and conclusions or recommendations expressed in this material are those of the author(s) and do not necessarily reflect the views of the supporting agencies.

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
