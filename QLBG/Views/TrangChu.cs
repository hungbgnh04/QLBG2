using Guna.Charts.WinForms;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QLBG.DAL;
using QLBG.Helpers;
using System.Collections.Generic;
using System.IO;

namespace QLBG.Views
{
    public partial class TrangChu : UserControl
    {
        private readonly HoaDonBanDAL hoaDonBanDAL;

        public TrangChu()
        {
            InitializeComponent();
            hoaDonBanDAL = new HoaDonBanDAL();
        }

        public void HomePage_Load(object sender, EventArgs e)
        {
            // Hiển thị thông tin cá nhân từ Session.MaNV
            LoadEmployeeInfo();

            // Cập nhật dữ liệu thống kê và biểu đồ
            UpdateStatistics();
            LoadChartData();
        }

        private void LoadEmployeeInfo()
        {
            DatabaseHelper dbHelper = new DatabaseHelper();
            DataRow employee = dbHelper.GetEmployeeByMaNV(Session.MaNV);
            if (employee != null)
            {
                // Hiển thị tên, mã và công việc
                NameLb.Text = employee["TenNV"].ToString();
                IDLb.Text = employee["MaNV"].ToString();
                JobLb.Text = employee["TenCV"].ToString();

                // Tải ảnh nhân viên
                string imageName = employee["Anh"].ToString();
                string projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
                string imageDirectory = Path.Combine(projectDirectory, "Resources", "EmployeeImages");
                string imagePath = Path.Combine(imageDirectory, imageName);

                // Kiểm tra nếu ảnh nhân viên tồn tại
                if (!string.IsNullOrEmpty(imageName) && File.Exists(imagePath))
                {
                    UserIcon.Image = Image.FromFile(imagePath);
                }
                else
                {
                    // Sử dụng ảnh mặc định nếu không tìm thấy ảnh nhân viên
                    string defaultImagePath = Path.Combine(imageDirectory, "ic_user.png");
                    if (File.Exists(defaultImagePath))
                    {
                        UserIcon.Image = Image.FromFile(defaultImagePath);
                    }
                    else
                    {
                        // Nếu ảnh mặc định không tồn tại, sử dụng một hình ảnh tích hợp sẵn
                        UserIcon.Image = Properties.Resources.eye; // Thay 'eye' bằng ảnh tích hợp khác nếu cần
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

                // Tạo dataset cho từng loại sản phẩm với màu ngẫu nhiên
                var datasetsByProductType = new Dictionary<string, GunaBarDataset>();
                Random random = new Random();

                foreach (DataRow row in salesData.Rows)
                {
                    string productType = row["ProductType"].ToString();
                    string month = "Tháng " + row["Month"].ToString();
                    double revenue = Convert.ToDouble(row["Revenue"]);

                    // Nếu chưa có dataset cho loại sản phẩm, tạo mới và thêm vào chart với màu ngẫu nhiên
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

                    // Thêm doanh thu vào dataset tương ứng
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
