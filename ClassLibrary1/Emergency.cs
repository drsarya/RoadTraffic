using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static ClassLibrary1.Map;
using static ClassLibrary1.StateEnum;

namespace ClassLibrary1
{
    public class Emergency : IEmergencyService
    {
        public Point CurrCoord { get; set; }
        public Point PlaceOfCrash { get; set; }
        public Point BeginPoint { get; set; }

        public StateOfImServices State { get; set; }
        public Queue<Point> Queue { get; set; }
        public delegate void AccountHandler(StateOfImServices message, Point crash, String type, IEmergencyService emergencyService);
        public event AccountHandler Notify;

        Thread t;

        public Emergency(Point beginPoint)
        {
            Queue = new Queue<Point>();
            BeginPoint = beginPoint;
            this.CurrCoord = beginPoint;
            State = StateOfImServices.Wait;

        }
        public string Fine()
        {
            return "Авария с участием человека";
        }
        public void AddCrashPointToQueue(Point point)
        {
            Queue.Enqueue(point);
        }
        public void GoToCrash()
        {

            if (State == StateOfImServices.Wait)
            {
                PlaceOfCrash = Queue.Dequeue();
                State = StateOfImServices.Start;
                Notify?.Invoke(StateOfImServices.Start, PlaceOfCrash, "emergency", this);
                t = new Thread(new ThreadStart(Ride));
                t.Start();
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

            if ((int)CurrCoord.X != (int)PlaceOfCrash.X && (int)CurrCoord.Y != (int)PlaceOfCrash.Y)
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
                Thread.Sleep(250);
                State = StateOfImServices.Purpose;
                Notify?.Invoke(StateOfImServices.Purpose, PlaceOfCrash, "emergency", this);
            }
        }

        public void Ride()
        {
            bool trof = true;
            Console.WriteLine("старт" + Thread.CurrentThread.ManagedThreadId);

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
                    Notify?.Invoke(StateOfImServices.Stop, PlaceOfCrash, "emergency", this);
                    if (Queue.Count > 0)
                    {
                        State = StateOfImServices.Start;
                        PlaceOfCrash = Queue.Dequeue();
                    }
                    else
                    {
                        State = StateOfImServices.Wait;
                        trof = false;
                        PlaceOfCrash = null;
                    }
                }
                Thread.Sleep(10);
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

