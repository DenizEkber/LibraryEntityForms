using LibraryDashboard.Design;
using LibraryDashboard.LoginRegister;
using LibraryEntityForms.CodeFirst.Entity.UserData;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using LibraryDashboard.Helpers;

namespace LibraryDashboard.Navigation
{
    public class LeftNavigation : Panel
    {
        private SpeechRecognitionEngine recognizer;
        private SpeechSynthesizer synthesizer;
        private bool isListeningForCommand;
        private dynamic data;
        private int buttonTop = 130;
        private Form1 parentPanel;
        private Button selectedButton = null;
        private List<Button> navigationButtons = new List<Button>();
        private Dashboard dashboardPanel;
        private Books booksPanel;
        private Library libraryPanel;
        private Student studentPanel;
        private Teacher teacherPanel;
        private Author authorPanel;
        private Setting settingPanel;

        public LeftNavigation(Form1 parentForm, dynamic data)
        {
            this.parentPanel = parentForm;
            this.data = data;
            InitializePanel();
            InitializePanels();
            LibraryTop();
            CreateNavigationItems();
            InitializeSpeechRecognition();
            SelectButton(navigationButtons[0]);
        }

        private void InitializePanel()
        {
            MaximumSize = new Size(345, 1195);
            Height = 1195;
            Width = 345;
            BackColor = Color.White;
            Location = new Point(0, 0);
            Padding = new Padding(10);
            parentPanel.Controls.Add(this);
        }

        private void InitializePanels()
        {
            dashboardPanel = new Dashboard(parentPanel) { Name = "Dashboard" };
            booksPanel = new Books(parentPanel) { Name = "Books" };
            libraryPanel = new Library(parentPanel) { Name = "Library" };
            studentPanel = new Student(parentPanel) { Name = "Students" };
            teacherPanel = new Teacher(parentPanel) { Name = "Teacher" };
            authorPanel = new Author(parentPanel) { Name = "Authors" };
            settingPanel = new Setting(parentPanel,data) { Name = "Settings" };
        }

        private void LibraryTop()
        {
            Label label = new Label
            {
                Text = "Library",
                TextAlign = ContentAlignment.MiddleRight,
                ImageAlign = ContentAlignment.MiddleLeft,
                Size = new Size(315, 120),
                Font = new Font("Segoe UI", 20, FontStyle.Regular),
                ForeColor = Color.Black,
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Padding = new Padding(20, 0, 20, 0),
                Margin = new Padding(0, 10, 0, 10),
                Image = PanelHelper.ResizeImage(Image.FromFile("C:\\Users\\LENOVO\\Desktop\\LibraryEntityForms\\LibraryDashboard\\icon\\eaglevision.png"), new Size(50, 50)),
                Top = 0,
                Left = 15
            };
            Controls.Add(label);
        }

        private void CreateNavigationItems()
        {
            AddNavigationItem("Dashboard", "C:\\Users\\LENOVO\\Desktop\\LibraryEntityForms\\LibraryDashboard\\icon\\dashboard.png", "C:\\Users\\LENOVO\\Desktop\\LibraryEntityForms\\LibraryDashboard\\icon\\dashboard-white.png");
            AddNavigationItem("Books", "C:\\Users\\LENOVO\\Desktop\\LibraryEntityForms\\LibraryDashboard\\icon\\book.png", "C:\\Users\\LENOVO\\Desktop\\LibraryEntityForms\\LibraryDashboard\\icon\\book-white.png");
            AddNavigationItem("Library", "C:\\Users\\LENOVO\\Desktop\\LibraryEntityForms\\LibraryDashboard\\icon\\library.png", "C:\\Users\\LENOVO\\Desktop\\LibraryEntityForms\\LibraryDashboard\\icon\\library-white.png");

            if (data.Role == Role.Admin)
            {
                AddNavigationItem("Teacher", "C:\\Users\\LENOVO\\Desktop\\LibraryEntityForms\\LibraryDashboard\\icon\\teacher.png", "C:\\Users\\LENOVO\\Desktop\\LibraryEntityForms\\LibraryDashboard\\icon\\teacher-white.png");
                AddNavigationItem("Students", "C:\\Users\\LENOVO\\Desktop\\LibraryEntityForms\\LibraryDashboard\\icon\\student.png", "C:\\Users\\LENOVO\\Desktop\\LibraryEntityForms\\LibraryDashboard\\icon\\student-white.png");
            }

            AddNavigationItem("Authors", "C:\\Users\\LENOVO\\Desktop\\LibraryEntityForms\\LibraryDashboard\\icon\\author.png", "C:\\Users\\LENOVO\\Desktop\\LibraryEntityForms\\LibraryDashboard\\icon\\author-white.png");
            AddNavigationItem("Settings", "C:\\Users\\LENOVO\\Desktop\\LibraryEntityForms\\LibraryDashboard\\icon\\setting.png", "C:\\Users\\LENOVO\\Desktop\\LibraryEntityForms\\LibraryDashboard\\icon\\setting-white.png");
            AddNavigationItem("Sign Out", "C:\\Users\\LENOVO\\Desktop\\LibraryEntityForms\\LibraryDashboard\\icon\\sign-out.png", "C:\\Users\\LENOVO\\Desktop\\LibraryEntityForms\\LibraryDashboard\\icon\\sign-out-white.png");
        }

