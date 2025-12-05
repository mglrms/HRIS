using System.Drawing;
using System.Windows.Forms;

namespace HRISCDBS
{
    internal static class dgvFormat
    {
        // Enable double buffering to reduce flicker
        public static void EnableDoubleBuffer(DataGridView dgv)
        {
            dgv.GetType().GetProperty("DoubleBuffered",
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.NonPublic)
               ?.SetValue(dgv, true, null);
        }

        // Apply uniform header style with adjustable height
        public static void ApplyHeader(DataGridView dgv, Color back, Color fore, int headerHeight = 36)
        {
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = back;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = fore;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 8, FontStyle.Bold);
            dgv.ColumnHeadersHeight = headerHeight;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        }

        // Apply common row/cell style
        public static void ApplyCommon(DataGridView dgv)
        {
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
            dgv.GridColor = Color.Gainsboro;
            dgv.RowHeadersVisible = false;
            dgv.RowTemplate.Height = 34;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 8F, FontStyle.Regular);
            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.DefaultCellStyle.ForeColor = Color.Black;
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 240, 255);
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;

            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
        }

        // Full preset: Master table style
        public static void ApplyMaster(DataGridView dgv, int headerHeight = 36)
        {
            EnableDoubleBuffer(dgv);
            ApplyHeader(dgv, Color.FromArgb(0, 32, 96), Color.White, headerHeight); // deep navy header
            ApplyCommon(dgv);
        }

        // Full preset: Detail/checklist table style with adjustable row & header height
        public static void ApplyDetail(DataGridView dgv, int rowHeight = 40, int headerHeight = 60)
        {
            EnableDoubleBuffer(dgv);
            ApplyHeader(dgv, Color.FromArgb(0, 32, 96), Color.White, headerHeight);
            ApplyCommon(dgv);

            // Uniform row height
            dgv.RowTemplate.Height = rowHeight;

            // Disable user modifications
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeRows = false;
            dgv.AllowUserToResizeColumns = false;
            dgv.AllowUserToOrderColumns = false;
            dgv.RowHeadersVisible = false;

            // Default alignment
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }
    }
}
