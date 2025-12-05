namespace HRISCDBS
{
    partial class FormDB_BackupRestore
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
            panel5 = new Panel();
            btnRestore = new Button();
            tbRestore = new TextBox();
            btnBrowse2 = new Button();
            label4 = new Label();
            panel6 = new Panel();
            label2 = new Label();
            panel2 = new Panel();
            btnBackup = new Button();
            btnBrowse = new Button();
            tbBackup = new TextBox();
            label3 = new Label();
            panel4 = new Panel();
            label1 = new Label();
            panel3 = new Panel();
            label5 = new Label();
            panel1.SuspendLayout();
            panel5.SuspendLayout();
            panel6.SuspendLayout();
            panel2.SuspendLayout();
            panel4.SuspendLayout();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.ActiveCaption;
            panel1.Controls.Add(panel5);
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(panel3);
            panel1.Location = new Point(318, 4);
            panel1.Margin = new Padding(3, 2, 3, 2);
            panel1.Name = "panel1";
            panel1.Size = new Size(1335, 661);
            panel1.TabIndex = 0;
            // 
            // panel5
            // 
            panel5.BackColor = Color.FromArgb(255, 246, 224);
            panel5.Controls.Add(btnRestore);
            panel5.Controls.Add(tbRestore);
            panel5.Controls.Add(btnBrowse2);
            panel5.Controls.Add(label4);
            panel5.Controls.Add(panel6);
            panel5.Location = new Point(422, 390);
            panel5.Name = "panel5";
            panel5.Size = new Size(569, 238);
            panel5.TabIndex = 7;
            // 
            // btnRestore
            // 
            btnRestore.BackColor = Color.FromArgb(217, 234, 253);
            btnRestore.FlatStyle = FlatStyle.Flat;
            btnRestore.Location = new Point(448, 143);
            btnRestore.Name = "btnRestore";
            btnRestore.Size = new Size(75, 23);
            btnRestore.TabIndex = 6;
            btnRestore.Text = "Restore";
            btnRestore.UseVisualStyleBackColor = false;
            btnRestore.Click += btnRestore_Click_1;
            // 
            // tbRestore
            // 
            tbRestore.Location = new Point(124, 119);
            tbRestore.Name = "tbRestore";
            tbRestore.Size = new Size(306, 23);
            tbRestore.TabIndex = 3;
            // 
            // btnBrowse2
            // 
            btnBrowse2.BackColor = Color.FromArgb(217, 234, 253);
            btnBrowse2.FlatStyle = FlatStyle.Flat;
            btnBrowse2.Location = new Point(448, 100);
            btnBrowse2.Name = "btnBrowse2";
            btnBrowse2.Size = new Size(75, 23);
            btnBrowse2.TabIndex = 5;
            btnBrowse2.Text = "Browse";
            btnBrowse2.UseVisualStyleBackColor = false;
            btnBrowse2.Click += btnBrowse2_Click_1;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(44, 122);
            label4.Name = "label4";
            label4.Size = new Size(67, 15);
            label4.TabIndex = 1;
            label4.Text = "Backup File";
            // 
            // panel6
            // 
            panel6.BackColor = Color.FromArgb(219, 226, 239);
            panel6.Controls.Add(label2);
            panel6.Dock = DockStyle.Top;
            panel6.Location = new Point(0, 0);
            panel6.Name = "panel6";
            panel6.Size = new Size(569, 39);
            panel6.TabIndex = 0;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F);
            label2.Location = new Point(3, 9);
            label2.Name = "label2";
            label2.Size = new Size(131, 21);
            label2.TabIndex = 0;
            label2.Text = "Database Restore";
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(255, 246, 224);
            panel2.Controls.Add(btnBackup);
            panel2.Controls.Add(btnBrowse);
            panel2.Controls.Add(tbBackup);
            panel2.Controls.Add(label3);
            panel2.Controls.Add(panel4);
            panel2.Location = new Point(422, 103);
            panel2.Name = "panel2";
            panel2.Size = new Size(569, 245);
            panel2.TabIndex = 6;
            // 
            // btnBackup
            // 
            btnBackup.BackColor = Color.FromArgb(217, 234, 253);
            btnBackup.FlatStyle = FlatStyle.Flat;
            btnBackup.Location = new Point(448, 140);
            btnBackup.Name = "btnBackup";
            btnBackup.Size = new Size(75, 23);
            btnBackup.TabIndex = 4;
            btnBackup.Text = "Backup";
            btnBackup.UseVisualStyleBackColor = false;
            btnBackup.Click += btnBackup_Click_1;
            // 
            // btnBrowse
            // 
            btnBrowse.BackColor = Color.FromArgb(217, 234, 253);
            btnBrowse.FlatStyle = FlatStyle.Flat;
            btnBrowse.Location = new Point(448, 97);
            btnBrowse.Name = "btnBrowse";
            btnBrowse.Size = new Size(75, 23);
            btnBrowse.TabIndex = 3;
            btnBrowse.Text = "Browse";
            btnBrowse.UseVisualStyleBackColor = false;
            btnBrowse.Click += btnBrowse_Click_1;
            // 
            // tbBackup
            // 
            tbBackup.Location = new Point(124, 119);
            tbBackup.Name = "tbBackup";
            tbBackup.Size = new Size(306, 23);
            tbBackup.TabIndex = 2;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(44, 122);
            label3.Name = "label3";
            label3.Size = new Size(74, 15);
            label3.TabIndex = 1;
            label3.Text = "File Location";
            // 
            // panel4
            // 
            panel4.BackColor = Color.FromArgb(219, 226, 239);
            panel4.Controls.Add(label1);
            panel4.Dock = DockStyle.Top;
            panel4.Location = new Point(0, 0);
            panel4.Name = "panel4";
            panel4.Size = new Size(569, 39);
            panel4.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F);
            label1.Location = new Point(3, 9);
            label1.Name = "label1";
            label1.Size = new Size(128, 21);
            label1.TabIndex = 0;
            label1.Text = "Database Backup";
            // 
            // panel3
            // 
            panel3.BackColor = Color.FromArgb(255, 246, 224);
            panel3.Controls.Add(label5);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(0, 0);
            panel3.Name = "panel3";
            panel3.Size = new Size(1335, 57);
            panel3.TabIndex = 5;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 24F);
            label5.Location = new Point(3, 5);
            label5.Name = "label5";
            label5.Size = new Size(440, 45);
            label5.TabIndex = 1;
            label5.Text = "Database Backup and Restore";
            // 
            // FormDB_BackupRestore
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(219, 226, 239);
            ClientSize = new Size(1664, 669);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(3, 2, 3, 2);
            Name = "FormDB_BackupRestore";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FormDB_BackupRestore";
            panel1.ResumeLayout(false);
            panel5.ResumeLayout(false);
            panel5.PerformLayout();
            panel6.ResumeLayout(false);
            panel6.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel panel5;
        private Button btnRestore;
        private TextBox tbRestore;
        private Button btnBrowse2;
        private Label label4;
        private Panel panel6;
        private Label label2;
        private Panel panel2;
        private Button btnBackup;
        private Button btnBrowse;
        private TextBox tbBackup;
        private Label label3;
        private Panel panel4;
        private Label label1;
        private Panel panel3;
        private Label label5;
    }
}