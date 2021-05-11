using System;

namespace ClassLibrary1
{
    public class AnomalyReport
    {
        public string Description { get; }
        public long Timestep { get; }

        public AnomalyReport(string description, long timestep)
        {
            Description = description;
            Timestep = timestep;
            Console.WriteLine(description + " " + timestep);
        }

        public AnomalyReport()
        {
            Description = "";
            Timestep = 0;
        }
    }
}