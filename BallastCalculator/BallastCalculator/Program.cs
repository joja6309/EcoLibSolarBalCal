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
            MessageBox.Show(FilePathContainer.dxfPath);
            MessageBox.Show(FilePathContainer.excelPath);
            MessageBox.Show(FilePathContainer.outPath);

            string file_path = FilePathContainer.dxfPath;
            string output_path = FilePathContainer.outPath;
            string excel_path = FilePathContainer.excelPath; 

            DxfIO dxfInterface = new DxfIO(file_path, output_path);
            BasicDimensions BlockPerimeter = dxfInterface.InParse.GetValuesFromBlockSection();
            IFIPerimeter IFIboarder = dxfInterface.InParse.GetIFIValues();
            List<EcoPanel> PanelList = dxfInterface.InParse.GetEntitiesPanels();
            BlockPerimeter.CalculateCenter();
            IFIboarder.CalculateCenter();
            IFIboarder.SetCorners();

            foreach (EcoPanel EcoPanel in PanelList)
            {
                EcoPanel.CalculatePanelCenter(BlockPerimeter.Center.Item1, BlockPerimeter.Center.Item2);
                EcoPanel.SetPanelZones(IFIboarder);
            }

                //Moved all function calls to constructor inside the PanelGrid Class 
                //1. PanelGrid PanelGrid
                //2. PanelGrid RunPanlCalculations
                //2. PanelGrid RunBasePanelCalculations 
                PanelGrid grid = new PanelGrid(BlockPerimeter, PanelList);



                //IFIboarder.PrintIFIData();
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




//Console.WriteLine("Input (N) radius that should be checked: ");

//string input_string = Console.ReadLine();

//int input_n = 0;

//if (string.IsNullOrEmpty(input_string))
//{
//    input_n = 3;

//}
//else
//{
//    input_n = Convert.ToInt32(input_string);

//}




//foreach (EcoPanel EcoPanel in panel_list)
//{
//    Console.WriteLine(EcoPanel.PanelID);

//    foreach (var x in EcoPanel.NeighborHood)
//    {
//        Console.WriteLine(x);
//    }
//    foreach (var y in EcoPanel.DirectionList)
//    {
//        Console.WriteLine(y);
//    }

//    Console.ReadKey();
//}




//Reference Sheet Set Based on Land or Portrait 

//Excel Parsing Class
//Get First Sheet Data 
//string firstSheet = "1-Eng Inputs";
//string deflectorCell = "C32";
//string modCell = "C33";
//string balCell = "B34";
//Tuple<string, uint> slidingCell = new Tuple<string,uint>("G" ,38);
//Tuple<string, uint> upliftCell = new Tuple<string, uint>("C", 38); 
//bool deflector = ExInterface.CheckFirst(firstSheet, deflectorCell);
//bool landscape = ExInterface.CheckFirst(firstSheet, modCell);
//double bal = ExInterface.GetBalast(firstSheet, balCell);
//Console.WriteLine(1);
//List<int> WODeflector_Refzones = new List<int>() { 103, 112, 121, 130, 139 };
//List<int> WDeflector_Refzones = new List<int>() { 51, 60, 69, 78, 87 };
////List<int> WOBallasrreference_zones = new List<int>() {}
//string column = null; 
//string referenceSheet = null; 
//if(landscape)
//{
//    referenceSheet = "wind load calc_10d";
//    column = "M";
//}
//else
//{
//    referenceSheet = "wind load calc_5d";
//    column = "K";
//}

//foreach (EcoPanel EcoPanel in PanelList)
//{
//    int startingCell_NE = 0;
//    int startingCell_NW = 0;
//    List<int> ColumnPositions = new List<int>();
//    ExInterface.InsertText(referenceSheet, upliftCell, EcoPanel.Sliding.ToString());
//    ExInterface.InsertText(referenceSheet, slidingCell, EcoPanel.Uplift.ToString()); 
//    ExInterface.Update();

