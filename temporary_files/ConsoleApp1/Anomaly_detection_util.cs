using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    class Anomaly_detection_util
    {
		public static double avg(double[] x, int size)
		{
			double sum = 0;
			for (int i = 0; i < size; sum += x[i], i++) ;
			return sum / size;
		}

		public static double var(double[] x, int size)
		{
			double av = avg(x, size);
			double sum = 0;
			for (int i = 0; i < size; i++)
			{
				sum += x[i] * x[i];
			}
			return sum / size - av * av;
		}

		public static double cov(double[] x, double[] y, int size)
		{
			double sum = 0;
			for (int i = 0; i < size; i++)
			{
				sum += x[i] * y[i];
			}
			sum /= size;

			return sum - avg(x, size) * avg(y, size);
		}

		public static double pearson(double[] x, double[] y, int size)
		{
			return cov(x, y, size) / (Math.Sqrt(var(x, size)) * Math.Sqrt(var(y, size)));
		}

		public static Line linear_reg(Point[] points, int size)
		{
			double[] x = new double[size];
			double[] y = new double[size];
			for (int i = 0; i < size; i++)
			{
				x[i] = points[i].x;
				y[i] = points[i].y;
			}
			double a = cov(x, y, size) / var(x, size);
			double b = avg(y, size) - a * (avg(x, size));

			return new Line(a, b);
		}

		public static double dev(Point p, Point[] points, int size)
		{
			Line l = linear_reg(points, size);
			return dev(p, l);
		}

		public static double dev(Point p, Line l)
		{
			return Math.Abs(p.y - l.f(p.x));
		}
	}

	public class Point
	{
		public double x { get; }
		public double y { get; }
		public Point(double x, double y)
		{
			this.x = x;
			this.y = y;
		}
	}
	public class Line
	{
		double a, b;
		public Line()
		{
			this.a = 0;
			this.b = 0;
		}
		public Line(double a, double b)
		{
			this.a = a;
			this.b = b;
		}
		public double f(double x)
		{
			return this.a * x + this.b;
		}
	}
}
