using System;
using System.Collections.Generic;
using System.Linq;

namespace ClassLibrary1
{
    internal class HybridAnomalyDetector : AnomalyDetector
    {
        public static void LearnNormal(TimeSeries ts)
        {
            List<string> atts = ts.GettAttributes();
            // Console.WriteLine(atts[0]);
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

                //Console.WriteLine(f1 + "   " + f2);

                LearnHelper(ts, max, f1, f2, ps);
            }
        }

        private static void LearnHelper(TimeSeries ts, double p/*pearson*/, string f1, string f2, Point[] ps)
        {
            RegressionAnomalyDetector.LearnHelper(ts, p, f1, f2, ps);
            if (p > 0.5 && p < threshold)
            {
                //Console.WriteLine(f1 + ", " + f2);
                Circle cl = Circle.FindMinCircle(ps, ts.GetRowSize());
                CorrelatedFeatures c = new CorrelatedFeatures(f1, f2, p, null, cl.Radius * 1.1,
                    cl.Center.X, cl.Center.Y, true);
                cf.Add(c);
            }
        }

        public static List<AnomalyReport> Detect(TimeSeries ts)
        {
            List<AnomalyReport> v = new List<AnomalyReport>();
            foreach (CorrelatedFeatures c in cf)
            {
                List<double> x = ts.GetAttributeData(c.Feature1);
                List<double> y = ts.GetAttributeData(c.Feature2);
                int size = Math.Min(x.Count, y.Count);
                for (int i = 0; i < size; i++)
                {
                    if (IsAnomalous(x.ElementAt(i), y.ElementAt(i), c))
                    {
                        string d = c.Feature1 + "!" + c.Feature2;
                        v.Add(new AnomalyReport(d, (i + 1)));
                        Console.WriteLine(d);
                    }
                }
            }
            return v;
        }

        private static bool IsAnomalous(double x, double y, CorrelatedFeatures c)
        {
            //return (Math.Abs(y - c.lin_reg.f(x)) > c.threshold);
            return (c.Correlation >= threshold && RegressionAnomalyDetector.IsAnomalous(x, y, c)) ||
        (c.Correlation > 0.5 && c.Correlation < threshold && Circle.Dist(new Point(c.Cx, c.Cy), new Point(x, y)) > c.Threshold);
        }
    }
}