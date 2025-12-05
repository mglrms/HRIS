using HRISCDBS.Module;
using HRISCDBS.Repository;
using Microsoft.Data.SqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace HRISCDBS
{
    public partial class ApplicationForm : Form
    {
        private readonly int? _applicantId; // null = new; has value = edit
        private CheckBox[] statusSequence;
        private bool _isLoading = true;
        private bool _isReadOnly = false;
        public ApplicationForm()
        {
            InitializeComponent();
        }
        public ApplicationForm(int applicantId, bool readOnly)
        {
            InitializeComponent();
            _applicantId = applicantId;
            _isReadOnly = readOnly;

            LoadApplicant(_applicantId.Value);

            if (_isReadOnly)
                MakeFormReadOnly();
        }

        private void ApplicationForm_Load(object sender, EventArgs e)
        {
            _isLoading = true;

            // Initialize checkbox sequence
            statusSequence = new CheckBox[]
            {
        chkApplied,
        chkReview,
        chkInterview,
        chkExam,
        chkShortlist,
        chkHired
            };

            ResetStatusGroup(); // hide all checkboxes by default

            bool isAdmin = CurrentUser.Role?.Trim().Equals("Admin", StringComparison.OrdinalIgnoreCase) == true;

            // Status group visible only for admin
            gbStatus.Visible = isAdmin;

            if (isAdmin)
            {
                if (!_applicantId.HasValue)
                {
                    // New applicant: only first checkbox visible/enabled
                    statusSequence[0].Visible = true;
                    statusSequence[0].Enabled = true;
                }
                else
                {
                    // Editing existing applicant
                    string savedStatus = GetSavedStatus(_applicantId.Value);
                    RestoreCheckboxSequence(savedStatus);
                }
            }

            // Subscribe to position desired change
            cbPositionDesired.SelectedIndexChanged += cbPositionDesired_SelectedIndexChanged;

            // Load applicant if editing
            if (_applicantId.HasValue)
                LoadApplicant(_applicantId.Value);

            // For non-admins, sub/department should still appear if teaching/non-teaching is selected
            if (!_applicantId.HasValue || cbPositionDesired.SelectedItem == null)
            {
                HideSubDepartmentControls(); // hide initially
            }
            else
            {
                // Trigger sub/department display if already selected
                string selected = cbPositionDesired.SelectedItem?.ToString() ?? "";
                if (selected.Equals("Teaching", StringComparison.OrdinalIgnoreCase) ||
                    selected.Equals("Non-Teaching", StringComparison.OrdinalIgnoreCase))
                {
                    cbPositionDesired_SelectedIndexChanged(cbPositionDesired, EventArgs.Empty);
                }
                else
                {
                    HideSubDepartmentControls();
                }
            }

            _isLoading = false;
        }

        private void cbPositionDesired_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isLoading) return; // Prevent firing during initial load

            string selected = cbPositionDesired.SelectedItem?.ToString() ?? "";

            if (selected.Equals("Teaching", StringComparison.OrdinalIgnoreCase))
            {
                ShowSubDepartmentControls("Field of specialization:", new string[]
                {
            "Mathematics","English","Science","Filipino","Araling Panlipunan","MAPEH","TLE","Values Education"
                });
            }
            else if (selected.Equals("Non-Teaching", StringComparison.OrdinalIgnoreCase))
            {
                ShowSubDepartmentControls("Department:", new string[]
                {
            "Human Resources","Finance","Information Technology","Registrar","Library","Guidance Office","Clinic"
                });
            }
            else
            {
                HideSubDepartmentControls();
            }
        }

        private void HideSubDepartmentControls()
        {
            cbSub.Visible = false;
            lblSub.Visible = false;
            label41.Visible = false;
            cbSub.Items.Clear();
            cbSub.SelectedIndex = -1;
        }

        private void ShowSubDepartmentControls(string labelText, string[] items)
        {
            lblSub.Text = labelText;
            cbSub.Items.Clear();
            cbSub.Items.AddRange(items);
            cbSub.SelectedIndex = -1;

            cbSub.Visible = true;
            lblSub.Visible = true;
            label41.Visible = true;
        }

        public ApplicationForm(int applicantId) : this()
        {
            _applicantId = applicantId;
        }

        private void RestoreCheckboxSequence(string savedStatus)
        {
            var checkboxes = gbStatus.Controls.OfType<CheckBox>().OrderBy(chk => chk.TabIndex).ToArray();
            int lastCheckedIndex = Array.FindIndex(checkboxes, chk => chk.Text.Equals(savedStatus, StringComparison.OrdinalIgnoreCase));

            for (int i = 0; i < checkboxes.Length; i++)
            {
                if (i < lastCheckedIndex)
                {
                    checkboxes[i].Visible = true;
                    checkboxes[i].Checked = true;
                    checkboxes[i].Enabled = false; // readonly
                }
                else if (i == lastCheckedIndex)
                {
                    checkboxes[i].Visible = true;
                    checkboxes[i].Checked = true;
                    checkboxes[i].Enabled = true; // can uncheck
                }
                else if (i == lastCheckedIndex + 1)
                {
                    checkboxes[i].Visible = true;
                    checkboxes[i].Checked = false;
                    checkboxes[i].Enabled = true; // can check
                }
                else
                {
                    checkboxes[i].Visible = false;
                    checkboxes[i].Checked = false;
                    checkboxes[i].Enabled = false;
                }
            }
        }

        private void UpdateCheckboxSequence()
        {
            var checkboxes = gbStatus.Controls.OfType<CheckBox>().OrderBy(chk => chk.TabIndex).ToArray();
            int lastCheckedIndex = -1;

            for (int i = 0; i < checkboxes.Length; i++)
            {
                if (checkboxes[i].Checked)
                    lastCheckedIndex = i;
            }

            for (int i = 0; i < checkboxes.Length; i++)
            {
                if (i < lastCheckedIndex)
                {
                    checkboxes[i].Visible = true;
                    checkboxes[i].Enabled = false; // readonly
                }
                else if (i == lastCheckedIndex)
                {
                    checkboxes[i].Visible = true;
                    checkboxes[i].Enabled = true; // can uncheck
                }
                else if (i == lastCheckedIndex + 1)
                {
                    checkboxes[i].Visible = true;
                    checkboxes[i].Enabled = true; // can check
                }
                else
                {
                    checkboxes[i].Visible = false;
                    checkboxes[i].Enabled = false;
                    checkboxes[i].Checked = false;
                }
            }
        }
        private void ResetStatusGroup()
        {
            gbStatus.Visible = false;

            if (statusSequence == null) return; // safety check

            foreach (var chk in statusSequence)
            {
                chk.Visible = false;
                chk.Enabled = false;
                chk.Checked = false;
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // ---------- Helpers ----------
        private object NullIfEmpty(string s) => string.IsNullOrWhiteSpace(s) ? (object)DBNull.Value : s;
        private object TryParseDecimalOrDbNull(string text) => decimal.TryParse(text, out var v) ? v : (object)DBNull.Value;
        private object RowCellOrDbNull(DataGridViewRow row, string colName)
        {
            var v = row.Cells[colName]?.Value?.ToString();
            return string.IsNullOrWhiteSpace(v) ? (object)DBNull.Value : v;
        }
        private object RowCellDecimalOrDbNull(DataGridViewRow row, string colName)
        {
            var s = row.Cells[colName]?.Value?.ToString();
            return decimal.TryParse(s, out var d) ? (object)d : DBNull.Value;
        }
        private object RowCellDateOrDbNull(DataGridViewRow row, string colName)
        {
            var s = row.Cells[colName]?.Value?.ToString();
            return DateTime.TryParse(s, out var dt) ? (object)dt : DBNull.Value;
        }
        private string GetSavedStatus(int applicantId)
        {
            string cs = ConfigurationManager.ConnectionStrings["HRISDBCDBS"].ConnectionString;
            using var conn = new SqlConnection(cs);
            conn.Open();
            using var cmd = new SqlCommand("SELECT Status FROM Applicants WHERE ApplicantID=@id", conn);
            cmd.Parameters.AddWithValue("@id", applicantId);
            var result = cmd.ExecuteScalar();
            return result?.ToString() ?? "Pending";
        }


        // ---------- Load applicant ----------
        private void LoadApplicant(int applicantId)
        {
            string cs = ConfigurationManager.ConnectionStrings["HRISDBCDBS"].ConnectionString;

            using var conn = new SqlConnection(cs);
            conn.Open();

            // Applicants (header)
            using (var cmd = new SqlCommand("SELECT * FROM Applicants WHERE ApplicantID=@id", conn))
            {
                cmd.Parameters.AddWithValue("@id", applicantId);
                using var rd = cmd.ExecuteReader();
                if (rd.Read())
                {
                    // Basic info
                    tbLname.Text = rd["LastName"]?.ToString();
                    tbFname.Text = rd["FirstName"]?.ToString();
                    tbMname.Text = rd["MiddleName"]?.ToString();
                    tbSuffix.Text = rd["Suffix"]?.ToString();
                    tbPresentAddress.Text = rd["PresentAddress"]?.ToString();
                    tbPermanentAddress.Text = rd["PermanentAddress"]?.ToString();
                    tbMobileNo.Text = rd["MobileNo"]?.ToString();
                    tbEmailAddress.Text = rd["Email"]?.ToString();

                    if (DateTime.TryParse(rd["DateOfBirth"]?.ToString(), out var dob))
                        dtpDateofBirth.Value = dob;

                    tbPlaceofBirth.Text = rd["PlaceOfBirth"]?.ToString();
                    SelectCombo(cbSex, rd["Sex"]?.ToString());
                    tbCitizenship.Text = rd["Citizenship"]?.ToString();
                    tbReligon.Text = rd["Religion"]?.ToString();
                    SelectCombo(cbMartialStatus, rd["MaritalStatus"]?.ToString());
                    tbHeight.Text = rd["HeightCM"]?.ToString();
                    tbWeight.Text = rd["WeightKG"]?.ToString();

                    // Hide sub/department by default
                    HideSubDepartmentControls();

                    // --- Inside LoadApplicant ---
                    SelectCombo(cbPositionDesired, rd["PositionDesired"]?.ToString());

                    // Temporarily allow SelectedIndexChanged to run
                    bool oldLoading = _isLoading;
                    _isLoading = false;
                    cbPositionDesired_SelectedIndexChanged(cbPositionDesired, EventArgs.Empty);
                    _isLoading = oldLoading;

                    // Now cbSub is visible, so select the saved sub/department
                    if (cbSub.Visible)
                        SelectCombo(cbSub, rd["SubOrDept"]?.ToString());

                    // Expected salary
                    SelectCombo(cbExpectedSalary, rd["ExpectedSalary"]?.ToString());
                    string status = rd["Status"]?.ToString();
                    if (!string.IsNullOrEmpty(status) && status.Equals("Hired", StringComparison.OrdinalIgnoreCase))
                    {
                        MakeFormReadOnly();
                    }
                }
            }

            // Load child tables
            FillGrid(conn,
                "SELECT Name AS FullName, Relationship, OccupationCompany AS Occupation FROM FamilyDetails WHERE ApplicantID=@id",
                dgvFamilyDetails,
                new[] { "FullName", "Relationship", "Occupation" },
                applicantId);

            FillGrid(conn,
                "SELECT ContactName AS EmergencyName, Relationship AS RelationshipToApplicant, ContactNumber AS EmergencyContact FROM EmergencyContacts WHERE ApplicantID=@id",
                dgvEmergency,
                new[] { "EmergencyName", "RelationshipToApplicant", "EmergencyContact" },
                applicantId);

            FillGrid(conn,
                "SELECT InstitutionAddress AS Institution, DegreeMajor AS Degree, YearGraduated_UnitsEarned AS YearGraduated, Honors FROM EducationDetails WHERE ApplicantID=@id",
                dgvEducationalDetails,
                new[] { "Institution", "Degree", "YearGraduated", "Honors" },
                applicantId);

            FillGrid(conn,
                "SELECT DateFrom_To AS DateEmployed, Organization, Designation, ReasonForLeaving AS Reason FROM EmploymentDetails WHERE ApplicantID=@id",
                dgvEmploymentDetails,
                new[] { "DateEmployed", "Organization", "Designation", "Reason" },
                applicantId);

            FillGrid(conn,
                "SELECT FullName AS FullNameReferee, Relationship AS RelationshipReferee, CompanyName AS CompanyNameReferee, EmailAddress AS Email, ContactNumber AS ContactReferee FROM CharacterReferees WHERE ApplicantID=@id",
                dgvReferee,
                new[] { "FullNameReferee", "RelationshipReferee", "CompanyNameReferee", "Email", "ContactReferee" },
                applicantId);
        }

        private void SelectCombo(ComboBox cb, string value)
        {
            if (value == null) return;
            for (int i = 0; i < cb.Items.Count; i++)
            {
                if (string.Equals(cb.Items[i]?.ToString(), value, StringComparison.OrdinalIgnoreCase))
                {
                    cb.SelectedIndex = i;
                    return;
                }
            }
            cb.Items.Add(value);
            cb.SelectedItem = value;
        }

        private void FillGrid(SqlConnection conn, string sql, DataGridView grid, string[] cols, int applicantId)
        {
            using var da = new SqlDataAdapter(sql, conn);
            da.SelectCommand.Parameters.AddWithValue("@id", applicantId);
            var dt = new DataTable();
            da.Fill(dt);

            grid.Rows.Clear();
            foreach (DataRow r in dt.Rows)
            {
                var vals = new object[cols.Length];
                for (int i = 0; i < cols.Length; i++) vals[i] = r[cols[i]];
                grid.Rows.Add(vals);
            }

            if (grid.Columns.Contains("DateOfBirth"))
            {
                grid.Columns["DateOfBirth"].DefaultCellStyle.Format = "MM-dd-yyyy";
            }
        }

        private bool HasAtLeastNEmergencyContacts(int minCount)
        {
            int filledCount = 0;
            if (dgvEmergency != null)
            {
                foreach (DataGridViewRow row in dgvEmergency.Rows)
                {
                    if (row.IsNewRow) continue;

                    var name = row.Cells["EmergencyName"]?.Value?.ToString();
                    var rel = row.Cells["RelationshipToApplicant"]?.Value?.ToString();
                    var tel = row.Cells["EmergencyContact"]?.Value?.ToString();

                    if (!string.IsNullOrWhiteSpace(name) &&
                        !string.IsNullOrWhiteSpace(rel) &&
                        !string.IsNullOrWhiteSpace(tel))
                    {
                        filledCount++;
                    }
                }
            }
            return filledCount >= minCount;
        }


        private bool HasAtLeastOneCharacterReference(int minCount)
        {
            int filledCount = 0;
            if (dgvReferee != null)
            {
                foreach (DataGridViewRow row in dgvReferee.Rows)
                {
                    if (row.IsNewRow) continue;

                    var name = row.Cells["FullNameReferee"]?.Value?.ToString();
                    var relation = row.Cells["RelationshipReferee"]?.Value?.ToString();
                    var company = row.Cells["CompanyNameReferee"]?.Value?.ToString();
                    var email = row.Cells["Email"]?.Value?.ToString();
                    var contact = row.Cells["ContactReferee"]?.Value?.ToString();

                    if (!string.IsNullOrWhiteSpace(name) &&
                        !string.IsNullOrWhiteSpace(relation) &&
                        !string.IsNullOrWhiteSpace(company) &&
                        !string.IsNullOrWhiteSpace(email) &&
                        !string.IsNullOrWhiteSpace(contact))
                    {
                        // validate formats (same as your existing code)
                        if (!Regex.IsMatch(name, @"^[A-Za-z\s]+$"))
                            throw new HRISCDBS.Exeptions.ApplicationFormException("Reference name can only contain letters and spaces.");
                        if (!Regex.IsMatch(relation, @"^[A-Za-z\s]+$"))
                            throw new HRISCDBS.Exeptions.ApplicationFormException("Reference relationship can only contain letters and spaces.");
                        if (!Regex.IsMatch(contact, @"^\d+$"))
                            throw new HRISCDBS.Exeptions.ApplicationFormException("Reference contact must contain only digits.");
                        if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                            throw new HRISCDBS.Exeptions.ApplicationFormException($"Invalid email format for reference: {email}");

                        filledCount++;
                    }
                }
            }
            return filledCount >= minCount;
        }

        private string GetStatusFromCheckboxes()
        {
            for (int i = statusSequence.Length - 1; i >= 0; i--)
            {
                if (statusSequence[i].Checked)
                    return statusSequence[i].Text;
            }
            return "Pending";
        }

        // ---------- Save ----------
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                HRISCDBS.Exeptions.ApplicationFormException.ValidateInputs(
                    tbFname, tbLname, tbMname, cbSex, tbEmailAddress,
                    tbPlaceofBirth, tbCitizenship, tbReligon, tbPresentAddress,
                    tbPermanentAddress, tbHeight, tbWeight, cbMartialStatus, tbMobileNo
                );

                if (cbPositionDesired.SelectedItem == null)
                    throw new HRISCDBS.Exeptions.ApplicationFormException("Position Desired must be selected.");
                if (cbExpectedSalary.SelectedItem == null)
                    throw new HRISCDBS.Exeptions.ApplicationFormException("Expected Salary must be selected.");
                if (!HasAtLeastNEmergencyContacts(2))
                    throw new HRISCDBS.Exeptions.ApplicationFormException("At least 2 emergency contacts are required.");
                if (!HasAtLeastOneCharacterReference(3))
                    throw new HRISCDBS.Exeptions.ApplicationFormException("At least 3 complete character reference is required.");

                string cs = ConfigurationManager.ConnectionStrings["HRISDBCDBS"].ConnectionString;

                using var conn = new SqlConnection(cs);
                conn.Open();
                using var tx = conn.BeginTransaction();

                int applicantId;

                if (_applicantId.HasValue)
                {
                    // UPDATE
                    string updateSql = @"
UPDATE Applicants SET
 PositionDesired=@PositionDesired, ExpectedSalary=@ExpectedSalary, Status=@Status,
 LastName=@LastName, FirstName=@FirstName, MiddleName=@MiddleName, Suffix=@Suffix,
 PresentAddress=@PresentAddress, PermanentAddress=@PermanentAddress,
 MobileNo=@MobileNo, Email=@Email, DateOfBirth=@DateOfBirth, PlaceOfBirth=@PlaceOfBirth,
 Sex=@Sex, Citizenship=@Citizenship, Religion=@Religion, MaritalStatus=@MaritalStatus,
 HeightCM=@HeightCM, WeightKG=@WeightKG,
 SubOrDept=@SubOrDept
WHERE ApplicantID=@ApplicantID;";

                    using var cmd = new SqlCommand(updateSql, conn, tx);
                    AddApplicantHeaderParams(cmd);
                    cmd.Parameters.AddWithValue("@ApplicantID", _applicantId.Value);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected == 0)
                        throw new HRISCDBS.Exeptions.ApplicationFormException($"Applicant with ID {_applicantId} not found.");

                    applicantId = _applicantId.Value;

                    if (GetStatusFromCheckboxes() == "Hired")
                    {
                        var repo = new EmployeeRepository();

                        var newEmp = new Employee
                        {
                            firstName = tbFname.Text,
                            lastName = tbLname.Text,
                            middleName = tbMname.Text,
                            suffix = tbSuffix.Text,           // <-- suffix here
                            birthDate = dtpDateofBirth.Value,
                            gender = cbSex.SelectedItem?.ToString() ?? "",
                            contactNum = tbMobileNo.Text,
                            email = tbEmailAddress.Text,
                            address = tbPresentAddress.Text,
                            position = cbPositionDesired.SelectedItem?.ToString(),
                            subOrDept = cbSub.Visible ? cbSub.SelectedItem?.ToString() : "", // <-- sub/department
                            hireDate = DateTime.Now,
                            employmentStatus = "Active"
                        };


                        int newEmployeeID = repo.CreateEmployee(newEmp);

                        if (newEmployeeID > 0)
                        {
                            MessageBox.Show(
                                "Applicant has been marked as Hired. Employee record has been automatically created.",
                                "Info",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information
                            );

                            // ✅ lock the form
                            MakeFormReadOnly();

                            using var reqForm = new RequirementsPreview(newEmployeeID);
                            if (reqForm.ShowDialog() == DialogResult.OK)
                            {
                                this.DialogResult = DialogResult.OK;
                                Close();
                            }
                        }
                    }

                    DeleteChildren(conn, tx, applicantId);
                }
                else
                {
                    // INSERT
                    string insertSql = @"
INSERT INTO Applicants
(PositionDesired, ExpectedSalary, Status, LastName, FirstName, MiddleName, Suffix, PresentAddress, PermanentAddress,
 MobileNo, Email, DateOfBirth, PlaceOfBirth, Sex, Citizenship, Religion, MaritalStatus, HeightCM, WeightKG,
 SubOrDept)
VALUES
(@PositionDesired, @ExpectedSalary, @Status, @LastName, @FirstName, @MiddleName, @Suffix, @PresentAddress, @PermanentAddress,
 @MobileNo, @Email, @DateOfBirth, @PlaceOfBirth, @Sex, @Citizenship, @Religion, @MaritalStatus, @HeightCM, @WeightKG,
 @SubOrDept);
SELECT CAST(SCOPE_IDENTITY() AS INT);";

                    using var cmd = new SqlCommand(insertSql, conn, tx);
                    AddApplicantHeaderParams(cmd);
                    var result = cmd.ExecuteScalar();
                    if (result == null || !int.TryParse(result.ToString(), out applicantId) || applicantId <= 0)
                        throw new HRISCDBS.Exeptions.ApplicationFormException("Failed to insert new applicant and retrieve ID.");
                }

                InsertChildren(conn, tx, applicantId);

                tx.Commit();

                UpdateCheckboxSequence(); // ✅ Update sequential checkboxes after save

                MessageBox.Show(_applicantId.HasValue
                    ? "Application updated successfully!"
                    : "Application saved successfully!");

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (HRISCDBS.Exeptions.ApplicationFormException ex)
            {
                MessageBox.Show("Validation or Save Error: " + ex.Message, "Application Form Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unexpected Error: " + ex.Message, "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddApplicantHeaderParams(SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue("@PositionDesired", NullIfEmpty(cbPositionDesired.SelectedItem?.ToString()));
            cmd.Parameters.AddWithValue("@ExpectedSalary", NullIfEmpty(cbExpectedSalary.SelectedItem?.ToString()));
            cmd.Parameters.AddWithValue("@Status", GetStatusFromCheckboxes());

            cmd.Parameters.AddWithValue("@LastName", NullIfEmpty(tbLname.Text));
            cmd.Parameters.AddWithValue("@FirstName", NullIfEmpty(tbFname.Text));
            cmd.Parameters.AddWithValue("@MiddleName", NullIfEmpty(tbMname.Text));
            cmd.Parameters.AddWithValue("@Suffix", NullIfEmpty(tbSuffix.Text));

            cmd.Parameters.AddWithValue("@PresentAddress", NullIfEmpty(tbPresentAddress.Text));
            cmd.Parameters.AddWithValue("@PermanentAddress", NullIfEmpty(tbPermanentAddress.Text));

            cmd.Parameters.AddWithValue("@MobileNo", NullIfEmpty(tbMobileNo.Text));
            cmd.Parameters.AddWithValue("@Email", NullIfEmpty(tbEmailAddress.Text));

            cmd.Parameters.AddWithValue("@DateOfBirth", dtpDateofBirth.Value);
            cmd.Parameters.AddWithValue("@PlaceOfBirth", NullIfEmpty(tbPlaceofBirth.Text));
            cmd.Parameters.AddWithValue("@Sex", cbSex.SelectedItem?.ToString() ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Citizenship", NullIfEmpty(tbCitizenship.Text));
            cmd.Parameters.AddWithValue("@Religion", NullIfEmpty(tbReligon.Text));
            cmd.Parameters.AddWithValue("@MaritalStatus", cbMartialStatus.SelectedItem?.ToString() ?? (object)DBNull.Value);

            cmd.Parameters.AddWithValue("@HeightCM", TryParseDecimalOrDbNull(tbHeight.Text));
            cmd.Parameters.AddWithValue("@WeightKG", TryParseDecimalOrDbNull(tbWeight.Text));

            cmd.Parameters.AddWithValue("@SubOrDept", NullIfEmpty(cbSub.Visible ? cbSub.SelectedItem?.ToString() : null));
        }

        private void DeleteChildren(SqlConnection conn, SqlTransaction tx, int applicantId)
        {
            string[] childTables = { "FamilyDetails", "EducationDetails", "EmploymentDetails", "CharacterReferees", "EmergencyContacts" };
            foreach (var table in childTables)
            {
                using var cmd = new SqlCommand($"DELETE FROM {table} WHERE ApplicantID=@ApplicantID", conn, tx);
                cmd.Parameters.AddWithValue("@ApplicantID", applicantId);
                cmd.ExecuteNonQuery();
            }
        }

        private void InsertChildren(SqlConnection conn, SqlTransaction tx, int applicantId)
        {
            // FamilyDetails
            foreach (DataGridViewRow row in dgvFamilyDetails.Rows)
            {
                if (row.IsNewRow) continue;
                using var cmd = new SqlCommand(@"INSERT INTO FamilyDetails(ApplicantID, Name, Relationship, OccupationCompany) 
VALUES(@ApplicantID, @Name, @Relationship, @Occupation)", conn, tx);
                cmd.Parameters.AddWithValue("@ApplicantID", applicantId);
                cmd.Parameters.AddWithValue("@Name", RowCellOrDbNull(row, "FullName"));
                cmd.Parameters.AddWithValue("@Relationship", RowCellOrDbNull(row, "Relationship"));
                cmd.Parameters.AddWithValue("@Occupation", RowCellOrDbNull(row, "Occupation"));
                cmd.ExecuteNonQuery();
            }

            // EducationDetails
            foreach (DataGridViewRow row in dgvEducationalDetails.Rows)
            {
                if (row.IsNewRow) continue;
                using var cmd = new SqlCommand(@"INSERT INTO EducationDetails(ApplicantID, InstitutionAddress, DegreeMajor, YearGraduated_UnitsEarned, Honors) 
VALUES(@ApplicantID, @Institution, @Degree, @YearGraduated, @Honors)", conn, tx);
                cmd.Parameters.AddWithValue("@ApplicantID", applicantId);
                cmd.Parameters.AddWithValue("@Institution", RowCellOrDbNull(row, "Institution"));
                cmd.Parameters.AddWithValue("@Degree", RowCellOrDbNull(row, "Degree"));
                cmd.Parameters.AddWithValue("@YearGraduated", RowCellOrDbNull(row, "YearGraduated"));
                cmd.Parameters.AddWithValue("@Honors", RowCellOrDbNull(row, "Honors"));
                cmd.ExecuteNonQuery();
            }

            // EmploymentDetails
            foreach (DataGridViewRow row in dgvEmploymentDetails.Rows)
            {
                if (row.IsNewRow) continue;
                using var cmd = new SqlCommand(@"INSERT INTO EmploymentDetails(ApplicantID, DateFrom_To, Organization, Designation, ReasonForLeaving) 
VALUES(@ApplicantID, @DateEmployed, @Organization, @Designation, @Reason)", conn, tx);
                cmd.Parameters.AddWithValue("@ApplicantID", applicantId);
                cmd.Parameters.AddWithValue("@DateEmployed", RowCellOrDbNull(row, "DateEmployed"));
                cmd.Parameters.AddWithValue("@Organization", RowCellOrDbNull(row, "Organization"));
                cmd.Parameters.AddWithValue("@Designation", RowCellOrDbNull(row, "Designation"));
                cmd.Parameters.AddWithValue("@Reason", RowCellOrDbNull(row, "Reason"));
                cmd.ExecuteNonQuery();
            }

            // CharacterReferees
            foreach (DataGridViewRow row in dgvReferee.Rows)
            {
                if (row.IsNewRow) continue;
                using var cmd = new SqlCommand(@"INSERT INTO CharacterReferees(ApplicantID, FullName, Relationship, CompanyName, EmailAddress, ContactNumber) 
VALUES(@ApplicantID, @FullName, @Relationship, @CompanyName, @Email, @ContactNumber)", conn, tx);
                cmd.Parameters.AddWithValue("@ApplicantID", applicantId);
                cmd.Parameters.AddWithValue("@FullName", RowCellOrDbNull(row, "FullNameReferee"));
                cmd.Parameters.AddWithValue("@Relationship", RowCellOrDbNull(row, "RelationshipReferee"));
                cmd.Parameters.AddWithValue("@CompanyName", RowCellOrDbNull(row, "CompanyNameReferee"));
                cmd.Parameters.AddWithValue("@Email", RowCellOrDbNull(row, "Email"));
                cmd.Parameters.AddWithValue("@ContactNumber", RowCellOrDbNull(row, "ContactReferee"));
                cmd.ExecuteNonQuery();
            }

            // EmergencyContacts
            foreach (DataGridViewRow row in dgvEmergency.Rows)
            {
                if (row.IsNewRow) continue;

                using var cmd = new SqlCommand(@"
        INSERT INTO EmergencyContacts(ApplicantID, ContactName, Relationship, ContactNumber) 
        VALUES(@ApplicantID, @ContactName, @Relationship, @ContactNumber)", conn, tx);

                cmd.Parameters.AddWithValue("@ApplicantID", applicantId);
                cmd.Parameters.AddWithValue("@ContactName", RowCellOrDbNull(row, "EmergencyName"));
                cmd.Parameters.AddWithValue("@Relationship", RowCellOrDbNull(row, "RelationshipToApplicant"));
                cmd.Parameters.AddWithValue("@ContactNumber", RowCellOrDbNull(row, "EmergencyContact"));

                cmd.ExecuteNonQuery();
            }
        }
        private void MakeFormReadOnly()
        {
            SetControlsReadOnly(this);

            // Disable status checkboxes group
            gbStatus.Enabled = false;

            // Hide/disable buttons except Back
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Button btn && btn != btnBack)
                    btn.Enabled = false;
            }

            btnSubmit.Visible = false;  // Hide submit
            btnBack.Enabled = true;     // Keep back active
        }

        private void SetControlsReadOnly(Control parent)
        {
            foreach (Control ctrl in parent.Controls)
            {
                if (ctrl is TextBox tb)
                {
                    tb.ReadOnly = true;
                }
                else if (ctrl is ComboBox cb)
                {
                    cb.Enabled = false;
                }
                else if (ctrl is DateTimePicker dt)
                {
                    dt.Enabled = false;
                }
                else if (ctrl is DataGridView dgv)
                {
                    dgv.ReadOnly = true;
                    dgv.AllowUserToAddRows = false;
                    dgv.AllowUserToDeleteRows = false;
                }
                else if (ctrl.HasChildren)
                {
                    // Recursively process groupboxes, panels, tabpages, etc.
                    SetControlsReadOnly(ctrl);
                }
            }
        }
    }
}