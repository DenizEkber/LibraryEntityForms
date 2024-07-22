using LibraryEntityForms.CodeFirst.Context;
using LibraryDashboard.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LibraryDashboard
{
    internal class Student : Panel
    {
        private FlowLayoutPanel flowLayoutPanel;

        public Student(Form parentForm)
        {
            this.Size = new Size(1575, 1075);
            this.Location = new Point(345, 120);
            this.BackColor = ColorTranslator.FromHtml("#FAFBFC");
            this.Visible = false;

            parentForm.Controls.Add(this);
            InitializeDashboard();
        }

        private void InitializeDashboard()
        {
            flowLayoutPanel = new FlowLayoutPanel
            {
                Location = new Point(10, 10),
                Size = new Size(1500, 800),
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                BackColor = ColorTranslator.FromHtml("#FAFBFC")
            };
            this.Controls.Add(flowLayoutPanel);
            LoadDataToTable();
        }

        private async void LoadDataToTable()
        {
            using (var ctx = new LibraryContext())
            {
                var data = await (from student in ctx.Students
                                  join groups in ctx.Groups on student.Id_Group equals groups.Id
                                  join faculty in ctx.Faculties on groups.Id_Faculty equals faculty.Id
                                  select new
                                  {
                                      FirstName = student.FirstName,
                                      LastName = student.LastName,
                                      Term = student.Term,
                                      Group = groups.Name,
                                      Faculty = faculty.Name,
                                      PhotoData = student.PhotoData,
                                  }).ToListAsync();

                DisplayStudents(data);
            }
        }

        private void DisplayStudents(IEnumerable<dynamic> students)
        {
            flowLayoutPanel.Controls.Clear(); // Clear previous data

            foreach (var student in students)
            {
                Panel studentPanel = CreateStudentPanel(student);
                flowLayoutPanel.Controls.Add(studentPanel);
            }
        }

        private Panel CreateStudentPanel(dynamic student)
        {
            Panel panel = PanelHelper.CreatePanel(new Size(720, 150), new Padding(10), Color.LightPink, new Padding(10));

            // Student Image
            PictureBox pictureBox = PanelHelper.CreatePictureBox(student.PhotoData, "C:\\Users\\LENOVO\\Desktop\\LibraryEntityForms\\LibraryDashboard\\icon\\profileImage.jpg", new Size(100, 100), new Point(10, 10));
            panel.Controls.Add(pictureBox);

            // Full Name
            Label nameLabel = PanelHelper.CreateLabel($"{student.FirstName} {student.LastName}", new Font("Arial", 14, FontStyle.Bold), Color.DarkRed, new Point(120, 10));
            panel.Controls.Add(nameLabel);

            // Faculty
            Label facultyLabel = PanelHelper.CreateLabel($"Faculty: {student.Faculty} ", new Font("Arial", 12, FontStyle.Regular), Color.Black, new Point(120, 40));
            panel.Controls.Add(facultyLabel);

            // Group
            Label phoneLabel = PanelHelper.CreateLabel($"Group: {student.Group}", new Font("Arial", 12, FontStyle.Regular), Color.Black, new Point(120, 70));
            panel.Controls.Add(phoneLabel);

            //Term
            Label termLabel = PanelHelper.CreateLabel($"Term: {student.Term}", new Font("Arial", 12, FontStyle.Regular), Color.Black, new Point(120, 110));
            panel.Controls.Add(termLabel);

            return panel;
        }
    }
}
