using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Windows.Forms;

namespace HRISCDBS
{
    public partial class Status : Form
    {
        private readonly int applicantId;
        private string selectedStatus;
        private readonly string connString =
            ConfigurationManager.ConnectionStrings["HRISDBCDBS"].ConnectionString;

        private readonly Dictionary<PictureBox, bool> pictureBoxStates = new Dictionary<PictureBox, bool>();
        private readonly List<(PictureBox pb, string status)> sequence;

        public Status(int applicantId, string currentStatus)
        {
            InitializeComponent();

            this.applicantId = applicantId;
            this.selectedStatus = currentStatus;
            requirements_btn.Visible = false;
            LoadApplicantInfo();

            // set up sequence
            sequence = new List<(PictureBox, string)>
            {
                (pictureBox1, "Applied"),
                (pictureBox2, "Under Review"),
                (pictureBox3, "For Interview"),
                (pictureBox4, "For Exam/Assessment"),
                (pictureBox5, "Shortlisted"),
                (pictureBox6, "Hired")
            };

            foreach (var step in sequence)
            {
                step.pb.Click += PictureBox_Click;
                pictureBoxStates[step.pb] = false;
            }

            HighlightCurrentStatus(currentStatus);

            
            if (currentStatus == "Hired")
            {
                requirements_btn.Visible = true;
            }
        }

