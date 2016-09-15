using System.IO;
using System;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using Microsoft.Office.Interop.Excel;
using System.Collections.Generic;


namespace ExcelInterface
{
    public class ExcelIO
    {
        private readonly string _filePath;
        private readonly string _firstSheetName;
        public bool def;
        public bool land;
        public double bal;

        private List<int> WODeflector_Refzones = new List<int>() { 103, 112, 121, 130, 139 };
        private List<int> WDeflector_Refzones = new List<int>() { 51, 60, 69, 78, 87 };
        //KB DEBUG: corrected sliding and uplift cell values from 38 to 39
        private Tuple<string, uint> slidingCell = new Tuple<string, uint>("G", 39);
        private Tuple<string, uint> upliftCell = new Tuple<string, uint>("C", 39);
        //List<int> WOBallasrreference_zones = new List<int>() {}
        private string referenceSheet;
        private string column;

        public ExcelIO(string filePath)
        {
            _filePath = filePath;
            using (SpreadsheetDocument excelDoc = SpreadsheetDocument.Open(_filePath, true))
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
        public void WritetoSandU(string uplift, string sliding)
        {
            InsertText(referenceSheet, upliftCell, uplift);
            InsertText(referenceSheet, slidingCell, sliding);
            Update();
            return;
        }

        public double CellIO(int NE_Zone, int NW_Zone, int IFINorth, int IFISouth, int IFIEast, int IFIWest)
        {

            int startingCell_NE = 0;
            int startingCell_NW = 0;
            //Console.WriteLine(def);
            //Console.WriteLine(bal);
            //Console.WriteLine(land);
            //Console.ReadKey(); 
            List<int> ColumnPositions = new List<int>();


            if (def)
            {   // reference sheet 10d at correct zone 
                //KB DEBUG: corrected with or without deflector call
                startingCell_NE = WDeflector_Refzones[NE_Zone - 1];
                startingCell_NW = WDeflector_Refzones[NW_Zone - 1];
            }
            else
            {   // Same Row Reference Array if sheet 5d at correct zone 
                //KB DEBUG: corrected with or without deflector call
                startingCell_NE = WODeflector_Refzones[NE_Zone - 1];
                startingCell_NW = WODeflector_Refzones[NW_Zone - 1];

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


            if (IFINorth == 0)
            {
                int temp_cell_West = startingCell_NW + 1;
                int temp_cell_East = startingCell_NE + 1;

                if (IFIEast.Equals(2))
                {
                    temp_cell_East = startingCell_NE + 1;


                }
                else if (IFIEast.Equals(2))
                {
                    temp_cell_West = startingCell_NW + 1;
                }
                ColumnPositions.Add(temp_cell_West);
                ColumnPositions.Add(temp_cell_East);

            }
            else if (IFISouth == 0 && IFINorth != 0)
            {
                if (IFINorth == 1)
                {
                    int temp_cell_West = startingCell_NW + 1;
                    int temp_cell_East = startingCell_NE + 1;
                    if (IFIEast.Equals(2))
                    {
                        temp_cell_West = startingCell_NE + 1;


                    }
                    else if (IFIEast.Equals(2))
                    {
                        temp_cell_East = startingCell_NW + 1;
                    }
                    ColumnPositions.Add(temp_cell_West);
                    ColumnPositions.Add(temp_cell_East);

                }
                else if (IFINorth == 2)
                {
                    int temp_cell_West = startingCell_NW + 2;
                    int temp_cell_East = startingCell_NE + 2;
                    if (IFIEast.Equals(2))
                    {
                        temp_cell_West = startingCell_NE + 1;


                    }
                    else if (IFIEast.Equals(2))
                    {
                        temp_cell_East = startingCell_NW + 1;
                    }
                    ColumnPositions.Add(temp_cell_West);
                    ColumnPositions.Add(temp_cell_East);
                }

            }
            else if (IFISouth == 1 | IFISouth == 0)
            {
                int temp_cell_West = startingCell_NW + 6;
                int temp_cell_East = startingCell_NE + 6;
                if (IFIEast.Equals(2))
                {
                    temp_cell_West = startingCell_NE + 1;

                }
                else if (IFIEast.Equals(2))
                {
                    temp_cell_East = startingCell_NW + 1;
                }
                ColumnPositions.Add(temp_cell_West);
                ColumnPositions.Add(temp_cell_East);

            }


            List<double> Results = new List<double>();
            foreach (var position in ColumnPositions)
            {
                var return_cell = ReadCell(referenceSheet, column + position.ToString());
                Results.Add(Convert.ToDouble(return_cell));
            }
            foreach (var x in Results)
            {
                Console.WriteLine(x);
            }
            Console.WriteLine("==========================");
            Console.ReadKey();
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

        public void Update()
        {
            using (SpreadsheetDocument excelDoc = SpreadsheetDocument.Open(_filePath, true))
            {
                // tell Excel to recalculate formulas next time it opens the doc
                excelDoc.WorkbookPart.Workbook.CalculationProperties.ForceFullCalculation = true;
                excelDoc.WorkbookPart.Workbook.CalculationProperties.FullCalculationOnLoad = true;
                excelDoc.WorkbookPart.Workbook.Save();
            }
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

        /// <see cref="IExcelDocument.ReadCell" />
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

        /// <see cref="IExcelDocument.InsertSharedStringItem(string, object)" />

        public void InsertText(string sheetName, Tuple<string, uint> cellCO, string text)
        {

            // Open the document for editing.
            using (SpreadsheetDocument excelDoc = SpreadsheetDocument.Open(_filePath, true))
            {
                // Get the SharedStringTablePart. If it does not exist, create a new one.
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

        /// <see cref="IExcelDocument.UpdateCell" />
        public void UpdateCell(string sheetName, string cellCoordinates, object cellValue)
        {
            using (SpreadsheetDocument excelDoc = SpreadsheetDocument.Open(_filePath, true))
            {
                // tell Excel to recalculate formulas next time it opens the doc
                excelDoc.WorkbookPart.Workbook.CalculationProperties.ForceFullCalculation = true;
                excelDoc.WorkbookPart.Workbook.CalculationProperties.FullCalculationOnLoad = true;

                WorksheetPart worksheetPart = GetWorksheetPart(excelDoc, sheetName);
                Cell cell = GetCell(worksheetPart, cellCoordinates);
                cell.CellValue = new CellValue(cellValue.ToString());
                worksheetPart.Worksheet.Save();
            }
        }

        /// <summary>Refreshes an Excel document by opening it and closing in background by the Excep Application</summary>
        /// <see cref="IExcelDocument.Refresh" />
        public void Refresh()
        {
            var excelApp = new Application();
            var workbook = excelApp.Workbooks.Open(Path.GetFullPath(_filePath));
            workbook.Close(true);
            excelApp.Quit();
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


