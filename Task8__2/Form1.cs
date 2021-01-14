using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ClassLibrary1;
using static ClassLibrary1.TrafficLights;
using Timer = System.Windows.Forms.Timer;

namespace Task8__2
{
    public partial class Form1 : Form
    {
        public Timer timer;
        public Map map;
        public List<Car> cars;



        public Form1()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            cars = new List<Car>();
            cars.Add(new Car(new ClassLibrary1.Point(0, 250), Color.Red));
            cars.Add(new Car(new ClassLibrary1.Point(0, 250), Color.Green));
            cars.Add(new Car(new ClassLibrary1.Point(0, 250), Color.Olive));
            cars.Add(new Car(new ClassLibrary1.Point(0, 250), Color.Aquamarine));
            cars.Add(new Car(new ClassLibrary1.Point(0, 250), Color.Chocolate));
            cars.Add(new Car(new ClassLibrary1.Point(0, 250), Color.DarkOrchid));

            map = new Map(cars, Width, Height);


            map.AddTraficLight();
            map.AddTraficLight();
            map.AddTraficLight();
            map.AddTraficLight();

            map.AddWalker();
            map.AddWalker();
            map.AddWalker();
            map.AddWalker();
            map.AddWalker();
            map.AddWalker();
            map.AddWalker();
            map.AddWalker();
            map.AddWalker();
            map.AddWalker();
            map.AddWalker();



            timer = new Timer();
            timer.Interval = (15);
            timer.Tick += timer_Tick;
            timer.Start();

        }


