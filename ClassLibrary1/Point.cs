using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary1
{
    public class Point
    {
        public double X { get; set; }
        public double Y { get; set; }
        public Point(double x, double y)
        {
            this.X = x;
            this.Y = y;


        }
        public override string ToString()
        {
            return X + "   " + Y;
        }
        public override bool Equals(object obj)
        {
            Point p = (Point)obj;

            return this.X == (p.X) && this.Y == (p.Y);
        }
    }
}
