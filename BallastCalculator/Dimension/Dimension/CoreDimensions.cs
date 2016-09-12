using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dimensions
{
    
        public class BasicDimensions
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
        public class IFIPerimeter : BasicDimensions
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
        public class EcoPanel : BasicDimensions
        {
            public int PanelID;

            public List<PanelBase> PanelBases = new List<PanelBase>();
            public int BallastLocation = 0;

            public List<EcoPanel> NeighborHood = new List<EcoPanel>();
            public int Sliding;
            public int Uplift;

            public int NE_Zone = 4;
            public int NW_Zone = 4;

            public int IFI_W2E_Port = 4; //count of total panels east of a given module until break
            public int IFI_W2E_Land = 4;

            public int IFI_SOUTH_Land = 4; //EcoPanel.IFI_south = EcoPanel variable. For 0, EcoPanel is south edge. For 1, EcoPanel is NOT south edge.
            public int IFI_SOUTH_Port = 4; //EcoPanel.IFI_south = EcoPanel variable. For 0, EcoPanel is south edge. For 1, EcoPanel is NOT south edge.

            public int IFI_NORTH_Land = 4;
            public int IFI_NORTH_Port = 4;

            public int IFI_E2W_Port = 4;  //EcoPanel.IFI_E2W = EcoPanel variable. For 0, EcoPanel is east edge. For 1, EcoPanel is in col 2-4, for 2, EcoPanel is >= col 5
            public int IFI_E2W_Land = 4;   //EcoPanel.IFI_E2W = EcoPanel variable. For 0, EcoPanel is east edge. For 1, EcoPanel is in col 2-4, for 2, EcoPanel is >= col 5

            public List<int> DirectionList = new List<int>();
            public void SetPanelZones(IFIPerimeter perimeter)
            {


                if (((Math.Abs(perimeter.NW_corner.Item1 - Center.Item1)) < (787.402) && (Math.Abs(perimeter.NW_corner.Item2 - Center.Item2) < 787.402)))

                    NW_Zone = 1;


                else if ((Math.Abs(perimeter.NW_corner.Item1 - Center.Item1) < 787.402) && (Math.Abs(perimeter.NW_corner.Item2 - Center.Item2) > 787.402))

                    NW_Zone = 2;


                else if (Math.Abs(perimeter.NW_corner.Item1 - Center.Item1) < 1574.804)

                    NW_Zone = 3;


                else if (Math.Abs(perimeter.NW_corner.Item1 - Center.Item1) < 2362.206)

                    NW_Zone = 4;

                else

                    NW_Zone = 5;
                if ((Math.Abs(perimeter.NE_corner.Item1 - Center.Item1) < 787.402) && (Math.Abs(perimeter.NE_corner.Item2 - Center.Item2) < 787.402))

                    NE_Zone = 1;


                else if ((Math.Abs(perimeter.NE_corner.Item1 - Center.Item1) < 787.402) && (Math.Abs(perimeter.NE_corner.Item2 - Center.Item2) > 787.402))

                    NE_Zone = 2;


                else if (Math.Abs(perimeter.NE_corner.Item1 - Center.Item1) < 1574.804)

                    NE_Zone = 3;


                else if (Math.Abs(perimeter.NE_corner.Item1 - Center.Item1) < 2362.206)

                    NE_Zone = 4;

                else

                    NE_Zone = 5;



            }

            public void CalculatePanelCenter(double center_x, double center_y)
            {
                Center = new Tuple<double, double>((Xvalue + center_x), (Yvalue + center_y));

            }

            public Dictionary<string, List<string>> ExcelResults = new Dictionary<string, List<string>>();
            public double ValueFromExcel = 0;
        
        }
        public class PanelBase : BasicDimensions
        {
            public PanelBase(string baseId, int ballastLocation, Tuple<double, double>  center, double excelValue)
                {
                    UniqueID = baseId;
                    BallastLocation = ballastLocation;
                    Center = center;
                    ExcelValue = excelValue; 

                }
            public string UniqueID;
            public List<int> PanelIDList = new List<int>();
            public List<double> BlockWeightList = new List<double>(); 
            public string EdgeID;
            public int LoadShare;
            public double LoadValue;
            public int BallastLocation;
            public double BlockWeight;
            public double IFIBaseTotal;
            public double ExcelValue;
            public int BlockTotal; 
            
        }

    }

