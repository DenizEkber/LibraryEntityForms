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
    internal class Books : Panel
    {
        private FlowLayoutPanel flowLayoutPanel;

        public Books(Form parentForm)
        {
            this.Size = new Size(1575, 1075);
            this.Location = new Point(345, 120);
            this.BackColor = ColorTranslator.FromHtml("#FAFBFC");
            this.Visible = false;

            parentForm.Controls.Add(this);
            InitializeDashboard();
        }

        private async void InitializeDashboard()
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
                var data = await (from book in ctx.Books
                                  join author in ctx.Authors on book.Id_Author equals author.Id
                                  join theme in ctx.Themes on book.Id_Themes equals theme.Id
                                  join category in ctx.Categories on book.Id_Category equals category.Id
                                  join press in ctx.Presses on book.Id_Press equals press.Id
                                  select new
                                  {
                                      BookName = book.Name,
                                      BookPages = book.Pages,
                                      BookYearPress = book.YearPress,
                                      BookComment = book.Comment,
                                      BookQuantity = book.Quantity,
                                      AuthorFirstName = author.FirstName,
                                      AuthorLastName = author.LastName,
                                      ThemeName = theme.Name,
                                      CategoryName = category.Name,
                                      PressName = press.Name,
                                      PhotoData = book.PhotoData 

                                  }).ToListAsync();

                DisplayBooks(data);
            }
        }

        private void DisplayBooks(IEnumerable<dynamic> books)
        {
            flowLayoutPanel.Controls.Clear(); 

            foreach (var book in books)
            {
                Panel bookPanel = CreateBookPanel(book);
                flowLayoutPanel.Controls.Add(bookPanel);
            }
        }

        private Panel CreateBookPanel(dynamic book)
        {
            Panel panel = PanelHelper.CreatePanel(new Size(720, 300), new Padding(10), Color.LightCyan, new Padding(10));

            // Book Image
            PictureBox pictureBox = PanelHelper.CreatePictureBox(book.PhotoData, "C:\\Users\\LENOVO\\Desktop\\LibraryEntityForms\\LibraryDashboard\\icon\\profileImage.jpg", new Size(100, 150), new Point(10, 10));
            panel.Controls.Add(pictureBox);

            // Book Name
            Label titleLabel = PanelHelper.CreateLabel(book.BookName, new Font("Arial", 14, FontStyle.Bold), Color.DarkBlue, new Point(120, 10));
            panel.Controls.Add(titleLabel);

            // Author
            Label authorLabel = PanelHelper.CreateLabel($"by {book.AuthorFirstName} {book.AuthorLastName}", new Font("Arial", 12, FontStyle.Italic), Color.DarkSlateGray, new Point(120, 35));
            panel.Controls.Add(authorLabel);

            // Description
            Label descriptionLabel = PanelHelper.CreateLabel(book.BookComment, new Font("Arial", 11, FontStyle.Regular), Color.Black, new Point(120, 60));
            panel.Controls.Add(descriptionLabel);

            // Details
            Label detailsLabel = PanelHelper.CreateLabel($"{book.BookPages} pages, Published in {book.BookYearPress}", new Font("Arial", 11, FontStyle.Regular), Color.Black, new Point(120, 85));
            panel.Controls.Add(detailsLabel);

            // Category
            Label categoryLabel = PanelHelper.CreateLabel($"Category: {book.CategoryName}", new Font("Arial", 11, FontStyle.Regular), Color.MediumSeaGreen, new Point(120, 110));
            panel.Controls.Add(categoryLabel);

            // Theme
            Label themeLabel = PanelHelper.CreateLabel($"Theme: {book.ThemeName}", new Font("Arial", 11, FontStyle.Regular), Color.OrangeRed, new Point(120, 135));
            panel.Controls.Add(themeLabel);

            // Press
            Label pressLabel = PanelHelper.CreateLabel($"Press: {book.PressName}", new Font("Arial", 11, FontStyle.Regular), Color.Purple, new Point(120, 160));
            panel.Controls.Add(pressLabel);

            return panel;
        }
    }
}
