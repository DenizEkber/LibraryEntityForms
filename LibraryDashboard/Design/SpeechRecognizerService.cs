using LibraryDashboard.LoginRegister;
using System;
using System.Speech.Recognition;
using System.Speech.Synthesis;

namespace LibraryDashboard.Helpers
{
    internal class SpeechRecognizerService
    {
        private SpeechRecognitionEngine recognizer;
        private SpeechSynthesizer synthesizer;
        private bool isListeningForCommand;
        private Form1 parentForm;
        private Login login;
        private Register register;

        public SpeechRecognizerService(Form1 parentForm, Login login, Register register)
        {
            this.parentForm = parentForm;
            this.login = login;
            this.register = register;

            InitializeSpeechRecognition();
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

        private void ShowLoginPanel()
        {
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
