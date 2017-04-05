using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThesisV1
{
    class DataRow
    {
        public const char Delimiter = '\t';
        public const short NumNonDataCells = 9;
        public int Trigger;
        public string Stamp;   // time, could be converted
        public short Channel;
        public int Index;
        public short Type;
        public float Delay;
        public short Factor;
        public float Rate;
        public short Count;
        public float[] Data;

        public DataRow(string input)
        {
            try
            {
                string[] currentLine = input.Split(Delimiter);
                Trigger = Int32.Parse(currentLine[0]);
                Stamp = currentLine[1];
                Channel = Int16.Parse(currentLine[2]);
                Index = Int32.Parse(currentLine[3]);
                Type = Int16.Parse(currentLine[4]);
                Delay = Single.Parse(currentLine[5]);
                Factor = Int16.Parse(currentLine[6]);
                Rate = Single.Parse(currentLine[7]);
                Count = Int16.Parse(currentLine[8]);
                Data = new float[Count];
                for (int i = 0; i < Count; i++) // assuming count is accurate (it is, so blank cells are ignored)
                {
                    Data[i] = Single.Parse(currentLine[i + NumNonDataCells]);
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
            Console.WriteLine(Trigger + "\t" + Stamp + "\t" + Channel + "\t" + Index + "\t" + Type + "\t" + Delay + "\t" + Factor + "\t" + Rate + "\t" + Count);
            foreach(float dataItem in Data)
            {
                Console.Write(dataItem + "\t");
            }
        }


    }
}
