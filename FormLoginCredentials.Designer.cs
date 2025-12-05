namespace HRISCDBS
{
    partial class FormLoginCredentials
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
            panel1 = new Panel();
            panel2 = new Panel();
            cbEmployeeType = new ComboBox();
            label4 = new Label();
            label2 = new Label();
            panel3 = new Panel();
            label1 = new Label();
            tbSearch = new TextBox();
            btnEdit = new Button();
            dgvEmployeeRecords = new DataGridView();
            refreshTimer = new System.Windows.Forms.Timer(components);
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvEmployeeRecords).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.ActiveCaption;
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
            panel2.Controls.Add(label4);
            panel2.Controls.Add(label2);
            panel2.Controls.Add(panel3);
            panel2.Controls.Add(tbSearch);
            panel2.Controls.Add(btnEdit);
            panel2.Controls.Add(dgvEmployeeRecords);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(1526, 881);
            panel2.TabIndex = 1;
            // 
            // cbEmployeeType
            // 
            cbEmployeeType.DropDownStyle = ComboBoxStyle.DropDownList;
            cbEmployeeType.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cbEmployeeType.FormattingEnabled = true;
            cbEmployeeType.Items.AddRange(new object[] { "All", "Teaching", "Non-Teaching", "Maintenance", "Coach" });
            cbEmployeeType.Location = new Point(179, 114);
            cbEmployeeType.Name = "cbEmployeeType";
            cbEmployeeType.Size = new Size(187, 31);
            cbEmployeeType.TabIndex = 16;
            cbEmployeeType.SelectedIndexChanged += cbEmployeeType_SelectedIndexChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            label4.Location = new Point(1222, 114);
            label4.Name = "label4";
            label4.Size = new Size(77, 28);
            label4.TabIndex = 10;
            label4.Text = "Search:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(18, 114);
            label2.Name = "label2";
            label2.Size = new Size(155, 28);
            label2.TabIndex = 15;
            label2.Text = "Employee Type:";
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
            label1.Size = new Size(332, 54);
            label1.TabIndex = 0;
            label1.Text = "Login Credentials";
            // 
            // tbSearch
            // 
            tbSearch.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            tbSearch.Location = new Point(1305, 112);
            tbSearch.Name = "tbSearch";
            tbSearch.Size = new Size(206, 34);
            tbSearch.TabIndex = 3;
            tbSearch.TextChanged += tbSearch_TextChanged;
            // 
            // btnEdit
            // 
            btnEdit.BackColor = Color.FromArgb(255, 246, 224);
            btnEdit.FlatStyle = FlatStyle.Flat;
            btnEdit.Location = new Point(1122, 117);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(94, 29);
            btnEdit.TabIndex = 2;
            btnEdit.Text = "Edit";
            btnEdit.UseVisualStyleBackColor = false;
            btnEdit.Click += btnEdit_Click;
            // 
            // dgvEmployeeRecords
            // 
            dgvEmployeeRecords.AllowUserToAddRows = false;
            dgvEmployeeRecords.AllowUserToDeleteRows = false;
            dgvEmployeeRecords.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvEmployeeRecords.Location = new Point(18, 155);
            dgvEmployeeRecords.MultiSelect = false;
            dgvEmployeeRecords.Name = "dgvEmployeeRecords";
            dgvEmployeeRecords.ReadOnly = true;
            dgvEmployeeRecords.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;
            dgvEmployeeRecords.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEmployeeRecords.Size = new Size(1493, 704);
            dgvEmployeeRecords.TabIndex = 0;
            // 
            // refreshTimer
            // 
            refreshTimer.Tick += refreshTimer_Tick;
            // 
            // FormLoginCredentials
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(219, 226, 239);
            ClientSize = new Size(1902, 892);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FormLoginCredentials";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FormLoginCredentials";
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvEmployeeRecords).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
        private TextBox tbSearch;
        private Button btnEdit;
        private DataGridView dgvEmployeeRecords;
        private Label label1;
        private System.Windows.Forms.Timer refreshTimer;
        private Label label4;
        private ComboBox cbEmployeeType;
        private Label label2;
    }
}