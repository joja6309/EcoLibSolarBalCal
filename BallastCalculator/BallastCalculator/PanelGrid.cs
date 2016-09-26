using System;
using System.Collections.Generic;
using System.Linq;
using Dimensions;


namespace BallastCalculator
{

    class PanelGrid
    {
        private readonly BasicDimensions BlocksValues;
        public List<EcoPanel> PanelList;
        public List<Base> PanelBaseList = new List<Base>();
        public Random rand = new Random();
        private double BallastValue;
        //private bool Landscape;
        public PanelGrid(BasicDimensions perimeter, List<EcoPanel> plist, double bal) // Called First 
        {
            BlocksValues = perimeter;
            PanelList = plist;
            BallastValue = bal;
            RunPanelCalculations(); // Generates Call -> Program Now in Run EcoPanel Calculations Function

        }
        public void SetPanelList(List<EcoPanel> plist)
        {
            PanelList = plist;
        }
        public void RunBasePanelCalculations()
        {
            foreach(EcoPanel panel in PanelList)
            {
                CalculatePanelCorners(panel); 
            }
            foreach(Base pb in PanelBaseList)
            {
                CalculateBlockTotalValues(pb);
            }
        }
       
//IFI_Value(pulled from excel, per module)
//ballastLoc variable(previously calculated, per module)...
//-------------
//| 1 | 2 | 3 |
//-------------
//| 4 | 5 | 6 |
//-------------
//| 7 | 8 | 9 |
//-------------
//ModNW - portion of IFI_Value sent to northwest corner of module
//ModNE - portion of IFI_Value sent to northeast corner of module
//ModSW - portion of IFI_Value sent to southwest corner of module
//ModSE - portion of IFI_Value sent to southeast corner of module
//northRow - modifier for north row feet(1.4 for now)
//southRow - modifier for south row feet(1.6 for now)

