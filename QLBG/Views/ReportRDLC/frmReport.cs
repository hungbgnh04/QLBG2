using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLBG.Views.ReportRDLC
{
    public partial class frmReport : Form
    {
        public frmReport()
        {
            InitializeComponent();
        }

        private void frmReport_Load(object sender, EventArgs e)
        {

        }

        public void LoadReport(DataTable dataTable, string dataSourceName, string reportFileName)
        {
            string reportPath = Application.StartupPath + @"\Report\" + reportFileName;
            reportViewer1.LocalReport.ReportPath = reportPath;
            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để hiển thị trong báo cáo.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            reportViewer1.LocalReport.DataSources.Clear();
            ReportDataSource reportDataSource = new ReportDataSource(dataSourceName, dataTable);
            reportViewer1.LocalReport.DataSources.Add(reportDataSource);
            reportViewer1.ZoomMode = Microsoft.Reporting.WinForms.ZoomMode.FullPage;
            reportViewer1.RefreshReport();
        }

    }
}
