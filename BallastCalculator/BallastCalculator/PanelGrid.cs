using System;
using System.Collections.Generic;
using System.Linq;
using Dimensions;


namespace BallastCalculator
{

    class PanelGrid
    {
        private readonly BasicDimensions BlocksValues;
        private List<EcoPanel> PanelList;
        private List<PanelBase> PanelBaseList = new List<PanelBase>();
        private List<double> list = new List<double>{ 155.573628951034,90.3798510480827,
                109.436766290018,
                108.672042183518,
                99.9250064931934,
                89.5751514696727,
                79.4525671971918,
                133.959639797999,
                75.0400248738035,
                95.8335446926647,
                97.2094696723151,
                87.1839679312824,
                74.2970806754946,
                66.560550033073,
                77.9123674347747,
                115.083674833155,
                125.417775213692,
                118.060996242743,
                66.560550033073,
                95.8335446926647,
                97.2094696723151,
                87.1839679312824,
                74.2970806754946,
                66.560550033073,
                71.8378419830429 };
        public Random rand = new Random();
        public void SetExcelValues()
        {
            foreach (var x in PanelList)
            {
                x.ValueFromExcel = Convert.ToDouble(rand.Next(Convert.ToInt32(list.Min()),Convert.ToInt32(list.Max()))); 
            }
        }



