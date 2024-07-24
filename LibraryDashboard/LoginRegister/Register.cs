using LibraryDashboard.Design;
using LibraryDashboard.Helpers;
using LibraryEntityForms.CodeFirst.Context;
using LibraryEntityForms.CodeFirst.Entity.UserData;
using System;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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
            InitializePanel();
            InitializeDashboard();
            this.BringToFront();
        }

        private void InitializePanel()
        {
            this.Size = new Size(1920, 1075);
            this.Location = new Point(0, 0);
            this.BackColor = Color.Black;
            parentForm.Controls.Add(this);
        }

        private void InitializeDashboard()
        {
            var panel = PanelHelper.CreatePanel(new Size(400, 400),
                new Point((this.Width - 400) / 2, (this.Height - 400) / 2), Color.White, BorderStyle.FixedSingle);

            AddControlsToPanel(panel);
            this.Controls.Add(panel);
        }

        private void AddControlsToPanel(Panel panel)
        {
            panel.Controls.Add(PanelHelper.CreateLabel("Username:", new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point), Color.Black, new Point(20, 30), true));
            txtUserName = PanelHelper.CreateTextBox(new Point(130, 30), new Size(240, 30), new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point), false);
            panel.Controls.Add(txtUserName);

            panel.Controls.Add(PanelHelper.CreateLabel("Password:", new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point), Color.Black, new Point(20, 70), true));
            txtPassword = PanelHelper.CreateTextBox(new Point(130, 70), new Size(240, 30), new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point), true);
            panel.Controls.Add(txtPassword);

            panel.Controls.Add(PanelHelper.CreateLabel("Email:", new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point), Color.Black, new Point(20, 110), true));
            txtEmail = PanelHelper.CreateTextBox(new Point(130, 110), new Size(240, 30), new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point), true);
            panel.Controls.Add(txtEmail);

            panel.Controls.Add(PanelHelper.CreateLabel("First Name:", new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point), Color.Black, new Point(20, 150), true));
            txtFirstName = PanelHelper.CreateTextBox(new Point(130, 150), new Size(240, 30), new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point), true);
            panel.Controls.Add(txtFirstName);

            panel.Controls.Add(PanelHelper.CreateLabel("Last Name:", new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point), Color.Black, new Point(20, 190), true));
            txtLastName = PanelHelper.CreateTextBox(new Point(130, 190), new Size(240, 30), new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point), true);
            panel.Controls.Add(txtLastName);

            panel.Controls.Add(PanelHelper.CreateLabel("Role:", new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point), Color.Black, new Point(20, 230), true));
            cboRole = PanelHelper.CreateComboBox(new Point(130, 230), new Size(240, 30),new object[] { "Student", "Teacher"/*, "Admin"*/ }, 0);
            panel.Controls.Add(cboRole);

            panel.Controls.Add(PanelHelper.CreateButton("Register", new Point(130, 277), new Size(150, 60), Color.Black, Color.White, new Font("Segoe UI", 12F, FontStyle.Regular), BtnRegister_Click));

            this.Controls.Add(PanelHelper.CreateButton("Back", new Point(20, 20), new Size(150, 40), Color.FromArgb(200, 0, 0), Color.White, new Font("Segoe UI", 12F, FontStyle.Bold), BtnBack_Click));

            lblMessage = PanelHelper.CreateLabel(string.Empty, new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point), Color.Black, new Point(20, 350), true);
            panel.Controls.Add(lblMessage);
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
            RegisterUser();
        }

        private void RegisterUser()
        {
            string userName = txtUserName.Text;
            string password = txtPassword.Text;
            string email = txtEmail.Text.ToLower();
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

            if (!IsValidEmail(email))
            {
                lblMessage.Text = "Invalid email format.";
                lblMessage.ForeColor = Color.Red;
                return;
            }

            Role? role = GetRoleFromSelection(selectedRole);

            if (role == null)
            {
                lblMessage.Text = "Invalid role selected.";
                lblMessage.ForeColor = Color.Red;
                return;
            }

            // Hash and salt the password
            (string passwordHash, string passwordSalt) = HashPassword(password);

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
                    Password = passwordHash,
                    PasswordSalt = passwordSalt,
                    Role = role.Value,
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
                    UpdatedDate = DateTime.Now,
                    PhotoData = new byte[0]
                };

                ctx.UserDetail.Add(userDetail);
                ctx.SaveChanges();

                lblMessage.Text = "Registration successful!";
                lblMessage.ForeColor = Color.Green;

                CloseForm();
            }
        }

        private Role? GetRoleFromSelection(string selectedRole)
        {
            return selectedRole switch
            {
                "Student" => Role.Student,
                "Teacher" => Role.Teacher,
                /*"Admin" => Role.Admin,*/
                _ => null
            };
        }

        private void CloseForm()
        {
            Form registerForm = this.FindForm();
            if (registerForm != null)
            {
                registerForm.Controls.Remove(this);
                this.Dispose();
            }
            this.Visible = false;
        }

        private (string, string) HashPassword(string password)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] saltBytes = new byte[16];
                rng.GetBytes(saltBytes);
                string salt = Convert.ToBase64String(saltBytes);

                using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltBytes, 10000))
                {
                    byte[] hashBytes = rfc2898DeriveBytes.GetBytes(20);
                    string hash = Convert.ToBase64String(hashBytes);
                    return (hash, salt);
                }
            }
        }
        private bool IsValidEmail(string email)
        {
            return email.Contains("@") && email.Contains(".");
        }
    }
}