        private void AddNavigationItem(string text, string iconPath, string whiteIconPath)
        {
            Button button = new RoundedButton
            {
                Text = text,
                TextAlign = ContentAlignment.MiddleRight,
                ImageAlign = ContentAlignment.MiddleLeft,
                Size = new Size(315, 60),
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                ForeColor = Color.Gray,
                BackColor = Color.White,
                Padding = new Padding(20, 0, 20, 0),
                Margin = new Padding(0, 10, 0, 10),
                Image = PanelHelper.ResizeImage(LoadImage(iconPath), new Size(32, 32)),
                Tag = new string[] { iconPath, whiteIconPath },
                Top = buttonTop,
                Left = 15
            };

            button.Click += (sender, e) => SelectButton((Button)sender);

            Controls.Add(button);
            navigationButtons.Add(button);
            buttonTop += button.Height + 10;
        }

        private void SelectButton(Button button)
        {
            if (selectedButton != null)
            {
                selectedButton.BackColor = Color.White;
                selectedButton.ForeColor = Color.Gray;
                string[] paths = (string[])selectedButton.Tag;
                selectedButton.Image = PanelHelper.ResizeImage(LoadImage(paths[0]), new Size(32, 32));
            }

            button.BackColor = Color.Blue;
            button.ForeColor = Color.White;
            string[] newPaths = (string[])button.Tag;
            button.Image = PanelHelper.ResizeImage(LoadImage(newPaths[1]), new Size(32, 32));

            selectedButton = button;

            ShowPanel(button.Text);

            if (button.Text == "Sign Out")
            {
                ClickSignOut();
            }
        }

        private void ShowPanel(string panelName)
        {
            foreach (var panel in new List<Panel> { dashboardPanel, booksPanel, libraryPanel, teacherPanel, studentPanel, authorPanel, settingPanel})
            {
                
                panel.Visible = panel.Name == panelName;
            }
        }

        private void ClickSignOut()
        {
            Form dashboardForm = this.FindForm();
            if (dashboardForm != null)
            {
                // Remove all controls in the dashboardForm that are of type TopNavigation, Dashboard, or UserData
                foreach (Control control in dashboardForm.Controls.OfType<TopNavigation>().ToList())
                {
                    dashboardForm.Controls.Remove(control);
                    control.Dispose();
                }

                foreach (Control control in dashboardForm.Controls.OfType<Dashboard>().ToList())
                {
                    dashboardForm.Controls.Remove(control);
                    control.Dispose();
                }
                foreach (Control control in dashboardForm.Controls.OfType<Author>().ToList())
                {
                    dashboardForm.Controls.Remove(control);
                    control.Dispose();
                }
                foreach (Control control in dashboardForm.Controls.OfType<Books>().ToList())
                {
                    dashboardForm.Controls.Remove(control);
                    control.Dispose();
                }
                foreach (Control control in dashboardForm.Controls.OfType<Library>().ToList())
                {
                    dashboardForm.Controls.Remove(control);
                    control.Dispose();
                }
                foreach (Control control in dashboardForm.Controls.OfType<Setting>().ToList())
                {
                    dashboardForm.Controls.Remove(control);
                    control.Dispose();
                }
                foreach (Control control in dashboardForm.Controls.OfType<Student>().ToList())
                {
                    dashboardForm.Controls.Remove(control);
                    control.Dispose();
                }
                foreach (Control control in dashboardForm.Controls.OfType<Teacher>().ToList())
                {
                    dashboardForm.Controls.Remove(control);
                    control.Dispose();
                }

                foreach (Control control in dashboardForm.Controls.OfType<UserData>().ToList())
                {
                    dashboardForm.Controls.Remove(control);
                    control.Dispose();
                }

                // Remove and dispose of the current instance (this)
                dashboardForm.Controls.Remove(this);
                this.Dispose();

                // Optionally, you can force a refresh of the form to ensure UI update
                dashboardForm.Invalidate();
                dashboardForm.Update();

                // Create a new instance of Menu and add it to the form
                new Menu(parentPanel);
            }
        }


        private Image LoadImage(string path)
        {
            try
            {
                return Image.FromFile(path);
            }
            catch (IOException ex)
            {
                MessageBox.Show($"Error loading image: {ex.Message}");
                return null; // Or return a default image
            }
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
            commands.Add("dashboard");
            commands.Add("book");
            commands.Add("library");
            commands.Add("teacher");
            commands.Add("student");
            commands.Add("author");
            commands.Add("sign out");
            commands.Add("how are you today");

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
                    case "dashboard":
                        ShowPanel("Dashboard");
                        break;
                    case "book":
                        ShowPanel("Books");
                        break;
                    case "library":
                        ShowPanel("Library");
                        break;
                    case "teacher":
                        ShowPanel("Teacher");
                        break;
                    case "student":
                        ShowPanel("Students");
                        break;
                    case "author":
                        ShowPanel("Authors");
                        break;
                    case "sign out":
                        ClickSignOut();
                        break;
                    case "how are you today":
                        synthesizer.SpeakAsync("Thanks, I'm fine today. You?");
                        break;
                    default:
                        synthesizer.SpeakAsync("Sorry, I didn't understand that command.");
                        break;
                }
                isListeningForCommand = false;
            }
        }
    }
}
