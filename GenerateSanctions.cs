using HRISCDBS.Module;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iText = iTextSharp.text;
using iTextFont = iTextSharp.text.Font;
using iTextRect = iTextSharp.text.Rectangle;

namespace HRISCDBS
{
    public partial class GenerateSanctions : Form
    {
        private readonly string createdBy;
        private readonly string connString = ConfigurationManager.ConnectionStrings["HRISDBCDBS"].ConnectionString;
        private readonly List<string> employeeList;

        public GenerateSanctions(string currentUserName, string sanctionType, List<string> employees)
        {
            InitializeComponent();

            createdBy = currentUserName;
            employeeList = employees;
            dgvFormat.ApplyDetail(dgvEmployeeList, 30);

            lblDays.Visible = false;
            cbDays.Visible = false;

            cbSanctionType.SelectedIndexChanged += cbSanctionType_SelectedIndexChanged;
            cbDays.SelectedIndexChanged += cbDays_SelectedIndexChanged;

            if (cbSanctionType.Items.Contains(sanctionType))
                cbSanctionType.SelectedItem = sanctionType;

            dgvEmployeeList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEmployeeList.MultiSelect = true;
            dgvEmployeeList.AllowUserToAddRows = false;

            PopulateSuspensionDays();
            DisplayEmployees();
        }

        private void PopulateSuspensionDays()
        {
            cbDays.Items.Clear();
            cbDays.Items.Add("3");
            cbDays.Items.Add("5");
            cbDays.Items.Add("10");
            // Don't set a default selection - user must choose
        }

        private void DisplayEmployees()
        {
            dgvEmployeeList.Rows.Clear();

            // Initialize column if not already present
            if (dgvEmployeeList.Columns.Count == 0)
            {
                dgvEmployeeList.Columns.Add("EmployeeName", "Employee Name");
            }

            foreach (var emp in employeeList)
            {
                dgvEmployeeList.Rows.Add(emp);
            }
        }

