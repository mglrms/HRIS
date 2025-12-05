namespace HRISCDBS
{
    partial class FormLeave
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
            label4 = new Label();
            panel2 = new Panel();
            label1 = new Label();
            tbSearch = new TextBox();
            btnEdit = new Button();
            dgvLeaves = new DataGridView();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvLeaves).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.ActiveCaption;
            panel1.Controls.Add(label4);
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(tbSearch);
            panel1.Controls.Add(btnEdit);
            panel1.Controls.Add(dgvLeaves);
            panel1.Location = new Point(363, 5);
            panel1.Name = "panel1";
            panel1.Size = new Size(1526, 881);
            panel1.TabIndex = 1;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            label4.Location = new Point(1222, 110);
            label4.Name = "label4";
            label4.Size = new Size(77, 28);
            label4.TabIndex = 10;
            label4.Text = "Search:";
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
            label1.Location = new Point(3, 2);
            label1.Name = "label1";
            label1.Size = new Size(274, 54);
            label1.TabIndex = 0;
            label1.Text = "Leave Records";
            // 
            // tbSearch
            // 
            tbSearch.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            tbSearch.Location = new Point(1305, 109);
            tbSearch.Name = "tbSearch";
            tbSearch.Size = new Size(206, 34);
            tbSearch.TabIndex = 3;
            tbSearch.TextChanged += tbSearch_TextChanged;
            // 
            // btnEdit
            // 
            btnEdit.BackColor = Color.FromArgb(255, 246, 224);
            btnEdit.FlatStyle = FlatStyle.Flat;
            btnEdit.Location = new Point(1122, 113);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(94, 29);
            btnEdit.TabIndex = 2;
            btnEdit.Text = "Update";
            btnEdit.UseVisualStyleBackColor = false;
            btnEdit.Click += btnEdit_Click;
            // 
            // dgvLeaves
            // 
            dgvLeaves.AllowUserToAddRows = false;
            dgvLeaves.AllowUserToDeleteRows = false;
            dgvLeaves.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvLeaves.Location = new Point(18, 150);
            dgvLeaves.MultiSelect = false;
            dgvLeaves.Name = "dgvLeaves";
            dgvLeaves.ReadOnly = true;
            dgvLeaves.RowHeadersVisible = false;
            dgvLeaves.RowHeadersWidth = 51;
            dgvLeaves.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLeaves.Size = new Size(1493, 704);
            dgvLeaves.TabIndex = 0;
            // 
            // FormLeave
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1902, 892);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FormLeave";
            Text = "FormLeave";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvLeaves).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel panel2;
        private Label label1;
        private TextBox tbSearch;
        private Button btnEdit;
        private DataGridView dgvLeaves;
        private Label label4;
    }
}