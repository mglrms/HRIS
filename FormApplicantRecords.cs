using HRISCDBS.Module;
using Microsoft.Data.SqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Windows.Forms;

namespace HRISCDBS
{
    public partial class FormApplicantRecords : Form
    {
        private DataTable dt;
        public FormApplicantRecords()
        {
            InitializeComponent();
            dgvFormat.ApplyDetail(dgvApplicantRecords, 40);
        }

        private void FormApplicantRecords_Load(object sender, EventArgs e)
        {
            LoadApplicantRecords();
            Logger.LogAction("Applicant Records Opened",
                $"UserID {CurrentUser.UserID} ({CurrentUser.Username}) opened the applicant records.");
        }

        private int? GetSelectedApplicantId()
        {
            if (dgvApplicantRecords.CurrentRow == null) return null;
            var cell = dgvApplicantRecords.CurrentRow.Cells["ApplicantID"];
            if (cell?.Value == null) return null;
            if (int.TryParse(cell.Value.ToString(), out var id)) return id;
            return null;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            var id = GetSelectedApplicantId();
            if (id == null)
            {
                MessageBox.Show("Please select an applicant to preview.");
                return;
            }

            // Open in ReadOnly Preview Mode
            using (var applicationForm = new ApplicationForm(id.Value, true))
            {
                applicationForm.ShowDialog(this);

                Logger.LogAction("Applicant Previewed",
                    $"ApplicantID {id} was previewed (read-only) by UserID {CurrentUser.UserID} ({CurrentUser.Username}).");
            }
        }

        private void LoadApplicantRecords()
        {
            string connString = ConfigurationManager.ConnectionStrings["HRISDBCDBS"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();

                    // ✅ Only select the needed columns
                    string query = @"
SELECT 
    ApplicantID,
    PositionDesired AS [Position Desired], 
    SubOrDept AS [Subject/Department],
    (LastName + ', ' + FirstName + ' ' + ISNULL(MiddleName, '') + ' ' + ISNULL(Suffix, '')) AS [Full Name],
    PresentAddress AS [Present Address],
    MobileNo AS [Mobile No],
    Email,
    DateOfBirth AS [Date of Birth],
    Sex,
    Status
FROM Applicants
ORDER BY ApplicantID DESC";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    dt = new DataTable();
                    adapter.Fill(dt);

                    dgvApplicantRecords.DataSource = dt;

                    dgvApplicantRecords.AutoResizeColumns();

                    Logger.LogAction("Applicant Records Loaded",
                        $"Applicant records loaded by UserID {CurrentUser.UserID} ({CurrentUser.Username}).");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading applicant records: " + ex.Message);

                    // ✅ Log failure
                    Logger.LogAction("Applicant Records Load Failed",
                        $"Error loading applicant records: {ex.Message}. UserID {CurrentUser.UserID} ({CurrentUser.Username}).");
                }
            }
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyFilters();

            // No need to reassign DataSource; the DataGridView updates automatically
        }


        private void cbEmployeeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            if (dt == null) return;
            DataView dv = dt.DefaultView;

            string searchValue = tbSearch.Text.Trim().Replace("'", "''");
            string typeFilter = "";
            string searchFilter = "";

            // 🔹 Position filter
            if (cbEmployeeType.SelectedItem != null)
            {
                string selected = cbEmployeeType.SelectedItem.ToString();
                if (selected == "Teaching")
                    typeFilter = "[Position Desired] = 'Teaching'";
                else if (selected == "Non-Teaching")
                    typeFilter = "[Position Desired] = 'Non-Teaching'";
                else if (selected == "All")
                    typeFilter = ""; // no filter
                else
                    typeFilter = $"[Position Desired] = '{selected}'";
            }

            // 🔹 Search filter (match aliases!)
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchFilter =
                    $"Convert([ApplicantID], 'System.String') LIKE '%{searchValue}%' OR " +
                    $"[Full Name] LIKE '%{searchValue}%' OR " +
                    $"[Position Desired] LIKE '%{searchValue}%' OR " +
                    $"[Subject/Department] LIKE '%{searchValue}%' OR " +
                    $"Email LIKE '%{searchValue}%'";
            }

            // 🔹 Combine filters
            if (!string.IsNullOrEmpty(typeFilter) && !string.IsNullOrEmpty(searchFilter))
                dv.RowFilter = $"{typeFilter} AND ({searchFilter})";
            else if (!string.IsNullOrEmpty(typeFilter))
                dv.RowFilter = typeFilter;
            else if (!string.IsNullOrEmpty(searchFilter))
                dv.RowFilter = searchFilter;
            else
                dv.RowFilter = ""; // show all
        }

        private void editstatus_btn_Click(object sender, EventArgs e)
        {
            var id = GetSelectedApplicantId();
            if (id == null)
            {
                MessageBox.Show("Please select an applicant to edit the status.");
                return;
            }

            
            string currentStatus = dgvApplicantRecords.CurrentRow.Cells["Status"].Value?.ToString();

            
            using (var statusForm = new Status(id.Value, currentStatus))
            {
                if (statusForm.ShowDialog(this) == DialogResult.OK)
                {
                    LoadApplicantRecords();

                    Logger.LogAction("Applicant Status Updated",
                        $"ApplicantID {id} status updated by UserID {CurrentUser.UserID} ({CurrentUser.Username}).");
                }
                else
                {
                    Logger.LogAction("Applicant Status Update Cancelled",
                        $"ApplicantID {id} status update cancelled by UserID {CurrentUser.UserID} ({CurrentUser.Username}).");
                }
            }
        }

    }
}
