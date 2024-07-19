using LibraryDashboard.LoginRegister;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace LibraryDashboard.Navigation
{
    public class TopNavigation : Panel
    {
        UserData ud;
        dynamic user;
        
        private Button userButton;
        private PictureBox profilePicture;
        private Label userNameLabel;
        private PictureBox arrowPictureBox;

        public TopNavigation(Form parentForm,dynamic user)
        {
            ud=new UserData(parentForm,user);
            
            this.user = user;
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

            profilePicture = new PictureBox
            {
                Image = ResizeImage(Image.FromFile("C:\\Users\\LENOVO\\Desktop\\LibraryEntityForms\\LibraryDashboard\\icon\\profileImage.jpg"), new Size(60, 60)),
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
                Text = "Kullanıcı Adı",
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
                if (panelVisible == true)
                {
                    ud.Visible = true;
                }
                else
                {
                    ud.Visible = false;
                }

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

        private Image ResizeImage(Image imgToResize, Size size)
        {
            return new Bitmap(imgToResize, size);
        }
    }
}
