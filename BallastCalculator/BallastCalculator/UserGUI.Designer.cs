using System.IO;
using System.Windows.Forms;
using System.Drawing;
namespace BallastCalculator
{
    partial class UserGUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private OpenFileDialog openFileDialog1;
        private Button dxfButton;
        private TextBox textBox1;
        private TextBox excelTextBox;
        private Button button1;
        private TextBox dxfTextBox;
        private System.ComponentModel.IContainer components = null;


        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserGUI));
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.dxfButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.excelTextBox = new System.Windows.Forms.TextBox();
            this.dxfTextBox = new System.Windows.Forms.TextBox();
            this.excelButton = new System.Windows.Forms.Button();
            this.outPutButton = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.DefaultTextBox = new System.Windows.Forms.TextBox();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.textBox10 = new System.Windows.Forms.TextBox();
            this.textBox11 = new System.Windows.Forms.TextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // dxfButton
            // 
            this.dxfButton.BackColor = System.Drawing.SystemColors.ControlDark;
            this.dxfButton.Location = new System.Drawing.Point(352, 28);
            this.dxfButton.Name = "dxfButton";
            this.dxfButton.Size = new System.Drawing.Size(91, 23);
            this.dxfButton.TabIndex = 1;
            this.dxfButton.TabStop = false;
            this.dxfButton.Text = "Dxf: ( .dxf )";
            this.dxfButton.UseVisualStyleBackColor = false;
            this.dxfButton.Click += new System.EventHandler(this.dxfButton_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.button1.Location = new System.Drawing.Point(352, 54);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(91, 23);
            this.button1.TabIndex = 2;
            this.button1.TabStop = false;
            this.button1.Text = "Excel: (  .xlsx  )";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.excelButton_Click);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.Control;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(352, 10);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(153, 14);
            this.textBox1.TabIndex = 3;
            this.textBox1.TabStop = false;
            this.textBox1.Text = "Select I/O File Paths: ";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // excelTextBox
            // 
            this.excelTextBox.Location = new System.Drawing.Point(458, 57);
            this.excelTextBox.Name = "excelTextBox";
            this.excelTextBox.Size = new System.Drawing.Size(212, 20);
            this.excelTextBox.TabIndex = 4;
            this.excelTextBox.Text = "<-- Select Excel File";
            this.excelTextBox.TextChanged += new System.EventHandler(this.excelTextBox_TextChanged);
            // 
            // dxfTextBox
            // 
            this.dxfTextBox.Location = new System.Drawing.Point(458, 31);
            this.dxfTextBox.Name = "dxfTextBox";
            this.dxfTextBox.Size = new System.Drawing.Size(212, 20);
            this.dxfTextBox.TabIndex = 5;
            this.dxfTextBox.Text = "<-- Select DXF File";
            this.dxfTextBox.TextChanged += new System.EventHandler(this.dxfTextBox_TextChanged);
            // 
            // excelButton
            // 
            this.excelButton.BackColor = System.Drawing.SystemColors.ControlDark;
            this.excelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.excelButton.Location = new System.Drawing.Point(404, 155);
            this.excelButton.Name = "excelButton";
            this.excelButton.Size = new System.Drawing.Size(198, 26);
            this.excelButton.TabIndex = 6;
            this.excelButton.Text = "Execute Ballast Calculations\r\n";
            this.excelButton.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.excelButton.UseVisualStyleBackColor = false;
            this.excelButton.Click += new System.EventHandler(this.excelButton_Click_1);
            // 
            // outPutButton
            // 
            this.outPutButton.BackColor = System.Drawing.SystemColors.ControlDark;
            this.outPutButton.Location = new System.Drawing.Point(400, 109);
            this.outPutButton.Name = "outPutButton";
            this.outPutButton.Size = new System.Drawing.Size(202, 23);
            this.outPutButton.TabIndex = 9;
            this.outPutButton.Text = "(Optional) Select Output DXF Path";
            this.outPutButton.UseVisualStyleBackColor = false;
            this.outPutButton.Click += new System.EventHandler(this.outPutButton_Click);
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.SystemColors.Control;
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.Location = new System.Drawing.Point(12, 118);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(111, 13);
            this.textBox2.TabIndex = 11;
            this.textBox2.TabStop = false;
            this.textBox2.Text = "Default Outpath (.dxf): ";
            this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            // 
            // DefaultTextBox
            // 
            this.DefaultTextBox.Location = new System.Drawing.Point(118, 115);
            this.DefaultTextBox.Name = "DefaultTextBox";
            this.DefaultTextBox.Size = new System.Drawing.Size(212, 20);
            this.DefaultTextBox.TabIndex = 12;
            this.DefaultTextBox.TextChanged += new System.EventHandler(this.DefaultTextBox_TextChanged);
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.SystemColors.Control;
            this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox3.Location = new System.Drawing.Point(12, 10);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(100, 14);
            this.textBox3.TabIndex = 13;
            this.textBox3.TabStop = false;
            this.textBox3.Text = "Saved Inputs: ";
            this.textBox3.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            // 
            // textBox4
            // 
            this.textBox4.BackColor = System.Drawing.SystemColors.Control;
            this.textBox4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox4.Location = new System.Drawing.Point(12, 41);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(100, 13);
            this.textBox4.TabIndex = 14;
            this.textBox4.TabStop = false;
            this.textBox4.Text = "Saved DXF Path: ";
            this.textBox4.TextChanged += new System.EventHandler(this.textBox4_TextChanged);
            // 
            // textBox5
            // 
            this.textBox5.BackColor = System.Drawing.SystemColors.Control;
            this.textBox5.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox5.Location = new System.Drawing.Point(12, 67);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(100, 13);
            this.textBox5.TabIndex = 15;
            this.textBox5.TabStop = false;
            this.textBox5.Text = "Saved Excel Path: ";
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(118, 38);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(212, 20);
            this.textBox6.TabIndex = 16;
            // 
            // textBox7
            // 
            this.textBox7.Location = new System.Drawing.Point(118, 64);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(212, 20);
            this.textBox7.TabIndex = 17;
            this.textBox7.TextChanged += new System.EventHandler(this.textBox7_TextChanged);
            // 
            // textBox8
            // 
            this.textBox8.Location = new System.Drawing.Point(118, 90);
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new System.Drawing.Size(212, 20);
            this.textBox8.TabIndex = 18;
            this.textBox8.TextChanged += new System.EventHandler(this.textBox8_TextChanged);
            // 
            // textBox9
            // 
            this.textBox9.BackColor = System.Drawing.SystemColors.Control;
            this.textBox9.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox9.Location = new System.Drawing.Point(12, 93);
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new System.Drawing.Size(100, 13);
            this.textBox9.TabIndex = 19;
            this.textBox9.TabStop = false;
            this.textBox9.Text = "Saved Panel Name: ";
            this.textBox9.TextChanged += new System.EventHandler(this.textBox9_TextChanged);
            // 
            // textBox10
            // 
            this.textBox10.Location = new System.Drawing.Point(458, 83);
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new System.Drawing.Size(212, 20);
            this.textBox10.TabIndex = 21;
            this.textBox10.Text = "-- Enter Panel Block Name Here --";
            this.textBox10.TextChanged += new System.EventHandler(this.textBox10_TextChanged);
            // 
            // textBox11
            // 
            this.textBox11.BackColor = System.Drawing.SystemColors.Control;
            this.textBox11.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox11.Location = new System.Drawing.Point(352, 86);
            this.textBox11.Name = "textBox11";
            this.textBox11.Size = new System.Drawing.Size(91, 13);
            this.textBox11.TabIndex = 22;
            this.textBox11.TabStop = false;
            this.textBox11.Text = "Enter Panel Name: ";
            this.textBox11.TextChanged += new System.EventHandler(this.textBox11_TextChanged_1);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(66, 155);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(167, 17);
            this.checkBox1.TabIndex = 24;
            this.checkBox1.Tag = "";
            this.checkBox1.Text = "Use Saved Values / Defaults ";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // UserGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(705, 206);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.textBox11);
            this.Controls.Add(this.textBox10);
            this.Controls.Add(this.textBox9);
            this.Controls.Add(this.textBox8);
            this.Controls.Add(this.textBox7);
            this.Controls.Add(this.textBox6);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.DefaultTextBox);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.outPutButton);
            this.Controls.Add(this.excelButton);
            this.Controls.Add(this.dxfTextBox);
            this.Controls.Add(this.excelTextBox);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dxfButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "UserGUI";
            this.Text = "Ballast Calculator";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }



        #endregion

        private Button excelButton;
        private Button outPutButton;
        private TextBox textBox2;
        private NotifyIcon notifyIcon1;
        private SaveFileDialog saveFileDialog1;
        private TextBox DefaultTextBox;
        private TextBox textBox3;
        private TextBox textBox4;
        private TextBox textBox5;
        private TextBox textBox6;
        private TextBox textBox7;
        private TextBox textBox8;
        private TextBox textBox9;
        private TextBox textBox10;
        private TextBox textBox11;
        private CheckBox checkBox1;
    }
}
