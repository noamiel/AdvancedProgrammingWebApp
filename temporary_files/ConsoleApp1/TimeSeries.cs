using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace ConsoleApp1
{

	public class timeseries
	{
		Dictionary<string, List<double>> ts = new Dictionary<string, List<double>>();
		//JSON.stringify
		List<string> atts = new List<string>();
		int dataRowSize; // suppose to be size_t type


		public timeseries(string CSVfileName)
		{
			var reader = new StreamReader(File.OpenRead(CSVfileName));
			string head = reader.ReadLine();
			string[] hss = head.Split(',');
			foreach (string att in hss)
			{
				List<double> l = new List<double>();
				ts[att] = l;
				atts.Add(att);
			}
			string line;
			while ((line = reader.ReadLine()) != null)
			{
				string[] lss = line.Split(',');
				int i = 0;
				foreach (string val in lss)
				{
					ts[atts[i]].Add(double.Parse(val));
					i++;
				}
			}
			reader.Close();
			dataRowSize = ts[atts[0]].Count();
		}

		public List<double> getAttributeData(string name)
		{
			return this.ts[name];
		}

		public List<string> gettAttributes()
		{
			return this.atts;
		}

		public int getRowSize()
		{
			return dataRowSize;
		}
	}
}
