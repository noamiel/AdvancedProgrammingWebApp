using System;
using System.Collections.Generic;

namespace ClassLibrary1
{
    public class Circle
    {
        public Point Center { get; }
        public double Radius { get; }

        public Circle(Point c, double r)
        {
            Center = c;
            Radius = r;
        }

        public static double Dist(Point a, Point b)
        {
            double x2 = (a.X - b.X) * (a.X - b.X);
            double y2 = (a.Y - b.Y) * (a.Y - b.Y);
            return Math.Sqrt(x2 + y2);
        }

        public static Circle From2Points(Point a, Point b)
        {
            double x = (a.X + b.X) / 2;
            double y = (a.Y + b.Y) / 2;
            double r = Dist(a, b) / 2;
            return new Circle(new Point(x, y), r);
        }

        public static Circle From3Points(Point a, Point b, Point c)
        {
            Point mAB = new Point((a.X + b.X) / 2, (a.Y + b.Y) / 2);
            double slopAB = (b.Y - a.Y) / (b.X - a.X);
            double pSlopAB = -1 / slopAB;
            Point mBC = new Point((b.X + c.X) / 2, (b.Y + c.Y) / 2);
            double slopBC = (c.Y - b.Y) / (c.X - b.X); // the slop of BC
            double pSlopBC = -1 / slopBC;

            double x = (-pSlopBC * mBC.X + mBC.Y + pSlopAB * mAB.X - mAB.Y) / (pSlopAB - pSlopBC);
            double y = pSlopAB * (x - mAB.X) + mAB.Y;
            Point center = new Point(x, y);
            double R = Dist(center, a);

            return new Circle(center, R);
        }

        public static Circle Trivial(List<Point> P)
        {
            if (P.Count == 0)
                return new Circle(new Point(0, 0), 0);
            else if (P.Count == 1)
                return new Circle(P[0], 0);
            else if (P.Count == 2)
                return From2Points(P[0], P[1]);

            Circle c = From2Points(P[0], P[1]);
            if (Dist(P[2], c.Center) <= c.Radius)
                return c;
            c = From2Points(P[0], P[2]);
            if (Dist(P[1], c.Center) <= c.Radius)
                return c;
            c = From2Points(P[1], P[2]);
            if (Dist(P[0], c.Center) <= c.Radius)
                return c;
            // else find the unique circle from 3 points
            return From3Points(P[0], P[1], P[2]);
        }

        public static Circle Welzl(Point[] P, List<Point> R, int n)
        {
            if (n < 0)
                throw new ArgumentException("n must be larger than 0 (or equal to it)");
            if (n == 0 || R.Count == 3)
            {
                return Trivial(R);
            }
            
			Random rnd = new Random();
			int i = rnd.Next(0, n);
			Point p = new Point(P[i].X, P[i].Y);
			
            // Point p = P[0];

            Point temp = P[0];
            P[0] = P[n - 1];
            P[n - 1] = temp;
            // Console.WriteLine(n);
            Circle c = Welzl(P, R, n - 1);

            if (Dist(p, c.Center) <= c.Radius)
            {
                return c;
            }

            R.Add(p);

            return Welzl(P, R, n - 1);
        }

        public static Circle FindMinCircle(Point[] points, int size)
        {
            return Welzl(points, new List<Point>(), size);
        }
    }
}