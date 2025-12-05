namespace HRISCDBS
{
    partial class FormEmployeeRecords
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
            btnGenerateEmployees = new Button();
            label4 = new Label();
            tbSearch = new TextBox();
            cbEmployeeType = new ComboBox();
            label2 = new Label();
            panel2 = new Panel();
            label1 = new Label();
            btnEdit = new Button();
            btnAddnew = new Button();
            dgvEmployeeRecords = new DataGridView();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvEmployeeRecords).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.ActiveCaption;
            panel1.Controls.Add(btnGenerateEmployees);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(tbSearch);
            panel1.Controls.Add(cbEmployeeType);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(btnEdit);
            panel1.Controls.Add(btnAddnew);
            panel1.Controls.Add(dgvEmployeeRecords);
            panel1.Location = new Point(363, 5);
            panel1.Name = "panel1";
            panel1.Size = new Size(1526, 881);
            panel1.TabIndex = 0;
            // 
            // btnGenerateEmployees
            // 
            btnGenerateEmployees.BackColor = Color.FromArgb(255, 246, 224);
            btnGenerateEmployees.FlatStyle = FlatStyle.Flat;
            btnGenerateEmployees.Location = new Point(1272, 825);
            btnGenerateEmployees.Name = "btnGenerateEmployees";
            btnGenerateEmployees.Size = new Size(238, 43);
            btnGenerateEmployees.TabIndex = 11;
            btnGenerateEmployees.Text = "Generate Employee Records List";
            btnGenerateEmployees.UseVisualStyleBackColor = false;
            btnGenerateEmployees.Click += btnGenerateEmployees_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            label4.Location = new Point(1221, 99);
            label4.Name = "label4";
            label4.Size = new Size(77, 28);
            label4.TabIndex = 10;
            label4.Text = "Search:";
            // 
            // tbSearch
            // 
            tbSearch.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            tbSearch.Location = new Point(1304, 96);
            tbSearch.Name = "tbSearch";
            tbSearch.Size = new Size(206, 34);
            tbSearch.TabIndex = 3;
            tbSearch.TextChanged += tbSearch_TextChanged;
            // 
            // cbEmployeeType
            // 
            cbEmployeeType.DropDownStyle = ComboBoxStyle.DropDownList;
            cbEmployeeType.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cbEmployeeType.FormattingEnabled = true;
            cbEmployeeType.Items.AddRange(new object[] { "All", "Teaching", "Non-Teaching", "Maintenance", "Coach" });
            cbEmployeeType.Location = new Point(179, 95);
            cbEmployeeType.Name = "cbEmployeeType";
            cbEmployeeType.Size = new Size(187, 31);
            cbEmployeeType.TabIndex = 6;
            cbEmployeeType.SelectedIndexChanged += cbEmployeeType_SelectedIndexChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(18, 95);
            label2.Name = "label2";
            label2.Size = new Size(155, 28);
            label2.TabIndex = 5;
            label2.Text = "Employee Type:";
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(255, 246, 224);
            panel2.Controls.Add(label1);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Margin = new Padding(3, 4, 3, 4);
            panel2.Name = "panel2";
            panel2.Size = new Size(1526, 76);
            panel2.TabIndex = 4;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 24F);
            label1.Location = new Point(3, 7);
            label1.Name = "label1";
            label1.Size = new Size(346, 54);
            label1.TabIndex = 0;
            label1.Text = "Employee Records";
            // 
            // btnEdit
            // 
            btnEdit.BackColor = Color.White;
            btnEdit.FlatStyle = FlatStyle.Flat;
            btnEdit.Location = new Point(1121, 98);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(94, 29);
            btnEdit.TabIndex = 2;
            btnEdit.Text = "Edit";
            btnEdit.UseVisualStyleBackColor = false;
            btnEdit.Click += btnEdit_Click;
            // 
            // btnAddnew
            // 
            btnAddnew.BackColor = Color.White;
            btnAddnew.FlatStyle = FlatStyle.Flat;
            btnAddnew.Location = new Point(1021, 98);
            btnAddnew.Name = "btnAddnew";
            btnAddnew.Size = new Size(94, 29);
            btnAddnew.TabIndex = 1;
            btnAddnew.Text = "Add New";
            btnAddnew.UseVisualStyleBackColor = false;
            btnAddnew.Click += btnAddnew_Click;
            // 
            // dgvEmployeeRecords
            // 
            dgvEmployeeRecords.AllowUserToAddRows = false;
            dgvEmployeeRecords.AllowUserToDeleteRows = false;
            dgvEmployeeRecords.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvEmployeeRecords.Location = new Point(18, 140);
            dgvEmployeeRecords.MultiSelect = false;
            dgvEmployeeRecords.Name = "dgvEmployeeRecords";
            dgvEmployeeRecords.ReadOnly = true;
            dgvEmployeeRecords.RowHeadersVisible = false;
            dgvEmployeeRecords.RowHeadersWidth = 51;
            dgvEmployeeRecords.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEmployeeRecords.Size = new Size(1493, 666);
            dgvEmployeeRecords.TabIndex = 0;
            // 
            // FormEmployeeRecords
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(219, 226, 239);
            ClientSize = new Size(1902, 892);
            Controls.Add(panel1);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.None;
            Name = "FormEmployeeRecords";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FormEmployeeRecords";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvEmployeeRecords).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private TextBox tbSearch;
        private Button btnEdit;
        private Button btnAddnew;
        private DataGridView dgvEmployeeRecords;
        private Panel panel2;
        private Label label1;
        private Label label4;
        private Label label2;
        private ComboBox cbEmployeeType;
        private Button btnGenerateEmployees;
    }
}