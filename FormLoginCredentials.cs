using HRISCDBS.Module;
using Microsoft.Data.SqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Windows.Forms;

namespace HRISCDBS
{
    public partial class FormLoginCredentials : Form
    {
        private readonly string connString = ConfigurationManager.ConnectionStrings["HRISDBCDBS"].ConnectionString;
        private DataTable dt; // keep master table

        public FormLoginCredentials()
        {
            InitializeComponent();
            populateLogInCredentials();

            // Configure refresh timer
            refreshTimer.Interval = 5000; // Refresh every 5 seconds
            refreshTimer.Tick += refreshTimer_Tick;
            refreshTimer.Start();
            dgvFormat.ApplyDetail(dgvEmployeeRecords, 40);
        }

        // -------------------- Populate Grid --------------------
        public void populateLogInCredentials()
        {
            // Join UserAccount with Employee and concatenate RoleName + SubOrDept
            string query = @"
SELECT 
    ua.UserID,
    ua.Username,
    ua.PasswordHash,
    ua.RoleName,
    e.SubOrDept,
    ua.EmployeeID,
    ua.IsActive
FROM UserAccount ua
LEFT JOIN Employee e ON ua.EmployeeID = e.EmployeeID WHERE ua.RoleName <> 'Admin';";

            try
            {
                // Remember currently selected row to restore later
                int? selectedUserID = null;
                if (dgvEmployeeRecords.CurrentRow != null)
                    selectedUserID = Convert.ToInt32(dgvEmployeeRecords.CurrentRow.Cells["UserID"].Value);

                using (SqlConnection con = new SqlConnection(connString))
                {
                    con.Open();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, con))
                    {
                        dt = new DataTable();
                        adapter.Fill(dt);

                        dgvEmployeeRecords.AutoGenerateColumns = true;
                        dgvEmployeeRecords.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        dgvEmployeeRecords.DataSource = dt;

                        // Restore selection if possible
                        if (selectedUserID.HasValue)
                        {
                            foreach (DataGridViewRow row in dgvEmployeeRecords.Rows)
                            {
                                if (Convert.ToInt32(row.Cells["UserID"].Value) == selectedUserID.Value)
                                {
                                    row.Selected = true;
                                    dgvEmployeeRecords.CurrentCell = row.Cells[0];
                                    break;
                                }
                            }
                        }
                        ApplyFilters();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading user credentials: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // -------------------- Edit Button --------------------
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvEmployeeRecords.CurrentRow != null)
            {
                int userId = Convert.ToInt32(dgvEmployeeRecords.CurrentRow.Cells["UserID"].Value);
                string username = dgvEmployeeRecords.CurrentRow.Cells["Username"].Value?.ToString();

                using (LogInCredentialsEdit logInCredentialsEdit = new LogInCredentialsEdit(userId))
                {
                    var result = logInCredentialsEdit.ShowDialog();

                    if (result == DialogResult.OK)
                    {
                        populateLogInCredentials();

                        // ✅ Log successful edit
                        Logger.LogAction("Login Credentials Edited",
                            $"UserID {userId} (Username = {username}) credentials were edited by UserID {CurrentUser.UserID} ({CurrentUser.Username}).");
                    }
                    else
                    {
                        // ✅ Log cancelled edit
                        Logger.LogAction("Login Credentials Edit Cancelled",
                            $"Edit cancelled for UserID {userId} (Username = {username}) by UserID {CurrentUser.UserID} ({CurrentUser.Username}).");
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a row first.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        // -------------------- Timer Tick --------------------
        private void refreshTimer_Tick(object sender, EventArgs e)
        {
            populateLogInCredentials(); // Auto-refresh the grid
        }

        // -------------------- Form Closing --------------------
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (refreshTimer != null)
            {
                refreshTimer.Stop();
                refreshTimer.Dispose();
            }
            base.OnFormClosing(e);
        }

        private void ApplyFilters()
        {
            if (dt == null) return;
            DataView dv = dt.DefaultView;

            string searchValue = tbSearch.Text.Trim().Replace("'", "''");
            string roleFilter = "";
            string searchFilter = "";

            // Always hide Admin
            string hideAdmin = "RoleName <> 'Admin'";

            if (cbEmployeeType.SelectedItem != null)
            {
                string selected = cbEmployeeType.SelectedItem.ToString();
                if (selected == "Active")
                    roleFilter = "IsActive = True";
                else if (selected == "Inactive")
                    roleFilter = "IsActive = False";
                else if (selected != "All")
                    roleFilter = $"RoleName = '{selected}'";
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                searchFilter =
                    $"Convert(UserID, 'System.String') LIKE '%{searchValue}%' OR " +
                    $"Username LIKE '%{searchValue}%' OR " +
                    $"RoleName LIKE '%{searchValue}%' OR " +
                    $"SubOrDept LIKE '%{searchValue}%'";
            }

            // Combine all filters
            string combined = hideAdmin;   // 👈 always applied
            if (!string.IsNullOrEmpty(roleFilter)) combined += " AND " + roleFilter;
            if (!string.IsNullOrEmpty(searchFilter)) combined += " AND (" + searchFilter + ")";

            dv.RowFilter = combined;
        }

        private void cbEmployeeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }
    }
}
