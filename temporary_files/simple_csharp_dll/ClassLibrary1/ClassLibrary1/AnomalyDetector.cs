using System;
using System.Collections.Generic;
using System.Linq;

namespace ClassLibrary1
{
    internal abstract class AnomalyDetector
    {
        protected static List<CorrelatedFeatures> cf = new List<CorrelatedFeatures>();
        protected static double threshold = 0.9;

        protected static Point[] ToPoints(List<double> x, List<double> y)
        {
            int size = x.Count;
            Point[] ps = new Point[size];
            for (int i = 0; i < size; i++)
            {
                ps[i] = new Point(x[i], y[i]);
            }
            return ps;
        }

        protected static double FindThreshold(Point[] ps, int len, Line rl)
        {
            double max = 0;
            for (int i = 0; i < len; i++)
            {
                double d = Math.Abs(ps[i].Y - rl.F(ps[i].X));
                if (d > max)
                    max = d;
            }
            return max;
        }

        protected static double[] GetRow(double[,] twoD, int row)
        {
            return System.Linq.Enumerable.Range(0, twoD.GetLength(1))
                    .Select(x => twoD[row, x])
                    .ToArray();
        }
    }
}