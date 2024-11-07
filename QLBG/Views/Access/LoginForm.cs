using QLBG.Helpers;
using QLBG.DAL;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.SqlServer.Server;
using System.Data;

namespace QLBG.Views.Access
{
    public partial class LoginForm : Form
    {
        private string emailToChangePass = "";
        private int stateOTPKind = 0;
        private int countDownOTPSecond = 60;
        private Thread thread;
        private string OTPCode;
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;
        private DatabaseHelper dbHelper;

        public LoginForm()
        {
            InitializeComponent();
            lackPassword.Visible = false;
            lackUsername.Visible = false;
            incorrectInfo.Visible = false;
            lackOTPLabel.Visible = false;
            wrongOTPLabel.Visible = false;
            createNewPassPanel.Visible = false;
            OTPVerifyPanel.Visible = false;
            pnlForgotPassword.Visible = false;
            emailNotFound.Visible = false;
            emailNotType.Visible = false;

            panel1.MouseDown += DraggablePanel_MouseDown;
            panel1.MouseMove += DraggablePanel_MouseMove;
            panel1.MouseUp += DraggablePanel_MouseUp;

            dbHelper = new DatabaseHelper();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            RoundCorners(this, 60);
            RoundCorners(btnUpdatePass, 60);
        }

        private void RoundCorners(Control control, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.StartFigure();
            path.AddArc(new Rectangle(0, 0, radius, radius), 180, 90);
            path.AddArc(new Rectangle(control.Width - radius, 0, radius, radius), 270, 90);
            path.AddArc(new Rectangle(control.Width - radius, control.Height - radius, radius, radius), 0, 90);
            path.AddArc(new Rectangle(0, control.Height - radius, radius, radius), 90, 90);
            path.CloseFigure();
            control.Region = new Region(path);
        }

        private void initThread()
        {
            thread = new Thread(countDown);
            thread.IsBackground = true;
            thread.Start();
        }

        private void countDown()
        {
            int count = countDownOTPSecond;
            this.Invoke(new Action(() =>
            {
                btnOTP.Enabled = false;
                btnOTP.Visible = false;
                btnCountDown.Visible = true;
                btnCountDown.Location = btnOTP.Location;
            }));
            while (count > -1)
            {
                this.Invoke(new Action(() =>
                {
                    btnCountDown.Text = count.ToString();
                }));
                count--;
                Thread.Sleep(1000);
            }
            this.Invoke(new Action(() =>
            {
                btnOTP.Enabled = true;
                btnOTP.Visible = true;
                btnCountDown.Visible = false;
                OTPCode = "";
            }));
        }

