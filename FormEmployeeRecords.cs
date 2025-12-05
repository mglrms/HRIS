using HRISCDBS.Module;
using HRISCDBS.Repository;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Data.SqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Windows.Forms;
using iTextRectangle = iTextSharp.text.Rectangle;

namespace HRISCDBS
{
    public partial class FormEmployeeRecords : Form
    {
        private readonly string connString = ConfigurationManager.ConnectionStrings["HRISDBCDBS"].ConnectionString;
        private DataTable dt; // keep master table

        public FormEmployeeRecords()
        {
            InitializeComponent();
            populateEmployee();
            dgvFormat.ApplyDetail(dgvEmployeeRecords, 60);

            // 👇 set default filter to "All"
            cbEmployeeType.SelectedItem = "All";
        }

        public void populateEmployee()
        {
            string query = @"
    SELECT 
        EmployeeID AS ID,
        CONCAT(
            LastName, ', ', FirstName,
            CASE WHEN MiddleName IS NULL OR LTRIM(RTRIM(MiddleName)) = '' 
                 THEN '' ELSE ' ' + LEFT(MiddleName,1) + '.' END,
            CASE WHEN Suffix IS NULL OR LTRIM(RTRIM(Suffix)) = '' 
                 THEN '' ELSE ' ' + Suffix END
        ) AS FullName,
        Position,
        SubOrDept,
        EmploymentStatus,
        HireDate,
        DateOfBirth,
        Gender,
        ContactNumber,
        Email,
        Address,
        RFIDUID,
        Picture,
        PerformanceEvalResult
    FROM Employee
    WHERE Position <> 'Admin'  -- 👈 Exclude Admin
    ORDER BY EmployeeID;";


            using (var con = new SqlConnection(connString))
            using (var adapter = new SqlDataAdapter(query, con))
            {
                dt = new DataTable();
                adapter.Fill(dt);

                dgvEmployeeRecords.Columns.Clear();
                dgvEmployeeRecords.AutoGenerateColumns = true;
                dgvEmployeeRecords.DataSource = dt;

                // force Picture column into an Image column
                if (dgvEmployeeRecords.Columns.Contains("Picture"))
                {
                    int colIndex = dgvEmployeeRecords.Columns["Picture"].Index;
                    dgvEmployeeRecords.Columns.Remove("Picture");

                    var imgCol = new DataGridViewImageColumn
                    {
                        Name = "Picture",
                        HeaderText = "Picture",
                        DataPropertyName = "Picture",
                        ImageLayout = DataGridViewImageCellLayout.Zoom
                    };

                    dgvEmployeeRecords.Columns.Insert(colIndex, imgCol);
                    dgvEmployeeRecords.RowTemplate.Height = 56;
                }

                // formatting headers
                if (dgvEmployeeRecords.Columns.Contains("FullName"))
                    dgvEmployeeRecords.Columns["FullName"].HeaderText = "Full Name";
                if (dgvEmployeeRecords.Columns.Contains("HireDate"))
                    dgvEmployeeRecords.Columns["HireDate"].DefaultCellStyle.Format = "dd/MM/yyyy";
                if (dgvEmployeeRecords.Columns.Contains("DateOfBirth"))
                    dgvEmployeeRecords.Columns["DateOfBirth"].DefaultCellStyle.Format = "dd/MM/yyyy";
                if (dgvEmployeeRecords.Columns.Contains("RFIDUID"))
                    dgvEmployeeRecords.Columns["RFIDUID"].HeaderText = "RFID No.";
                if (dgvEmployeeRecords.Columns.Contains("SubOrDept"))
                    dgvEmployeeRecords.Columns["SubOrDept"].HeaderText = "Subject/Dept"; // Optional: friendly name
                if (dgvEmployeeRecords.Columns.Contains("PerformanceEvalResult"))
                    dgvEmployeeRecords.Columns["PerformanceEvalResult"].HeaderText = "Performance Eval";
                if (dgvEmployeeRecords.Columns.Contains("EmploymentStatus"))
                    dgvEmployeeRecords.Columns["EmploymentStatus"].HeaderText = "Employment Status";
                if (dgvEmployeeRecords.Columns.Contains("ContactNumber"))
                    dgvEmployeeRecords.Columns["ContactNumber"].HeaderText = "Contact Number";
            }
        }

