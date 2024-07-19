using LibraryDashboard.Design;
using System;
using System.Drawing;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace LibraryDashboard.LoginRegister
{
    internal class Menu : Panel
    {

        private SpeechRecognitionEngine recognizer;
        private SpeechSynthesizer synthesizer;
        private bool isListeningForCommand;
        private Button btnLogin;
        private Button btnRegister;
        private PanelCreated pc;
        private Form1 parentForm;

        public Menu(Form1 parentForm)
        {
            this.parentForm = parentForm;
            this.Size = new Size(1920, 1075);
            this.Location = new Point(0, 0);
            this.BackColor = ColorTranslator.FromHtml("#0095FF");

            parentForm.Controls.Add(this);
            ShowDashboard();
            InitializeSpeechRecognition();
        }
        private System.Drawing.Image ResizeImage(System.Drawing.Image imgToResize, Size size)
        {
            return new Bitmap(imgToResize, size);
        }
        public void ShowDashboard()
        {
            // Creating a central panel
            
            pc = new PanelCreated();
            Label picture = new Label {
                ImageAlign = ContentAlignment.MiddleLeft,
                Size = new Size(300, 200),
                Font = new System.Drawing.Font("Segoe UI", 20, FontStyle.Regular),
                ForeColor = Color.Black,
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Padding = new Padding(20, 0, 20, 0),
                Margin = new Padding(0, 10, 0, 10),
                Top = 300, // Üst konumunu ayarla
                Left = 795 // Sol konumunu ayarla
            };
            picture.Image = ResizeImage(System.Drawing.Image.FromFile("C:\\Users\\LENOVO\\Desktop\\LibraryEntityForms\\LibraryDashboard\\icon\\eaglevision.png"), new Size(300, 300));
            Label topLabel = pc.CreateLabel("EagleVision", new Point((this.Width - 650) / 2, (this.Height - 950) / 2), new System.Drawing.Font("Segoe UI", 70, FontStyle.Regular), ColorTranslator.FromHtml("#0095FF"), new Size(700, 200));
            Label coolLabel = pc.CreateLabel("Don't mess with simple people. Eagles fly high.", new Point((this.Width - 730) / 2, (this.Height - 600) / 2), new System.Drawing.Font("Segoe UI", 20, FontStyle.Regular), ColorTranslator.FromHtml("#0095FF"), new Size(800, 200));
            topLabel.ForeColor = Color.White;
            coolLabel.ForeColor = Color.White;
            Panel mainPanel = pc.CreatePanel(new Point(0, 0), new Size(500, 300), ColorTranslator.FromHtml("#07E098"));
            mainPanel.Region = System.Drawing.Region.FromHrgn(RoundCorner.CreateRoundRectRgn(0, 0, 500, 300, 15, 15));
            mainPanel.Location = new Point((this.Width - mainPanel.Width) / 2, (this.Height ) / 2);
            this.Controls.Add(picture);
            this.Controls.Add(coolLabel);
            this.Controls.Add(topLabel);

            // Styling and positioning Login Button
            Button btnLogin = new RoundedButton
            {
                Text = "Login",
                TextAlign = ContentAlignment.MiddleRight,
                ImageAlign = ContentAlignment.MiddleLeft,
                Size = new Size(150, 60),
                Font = new System.Drawing.Font("Segoe UI", 12, FontStyle.Regular),
                ForeColor = Color.Gray,
                BackColor = Color.White, // Tüm butonların arka plan rengi başlangıçta beyaz
                Padding = new Padding(20, 0, 20, 0),
                Margin = new Padding(0, 10, 0, 10),
                Top = 100, // Üst konumunu ayarla
                Left = 50 // Sol konumunu ayarla
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Click += BtnLogin_Click;
            mainPanel.Controls.Add(btnLogin);

            // Styling and positioning Register Button
            Button btnRegister = new RoundedButton
            {
                Text = "Register",
                TextAlign = ContentAlignment.MiddleRight,
                ImageAlign = ContentAlignment.MiddleLeft,
                Size = new Size(150, 60),
                Font = new System.Drawing.Font("Segoe UI", 12, FontStyle.Regular),
                ForeColor = Color.Gray,
                BackColor = Color.White, // Tüm butonların arka plan rengi başlangıçta beyaz
                Padding = new Padding(20, 0, 20, 0),
                Margin = new Padding(0, 10, 0, 10),
                Top = 100, // Üst konumunu ayarla
                Left = 300 // Sol konumunu ayarla
            };
            btnRegister.FlatAppearance.BorderSize = 0;
            btnRegister.Click += BtnRegister_Click;
            mainPanel.Controls.Add(btnRegister);

            this.Controls.Add(mainPanel);
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            Login loginPanel = new Login(parentForm);
            loginPanel.BringToFront();
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            Register registerPanel = new Register(parentForm);
            registerPanel.BringToFront();
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
        Login login;
        Register register;
        private void ShowLoginPanel()
        {
             login = new Login(parentForm);
            login.BringToFront();
            
        }
        private void HideLogin()
        {
            login.Visible = false;
        }

        private void HideRegister()
        {
            register.Visible = false;
        }

        private void ShowRegisterPanel()
        {
            
             register = new Register(parentForm);
            register.BringToFront();
            
        }

    }
}
