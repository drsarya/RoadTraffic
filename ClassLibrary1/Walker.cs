using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using static ClassLibrary1.TrafficLights;

namespace ClassLibrary1
{
    public class Walker
    {
        public double Velocity;
        public Boolean State { get; set; }
        public Point CurrCoord { get; set; }
        private Thread t;
        public Point BeginPoint;
        private State trafficLight;
        private double LastVelocity;
        private static Random rnd = new Random();
        public TrafficLights tr { get; set; }
        private int heighOfGrass = 60;   
        AutoResetEvent sync = new AutoResetEvent(false);

        public Walker(int height, Point currCoord, TrafficLights tr)
        {
            BeginPoint = currCoord;
            State = false;
            trafficLight = tr.CurrState;
            this.Height = height;
            this.CurrCoord = currCoord;
            Velocity = rnd.NextDouble() * (1 - 0.7) + 0.7;
            LastVelocity = Velocity;
            this.tr = tr;
            tr.Notify += ChangeColor;
            t = new Thread(new ThreadStart(Go));
            t.Start();

        }
        public int Height;

        public void ChangeColor(State state, Point Location)
        {
            if (state == TrafficLights.State.Green)
            {
                sync.Set();
            }
            trafficLight = state;
        }


        public void invalide()
        {
            State = true;
        }

        public void Go()
        {
            while (!State)
            {
                if (trafficLight == TrafficLights.State.Green)
                {
                    this.Velocity = LastVelocity;
                    Point newCoor = new Point((CurrCoord.X), (CurrCoord.Y + Velocity) * 1.0 % Height);
                    CurrCoord = newCoor;
                    Thread.Sleep(15);
                }
                else
                {
                    if (CurrCoord.Y + 30 < heighOfGrass || CurrCoord.Y > Height - heighOfGrass)
                    {
                        this.Velocity = LastVelocity;
                        Point newCoor = new Point((CurrCoord.X), (CurrCoord.Y + Velocity) * 1.0 % Height);
                        CurrCoord = newCoor;
                        Thread.Sleep(15);
                    }
                    else
                    {
                        if (CurrCoord.Y > heighOfGrass)
                        {
                            this.Velocity = LastVelocity * 2;
                            Point newCoor = new Point((CurrCoord.X), (CurrCoord.Y + Velocity) * 1.0 % Height);
                            CurrCoord = newCoor;
                            Thread.Sleep(15);
                        }
                        else {
                            sync.WaitOne();
                        }
                    }
                }
            }
        }
    }
}
