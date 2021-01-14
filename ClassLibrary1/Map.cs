using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static ClassLibrary1.TrafficLights;

namespace ClassLibrary1
{
    public class Map
    {

        public Map(List<Car> car, int width, int height)
        {
            this.Cars = car;
            this.Width = width;
            this.Height = height;
            Walkers = new List<Walker>();

        }

        public List<TrafficLights> Lights = new List<TrafficLights>();
        public List<Walker> Walkers { get; set; }
        private List<Car> Cars;
        public List<Walker> InvalideWalkers = new List<Walker>();  
        private List<Walker> HospitalWalkers = new List<Walker>();

        public List<Car> CrashCars = new List<Car>();
        private int WidthCar = 40;
        private int WidthPeshehod = 160;
        private int Width;
        private int Height;
        private int Distance = 20;
        


        private void ChangeColorTrafficLight(State message, Point location)
        {
            if (message == State.Green)
            {
                CheckCrash(location);
            }
        }
        private static Random rnd = new Random();

        public void AddWalker()
        {
            int ind = rnd.Next(Lights.Count);
            int dx = rnd.Next(120);
            Walker walker = new Walker(Height, new ClassLibrary1.Point(Lights[ind].Location.X + dx, 0), Lights[ind]);
            Walkers.Add(walker);

        }
        public void AddTraficLight()
        {
            TrafficLights.State currStateLight;
            if (rnd.Next(0, 2) == 1) { currStateLight = TrafficLights.State.Red; }
            else
            {
                currStateLight = TrafficLights.State.Green;
            }
            TrafficLights t = new TrafficLights(currStateLight);
            Lights.Add(t);
            double onePiece = Width / (Lights.Count + 1);
            int i = 1;
            foreach (var l in Lights.ToArray())
            {
                l.setLocation(new Point(onePiece * i - 80, 18));
                i++;
            }
            UpdateTrafficLightCar();
            t.Notify += ChangeColorTrafficLight;
        }
        public void UpdateTrafficLightCar()
        {
            double onePiece = Width / (Lights.Count + 1);
            foreach (var car in Cars.ToArray())
            {
                if (car != null)
                {
                    int currLight = (int)Math.Truncate((car.CurrCoord.X - 60) / onePiece);
                    car.setTrafficLight(Lights[currLight % Lights.Count]);
                }
            }
        }

        public void AddCar(Car newCar)
        {
            Cars.Add(newCar);
            updateMap();
        }

        public void CheckCrash(Point locationTr)
        {
            foreach (var car in Cars.ToArray())
            {
                if ((car.CurrCoord.X > car.tr.Location.X || car.CurrCoord.X + WidthCar > car.tr.Location.X) && car.CurrCoord.X < car.tr.Location.X + WidthPeshehod && car.tr.CurrState == TrafficLights.State.Green)
                {
                    if (car.tr.Location.Equals(locationTr))
                    {
                        if (!CrashCars.Contains(car))
                        {
                            CrashCars.Add(car);
                            int id = rnd.Next(0, 2);
                            car.tr.service[id].AddCrashPointToQueue(car.CurrCoord);
                            car.tr.service[id].GoToCrash();
                            Police newPol = (Police)car.tr.service[id];
                            newPol.Notify += updateEmergencyServise;

                        }
                    }
                }
            }
        }

