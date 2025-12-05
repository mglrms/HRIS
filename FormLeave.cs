using HRISCDBS.Module;
using Microsoft.Data.SqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Windows.Forms;

namespace HRISCDBS
{
    public partial class FormLeave : Form
    {
        private readonly string cs = ConfigurationManager.ConnectionStrings["HRISDBCDBS"].ConnectionString;

        public FormLeave()
        {
            InitializeComponent();
            populateLeave();
            dgvFormat.ApplyDetail(dgvLeaves, 40);
        }

        public void populateLeave()
        {
            string query = @"
SELECT lr.LeaveID,
       e.firstname + ' ' + ISNULL(e.middlename,'') + ' ' + e.lastname + ' ' + ISNULL(e.suffix,'') AS EmployeeName,
       lr.LeaveType,
       lr.StartDate,
       lr.EndDate,
       lr.Status,
       e.SickLeaveHours,
       e.EmergencyLeaveHours,
       e.PaternityLeaveHours,
       e.VacationLeaveHours
FROM LeaveRequest lr
INNER JOIN Employee e ON lr.EmployeeID = e.EmployeeID";

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, con))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgvLeaves.AutoGenerateColumns = true;
                    dgvLeaves.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dgvLeaves.ReadOnly = true;
                    dgvLeaves.AllowUserToAddRows = false;
                    dgvLeaves.RowHeadersVisible = false;
                    dgvLeaves.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    dgvLeaves.DataSource = dt;

                    dgvLeaves.Columns["StartDate"].DefaultCellStyle.Format = "MM/dd/yyyy";
                    dgvLeaves.Columns["EndDate"].DefaultCellStyle.Format = "MM/dd/yyyy";

                    // Adjust headers
                    dgvLeaves.Columns["SickLeaveHours"].HeaderText = "Sick Leave (Days)";
                    dgvLeaves.Columns["EmergencyLeaveHours"].HeaderText = "Emergency Leave (Days)";
                    dgvLeaves.Columns["PaternityLeaveHours"].HeaderText = "Paternity Leave (Days)";
                    dgvLeaves.Columns["VacationLeaveHours"].HeaderText = "Vacation Leave (Days)";
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvLeaves.SelectedRows.Count > 0)
            {
                try
                {
                    int leaveId = Convert.ToInt32(dgvLeaves.SelectedRows[0].Cells["LeaveID"].Value);
                    string employeeName = dgvLeaves.SelectedRows[0].Cells["EmployeeName"]?.Value.ToString() ?? "Unknown";

                    // Open the edit form
                    LeaveEdit editForm = new LeaveEdit(leaveId);

                    if (editForm.ShowDialog() == DialogResult.OK)
                    {
                        populateLeave(); // refresh the grid

                        // 🔹 Log the update action
                        int currentUserId = CurrentUser.UserID;
                        string action = "Update Leave Record";
                        string details = $"LeaveID {leaveId} for {employeeName} was updated by Admin (UserID={currentUserId}).";

                        Logger.LogAction(currentUserId, action, details);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error while opening LeaveEdit form: " + ex.Message,
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a leave record to edit.",
                    "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            var searchText = tbSearch.Text?.Trim();

            if (string.IsNullOrWhiteSpace(searchText))
            {
                LoadAllLeaves();
            }
            else
            {
                LoadLeavesBySearch(searchText);
            }
        }

        private void LoadLeavesBySearch(string searchText)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"
SELECT 
    L.LeaveID, 
    (E.FirstName + ' ' + ISNULL(E.MiddleName,'') + ' ' + E.LastName + ' ' + ISNULL(E.Suffix,'')) AS EmployeeName, 
    L.LeaveType, 
    L.StartDate, 
    L.EndDate, 
    L.Status,
    E.SickLeaveHours,
    E.EmergencyLeaveHours,
    E.PaternityLeaveHours,
    E.VacationLeaveHours
FROM LeaveRequest L
INNER JOIN Employee E ON L.EmployeeID = E.EmployeeID
WHERE CONVERT(VARCHAR, L.LeaveID) LIKE @search
   OR (E.FirstName + ' ' + E.LastName) LIKE @search
ORDER BY L.LeaveID";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@search", $"%{searchText}%");

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvLeaves.DataSource = dt;

                    dgvLeaves.Columns["StartDate"].DefaultCellStyle.Format = "MM/dd/yyyy";
                    dgvLeaves.Columns["EndDate"].DefaultCellStyle.Format = "MM/dd/yyyy";

                    // Adjust headers
                    dgvLeaves.Columns["LeaveID"].HeaderText = "Leave ID";
                    dgvLeaves.Columns["EmployeeName"].HeaderText = "Employee Name";
                    dgvLeaves.Columns["LeaveType"].HeaderText = "Type";
                    dgvLeaves.Columns["StartDate"].HeaderText = "Start Date";
                    dgvLeaves.Columns["EndDate"].HeaderText = "End Date";
                    dgvLeaves.Columns["Status"].HeaderText = "Status";
                    dgvLeaves.Columns["SickLeaveHours"].HeaderText = "Sick Leave (Days)";
                    dgvLeaves.Columns["EmergencyLeaveHours"].HeaderText = "Emergency Leave (Days)";
                    dgvLeaves.Columns["PaternityLeaveHours"].HeaderText = "Paternity Leave (Days)";
                    dgvLeaves.Columns["VacationLeaveHours"].HeaderText = "Vacation Leave (Days)";
                }
            }
        }

        private void LoadAllLeaves()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"
SELECT 
    L.LeaveID, 
    (E.FirstName + ' ' + ISNULL(E.MiddleName,'') + ' ' + E.LastName + ' ' + ISNULL(E.Suffix,'')) AS EmployeeName,
    L.LeaveType, 
    L.StartDate, 
    L.EndDate, 
    L.Status,
    E.SickLeaveHours,
    E.EmergencyLeaveHours,
    E.PaternityLeaveHours,
    E.VacationLeaveHours
FROM LeaveRequest L
INNER JOIN Employee E ON L.EmployeeID = E.EmployeeID
ORDER BY L.LeaveID";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvLeaves.DataSource = dt;

                dgvLeaves.Columns["StartDate"].DefaultCellStyle.Format = "MM/dd/yyyy";
                dgvLeaves.Columns["EndDate"].DefaultCellStyle.Format = "MM/dd/yyyy";

                // Adjust headers
                dgvLeaves.Columns["LeaveID"].HeaderText = "Leave ID";
                dgvLeaves.Columns["EmployeeName"].HeaderText = "Employee Name";
                dgvLeaves.Columns["LeaveType"].HeaderText = "Type";
                dgvLeaves.Columns["StartDate"].HeaderText = "Start Date";
                dgvLeaves.Columns["EndDate"].HeaderText = "End Date";
                dgvLeaves.Columns["Status"].HeaderText = "Status";
                dgvLeaves.Columns["SickLeaveHours"].HeaderText = "Sick Leave (Days)";
                dgvLeaves.Columns["EmergencyLeaveHours"].HeaderText = "Emergency Leave (Days)";
                dgvLeaves.Columns["PaternityLeaveHours"].HeaderText = "Paternity Leave (Days)";
                dgvLeaves.Columns["VacationLeaveHours"].HeaderText = "Vacation Leave (Days)";
            }
        }
    }
}