        private void btnAddnew_Click(object sender, EventArgs e)
        {
            using (EmployeeCreateEdit form = new EmployeeCreateEdit(isNew: true))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    populateEmployee();
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            var val = this.dgvEmployeeRecords.SelectedRows[0].Cells[0].Value.ToString();
            if (string.IsNullOrWhiteSpace(val)) return;

            int employeeID = int.Parse(val);

            var repo = new EmployeeRepository();
            var employee = repo.GetEmployee(employeeID);

            if (employee == null) return;

            try
            {
                EmployeeCreateEdit form = new EmployeeCreateEdit();
                form.EditEmployee(employee);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    populateEmployee();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error showing form: " + ex.Message);
            }
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void cbEmployeeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        /// Combines Position filter and Search filter
        private void ApplyFilters()
        {
            if (dt == null) return;
            DataView dv = dt.DefaultView;

            string searchValue = tbSearch.Text.Trim().Replace("'", "''");
            string typeFilter = "";
            string searchFilter = "";

            // Position filter
            if (cbEmployeeType.SelectedItem != null)
            {
                string selected = cbEmployeeType.SelectedItem.ToString();
                if (selected == "Teaching")
                    typeFilter = "Position = 'Teaching'";
                else if (selected == "Non-Teaching")
                    typeFilter = "Position = 'Non-Teaching'";
                else if (selected == "All")
                    typeFilter = "";
                else
                    typeFilter = $"Position = '{selected}'";
            }

            // Always hide Admins 👇
            string hideAdminFilter = "Position <> 'Admin'";

            // Search filter
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchFilter =
                    $"Convert([ID], 'System.String') LIKE '%{searchValue}%' OR " +
                    $"[FullName] LIKE '%{searchValue}%' OR " +
                    $"Position LIKE '%{searchValue}%' OR " +
                    $"SubOrDept LIKE '%{searchValue}%'";
            }

            // Combine filters
            var filters = new[] { typeFilter, searchFilter, hideAdminFilter };
            dv.RowFilter = string.Join(" AND ", filters.Where(f => !string.IsNullOrEmpty(f)));
        }

