using LibraryDashboard.LoginRegister;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using LibraryEntityForms.CodeFirst.Context;
using System.IO;

namespace LibraryDashboard.Navigation
{
    public class TopNavigation : Panel
    {
        private UserData ud;
        private dynamic data;

        private Button userButton;
        private PictureBox profilePicture;
        private Label userNameLabel;
        private PictureBox arrowPictureBox;

        public TopNavigation(Form1 parentForm, dynamic data)
        {
            ud = new UserData(parentForm, data, this);
            this.data = data;

            InitializeComponents(parentForm);
        }

        private void InitializeComponents(Form parentForm)
        {
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

            // Load user photo
            byte[] photoData = GetUserPhotoData(data.Id);
            Image profileImage = photoData != null ? ByteArrayToImage(photoData) : GetDefaultProfileImage();

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
                Image = Image.FromFile(GetArrowImagePath(false)),
                SizeMode = PictureBoxSizeMode.Zoom,
                Size = new Size(20, 20),
                Location = new Point(userButton.Width - 30, userButton.Height / 2 - 20 / 2)
            };
            userButton.Controls.Add(arrowPictureBox);

            bool panelVisible = false;
            userButton.Click += (sender, e) =>
            {
                panelVisible = !panelVisible;
                arrowPictureBox.Image = Image.FromFile(GetArrowImagePath(panelVisible));
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
            if (byteArrayIn == null || byteArrayIn.Length == 0)
            {
                return GetDefaultProfileImage();
            }

            using (var ms = new MemoryStream(byteArrayIn))
            {
                try
                {
                    return Image.FromStream(ms);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine("Invalid image data: " + ex.Message);
                    return GetDefaultProfileImage();
                }
            }
        }

        private Image GetDefaultProfileImage()
        {
            return Image.FromFile("C:\\Users\\LENOVO\\Desktop\\LibraryEntityForms\\LibraryDashboard\\icon\\profileImage.jpg");
        }

        private string GetArrowImagePath(bool isPanelVisible)
        {
            return isPanelVisible ?
                "C:\\Users\\LENOVO\\Desktop\\LibraryEntityForms\\LibraryDashboard\\icon\\arrow_up.png" :
                "C:\\Users\\LENOVO\\Desktop\\LibraryEntityForms\\LibraryDashboard\\icon\\arrow_down.png";
        }

        public void UpdateProfilePicture(byte[] photoData)
        {
            Image profileImage = photoData != null ? ByteArrayToImage(photoData) : GetDefaultProfileImage();
            profilePicture.Image = ResizeImage(profileImage, new Size(60, 60));
        }

        private Image ResizeImage(Image imgToResize, Size size)
        {
            return new Bitmap(imgToResize, size);
        }
    }
}
