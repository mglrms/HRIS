using HRISCDBS.Module;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using iTextRectangle = iTextSharp.text.Rectangle;

namespace HRISCDBS
{
    public partial class FormAttendanceMonitoring : Form
    {
        private readonly string _cs =
            ConfigurationManager.ConnectionStrings["HRISDBCDBS"].ConnectionString;

        // Cache holidays per year to avoid repeated API calls
        private static readonly Dictionary<int, List<Holiday>> _holidayCache = new();

        // Master data table used throughout (kept as field as in your original code)
        private DataTable dtMaster;

        private System.Windows.Forms.Timer refreshTimer;

        // Tracks whether the user is actively scrolling (pauses auto-refresh)
        private volatile bool _isUserScrolling = false;

        // Timer to debounce scroll events (resumes auto-refresh after user stops scrolling)
        private System.Windows.Forms.Timer _scrollPauseTimer;


        public FormAttendanceMonitoring()
        {
            InitializeComponent();
            comboBox1.SelectedItem = "None";
            dtp_from.Enabled = true;
            dtp_to.Enabled = true;

            // keep your existing call
            dgvFormat.ApplyDetail(dgv_attendance, 40);
            // initialize scroll debounce timer
            _scrollPauseTimer = new System.Windows.Forms.Timer { Interval = 10000 }; // 800ms after last scroll -> resume
            _scrollPauseTimer.Tick += (s, e) =>
            {
                _isUserScrolling = false;
                _scrollPauseTimer.Stop();
            };

            // attach scroll handler to dgv (so it detects user scrolling)
            dgv_attendance.Scroll += Dgv_attendance_Scroll;
        }
        private void Dgv_attendance_Scroll(object sender, ScrollEventArgs e)
        {
            // user is scrolling — pause auto-refresh briefly
            _isUserScrolling = true;
            _scrollPauseTimer.Stop();
            _scrollPauseTimer.Start();
        }


        private async void FormAttendanceMonitoring_Load(object sender, EventArgs e)
        {
            var today = DateTime.Today;
            dtp_from.Value = today;
            dtp_to.Value = today;

            // wire up pickers (make handlers async)
            dtp_from.ValueChanged += DatePickers_ValueChanged;
            dtp_to.ValueChanged += DatePickers_ValueChanged;

            // initial load (async)
            await LoadRangeAsync(dtp_from.Value, dtp_to.Value);

            // Initialize the timer (auto-refresh)
            refreshTimer = new System.Windows.Forms.Timer
            {
                Interval = 1000 // 1 second
            };
            refreshTimer.Tick += RefreshTimer_Tick;
            refreshTimer.Start();
        }