        private List<double> CornerWeightContribution(int ballastLoc, double IFI_Value)
        {
            double ModNW = 0;
            double ModNE = 0;
            double ModSW = 0;
            double ModSE = 0;
            double northRow = 1.4;
            double southRow = 1.6;
            List<double> mod_values = new List<double>();


            if (ballastLoc == 1)
            {
                ModNW = IFI_Value * 2 / 7 * northRow;
                ModNE = IFI_Value * 2 / 7 * northRow;
                ModSW = IFI_Value * 2 / 7;
                ModSE = IFI_Value * 1 / 7;
                mod_values.Add(ModNE);
                mod_values.Add(ModNW);
                mod_values.Add(ModSW);
                mod_values.Add(ModSE);
                return mod_values;
            }
            else if (ballastLoc == 2)
            {
                ModNW = IFI_Value * 1 / 3 * northRow;
                ModNE = IFI_Value * 1 / 3 * northRow;
                ModSW = IFI_Value * 1 / 6;
                ModSE = IFI_Value * 1 / 6;
                mod_values.Add(ModNE);
                mod_values.Add(ModNW);
                mod_values.Add(ModSW);
                mod_values.Add(ModSE);
                return mod_values;
            }
            else if (ballastLoc == 3)
            {
                ModNW = IFI_Value * 2 / 7 * northRow;
                ModNE = IFI_Value * 2 / 7 * northRow;
                ModSW = IFI_Value * 1 / 7;
                ModSE = IFI_Value * 2 / 7;
                mod_values.Add(ModNE);
                mod_values.Add(ModNW);
                mod_values.Add(ModSW);
                mod_values.Add(ModSE);
                return mod_values;
            }
            else if (ballastLoc == 4)
            {
                ModNW = IFI_Value * 1 / 3;
                ModNE = IFI_Value * 1 / 6;
                ModSW = IFI_Value * 1 / 3;
                ModSE = IFI_Value * 1 / 6;
                mod_values.Add(ModNE);
                mod_values.Add(ModNW);
                mod_values.Add(ModSW);
                mod_values.Add(ModSE);
                return mod_values;
            }
            else if (ballastLoc == 5)
            {
                ModNW = IFI_Value * 1 / 4;
                ModNE = IFI_Value * 1 / 4;
                ModSW = IFI_Value * 1 / 4;
                ModSE = IFI_Value * 1 / 4;
                mod_values.Add(ModNE);
                mod_values.Add(ModNW);
                mod_values.Add(ModSW);
                mod_values.Add(ModSE);
                return mod_values;
            }
            else if (ballastLoc == 6)
            {
                ModNW = IFI_Value * 1 / 6;
                ModNE = IFI_Value * 1 / 3;
                ModSW = IFI_Value * 1 / 6;
                ModSE = IFI_Value * 1 / 3;
                mod_values.Add(ModNE);
                mod_values.Add(ModNW);
                mod_values.Add(ModSW);
                mod_values.Add(ModSE);
                return mod_values;
            }
            else if (ballastLoc == 7)
            {
                ModNW = IFI_Value * 1 / 3;
                ModNE = IFI_Value * 1 / 6;
                ModSW = IFI_Value * 1 / 3 * southRow;
                ModSE = IFI_Value * 1 / 6 * southRow;
                mod_values.Add(ModNE);
                mod_values.Add(ModNW);
                mod_values.Add(ModSW);
                mod_values.Add(ModSE);
                return mod_values;
            }
            else if (ballastLoc == 8)
            {
                ModNW = IFI_Value * 1 / 4;
                ModNE = IFI_Value * 1 / 4;
                ModSW = IFI_Value * 1 / 4 * southRow;
                ModSE = IFI_Value * 1 / 4 * southRow;
                mod_values.Add(ModNE);
                mod_values.Add(ModNW);
                mod_values.Add(ModSW);
                mod_values.Add(ModSE);
                return mod_values;
            }
            else if (ballastLoc == 9)
            {
                ModNW = IFI_Value * 1 / 6;
                ModNE = IFI_Value * 1 / 3;
                ModSW = IFI_Value * 1 / 6 * southRow;
                ModSE = IFI_Value * 1 / 3 * southRow;
                mod_values.Add(ModNE);
                mod_values.Add(ModNW);
                mod_values.Add(ModSW);
                mod_values.Add(ModSE);
                return mod_values;
            }
            else
                mod_values.Add(1);
                mod_values.Add(1);
                mod_values.Add(1);
            
            mod_values.Add(1);

            return mod_values;
                return mod_values;
        }
        private void CalculatePanelCorners(EcoPanel EcoPanel)
        {
            var x_start = EcoPanel.Center.Item1;
            var y_start = EcoPanel.Center.Item2;
            Tuple<double, double> corner_neighbor_NW = new Tuple<double, double>(x_start - .5 * (.5 + BlocksValues.Width), y_start + .5 * (17.494 + BlocksValues.Height)); //NWest
            Tuple<double, double> corner_neighbor_NE = new Tuple<double, double>(x_start + .5 * (.5 + BlocksValues.Width), y_start + .5 * (17.494 + BlocksValues.Height)); //NEast
            Tuple<double, double> corner_neighbor_SE = new Tuple<double, double>(x_start + .5 * (.5 + BlocksValues.Width), y_start - .5 * (17.494 + BlocksValues.Height)); //SEast
            Tuple<double, double> corner_neighbor_SW = new Tuple<double, double>(x_start - .5 * (.5 + BlocksValues.Width), y_start - .5 * (17.494 + BlocksValues.Height)); //SWest
            List<Tuple<double, double>> temp_list = new List<Tuple<double, double>>();
            temp_list.Add(corner_neighbor_NE);
            temp_list.Add(corner_neighbor_NW);
            temp_list.Add(corner_neighbor_SW);
            temp_list.Add(corner_neighbor_SE);
            List<double> mod_values;
            if (EcoPanel.BallastLocation == 0)
            {
                mod_values = CornerWeightContribution(1, EcoPanel.ValueFromExcel);

            }
            mod_values = CornerWeightContribution(EcoPanel.BallastLocation, EcoPanel.ValueFromExcel);

           
                for (int x = 0; x < temp_list.Count; x++)
                {
                    if (PanelBaseList.Count == 0)
                    {
                       
                        Base new_base = new Base(PanelBaseList.Count().ToString(), temp_list[x]);
                        new_base.ContributionList.Add(mod_values[x]);
                    PanelBaseList.Add(new_base);
                    }
                    else
                    {
                        List<Base> matching_bases = PanelBaseList.Where(c => Math.Abs(c.Center.Item1 - temp_list[x].Item1) <= .5 && Math.Abs(c.Center.Item2 - temp_list[x].Item2) <= .5).Distinct().ToList();
                        if (matching_bases.Count() != 0)
                        {
                            foreach (var p in matching_bases)
                            {
                                p.ContributionList.Add(mod_values[x]);

                            }

                        }
                        else
                        {
                            Base new_base = new Base(PanelBaseList.Count().ToString(), temp_list[x]);
                            new_base.ContributionList.Add(mod_values[x]);
                        PanelBaseList.Add(new_base);

                    }


                }
                }

            }
        private void CalculateBlockTotalValues(Base base_panel)
        {
            double IFI_Base_Total = 0;
            foreach (double cornerValue in base_panel.ContributionList)
            {
                IFI_Base_Total += cornerValue; //IFI_Base_Total

            }
            base_panel.UnroundedBallastBlockValue = (IFI_Base_Total / BallastValue) - 0.3; 

            base_panel.BallastBlockValue = Convert.ToInt32(Math.Ceiling(base_panel.UnroundedBallastBlockValue));

        }
        private void RunPanelCalculations()
        {

            foreach (EcoPanel EcoPanel in PanelList)
            {
                EcoPanel.CalculatePanelCenter(BlocksValues.Center.Item1, BlocksValues.Center.Item2);
                EcoPanel.Uplift = GetUpliftValue(EcoPanel);
                EcoPanel.Sliding = GetSlidingValue(EcoPanel);
            }
            RunIFILocationChecks();
            BuildDirectionList();
            CalculateBallastLocation();

        }
        private Tuple<double, double, int> GenerateNeighbor(int n, double x_start, double y_start, int direction)
        {
            Tuple<double, double, int> temp_neighbor = new Tuple<double, double, int>(0, 0, 0);
            if (direction == 0)// East
                temp_neighbor = new Tuple<double, double, int>(x_start + (.5 + BlocksValues.Width) * n, y_start, 0); //East
            else if (direction == 1) //North
                temp_neighbor = new Tuple<double, double, int>(x_start, y_start + (17.494 + BlocksValues.Height) * n, 1);//North
            else if (direction == 2)//South
                temp_neighbor = new Tuple<double, double, int>(x_start, y_start - (17.494 + BlocksValues.Height), 2); //South
            else if (direction == 3)//West
                temp_neighbor = new Tuple<double, double, int>(x_start - (.5 + BlocksValues.Width) * n, y_start, 3); // West
            return temp_neighbor;
        }
        private void BuildDirectionList()
        {
            foreach (EcoPanel panel in PanelList)
            {
                //panel.DirectionList = null; //KB DEBUG! clears out potential direction 0?
                //Console.WriteLine("direction list for panel " + panel.PanelID + " includes:");
                for (int i = 0; i <= 3; i++)
                {
                    var temp = GenerateNeighbor(1, panel.Center.Item1, panel.Center.Item2, i);
                    if (PanelList.Any(x => (Math.Abs(x.Center.Item1 - temp.Item1) <= .5) && (Math.Abs(x.Center.Item2 - temp.Item2) <= .5)))
                    {
                        panel.DirectionList.Add(i);
                        //Console.WriteLine(i);
                    }

                }
                
            }
      
        }
        public void CalculateBallastLocation()
        {//KB DEBUG! Syntax wasn't working, returning all 0s for ballast locations
            foreach (EcoPanel EcoPanel in PanelList)
            {
                if (EcoPanel.DirectionList.Contains(2) && EcoPanel.DirectionList.Contains(0) && !EcoPanel.DirectionList.Contains(3) && !EcoPanel.DirectionList.Contains(1))
                {
                    EcoPanel.BallastLocation = 1;
                    EcoPanel.isEdge = true;
                }
                else if (EcoPanel.DirectionList.Contains(2) && EcoPanel.DirectionList.Contains(0) && EcoPanel.DirectionList.Contains(3) && !EcoPanel.DirectionList.Contains(1))
                {
                    EcoPanel.BallastLocation = 2;
                }
                else if (EcoPanel.DirectionList.Contains(2) && EcoPanel.DirectionList.Contains(3) && !EcoPanel.DirectionList.Contains(0) && !EcoPanel.DirectionList.Contains(1))
                {
                    EcoPanel.BallastLocation = 3;
                    EcoPanel.isEdge = true;
                }
                else if (EcoPanel.DirectionList.Contains(1) && EcoPanel.DirectionList.Contains(0) && EcoPanel.DirectionList.Contains(2) && !EcoPanel.DirectionList.Contains(3))
                {
                    EcoPanel.BallastLocation = 4;
                    EcoPanel.isEdge = true;
                }
                else if (EcoPanel.DirectionList.Contains(1) && EcoPanel.DirectionList.Contains(0) && EcoPanel.DirectionList.Contains(3) && EcoPanel.DirectionList.Contains(2))
                {
                    EcoPanel.BallastLocation = 5;
                }
                else if (EcoPanel.DirectionList.Contains(1) && EcoPanel.DirectionList.Contains(2) && EcoPanel.DirectionList.Contains(3) && !EcoPanel.DirectionList.Contains(0))
                {
                    EcoPanel.BallastLocation = 6;
                    EcoPanel.isEdge = true;
                }
                else if (EcoPanel.DirectionList.Contains(1) && EcoPanel.DirectionList.Contains(0) && !EcoPanel.DirectionList.Contains(3) && !EcoPanel.DirectionList.Contains(2))
                {
                    EcoPanel.BallastLocation = 7;
                    EcoPanel.isEdge = true;
                }
                else if (EcoPanel.DirectionList.Contains(1) && EcoPanel.DirectionList.Contains(0) && EcoPanel.DirectionList.Contains(3) && !EcoPanel.DirectionList.Contains(2))
                {
                    EcoPanel.BallastLocation = 8;
                }
                else if (EcoPanel.DirectionList.Contains(3) && EcoPanel.DirectionList.Contains(1) && !EcoPanel.DirectionList.Contains(2) && !EcoPanel.DirectionList.Contains(0))
                {
                    EcoPanel.BallastLocation = 9;
                    EcoPanel.isEdge = true;
                }
                else
                {
                    Console.WriteLine("ERROR! Module at (" + Math.Round(EcoPanel.Center.Item1, 3) + "," + Math.Round(EcoPanel.Center.Item2, 3) + ") appears to be part of a single row or column.");
                    Console.WriteLine("Please revise layout to remove single rows or columns when using EF3.");
                    Console.ReadKey();
                }

            }
        }
        private void Set_E2WTruCol_LAND(List<EcoPanel> PanelList)
        {
            foreach (var panel in PanelList)
            {
                if (panel.ColumnNumberE2W_LAND == 1)
                {
                    IEnumerable<EcoPanel> return_PA = PanelList.Where(x => ((x.Center.Item1 - panel.Center.Item1) > 0 && (x.Center.Item1 - panel.Center.Item1) < (BlocksValues.Width + 55) && (x.Center.Item2 - panel.Center.Item2) > 0 && (x.Center.Item2 - panel.Center.Item2) <= (BlocksValues.Height + 8.75)));
                    IEnumerable<EcoPanel> return_PB = PanelList.Where(x => ((x.Center.Item1 - panel.Center.Item1) > 0 && (x.Center.Item1 - panel.Center.Item1) < (BlocksValues.Width + 55) && (panel.Center.Item2 - x.Center.Item2) > 0 && (panel.Center.Item2 - x.Center.Item2) < (BlocksValues.Height + 8.75)));
                    if ((return_PB.Count() != 0) && (return_PA.Count() != 0))
                    {
                        panel.TrueE2Wcol_LAND = return_PA.First().TrueE2Wcol_LAND + 1;
                    }
                    else
                    {
                        panel.TrueE2Wcol_LAND = panel.ColumnNumberE2W_LAND;
                    }
                }
                else if (panel.ColumnNumberE2W_LAND > 1 && panel.ColumnNumberE2W_LAND <= 4)
                {
                    IEnumerable<EcoPanel> return_PA = PanelList.Where(x => ((Math.Abs(panel.Center.Item1 + (BlocksValues.Width + .5) - x.Center.Item1) < .5) && (Math.Abs(panel.Center.Item2 - x.Center.Item2) < .5)));
                    panel.TrueE2Wcol_LAND = return_PA.First().TrueE2Wcol_LAND + 1;
                }
                else
                {
                    panel.TrueE2Wcol_LAND = 5;
                }

            }
        }
        private void Set_E2WTruCol_PORT(List<EcoPanel> PanelList)
        {
            foreach (var panel in PanelList)
            {
                if (panel.ColumnNumberE2W_PORT == 1)
                {

                    IEnumerable<EcoPanel> return_PA = PanelList.Where(x => ((x.Center.Item1 - panel.Center.Item1) > 0 && (x.Center.Item1 - panel.Center.Item1) < (BlocksValues.Width + 55) && (x.Center.Item2 - panel.Center.Item2) > 0 && (x.Center.Item2 - panel.Center.Item2) <= (BlocksValues.Height + 8.75)));
                    IEnumerable<EcoPanel> return_PB = PanelList.Where(x => ((x.Center.Item1 - panel.Center.Item1) > 0 && (x.Center.Item1 - panel.Center.Item1) < (BlocksValues.Width + 55) && (panel.Center.Item2 - x.Center.Item2) > 0 && (panel.Center.Item2 - x.Center.Item2) < (BlocksValues.Height + 8.75)));
                    if ((return_PB.Count() != 0) && (return_PA.Count() != 0))
                    {
                        panel.TrueE2Wcol_PORT = return_PA.First().TrueE2Wcol_PORT + 1;
                    }
                    else
                    {
                        panel.TrueE2Wcol_PORT = panel.ColumnNumberE2W_PORT;
                    }
                }
                else if (panel.ColumnNumberE2W_PORT > 1 && panel.ColumnNumberE2W_PORT <= 10)
                {
                    IEnumerable<EcoPanel> return_PA = PanelList.Where(x => ((Math.Abs(panel.Center.Item1 + (BlocksValues.Width + .5) - x.Center.Item1) < .5) && (Math.Abs(panel.Center.Item2 - x.Center.Item2) < .5)));
                    panel.TrueE2Wcol_PORT = return_PA.First().TrueE2Wcol_PORT + 1;
                }
                else
                {
                    panel.TrueE2Wcol_PORT = 11;
                }
            }
        }
        private void Set_W2ETruCol_LAND(List<EcoPanel> PanelList)
        {
            foreach (var panel in PanelList)
            {
                if (panel.ColumnNumberW2E_LAND == 1)
                {
                    IEnumerable<EcoPanel> return_PA = PanelList.Where(x => ((panel.Center.Item1 - x.Center.Item1) > 0 && (panel.Center.Item1 - x.Center.Item1) < (BlocksValues.Width + 55) && (x.Center.Item2 - panel.Center.Item2) > 0 && (x.Center.Item2 - panel.Center.Item2) <= (BlocksValues.Height + 8.75)));
                    IEnumerable<EcoPanel> return_PB = PanelList.Where(x => ((panel.Center.Item1 - x.Center.Item1) > 0 && (panel.Center.Item1 - x.Center.Item1) < (BlocksValues.Width + 55) && (panel.Center.Item2 - x.Center.Item2) > 0 && (panel.Center.Item2 - x.Center.Item2) < (BlocksValues.Height + 8.75)));
                    if ((return_PB.Count() != 0) && (return_PA.Count() != 0))
                    {

                        panel.TrueW2Ecol_LAND = return_PA.First().TrueW2Ecol_LAND + 1;
                    }
                    else
                    {
                        panel.TrueW2Ecol_LAND = panel.ColumnNumberW2E_LAND;
                    }
                }
                else if (panel.ColumnNumberW2E_LAND > 1 && panel.ColumnNumberW2E_LAND <= 4)
                {
                    IEnumerable<EcoPanel> return_PA = PanelList.Where(x => ((Math.Abs(panel.Center.Item1 - (BlocksValues.Width + .5) - x.Center.Item1) < .5) && (Math.Abs(panel.Center.Item2 - x.Center.Item2) < .5)));
                    panel.TrueW2Ecol_LAND = return_PA.First().TrueW2Ecol_LAND + 1;
                }
                else
                {
                    panel.TrueW2Ecol_LAND = 5;
                }
            }
        }
        private void Set_W2ETruCol_PORT(List<EcoPanel> PanelList)
        {
            foreach (var panel in PanelList)
            {
                if (panel.ColumnNumberW2E_PORT == 1)
                {
                    IEnumerable<EcoPanel> return_PA = PanelList.Where(x => ((panel.Center.Item1 - x.Center.Item1) > 0 && (panel.Center.Item1 - x.Center.Item1) < (BlocksValues.Width + 55) && (x.Center.Item2 - panel.Center.Item2) > 0 && (x.Center.Item2 - panel.Center.Item2) <= (BlocksValues.Height + 8.75)));
                    IEnumerable<EcoPanel> return_PB = PanelList.Where(x => ((panel.Center.Item1 - x.Center.Item1) > 0 && (panel.Center.Item1 - x.Center.Item1) < (BlocksValues.Width + 55) && (panel.Center.Item2 - x.Center.Item2) > 0 && (panel.Center.Item2 - x.Center.Item2) < (BlocksValues.Height + 8.75)));
                    if ((return_PB.Count() != 0) && (return_PA.Count() != 0))
                    {
                        panel.TrueW2Ecol_PORT = return_PA.First().TrueW2Ecol_PORT + 1;
                    }
                    else
                    {
                        panel.TrueW2Ecol_PORT = panel.ColumnNumberW2E_PORT;
                    }
                }
                else if (panel.ColumnNumberW2E_PORT > 1 && panel.ColumnNumberW2E_PORT <= 10)
                {
                    IEnumerable<EcoPanel> return_PA = PanelList.Where(x => ((Math.Abs(panel.Center.Item1 - (BlocksValues.Width + .5) - x.Center.Item1) < .5) && (Math.Abs(panel.Center.Item2 - x.Center.Item2) < .5)));
                    panel.TrueW2Ecol_PORT = return_PA.First().TrueW2Ecol_PORT + 1;
                }
                else
                {
                    panel.TrueW2Ecol_PORT = 11;
                }
            }
        }
        private void E2WLandCheck(List<EcoPanel> PanelList)
        {
            foreach (var panel in PanelList)
            {
                if (panel.TrueE2Wcol_LAND >= 1 && panel.TrueE2Wcol_LAND <= 4)
                    panel.IFI_E2W_Land = 1;
                else
                    panel.IFI_E2W_Land = 2;
            }
        }
        private void E2WPortCheck(List<EcoPanel> PanelList)
        {
            foreach (var panel in PanelList)
            {
                if (panel.TrueE2Wcol_PORT >= 1 && panel.TrueE2Wcol_PORT <= 10)
                    panel.IFI_E2W_Port = 1;
                else
                    panel.IFI_E2W_Port = 2;
            }
        }
        private void W2ELandCheck(List<EcoPanel> PanelList)
        {
            foreach (var panel in PanelList)
            {
                if (panel.TrueW2Ecol_LAND >= 1 && panel.TrueW2Ecol_LAND <= 4)
                    panel.IFI_W2E_Land = 1;
                else
                    panel.IFI_W2E_Land = 2;
            }
        }
        private void W2EPortCheck(List<EcoPanel> PanelList)
        {
            foreach (var panel in PanelList)
            {
                if (panel.TrueW2Ecol_PORT >= 1 && panel.TrueW2Ecol_PORT <= 10)
                    panel.IFI_W2E_Port = 1;
                else
                    panel.IFI_W2E_Port = 2;
            }
        }
        private void E2W_LAND_COL_SET(EcoPanel EcoPanel)
        {
            var x_start = EcoPanel.Center.Item1;
            var y_start = EcoPanel.Center.Item2;
            int direction = 0;
            int input_n = 4;
            EcoPanel.ColumnNumberE2W_LAND = 0;

            List<EcoPanel> neighborhood = new List<EcoPanel>();
            // classification of module position; 0 = east edge, 1 = cols 2-4 from edge, 2 = cols >= 5 from edge

            for (int n = 0; n <= input_n; n++)
            {
                var return_neighbor = GenerateNeighbor(n, x_start, y_start, direction);
                List<EcoPanel> temp_neighbor = PanelList.Where(x => ((Math.Abs(x.Center.Item1 - return_neighbor.Item1) <= .5) && (Math.Abs(x.Center.Item2 - return_neighbor.Item2) <= .5))).ToList();
                if (temp_neighbor.Count != 0)
                {
                    if (!neighborhood.Contains(temp_neighbor[0]))
                    {
                        neighborhood.Add(temp_neighbor[0]);
                        EcoPanel.ColumnNumberE2W_LAND = EcoPanel.ColumnNumberE2W_LAND + 1;
                    }
                }
                else
                {
                    break;
                }
            }

        }
        private void E2W_PORT_COL_SET(EcoPanel EcoPanel)
        {
            var x_start = EcoPanel.Center.Item1;
            var y_start = EcoPanel.Center.Item2;
            int direction = 0;
            int input_n = 10;
            EcoPanel.ColumnNumberE2W_PORT = 0;

            List<EcoPanel> neighborhood = new List<EcoPanel>();
            // classification of module position; 0 = east edge, 1 = cols 2-4 from edge, 2 = cols >= 5 from edge

            for (int n = 0; n <= input_n; n++)
            {
                var return_neighbor = GenerateNeighbor(n, x_start, y_start, direction);
                List<EcoPanel> temp_neighbor = PanelList.Where(x => ((Math.Abs(x.Center.Item1 - return_neighbor.Item1) <= .5) && (Math.Abs(x.Center.Item2 - return_neighbor.Item2) <= .5))).ToList();
                if (temp_neighbor.Count != 0)
                {
                    if (!neighborhood.Contains(temp_neighbor[0]))
                    {
                        neighborhood.Add(temp_neighbor[0]);
                        EcoPanel.ColumnNumberE2W_PORT = EcoPanel.ColumnNumberE2W_PORT + 1;
                    }
                }
                else
                {
                    break;
                }
            }

        }
        private void N_LAND_Check(EcoPanel EcoPanel)
        {
            // classification of module position; 0 = north edge, 1 = rows 2-6 from edge, 2 = rows >= 7 from edge
            var x_start = EcoPanel.Center.Item1;
            var y_start = EcoPanel.Center.Item2;
            int direction = 1;
            int input_n = 6;
            List<EcoPanel> neighborhood = new List<EcoPanel>();
            for (int n = 0; n <= input_n; n++)
            {
                var return_neighbor = GenerateNeighbor(n, x_start, y_start, direction);
                List<EcoPanel> temp_neighbor = PanelList.Where(x => ((Math.Abs(x.Center.Item1 - return_neighbor.Item1) <= .5) && (Math.Abs(x.Center.Item2 - return_neighbor.Item2) <= .5))).ToList();
                if (temp_neighbor.Count != 0)
                {
                    if (!neighborhood.Contains(temp_neighbor[0]))
                    {
                        neighborhood.Add(temp_neighbor[0]);
                    }
                }
                else
                {
                    break;
                }
            }
            if (neighborhood.Count == 1)
            {
                EcoPanel.IFI_NORTH_Land = 0;
            }
            else if ((neighborhood.Count >= 2) && (neighborhood.Count <= 6))
            {
                EcoPanel.IFI_NORTH_Land = 1;
            }
            else
            {
                EcoPanel.IFI_NORTH_Land = 2;
            }
            EcoPanel.N2S = neighborhood.OrderBy(x => x.Center.Item1).ToList();

        }
        private void N_PORT_Check(EcoPanel EcoPanel)
        {
            var x_start = EcoPanel.Center.Item1;
            var y_start = EcoPanel.Center.Item2;
            int direction = 1;
            int input_n = 4;
            List<EcoPanel> neighborhood = new List<EcoPanel>();
            // classification of module position; 0 = east edge, 1 = cols 2-4 from edge, 2 = cols >= 5 from edge
            for (int n = 0; n <= input_n; n++)
            {
                var return_neighbor = GenerateNeighbor(n, x_start, y_start, direction);
                List<EcoPanel> temp_neighbor = PanelList.Where(x => ((Math.Abs(x.Center.Item1 - return_neighbor.Item1) <= .5) && (Math.Abs(x.Center.Item2 - return_neighbor.Item2) <= .5))).ToList();
                if (temp_neighbor.Count != 0)
                {
                    if (!neighborhood.Contains(temp_neighbor[0]))
                    {
                        neighborhood.Add(temp_neighbor[0]);
                    }
                }
                else
                {
                    break;
                }
            }
            if (neighborhood.Count == 1)
            {
                EcoPanel.IFI_NORTH_Port = 0;
            }
            else if ((neighborhood.Count >= 2) && (neighborhood.Count <= 4))
            {
                EcoPanel.IFI_NORTH_Port = 1;
            }
            else
            {
                EcoPanel.IFI_NORTH_Port = 2;
            }
            EcoPanel.N2S = neighborhood.OrderBy(x => x.Center.Item1).ToList();

        }
        private void S_PORT_Check(EcoPanel EcoPanel)
        {
            var x_start = EcoPanel.Center.Item1;
            var y_start = EcoPanel.Center.Item2;
            int direction = 2;
            int input_n = 0;
            List<EcoPanel> neighborhood = new List<EcoPanel>();
            // classification of module position; 0 = east edge, 1 = cols 2-4 from edge, 2 = cols >= 5 from edge
            {
                int n = input_n;
                var return_neighbor = GenerateNeighbor(n, x_start, y_start, direction);
                List<EcoPanel> temp_neighbor = PanelList.Where(x => ((Math.Abs(x.Center.Item1 - return_neighbor.Item1) <= .5) && (Math.Abs(x.Center.Item2 - return_neighbor.Item2) <= .5))).ToList();
                if (temp_neighbor.Count != 0)
                {
                    if (!neighborhood.Contains(temp_neighbor[0]))
                    {
                        neighborhood.Add(temp_neighbor[0]);
                    }
                }

            }
            if (neighborhood.Count == 1)
            {
                EcoPanel.IFI_SOUTH_Port = 1;
            }
            else
            {
                EcoPanel.IFI_SOUTH_Port = 0;
            }
        }
        private void S_LAND_Check(EcoPanel EcoPanel)
        {
            var x_start = EcoPanel.Center.Item1;
            var y_start = EcoPanel.Center.Item2;
            int direction = 2;
            int input_n = 0;
            List<EcoPanel> neighborhood = new List<EcoPanel>();
            // classification of module position; 0 = east edge, 1 = cols 2-4 from edge, 2 = cols >= 5 from edge
            {
                int n = input_n;
                var return_neighbor = GenerateNeighbor(n, x_start, y_start, direction);
                List<EcoPanel> temp_neighbor = PanelList.Where(x => ((Math.Abs(x.Center.Item1 - return_neighbor.Item1) <= .5) && (Math.Abs(x.Center.Item2 - return_neighbor.Item2) <= .5))).ToList();
                if (temp_neighbor.Count != 0)
                {
                    if (!neighborhood.Contains(temp_neighbor[0]))
                    {
                        neighborhood.Add(temp_neighbor[0]);

                    }
                }
            }
            if (neighborhood.Count == 1)
            {
                EcoPanel.IFI_SOUTH_Land = 1;
            }
            else
            {
                EcoPanel.IFI_SOUTH_Land = 0;
            }
        }
        private void W2E_LAND_Col_Set(EcoPanel EcoPanel)
        {
            var x_start = EcoPanel.Center.Item1;
            var y_start = EcoPanel.Center.Item2;
            int direction = 3;
            int input_n = 4;
            EcoPanel.ColumnNumberW2E_LAND = 0;

            List<EcoPanel> neighborhood = new List<EcoPanel>();
            // classification of module position; 0 = east edge, 1 = cols 2-4 from edge, 2 = cols >= 5 from edge
            for (int n = 0; n <= input_n; n++)
            {
                var return_neighbor = GenerateNeighbor(n, x_start, y_start, direction);
                List<EcoPanel> temp_neighbor = PanelList.Where(x => ((Math.Abs(x.Center.Item1 - return_neighbor.Item1) <= .5) && (Math.Abs(x.Center.Item2 - return_neighbor.Item2) <= .5))).ToList();
                if (temp_neighbor.Count != 0)
                {
                    if (!neighborhood.Contains(temp_neighbor[0]))
                    {
                        neighborhood.Add(temp_neighbor[0]);
                        EcoPanel.ColumnNumberW2E_LAND = EcoPanel.ColumnNumberW2E_LAND + 1;
                    }
                }
                else
                {
                    break;
                }
            }

        }
        private void W2E_PORT_Col_Set(EcoPanel EcoPanel)
        {
            var x_start = EcoPanel.Center.Item1;
            var y_start = EcoPanel.Center.Item2;
            int direction = 3;
            int input_n = 10;
            EcoPanel.ColumnNumberW2E_PORT = 0;

            List<EcoPanel> neighborhood = new List<EcoPanel>();
            // classification of module position; 0 = east edge, 1 = cols 2-4 from edge, 2 = cols >= 5 from edge
            for (int n = 0; n <= input_n; n++)
            {
                var return_neighbor = GenerateNeighbor(n, x_start, y_start, direction);
                List<EcoPanel> temp_neighbor = PanelList.Where(x => ((Math.Abs(x.Center.Item1 - return_neighbor.Item1) <= .5) && (Math.Abs(x.Center.Item2 - return_neighbor.Item2) <= .5))).ToList();
                if (temp_neighbor.Count != 0)
                {
                    if (!neighborhood.Contains(temp_neighbor[0]))
                    {
                        neighborhood.Add(temp_neighbor[0]);
                        EcoPanel.ColumnNumberW2E_PORT = EcoPanel.ColumnNumberW2E_PORT + 1;
                    }
                }
                else
                {
                    break;
                }
            }
        }
        private int GetUpliftValue(EcoPanel EcoPanel)
        {
            var x_start = EcoPanel.Center.Item1;
            var y_start = EcoPanel.Center.Item2;
            int input_n = 2;
            List<Tuple<double, double, int>> neighborhood = new List<Tuple<double, double, int>>();
            for (int x = 0; x <= input_n; x++)
            {
                for (int y = 0; y <= input_n; y++)
                {
                    for (int i = -1; i <= 1; i += 2)
                    {
                        if (x.Equals(0))
                            i = 1;
                        for (int j = -1; j <= 1; j += 2)
                        {
                            if (y.Equals(0))
                                j = 1;
                            {
                                Tuple<double, double, int> temp_neighbor = new Tuple<double, double, int>(x_start + (0.5 + BlocksValues.Width) * i * x, y_start + (17.494 + BlocksValues.Height) * j * y, 4);
                                neighborhood.Add(temp_neighbor);
                            }
                        }
                    }
                }
            }

            int total_count = 0;
            foreach (var neighbor in neighborhood)
            {
                foreach (var x in PanelList)
                {
                    if ((Math.Abs((x.Center.Item1 - neighbor.Item1)) <= .5) && (Math.Abs(neighbor.Item2 - x.Center.Item2) <= .5))
                    {
                        total_count = total_count + 1;
                    }
                }
            }
            return total_count;
        }
        private int GetSlidingValue(EcoPanel EcoPanel)
        {
            var x_start = EcoPanel.Center.Item1;
            var y_start = EcoPanel.Center.Item2;
            int input_n = 6;
            List<Tuple<double, double, int>> neighborhood = new List<Tuple<double, double, int>>();
            for (int x = 0; x <= input_n; x++)
            {
                for (int y = 0; y <= input_n; y++)
                {
                    for (int i = -1; i <= 1; i += 2)
                    {
                        if (x.Equals(0))
                            i = 1;
                        for (int j = -1; j <= 1; j += 2)
                        {
                            if (y.Equals(0))
                                j = 1;
                            {
                                Tuple<double, double, int> temp_neighbor = new Tuple<double, double, int>(x_start + (0.5 + BlocksValues.Width) * i * x, y_start + (17.494 + BlocksValues.Height) * j * y, 4);
                                neighborhood.Add(temp_neighbor);
                            }
                        }
                    }
                }
            }

            int total_count = 0;
            foreach (var neighbor in neighborhood)
            {
                foreach (var x in PanelList)
                {
                    if ((Math.Abs((x.Center.Item1 - neighbor.Item1)) <= .5) && (Math.Abs(neighbor.Item2 - x.Center.Item2) <= .5))
                    {
                        total_count = total_count + 1;
                    }
                }
            }
            return total_count;
        }
        public void RunIFILocationChecks()
        {
            foreach (var EcoPanel in PanelList)
            {
                E2W_LAND_COL_SET(EcoPanel);
                E2W_PORT_COL_SET(EcoPanel);
                N_LAND_Check(EcoPanel);
                N_PORT_Check(EcoPanel);
                S_PORT_Check(EcoPanel);
                S_LAND_Check(EcoPanel);
                W2E_LAND_Col_Set(EcoPanel);
                W2E_PORT_Col_Set(EcoPanel);
            }
            {
                PanelList = PanelList.OrderByDescending(x => x.Center.Item1).ToList();  // sort panel List largest to smallest x value
                Set_E2WTruCol_LAND(PanelList);
                Set_E2WTruCol_PORT(PanelList);
                E2WPortCheck(PanelList);
                E2WLandCheck(PanelList);

                PanelList = PanelList.OrderBy(x => x.Center.Item1).ToList();            // sort panel list smallest to larges x value
                Set_W2ETruCol_LAND(PanelList);
                Set_W2ETruCol_PORT(PanelList);
                W2ELandCheck(PanelList);
                W2EPortCheck(PanelList);
            }
            PanelList = PanelList.OrderBy(x => x.Center.Item1).ThenByDescending(x =>x.Center.Item2).ToList(); // organizes panel list for easier console output and debug (columns left to right, top to bottom of each column)
        }
        public List<EcoPanel> GetPanels()
        {
            return PanelList;
        }
        public List<Base> GetPanelBases()
        {
            return PanelBaseList;
        }
        public void PrintPanelData()
        {
            Console.WriteLine("EcoPanel/Entities Values:");
            Console.WriteLine("======================");
            foreach (var x in PanelList)
            {
                Console.WriteLine("EcoPanel Number {0}: ", x.PanelID);
                Console.WriteLine("X value: {0}", x.Xvalue.ToString());
                Console.WriteLine("Y Value: {0} ", x.Yvalue.ToString());
                Console.WriteLine("Center Value: {0}", x.Center.ToString());
                Console.WriteLine("North East Zone: {0}", x.NE_Zone.ToString());
                Console.WriteLine("North West Zone: {0}", x.NW_Zone.ToString());
                Console.WriteLine("IFI Location Variables: ");
                Console.WriteLine("W2E Port Position: {0}", x.IFI_W2E_Port);
                Console.WriteLine("W2E Land Position: {0}", x.IFI_W2E_Land);
                Console.WriteLine("E2W Port Position: {0}", x.IFI_E2W_Port);
                Console.WriteLine("E2W Land Position: {0}", x.IFI_E2W_Land);
                Console.WriteLine("North Land Position: {0}", x.IFI_NORTH_Land);
                Console.WriteLine("South Land Position: {0}", x.IFI_SOUTH_Land);
                Console.WriteLine("Ballast Location: {0}", x.BallastLocation);
                Console.WriteLine("Direction List: ");
                Console.WriteLine("=================");
                foreach (var d in x.DirectionList)
                {
                    Console.WriteLine(d);
                }
                Console.WriteLine("=================");
                Console.WriteLine("\n");
                Console.ReadKey();
            }
            return;
        }
        //private void CalculateBlockTotalValues(Base base_panel)
        //{
        //    double IFI_Base_Total = 0;
        //    foreach (double cornerValue in base_panel.ContributionList)
        //    {
        //        IFI_Base_Total += cornerValue; //IFI_Base_Total
        //    }
        //    base_panel.BallastBlockValue = Convert.ToInt32(Math.Ceiling(((IFI_Base_Total) / base_panel.BlockWeight) - .03));
        //}


