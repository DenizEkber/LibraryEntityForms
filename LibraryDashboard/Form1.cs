using System;
using System.Speech.Recognition;
using System.Windows.Forms;
using LibraryDashboard.LoginRegister;
using LibraryDashboard.Navigation;
using LibraryEntityForms.CodeFirst.Entity.UserData;
using System.Speech.Recognition;
using System.Speech.Synthesis;

namespace LibraryDashboard
{
    public partial class Form1 : Form
    {
        private bool isLogin;
        private SpeechRecognitionEngine recognizer;
        private SpeechSynthesizer synthesizer;
        private bool isListeningForCommand;
        public Form1()
        {
            this.isLogin=isLogin;
            InitializeComponent();
            

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            InitializeApplication();
            //InitializeSpeechRecognition();

        }

        private void InitializeApplication()
        {

            if(isLogin)
            {

                /*TopNavigation topNavigation = new TopNavigation(this);


                LeftNavigation leftNavigation = new LeftNavigation(this);*/
                  // LoadDashboard(Role.Admin);
            }
            else
            {
                Menu menu=new Menu(this);

                //Login loginPanel = new Login(this);
            }
            

            
            //DataViewPanel dataViewPanel = new DataViewPanel(this);
        }

        public void LoadDashboard(dynamic user)
        {
            this.Controls.Clear();
            TopNavigation topNavigation = new TopNavigation(this, user);
            LeftNavigation leftNavigation = new LeftNavigation(this,user.Role);
        }

        /*private void InitializeSpeechRecognition()
        {
            recognizer = new SpeechRecognitionEngine();
            synthesizer = new SpeechSynthesizer();

            // Tetikleyici kelime ve komutlar için dil modelleri oluşturun
            var triggerCommands = new Choices();
            triggerCommands.Add("Hey Jarvis");

            var grammarTrigger = new Grammar(new GrammarBuilder(triggerCommands));
            recognizer.LoadGrammar(grammarTrigger);

            var commands = new Choices();
            commands.Add("open the login");
            commands.Add("open the register");
            commands.Add("show profile");

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
                    case "open the login":
                        ShowLoginPanel();
                        break;
                    case "open the register":
                        ShowRegisterPanel();
                        break;
                    case "show profile":
                        ShowUserProfile();
                        break;
                    default:
                        synthesizer.SpeakAsync("I didn't understand that command.");
                        break;
                }

                isListeningForCommand = false; // Bir komut alındıktan sonra tekrar tetikleyici kelimeyi bekleyin
            }
        }
        private void ShowLoginPanel()
        {
            Login login =new Login(this);
        }

        private void ShowRegisterPanel()
        {
            Register register =new Register(this);
        }

        private void ShowUserProfile()
        {
            // Kullanıcı profilini gösteren kodu buraya ekleyin
        }*/
    }
}
