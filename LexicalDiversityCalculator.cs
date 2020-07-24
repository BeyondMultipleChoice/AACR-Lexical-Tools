using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Text.RegularExpressions;
using System.Collections;
using System.Windows.Forms;
using System.Drawing;

//Purpose: to calculate a variety of lexical diversity measurement in a string of text that represents the whole dataset of student responses
//Content: Methods to pre-process ("clean") data, count number of words (tokens), Distinct words (types), and calculate the lexical measurements.
//Programmer: Marisol Mercado Santiago (marisolvsh@gmail.com)
//Automated Analysis of Constructed Response Research Group
class LexicalDiversityCalculator
{

    //-------for the WHOLE dataset------
    private string allResponsesInAString;
    private string[] allWords;
    private string[] allStemmedWords;
    private ArrayList allDistinctStemmedWords; //instantiated in CalculateDistinctWords()
    private double numberOfWords; //tokens (N)
    private double numberOfDistinctWords; //types (V) - count of different words -- so if a word appears 20 times, it will only be counted once.
    private double v1; //v1 - count of words which appear only once in the dataset. Also known as hapax legomena.
    private int[] countDistinct;  //This holds the count of EACH type appearing in the dataset. Each index corresponds to the index in allDistinctStemmedWords. So if allDistinctStemmedWords[0] had "cat", then vRCounts[0] will have the number of times "cat" appeared in the whole dataset. It is used by CalculateYuleK();
   
    //------this is to later on calculate Uber index and/or vocd-D for each one-----------
    private string[] allResponses; //each response in its own index/
    private List<string[]> allWordsOfEachResp;  //The List will have on each dimension the array of words of each response.
    private List<string[]> allStemmedWordsOfEachResp;   //The List will have on each dimension the array of stemmed words of each response.
    private double[] uberIndexOfEachResp;
    private double[] vocdDOfEachResp;
    private double[] numberOfWordsOfEachResp; //tokens (N)
    private double[] numberOfDistinctWordsOfEachResp; //types (V) - count of different words -- so if a word appears 20 times, it will only be counted once.
    //-------------------------------------------
    private double ttrResult;
    private double brunetIndexResult;
    private double guiraudRResult;
    private double honoreSResult;
    private double herdanC;
    private double correctedTTR;
    private double somersS;  //Somers' S
    private double yuleKResult;
    private double uberIndex;

    //--vocd-D properties below-------------------:
    private double finalD;
    private double[] D_estimate;
    private ArrayList ds; // will hold the 16 * 3 ds (array with 0-47 values)
    private ArrayList ttrs;  //will hold the 16 * 3 ttrs (array with 0-47 values)
    private ArrayList stdevs; //will hold the 16 * 3 standard deviations (array with 0-47 values)
    //------------------------------------------

    //This constructor calculates BY DEVAULT V, N, and TTR of the WHOLE dataset at the moment of instantiation
    public LexicalDiversityCalculator(DataTable dt, string start, string end, string type)
    {   
         
        AllWordsToAString(dt, start, end, type);   //all words will be in the giant string "allWords"
        AllWordsToArray();
        StemWords();
        DistinctStemmedWords();
        CalculateNumberOfWords();
        CalculateDistinctWords();  
        CalculateTTR();

    }

    //Default "type" is "responses"
    public void AllWordsToAString(DataTable dt, string start, string end, string type="responses")
    {

        int startIndex = dt.Columns.IndexOf(start);
        int endIndex = dt.Columns.IndexOf(end);

        string tempString = "";
        string temp0 = "";

        if (type == "binary")
        {
            foreach (DataRow row in dt.Rows)
            {
                //concatenate across binary code columns
                for (int x = startIndex; x < endIndex + 1; x++)
                {
                    tempString += row[x].ToString();
                }
            }

            allResponsesInAString = tempString;
        }

        else if (type == "responses")
        {
            foreach (DataRow row in dt.Rows)
            {

                for (int x = startIndex; x < endIndex + 1; x++)
                {
                    temp0 = row[x].ToString();
                    temp0 = temp0.Trim();

                    if (temp0 != "")
                    {
                        tempString += temp0 + " ";
                    }
                }
            }

            tempString = tempString.TrimEnd(); //removes the "tail end" space
            allResponsesInAString = CleanText(tempString); //final assignment - assign it to the global variable
        }
    } // end of function

  
    //Splits the big string with the whole dataset. Any word marked within white spaces
    //will be added in the array allWords.
    public void AllWordsToArray()
    {
        allWords = allResponsesInAString.Split(' ');

    }

