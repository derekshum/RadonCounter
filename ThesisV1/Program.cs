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
			try
			{
				string input;
				using (StreamReader sr = new StreamReader(@"C:\Users\derek\Documents\Visual Studio 2015\Projects\ThesisV1\TestFile.txt"))
				{
					string[] headers = sr.ReadLine().Split('\t');
                    string[] currentLine;

                    /* Test of DataRow */
                    DataRow test = new DataRow(sr.ReadLine());
                    test.print();
					/* while ((input = sr.ReadLine()) != null)
					{
						currentLine = input.Split('\t');

					}*/
				}
				Console.ReadLine();
			}
			catch (Exception e)
			{
				Console.WriteLine("The file could not be read:");
				Console.WriteLine(e.Message);
			}
		}
	}
}
