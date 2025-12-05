using HRISCDBS.EmployeeModule;
using HRISCDBS.Module;
using Microsoft.Data.SqlClient;
using System;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;

namespace HRISCDBS
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private readonly string connstring = ConfigurationManager.ConnectionStrings["HRISDBCDBS"].ConnectionString;

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = tbUsername.Text.Trim();
            string password = tbPass.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connstring))
                {
                    connection.Open();

                    // Fetch user + employee info
                    string sql = @"
    SELECT ua.UserID, ua.Username, ua.RoleName, e.EmployeeID, e.FirstName, e.LastName, e.MiddleName, e.Suffix
    FROM UserAccount ua
    INNER JOIN Employee e ON ua.EmployeeID = e.EmployeeID
    WHERE ua.Username = @Username COLLATE SQL_Latin1_General_CP1_CS_AS
      AND ua.PasswordHash = @Password COLLATE SQL_Latin1_General_CP1_CS_AS
      AND ua.IsActive = 1";

                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@Password", password);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // ✅ Populate CurrentUser with valid data
                                CurrentUser.UserID = reader.GetInt32(reader.GetOrdinal("UserID"));
                                CurrentUser.EmployeeID = reader.GetInt32(reader.GetOrdinal("EmployeeID"));
                                CurrentUser.Username = reader.GetString(reader.GetOrdinal("Username"));
                                CurrentUser.Role = reader.GetString(reader.GetOrdinal("RoleName"));
                                CurrentUser.FirstName = reader.GetString(reader.GetOrdinal("FirstName"));
                                CurrentUser.LastName = reader.GetString(reader.GetOrdinal("LastName"));
                                CurrentUser.MiddleName = reader["MiddleName"] as string ?? "";
                                CurrentUser.Suffix = reader["Suffix"] as string ?? "";

                                // ✅ Log successful login
                                Logger.LogAction("Login", $"User {CurrentUser.Username} logged in.");

                                // Open correct form
                                if (CurrentUser.Role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                                {
                                    AdminForm adminForm = new AdminForm();
                                    adminForm.Show();
                                }
                                else
                                {
                                    EmployeeForm employeeForm = new EmployeeForm();
                                    employeeForm.Show();
                                }

                                this.Hide();
                            }
                            else
                            {
                                // ✅ Log failed login attempt anonymously
                                Logger.LogAction("Login Failed", $"Invalid login attempt with username '{username}'");

                                MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting to database: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            using (SolidBrush shadowBrush = new SolidBrush(Color.FromArgb(100, 242, 242, 242)))
            {
                int shadowSize = 0;
                e.Graphics.FillRectangle(
                    shadowBrush,
                    shadowSize,
                    shadowSize,
                    panel1.Width - shadowSize,
                    panel1.Height - shadowSize
                );
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void tbUsername_TextChanged(object sender, EventArgs e) { }
        private void tbPass_TextChanged(object sender, EventArgs e) { }

        private void btnApply_Click(object sender, EventArgs e)
        {
            using (ApplicationForm form = new ApplicationForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    // Determine user info for logging
                    string userInfo;
                    if (CurrentUser.UserID > 0)
                    {
                        userInfo = $"UserID {CurrentUser.UserID} ({CurrentUser.Username})";
                    }
                    else
                    {
                        userInfo = "Anonymous";
                    }

                    // Log the application submission
                    Logger.LogAction("Applicant Applied", $"New applicant submitted an application by {userInfo}");

                    MessageBox.Show("Your application has been submitted successfully.", "Application Submitted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Optional: log cancellation
                    string userInfo = CurrentUser.UserID > 0 ? $"UserID {CurrentUser.UserID} ({CurrentUser.Username})" : "Anonymous";
                    Logger.LogAction("Applicant Application Cancelled", $"Application cancelled by {userInfo}");
                }
            }
        }


        private void tbPass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin.PerformClick();
            }
        }
    }
}
