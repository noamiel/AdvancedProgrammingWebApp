using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace ClassLibrary1
{
    public class TimeSeries
    {
        private readonly Dictionary<string, List<double>> ts = new Dictionary<string, List<double>>();

        //JSON.stringify
        private readonly List<string> atts = new List<string>();

        private readonly int dataRowSize; // suppose to be size_t type

        public TimeSeries(string CSVfileName)
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

        public List<double> GetAttributeData(string name)
        {
            return this.ts[name];
        }

        public List<string> GettAttributes()
        {
            return this.atts;
        }

        public int GetRowSize()
        {
            return dataRowSize;
        }
    }
}