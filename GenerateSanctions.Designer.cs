namespace HRISCDBS
{
    partial class GenerateSanctions
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
            btnCancel = new Button();
            btnGenerate = new Button();
            cbSanctionType = new ComboBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            lblDays = new Label();
            cbDays = new ComboBox();
            dgvEmployeeList = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgvEmployeeList).BeginInit();
            SuspendLayout();
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(340, 386);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(103, 56);
            btnCancel.TabIndex = 0;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnGenerate
            // 
            btnGenerate.BackColor = Color.FromArgb(27, 60, 83);
            btnGenerate.FlatStyle = FlatStyle.Flat;
            btnGenerate.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnGenerate.ForeColor = Color.White;
            btnGenerate.Location = new Point(449, 386);
            btnGenerate.Name = "btnGenerate";
            btnGenerate.Size = new Size(129, 56);
            btnGenerate.TabIndex = 1;
            btnGenerate.Text = "Generate Letter";
            btnGenerate.UseVisualStyleBackColor = false;
            btnGenerate.Click += btnGenerate_Click;
            // 
            // cbSanctionType
            // 
            cbSanctionType.DropDownStyle = ComboBoxStyle.DropDownList;
            cbSanctionType.Enabled = false;
            cbSanctionType.Font = new Font("Segoe UI", 12F);
            cbSanctionType.FormattingEnabled = true;
            cbSanctionType.Items.AddRange(new object[] { "Written Warning Letter", "Suspension Letter", "Dismissal Letter" });
            cbSanctionType.Location = new Point(340, 106);
            cbSanctionType.Name = "cbSanctionType";
            cbSanctionType.Size = new Size(238, 36);
            cbSanctionType.TabIndex = 3;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            label1.Location = new Point(25, 75);
            label1.Name = "label1";
            label1.Size = new Size(138, 28);
            label1.TabIndex = 4;
            label1.Text = "Employee List";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            label2.Location = new Point(340, 75);
            label2.Name = "label2";
            label2.Size = new Size(139, 28);
            label2.TabIndex = 5;
            label2.Text = "Sanction Type";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            label3.Location = new Point(71, 9);
            label3.Name = "label3";
            label3.Size = new Size(448, 46);
            label3.TabIndex = 6;
            label3.Text = "Sanction Letter Generation";
            // 
            // lblDays
            // 
            lblDays.AutoSize = true;
            lblDays.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            lblDays.Location = new Point(340, 161);
            lblDays.Name = "lblDays";
            lblDays.Size = new Size(60, 28);
            lblDays.TabIndex = 8;
            lblDays.Text = "Days:";
            // 
            // cbDays
            // 
            cbDays.DropDownStyle = ComboBoxStyle.DropDownList;
            cbDays.Font = new Font("Segoe UI", 12F);
            cbDays.FormattingEnabled = true;
            cbDays.Items.AddRange(new object[] { "3", "5", "10" });
            cbDays.Location = new Point(340, 192);
            cbDays.Name = "cbDays";
            cbDays.Size = new Size(124, 36);
            cbDays.TabIndex = 10;
            // 
            // dgvEmployeeList
            // 
            dgvEmployeeList.AllowUserToAddRows = false;
            dgvEmployeeList.AllowUserToDeleteRows = false;
            dgvEmployeeList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvEmployeeList.Location = new Point(28, 106);
            dgvEmployeeList.Name = "dgvEmployeeList";
            dgvEmployeeList.ReadOnly = true;
            dgvEmployeeList.RowHeadersWidth = 51;
            dgvEmployeeList.Size = new Size(306, 336);
            dgvEmployeeList.TabIndex = 11;
            // 
            // GenerateSanctions
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(255, 246, 224);
            ClientSize = new Size(584, 458);
            Controls.Add(dgvEmployeeList);
            Controls.Add(cbDays);
            Controls.Add(lblDays);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(cbSanctionType);
            Controls.Add(btnGenerate);
            Controls.Add(btnCancel);
            FormBorderStyle = FormBorderStyle.None;
            Name = "GenerateSanctions";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "GenerateSanctions";
            ((System.ComponentModel.ISupportInitialize)dgvEmployeeList).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnCancel;
        private Button btnGenerate;
        private ComboBox cbSanctionType;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label lblDays;
        private ComboBox cbDays;
        private DataGridView dgvEmployeeList;
    }
}