        //private void CalculatePanelCorners(EcoPanel EcoPanel)
        //{
        //    var x_start = EcoPanel.Center.Item1;
        //    var y_start = EcoPanel.Center.Item2;
        //    Tuple<double, double> corner_neighbor_east = new Tuple<double, double>(x_start - .5 * (.5 + BlocksValues.Width), y_start + .5 * (17.494 + BlocksValues.Height)); //NWest
        //    Tuple<double, double> corner_neighbor_north = new Tuple<double, double>(x_start + .5 * (.5 + BlocksValues.Width), y_start + (17.494 + BlocksValues.Height));//NEast
        //    Tuple<double, double> corner_neighbor_south = new Tuple<double, double>(x_start + .5 * (.5 + BlocksValues.Width), y_start - (17.494 - BlocksValues.Height)); //SEast
        //    Tuple<double, double> corner_neighbor_west = new Tuple<double, double>(x_start - .5 * (.5 + BlocksValues.Width), y_start - (17.494 - BlocksValues.Height)); //SWest
        //    List<Tuple<double, double>> temp_list = new List<Tuple<double, double>>();
        //    temp_list.Add(corner_neighbor_east);
        //    temp_list.Add(corner_neighbor_west);
        //    temp_list.Add(corner_neighbor_north);
        //    temp_list.Add(corner_neighbor_south);
        //    Random rand = new Random();
        //    string random_number = Convert.ToString(rand.Next(0, 10));
        //    for (int x = 0; x < temp_list.Count; x++)
        //    {
        //        //Console.WriteLine(temp_list[x]); 
        //        List<Base> matching_bases = PanelBaseList.Where(c => c.Center.Item1 == temp_list[x].Item1 && c.Center.Item2 == temp_list[x].Item2).ToList();
        //        if (matching_bases.Count() != 0)
        //        {
        //            foreach (Base pb in matching_bases)
        //            {
        //                pb.PanelIDList.Add(EcoPanel.PanelID);
        //                pb.ContributionList.Add(EcoPanel.ValueFromExcel);
        //            }
        //        }
        //        else
        //        {
        //            Base temp = new Base(PanelBaseList.Count.ToString(), EcoPanel.BallastLocation, temp_list[x], EcoPanel.ValueFromExcel);
        //            PanelBaseList.Add(temp);
        //        }