        private void cbSanctionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbSanctionType.SelectedIndex == 1) // Suspension Letter
            {
                lblDays.Visible = true;
                cbDays.Visible = true;
                cbDays.SelectedIndex = -1; // No default selection
                dgvEmployeeList.Rows.Clear(); // Clear the grid until user selects days
            }
            else
            {
                lblDays.Visible = false;
                cbDays.Visible = false;
                DisplayEmployees(); // Show all employees for other sanction types
            }
        }

        private void cbDays_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbSanctionType.SelectedIndex != 1) return;

            DisplayEligibleEmployeesForSuspension();
        }

        private void GenerateSanctions_Load(object sender, EventArgs e)
        {
            // Refresh the form when it first loads
            if (cbSanctionType.SelectedIndex == 1) // If Suspension Letter is selected
            {
                DisplayEligibleEmployeesForSuspension();
            }
            else if (cbSanctionType.SelectedIndex != -1) // If any other sanction type is selected
            {
                DisplayEmployees();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (cbSanctionType.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a sanction type.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // For Suspension Letter, check if employees are selected
            if (cbSanctionType.SelectedIndex == 1) // Suspension Letter
            {
                if (dgvEmployeeList.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select employee(s) to suspend.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(cbDays.Text))
                {
                    MessageBox.Show("Please select the number of suspension days.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            // Open folder dialog ONCE before the loop
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            folderDialog.Description = "Select folder to save PDF files";

            if (folderDialog.ShowDialog() != DialogResult.OK)
            {
                MessageBox.Show("No folder selected. PDF generation cancelled.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string selectedFolder = folderDialog.SelectedPath;
            string sanctionType = cbSanctionType.SelectedItem.ToString();
            string schoolName = "Caritas Don Bosco School";

            // Determine which employees to process
            List<string> employeesToProcess = new List<string>();

            if (cbSanctionType.SelectedIndex == 1) // Suspension Letter - selected rows from datagridview
            {
                foreach (DataGridViewRow row in dgvEmployeeList.SelectedRows)
                {
                    if (row.Cells["EmployeeName"].Value != null)
                        employeesToProcess.Add(row.Cells["EmployeeName"].Value.ToString());
                }
            }
            else // Written Warning or Termination - all employees from employeeList
            {
                employeesToProcess = employeeList;
            }

            foreach (string employeeName in employeesToProcess)
            {
                int lateCount = 0, absentCount = 0;

                // Fetch attendance summary
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    string query = @"
                SELECT 
                    SUM(CASE WHEN A.Status LIKE 'Late%' THEN 1 ELSE 0 END) AS LateCount,
                    SUM(CASE WHEN A.TimeIn IS NULL THEN 1 ELSE 0 END) AS AbsentCount
                FROM Attendance A
                INNER JOIN Employee E ON E.EmployeeID = A.EmployeeID
                WHERE E.FirstName + ' ' + E.LastName = @FullName
                AND A.Date >= DATEFROMPARTS(YEAR(GETDATE()), 6, 1)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@FullName", employeeName);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                lateCount = reader["LateCount"] == DBNull.Value ? 0 : Convert.ToInt32(reader["LateCount"]);
                                absentCount = reader["AbsentCount"] == DBNull.Value ? 0 : Convert.ToInt32(reader["AbsentCount"]);
                            }
                        }
                    }
                }

                string fileName = $"{employeeName}_{sanctionType.Replace(" ", "_")}.pdf";
                string filePath = Path.Combine(selectedFolder, fileName);

                // Create PDF
                iText.Document doc = new iText.Document(PageSize.A4, 50, 50, 80, 50);
                PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));
                doc.Open();

                iTextFont titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
                iTextFont subHeaderFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14);
                iTextFont normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);
                iTextFont underlineFont = FontFactory.GetFont("Times New Roman", 12f, iTextFont.UNDERLINE);
                iTextFont infoFont = FontFactory.GetFont("Times New Roman", 11f, iTextFont.NORMAL);

                string currentDate = DateTime.Now.ToString("MMMM dd, yyyy");
                string closingPhrase = "";

                // Header
                doc.Add(new iText.Paragraph($"{schoolName}\n", titleFont)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 5f
                });

                doc.Add(new iText.Paragraph($"{sanctionType}\n\n", subHeaderFont)
                {
                    Alignment = Element.ALIGN_CENTER
                });

                iText.Paragraph body = new iText.Paragraph
                {
                    SpacingBefore = 20f
                };

                body.Add(new iText.Paragraph($"Date: {currentDate}\n", normalFont));
                body.Add(new iText.Paragraph($"To: {employeeName}\n\n", normalFont));
                body.Add(new iText.Paragraph($"Dear {employeeName},\n\n", normalFont));

                switch (cbSanctionType.SelectedIndex)
                {
                    case 0: // Written Warning
                        body.Add(new iText.Paragraph(
                            $"This letter serves as a formal written warning concerning your attendance record. " +
                            $"Our records indicate that you have accumulated {lateCount} instances of tardiness and {absentCount} absences during the recent evaluation period. " +
                            $"As a valued member of {schoolName}, it is expected that you maintain consistent punctuality and adherence to the school's attendance policies.\n\n" +
                            $"You are hereby advised to take the necessary steps to improve your attendance moving forward. Continued non-compliance may result in further disciplinary action.\n\n", normalFont));
                        closingPhrase = "Respectfully,";
                        break;

                    case 1: // Suspension Letter
                        body.Add(new iText.Paragraph(
                            $"This letter serves as formal notification that you are being placed under suspension for {cbDays.Text} day(s), " +
                            $"effective immediately, due to repeated violations of the school's attendance policy. Records indicate {lateCount} cases of tardiness and {absentCount} absences.\n\n" +
                            $"Please regard this suspension as an opportunity to reflect on the importance of reliability and punctuality in the workplace. " +
                            $"Failure to demonstrate improvement upon your return may result in further disciplinary measures.\n\n", normalFont));
                        closingPhrase = "Respectfully,";
                        break;

                    case 2: // Termination Letter
                        body.Add(new iText.Paragraph(
                            $"After thorough review and consideration of your attendance record, which shows {lateCount} instances of tardiness and {absentCount} absences, " +
                            $"the administration has decided to terminate your employment with {schoolName}, effective immediately.\n\n" +
                            $"This decision follows multiple warnings and interventions that have unfortunately not resulted in sufficient improvement. " +
                            $"We sincerely thank you for your past contributions and wish you well in your future endeavors.\n\n", normalFont));
                        closingPhrase = "Respectfully,";
                        break;
                }

                body.Add(new iText.Paragraph($"{closingPhrase}\n\n\n\n", normalFont));
                doc.Add(body);

                // Signature
                PdfPTable signatureTable = new PdfPTable(1) { WidthPercentage = 100 };
                string createdOn = $"Created on: {DateTime.Now:MMMM dd, yyyy hh:mm tt}";

                signatureTable.AddCell(new PdfPCell(new Phrase(createdBy, underlineFont))
                {
                    Border = iText.Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_LEFT
                });
                signatureTable.AddCell(new PdfPCell(new Phrase("Prepared by", infoFont))
                {
                    Border = iText.Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_LEFT
                });
                signatureTable.AddCell(new PdfPCell(new Phrase(createdOn, infoFont))
                {
                    Border = iText.Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    PaddingTop = 5f
                });

                doc.Add(signatureTable);
                doc.Close();

                // Insert sanction record into database
                InsertSanctionRecord(employeeName, sanctionType, lateCount, absentCount);

                Logger.LogAction("PDF Generated", $"{sanctionType} issued to {employeeName} by {createdBy}");
            }

            MessageBox.Show("PDFs generated successfully for selected employee(s)!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void InsertSanctionRecord(string employeeName, string sanctionType, int lateCount, int absentCount)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    // Get EmployeeID from employee name
                    string getEmployeeIdQuery = @"
                    SELECT EmployeeID 
                    FROM Employee 
                    WHERE FirstName + ' ' + LastName = @FullName";

                    int employeeId = 0;
                    using (SqlCommand getIdCmd = new SqlCommand(getEmployeeIdQuery, conn))
                    {
                        getIdCmd.Parameters.AddWithValue("@FullName", employeeName);
                        var result = getIdCmd.ExecuteScalar();
                        if (result != null)
                            employeeId = Convert.ToInt32(result);
                    }

                    // Insert sanction record
                    string insertQuery = @"
                    INSERT INTO Sanctions (EmployeeID, EmployeeName, SanctionType, SuspensionDays, LateCount, AbsentCount, DateIssued, IssuedBy)
                    VALUES (@EmployeeID, @EmployeeName, @SanctionType, @SuspensionDays, @LateCount, @AbsentCount, @DateIssued, @IssuedBy)";

                    using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn))
                    {
                        insertCmd.Parameters.AddWithValue("@EmployeeID", employeeId > 0 ? employeeId : (object)DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@EmployeeName", employeeName);
                        insertCmd.Parameters.AddWithValue("@SanctionType", sanctionType);
                        insertCmd.Parameters.AddWithValue("@SuspensionDays", sanctionType == "Suspension Letter" && int.TryParse(cbDays.Text, out int days) ? days : (object)DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@LateCount", lateCount);
                        insertCmd.Parameters.AddWithValue("@AbsentCount", absentCount);
                        insertCmd.Parameters.AddWithValue("@DateIssued", DateTime.Now);
                        insertCmd.Parameters.AddWithValue("@IssuedBy", createdBy);
                        insertCmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inserting sanction record: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayEligibleEmployeesForSuspension()
        {
            dgvEmployeeList.Rows.Clear();

            // Ensure only EmployeeName column exists
            if (dgvEmployeeList.Columns.Count == 0)
            {
                dgvEmployeeList.Columns.Add("EmployeeName", "Employee Name");
            }

            if (cbSanctionType.SelectedIndex != 1) // Only for Suspension Letter
                return;

            // Thresholds for Suspension Days
            Dictionary<int, int> dayThresholds = new Dictionary<int, int>
            {
                { 3, 6 },   // 3 days suspension requires 6 lates OR absences
                { 5, 7 },   // 5 days -> 7
                { 10, 8 }   // 10 days -> 8
            };

            string selectedText = cbDays.SelectedItem?.ToString() ?? "3";

            if (!int.TryParse(selectedText, out int selectedDays))
            {
                return;
            }

            int threshold = dayThresholds.ContainsKey(selectedDays) ? dayThresholds[selectedDays] : 6;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                foreach (var emp in employeeList)
                {
                    int lateCount = 0, absentCount = 0;

                    // Check if employee already has a sanction
                    string sanctionCheckQuery = @"
                    SELECT TOP 1 SuspensionDays
                    FROM Sanctions
                    WHERE EmployeeName = @EmployeeName
                    AND SanctionType = 'Suspension Letter'
                    ORDER BY DateIssued DESC";

                    int existingSuspensionDays = 0;
                    using (SqlCommand sanctionCmd = new SqlCommand(sanctionCheckQuery, conn))
                    {
                        sanctionCmd.Parameters.AddWithValue("@EmployeeName", emp);
                        using (SqlDataReader sanctionReader = sanctionCmd.ExecuteReader())
                        {
                            if (sanctionReader.Read())
                            {
                                existingSuspensionDays = sanctionReader["SuspensionDays"] == DBNull.Value ? 0 : Convert.ToInt32(sanctionReader["SuspensionDays"]);
                            }
                        }
                    }

                    // Skip employee if they already have a sanction with the same or higher days
                    if (existingSuspensionDays >= selectedDays)
                    {
                        continue;
                    }

                    string query = @"
                    SELECT 
                        SUM(CASE WHEN A.Status LIKE 'Late%' THEN 1 ELSE 0 END) AS LateCount,
                        SUM(CASE WHEN A.TimeIn IS NULL THEN 1 ELSE 0 END) AS AbsentCount
                    FROM Attendance A
                    INNER JOIN Employee E ON E.EmployeeID = A.EmployeeID
                    WHERE E.FirstName + ' ' + E.LastName = @FullName
                    AND A.Date >= DATEFROMPARTS(YEAR(GETDATE()), 6, 1)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@FullName", emp);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                lateCount = reader["LateCount"] == DBNull.Value ? 0 : Convert.ToInt32(reader["LateCount"]);
                                absentCount = reader["AbsentCount"] == DBNull.Value ? 0 : Convert.ToInt32(reader["AbsentCount"]);
                            }
                        }
                    }

                    // Only display employees who have exactly the threshold in either lates or absences
                    if (lateCount == threshold || absentCount == threshold)
                    {
                        dgvEmployeeList.Rows.Add(emp);
                    }
                }
            }
        }
    }
}