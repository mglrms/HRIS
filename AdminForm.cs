using HRISCDBS.EmployeeModule;
using HRISCDBS.Module;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//form size: 1902, 892
namespace HRISCDBS
{
    public partial class AdminForm : Form
    {
        FormDashboard Dashboard;
        FormAttendanceMonitoring AttendanceMonitoring;
        FormApplicantRecords ApplicantRecords;
        FormRequirmentsRecords RequirmentsRecords;
        FormEmployeeRecords EmployeeRecords;
        FormLoginCredentials LoginCredentials;
        FormLogTrail LogTrail;
        FormDB_BackupRestore BackupRestore;
        FormLeave Leave;
        public static AdminForm Instance { get; private set; }
        public AdminForm()
        {
            InitializeComponent();
            this.Load += AdminForm_Load;
            this.IsMdiContainer = true; // ✅ Make it the parent container
            Instance = this;
        }

        bool applicantExpand = false;
        bool employeeExpand = false;

        bool applicantCollapsePending = false;
        bool employeeCollapsePending = false;

        private void applicantTimer_Tick(object sender, EventArgs e)
        {
            if (applicantCollapsePending) // collapsing
            {
                flpApplicant.Height -= 10;
                if (flpApplicant.Height <= 76)
                {
                    flpApplicant.Height = 76;
                    applicantCollapsePending = false;
                    applicantExpand = false;
                    applicantTimer.Stop();
                }
            }
            else
            {
                if (!applicantExpand)
                {
                    flpApplicant.Height += 10;
                    if (flpApplicant.Height >= 222)
                    {
                        flpApplicant.Height = 222;
                        applicantExpand = true;
                        applicantTimer.Stop();
                    }
                }
                else
                {
                    flpApplicant.Height -= 10;
                    if (flpApplicant.Height <= 76)
                    {
                        flpApplicant.Height = 76;
                        applicantExpand = false;
                        applicantTimer.Stop();
                    }
                }
            }
        }

        private void btnApplicantRecordsMgm_Click(object sender, EventArgs e)
        {
            CollapsePanelsSmoothly();
            applicantTimer.Start();
        }

        private void employeeTimer_Tick(object sender, EventArgs e)
        {
            if (employeeCollapsePending) // collapsing
            {
                flpEmployee.Height -= 10;
                if (flpEmployee.Height <= 76)
                {
                    flpEmployee.Height = 76;
                    employeeCollapsePending = false;
                    employeeExpand = false;
                    employeeTimer.Stop();
                }
            }
            else
            {
                if (!employeeExpand)
                {
                    flpEmployee.Height += 10;
                    if (flpEmployee.Height >= 305)
                    {
                        flpEmployee.Height = 305;
                        employeeExpand = true;
                        employeeTimer.Stop();
                    }
                }
                else
                {
                    flpEmployee.Height -= 10;
                    if (flpEmployee.Height <= 76)
                    {
                        flpEmployee.Height = 76;
                        employeeExpand = false;
                        employeeTimer.Stop();
                    }
                }
            }
        }

        private void AdminForm_Load(object sender, EventArgs e)
        {
            btnDashboard_Click(sender, e);
        }

        private void btnEmployeeRecordsMgm_Click(object sender, EventArgs e)
        {
            CollapsePanelsSmoothly();
            employeeTimer.Start();
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            CollapsePanelsSmoothly();
            Dashboard = FormManager.OpenForm(Dashboard, this);
        }

        private void btnAttendance_Click(object sender, EventArgs e)
        {
            CollapsePanelsSmoothly();
            AttendanceMonitoring = FormManager.OpenForm(AttendanceMonitoring, this);
        }

        private void btnApplicantRecords_Click(object sender, EventArgs e)
        {
            ApplicantRecords = FormManager.OpenForm(ApplicantRecords, this);
        }

        private void btnReqRecords_Click(object sender, EventArgs e)
        {
            RequirmentsRecords = FormManager.OpenForm(RequirmentsRecords, this);
        }

        private void btnEmployeeRecords_Click(object sender, EventArgs e)
        {
            EmployeeRecords = FormManager.OpenForm(EmployeeRecords, this);
        }

        private void btnLoginCredentials_Click(object sender, EventArgs e)
        {
            LoginCredentials = FormManager.OpenForm(LoginCredentials, this);
        }

        private void btnLogTrail_Click(object sender, EventArgs e)
        {
            CollapsePanelsSmoothly();
            LogTrail = FormManager.OpenForm(LogTrail, this);
        }

        private void btnBackupRestore_Click(object sender, EventArgs e)
        {
            CollapsePanelsSmoothly();
            BackupRestore = FormManager.OpenForm(BackupRestore, this);
        }


        private void btnLogout_Click(object sender, EventArgs e)
        {
            try
            {
                // 🔹 Log the logout action
                Logger.LogAction("Logout", $"User {CurrentUser.Username} (UserID={CurrentUser.UserID}) has logged out.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to log logout action: " + ex.Message,
                    "Logging Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            CurrentUser.Clear();
            LoginForm form = new LoginForm();
            form.Show();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            EmployeeForm emp = new EmployeeForm();
            emp.Show();
            this.Close();
        }

        private void btnLeave_Click(object sender, EventArgs e)
        {
            Leave = FormManager.OpenForm(Leave, this);
        }

        private void CollapsePanelsSmoothly()
        {
            if (applicantExpand)
            {
                applicantCollapsePending = true;
                applicantTimer.Start();
            }

            if (employeeExpand)
            {
                employeeCollapsePending = true;
                employeeTimer.Start();
            }
        }


    }
}

