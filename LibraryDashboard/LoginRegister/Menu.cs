using LibraryDashboard.Design;
using LibraryDashboard.Helpers;
using Microsoft.VisualBasic.Logging;
using Microsoft.Win32;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Windows.Forms;

namespace LibraryDashboard.LoginRegister
{
    internal class Menu : Panel
    {
        private Button btnLogin;
        private Button btnRegister;
        private SpeechRecognizerService speechRecognizerService;
        private Form1 parentForm;

        private SpeechRecognitionEngine recognizer;
        private SpeechSynthesizer synthesizer;
        private bool isListeningForCommand;
        private Login login;
        private Register register;

        public Menu(Form1 parentForm)
        {
            parentForm.Text = "Library System";
            this.parentForm = parentForm;
            this.Size = new Size(1920, 1075);
            this.Location = new Point(0, 0);
            this.BackColor = Color.Transparent;
            parentForm.Controls.Add(this);
            ShowDashboard();
            //this.Resize += new EventHandler(Menu_Resize);
            //this.Anchor = (/*/AnchorStyles.Bottom | AnchorStyles.Right*/  AnchorStyles.Top | AnchorStyles.Right);
            //this.Dock = DockStyle.Fill;
            this.Paint += new PaintEventHandler(Menu_Paint);
        }

        private void Menu_Paint(object sender, PaintEventArgs e)
        {
            // Gradyan renklerini belirleyin
            Color color1 = ColorTranslator.FromHtml("#0095FF");
            Color color2 = ColorTranslator.FromHtml("#07E098");

            // Gradyan fırçası oluşturun
            using (LinearGradientBrush brush = new LinearGradientBrush(this.ClientRectangle, color1, color2, 90F))
            {
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }
        }
        private void Menu_Resize(object sender, EventArgs e)
        {
            // PictureBox yeniden boyutlandırma
            foreach (Control control in this.Controls)
            {
                if (control is PictureBox pictureBox)
                {
                    pictureBox.Size = new Size(this.ClientSize.Width / 4, this.ClientSize.Height / 4);
                    pictureBox.Location = new Point((this.ClientSize.Width - pictureBox.Width) / 2, this.ClientSize.Height / 3);
                }
                else if (control is Label label)
                {
                    if (label.Text == "EagleVision")
                    {
                        label.Font = new Font("Segoe UI", this.ClientSize.Width / 20, FontStyle.Regular);
                        label.Location = new Point((this.ClientSize.Width - label.Width) / 2, (this.ClientSize.Height - 950) / 2);
                    }
                    else if (label.Text == "Don't mess with simple people. Eagles fly high.")
                    {
                        label.Font = new Font("Segoe UI", this.ClientSize.Width / 50, FontStyle.Regular);
                        label.Location = new Point((this.ClientSize.Width - label.Width) / 2, (this.ClientSize.Height - 600) / 2);
                    }
                }
                else if (control is Panel mainPanel)
                {
                    mainPanel.Size = new Size(this.ClientSize.Width / 4, this.ClientSize.Height / 6);
                    mainPanel.Location = new Point((this.Width - mainPanel.Width) / 2, (this.Height + 150) / 2);

                    // İçindeki butonları yeniden boyutlandırma ve konumlandırma
                    foreach (Control panelControl in mainPanel.Controls)
                    {
                        if (panelControl is Button button)
                        {
                            button.Size = new Size(mainPanel.Width / 3, mainPanel.Height / 3);
                            button.Font = new Font("Segoe UI", mainPanel.Width / 25, FontStyle.Regular);
                            button.Left = (mainPanel.Width - button.Width) / 2;
                            if (button == btnLogin)
                            {
                                button.Top = mainPanel.Height / 5;
                            }
                            else if (button == btnRegister)
                            {
                                button.Top = (mainPanel.Height / 5) * 3;
                            }
                        }
                    }
                }
            }
        }

