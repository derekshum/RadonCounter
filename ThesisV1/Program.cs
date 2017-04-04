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
		static void Main(string[] args)
		{
            analyzeSingleExport();
            Console.ReadLine();
        }

        static void analyzeSingleExport()
        {
            /*Import Data*/
            string docAddress = @"C:\Users\derek\Documents\Visual Studio 2015\Projects\ThesisV1\TestFile.txt";
            string input;

            var lineCount = File.ReadLines(docAddress).Count(); //count number of lines in input
            DataRow[] AllRows = new DataRow[lineCount - 1]; //initialize array of dataRows that contains all data excep the header row

            try
            {
                using (StreamReader sr = new StreamReader(docAddress))
                {
                    string[] headers = sr.ReadLine().Split('\t');
                    int i = 0;

                    /*fixing*/
                    input = sr.ReadLine();
                    AllRows[i] = new DataRow(input);

                    /*while ((input = sr.ReadLine()) != null)
					{
                        AllRows[i] = new DataRow(input);
                        i++;
					}*/
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
                return;
            }
            /*Analyze Data*/
            /*Try analyzing first row first, finding rise and drop times*/
            Console.WriteLine(AllRows[0].data.Length);
            Console.WriteLine(AllRows[0].data.Min());
            Console.WriteLine(AllRows[0].data.Max());
        }
	}
}
