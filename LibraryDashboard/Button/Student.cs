using LibraryDashboard.Design;
using LibraryEntityForms.CodeFirst.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryDashboard
{
    internal class Student : Panel
    {
        PanelCreated pc;
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
            Panel tablePanel = CreateTablePanel(new Point(10, 10), new Size(1500, 800));
            this.Controls.Add(tablePanel);
            LoadDataToTable(tablePanel);
        }
        private Panel CreateTablePanel(Point location, Size size)
        {
            Panel panel = new Panel
            {
                Location = location,
                Size = size,
                AutoScroll = true 
            };

            
            DataGridView dataGridView = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells, 
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                ReadOnly = true,
                AllowUserToAddRows = false, 
                AllowUserToDeleteRows = false, 
                AllowUserToResizeColumns = false, 
                AllowUserToResizeRows = false, 
                BackgroundColor = Color.White, 
                GridColor = Color.LightGray, 
                AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.AliceBlue } 
            };

            
            dataGridView.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                Font = new Font("Arial", 12, FontStyle.Bold),
                BackColor = Color.Navy,
                ForeColor = Color.White,
                Alignment = DataGridViewContentAlignment.MiddleCenter
            };

            
            dataGridView.DefaultCellStyle = new DataGridViewCellStyle
            {
                Font = new Font("Arial", 12),
                BackColor = Color.White,
                ForeColor = Color.Black,
                SelectionBackColor = Color.CornflowerBlue,
                SelectionForeColor = Color.White
            };

            
            panel.Controls.Add(dataGridView);

            return panel;
        }



        private async void LoadDataToTable(Panel panel)
        {
            using (var ctx = new LibraryContext())
            {
                // Veritabanından veriyi çek
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
                                  }).ToListAsync();

                // Paneldeki DataGridView kontrolünü bul
                DataGridView dataGridView = panel.Controls.OfType<DataGridView>().FirstOrDefault();
                if (dataGridView != null)
                {
                    // Veriyi DataGridView'e bağla
                    dataGridView.DataSource = data;
                }
            }
        }
    }
}