//    if (deflector)
//    {   // reference sheet 10d at correct zone 
//        startingCell_NE = WDeflector_Refzones[EcoPanel.NE_Zone - 1 ];
//        startingCell_NW = WODeflector_Refzones[EcoPanel.NW_Zone - 1 ];
//    }
//    else
//    {   // Same Row Reference Array if sheet 5d at correct zone 
//        startingCell_NE = WDeflector_Refzones[EcoPanel.NE_Zone - 1 ];
//        startingCell_NW = WDeflector_Refzones[EcoPanel.NW_Zone - 1];

//    }
//    // N0 S0 both
//    // S0 1N only south
//    // S1 just north

//    // NEZone -> E2W
//    // NWZone -> W2E
//    //N0
//    //N1 S1
//    //N2 S1
//    //S0
//    if (landscape)
//    {

//        if (EcoPanel.IFI_NORTH_Land == 0)
//        {
//            int temp_cell_West = startingCell_NW + 1;
//            int temp_cell_East = startingCell_NE + 1;

//            if (EcoPanel.IFI_E2W_Land.Equals(2))
//            {
//                temp_cell_East = startingCell_NE + 1;


//            }
//            else if (EcoPanel.IFI_E2W_Land.Equals(2))
//            {
//                temp_cell_West = startingCell_NW + 1;
//            }
//            ColumnPositions.Add(temp_cell_West);
//            ColumnPositions.Add(temp_cell_East);

//        }
//        else if (EcoPanel.IFI_SOUTH_Land == 0 && EcoPanel.IFI_NORTH_Land != 0)
//        {
//            if (EcoPanel.IFI_NORTH_Land == 1)
//            {
//                int temp_cell_West = startingCell_NW + 1;
//                int temp_cell_East = startingCell_NE + 1;
//                if (EcoPanel.IFI_E2W_Land.Equals(2))
//                {
//                    temp_cell_West = startingCell_NE + 1;


//                }
//                else if (EcoPanel.IFI_E2W_Land.Equals(2))
//                {
//                    temp_cell_East = startingCell_NW + 1;
//                }
//                ColumnPositions.Add(temp_cell_West);
//                ColumnPositions.Add(temp_cell_East);

//            }
//            else if (EcoPanel.IFI_NORTH_Land == 2)
//            {
//                int temp_cell_West = startingCell_NW + 2;
//                int temp_cell_East = startingCell_NE + 2;
//                if (EcoPanel.IFI_E2W_Land.Equals(2))
//                {
//                    temp_cell_West = startingCell_NE + 1;


//                }
//                else if (EcoPanel.IFI_E2W_Land.Equals(2))
//                {
//                    temp_cell_East = startingCell_NW + 1;
//                }
//                ColumnPositions.Add(temp_cell_West);
//                ColumnPositions.Add(temp_cell_East);

//            }

//        }
//        else if (EcoPanel.IFI_SOUTH_Land == 1 | EcoPanel.IFI_SOUTH_Land == 0)
//        {
//            int temp_cell_West = startingCell_NW + 6;
//            int temp_cell_East = startingCell_NE + 6;
//            if (EcoPanel.IFI_E2W_Land.Equals(2))
//            {
//                temp_cell_West = startingCell_NE + 1;


//            }
//            else if (EcoPanel.IFI_E2W_Land.Equals(2))
//            {
//                temp_cell_East = startingCell_NW + 1;
//            }
//            ColumnPositions.Add(temp_cell_West);
//            ColumnPositions.Add(temp_cell_East);

//        }

//    }
//    List<double> Results = new List<double>();
//    foreach (var position in ColumnPositions)
//    {
//        var return_cell = ExInterface.ReadCell(referenceSheet, column + position.ToString());
//        Results.Add(Convert.ToDouble(return_cell));
//    }
//    double final_value = Results.Max();
//    foreach(var x in Results)
//    {
//        Console.WriteLine(x);


//    }
//    EcoPanel.ValueFromExcel = final_value;
//}

// land = 10 deg 
// port = 5 deg 

//deflector 
// - pair 8 conditions 
//- pair 8 conditions 

//Write to Uplift 
//Write to Sliding 

//InOut Excel
//Landscape or Portrait     --> What cells to reference in file 
//With without deflectors  --> 

//Use roof zone 
// East 2 West true col -> with give us 
//West 2 East true col 
//IFI North 
//IFI South 

