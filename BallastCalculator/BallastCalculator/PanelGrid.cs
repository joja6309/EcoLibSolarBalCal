﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dimensions;


namespace BallastCalculator
{

    class PanelGrid 
    {
        private readonly BasicDimensions BlocksValues;
        private List<EcoPanel> PanelList;
        private List<PanelBase> PanelBaseList = new List<PanelBase>();
        private List<PanelBase> X_direction = new List<PanelBase>();
        private bool Landscape; 
        public PanelGrid(BasicDimensions perimeter, List<EcoPanel> plist) // Called First 
        {
            BlocksValues = perimeter;
            PanelList = plist;
            RunPanelCalculations(); // Generates Call -> Program Now in Run EcoPanel Calculations Function
            //RunBasePanelCalculations(); 
        }
        //Called By Constructor
        private void RunPanelCalculations()
        {
            //BlocksValues = perimeter;
            //Console.WriteLine(perimeter.Center.Item1); -> You Can Use this chunk to interate through calulations 
            //Console.WriteLine(perimeter.Center.Item2); -> copy and past to values in different areas to do checks 
            //Console.ReadKey();
            foreach (EcoPanel EcoPanel in PanelList)
            {

                EcoPanel.CalculatePanelCenter(BlocksValues.Center.Item1, BlocksValues.Center.Item2);
                //Console.WriteLine(EcoPanel.Xvalue);
                //Console.WriteLine(EcoPanel.Yvalue);
                //Console.WriteLine(EcoPanel.PanelID);
                //Console.WriteLine(EcoPanel.Center);
                //Console.ReadKey();

            }
            //CalculateNeighbors(1);

            //foreach (EcoPanel EcoPanel in PanelList)
            //{
            //    EcoPanel.Sliding = GetSlidingValue(EcoPanel);
            //}
            //foreach (EcoPanel EcoPanel in PanelList)
            //{
            //    EcoPanel.Uplift = GetUpliftValue(EcoPanel);
            //}
            RunIFILocationChecks();
            //CalculateBallastLocation(); 


        } 
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
        ////public void SetInitialColumns()
        ////{
        ////  if
        ////    RunE2WCheck();
        ////    RunW2ECheck(); 
        ////}
        ////public void RunE2WCheck()
        ////{
        ////    foreach( var panel in PanelList)
        ////    {
        ////        if(Landscape)
        ////        {

        ////        }
        ////        else
        ////        {

        ////        }
        ////    }
        ////}
        //public void CheckForCenterEdge(EcoPanel panel)
        //{


        //}
        //public void TRUE_COLUMN_Check(EcoPanel panel)
        //{   // Sorted Smallest to Largest 

        //    //if(panel.Neighb)
           