    //Stems all words which were already placed in allWords[]
    //Adds stemmed words in allStemmedWords[]
    public void StemWords()
    {
        PorterStemmer stemmer = new PorterStemmer();
        allStemmedWords = new string[allWords.Length];

        for (int x = 0; x < allWords.Length; x++)
        {
            allStemmedWords[x] = stemmer.stem(allWords[x]);

        }
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~`
    //Default "type" is "responses"
    public void CalculateTokensAndTypesOfEachResp(DataTable dt, string start, string end, string type = "responses")  //Call this from Form using an object type LexicalDiversityCalculator
    {
        AllWordsToAStringRESP();
        AllWordsToArrayRESP();
        StemWordsRESP();
        CalculateNumberOfWordsRESP();
        CalculateDistinctWordsRESP();
       
        void AllWordsToAStringRESP()
        {

            int startIndex = dt.Columns.IndexOf(start);
            int endIndex = dt.Columns.IndexOf(end);

            string tempString = "";
            string temp0 = "";

            allResponses = new string[dt.Rows.Count]; //initialize array that will hold the responses
            int i = 0; //this will be used to navigate through allResponses[]


            if (type == "binary")
            {
                foreach (DataRow row in dt.Rows)
                {
                    //concatenate across binary code columns
                    for (int x = startIndex; x < endIndex + 1; x++)
                    {
                        tempString += row[x].ToString();
                    }
                }

                allResponsesInAString = tempString;
            }

            else if (type == "responses")
            {
                foreach (DataRow row in dt.Rows)
                {
                    for (int x = startIndex; x < endIndex + 1; x++)  //concatenates across   <----->
                    {
                        temp0 = row[x].ToString();
                        temp0 = temp0.Trim();

                        if (temp0 != "")
                        {
                            tempString += temp0 + " ";
                        }
                    }

                    tempString = tempString.Trim(); //removes the "tail end" space

                    allResponses[i] = CleanText(tempString); //"Cleans" the text from symbols, etc. Saves the response in index [i]

                    tempString = ""; //clean the string for the next response

                    i++;
                }
            }
        } // end of function

         void AllWordsToArrayRESP()
        {
            allWordsOfEachResp = new List<string[]>();

            for (int i = 0; i < allResponses.Length; i++)
            {
               allWordsOfEachResp.Add(allResponses[i].Split(' '));
            }

        }

        //Stems all words which were already placed in allWordsOfEachResp
        //Adds stemmed words from each response in allStemmedWordsOfEachResp
         void StemWordsRESP()
        {
            PorterStemmer stemmer = new PorterStemmer();
            allStemmedWordsOfEachResp = new List<string[]>();

            for (int i = 0; i < allResponses.Length; i++)
            {
                string[] tempArray = new string[allWordsOfEachResp[i].Length];
                tempArray = allWordsOfEachResp[i];
                string tempString = "";

                for (int j = 0; j < tempArray.Length; j++)
                {
                    tempString = stemmer.stem(tempArray[j]);
                    tempArray[j] = tempString;  //Replace the word with the stemmed word in the same index
                }

                allStemmedWordsOfEachResp.Add(tempArray);  //add the array of stemmed words of response [i] in the List allStemmedWordsOfEachResp

            }

        }

         void CalculateNumberOfWordsRESP()  //Calculate the NUMBER/COUNT of N (tokens) of each response
        {
            int i = 0;
            numberOfWordsOfEachResp = new double[allResponses.Length];
            foreach (string[] row in allStemmedWordsOfEachResp)
            {
                numberOfWordsOfEachResp[i] = row.Length;
                i++;
            }

        }

         void CalculateDistinctWordsRESP()   //Calculate the NUMBER/COUNT of V (types) of each response
        {

            int i = 0;
            numberOfDistinctWordsOfEachResp = new double[allResponses.Length];

            foreach (string[] row in allStemmedWordsOfEachResp)
            {
                var tempCount = row.Distinct();
                numberOfDistinctWordsOfEachResp[i] = tempCount.Count();
                i++;  
            }
        }
    }

    //Splits each response string into an array of words. Any word delimited within white spaces will be added in the LIST allWordsOfEachResp.
    //Note: the "log" that statistical formulas refer to is the natural logarithm.

    public void CalculateUberIndexOfEachResp()  //Calculates Uber Index of EACH RESPONSE
    {
        uberIndexOfEachResp = new double[allResponses.Length];

        for (int i=0; i < allResponses.Length; i++)
        {
            // (logN)^2 / (logN - logV)
            double logN = Math.Log(numberOfWordsOfEachResp[i]);
            double logV = Math.Log(numberOfDistinctWordsOfEachResp[i]);
            uberIndexOfEachResp[i] = Math.Pow(logN, 2) / (logN - logV);
            uberIndexOfEachResp[i] = System.Math.Round(uberIndexOfEachResp[i], 4);
        }
  
    }

    public double[] GetUberIndexOfEachResp()
    {
        return uberIndexOfEachResp;
    }


    //Calculate vocd-D for EACH RESPONSE
    //Reference: McKee, Malvern, & Richards. (2000). Measuring vocabulary diversity using dedicated software.Literary and Linguistic Computing, 15(3), 323–338. https://doi.org/10.1093/llc/15.3.323
    //Note: response must have at least 50 tokens.
    public void CalculateVocdDOfEachResp()
    {

        double[] d = new double[16]; //"little d's" of the 16 segments
        double[] tempSDev = new double[16]; //standard deviation of the 16 segments
        tempTTRArray = new double[100];
        D_estimate = new double[3]; //These are the big D[i]s that will then be averaged to find the final D that the user will see
        vocdDOfEachResp = new double[allResponses.Length];  //Will hold a D for each response

        finalD = 0;
        int x = 0;  //this will be accessed not only by the first "for", but by CalculateAverageTTRforD_RESP to know the particular response index that needs to look

        //initialize the ArrayLists.
        ds = new ArrayList();
        ttrs = new ArrayList();
        stdevs = new ArrayList();

        for (x = 0; x < allResponses.Length; x++) 
        {
            string[] response = allStemmedWordsOfEachResp[x];

            if (response.Length >= 50)
            {

                for (int i = 0; i < 3; i++)  //3 Ds as the final D value that the user will see
                {
                    for (int n = 35; n <= 50; n++) //equivalent to [0-15]
                    {
                        double averageTTR = CalculateAverageTTRforD_RESP(n);  //average of 100 TTRs, n words at random from the whole set, types from that same universe of n words
                        int index = n - 35;
                        d[index] = 0.5 * ((((double)n) * averageTTR * averageTTR) / (1.0 - averageTTR));
                        tempSDev[index] = StdDev(tempTTRArray, true);  //if needed: print out each standard deviation

                        //Add all of the 16 * 3 standard deviations, ttrs, and little ds. To show them to the user.
                        ttrs.Add(System.Math.Round(averageTTR, 4));
                        ds.Add(System.Math.Round(d[index], 4));
                        stdevs.Add(System.Math.Round(tempSDev[index], 4));
                    }

                    List<PointF> points = new List<PointF>();
                    for (int j = 35; j <= 50; j++)
                    {
                        int index = j - 35;
                        points.Add(new PointF(j, (float)d[index]));  //x,y
                    }

                    D_estimate[i] = FindtheYIntercept(points); // Y-intercept = b = mean of expected values = D-estimate
                    finalD += D_estimate[i];  //this will be used to calc the average after the "}"

                }

                finalD = finalD / 3;
                finalD = System.Math.Round(finalD, 4);

                vocdDOfEachResp[x] = finalD;


            }
            else  //response doesn't have at least 50 words
            {
                vocdDOfEachResp[x] = double.NaN;   //save a NaN to signal this
            }

        }

        double CalculateAverageTTRforD_RESP(int n)
        {
            int indexWord;
            //string randomToken;
            string[] randomTokenArray = new string[n];
            double tempTokens;
            double tempTypes;
            double sumTempTTR = 0;
            double tempTTR = 0;
            double averageTTR = 0;  //Average of segments [35-50]. 16.
            Random rnd;

            for (int i = 0; i < 100; i++) //100 times = 100 tempTTRs that will be used to get the averageD of this segment
            {

                //generate random indexes from the length of allStemmedWordsOfEachResp[x]:
                int min = 0;
                string[] response = allStemmedWordsOfEachResp[x];   //get response x string[] with stemmed words
                int max = response.Length;   //get the length of the string[] = number of responses -1

                for (int j = 0; j < n; j++)
                {
                    rnd = new Random(Guid.NewGuid().GetHashCode()); // random number not attached to the computer clock tick
                    indexWord = rnd.Next(min, max);
                    randomTokenArray[j] = response[indexWord];   //look at the word, but in the response[] = array with stemmed words of that particular response x
                }

                //Now that you've the 35 random words, calculate its TTR, save it in an array[100]
                tempTokens = randomTokenArray.Length;
                var tempCount = randomTokenArray.Distinct();
                tempTypes = tempCount.Count();
                tempTTR = tempTypes / tempTokens;
                sumTempTTR += tempTTR;  //summatory of 100 TTRs for averageTTR
                tempTTRArray[i] = tempTTR;  //save the tempTTR [0 - 99] to calc it standard deviation in CalculatevocdD()
                                         
            }
            averageTTR = sumTempTTR / 100;
            return averageTTR;  //use this average for each d[16] in CalculatevocdD()
        }

    }

    public double[] GetVocdDOfEachResp()
    {
        return vocdDOfEachResp;
    }


    //~~~~~~~~~~~~~END OF UBER INDEX AND/OR VOCD-D OF EACH RESPONSE~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

    //Removes special characters, replaces "/" for a space, trims spaces, removes default stopwords an, a, and the
    public string CleanText(string tempString)
    {
        tempString = tempString.ToLower();   //all words to lower case
        tempString = Regex.Replace(tempString, @"/", " ");  //Replaces "/" for a space
        tempString = Regex.Replace(tempString, @"'", ""); //Removes apostrophes ('). Replaces them with nothing. So "can't" becomes "cant"
        tempString = Regex.Replace(tempString, @"\ba\b", "");
        tempString = Regex.Replace(tempString, @"\ban\b", ""); // the \b matches word boundaries https://stackoverflow.com/questions/6143642/way-to-have-string-replace-only-hit-whole-words
        tempString = Regex.Replace(tempString, @"\bthe\b", "");
        tempString = Regex.Replace(tempString, @"[^\w\d\s]", "");  //removes all special characters except white spaces and words
        tempString = Regex.Replace(tempString, @"\s+", " ");  //this replaces more than two white spaces. NOTE: leave this at the end 

        return tempString;
    }

    //Saves in an array all Distinct stemmed words (save only once each word, no matter how many times it repeats in the document)
    //Adds Distinct stemmed words in allDistinctStemmedWords[]
    //Code in the function altered from http://www.java2s.com/Code/CSharp/Data-Types/Findalluniquewordsinanarrayofwords.htm
    public void DistinctStemmedWords()
    {
    //Code altered from http://www.java2s.com/Code/CSharp/Data-Types/Findalluniquewordsinanarrayofwords.htm
    //Original notice below:
    /*
            '
            ' Copyright (c) 2004 Ryan Whitaker
            '
            ' This software is provided 'as-is', without any express or implied warranty. In no 
            ' event will the authors be held liable for any damages arising from the use of this 
            ' software.
            ' 
            ' Permission is granted to anyone to use this software for any purpose, including 
            ' commercial applications, and to alter it and redistribute it freely, subject to the 
            ' following restrictions:
            '
            ' 1. The origin of this software must not be misrepresented; you must not claim that 
            ' you wrote the original software. If you use this software in a product, an 
            ' acknowledgment (see the following) in the product documentation is required.
            '
            ' This product uses software written by the developers of NClassifier
            ' (http://nclassifier.sourceforge.net).  NClassifier is a .NET port of the Nick
            ' Lothian's Java text classification engine, Classifier4J 
            ' (http://classifier4j.sourceforge.net).
            '
            ' 2. Altered source versions must be plainly marked as such, and must not be 
            ' misrepresented as being the original software.
            '
            ' 3. This notice may not be removed or altered from any source distribution.
            '
            '*******************************************************************************
        */
        //************** START of Copyright (c) 2004 Ryan Whitaker source code
        allDistinctStemmedWords = new ArrayList();   //ArrayList name was changed to work with this class.

    for (int i = 0; i < allStemmedWords.Length; i++)
            if (!allDistinctStemmedWords.Contains(allStemmedWords[i]))
                allDistinctStemmedWords.Add(allStemmedWords[i]);

    //************** END of Copyright (c) 2004 Ryan Whitaker source code********************
}

//assigns the length of allStemmedWords as the numberOfWords.
public void CalculateNumberOfWords()
    {
        numberOfWords = allStemmedWords.Length;
    }

    //Counts distinct words (types) and assigns the numeric value to numberOfDistinctWords
    public void CalculateDistinctWords()
    {
        var tempCount = allStemmedWords.Distinct(); //use the array of all stemmed words (tokens). Do not use the array of Distinct stemmed words (types).
        numberOfDistinctWords = tempCount.Count();  //This is the number of types.
    }

    //Called from CalculateHonoreStatistic, saves the result of v1
    //Counts the number of words appearing only once in the dataset
    public void CalculateV1()
    {
        //https://stackoverflow.com/questions/33992512/count-unique-words-in-a-string-in-c-sharp
        IEnumerable<string> wordsAppearingOnce = allStemmedWords.GroupBy(w => w).Where(g => g.Count() == 1).Select(g => g.Key);
        v1 = wordsAppearingOnce.Count();
    }

    //Call these other functions in the following order before calling CalculateTTR(): 
    //AllWordsToAString(dt, start, end, type), AllWordsToArray(), 
    //StemWords(), DistinctStemmedWords(), CalculateNumberOfWords(), CalculateDistinctWords().
    //Refer to this class' constructor
    public void CalculateTTR()
    {
        try
        {
            ttrResult = numberOfDistinctWords / numberOfWords;
            ttrResult = System.Math.Round(ttrResult, 4);

            }
        catch (DivideByZeroException e)
        {
            MessageBox.Show("Division by zero error. No words were found in your file.");
        }
    }


    //Additional calculations calculated separately from the constructor -- or perhaps just call them all in the constructor?
    //Create a TTRCalculator object, call these functions in order: 
    public void CalculateBrunetIndex()
    {
        //Formula:  BI = N ^ [ V ^ -0.165 ] 
        double exp = Math.Pow(numberOfDistinctWords, -0.165);
        brunetIndexResult = Math.Pow(numberOfWords, exp);
        brunetIndexResult = System.Math.Round(brunetIndexResult, 4);
    }

    public void CalculateGuiraudR()
    {
        // Formula:  V / RootSquare(N)
        try
        {
            guiraudRResult = numberOfDistinctWords / Math.Sqrt(numberOfWords);
            guiraudRResult = System.Math.Round(guiraudRResult, 4);
        }
        catch (Exception e)
        {
            MessageBox.Show("Error in calculation. Please check your dataset." + "\n\n" + e.Message);
        }

    }

    public void CalculateHonoreStatistic()
    {
        //N =  tokens = all words
        //V = types = distinct words
        //v1 is the count of words that ONLY appear ONCE in the dataset
        //Formula: (100 * log(N)) / 1 - V1 / V]
        CalculateV1();
        double num = 100 * Math.Log(numberOfWords);
        double denom = 1 - (v1 / numberOfDistinctWords);
        honoreSResult = num / denom;
        honoreSResult = System.Math.Round(honoreSResult, 4);

    }

    //Note: Yule K info:
    //Reference: https://www.google.com/books/edition/Corpus_Linguistics/UceXDXyIIqoC?hl=en&gbpv=1&dq=yule+k&pg=PA1074&printsec=frontcover
    //AND this reference: Choi, W., & Jeong, H. (2016). Finding an appropriate lexical diversity measurement for a small-sized corpus and its application to a comparative study of L2 learners’ writings. Multimedia Tools and Applications, 75(21), 13015–13022. https://doi.org/10.1007/s11042-015-2529-1
    //Formula:  10,000 * (M - N) / N^2   ;  where M = summatory of r^2 * Vr
    public void CalculateYuleK()
    {
        countDistinct = new int[allDistinctStemmedWords.Count];  //Will hold the count for each type

        IEnumerable<string> countvr = allStemmedWords;

        //Count the number of times each type appears in the IEnumberable
        for (int x = 0; x < allDistinctStemmedWords.Count; x++)
        {
            var type = allDistinctStemmedWords[x];   //allDistinctStemmedWords has the types we need to find their r (number of times they appear in the whole dataset)
            countDistinct[x] = countvr.Where(w => w.Equals(type)).Count(); //Count the number of times EACH distinct stemmed word appears in the whole dataset. How many times "type" appears in the whole dataset?
        }

        IEnumerable<int> vrTemp = countDistinct;  //list with number of times each distinct stemmed word appears, in the same order as it is in allDistinctStemmedWords
        int[] r =  countDistinct.Distinct().ToArray();  //Has a list of distinct r's (so distinct frequencies)
        int[] vrFinal = new int[r.Length];

        double num1 = Math.Pow(10, 4); //10^4 = 10,000
        double summatory = 0;   

        //Summatory portion of the numerator
        for (int x = 0; x < r.Count(); x++)  
        {
            vrFinal[x] = vrTemp.Where(w => w.Equals(r.ElementAt(x))).Count(); //How many types have a frequency of x? 
            summatory += Math.Pow( r.ElementAt(x), 2) * vrFinal[x];
        }

        double num2 = summatory - numberOfWords; 

        double finalNum = num1 * num2;   //numerator
        double denom = Math.Pow(numberOfWords, 2);   //denominator

        yuleKResult = finalNum / denom;

        yuleKResult = System.Math.Round(yuleKResult, 4);


    }

    public void CalculateHerdanC()
    {
        // logV / logN
        herdanC = Math.Log(numberOfDistinctWords) / Math.Log(numberOfWords);
        herdanC = System.Math.Round(herdanC, 4);
    }

    public void CalculateCorrectedTTR()
    {
        //  V  / rootsq (2*N)
        correctedTTR = numberOfDistinctWords / (Math.Sqrt(2 * numberOfWords));
        correctedTTR = System.Math.Round(correctedTTR, 4);
    }

    public void CalculateSomersS()
    {
        // (loglogV) / (loglogN)
        somersS = Math.Log(Math.Log(numberOfDistinctWords)) / Math.Log(Math.Log(numberOfWords));
        somersS = System.Math.Round(somersS, 4);
    }

    public void CalculateUberIndex()
    {
        // (logN)^2 / (logN - logV)
        double logN = Math.Log(numberOfWords);
        double logV = Math.Log(numberOfDistinctWords);
        uberIndex = Math.Pow(logN, 2) / (logN - logV);
        uberIndex = System.Math.Round(uberIndex, 4);
    }

    //---------------Methods to calculate vocd-D for the entire dataset according to 
    //McKee, Malvern, & Richards. (2000). Measuring vocabulary diversity using dedicated software.Literary and Linguistic Computing, 15(3), 323–338. https://doi.org/10.1093/llc/15.3.323
    //NOte: transcript must have at least 50 tokens...
    double[] tempTTRArray;
    public void CalculateVocdD()
    {

        double[] d = new double[16]; //"little d's" of the 16 segments
        double[] tempSDev = new double[16]; //standard deviation of the 16 segments
        tempTTRArray = new double[100];
        D_estimate = new double[3]; //These are the big D[i]s that will then be averaged to find the final D that the user will see
        finalD= 0;
        //double sumd = 0;

        //initialize the ArrayLists.
        ds = new ArrayList();
        ttrs = new ArrayList();
        stdevs = new ArrayList();

        for (int i = 0; i < 3; i++)  //McKee, Malvern, & Richards. (2000) implemented the average of 3 Ds as the final D value that the user will see
        {
            for (int n = 35; n <= 50; n++) //equivalent to [0-15]
            {
                double averageTTR = CalculateAverageTTRforD(n);  //average of 100 TTRs, n words at random from the whole set, types from that same universe of n words
                int index = n - 35;
                d[index] = 0.5 * ((((double)n) * averageTTR * averageTTR) / (1.0 - averageTTR));
                tempSDev[index] = StdDev(tempTTRArray, true);  //if needed: print out each standard deviation                                     

                //Add all of the 16 * 3 standard deviations, ttrs, and little ds. To show them to the user.
                ttrs.Add(System.Math.Round(averageTTR, 4));
                ds.Add(System.Math.Round(d[index], 4));
                stdevs.Add(System.Math.Round(tempSDev[index], 4));
            }
          
            List<PointF> points = new List<PointF>();
            for (int j = 35; j <= 50; j++)
            {
                int index = j - 35;
                points.Add(new PointF(j, (float)d[index]));  //x,y
            }

            D_estimate[i] = FindtheYIntercept(points); // Y-intercept = b = mean of expected values = D-estimate
            finalD += D_estimate[i];  //this will be used to calc the average after the "}"
  
        }

        finalD = finalD / 3;
        finalD = System.Math.Round(finalD, 4);

        double CalculateAverageTTRforD(int n)
        {
            int indexWord;
            //string randomToken;
            string[] randomTokenArray = new string[n];
            double tempTokens;
            double tempTypes;
            double sumTempTTR =0;
            double tempTTR = 0;
            double averageTTR = 0;  //Average of segments [35-50]. 16.
            Random rnd;

            for (int i = 0; i < 100; i++) //100 times = 100 tempTTRs that will be used to get the averageD of this segment
            {
               
                //generate random indexes from the length of allStemmedWords:
                int min = 0;
                int max = allStemmedWords.Length;

                for (int j = 0; j < n; j++)
                {
                    rnd = new Random(Guid.NewGuid().GetHashCode()); // random number not attached to the computer clock tick
                    indexWord = rnd.Next(min, max);
                    randomTokenArray[j] = allStemmedWords[indexWord];
                }

                //Now that you've the 35 random words, calculate its TTR, save it in an array[100]
                tempTokens = randomTokenArray.Length;
                var tempCount = randomTokenArray.Distinct();
                tempTypes = tempCount.Count();
                tempTTR = tempTypes / tempTokens;
                sumTempTTR += tempTTR;  //summatory of 100 TTRs for averageTTR
                tempTTRArray[i] = tempTTR;  //save the tempTTR [0 - 99] to calc it standard deviation in CalculatevocdD()
            }
            averageTTR = sumTempTTR / 100;
            return averageTTR;  //use this average for each d[16] in CalculatevocdD()
        }

    }

   

    // Return the standard deviation of an array of Doubles.
    // If the second argument is True, evaluate as a sample.
    // If the second argument is False, evaluate as a population.
    // Modified from http://csharphelper.com/blog/2015/12/make-an-extension-method-that-calculates-standard-deviation-in-c/
    // Called by CalculateVocdD()
    private double StdDev(IEnumerable<double> values, bool as_sample)  //Pass true = as_sample
    {
        // Get the mean.
        double mean = values.Sum() / values.Count();

        // Get the sum of the squares of the differences
        // between the values and the mean.
        var squares_query =
            from double value in values
            select (value - mean) * (value - mean);
        double sum_of_squares = squares_query.Sum();

        if (as_sample)
        {
            return Math.Sqrt(sum_of_squares / (values.Count() - 1));  //Variance is only:  sum_of_squares / (values.Count() - 1)
        }
        else
        {
            return Math.Sqrt(sum_of_squares / values.Count());
        }
    }


    // Find the least squares linear fit.
    // Return the total error.
    // Modified from http://csharphelper.com/blog/2014/10/find-a-linear-least-squares-fit-for-a-set-of-points-in-c/
    // Called by CalculateVocdD()
    private double FindLinearLeastSquaresFit(List<PointF> points)
    {
        // Perform the calculation.
        // Find the values S1, Sx, Sy, Sxx, and Sxy.
        double S1 = points.Count;
        double Sx = 0;
        double Sy = 0;
        double Sxx = 0;
        double Sxy = 0;
        foreach (PointF pt in points)
        {
            Sx += pt.X;
            Sy += pt.Y;
            Sxx += pt.X * pt.X;
            Sxy += pt.X * pt.Y;
        }

        // Solve for m and b.
        double m = (Sxy * S1 - Sx * Sy) / (Sxx * S1 - Sx * Sx);
        double b = (Sxy * Sx - Sy * Sxx) / (Sx * Sx - S1 * Sxx); 
        
        return Math.Sqrt(ErrorSquared(points, m, b));

        // Return the error squared.
        double ErrorSquared(List<PointF> _points, double _m, double _b)
        {
            double total = 0;
            foreach (PointF pt in _points)
            {
                double dy = pt.Y - (_m * pt.X + _b);
                total += dy * dy;
            }
            return total;
        }

    }

    // Find the Y intercept (b) using the line of best fit equation
    // Return b
    // Modified from http://csharphelper.com/blog/2014/10/find-a-linear-least-squares-fit-for-a-set-of-points-in-c/
    // Called by CalculateVocdD()
    private double FindtheYIntercept(List<PointF> points)
    {
        // Perform the calculation.
        // Find the values S1, Sx, Sy, Sxx, and Sxy.
        double S1 = points.Count;
        double Sx = 0;
        double Sy = 0;
        double Sxx = 0;
        double Sxy = 0;
        foreach (PointF pt in points)
        {
            Sx += pt.X;
            Sy += pt.Y;
            Sxx += pt.X * pt.X;
            Sxy += pt.X * pt.Y;
        }

        // Solve for b.
        double b = (Sxy * Sx - Sy * Sxx) / (Sx * Sx - S1 * Sxx);
        b = System.Math.Round(b, 4);
        return b;

    }

    //------------END OF vocd-D METHODS SECTION ---------------------


    //---------------"Get" methods ----------------------

    public double GetTTRResult()
    {
        return ttrResult;
    }

    public double GetBrunetIndex()
    {
        return brunetIndexResult;
    }

    public double GetGuiraudR()
    {
        return guiraudRResult;
    }

    public double GetHonoreStatistic()
    {
        return honoreSResult;
    }

    public double GetHerdanC()
    {
        return herdanC;
    }
    public double GetCorrectedTTR()
    {
        return correctedTTR;
    }
    public double GetSomersS()
    {
        return somersS;
    }

    public double GetYuleK()
    {
    return yuleKResult;
    }

    public double GetUberIndex()
    {
    return uberIndex;
    }

    public double GetFinalVocdD()
    {
        return finalD;
    }

    public double[] GetD_Estimate()
    {
        return D_estimate;
    }

    //called after calculating vocd-D
    public ArrayList Get48ttrs()
    {
        return ttrs;
    }

    //called after calculating vocd-D
    public ArrayList Get48ds()
    {
        return ds;
    }

    //called after calculating vocd-D
    public ArrayList Get48stdevs()
    {
        return stdevs;
    }

    //get number of tokens
    public double GetNumberOfWords()
    {
        return numberOfWords;
    }

    //get number of types
    public double GetNumberOfDistinctWords()
    {
        return numberOfDistinctWords;
    }

    //Get v1 or the number of words which only appear once
    public double GetV1()
    {
        return v1;
    }

    public int[] GetvrCount()
    {
        return countDistinct;  //This is used in CalculateYuleK() but could be useful to display the number of times a stemmed word appears in the whole dataset.
    }


    //returns the array with all stemmed words
    public string[] GetAllDistinctStemmedWords()
    {
        return (string[])allDistinctStemmedWords.ToArray(typeof(string)); //convert the ArrayList to array
    }


}