        private async void RefreshTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                // ✅ Only reload data, don't search here
                // Search is handled by ApplyFilters() in tb_search_TextChanged
                if (_isUserScrolling) return;
                await LoadRangeAsync(dtp_from.Value, dtp_to.Value);
            }
            catch
            {
                // ignore timer errors to avoid repeated message boxes
            }
        }

        // ---------- Loaders (async versions) ----------
        private void NormalizeRange(ref DateTime from, ref DateTime to)
        {
            from = from.Date; to = to.Date;
            if (to < from) (from, to) = (to, from);
        }

        private async Task LoadRangeAsync(DateTime from, DateTime to)
        {
            NormalizeRange(ref from, ref to);

            const string sql = @"
SELECT 
    a.AttendanceID,
    e.EmployeeID,
    (e.LastName + ', ' + e.FirstName + ISNULL(' ' + e.MiddleName,'')) AS EmployeeName,
    e.Position,
    e.SubOrDept,
    a.[Date] AS AttendanceDate,
    CONVERT(varchar(8), a.TimeIn, 108)  AS TimeIn,
    CONVERT(varchar(8), a.TimeOut, 108) AS TimeOut,
    a.Status
FROM Attendance a
JOIN Employee e ON e.EmployeeID = a.EmployeeID
WHERE a.[Date] >= @from AND a.[Date] <= @to
AND e.Position <> 'Admin'
ORDER BY a.[Date], e.LastName, e.FirstName;";

            await BindToGridAsync(sql, cmd =>
            {
                cmd.Parameters.Add("@from", SqlDbType.Date).Value = from;
                cmd.Parameters.Add("@to", SqlDbType.Date).Value = to;
            });
        }

        // Core binding - async and non-blocking
        private async Task BindToGridAsync(string sql, Action<SqlCommand> addParams)
        {
            try
            {
                // Save UI state before fetch
                int? savedFirstDisplayedRow = null;
                try
                {
                    if (dgv_attendance.FirstDisplayedScrollingRowIndex >= 0)
                        savedFirstDisplayedRow = dgv_attendance.FirstDisplayedScrollingRowIndex;
                }
                catch { savedFirstDisplayedRow = null; }

                object savedSelectedIdObj = null;
                // store selected employee id (if any)
                try
                {
                    if (dgv_attendance.CurrentRow != null && dgv_attendance.CurrentRow.Cells["EmployeeID"].Value != null)
                        savedSelectedIdObj = dgv_attendance.CurrentRow.Cells["EmployeeID"].Value;
                }
                catch { savedSelectedIdObj = null; }

                // Fill a temporary DataTable off the UI thread
                DataTable newData = new DataTable();
                using (var conn = new SqlConnection(_cs))
                using (var da = new SqlDataAdapter(sql, conn))
                {
                    addParams?.Invoke(da.SelectCommand);
                    await Task.Run(() => da.Fill(newData));
                }

                // Ensure columns exist and types (same as before)
                // (We do this on newData, then copy into dtMaster)
                if (!newData.Columns.Contains("AttendanceID"))
                    newData.Columns.Add("AttendanceID", typeof(int));
                if (!newData.Columns.Contains("EmployeeID"))
                    newData.Columns.Add("EmployeeID", typeof(int));
                if (!newData.Columns.Contains("EmployeeName"))
                    newData.Columns.Add("EmployeeName", typeof(string));
                if (!newData.Columns.Contains("Position"))
                    newData.Columns.Add("Position", typeof(string));
                if (!newData.Columns.Contains("SubOrDept"))
                    newData.Columns.Add("SubOrDept", typeof(string));
                if (!newData.Columns.Contains("AttendanceDate"))
                    newData.Columns.Add("AttendanceDate", typeof(DateTime));
                if (!newData.Columns.Contains("TimeIn"))
                    newData.Columns.Add("TimeIn", typeof(string));
                if (!newData.Columns.Contains("TimeOut"))
                    newData.Columns.Add("TimeOut", typeof(string));
                if (!newData.Columns.Contains("Status"))
                    newData.Columns.Add("Status", typeof(string));

                // Now update dtMaster in-place to avoid breaking DataSource reference
                if (dtMaster == null)
                {
                    dtMaster = newData.Copy();
                }
                else
                {
                    // Replace rows while keeping the same DataTable instance referenced by the grid
                    dtMaster.BeginLoadData();
                    dtMaster.Clear();
                    foreach (DataRow r in newData.Rows)
                        dtMaster.ImportRow(r);
                    dtMaster.EndLoadData();
                }

                // After dtMaster is updated, continue with leave/holiday logic as before
                var leaveRecords = GetLeaveDates(dtp_from.Value.Date, dtp_to.Value.Date);

                // Mark existing rows with leave status (collect updates first)
                List<(DataRow row, string status)> rowUpdates = new List<(DataRow, string)>();
                foreach (DataRow row in dtMaster.Rows)
                {
                    if (row["EmployeeID"] == DBNull.Value) continue;
                    int empId = Convert.ToInt32(row["EmployeeID"]);
                    DateTime attDate = Convert.ToDateTime(row["AttendanceDate"]).Date;

                    if (leaveRecords.ContainsKey(empId))
                    {
                        var leave = leaveRecords[empId].FirstOrDefault(l => l.Date == attDate);
                        if (leave != null)
                            rowUpdates.Add((row, $"On Leave ({leave.LeaveType})"));
                    }
                }
                foreach (var (row, status) in rowUpdates) row["Status"] = status;

                //Color for the cell
                foreach (DataGridViewRow dgvRow in dgv_attendance.Rows)
                {
                    string status = dgvRow.Cells["Status"].Value?.ToString();

                    if (status == "Absent")
                    {
                        dgvRow.DefaultCellStyle.BackColor = Color.LightCoral;
                        dgvRow.DefaultCellStyle.ForeColor = Color.White;
                    }
                    else if (status.Contains("On Leave"))
                    {
                        dgvRow.DefaultCellStyle.BackColor = Color.LightBlue;
                        dgvRow.DefaultCellStyle.ForeColor = Color.Black;
                    }
                    
                    else if (status.Contains("Late"))
                    {
                        dgvRow.DefaultCellStyle.BackColor = Color.LightYellow;
                        dgvRow.DefaultCellStyle.ForeColor = Color.Black;
                    }
                    else if (status.Contains("Holiday"))
                    {
                        dgvRow.DefaultCellStyle.BackColor = Color.LightGreen;
                        dgvRow.DefaultCellStyle.ForeColor = Color.Black;
                    }
                    else if (status.Contains("Present"))
                    {
                        dgvRow.DefaultCellStyle.BackColor = Color.LightCyan;
                        dgvRow.DefaultCellStyle.ForeColor = Color.Black;
                    }
                    else if (status.Contains("On Time"))
                    {
                        dgvRow.DefaultCellStyle.BackColor = Color.LightCyan;
                        dgvRow.DefaultCellStyle.ForeColor = Color.Black;
                    }
                }

                // Add missing leave-only rows (same as your existing logic)
                List<DataRow> newRowsToAdd = new List<DataRow>();
                foreach (var kvp in leaveRecords)
                {
                    int empId = kvp.Key;
                    string empName = "";
                    string empPos = "";
                    string empDept = "";

                    using (var connection = new SqlConnection(_cs))
                    using (var cmd = new SqlCommand(@"
                SELECT LastName, FirstName, MiddleName, Suffix, Position, SubOrDept 
                FROM Employee 
                WHERE EmployeeID = @EmployeeID", connection))
                    {
                        cmd.Parameters.AddWithValue("@EmployeeID", empId);
                        connection.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                empName = reader["LastName"].ToString() + ", " + reader["FirstName"].ToString();
                                if (!string.IsNullOrEmpty(reader["MiddleName"].ToString()))
                                    empName += " " + reader["MiddleName"].ToString();
                                if (!string.IsNullOrEmpty(reader["Suffix"].ToString()))
                                    empName += " " + reader["Suffix"].ToString();
                                empPos = reader["Position"].ToString();
                                empDept = reader["SubOrDept"].ToString();
                            }
                        }
                    }

                    foreach (var leave in kvp.Value)
                    {
                        bool exists = false;
                        foreach (DataRow existingRow in dtMaster.Rows)
                        {
                            if (existingRow["EmployeeID"] != DBNull.Value &&
                                Convert.ToInt32(existingRow["EmployeeID"]) == empId &&
                                Convert.ToDateTime(existingRow["AttendanceDate"]).Date == leave.Date.Date)
                            {
                                exists = true;
                                break;
                            }
                        }

                        if (!exists)
                        {
                            DataRow newRow = dtMaster.NewRow();
                            newRow["AttendanceID"] = 0;
                            newRow["EmployeeID"] = empId;
                            newRow["EmployeeName"] = empName;
                            newRow["Position"] = empPos;
                            newRow["SubOrDept"] = empDept;
                            newRow["AttendanceDate"] = leave.Date;
                            newRow["TimeIn"] = "-";
                            newRow["TimeOut"] = "-";
                            newRow["Status"] = $"On Leave ({leave.LeaveType})";
                            newRowsToAdd.Add(newRow);
                        }
                    }
                }
                foreach (var nr in newRowsToAdd) dtMaster.Rows.Add(nr);

                // Fetch holidays (same pattern)
                var years = Enumerable.Range(dtp_from.Value.Year, dtp_to.Value.Year - dtp_from.Value.Year + 1).ToList();
                var allHolidays = new List<Holiday>();
                foreach (var y in years)
                {
                    var hlist = await GetHolidaysAsync(y);
                    if (hlist != null && hlist.Count > 0) allHolidays.AddRange(hlist);
                }

                await SaveHolidayAttendanceAsync(allHolidays);

                // Mark holidays
                List<(DataRow row, string status)> holidayUpdates = new List<(DataRow, string)>();
                foreach (DataRow row in dtMaster.Rows)
                {
                    DateTime attDate = Convert.ToDateTime(row["AttendanceDate"]).Date;
                    var holiday = allHolidays.FirstOrDefault(h => h.Date.Date == attDate);
                    if (holiday != null)
                        holidayUpdates.Add((row, $"Holiday ({holiday.LocalName})"));
                }
                foreach (var (row, status) in holidayUpdates)
                {
                    row["Status"] = status;
                    row["TimeIn"] = "-";
                    row["TimeOut"] = "-";
                }

                // Reapply filters and rebind
                ApplyFilters();

                try
                {
                    if (savedFirstDisplayedRow.HasValue && savedFirstDisplayedRow.Value < dgv_attendance.Rows.Count)
                        dgv_attendance.FirstDisplayedScrollingRowIndex = savedFirstDisplayedRow.Value;
                }
                catch {}

                // Restore selection by EmployeeID (best effort)
                try
                {
                    if (savedSelectedIdObj != null)
                    {
                        int savedEmpId;
                        if (int.TryParse(savedSelectedIdObj.ToString(), out savedEmpId))
                        {
                            foreach (DataGridViewRow row in dgv_attendance.Rows)
                            {
                                if (row.Cells["EmployeeID"].Value != null &&
                                    int.TryParse(row.Cells["EmployeeID"].Value.ToString(), out int empId) &&
                                    empId == savedEmpId)
                                {
                                    row.Selected = true;
                                    dgv_attendance.CurrentCell = row.Cells.Cast<DataGridViewCell>().FirstOrDefault(c => c.Visible) ?? row.Cells[0];
                                    break;
                                }
                            }
                        }
                    }
                }
                catch {}
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading attendance: {ex.Message}");
                if (dtMaster == null)
                    dtMaster = new DataTable();
            }
        }


        private void SetHeader(string name, string header)
        {
            if (dgv_attendance.Columns.Contains(name))
                dgv_attendance.Columns[name].HeaderText = header;
        }

        // ---------- Events ----------
        private async void DatePickers_ValueChanged(object sender, EventArgs e)
        {
            // ✅ Always reload full data when date changes
            await LoadRangeAsync(dtp_from.Value, dtp_to.Value);
        }

        private void tb_search_TextChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        // ---------- PDF Export ----------
        private void btnGenerateOutput_Click(object sender, EventArgs e)
        {
            if (dgv_attendance.Rows.Count == 0)
            {
                MessageBox.Show("No data to export.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Logger.LogAction("Attendance PDF Export Failed",
                    $"No data available for export. UserID {CurrentUser.UserID} ({CurrentUser.Username}).");
                return;
            }

            var employeeNames = dgv_attendance.Rows
                .Cast<DataGridViewRow>()
                .Where(r => !r.IsNewRow)
                .Select(r => r.Cells["EmployeeName"].Value?.ToString())
                .Where(name => !string.IsNullOrWhiteSpace(name))
                .Distinct()
                .ToList();

            string selected = comboBox1.SelectedItem?.ToString();
            bool isCutOff = selected == "13th Payroll" || selected == "28th Payroll";

            if (isCutOff && employeeNames.Count > 1)
            {
                using (FolderBrowserDialog fbd = new FolderBrowserDialog())
                {
                    fbd.Description = "Select folder to save PDF files";
                    if (fbd.ShowDialog() != DialogResult.OK)
                        return;

                    foreach (string employeeName in employeeNames)
                    {
                        var employeeRows = dgv_attendance.Rows
                            .Cast<DataGridViewRow>()
                            .Where(r => !r.IsNewRow && r.Cells["EmployeeName"].Value?.ToString() == employeeName)
                            .ToList();

                        string fileName = GeneratePDFForEmployee(employeeName, employeeRows, fbd.SelectedPath);
                        if (string.IsNullOrEmpty(fileName))
                            return;
                    }

                    MessageBox.Show($"PDFs generated successfully for all {employeeNames.Count} employees!",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Logger.LogAction("Attendance PDF Exported",
                        $"Attendance reports exported successfully for {employeeNames.Count} employees by UserID {CurrentUser.UserID} ({CurrentUser.Username}).");
                }
            }
            else
            {
                string employeePart = employeeNames.Count == 1 ? employeeNames[0].Replace(" ", "_") : "MultipleEmployees";
                string dateRangeText = GetDateRangeText();
                string fileDateText = GetFileDateText();
                string fileName = $"AttendanceReport_{employeePart}_{fileDateText}.pdf";

                using (SaveFileDialog sfd = new SaveFileDialog()
                {
                    Filter = "PDF|*.pdf",
                    FileName = fileName
                })
                {
                    if (sfd.ShowDialog() != DialogResult.OK)
                        return;

                    if (GenerateSinglePDF(sfd.FileName, employeeNames))
                    {
                        MessageBox.Show("PDF generated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Logger.LogAction("Attendance PDF Exported",
                            $"Attendance report ({dateRangeText}) for {employeePart} exported successfully to '{sfd.FileName}' by UserID {CurrentUser.UserID} ({CurrentUser.Username}).");
                    }
                }
            }
        }

        private string GeneratePDFForEmployee(string employeeName, List<DataGridViewRow> employeeRows, string folderPath)
        {
            string dateRangeText = GetDateRangeText();
            string fileDateText = GetFileDateText();
            string fileName = $"AttendanceReport_{employeeName.Replace(" ", "_")}_{fileDateText}.pdf";
            string filePath = Path.Combine(folderPath, fileName);

            try
            {
                Document doc = new Document(PageSize.A4, 36, 36, 54, 54);
                PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));
                doc.Open();

                // ---------- HEADER ----------
                iTextSharp.text.Font schoolFont = FontFactory.GetFont("Arial", 16f, iTextSharp.text.Font.BOLD);
                iTextSharp.text.Font subFont = FontFactory.GetFont("Arial", 12f, iTextSharp.text.Font.NORMAL);

                Paragraph schoolName = new Paragraph("[School Name]", schoolFont)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 3f
                };
                doc.Add(schoolName);

                Paragraph subTitle = new Paragraph("Attendance Monitoring", subFont)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 20f
                };
                doc.Add(subTitle);

                // ---------- EMPLOYEE INFO ----------
                iTextSharp.text.Font infoFont = FontFactory.GetFont("Arial", 11f, iTextSharp.text.Font.NORMAL);
                iTextSharp.text.Font labelFont = FontFactory.GetFont("Arial", 11f, iTextSharp.text.Font.BOLD);

                var firstRow = employeeRows.FirstOrDefault();
                string position = firstRow?.Cells["Position"].Value?.ToString();
                string subOrDept = firstRow?.Cells["SubOrDept"].Value?.ToString();

                Paragraph empInfo = new Paragraph();
                empInfo.Add(new Chunk("Employee: ", labelFont));
                empInfo.Add(new Chunk(employeeName + "\n", infoFont));

                if (!string.IsNullOrWhiteSpace(position))
                {
                    empInfo.Add(new Chunk("Position: ", labelFont));
                    empInfo.Add(new Chunk(position + "\n", infoFont));
                }

                if (!string.IsNullOrWhiteSpace(subOrDept))
                {
                    empInfo.Add(new Chunk("Sub/Dept: ", labelFont));
                    empInfo.Add(new Chunk(subOrDept, infoFont));
                }

                empInfo.SpacingAfter = 15f;
                doc.Add(empInfo);

                // ---------- DATE RANGE DISPLAY ----------
                Paragraph dateRangeParagraph = new Paragraph();
                iTextSharp.text.Font dateRangeFont = FontFactory.GetFont("Arial", 10f, iTextSharp.text.Font.BOLD);

                string fromDateStr = dtp_from.Value.ToString("MMMM dd, yyyy");
                string toDateStr = dtp_to.Value.ToString("MMMM dd, yyyy");

                string dateRangeTextDisplay = dtp_from.Value.Date == dtp_to.Value.Date
                    ? $"Date: {fromDateStr}"
                    : $"From: {fromDateStr}   To: {toDateStr}";

                dateRangeParagraph.Add(new Chunk(dateRangeTextDisplay, dateRangeFont));
                dateRangeParagraph.Alignment = Element.ALIGN_CENTER;
                dateRangeParagraph.SpacingAfter = 10f;
                doc.Add(dateRangeParagraph);

                // ---------- TABLE ----------
                AddAttendanceTable(doc, employeeRows);

                // ---------- SIGNATURES ----------
                doc.Add(new Paragraph("\n\n\n\n\n\n"));

                PdfPTable signatureTable = new PdfPTable(2);
                signatureTable.WidthPercentage = 100;
                signatureTable.SetWidths(new float[] { 50, 50 });

                string createdBy = $"{CurrentUser.FirstName} {CurrentUser.LastName}";
                string createdOn = $"Created on: {DateTime.Now:MMMM dd, yyyy hh:mm tt}";

                iTextSharp.text.Font nameFont = FontFactory.GetFont("Arial", 12f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
                iTextSharp.text.Font infoFont2 = FontFactory.GetFont("Arial", 11f, iTextSharp.text.Font.NORMAL);

                PdfPCell preparedByCell = new PdfPCell { Border = iTextRectangle.NO_BORDER };
                preparedByCell.HorizontalAlignment = Element.ALIGN_CENTER;
                preparedByCell.AddElement(new Paragraph(createdBy, nameFont) { Alignment = Element.ALIGN_CENTER });
                preparedByCell.AddElement(new Paragraph("Prepared By", infoFont2) { Alignment = Element.ALIGN_CENTER });
                preparedByCell.AddElement(new Paragraph(createdOn, infoFont2) { Alignment = Element.ALIGN_CENTER });
                signatureTable.AddCell(preparedByCell);

                PdfPCell approvedByCell = new PdfPCell { Border = iTextRectangle.NO_BORDER };
                approvedByCell.HorizontalAlignment = Element.ALIGN_CENTER;
                approvedByCell.AddElement(new Paragraph("HR Manager", nameFont) { Alignment = Element.ALIGN_CENTER });
                approvedByCell.AddElement(new Paragraph("Approved By", infoFont2) { Alignment = Element.ALIGN_CENTER });
                signatureTable.AddCell(approvedByCell);

                doc.Add(signatureTable);
                doc.Close();

                return filePath;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating PDF for {employeeName}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private bool GenerateSinglePDF(string filePath, List<string> employeeNames)
        {
            try
            {
                Document doc = new Document(PageSize.A4, 36, 36, 54, 54);
                PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));
                doc.Open();

                iTextSharp.text.Font schoolFont = FontFactory.GetFont("Arial", 16f, iTextSharp.text.Font.BOLD);
                iTextSharp.text.Font subFont = FontFactory.GetFont("Arial", 12f, iTextSharp.text.Font.NORMAL);

                // Header
                Paragraph schoolName = new Paragraph("[School Name]", schoolFont)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 3f
                };
                doc.Add(schoolName);

                Paragraph subTitle = new Paragraph("Attendance Monitoring", subFont)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 20f
                };
                doc.Add(subTitle);

                // ---------- EMPLOYEE INFO (if single employee) ----------
                if (employeeNames.Count == 1)
                {
                    iTextSharp.text.Font infoFont = FontFactory.GetFont("Arial", 11f, iTextSharp.text.Font.NORMAL);
                    iTextSharp.text.Font labelFont = FontFactory.GetFont("Arial", 11f, iTextSharp.text.Font.BOLD);

                    var firstRow = dgv_attendance.Rows.Cast<DataGridViewRow>()
                        .FirstOrDefault(r => !r.IsNewRow && r.Cells["EmployeeName"].Value?.ToString() == employeeNames[0]);

                    if (firstRow != null)
                    {
                        string position = firstRow.Cells["Position"].Value?.ToString();
                        string subOrDept = firstRow.Cells["SubOrDept"].Value?.ToString();

                        Paragraph empInfo = new Paragraph();
                        empInfo.Add(new Chunk("Employee: ", labelFont));
                        empInfo.Add(new Chunk(employeeNames[0] + "\n", infoFont));

                        if (!string.IsNullOrWhiteSpace(position))
                        {
                            empInfo.Add(new Chunk("Position: ", labelFont));
                            empInfo.Add(new Chunk(position + "\n", infoFont));
                        }

                        if (!string.IsNullOrWhiteSpace(subOrDept))
                        {
                            empInfo.Add(new Chunk("Sub/Dept: ", labelFont));
                            empInfo.Add(new Chunk(subOrDept, infoFont));
                        }

                        empInfo.SpacingAfter = 15f;
                        doc.Add(empInfo);
                    }

                    // ---------- DATE RANGE DISPLAY ----------
                    Paragraph dateRangeParagraph = new Paragraph();
                    iTextSharp.text.Font dateRangeFont = FontFactory.GetFont("Arial", 10f, iTextSharp.text.Font.BOLD);

                    string fromDateStr = dtp_from.Value.ToString("MMMM dd, yyyy");
                    string toDateStr = dtp_to.Value.ToString("MMMM dd, yyyy");

                    string dateRangeTextDisplay = dtp_from.Value.Date == dtp_to.Value.Date
                        ? $"Date: {fromDateStr}"
                        : $"From: {fromDateStr}   To: {toDateStr}";

                    dateRangeParagraph.Add(new Chunk(dateRangeTextDisplay, dateRangeFont));
                    dateRangeParagraph.Alignment = Element.ALIGN_CENTER;
                    dateRangeParagraph.SpacingAfter = 10f;
                    doc.Add(dateRangeParagraph);
                }

                // Add the table with all rows
                AddAttendanceTable(doc, dgv_attendance.Rows.Cast<DataGridViewRow>().Where(r => !r.IsNewRow).ToList());

                // ---------- SIGNATURES (if single employee) ----------
                if (employeeNames.Count == 1)
                {
                    doc.Add(new Paragraph("\n\n\n\n\n\n"));

                    PdfPTable signatureTable = new PdfPTable(2);
                    signatureTable.WidthPercentage = 100;
                    signatureTable.SetWidths(new float[] { 50, 50 });

                    string createdBy = $"{CurrentUser.FirstName} {CurrentUser.LastName}";
                    string createdOn = $"Created on: {DateTime.Now:MMMM dd, yyyy hh:mm tt}";

                    iTextSharp.text.Font nameFont = FontFactory.GetFont("Arial", 12f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
                    iTextSharp.text.Font infoFont2 = FontFactory.GetFont("Arial", 11f, iTextSharp.text.Font.NORMAL);

                    PdfPCell preparedByCell = new PdfPCell { Border = iTextRectangle.NO_BORDER };
                    preparedByCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    preparedByCell.AddElement(new Paragraph(createdBy, nameFont) { Alignment = Element.ALIGN_CENTER });
                    preparedByCell.AddElement(new Paragraph("Prepared By", infoFont2) { Alignment = Element.ALIGN_CENTER });
                    preparedByCell.AddElement(new Paragraph(createdOn, infoFont2) { Alignment = Element.ALIGN_CENTER });
                    signatureTable.AddCell(preparedByCell);

                    PdfPCell approvedByCell = new PdfPCell { Border = iTextRectangle.NO_BORDER };
                    approvedByCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    approvedByCell.AddElement(new Paragraph("HR Manager", nameFont) { Alignment = Element.ALIGN_CENTER });
                    approvedByCell.AddElement(new Paragraph("Approved By", infoFont2) { Alignment = Element.ALIGN_CENTER });
                    signatureTable.AddCell(approvedByCell);

                    doc.Add(signatureTable);
                }

                doc.Close();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating PDF: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void AddAttendanceTable(Document doc, List<DataGridViewRow> rows)
        {
            string[] excludedColumns = { "Position", "SubOrDept", "EmployeeName" };
            var includedColumns = dgv_attendance.Columns
                .Cast<DataGridViewColumn>()
                .Where(c => !excludedColumns.Contains(c.Name))
                .ToList();

            PdfPTable table = new PdfPTable(includedColumns.Count + 1);
            table.WidthPercentage = 100;
            table.SpacingBefore = 10f;

            iTextSharp.text.Font headerFont = FontFactory.GetFont("Arial", 10f, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
            iTextSharp.text.Font cellFont = FontFactory.GetFont("Arial", 9f, iTextSharp.text.Font.NORMAL);

            BaseColor headerBgColor = new BaseColor(25, 55, 109);
            BaseColor evenRowColor = new BaseColor(240, 248, 255);
            BaseColor oddRowColor = BaseColor.WHITE;

            // Headers
            foreach (var column in includedColumns)
            {
                PdfPCell headerCell = new PdfPCell(new Phrase(column.HeaderText, headerFont))
                {
                    BackgroundColor = headerBgColor,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    Padding = 8f,
                    BorderWidth = 0.5f,
                    BorderColor = BaseColor.WHITE
                };
                table.AddCell(headerCell);
            }

            PdfPCell workHoursHeader = new PdfPCell(new Phrase("Work Hours", headerFont))
            {
                BackgroundColor = headerBgColor,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Padding = 8f,
                BorderWidth = 0.5f,
                BorderColor = BaseColor.WHITE
            };
            table.AddCell(workHoursHeader);

            // Rows
            double totalWorkHoursAll = 0;
            int rowIndex = 0;

            foreach (DataGridViewRow row in rows)
            {
                if (row.IsNewRow) continue;

                TimeSpan workHours = CalculateWorkHours(row);
                totalWorkHoursAll += workHours.TotalHours;

                BaseColor rowColor = (rowIndex % 2 == 0) ? evenRowColor : oddRowColor;

                foreach (var column in includedColumns)
                {
                    string displayValue = "";
                    object cellValue = row.Cells[column.Name].Value;

                    if (column.Name == "AttendanceDate" && cellValue != null &&
                        DateTime.TryParse(cellValue.ToString(), out DateTime parsedDate))
                        displayValue = parsedDate.ToString("MM/dd/yyyy");
                    else
                        displayValue = cellValue?.ToString() ?? "";

                    PdfPCell cell = new PdfPCell(new Phrase(displayValue, cellFont))
                    {
                        BackgroundColor = rowColor,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        VerticalAlignment = Element.ALIGN_MIDDLE,
                        Padding = 6f,
                        BorderWidth = 0.5f,
                        BorderColor = new BaseColor(200, 200, 200)
                    };
                    table.AddCell(cell);
                }

                int hours = (int)workHours.TotalHours;
                int minutes = workHours.Minutes;
                string workHoursFormatted = $"{hours} hrs {minutes} mins";

                PdfPCell workHoursCell = new PdfPCell(new Phrase(workHoursFormatted, cellFont))
                {
                    BackgroundColor = rowColor,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    Padding = 6f,
                    BorderWidth = 0.5f,
                    BorderColor = new BaseColor(200, 200, 200)
                };
                table.AddCell(workHoursCell);

                rowIndex++;
            }

            // Summary
            TimeSpan totalWorkTimeSpan = TimeSpan.FromHours(totalWorkHoursAll);
            int totalHours = (int)totalWorkTimeSpan.TotalHours;
            int totalMinutes = totalWorkTimeSpan.Minutes;
            string totalFormatted = $"{totalHours} hrs {totalMinutes} mins";

            PdfPCell summaryLabelCell = new PdfPCell(new Phrase("Total Work Hours",
                FontFactory.GetFont("Arial", 10f, iTextSharp.text.Font.BOLD, BaseColor.WHITE)))
            {
                Colspan = includedColumns.Count,
                HorizontalAlignment = Element.ALIGN_RIGHT,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                BackgroundColor = new BaseColor(70, 130, 180),
                Padding = 8f,
                BorderWidth = 0.5f,
                BorderColor = BaseColor.WHITE
            };
            table.AddCell(summaryLabelCell);

            PdfPCell summaryValueCell = new PdfPCell(new Phrase(totalFormatted,
                FontFactory.GetFont("Arial", 10f, iTextSharp.text.Font.BOLD, BaseColor.WHITE)))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                BackgroundColor = new BaseColor(70, 130, 180),
                Padding = 8f,
                BorderWidth = 0.5f,
                BorderColor = BaseColor.WHITE
            };
            table.AddCell(summaryValueCell);

            doc.Add(table);
        }

        private TimeSpan CalculateWorkHours(DataGridViewRow row)
        {
            TimeSpan workHours = TimeSpan.Zero;
            object timeInObj = row.Cells["TimeIn"].Value;
            object timeOutObj = row.Cells["TimeOut"].Value;
            string status = row.Cells["Status"]?.Value?.ToString()?.Trim().ToLower() ?? "";

            if (status.Contains("on leave") || status.Contains("holiday"))
            {
                workHours = TimeSpan.FromHours(8);
            }
            else if (!string.IsNullOrWhiteSpace(timeInObj?.ToString()) &&
                     !string.IsNullOrWhiteSpace(timeOutObj?.ToString()) &&
                     timeInObj.ToString() != "-" &&
                     timeOutObj.ToString() != "-" &&
                     TimeSpan.TryParse(timeInObj.ToString(), out TimeSpan timeIn) &&
                     TimeSpan.TryParse(timeOutObj.ToString(), out TimeSpan timeOut))
            {
                DateTime date = DateTime.Parse(row.Cells["AttendanceDate"].Value.ToString());
                DateTime shiftStart = date.Date.AddHours(7);
                DateTime shiftEnd = date.Date.AddHours(16);
                DateTime breakStart = date.Date.AddHours(12);
                DateTime breakEnd = date.Date.AddHours(13);

                DateTime actualIn = date.Date.Add(timeIn);
                DateTime actualOut = date.Date.Add(timeOut);

                if (actualIn < shiftStart) actualIn = shiftStart;
                if (actualOut > shiftEnd) actualOut = shiftEnd;

                if (actualOut > actualIn)
                {
                    workHours = actualOut - actualIn;
                    if (actualIn < breakEnd && actualOut > breakStart)
                        workHours -= TimeSpan.FromHours(1);

                    if (workHours < TimeSpan.Zero)
                        workHours = TimeSpan.Zero;
                }
            }

            return workHours;
        }

        private string GetDateRangeText()
        {
            if (comboBox1.SelectedIndex == 1)
                return $"{dtp_from.Value:MMMM} 1–15, {dtp_from.Value:yyyy}";
            else if (comboBox1.SelectedIndex == 0)
                return $"{dtp_from.Value:MMMM} 16–{dtp_to.Value.Day}, {dtp_from.Value:yyyy}";
            else if (dtp_from.Value.Date == dtp_to.Value.Date)
                return dtp_from.Value.ToString("MMMM dd, yyyy");
            else
                return $"{dtp_from.Value:MMMM dd, yyyy} - {dtp_to.Value:MMMM dd, yyyy}";
        }

        private string GetFileDateText()
        {
            if (comboBox1.SelectedIndex == 1)
                return $"{dtp_from.Value:yyyy-MM}_{dtp_from.Value:MMMM}_1-15";
            else if (comboBox1.SelectedIndex == 0)
                return $"{dtp_from.Value:yyyy-MM}_{dtp_from.Value:MMMM}_16-{dtp_to.Value.Day}";
            else if (dtp_from.Value.Date == dtp_to.Value.Date)
                return $"{dtp_from.Value:yyyy-MM-dd}_{dtp_from.Value:MMMM}";
            else
                return $"{dtp_from.Value:yyyy-MM-dd}_{dtp_to.Value:yyyy-MM-dd}_{dtp_from.Value:MMMM}";
        }

        // ======================= LEAVE RECORD CLASS =======================
        public class LeaveRecord
        {
            public DateTime Date { get; set; }
            public string LeaveType { get; set; }
        }

        // ======================= GET LEAVE DATES HELPER =======================
        private Dictionary<int, List<LeaveRecord>> GetLeaveDates(DateTime fromDate, DateTime toDate)
        {
            var leaveDates = new Dictionary<int, List<LeaveRecord>>();

            try
            {
                using (SqlConnection con = new SqlConnection(_cs))
                {
                    string query = @"
                SELECT lr.EmployeeID, lr.StartDate, lr.EndDate, lr.LeaveType
                FROM LeaveRequest lr
                WHERE lr.Status = 'Approved'
                  AND (
                      (lr.StartDate BETWEEN @FromDate AND @ToDate) OR
                      (lr.EndDate BETWEEN @FromDate AND @ToDate) OR
                      (lr.StartDate <= @FromDate AND lr.EndDate >= @ToDate)
                  );";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@FromDate", fromDate);
                        cmd.Parameters.AddWithValue("@ToDate", toDate);

                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int employeeId = Convert.ToInt32(reader["EmployeeID"]);
                                DateTime leaveStart = Convert.ToDateTime(reader["StartDate"]);
                                DateTime leaveEnd = Convert.ToDateTime(reader["EndDate"]);
                                string leaveType = reader["LeaveType"].ToString();

                                if (!leaveDates.ContainsKey(employeeId))
                                    leaveDates[employeeId] = new List<LeaveRecord>();

                                for (DateTime date = leaveStart.Date; date <= leaveEnd.Date; date = date.AddDays(1))
                                {
                                    if (date >= fromDate && date <= toDate)
                                    {
                                        leaveDates[employeeId].Add(new LeaveRecord
                                        {
                                            Date = date,
                                            LeaveType = leaveType
                                        });
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching leave dates: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return leaveDates;
        }

        // ======================= HOLIDAY CLASS & FETCHER =======================
        public class Holiday
        {
            public string LocalName { get; set; }
            public string Name { get; set; }
            public DateTime Date { get; set; }
            public string CountryCode { get; set; }
            public bool Fixed { get; set; }
            public bool Global { get; set; }
            public string[] Counties { get; set; }
            public int? LaunchYear { get; set; }

            // ✅ This is the missing property used in your code
            public List<string> Types { get; set; }
        }

        private async Task<List<Holiday>> GetHolidaysAsync(int year)
        {
            if (_holidayCache.ContainsKey(year))
                return _holidayCache[year];

            try
            {
                using var http = new HttpClient();
                string url = $"https://date.nager.at/api/v3/PublicHolidays/{year}/PH";
                string json = await http.GetStringAsync(url);
                var holidays = JsonConvert.DeserializeObject<List<Holiday>>(json);

                _holidayCache[year] = holidays ?? new List<Holiday>();
                return _holidayCache[year];
            }
            catch (Exception ex)
            {
                // Fail silently and return empty list so UI continues to work
                System.Diagnostics.Debug.WriteLine("Holiday fetch failed: " + ex.Message);
                return new List<Holiday>();
            }
        }

        // ================== OPTIONAL CHECKER ==================
        private bool IsHoliday(DateTime date, List<Holiday> holidays)
        {
            return holidays.Any(h => h.Date.Date == date.Date);
        }
        private async Task SaveHolidayAttendanceAsync(List<Holiday> holidays)
        {
            if (holidays == null || holidays.Count == 0)
                return;

            try
            {
                using (var conn = new SqlConnection(_cs))
                {
                    await conn.OpenAsync();

                    // Fetch EmployeeID and HireDate
                    Dictionary<int, DateTime> employeeHireDates = new Dictionary<int, DateTime>();
                    using (var cmdEmp = new SqlCommand("SELECT EmployeeID, HireDate FROM Employee", conn))
                    using (var reader = await cmdEmp.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            int empId = reader.GetInt32(0);
                            DateTime hireDate = reader.GetDateTime(1);
                            employeeHireDates[empId] = hireDate;
                        }
                    }

                    // Insert holiday attendance for employees, respecting HireDate
                    foreach (var holiday in holidays)
                    {
                        foreach (var emp in employeeHireDates)
                        {
                            int empId = emp.Key;
                            DateTime hireDate = emp.Value;

                            // Skip holidays before hire date
                            if (holiday.Date.Date < hireDate.Date)
                                continue;

                            // Check if holiday record already exists
                            using (var checkCmd = new SqlCommand(@"
                        SELECT COUNT(*) 
                        FROM Attendance 
                        WHERE EmployeeID = @EmployeeID AND [Date] = @Date AND Status LIKE 'Holiday%'", conn))
                            {
                                checkCmd.Parameters.AddWithValue("@EmployeeID", empId);
                                checkCmd.Parameters.AddWithValue("@Date", holiday.Date.Date);

                                int count = (int)await checkCmd.ExecuteScalarAsync();
                                if (count > 0)
                                    continue; // Already exists
                            }

                            // Insert holiday attendance
                            using (var insertCmd = new SqlCommand(@"
                        INSERT INTO Attendance (EmployeeID, [Date], TimeIn, TimeOut, Status)
                        VALUES (@EmployeeID, @Date, NULL, NULL, @Status)", conn))
                            {
                                insertCmd.Parameters.AddWithValue("@EmployeeID", empId);
                                insertCmd.Parameters.AddWithValue("@Date", holiday.Date.Date);
                                insertCmd.Parameters.AddWithValue("@Status", $"Holiday ({holiday.LocalName})");
                                await insertCmd.ExecuteNonQueryAsync();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving holiday records: {ex.Message}",
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Guard clause: if no item is selected, just return
            if (comboBox1.SelectedItem == null)
                return;

            string selected = comboBox1.SelectedItem.ToString();
            DateTime selectedMonth = dtp_from.Value;
            DateTime from, to;

            if (selected == "28th Payroll")
            {
                // 1st to 15th cutoff
                from = new DateTime(selectedMonth.Year, selectedMonth.Month, 1);
                to = new DateTime(selectedMonth.Year, selectedMonth.Month, 15);

                dtp_from.Enabled = false;
                dtp_to.Enabled = false;

                dtp_from.Value = from;
                dtp_to.Value = to;

                _ = LoadRangeAsync(from, to); // fire-and-forget async
            }
            else if (selected == "13th Payroll")
            {
                // 16th to end of month cutoff
                from = new DateTime(selectedMonth.Year, selectedMonth.Month, 16);
                to = new DateTime(selectedMonth.Year, selectedMonth.Month,
                    DateTime.DaysInMonth(selectedMonth.Year, selectedMonth.Month));

                dtp_from.Enabled = false;
                dtp_to.Enabled = false;

                dtp_from.Value = from;
                dtp_to.Value = to;

                _ = LoadRangeAsync(from, to);
            }
            else if (selected == "None")
            {
                // Allow user to manually select range
                dtp_from.Enabled = true;
                dtp_to.Enabled = true;
            }
        }

        private void cbEmployeeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            if (dtMaster == null || dtMaster.Columns.Count == 0) return;

            try
            {
                string searchValue = tb_search.Text.Trim().Replace("'", "''");
                string typeFilter = "";
                string searchFilter = "";

                if (cbEmployeeType.SelectedItem != null && cbEmployeeType.SelectedItem.ToString() != "All")
                    typeFilter = $"Position = '{cbEmployeeType.SelectedItem}'";

                if (!string.IsNullOrEmpty(searchValue))
                {
                    List<string> searchConditions = new List<string>();

                    // ✅ Only add filters for columns that exist
                    if (dtMaster.Columns.Contains("EmployeeID"))
                        searchConditions.Add($"CONVERT(EmployeeID, 'System.String') LIKE '%{searchValue}%'");

                    if (dtMaster.Columns.Contains("EmployeeName"))
                        searchConditions.Add($"EmployeeName LIKE '%{searchValue}%'");

                    if (dtMaster.Columns.Contains("Position"))
                        searchConditions.Add($"Position LIKE '%{searchValue}%'");

                    // ✅ Handle NULL values in SubOrDept - only search non-null values
                    if (dtMaster.Columns.Contains("SubOrDept"))
                        searchConditions.Add($"(SubOrDept IS NOT NULL AND SubOrDept LIKE '%{searchValue}%')");

                    if (dtMaster.Columns.Contains("Status"))
                        searchConditions.Add($"Status LIKE '%{searchValue}%'");

                    if (searchConditions.Count > 0)
                        searchFilter = string.Join(" OR ", searchConditions);
                }

                string combinedFilter = "";
                if (!string.IsNullOrEmpty(typeFilter) && !string.IsNullOrEmpty(searchFilter))
                    combinedFilter = $"{typeFilter} AND ({searchFilter})";
                else if (!string.IsNullOrEmpty(typeFilter))
                    combinedFilter = typeFilter;
                else if (!string.IsNullOrEmpty(searchFilter))
                    combinedFilter = searchFilter;

                DataView dv = dtMaster.DefaultView;
                dv.RowFilter = combinedFilter;

                dgv_attendance.DataSource = dv;

                // Reset column headers
                SetHeader("AttendanceID", "Attendance ID");
                SetHeader("EmployeeID", "Employee ID");
                SetHeader("EmployeeName", "Employee");
                SetHeader("Position", "Position");
                SetHeader("SubOrDept", "Sub/Dept");
                SetHeader("AttendanceDate", "Date");
                SetHeader("TimeIn", "Time In");
                SetHeader("TimeOut", "Time Out");
                SetHeader("Status", "Status");

                if (dgv_attendance.Columns.Contains("AttendanceID"))
                    dgv_attendance.Columns["AttendanceID"].Visible = false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ApplyFilters error: {ex.Message}");
                // If filter fails, show all data without filter
                dgv_attendance.DataSource = dtMaster;
            }
        }
    }
}