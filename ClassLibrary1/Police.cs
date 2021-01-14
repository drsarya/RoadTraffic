using System;
using System.Collections.Generic;
using System.Threading;
using static ClassLibrary1.StateEnum;

namespace ClassLibrary1
{
    public class Police : IEmergencyService
    {
        public Point CurrCoord { get; set; }
        public Point PlaceOfCrash { get; set; }
        public Point BeginPoint { get; set; }
        public StateOfImServices State { get; set; }
        Thread Thread;
        public Queue<Point> Queue { get; set; }

        public Police(Point beginPoint)
        {
            BeginPoint = beginPoint;
            this.CurrCoord = beginPoint;
            State = StateOfImServices.Wait;
            Queue = new Queue<Point>();

        }
        public void GoToCrash()
        {
            if (State == StateOfImServices.Wait)
            {
                PlaceOfCrash = Queue.Dequeue();
                State = StateOfImServices.Start;
                Notify?.Invoke(StateOfImServices.Start, PlaceOfCrash, "policy", this);
                Thread = new Thread(new ThreadStart(Ride));
                Thread.Start();
            }
        }

        public void AddCrashPointToQueue(Point point)
        {
            Queue.Enqueue(point);
        }

        public string Fine()
        {
            return "Нарушение ПДД";
        }
        public delegate void AccountHandler(StateOfImServices message, Point crash, String policy, IEmergencyService emergencyService);
        public event AccountHandler Notify;

        public void Ride()
        {
            bool trof = true;
            while (trof)
            {
                if (State == StateOfImServices.Start)
                {
                    Go();
                }
                else if (State == StateOfImServices.Purpose)
                {
                    GoBack();
                }
                else if (State == StateOfImServices.Stop)
                {
                    Notify?.Invoke(StateOfImServices.Stop, PlaceOfCrash, "policy", this);
                    if (Queue.Count > 0)
                    {
                        State = StateOfImServices.Start;
                        PlaceOfCrash = Queue.Dequeue();
                    }
                    else
                    {
                        trof = false;
                        State = StateOfImServices.Wait;
                        PlaceOfCrash = null;
                    }
                }
                Thread.Sleep(10);
            }
        }

        public void Go()
        {
            if (PlaceOfCrash == null)
            {
                return;
            }
            double dy = PlaceOfCrash.Y - CurrCoord.Y;
            double dx = PlaceOfCrash.X - CurrCoord.X;

            if (CurrCoord.X != PlaceOfCrash.X && CurrCoord.Y != PlaceOfCrash.Y)
            {
                if (Math.Abs(dx) > Math.Abs(dy))
                {
                    Point newCoor = new Point((CurrCoord.X + dx / dy), CurrCoord.Y + 1);
                    CurrCoord = newCoor;
                }
                else
                {
                    Point newCoor = new Point((CurrCoord.X + 1), CurrCoord.Y + dy / dx);
                    CurrCoord = newCoor;
                }
            }
            else
            {
                Thread.Sleep(150);
                State = StateOfImServices.Purpose;
                Notify?.Invoke(StateOfImServices.Purpose, PlaceOfCrash, "policy", this);
            }
        }


        public void GoBack()
        {
            double dy = CurrCoord.Y - BeginPoint.Y;
            double dx = CurrCoord.X - BeginPoint.X;
            if (CurrCoord.X != BeginPoint.X && CurrCoord.Y != BeginPoint.Y)
            {
                if (Math.Abs(dx) > Math.Abs(dy))
                {
                    Point newCoor = new Point((CurrCoord.X - dx / dy), CurrCoord.Y - 1);
                    CurrCoord = newCoor;
                }
                else
                {
                    Point newCoor = new Point((CurrCoord.X - 1), CurrCoord.Y - dy / dx);
                    CurrCoord = newCoor;
                }
            }
            else
            {
                State = StateOfImServices.Stop;
            }
        }
    }
}
