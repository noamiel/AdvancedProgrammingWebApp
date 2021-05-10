using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            timeseries ts = new timeseries("C://Users//omerz//Desktop//אוניברסיטה//web app//file1.csv");
            timeseries anomaly = new timeseries("C://Users//omerz//Desktop//אוניברסיטה//web app//anomaly_flight.csv");
            Console.WriteLine(ts.getRowSize());

            
            HybridAnomalyDetector.learnNormal(ts);
            HybridAnomalyDetector.detect(anomaly);
            int a = 1;
        }
    }
}
