using LibraryDashboard.Helpers;
using LibraryEntityForms.CodeFirst.Context;
using System;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LibraryDashboard.LoginRegister
{
    internal class Login : Panel
    {
        private TextBox txtUserEmail;
        private TextBox txtPassword;
        private Button btnLogin;
        private Label lblMessage;
        private Form1 parentForm;


        public Login(Form1 parentForm)
        {
            this.parentForm = parentForm;
            this.Size = new Size(1920, 1075);
            this.Location = new Point(0, 0);
            this.BackColor = Color.White;

            parentForm.Controls.Add(this);
            InitializeDashboard();
        }

        private void InitializeDashboard()
        {
            Panel panel = PanelHelper.CreatePanel(new Size(400, 250), new Padding(0), Color.White, new Padding(0));
            panel.Location = new Point((this.Width - panel.Width) / 2, (this.Height - panel.Height) / 2);

            Label lblUserName = PanelHelper.CreateLabel("Username:", new Font("Segoe UI", 12F), Color.Black, new Point(20, 30));
            panel.Controls.Add(lblUserName);

            txtUserEmail = PanelHelper.CreateTextBox(new Point(120, 30), new Size(240, 30), new Font("Segoe UI", 12F));
            txtUserEmail.KeyDown += TxtUserEmail_KeyDown;
            panel.Controls.Add(txtUserEmail);

            Label lblPassword = PanelHelper.CreateLabel("Password:", new Font("Segoe UI", 12F), Color.Black, new Point(20, 80));
            panel.Controls.Add(lblPassword);

            txtPassword = PanelHelper.CreateTextBox(new Point(120, 80), new Size(240, 30), new Font("Segoe UI", 12F), true);
            txtPassword.KeyDown += TxtPassword_KeyDown;
            panel.Controls.Add(txtPassword);

            btnLogin = PanelHelper.CreateButton("Login", new Point(130, 130), new Size(140, 40), Color.FromArgb(0, 122, 204), Color.White, new Font("Segoe UI", 12F, FontStyle.Bold), BtnLogin_Click);
            panel.Controls.Add(btnLogin);

            Button btnBack = PanelHelper.CreateButton("Back", new Point(20, 20), new Size(150, 40), Color.FromArgb(200, 0, 0), Color.White, new Font("Segoe UI", 12F, FontStyle.Bold), BtnBack_Click);
            this.Controls.Add(btnBack);

            lblMessage = PanelHelper.CreateLabel(string.Empty, new Font("Segoe UI", 10F), Color.Black, new Point(20, 180));
            panel.Controls.Add(lblMessage);

            this.Controls.Add(panel);
        }

        private void TxtUserEmail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtPassword.Focus(); 
                e.SuppressKeyPress = true; 
            }
        }

        private void TxtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtnLogin_Click(sender, e); 
            }
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

        private async void BtnLogin_Click(object sender, EventArgs e)
        {
            var userEmail = txtUserEmail.Text.Trim();
            var password = txtPassword.Text.Trim();

            var user = await Task.Run(() => GetUserData(userEmail, password));

            if (user != null)
            {
                lblMessage.Text = "Login successful!";
                lblMessage.ForeColor = Color.Green;

                this.Visible = false;
                var data = new
                {
                    user.Id,
                    user.Role,
                    user.Email,
                    user.LastName,
                    user.FirstName,
                    user.UserName
                };
                Form loginForm = this.FindForm();
                if (loginForm != null)
                {
                    loginForm.Controls.Remove(this);
                    this.Dispose();
                }
                parentForm.LoadDashboard(data);
            }
            else
            {
                lblMessage.Text = "Invalid username or password.";
                lblMessage.ForeColor = Color.Red;
            }
        }

        private dynamic GetUserData(string userEmail, string password)
        {
            using (var ctx = new LibraryContext())
            {
                var user = (from u in ctx.Users
                            join ud in ctx.UserDetail on u.Id equals ud.Id_User
                            where ud.Email == userEmail
                            select new
                            {
                                u.Id,
                                u.Name,
                                u.Password,
                                u.PasswordSalt,
                                u.Role,
                                ud.Email,
                                ud.FirstName,
                                ud.LastName
                            }).FirstOrDefault();

                if (user != null && VerifyPassword(password, user.Password, user.PasswordSalt))
                {
                    return new
                    {
                        user.Id,
                        UserName = user.Name,
                        Role = user.Role,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName
                    };
                }

                return null;
            }
        }

        private bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt)
        {
            using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(enteredPassword, Convert.FromBase64String(storedSalt), 10000))
            {
                string hash = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(20));
                return hash == storedHash;
            }
        }
    }
}
