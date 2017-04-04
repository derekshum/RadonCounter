using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThesisV1
{
	class Program
	{
        const string docAddress = @"C:\Users\derek\Documents\Visual Studio 2015\Projects\ThesisV1\TestFile.txt";
        const char delimiter = '\t';
        static void Main(string[] args)
		{
            analyzeSingleExport();
            Console.ReadLine();
        }

        static void analyzeSingleExport()
        {
            /*Import Data*/
            string input;

            var lineCount = File.ReadLines(docAddress).Count(); //count number of lines in input
            DataRow[] AllRows = new DataRow[lineCount - 1]; //initialize array of dataRows that contains all data excep the header row

            try
            {
                using (StreamReader sr = new StreamReader(docAddress))
                {
                    string[] headers = sr.ReadLine().Split(delimiter);   
                    int i = 0;

                    while ((input = sr.ReadLine()) != null)
					{
                        AllRows[i] = new DataRow(input);
                        i++;
					}
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
                return;
            }
            /*Analyze Data*/
            AllRows[50].print();
            
        }

        static void DetectLevelChanges(DataRow[] AllRows)
        {

        }
	}
}
