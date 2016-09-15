using System;
using System.Collections.Generic;
using System.Linq;
using ExcelInterface;
using Dimensions;
using DXFInterface;



namespace BallastCalculator
{
    public static class FilePathContainer
    {
        public static string excelPath;
        public static string dxfPath;
        public static string outPath;
        public static string panelName;
        static void SetExcelPath(string path)
        {
            excelPath = path;
        }
        static void SetDxfPath(string path)
        {
            dxfPath = path;
        }
        static void SetOutPath(string path)
        {
            outPath = path;
        }
        static void SetPanelName(string panel)
        {
            panelName = panel;
        }
    }

    class Program
    {


        [STAThread]
        static void Main(string[] args)
        {

            //Application.Run(new UserGUI());

            //string file_path = FilePathContainer.dxfPath;
            //string output_path = FilePathContainer.outPath;
            //string excel_path = FilePathContainer.excelPath;
            //string panelName = FilePathContainer.panelName;
            //Console.WriteLine(excel_path);
            //Console.WriteLine(output_path);
            //Console.WriteLine(file_path);
            string file_path, excel_path, panelName, output_path;
            file_path = @"C:\Users\KBasarich\Desktop\IPS_Nissan Boulder_Ecolibrium Layout Rev C DEFLECTOR FOR EW CHECK.dxf";
            excel_path = @"C:\Users\KBasarich\Desktop\Boulder Nissan Threecocalcs 0_5_1 DEFLECTOR.xlsx";
            panelName = "SPR-P17";
            output_path = "";
            
            ExcelIO ExInterface = new ExcelIO(excel_path);
            //Def B32
            //Orent B33 
            //Bal B34 
            bool def = ExInterface.CheckFirst("C32");
            bool land = ExInterface.CheckFirst("C33");
            double bal = ExInterface.GetBalast("B34");

            DxfIO dxfInterface = new DxfIO(file_path, output_path, panelName, land);
            dxfInterface.ParseFile();

            //dxfInterface.RunOutTesting();
            BasicDimensions BlockPerimeter = dxfInterface.GetValuesFromBlockSection();
            IFIPerimeter IFIboarder = dxfInterface.GetIFIValues();
            List<EcoPanel> PanelList = dxfInterface.GetEntitiesPanels();

            BlockPerimeter.CalculateCenter();
            IFIboarder.CalculateCenter();
            IFIboarder.SetCorners();

            foreach (EcoPanel EcoPanel in PanelList)
            {
                EcoPanel.CalculatePanelCenter(BlockPerimeter.Center.Item1, BlockPerimeter.Center.Item2);
                EcoPanel.SetPanelZones(IFIboarder);
            }
            PanelGrid grid = new PanelGrid(BlockPerimeter, PanelList);
            List<EcoPanel> processedList = grid.GetPanels();
            List<int> WODeflector_Refzones = new List<int>() { 103, 112, 121, 130, 139 };
            List<int> WDeflector_Refzones = new List<int>() { 51, 60, 69, 78, 87 };
            //KB DEBUG: corrected sliding and uplift cell values from 38 to 39
            Tuple<string, uint> slidingCell = new Tuple<string, uint>("G", 39);
            Tuple<string, uint> upliftCell = new Tuple<string, uint>("C", 39);
            //List<int> WOBallasrreference_zones = new List<int>() {}
            string referenceSheet;
            string column;
            if (land)
            {
                referenceSheet = "wind load calc_10d";
                column = "M";
            }
            else
            {
                referenceSheet = "wind load calc_5d";
                column = "K";
            }

            foreach (EcoPanel panel in processedList)
            {
                int startingCell_NE = 0;
                int startingCell_NW = 0;
                List<int> ColumnPositions = new List<int>();
                ExInterface.InsertText(referenceSheet, upliftCell, panel.ToString());
                ExInterface.InsertText(referenceSheet, slidingCell, panel.Uplift.ToString());
                ExInterface.Update();

                if (def)
                {   // reference sheet 10d at correct zone 
                    //KB DEBUG: corrected with or without deflector call
                    startingCell_NE = WDeflector_Refzones[panel.NE_Zone - 1];
                    startingCell_NW = WDeflector_Refzones[panel.NW_Zone - 1];
                }
                else
                {   // Same Row Reference Array if sheet 5d at correct zone 
                    //KB DEBUG: corrected with or without deflector call
                    startingCell_NE = WODeflector_Refzones[panel.NE_Zone - 1];
                    startingCell_NW = WODeflector_Refzones[panel.NW_Zone - 1];

                }
                // N0 S0 both
                // S0 1N only south
                // S1 just north

                // NEZone -> E2W
                // NWZone -> W2E
                //N0
                //N1 S1
                //N2 S1
                //S0
                if (land)
                {

                    if (panel.IFI_NORTH_Land == 0)
                    {
                        int temp_cell_West = startingCell_NW + 1;
                        int temp_cell_East = startingCell_NE + 1;

                        if (panel.IFI_E2W_Land.Equals(2))
                        {
                            temp_cell_East = startingCell_NE + 1;


                        }
                        else if (panel.IFI_E2W_Land.Equals(2))
                        {
                            temp_cell_West = startingCell_NW + 1;
                        }
                        ColumnPositions.Add(temp_cell_West);
                        ColumnPositions.Add(temp_cell_East);

                    }
                    else if (panel.IFI_SOUTH_Land == 0 && panel.IFI_NORTH_Land != 0)
                    {
                        if (panel.IFI_NORTH_Land == 1)
                        {
                            int temp_cell_West = startingCell_NW + 1;
                            int temp_cell_East = startingCell_NE + 1;
                            if (panel.IFI_E2W_Land.Equals(2))
                            {
                                temp_cell_West = startingCell_NE + 1;


                            }
                            else if (panel.IFI_E2W_Land.Equals(2))
                            {
                                temp_cell_East = startingCell_NW + 1;
                            }
                            ColumnPositions.Add(temp_cell_West);
                            ColumnPositions.Add(temp_cell_East);

                        }
                        else if (panel.IFI_NORTH_Land == 2)
                        {
                            int temp_cell_West = startingCell_NW + 2;
                            int temp_cell_East = startingCell_NE + 2;
                            if (panel.IFI_E2W_Land.Equals(2))
                            {
                                temp_cell_West = startingCell_NE + 1;


                            }
                            else if (panel.IFI_E2W_Land.Equals(2))
                            {
                                temp_cell_East = startingCell_NW + 1;
                            }
                            ColumnPositions.Add(temp_cell_West);
                            ColumnPositions.Add(temp_cell_East);
                        }

                    }
                    else if (panel.IFI_SOUTH_Land == 1 | panel.IFI_SOUTH_Land == 0)
                    {
                        int temp_cell_West = startingCell_NW + 6;
                        int temp_cell_East = startingCell_NE + 6;
                        if (panel.IFI_E2W_Land.Equals(2))
                        {
                            temp_cell_West = startingCell_NE + 1;
                            
                        }
                        else if (panel.IFI_E2W_Land.Equals(2))
                        {
                            temp_cell_East = startingCell_NW + 1;
                        }
                        ColumnPositions.Add(temp_cell_West);
                        ColumnPositions.Add(temp_cell_East);

                    }

                }
                List<double> Results = new List<double>();
                foreach (var position in ColumnPositions)
                {
                    var return_cell = ExInterface.ReadCell(referenceSheet, column + position.ToString());
                    Results.Add(Convert.ToDouble(return_cell));
                }
                double final_value = Results.Max();
                Console.WriteLine("Panel No. " + panel.PanelID + " " + panel.Center);
                Console.WriteLine("NE zone: " + panel.NE_Zone);
                Console.WriteLine("NW zone: " + panel.NW_Zone);
                Console.WriteLine("north zone: " + panel.IFI_NORTH_Land + " or " + panel.IFI_NORTH_Port);
                Console.WriteLine("E2W trucol: " + panel.TrueE2Wcol_LAND + " or " + panel.TrueE2Wcol_PORT);
                Console.WriteLine("E2W zone: " + panel.IFI_E2W_Land + " or " + panel.IFI_E2W_Port);
                Console.WriteLine("south zone: " + panel.IFI_SOUTH_Land + " should be same as " + panel.IFI_SOUTH_Port);
                Console.WriteLine("W2E trucol: " + panel.TrueW2Ecol_LAND + " or " + panel.TrueW2Ecol_PORT);
                Console.WriteLine("W2E zone: " + panel.IFI_W2E_Land + " or " + panel.IFI_W2E_Port);
                Console.WriteLine("Below are the two values output from Excel:");
                foreach (var x in Results)
                {
                    Console.WriteLine(x);
                }
                Console.WriteLine("==========================");
                Console.ReadKey();
                panel.ValueFromExcel = final_value;
            }
//            foreach (EcoPanel panel in processedList)
//            {
//                Console.WriteLine(panel.ValueFromExcel);
//            }
//            Console.ReadKey();











            //IFIboarder.PrintIFIData();     
            //grid.PrintPanelData();
            //foreach(EcoPanel panel in EcoPanel)
            //{
            //    Console.WriteLine(panel.IFI_E2W_Land)
            //}
            // Input Center of Block and List of Panels with correct Zones
            //List <PanelBase> baseValues = grid.GetPanelBases();
            //dxfInterface.OutGen.GenerateFileOut(baseValues);
            // Current Debug Targets! 




            ///////////////////////////////////////
            //Under development 
            ///////////////////////////////////////

            //List<EcoPanel> panel_list = grid.GetPanels();
            //List<PanelBase> baseList = grid.GetPanelBases();
            //grid.PrintPanelData();
            //foreach (var x in panel_list)
            //{
            //    Console.WriteLine(x.BallastLocation);
            //}
            //Console.ReadKey();
            //List<EcoPanel> FinishedList = grid.GetPanels();

        }
        }
    }




