using System.IO;
using System;
using System.Diagnostics; 
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using Microsoft.Office.Interop.Excel;
using System.Collections.Generic;
using Dimensions;
using System.Threading.Tasks;
using System.Security.AccessControl;
using System.Security.Principal;

namespace ExcelInterface
{
    public class ExcelIO
    {
        private readonly string _filePath;
        private readonly string _firstSheetName; //KB 9.29.16 - Is this for the engineering inputs? Can we hard code tab name as with the read operations? 
        public bool def;
        public bool land;
        public double bal;
        private List<int> WODeflector_Refzones = new List<int>() { 103, 112, 121, 130, 139 };
        private List<int> WDeflector_Refzones = new List<int>() { 51, 60, 69, 78, 87 };
        public Tuple<string, uint> slidingCell = new Tuple<string, uint>("G", 39);
        public Tuple<string, uint> upliftCell = new Tuple<string, uint>("C", 39);
        public string referenceSheet;
        public string column;

        public ExcelIO(string filePath)
        {
            _filePath = filePath;

            using (
                SpreadsheetDocument excelDoc = SpreadsheetDocument.Open(_filePath, true))
            {
                _firstSheetName = excelDoc.WorkbookPart.Workbook.Descendants<Sheet>().ElementAt(1).Name;
            }
        }
        public void ProcessFirstSheet()
        {
            def = CheckFirst("C32");
            land = CheckFirst("C33");
            bal = GetBalast("B34");
            SetReferences();
        }
        public void SetReferences()
        {
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
        }
        public void Write_U_and_S_To_Sheet(string excel_file_path, string uplift, string sliding) //KB 9.29.16 inserts uplift and sliding cells into appropriate excel sheet
        {
            InsertText(excel_file_path, referenceSheet, upliftCell, uplift);
            InsertText(excel_file_path, referenceSheet, slidingCell, sliding);
            //Update();
            //Refresh();
            return;
        }
        public double CellIO(EcoPanel panel)
        {
            int startingCell_NE = 0;
            int startingCell_NW = 0;
            int nmod = 0;
            int e2wmod = 0;
            int w2emod = 0;
            int defmod = 0;
            Console.WriteLine(panel.PanelID);

            List<int> ColumnPositions = new List<int>();
            int IFINorth, IFISouth, IFIEast, IFIWest;
            if (land)
            {
                IFINorth = panel.IFI_NORTH_Land;
                IFIEast = panel.IFI_E2W_Land;
                IFISouth = panel.IFI_SOUTH_Land;
                IFIWest = panel.IFI_W2E_Land;
            }
            else
            {
                IFINorth = panel.IFI_NORTH_Port;
                IFIEast = panel.IFI_E2W_Port;
                IFISouth = panel.IFI_SOUTH_Port;
                IFIWest = panel.IFI_W2E_Port;

            }

            if (!def)
                defmod = 52;

            switch (panel.NW_Zone)
            {
                case 1:
                    startingCell_NW = WDeflector_Refzones[0];
                    break;
                case 2:
                    startingCell_NW = WDeflector_Refzones[1];
                    break;
                case 3:
                    startingCell_NW = WDeflector_Refzones[2];
                    break;
                case 4:
                    startingCell_NW = WDeflector_Refzones[3];
                    break;
                case 5:
                    startingCell_NW = WDeflector_Refzones[4];
                    break;
            }

            switch (panel.NE_Zone)
            {
                case 1:
                    startingCell_NE = WDeflector_Refzones[0];
                    break;
                case 2:
                    startingCell_NE = WDeflector_Refzones[1];
                    break;
                case 3:
                    startingCell_NE = WDeflector_Refzones[2];
                    break;
                case 4:
                    startingCell_NE = WDeflector_Refzones[3];
                    break;
                case 5:
                    startingCell_NE = WDeflector_Refzones[4];
                    break;
            }

            switch (IFINorth)
            {
                case 0:
                    nmod = 0;
                    break;
                case 1:
                    nmod = 2;
                    break;
                case 2:
                    nmod = 4;
                    break;
            }

            switch (IFIEast)
            {
                case 1:
                    {
                        e2wmod = 0;
                        int tempe = startingCell_NE + defmod + nmod + e2wmod;
                        ColumnPositions.Add(tempe);
                        break;
                    }
                case 2:
                    {
                        e2wmod = 1;
                        int tempe = startingCell_NE + defmod + nmod + e2wmod;
                        ColumnPositions.Add(tempe);
                        break;
                    }
            }

            switch (IFIWest)
            {
                case 1:
                    {
                        w2emod = 0;
                        int tempw = startingCell_NW + defmod + nmod + w2emod;
                        ColumnPositions.Add(tempw);
                        break;
                    }
                case 2:
                    {
                        w2emod = 1;
                        int tempw = startingCell_NW + defmod + nmod + w2emod;
                        ColumnPositions.Add(tempw);
                        break;
                    }
            }

            if (IFISouth == 0)
            {
                int tempsw = startingCell_NW + defmod + 6 + w2emod;
                int tempse = startingCell_NE + defmod + 6 + e2wmod;
                ColumnPositions.Add(tempse);
                ColumnPositions.Add(tempsw);
            }

            List<double> Results = new List<double>();
            foreach (var position in ColumnPositions)
            {
                var return_cell = ReadCell(referenceSheet, column + position.ToString());
                Console.WriteLine(return_cell);
                Results.Add(Convert.ToDouble(return_cell));
            }


            double final_value = Results.Max();

            return final_value;
        }
        public bool CheckFirst(string cellCo)
        {
            int outInt = 0;
            using (SpreadsheetDocument excelDoc = SpreadsheetDocument.Open(_filePath, true))
            {
                WorksheetPart wp = GetWorksheetPart(excelDoc, _firstSheetName);
                Cell outPutCell = GetCell(wp, cellCo);
                var Cell = outPutCell.CellValue;
                string value = Cell.InnerText.ToString();
                outInt = Convert.ToInt32(value);
            }
            if (outInt.Equals(0))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public double GetBalast(string cellCo)
        {
            string doubleCell = null;
            using (SpreadsheetDocument excelDoc = SpreadsheetDocument.Open(_filePath, true))
            {
                WorksheetPart wp = GetWorksheetPart(excelDoc, _firstSheetName);
                Cell outPutCell = GetCell(wp, cellCo);
                var value = outPutCell.CellValue;
                doubleCell = value.InnerText.ToString();
            }

            return Convert.ToDouble(doubleCell);

        }
        private WorksheetPart GetWorksheetPart(SpreadsheetDocument excelDoc, string sheetName)
        {
            Sheet sheet = excelDoc.WorkbookPart.Workbook.Descendants<Sheet>().SingleOrDefault(s => s.Name == sheetName);
            if (sheet == null)
            {
                throw new ArgumentException(
                    String.Format("No sheet named {0} found in spreadsheet {1}", sheetName, _filePath), "sheetName");
            }
            return (WorksheetPart)excelDoc.WorkbookPart.GetPartById(sheet.Id);
        }
        public string ReadCell(string sheetName, string cellCoordinates)
        {
            string string_cell = null;

            using (SpreadsheetDocument excelDoc = SpreadsheetDocument.Open(_filePath, true))
            {
                WorksheetPart wbpart = GetWorksheetPart(excelDoc, sheetName);
                Cell cell = GetCell(excelDoc, sheetName, cellCoordinates);

                string_cell = cell.CellValue.InnerText.ToString();

            }
            if (string_cell != null)
            {
                return string_cell;

            }
            else
            {
                return "0";
            }
        }
        public void InsertText(string filePath, string sheetName, Tuple<string, uint> cellCO, string text)
        {
            // Open the document for editing.


            using (SpreadsheetDocument excelDoc = SpreadsheetDocument.Open(filePath, true))
            {
                //Get the SharedStringTablePart.If it does not exist, create a new one.

                SharedStringTablePart shareStringPart;
                if (excelDoc.WorkbookPart.GetPartsOfType<SharedStringTablePart>().Count() > 0)
                {
                    shareStringPart = excelDoc.WorkbookPart.GetPartsOfType<SharedStringTablePart>().First();
                }
                else
                {
                    shareStringPart = excelDoc.WorkbookPart.AddNewPart<SharedStringTablePart>();
                }

                // Insert the text into the SharedStringTablePart.
                int index = InsertSharedStringItem(text, shareStringPart);

                // Insert a new worksheet.
                WorksheetPart worksheetPart = GetWorksheetPart(excelDoc, sheetName);
                string column = cellCO.Item1;
                uint row = cellCO.Item2;
                // Insert cell A1 into the new worksheet.
                Cell cell = InsertCellInWorksheet(column, row, worksheetPart);

                // Set the value of cell A1.
                cell.CellValue = new CellValue(index.ToString());
                cell.DataType = new EnumValue<CellValues>(CellValues.SharedString);

                // Save the new worksheet.
                worksheetPart.Worksheet.Save();
            }
           
        }
        // Given text and a SharedStringTablePart, creates a SharedStringItem with the specified text 
        // and inserts it into the SharedStringTablePart. If the item already exists, returns its index.
        private static int InsertSharedStringItem(string text, SharedStringTablePart shareStringPart)
        {
            // If the part does not contain a SharedStringTable, create one.
            if (shareStringPart.SharedStringTable == null)
            {
                shareStringPart.SharedStringTable = new SharedStringTable();
            }

            int i = 0;

            // Iterate through all the items in the SharedStringTable. If the text already exists, return its index.
            foreach (SharedStringItem item in shareStringPart.SharedStringTable.Elements<SharedStringItem>())
            {
                if (item.InnerText == text)
                {
                    return i;
                }

                i++;
            }

            // The text does not exist in the part. Create the SharedStringItem and return its index.
            shareStringPart.SharedStringTable.AppendChild(new SharedStringItem(new DocumentFormat.OpenXml.Spreadsheet.Text(text)));
            shareStringPart.SharedStringTable.Save();

            return i;
        }
        private bool GrantAccess(string fullPath)
        {
            DirectoryInfo dInfo = new DirectoryInfo(fullPath);
            DirectorySecurity dSecurity = dInfo.GetAccessControl();
            dSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
            dInfo.SetAccessControl(dSecurity);
            return true;
        }
        private void DeleteDirectory(string desktop_path)
        {
            File.SetAttributes(desktop_path, FileAttributes.Normal);
            DirectoryInfo dir = new DirectoryInfo(desktop_path);


            foreach (FileInfo file in dir.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dirc in dir.GetDirectories())
            {
                dirc.Delete(true);
            }
        }
        private string BuildHiddenDirectories(List<EcoPanel> PanelList)
        {
            var desktop_path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            desktop_path = desktop_path + @"\ExcelCopies";
            if (Directory.Exists(desktop_path))
            {

                DeleteDirectory(desktop_path);
            }
            DirectoryInfo di = Directory.CreateDirectory(desktop_path);
            foreach (var panel in PanelList)
            {
                File.Copy(_filePath, Path.Combine(desktop_path, panel.PanelID.ToString() + Path.GetFileName(_filePath)));
                panel.ExcelFilePath = Path.Combine(desktop_path, panel.PanelID.ToString() + Path.GetFileName(_filePath));

            }
            return desktop_path;

        }
        private void RunPolyExcelReadandWrite(List<EcoPanel> PanelList)
        {


            Parallel.ForEach<EcoPanel>(PanelList, panel => ReadandWrite(panel));

            Console.WriteLine("Results: ");
            foreach (EcoPanel panel in PanelList)
            {
                Console.WriteLine("=============");
                Console.WriteLine(panel.PanelID);
                Console.WriteLine(panel.ValueFromExcel);
                Console.WriteLine("=============");
            }
            Console.ReadKey();

        }
        public void ReadandWrite(EcoPanel panel)
        {
            using (SpreadsheetDocument excelDoc = SpreadsheetDocument.Open(panel.ExcelFilePath, true))
            {
                excelDoc.WorkbookPart.Workbook.CalculationProperties.ForceFullCalculation = true;
                excelDoc.WorkbookPart.Workbook.CalculationProperties.FullCalculationOnLoad = true;
            }
            var excelApp = new Application();
            if (land)
            {
                //KB DEBUG: uplift and sliding were backwards, fixed it.
                Write_U_and_S_To_Sheet(panel.ExcelFilePath, panel.Uplift.ToString(), panel.Sliding.ToString());
                var workbook = excelApp.Workbooks.Open(panel.ExcelFilePath, true);
                workbook.Save();
                workbook.Close(true);
                panel.ValueFromExcel = CellIO(panel);

            }
            else
            {
                //KB DEBUG: uplift and sliding were backwards, fixed it.
                Write_U_and_S_To_Sheet(panel.ExcelFilePath, panel.Uplift.ToString(), panel.Sliding.ToString());
                var workbook = excelApp.Workbooks.Open(panel.ExcelFilePath, true);
                workbook.Save();
                workbook.Close(true);
                panel.ValueFromExcel = CellIO(panel);

            }
        }

        //Console.WriteLine("Panel ID{0}", panel.PanelID); 
        //Console.WriteLine("Panel")

    

        public void RUNIO(bool land, List<EcoPanel> PanelList)//KB 9.29.16 forces sheet to recalculate on open, opens excel app, for each panel writes to excel, opens workbook, saves, closes workbook (this updates equation values), then closes excel app when done.
        {
            //using (SpreadsheetDocument excelDoc = SpreadsheetDocument.Open(_filePath, true))
            //{
            //    excelDoc.WorkbookPart.Workbook.CalculationProperties.ForceFullCalculation = true;
            //    excelDoc.WorkbookPart.Workbook.CalculationProperties.FullCalculationOnLoad = true;
            //}
            
           string directory_path =  BuildHiddenDirectories(PanelList);
           
            RunPolyExcelReadandWrite(PanelList);

            DeleteDirectory(directory_path);
          
        } 
        private Cell GetCell(SpreadsheetDocument excelDoc, string sheetName, string cellCoordinates)
        {
            WorksheetPart worksheetPart = GetWorksheetPart(excelDoc, sheetName);
            return GetCell(worksheetPart, cellCoordinates);
        }
        private Cell GetCell(WorksheetPart worksheetPart, string cellCoordinates)
        {
            int rowIndex = int.Parse(cellCoordinates.Substring(1));
            Row row = GetRow(worksheetPart, rowIndex);

            Cell cell = row.Elements<Cell>().FirstOrDefault(c => cellCoordinates.Equals(c.CellReference.Value));
            if (cell == null)
            {
                throw new ArgumentException(String.Format("Cell {0} not found in spreadsheet", cellCoordinates));
            }
            return cell;
        }
        private Row GetRow(WorksheetPart worksheetPart, int rowIndex)
        {
            Row row = worksheetPart.Worksheet.GetFirstChild<SheetData>().
                                    Elements<Row>().FirstOrDefault(r => r.RowIndex == rowIndex);
            if (row == null)
            {
                throw new ArgumentException(String.Format("No row with index {0} found in spreadsheet", rowIndex));
            }
            return row;
        }
        private static Cell InsertCellInWorksheet(string columnName, uint rowIndex, WorksheetPart worksheetPart)
        {
            var worksheet = worksheetPart.Worksheet;
            SheetData sheetData = worksheet.GetFirstChild<SheetData>();
            string cellReference = columnName + rowIndex;

            // If the worksheet does not contain a row with the specified row index, insert one.
            Row row;
            if (sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).Count() != 0)
            {
                row = sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).First();
            }
            else
            {
                row = new Row() { RowIndex = rowIndex };
                sheetData.Append(row);
            }

