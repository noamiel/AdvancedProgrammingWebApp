using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            timeseries ts = new timeseries("C://Users//buein//OneDrive - Bar-Ilan University//שנה ב סמסטר ב//תכנות מתקדם 2//אבן דרך 2//reg_flight.csv");
            timeseries anomaly = new timeseries("C://Users//buein//OneDrive - Bar-Ilan University//שנה ב סמסטר ב//תכנות מתקדם 2//אבן דרך 2//anomaly_flight.csv");
            Console.WriteLine(ts.getRowSize());

            RegretionAnomalyDetector.learnNormal(ts);
            RegretionAnomalyDetector.detect(anomaly);
            // int a = 1;
        }
    }
}