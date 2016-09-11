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

namespace BallastCalcGUI
{
    public partial class Form1 : Form
    {
        private Button button1;
        private Button button2;
        
        public Form1()
        {
            InitializeComponent();
        }
     
        private void button1_Click_1(object sender, EventArgs e)
        {
            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;
            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        string strfilename = openFileDialog1.InitialDirectory + openFileDialog1.FileName;
                        result = MessageBox.Show("file path", strfilename, buttons);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }

        }
        private void button2_Click_1(object sender, EventArgs e)
        {
            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;
            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        string strfilename = openFileDialog1.InitialDirectory + openFileDialog1.FileName;
                        result = MessageBox.Show("file path", strfilename, buttons);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }

        }
    }
}
