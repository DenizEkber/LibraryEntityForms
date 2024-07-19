using LibraryDashboard.Design;
using LibraryEntityForms.CodeFirst.Context;
using LibraryEntityForms.CodeFirst.Entity.UserData;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LibraryDashboard.LoginRegister
{
    internal class Register : Panel
    {
        private TextBox txtUserName;
        private TextBox txtPassword;
        private TextBox txtEmail;
        private TextBox txtFirstName;
        private TextBox txtLastName;
        private ComboBox cboRole;
        private Label lblMessage;
        private Form1 parentForm;

        public Register(Form1 parentForm)
        {
            this.parentForm = parentForm;
            this.Size = new Size(1920, 1075);
            this.Location = new Point(0, 0);
            this.BackColor = Color.Black;

            parentForm.Controls.Add(this);
            InitializeDashboard();
            this.BringToFront(); 
        }

        private void InitializeDashboard()
        {
            var panel = new Panel
            {
                Size = new Size(400, 400),
                Location = new Point((this.Width - 400) / 2, (this.Height - 400) / 2),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            panel.Region = System.Drawing.Region.FromHrgn(RoundCorner.CreateRoundRectRgn(0, 0, 400, 400, 15, 15));

            
            Label lblUserName = new Label
            {
                Text = "Username:",
                Location = new Point(20, 30),
                AutoSize = true,
                Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point),
                ForeColor = Color.Black
            };
            panel.Controls.Add(lblUserName);

            
            txtUserName = new TextBox
            {
                Location = new Point(120, 30),
                Size = new Size(240, 30),
                Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point)
            };
            panel.Controls.Add(txtUserName);

            
            Label lblPassword = new Label
            {
                Text = "Password:",
                Location = new Point(20, 70),
                AutoSize = true,
                Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point),
                ForeColor = Color.Black
            };
            panel.Controls.Add(lblPassword);

            
            txtPassword = new TextBox
            {
                Location = new Point(120, 70),
                Size = new Size(240, 30),
                UseSystemPasswordChar = true,
                Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point)
            };
            panel.Controls.Add(txtPassword);

            
            Label lblEmail = new Label
            {
                Text = "Email:",
                Location = new Point(20, 110),
                AutoSize = true,
                Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point),
                ForeColor = Color.Black
            };
            panel.Controls.Add(lblEmail);

            
            txtEmail = new TextBox
            {
                Location = new Point(120, 110),
                Size = new Size(240, 30),
                Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point)
            };
            panel.Controls.Add(txtEmail);

            
            Label lblFirstName = new Label
            {
                Text = "First Name:",
                Location = new Point(20, 150),
                AutoSize = true,
                Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point),
                ForeColor = Color.Black
            };
            panel.Controls.Add(lblFirstName);

            
            txtFirstName = new TextBox
            {
                Location = new Point(120, 150),
                Size = new Size(240, 30),
                Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point)
            };
            panel.Controls.Add(txtFirstName);

            
            Label lblLastName = new Label
            {
                Text = "Last Name:",
                Location = new Point(20, 190),
                AutoSize = true,
                Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point),
                ForeColor = Color.Black
            };
            panel.Controls.Add(lblLastName);

            
            txtLastName = new TextBox
            {
                Location = new Point(120, 190),
                Size = new Size(240, 30),
                Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point)
            };
            panel.Controls.Add(txtLastName);

            
            Label lblRole = new Label
            {
                Text = "Role:",
                Location = new Point(20, 230),
                AutoSize = true,
                Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point),
                ForeColor = Color.Black
            };
            panel.Controls.Add(lblRole);

            
            cboRole = new ComboBox
            {
                Location = new Point(120, 230),
                Size = new Size(240, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point)
            };
            cboRole.Items.AddRange(new object[] { "Student", "Teacher" });
            cboRole.SelectedIndex = 0; 
            panel.Controls.Add(cboRole);

            
            Button btnRegister = new RoundedButton
            {
                Text = "Register",
                TextAlign = ContentAlignment.MiddleRight,
                ImageAlign = ContentAlignment.MiddleLeft,
                Size = new Size(150, 60),
                Font = new System.Drawing.Font("Segoe UI", 12, FontStyle.Regular),
                ForeColor = Color.White,
                BackColor = Color.Black, 
                Padding = new Padding(20, 0, 20, 0),
                Margin = new Padding(0, 10, 0, 10),
                Top = 277, 
                Left = 120 
            };
            btnRegister.FlatAppearance.BorderSize = 0;
            btnRegister.Click += BtnRegister_Click;
            panel.Controls.Add(btnRegister);


             Button btnBack = new RoundedButton
            {
                Text = "Back",
                Location = new Point(20, 20),
                Size = new Size(150, 40),
                BackColor = Color.FromArgb(200, 0, 0),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point),
                FlatStyle = FlatStyle.Flat
            };
            btnBack.FlatAppearance.BorderSize = 0;
            btnBack.Click += BtnBack_Click;
            this.Controls.Add(btnBack);

            
            lblMessage = new Label
            {
                Location = new Point(20, 320),
                AutoSize = true,
                Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point)
            };
            panel.Controls.Add(lblMessage);

            this.Controls.Add(panel);
        }
        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Visible = false; 

            
            var menuPanel = parentForm.Controls.OfType<Menu>().FirstOrDefault();
            if (menuPanel != null)
            {
                menuPanel.Visible = true;
            }
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            string userName = txtUserName.Text;
            string password = txtPassword.Text;
            string email = txtEmail.Text;
            string firstName = txtFirstName.Text;
            string lastName = txtLastName.Text;
            string selectedRole = cboRole.SelectedItem.ToString();

            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(firstName) ||
                string.IsNullOrWhiteSpace(lastName))
            {
                lblMessage.Text = "All fields are required.";
                lblMessage.ForeColor = Color.Red;
                return;
            }

            
            Role role;
            switch (selectedRole)
            {
                case "Student":
                    role = Role.Student;
                    break;
                case "Teacher":
                    role = Role.Teacher;
                    break;
                default:
                    lblMessage.Text = "Invalid role selected.";
                    lblMessage.ForeColor = Color.Red;
                    return;
            }

            using (var ctx = new LibraryContext())
            {
                
                if (ctx.UserDetail.Any(u => u.Email == email))
                {
                    lblMessage.Text = "Email already exists.";
                    lblMessage.ForeColor = Color.Red;
                    return;
                }

                
                var user = new Users
                {
                    Name = userName,
                    Password = password,
                    Role = role,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now
                };

                ctx.Users.Add(user);
                ctx.SaveChanges();

                
                var userDetail = new UserDetail
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    Id_User = user.Id, 
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now
                };

                ctx.UserDetail.Add(userDetail);
                ctx.SaveChanges();

                lblMessage.Text = "Registration successful!";
                lblMessage.ForeColor = Color.Green;

                
                this.Visible = false;
            }
        }
    }
}
