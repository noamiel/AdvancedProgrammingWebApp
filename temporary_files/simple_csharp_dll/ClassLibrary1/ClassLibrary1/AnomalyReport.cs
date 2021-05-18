using System;

namespace ClassLibrary1
{
    public class AnomalyReport
    {
        public string Description { get; }
        public long TimeStep { get; }

        public AnomalyReport(string description, long timestep)
        {
            Description = description;
            TimeStep = timestep;
            // Console.WriteLine(description + " " + timestep);
        }

        public AnomalyReport()
        {
            Description = "";
            TimeStep = 0;
        }
    }
}