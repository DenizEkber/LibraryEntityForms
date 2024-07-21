using LibraryDashboard.Design;
using LibraryDashboard.Navigation;
using LibraryEntityForms.CodeFirst.Context;
using LibraryEntityForms.CodeFirst.Entity.UserData;
using System;
using System.Drawing;
using System.Linq;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LibraryDashboard.LoginRegister
{
    internal class Login : Panel
    {
        PanelCreated pc;
        private TextBox txtUserName;
        private TextBox txtPassword;
        private Button btnLogin;
        private Label lblMessage;
        private Form1 parentForm;
        private SpeechRecognitionEngine recognizer;
        private SpeechSynthesizer synthesizer;
        private bool isListeningForCommand;
        private bool isSettingUsername;
        private bool isSettingPassword;

        public Login(Form1 parentForm)
        {
            this.parentForm = parentForm;
            this.Size = new Size(1920, 1075);
            this.Location = new Point(0, 0);
            this.BackColor = Color.White;

            parentForm.Controls.Add(this);
            InitializeDashboard();
            //InitializeSpeechRecognition();
        }

        private void InitializeDashboard()
        {
            pc = new PanelCreated();

            Panel panel = pc.CreatePanel(new Point(0, 0), new Size(400, 250), Color.White);
            panel.Region = System.Drawing.Region.FromHrgn(RoundCorner.CreateRoundRectRgn(0, 0, 400, 250, 15, 15));
            panel.Location = new Point((this.Width - panel.Width) / 2, (this.Height - panel.Height) / 2);
            panel.BorderStyle = BorderStyle.FixedSingle;

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
                Location = new Point(20, 80),
                AutoSize = true,
                Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point),
                ForeColor = Color.Black
            };
            panel.Controls.Add(lblPassword);

            txtPassword = new TextBox
            {
                Location = new Point(120, 80),
                Size = new Size(240, 30),
                UseSystemPasswordChar = true,
                Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point)
            };
            panel.Controls.Add(txtPassword);

            btnLogin = new Button
            {
                Text = "Login",
                Location = new Point(130, 130),
                Size = new Size(140, 40),
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point),
                FlatStyle = FlatStyle.Flat
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Click += BtnLogin_Click;
            panel.Controls.Add(btnLogin);

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
                Location = new Point(20, 180),
                AutoSize = true,
                Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point)
            };
            panel.Controls.Add(lblMessage);

            this.Controls.Add(panel);
        }

        private void InitializeSpeechRecognition()
        {
            recognizer = new SpeechRecognitionEngine();
            synthesizer = new SpeechSynthesizer();


            var triggerCommands = new Choices();
            triggerCommands.Add("Hey Jarvis");

            var grammarTrigger = new Grammar(new GrammarBuilder(triggerCommands));
            recognizer.LoadGrammar(grammarTrigger);

            var commands = new Choices();
            commands.Add("login");
            commands.Add("register");
            commands.Add("Back to login");
            commands.Add("Back to register");

            var grammarBuilder = new GrammarBuilder();
            grammarBuilder.Append(commands);

            var grammarCommands = new Grammar(grammarBuilder);
            recognizer.LoadGrammar(grammarCommands);

            recognizer.SpeechRecognized += Recognizer_SpeechRecognized;
            recognizer.SetInputToDefaultAudioDevice();
            recognizer.RecognizeAsync(RecognizeMode.Multiple);

            isListeningForCommand = false;
        }

        private void Recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (!isListeningForCommand)
            {
                if (e.Result.Text == "Hey Jarvis")
                {
                    synthesizer.SpeakAsync("Yes sir.");
                    isListeningForCommand = true;
                }
            }
            else if (isSettingUsername)
            {
                txtUserName.Text = e.Result.Text;
                isSettingUsername = false;
                lblMessage.Text = "Username set.";
                lblMessage.ForeColor = Color.Green;
                isListeningForCommand = false;
            }
            else if (isSettingPassword)
            {
                txtPassword.Text = e.Result.Text;
                isSettingPassword = false;
                lblMessage.Text = "Password set.";
                lblMessage.ForeColor = Color.Green;
                isListeningForCommand = false;
            }
            else
            {
                switch (e.Result.Text)
                {
                    case "set username":
                        isSettingUsername = true;
                        lblMessage.Text = "Please say the username.";
                        lblMessage.ForeColor = Color.Blue;
                        break;
                    case "set password":
                        isSettingPassword = true;
                        lblMessage.Text = "Please say the password.";
                        lblMessage.ForeColor = Color.Blue;
                        break;
                    case "login":
                        BtnLogin_Click(btnLogin, EventArgs.Empty);
                        break;
                }
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
            var userName = txtUserName.Text;
            var password = txtPassword.Text;

            var user = await Task.Run(() => GetUserData(userName, password));

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

        private dynamic GetUserData(string userName, string password)
        {
            using (var ctx = new LibraryContext())
            {
                var data = (from user in ctx.Users
                            join userDetail in ctx.UserDetail on user.Id equals userDetail.Id_User
                            where user.Name == userName && user.Password == password
                            select new
                            {
                                Id = user.Id,
                                UserName = user.Name,
                                Password = user.Password,
                                Role = user.Role,
                                Email = userDetail.Email,
                                FirstName = userDetail.FirstName,
                                LastName = userDetail.LastName
                            }).FirstOrDefault();
                return data;
            }
        }
    }
}