        public void CheckCrashWithPerson2()
        {
            foreach (var car in Cars.ToArray())
            {
                if (car != null && (car.CurrCoord.X > car.tr.Location.X || car.CurrCoord.X + WidthCar > car.tr.Location.X) && car.CurrCoord.X < car.tr.Location.X + WidthPeshehod)
                {
                    foreach (var walker in Walkers.ToArray())
                    {
                        if (walker.CurrCoord.X > car.CurrCoord.X - 5 && walker.CurrCoord.X < car.CurrCoord.X + WidthCar + 10)
                        {
                            if ((walker.CurrCoord.Y > car.CurrCoord.Y || walker.CurrCoord.Y + 20 > car.CurrCoord.Y) && walker.CurrCoord.Y < car.CurrCoord.Y + 30)
                            {
                                if (!InvalideWalkers.Contains(walker))
                                {
                                    walker.invalide();
                                    int id = rnd.Next(2, 4);
                                    car.tr.service[id].AddCrashPointToQueue(walker.CurrCoord);
                                    car.tr.service[id].GoToCrash();
                                    Emergency newPol = (Emergency)car.tr.service[id];
                                    newPol.Notify += updateEmergencyServise;
                                    InvalideWalkers.Add(walker);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void HelpWalker(Point crash)
        {
            foreach (var walker in InvalideWalkers.ToArray())
            {
                if (walker != null && walker.CurrCoord.Equals(crash))
                {
                    HospitalWalkers.Add(walker);
                    Walkers.Remove(walker);
                    InvalideWalkers.Remove(walker);
                    break;
                }
            }
        }


        public void DeleteCar(IEmergencyService emergencyService)
        {
            foreach (var car in CrashCars.ToArray())
            {
                if (car != null && emergencyService.CurrCoord.Equals(car.CurrCoord))
                {
                    Cars.Remove(car);
                    CrashCars.Remove(car);
                    DeleteCar(emergencyService);
                    break;

                }

            }
        }

        public Boolean FindCarOnBoarder(Car currCar)
        {
            Car car = new Car();
            double minX = -1;
            for (int i = 0; i < Cars.Count; i++)
            {
                if (Cars[i] != null)
                {
                    if (minX == -1)
                    {
                        minX = Cars[i].CurrCoord.X;
                        car = Cars[i];
                    }
                    else if (Cars[i].CurrCoord.X < minX)
                    {
                        minX = Cars[i].CurrCoord.X;
                        car = Cars[i];
                    }
                }
            }
            if (Width - currCar.CurrCoord.X + car.CurrCoord.X > Distance + WidthCar + 7)
            {
                return true;
            }
            else { return false; }
        }

        public Car FindPrevCar(int index)
        {
            Car car = new Car();
            if (Cars[index].Counter == 0 || Cars[index].Counter == 0 && index == 0)
            {
                if (index == 0)
                {
                    return Cars[index];
                }
                else
                {
                    return Cars[index - 1];
                }
            }
            else if (Cars[index].Counter == 1)
            {
                double minDel = -1;
                for (int i = 0; i < Cars.Count; i++)
                {
                    if (i != index)
                    {
                        if (Cars[i] != null)
                        {
                            if (Cars[i].CurrCoord.X > Cars[index].CurrCoord.X)
                            {
                                if (minDel == -1)
                                {
                                    minDel = Cars[i].CurrCoord.X - Cars[index].CurrCoord.X;
                                    car = Cars[i];
                                }
                                else
                                {
                                    if (Cars[i].CurrCoord.X - Cars[index].CurrCoord.X < minDel)
                                    {
                                        minDel = Cars[i].CurrCoord.X - Cars[index].CurrCoord.X;
                                        car = Cars[i];
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return car;
        }

        private void ReturnWalker(Point crash)
        {
            foreach (var walker in HospitalWalkers)
            {
                if (walker.CurrCoord.Equals(crash))
                {
                    Walker a = new Walker(Height, walker.BeginPoint, walker.tr);
                    HospitalWalkers.Remove(walker);
                    Walkers.Add(a);
                    break;
                }
            }
        }

        public void updateEmergencyServise(StateEnum.StateOfImServices state, Point crash, String type, IEmergencyService imServ)
        {
            if (state == StateEnum.StateOfImServices.Purpose)
            {
                if (type.Equals("emergency"))
                {
                    HelpWalker(crash);
                }
                else
                {
                    DeleteCar(imServ);
                }
            }
            else if (state == StateEnum.StateOfImServices.Stop)
            {
                if (type.Equals("emergency"))
                {
                    ReturnWalker(crash);
                }
            }
        }

        
        public void updateMap()
        {

            UpdateTrafficLightCar();

            if (CrashCars.Count == 0 && InvalideWalkers.Count == 0)
            {

                for (int i = 0; i < Cars.Count; i++)
                {
                    if (Cars[i].tr.CurrState == TrafficLights.State.Green)
                    {
                        if (Cars[i].CurrCoord.X > Cars[i].tr.Location.X + 160 || Cars[i].tr.Location.X - Cars[i].CurrCoord.X > Distance * 2)
                        {
                            Car prevcar = FindPrevCar(i);
                            if (prevcar.CurrCoord == null)
                            {
                                if (FindCarOnBoarder(Cars[i]))
                                {
                                    Cars[i].Go(Width);
                                }
                                else
                                {
                                    Cars[i].Stand();
                                }
                            }
                            else
                            {
                                if ((Cars[i].CurrCoord.X > prevcar.CurrCoord.X || prevcar.CurrCoord.X - Cars[i].CurrCoord.X > WidthCar + Distance) || (i == 0 && Cars[0].Counter == 0))
                                {
                                    Cars[i].Go(Width);
                                }
                                else
                                {
                                    Cars[i].Stand();
                                }
                            }
                        }
                        else
                        {
                            Cars[i].Stand();
                        }
                    }
                    else
                    {
                        Car prevcar = FindPrevCar(i);
                        if (prevcar.CurrCoord == null)
                        {
                            if (FindCarOnBoarder(Cars[i]))
                            {
                                Cars[i].Go(Width);
                            }
                            else
                            {
                                Cars[i].Stand();
                            }
                        }
                        else
                        {
                            if ((Cars[i].CurrCoord.X > prevcar.CurrCoord.X || prevcar.CurrCoord.X - Cars[i].CurrCoord.X > WidthCar + Distance) || (i == 0 && Cars[0].Counter == 0))
                            {
                                Cars[i].Go(Width);
                            }
                            else
                            {
                                Cars[i].Stand();
                            }
                        }
                    }
                }
            }
        }
    }
}