        private void timer_Tick(object sender, EventArgs e)
        {
            map.updateMap();
            map.CheckCrashWithPerson2();
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Draw(e.Graphics);
        }
        public void Draw(Graphics g)
        {

            foreach (var light in map.Lights)
            {
                SolidBrush blueBrush = new SolidBrush(Color.White);
                Rectangle rect = new Rectangle((int)light.Location.X, 0, 160, Height);
                g.FillRectangle(blueBrush, rect);

            }

            SolidBrush blueBrush1 = new SolidBrush(Color.Green); Rectangle rect1 = new Rectangle(0, 0, Width, 60);
            Rectangle rect2 = new Rectangle(0, Height - 100, Width, 100);
            g.FillRectangle(blueBrush1, rect1);
            g.FillRectangle(blueBrush1, rect2);


            foreach (var light in map.Lights)
            {
                if (light.CurrState == TrafficLights.State.Green)
                {
                    SolidBrush brush = new SolidBrush(Color.Black);
                    Rectangle elllipse = new Rectangle((int)light.Location.X - 102 + 80, 18, 24, 24);
                    g.FillRectangle(brush, elllipse);
                    brush = new SolidBrush(Color.Green);
                    elllipse = new Rectangle((int)light.Location.X - 100 + 80, 20, 20, 20);
                    g.FillRectangle(brush, elllipse);
                }
                else
                {
                    SolidBrush brush = new SolidBrush(Color.Black);
                    Rectangle elllipse = new Rectangle((int)light.Location.X - 102 + 80, 18, 24, 24);
                    g.FillRectangle(brush, elllipse);
                    brush = new SolidBrush(Color.Red);
                    elllipse = new Rectangle((int)light.Location.X - 100 + 80, 20, 20, 20);
                    g.FillRectangle(brush, elllipse);
                }
            }
            for (int i = 0; i < cars.Count; i++)
            {


                //фары
                SolidBrush brush = new SolidBrush(Color.Yellow);
                Rectangle elllipse = new Rectangle((int)cars[i].CurrCoord.X + 15, (int)cars[i].CurrCoord.Y - 5, 30, 30);
                g.FillPie(brush, elllipse, 330, 60);

                //машина
                brush = new SolidBrush(cars[i].Color);
                elllipse = new Rectangle((int)cars[i].CurrCoord.X, (int)cars[i].CurrCoord.Y, 40, 20);
                g.FillEllipse(brush, elllipse);

                //лобовое стекло
                brush = new SolidBrush(Color.Blue);
                elllipse = new Rectangle((int)cars[i].CurrCoord.X + 27, (int)cars[i].CurrCoord.Y + 2, 7, 15);
                g.FillEllipse(brush, elllipse);

            }

            foreach (var walker in map.Walkers.ToArray())
            {

                SolidBrush brush = new SolidBrush(Color.Black);
                Rectangle elllipse = new Rectangle((int)walker.CurrCoord.X, (int)walker.CurrCoord.Y, 10, 10);
                g.FillEllipse(brush, elllipse);

                elllipse = new Rectangle((int)walker.CurrCoord.X + 2, (int)walker.CurrCoord.Y, 5, 20);
                g.FillEllipse(brush, elllipse);
                //руки
                elllipse = new Rectangle((int)walker.CurrCoord.X - 4, (int)walker.CurrCoord.Y, 20, 20);
                g.FillPie(brush, elllipse, 340, 10);

                elllipse = new Rectangle((int)walker.CurrCoord.X - 8, (int)walker.CurrCoord.Y, 20, 20);
                g.FillPie(brush, elllipse, 200, 10);

                //ноги
                elllipse = new Rectangle((int)walker.CurrCoord.X - 4, (int)walker.CurrCoord.Y + 10, 20, 20);
                g.FillPie(brush, elllipse, 50, 10);

                elllipse = new Rectangle((int)walker.CurrCoord.X - 8, (int)walker.CurrCoord.Y + 10, 20, 20);
                g.FillPie(brush, elllipse, 130, 10);
            }

            foreach (var light in map.Lights.ToArray())
            {
                foreach (var police in light.service.ToArray())
                {
                    if (police.GetType().Name.Equals("Police"))
                    {
                        if (police.State == StateEnum.StateOfImServices.Start)
                        {
                            SolidBrush brush = new SolidBrush(Color.White);
                            Rectangle elllipse = new Rectangle((int)police.CurrCoord.X, (int)police.CurrCoord.Y, 40, 20);
                            g.FillRectangle(brush, elllipse);

                            g.DrawRectangle(Pens.Black, (int)police.CurrCoord.X, (int)police.CurrCoord.Y, 40, 20);
                            //лобовое стекло
                            brush = new SolidBrush(Color.Aqua);
                            elllipse = new Rectangle((int)police.CurrCoord.X + 30, (int)police.CurrCoord.Y, 10, 20);
                            g.FillEllipse(brush, elllipse);

                            brush = new SolidBrush(Color.Blue);
                            elllipse = new Rectangle((int)police.CurrCoord.X + 10, (int)police.CurrCoord.Y, 10, 20);
                            g.FillRectangle(brush, elllipse);


                            brush = new SolidBrush(Color.Red);

                            elllipse = new Rectangle((int)police.CurrCoord.X + 28, (int)police.CurrCoord.Y + 6, 4, 10);
                            g.FillEllipse(brush, elllipse);

                        }
                        else
                        {


                            SolidBrush brush = new SolidBrush(Color.White);
                            Rectangle elllipse = new Rectangle((int)police.CurrCoord.X, (int)police.CurrCoord.Y, 40, 20);
                            g.FillRectangle(brush, elllipse);

                            g.DrawRectangle(Pens.Black, (int)police.CurrCoord.X, (int)police.CurrCoord.Y, 40, 20);
                            //лобовое стекло
                            brush = new SolidBrush(Color.Aqua);
                            elllipse = new Rectangle((int)police.CurrCoord.X, (int)police.CurrCoord.Y, 10, 20);
                            g.FillEllipse(brush, elllipse);

                            brush = new SolidBrush(Color.Blue);
                            elllipse = new Rectangle((int)police.CurrCoord.X + 20, (int)police.CurrCoord.Y, 10, 20);
                            g.FillRectangle(brush, elllipse);


                            brush = new SolidBrush(Color.Red);

                            elllipse = new Rectangle((int)police.CurrCoord.X + 10, (int)police.CurrCoord.Y + 6, 4, 10);
                            g.FillEllipse(brush, elllipse);








                        }

                    }
                    else if (police.GetType().Name.Equals("Emergency"))
                    {
                        if (police.State == StateEnum.StateOfImServices.Start)
                        {
                            SolidBrush brush = new SolidBrush(Color.White);
                            Rectangle elllipse = new Rectangle((int)police.CurrCoord.X, (int)police.CurrCoord.Y, 40, 20);
                            g.FillRectangle(brush, elllipse);
                            g.DrawRectangle(Pens.Black, (int)police.CurrCoord.X - 1, (int)police.CurrCoord.Y - 1, 42, 22);

                            brush = new SolidBrush(Color.Red);
                            elllipse = new Rectangle((int)police.CurrCoord.X + 20, (int)police.CurrCoord.Y + 10, 11, 2);
                            g.FillRectangle(brush, elllipse);
                            elllipse = new Rectangle((int)police.CurrCoord.X + 24, (int)police.CurrCoord.Y + 6, 2, 11);
                            g.FillRectangle(brush, elllipse);
                            //лобовое стекло
                            brush = new SolidBrush(Color.Aqua);
                            elllipse = new Rectangle((int)police.CurrCoord.X + 30, (int)police.CurrCoord.Y, 10, 20);
                            g.FillEllipse(brush, elllipse);
                        }
                        else
                        {
                            SolidBrush brush = new SolidBrush(Color.White);
                            Rectangle elllipse = new Rectangle((int)police.CurrCoord.X, (int)police.CurrCoord.Y, 40, 20);
                            g.FillRectangle(brush, elllipse);
                            g.DrawRectangle(Pens.Black, (int)police.CurrCoord.X - 1, (int)police.CurrCoord.Y - 1, 42, 22);

                            brush = new SolidBrush(Color.Red);
                            elllipse = new Rectangle((int)police.CurrCoord.X + 20, (int)police.CurrCoord.Y + 10, 11, 2);
                            g.FillRectangle(brush, elllipse);
                            elllipse = new Rectangle((int)police.CurrCoord.X + 24, (int)police.CurrCoord.Y + 6, 2, 11);
                            g.FillRectangle(brush, elllipse);
                            //лобовое стекло
                            brush = new SolidBrush(Color.Aqua);
                            elllipse = new Rectangle((int)police.CurrCoord.X + 10, (int)police.CurrCoord.Y, 10, 20);
                            g.FillEllipse(brush, elllipse);

                        }
                    }
                }
            }


        }
        private Random rnd = new Random();

        private void button1_Click(object sender, EventArgs e)
        {
            Color randomColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
            Car car = new Car(new ClassLibrary1.Point(0, 250), randomColor);
            car.Counter = 1;
            map.AddCar(car);

        }
    }
}
