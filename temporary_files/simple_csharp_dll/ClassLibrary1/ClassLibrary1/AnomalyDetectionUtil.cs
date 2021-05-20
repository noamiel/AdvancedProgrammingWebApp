using System;

namespace ClassLibrary1
{
    internal class AnomalyDetectionUtil
    {
        public static double Avg(double[] x, int size)
        {
            double sum = 0;
            for (int i = 0; i < size; sum += x[i], i++) ;
            return sum / size;
        }

        public static double Var(double[] x, int size)
        {
            double av = Avg(x, size);
            double sum = 0;
            for (int i = 0; i < size; i++)
            {
                sum += x[i] * x[i];
            }
            return sum / size - av * av;
        }

        public static double Cov(double[] x, double[] y, int size)
        {
            double sum = 0;
            for (int i = 0; i < size; i++)
            {
                sum += x[i] * y[i];
            }
            sum /= size;

            return sum - Avg(x, size) * Avg(y, size);
        }

        public static double Pearson(double[] x, double[] y, int size)
        {
            return Cov(x, y, size) / (Math.Sqrt(Var(x, size)) * Math.Sqrt(Var(y, size)));
        }

        public static Line LinearReg(Point[] points, int size)
        {
            double[] x = new double[size];
            double[] y = new double[size];
            for (int i = 0; i < size; i++)
            {
                x[i] = points[i].X;
                y[i] = points[i].Y;
            }
            double a = Cov(x, y, size) / Var(x, size);
            double b = Avg(y, size) - a * (Avg(x, size));

            return new Line(a, b);
        }

        public static double Dev(Point p, Point[] points, int size)
        {
            Line l = LinearReg(points, size);
            return Dev(p, l);
        }

        public static double Dev(Point p, Line l)
        {
            return Math.Abs(p.Y - l.F(p.X));
        }
    }

    public class Point
    {
        public double X { get; }
        public double Y { get; }

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Point Subtract(Point p)
        {
            return new Point(X - p.X, Y - p.Y);
        }

        public double Distance(Point p)
        {
            double dx = X - p.X;
            double dy = Y - p.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        // Signed area / determinant thing
        public double Cross(Point p)
        {
            return X * p.Y - Y * p.X;
        }
    }

    public class Line
    {
        private readonly double a;
        private readonly double b;

        public Line()
        {
            a = 0;
            b = 0;
        }

        public Line(double a, double b)
        {
            this.a = a;
            this.b = b;
        }

        public double F(double x)
        {
            return a * x + b;
        }
    }
}