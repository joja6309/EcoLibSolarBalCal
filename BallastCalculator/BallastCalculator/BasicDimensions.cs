using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BallastCalculator
{
    class BasicDimensions
    {
        public Tuple<double, double> Corner1;
        public Tuple<double, double> Corner2;
        public Tuple<double, double> Corner3;
        public Tuple<double, double> Corner4;
        public Tuple<double, double> Center = new Tuple<double, double>(0, 0);
        public double Height;
        public double Width;
        public double Xvalue;
        public double Yvalue;
        public void PrintAttributes()
        {
            Console.WriteLine("Corner 1: {0} ", Corner1);
            Console.WriteLine("Corner 2: {0} ", Corner2);
            Console.WriteLine("Corner 3: {0} ", Corner3);
            Console.WriteLine("Corner 4: {0} ", Corner4);
            Console.WriteLine("Center : {0}", Center);
            Console.WriteLine("Dimensions: ");
            Console.WriteLine("==================");
            Console.WriteLine("Width: {0}", Width);
            Console.WriteLine("Height: {0}", Height);
            Console.WriteLine(Environment.NewLine);

            Console.ReadKey();
            Console.WriteLine("Press Enter to Continue: ");
            Console.WriteLine(Environment.NewLine);


        }
        public void CalculateCenter()
        {
            double x1, x2, y1, y2;
            if (!(Corner1.Item1.Equals(Corner2.Item1)) && !(Corner1.Item2.Equals(Corner2.Item2)))
            {
                x1 = Corner1.Item1;
                x2 = Corner2.Item1;

                y1 = Corner1.Item2;
                y2 = Corner2.Item2;

            }

            else if (!(Corner1.Item1.Equals(Corner3.Item1)) && !(Corner1.Item2.Equals(Corner3.Item2)))

            //Corrected 

            {
                x1 = Corner1.Item1;
                x2 = Corner3.Item1;

                y1 = Corner1.Item2;
                y2 = Corner3.Item2;

            }
            else
            {
                x1 = Corner1.Item1;
                x2 = Corner4.Item1;

                y1 = Corner1.Item2;
                y2 = Corner4.Item2;

            }

            Width = x2 - x1;
            Height = y2 - y1;
            var x0 = x2;
            var y0 = y2;
            //KB NOTE: Extra variables added to identify fixed corner (NE corner) and then to be used in finding center consistently.
            if (Width < 0)
            {
                Width = Width * -1;
                x0 = x1;
            }
            if (Height < 0)
            {
                Height = Height * -1;
                y0 = y1;
            }


            // KB NOTE: changed Center so that it is always calculated from NE corner (largest x and largest y in block)
            Center = new Tuple<double, double>((x0 - (Width / 2)), (y0 - (Height / 2)));



        }

    }

}
   

