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
    internal class Teacher : Panel
    {
        private FlowLayoutPanel flowLayoutPanel;

        public Teacher(Form parentForm)
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
                var data = await (from teacher in ctx.Teachers
                                  join department in ctx.Departments on teacher.Id_Dep equals department.Id
                                  select new
                                  {
                                      FirstName = teacher.FirstName,
                                      LastName = teacher.LastName,
                                      Department = department.Name,
                                      PhotoData = teacher.PhotoData // Base64 fotoğraf verisi
                                  }).ToListAsync();

                DisplayTeachers(data);
            }
        }

        private void DisplayTeachers(IEnumerable<dynamic> teachers)
        {
            flowLayoutPanel.Controls.Clear(); // Clear previous data

            foreach (var teacher in teachers)
            {
                Panel teacherPanel = CreateTeacherPanel(teacher);
                flowLayoutPanel.Controls.Add(teacherPanel);
            }
        }

        private Panel CreateTeacherPanel(dynamic teacher)
        {
            Panel panel = PanelHelper.CreatePanel(new Size(720, 150), new Padding(10), Color.LightBlue, new Padding(10));

            // Teacher Image
            PictureBox pictureBox = PanelHelper.CreatePictureBox(teacher.PhotoData, "C:\\Users\\LENOVO\\Desktop\\LibraryEntityForms\\LibraryDashboard\\icon\\profileImage.jpg", new Size(100, 100), new Point(10, 10));
            panel.Controls.Add(pictureBox);

            // Full Name
            Label nameLabel = PanelHelper.CreateLabel($"{teacher.FirstName} {teacher.LastName}", new Font("Arial", 14, FontStyle.Bold), Color.DarkBlue, new Point(120, 10));
            panel.Controls.Add(nameLabel);

            // Email
            Label departmentLabel = PanelHelper.CreateLabel($"Department: {teacher.Department}", new Font("Arial", 12, FontStyle.Regular), Color.Black, new Point(120, 40));
            panel.Controls.Add(departmentLabel);

            // Phone Number
            /*Label phoneLabel = PanelHelper.CreateLabel($"Phone: {teacher.PhoneNumber}", new Font("Arial", 12, FontStyle.Regular), Color.Black, new Point(120, 70));
            panel.Controls.Add(phoneLabel);*/

            return panel;
        }
    }
}
