using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp1
{
    class RegretionAnomalyDetector : AnomalyDetector
    {
		private static int total = 0;
		public static void learnNormal(timeseries ts)
		{
			List<string> atts = ts.gettAttributes();
			int len = ts.getRowSize();
			int attsSize = atts.Count;
			double[,] vals = new double[attsSize, len];
			for(int i=0 ; i < attsSize ; i++){
				List<double> x = ts.getAttributeData(atts[i]);
				for(int  j = 0 ; j < len ; j++){
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
					double p = Math.Abs(Anomaly_detection_util.pearson(getRow(vals, i), getRow(vals, j), len));
					if (p > max)
					{
						max = p;
						jmax = j;
					}
				}
				string f2 = atts[jmax];
				Point[] ps = toPoints(ts.getAttributeData(f1), ts.getAttributeData(f2));
				
				learnHelper(ts, max, f1, f2, ps);
			}
		}

		public static void learnHelper(timeseries ts,double p/*pearson*/,string f1, string f2, Point[] ps){
			if(p > threshold){
				//Console.WriteLine(f1 + "   " + f2);
				int len = ts.getRowSize();
				Line reg = Anomaly_detection_util.linear_reg(ps, len);
				CorrelatedFeatures c = new CorrelatedFeatures(f1, f2, p, reg,
					RegretionAnomalyDetector.findThreshold(ps, len, reg) * 1.1, 0, 0, false);
				cf.Add(c);
				//Console.WriteLine(c.feature1 + ", " + c.feature2 + " , " + c.threshold);
			}
		}

		public static List<AnomalyReport> detect(timeseries ts){
			List<AnomalyReport> v = new List<AnomalyReport>();
			foreach(CorrelatedFeatures c in cf)
            {
				List<double> x = ts.getAttributeData(c.feature1);
				List<double> y = ts.getAttributeData(c.feature2);
				for (int i = 0; i < Math.Min(x.Count, y.Count); i++)
				{
					if (isAnomalous(x.ElementAt(i), y.ElementAt(i), c))
					{
						string d = c.feature1 + "!" + c.feature2;
						v.Add(new AnomalyReport(d, (i + 1)));
						Console.WriteLine(total++);
					}
				}
			}
			return v;
		}
		public static bool isAnomalous(double x, double y, CorrelatedFeatures c)
		{
			return (Math.Abs(y - c.lin_reg.f(x)) > c.threshold);
		}

	}
}


