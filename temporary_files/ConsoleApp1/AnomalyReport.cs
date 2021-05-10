using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    public class AnomalyReport
    {
        public string description { get; }
        public long timestep { get; }
        public AnomalyReport(string description, long timestep)
        {
            this.description = description;
            this.timestep = timestep;
            Console.WriteLine(description + " " + timestep);
        }

        public AnomalyReport()
        {
            this.description = "";
            this.timestep = 0;
        }

    }
}