//
//private void Write_to_file(IFIPerimeter perm)
//{
//    string current_directory = Directory.GetCurrentDirectory();
//    string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
//    using (StreamWriter outputFile = new StreamWriter(mydocpath + @"\Output.txt"))
//    {
//        outputFile.Write("perm SECTION VALUES ");
//        outputFile.Write(Environment.NewLine);

//        outputFile.Write("====================");
//        outputFile.Write(Environment.NewLine);

//        outputFile.Write("Corner 1: {0} ", BlockPerimeter.Corner1);
//        outputFile.Write(Environment.NewLine
//            );

//        outputFile.Write("Corner 2: {0} ", BlockPerimeter.Corner2);
//        outputFile.Write(Environment.NewLine);

//        outputFile.Write("Corner 3: {0} ", BlockPerimeter.Corner3);
//        outputFile.Write(Environment.NewLine);

//        outputFile.Write("Corner 4: {0} ", BlockPerimeter.Corner4);
//        outputFile.Write(Environment.NewLine);

//        outputFile.Write("Center : {0}", BlockPerimeter.Center);
//        outputFile.Write(Environment.NewLine);

//        outputFile.Write("perm Dimensions: ");
//        outputFile.Write(Environment.NewLine);

//        outputFile.Write("==================");
//        outputFile.Write(Environment.NewLine);

//        outputFile.Write("Width: {0}", BlockPerimeter.Width);
//        outputFile.Write(Environment.NewLine);

//        outputFile.Write("Height: {0}", BlockPerimeter.Height);
//        outputFile.Write(Environment.NewLine);


//        outputFile.Write("IFI Values:");
//        outputFile.Write(Environment.NewLine);

//        outputFile.Write("============");
//        outputFile.Write(Environment.NewLine);

//        outputFile.Write("Corner 1: {0}", IFIBoarder.Corner1);
//        outputFile.Write(Environment.NewLine);

//        outputFile.Write("Corner 2: {0}", IFIBoarder.Corner2);
//        outputFile.Write(Environment.NewLine);

//        outputFile.Write("Corner 3: {0}", IFIBoarder.Corner3);
//        outputFile.Write(Environment.NewLine);

//        outputFile.Write("Corner 4: {0}", IFIBoarder.Corner4);
//        outputFile.Write(Environment.NewLine);


//        outputFile.Write("IFI Dimensions: ");
//        outputFile.Write(Environment.NewLine);

//        outputFile.Write("==================");
//        outputFile.Write(Environment.NewLine);

//        outputFile.Write("Width: {0}", IFIBoarder.Width);
//        outputFile.Write(Environment.NewLine);

//        outputFile.Write("Height: {0}", IFIBoarder.Height);
//        outputFile.Write(Environment.NewLine);


//        outputFile.Write("EcoPanel/Entities Values:");
//        outputFile.Write(Environment.NewLine);

//        outputFile.Write("======================");
//        outputFile.Write(Environment.NewLine);


//        int count2 = 0;
//        foreach (var x in PanelList)
//        {
//            outputFile.Write("EcoPanel Number {0}: ", count2);
//            outputFile.Write(Environment.NewLine);

//            outputFile.Write("X value: {0}", x.Xvalue.ToString());
//            outputFile.Write(Environment.NewLine);

//            outputFile.Write("Y Value: {0} ", x.Yvalue.ToString());
//            outputFile.Write(Environment.NewLine);

//            outputFile.Write("Center Value: {0}", x.Center.ToString());
//            outputFile.Write(Environment.NewLine);

//            outputFile.Write("EcoPanel NE_Zone: {0}", x.NE_Zone.ToString());
//            outputFile.Write(Environment.NewLine);


//            count2 += 1;
//        }

//        outputFile.Write("IFI Corners: ");
//        outputFile.Write(Environment.NewLine);

//        outputFile.Write("North East Corner: {0}", perm.NE_corner);
//        outputFile.Write(Environment.NewLine);

//        outputFile.Write("North west Corner: {0}", perm.NW_corner);
//        outputFile.Write(Environment.NewLine);

//    }


//}

//5 and 9 (n) value check 
// Neighboring lift modules 5 
// Neighboring sliding modules 8 



