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
        const string inputDocAddress = @"C:\Users\derek\Documents\Visual Studio 2015\Projects\ThesisV1\ExpDecay.txt";
        const string outputDocAddress = @"C:\Users\derek\Documents\Visual Studio 2015\Projects\ThesisV1\Output.txt";
        const char delimiter = '\t';
        static void Main(string[] args)
		{
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            var output = analyzeSingleExport();
            System.IO.File.WriteAllLines(outputDocAddress, output);
            Console.WriteLine("ticks: " + timer.ElapsedTicks);
            timer.Stop();
            Console.ReadLine();
        }

        static List<string> analyzeSingleExport()
        {
            /*Import Data*/
            string input;
            

            var lineCount = File.ReadLines(inputDocAddress).Count(); //count number of lines in input
            DataRow[] AllRows = new DataRow[lineCount - 1]; //initialize array of dataRows that contains all data excep the header row

            try
            {
                using (StreamReader sr = new StreamReader(inputDocAddress))
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
            }

            /*Analyze Data*/
            return LucasCellDetection(AllRows);
            
        }

        static List<string> BiPoDetection(DataRow[] AllRows)
        {
            var peaks = new List<string>();

            const float lowerCutoff = 10;   // mV, lower bound on noise consideration
            const float midCutoff = 20;     // mV, minimum for classification as a low peak, upper bound on noise
            const float highCutoff = 50;    // mV, minimum for classification as a high peak
            bool consistent = false;    // indicates whether the last point was larger that its previous one- only set when a point above lowest
            bool threeConsistent = false;   // indicates whether next fall shouldn't indicate a peak because zone was stayed in too long
            bool falling = false;   //indicates whether a peak has been recorded already
            short zone = 1; // voltage location of last point: zone 1 is below lower, zone 2 is between lower and mid, etc
            short pointsAboveLow = 0;   // count of the number of points in a row that have been above the low range without a point in low  
            long counter = 0;   // time since counter was updated
            string lastTime = "";

            foreach (DataRow row in AllRows)
            {
                if (!(row.Stamp == lastTime)) // add new time stamp and reset counter if new data input
                {
                    peaks.Add(row.Stamp);
                    counter = 1;
                }
                else
                {
                    counter++;
                }
                foreach (float point in row.Data)
                {
                    if (point < lowerCutoff)    // in zone 1
                    {
                        zone = 1;
                        consistent = false;
                        threeConsistent = false;
                        falling = false;
                    }
                    else if (point < midCutoff) // in zone 2 (noise)
                    {
                        if (zone > 2)   //falling 
                        {
                            if (!threeConsistent)
                            { 
                                peaks.Add((counter - 1).ToString() + "\tlow");
                            }
                            falling = true;
                            consistent = false;
                            threeConsistent = false;
                        }
                        else if (zone == 2)
                        {
                            if (consistent) // 3 points in the same row at a time- error
                            {
                                threeConsistent = true;
                            }
                            consistent = true;
                        }
                        else // rising peak
                        {
                            consistent = false;
                            threeConsistent = false;
                            // do nothing, will record peak later
                        }
                        zone = 2; 
                    }
                    else if (point < highCutoff)    // in zone 3 (low peak)
                    {
                        if (zone > 3)   //falling 
                        {
                            if (!threeConsistent)
                            {
                                peaks.Add((counter - 1).ToString() + "\thigh");
                            }
                            falling = true;
                            consistent = false;
                            threeConsistent = false;
                        }
                        else if (zone == 3)
                        {
                            if (consistent) // 3 points in the same row at a time- error
                            {
                                threeConsistent = true;
                            }
                            consistent = true;
                        }
                        else // rising peak
                        {
                            consistent = false;
                            threeConsistent = false;
                            // do nothing, will record peak later
                        }
                        zone = 3;
                    }
                    else // in zone 4 (high peak)
                    {
                        if (zone == 4)  // consistent peak
                        {
                            if (consistent) // 3 points in the same row at a time- error
                            {
                                threeConsistent = true;
                            }
                            consistent = true;
                        }
                        else // rising peak
                        {
                            consistent = false;
                            threeConsistent = false;
                            // do nothing, will record peak later
                        }
                        zone = 4;
                    }
                    counter++;
                }                
            }
            return peaks;
        }

        static List<string> LucasCellDetection(DataRow[] AllRows)
        {
            /* simplest Lucas Cell detection is simply marking when a point is above a threshold */
            List<string> incidences = new List<string>();

            /*find median value*/
            float median = 0;   // assumes signal in zeroed, can be set more analytically if necessary

            /* find every peak that goes from below the lower cutoff to above the upper cutoff */
            const float spread = 50;  // minimum expected height of peaks given sampling rate and uncertainty
            const short streakLengthToAlert = 10;   // number of points in a row above the low range that will trigger an alert
            float upperCutoff = median + spread;
            float lowerCutoff = (float)(0.75 * median + 0.25 * upperCutoff);
            float prev = AllRows[0].Data[0];    // used for analysis, records the previous value
            Boolean exitLow = false;    // set to true when the signal rises from below the lowerCutoff to above it on the last point
            short pointsAboveLow = 0;   // count of the number of points in a row that have been above the low range without a point in low  
            
            foreach (DataRow row in AllRows)
            {
                foreach(float point in row.Data)
                {
                    /* allow for two data points to hit a rising peak */
                    if (point <= lowerCutoff)
                    {
                        pointsAboveLow = 0;
                    }
                    else if (point > lowerCutoff)  // point >= lowerCutoff
                    {
                        if (point > upperCutoff)
                        {
                            if(prev < lowerCutoff || exitLow)
                            {
                                incidences.Add(row.Stamp);  // record the time of a detected Radon Incidence if rising fast enough
                            }
                            else
                            {
                                pointsAboveLow++;   // not a new peak, record how many points in a row this is
                            }
                        }
                        else if (prev < lowerCutoff)
                        {
                            exitLow = true;
                        }
                        else // point < upperCutoff && prev > lowerCutoff
                        {
                            pointsAboveLow++;   // not a new peak, record how many points in a row this is
                        }
                    }
                    else if (exitLow)
                    {
                        if (point > upperCutoff)
                        {
                            incidences.Add(row.Stamp);
                        }
                        exitLow = false;
                    }
                    prev = point;   // record point for comparisson to next point if needed
                }
                if (pointsAboveLow >= streakLengthToAlert)
                {
                    incidences.Add("a" + row.Stamp);   // adding "a" and then the time stamp notes that the current row has a possible changed baseline
                }
            }
            /* debugging prints */
            foreach(string s in incidences)
            {
                Console.Write("incident at " + s + '\n');
            }
            return incidences;
            
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
