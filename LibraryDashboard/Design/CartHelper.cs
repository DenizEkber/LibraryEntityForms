using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using LiveCharts;
using LiveCharts.Wpf;

namespace LibraryDashboard.Helpers
{
    public static class ChartHelper
    {
        public static LiveCharts.WinForms.CartesianChart CreateBarChart(Point location, Size size, IEnumerable<(string Label, int Value)> data, string title)
        {
            var chart = new LiveCharts.WinForms.CartesianChart
            {
                Location = location,
                Size = size,
                LegendLocation = LegendLocation.Bottom
            };

            var series = new ColumnSeries
            {
                Title = title,
                Values = new ChartValues<int>(data.Select(d => d.Value)),
                Fill = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 149, 255))
            };

            chart.Series.Add(series);

            chart.AxisX.Add(new Axis
            {
                Labels = data.Select(d => d.Label).ToList()
            });

            chart.AxisY.Add(new Axis
            {
                LabelFormatter = value => value.ToString("N")
            });

            return chart;
        }

        public static LiveCharts.WinForms.PieChart CreateDoughnutChart(Point location, Size size, IEnumerable<(string Label, int Percentage, int SumLabel)> data)
        {
            var pieChart = new LiveCharts.WinForms.PieChart
            {
                Location = location,
                Size = size,
                InnerRadius = 100,
                LegendLocation = LegendLocation.Right
            };

            foreach (var item in data)
            {
                pieChart.Series.Add(new PieSeries
                {
                    Title = item.Label,
                    Values = new ChartValues<double> { item.Percentage/item.SumLabel*100 },
                    DataLabels = true,
                    LabelPoint = chartPoint => $" {chartPoint.Y:F2}%",
                    Foreground = System.Windows.Media.Brushes.Black,
                    FontWeight = System.Windows.FontWeights.Bold,
                    LabelPosition = PieLabelPosition.OutsideSlice
                });
            }

            return pieChart;
        }

        public static LiveCharts.WinForms.CartesianChart CreateLineChart(Point location, Size size, IEnumerable<(DateTime Date, int StudentOverdue, int TeacherOverdue)> data)
        {
            var lineChart = new LiveCharts.WinForms.CartesianChart
            {
                Location = location,
                Size = size,
                LegendLocation = LegendLocation.Bottom
            };

            var studentSeries = new LineSeries
            {
                Title = "Student",
                Values = new ChartValues<int>(data.Select(d => d.StudentOverdue)),
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
                PointGeometry = DefaultGeometries.Circle
            };

            var teacherSeries = new LineSeries
            {
                Title = "Teacher",
                Values = new ChartValues<int>(data.Select(d => d.TeacherOverdue)),
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
                PointGeometry = DefaultGeometries.Circle
            };

            lineChart.Series.Add(studentSeries);
            lineChart.Series.Add(teacherSeries);

            lineChart.AxisX.Add(new Axis
            {
                Labels = data.Select(d => d.Date.ToString("MMM")).ToList()
            });

            lineChart.AxisY.Add(new Axis
            {
                LabelFormatter = value => value.ToString("N")
            });

            return lineChart;
        }

        public static CartesianChart CreateColumnChart(IEnumerable<(DateTime Month, int TeachersCount, int StudentsCount, int totalStudents, int totalTeachers)> data)
        {
            var chart = new CartesianChart();

            var studentSeries = new ColumnSeries
            {
                Title = "Students",
                Values = new ChartValues<int>(),
                DataLabels = true
            };
            var teacherSeries = new ColumnSeries
            {
                Title = "Teachers",
                Values = new ChartValues<int>(),
                DataLabels = true
            };
            var labels = new List<string>();

            foreach (var item in data)
            {
                studentSeries.Values.Add(item.StudentsCount);
                teacherSeries.Values.Add(item.TeachersCount);
                labels.Add(item.Month.ToString("MMM yyyy"));
            }

            chart.Series = new SeriesCollection { studentSeries, teacherSeries };
            chart.AxisX.Add(new Axis { Title = "Month", Labels = labels });
            chart.AxisY.Add(new Axis { Title = "Usage Count" });

            return chart;
        }
    }
}