        private void PictureBox_Click(object sender, EventArgs e)
        {
            var pb = sender as PictureBox;
            if (pb == null) return;

            int clickedIndex = sequence.FindIndex(x => x.pb == pb);
            if (clickedIndex == -1) return;

            int currentIndex = sequence.FindIndex(x => x.status == selectedStatus);

            if (clickedIndex > currentIndex + 1)
            {
                MessageBox.Show("You must follow the sequence.", "Invalid Action",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (clickedIndex < currentIndex)
            {
                MessageBox.Show("You cannot go back to a previous status.", "Invalid Action",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (sequence[clickedIndex].status == "Hired" &&
                selectedStatus != "Shortlisted")
            {
                MessageBox.Show("You must be Shortlisted before moving to Hired.", "Invalid Action",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // update check states
            for (int i = 0; i <= clickedIndex; i++)
                pictureBoxStates[sequence[i].pb] = true;
            for (int i = clickedIndex + 1; i < sequence.Count; i++)
                pictureBoxStates[sequence[i].pb] = false;

            UpdatePictureBoxes();

            selectedStatus = sequence[clickedIndex].status;

            if (selectedStatus == "Hired")
            {
                var result = MessageBox.Show(
                    "Do you want to proceed to requirements?",
                    "Proceed",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        int newEmpId = SaveApplicantToEmployee(applicantId);

                        MessageBox.Show("Applicant moved to Employees table!",
                            "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        this.DialogResult = DialogResult.OK;
                        this.Hide();
                        this.Owner?.Invoke(new Action(() => {
                            // This will trigger any refresh logic in parent form
                        }));

                        using (var req = new RequirementsPreview(newEmpId))
                        {
                            req.ShowDialog();
                        }

                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error saving employee: " + ex.Message, "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    requirements_btn.Visible = true;
                }
            }
        }

        private void UpdatePictureBoxes()
        {
            foreach (var kvp in pictureBoxStates)
            {
                kvp.Key.Image = kvp.Value
                    ? Properties.Resources.check_mark
                    : Properties.Resources.check_box;
            }
        }

        private void HighlightCurrentStatus(string currentStatus)
        {
            int currentIndex = sequence.FindIndex(x => x.status == currentStatus);
            if (currentIndex >= 0)
            {
                for (int i = 0; i <= currentIndex; i++)
                    pictureBoxStates[sequence[i].pb] = true;
                for (int i = currentIndex + 1; i < sequence.Count; i++)
                    pictureBoxStates[sequence[i].pb] = false;

                UpdatePictureBoxes();
                selectedStatus = currentStatus;
            }
        }

        private int SaveApplicantToEmployee(int applicantId)
        {
            int existingEmployeeId = 0;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                // ✅ 1. Check if this applicant was already moved
                string checkQuery = "SELECT EmployeeID FROM Employee WHERE SourceApplicantID = @ApplicantID";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@ApplicantID", applicantId);
                    object result = checkCmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        existingEmployeeId = Convert.ToInt32(result);
                        return existingEmployeeId; // return existing record, no duplication
                    }
                }

                // ✅ 2. If not found, proceed with transaction
                SqlTransaction tx = conn.BeginTransaction();

                try
                {
                    string select = @"
                SELECT 
                    LastName, FirstName, MiddleName, Suffix, DateOfBirth, Sex,
                    Citizenship, Religion, MaritalStatus,
                    PresentAddress, PermanentAddress,
                    MobileNo, Email, PlaceOfBirth, PositionDesired, SubOrDept
                FROM Applicants
                WHERE ApplicantID = @id";

                    SqlCommand sel = new SqlCommand(select, conn, tx);
                    sel.Parameters.AddWithValue("@id", applicantId);

                    SqlDataReader r = sel.ExecuteReader();
                    if (!r.Read())
                        throw new Exception("Applicant not found.");

                    string last = r["LastName"]?.ToString();
                    string first = r["FirstName"]?.ToString();
                    string mid = r["MiddleName"]?.ToString();
                    string suffix = r["Suffix"]?.ToString();
                    DateTime birth = Convert.ToDateTime(r["DateOfBirth"]);
                    string gender = r["Sex"]?.ToString();
                    string citizen = r["Citizenship"]?.ToString();
                    string religion = r["Religion"]?.ToString();
                    string marital = r["MaritalStatus"]?.ToString();
                    string present = r["PresentAddress"]?.ToString();
                    string permanent = r["PermanentAddress"]?.ToString();
                    string mobile = r["MobileNo"]?.ToString();
                    string email = r["Email"]?.ToString();
                    string pob = r["PlaceOfBirth"]?.ToString();
                    string pos = r["PositionDesired"]?.ToString();
                    string subOrDept = r["SubOrDept"]?.ToString();
                    r.Close();

                    // ✅ 3. Insert new Employee and link it to Applicant
                    string insertEmp = @"
                INSERT INTO Employee
                    (FirstName, LastName, MiddleName, Suffix, DateOfBirth, Gender,
                     ContactNumber, Email, Address, Position, SubOrDept, HireDate, EmploymentStatus, SourceApplicantID)
                OUTPUT INSERTED.EmployeeID
                VALUES
                    (@First, @Last, @Mid, @Suffix, @Birth, @Gender,
                     @Mobile, @Email, @Present, @Pos, @SubOrDept, @HireDate, 'Active', @ApplicantID)";

                    SqlCommand insEmp = new SqlCommand(insertEmp, conn, tx);
                    insEmp.Parameters.AddWithValue("@First", first ?? (object)DBNull.Value);
                    insEmp.Parameters.AddWithValue("@Last", last ?? (object)DBNull.Value);
                    insEmp.Parameters.AddWithValue("@Mid", (object)mid ?? DBNull.Value);
                    insEmp.Parameters.AddWithValue("@Suffix", (object)suffix ?? DBNull.Value);
                    insEmp.Parameters.AddWithValue("@Birth", birth);
                    insEmp.Parameters.AddWithValue("@Gender", gender ?? (object)DBNull.Value);
                    insEmp.Parameters.AddWithValue("@Mobile", mobile ?? (object)DBNull.Value);
                    insEmp.Parameters.AddWithValue("@Email", email ?? (object)DBNull.Value);
                    insEmp.Parameters.AddWithValue("@Present", present ?? (object)DBNull.Value);
                    insEmp.Parameters.AddWithValue("@Pos", pos ?? (object)DBNull.Value);
                    insEmp.Parameters.AddWithValue("@SubOrDept", (object)subOrDept ?? DBNull.Value);
                    insEmp.Parameters.AddWithValue("@HireDate", DateTime.Now);
                    insEmp.Parameters.AddWithValue("@ApplicantID", applicantId);

                    int newEmployeeId = (int)insEmp.ExecuteScalar();

                    // ✅ 4. Update Applicant status to “Hired”
                    string updateStatus = "UPDATE Applicants SET Status = 'Hired' WHERE ApplicantID = @id";
                    SqlCommand updateCmd = new SqlCommand(updateStatus, conn, tx);
                    updateCmd.Parameters.AddWithValue("@id", applicantId);
                    updateCmd.ExecuteNonQuery();

                    tx.Commit();
                    return newEmployeeId;
                }
                catch
                {
                    tx.Rollback();
                    throw;
                }
            }
        }
        private void LoadApplicantInfo()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    string q = @"
                        SELECT 
                          (LastName + ', ' + FirstName + ' ' + ISNULL(MiddleName,'') + ' ' + ISNULL(Suffix,'')) AS FullName,
                          PositionDesired, ExpectedSalary, MobileNo, Email
                        FROM Applicants
                        WHERE ApplicantID = @id";

                    SqlCommand cmd = new SqlCommand(q, conn);
                    cmd.Parameters.AddWithValue("@id", applicantId);

                    SqlDataReader r = cmd.ExecuteReader();
                    if (r.Read())
                    {
                        namelbl.Text = $"Name: {r["FullName"]}";
                        positionlbl.Text = $"Position: {r["PositionDesired"]}";
                        salarylbl.Text = r["ExpectedSalary"] != DBNull.Value
                            ? $"Expected Salary: {string.Format("{0:C}", r["ExpectedSalary"])}"
                            : "Expected Salary: N/A";
                        mobilelbl.Text = $"Mobile No: {r["MobileNo"]}";
                        emaillbl.Text = $"Email: {r["Email"]}";
                    }
                    else
                    {
                        MessageBox.Show("Applicant not found.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading applicant: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void requirements_btn_Click(object sender, EventArgs e)
        {
            try
            {
                int newEmpId = SaveApplicantToEmployee(applicantId);

                MessageBox.Show("Applicant moved to Employees table!",
                    "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                using (var req = new RequirementsPreview(newEmpId))
                {
                    req.ShowDialog();
                }

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving employee: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedStatus))
            {
                MessageBox.Show("Please select a status before saving.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    string query = "UPDATE Applicants SET Status = @Status WHERE ApplicantID = @ApplicantID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Status", selectedStatus);
                        cmd.Parameters.AddWithValue("@ApplicantID", applicantId);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Status updated successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating status: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Cancel_btn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}