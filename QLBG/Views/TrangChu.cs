using Guna.Charts.WinForms;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QLBG.DAL;
using QLBG.Helpers;
using System.Collections.Generic;
using System.IO;
using QLBG.Views.NhanVien;
namespace QLBG.Views
{
    public partial class TrangChu : UserControl
    {
        private readonly HoaDonBanDAL hoaDonBanDAL;

        public TrangChu()
        {
            InitializeComponent();
            hoaDonBanDAL = new HoaDonBanDAL();
            ChiTietNhanVien.HomePageUpdated += (s, args) => HomePage_Load(s, args);
        }

        public void HomePage_Load(object sender, EventArgs e)
        {
            LoadEmployeeInfo();
            UpdateStatistics();
            LoadChartData();

        }

        private void LoadEmployeeInfo()
        {
            DatabaseHelper dbHelper = new DatabaseHelper();
            DataRow employee = dbHelper.GetEmployeeByMaNV(Session.MaNV);
            if (employee != null)
            {
                NameLb.Text = employee["TenNV"].ToString();
                IDLb.Text = employee["MaNV"].ToString();
                JobLb.Text = employee["TenCV"].ToString();

                string imageName = employee["Anh"].ToString();
                string projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
                string imageDirectory = Path.Combine(projectDirectory, "Resources", "EmployeeImages");
                string imagePath = Path.Combine(imageDirectory, imageName);

                if (!string.IsNullOrEmpty(imageName) && File.Exists(imagePath))
                {
                    UserIcon.Image = Image.FromFile(imagePath);
                }
                else
                {
                    string defaultImagePath = Path.Combine(imageDirectory, "ic_user.png");
                    if (File.Exists(defaultImagePath))
                    {
                        UserIcon.Image = Image.FromFile(defaultImagePath);
                    }
                    else
                    {
                        UserIcon.Image = Properties.Resources.eye;
                    }
                }
            }
            else
            {
                MessageBox.Show("Không tìm thấy thông tin nhân viên!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateStatistics()
        {
            int currentMonth = DateTime.Now.Month;
            int currentYear = DateTime.Now.Year;

            decimal monthlyRevenue = hoaDonBanDAL.GetMonthlyRevenue(currentMonth, currentYear);
            MonthInvoiceLb.Text = monthlyRevenue.ToString("N0") + " VND";

            int invoiceCountToday = hoaDonBanDAL.GetInvoiceCountToday();
            BillTodayLb.Text = invoiceCountToday.ToString();

            int totalProductInStock = hoaDonBanDAL.GetTotalProductCountInStock();
            ProductLb.Text = totalProductInStock.ToString();
        }

        private void LoadChartData()
        {
            DataTable salesData = hoaDonBanDAL.GetMonthlySalesByProductType();

            if (salesData != null)
            {
                OverallChart.Datasets.Clear();

                var datasetsByProductType = new Dictionary<string, GunaBarDataset>();
                Random random = new Random();

                foreach (DataRow row in salesData.Rows)
                {
                    string productType = row["ProductType"].ToString();
                    string month = "Tháng " + row["Month"].ToString();

                    double revenue = 0.0;
                    if (row["Revenue"] != DBNull.Value)
                    {
                        revenue = Convert.ToDouble(row["Revenue"]);
                    }
                    else
                    {
                        Console.WriteLine($"Warning: Revenue is NULL for ProductType '{productType}' in {month}.");
                    }

                    if (!datasetsByProductType.ContainsKey(productType))
                    {
                        var randomColor = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
                        var dataset = new GunaBarDataset
                        {
                            Label = productType,
                            FillColors = new ColorCollection { randomColor }
                        };
                        datasetsByProductType[productType] = dataset;
                        OverallChart.Datasets.Add(dataset);
                    }

                    datasetsByProductType[productType].DataPoints.Add(month, revenue);
                }

                OverallChart.Update();
            }
            else
            {
                MessageBox.Show("Không có dữ liệu doanh thu để hiển thị!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
