using HRISCDBS.Module;
using Microsoft.Data.SqlClient;
using System;
using System.Configuration;
using System.Windows.Forms;

namespace HRISCDBS
{
    public partial class LogInCredentialsEdit : Form
    {
        private int userID;

        public LogInCredentialsEdit(int userId)
        {
            InitializeComponent();
            userID = userId;
            LoadUserData(userId); // ✅ fetch directly from DB using UserID
        }

        private void LoadUserData(int userId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HRISDBCDBS"].ConnectionString))
                {
                    conn.Open();

                    string query = @"
                        SELECT Username, PasswordHash, RoleName, IsActive
                        FROM UserAccount
                        WHERE UserID = @UserID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                tbUsername.Text = reader["Username"].ToString();
                                tbPassword.Text = reader["PasswordHash"].ToString();
                                tbRole.Text = reader["RoleName"].ToString();

                                bool isActive = Convert.ToBoolean(reader["IsActive"]);
                                rbActive.Checked = isActive;
                                rbInactive.Checked = !isActive;
                            }
                            else
                            {
                                MessageBox.Show("User account not found.", "Warning",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                this.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading user data: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HRISDBCDBS"].ConnectionString))
                {
                    conn.Open();

                    string query = @"
                        UPDATE UserAccount
                        SET Username = @Username,
                            PasswordHash = @PasswordHash,
                            RoleName = @RoleName,
                            IsActive = @IsActive
                        WHERE UserID = @UserID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", tbUsername.Text.Trim());
                        cmd.Parameters.AddWithValue("@PasswordHash", tbPassword.Text.Trim());
                        cmd.Parameters.AddWithValue("@RoleName", tbRole.Text.Trim());
                        cmd.Parameters.AddWithValue("@IsActive", rbActive.Checked);
                        cmd.Parameters.AddWithValue("@UserID", userID);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            Logger.LogAction("Update User Credentials",
                                $"UserID {userID} updated: Username = {tbUsername.Text}, Role = {tbRole.Text}, Active = {rbActive.Checked}");

                            MessageBox.Show("User credentials updated successfully.",
                                "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("No rows updated. Check UserID.",
                                "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating user credentials: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}