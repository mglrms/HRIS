namespace HRISCDBS
{
    partial class FormAttendanceMonitoring
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
            panel1 = new Panel();
            panel2 = new Panel();
            cbEmployeeType = new ComboBox();
            label6 = new Label();
            comboBox1 = new ComboBox();
            label5 = new Label();
            label4 = new Label();
            dtp_to = new DateTimePicker();
            label3 = new Label();
            dtp_from = new DateTimePicker();
            label2 = new Label();
            btnGenerateOutput = new Button();
            panel3 = new Panel();
            label1 = new Label();
            tb_search = new TextBox();
            dgv_attendance = new DataGridView();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgv_attendance).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.ControlLight;
            panel1.Controls.Add(panel2);
            panel1.Location = new Point(363, 5);
            panel1.Name = "panel1";
            panel1.Size = new Size(1526, 881);
            panel1.TabIndex = 0;
            // 
            // panel2
            // 
            panel2.BackColor = SystemColors.ActiveCaption;
            panel2.Controls.Add(cbEmployeeType);
            panel2.Controls.Add(label6);
            panel2.Controls.Add(comboBox1);
            panel2.Controls.Add(label5);
            panel2.Controls.Add(label4);
            panel2.Controls.Add(dtp_to);
            panel2.Controls.Add(label3);
            panel2.Controls.Add(dtp_from);
            panel2.Controls.Add(label2);
            panel2.Controls.Add(btnGenerateOutput);
            panel2.Controls.Add(panel3);
            panel2.Controls.Add(tb_search);
            panel2.Controls.Add(dgv_attendance);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(1526, 881);
            panel2.TabIndex = 1;
            // 
            // cbEmployeeType
            // 
            cbEmployeeType.DropDownStyle = ComboBoxStyle.DropDownList;
            cbEmployeeType.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cbEmployeeType.FormattingEnabled = true;
            cbEmployeeType.Items.AddRange(new object[] { "All", "Teaching", "Non-Teaching", "Maintenance", "Coach" });
            cbEmployeeType.Location = new Point(872, 106);
            cbEmployeeType.Name = "cbEmployeeType";
            cbEmployeeType.Size = new Size(187, 36);
            cbEmployeeType.TabIndex = 13;
            cbEmployeeType.SelectedIndexChanged += cbEmployeeType_SelectedIndexChanged;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.Location = new Point(711, 114);
            label6.Name = "label6";
            label6.Size = new Size(155, 28);
            label6.TabIndex = 12;
            label6.Text = "Employee Type:";
            // 
            // comboBox1
            // 
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.Font = new Font("Segoe UI", 12F);
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "None", "13th Payroll", "28th Payroll" });
            comboBox1.Location = new Point(538, 107);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(151, 36);
            comboBox1.TabIndex = 11;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            label5.Location = new Point(447, 114);
            label5.Name = "label5";
            label5.Size = new Size(85, 28);
            label5.TabIndex = 10;
            label5.Text = "Cut-Off:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            label4.Location = new Point(1189, 115);
            label4.Name = "label4";
            label4.Size = new Size(77, 28);
            label4.TabIndex = 9;
            label4.Text = "Search:";
            // 
            // dtp_to
            // 
            dtp_to.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            dtp_to.Format = DateTimePickerFormat.Short;
            dtp_to.Location = new Point(286, 109);
            dtp_to.Margin = new Padding(3, 4, 3, 4);
            dtp_to.Name = "dtp_to";
            dtp_to.Size = new Size(155, 34);
            dtp_to.TabIndex = 8;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            label3.Location = new Point(242, 115);
            label3.Name = "label3";
            label3.Size = new Size(38, 28);
            label3.TabIndex = 7;
            label3.Text = "To:";
            // 
            // dtp_from
            // 
            dtp_from.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            dtp_from.Format = DateTimePickerFormat.Short;
            dtp_from.Location = new Point(88, 109);
            dtp_from.Margin = new Padding(3, 4, 3, 4);
            dtp_from.Name = "dtp_from";
            dtp_from.Size = new Size(148, 34);
            dtp_from.TabIndex = 6;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            label2.Location = new Point(18, 115);
            label2.Name = "label2";
            label2.Size = new Size(64, 28);
            label2.TabIndex = 1;
            label2.Text = "From:";
            // 
            // btnGenerateOutput
            // 
            btnGenerateOutput.BackColor = Color.FromArgb(255, 246, 224);
            btnGenerateOutput.FlatStyle = FlatStyle.Flat;
            btnGenerateOutput.Location = new Point(1313, 825);
            btnGenerateOutput.Name = "btnGenerateOutput";
            btnGenerateOutput.Size = new Size(197, 43);
            btnGenerateOutput.TabIndex = 5;
            btnGenerateOutput.Text = "Generate Output";
            btnGenerateOutput.UseVisualStyleBackColor = false;
            btnGenerateOutput.Click += btnGenerateOutput_Click;
            // 
            // panel3
            // 
            panel3.BackColor = Color.FromArgb(255, 246, 224);
            panel3.Controls.Add(label1);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(0, 0);
            panel3.Margin = new Padding(3, 4, 3, 4);
            panel3.Name = "panel3";
            panel3.Size = new Size(1526, 76);
            panel3.TabIndex = 4;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 24F);
            label1.Location = new Point(3, 7);
            label1.Name = "label1";
            label1.Size = new Size(438, 54);
            label1.TabIndex = 0;
            label1.Text = "Attendance Monitoring";
            // 
            // tb_search
            // 
            tb_search.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            tb_search.Location = new Point(1273, 108);
            tb_search.Name = "tb_search";
            tb_search.Size = new Size(238, 34);
            tb_search.TabIndex = 3;
            tb_search.TextChanged += tb_search_TextChanged;
            // 
            // dgv_attendance
            // 
            dgv_attendance.AllowUserToAddRows = false;
            dgv_attendance.AllowUserToDeleteRows = false;
            dgv_attendance.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv_attendance.Location = new Point(18, 155);
            dgv_attendance.MultiSelect = false;
            dgv_attendance.Name = "dgv_attendance";
            dgv_attendance.ReadOnly = true;
            dgv_attendance.RowHeadersVisible = false;
            dgv_attendance.RowHeadersWidth = 51;
            dgv_attendance.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv_attendance.Size = new Size(1493, 657);
            dgv_attendance.TabIndex = 0;
            // 
            // FormAttendanceMonitoring
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(219, 226, 239);
            ClientSize = new Size(1902, 892);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FormAttendanceMonitoring";
            Text = "FormAttendanceMonitoring";
            Load += FormAttendanceMonitoring_Load;
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgv_attendance).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel panel2;
        private Button btnGenerateOutput;
        private Panel panel3;
        private TextBox tb_search;
        private DataGridView dgv_attendance;
        private Label label1;
        private DateTimePicker dtp_to;
        private Label label3;
        private DateTimePicker dtp_from;
        private Label label2;
        private Label label4;
        private ComboBox comboBox1;
        private Label label5;
        private ComboBox cbEmployeeType;
        private Label label6;
    }
}