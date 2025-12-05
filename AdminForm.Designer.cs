namespace HRISCDBS
{
    partial class AdminForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdminForm));
            panel1 = new Panel();
            label1 = new Label();
            panel2 = new Panel();
            flowLayoutPanel1 = new FlowLayoutPanel();
            panel3 = new Panel();
            btnDashboard = new Button();
            panel8 = new Panel();
            btnAttendance = new Button();
            flpApplicant = new FlowLayoutPanel();
            panel4 = new Panel();
            btnApplicantRecordsMgm = new Button();
            panel6 = new Panel();
            btnApplicantRecords = new Button();
            panel9 = new Panel();
            btnReqRecords = new Button();
            flpEmployee = new FlowLayoutPanel();
            panel5 = new Panel();
            btnEmployeeRecordsMgm = new Button();
            panel11 = new Panel();
            btnEmployeeRecords = new Button();
            panel12 = new Panel();
            btnLoginCredentials = new Button();
            panel14 = new Panel();
            btnLeave = new Button();
            panel7 = new Panel();
            btnLogTrail = new Button();
            panel10 = new Panel();
            btnBackupRestore = new Button();
            panel13 = new Panel();
            btnLogout = new Button();
            applicantTimer = new System.Windows.Forms.Timer(components);
            employeeTimer = new System.Windows.Forms.Timer(components);
            panel1.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            panel3.SuspendLayout();
            panel8.SuspendLayout();
            flpApplicant.SuspendLayout();
            panel4.SuspendLayout();
            panel6.SuspendLayout();
            panel9.SuspendLayout();
            flpEmployee.SuspendLayout();
            panel5.SuspendLayout();
            panel11.SuspendLayout();
            panel12.SuspendLayout();
            panel14.SuspendLayout();
            panel7.SuspendLayout();
            panel10.SuspendLayout();
            panel13.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(17, 45, 78);
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1902, 82);
            panel1.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("STLiti", 38F, FontStyle.Bold | FontStyle.Italic);
            label1.ForeColor = Color.White;
            label1.Location = new Point(4, 8);
            label1.Name = "label1";
            label1.Size = new Size(639, 65);
            label1.TabIndex = 0;
            label1.Text = "Caritas Don Bosco School";
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(17, 45, 78);
            panel2.Dock = DockStyle.Bottom;
            panel2.Location = new Point(0, 981);
            panel2.Margin = new Padding(0);
            panel2.Name = "panel2";
            panel2.Size = new Size(1902, 52);
            panel2.TabIndex = 1;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.BackColor = Color.FromArgb(17, 45, 78);
            flowLayoutPanel1.Controls.Add(panel3);
            flowLayoutPanel1.Controls.Add(panel8);
            flowLayoutPanel1.Controls.Add(flpApplicant);
            flowLayoutPanel1.Controls.Add(flpEmployee);
            flowLayoutPanel1.Controls.Add(panel7);
            flowLayoutPanel1.Controls.Add(panel10);
            flowLayoutPanel1.Controls.Add(panel13);
            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel1.Location = new Point(0, 83);
            flowLayoutPanel1.Margin = new Padding(0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(358, 907);
            flowLayoutPanel1.TabIndex = 2;
            // 
            // panel3
            // 
            panel3.Controls.Add(btnDashboard);
            panel3.Location = new Point(0, 0);
            panel3.Margin = new Padding(0);
            panel3.Name = "panel3";
            panel3.Size = new Size(358, 76);
            panel3.TabIndex = 3;
            // 
            // btnDashboard
            // 
            btnDashboard.BackColor = Color.FromArgb(249, 247, 247);
            btnDashboard.Dock = DockStyle.Fill;
            btnDashboard.FlatStyle = FlatStyle.Flat;
            btnDashboard.Image = (Image)resources.GetObject("btnDashboard.Image");
            btnDashboard.ImageAlign = ContentAlignment.MiddleLeft;
            btnDashboard.Location = new Point(0, 0);
            btnDashboard.Name = "btnDashboard";
            btnDashboard.Padding = new Padding(15, 0, 0, 0);
            btnDashboard.Size = new Size(358, 76);
            btnDashboard.TabIndex = 0;
            btnDashboard.Text = "Dashboard";
            btnDashboard.UseVisualStyleBackColor = false;
            btnDashboard.Click += btnDashboard_Click;
            // 
            // panel8
            // 
            panel8.Controls.Add(btnAttendance);
            panel8.Location = new Point(0, 76);
            panel8.Margin = new Padding(0);
            panel8.Name = "panel8";
            panel8.Size = new Size(358, 76);
            panel8.TabIndex = 5;
            // 
            // btnAttendance
            // 
            btnAttendance.BackColor = Color.FromArgb(249, 247, 247);
            btnAttendance.Dock = DockStyle.Fill;
            btnAttendance.FlatStyle = FlatStyle.Flat;
            btnAttendance.Image = (Image)resources.GetObject("btnAttendance.Image");
            btnAttendance.ImageAlign = ContentAlignment.MiddleLeft;
            btnAttendance.Location = new Point(0, 0);
            btnAttendance.Name = "btnAttendance";
            btnAttendance.Padding = new Padding(15, 0, 0, 0);
            btnAttendance.Size = new Size(358, 76);
            btnAttendance.TabIndex = 0;
            btnAttendance.Text = "Attendance Monitoring";
            btnAttendance.UseVisualStyleBackColor = false;
            btnAttendance.Click += btnAttendance_Click;
            // 
            // flpApplicant
            // 
            flpApplicant.Controls.Add(panel4);
            flpApplicant.Controls.Add(panel6);
            flpApplicant.Controls.Add(panel9);
            flpApplicant.Location = new Point(0, 152);
            flpApplicant.Margin = new Padding(0);
            flpApplicant.Name = "flpApplicant";
            flpApplicant.Size = new Size(358, 76);
            flpApplicant.TabIndex = 7;
            // 
            // panel4
            // 
            panel4.Controls.Add(btnApplicantRecordsMgm);
            panel4.Location = new Point(0, 0);
            panel4.Margin = new Padding(0);
            panel4.Name = "panel4";
            panel4.Size = new Size(358, 76);
            panel4.TabIndex = 4;
            // 
            // btnApplicantRecordsMgm
            // 
            btnApplicantRecordsMgm.BackColor = Color.FromArgb(249, 247, 247);
            btnApplicantRecordsMgm.Dock = DockStyle.Fill;
            btnApplicantRecordsMgm.FlatStyle = FlatStyle.Flat;
            btnApplicantRecordsMgm.Image = (Image)resources.GetObject("btnApplicantRecordsMgm.Image");
            btnApplicantRecordsMgm.ImageAlign = ContentAlignment.MiddleLeft;
            btnApplicantRecordsMgm.Location = new Point(0, 0);
            btnApplicantRecordsMgm.Margin = new Padding(0);
            btnApplicantRecordsMgm.Name = "btnApplicantRecordsMgm";
            btnApplicantRecordsMgm.Padding = new Padding(15, 0, 0, 0);
            btnApplicantRecordsMgm.Size = new Size(358, 76);
            btnApplicantRecordsMgm.TabIndex = 0;
            btnApplicantRecordsMgm.Text = "Applicant Records Management";
            btnApplicantRecordsMgm.UseVisualStyleBackColor = false;
            btnApplicantRecordsMgm.Click += btnApplicantRecordsMgm_Click;
            // 
            // panel6
            // 
            panel6.Controls.Add(btnApplicantRecords);
            panel6.Location = new Point(0, 76);
            panel6.Margin = new Padding(0);
            panel6.Name = "panel6";
            panel6.Size = new Size(358, 76);
            panel6.TabIndex = 5;
            // 
            // btnApplicantRecords
            // 
            btnApplicantRecords.BackColor = Color.PaleTurquoise;
            btnApplicantRecords.Dock = DockStyle.Fill;
            btnApplicantRecords.FlatStyle = FlatStyle.Flat;
            btnApplicantRecords.Image = (Image)resources.GetObject("btnApplicantRecords.Image");
            btnApplicantRecords.ImageAlign = ContentAlignment.MiddleLeft;
            btnApplicantRecords.Location = new Point(0, 0);
            btnApplicantRecords.Margin = new Padding(0);
            btnApplicantRecords.Name = "btnApplicantRecords";
            btnApplicantRecords.Padding = new Padding(50, 0, 0, 0);
            btnApplicantRecords.Size = new Size(358, 76);
            btnApplicantRecords.TabIndex = 0;
            btnApplicantRecords.Text = "Applicant Records";
            btnApplicantRecords.UseVisualStyleBackColor = false;
            btnApplicantRecords.Click += btnApplicantRecords_Click;
            // 
            // panel9
            // 
            panel9.Controls.Add(btnReqRecords);
            panel9.Location = new Point(0, 152);
            panel9.Margin = new Padding(0);
            panel9.Name = "panel9";
            panel9.Size = new Size(358, 76);
            panel9.TabIndex = 5;
            // 
            // btnReqRecords
            // 
            btnReqRecords.BackColor = Color.PaleTurquoise;
            btnReqRecords.Dock = DockStyle.Fill;
            btnReqRecords.FlatStyle = FlatStyle.Flat;
            btnReqRecords.Image = (Image)resources.GetObject("btnReqRecords.Image");
            btnReqRecords.ImageAlign = ContentAlignment.MiddleLeft;
            btnReqRecords.Location = new Point(0, 0);
            btnReqRecords.Margin = new Padding(0);
            btnReqRecords.Name = "btnReqRecords";
            btnReqRecords.Padding = new Padding(50, 0, 0, 0);
            btnReqRecords.Size = new Size(358, 76);
            btnReqRecords.TabIndex = 0;
            btnReqRecords.Text = "Requirements Records";
            btnReqRecords.UseVisualStyleBackColor = false;
            btnReqRecords.Click += btnReqRecords_Click;
            // 
            // flpEmployee
            // 
            flpEmployee.Controls.Add(panel5);
            flpEmployee.Controls.Add(panel11);
            flpEmployee.Controls.Add(panel12);
            flpEmployee.Controls.Add(panel14);
            flpEmployee.Location = new Point(0, 228);
            flpEmployee.Margin = new Padding(0);
            flpEmployee.Name = "flpEmployee";
            flpEmployee.Size = new Size(358, 76);
            flpEmployee.TabIndex = 8;
            // 
            // panel5
            // 
            panel5.Controls.Add(btnEmployeeRecordsMgm);
            panel5.Location = new Point(0, 0);
            panel5.Margin = new Padding(0);
            panel5.Name = "panel5";
            panel5.Size = new Size(358, 76);
            panel5.TabIndex = 4;
            // 
            // btnEmployeeRecordsMgm
            // 
            btnEmployeeRecordsMgm.BackColor = Color.FromArgb(249, 247, 247);
            btnEmployeeRecordsMgm.Dock = DockStyle.Fill;
            btnEmployeeRecordsMgm.FlatStyle = FlatStyle.Flat;
            btnEmployeeRecordsMgm.Image = (Image)resources.GetObject("btnEmployeeRecordsMgm.Image");
            btnEmployeeRecordsMgm.ImageAlign = ContentAlignment.MiddleLeft;
            btnEmployeeRecordsMgm.Location = new Point(0, 0);
            btnEmployeeRecordsMgm.Name = "btnEmployeeRecordsMgm";
            btnEmployeeRecordsMgm.Padding = new Padding(15, 0, 0, 0);
            btnEmployeeRecordsMgm.Size = new Size(358, 76);
            btnEmployeeRecordsMgm.TabIndex = 0;
            btnEmployeeRecordsMgm.Text = "Employee Records Management";
            btnEmployeeRecordsMgm.UseVisualStyleBackColor = false;
            btnEmployeeRecordsMgm.Click += btnEmployeeRecordsMgm_Click;
            // 
            // panel11
            // 
            panel11.Controls.Add(btnEmployeeRecords);
            panel11.Location = new Point(0, 76);
            panel11.Margin = new Padding(0);
            panel11.Name = "panel11";
            panel11.Size = new Size(358, 76);
            panel11.TabIndex = 5;
            // 
            // btnEmployeeRecords
            // 
            btnEmployeeRecords.BackColor = Color.PaleTurquoise;
            btnEmployeeRecords.Dock = DockStyle.Fill;
            btnEmployeeRecords.FlatStyle = FlatStyle.Flat;
            btnEmployeeRecords.Image = (Image)resources.GetObject("btnEmployeeRecords.Image");
            btnEmployeeRecords.ImageAlign = ContentAlignment.MiddleLeft;
            btnEmployeeRecords.Location = new Point(0, 0);
            btnEmployeeRecords.Name = "btnEmployeeRecords";
            btnEmployeeRecords.Padding = new Padding(50, 0, 0, 0);
            btnEmployeeRecords.Size = new Size(358, 76);
            btnEmployeeRecords.TabIndex = 0;
            btnEmployeeRecords.Text = "Employee Records";
            btnEmployeeRecords.UseVisualStyleBackColor = false;
            btnEmployeeRecords.Click += btnEmployeeRecords_Click;
            // 
            // panel12
            // 
            panel12.Controls.Add(btnLoginCredentials);
            panel12.Location = new Point(0, 152);
            panel12.Margin = new Padding(0);
            panel12.Name = "panel12";
            panel12.Size = new Size(358, 76);
            panel12.TabIndex = 5;
            // 
            // btnLoginCredentials
            // 
            btnLoginCredentials.BackColor = Color.PaleTurquoise;
            btnLoginCredentials.Dock = DockStyle.Fill;
            btnLoginCredentials.FlatStyle = FlatStyle.Flat;
            btnLoginCredentials.Image = (Image)resources.GetObject("btnLoginCredentials.Image");
            btnLoginCredentials.ImageAlign = ContentAlignment.MiddleLeft;
            btnLoginCredentials.Location = new Point(0, 0);
            btnLoginCredentials.Name = "btnLoginCredentials";
            btnLoginCredentials.Padding = new Padding(50, 0, 0, 0);
            btnLoginCredentials.Size = new Size(358, 76);
            btnLoginCredentials.TabIndex = 0;
            btnLoginCredentials.Text = "Login Credentials";
            btnLoginCredentials.UseVisualStyleBackColor = false;
            btnLoginCredentials.Click += btnLoginCredentials_Click;
            // 
            // panel14
            // 
            panel14.Controls.Add(btnLeave);
            panel14.Location = new Point(0, 228);
            panel14.Margin = new Padding(0);
            panel14.Name = "panel14";
            panel14.Size = new Size(358, 76);
            panel14.TabIndex = 6;
            // 
            // btnLeave
            // 
            btnLeave.BackColor = Color.PaleTurquoise;
            btnLeave.Dock = DockStyle.Fill;
            btnLeave.FlatStyle = FlatStyle.Flat;
            btnLeave.Image = (Image)resources.GetObject("btnLeave.Image");
            btnLeave.ImageAlign = ContentAlignment.MiddleLeft;
            btnLeave.Location = new Point(0, 0);
            btnLeave.Name = "btnLeave";
            btnLeave.Padding = new Padding(50, 0, 0, 0);
            btnLeave.Size = new Size(358, 76);
            btnLeave.TabIndex = 0;
            btnLeave.Text = "Leave Records ";
            btnLeave.UseVisualStyleBackColor = false;
            btnLeave.Click += btnLeave_Click;
            // 
            // panel7
            // 
            panel7.Controls.Add(btnLogTrail);
            panel7.Location = new Point(0, 304);
            panel7.Margin = new Padding(0);
            panel7.Name = "panel7";
            panel7.Size = new Size(358, 76);
            panel7.TabIndex = 5;
            // 
            // btnLogTrail
            // 
            btnLogTrail.BackColor = Color.FromArgb(249, 247, 247);
            btnLogTrail.Dock = DockStyle.Fill;
            btnLogTrail.FlatStyle = FlatStyle.Flat;
            btnLogTrail.Image = (Image)resources.GetObject("btnLogTrail.Image");
            btnLogTrail.ImageAlign = ContentAlignment.MiddleLeft;
            btnLogTrail.Location = new Point(0, 0);
            btnLogTrail.Name = "btnLogTrail";
            btnLogTrail.Padding = new Padding(15, 0, 0, 0);
            btnLogTrail.Size = new Size(358, 76);
            btnLogTrail.TabIndex = 0;
            btnLogTrail.Text = "Log Trail";
            btnLogTrail.UseVisualStyleBackColor = false;
            btnLogTrail.Click += btnLogTrail_Click;
            // 
            // panel10
            // 
            panel10.Controls.Add(btnBackupRestore);
            panel10.Location = new Point(0, 380);
            panel10.Margin = new Padding(0);
            panel10.Name = "panel10";
            panel10.Size = new Size(358, 76);
            panel10.TabIndex = 6;
            // 
            // btnBackupRestore
            // 
            btnBackupRestore.BackColor = Color.FromArgb(249, 247, 247);
            btnBackupRestore.Dock = DockStyle.Fill;
            btnBackupRestore.FlatStyle = FlatStyle.Flat;
            btnBackupRestore.Image = (Image)resources.GetObject("btnBackupRestore.Image");
            btnBackupRestore.ImageAlign = ContentAlignment.MiddleLeft;
            btnBackupRestore.Location = new Point(0, 0);
            btnBackupRestore.Name = "btnBackupRestore";
            btnBackupRestore.Padding = new Padding(15, 0, 0, 0);
            btnBackupRestore.Size = new Size(358, 76);
            btnBackupRestore.TabIndex = 0;
            btnBackupRestore.Text = "Database Backup/Restore";
            btnBackupRestore.UseVisualStyleBackColor = false;
            btnBackupRestore.Click += btnBackupRestore_Click;
            // 
            // panel13
            // 
            panel13.Controls.Add(btnLogout);
            panel13.Location = new Point(0, 456);
            panel13.Margin = new Padding(0);
            panel13.Name = "panel13";
            panel13.Size = new Size(358, 76);
            panel13.TabIndex = 7;
            // 
            // btnLogout
            // 
            btnLogout.BackColor = Color.FromArgb(249, 247, 247);
            btnLogout.Dock = DockStyle.Fill;
            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.Image = (Image)resources.GetObject("btnLogout.Image");
            btnLogout.ImageAlign = ContentAlignment.MiddleLeft;
            btnLogout.Location = new Point(0, 0);
            btnLogout.Name = "btnLogout";
            btnLogout.Padding = new Padding(15, 0, 0, 0);
            btnLogout.Size = new Size(358, 76);
            btnLogout.TabIndex = 0;
            btnLogout.Text = "Log out";
            btnLogout.UseVisualStyleBackColor = false;
            btnLogout.Click += btnLogout_Click;
            // 
            // applicantTimer
            // 
            applicantTimer.Interval = 5;
            applicantTimer.Tick += applicantTimer_Tick;
            // 
            // employeeTimer
            // 
            employeeTimer.Interval = 5;
            employeeTimer.Tick += employeeTimer_Tick;
            // 
            // AdminForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1902, 1033);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(panel2);
            Controls.Add(panel1);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.None;
            IsMdiContainer = true;
            Name = "AdminForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "AdminForm";
            Load += AdminForm_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            flowLayoutPanel1.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel8.ResumeLayout(false);
            flpApplicant.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel6.ResumeLayout(false);
            panel9.ResumeLayout(false);
            flpEmployee.ResumeLayout(false);
            panel5.ResumeLayout(false);
            panel11.ResumeLayout(false);
            panel12.ResumeLayout(false);
            panel14.ResumeLayout(false);
            panel7.ResumeLayout(false);
            panel10.ResumeLayout(false);
            panel13.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel panel2;
        private FlowLayoutPanel flowLayoutPanel1;
        private Panel panel3;
        private Button btnDashboard;
        private Panel panel4;
        private Button btnApplicantRecordsMgm;
        private Panel panel5;
        private Button btnEmployeeRecordsMgm;
        private Panel panel7;
        private Button btnLogTrail;
        private Panel panel8;
        private Button btnAttendance;
        private Panel panel10;
        private Button btnBackupRestore;
        private FlowLayoutPanel flpApplicant;
        private Panel panel6;
        private Button btnApplicantRecords;
        private Panel panel9;
        private Button btnReqRecords;
        private FlowLayoutPanel flpEmployee;
        private Panel panel11;
        private Button btnEmployeeRecords;
        private Panel panel12;
        private Button btnLoginCredentials;
        private System.Windows.Forms.Timer applicantTimer;
        private System.Windows.Forms.Timer employeeTimer;
        private Panel panel13;
        private Button btnLogout;
        private Panel panel14;
        private Button btnLeave;
        private Label label1;
    }
}