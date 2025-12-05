using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HRISCDBS
{
    public partial class FormDB_BackupRestore : Form
    {
        private readonly string connString = ConfigurationManager.ConnectionStrings["HRISDBCDBS"].ConnectionString;
        //SqlConnection con = new SqlConnection("server=localhost; database= HRISCDBS_DB; integrated security=true; TrustServerCertificate=True"); //or .\\SQLEXPRESS

        public FormDB_BackupRestore()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click_1(object sender, EventArgs e)
        {

            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedPath = folderDialog.SelectedPath;
                    tbBackup.Text = selectedPath;
                }
            }
        }

        private void btnBackup_Click_1(object sender, EventArgs e)
        {
            string backupPath = tbBackup.Text.Trim();

            //  Check if the user actually selected a folder
            if (string.IsNullOrWhiteSpace(backupPath))
            {
                MessageBox.Show("Please select a backup folder first.", "Missing Folder", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //  Check if folder exists
            if (!Directory.Exists(backupPath))
            {
                MessageBox.Show("The selected backup folder does not exist.", "Invalid Folder", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SqlConnection connection = new SqlConnection(connString))
            using (SqlCommand command = new SqlCommand("BackUpDatabase", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@location", backupPath);
                connection.Open();

                try
                {
                    command.ExecuteNonQuery();

                    string date = DateTime.Now.ToString("yyyyMMdd");
                    string fileName = Path.Combine(backupPath, $"HRISCDB_DB_{date}.BAK");

                    MessageBox.Show($"Backup successful!\nFile saved at:\n{fileName}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error executing stored procedure:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnBrowse2_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Backup files (*.bak)|*.bak|All files (*.*)|*.*"; // optional filter

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Get full path including filename
                string filePath = openFileDialog.FileName;
                tbRestore.Text = filePath;
            }
        }

        private void btnRestore_Click_1(object sender, EventArgs e)
        {
            string backupPath = tbRestore.Text.Trim();

            if (string.IsNullOrWhiteSpace(backupPath))
            {
                MessageBox.Show("Please select a backup file first.", "Missing File",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!File.Exists(backupPath))
            {
                MessageBox.Show("The selected backup file does not exist.", "Invalid File",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Confirm restore action
            var result = MessageBox.Show(
                "This will disconnect all users and overwrite the current database.\n\n" +
                "Are you sure you want to continue?",
                "Confirm Restore",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result != DialogResult.Yes)
                return;

            try
            {
                // Build connection string to master database
                var builder = new SqlConnectionStringBuilder(connString)
                {
                    InitialCatalog = "master"
                };

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();

                    string database = "HRISCDBS_DB"; // Your database name

                    // Step 1: Kill all active connections
                    string killConnections = $@"
                DECLARE @kill varchar(8000) = '';
                SELECT @kill = @kill + 'KILL ' + CONVERT(varchar(5), session_id) + ';'
                FROM sys.dm_exec_sessions
                WHERE database_id = DB_ID('{database}')
                  AND session_id <> @@SPID;
                
                EXEC(@kill);";

                    using (SqlCommand cmd = new SqlCommand(killConnections, connection))
                    {
                        cmd.CommandTimeout = 60;
                        cmd.ExecuteNonQuery();
                    }

                    // Step 2: Set to SINGLE_USER mode
                    string singleUser = $@"
                ALTER DATABASE [{database}] 
                SET SINGLE_USER WITH ROLLBACK IMMEDIATE;";

                    using (SqlCommand cmd = new SqlCommand(singleUser, connection))
                    {
                        cmd.CommandTimeout = 60;
                        cmd.ExecuteNonQuery();
                    }

                    // Step 3: Restore the database
                    string restoreQuery = $@"
                RESTORE DATABASE [{database}] 
                FROM DISK = @BackupPath 
                WITH REPLACE, RECOVERY;";

                    using (SqlCommand cmd = new SqlCommand(restoreQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@BackupPath", backupPath);
                        cmd.CommandTimeout = 600; // 10 minutes for large databases
                        cmd.ExecuteNonQuery();
                    }

                    // Step 4: Set back to MULTI_USER mode
                    string multiUser = $@"
                ALTER DATABASE [{database}] 
                SET MULTI_USER;";

                    using (SqlCommand cmd = new SqlCommand(multiUser, connection))
                    {
                        cmd.CommandTimeout = 60;
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show(
                    "Database restored successfully!\n\n" +
                    "All users have been disconnected.\n" +
                    "Please log in again if needed.",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                // Close current form or application after restore
                // since the connection context has changed
                DialogResult restartResult = MessageBox.Show(
                    "The application needs to restart to reconnect to the restored database.\n\n" +
                    "Click OK to close the application.",
                    "Restart Required",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                if (restartResult == DialogResult.OK)
                {
                    Application.Exit();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(
                    $"Database restore unsuccessful.\n\n" +
                    $"SQL Error: {ex.Message}\n\n" +
                    $"Error Number: {ex.Number}\n\n" +
                    "Common issues:\n" +
                    "1. SQL Server lacks file access permissions\n" +
                    "2. Backup file is corrupted\n" +
                    "3. Active connections couldn't be closed\n" +
                    "4. Insufficient disk space",
                    "Restore Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Unexpected error: {ex.Message}\n\n{ex.StackTrace}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
