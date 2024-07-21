using LibraryDashboard.LoginRegister;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using LibraryEntityForms.CodeFirst.Context;

namespace LibraryDashboard.Navigation
{
    public class TopNavigation : Panel
    {

        UserData ud;
        dynamic data;
        
        private Button userButton;
        private PictureBox profilePicture;
        private Label userNameLabel;
        private PictureBox arrowPictureBox;

        public TopNavigation(Form parentForm,dynamic data)
        {
            ud=new UserData(parentForm,data, this);

            this.data = data;
            MaximumSize = new Size(1575, 120);
            Height = 120;
            Width = 1575;
            BackColor = Color.White;
            Location = new Point(345, 0);
            parentForm.Controls.Add(this);

            CreateTopLabel();
            CreateUserButton();
        }

        private void CreateTopLabel()
        {
            Label topName = new Label
            {
                Text = "EagleVision",
                AutoSize = true,
                Location = new Point(40, (Height - 40) / 2),
                Font = new Font("Comic Sans MS", 20, FontStyle.Bold)
            };
            Controls.Add(topName);
        }

        private void CreateUserButton()
        {
            userButton = new Button
            {
                Size = new Size(226, 70),
                TextAlign = ContentAlignment.MiddleLeft,
                Location = new Point(800, (Height - 40) / 2),
                TextImageRelation = TextImageRelation.ImageBeforeText
            };

            // Kullanıcı fotoğrafını veritabanından çekin
            byte[] photoData = GetUserPhotoData(data.Id);
            Image profileImage = photoData != null ? ByteArrayToImage(photoData) : Image.FromFile("C:\\Users\\LENOVO\\Desktop\\LibraryEntityForms\\LibraryDashboard\\icon\\profileImage.jpg");

            profilePicture = new PictureBox
            {
                Image = ResizeImage(profileImage, new Size(60, 60)),
                SizeMode = PictureBoxSizeMode.Zoom,
                Size = new Size(60, 60),
                Padding = new Padding(0),
                Location = new Point(5, 5)
            };
            profilePicture.Paint += (sender, e) =>
            {
                GraphicsPath path = new GraphicsPath();
                path.AddEllipse(0, 0, profilePicture.Width - 1, profilePicture.Height - 1);
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.DrawEllipse(Pens.Transparent, 0, 0, profilePicture.Width - 1, profilePicture.Height - 1);
                profilePicture.Region = new Region(path);
            };
            userButton.Controls.Add(profilePicture);

            userNameLabel = new Label
            {
                Text = data.UserName,
                AutoSize = true,
                Location = new Point(profilePicture.Right + 10, userButton.Height / 2 - 20 / 2)
            };
            userButton.Controls.Add(userNameLabel);

            arrowPictureBox = new PictureBox
            {
                Image = Image.FromFile("C:\\Users\\LENOVO\\Desktop\\LibraryEntityForms\\LibraryDashboard\\icon\\arrow_down.png"),
                SizeMode = PictureBoxSizeMode.Zoom,
                Size = new Size(20, 20),
                Location = new Point(userButton.Width - 30, userButton.Height / 2 - 20 / 2)
            };
            userButton.Controls.Add(arrowPictureBox);

            bool panelVisible = false;
            userButton.Click += (sender, e) =>
            {
                panelVisible = !panelVisible;
                arrowPictureBox.Image = Image.FromFile(panelVisible ?
                    "C:\\Users\\LENOVO\\Desktop\\LibraryEntityForms\\LibraryDashboard\\icon\\arrow_up.png" :
                    "C:\\Users\\LENOVO\\Desktop\\LibraryEntityForms\\LibraryDashboard\\icon\\arrow_down.png");
                ud.Visible = panelVisible;
            };

            Controls.Add(userButton);

            void Control_MouseDown(object sender, MouseEventArgs e)
            {
                userButton.PerformClick();
            }

            profilePicture.MouseDown += new MouseEventHandler(Control_MouseDown);
            userNameLabel.MouseDown += new MouseEventHandler(Control_MouseDown);
            arrowPictureBox.MouseDown += new MouseEventHandler(Control_MouseDown);
        }

        private byte[] GetUserPhotoData(int userId)
        {
            using (var context = new LibraryContext())
            {
                var userDetail = context.UserDetail.FirstOrDefault(ud => ud.Id_User == userId);
                return userDetail?.PhotoData;
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

        public void UpdateProfilePicture(byte[] photoData)
        {
            Image profileImage = photoData != null ? ByteArrayToImage(photoData) : Image.FromFile("C:\\Users\\LENOVO\\Desktop\\LibraryEntityForms\\LibraryDashboard\\icon\\profileImage.jpg");

            profilePicture.Image = ResizeImage(profileImage, new Size(60, 60));
        }


        private Image ResizeImage(Image imgToResize, Size size)
        {
            return new Bitmap(imgToResize, size);
        }
    }
}