        //public void CalculateNeighbors(int input_n)
        //{
        //    foreach (EcoPanel EcoPanel in PanelList)
        //    {
        //        int direction = 4;
        //        List<Tuple<double, double, int>> neighborhood = GenerateNeighborhood(input_n, EcoPanel.Center.Item1, EcoPanel.Center.Item2, direction);
        //        foreach (var neighbor in neighborhood)
        //        {
        //            foreach (var x in PanelList)
        //            {
        //                if ((Math.Abs(neighbor.Item1 - x.Center.Item1) <= .5) && (Math.Abs(neighbor.Item2 - x.Center.Item2) <= .5))
        //                {
        //                    EcoPanel.NeighborHood.Add(x);
        //                    EcoPanel.DirectionList.Add(neighbor.Item3);
        //                }
        //            }
        //        }
        //        IEnumerable<int> values = EcoPanel.DirectionList.Cast<int>().Distinct();
        //        EcoPanel.DirectionList = values.ToList<int>();
        //    }
        //}


        //KB DEBUG: added back in original neighborhood calculation for n=2 and n=4 calculations (lift and sliding)
        //private List<EcoPanel> CountNeighbors(List<Tuple<double, double, int>> neighborhood)
        //{
        //    List<EcoPanel> temp_neighbors = new List<EcoPanel>(); 
        //    int count = 0;
        //    foreach (var neighbor in neighborhood)
        //    {
        //        foreach (var EcoPanel in PanelList)
        //        {
        //            if ((Math.Abs(neighbor.Item1 - EcoPanel.Center.Item1) <= .5) && (Math.Abs(neighbor.Item2 - EcoPanel.Center.Item2) <= .5))
        //            {
        //                temp_neighbors.Add(EcoPanel);
        //            }
        //            else
        //            {
        //                return temp_neighbors; 
        //            }
        //        }
        //    }
        //    return temp_neighbors;
        //}


