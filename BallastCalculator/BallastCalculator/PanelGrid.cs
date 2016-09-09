using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dimensions;


namespace BallastCalculator
{

    class PanelGrid 
    {

        public PanelGrid(BasicDimensions perimeter, List<Panel> plist)
        {
            PanelList = plist;
            //IFIBoarder = perimeter;
            //Console.WriteLine(perimeter.Center.Item1);
            //Console.WriteLine(perimeter.Center.Item2);
            //Console.ReadKey();
            foreach (Panel panel in PanelList)
            {

                panel.CalculatePanelCenter(perimeter.Center.Item1, perimeter.Center.Item2);
                //Console.WriteLine(panel.Xvalue);
                //Console.WriteLine(panel.Yvalue);
                //Console.WriteLine(panel.PanelID);
                //Console.WriteLine(panel.Center);
                //Console.ReadKey();

            }
            foreach (Panel panel in PanelList)
            {
                panel.Sliding = GetSlidingValue(panel);
            }
            foreach (Panel panel in PanelList)
            {
                panel.Uplift = GetUpliftValue(panel);
            }
            foreach (Panel panel in PanelList)
            {
                CalculatePanelCorners(panel);
            }

        }
        private void CalculatePanelCorners(Panel panel)
        {
            var x_start = panel.Center.Item1;
            var y_start = panel.Center.Item2;
            Tuple<double, double> corner_neighbor_east = new Tuple<double, double>(x_start - .5 * (.5 + IFIBoarder.Width), y_start + .5 * (17.494 + IFIBoarder.Height)); //NWest
            Tuple<double, double> corner_neighbor_north = new Tuple<double, double>(x_start + .5 * (.5 + IFIBoarder.Width), y_start + (17.494 + IFIBoarder.Height));//NEast
            Tuple<double, double> corner_neighbor_south = new Tuple<double, double>(x_start + .5 * (.5 + IFIBoarder.Width), y_start - (17.494 - IFIBoarder.Height)); //SEast
            Tuple<double, double> corner_neighbor_west = new Tuple<double, double>(x_start - .5 * (.5 + IFIBoarder.Width), y_start - (17.494 - IFIBoarder.Height)); //SWest
            List<Tuple<double, double>> temp_list = new List<Tuple<double, double>>();

            temp_list.Add(corner_neighbor_east);
            temp_list.Add(corner_neighbor_west);
            temp_list.Add(corner_neighbor_north);
            temp_list.Add(corner_neighbor_south);
            Random rand = new Random();
            string random_number = Convert.ToString(rand.Next(0, 10));

            foreach (var x in temp_list)
            {
                PanelBase temp = new PanelBase();
                temp.CenterPoint = x;
                temp.EdgeID = random_number + RandomLetter.GetLetter() + RandomLetter.GetLetter() + RandomLetter.GetLetter();
                temp.PanelID = panel.PanelID;
                temp.BallastLocation = panel.BallastLocation;
                PanelBaseList.Add(temp);
            }
        }
        static class RandomLetter
        {
            static Random _random = new Random();
            public static string GetLetter()
            {
                // This method returns a random lowercase letter.
                // ... Between 'a' and 'z' inclusize.
                int num = _random.Next(0, 26); // Zero to 25
                char let = (char)('a' + num);
                return Convert.ToString(let);
            }
        }
        BasicDimensions IFIBoarder = new BasicDimensions();
        private List<Panel> PanelList = new List<Panel>();
        public List<Panel> GetPanels()
        {
            return PanelList;
        }
        public List<PanelBase> GetPanelBases()
        {
            return PanelBaseList;
        }
        public void CalculateBallastLocation()
        {
            foreach (Panel panel in PanelList)
            {

                //0W1N2S3E
                if (panel.DirectionList.Contains(0) && panel.DirectionList.Contains(1))
                {
                    panel.BallastLocation = 0;

                }
                else if (panel.DirectionList.Contains(1) && panel.DirectionList.Contains(3))
                {
                    panel.BallastLocation = 3;

                }
                else if (panel.DirectionList.Contains(2) && panel.DirectionList.Contains(3))
                {
                    panel.BallastLocation = 9;

                }
                else if (panel.DirectionList.Contains(2) && panel.DirectionList.Contains(0))
                {
                    panel.BallastLocation = 7;

                }
                else if (panel.DirectionList.Contains(0) && panel.DirectionList.Count == 1)
                {
                    panel.BallastLocation = 3;

                }
                else if (panel.DirectionList.Contains(1) && panel.DirectionList.Count == 1)
                {
                    panel.BallastLocation = 2;
                }
                else if (panel.DirectionList.Contains(2) && panel.DirectionList.Count == 1)
                {
                    panel.BallastLocation = 1;
                }
                else if (panel.DirectionList.Contains(3) && panel.DirectionList.Count == 1)
                {
                    panel.BallastLocation = 6;
                } else if ((panel.DirectionList.Contains(0)) && (panel.DirectionList.Contains(1)) & (panel.DirectionList.Contains(2)) & (panel.DirectionList.Contains(3)))
                {
                    panel.BallastLocation = 8;
                }

            }


        }
        private List<PanelBase> PanelBaseList = new List<PanelBase>();
        private int GetUpliftValue(Panel panel)
        {
            var x_start = panel.Center.Item1;
            var y_start = panel.Center.Item2;
            int direction = 4;
            int input_n = 2; 
            var neighborhood = GenerateNeighborhood(input_n, x_start, y_start, direction);
            int total_matches = 0; 
            foreach (var neighbor in neighborhood)
            {
                foreach (var x in PanelList)
                {

                    if ((Math.Abs(neighbor.Item1 - x.Center.Item1) <= .5) && (Math.Abs(neighbor.Item2 - x.Center.Item2) <= .5))
                    {
                        total_matches = total_matches + 1; 

                    }
                }
            }
            return total_matches; 
        }
        private int GetSlidingValue(Panel panel)
        {
            var x_start = panel.Center.Item1;
            var y_start = panel.Center.Item2;
            int direction = 4;
            int input_n = 4;
            var neighborhood = GenerateNeighborhood(input_n, x_start, y_start, direction);
            int total_matches = 0;
            foreach (var neighbor in neighborhood)
            {
                foreach (var x in PanelList)
                {

                    if ((Math.Abs(neighbor.Item1 - x.Center.Item1) <= .5) && (Math.Abs(neighbor.Item2 - x.Center.Item2) <= .5))
                    {
                        total_matches = total_matches + 1;

                    }
                }
            }
            return total_matches;
        }
        public void RunIFILocationChecks()
        {
            foreach (var panel in PanelList)
            {
                E2W_LAND_Check(panel);
                E2W_PORT_Check(panel);
                N_LAND_Check(panel);
                N_PORT_Check(panel);
                S_PORT_Check(panel);
                S_LAND_Check(panel);
                W2E_LAND_Check(panel);
                W2E_PORT_Check(panel);

            }
        }
        public void CalculateNeighbors(int input_n)

