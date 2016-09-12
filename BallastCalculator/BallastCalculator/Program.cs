using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using ExcelInterface;
using Dimensions;
using DXFInterface;
using System.Windows.Forms;



namespace BallastCalculator
{
    public static class FilePathContainer
    {
        public static string excelPath;
        public static string dxfPath;
        public static string outPath;
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
    }

    class Program
    {

      
        [STAThread]
        static void Main(string[] args)
            {
            ////GetUserInputs()
            //Console.WriteLine("Press Enter to Continue: ");
            ////Console.WriteLine("Copy and Paste the input file path");
            ////string IncomingFilePath = Console.ReadLine(); 
            ////IncomingFilePath = 
            //string file_path = @"C:\Users\Owner\Desktop\EcoLibriumSolar\Greenskies_Griswold Elem EF3_Ecolibrium Layout (WITH IFI PERIMETER) stripped down.dxf";
            //string output_path = "output";
            Application.Run(new UserGUI());
            //MessageBox.Show(FilePathContainer.dxfPath);
            //MessageBox.Show(FilePathContainer.excelPath);
            //MessageBox.Show(FilePathContainer.outPath);

            string file_path = FilePathContainer.dxfPath;
            string output_path = FilePathContainer.outPath;
            string excel_path = FilePathContainer.excelPath;
            //Console.WriteLine(output_path);
            //Console.WriteLine(file_path);
            //Console.ReadKey(); 
            DxfIO dxfInterface = new DxfIO(file_path, output_path);
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
                /*
                Console.WriteLine("Panel No. " + EcoPanel.PanelID);
                Console.WriteLine("Center: " + EcoPanel.Center);
                Console.WriteLine("Zone from the east: " + EcoPanel.NE_Zone);
                Console.WriteLine("Zone from the West: " + EcoPanel.NW_Zone);
                //Console.WriteLine("Ballast Location: " + EcoPanel.BallastLocation);
                Console.ReadLine();
                */
            }

            //Moved all function calls to constructor inside the PanelGrid Class 
            //1. PanelGrid PanelGrid
            //2. PanelGrid RunPanlCalculations
            //2. PanelGrid RunBasePanelCalculations 
            PanelGrid grid = new PanelGrid(BlockPerimeter, PanelList);
            IFIboarder.PrintIFIData();
            List<EcoPanel> list  = grid.GetPanels(); 
            foreach( EcoPanel panel in list  )
            {
                Console.WriteLine("Panel No. " + panel.PanelID);
                Console.WriteLine("Center: " + panel.Center);
                Console.WriteLine("Zone from the east: " + panel.NE_Zone);
                Console.WriteLine("Zone from the West: " + panel.NW_Zone);
                Console.WriteLine("Panel ID: {0}: ", panel.PanelID);
                Console.WriteLine(panel.DirectionList.Count);
                Console.WriteLine("Panel Direction List: {0}");
                Console.WriteLine("===================="); 
                foreach(var c in panel.DirectionList)
                {
                    Console.WriteLine(c); 
                }
                Console.WriteLine("Panel Neighbors: ");
                Console.WriteLine("==================="); 
                foreach(var neigh in panel.NeighborHood)
                {
                    Console.WriteLine(neigh.Center);

                }
                Console.ReadKey();

            }
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
            //string excel_filepath = @"C:\Users\Owner\Downloads\Threecocalcs 0_5_1.xlsx";
            //ExcelIO ExInterface = new ExcelIO(excel_filepath);
        }
    }
}



