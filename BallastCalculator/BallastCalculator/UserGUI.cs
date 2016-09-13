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

namespace BallastCalculator
{
    public partial class UserGUI : Form
    {
       
            public UserGUI()
            {
                InitializeComponent();
            }
        private string panel_name;
        private string excel_path;
        private string dxf_path;
        private string default_out; 
        private bool useDefaults = false;
            private void dxfButton_Click(object sender, System.EventArgs e)
            {
            if (openFileDialog1.ShowDialog() == DialogResult.OK) // Test result.
            {



                dxfTextBox.Text = Path.GetFileName(openFileDialog1.FileName);
                    FilePathContainer.dxfPath = openFileDialog1.FileName;
                string wanted_path = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
                dxf_path = wanted_path + "/" + openFileDialog1.FileName;
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    string out_name = Path.GetFileNameWithoutExtension(openFileDialog1.FileName) + "_out.dxf";
                    path = path + "/" + out_name;
                    DefaultTextBox.Text = Path.GetFullPath(out_name);
                    
                    FilePathContainer.outPath = Path.GetFullPath(path);
                
                }
            }
            private void excelButton_Click(object sender, System.EventArgs e)
            {

                if (openFileDialog1.ShowDialog() == DialogResult.OK) // Test result.
                {
                    excelTextBox.Text = Path.GetFileName(openFileDialog1.FileName);
                string wanted_path = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
                excel_path = wanted_path = "/" + openFileDialog1.FileName; 
                FilePathContainer.excelPath = openFileDialog1.FileName;

                }

            }

        private void Form1_Load(object sender, EventArgs e)
        {
            //File.Exists(curFile) ? "File exists." : "File does not exist.");
            PrepareTooltips();

            string strPath = Environment.GetFolderPath(
                     System.Environment.SpecialFolder.DesktopDirectory);
            string final_path = strPath + "/" + "saved_paths.txt";
            if (File.Exists(final_path))
            {
                var _inputFile = File.ReadAllLines(final_path);
                foreach (var line in _inputFile)
                {
                    if (line.Contains("Ex:"))
                    {
                        excel_path = line.Substring(line.LastIndexOf(':') + 1);
                        textBox6.Text = excel_path;
                    }
                    else if (line.Contains("Dx:"))
                    {
                        dxf_path = line.Substring(line.LastIndexOf(':') + 1);
                        textBox7.Text = dxf_path;
                    }
                    else if (line.Contains("Panel:"))
                    {
                         panel_name = line.Substring(line.LastIndexOf(':') + 1);
                         textBox8.Text = panel_name;

                    }
                    else if (line.Contains("Def:"))
                    {
                        default_out = line.Substring(line.LastIndexOf(':') + 1);
                        DefaultTextBox.Text = default_out;
                    }
                }

            }
            else
            {
                textBox6.Text = "Excel Path Not Saved: ";
                textBox7.Text = "DXF Path Not Saved: ";
                textBox8.Text = "Panel Name Not Saved: ";


            }



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
            string strPath = Environment.GetFolderPath(
                    System.Environment.SpecialFolder.DesktopDirectory);
            string final_path = strPath + "/" + "saved_paths.txt";
            if (!File.Exists(final_path))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(final_path))
                {
                    sw.WriteLine("Ex:{0}", excel_path);
                    sw.WriteLine("Dx:{0}",dxf_path );
                    sw.WriteLine("Panel:{0}",panel_name );
                    sw.Flush();
                    sw.Close(); 
                }
            }
            if(useDefaults)
            {
                //MessageBox.Show(panel_name + " " + excel_path + " " + Defa)
                FilePathContainer.panelName = panel_name;
                FilePathContainer.excelPath = Path.GetFullPath(excel_path);
                FilePathContainer.outPath = default_out;
                FilePathContainer.dxfPath = Path.GetFullPath(dxf_path); 
            }
            else
            {
                if (File.Exists(final_path))
                {
                    // Create a file to write to.
                    using (StreamWriter sw = File.CreateText(final_path))
                    {
                        sw.WriteLine("Ex:{0}", this.excelTextBox.Text);
                        sw.WriteLine("Dx:{0}", this.dxfTextBox.Text);
                        sw.WriteLine("Panel:{0}", this.textBox10.Text);
                        sw.WriteLine("Def:{0}", this.DefaultTextBox.Text); 
                        sw.Flush();
                        sw.Close();
                    }
                }

            }
            

            this.Close();
            }

            private void textBox2_TextChanged(object sender, EventArgs e)
            {

            }

            private void DefaultTextBox_TextChanged(object sender, EventArgs e)
            {

            }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Input Panel Name: ",this.textBox10.Text);
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            FilePathContainer.panelName = textBox10.Text; 
        }

        private void textBox11_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }
        private System.Windows.Forms.ToolTip ToolTip1;
        private void PrepareTooltips()
        {
            ToolTip1 = new System.Windows.Forms.ToolTip();
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is TextBox && ctrl.Tag is string)
                {
                    ctrl.MouseHover += new EventHandler(delegate (Object o, EventArgs a)
                    {
                        var btn = (Control)o;
                        ToolTip1.SetToolTip(btn, btn.Tag.ToString());
                    });
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            useDefaults = !useDefaults;
        }
    }
    }

