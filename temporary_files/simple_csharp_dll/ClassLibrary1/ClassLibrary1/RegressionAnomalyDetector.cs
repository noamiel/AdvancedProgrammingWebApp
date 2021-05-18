using System;
using System.Collections.Generic;
using System.Linq;

namespace ClassLibrary1
{
    internal class RegressionAnomalyDetector : AnomalyDetector
    {
        // private static int total = 0;

        public static void LearnNormal(TimeSeries ts)
        {
            List<string> atts = ts.GettAttributes();
            int len = ts.GetRowSize();
            int attsSize = atts.Count;
            double[,] vals = new double[attsSize, len];
            for (int i = 0; i < attsSize; i++)
            {
                List<double> x = ts.GetAttributeData(atts[i]);
                for (int j = 0; j < len; j++)
                {
                    vals[i, j] = x[j];
                }
            }

            for (int i = 0; i < attsSize; i++)
            {
                string f1 = atts[i];
                double max = 0;
                int jmax = 0;
                for (int j = i + 1; j < attsSize; j++)
                {
                    double p = Math.Abs(AnomalyDetectionUtil.Pearson(GetRow(vals, i), GetRow(vals, j), len));
                    if (p > max)
                    {
                        max = p;
                        jmax = j;
                    }
                }
                string f2 = atts[jmax];
                Point[] ps = ToPoints(ts.GetAttributeData(f1), ts.GetAttributeData(f2));

                LearnHelper(ts, max, f1, f2, ps);
            }
            Console.WriteLine(cf.Count());
        }

        public static void LearnHelper(TimeSeries ts, double p /*pearson*/, string f1, string f2, Point[] ps)
        {
            if (p > threshold)
            {
                // Console.WriteLine(f1 + "   " + f2);
                int len = ts.GetRowSize();
                Line reg = AnomalyDetectionUtil.LinearReg(ps, len);
                CorrelatedFeatures c = new CorrelatedFeatures(f1, f2, p, reg,
                    RegressionAnomalyDetector.FindThreshold(ps, len, reg) * 1.1, 0, 0, false);
                cf.Add(c);
                Console.WriteLine(c.Feature1 + ", " + c.Feature2 + " , " + c.Threshold);
            }
        }

        public static List<AnomalyReport> Detect(TimeSeries ts)
        {
            List<AnomalyReport> v = new List<AnomalyReport>();
            foreach (CorrelatedFeatures c in cf)
            {
                List<double> x = ts.GetAttributeData(c.Feature1);
                List<double> y = ts.GetAttributeData(c.Feature2);
                for (int i = 0; i < Math.Min(x.Count, y.Count); i++)
                {
                    
                    if (IsAnomalous(x.ElementAt(i), y.ElementAt(i), c))
                    {
                        Console.WriteLine($" - {i}");
                        string d = c.Feature1 + "!" + c.Feature2;
                        v.Add(new AnomalyReport(d, (i + 1)));
                        // total++;
                        // Console.WriteLine(total++);
                    }
                }
            }
            return v;
        }

        public static bool IsAnomalous(double x, double y, CorrelatedFeatures c)
        {
            double diff = Math.Abs(y - c.LinReg.F(x));
            if (c.Feature1 == "throttle" && c.Feature2 == "engine_rpm" && diff > 1000)
                Console.Write($"diff = {diff} < {c.Threshold}");
            return (diff > c.Threshold);
        }
    }
}