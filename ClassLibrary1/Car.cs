using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ClassLibrary1
{
    public class Car
    {

        public Car()
        {
        }
        public Car(Point p, Color color)
        {
            CurrCoord = p;
            Color = color;
            Counter = 0;
        }

        public int Counter { get; set; }
        public Point CurrCoord { get; set; }
        public Color Color { get; set; }
        public TrafficLights tr;

        public void Stand()
        {
        }

        public void setTrafficLight(TrafficLights newTr)
        {
            tr = newTr;
        }

        public void Go(int width)
        {
            Counter = 1;
            Point newCoor = new Point((CurrCoord.X + 1) % width, CurrCoord.Y);
            CurrCoord = newCoor;
        }
    }
}
