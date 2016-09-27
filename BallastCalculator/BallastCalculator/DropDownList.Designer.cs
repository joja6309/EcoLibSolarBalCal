namespace BallastCalculator
{
    partial class DropDownList
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DropDownList));
            this.S = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox11 = new System.Windows.Forms.TextBox();
            this.textBox10 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // S
            // 
            this.S.FormattingEnabled = true;
            this.S.Location = new System.Drawing.Point(12, 12);
            this.S.Name = "S";
            this.S.Size = new System.Drawing.Size(187, 21);
            this.S.TabIndex = 0;
            this.S.Text = " Available Panel Block Names ";
            this.S.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 95);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Select ";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox11
            // 
            this.textBox11.BackColor = System.Drawing.SystemColors.Control;
            this.textBox11.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox11.Location = new System.Drawing.Point(12, 57);
            this.textBox11.Name = "textBox11";
            this.textBox11.Size = new System.Drawing.Size(91, 13);
            this.textBox11.TabIndex = 24;
            this.textBox11.TabStop = false;
            this.textBox11.Text = "Enter Panel Name: ";
            // 
            // textBox10
            // 
            this.textBox10.Location = new System.Drawing.Point(109, 54);
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new System.Drawing.Size(212, 20);
            this.textBox10.TabIndex = 23;
            this.textBox10.Text = "-- Enter Panel Block Name Here --";
            this.textBox10.TextChanged += new System.EventHandler(this.textBox10_TextChanged);
            // 
            // DropDownList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(339, 151);
            this.Controls.Add(this.textBox11);
            this.Controls.Add(this.textBox10);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.S);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DropDownList";
            this.Text = "Available Panels";
            this.Load += new System.EventHandler(this.DropDownList_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox S;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox11;
        private System.Windows.Forms.TextBox textBox10;
    }
}