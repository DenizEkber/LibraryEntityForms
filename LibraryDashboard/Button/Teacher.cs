﻿using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryEntityForms.CodeFirst.Context;

namespace LibraryDashboard
{
    internal class Teacher : Panel
    {


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
                
                var data = await (from teacher in ctx.Teachers
                                  join department in ctx.Departments on teacher.Id_Dep equals department.Id
                                  
                                  select new
                                  {
                                      FirstName = teacher.FirstName,
                                      LastName = teacher.LastName,
                                      Dep = department.Name,
                                  }).ToListAsync();

                
                DataGridView dataGridView = panel.Controls.OfType<DataGridView>().FirstOrDefault();
                if (dataGridView != null)
                {
                    
                    dataGridView.DataSource = data;
                }
            }
        }
    }
}
