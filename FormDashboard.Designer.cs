namespace HRISCDBS
{
    partial class FormDashboard
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
            components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title2 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDashboard));
            panel1 = new Panel();
            panel2 = new Panel();
            groupBox5 = new GroupBox();
            lblToday = new Label();
            groupBox3 = new GroupBox();
            lblSY = new Label();
            label1 = new Label();
            dtpFrom = new DateTimePicker();
            btnToday = new Button();
            btnDays = new Button();
            btnMonth = new Button();
            dtpTo = new DateTimePicker();
            chartRecords = new System.Windows.Forms.DataVisualization.Charting.Chart();
            chartAttendance = new System.Windows.Forms.DataVisualization.Charting.Chart();
            gbLeaveRequest = new GroupBox();
            pbLeave = new PictureBox();
            lblLeaveNotif = new Label();
            groupBox2 = new GroupBox();
            pictureBox2 = new PictureBox();
            lblPresentEmp = new Label();
            groupBox1 = new GroupBox();
            pictureBox1 = new PictureBox();
            lblTotalEmp = new Label();
            refreshTimer = new System.Windows.Forms.Timer(components);
            groupBox4 = new GroupBox();
            dgvAttendanceStatus = new DataGridView();
            label2 = new Label();
            cbSanctions = new ComboBox();
            btnGenerate = new Button();
            todayTimer = new System.Windows.Forms.Timer(components);
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            groupBox5.SuspendLayout();
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)chartRecords).BeginInit();
            ((System.ComponentModel.ISupportInitialize)chartAttendance).BeginInit();
            gbLeaveRequest.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbLeave).BeginInit();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAttendanceStatus).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(27, 60, 83);
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(chartRecords);
            panel1.Controls.Add(chartAttendance);
            panel1.Controls.Add(gbLeaveRequest);
            panel1.Controls.Add(groupBox2);
            panel1.Controls.Add(groupBox1);
            panel1.Location = new Point(363, 5);
            panel1.Name = "panel1";
            panel1.Size = new Size(1526, 881);
            panel1.TabIndex = 1;
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(27, 60, 83);
            panel2.Controls.Add(groupBox5);
            panel2.Controls.Add(groupBox3);
            panel2.Controls.Add(label1);
            panel2.Controls.Add(dtpFrom);
            panel2.Controls.Add(btnToday);
            panel2.Controls.Add(btnDays);
            panel2.Controls.Add(btnMonth);
            panel2.Controls.Add(dtpTo);
            panel2.Location = new Point(0, -15);
            panel2.Margin = new Padding(3, 4, 3, 4);
            panel2.Name = "panel2";
            panel2.Size = new Size(1526, 175);
            panel2.TabIndex = 17;
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(lblToday);
            groupBox5.Font = new Font("Segoe UI", 16.2F, FontStyle.Bold);
            groupBox5.ForeColor = Color.White;
            groupBox5.Location = new Point(802, 15);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new Size(383, 115);
            groupBox5.TabIndex = 19;
            groupBox5.TabStop = false;
            groupBox5.Text = "Today is:";
            // 
            // lblToday
            // 
            lblToday.AutoSize = true;
            lblToday.Font = new Font("Constantia", 15F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblToday.ForeColor = Color.White;
            lblToday.Location = new Point(10, 53);
            lblToday.Name = "lblToday";
            lblToday.Size = new Size(78, 31);
            lblToday.TabIndex = 18;
            lblToday.Text = "label2";
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(lblSY);
            groupBox3.Font = new Font("Segoe UI", 16.2F, FontStyle.Bold);
            groupBox3.ForeColor = Color.White;
            groupBox3.Location = new Point(1191, 15);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(321, 115);
            groupBox3.TabIndex = 18;
            groupBox3.TabStop = false;
            groupBox3.Text = "School Year";
            // 
            // lblSY
            // 
            lblSY.AutoSize = true;
            lblSY.Font = new Font("Constantia", 33F, FontStyle.Bold);
            lblSY.ForeColor = Color.White;
            lblSY.Location = new Point(5, 37);
            lblSY.Name = "lblSY";
            lblSY.Size = new Size(184, 67);
            lblSY.TabIndex = 17;
            lblSY.Text = "label2";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Constantia", 72F, FontStyle.Bold);
            label1.ForeColor = Color.White;
            label1.Location = new Point(10, 19);
            label1.Name = "label1";
            label1.Size = new Size(715, 146);
            label1.TabIndex = 16;
            label1.Text = "WELCOME!";
            // 
            // dtpFrom
            // 
            dtpFrom.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            dtpFrom.Format = DateTimePickerFormat.Short;
            dtpFrom.Location = new Point(802, 134);
            dtpFrom.Margin = new Padding(3, 4, 3, 4);
            dtpFrom.MaxDate = new DateTime(2027, 12, 31, 0, 0, 0, 0);
            dtpFrom.MinDate = new DateTime(2025, 1, 1, 0, 0, 0, 0);
            dtpFrom.Name = "dtpFrom";
            dtpFrom.Size = new Size(152, 34);
            dtpFrom.TabIndex = 11;
            dtpFrom.Value = new DateTime(2025, 8, 27, 0, 0, 0, 0);
            // 
            // btnToday
            // 
            btnToday.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            btnToday.Location = new Point(1121, 134);
            btnToday.Margin = new Padding(3, 4, 3, 4);
            btnToday.Name = "btnToday";
            btnToday.Size = new Size(126, 34);
            btnToday.TabIndex = 13;
            btnToday.Text = "Today";
            btnToday.UseVisualStyleBackColor = true;
            btnToday.Click += btnToday_Click;
            // 
            // btnDays
            // 
            btnDays.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            btnDays.Location = new Point(1254, 134);
            btnDays.Margin = new Padding(3, 4, 3, 4);
            btnDays.Name = "btnDays";
            btnDays.Size = new Size(126, 34);
            btnDays.TabIndex = 14;
            btnDays.Text = "Last 7 days";
            btnDays.UseVisualStyleBackColor = true;
            btnDays.Click += btnDays_Click;
            // 
            // btnMonth
            // 
            btnMonth.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            btnMonth.Location = new Point(1386, 134);
            btnMonth.Margin = new Padding(3, 4, 3, 4);
            btnMonth.Name = "btnMonth";
            btnMonth.Size = new Size(126, 34);
            btnMonth.TabIndex = 15;
            btnMonth.Text = "This month";
            btnMonth.UseVisualStyleBackColor = true;
            btnMonth.Click += btnMonth_Click;
            // 
            // dtpTo
            // 
            dtpTo.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            dtpTo.Format = DateTimePickerFormat.Short;
            dtpTo.Location = new Point(961, 134);
            dtpTo.Margin = new Padding(3, 4, 3, 4);
            dtpTo.MaxDate = new DateTime(2027, 12, 31, 0, 0, 0, 0);
            dtpTo.MinDate = new DateTime(2025, 1, 1, 0, 0, 0, 0);
            dtpTo.Name = "dtpTo";
            dtpTo.Size = new Size(153, 34);
            dtpTo.TabIndex = 12;
            dtpTo.Value = new DateTime(2025, 8, 27, 0, 0, 0, 0);
            // 
            // chartRecords
            // 
            chartRecords.BackColor = Color.FromArgb(210, 193, 182);
            chartRecords.BorderlineColor = Color.Black;
            chartArea1.AlignmentOrientation = System.Windows.Forms.DataVisualization.Charting.AreaAlignmentOrientations.Horizontal;
            chartArea1.BackColor = Color.White;
            chartArea1.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            chartArea1.BorderWidth = 2;
            chartArea1.Name = "ChartArea1";
            chartRecords.ChartAreas.Add(chartArea1);
            legend1.BackColor = Color.FromArgb(27, 60, 83);
            legend1.ForeColor = Color.White;
            legend1.Name = "Legend1";
            legend1.TitleForeColor = Color.White;
            chartRecords.Legends.Add(legend1);
            chartRecords.Location = new Point(469, 156);
            chartRecords.Name = "chartRecords";
            chartRecords.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Pastel;
            chartRecords.Size = new Size(1057, 493);
            chartRecords.TabIndex = 16;
            chartRecords.Text = "chart1";
            title1.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            title1.ForeColor = Color.White;
            title1.Name = "Title1";
            title1.Text = "Present Employees";
            chartRecords.Titles.Add(title1);
            // 
            // chartAttendance
            // 
            chartAttendance.BackColor = Color.FromArgb(27, 60, 83);
            chartArea2.BackColor = Color.White;
            chartArea2.BorderColor = Color.White;
            chartArea2.Name = "ChartArea1";
            chartAttendance.ChartAreas.Add(chartArea2);
            legend2.BackColor = Color.FromArgb(17, 45, 78);
            legend2.ForeColor = Color.White;
            legend2.Name = "Legend1";
            legend2.TitleForeColor = Color.White;
            chartAttendance.Legends.Add(legend2);
            chartAttendance.Location = new Point(961, 645);
            chartAttendance.Name = "chartAttendance";
            chartAttendance.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Pastel;
            series1.BorderWidth = 2;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Doughnut;
            series1.LabelForeColor = Color.White;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            chartAttendance.Series.Add(series1);
            chartAttendance.Size = new Size(565, 236);
            chartAttendance.TabIndex = 7;
            chartAttendance.Text = "chart1";
            title2.ForeColor = Color.White;
            title2.Name = "Title1";
            title2.Text = "Today's Attendance";
            chartAttendance.Titles.Add(title2);
            // 
            // gbLeaveRequest
            // 
            gbLeaveRequest.BackColor = Color.FromArgb(27, 60, 83);
            gbLeaveRequest.Controls.Add(pbLeave);
            gbLeaveRequest.Controls.Add(lblLeaveNotif);
            gbLeaveRequest.Font = new Font("Segoe UI", 16.2F, FontStyle.Bold);
            gbLeaveRequest.ForeColor = Color.White;
            gbLeaveRequest.Location = new Point(642, 645);
            gbLeaveRequest.Margin = new Padding(3, 4, 3, 4);
            gbLeaveRequest.Name = "gbLeaveRequest";
            gbLeaveRequest.Padding = new Padding(3, 4, 3, 4);
            gbLeaveRequest.Size = new Size(312, 236);
            gbLeaveRequest.TabIndex = 10;
            gbLeaveRequest.TabStop = false;
            gbLeaveRequest.Text = "Leave Request";
            // 
            // pbLeave
            // 
            pbLeave.Image = (Image)resources.GetObject("pbLeave.Image");
            pbLeave.Location = new Point(7, 56);
            pbLeave.Margin = new Padding(3, 4, 3, 4);
            pbLeave.Name = "pbLeave";
            pbLeave.Size = new Size(122, 151);
            pbLeave.SizeMode = PictureBoxSizeMode.StretchImage;
            pbLeave.TabIndex = 3;
            pbLeave.TabStop = false;
            pbLeave.Click += pbLeave_Click;
            // 
            // lblLeaveNotif
            // 
            lblLeaveNotif.AutoSize = true;
            lblLeaveNotif.Font = new Font("Segoe UI", 36F);
            lblLeaveNotif.ForeColor = Color.White;
            lblLeaveNotif.Location = new Point(174, 83);
            lblLeaveNotif.Name = "lblLeaveNotif";
            lblLeaveNotif.Size = new Size(67, 81);
            lblLeaveNotif.TabIndex = 1;
            lblLeaveNotif.Text = "0";
            lblLeaveNotif.Click += lblLeaveNotif_Click;
            // 
            // groupBox2
            // 
            groupBox2.BackColor = Color.FromArgb(27, 60, 83);
            groupBox2.Controls.Add(pictureBox2);
            groupBox2.Controls.Add(lblPresentEmp);
            groupBox2.Font = new Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox2.ForeColor = Color.White;
            groupBox2.Location = new Point(319, 645);
            groupBox2.Margin = new Padding(3, 4, 3, 4);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new Padding(3, 4, 3, 4);
            groupBox2.Size = new Size(312, 236);
            groupBox2.TabIndex = 9;
            groupBox2.TabStop = false;
            groupBox2.Text = "Present Employee";
            // 
            // pictureBox2
            // 
            pictureBox2.Image = (Image)resources.GetObject("pictureBox2.Image");
            pictureBox2.Location = new Point(7, 56);
            pictureBox2.Margin = new Padding(3, 4, 3, 4);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(137, 151);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 3;
            pictureBox2.TabStop = false;
            // 
            // lblPresentEmp
            // 
            lblPresentEmp.AutoSize = true;
            lblPresentEmp.Font = new Font("Segoe UI", 36F);
            lblPresentEmp.ForeColor = Color.White;
            lblPresentEmp.Location = new Point(174, 83);
            lblPresentEmp.Name = "lblPresentEmp";
            lblPresentEmp.Size = new Size(67, 81);
            lblPresentEmp.TabIndex = 1;
            lblPresentEmp.Text = "0";
            // 
            // groupBox1
            // 
            groupBox1.BackColor = Color.FromArgb(27, 60, 83);
            groupBox1.Controls.Add(pictureBox1);
            groupBox1.Controls.Add(lblTotalEmp);
            groupBox1.Font = new Font("Segoe UI", 16.2F, FontStyle.Bold);
            groupBox1.ForeColor = Color.White;
            groupBox1.Location = new Point(0, 645);
            groupBox1.Margin = new Padding(3, 4, 3, 4);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(3, 4, 3, 4);
            groupBox1.Size = new Size(312, 236);
            groupBox1.TabIndex = 8;
            groupBox1.TabStop = false;
            groupBox1.Text = "Total Employee";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(7, 56);
            pictureBox1.Margin = new Padding(3, 4, 3, 4);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(138, 151);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 2;
            pictureBox1.TabStop = false;
            // 
            // lblTotalEmp
            // 
            lblTotalEmp.AutoSize = true;
            lblTotalEmp.Font = new Font("Segoe UI", 36F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTotalEmp.ForeColor = Color.White;
            lblTotalEmp.Location = new Point(174, 80);
            lblTotalEmp.Name = "lblTotalEmp";
            lblTotalEmp.Size = new Size(67, 81);
            lblTotalEmp.TabIndex = 0;
            lblTotalEmp.Text = "0";
            // 
            // refreshTimer
            // 
            refreshTimer.Tick += refreshTimer_Tick;
            // 
            // groupBox4
            // 
            groupBox4.BackColor = Color.FromArgb(27, 60, 83);
            groupBox4.Controls.Add(dgvAttendanceStatus);
            groupBox4.Controls.Add(label2);
            groupBox4.Controls.Add(cbSanctions);
            groupBox4.Controls.Add(btnGenerate);
            groupBox4.Font = new Font("Segoe UI", 16.2F, FontStyle.Bold);
            groupBox4.ForeColor = Color.White;
            groupBox4.Location = new Point(363, 147);
            groupBox4.Margin = new Padding(3, 4, 3, 4);
            groupBox4.Name = "groupBox4";
            groupBox4.Padding = new Padding(3, 4, 3, 4);
            groupBox4.Size = new Size(463, 507);
            groupBox4.TabIndex = 9;
            groupBox4.TabStop = false;
            groupBox4.Text = "Attendance Status";
            // 
            // dgvAttendanceStatus
            // 
            dgvAttendanceStatus.AllowUserToAddRows = false;
            dgvAttendanceStatus.AllowUserToDeleteRows = false;
            dgvAttendanceStatus.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvAttendanceStatus.Location = new Point(12, 43);
            dgvAttendanceStatus.Name = "dgvAttendanceStatus";
            dgvAttendanceStatus.ReadOnly = true;
            dgvAttendanceStatus.RowHeadersWidth = 51;
            dgvAttendanceStatus.Size = new Size(445, 367);
            dgvAttendanceStatus.TabIndex = 5;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            label2.Location = new Point(10, 413);
            label2.Name = "label2";
            label2.Size = new Size(174, 32);
            label2.TabIndex = 4;
            label2.Text = "Sanction Type";
            // 
            // cbSanctions
            // 
            cbSanctions.DropDownStyle = ComboBoxStyle.DropDownList;
            cbSanctions.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            cbSanctions.FormattingEnabled = true;
            cbSanctions.Items.AddRange(new object[] { "All", "Written Warning Letter", "Suspension Letter", "Dismissal Letter" });
            cbSanctions.Location = new Point(10, 452);
            cbSanctions.Name = "cbSanctions";
            cbSanctions.Size = new Size(245, 36);
            cbSanctions.TabIndex = 3;
            // 
            // btnGenerate
            // 
            btnGenerate.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnGenerate.ForeColor = Color.Black;
            btnGenerate.Location = new Point(261, 416);
            btnGenerate.Name = "btnGenerate";
            btnGenerate.Size = new Size(196, 80);
            btnGenerate.TabIndex = 2;
            btnGenerate.Text = "Generate Sanctions";
            btnGenerate.UseVisualStyleBackColor = true;
            btnGenerate.Click += btnGenerate_Click;
            // 
            // todayTimer
            // 
            todayTimer.Tick += todayTimer_Tick;
            // 
            // FormDashboard
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(27, 60, 83);
            ClientSize = new Size(1902, 892);
            Controls.Add(groupBox4);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FormDashboard";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FormDashboard";
            Load += FormDashboard_Load;
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            groupBox5.ResumeLayout(false);
            groupBox5.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)chartRecords).EndInit();
            ((System.ComponentModel.ISupportInitialize)chartAttendance).EndInit();
            gbLeaveRequest.ResumeLayout(false);
            gbLeaveRequest.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pbLeave).EndInit();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAttendanceStatus).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private System.Windows.Forms.Timer leaveRefreshTimer;
        private System.Windows.Forms.Timer refreshTimer;
        private GroupBox groupBox1;
        private PictureBox pictureBox1;
        private Label lblTotalEmp;
        private PictureBox pbLeave;
        private Label lblLeaveNotif;
        private PictureBox pictureBox2;
        private Label lblPresentEmp;
        private GroupBox gbLeaveRequest;
        private GroupBox groupBox2;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartAttendance;
        private DateTimePicker dtpFrom;
        private DateTimePicker dtpTo;
        private Button btnMonth;
        private Button btnDays;
        private Button btnToday;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartRecords;
        private Panel panel2;
        private Label label1;
        private GroupBox groupBox4;
        private Panel panelAttendanceStatus;
        private Label lblSY;
        private GroupBox groupBox5;
        private GroupBox groupBox3;
        private Label lblToday;
        private System.Windows.Forms.Timer todayTimer;
        private Button btnGenerate;
        private ComboBox cbSanctions;
        private Label label2;
        private DataGridView dgvAttendanceStatus;
    }
}