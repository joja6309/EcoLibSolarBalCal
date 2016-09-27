using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BallastCalculator
{
    public partial class DropDownList : Form
    {
        public DropDownList()
        {
            InitializeComponent();
        }

        private void DropDownList_Load(object sender, EventArgs e)
        {
            S.DataSource = FilePathContainer.panelNames;

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get the selected item in the combobox
            string selectedPair = S.SelectedItem.ToString();

            // Show selected information on screen
          
            FilePathContainer.panelName = selectedPair;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            FilePathContainer.panelName = textBox10.Text.ToString();

        }
    }
}
