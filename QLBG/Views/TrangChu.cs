using Guna.Charts.WinForms;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DataTable = System.Data.DataTable;

namespace QLBG.Views
{
    public partial class TrangChu : UserControl
    {
        public TrangChu()
        {
            InitializeComponent();
        }

        private void HomePage_Load(object sender, EventArgs e)
        {
            NameLb.Text = "Mai Việt Hùng";
            IDLb.Text = "B1809276";
            JobLb.Text = "Quản lý";

            MonthInvoiceLb.Text = 30000000.ToString("N0") + " VND";
            BillTodayLb.Text = "10";
            ProductLb.Text = "100";

            LoadChartData();
        }

        private void LoadChartData()
        {
            DataTable salesData = GetMonthlySalesData();

            if (salesData != null)
            {
                OverallChart.Datasets.Clear();

                var tableDataset = new Guna.Charts.WinForms.GunaBarDataset
                {
                    Label = "Bàn",
                    DataPoints = new LPointCollection(),
                    FillColors = new ColorCollection { Color.MediumAquamarine }
                };

                var chairDataset = new Guna.Charts.WinForms.GunaBarDataset
                {
                    Label = "Ghế",
                    DataPoints = new LPointCollection(),
                    FillColors = new ColorCollection { Color.MediumVioletRed }
                };

                foreach (DataRow row in salesData.Rows)
                {
                    string month = row["Month"].ToString();
                    double tableSales = Convert.ToDouble(row["TableSales"]);
                    double chairSales = Convert.ToDouble(row["ChairSales"]);

                    tableDataset.DataPoints.Add(month, tableSales);
                    chairDataset.DataPoints.Add(month, chairSales);
                }
                OverallChart.Datasets.Add(tableDataset);
                OverallChart.Datasets.Add(chairDataset);

                OverallChart.Update();
            }
        }

        private DataTable GetMonthlySalesData()
        {
            DataTable table = new DataTable();
            table.Columns.Add("Month", typeof(string));
            table.Columns.Add("TableSales", typeof(double));
            table.Columns.Add("ChairSales", typeof(double));

            Random random = new Random();

            for (int i = 1; i <= 10; i++)
            {
                table.Rows.Add("Tháng " + i, random.Next(5000000, 20000000), random.Next(5000000, 20000000));
            }

            return table;
        }
    }
}
