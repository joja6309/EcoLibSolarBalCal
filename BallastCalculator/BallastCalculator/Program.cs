
using System.Windows.Forms; 
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


            Application.Run(new UserGUI());
            //Application.Run(new UserGUI());

            //string file_path = FilePathContainer.dxfPath;
            //string output_path = FilePathContainer.outPath;
            //string excel_path = FilePathContainer.excelPath;
            //string panelName = FilePathContainer.panelName;
            //Console.WriteLine(excel_path);
            //Console.WriteLine(output_path);
            //Console.WriteLine(file_path);


            string file_path = FilePathContainer.dxfPath;
            string output_path = FilePathContainer.outPath;
            string excel_path = FilePathContainer.excelPath;
            string panelName = FilePathContainer.panelName;

            bool land;

            ExcelIO ExInterface = new ExcelIO(excel_path);
            //Def B32
            //Orent B33 
            //Bal B34 
            ExInterface.ProcessFirstSheet();
            land = ExInterface.land;
            var bal = ExInterface.bal; 

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
            PanelGrid grid = new PanelGrid(BlockPerimeter, PanelList,bal);
            
            //{
            //    //Console.WriteLine("Panel No. " + panel.PanelID + " " + panel.Center);
            //    //Console.WriteLine("Panel Lift count: " + panel.Uplift);
            //    //Console.WriteLine("Panel Sliding Count: " + panel.Sliding);

            //    if (land)
            //    {
            //        //KB DEBUG: uplift and sliding were backwards, fixed it.
            //        ExInterface.WritetoSandU(panel.Uplift.ToString(), panel.Sliding.ToString());
            //        panel.ValueFromExcel = ExInterface.CellIO(panel.NE_Zone, panel.NW_Zone, panel.IFI_NORTH_Land, panel.IFI_SOUTH_Land, panel.IFI_E2W_Land, panel.IFI_W2E_Land);
            //    }
            //    else
            //    {
            //        //KB DEBUG: uplift and sliding were backwards, fixed it.
            //        ExInterface.WritetoSandU(panel.Uplift.ToString(), panel.Sliding.ToString());
            //        panel.ValueFromExcel = ExInterface.CellIO(panel.NE_Zone, panel.NW_Zone, panel.IFI_NORTH_Port, panel.IFI_SOUTH_Port, panel.IFI_E2W_Port, panel.IFI_W2E_Port);
            //    }
            //    Console.WriteLine("Output Excel Value {0}", panel.ValueFromExcel);

            //    //Console.WriteLine("-----------------------------------");
            //    //Console.WriteLine("NE zone: " + panel.NE_Zone);
            //    //Console.WriteLine("north zone: " + panel.IFI_NORTH_Land + " should be same as " + panel.IFI_NORTH_Port);
            //    //Console.WriteLine("E2W col: " + panel.ColumnNumberE2W_LAND + " or " + panel.ColumnNumberE2W_PORT);
            //    //Console.WriteLine("E2W trucol: " + panel.TrueE2Wcol_LAND + " or " + panel.TrueE2Wcol_PORT);
            //    //Console.WriteLine("E2W zone: " + panel.IFI_E2W_Land + " or " + panel.IFI_E2W_Port);
            //    //Console.WriteLine("south zone: " + panel.IFI_SOUTH_Land + " should be same as " + panel.IFI_SOUTH_Port);
            //    //Console.WriteLine("-----------------------------------");
            //    //Console.WriteLine("NW zone: " + panel.NW_Zone);
            //    //Console.WriteLine("north zone: " + panel.IFI_NORTH_Land + " should be same as " + panel.IFI_NORTH_Port);
            //    //Console.WriteLine("W2E col: " + panel.ColumnNumberW2E_LAND + " or " + panel.ColumnNumberW2E_PORT);
            //    //Console.WriteLine("W2E trucol: " + panel.TrueW2Ecol_LAND + " or " + panel.TrueW2Ecol_PORT);
            //    //Console.WriteLine("W2E zone: " + panel.IFI_W2E_Land + " or " + panel.IFI_W2E_Port);
            //    //Console.WriteLine("south zone: " + panel.IFI_SOUTH_Land + " should be same as " + panel.IFI_SOUTH_Port);
            //    //Console.WriteLine("-----------------------------------");
            //    //Console.WriteLine("Max output from Excel:" + panel.ValueFromExcel);
            //    //Console.WriteLine("VVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVV");
            //    //Console.ReadKey();
            //
            Stack<double> excel_val = new Stack<double>();


            //foreach( var x in list)
            //{
            //    excel_val.Push(x);
            //    Console.WriteLine(x);
            //}
            //Console.WriteLine(excel_val.Count); 
            //Console.ReadKey();
            //foreach( var x in p_list)
            //{
            //    x.ValueFromExcel = excel_val.Pop();
            //}
            grid.SetExcelValues();



            //Console.WriteLine("Block Calculations: ");
            //Console.WriteLine("=================="); 
            grid.RunBasePanelCalculations();
            List<Base> final_bases = grid.PanelBaseList;
            //foreach (var panel in final_bases)
            //{
            //    Console.WriteLine("=======================");
            //    Console.WriteLine("Base Id {0}", panel.UniqueID);
            //    Console.WriteLine("Contribution list for base");
            //    foreach (var p in panel.ContributionList)
            //    {
            //        Console.WriteLine(p);
            //    }
            //    Console.WriteLine("Ballast Block Value {0}", panel.BallastBlockValue);
            //    Console.WriteLine("Ballast Block Unrounded Value {0}", panel.UnroundedBallastBlockValue);
            //    Console.WriteLine("=======================");

            //}
            //Console.ReadKey();

            dxfInterface.GenerateFileOut(final_bases);

            //foreach(var c in final_bases)
            //{
            //    Console.WriteLine("============");
            //    Console.WriteLine(c.BallastBlockValue);
            //    Console.WriteLine(c.BlockWeight);
            //    Console.WriteLine(c.IFIBaseTotal);
            //    Console.WriteLine("============");
            //}
            //Console.ReadKey();


            //foreach (var panel in processedList)
            //{
            //    Console.WriteLine("Panel No. " + panel.PanelID + " " + panel.Center);
            //    Console.WriteLine("NE zone: " + panel.NE_Zone);
            //    Console.WriteLine("NW zone: " + panel.NW_Zone);
            //    Console.WriteLine("north zone: " + panel.IFI_NORTH_Land + " or " + panel.IFI_NORTH_Port);
            //    Console.WriteLine("E2W trucol: " + panel.TrueE2Wcol_LAND + " or " + panel.TrueE2Wcol_PORT);
            //    Console.WriteLine("E2W zone: " + panel.IFI_E2W_Land + " or " + panel.IFI_E2W_Port);
            //    Console.WriteLine("south zone: " + panel.IFI_SOUTH_Land + " should be same as " + panel.IFI_SOUTH_Port);
            //    Console.WriteLine("W2E trucol: " + panel.TrueW2Ecol_LAND + " or " + panel.TrueW2Ecol_PORT);
            //    Console.WriteLine("W2E zone: " + panel.IFI_W2E_Land + " or " + panel.IFI_W2E_Port);
            //    Console.WriteLine("Below are the two values output from Excel:");
            //    Console.ReadKey();

            //}

        }
   
    }
}














