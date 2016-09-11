using System.IO;
using System.Windows.Forms;
using System.Drawing; 
namespace GUIBalCal
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
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
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // dxfButton
            // 
            this.dxfButton.Location = new System.Drawing.Point(12, 30);
            this.dxfButton.Name = "dxfButton";
            this.dxfButton.Size = new System.Drawing.Size(91, 23);
            this.dxfButton.TabIndex = 1;
            this.dxfButton.TabStop = false;
            this.dxfButton.Text = "Dxf: ( .dxf )";
            this.dxfButton.UseVisualStyleBackColor = true;
            this.dxfButton.Click += new System.EventHandler(this.dxfButton_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 60);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(91, 23);
            this.button1.TabIndex = 2;
            this.button1.TabStop = false;
            this.button1.Text = "Excel: (  .xlsx  )";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.excelButton_Click);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.Control;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Location = new System.Drawing.Point(12, 11);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 13);
            this.textBox1.TabIndex = 3;
            this.textBox1.TabStop = false;
            this.textBox1.Text = "Select I/O File Paths: ";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // excelTextBox
            // 
            this.excelTextBox.Location = new System.Drawing.Point(118, 62);
            this.excelTextBox.Name = "excelTextBox";
            this.excelTextBox.Size = new System.Drawing.Size(100, 20);
            this.excelTextBox.TabIndex = 4;
            this.excelTextBox.TextChanged += new System.EventHandler(this.excelTextBox_TextChanged);
            // 
            // dxfTextBox
            // 
            this.dxfTextBox.Location = new System.Drawing.Point(118, 32);
            this.dxfTextBox.Name = "dxfTextBox";
            this.dxfTextBox.Size = new System.Drawing.Size(100, 20);
            this.dxfTextBox.TabIndex = 5;
            this.dxfTextBox.TextChanged += new System.EventHandler(this.dxfTextBox_TextChanged);
            // 
            // excelButton
            // 
            this.excelButton.Location = new System.Drawing.Point(12, 194);
            this.excelButton.Name = "excelButton";
            this.excelButton.Size = new System.Drawing.Size(155, 22);
            this.excelButton.TabIndex = 6;
            this.excelButton.Text = "Execute Ballast Calculations\r\n";
            this.excelButton.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.excelButton.UseVisualStyleBackColor = true;
            this.excelButton.Click += new System.EventHandler(this.excelButton_Click_1);
            // 
            // outPutButton
            // 
            this.outPutButton.Location = new System.Drawing.Point(12, 98);
            this.outPutButton.Name = "outPutButton";
            this.outPutButton.Size = new System.Drawing.Size(139, 23);
            this.outPutButton.TabIndex = 9;
            this.outPutButton.Text = "(Optional) Select Outpath";
            this.outPutButton.UseVisualStyleBackColor = true;
            this.outPutButton.Click += new System.EventHandler(this.outPutButton_Click);
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.SystemColors.Control;
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.Location = new System.Drawing.Point(12, 136);
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
            this.DefaultTextBox.Location = new System.Drawing.Point(12, 155);
            this.DefaultTextBox.Name = "DefaultTextBox";
            this.DefaultTextBox.Size = new System.Drawing.Size(284, 20);
            this.DefaultTextBox.TabIndex = 12;
            this.DefaultTextBox.TextChanged += new System.EventHandler(this.DefaultTextBox_TextChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(308, 247);
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
            this.Name = "Form1";
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
    }
}