        private void btnGenerateEmployees_Click(object sender, EventArgs e)
        {
            if (dgvEmployeeRecords.Rows.Count == 0)
            {
                MessageBox.Show("No employee data to export.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Logger.LogAction("Employee PDF Export Failed",
                    $"No data available for export. UserID {CurrentUser.UserID} ({CurrentUser.Username}).");
                return;
            }

            string fileDateText = DateTime.Now.ToString("yyyy-MM-dd");

            // Get the selected employee type from the ComboBox
            string selectedType = cbEmployeeType?.SelectedItem?.ToString() ?? "All";

            // Clean the name to avoid invalid filename characters
            string safeType = string.Join("_", selectedType.Split(Path.GetInvalidFileNameChars()));

            // Include the employee type in the filename
            string fileName = $"EmployeeRecords_{safeType}_{fileDateText}.pdf";


            using (SaveFileDialog sfd = new SaveFileDialog()
            {
                Filter = "PDF|*.pdf",
                FileName = fileName
            })
            {
                if (sfd.ShowDialog() != DialogResult.OK)
                    return;

                try
                {
                    // 1.27 cm = 36 points
                    float marginPoints = 36f;
                    Document doc = new Document(PageSize.A3.Rotate(), marginPoints, marginPoints, marginPoints, marginPoints);
                    PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(sfd.FileName, FileMode.Create));
                    doc.Open();

                    // ---------- HEADER ----------
                    iTextSharp.text.Font schoolFont = FontFactory.GetFont("Arial", 16f, iTextSharp.text.Font.BOLD);
                    iTextSharp.text.Font subFont = FontFactory.GetFont("Arial", 12f, iTextSharp.text.Font.NORMAL);
                    iTextSharp.text.Font categoryFont = FontFactory.GetFont("Arial", 11f, iTextSharp.text.Font.ITALIC, new BaseColor(60, 60, 60));

                    Paragraph schoolName = new Paragraph("[School Name]", schoolFont)
                    {
                        Alignment = Element.ALIGN_CENTER,
                        SpacingAfter = 3f
                    };
                    doc.Add(schoolName);

                    Paragraph subTitle = new Paragraph("Employee Records", subFont)
                    {
                        Alignment = Element.ALIGN_CENTER,
                        SpacingAfter = 5f
                    };
                    doc.Add(subTitle);

                    // ---------- CATEGORY LABEL (from dropdown) ----------
                    string selectedCategory = cbEmployeeType?.SelectedItem?.ToString() ?? "All Employees";

                    Paragraph categoryLabel = new Paragraph($"Department: {selectedCategory}", categoryFont)
                    {
                        Alignment = Element.ALIGN_CENTER,
                        SpacingAfter = 20f
                    };
                    doc.Add(categoryLabel);

                    // ---------- TABLE SETUP ----------
                    var includedColumns = dgvEmployeeRecords.Columns
                        .Cast<DataGridViewColumn>()
                        .Where(c => c.Visible && !c.Name.Equals("Picture", StringComparison.OrdinalIgnoreCase))
                        .ToList();

                    PdfPTable table = new PdfPTable(includedColumns.Count)
                    {
                        WidthPercentage = 100,
                        SpacingBefore = 10f
                    };

                    iTextSharp.text.Font headerFont = FontFactory.GetFont("Arial", 10f, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
                    iTextSharp.text.Font cellFont = FontFactory.GetFont("Arial", 9f, iTextSharp.text.Font.NORMAL);

                    BaseColor headerBgColor = new BaseColor(25, 55, 109);
                    BaseColor evenRowColor = new BaseColor(240, 248, 255);
                    BaseColor oddRowColor = BaseColor.WHITE;

                    // ---------- TABLE HEADERS ----------
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

                    // ---------- TABLE ROWS ----------
                    int rowIndex = 0;

                    foreach (DataGridViewRow row in dgvEmployeeRecords.Rows)
                    {
                        if (row.IsNewRow) continue;
                        BaseColor rowColor = (rowIndex % 2 == 0) ? evenRowColor : oddRowColor;

                        foreach (var column in includedColumns)
                        {
                            object value = row.Cells[column.Name].Value;
                            string displayValue = value != null ? value.ToString() : "";

                            if (value is DateTime dt)
                                displayValue = dt.ToString("MM/dd/yyyy");

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

                        rowIndex++;
                    }

                    doc.Add(table);

                    // ---------- SIGNATURE (Prepared By - LEFT SIDE) ----------
                    doc.Add(new Paragraph("\n\n\n"));

                    iTextSharp.text.Font nameFont = FontFactory.GetFont("Arial", 12f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
                    iTextSharp.text.Font infoFont2 = FontFactory.GetFont("Arial", 11f, iTextSharp.text.Font.NORMAL);

                    string createdBy = $"{CurrentUser.FirstName} {CurrentUser.LastName}";
                    string createdOn = $"Created on: {DateTime.Now:MMMM dd, yyyy hh:mm tt}";

                    Paragraph preparedBy = new Paragraph(createdBy, nameFont)
                    {
                        Alignment = Element.ALIGN_LEFT,
                        SpacingAfter = 3f
                    };
                    doc.Add(preparedBy);

                    Paragraph preparedLabel = new Paragraph("Prepared By", infoFont2)
                    {
                        Alignment = Element.ALIGN_LEFT,
                        SpacingAfter = 2f
                    };
                    doc.Add(preparedLabel);

                    Paragraph dateParagraph = new Paragraph(createdOn, infoFont2)
                    {
                        Alignment = Element.ALIGN_LEFT
                    };
                    doc.Add(dateParagraph);

                    doc.Close();

                    MessageBox.Show("Employee PDF generated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Logger.LogAction("Employee PDF Exported",
                        $"Employee records ({selectedCategory}) exported successfully to '{sfd.FileName}' by UserID {CurrentUser.UserID} ({CurrentUser.Username}).");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error generating Employee PDF: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Logger.LogAction("Employee PDF Export Failed",
                        $"Error while exporting employee records: {ex.Message}. UserID {CurrentUser.UserID} ({CurrentUser.Username}).");
                }
            }
        }
    }
}
