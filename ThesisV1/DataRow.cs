using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThesisV1
{
    class DataRow
    {
        public int trigger;
        public string stamp;   // time, could be converted
        public short channel;
        public int index;
        public short type;
        public float delay;
        public short factor;
        public float rate;
        public short count;
        public float[] data;
        public const short numNonDataCells = 9;

        public DataRow(string input)
        {
            try
            {
                string[] currentLine = input.Split('\t');
                trigger = Int32.Parse(currentLine[0]);
                stamp = currentLine[1];
                channel = Int16.Parse(currentLine[2]);
                index = Int32.Parse(currentLine[3]);
                type = Int16.Parse(currentLine[4]);
                delay = Single.Parse(currentLine[5]);
                Console.WriteLine(delay);    //debug
                Console.WriteLine("raw factor: " + currentLine[6]);    //debug
                factor = Int16.Parse(currentLine[6]);
                Console.WriteLine(factor);    //debug
                Console.WriteLine("raw rate: " + currentLine[7]);    //debug
                rate = Single.Parse(currentLine[7]);
                Console.WriteLine(rate);    //debug
                Console.WriteLine("raw count: " + currentLine[8]);    //debug
                count = Int16.Parse(currentLine[8]);
                Console.WriteLine(count);   //debug
                Console.WriteLine("rawdata1: " + currentLine[9]);    //debug
                Console.WriteLine("rawdata2: " + currentLine[10]);    //debug
                data = new float[count];
                for (int i = 0; i < count; i++) // assuming count is accurate
                {
                    data[i] = Single.Parse(currentLine[i + numNonDataCells]);
                    Console.WriteLine(data[i]); //debug
                }
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("data row could incorrectly formatted");
                Console.WriteLine("new DataRow failed to initialize");
            }
        }

        public void print()
        {
            Console.WriteLine(trigger + "\t" + stamp + "\t" + channel + "\t" + index + "\t" + type + "\t" + delay + "\t" + factor + "\t" + rate + "\t" + count);
            foreach(float dataItem in data)
            {
                Console.Write(dataItem + "\t");
            }
        }


    }
}
