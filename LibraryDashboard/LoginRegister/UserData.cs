using LibraryDashboard.Design;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace LibraryDashboard.LoginRegister
{
    internal class UserData : Panel
    {
        private PanelCreated pc;
        private dynamic user;
        private Label lblUserName;
        private Label lblFullName;
        private Label lblEmail;
        private Label lblRole;

        public UserData(Form parentForm, dynamic data)
        {
            this.user = data;
            pc = new PanelCreated();
            this.Size = new Size(400, 400);
            this.Location = new Point(1000, 150);
            this.BackColor = Color.FromArgb(242, 242, 242); 
            this.BorderStyle = BorderStyle.FixedSingle;
            this.Visible = false;
            this.Region = System.Drawing.Region.FromHrgn(RoundCorner.CreateRoundRectRgn(0, 0, 400, 400, 15, 15));

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

            
            /*var btnEdit = new Button
            {
                Text = "Edit",
                Location = new Point(20, 220),
                Size = new Size(120, 40),
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnEdit.FlatAppearance.BorderSize = 0;
            btnEdit.Click += BtnEdit_Click;
            containerPanel.Controls.Add(btnEdit);*/

            
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

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            
            MessageBox.Show("Edit functionality not implemented yet.");
        }
    }
}
