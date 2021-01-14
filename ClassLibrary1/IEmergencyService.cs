using System;
using System.Collections.Generic;
using System.Text;
using static ClassLibrary1.StateEnum;

namespace ClassLibrary1
{

    public interface IEmergencyService
    {
        string Fine();
        void Go();
        void GoBack();
        void Ride();
        void GoToCrash();
        Point PlaceOfCrash { get; set; }
        Point CurrCoord { get; set; }
        Point BeginPoint { get; set; }
        StateOfImServices State { get; set; }
        Queue<Point> Queue { get; set; }
        void AddCrashPointToQueue(Point point);

    }
}
