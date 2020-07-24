namespace Balance_Calculator
{
    partial class Form1
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.endSC = new System.Windows.Forms.ComboBox();
            this.startSC = new System.Windows.Forms.ComboBox();
            this.startSR = new System.Windows.Forms.ComboBox();
            this.endSR = new System.Windows.Forms.ComboBox();
            this.labelPath = new System.Windows.Forms.Label();
            this.ButtonPath = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.ButtonCalculate = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.labelFile = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.check = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(225, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Start of student response column: ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(220, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "End of student response column: ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(29, 179);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(152, 17);
            this.label4.TabIndex = 4;
            this.label4.Text = "End of score columns: ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(29, 140);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(157, 17);
            this.label5.TabIndex = 3;
            this.label5.Text = "Start of score columns: ";
            // 
            // endSC
            // 
            this.endSC.FormattingEnabled = true;
            this.endSC.Location = new System.Drawing.Point(185, 176);
            this.endSC.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.endSC.Name = "endSC";
            this.endSC.Size = new System.Drawing.Size(259, 24);
            this.endSC.TabIndex = 5;
            // 
            // startSC
            // 
            this.startSC.FormattingEnabled = true;
            this.startSC.Location = new System.Drawing.Point(185, 138);
            this.startSC.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.startSC.Name = "startSC";
            this.startSC.Size = new System.Drawing.Size(259, 24);
            this.startSC.TabIndex = 4;
            // 
            // startSR
            // 
            this.startSR.FormattingEnabled = true;
            this.startSR.Location = new System.Drawing.Point(232, 62);
            this.startSR.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.startSR.Name = "startSR";
            this.startSR.Size = new System.Drawing.Size(227, 24);
            this.startSR.TabIndex = 2;
            // 
            // endSR
            // 
            this.endSR.FormattingEnabled = true;
            this.endSR.Location = new System.Drawing.Point(232, 100);
            this.endSR.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.endSR.Name = "endSR";
            this.endSR.Size = new System.Drawing.Size(227, 24);
            this.endSR.TabIndex = 3;
            // 
            // labelPath
            // 
            this.labelPath.AutoSize = true;
            this.labelPath.Location = new System.Drawing.Point(29, 33);
            this.labelPath.Name = "labelPath";
            this.labelPath.Size = new System.Drawing.Size(0, 17);
            this.labelPath.TabIndex = 9;
            // 
            // ButtonPath
            // 
            this.ButtonPath.Location = new System.Drawing.Point(32, 4);
            this.ButtonPath.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ButtonPath.Name = "ButtonPath";
            this.ButtonPath.Size = new System.Drawing.Size(343, 28);
            this.ButtonPath.TabIndex = 1;
            this.ButtonPath.Text = "Select a dataset with responses and bins";
            this.ButtonPath.UseVisualStyleBackColor = true;
            this.ButtonPath.Click += new System.EventHandler(this.ButtonPath_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "CSV and Excel files|*.csv;*.xls;*.xlsx";
            this.openFileDialog1.Title = "Select a dataset with responses and bins";
            // 
            // ButtonCalculate
            // 
            this.ButtonCalculate.Location = new System.Drawing.Point(326, 212);
            this.ButtonCalculate.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ButtonCalculate.Name = "ButtonCalculate";
            this.ButtonCalculate.Size = new System.Drawing.Size(118, 49);
            this.ButtonCalculate.TabIndex = 6;
            this.ButtonCalculate.Text = "Calculate";
            this.ButtonCalculate.UseVisualStyleBackColor = true;
            this.ButtonCalculate.Click += new System.EventHandler(this.ButtonCalculate_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.Location = new System.Drawing.Point(465, 52);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidth = 62;
            this.dataGridView1.RowTemplate.Height = 28;
            this.dataGridView1.Size = new System.Drawing.Size(671, 724);
            this.dataGridView1.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(463, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 17);
            this.label1.TabIndex = 14;
            this.label1.Text = "Data preview";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(16, 265);
            this.textBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(431, 184);
            this.textBox1.TabIndex = 7;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(16, 453);
            this.textBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox2.Size = new System.Drawing.Size(431, 160);
            this.textBox2.TabIndex = 8;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(463, 33);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 17);
            this.label6.TabIndex = 20;
            this.label6.Text = "File:";
            // 
            // labelFile
            // 
            this.labelFile.AutoSize = true;
            this.labelFile.Location = new System.Drawing.Point(503, 33);
            this.labelFile.Name = "labelFile";
            this.labelFile.Size = new System.Drawing.Size(0, 17);
            this.labelFile.TabIndex = 21;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(16, 617);
            this.textBox3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox3.Size = new System.Drawing.Size(431, 160);
            this.textBox3.TabIndex = 22;
            // 
            // check
            // 
            this.check.Location = new System.Drawing.Point(16, 212);
            this.check.Name = "check";
            this.check.Size = new System.Drawing.Size(213, 48);
            this.check.TabIndex = 25;
            this.check.Text = "Do you wish to calculate vocd-D for each response?";
            this.check.UseVisualStyleBackColor = true;
            this.check.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1141, 780);
            this.Controls.Add(this.check);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.labelFile);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.ButtonCalculate);
            this.Controls.Add(this.ButtonPath);
            this.Controls.Add(this.labelPath);
            this.Controls.Add(this.startSR);
            this.Controls.Add(this.endSR);
            this.Controls.Add(this.startSC);
            this.Controls.Add(this.endSC);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "AACR Lexical Tools";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox endSC;
        private System.Windows.Forms.ComboBox startSC;
        private System.Windows.Forms.ComboBox startSR;
        private System.Windows.Forms.ComboBox endSR;
        private System.Windows.Forms.Label labelPath;
        private System.Windows.Forms.Button ButtonPath;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button ButtonCalculate;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label labelFile;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.CheckBox check;
    }
}

