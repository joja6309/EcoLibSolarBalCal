using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;

namespace GUIBalCal
{
    class Program
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

            static void Main(string[] args)
            {

                // Create the GUIBalCal, that derives from ApplicationContext,
                // that manages when the application should exit.
                Application.EnableVisualStyles();
                Application.DoEvents();
                Application.Run(new Form1());
                MessageBox.Show(FilePathContainer.dxfPath);
                MessageBox.Show(FilePathContainer.excelPath);
                MessageBox.Show(FilePathContainer.outPath);



            }
        }
    }
}
