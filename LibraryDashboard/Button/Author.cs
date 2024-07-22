using LibraryEntityForms.CodeFirst.Context;
using LibraryDashboard.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LibraryDashboard
{
    internal class Author : Panel
    {
        private FlowLayoutPanel flowLayoutPanel;

        public Author(Form parentForm)
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
                var data = await (from author in ctx.Authors
                                  select new
                                  {
                                      FirstName = author.FirstName,
                                      LastName = author.LastName,
                                      PhotoData = author.PhotoData // Byte[] fotoğraf verisi
                                  }).ToListAsync();

                DisplayAuthors(data);
            }
        }

        private void DisplayAuthors(IEnumerable<dynamic> authors)
        {
            flowLayoutPanel.Controls.Clear(); // Clear previous data

            foreach (var author in authors)
            {
                Panel authorPanel = CreateAuthorPanel(author.FirstName, author.LastName, author.PhotoData);
                flowLayoutPanel.Controls.Add(authorPanel);
            }
        }

        private Panel CreateAuthorPanel(string firstName, string lastName, byte[] photoData)
        {
            Panel panel = PanelHelper.CreatePanel(new Size(720, 120), new Padding(10), Color.LightYellow, new Padding(10));

            // Author Picture
            PictureBox pictureBox = PanelHelper.CreatePictureBox(photoData, "C:\\Users\\LENOVO\\Desktop\\LibraryEntityForms\\LibraryDashboard\\icon\\profileImage.jpg", new Size(100, 100), new Point(10, 10));
            panel.Controls.Add(pictureBox);

            // Full Name
            Label nameLabel = PanelHelper.CreateLabel($"{firstName} {lastName}", new Font("Arial", 14, FontStyle.Bold), Color.DarkGreen, new Point(120, 10));
            panel.Controls.Add(nameLabel);

            return panel;
        }
    }
}
