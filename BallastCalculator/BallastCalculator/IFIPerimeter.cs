using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BallastCalculator
{
    class IFIPerimeter : BasicDimensions
    {
        public Tuple<double, double> NE_corner = new Tuple<double, double>(0, 0);
        public Tuple<double, double> NW_corner = new Tuple<double, double>(0, 0);
        // IFI from entities 
        // Set NE 
        // Set WN
        public void SetCorners()
        {
            List<Tuple<double, double>> corner_list = new List<Tuple<double, double>>();
            corner_list.Add(Corner1);
            corner_list.Add(Corner2);
            corner_list.Add(Corner3);
            corner_list.Add(Corner4);
            var max_x = corner_list[0].Item1;
            var max_y = corner_list[0].Item2;
            var min_x = corner_list[0].Item1;
            foreach (var x in corner_list)
            {
                if (x.Item1 >= max_x)
                {
                    max_x = x.Item1;

                }
                if (x.Item2 >= max_y)
                {
                    max_y = x.Item2;
                }
                if (x.Item1 <= min_x)
                {
                    min_x = x.Item1;
                }

            }
            if ((Corner1.Item1 == max_x) && (Corner1.Item2 == max_y))
            {
                NE_corner = Corner1;

            }
            else if ((Corner2.Item1 == max_x) && (Corner2.Item2 == max_y))
            {
                NE_corner = Corner2;
            }
            else if ((Corner3.Item1 == max_x) && (Corner3.Item2 == max_y))
            {
                NE_corner = Corner3;
            }
            else
            {
                NE_corner = Corner4;
            }

            if ((Corner1.Item1 == min_x) && (Corner1.Item2 == max_y))
            {
                NW_corner = Corner1;

            }
            else if ((Corner2.Item1 == min_x) && (Corner2.Item2 == max_y))
            {
                NW_corner = Corner2;
            }
            else if ((Corner3.Item1 == min_x) && (Corner3.Item2 == max_y))
            {
                NW_corner = Corner3;
            }
            else
            {
                NW_corner = Corner4;
            }

            return;

        }

        public void PrintIFIData()
        {

            Console.WriteLine("IFI Values:");
            Console.WriteLine("============");
            PrintAttributes();
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("IFI Corners: ");
            Console.WriteLine("North East Corner: {0}", NE_corner);
            Console.WriteLine("North west Corner: {0}", NW_corner);
            Console.WriteLine(Environment.NewLine);

        }


    }
}
    
//Uplift 25 
//Sliding 81 