using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

using LibraryDashboard.Design;
using LibraryEntityForms.CodeFirst.Context;
using LibraryEntityForms.CodeFirst.Entity.LibraryData;
using LiveCharts;

using Microsoft.EntityFrameworkCore;
using static System.Windows.Forms.MonthCalendar;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace LibraryDashboard
{
    public class Dashboard : Panel
    {
        LibraryContext ctx;
        PanelCreated pc;
        public Dashboard(Form parentForm)
        {
            this.Size = new Size(1575, 1075);
            this.Location = new Point(345, 120);
            this.BackColor = ColorTranslator.FromHtml("#FAFBFC");
            this.Visible = false; 
            
            parentForm.Controls.Add(this);
            InitializeDashboard();
        }

        private IEnumerable<(string FamousBook, int UsageCount)> MostReadBook()
        {
            using (var ctx = new LibraryContext())
            {
                var query = (
                    from sc in ctx.S_Cards
                    join b in ctx.Books on sc.Id_Book equals b.Id
                    select new
                    {
                        BookName = b.Name,
                        UsageCount = 1 // Her kayıt bir kullanım olarak sayılır
                    })
                    .Concat(
                    from tc in ctx.T_Cards
                    join b in ctx.Books on tc.Id_Book equals b.Id
                    select new
                    {
                        BookName = b.Name,
                        UsageCount = 1 // Her kayıt bir kullanım olarak sayılır
                    })
                    .GroupBy(x => x.BookName)
                    .Select(g => new
                    {
                        FamousBook = g.Key,
                        UsageCount = g.Sum(x => x.UsageCount)
                    })
                    .OrderByDescending(x => x.UsageCount)
                    .Take(4) // En çok okunan ilk 4 kitabı al
                    .ToList();

                return query.Select(x => (x.FamousBook, x.UsageCount));
            }
        }
        private IEnumerable<(string BookThemes, int UsageCount, int SumThemes)> ReadBookThemes()
        {
            using (LibraryContext ctx = new LibraryContext())
            {
                var query = (
                        from sc in ctx.S_Cards
                        join b in ctx.Books on sc.Id_Book equals b.Id
                        join c in ctx.Themes on b.Id_Themes equals c.Id
                        select new
                        {
                            BookThemes = c.Name,
                            UsageCount = 1
                        })
                        .Concat(
                        from tc in ctx.T_Cards
                        join b in ctx.Books on tc.Id_Book equals b.Id
                        join d in ctx.Themes on b.Id_Themes equals d.Id
                        select new
                        {
                            BookThemes = d.Name,
                            UsageCount = 1
                        }
                        ).GroupBy(x => x.BookThemes)
                        .Select(g => new
                        {
                            BooksThemes = g.Key,
                            UsageCount = g.Sum(x => x.UsageCount)
                        })
                        .OrderByDescending(x => x.UsageCount)
                        .ToList();

                int sumOfUsageCounts = query.Sum(x => x.UsageCount);

                return query.Select(x => (x.BooksThemes, x.UsageCount, sumOfUsageCounts));
            }
        }
        private IEnumerable<(string BookCatgeory, int UsageCount)> ReadBookCategory()
        {
            using (LibraryContext ctx =new LibraryContext())
            {
                var query = (
                        from sc in ctx.S_Cards
                        join b in ctx.Books on sc.Id_Book equals b.Id
                        join c in ctx.Categories on b.Id_Category equals c.Id
                        select new
                        {
                            BookCategory = c.Name,
                            UsageCount = 1
                        })
                        .Concat(
                        from tc in ctx.T_Cards
                        join b in ctx.Books on tc.Id_Book equals b.Id
                        join d in ctx.Categories on b.Id_Category equals d.Id
                        select new
                        {
                            BookCategory = d.Name,
                            UsageCount = 1
                        }
                        ).GroupBy(x => x.BookCategory)
                        .Select(g => new
                        {
                            BookCategory = g.Key,
                            UsageCount = g.Sum(x => x.UsageCount)
                        })
                        .OrderByDescending(x => x.UsageCount)
                        .ToList();


                

                return query.Select(x => (x.BookCategory, x.UsageCount));
            }
        }
        private IEnumerable<(DateTime Date, int StudentOverdue, int TeacherOverdue)> OverDue()
        {
            DateTime currentDate = DateTime.Now.Date; // Şu anki tarihi zaman kısmı olmadan al

            using (LibraryContext ctx = new LibraryContext())
            {
                var studentOverdues = ctx.S_Cards
                    .AsEnumerable() // Veriyi belleğe al
                    .Where(s => (currentDate - s.DataOut.Date).TotalDays > s.TimeLimit)
                    .GroupBy(s => s.DataOut.Date)
                    .Select(g => new { Date = g.Key, Count = g.Count() }).ToList();

                var teacherOverdues = ctx.T_Cards
                    .AsEnumerable() // Veriyi belleğe al
                    .Where(t => (currentDate - t.DataOut.Date).TotalDays > t.TimeLimit)
                    .GroupBy(t => t.DataOut.Date)
                    .Select(g => new { Date = g.Key, Count = g.Count() }).ToList();

                var overdueData = (from s in studentOverdues
                                   join t in teacherOverdues on s.Date equals t.Date into st
                                   from sub in st.DefaultIfEmpty()
                                   select new
                                   {
                                       Date = s.Date,
                                       StudentOverdue = s.Count,
                                       TeacherOverdue = sub != null ? sub.Count : 0
                                   }).ToList();

                return overdueData.Select(x => (x.Date, x.StudentOverdue, x.TeacherOverdue));
            }
        }

        IEnumerable<(DateTime Month, int TeachersCount, int StudentsCount, int totalStudents, int totalTeachers)> data ()
        {
            using (LibraryContext ctx =new LibraryContext())
            {
                var studentData = ctx.S_Cards
                .GroupBy(sc => new { sc.DataOut.Year, sc.DataOut.Month })
                .Select(g => new
                {
                    Month = new DateTime(g.Key.Year, g.Key.Month, 1),
                    Count = g.Count()
                })
                .ToList();

                // Group by month and count books read by teachers
                var teacherData = ctx.T_Cards
                    .GroupBy(tc => new { tc.DataOut.Year, tc.DataOut.Month })
                    .Select(g => new
                    {
                        Month = new DateTime(g.Key.Year, g.Key.Month, 1),
                        Count = g.Count()
                    })
                    .ToList();

                // Combine data
                var combinedData = (from sd in studentData
                                    join td in teacherData
                                    on sd.Month equals td.Month into g
                                    from subTd in g.DefaultIfEmpty()
                                    select new
                                    {
                                        Month = sd.Month,
                                        StudentCount = sd.Count,
                                        TeacherCount = subTd?.Count ?? 0
                                    }).ToList();

                int totalStudents = studentData.Sum(sd => sd.Count);
                int totalTeachers = teacherData.Sum(td => td.Count);

                return combinedData.Select(x=>(x.Month, x.StudentCount, x.TeacherCount, totalStudents, totalTeachers));
            }
        }
        private void InitializeDashboard()
        {
            pc = new PanelCreated();
            using (ctx = new LibraryContext())
            {
                
                var mostReadBooks = MostReadBook();

                Panel totalBooksPanel = pc.CreatePanel(new Point(20, 20), new Size(877, 366), Color.White);
                
                totalBooksPanel.Region = System.Drawing.Region.FromHrgn(RoundCorner.CreateRoundRectRgn(0, 0, 877, 366, 15, 15));

                Label totalBooksLabel = pc.CreateLabel($"Total Books", new Point(20, 20), new Font("Poppins", 15, FontStyle.Bold),Color.White, new Size(200,30));
                totalBooksPanel.Controls.Add(totalBooksLabel);

                
                Color[] colors = { ColorTranslator.FromHtml("#FFE2E5"), ColorTranslator.FromHtml("#FFF4DE"), ColorTranslator.FromHtml("#DCFCE7"), ColorTranslator.FromHtml("#F3E8FF") };
                Image[] images = { Image.FromFile("C:\\Users\\LENOVO\\Desktop\\LibraryEntityForms\\LibraryDashboard\\icon\\TopBooks\\icon1.png"), Image.FromFile("C:\\Users\\LENOVO\\Desktop\\LibraryEntityForms\\LibraryDashboard\\icon\\TopBooks\\icon2.png"), Image.FromFile("C:\\Users\\LENOVO\\Desktop\\LibraryEntityForms\\LibraryDashboard\\icon\\TopBooks\\icon3.png"), Image.FromFile("C:\\Users\\LENOVO\\Desktop\\LibraryEntityForms\\LibraryDashboard\\icon\\TopBooks\\icon4.png") };
                int xPos = 20;
                
                int i = 0;
                foreach (var bookGroup in mostReadBooks)
                {
                    var bookName = bookGroup.FamousBook;
                    var readCount = bookGroup.UsageCount;
                    Color panelColor = colors[i % colors.Length];
                    Panel bookPanel = pc.CreatePanel(new Point(xPos, 160), new Size(180, 170), panelColor);
                    bookPanel.Region = System.Drawing.Region.FromHrgn(RoundCorner.CreateRoundRectRgn(0, 0, 180, 170, 20, 20));
                    PictureBox pictureBox = new PictureBox();
                    pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
                    pictureBox.Location = new Point(10, 10);
                    pictureBox.Image = images[i];
                    bookPanel.Controls.Add(pictureBox);
                    Label bookCountLabel = pc.CreateLabel($"{readCount}", new Point(10, 60), new Font("Poppins", 18, FontStyle.Bold), panelColor, new Size(100, 40));
                    bookCountLabel.ForeColor = ColorTranslator.FromHtml("#151D48");
                    
                    Label bookReadLabel = pc.CreateLabel($"{bookName}", new Point(10, 100), new Font("Poppins", 12, FontStyle.Regular),panelColor, new Size(160,50));
                    bookReadLabel.ForeColor = ColorTranslator.FromHtml("#425166");

                    bookPanel.Controls.Add(bookCountLabel);
                    bookPanel.Controls.Add(bookReadLabel);
                    totalBooksPanel.Controls.Add(bookPanel);

                    xPos += 200; 
                    i++;
                }

                this.Controls.Add(totalBooksPanel);


                // Books By Themes chart
                Panel booksByThemesPanel = pc.CreatePanel(new Point(947, 20), new Size(542, 506), Color.White);
                booksByThemesPanel.Region = System.Drawing.Region.FromHrgn(RoundCorner.CreateRoundRectRgn(0, 0, 542, 506, 20, 20));
                Label booksByThemesLabel = pc.CreateLabel("Books By Themes", new Point(20, 20), new Font("Segoe UI", 12, FontStyle.Bold), Color.White, new Size(200,40));
                Chart booksByThemesChart = CreateDountChart(new Point(50, 50), new Size(470, 420), ReadBookThemes());
                booksByThemesPanel.Controls.Add(booksByThemesLabel);
                booksByThemesPanel.Controls.Add(booksByThemesChart);
                this.Controls.Add(booksByThemesPanel);

                // Books By Category chart
                Panel booksByCategoryPanel = pc.CreatePanel(new Point(20, 540), new Size(645, 319), Color.White);
                booksByCategoryPanel.Region = System.Drawing.Region.FromHrgn(RoundCorner.CreateRoundRectRgn(0, 0, 645, 319, 20, 20));
                Label booksByCategoryLabel = pc.CreateLabel("Books By Category", new Point(20, 20), new Font("Segoe UI", 12, FontStyle.Bold), Color.White, new Size(200, 40));
                Chart booksByCategoryChart = CreateBarChart(new Point(0, 100), new Size(589, 200), ReadBookCategory());
                booksByCategoryPanel.Controls.Add(booksByCategoryLabel);
                booksByCategoryPanel.Controls.Add(booksByCategoryChart);
                this.Controls.Add(booksByCategoryPanel);

                // Overdue chart
                Panel overduePanel = pc.CreatePanel(new Point(700, 540), new Size(420, 319), Color.White);
                overduePanel.Region = System.Drawing.Region.FromHrgn(RoundCorner.CreateRoundRectRgn(0, 0, 420, 319, 20, 20));
                Label overdueLabel = pc.CreateLabel("Overdue", new Point(20, 20), new Font("Segoe UI", 12, FontStyle.Bold), Color.White, new Size(200, 40));
                overduePanel.Controls.Add(overdueLabel); 
                LiveCharts.WinForms.CartesianChart overdueChart = CreateLineChart(new Point(20, 60), new Size(400, 250), OverDue());
                overduePanel.Controls.Add(overdueChart);
                this.Controls.Add(overduePanel);


                // Teachers vs Students chart
                Panel teachersVsStudentsPanel = pc.CreatePanel(new Point(1140, 540), new Size(371, 319), Color.White);
                teachersVsStudentsPanel.Region = System.Drawing.Region.FromHrgn(RoundCorner.CreateRoundRectRgn(0, 0, 371, 319, 20, 20));
                Label teachersVsStudentsLabel = pc.CreateLabel("Teachers vs Students", new Point(20, 20), new Font("Segoe UI", 12, FontStyle.Bold), Color.White, new Size(200, 40));
                Chart teachersVsStudentsChart = CreateBarChartStudTeach(new Point(0, 50), new Size(334, 157),data());
                var query = data().First();
                Label totalLabel = new Label
                {
                    Text = $"Teachers: {query.totalTeachers}\nStudents: {query.totalStudents}",
                    Location = new Point(20, 240),
                    Font = new Font("Segoe UI", 12, FontStyle.Regular),
                    ForeColor = Color.Black,
                    Size = new Size(200, 80)
                };
                teachersVsStudentsPanel.Controls.Add(teachersVsStudentsLabel);
                teachersVsStudentsPanel.Controls.Add(teachersVsStudentsChart);
                teachersVsStudentsPanel.Controls.Add(totalLabel);
                this.Controls.Add(teachersVsStudentsPanel);

            }
        }

        private Chart CreateBarChartStudTeach(Point location, Size size, IEnumerable<(DateTime Month, int TeachersCount, int StudentsCount, int totalStudents, int totalTeachers)> data)
        {
            Chart chart = new Chart
            {
                Location = location,
                Size = size
            };

            ChartArea chartArea = new ChartArea
            {
                AxisX =
                {
                    MajorGrid = { Enabled = false },
                    MinorGrid = { Enabled = false },
                    MajorTickMark = { Enabled = false },
                    MinorTickMark = { Enabled = false },
                    
                    LineWidth = 0, // Hide X axis line
                    LabelStyle = { Enabled = true }
                },
                AxisY =
                {
                    MajorGrid = { Enabled = false }, // Disable major grid lines
                    MinorGrid = { Enabled = false }, // Disable minor grid lines
                    LineWidth = 0, // Hide Y axis line
                    
                    LabelStyle = { Enabled = false }
                },
                BorderWidth = 0, // Remove chart area border
                BackColor = Color.Transparent // Make chart area background transparent
            };

            chart.ChartAreas.Add(chartArea);

            Series teachersSeries = new Series
            {
                ChartType = SeriesChartType.Column,
                
                Name = "Teachers",
                Color = ColorTranslator.FromHtml("#82CD47"),
                CustomProperties = "PointWidth=0.4"
                
            };


            Series studentsSeries = new Series
            {
                ChartType = SeriesChartType.Column,
                Name = "Students",
                Color = ColorTranslator.FromHtml("#FFD93D"),
                CustomProperties = "PointWidth=0.4"
            };

            foreach (var item in data)
            {
                teachersSeries.Points.AddXY(item.Month.ToString("MMM"), item.TeachersCount);
                studentsSeries.Points.AddXY(item.Month.ToString("MMM"), item.StudentsCount);
            }

            chart.Series.Add(teachersSeries);
            chart.Series.Add(studentsSeries);




            return chart;
        }


      

        

        private Chart CreateDountChart(Point location, Size size, IEnumerable<(string BookThemes, int UsageCount, int SumThemes)> ReadBookThemes)
        {
            Chart chart = new Chart
            {
                Location = location,
                Size = size
            };

            // Chart alanı oluşturun ve ayarlayın
            ChartArea chartArea = new ChartArea();
            chart.ChartAreas.Add(chartArea);

            // Series oluşturun (Donut chart için Pie türü kullanılır)
            Series series = new Series
            {
                ChartType = SeriesChartType.Doughnut,
                Name = "BooksByThemes"
            };

            // Örnek veri
            /*series.Points.Add(new DataPoint(0, 100) { Label = "Category A", LegendText = "Category A", Color = Color.Red });
            series.Points.Add(new DataPoint(0, 0) { Label = "Category B", LegendText = "Category B", Color = Color.Green });
            series.Points.Add(new DataPoint(0, 0) { Label = "Category C", LegendText = "Category C", Color = Color.Blue });*/

            foreach (var theme in ReadBookThemes)
            {
                // Eğer SumThemes sıfırdan büyükse, yüzde hesaplamasını yapın
                double percentage = theme.SumThemes > 0 ? (double)theme.UsageCount / theme.SumThemes * 100 : 0;
                
                series.Points.Add(new DataPoint(0, percentage)
                {
                    Label = $"{theme.BookThemes} ({percentage} %)",
                    LegendText = $"{theme.BookThemes}"
                });
            }

            

            // Her bir dilim için CustomProperties ayarlayın (kenar dışına çıkmasını sağlar)
            /*foreach (DataPoint point in series.Points)
            {
                point.CustomProperties = "PieDrawingStyle=Outside";
            }*/

            // Series'ı chart kontrolüne ekle
            chart.Series.Add(series);

            // Chart kontrolünü geri döndür
            return chart;
        }


        private Chart CreateBarChart(Point location, Size size, IEnumerable<(string BookCategory, int UsageCount)> ReadBookCategory)
        {
            Chart chart = new Chart
            {
                Location = location,
                Size = size
            };

            ChartArea chartArea = new ChartArea();

            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisX.MinorGrid.Enabled = false;
            chartArea.AxisX.MajorTickMark.Enabled = false;
            chartArea.AxisX.MinorTickMark.Enabled = false;

            chartArea.AxisY.MajorGrid.Enabled = true; // Y ekseni büyük çizgileri etkinleştir
            chartArea.AxisY.MajorGrid.LineColor = ColorTranslator.FromHtml("#EFF1F3"); // Y ekseni büyük çizgileri rengi
            chartArea.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Solid; // Y ekseni büyük çizgileri stil
            chartArea.AxisY.MinorGrid.Enabled = false; // Y ekseni küçük çizgileri kaldır

            chartArea.AxisY.LineColor = Color.DarkGray;
            chartArea.AxisY.LineWidth = 1;

            chart.ChartAreas.Add(chartArea);

            Series series = new Series
            {
                ChartType = SeriesChartType.Column,
                Name = "BooksByCategory",
                BorderColor = ColorTranslator.FromHtml("#0095FF"),
                CustomProperties = "PointWidth=0.2"
            };

            foreach (var category in ReadBookCategory)
            {
                // Eğer SumThemes sıfırdan büyükse, yüzde hesaplamasını yapın


                series.Points.AddXY($"{category.BookCategory}", category.UsageCount);
            }
            // Gridlines ve Major/Minor Ticklerı kaldırma
            /*chartArea.AxisX.MajorGrid.Enabled = true; // X ekseni büyük çizgileri etkinleştir
            chartArea.AxisX.MajorGrid.LineColor = Color.LightGray; // X ekseni büyük çizgileri rengi
            chartArea.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Solid; // X ekseni büyük çizgileri stil*/
            //chartArea.AxisX.MinorGrid.Enabled = false; // X ekseni küçük çizgileri kaldır

            

            // Yatay ve dikey eksenlerin çizgi rengini ve kalınlığını ayarla
            
            

            //chartArea.AxisX.LineColor = Color.Blue;
            chart.Series.Add(series);

            return chart;
        }

        private LiveCharts.WinForms.CartesianChart CreateLineChart(Point location, Size size, IEnumerable<(DateTime Date, int StudentOverdue, int TeacherOverdue)> OverDue)
        {
            
            var lineChart = new LiveCharts.WinForms.CartesianChart
            {
                Location = location,
                Size = size,
                LegendLocation = LiveCharts.LegendLocation.Bottom
            };

            
            LiveCharts.Wpf.LineSeries studentSeries = new LiveCharts.Wpf.LineSeries
            {
                Title = "Student",
                Values = new ChartValues<int>(),
                StrokeThickness = 3,
                Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(7, 224, 152)),
                PointGeometrySize = 8,
                DataLabels = false,
                Fill = new System.Windows.Media.LinearGradientBrush
                {
                    GradientStops = new System.Windows.Media.GradientStopCollection
            {
                new System.Windows.Media.GradientStop(System.Windows.Media.Colors.LightGreen, 0),
                new System.Windows.Media.GradientStop(System.Windows.Media.Colors.Transparent, 1)
            }
                },
                PointGeometry = LiveCharts.Wpf.DefaultGeometries.Circle 
            };

            LiveCharts.Wpf.LineSeries teacherSeries = new LiveCharts.Wpf.LineSeries
            {
                Title = "Teacher",
                Values = new ChartValues<int>(),
                StrokeThickness = 3,
                Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 149, 255)),
                PointGeometrySize = 8,
                DataLabels = false,
                Fill = new System.Windows.Media.LinearGradientBrush
                {
                    GradientStops = new System.Windows.Media.GradientStopCollection
            {
                new System.Windows.Media.GradientStop(System.Windows.Media.Colors.LightBlue, 0),
                new System.Windows.Media.GradientStop(System.Windows.Media.Colors.Transparent, 1)
            }
                },
                PointGeometry = LiveCharts.Wpf.DefaultGeometries.Circle 
            };

            
            foreach (var overdue in OverDue)
            {
                studentSeries.Values.Add(overdue.StudentOverdue);
                teacherSeries.Values.Add(overdue.TeacherOverdue);
            }

            lineChart.Series.Add(studentSeries);
            lineChart.Series.Add(teacherSeries);

            
            lineChart.AxisX.Add(new LiveCharts.Wpf.Axis
            {
                
                Labels = OverDue.Select(o => o.Date.ToString("MMM")).ToList()
            });

            
            lineChart.AxisY.Add(new LiveCharts.Wpf.Axis
            {
                
                LabelFormatter = value => value.ToString("N")
            });

            return lineChart;
        }

    }
}
