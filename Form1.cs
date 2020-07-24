using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Windows;
using System.Globalization;
using System.Collections;

//Interface where user inputs Excel or CSV file with student responses and human scores, and requests to calculate balance of codes and lexical analysis measurements
//Programmer: Marisol Mercado Santiago (marisolvsh@gmail.com)
//Automated Analysis of Constructed Response Research Group

//Acknowledgements: 
//This material is based upon work supported by the National Science Foundation(Grants 1323162 and 1347740). 
//Any opinions, findings and conclusions or recommendations expressed in this material are those of the author(s) and do not necessarily reflect the views of the supporting agencies.


namespace Balance_Calculator
{
   
    public partial class Form1 : Form
    {
        ReadDataset myData;
        DataTable myTable;
        List<string> myHeaders;


        public Form1()
        {
            InitializeComponent();
        }

        private void ButtonPath_Click(object sender, EventArgs e)
        {

            ClearAll();

            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.

            if (result == DialogResult.OK) // Test result.
            {
                string file = openFileDialog1.FileName;
                string ext = Path.GetExtension(openFileDialog1.FileName);

                    if (File.Exists(file))
                    {

                        myData = new ReadDataset(file, ext);
                        myHeaders = myData.GetHeaders();
                        myTable = myData.GetDataTable(); //original dataset as input by the user

                        labelPath.Text = "Data loaded. Please select response and score columns below.";

                        dataGridView1.DataSource = myTable;

                        startSR.Items.AddRange(myHeaders.ToArray());
                        endSR.Items.AddRange(myHeaders.ToArray());
                        startSC.Items.AddRange(myHeaders.ToArray());
                        endSC.Items.AddRange(myHeaders.ToArray());

                        labelFile.Text = Path.GetFullPath(openFileDialog1.FileName);

                    }
                    else
                    { MessageBox.Show("Please make sure that the file exists"); }
            }

        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void ClearAll()
        {
            labelPath.Text = "";

            if (myTable != null)
            {
                dataGridView1.DataSource = null;
                myTable.Clear();
                myHeaders.Clear();
                myData = null;

            }

            startSR.DataSource = null;
            startSR.Text = null;
            startSR.Items.Clear();

            endSR.DataSource = null;
            endSR.Text = null;
            endSR.Items.Clear();

            startSC.DataSource = null;
            startSC.Text = null;
            startSC.Items.Clear();

            endSC.DataSource = null;
            endSC.Text = null;
            endSC.Items.Clear();

            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();

            labelFile.Text = null;

        }

        private void ButtonCalculate_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();

            if (startSR.Text != "" && endSR.Text != "" && startSC.Text != "" && endSC.Text != "")
            {

                string file = openFileDialog1.FileName;
                string ext = Path.GetExtension(openFileDialog1.FileName);
                int startIndex = myTable.Columns.IndexOf(startSC.Text);
                int endIndex = myTable.Columns.IndexOf(endSC.Text);
                double k, n;
                List<double> c;
                double result;
                List<string> levelNames;
                BalanceOfCodes balanceResult;

                // ReadDataset Object functions call order from Form: 
                // (1) ConcatenateResponse, (2) ConcatenateCodes(), 
                // (3) GetNumberOfResponses(), (4) GetDistinctLevels(),
                // (5) GetNumberOfLevels(), (6) GetCountForEachLevel()
                //Values will be saved in Form, then Form will create a BalanceOfCodes object, then call GetBalance.
                //Return List of GetBalance() will be saved in Form to be then displayed
                //If the data has more than one bin: display the balance for each bin independently. Show results one by one in the textboxes
                myData.ConcatenateResponse(startSR.Text, endSR.Text);

                //Calculate the balance for each bin - one at a time. If there's only one bin, it will only show one response in the listbox
                for (int x = startIndex; x < endIndex + 1; x++)
                {
                    myData.ConcatenateCodes(myHeaders[x], myHeaders[x]);  //only one bin at a time, so x to x...

                    n = myData.GetNumberOfResponses();

                    levelNames = new List<string>();
                    levelNames = myData.GetDistinctLevels(); //only two (0 and 1) when it's an analytic. 2 or more if it's holistic.

                    k = myData.GetNumberOfLevels();

                    c = new List<double>();
                    c = myData.GetCountforEachLevel(levelNames);

                    balanceResult = new BalanceOfCodes(k, n, c, levelNames);
                    result = balanceResult.GetBalance();

                    textBox1.AppendText("Balance of " + myHeaders[x] + " bin: " + result);
                    textBox1.AppendText(Environment.NewLine);

                    textBox2.AppendText("---Information of " + myHeaders[x] + " bin: ---");
                    textBox2.AppendText(Environment.NewLine);
                    textBox2.AppendText("k: " + k);
                    textBox2.AppendText(Environment.NewLine);
                    textBox2.AppendText("n: " + n);
                    textBox2.AppendText(Environment.NewLine);

                    for (int i = 0; i < c.Count(); i++)
                    {
                        textBox2.AppendText("counts of " + levelNames[i] + ":  " + c[i]);
                        textBox2.AppendText(Environment.NewLine);
                    }

                }

                textBox1.AppendText(Environment.NewLine); //This is to signal the starting of a subsection

                //New object LexicalDiversityCalculator
                LexicalDiversityCalculator myCalc = new LexicalDiversityCalculator(myTable, startSR.Text, endSR.Text, "responses");
                
                //Show the TTR calculated:
                double ttr = myCalc.GetTTRResult();
                textBox1.AppendText("Type-to-token ratio: " + ttr);
                    //ttr.ToString("P", CultureInfo.InvariantCulture)); -->>> This shows the ttr (or any double) as a percentage.
                textBox1.AppendText(Environment.NewLine);
            

                //Show Brunet's Index:
                myCalc.CalculateBrunetIndex();
                double brunetIndex = myCalc.GetBrunetIndex();
                textBox1.AppendText("Brunet's Index: " + brunetIndex);
                textBox1.AppendText(Environment.NewLine);

                //Show Guiraud's R:
                myCalc.CalculateGuiraudR();
                double guiraudR = myCalc.GetGuiraudR();
                textBox1.AppendText("Guiraud's R: " + guiraudR);
                textBox1.AppendText(Environment.NewLine);

                //Show Honore's Statistic:
                myCalc.CalculateHonoreStatistic();
                double honoreS = myCalc.GetHonoreStatistic();
                textBox1.AppendText("Honore's Statistic: " + honoreS);
                textBox1.AppendText(Environment.NewLine);

                //Show Herdan C:
                myCalc.CalculateHerdanC();
                double herdanC = myCalc.GetHerdanC();
                textBox1.AppendText("Herdan's C: " + herdanC);
                textBox1.AppendText(Environment.NewLine);
                
                //Show Corrected TTR:
                myCalc.CalculateCorrectedTTR();
                double correctedTTR = myCalc.GetCorrectedTTR();
                textBox1.AppendText("Carroll's Corrected TTR: " + correctedTTR);
                textBox1.AppendText(Environment.NewLine);

                //Show Somers' S:
                myCalc.CalculateSomersS();
                double summerIndex = myCalc.GetSomersS();
                textBox1.AppendText("Somers' S: " + summerIndex);
                textBox1.AppendText(Environment.NewLine);

                //Show Uber Index:
                myCalc.CalculateUberIndex();
                double uberIndex = myCalc.GetUberIndex();
                textBox1.AppendText("Dugast's Uber Index: " + uberIndex);
                textBox1.AppendText(Environment.NewLine);

                //Show Yule's K
                myCalc.CalculateYuleK();
                double yuleK = myCalc.GetYuleK();
                int[] countsforEachDistinct = myCalc.GetvrCount();
                textBox1.AppendText("Yule's K: " + yuleK);
                textBox1.AppendText(Environment.NewLine);

                //Get number of words and distinct words
                double tokens = myCalc.GetNumberOfWords();
                double types = myCalc.GetNumberOfDistinctWords();

                //Show Vocd-D
                //The calculation needs at least 50 tokens. If this is not true, notify the user
                if (tokens < 50)
                {
                    textBox1.AppendText(Environment.NewLine);
                    textBox1.AppendText("--Note: Cannot calculate vocd-D: A minimum of 50 words (tokens) are needed." + Environment.NewLine);
                }
                else
                {
                    textBox1.AppendText(Environment.NewLine);
                    textBox1.AppendText("--Vocd-D results below:" + Environment.NewLine);
                    myCalc.CalculateVocdD(); 
                    double finalVocdD = myCalc.GetFinalVocdD();
                    double[] bigD = myCalc.GetD_Estimate();  //get the array with the result of the three runs of D-estimate calculations
                    int initial = 0;
                    int num = 16;
                    int j;

                    ArrayList ttrs = myCalc.Get48ttrs();
                    ArrayList ds = myCalc.Get48ds();
                    ArrayList stdevs = myCalc.Get48stdevs();

                   for (int i = 0; i < 3; i++)
                    {
                        textBox1.AppendText("D-estimate #" + (i+1) + " information: " + Environment.NewLine);
                        textBox1.AppendText("|ttrs|" + "\t" + "|ds|" + "\t" + "|stdevs|" + Environment.NewLine);

                        //This for is doing the following:
                        //The three ArrayList hold the ttrs, ds, and standard deviations not divided by each D
                        //here in this for, you'll output them in the appropriate order corresponding to each D:
                        //Ranges: [0-15], [16-31], [32-47]
                        /* j = 0; j < 16  [0 - 15]
                        j = 16; j < 32    [16 - 31]
                        j = 32; j < 48    [32 - 47]
                        */
                        for (j = initial; j < num; j++)
                        {
                            textBox1.AppendText(ttrs[j] + "\t" + ds[j] + "\t" + stdevs[j]  + Environment.NewLine);
                        }

                        initial = j;
                        num = num + 16;

                        textBox1.AppendText("D-estimate #" + (i+1) + ": " + bigD[i] + Environment.NewLine);
                        textBox1.AppendText(Environment.NewLine);

                    }

               
                    textBox1.AppendText("*VOCD-D*: " + finalVocdD);
                    textBox1.AppendText(Environment.NewLine);
                    textBox1.AppendText("--------------------------------------------");

                }

                //Show number of words and number of unique words in student responses:
                textBox2.AppendText("----------------------------------------");
                textBox2.AppendText(Environment.NewLine);
                textBox2.AppendText("---Count of words in responses: " + tokens);
                textBox2.AppendText(Environment.NewLine);
                textBox2.AppendText("---Count of *distinct* words in responses: " + types);

                //Show stemmed words for inspection
                string[] stemmedWords = myCalc.GetAllDistinctStemmedWords();
                textBox3.AppendText("---Distinct stemmed words: ");
                textBox3.AppendText(Environment.NewLine);
                int temp;
                string tempStr;

                //Show the list of distinct stemmed words and frequencies for each one
                //in decreasing order (max to min frequencies and their words)
                for (int i = 0; i < countsforEachDistinct.Length; i++)
                {
                    // decreasing order
                    // traverse i+1 to array length 
                    //Source: https://www.geeksforgeeks.org/different-ways-to-sort-an-array-in-descending-order-in-c-sharp/
                    for (int j = i + 1; j < countsforEachDistinct.Length; j++)

                        // compare array element with  
                        // all next element 
                        if (countsforEachDistinct[i] < countsforEachDistinct[j])
                        {
                            temp = countsforEachDistinct[i];
                            tempStr = stemmedWords[i];

                            countsforEachDistinct[i] = countsforEachDistinct[j];
                            stemmedWords[i] = stemmedWords[j];

                            countsforEachDistinct[j] = temp;
                            stemmedWords[j] = tempStr;
                        }

                    textBox3.AppendText(stemmedWords[i] + " - " + countsforEachDistinct[i] + Environment.NewLine);
                   
                }

                /*
                if (check.Checked)   //user wants to calculate vocd-D for each response -- obsolete: this checkbox is invisible
                                     //If you need this: uncomment and make visible checkbox "check"
                {
                    myCalc.CalculateTokensAndTypesOfEachResp(myTable, startSR.Text, endSR.Text, "responses");

                    MessageBox.Show("Please, wait for the vocd-D calculations for each response to finalize. Depending on the number of responses, it could take several minutes. Values will be shown in the first textbox. Click on OK to continue.");

                    myCalc.CalculateVocdDOfEachResp();

                    double[] vocdDRESP = myCalc.GetVocdDOfEachResp();

                    textBox1.AppendText(Environment.NewLine);
                    textBox1.AppendText(Environment.NewLine);
                    textBox1.AppendText("--Vocd-D of each response (Note: NaN means that the index cannot be calculated because tokens are less than 50): ");
                    textBox1.AppendText(Environment.NewLine);

                    for (int i = 0; i < vocdDRESP.Length; i++)
                    {
                        textBox1.AppendText(vocdDRESP[i].ToString());
                        textBox1.AppendText(Environment.NewLine);
                    }
                }
                */

                //~~~~~~~~~~~~~~Uuber and vocd-D indices of each response   6/4/2020~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                //If you need this: uncomment and make visible checkbox "check"
                // myCalc.CalculateTokensAndTypesOfEachResp(myTable, startSR.Text, endSR.Text, "responses");
                // myCalc.CalculateVocdDOfEachResp();
                // double[] vocdDRESP = myCalc.GetVocdDOfEachResp();

                /*
               myCalc.CalculateUberIndexOfEachResp();
               double[] uberIndices = myCalc.GetUberIndexOfEachResp();

               //Output Uber and vocd-D Indices on the first textbox:
               textBox1.AppendText(Environment.NewLine);
               textBox1.AppendText(Environment.NewLine);
               textBox1.AppendText("--Uber index of each response: ");
               textBox1.AppendText(Environment.NewLine);

               for (int i = 0; i < uberIndices.Length; i++)
               {
                   textBox1.AppendText(uberIndices[i].ToString());
                   textBox1.AppendText(Environment.NewLine);
               }
/*/
                /*
                                textBox1.AppendText(Environment.NewLine);
                                textBox1.AppendText(Environment.NewLine);
                                textBox1.AppendText("--Vocd-D of each response: ");
                                textBox1.AppendText(Environment.NewLine);

                                for (int i = 0; i < vocdDRESP.Length; i++)
                                {
                                    textBox1.AppendText(vocdDRESP[i].ToString());
                                    textBox1.AppendText(Environment.NewLine);
                                }
                                */


                //================================================

            } // end of 'IF (startSR.Text != "" && endSR.Text != "" && startSC.Text != "" && endSC.Text != "") '

            textBox1.SelectionStart = 0;
            textBox2.SelectionStart = 0;
            textBox3.SelectionStart = 0;
            textBox1.ScrollToCaret();
            textBox2.ScrollToCaret();
            textBox3.ScrollToCaret();
        }

    }
}
