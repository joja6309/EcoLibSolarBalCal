using System;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Office.Excel;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Linq;

namespace WindowsWriteToExcelAPP
{


    public class Program
    {
        static void Main()
        {
            string sheet = "wind load calc_10d";
            string path = @"C:\Users\Owner\Desktop\Momentum_Harvest Outreach_3cocalcs 0_2.xlsx";

            ExcelDocument new_book = new ExcelDocument(path);

            var the_cell = new_book.ReadCell(sheet, "C39");
            string text = the_cell.InnerText;
            var cell2 = new_book.ReadCell(sheet, "C38");
            string text2 = cell2.InnerText;
            var cell3 = new_book.ReadCell(sheet, "F50");
            string text3  = cell3.InnerText;

            Console.WriteLine(text);
            Console.WriteLine(text2);
            Console.WriteLine(text3); 

            Console.ReadLine();

            new_book.Update();
            Console.WriteLine("Here");
            var input_tuple = new Tuple<string, uint>("C", 38);
            new_book.InsertText(sheet, input_tuple,"9");
            new_book.Refresh(); 
            var cell4 = new_book.ReadCell(sheet, "F50");
            string text4 = cell4.InnerText;
            Console.WriteLine(text4);

            Console.ReadLine(); 





        }




    }
}
