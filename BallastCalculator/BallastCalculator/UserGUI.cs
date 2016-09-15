using System;
using System.Windows.Forms;
using System.IO;

namespace BallastCalculator
{
    public partial class UserGUI : Form
    {
       
            public UserGUI()
            {
                InitializeComponent();
                openFileDialog1.InitialDirectory = @"C:\";
            
        }
        private string panelStorage;
        private string excelPathStorage;
        private string dxfPathStorage;
        private string defaultOutStorage; 
        private bool useDefaults = false;
        
            private void dxfButton_Click(object sender, System.EventArgs e)
            {
            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

           
            if (openFileDialog1.ShowDialog() == DialogResult.OK) // Test result.
            {

                dxfTextBox.Text = openFileDialog1.FileName;
                dxfPathStorage = openFileDialog1.FileName;
                string out_name = openFileDialog1.FileName + "_out.dxf";
                DefaultTextBox.Text = out_name;
                defaultOutStorage = out_name;

            }
        }
            private void excelButton_Click(object sender, System.EventArgs e)
            {

                if (openFileDialog1.ShowDialog() == DialogResult.OK) // Test result.
                {
                excelTextBox.Text = openFileDialog1.FileName;
                string wanted_path = openFileDialog1.FileName;
                excelPathStorage = wanted_path = "/" + openFileDialog1.FileName; 

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
                        excelPathStorage = line.Substring(line.LastIndexOf(':') + 1);
                        textBox6.Text = line.Substring(line.LastIndexOf(':') + 1);
                    }
                    else if (line.Contains("Dx:"))
                    {
                        dxfPathStorage = line.Substring(line.LastIndexOf(':') + 1);
                        textBox7.Text = line.Substring(line.LastIndexOf(':') + 1);
                    }
                    else if (line.Contains("Panel:"))
                    {
                         panelStorage = line.Substring(line.LastIndexOf(':') + 1);
                         textBox8.Text = line.Substring(line.LastIndexOf(':') + 1);

                    }
                    else if (line.Contains("Def:"))
                    {
                        defaultOutStorage = line.Substring(line.LastIndexOf(':') + 1);
                        DefaultTextBox.Text = line.Substring(line.LastIndexOf(':') + 1);
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

                    DefaultTextBox.Text = openFileDialog1.FileName;
                    defaultOutStorage = openFileDialog1.FileName;

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
                    sw.WriteLine("Ex:{0}", excelPathStorage);
                    sw.WriteLine("Dx:{0}",dxfPathStorage );
                    sw.WriteLine("Panel:{0}",panelStorage );
                    sw.WriteLine("Def:{0}", defaultOutStorage);
                    sw.Flush();
                    sw.Close(); 
                }
            }
            if(useDefaults)
            {
                //MessageBox.Show(panel_name + " " + excel_path + " " + Defa)
                FilePathContainer.panelName = panelStorage;
                FilePathContainer.excelPath = excelPathStorage;
                FilePathContainer.outPath = defaultOutStorage;
                FilePathContainer.dxfPath = dxfPathStorage;
                if (File.Exists(final_path))
                {
                    // Create a file to write to.
                    using (StreamWriter sw = File.CreateText(final_path))
                    {
                        sw.WriteLine("Ex:{0}", excelPathStorage);
                        sw.WriteLine("Dx:{0}", dxfPathStorage);
                        sw.WriteLine("Panel:{0}", panelStorage);
                        sw.WriteLine("Def:{0}", defaultOutStorage);
                        sw.Flush();
                        sw.Close();
                    }
                }
            }
            else
            {
                FilePathContainer.panelName = textBox10.Text;
                FilePathContainer.outPath = DefaultTextBox.Text;
                FilePathContainer.excelPath = excelTextBox.Text;
                FilePathContainer.dxfPath = dxfTextBox.Text; 

                if (File.Exists(final_path))
                {
                    // Create a file to write to.
                    using (StreamWriter sw = File.CreateText(final_path))
                    {
                        sw.WriteLine("Ex:{0}", excelPathStorage);
                        sw.WriteLine("Dx:{0}", dxfPathStorage);
                        sw.WriteLine("Panel:{0}",panelStorage);
                        sw.WriteLine("Def:{0}", defaultOutStorage); 
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
            panelStorage = this.textBox10.Text;
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            FilePathContainer.panelName = textBox10.Text;
            panelStorage = this.textBox10.Text;

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

