namespace QLBG.Views.SanPham
{
    partial class TheSanPham
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TheSanPham));
            this.HangLb = new System.Windows.Forms.Label();
            this.TenLb = new System.Windows.Forms.Label();
            this.PictureBoxAnh = new Guna.UI2.WinForms.Guna2PictureBox();
            this.guna2ShadowPanel1 = new Guna.UI2.WinForms.Guna2ShadowPanel();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxAnh)).BeginInit();
            this.guna2ShadowPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // HangLb
            // 
            this.HangLb.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HangLb.Location = new System.Drawing.Point(15, 327);
            this.HangLb.Name = "HangLb";
            this.HangLb.Size = new System.Drawing.Size(250, 60);
            this.HangLb.TabIndex = 5;
            this.HangLb.Text = "Hòa Phát";
            this.HangLb.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.HangLb.Click += new System.EventHandler(this.TenLb_Click_1);
            // 
            // TenLb
            // 
            this.TenLb.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TenLb.Location = new System.Drawing.Point(15, 260);
            this.TenLb.Margin = new System.Windows.Forms.Padding(20);
            this.TenLb.Name = "TenLb";
            this.TenLb.Size = new System.Drawing.Size(250, 60);
            this.TenLb.TabIndex = 4;
            this.TenLb.Text = "Ghế gỗ vải";
            this.TenLb.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.TenLb.Click += new System.EventHandler(this.TenLb_Click_1);
            // 
            // PictureBoxAnh
            // 
            this.PictureBoxAnh.BackColor = System.Drawing.Color.Transparent;
            this.PictureBoxAnh.Dock = System.Windows.Forms.DockStyle.Top;
            this.PictureBoxAnh.Image = ((System.Drawing.Image)(resources.GetObject("PictureBoxAnh.Image")));
            this.PictureBoxAnh.ImageRotate = 0F;
            this.PictureBoxAnh.Location = new System.Drawing.Point(0, 0);
            this.PictureBoxAnh.Name = "PictureBoxAnh";
            this.PictureBoxAnh.Size = new System.Drawing.Size(280, 250);
            this.PictureBoxAnh.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PictureBoxAnh.TabIndex = 3;
            this.PictureBoxAnh.TabStop = false;
            this.PictureBoxAnh.Click += new System.EventHandler(this.TenLb_Click_1);
            // 
            // guna2ShadowPanel1
            // 
            this.guna2ShadowPanel1.BackColor = System.Drawing.Color.Transparent;
            this.guna2ShadowPanel1.Controls.Add(this.HangLb);
            this.guna2ShadowPanel1.Controls.Add(this.TenLb);
            this.guna2ShadowPanel1.Controls.Add(this.PictureBoxAnh);
            this.guna2ShadowPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2ShadowPanel1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(230)))));
            this.guna2ShadowPanel1.Location = new System.Drawing.Point(0, 0);
            this.guna2ShadowPanel1.Name = "guna2ShadowPanel1";
            this.guna2ShadowPanel1.Radius = 15;
            this.guna2ShadowPanel1.ShadowColor = System.Drawing.Color.Black;
            this.guna2ShadowPanel1.Size = new System.Drawing.Size(280, 400);
            this.guna2ShadowPanel1.TabIndex = 0;
            this.guna2ShadowPanel1.Click += new System.EventHandler(this.TenLb_Click_1);
            this.guna2ShadowPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.guna2ShadowPanel1_Paint);
            // 
            // TheSanPham
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.guna2ShadowPanel1);
            this.Margin = new System.Windows.Forms.Padding(25);
            this.Name = "TheSanPham";
            this.Size = new System.Drawing.Size(280, 400);
            this.Resize += new System.EventHandler(this.TheSanPham_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxAnh)).EndInit();
            this.guna2ShadowPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        public System.Windows.Forms.Label HangLb;
        public System.Windows.Forms.Label TenLb;//
        private Guna.UI2.WinForms.Guna2PictureBox PictureBoxAnh;
        private Guna.UI2.WinForms.Guna2ShadowPanel guna2ShadowPanel1;
    }
}
