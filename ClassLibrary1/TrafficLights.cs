
using System;
using System.Collections.Generic;
using System.Threading;

namespace ClassLibrary1
{
    public class TrafficLights
    {

        public delegate void AccountHandler(State message, Point Location);
        public event AccountHandler Notify;
        public State CurrState { get; set; }
        public Point Location { get; set; }
        public List<IEmergencyService> service = new List<IEmergencyService>();

        public TrafficLights(State state)
        {
            CurrState = state;
            runLight();
        }
        public void setLocation(Point Location)
        {
            service.Clear();
            this.Location = Location;
            double X = Location.X;
            service.Add(new Police(new Point(X - 80, Location.Y - 20)));
            service.Add(new Police(new Point(X - 80, Location.Y)));
            service.Add(new Emergency(new Point(X - 145, Location.Y - 20)));
            service.Add(new Emergency(new Point(X - 145, Location.Y)));

        }
        public enum State
        {
            Green,
            Red
        }

        public void runLight()
        {
            new Thread(() =>
            {
                while (true)
                {
                    changeState();
                    Thread.Sleep(7000);
                }
            }).Start();

        }
        public void changeState()
        {
            if (CurrState == State.Green)
            {
                CurrState = State.Red;
                Notify?.Invoke(State.Red, Location);
            }
            else if (CurrState == State.Red)
            {
                CurrState = State.Green;
                Notify?.Invoke(State.Green, Location);
            }
        }
    }
}
