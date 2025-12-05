using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using System.Configuration;


namespace HRISCDBS
{
    public partial class FormLogTrail : Form
    {
        private readonly string connString = ConfigurationManager.ConnectionStrings["HRISDBCDBS"].ConnectionString;

        public FormLogTrail()
        {
            InitializeComponent();
            LoadLogs();
            dgvFormat.ApplyDetail(dgvLogTrail, 40);
        }

        private void FormLogTrail_Load(object sender, EventArgs e)
        {
            LoadLogs();
        }

        private void LoadLogs(string search = "", DateTime? fromDate = null, DateTime? toDate = null)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                string query = @"
                    SELECT 
                        L.LogID,
                        L.UserID,
                        U.Username,
                        U.RoleName,
                        L.Action,
                        L.Details,
                        L.Timestamp
                    FROM SystemLogs L
                    INNER JOIN UserAccount U ON L.UserID = U.UserID
                    WHERE 1=1";

                // Apply date range filter if provided
                if (fromDate.HasValue && toDate.HasValue)
                    query += " AND L.Timestamp BETWEEN @FromDate AND @ToDate";

                // Apply search filter (including UserID)
                if (!string.IsNullOrWhiteSpace(search))
                    query += " AND (CAST(L.UserID AS VARCHAR) LIKE @Search OR U.Username LIKE @Search OR L.Action LIKE @Search)";

                // Order newest to oldest
                query += " ORDER BY L.Timestamp DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (fromDate.HasValue && toDate.HasValue)
                    {
                        cmd.Parameters.AddWithValue("@FromDate", fromDate.Value.Date);
                        cmd.Parameters.AddWithValue("@ToDate", toDate.Value.Date.AddDays(1).AddSeconds(-1));
                    }

                    if (!string.IsNullOrWhiteSpace(search))
                        cmd.Parameters.AddWithValue("@Search", "%" + search + "%");

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvLogTrail.DataSource = dt;

                    // Hide unnecessary column
                    if (dgvLogTrail.Columns.Contains("LogID"))
                        dgvLogTrail.Columns["LogID"].Visible = false;

                    // Adjust column sizing
                    if (dgvLogTrail.Columns.Contains("Details"))
                        dgvLogTrail.Columns["Details"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                    if (dgvLogTrail.Columns.Contains("Timestamp"))
                        dgvLogTrail.Columns["Timestamp"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                    // Explicitly sort in descending order (just to reinforce)
                    if (dgvLogTrail.Columns.Contains("Timestamp"))
                        dgvLogTrail.Sort(dgvLogTrail.Columns["Timestamp"], ListSortDirection.Descending);
                }
            }
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            LoadLogs(tbSearch.Text, dtp_from.Value, dtp_to.Value);
        }

        private void dtp_from_ValueChanged(object sender, EventArgs e)
        {
            LoadLogs(tbSearch.Text, dtp_from.Value, dtp_to.Value);
        }

        private void dtp_to_ValueChanged(object sender, EventArgs e)
        {
            LoadLogs(tbSearch.Text, dtp_from.Value, dtp_to.Value);
        }
    }
}