        {
            foreach (Panel panel in PanelList)
            {
                int direction = 4;

                List<Tuple<double,double,int>> neighborhood = GenerateNeighborhood(input_n, panel.Center.Item1, panel.Center.Item2, direction);
                

                foreach (var neighbor in neighborhood)
                {
                    foreach (var x in PanelList)
                    {
                       
                        if ((Math.Abs(neighbor.Item1 - x.Center.Item1) <= .5) && (Math.Abs(neighbor.Item2 - x.Center.Item2) <= .5))
                        {
                            panel.NeighborHood.Add(x);
                            panel.DirectionList.Add(neighbor.Item3); 
                           
                                }
                    }
 
                }
                IEnumerable<int> values  = panel.DirectionList.Cast<int>().Distinct();
                panel.DirectionList = values.ToList<int>(); 
            }
        }
        private List<Tuple<double, double,int >> GenerateNeighborhood(int input_n, double x_start , double y_start, int direction)
        {
            List<Tuple<double, double,int>> neighborhood = new List<Tuple<double, double,int>>();
            for (int n = 0; n <= input_n; n++)
            {
                if (direction == 0)// East 
                {
                    Tuple<double, double, int> temp_neighbor = new Tuple<double, double,int>(x_start - (.5 + IFIBoarder.Width) * n, y_start, 0); //East 
                    neighborhood.Add(temp_neighbor);
                }
                else if (direction == 1) //North
                {
                    Tuple<double, double,int > temp_neighbor = new Tuple<double, double,int>(x_start, y_start + (17.494 + IFIBoarder.Height) * n,1);//North
                    neighborhood.Add(temp_neighbor);

                }
                else if (direction == 2)//South
                {
                    Tuple<double, double,int > temp_neighbor = new Tuple<double, double,int>(x_start, y_start + (17.494 - IFIBoarder.Height),2); //South
                    neighborhood.Add(temp_neighbor);

                }
                else if (direction == 3)//West 
                {
                    Tuple<double, double,int > temp_neighbor = new Tuple<double, double,int>(x_start + (.5 + IFIBoarder.Width) * n, y_start,3); // West
                    neighborhood.Add(temp_neighbor);

                }
                else if (direction == 4) //All Directions 
                {
                    Tuple<double, double,int> temp_neighbor_east = new Tuple<double, double,int >(x_start - (.5 + IFIBoarder.Width) * n, y_start,0); //East 
                    Tuple<double, double,int> temp_neighbor_north = new Tuple<double, double,int >(x_start, y_start + (17.494 + IFIBoarder.Height) * n,1);//North
                    Tuple<double, double,int> temp_neighbor_south = new Tuple<double, double,int >(x_start, y_start + (17.494 - IFIBoarder.Height)*n,2); //South
                    Tuple<double, double,int> temp_neighbor_west = new Tuple<double, double,int >(x_start + (.5 + IFIBoarder.Width) * n, y_start,3); // West 
                    neighborhood.Add(temp_neighbor_east);
                    neighborhood.Add(temp_neighbor_west);
                    neighborhood.Add(temp_neighbor_north);
                    neighborhood.Add(temp_neighbor_south);
                }
            }
            return neighborhood;
        }
        public void PrintPanelData()
        {
            Console.WriteLine("Panel/Entities Values:");
            Console.WriteLine("======================");
            foreach (var x in PanelList)
            {
                Console.WriteLine("Panel Number {0}: ", x.PanelID);
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

            }

            Console.WriteLine("Press Enter to Continue: ");
            Console.ReadKey();


            return;
        }
        private int CountNeighbors(List<Tuple<double, double,int>> neighborhood)
        {
            int count = 0;
            foreach (var neighbor in neighborhood)
            {
                foreach (var panel in PanelList)
                {
                    if ((Math.Abs(neighbor.Item1 - panel.Center.Item1) <= .5) && (Math.Abs(neighbor.Item2 - panel.Center.Item2) <= .5))
                    {
                        count = count + 1;

                    }

                }
            }




            return count;
        }
        private void E2W_LAND_Check(Panel panel)
        {
            var x_start = panel.Center.Item1;
            var y_start = panel.Center.Item2;
            int direction = 0; 
            int input_n = 4;
            List<Tuple<double, double,int >> neighborhood = GenerateNeighborhood(input_n, x_start, y_start, direction);
            // classification of module position; 0 = east edge, 1 = cols 2-4 from edge, 2 = cols >= 5 from edge
            int IFI_E2W_count = CountNeighbors(neighborhood); //count of total panels east of a given module until break

            if (IFI_E2W_count == 0)
            {
                panel.IFI_E2W_Land = 0;
            }
            else if ((IFI_E2W_count >= 1) && (IFI_E2W_count <= 3))
            {
                panel.IFI_E2W_Land = 1;
            }
            else
            {
                panel.IFI_E2W_Land = 2;
            }
        }
        private void E2W_PORT_Check(Panel panel)
        {

            var x_start = panel.Center.Item1;
            var y_start = panel.Center.Item2;
            int direction = 0;
            int input_n = 10;
            List<Tuple<double, double,int>> neighborhood = GenerateNeighborhood(input_n, x_start, y_start, direction);
            int IFI_E2W_count = CountNeighbors(neighborhood);
            if (IFI_E2W_count == 0)
            {
                panel.IFI_E2W_Port = 0;
            }
            else if ((IFI_E2W_count >= 1) && (IFI_E2W_count <= 9))
            {
                panel.IFI_E2W_Port = 1;
            }
            else
            {
                panel.IFI_E2W_Port = 2;
            }


        }
        private void N_LAND_Check(Panel panel)
        {
            // classification of module position; 0 = north edge, 1 = rows 2-6 from edge, 2 = rows >= 7 from edge
            var x_start = panel.Center.Item1;
            var y_start = panel.Center.Item2;
            int direction = 1;
            int input_n = 6;
            List<Tuple<double, double,int>> neighborhood = GenerateNeighborhood(input_n, x_start, y_start, direction);
            int IFI_North_count = CountNeighbors(neighborhood);

            if (IFI_North_count == 0)
            {
                panel.IFI_NORTH_Land = 0;
            }
            else if ((IFI_North_count >= 1) && (IFI_North_count <= 5))
            {
                panel.IFI_NORTH_Land = 1;
            }
            else
            {
                panel.IFI_NORTH_Land = 2;
            }


        }
        private void N_PORT_Check(Panel panel)
        {
           
                var x_start = panel.Center.Item1;
                var y_start = panel.Center.Item2;
                int direction = 1;
                int input_n = 4;
                List<Tuple<double, double,int>> neighborhood = GenerateNeighborhood(input_n, x_start, y_start, direction);
                int IFI_north_count = CountNeighbors(neighborhood);

               
                    if (IFI_north_count == 0)
                    {
                        panel.IFI_NORTH_Port = 0;
                    }
                    else if ((IFI_north_count >= 1) && (IFI_north_count <= 3))
                    {
                        panel.IFI_NORTH_Port = 2;
                        
                    }
                    else
                    {
                        panel.IFI_NORTH_Port = 2;

                }
            

            }
        private void S_PORT_Check(Panel panel)
        {

            var x_start = panel.Center.Item1;
            var y_start = panel.Center.Item2;
            int direction = 2;
            int input_n = 0;
            List<Tuple<double, double,int>> neighborhood = GenerateNeighborhood(input_n, x_start, y_start, direction);
            int IFI_South_count = CountNeighbors(neighborhood);

            if (IFI_South_count == 1)
            {
                panel.IFI_SOUTH_Port = 1;
            }
            else
            {
                panel.IFI_SOUTH_Port = 0;

            }

        }
        private void S_LAND_Check(Panel panel)
        {
            var x_start = panel.Center.Item1;
            var y_start = panel.Center.Item2;
            int direction = 2;
            int input_n = 0;
            List<Tuple<double, double,int>> neighborhood = GenerateNeighborhood(input_n, x_start, y_start, direction);
            int IFI_South_count = CountNeighbors(neighborhood);
            if (IFI_South_count == 1)
            {
                panel.IFI_SOUTH_Land = 1;
            }
            else
            {
                panel.IFI_SOUTH_Land = 0;

            }
        }
        private void W2E_LAND_Check(Panel panel)
        {

            var x_start = panel.Center.Item1;
            var y_start = panel.Center.Item2;
            int direction = 3;
            int input_n = 4;
            List<Tuple<double, double,int>> neighborhood = GenerateNeighborhood(input_n, x_start, y_start, direction);
            var IFI_W2E_Land_count = CountNeighbors(neighborhood);

            if (IFI_W2E_Land_count == 0)
            {
                panel.IFI_W2E_Land = 0;
            }
            else if ((IFI_W2E_Land_count >= 1) && (IFI_W2E_Land_count <= 3))
            {
                panel.IFI_W2E_Land = 1;
            }
            else
            {
                panel.IFI_W2E_Land = 2;
            }
        }
        private void W2E_PORT_Check(Panel panel)
        {

            var x_start = panel.Center.Item1;
            var y_start = panel.Center.Item2;
            int direction = 3;
            int input_n = 10;
            List<Tuple<double, double,int>> neighborhood = GenerateNeighborhood(input_n, x_start, y_start, direction);
            var IFI_W2E_Port_count = CountNeighbors(neighborhood);


            // classification of module position; 0 = west edge, 1 = cols 2-4 from west, 2 = cols >= 5 from edge
            if (IFI_W2E_Port_count == 0)
            {
                panel.IFI_W2E_Port = 0;
            }
            else if ((IFI_W2E_Port_count >= 1) && (IFI_W2E_Port_count <= 9))
            {
                panel.IFI_W2E_Port = 1;
            }
            else
            {
                panel.IFI_W2E_Port = 2;
            }
        }
    }
}

