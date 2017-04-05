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
        const string docAddress = @"C:\Users\derek\Documents\Visual Studio 2015\Projects\ThesisV1\ExpDecay.txt";
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
            LucasCellDetection(AllRows);
            
        }

        static void LucasCellDetection(DataRow[] AllRows)
        {
            /* simplest Lucas Cell detection is simply marking when a point is above a threshold */
            List<string> incidences = new List<string>();

            /*find median value*/
            float median = 0;   // assumes signal in zeroed, can be set more analytically if necessary

            /* find every peak that goes from below the lower cutoff to above the upper cutoff */
            float spread = 50;  // minimum expected height of peaks given sampling rate and uncertainty
            float upperCutOff = median + spread;
            float lowerCutOff = (float)(0.75 * median + 0.25 * upperCutOff);
            float prev = AllRows[0].Data[0];    // used for analysis, records the previous value
            Boolean exitLow = false;    // set to true when the signal rises from below the lowerCutOff to above it on the last point
            //Boolean exitMed = false;    // set to true when the signal rises from below the upperCutOff to above it on the last point

            foreach (DataRow row in AllRows)
            {
                foreach(float point in row.Data)
                {
                    /* allow for two data points to hit a rising peak */
                    if (point <= lowerCutOff) {/* do nothing */}   // put first to optimize analysis
                    else if ()  // point >= lowerCutOff
                    {
                        if (point > upperCutOff)
                        {

                        }
                        else if (prev < lowerCutOff)
                        {
                            exitLow = true;
                        }

                        
                    }
                    
                    else if (exitLow)
                    {
                        if (point > upperCutOff)
                        {
                            incidences.Add(row.Stamp);
                        }
                        exitLow = false;
                    }
                    
                    prev = point;
                }
            }
            
        }

        static void DetectLevelChanges(DataRow[] AllRows)
        {
            float dataMin = (float)(3.4 * Math.Pow(10, 34));   // min value in AllRows' data, initialized as the max float value
            float dataMax = (float)(-3.4 * Math.Pow(10, 34));    // max value in AllRows' data, initialized as the min float value
            /* find the minimum and maximum in AllRows' data*/
            foreach (DataRow row in AllRows) 
            {
                dataMin = Math.Min(dataMin, row.Data.Min());
                dataMax = Math.Max(dataMax, row.Data.Max());
            }
            float cutoffPoint = (float)((3.0 * dataMin + dataMax) / 4.0);    // point we want to note the signal falling beneath
            float prev = AllRows[0].Data[0];    // value of the previous data point chronologically
            short[] dipCount = new short[AllRows.Length]; // number of times the current row of data dips below the cutoffPoint
            int i = 0;  // row index for dipCount
            /* note signal going to min every time it drops below 3/4 the way betweent he minimum and the maximum*/
            foreach (DataRow row in AllRows)
            {
                dipCount[i] = 0;
                foreach (float point in row.Data)
                {
                    if (prev > cutoffPoint && point < cutoffPoint)
                    {
                        dipCount[i]++;
                    }
                    prev = point;
                }
                Console.WriteLine(dipCount[i]);
                i++;
            }   
        }

        static void Test(DataRow[] AllRows)  //debugging method
        {
            AllRows[50].print();
        }
	}
}
