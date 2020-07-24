using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.OleDb;
using System.Data;
using System.Windows.Forms;


//Purpose: to read contents of a dataset in CSV or Excel formats
//A DataSet object holds the main DataTable with student responses and scores
//Programmer: Marisol Mercado Santiago (marisolvsh@gmail.com)
//Automated Analysis of Constructed Response Research Group
class ReadDataset
    {
        private string fileName, extension;
        private DataSet set;
        private DataTable dt; //Data Table that holds the original data as input by the user
        private DataTable concatResp;  //Data Table that holds a column with concatenated responses
        private DataTable concatCodes; //Data Table that holds a column with concatenated binary codes across columns
     
        private string concatCodesString;
        private string[] concatCodesArray;

        private List<string> distinctLevels; // Will hold a list with distinct level names from the concatenated codes string
        private List<double> countforEachLevel; // count of responses in each level

    public ReadDataset(string fn, string ext)
    {

        fileName = fn; 
        extension = ext;

        //Connection String
        //Modified from https://stackoverflow.com/questions/15828/reading-excel-files-from-c-sharp
        //https://stackoverflow.com/questions/6813607/parsing-csv-using-oledb-using-c-sharp

        
        if (ext == ".csv")
        {
           
            //Install 64-bit Microsoft Access Database Engine 2016 Redistributable
            //Uncheck "Prefer 32-bit" in Visual Studio Project --> "Balance Calculator" Properties options
            string connString = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source =" + Path.GetDirectoryName(fileName) + ";Extended Properties='text;HDR=YES;IMEX=1;CharacterSet=65001;';";

            using (OleDbConnection conn = new OleDbConnection(connString))
            {
                
                conn.Open();
                var query = "SELECT * FROM [" + Path.GetFileName(fileName) + "]";
                using (var adapter = new OleDbDataAdapter(query, conn))
                {
                    set = new DataSet();
                    try
                    {
                        adapter.Fill(set);
                    }
                    catch (System.Data.OleDb.OleDbException ex)
                    {
                        throw new ApplicationException(string.Format("Cannot open file {0}. Make sure it is not open in other application.", fileName), ex);

                    }
                    conn.Close();
                }
            }

        }
        else   //xlsx - reads only the first sheet //https://stackoverflow.com/questions/1438083/getting-the-first-sheet-from-an-excel-document-regardless-of-sheet-name-with-ole
        {
            
            string connString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0; Data Source={0};Extended Properties='Excel 8.0;HDR=YES;IMEX=1;CharacterSet=65001;';", fileName);

            DataSet data = new DataSet();

                using (OleDbConnection con = new OleDbConnection(connString))
                {
                    con.Open();
                    DataTable dtSchema = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                    string sheet1 = dtSchema.Rows[0].Field<string>("TABLE_NAME");
                    string query = string.Format("SELECT * FROM [{0}]", sheet1);
                    OleDbDataAdapter adapter = new OleDbDataAdapter(query, con);
                    set = new DataSet();
                    try
                    {
                        adapter.Fill(set);
                    }
                    catch (System.Data.OleDb.OleDbException ex)
                    {
                        throw new ApplicationException(string.Format("Cannot open file {0}. Make sure it is not open in other application.", fileName), ex);

                    }

                    con.Close();
                }
        }
    }

    public List<string> GetHeaders()
        {
            dt = set.Tables[0];

            List<string> headers = new List<string>();

            //https://stackoverflow.com/questions/6791894/in-vb-net-how-to-get-the-column-names-from-a-datatable

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                headers.Add(dt.Columns[i].ColumnName.ToString());
            }

            return headers;
        }

        public void ConcatenateResponse(string start, string end)
        {
            concatResp = new DataTable();

            int startIndex = dt.Columns.IndexOf(start);
            int endIndex = dt.Columns.IndexOf(end);

            string tempString = "";

            concatResp.Columns.Add("Concatenated");

            foreach (DataRow row in dt.Rows)
            {
                //concatenate across columns. Leave a space.
                for (int x = startIndex; x < endIndex + 1; x++)
                {
                    tempString += row[x].ToString() + " ";
                }
     

                tempString.TrimEnd();

                if (tempString.Trim() != "")   //Do not add empty lines in the text
                {
                    DataRow drow = concatResp.NewRow();
                    drow["Concatenated"] = tempString;
                    concatResp.Rows.Add(drow);
                }

                tempString = "";
            }

        }

        //Concatenate all 0s and 1s across columns
        //Will add a space between codes. GetDistinctLevels() need it in that formatting to find the unique levels.
        public void ConcatenateCodes(string start, string end)
        {
            concatCodes = new DataTable();

            int startIndex = dt.Columns.IndexOf(start);
            int endIndex = dt.Columns.IndexOf(end);

            string tempString = "";

            concatCodes.Columns.Add("Concatenated");


            foreach (DataRow row in dt.Rows)
            {
                //concatenate across binary code columns
                for (int x = startIndex; x < endIndex + 1; x++)
                {
                        if (row[x].ToString().Trim() != "")  //do not add empty codes
                        {

                            tempString += row[x].ToString() + " ";
                        }
                }

            }

            tempString = tempString.TrimEnd();
            
            concatCodesString = tempString;  //This will be used by GetDistinctLevels()

            //yields only one column, string of 0s and 1s or 2s
            DataRow drow = concatCodes.NewRow();
            drow["Concatenated"] = tempString;
            concatCodes.Rows.Add(drow);
            
        }

        // Methods call order from Form: 
        //(1) ConcatenateResponse(), (3) ConcatenateCodes(), 
        // (3) GetNumberOfResponses(), (4) GetDistinctLevels(),
        // (5) GetNumberOfLevels(), (6) GetCountForEachLevel()
        //Values will be saved in Form, then Form will create a BalanceOfCodes object (see BalanceOfCodes.cs).
        public DataTable GetDataTable()
        {
            dt = set.Tables[0];
            return dt;
        }

        public DataTable GetConcatRespDT()
        {
            return concatResp;
        }

        public DataTable GetConcatCodesDT()
        {
            return concatCodes;
        }

     
        public int GetNumberOfResponses()
        {
            //This is n: Use table concatResp
            return concatResp.Rows.Count;
        }


        //This works ONE bin at a time. Call ONE bin calculation at a time to get the distinct levels found in each bin.
        public List<string> GetDistinctLevels()   
        {
            distinctLevels = new List<string>();
            concatCodesArray = concatCodesString.Split(' ');

            distinctLevels = concatCodesArray.Distinct().ToList();
            distinctLevels.Sort();

            return distinctLevels;

        }

        //Must call first ConcatenateCodes() and GetDistinctLevels() first
        public int GetNumberOfLevels()
        {
            
            return distinctLevels.Count;
        }

        public List<double> GetCountforEachLevel(List<string> levels)
        {

            int count;
            countforEachLevel = new List<double>();

            for (int i=0; i < levels.Count(); i++)   //for each ordered level (e.g., 0 or 1;  or  2 3 5 for example)
            {
                count = 0;

                for (int x = 0; x < concatCodesArray.Length; x++)
                {
                    if( concatCodesArray[x] == levels[i])
                    {
                        count = count + 1;
                    }
                }
                countforEachLevel.Add(count);


            }


            return countforEachLevel;

        }


        //--------We're not using functions below anymore, but may be used in the future. Do not delete.----------
        public byte[] GetConcatRespDTBytes()
        {
            StringBuilder sb = new StringBuilder();
            byte[] dataBytes = null;

            foreach (DataRow row in concatResp.Rows)
            {
                //Use AppendLine in this case
                sb.AppendLine(row[0].ToString());  //There should always be only one column in this datatable
            }

            dataBytes = Encoding.UTF8.GetBytes(sb.ToString()); //Always UTF8

            return dataBytes;
        }

        public byte[] GetConcatCodesDTBytes()
        {
            StringBuilder sb = new StringBuilder();
            //string tempString = "";
            byte[] dataBytes = null;

            foreach (DataRow row in concatCodes.Rows)
            {
                //REM: Do NOT use AppendLine here....
                sb.Append(row[0].ToString());  //There should always be only one column in this datatable
            }

            dataBytes = Encoding.UTF8.GetBytes(sb.ToString()); //Always UTF8

            return dataBytes;
        }





    }

