using HRISCDBS.Exeptions;
using HRISCDBS.Module;
using HRISCDBS.Repository;
using Microsoft.Data.SqlClient;
using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace HRISCDBS
{
    public partial class EmployeeCreateEdit : Form
    {
        private byte[] selectedPicture;
        private int employeeID = 0;
        private bool isNew = false;

        public EmployeeCreateEdit()
        {
            InitializeComponent();
            dtpHireDate.MaxDate = DateTime.Today;

            // Populate dropdowns
            PopulatePerformanceEval();
            PopulateGender();

            // Hide sub-department dropdown by default
            HideSubDepartmentControls();

            // Populate position choices
            cbPosition.Items.Clear();
            cbPosition.Items.AddRange(new string[] {
        "Teaching",
        "Non-Teaching",
        "Maintenance",
        "Coach"
    });
            cbPosition.SelectedIndex = -1;

            // Subscribe to position change event
            cbPosition.SelectedIndexChanged += cbPosition_SelectedIndexChanged;
        }

        // Constructor for Add New mode
        public EmployeeCreateEdit(bool isNew) : this()
        {
            this.isNew = isNew;

            if (isNew)
            {
                this.Text = "Add New Employee";
                this.lblTitle.Text = "Add New Employee";
                this.employeeID = 0;
                lblID.Text = "(Auto)"; // ✅ Show placeholder

                cbEmplymentStatus.SelectedItem = "Active";
                cbEmplymentStatus.Enabled = false;

                cbPosition.SelectedIndex = 0;
                dtpHireDate.Value = DateTime.Today;

                cbPerformanceEval.SelectedIndex = -1;
                cbPerformanceEval.Enabled = false;
            }
        }

        // 🧩 Helper Methods for Sub/Department
        private void ShowSubDepartmentControls(string labelText, string[] items, string position)
        {
            lblSub.Text = labelText;
            cbSub.Items.Clear();
            cbSub.Items.AddRange(items);

            if (position.Equals("Non-Teaching", StringComparison.OrdinalIgnoreCase))
                cbSub.Location = new Point(465, 524);
            else if (position.Equals("Teaching", StringComparison.OrdinalIgnoreCase))
                cbSub.Location = new Point(532, 524);

            lblSub.Visible = true;
            cbSub.Visible = true;
        }

        private void HideSubDepartmentControls()
        {
            cbSub.Visible = false;
            lblSub.Visible = false;
            cbSub.Items.Clear();
        }

        private void SetupSubOrDept(string position)
        {
            if (string.IsNullOrWhiteSpace(position))
            {
                HideSubDepartmentControls();
                return;
            }

            if (position.Equals("Teaching", StringComparison.OrdinalIgnoreCase))
            {
                ShowSubDepartmentControls("Field of specialization", new string[]
                {
                    "Mathematics","English","Science","Filipino",
                    "Araling Panlipunan","MAPEH","TLE","Values Education"
                }, position);
            }
            else if (position.Equals("Non-Teaching", StringComparison.OrdinalIgnoreCase))
            {
                ShowSubDepartmentControls("Department", new string[]
                {
                    "Human Resources","Finance","Information Technology","Registrar",
                    "Library","Guidance Office","Clinic"
                }, position);
            }
            else
            {
                HideSubDepartmentControls();
            }
        }

        private void cbPosition_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbPosition.SelectedItem != null)
            {
                string selectedPosition = cbPosition.SelectedItem.ToString();
                SetupSubOrDept(selectedPosition);
            }
        }

        // Edit existing employee
        public void EditEmployee(Employee employee)
        {
            Logger.LogAction("Employee Edit Opened",
                $"EmployeeID: {employee.id} ({employee.firstName} {employee.lastName}) opened for edit by UserID: {CurrentUser.UserID} ({CurrentUser.Username}).");

            this.Text = "Edit Employee";
            this.lblTitle.Text = "Edit Employee";

            employeeID = employee.id;
            lblID.Text = employee.id.ToString(); // ✅ Auto-fill label

            tbFname.Text = employee.firstName;
            tbLname.Text = employee.lastName;
            tbMiddleName.Text = employee.middleName;
            tbSuffix.Text = employee.suffix;
            dtpBday.Value = employee.birthDate;
            cbGender.SelectedItem = employee.gender;
            tbContact.Text = employee.contactNum;
            tbEmail.Text = employee.email;
            tbAddress.Text = employee.address;
            tbRfid.Text = employee.rfidUID;
            cbEmplymentStatus.SelectedItem = employee.employmentStatus;
            cbEmplymentStatus.Enabled = true;

            // ✅ Load Picture
            if (employee.picture != null && employee.picture.Length > 0)
            {
                using (MemoryStream ms = new MemoryStream(employee.picture))
                    pbImage.Image = Image.FromStream(ms);
                selectedPicture = employee.picture;
            }
            else
            {
                pbImage.Image = null;
                selectedPicture = null;
            }

            // ✅ Ensure Position ComboBox is ready
            if (cbPosition.Items.Count == 0)
                cbPosition.Items.AddRange(new[] { "Teaching", "Non-Teaching" });

            // ✅ Select Position & Update Sub/Dept
            SelectComboBoxItem(cbPosition, employee.position);
            SetupSubOrDept(employee.position);
            SelectComboBoxItem(cbSub, employee.subOrDept);

            // ✅ Performance Evaluation
            PopulatePerformanceEval();
            SelectComboBoxItem(cbPerformanceEval, employee.performanceRating);

            // ✅ Hire Date
            dtpHireDate.Value = employee.hireDate < dtpHireDate.MinDate
                ? dtpHireDate.MinDate
                : employee.hireDate > dtpHireDate.MaxDate
                    ? dtpHireDate.MaxDate
                    : employee.hireDate;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate inputs
                EmployeeException.ValidateInputs(
                    tbFname, tbLname, tbMiddleName, cbGender, tbContact, tbEmail,
                    tbAddress, cbEmplymentStatus, cbPosition, tbRfid, pbImage
                );

                var employee = new Employee
                {
                    id = employeeID,
                    firstName = tbFname.Text,
                    lastName = tbLname.Text,
                    middleName = tbMiddleName.Text,
                    suffix = tbSuffix.Text,
                    birthDate = dtpBday.Value,
                    gender = cbGender.SelectedItem?.ToString(),
                    contactNum = tbContact.Text,
                    email = tbEmail.Text,
                    address = tbAddress.Text,
                    picture = selectedPicture,
                    position = cbPosition.SelectedItem?.ToString(),
                    subOrDept = cbSub.SelectedItem?.ToString(),
                    hireDate = dtpHireDate.Value,
                    employmentStatus = cbEmplymentStatus.SelectedItem?.ToString(),
                    rfidUID = tbRfid.Text,
                    performanceRating = cbPerformanceEval.SelectedItem?.ToString()
                };

                var repo = new EmployeeRepository();

                if (employee.id == 0)
                {
                    employee.id = repo.CreateEmployee(employee);
                    lblID.Text = employee.id.ToString(); // ✅ Update label after saving
                    Logger.LogAction("Employee Created",
                        $"EmployeeID {employee.id} ({employee.firstName} {employee.lastName}) created by UserID {CurrentUser.UserID} ({CurrentUser.Username}).");
                    MessageBox.Show("Employee successfully created!");
                }
                else
                {
                    repo.UpdateEmployee(employee);
                    Logger.LogAction("Employee Updated",
                        $"EmployeeID {employee.id} ({employee.firstName} {employee.lastName}) updated by UserID {CurrentUser.UserID} ({CurrentUser.Username}).");

                    DialogResult result = MessageBox.Show(
                        "Employee successfully updated!\nDo you want to edit login credentials?",
                        "Save Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                    if (result == DialogResult.Yes)
                    {
                        using (var frm = new LogInCredentialsEdit(EnsureUserAccount(employee.id)))
                            frm.ShowDialog();
                    }

                    this.DialogResult = DialogResult.OK;
                }
            }
            catch (EmployeeException ex)
            {
                MessageBox.Show(ex.Message, "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unexpected error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBack_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.Cancel;

        private void btnChoose_Click(object sender, EventArgs e)
        {
            using OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pbImage.Image = Image.FromFile(openFileDialog.FileName);
                selectedPicture = File.ReadAllBytes(openFileDialog.FileName);
            }
        }

        private void PopulatePerformanceEval()
        {
            cbPerformanceEval.Items.Clear();
            cbPerformanceEval.Items.AddRange(new[]
            {
                "Excellent / Outstanding",
                "Good",
                "Average",
                "Below Average / Needs Improvement",
                "Poor"
            });
        }

        private void PopulateGender()
        {
            cbGender.Items.Clear();
            cbGender.Items.AddRange(new[] { "Male", "Female" });
        }

        private void SelectComboBoxItem(ComboBox comboBox, string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return;
            int index = comboBox.FindStringExact(value.Trim());
            if (index >= 0)
                comboBox.SelectedIndex = index;
        }

        // ✅ Ensure UserAccount exists and return UserID
        private int EnsureUserAccount(int employeeID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HRISDBCDBS"].ConnectionString))
                {
                    conn.Open();

                    string getEmpSql = @"SELECT LastName, RFIDUID, Position 
                                         FROM Employee WHERE EmployeeID = @EmployeeID";
                    string lastName = "", rfid = "", role = "Employee";

                    using (SqlCommand empCmd = new SqlCommand(getEmpSql, conn))
                    {
                        empCmd.Parameters.AddWithValue("@EmployeeID", employeeID);
                        using var reader = empCmd.ExecuteReader();
                        if (reader.Read())
                        {
                            lastName = reader["LastName"].ToString();
                            rfid = reader["RFIDUID"].ToString();
                            role = reader["Position"].ToString();
                        }
                        else return 0;
                    }

                    if (string.IsNullOrWhiteSpace(rfid)) return 0;

                    string getUserSql = "SELECT UserID FROM UserAccount WHERE EmployeeID = @EmployeeID";
                    using (SqlCommand getUserCmd = new SqlCommand(getUserSql, conn))
                    {
                        getUserCmd.Parameters.AddWithValue("@EmployeeID", employeeID);
                        var existingUser = getUserCmd.ExecuteScalar();
                        if (existingUser != null) return (int)existingUser;
                    }

                    string username = lastName + new Random().Next(100000, 999999);
                    string password = lastName + new Random().Next(100, 999);

                    string insertSql = @"
                        INSERT INTO UserAccount (Username, PasswordHash, RoleName, EmployeeID, IsActive, CreatedAt)
                        OUTPUT INSERTED.UserID
                        VALUES (@Username, @PasswordHash, @RoleName, @EmployeeID, 1, GETDATE());";

                    using (SqlCommand insertCmd = new SqlCommand(insertSql, conn))
                    {
                        insertCmd.Parameters.AddWithValue("@Username", username);
                        insertCmd.Parameters.AddWithValue("@PasswordHash", password);
                        insertCmd.Parameters.AddWithValue("@RoleName", role);
                        insertCmd.Parameters.AddWithValue("@EmployeeID", employeeID);
                        return (int)insertCmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error ensuring user account: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
        }
    }
}