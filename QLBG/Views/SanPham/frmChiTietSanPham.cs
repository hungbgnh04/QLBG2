using Guna.UI2.WinForms;
using QLBG.DAL;
using QLBG.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLBG.Views.SanPham
{
    public partial class frmChiTietSanPham : Form
    {
        private ProductDAL productDAL = new ProductDAL();
        private LoaiDAL loaiDAL = new LoaiDAL();
        private KichThuocDAL kichThuocDAL = new KichThuocDAL();
        private HinhDangDAL hinhDangDAL = new HinhDangDAL();
        private ChatLieuDAL chatLieuDAL = new ChatLieuDAL();
        private NhaSanXuatDAL nhaSanXuatDAL = new NhaSanXuatDAL();
        private CongDungDAL congDungDAL = new CongDungDAL();
        private DacDiemDAL dacDiemDAL = new DacDiemDAL();
        private MauSacDAL mauSacDAL = new MauSacDAL();
        private NuocSXDAL nuocSXDAL = new NuocSXDAL();

        private string imageDirectory;


        public enum Mode
        {
            View,
            Edit,
            Add
        }

        private int id;
        public Mode mode;

        public frmChiTietSanPham(int id, Mode mode)
        {
            InitializeComponent();
            this.id = id;
            this.mode = mode;
            string projectDirectory = Directory.GetParent(Application.StartupPath).Parent.FullName;
            imageDirectory = Path.Combine(projectDirectory, @"Resources\ProductImages");
            Init();
            LoadLoaiToComboBox();
            LoadKichThuocToComboBox();
            LoadHinhDangToComboBox();
            LoadChatLieuToComboBox();
            LoadNhaSanXuatToComboBox();
            LoadCongDungToComboBox();
            LoadDacDiemToComboBox();
            LoadMauSacToComboBox();
            LoadNuocSXToComboBox();
            LoadCongDungToComboBox();
            LoadData();

        }

        private void Init()
        {
            switch (mode)
            {
                case Mode.Edit:
                    HeaderLb.Text = "SỬA SẢN PHẨM";
                    SuaPanel.BringToFront();
                    for (int i = 0; i < ThongTinPanel.Controls.Count; i++)
                    {
                        if (ThongTinPanel.Controls[i] is Guna2TextBox)
                        {
                            ((Guna2TextBox)ThongTinPanel.Controls[i]).Enabled = true;
                            ((Guna2TextBox)ThongTinPanel.Controls[i]).BorderThickness = 1;
                        }
                        if (ThongTinPanel.Controls[i] is Guna2ComboBox)
                        {
                            ((Guna2ComboBox)ThongTinPanel.Controls[i]).Enabled = true;
                            ((Guna2ComboBox)ThongTinPanel.Controls[i]).BorderThickness = 1;
                        }
                    }
                    PictureBoxAnh.Enabled = true;
                    lbMa.Visible = false;
                    txtMa.Visible = false;
                    lbSoLuong.Visible = false;
                    txtSoLuong.Visible = false;
                    SuaPanel.Location = ViewPanel.Location;
                    break;
                case Mode.Add:
                    HeaderLb.Text = "THÊM SẢN PHẨM";
                    ThemPanel.BringToFront();
                    for (int i = 0; i < ThongTinPanel.Controls.Count; i++)
                    {
                        if (ThongTinPanel.Controls[i] is Guna2TextBox)
                        {
                            ((Guna2TextBox)ThongTinPanel.Controls[i]).Enabled = true;
                            ((Guna2TextBox)ThongTinPanel.Controls[i]).BorderThickness = 1;
                            ((Guna2TextBox)ThongTinPanel.Controls[i]).Text = "";
                        }
                        if (ThongTinPanel.Controls[i] is Guna2ComboBox)
                        {
                            ((Guna2ComboBox)ThongTinPanel.Controls[i]).Enabled = true;
                            ((Guna2ComboBox)ThongTinPanel.Controls[i]).BorderThickness = 1;
                            ((Guna2ComboBox)ThongTinPanel.Controls[i]).SelectedIndex = -1;
                        }
                    }
                    PictureBoxAnh.Enabled = true;
                    txtMa.Visible = false;
                    lbMa.Visible = false;
                    txtSoLuong.Visible = false;
                    lbSoLuong.Visible = false;
                    ThemPanel.Location = ViewPanel.Location;
                    break;
                default:
                    HeaderLb.Text = "CHI TIẾT SẢN PHẨM";
                    ViewPanel.BringToFront();
                    for (int i = 0; i < ThongTinPanel.Controls.Count; i++)
                    {
                        if (ThongTinPanel.Controls[i] is Guna2TextBox)
                        {
                            ((Guna2TextBox)ThongTinPanel.Controls[i]).Enabled = false;
                            ((Guna2TextBox)ThongTinPanel.Controls[i]).BorderThickness = 0;
                        }
                        if (ThongTinPanel.Controls[i] is Guna2ComboBox)
                        {
                            ((Guna2ComboBox)ThongTinPanel.Controls[i]).Enabled = false;
                            ((Guna2ComboBox)ThongTinPanel.Controls[i]).BorderThickness = 0;
                        }
                    }
                    PictureBoxAnh.Enabled = false;
                    lbMa.Visible = true;
                    txtMa.Visible = true;
                    lbSoLuong.Visible = true;
                    txtSoLuong.Visible = true;
                    break;
            }
        }

        private void LoadData()
        {
            if (mode == Mode.Edit || mode == Mode.View)
            {
                ProductDTO product = productDAL.GetProductById(id);
                if (product != null)
                {
                    txtMa.Text = product.MaHang.ToString();
                    txtTenHangHoa.Text = product.TenHangHoa;
                    comboBoxLoaiSP.SelectedValue = product.MaLoai;
                    comboBoxKichThuoc.SelectedValue = product.MaKichThuoc;
                    comboBoxHinhDang.SelectedValue = product.MaHinhDang;
                    comboBoxChatLieu.SelectedValue = product.MaChatLieu;
                    comboBoxNuocSanXuat.SelectedValue = product.MaNuocSX;
                    comboBoxDacDiem.SelectedValue = product.MaDacDiem;
                    comboBoxMau.SelectedValue = product.MaMau;
                    comboBoxCongDung.SelectedValue = product.MaCongDung;
                    comboBoxNhaSanXuat.SelectedValue = product.MaNSX;
                    txtSoLuong.Text = product.SoLuong.ToString();
                    txtDonGiaNhap.Text = product.DonGiaNhap.ToString();
                    txtDonGiaBan.Text = product.DonGiaBan.ToString();
                    txtThoiGianBaoHanh.Text = product.ThoiGianBaoHanh.ToString();
                    txtGhiChu.Text = product.GhiChu;


                    string imagePath = Path.Combine(imageDirectory, product.Anh);

                    if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
                    {
                        PictureBoxAnh.Image = Image.FromFile(imagePath);
                    }
                    else
                    {
                        PictureBoxAnh.Image = Image.FromFile(imageDirectory + @"\default_product.png");
                    }
                }
            }
        }

        private void LoadMauSacToComboBox()
        {
            List<MauSacDTO> mauSacList = mauSacDAL.GetAllMauSac();
            comboBoxMau.DataSource = mauSacList;
            comboBoxMau.DisplayMember = "TenMau";
            comboBoxMau.ValueMember = "MaMau";

            if (mode == Mode.Edit || mode == Mode.View)
            {
                ProductDTO product = productDAL.GetProductById(id);
                if (product != null)
                {
                    comboBoxMau.SelectedValue = product.MaMau;
                }
            }
        }


        private void LoadLoaiToComboBox()
        {
            List<LoaiDTO> loaiList = loaiDAL.GetAllLoai();
            comboBoxLoaiSP.DataSource = loaiList;
            comboBoxLoaiSP.DisplayMember = "TenLoai";
            comboBoxLoaiSP.ValueMember = "MaLoai";
            if (mode == Mode.Edit || mode == Mode.View)
            {
                ProductDTO product = productDAL.GetProductById(id);
                if (product != null)
                {
                    comboBoxLoaiSP.SelectedValue = product.MaLoai;
                }
            }
        }


        private void LoadNuocSXToComboBox()
        {
            List<NuocSXDTO> nuocSXList = nuocSXDAL.GetAllNuocSX();
            comboBoxNuocSanXuat.DataSource = nuocSXList;
            comboBoxNuocSanXuat.DisplayMember = "TenNuocSX";
            comboBoxNuocSanXuat.ValueMember = "MaNuocSX";

            if (mode == Mode.Edit || mode == Mode.View)
            {
                ProductDTO product = productDAL.GetProductById(id);
                if (product != null)
                {
                    comboBoxNuocSanXuat.SelectedValue = product.MaNuocSX;
                }
            }
        }



        private void LoadKichThuocToComboBox()
        {
            List<KichThuocDTO> kichThuocList = kichThuocDAL.GetAllKichThuoc();
            comboBoxKichThuoc.DataSource = kichThuocList;
            comboBoxKichThuoc.DisplayMember = "TenKichThuoc";
            comboBoxKichThuoc.ValueMember = "MaKichThuoc";
            if (mode == Mode.Edit || mode == Mode.View)
            {
                ProductDTO product = productDAL.GetProductById(id);
                if (product != null)
                {
                    comboBoxKichThuoc.SelectedValue = product.MaKichThuoc;
                }
            }
        }

        private void LoadHinhDangToComboBox()
        {
            List<HinhDangDTO> hinhDangList = hinhDangDAL.GetAllHinhDang();
            comboBoxHinhDang.DataSource = hinhDangList;
            comboBoxHinhDang.DisplayMember = "TenHinhDang";
            comboBoxHinhDang.ValueMember = "MaHinhDang";
            if (mode == Mode.Edit || mode == Mode.View)
            {
                ProductDTO product = productDAL.GetProductById(id);
                if (product != null)
                {
                    comboBoxHinhDang.SelectedValue = product.MaHinhDang;
                }
            }
        }

        private void LoadChatLieuToComboBox()
        {
            List<ChatLieuDTO> chatLieuList = chatLieuDAL.GetAllChatLieu();
            comboBoxChatLieu.DataSource = chatLieuList;
            comboBoxChatLieu.DisplayMember = "TenChatLieu";
            comboBoxChatLieu.ValueMember = "MaChatLieu";

            if (mode == Mode.Edit || mode == Mode.View)
            {
                ProductDTO product = productDAL.GetProductById(id);
                if (product != null)
                {
                    comboBoxChatLieu.SelectedValue = product.MaChatLieu;
                }
            }
        }

        private void LoadNhaSanXuatToComboBox()
        {
            List<NhaSanXuatDTO> nhaSanXuatList = nhaSanXuatDAL.GetAllNhaSanXuat();
            comboBoxNhaSanXuat.DataSource = nhaSanXuatList;
            comboBoxNhaSanXuat.DisplayMember = "TenNSX";
            comboBoxNhaSanXuat.ValueMember = "MaNSX";

            if (mode == Mode.Edit || mode == Mode.View)
            {
                ProductDTO product = productDAL.GetProductById(id);
                if (product != null)
                {
                    comboBoxNhaSanXuat.SelectedValue = product.MaNSX;
                }
            }
        }

        private void LoadCongDungToComboBox()
        {
            List<CongDungDTO> congDungList = congDungDAL.GetAllCongDung();
            comboBoxCongDung.DataSource = congDungList;
            comboBoxCongDung.DisplayMember = "TenCongDung";
            comboBoxCongDung.ValueMember = "MaCongDung";

            if (mode == Mode.Edit || mode == Mode.View)
            {
                ProductDTO product = productDAL.GetProductById(id);
                if (product != null)
                {
                    comboBoxCongDung.SelectedValue = product.MaCongDung;
                }
            }
        }

        private void LoadDacDiemToComboBox()
        {
            List<DacDiemDTO> dacDiemList = dacDiemDAL.GetAllDacDiem();
            comboBoxDacDiem.DataSource = dacDiemList;
            comboBoxDacDiem.DisplayMember = "TenDacDiem";
            comboBoxDacDiem.ValueMember = "MaDacDiem";

            if (mode == Mode.Edit || mode == Mode.View)
            {
                ProductDTO product = productDAL.GetProductById(id);
                if (product != null)
                {
                    comboBoxDacDiem.SelectedValue = product.MaDacDiem;
                }
            }
        }



        private void PictureBoxAnh_Click(object sender, EventArgs e)
        {
            guna2ShadowForm1.SetShadowForm(this);
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                if (openFileDialog.ShowDialog() == DialogResult.OK && !String.IsNullOrEmpty(openFileDialog.FileName))
                {
                    PictureBoxAnh.ImageLocation = openFileDialog.FileName;
                }
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnQuayLai_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            mode = Mode.Edit;
            Init();
        }

        private void HuyBtn_Click(object sender, EventArgs e)
        {
            mode = Mode.View;
            Init();
        }

        private void ThoatBtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void ThemBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Directory.Exists(imageDirectory))
                {
                    Directory.CreateDirectory(imageDirectory);
                }

                string image = Path.GetFileName(PictureBoxAnh.ImageLocation);
                string imagePath = Path.Combine(imageDirectory, image);

                if (!File.Exists(imagePath))
                {
                    File.Copy(PictureBoxAnh.ImageLocation, imagePath);
                }

                int maLoai = (int)comboBoxLoaiSP.SelectedValue;
                int maKichThuoc = (int)comboBoxKichThuoc.SelectedValue;
                int maHinhDang = (int)comboBoxHinhDang.SelectedValue;
                int maChatLieu = (int)comboBoxChatLieu.SelectedValue;
                int maNuocSX = (int)comboBoxNuocSanXuat.SelectedValue;
                int maDacDiem = (int)comboBoxDacDiem.SelectedValue;
                int maMau = (int)comboBoxMau.SelectedValue;
                int maCongDung = (int)comboBoxCongDung.SelectedValue;
                int maNSX = (int)comboBoxNhaSanXuat.SelectedValue;
                int soLuong = 0;
                decimal donGiaNhap = decimal.Parse(txtDonGiaNhap.Text);
                decimal donGiaBan = decimal.Parse(txtDonGiaBan.Text);
                int thoiGianBaoHanh = int.Parse(txtThoiGianBaoHanh.Text);

                ProductDTO product = new ProductDTO
                {
                    MaHang = id,
                    TenHangHoa = txtTenHangHoa.Text,
                    MaLoai = maLoai,
                    MaKichThuoc = maKichThuoc,
                    MaHinhDang = maHinhDang,
                    MaChatLieu = maChatLieu,
                    MaNuocSX = maNuocSX,
                    MaDacDiem = maDacDiem,
                    MaMau = maMau,
                    MaCongDung = maCongDung,
                    MaNSX = maNSX,
                    SoLuong = soLuong,
                    DonGiaNhap = donGiaNhap,
                    DonGiaBan = donGiaBan,
                    ThoiGianBaoHanh = thoiGianBaoHanh,
                    Anh = image,
                    GhiChu = txtGhiChu.Text
                };

                bool isUpdated = productDAL.InsertProduct(product);
                if (isUpdated)
                {
                    MessageBox.Show("Sản phẩm đã được cập nhật thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Cập nhật sản phẩm thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Vui lòng nhập đúng định dạng số cho các trường số lượng, đơn giá và thời gian bảo hành.", "Lỗi định dạng", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void LuuBtn_Click_1(object sender, EventArgs e)
        {
            if (!Directory.Exists(imageDirectory))
            {
                Directory.CreateDirectory(imageDirectory);
            }

            string image = Path.GetFileName(PictureBoxAnh.ImageLocation);

            if (String.IsNullOrEmpty(image))
            {
                MessageBox.Show("Vui lòng chọn ảnh sản phẩm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string imagePath = Path.Combine(imageDirectory, image);

            if (!File.Exists(imagePath))
            {
                File.Copy(PictureBoxAnh.ImageLocation, imagePath);
            }

            ProductDTO product = new ProductDTO
            {
                MaHang = id,
                TenHangHoa = txtTenHangHoa.Text,
                MaLoai = (int)comboBoxLoaiSP.SelectedValue,
                MaKichThuoc = (int)comboBoxKichThuoc.SelectedValue,
                MaHinhDang = (int)comboBoxHinhDang.SelectedValue,
                MaChatLieu = (int)comboBoxChatLieu.SelectedValue,
                MaNuocSX = (int)comboBoxNuocSanXuat.SelectedValue,
                MaDacDiem = (int)comboBoxDacDiem.SelectedValue,
                MaMau = (int)comboBoxMau.SelectedValue,
                MaCongDung = (int)comboBoxCongDung.SelectedValue,
                MaNSX = (int)comboBoxNhaSanXuat.SelectedValue,
                SoLuong = int.Parse(txtSoLuong.Text),
                DonGiaNhap = decimal.Parse(txtDonGiaNhap.Text),
                DonGiaBan = decimal.Parse(txtDonGiaBan.Text),
                ThoiGianBaoHanh = int.Parse(txtThoiGianBaoHanh.Text),
                Anh = image,
                GhiChu = txtGhiChu.Text
            };

            bool isUpdated = productDAL.UpdateProduct(product);
            if (isUpdated)
            {
                MessageBox.Show("Sản phẩm đã được cập nhật thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Cập nhật sản phẩm thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("Bạn có chắc chắn muốn xóa sản phẩm này?",
                                        "Xác nhận xóa",
                                        MessageBoxButtons.YesNo,
                                        MessageBoxIcon.Warning);
            if (confirmResult == DialogResult.Yes)
            {
                bool isDeleted = productDAL.DeleteProduct(id); // 'id' là mã sản phẩm hiện tại

                if (isDeleted)
                {
                    MessageBox.Show("Sản phẩm đã được xóa thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close(); // Đóng form sau khi xóa thành công
                }
                else
                {
                    MessageBox.Show("Xóa sản phẩm thất bại. Vui lòng thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}

