using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BallastCalculator
{
    class Panel : BasicDimensions
    {
        public int PanelID;

        public List<PanelBase> PanelBases = new List<PanelBase>();
        public int BallastLocation = 0; 

        public List<Panel> NeighborHood = new List<Panel>();
        public int Sliding;
        public int Uplift; 

        public int NE_Zone = 4;
        public int NW_Zone = 4;

        public int IFI_W2E_Port = 4; //count of total panels east of a given module until break
        public int IFI_W2E_Land = 4;

        public int IFI_SOUTH_Land = 4; //panel.IFI_south = Panel variable. For 0, panel is south edge. For 1, panel is NOT south edge.
        public int IFI_SOUTH_Port = 4; //panel.IFI_south = Panel variable. For 0, panel is south edge. For 1, panel is NOT south edge.

        public int IFI_NORTH_Land = 4;
        public int IFI_NORTH_Port = 4;

        public int IFI_E2W_Port = 4;  //panel.IFI_E2W = Panel variable. For 0, panel is east edge. For 1, panel is in col 2-4, for 2, panel is >= col 5
        public int IFI_E2W_Land = 4;   //panel.IFI_E2W = Panel variable. For 0, panel is east edge. For 1, panel is in col 2-4, for 2, panel is >= col 5

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



}
