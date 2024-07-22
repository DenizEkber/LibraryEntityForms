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
    internal class Library : Panel
    {
        private FlowLayoutPanel flowLayoutPanel;

        public Library(Form parentForm)
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
                var data = await (from library in ctx.Libraries
                                  select new
                                  {
                                      Name = library.Name,
                                      /*Address = library.Address,
                                      PhoneNumber = library.PhoneNumber,
                                      Email = library.Email,*/
                                      PhotoData = library.PhotoData
                                  }).ToListAsync();

                DisplayLibraries(data);
            }
        }

        private void DisplayLibraries(IEnumerable<dynamic> libraries)
        {
            flowLayoutPanel.Controls.Clear(); // Clear previous data

            foreach (var library in libraries)
            {
                Panel libraryPanel = CreateLibraryPanel(library);
                flowLayoutPanel.Controls.Add(libraryPanel);
            }
        }

        private Panel CreateLibraryPanel(dynamic library)
        {
            Panel panel = PanelHelper.CreatePanel(new Size(720, 200), new Padding(10), Color.LightGray, new Padding(10));

            // Library Image
            PictureBox pictureBox = PanelHelper.CreatePictureBox(library.PhotoData, "C:\\Users\\LENOVO\\Desktop\\LibraryEntityForms\\LibraryDashboard\\icon\\libraryImage.jpg", new Size(100, 100), new Point(10, 10));
            panel.Controls.Add(pictureBox);

            // Library Name
            Label nameLabel = PanelHelper.CreateLabel(library.Name, new Font("Arial", 14, FontStyle.Bold), Color.DarkBlue, new Point(120, 10));
            panel.Controls.Add(nameLabel);

            // Address
            /*Label addressLabel = PanelHelper.CreateLabel(library.Address, new Font("Arial", 12, FontStyle.Regular), Color.Black, new Point(120, 40));
            panel.Controls.Add(addressLabel);

            // Phone Number
            Label phoneLabel = PanelHelper.CreateLabel($"Phone: {library.PhoneNumber}", new Font("Arial", 12, FontStyle.Regular), Color.Black, new Point(120, 70));
            panel.Controls.Add(phoneLabel);

            // Email
            Label emailLabel = PanelHelper.CreateLabel($"Email: {library.Email}", new Font("Arial", 12, FontStyle.Regular), Color.Black, new Point(120, 100));
            panel.Controls.Add(emailLabel);*/

            return panel;
        }
    }
}