        public void ShowDashboard()
        {
            // Creating a central panel
            //Environment.UserName
            var picture = PanelHelper.CreatePictureBox(null, "C:\\Users\\LENOVO\\Desktop\\LibraryEntityForms\\LibraryDashboard\\icon\\eaglevision.png", new Size(300, 300), new Point(795, 300));
            picture.BorderStyle = BorderStyle.None;

            var topLabel = PanelHelper.CreateLabel("EagleVision", new Font("Segoe UI", 70, FontStyle.Regular), Color.Black, new Point((this.ClientSize.Width - 650) / 2, (this.ClientSize.Height - 950) / 2), true);
            var coolLabel = PanelHelper.CreateLabel("Don't mess with simple people. Eagles fly high.", new Font("Segoe UI", 20, FontStyle.Regular), Color.Black, new Point((this.ClientSize.Width - 730) / 2, (this.ClientSize.Height - 600) / 2), true);
            
            var mainPanel = PanelHelper.CreatePanel(new Size(500, 300), new Padding(0), ColorTranslator.FromHtml("#07E098"), new Padding(0));
            mainPanel.BorderStyle = BorderStyle.None;
            mainPanel.BackColor = Color.Transparent;
            mainPanel.Location = new Point((this.Width - mainPanel.Width) / 2, (this.Height + 150) / 2);

            
            btnLogin = new RoundedButton
            {
                Text = "Login",
                TextAlign = ContentAlignment.MiddleRight,
                ImageAlign = ContentAlignment.MiddleLeft,
                Size = new Size(150, 60),
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                ForeColor = Color.Gray,
                BackColor = Color.White,
                Padding = new Padding(20, 0, 20, 0),
                Margin = new Padding(0, 10, 0, 10),
                Top = 100,
                Left = 300
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Click += BtnLogin_Click;
            mainPanel.Controls.Add(btnLogin);

            btnRegister = new RoundedButton
            {
                Text = "Register",
                TextAlign = ContentAlignment.MiddleRight,
                ImageAlign = ContentAlignment.MiddleLeft,
                Size = new Size(150, 60),
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                ForeColor = Color.Gray,
                BackColor = Color.White,
                Padding = new Padding(20, 0, 20, 0),
                Margin = new Padding(0, 10, 0, 10),
                Top = 100,
                Left = 50
            };
            btnRegister.FlatAppearance.BorderSize = 0;
            btnRegister.Click += BtnRegister_Click;
            mainPanel.Controls.Add(btnRegister);

            this.Controls.Add(picture);
            this.Controls.Add(coolLabel);
            this.Controls.Add(topLabel);
            this.Controls.Add(mainPanel);

            InitializeSpeechRecognition();
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            var login = new Login(parentForm);
            login.BringToFront();
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            var register = new Register(parentForm);
            register.BringToFront();
        }

        /*private void InitializeSpeechRecognition()
        {
            speechRecognizerService = new SpeechRecognizerService(parentForm, new Login(parentForm), new Register(parentForm));
        }*/
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
            else
            {
                switch (e.Result.Text)
                {
                    case "login":
                        ShowLoginPanel();
                        break;
                    case "register":
                        ShowRegisterPanel();
                        break;
                    case "Back to login":
                        HideLogin();
                        break;
                    case "Back to register":
                        HideRegister();
                        break;
                    default:
                        synthesizer.SpeakAsync("I didn't understand that command.");
                        break;
                }

                isListeningForCommand = false;
            }
        }

        private void ShowLoginPanel()
        {
            login = new Login(parentForm);
            if (login != null)
            {
                login.BringToFront();
            }
        }

        private void HideLogin()
        {
            if (login != null)
            {
                login.Visible = false;
            }
        }

        private void ShowRegisterPanel()
        {
            register = new Register(parentForm);
            if (register != null)
            {
                register.BringToFront();
            }
        }

        private void HideRegister()
        {
            if (register != null)
            {
                register.Visible = false;
            }
        }
    }
}
