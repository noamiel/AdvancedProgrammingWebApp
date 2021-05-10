using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
	public class Circle
	{
		public Point center { get; }
		public double radius { get; }
		public Circle(Point c, double r)
		{
			this.center = c;
			this.radius = r;
		}
		public static double dist(Point a, Point b)
		{
			double x2 = (a.x - b.x) * (a.x - b.x);
			double y2 = (a.y - b.y) * (a.y - b.y);
			return Math.Sqrt(x2 + y2);
		}

		public static Circle from2points(Point a, Point b)
		{
			double x = (a.x + b.x) / 2;
			double y = (a.y + b.y) / 2;
			double r = dist(a, b) / 2;
			return new Circle(new Point(x, y), r);
		}

		public static Circle from3Points(Point a, Point b, Point c)
		{
			Point mAB = new Point((a.x+b.x)/ 2 , (a.y + b.y) / 2);
			double slopAB = (b.y - a.y) / (b.x - a.x);
			double pSlopAB = -1 / slopAB;
			Point mBC = new Point((b.x+c.x)/ 2 , (b.y + c.y) / 2);
			double slopBC = (c.y - b.y) / (c.x - b.x); // the slop of BC
			double pSlopBC = -1 / slopBC; 

			double x = (-pSlopBC * mBC.x + mBC.y + pSlopAB * mAB.x - mAB.y) / (pSlopAB - pSlopBC);
			double y = pSlopAB * (x - mAB.x) + mAB.y;
			Point center = new Point(x, y);
			double R = dist(center, a);

			return new Circle(center, R);
		}

		public static Circle trivial(List<Point> P)
		{
			if (P.Count == 0)
				return new Circle(new Point(0, 0), 0);
			else if (P.Count == 1)
				return new Circle(P[0], 0);
			else if (P.Count == 2)
				return from2points(P[0], P[1]);

			Circle c = from2points(P[0], P[1]);
			if (dist(P[2], c.center) <= c.radius)
				return c;
			c = from2points(P[0], P[2]);
			if (dist(P[1], c.center) <= c.radius)
				return c;
			c = from2points(P[1], P[2]);
			if (dist(P[0], c.center) <= c.radius)
				return c;
			// else find the unique circle from 3 points
			return from3Points(P[0], P[1], P[2]);
		}

		public static Circle welzl(Point[] P, List<Point> R, int n)
		{
			if (n == 0 || R.Count == 3)
			{
				return trivial(R);
			}
			/*
			Random rnd = new Random();
			int i = rnd.Next(0, n);
			Point p = new Point(P[i].x, P[i].y);
			*/
			Point p = P[0];
			
			Point temp = P[0];
			P[0] = P[n - 1];
			P[n - 1] = temp;
			Circle c = welzl(P, R, n - 1);

			if (dist(p, c.center) <= c.radius)
            {
				return c;
			}
				

			R.Add(p);

			return welzl(P, R, n - 1);
		}

		public static Circle findMinCircle(Point[] points, int size)
		{
			return welzl(points, new List<Point>(),size);
		}
	}

	
}