        private string randDomOTP()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        private async Task<bool> SendEmailAsync(string toEmail, string otp)
        {
            if (!App_Default.ValidateEmailConfig())
            {
                MessageBox.Show("Email configuration is invalid.", "Configuration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            try
            {
                MailMessage mail = new MailMessage
                {
                    From = new MailAddress(App_Default.SenderEmail),
                    Subject = "Your OTP Code",
                    Body = $"Your OTP Code is: {otp}"
                };
                mail.To.Add(toEmail);

                using (SmtpClient smtpServer = new SmtpClient(App_Default.SmtpHost))
                {
                    smtpServer.Port = App_Default.SmtpPort;
                    smtpServer.Credentials = new System.Net.NetworkCredential(App_Default.SenderEmail, App_Default.SenderPassword);
                    smtpServer.EnableSsl = true;
                    await smtpServer.SendMailAsync(mail);
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to send OTP email: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void DraggablePanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                dragging = true;
                dragCursorPoint = Cursor.Position;
                dragFormPoint = this.Location;
                this.Opacity = 0.5;
            }
        }

        private void DraggablePanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }

        private void DraggablePanel_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
            this.Opacity = 1;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private async void button1_Click(object sender, EventArgs e) // btnLogin_Click
        {
            // Kiểm tra các trường trống
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                lackUsername.Visible = true;
            }
            else
            {
                lackUsername.Visible = false;
            }

            if (string.IsNullOrEmpty(textBox2.Text))
            {
                lackPassword.Visible = true;
            }
            else
            {
                lackPassword.Visible = false;
            }

            if (!lackUsername.Visible && !lackPassword.Visible)
            {
                bool isValid = dbHelper.CheckLogin(textBox1.Text, textBox2.Text);

                if (isValid)
                {
                    // Lấy thông tin nhân viên đăng nhập
                    DataRow employee = dbHelper.GetEmployeeByEmail(textBox1.Text);
                    if (employee != null)
                    {
                        int maNV = Convert.ToInt32(employee["MaNV"]);
                        bool quyenAdmin = Convert.ToBoolean(employee["QuyenAdmin"]);

                        // Lưu thông tin vào Session
                        Session.MaNV = maNV;
                        Session.QuyenAdmin = quyenAdmin;

                        incorrectInfo.Visible = false;
                        this.Hide();

                        // Mở form chính sau khi đăng nhập thành công
                        frmLayout frmLayout = new frmLayout();
                        frmLayout.Show();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy thông tin nhân viên.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    incorrectInfo.Visible = true;
                }
            }
        }


        private void button1_MouseEnter(object sender, EventArgs e) // btnLogin_MouseEnter
        {
            btnLogin.ForeColor = Color.Black;
        }

        private void button1_MouseHover(object sender, EventArgs e) // btnLogin_MouseHover
        {
            btnLogin.ForeColor = Color.Black;
        }

        private void button1_MouseLeave(object sender, EventArgs e) // btnLogin_MouseLeave
        {
            btnLogin.ForeColor = Color.Lime;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_MouseHover(object sender, EventArgs e) // Close button MouseHover
        {
            button2.BackColor = Color.Red;
        }

        private void button2_MouseLeave(object sender, EventArgs e) // Close button MouseLeave
        {
            button2.BackColor = panel1.BackColor;
        }

        private void button2_MouseMove(object sender, MouseEventArgs e) // Close button MouseMove
        {
            button2.BackColor = Color.Red;
        }

        private void button2_Click(object sender, EventArgs e) // Close button Click
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e) // Minimize button Click
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void label12_Click(object sender, EventArgs e)
        {
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {
        }

        private void initOTPPanelSetting(string noticeText)
        {
            OTPVerifyPanel.Visible = true;
            OTPVerifyPanel.Dock = DockStyle.Left;
            label7.AutoSize = false;
            label7.TextAlign = ContentAlignment.MiddleCenter;
            label7.Dock = DockStyle.Fill;
            label7.Text = noticeText;
            initThread();
        }

        private void signUpLabel_MouseHover(object sender, EventArgs e)
        {
            signUpLabel.ForeColor = Color.Lime;
        }

        private void signUpLabel_MouseLeave(object sender, EventArgs e)
        {
            signUpLabel.ForeColor = Color.White;
        }

        private void signUpLabel_MouseMove(object sender, EventArgs e)
        {
            signUpLabel.ForeColor = Color.Lime;
        }

        private void signUpLabel_MouseHover_1(object sender, EventArgs e)
        {
            signUpLabel.ForeColor = Color.Lime;
        }

        private void signUpLabel_MouseLeave_1(object sender, EventArgs e)
        {
            signUpLabel.ForeColor = Color.White;
        }

        private void signUpLabel_MouseMove(object sender, MouseEventArgs e)
        {
            signUpLabel.ForeColor = Color.Lime;
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void signUpPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {
            pnlForgotPassword.Visible = true;
            panel2.Dock = DockStyle.Right;
            pnlForgotPassword.Dock = DockStyle.Left;
        }

        private void label8_MouseHover(object sender, EventArgs e)
        {
            label8.ForeColor = Color.Lime;
        }

        private void label8_MouseLeave(object sender, EventArgs e)
        {
            label8.ForeColor = Color.White;
        }

        private void label8_MouseMove(object sender, MouseEventArgs e)
        {
            label8.ForeColor = Color.Lime;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click_1(object sender, EventArgs e)
        {
        }

        private void label16_MouseHover(object sender, EventArgs e)
        {
            label16.ForeColor = Color.Lime;
        }

        private void label16_MouseLeave(object sender, EventArgs e)
        {
            label16.ForeColor = Color.White;
        }

        private void label16_MouseMove(object sender, MouseEventArgs e)
        {
            label16.ForeColor = Color.Lime;
        }

        private void label16_Click(object sender, EventArgs e)
        {
            try
            {
                if (thread != null && thread.IsAlive)
                {
                    thread.Abort();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error stopping thread: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            OTPVerifyPanel.Visible = false;
            pnlForgotPassword.Visible = true;
            pnlForgotPassword.Dock = DockStyle.Left;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void NoLabel_Click(object sender, EventArgs e)
        {

        }

        private void label26_Click(object sender, EventArgs e)
        {
            panel2.Dock = DockStyle.Left;
            pnlForgotPassword.Visible = false;
        }

        private async void btnGetPass_Click(object sender, EventArgs e)
        {
            // Kiểm tra nếu email chưa được nhập
            if (string.IsNullOrEmpty(textBox7.Text))
            {
                emailNotType.Visible = true;
                return;
            }
            else
            {
                emailNotType.Visible = false;
            }

            // Kiểm tra nếu email tồn tại trong cơ sở dữ liệu
            bool emailExists = dbHelper.CheckEmailExist(textBox7.Text);
            if (!emailExists)
            {
                emailNotFound.Visible = true;
                return;
            }
            else
            {
                emailNotFound.Visible = false;
            }

            // Tạo OTP và gửi email
            OTPCode = randDomOTP();
            bool isSent = await SendEmailAsync(textBox7.Text, OTPCode); // Gọi hàm gửi email

            if (isSent)
            {
                pnlForgotPassword.Visible = false;
                string noticeText = $"We have just sent the OTP code to email address {textBox7.Text}, please enter the code to change new password.";
                initOTPPanelSetting(noticeText);
                stateOTPKind = 1;
                emailToChangePass = textBox7.Text;
            }
            else
            {
                MessageBox.Show("Failed to send OTP email. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGetPass_MouseHover(object sender, EventArgs e)
        {
            btnGetPass.ForeColor = Color.Black;
        }

        private void btnGetPass_MouseLeave(object sender, EventArgs e)
        {
            btnGetPass.ForeColor = Color.Lime;
        }

        private void btnGetPass_MouseMove(object sender, MouseEventArgs e)
        {
            btnGetPass.ForeColor = Color.Black;
        }

        private void label19_Click(object sender, EventArgs e)
        {
        }

        private void btnUpdatePass_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox11.Text))
            {
                lackNewPassLabel.Visible = true;
                return;
            }
            else
            {
                lackNewPassLabel.Visible = false;
            }

            if (string.IsNullOrEmpty(textBox8.Text))
            {
                lackNewPassAgainLabel.Visible = true;
                return;
            }
            else
            {
                lackNewPassAgainLabel.Visible = false;
            }

            if (textBox8.Text != textBox11.Text)
            {
                newPassNotMatchLabel.Visible = true;
                return;
            }
            else
            {
                newPassNotMatchLabel.Visible = false;
            }

            dbHelper.UpdatePassword(emailToChangePass, textBox8.Text);
            createNewPassPanel.Visible = false;
            panel2.Dock = DockStyle.Left;

            // Bạn có thể mở form thông báo thành công hoặc thực hiện hành động khác ở đây
            // Ví dụ:
            // MessageBox.Show("Change password successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void label18_Click(object sender, EventArgs e)
        {
            createNewPassPanel.Visible = false;
            pnlForgotPassword.Visible = true;
            pnlForgotPassword.Dock = DockStyle.Left;
        }

        private void label18_MouseHover(object sender, EventArgs e)
        {
            label18.ForeColor = Color.Lime;
        }

        private void label18_MouseLeave(object sender, EventArgs e)
        {
            label18.ForeColor = Color.White;
        }

        private void label18_MouseMove(object sender, MouseEventArgs e)
        {
            label18.ForeColor = Color.Lime;
        }

        private void btnUpdatePass_MouseHover(object sender, EventArgs e)
        {
            btnUpdatePass.ForeColor = Color.Black;
        }

        private void btnUpdatePass_MouseLeave(object sender, EventArgs e)
        {
            btnUpdatePass.ForeColor = Color.Lime;
        }

        private void btnUpdatePass_MouseMove(object sender, MouseEventArgs e)
        {
            btnUpdatePass.ForeColor = Color.Black;
        }

        private void label26_MouseHover(object sender, EventArgs e)
        {
            label26.ForeColor = Color.Lime;
        }

        private void label26_MouseLeave(object sender, EventArgs e)
        {
            label26.ForeColor = Color.White;
        }

        private void label26_MouseMove(object sender, MouseEventArgs e)
        {
            label26.ForeColor = Color.Lime;
        }

        private void label28_Click(object sender, EventArgs e)
        {

        }

        private void createNewPassPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void btnOTP_Click(object sender, EventArgs e)
        {
            if (stateOTPKind == 1)
            {
                string noticeText = $"We have just sent the OTP code to email address {textBox7.Text}, please enter the code to change new password.";
                initOTPPanelSetting(noticeText);
                stateOTPKind = 1;
                emailToChangePass = textBox7.Text;
            }
            else if (stateOTPKind == 2)
            {
                OTPCode = randDomOTP();
                bool isSent = SendEmailAsync(emailToChangePass, OTPCode).Result;
                if (isSent)
                {
                    string noticeText = $"We have just sent the OTP code to email address {emailToChangePass}, please enter the code to verify.";
                    initOTPPanelSetting(noticeText);
                    stateOTPKind = 2;
                }
                else
                {
                    MessageBox.Show("Failed to send OTP email. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnAcceptCheckOTP_MouseHover(object sender, EventArgs e)
        {
            btnAcceptCheckOTP.ForeColor = System.Drawing.Color.Black;
        }

        private void btnAcceptCheckOTP_MouseLeave(object sender, EventArgs e)
        {
            btnAcceptCheckOTP.ForeColor = System.Drawing.Color.Lime;
        }

        private void btnAcceptCheckOTP_MouseMove(object sender, MouseEventArgs e)
        {
            btnAcceptCheckOTP.ForeColor = System.Drawing.Color.Black;
        }

        private void btnAcceptCheckOTP_Click(object sender, EventArgs e)
        {
            if (ValidateOTPFields())
            {
                OTPVerifyPanel.Visible = false;
                createNewPassPanel.Visible = true;
                createNewPassPanel.Dock = DockStyle.Left;
                creatNewPassLabel.Text = "Create new password for account " + emailToChangePass;
                creatNewPassLabel.AutoSize = false;
                creatNewPassLabel.TextAlign = ContentAlignment.MiddleCenter;
            }
            else
            {
                wrongOTPLabel.Visible = true;
            }
        }

        private bool ValidateOTPFields()
        {
            return textBox10.Text == OTPCode;
        }

    }
}
