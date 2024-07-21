using LibraryDashboard.Design;
using LibraryDashboard.LoginRegister;
using LibraryEntityForms.CodeFirst.Entity.UserData;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Speech.Recognition;
using System.Speech.Synthesis;

using System.Windows.Forms;

namespace LibraryDashboard.Navigation
{
    public class LeftNavigation : Panel
    {
        private SpeechRecognitionEngine recognizer;
        private SpeechSynthesizer synthesizer;
        private bool isListeningForCommand;
        private Role role;
        private int buttonTop = 130;
        private Form1 parentPanel;
        private Button selectedButton = null; 
        private List<Button> navigationButtons = new List<Button>();
        private Dashboard dashboardPanel;
        private Books booksPanel;
        private Library libraryPanel;
        private Student studentPanel;
        private Teacher teacherPanle;
        private Author authorPanel;

        public LeftNavigation(Form1 parentForm, Role role)
        {
            this.parentPanel = parentForm;
            this.role = role;
            MaximumSize = new Size(345, 1195);
            Height = 1195;
            Width = 345;
            BackColor = Color.White;
            Location = new Point(0, 0);
            Padding = new Padding(10); 
            parentForm.Controls.Add(this);

            dashboardPanel = new Dashboard(parentForm);
            booksPanel = new Books(parentForm);
            libraryPanel = new Library(parentForm);
            studentPanel = new Student(parentForm);
            teacherPanle = new Teacher(parentForm);
            authorPanel = new Author(parentForm);
            LibraryTop();
            CreateNavigationItems();
            InitializeSpeechRecognition();
            SelectButton(navigationButtons[0]);
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
                Image = ResizeImage(Image.FromFile("C:\\Users\\LENOVO\\Desktop\\LibraryEntityForms\\LibraryDashboard\\icon\\eaglevision.png"), new Size(50, 50)),
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
            if (role == Role.Admin)
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
                Image = ResizeImage(Image.FromFile(iconPath), new Size(32, 32)),
                Tag = new string[] { iconPath, whiteIconPath }, 
                Top = buttonTop, 
                Left = 15 
            };

            
            button.Click += (sender, e) =>
            {
                var clickedButton = (Button)sender;
                SelectButton(clickedButton);
            };

            this.Controls.Add(button);
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
                selectedButton.Image = ResizeImage(Image.FromFile(paths[0]), new Size(32, 32));
            }

            
            button.BackColor = Color.Blue;
            button.ForeColor = Color.White;
            string[] newPaths = (string[])button.Tag;
            button.Image = ResizeImage(Image.FromFile(newPaths[1]), new Size(32, 32));

            selectedButton = button;

           
            if (button.Text == "Dashboard")
            {
                dashboardPanel.Visible = true;
            }
            else 
            {
                dashboardPanel.Visible = false;
            }

            if (button.Text == "Books")
            {
                booksPanel.Visible = true;
            }
            else
            {
                booksPanel.Visible = false;
            }

            if (button.Text == "Library")
            {
                libraryPanel.Visible = true;
            }
            else
            {
                libraryPanel.Visible = false;
            }
            if (button.Text == "Students")
            {
                studentPanel.Visible = true;
            }
            else
            {
                studentPanel.Visible = false;
            }

            if (button.Text == "Teacher")
            {
                teacherPanle.Visible = true;
            }
            else
            {
                teacherPanle.Visible = false;
            }

            if (button.Text == "Authors")
            {
                authorPanel.Visible = true;
            }
            else
            {
                authorPanel.Visible = false;
            }

            if(button.Text == "Sign Out")
            {
                ClickSignOut();
                
            }

        }

        private void ShowDashboard()
        {

            dashboardPanel.Visible = true;
            booksPanel.Visible = false;
            libraryPanel.Visible = false;
            teacherPanle.Visible = false;
            studentPanel.Visible = false;
            authorPanel.Visible = false;
        }
        private void ShowBooks()
        {

            dashboardPanel.Visible = false;
            booksPanel.Visible = true;
            libraryPanel.Visible = false;
            teacherPanle.Visible = false;
            studentPanel.Visible = false;
            authorPanel.Visible = false;
        }
        private void ShowLibrary()
        {

            dashboardPanel.Visible = false;
            booksPanel.Visible = false;
            libraryPanel.Visible = true;
            teacherPanle.Visible = false;
            studentPanel.Visible = false;
            authorPanel.Visible = false;
        }
        private void ShowTeacher()
        {

            dashboardPanel.Visible = false;
            booksPanel.Visible = false;
            libraryPanel.Visible = false;
            teacherPanle.Visible = true;
            studentPanel.Visible = false;
            authorPanel.Visible = false;
        }
        private void ShowStudents()
        {

            dashboardPanel.Visible = false;
            booksPanel.Visible = false;
            libraryPanel.Visible = false;
            teacherPanle.Visible = false;
            studentPanel.Visible = true;
            authorPanel.Visible = false;
        }
        private void ShowAuthors()
        {

            dashboardPanel.Visible = false;
            booksPanel.Visible = false;
            libraryPanel.Visible = false;
            teacherPanle.Visible = false;
            studentPanel.Visible = false;
            authorPanel.Visible = true;
        }
        private void ClickSignOut()
        {
            Form dashboardForm = this.FindForm();
            if (dashboardForm != null)
            {
                //MessageBox.Show(dashboardForm.GetType().Name);

                // Mevcut kontrolü kaldır
                /*dashboardForm.Controls.Remove(this);
                this.Dispose();*/

                var controlsToRemove = new List<Control>();
                //MessageBox.Show(control.GetType().Name);
                foreach (Control control in dashboardForm.Controls)
                {
                    // Kontrolün adını kontrol et
                    if (control is TopNavigation || control is Dashboard )
                    {
                        dashboardForm.Controls.Remove(control);
                        control.Dispose();
                    }
                }
                foreach (var control in controlsToRemove)
                {
                    dashboardForm.Controls.Remove(control);
                    control.Dispose();
                }


                dashboardForm.Controls.Remove(this);
                this.Dispose();

                Menu menu = new Menu(parentPanel);
            }
        }






        private Image ResizeImage(Image imgToResize, Size size)
        {
            return new Bitmap(imgToResize, size);
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
                        ShowDashboard();
                        break;
                    case "book":
                        ShowBooks();
                        break;
                    case "library":
                        ShowLibrary();
                        break;
                    case "teacher":
                        ShowTeacher();
                        break;
                    case "student":
                        ShowStudents();
                        break;
                    case "author":
                        ShowAuthors();
                        break;
                    case "sign out":
                        ClickSignOut();
                        break;
                    case "how are you today":
                        synthesizer.SpeakAsync("Thanks, I'm fine today. You?");
                        break;

                    default:
                        synthesizer.SpeakAsync("I didn't understand that command.");
                        break;
                }

                isListeningForCommand = false;
            }
        }
    }
}