        //private void RunBasePanelCalculations()
        //{
        //    Console.WriteLine(" Base EcoPanel Calculations in PanelGrid Class: ");
        //    foreach (EcoPanel EcoPanel in PanelList)
        //    {
        //        CalculatePanelCorners(EcoPanel);
        //    }
        //    //foreach (Base pb in PanelBaseList)
        //    //{
        //    //    CalculateBlockTotalValues(pb);
        //    //}
        //}


        //private void SetSortedPanels()
        //{   // Sort 
        //    X_direction = PanelBaseList.OrderBy(c => c.Center.Item1).ToList();
        //    //if 2 ignore 
        //    //Y_direction = PanelList.OrderBy(c => c.Center.Item2).ToList();
        //    // look to left and right of panel neighbors 
        //    // if 2 
        //    // switch 2 
        //    // R -> L 
        //    // if edge 0
        //    // if 1 check 
        //    // if center 0 and close 2 
        //    // change 
        //    // L -> R 
        //    // opposite direction 
        //    //if panel edge Right 1 (could be 2 or 1) 
        //    // check on reference 
        //    // i
        //    return;
        //}


        //public void SetInitialColumns()
        //{
        //  if
        //    RunE2WCheck();
        //    RunW2ECheck(); 
        //}
        //public void RunE2WCheck()
        //{
        //    foreach( var panel in PanelList)
        //    {
        //        if(Landscape)
        //        {
        //        }
        //        else
        //        {
        //        }
        //    }
        //}


        //public void CheckForCenterEdge(EcoPanel panel)
        //{
        //}

    }
}