        //    //var x_start = panel.Center.Item1;
        //    //var y_start = panel.Center.Item2;
        //    //int direction = 3;
        //    //int input_n = 10;
        //    //List<Tuple<double, double, int>> neighborhood = GenerateNeighborhood(input_n, x_start, y_start, direction);
        //    //var IFI_W2E_Port_count = CountNeighbors(neighborhood);
        //    //Right to left 
        //    // How many to the right 
        //    //Left to right 
        //    // How mant to the left 
        //    //TRUE CHECK 
        //    // if edge look to right 
        //}
    private Tuple<double, double, int> GenerateNeighbor(int n, double x_start, double y_start, int direction)
    {
        Tuple<double, double, int> temp_neighbor = new Tuple<double, double, int>(0,0,0);
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
        private void E2W_LAND_Check(EcoPanel EcoPanel)
        {
            var x_start = EcoPanel.Center.Item1;
            var y_start = EcoPanel.Center.Item2;
            int direction = 0;
            int input_n = 4;
            List<EcoPanel> neighborhood = new List<EcoPanel>();
            // classification of module position; 0 = east edge, 1 = cols 2-4 from edge, 2 = cols >= 5 from edge
             //count of total panels east of a given module until break
            for (int n = 0; n < input_n; n++)
            {
                var return_neighbor  = GenerateNeighbor(input_n, x_start, y_start, direction);
                List<EcoPanel> temp_neighbor = PanelList.Where(x => (Math.Abs(x.Center.Item1 - return_neighbor.Item1) <= .5) && (Math.Abs(x.Center.Item2 - return_neighbor.Item2) <= .5)).ToList();
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
            neighborhood = neighborhood.Distinct().ToList(); 
            if (neighborhood.Count == 0)
            {
                EcoPanel.IFI_E2W_Land = 0;
            }
            else if ((neighborhood.Count >= 1) && (neighborhood.Count<= 3))
            {
                EcoPanel.IFI_E2W_Land = 1;
            }
            else
            {
                EcoPanel.IFI_E2W_Land = 2;
                foreach(var x in neighborhood)
                {
                    Console.WriteLine(x.PanelID);
                    Console.WriteLine(x.Center); 
                    foreach(var r in neighborhood)
                    {
                        Console.WriteLine(r.PanelID);
                        Console.WriteLine(r.Center);
                    }

                }
            }
        }
        private void E2W_PORT_Check(EcoPanel EcoPanel)
        {

            var x_start = EcoPanel.Center.Item1;
            var y_start = EcoPanel.Center.Item2;
            int direction = 0;
            int input_n = 10;
            List<EcoPanel> neighborhood = new List<EcoPanel>();
            // classification of module position; 0 = east edge, 1 = cols 2-4 from edge, 2 = cols >= 5 from edge
            //count of total panels east of a given module until break
            for (int n = 0; n < input_n; n++)
            {
                var return_neighbor = GenerateNeighbor(input_n, x_start, y_start, direction);
                List<EcoPanel> temp_neighbor = PanelList.Where(x => (Math.Abs(x.Center.Item1 - return_neighbor.Item1) <= .5) && (Math.Abs(x.Center.Item2 - return_neighbor.Item2) <= .5)).ToList();
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
            if (neighborhood.Count  == 0)
            {
                EcoPanel.IFI_E2W_Port = 0;
            }
            else if ((neighborhood.Count >= 1) && (neighborhood.Count <= 9))
            {
                EcoPanel.IFI_E2W_Port = 1;
            }
            else
            {
                EcoPanel.IFI_E2W_Port = 2;
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
            // classification of module position; 0 = east edge, 1 = cols 2-4 from edge, 2 = cols >= 5 from edge
            //count of total panels east of a given module until break
            for (int n = 0; n < input_n; n++)
            {
                var return_neighbor = GenerateNeighbor(input_n, x_start, y_start, direction);
                List<EcoPanel> temp_neighbor = PanelList.Where(x => (Math.Abs(x.Center.Item1 - return_neighbor.Item1) <= .5) && (Math.Abs(x.Center.Item2 - return_neighbor.Item2) <= .5)).ToList();
                if (temp_neighbor.Count != 0)
                {
                    if (!neighborhood.Contains(temp_neighbor[0]))
                    {
                        //Console.WriteLine(
                        //neighborhood.Add(temp_neighbor[0]);

                    }
                }
                else
                {
                    break;
                    
                }

            }
            if (neighborhood.Count == 0)
            {
                EcoPanel.IFI_NORTH_Land = 0;
            }
            else if ((neighborhood.Count>= 1) && (neighborhood.Count <= 5))
            {
                EcoPanel.IFI_NORTH_Land = 1;
            }
            else
            {
                EcoPanel.IFI_NORTH_Land = 2;
            }


        }
        private void N_PORT_Check(EcoPanel EcoPanel)
        {

            var x_start = EcoPanel.Center.Item1;
            var y_start = EcoPanel.Center.Item2;
            int direction = 1;
            int input_n = 4;
            List<EcoPanel> neighborhood = new List<EcoPanel>();
            // classification of module position; 0 = east edge, 1 = cols 2-4 from edge, 2 = cols >= 5 from edge
            //count of total panels east of a given module until break
            for (int n = 0; n < input_n; n++)
            {
                var return_neighbor = GenerateNeighbor(input_n, x_start, y_start, direction);
                List<EcoPanel> temp_neighbor = PanelList.Where(x => (Math.Abs(x.Center.Item1 - return_neighbor.Item1) <= .5) && (Math.Abs(x.Center.Item2 - return_neighbor.Item2) <= .5)).ToList();
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

            if (neighborhood.Count == 0)
            {
                EcoPanel.IFI_NORTH_Port = 0;
            }
            else if ((neighborhood.Count >= 1) && (neighborhood.Count <= 3))
            {
                EcoPanel.IFI_NORTH_Port = 2;

            }
            else
            {
                EcoPanel.IFI_NORTH_Port = 2;

            }


        }
        private void S_PORT_Check(EcoPanel EcoPanel)
        {

            var x_start = EcoPanel.Center.Item1;
            var y_start = EcoPanel.Center.Item2;
            int direction = 2;
            int input_n = 0;
            List<EcoPanel> neighborhood = new List<EcoPanel>();
            // classification of module position; 0 = east edge, 1 = cols 2-4 from edge, 2 = cols >= 5 from edge
            //count of total panels east of a given module until break
            for (int n = 0; n < input_n; n++)
            {
                var return_neighbor = GenerateNeighbor(input_n, x_start, y_start, direction);
                List<EcoPanel> temp_neighbor = PanelList.Where(x => (Math.Abs(x.Center.Item1 - return_neighbor.Item1) <= .5) && (Math.Abs(x.Center.Item2 - return_neighbor.Item2) <= .5)).ToList();
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
            //count of total panels east of a given module until break
            for (int n = 0; n < input_n; n++)
            {
                var return_neighbor = GenerateNeighbor(input_n, x_start, y_start, direction);
                List<EcoPanel> temp_neighbor = PanelList.Where(x => (Math.Abs(x.Center.Item1 - return_neighbor.Item1) <= .5) && (Math.Abs(x.Center.Item2 - return_neighbor.Item2) <= .5)).ToList();
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
                EcoPanel.IFI_SOUTH_Land = 1;
            }
            else
            {
                EcoPanel.IFI_SOUTH_Land = 0;

            }
        }
        private void W2E_LAND_Check(EcoPanel EcoPanel)
        {

            var x_start = EcoPanel.Center.Item1;
            var y_start = EcoPanel.Center.Item2;
            int direction = 3;
            int input_n = 4;
            List<EcoPanel> neighborhood = new List<EcoPanel>();
            // classification of module position; 0 = east edge, 1 = cols 2-4 from edge, 2 = cols >= 5 from edge
            //count of total panels east of a given module until break
            for (int n = 0; n < input_n; n++)
            {
                var return_neighbor = GenerateNeighbor(input_n, x_start, y_start, direction);
                List<EcoPanel> temp_neighbor = PanelList.Where(x => (Math.Abs(x.Center.Item1 - return_neighbor.Item1) <= .5) && (Math.Abs(x.Center.Item2 - return_neighbor.Item2) <= .5)).ToList();
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

            if (neighborhood.Count == 0)
            {
                EcoPanel.IFI_W2E_Land = 0;
            }
            else if ((neighborhood.Count >= 1) && (neighborhood.Count <= 3))
            {
                EcoPanel.IFI_W2E_Land = 1;
            }
            else
            {
                EcoPanel.IFI_W2E_Land = 2;
            }
        }
        private void W2E_PORT_Check(EcoPanel EcoPanel)
        {

            var x_start = EcoPanel.Center.Item1;
            var y_start = EcoPanel.Center.Item2;
            int direction = 3;
            int input_n = 10;
            List<EcoPanel> neighborhood = new List<EcoPanel>();
            // classification of module position; 0 = east edge, 1 = cols 2-4 from edge, 2 = cols >= 5 from edge
            //count of total panels east of a given module until break
            for (int n = 0; n < input_n; n++)
            {
                var return_neighbor = GenerateNeighbor(input_n, x_start, y_start, direction);
                List<EcoPanel> temp_neighbor = PanelList.Where(x => (Math.Abs(x.Center.Item1 - return_neighbor.Item1) <= .5) && (Math.Abs(x.Center.Item2 - return_neighbor.Item2) <= .5)).ToList();
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
            // classification of module position; 0 = west edge, 1 = cols 2-4 from west, 2 = cols >= 5 from edge
            if (neighborhood.Count == 0)
            {
                EcoPanel.IFI_W2E_Port = 0;
            }
            else if ((neighborhood.Count >= 1) && (neighborhood.Count <= 9))
            {
                EcoPanel.IFI_W2E_Port = 1;
            }
            else
            {
                EcoPanel.IFI_W2E_Port = 2;
            }
        }
        private int GetUpliftValue(EcoPanel EcoPanel)
        {
            var x_start = EcoPanel.Center.Item1;
            var y_start = EcoPanel.Center.Item2;
            int input_n = 2;
            List<Tuple<double, double, int>> neighborhood = new List<Tuple<double, double, int>>();
            //KB DEBUG: added back in original neighborhood calculation for n=2 and n=4 calculations (lift and sliding)
            //    else if (direction == 4)
            //{
            for (int y = 0; y <= input_n; y++)
            {
                for (int i = -1; i <= 1; i += 2)
                {
                    if (y.Equals(0))
                        i = 1;
                    for (int j = -1; j <= 1; j += 2)
                    {
                        if (y.Equals(0))
                            j = 1;
                        {
                            Tuple<double, double, int> temp_neighbor = new Tuple<double, double, int>(x_start + (0.5 + BlocksValues.Width) * i * y, y_start + (17.494 + BlocksValues.Height) * j * y, 4);
                            neighborhood.Add(temp_neighbor);
                        }
                    }
                }
            }
            //}

            //KB DEBUG: changed the direction to utilize original neighborhood calculation

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
            List<Tuple<double,double,int>> neighborhood = new List<Tuple<double, double, int>>(); 
            //KB DEBUG: added back in original neighborhood calculation for n=2 and n=4 calculations (lift and sliding)
            //    else if (direction == 4)
            //{
            for (int y = 0; y <= input_n; y++)
            {
                for (int i = -1; i <= 1; i += 2)
                {
                    if (y.Equals(0))
                        i = 1;
                    for (int j = -1; j <= 1; j += 2)
                    {
                        if (y.Equals(0))
                            j = 1;
                        {
                            Tuple<double, double, int> temp_neighbor = new Tuple<double, double, int>(x_start + (0.5 + BlocksValues.Width) * i * y, y_start + (17.494 + BlocksValues.Height) * j * y, 4);
                            neighborhood.Add(temp_neighbor);
                        }
                    }
                }
            }
            //}

            //KB DEBUG: changed the direction to utilize original neighborhood calculation

            int total_count = 0; 
                foreach(var neighbor in neighborhood)
                {
                    foreach( var x in PanelList)
                {
                    if  ((Math.Abs((x.Center.Item1 - neighbor.Item1)) <= .5) && (Math.Abs(neighbor.Item2 - x.Center.Item2) <= .5))
                    {
                        total_count = total_count + 1;
                    }
                }
               

                }

                
            return total_count;
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
        private void CalculatePanelCorners(EcoPanel EcoPanel)
        {
            var x_start = EcoPanel.Center.Item1;
            var y_start = EcoPanel.Center.Item2;
            Tuple<double, double> corner_neighbor_east = new Tuple<double, double>(x_start - .5 * (.5 + BlocksValues.Width), y_start + .5 * (17.494 + BlocksValues.Height)); //NWest
            Tuple<double, double> corner_neighbor_north = new Tuple<double, double>(x_start + .5 * (.5 + BlocksValues.Width), y_start + (17.494 + BlocksValues.Height));//NEast
            Tuple<double, double> corner_neighbor_south = new Tuple<double, double>(x_start + .5 * (.5 + BlocksValues.Width), y_start - (17.494 - BlocksValues.Height)); //SEast
            Tuple<double, double> corner_neighbor_west = new Tuple<double, double>(x_start - .5 * (.5 + BlocksValues.Width), y_start - (17.494 - BlocksValues.Height)); //SWest
            List<Tuple<double, double>> temp_list = new List<Tuple<double, double>>();

            temp_list.Add(corner_neighbor_east);
            temp_list.Add(corner_neighbor_west);
            temp_list.Add(corner_neighbor_north);
            temp_list.Add(corner_neighbor_south);
            Random rand = new Random();
            string random_number = Convert.ToString(rand.Next(0, 10));
            for (int x = 0; x < temp_list.Count; x++)
            {
                //Console.WriteLine(temp_list[x]); 
                List<PanelBase> matching_bases = PanelBaseList.Where(c => c.Center.Item1 == temp_list[x].Item1 && c.Center.Item2 == temp_list[x].Item2).ToList();
                if (matching_bases.Count() != 0)
                {
                    foreach (PanelBase pb in matching_bases)
                    {
                        pb.PanelIDList.Add(EcoPanel.PanelID);
                        pb.BlockWeightList.Add(EcoPanel.ValueFromExcel);
                    }

                }
                else
                {
                    PanelBase temp = new PanelBase(PanelBaseList.Count.ToString(), EcoPanel.BallastLocation, temp_list[x], EcoPanel.ValueFromExcel);
                    PanelBaseList.Add(temp);


                }

            }
        }
        public void CalculateBallastLocation()
        {
            foreach (EcoPanel EcoPanel in PanelList)
            {

                //0W1N2S3E
                if (EcoPanel.DirectionList.Contains(0) && EcoPanel.DirectionList.Contains(1))
                {
                    EcoPanel.BallastLocation = 0;

                }
                else if (EcoPanel.DirectionList.Contains(1) && EcoPanel.DirectionList.Contains(3))
                {
                    EcoPanel.BallastLocation = 3;

                }
                else if (EcoPanel.DirectionList.Contains(2) && EcoPanel.DirectionList.Contains(3))
                {
                    EcoPanel.BallastLocation = 9;

                }
                else if (EcoPanel.DirectionList.Contains(2) && EcoPanel.DirectionList.Contains(0))
                {
                    EcoPanel.BallastLocation = 7;

                }
                else if (EcoPanel.DirectionList.Contains(0) && EcoPanel.DirectionList.Count == 1)
                {
                    EcoPanel.BallastLocation = 3;

                }
                else if (EcoPanel.DirectionList.Contains(1) && EcoPanel.DirectionList.Count == 1)
                {
                    EcoPanel.BallastLocation = 2;
                }
                else if (EcoPanel.DirectionList.Contains(2) && EcoPanel.DirectionList.Count == 1)
                {
                    EcoPanel.BallastLocation = 1;
                }
                else if (EcoPanel.DirectionList.Contains(3) && EcoPanel.DirectionList.Count == 1)
                {
                    EcoPanel.BallastLocation = 6;
                }
                else if ((EcoPanel.DirectionList.Contains(0)) && (EcoPanel.DirectionList.Contains(1)) & (EcoPanel.DirectionList.Contains(2)) & (EcoPanel.DirectionList.Contains(3)))
                {
                    EcoPanel.BallastLocation = 8;
                }

            }


        }
        public void RunIFILocationChecks()
        {
            foreach (var EcoPanel in PanelList)
            {
                E2W_LAND_Check(EcoPanel);
                E2W_PORT_Check(EcoPanel);
                N_LAND_Check(EcoPanel);
                N_PORT_Check(EcoPanel);
                S_PORT_Check(EcoPanel);
                S_LAND_Check(EcoPanel);
                W2E_LAND_Check(EcoPanel);
                W2E_PORT_Check(EcoPanel);

            }
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
                Console.WriteLine("Neighbor List: ");
                Console.WriteLine("=================");
                foreach (var neighbor in x.NeighborHood)
                {
                    Console.WriteLine("Neighbor ID: {0} ", neighbor.PanelID);
                }
                Console.WriteLine("=================");
                Console.WriteLine("\n");
                Console.ReadKey();

            }

            Console.WriteLine("Press Enter to Continue: ");


            return;
        }
    }
}