using LibraryDashboard.Design;
using LibraryDashboard.Navigation;
using LibraryEntityForms.CodeFirst.Context;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace LibraryDashboard.LoginRegister
{
    internal class UserData : Panel
    {
        private dynamic user;
        private Label lblUserName;
        private Label lblFullName;
        private Label lblEmail;
        private Label lblRole;
        private PictureBox profilePicture;
        private TopNavigation topNavigation;

        public UserData(Form parentForm, dynamic data, TopNavigation topNavigation)
        {
            this.user = data;
            this.Size = new Size(400, 400);
            this.Location = new Point(1000, 150);
            this.BackColor = Color.FromArgb(242, 242, 242);
            this.BorderStyle = BorderStyle.FixedSingle;
            this.Visible = false;
            this.Region = System.Drawing.Region.FromHrgn(RoundCorner.CreateRoundRectRgn(0, 0, 400, 400, 15, 15));
            this.topNavigation = topNavigation;
            parentForm.Controls.Add(this);
            InitializeDashboard();
            this.BringToFront();
        }

        private void InitializeDashboard()
        {
            var containerPanel = new Panel
            {
                Size = new Size(360, 340),
                Location = new Point(20, 20),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(20)
            };
            containerPanel.Region = System.Drawing.Region.FromHrgn(RoundCorner.CreateRoundRectRgn(0, 0, 360, 340, 15, 15));

            var titleLabel = new Label
            {
                Text = "User Profile",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 122, 204),
                Location = new Point(20, 20),
                AutoSize = true
            };
            containerPanel.Controls.Add(titleLabel);

            lblUserName = CreateLabel($"Username: {user.UserName}", new Point(20, 60));
            lblFullName = CreateLabel($"Full Name: {user.FirstName} {user.LastName}", new Point(20, 100));
            lblEmail = CreateLabel($"Email: {user.Email}", new Point(20, 140));
            lblRole = CreateLabel($"Role: {user.Role}", new Point(20, 180));

            containerPanel.Controls.Add(lblUserName);
            containerPanel.Controls.Add(lblFullName);
            containerPanel.Controls.Add(lblEmail);
            containerPanel.Controls.Add(lblRole);

            // Fotoğraf yükle
            byte[] photoData = GetUserPhotoData(user.Id);
            Image profileImage = photoData != null ? ByteArrayToImage(photoData) : Image.FromFile("C:\\Users\\LENOVO\\Desktop\\LibraryEntityForms\\LibraryDashboard\\icon\\profileImage.jpg");

            profilePicture = new PictureBox
            {
                Image = ResizeImage(profileImage, new Size(100, 100)),
                SizeMode = PictureBoxSizeMode.Zoom,
                Size = new Size(100, 100),
                Location = new Point(20, 220),
                Cursor = Cursors.Hand
            };
            profilePicture.Paint += (sender, e) =>
            {
                GraphicsPath path = new GraphicsPath();
                path.AddEllipse(0, 0, profilePicture.Width - 1, profilePicture.Height - 1);
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.DrawEllipse(Pens.Transparent, 0, 0, profilePicture.Width - 1, profilePicture.Height - 1);
                profilePicture.Region = new Region(path);
            };
            profilePicture.Click += ProfilePicture_Click;
            containerPanel.Controls.Add(profilePicture);

            this.Controls.Add(containerPanel);
        }

        private Label CreateLabel(string text, Point location)
        {
            return new Label
            {
                Text = text,
                Location = location,
                Font = new Font("Segoe UI", 14, FontStyle.Regular),
                ForeColor = Color.FromArgb(64, 64, 64),
                AutoSize = true
            };
        }

        private void ProfilePicture_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;",
                Title = "Fotoğraf Seçin"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                byte[] photoData = File.ReadAllBytes(openFileDialog.FileName);
                SaveUserPhotoData(user.Id, photoData);

                // Fotoğrafı güncelleyin
                profilePicture.Image = ResizeImage(ByteArrayToImage(photoData), new Size(100, 100));
                topNavigation.UpdateProfilePicture(photoData);
            }
        }

        private byte[] GetUserPhotoData(int userId)
        {
            using (var context = new LibraryContext())
            {
                var userDetail = context.UserDetail.FirstOrDefault(ud => ud.Id == userId);
                return userDetail?.PhotoData;
            }
        }

        private void SaveUserPhotoData(int userId, byte[] photoData)
        {
            using (var context = new LibraryContext())
            {
                var userDetail = context.UserDetail.FirstOrDefault(ud => ud.Id_User == userId);
                if (userDetail != null)
                {
                    userDetail.PhotoData = photoData;
                    context.SaveChanges();
                }
            }
        }

        private Image ByteArrayToImage(byte[] byteArrayIn)
        {
            using (var ms = new MemoryStream(byteArrayIn))
            {
                if (ms == null || ms.Length == 0)
                {
                    // Varsayılan bir resim döndür
                    return Image.FromFile("C:\\Users\\LENOVO\\Desktop\\LibraryEntityForms\\LibraryDashboard\\icon\\profileImage.jpg");
                }

                ms.Position = 0;

                try
                {
                    return Image.FromStream(ms);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine("Görüntü verisi geçerli değil: " + ex.Message);
                    return Image.FromFile("path_to_default_image.jpg"); // Varsayılan resim
                }
            }
        }

        private Image ResizeImage(Image imgToResize, Size size)
        {
            return new Bitmap(imgToResize, size);
        }
    }
}
