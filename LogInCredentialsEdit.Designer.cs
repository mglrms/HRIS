namespace HRISCDBS
{
    partial class LogInCredentialsEdit
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
            label1 = new Label();
            tbUsername = new TextBox();
            tbPassword = new TextBox();
            label2 = new Label();
            tbRole = new TextBox();
            label3 = new Label();
            label4 = new Label();
            rbActive = new RadioButton();
            rbInactive = new RadioButton();
            label5 = new Label();
            btnCancel = new Button();
            btnSave = new Button();
            groupBox1 = new GroupBox();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.FromArgb(255, 246, 224);
            label1.Font = new Font("Segoe UI", 10.8F);
            label1.Location = new Point(60, 115);
            label1.Name = "label1";
            label1.Size = new Size(82, 20);
            label1.TabIndex = 0;
            label1.Text = "Username: ";
            // 
            // tbUsername
            // 
            tbUsername.Font = new Font("Segoe UI", 10.8F);
            tbUsername.Location = new Point(164, 112);
            tbUsername.Margin = new Padding(3, 2, 3, 2);
            tbUsername.Name = "tbUsername";
            tbUsername.Size = new Size(253, 27);
            tbUsername.TabIndex = 1;
            // 
            // tbPassword
            // 
            tbPassword.Font = new Font("Segoe UI", 10.8F);
            tbPassword.Location = new Point(164, 164);
            tbPassword.Margin = new Padding(3, 2, 3, 2);
            tbPassword.Name = "tbPassword";
            tbPassword.Size = new Size(253, 27);
            tbPassword.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.FromArgb(255, 246, 224);
            label2.Font = new Font("Segoe UI", 10.8F);
            label2.Location = new Point(60, 166);
            label2.Name = "label2";
            label2.Size = new Size(77, 20);
            label2.TabIndex = 2;
            label2.Text = "Password: ";
            // 
            // tbRole
            // 
            tbRole.Enabled = false;
            tbRole.Font = new Font("Segoe UI", 10.8F);
            tbRole.Location = new Point(164, 218);
            tbRole.Margin = new Padding(3, 2, 3, 2);
            tbRole.Name = "tbRole";
            tbRole.Size = new Size(253, 27);
            tbRole.TabIndex = 5;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.FromArgb(255, 246, 224);
            label3.Font = new Font("Segoe UI", 10.8F);
            label3.Location = new Point(60, 220);
            label3.Name = "label3";
            label3.Size = new Size(46, 20);
            label3.TabIndex = 4;
            label3.Text = "Role: ";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.FromArgb(255, 246, 224);
            label4.Font = new Font("Segoe UI", 10.8F);
            label4.Location = new Point(60, 272);
            label4.Name = "label4";
            label4.Size = new Size(56, 20);
            label4.TabIndex = 6;
            label4.Text = "Status: ";
            // 
            // rbActive
            // 
            rbActive.AutoSize = true;
            rbActive.BackColor = Color.FromArgb(255, 246, 224);
            rbActive.Font = new Font("Segoe UI", 10.8F);
            rbActive.Location = new Point(164, 269);
            rbActive.Margin = new Padding(3, 2, 3, 2);
            rbActive.Name = "rbActive";
            rbActive.Size = new Size(68, 24);
            rbActive.TabIndex = 7;
            rbActive.TabStop = true;
            rbActive.Text = "Active";
            rbActive.UseVisualStyleBackColor = false;
            // 
            // rbInactive
            // 
            rbInactive.AutoSize = true;
            rbInactive.BackColor = Color.FromArgb(255, 246, 224);
            rbInactive.Font = new Font("Segoe UI", 10.8F);
            rbInactive.Location = new Point(240, 269);
            rbInactive.Margin = new Padding(3, 2, 3, 2);
            rbInactive.Name = "rbInactive";
            rbInactive.Size = new Size(78, 24);
            rbInactive.TabIndex = 8;
            rbInactive.TabStop = true;
            rbInactive.Text = "Inactive";
            rbInactive.UseVisualStyleBackColor = false;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.ForeColor = Color.White;
            label5.Location = new Point(126, 22);
            label5.Name = "label5";
            label5.Size = new Size(206, 32);
            label5.TabIndex = 9;
            label5.Text = "Log In Credentials";
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.FromArgb(255, 246, 224);
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Segoe UI", 10.2F);
            btnCancel.Location = new Point(38, 341);
            btnCancel.Margin = new Padding(3, 2, 3, 2);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(199, 42);
            btnCancel.TabIndex = 10;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnSave
            // 
            btnSave.BackColor = SystemColors.GradientActiveCaption;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI", 10.2F);
            btnSave.Location = new Point(240, 341);
            btnSave.Margin = new Padding(3, 2, 3, 2);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(199, 42);
            btnSave.TabIndex = 11;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // groupBox1
            // 
            groupBox1.BackColor = Color.FromArgb(255, 246, 224);
            groupBox1.Location = new Point(38, 74);
            groupBox1.Margin = new Padding(3, 2, 3, 2);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(3, 2, 3, 2);
            groupBox1.Size = new Size(401, 252);
            groupBox1.TabIndex = 12;
            groupBox1.TabStop = false;
            // 
            // LogInCredentialsEdit
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(17, 45, 78);
            ClientSize = new Size(479, 404);
            Controls.Add(btnSave);
            Controls.Add(btnCancel);
            Controls.Add(label5);
            Controls.Add(rbInactive);
            Controls.Add(rbActive);
            Controls.Add(label4);
            Controls.Add(tbRole);
            Controls.Add(label3);
            Controls.Add(tbPassword);
            Controls.Add(label2);
            Controls.Add(tbUsername);
            Controls.Add(label1);
            Controls.Add(groupBox1);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(3, 2, 3, 2);
            Name = "LogInCredentialsEdit";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "LogInCredentialsEdit";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox tbUsername;
        private TextBox tbPassword;
        private Label label2;
        private TextBox tbRole;
        private Label label3;
        private Label label4;
        private RadioButton rbActive;
        private RadioButton rbInactive;
        private Label label5;
        private Button btnCancel;
        private Button btnSave;
        private GroupBox groupBox1;
    }
}