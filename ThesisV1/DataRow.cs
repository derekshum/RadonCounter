using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThesisV1
{
    class DataRow
    {
        int trigger;
        string stamp;   // time, could be converted
        short channel;
        int index;
        short type;
        float delay;
        short factor;
        int rate;
        short count;
        float[] data;
        const short numNonDataCells = 9;

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
                factor = Int16.Parse(currentLine[6]);
                rate = Int32.Parse(currentLine[7]);
                count = Int16.Parse(currentLine[8]);
                data = new float[count];
                for (int i = 0; i < count; i++) // assuming count is accurate
                {
                    data[i] = Single.Parse(currentLine[i + numNonDataCells]);
                }
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("data row could incorrectly formatted");
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