            // If there is not a cell with the specified column name, insert one.  
            if (row.Elements<Cell>().Where(c => c.CellReference.Value == columnName + rowIndex).Count() > 0)
            {
                return row.Elements<Cell>().Where(c => c.CellReference.Value == cellReference).First();
            }
            else
            {
                // Cells must be in sequential order according to CellReference. Determine where to insert the new cell.
                Cell refCell = null;
                foreach (Cell cell in row.Elements<Cell>())
                {
                    if (string.Compare(cell.CellReference.Value, cellReference, true) > 0)
                    {
                        refCell = cell;
                        break;
                    }
                }

                Cell newCell = new Cell() { CellReference = cellReference };
                row.InsertBefore(newCell, refCell);

                worksheet.Save();
                return newCell;
            }
        }
    }
}

/// <see cref="IExcelDocument.UpdateCell" />
//      public void UpdateCell(string sheetName, string cellCoordinates, object cellValue)
//      {
//          using (SpreadsheetDocument excelDoc = SpreadsheetDocument.Open(_filePath, true))
//          {
//              // tell Excel to recalculate formulas next time it opens the doc
//              excelDoc.WorkbookPart.Workbook.CalculationProperties.ForceFullCalculation = true;
//              excelDoc.WorkbookPart.Workbook.CalculationProperties.FullCalculationOnLoad = true;
//
//              WorksheetPart worksheetPart = GetWorksheetPart(excelDoc, sheetName);
//              Cell cell = GetCell(worksheetPart, cellCoordinates);
//              cell.CellValue = new CellValue(cellValue.ToString());
//              worksheetPart.Worksheet.Save();
//          }
//          
//      }
///
//      public void Update()
//      {
//          using (SpreadsheetDocument excelDoc = SpreadsheetDocument.Open(_filePath, true))
//          {
//              excelDoc.WorkbookPart.Workbook.CalculationProperties.ForceFullCalculation = true;
//              excelDoc.WorkbookPart.Workbook.CalculationProperties.FullCalculationOnLoad = true;
//              excelDoc.WorkbookPart.Workbook.Save();
//
//
//          }
//      }
/// <summary>Refreshes an Excel document by opening it and closing in background by the Excep Application</summary>
/// <see cref="IExcelDocument.Refresh" />
//       public void Refresh()
//       {
//           var excelApp = new Application();
//           var workbook = excelApp.Workbooks.Open(_filePath,true );
//           workbook.Close(true);
//           excelApp.Quit();
//       }

