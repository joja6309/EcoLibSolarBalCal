




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
            file_path = @"C:\Users\Owner\Desktop\IPS_Nissan Boulder_Ecolibrium Layout Rev C DEFLECTOR.dxf";
            excel_path = @"C: \Users\Owner\Desktop\Boulder Nissan Threecocalcs 0_5_2 DEFLECTOR.xlsx";
            panelName = "SPR-P17";
            output_path = "";
            bool land;


            ExcelIO ExInterface = new ExcelIO(excel_path);
            //Def B32
            //Orent B33 
            //Bal B34 
            ExInterface.ProcessFirstSheet();
            land = ExInterface.land;

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
            //foreach (var panel in processedList)
            //{
            //    if (land)
            //    {
            //        ExInterface.WritetoSandU(panel.Sliding.ToString(), panel.Uplift.ToString());
            //        panel.ValueFromExcel = ExInterface.CellIO(panel.NE_Zone, panel.NW_Zone, panel.IFI_NORTH_Land, panel.IFI_SOUTH_Land, panel.IFI_E2W_Land, panel.IFI_W2E_Land);
            //    }
            //    else
            //    {
            //        ExInterface.WritetoSandU(panel.Sliding.ToString(), panel.Uplift.ToString());
            //        panel.ValueFromExcel = ExInterface.CellIO(panel.NE_Zone, panel.NW_Zone, panel.IFI_NORTH_Port, panel.IFI_SOUTH_Port, panel.IFI_E2W_Port, panel.IFI_W2E_Port);
            //    }
            //}

            foreach (var panel in processedList)
            {
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
                Console.ReadKey();

            }

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