        private bool Landscape;
        public PanelGrid(BasicDimensions perimeter, List<EcoPanel> plist) // Called First 
        {
            BlocksValues = perimeter;
            PanelList = plist;
            RunPanelCalculations(); // Generates Call -> Program Now in Run EcoPanel Calculations Function
            BuildDirectionList();
            CalculateBallastLocation();
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
            foreach(PanelBase pb in PanelBaseList)
            {
                CalculateBlockTotalValues(pb);
            }
        }
        //Called By Constructor
        private void CalculatePanelCorners(EcoPanel EcoPanel)
        {
            var x_start = EcoPanel.Center.Item1;
            var y_start = EcoPanel.Center.Item2;
            Tuple<double, double> corner_neighbor_east = new Tuple<double, double>(x_start - .5 * (.5 + BlocksValues.Width), y_start + .5 * (17.494 + BlocksValues.Height)); //NWest
            Tuple<double, double> corner_neighbor_north = new Tuple<double, double>(x_start + .5 * (.5 + BlocksValues.Width), y_start + .5 * (17.494 + BlocksValues.Height));//NEast
            Tuple<double, double> corner_neighbor_south = new Tuple<double, double>(x_start + .5 * (.5 + BlocksValues.Width), y_start - .5 * (17.494 - BlocksValues.Height)); //SEast
            Tuple<double, double> corner_neighbor_west = new Tuple<double, double>(x_start - .5 * (.5 + BlocksValues.Width), y_start - .5 * (17.494 - BlocksValues.Height)); //SWest
            List<Tuple<double, double>> temp_list = new List<Tuple<double, double>>();
            temp_list.Add(corner_neighbor_east);
            temp_list.Add(corner_neighbor_west);
            temp_list.Add(corner_neighbor_north);
            temp_list.Add(corner_neighbor_south);
          
            for (int x = 0; x < temp_list.Count; x++)
            {
                //Console.WriteLine(temp_list[x]); 
                List<PanelBase> matching_bases = PanelBaseList.Where(c => Math.Abs(c.Center.Item1 - temp_list[x].Item1) < .5 && Math.Abs(c.Center.Item2 -temp_list[x].Item2) < .5 ).ToList();
                List<EcoPanel> matching_panels = PanelList.Where(c => Math.Abs(c.Center.Item1 - temp_list[x].Item1) < .5 && Math.Abs(c.Center.Item2 - temp_list[x].Item2) < .5).ToList();
                if (matching_bases.Count() == 0)
                {
                    PanelBase temp = new PanelBase(PanelBaseList.Count.ToString(), EcoPanel.BallastLocation, temp_list[x], EcoPanel.ValueFromExcel);
                    temp.BlockWeight = EcoPanel.ValueFromExcel;
                    temp.BlockWeightList.Add(EcoPanel.ValueFromExcel);
                    PanelBaseList.Add(temp);
                }
                //if(matching_panels.Count() != 0)
                //{
                //    foreach (var i in matching_panels)
                //    {
                //        i.BlockWeightList.Add(i.ValueFromExcel);
                //    }

                //}
                else
                {
                    //Console.WriteLine(EcoPanel.ValueFromExcel);
                    //Console.ReadKey();
                   


                }

            }
        }
        private void CalculateBlockTotalValues(PanelBase base_panel)
        {
            double IFI_Base_Total = 0;
            foreach (double cornerValue in base_panel.BlockWeightList)
            {
                IFI_Base_Total += cornerValue; //IFI_Base_Total

            }
            base_panel.BlockTotal = Convert.ToInt32(Math.Ceiling(((IFI_Base_Total) / base_panel.BlockWeight) - .03));

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
                for (int i = 0; i <= 3; i++)
                {
                    var temp = GenerateNeighbor(1, panel.Center.Item1, panel.Center.Item2, i);
                    if (PanelList.Any(x => (Math.Abs(x.Center.Item1 - temp.Item1) <= .5) && (Math.Abs(x.Center.Item2 - temp.Item2) <= .5)))
                    {
                        if (!panel.DirectionList.Contains(i))
                            panel.DirectionList.Add(i);
                    }

                }
            }

        }
        public void CalculateBallastLocation()
        {
            foreach (EcoPanel EcoPanel in PanelList)
            {
                if (EcoPanel.DirectionList.Exists(x => x.Equals(3) && x.Equals(1)) && !EcoPanel.DirectionList.Exists(x => x.Equals(2) && x.Equals(0)))
                {
                    EcoPanel.BallastLocation = 9;
                    EcoPanel.isEdge = true;
                }

                else if (EcoPanel.DirectionList.Exists(x => x.Equals(1) && x.Equals(0)) && !EcoPanel.DirectionList.Exists(x => x.Equals(3) && x.Equals(2)))
                {
                    EcoPanel.BallastLocation = 7;
                    EcoPanel.isEdge = true;
                }

                else if (EcoPanel.DirectionList.Exists(x => x.Equals(2) && x.Equals(0)) && !EcoPanel.DirectionList.Exists(x => x.Equals(3) && x.Equals(1)))
                {
                    EcoPanel.BallastLocation = 1;
                    EcoPanel.isEdge = true;
                }

                else if (EcoPanel.DirectionList.Exists(x => x.Equals(2) && x.Equals(3)) && !EcoPanel.DirectionList.Exists(x => x.Equals(0) && x.Equals(1)))
                {
                    EcoPanel.BallastLocation = 3;
                    EcoPanel.isEdge = true;
                }

                else if (EcoPanel.DirectionList.Exists(x => x.Equals(1) && x.Equals(3) && x.Equals(0)) && !EcoPanel.DirectionList.Exists(x => x.Equals(2)))
                    EcoPanel.BallastLocation = 8;

                else if (EcoPanel.DirectionList.Exists(x => x.Equals(2) && x.Equals(3) && x.Equals(0)) && !EcoPanel.DirectionList.Exists(x => x.Equals(1)))
                    EcoPanel.BallastLocation = 2;

                else if (EcoPanel.DirectionList.Exists(x => x.Equals(2) && x.Equals(1) && x.Equals(0)) && !EcoPanel.DirectionList.Exists(x => x.Equals(3)))
                {
                    EcoPanel.BallastLocation = 4;
                    EcoPanel.isEdge = true;
                }

                else if (EcoPanel.DirectionList.Exists(x => x.Equals(2) && x.Equals(3) && x.Equals(1)) && !EcoPanel.DirectionList.Exists(x => x.Equals(0)))
                {
                    EcoPanel.BallastLocation = 6;
                    EcoPanel.isEdge = true;
                }

                else if (EcoPanel.DirectionList.Exists(x => x.Equals(2) && x.Equals(3) && x.Equals(0) && x.Equals(1)))
                {
                    EcoPanel.BallastLocation = 5;
                }

                else
                {
                    //Console.WriteLine("ERROR! A module appears to be part of a single row or column. Please revise layout to remove single rows or columns when using EF3.");
                    //Console.ReadKey();
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
            int input_n = 4;
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
        public List<PanelBase> GetPanelBases()
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
        //private void CalculateBlockTotalValues(PanelBase base_panel)
        //{
        //    double IFI_Base_Total = 0;
        //    foreach (double cornerValue in base_panel.BlockWeightList)
        //    {
        //        IFI_Base_Total += cornerValue; //IFI_Base_Total
        //    }
        //    base_panel.BlockTotal = Convert.ToInt32(Math.Ceiling(((IFI_Base_Total) / base_panel.BlockWeight) - .03));
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
        //        List<PanelBase> matching_bases = PanelBaseList.Where(c => c.Center.Item1 == temp_list[x].Item1 && c.Center.Item2 == temp_list[x].Item2).ToList();
        //        if (matching_bases.Count() != 0)
        //        {
        //            foreach (PanelBase pb in matching_bases)
        //            {
        //                pb.PanelIDList.Add(EcoPanel.PanelID);
        //                pb.BlockWeightList.Add(EcoPanel.ValueFromExcel);
        //            }
        //        }
        //        else
        //        {
        //            PanelBase temp = new PanelBase(PanelBaseList.Count.ToString(), EcoPanel.BallastLocation, temp_list[x], EcoPanel.ValueFromExcel);
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
        //    //foreach (PanelBase pb in PanelBaseList)
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