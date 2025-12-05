namespace HRISCDBS
{
    partial class FormLogTrail
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            panel1 = new Panel();
            label4 = new Label();
            dtp_to = new DateTimePicker();
            label3 = new Label();
            dtp_from = new DateTimePicker();
            label2 = new Label();
            tbSearch = new TextBox();
            dgvLogTrail = new DataGridView();
            panel3 = new Panel();
            label1 = new Label();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvLogTrail).BeginInit();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.ActiveCaption;
            panel1.Controls.Add(label4);
            panel1.Controls.Add(dtp_to);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(dtp_from);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(tbSearch);
            panel1.Controls.Add(dgvLogTrail);
            panel1.Controls.Add(panel3);
            panel1.Location = new Point(363, 5);
            panel1.Name = "panel1";
            panel1.Size = new Size(1526, 881);
            panel1.TabIndex = 0;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            label4.Location = new Point(1191, 115);
            label4.Name = "label4";
            label4.Size = new Size(77, 28);
            label4.TabIndex = 15;
            label4.Text = "Search:";
            // 
            // dtp_to
            // 
            dtp_to.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            dtp_to.Format = DateTimePickerFormat.Short;
            dtp_to.Location = new Point(289, 109);
            dtp_to.Margin = new Padding(3, 4, 3, 4);
            dtp_to.MaxDate = new DateTime(2027, 12, 31, 0, 0, 0, 0);
            dtp_to.MinDate = new DateTime(2025, 1, 1, 0, 0, 0, 0);
            dtp_to.Name = "dtp_to";
            dtp_to.Size = new Size(141, 34);
            dtp_to.TabIndex = 14;
            dtp_to.ValueChanged += dtp_to_ValueChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            label3.Location = new Point(245, 115);
            label3.Name = "label3";
            label3.Size = new Size(38, 28);
            label3.TabIndex = 13;
            label3.Text = "To:";
            // 
            // dtp_from
            // 
            dtp_from.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            dtp_from.Format = DateTimePickerFormat.Short;
            dtp_from.Location = new Point(90, 109);
            dtp_from.Margin = new Padding(3, 4, 3, 4);
            dtp_from.MaxDate = new DateTime(2027, 12, 31, 0, 0, 0, 0);
            dtp_from.MinDate = new DateTime(2025, 1, 1, 0, 0, 0, 0);
            dtp_from.Name = "dtp_from";
            dtp_from.Size = new Size(141, 34);
            dtp_from.TabIndex = 12;
            dtp_from.ValueChanged += dtp_from_ValueChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            label2.Location = new Point(21, 115);
            label2.Name = "label2";
            label2.Size = new Size(64, 28);
            label2.TabIndex = 10;
            label2.Text = "From:";
            // 
            // tbSearch
            // 
            tbSearch.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            tbSearch.Location = new Point(1274, 113);
            tbSearch.Name = "tbSearch";
            tbSearch.Size = new Size(238, 34);
            tbSearch.TabIndex = 11;
            tbSearch.TextChanged += tbSearch_TextChanged;
            // 
            // dgvLogTrail
            // 
            dgvLogTrail.AllowUserToAddRows = false;
            dgvLogTrail.AllowUserToDeleteRows = false;
            dgvLogTrail.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvLogTrail.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvLogTrail.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Window;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvLogTrail.DefaultCellStyle = dataGridViewCellStyle1;
            dgvLogTrail.Location = new Point(18, 155);
            dgvLogTrail.MultiSelect = false;
            dgvLogTrail.Name = "dgvLogTrail";
            dgvLogTrail.ReadOnly = true;
            dgvLogTrail.RowHeadersVisible = false;
            dgvLogTrail.RowHeadersWidth = 51;
            dgvLogTrail.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLogTrail.Size = new Size(1493, 704);
            dgvLogTrail.TabIndex = 6;
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
            panel3.TabIndex = 5;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 24F);
            label1.Location = new Point(3, 7);
            label1.Name = "label1";
            label1.Size = new Size(189, 54);
            label1.TabIndex = 0;
            label1.Text = "Log Trails";
            // 
            // FormLogTrail
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(219, 226, 239);
            ClientSize = new Size(1902, 892);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FormLogTrail";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FormLogTrail";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvLogTrail).EndInit();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel panel3;
        private Label label1;
        private DataGridView dgvLogTrail;
        private Label label4;
        private DateTimePicker dtp_to;
        private Label label3;
        private DateTimePicker dtp_from;
        private Label label2;
        private TextBox tbSearch;
    }
}