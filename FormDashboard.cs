using HRISCDBS.EmployeeModule;
using HRISCDBS.Module;
using Microsoft.Data.SqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace HRISCDBS
{
    public partial class FormDashboard : Form
    {
        private FormLeave _formLeave;
        private readonly string connString = ConfigurationManager.ConnectionStrings["HRISDBCDBS"].ConnectionString;

        private int totalEmployees = 0;
        private int presentToday = 0;

        private bool isUserScrolling = false;
        private System.Windows.Forms.Timer scrollTimer;

        public FormDashboard()
        {
            InitializeComponent();
            LoadSchoolYear();
            RefreshDashboard();

            cbSanctions.SelectedIndex = 0;

            cbSanctions.SelectedIndexChanged += cbSanctions_SelectedIndexChanged;
            dgvFormat.ApplyDetail(dgvAttendanceStatus, 30);

            // Auto refresh timer
            refreshTimer.Interval = 3000;
            refreshTimer.Tick += refreshTimer_Tick;
            refreshTimer.Start();

            // Date range handlers
            dtpFrom.ValueChanged += DateRangeChanged;
            dtpTo.ValueChanged += DateRangeChanged;

            // Buttons
            btnToday.Click += btnToday_Click;
            btnDays.Click += btnDays_Click;
            btnMonth.Click += btnMonth_Click;

            // Detect scrolling inside the flagged employees panel
            dgvAttendanceStatus.Scroll += dgvAttendanceStatus_Scroll;

            // Timer to detect when scrolling stops
            scrollTimer = new System.Windows.Forms.Timer();
            scrollTimer.Interval = 1000; // 1 seconds after scroll stops
            scrollTimer.Tick += ScrollTimer_Tick;
        }

        private void dgvAttendanceStatus_Scroll(object sender, ScrollEventArgs e)
        {
            if (!isUserScrolling)
            {
                isUserScrolling = true;
                refreshTimer.Stop(); // ⏸ Pause the dashboard refresh while scrolling
            }

            // Restart the scroll timer every time the user scrolls
            scrollTimer.Stop();
            scrollTimer.Start();
        }

        private void ScrollTimer_Tick(object sender, EventArgs e)
        {
            isUserScrolling = false;
            scrollTimer.Stop();
            refreshTimer.Start(); // ▶ Resume the auto-refresh after scrolling ends
        }

        private void RefreshDashboard()
        {
            LoadTotalEmployees();
            LoadPresentEmployees();
            LoadPendingLeaveCount();
            LoadAttendanceChart();
            LoadAttendanceRangeChart();
        }

        private void LoadTotalEmployees()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM Employee WHERE EmploymentStatus = 'Active'";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        totalEmployees = (int)cmd.ExecuteScalar();
                        lblTotalEmp.Text = totalEmployees.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading total employees: " + ex.Message);
            }
        }

        private void LoadPresentEmployees()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    string query = @"
                        SELECT COUNT(*) 
                        FROM Attendance
                        WHERE CAST([Date] AS DATE) = CAST(GETDATE() AS DATE)
                        AND [Status] = 'Present'";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        presentToday = (int)cmd.ExecuteScalar();
                        lblPresentEmp.Text = presentToday.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading present employees: " + ex.Message);
            }
        }

        private void LoadPendingLeaveCount()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM LeaveRequest WHERE Status = 'Pending'";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        int pendingCount = (int)cmd.ExecuteScalar();
                        lblLeaveNotif.Text = pendingCount.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading pending leave count: " + ex.Message);
            }
        }

        private void LoadAttendanceChart()
        {
            try
            {
                int absentToday = totalEmployees - presentToday;

                chartAttendance.Series.Clear();
                Series series = chartAttendance.Series.Add("Attendance");

                // 🔹 Change to Donut chart
                series.ChartType = SeriesChartType.Doughnut;

                // Add values
                series.Points.AddXY("Present", presentToday);
                series.Points.AddXY("Absent", absentToday);

                series.IsValueShownAsLabel = true;

                // 🔹 Optional: Customize colors
                series.Points[0].Color = Color.FromArgb(100, 200, 100); // Green for present
                series.Points[1].Color = Color.FromArgb(220, 80, 80);   // Red for absent

                // 🔹 Center text style
                series["PieLabelStyle"] = "Outside";
                series["DoughnutRadius"] = "60"; // controls inner hole size
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading donut chart: " + ex.Message);
            }
        }

        private void LoadAttendanceRangeChart()
        {
            try
            {
                chartRecords.Series.Clear();
                chartRecords.ChartAreas[0].AxisX.LabelStyle.Angle = -45;
                chartRecords.ChartAreas[0].AxisX.LabelStyle.Format = "MM-dd";
                chartRecords.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Days;
                chartRecords.ChartAreas[0].AxisX.Interval = 1;

                Series series = chartRecords.Series.Add(" ");

                // 🔹 Smooth area chart
                series.ChartType = SeriesChartType.SplineArea;
                series.BorderWidth = 3;
                series.Color = Color.FromArgb(180, Color.DodgerBlue);
                series.BorderColor = Color.Blue;

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    string query = @"
                SELECT CAST([Date] AS DATE) AS Day, COUNT(*) AS PresentCount
                FROM Attendance
                WHERE [Status] = 'Present'
                AND CAST([Date] AS DATE) BETWEEN @From AND @To
                GROUP BY CAST([Date] AS DATE)
                ORDER BY Day";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@From", dtpFrom.Value.Date);
                        cmd.Parameters.AddWithValue("@To", dtpTo.Value.Date);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DateTime day = reader.GetDateTime(0);
                                int count = reader.GetInt32(1);

                                // 🔹 Use real DateTime, not string
                                series.Points.AddXY(day, count);
                            }
                        }
                    }
                }

                chartRecords.Titles.Clear();
                chartRecords.Titles.Add($"Attendance from {dtpFrom.Value.ToShortDateString()} to {dtpTo.Value.ToShortDateString()}");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading attendance range chart: " + ex.Message);
            }
        }

        private void refreshTimer_Tick(object sender, EventArgs e)
        {
            RefreshDashboard();
            LoadFlaggedEmployees();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            refreshTimer?.Stop();
            base.OnFormClosing(e);
        }

        private void btnToday_Click(object sender, EventArgs e)
        {
            dtpFrom.Value = DateTime.Today;
            dtpTo.Value = DateTime.Today;
        }

        private void btnDays_Click(object sender, EventArgs e)
        {
            dtpFrom.Value = DateTime.Today.AddDays(-7);
            dtpTo.Value = DateTime.Today;
        }

        private void btnMonth_Click(object sender, EventArgs e)
        {
            dtpFrom.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            dtpTo.Value = DateTime.Today;
        }

        // Shared handler for both date pickers
        private void DateRangeChanged(object sender, EventArgs e)
        {
            LoadAttendanceRangeChart();
        }

        private void cbSanctions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbSanctions.SelectedItem == null)
                return;

            string selectedSanction = cbSanctions.SelectedItem.ToString();

            if (selectedSanction == "All")
            {
                // ▶ Resume auto-refresh and show all employees
                refreshTimer.Start();
                LoadFlaggedEmployees();

                // ❌ Disable generate button
                btnGenerate.Enabled = false;
            }
            else
            {
                // ⏸ Pause auto-refresh while viewing filtered results
                refreshTimer.Stop();
                LoadFlaggedEmployeesBySanction(selectedSanction);

                // ✅ Enable generate button
                btnGenerate.Enabled = true;
            }
        }

        private void LoadFlaggedEmployees()
        {
            try
            {
                dgvAttendanceStatus.DataSource = null; // Clear existing data
                dgvAttendanceStatus.Rows.Clear(); // Clear rows

                // Initialize columns if not already present
                if (dgvAttendanceStatus.Columns.Count == 0)
                {
                    dgvAttendanceStatus.Columns.Add("FullName", "Full Name");
                    dgvAttendanceStatus.Columns.Add("LateCount", "Late");
                    dgvAttendanceStatus.Columns.Add("AbsentCount", "Absent");
                }

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    string query = @"
        SELECT E.EmployeeID, E.FirstName + ' ' + E.LastName AS FullName,
       SUM(CASE WHEN A.Status LIKE 'Late%' THEN 1 ELSE 0 END) AS LateCount,
       SUM(CASE WHEN A.TimeIn IS NULL THEN 1 ELSE 0 END) AS AbsentCount
FROM Employee E
LEFT JOIN Attendance A ON E.EmployeeID = A.EmployeeID
WHERE A.Date >= DATEFROMPARTS(YEAR(GETDATE()), 6, 1) -- school year starts June 1
AND (E.Position <> 'Admin' OR E.Position IS NULL) -- exclude Admin
GROUP BY E.EmployeeID, E.FirstName, E.LastName
HAVING SUM(CASE WHEN A.Status LIKE 'Late%' THEN 1 ELSE 0 END) >= 3
    OR SUM(CASE WHEN A.TimeIn IS NULL THEN 1 ELSE 0 END) >= 3
ORDER BY FullName";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string fullName = reader["FullName"].ToString();
                            int lateCount = Convert.ToInt32(reader["LateCount"]);
                            int absentCount = Convert.ToInt32(reader["AbsentCount"]);

                            dgvAttendanceStatus.Rows.Add(fullName, lateCount, absentCount);
                        }
                    }
                }

                // Display message if no flagged employees
                if (dgvAttendanceStatus.Rows.Count == 0)
                {
                    MessageBox.Show("✅ No employees with excessive lates/absences.", "Attendance Status", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading flagged employees: " + ex.Message);
            }
        }

        private void LoadFlaggedEmployeesBySanction(string sanctionType)
        {
            try
            {
                dgvAttendanceStatus.DataSource = null;
                dgvAttendanceStatus.Rows.Clear();

                if (dgvAttendanceStatus.Columns.Count == 0)
                {
                    dgvAttendanceStatus.Columns.Add("FullName", "Full Name");
                    dgvAttendanceStatus.Columns.Add("LateCount", "Late");
                    dgvAttendanceStatus.Columns.Add("AbsentCount", "Absent");
                }

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    string query = @"
                SELECT 
                    E.EmployeeID, 
                    E.FirstName + ' ' + E.LastName AS FullName,
                    SUM(CASE WHEN A.Status LIKE 'Late%' THEN 1 ELSE 0 END) AS LateCount,
                    SUM(CASE WHEN A.TimeIn IS NULL THEN 1 ELSE 0 END) AS AbsentCount
                FROM Employee E
                LEFT JOIN Attendance A ON E.EmployeeID = A.EmployeeID
                WHERE A.Date >= DATEFROMPARTS(YEAR(GETDATE()), 6, 1)
                  AND (E.Position <> 'Admin' OR E.Position IS NULL)
                GROUP BY E.EmployeeID, E.FirstName, E.LastName
                ORDER BY FullName";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string fullName = reader["FullName"].ToString();
                            int lateCount = Convert.ToInt32(reader["LateCount"]);
                            int absentCount = Convert.ToInt32(reader["AbsentCount"]);

                            // Determine which sanction this employee qualifies for
                            string employeeSanction = GetSanctionType(lateCount, absentCount);

                            // Only show employees that match the selected sanction
                            if (employeeSanction == sanctionType)
                            {
                                // Check if employee should be filtered based on sanction type
                                if (ShouldFilterEmployee(conn, fullName, sanctionType, lateCount, absentCount))
                                {
                                    continue; // Skip this employee
                                }

                                dgvAttendanceStatus.Rows.Add(fullName, lateCount, absentCount);
                            }
                        }
                    }
                }

                if (dgvAttendanceStatus.Rows.Count == 0)
                {
                    MessageBox.Show($"✅ No employees qualify for {sanctionType}.", "Attendance Status", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading flagged employees: " + ex.Message);
            }
        }

        private bool ShouldFilterEmployee(SqlConnection conn, string employeeName, string sanctionType, int currentLateCount, int currentAbsentCount)
        {
            try
            {
                // For Suspension Letter, check if employee needs progressive discipline
                if (sanctionType == "Suspension Letter")
                {
                    return HasExistingSuspension(employeeName, currentLateCount, currentAbsentCount);
                }

                // For Written Warning and Dismissal Letter, check if already issued (can only be given once)
                using (SqlConnection checkConn = new SqlConnection(connString))
                {
                    checkConn.Open();
                    string query = @"
                    SELECT COUNT(*)
                    FROM Sanctions
                    WHERE EmployeeName = @EmployeeName
                    AND SanctionType = @SanctionType";

                    using (SqlCommand cmd = new SqlCommand(query, checkConn))
                    {
                        cmd.Parameters.AddWithValue("@EmployeeName", employeeName);
                        cmd.Parameters.AddWithValue("@SanctionType", sanctionType);
                        int count = (int)cmd.ExecuteScalar();

                        // If sanction has already been issued, filter out the employee
                        if (count > 0)
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking sanction history: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return false;
        }

        private bool HasExistingSuspension(string employeeName, int currentLateCount, int currentAbsentCount)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    string query = @"
                    SELECT TOP 1 SuspensionDays
                    FROM Sanctions
                    WHERE EmployeeName = @EmployeeName
                    AND SanctionType = 'Suspension Letter'
                    ORDER BY DateIssued DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@EmployeeName", employeeName);
                        var result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            int existingSuspensionDays = Convert.ToInt32(result);

                            // Thresholds for each suspension level
                            int currentThreshold = GetThresholdForSuspensionDays(existingSuspensionDays);
                            int maxAttendanceIssue = Math.Max(currentLateCount, currentAbsentCount);

                            // If employee's attendance hasn't improved beyond the next level, they should be filtered
                            // Example: If they had 3-day suspension (threshold 6), check if they now have 7+ for 5-day
                            if (existingSuspensionDays == 3 && maxAttendanceIssue < 7)
                            {
                                return true; // Still at 6, don't show in list
                            }
                            else if (existingSuspensionDays == 5 && maxAttendanceIssue < 8)
                            {
                                return true; // Still at 7, don't show in list
                            }
                            else if (existingSuspensionDays == 10)
                            {
                                return true; // Already at max suspension
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking existing suspension: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return false;
        }

        private int GetThresholdForSuspensionDays(int days)
        {
            return days switch
            {
                3 => 6,
                5 => 7,
                10 => 8,
                _ => 6
            };
        }

        private string GetSanctionType(int lateCount, int absentCount)
        {
            // Pick the highest applicable sanction based on either late OR absence count
            int maxValue = Math.Max(lateCount, absentCount);

            if (maxValue >= 9)
                return "Dismissal Letter";
            else if (maxValue >= 6)
                return "Suspension Letter";
            else if (maxValue >= 3)
                return "Written Warning Letter";
            else
                return null;
        }

        private void lblLeaveNotif_Click(object sender, EventArgs e)
        {
            _formLeave = FormManager.OpenForm(_formLeave, AdminForm.Instance);
        }

        private void pbLeave_Click(object sender, EventArgs e)
        {
            _formLeave = FormManager.OpenForm(_formLeave, AdminForm.Instance);
        }

        private void LoadSchoolYear()
        {
            int year = DateTime.Now.Year;

            // If before June, still part of previous school year
            if (DateTime.Now.Month < 6)
                lblSY.Text = $"{year - 1} - {year}";
            else
                lblSY.Text = $"{year} - {year + 1}";
        }

        private void todayTimer_Tick(object sender, EventArgs e)
        {
            lblToday.Text = DateTime.Now.ToString("MMMM dd, yyyy | hh:mm:ss tt");
            LoadSchoolYear();
        }

        private void FormDashboard_Load(object sender, EventArgs e)
        {
            lblToday.Text = DateTime.Now.ToString("MMMM dd, yyyy | hh:mm:ss tt");
            todayTimer.Interval = 1000;
            todayTimer.Start();

            // 🔹 Automatically set both date pickers to today's date
            dtpFrom.Value = DateTime.Today;
            dtpTo.Value = DateTime.Today;

            // 🔹 Load attendance data for today's range
            LoadAttendanceRangeChart();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (cbSanctions.SelectedItem == null)
            {
                MessageBox.Show("Please select a sanction type first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvAttendanceStatus.Rows.Count == 0)
            {
                MessageBox.Show("No employee records found for the selected sanction type.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // ✅ Collect all employee names from DataGridView
            List<string> employeeList = new List<string>();
            foreach (DataGridViewRow row in dgvAttendanceStatus.Rows)
            {
                if (row.Cells["FullName"].Value != null) // make sure this matches your actual column name
                {
                    employeeList.Add(row.Cells["FullName"].Value.ToString());
                }
            }

            string selectedSanction = cbSanctions.SelectedItem.ToString();

            // ✅ Pass all employees to GenerateSanctions
            GenerateSanctions gs = new GenerateSanctions($"{CurrentUser.FirstName} {CurrentUser.LastName}", selectedSanction, employeeList);
            gs.ShowDialog();

            // 🔹 After PDF generation, refresh the dashboard to show updated sanction records
            RefreshDashboard();
            LoadFlaggedEmployeesBySanction(selectedSanction);
        }
    }
}