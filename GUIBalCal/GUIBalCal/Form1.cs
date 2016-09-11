using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO; 

namespace GUIBalCal
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void dxfButton_Click(object sender, System.EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                dxfTextBox.Text = Path.GetFileName(openFileDialog1.FileName);
                FilePathContainer.dxfPath = openFileDialog1.FileName;

                string out_name = Path.GetFileNameWithoutExtension(openFileDialog1.FileName) + "_out.dxf";
                DefaultTextBox.Text = Path.GetFullPath(out_name);
                FilePathContainer.outPath = Path.GetFullPath(out_name);



            }
        }
        private void excelButton_Click(object sender, System.EventArgs e)
        {
          
            if (openFileDialog1.ShowDialog() == DialogResult.OK) // Test result.
            {
                excelTextBox.Text = Path.GetFileName(openFileDialog1.FileName);
                FilePathContainer.excelPath = openFileDialog1.FileName;

            }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        public void dxfTextBox_TextChanged(object sender, EventArgs e)
        {
            //dxf 
           

        }
        public void excelTextBox_TextChanged(object sender, EventArgs e)
        {
            //Excel
        }
        private void outPutButton_Click(object sender, EventArgs e)
        {
            //if (openFileDialog1.ShowDialog() == DialogResult.OK) // Test result.
            //{
            //    outPathText.Text = openFileDialog1.FileName;
            //    FilePathContainer.outPath = openFileDialog1.FileName;
            //} SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "AutoCad Format: (.dxf)|*.dxf|Text File Format: (.txt)|*.txt";
            saveFileDialog1.Title = "Create File";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK) // Test result.
            {

                DefaultTextBox.Text = Path.GetFullPath(saveFileDialog1.FileName);
                FilePathContainer.outPath = Path.GetFullPath(saveFileDialog1.FileName);

            }


        }
        private void outPathTextbox_TextChanged(object sender, EventArgs e)
        {
        }

        private void excelButton_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void DefaultTextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