//0W1N2S3E



// Old Contents of Calculate Neighbors 
//List<Tuple<double, double>> neighborhood = new List<Tuple<double, double>>();






//}



//private List<Tuple<double, double, int>> GenerateNeighborhood2(int input_n, double x_start, double y_start, int direction)
//{
//    List<Tuple<double, double, int>> neighborhood = new List<Tuple<double, double, int>>();
//    for (int x = 0; x <= input_n; x++)
//    {
//        for (int y = 0; y <= input_n; y++)
//        {
//            for (int i = -1; i <= 1; i += 2)
//            {
//                if (x.Equals(0))
//                    i = 1;


//                for (int j = -1; j <= 1; j += 2)
//                {
//                    if (y.Equals(0))
//                        j = 1;
//                    int temp_direction = 0;
//                    if (i == -1 && j == 0)
//                    {
//                        temp_direction = 3;
//                    }
//                    else if (i == 1 & j == 0)
//                    {
//                        temp_direction = 2;
//                    }
//                    else if (i == 0 & j == -1)
//                    {
//                        temp_direction = 3;
//                    }
//                    else
//                    {
//                        temp_direction = 1;
//                    }
//                    Tuple<double, double, int> temp_neighbor = new Tuple<double, double, int>(Center.Item1 + (0.5 + IFIBoarder.Width) * i * x, y_start + (17.494 + IFIBoarder.Height) * j * y, temp_direction);
//                    neighborhood.Add(temp_neighbor);
//                }

//            }
//        }
//    }
//    return neighborhood;
//}
