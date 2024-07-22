using LibraryEntityForms.CodeFirst.Context;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEntityForms.Server
{
    internal class DashboardHandler
    {
        private LibraryContext ctx;

        public void Handle(HttpListenerContext context)
        {
            ctx = new LibraryContext();
            if (context.Request.HttpMethod.Equals("GET", StringComparison.OrdinalIgnoreCase))
            {
                if (MostReadBook() != null || ReadBookThemes()!=null || ReadBookCategory()!=null || OverDue()!= null || TeacherVsStudent()!=null)
                {
                    var objectMapper = new JsonSerializer();
                    var data = new
                    {
                        mostReadBook = MostReadBook(),
                        readBookThemes = ReadBookThemes(),
                        readBookCategory = ReadBookCategory(),
                        overDue = OverDue(),
                        teacherVsStudent = TeacherVsStudent(),


                    };

                    var responseJson = new { success = true, data = data };
                    var response = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(responseJson));
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                    context.Response.OutputStream.Write(response, 0, response.Length);
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                }
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
            }
        }

        private IEnumerable<(string FamousBook, int UsageCount)> MostReadBook()
        {
            var query = (
                from sc in ctx.S_Cards
                join b in ctx.Books on sc.Id_Book equals b.Id
                select new
                {
                    BookName = b.Name,
                    UsageCount = 1
                })
                .Concat(
                from tc in ctx.T_Cards
                join b in ctx.Books on tc.Id_Book equals b.Id
                select new
                {
                    BookName = b.Name,
                    UsageCount = 1
                })
                .GroupBy(x => x.BookName)
                .Select(g => new
                {
                    FamousBook = g.Key,
                    UsageCount = g.Sum(x => x.UsageCount)
                })
                .OrderByDescending(x => x.UsageCount)
                .Take(4)
                .ToList();

            return query.Select(x => (x.FamousBook, x.UsageCount));
        }

        private IEnumerable<(string BookThemes, int UsageCount, int SumThemes)> ReadBookThemes()
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
                    join b in ctx   .Books on tc.Id_Book equals b.Id
                    join d in ctx.Themes on b.Id_Themes equals d.Id
                    select new
                    {
                        BookThemes = d.Name,
                        UsageCount = 1
                    })
                    .GroupBy(x => x.BookThemes)
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

        private IEnumerable<(string BookCategory, int UsageCount)> ReadBookCategory()
        {
            var query = (
                    from sc in ctx.S_Cards
                    join b in ctx   .Books on sc.Id_Book equals b.Id
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
                    })
                    .GroupBy(x => x.BookCategory)
                    .Select(g => new
                    {
                        BookCategory = g.Key,
                        UsageCount = g.Sum(x => x.UsageCount)
                    })
                    .OrderByDescending(x => x.UsageCount)
                    .ToList();

            return query.Select(x => (x.BookCategory, x.UsageCount));
        }

        private IEnumerable<(DateTime Date, int StudentOverdue, int TeacherOverdue)> OverDue()
        {
            DateTime currentDate = DateTime.Now.Date;

            var studentOverdues = ctx.S_Cards
                .AsEnumerable()
                .Where(s => (currentDate - s.DataOut.Date).TotalDays > s.TimeLimit)
                .GroupBy(s => s.DataOut.Date)
                .Select(g => new { Date = g.Key, Count = g.Count() }).ToList();

            var teacherOverdues = ctx.T_Cards
                .AsEnumerable()
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

        private IEnumerable<(DateTime Month, int TeachersCount, int StudentsCount, int totalStudents, int totalTeachers)> TeacherVsStudent()
        {
            var studentData = ctx.S_Cards
            .GroupBy(sc => new { sc.DataOut.Year, sc.DataOut.Month })
            .Select(g => new
            {
                Month = new DateTime(g.Key.Year, g.Key.Month, 1),
                Count = g.Count()
            })
            .ToList();

            var teacherData = ctx.T_Cards
                .GroupBy(tc => new { tc.DataOut.Year, tc.DataOut.Month })
                .Select(g => new
                {
                    Month = new DateTime(g.Key.Year, g.Key.Month, 1),
                    Count = g.Count()
                })
                .ToList();

            var combinedData = (from sd in studentData
                                join td in teacherData on sd.Month equals td.Month into g
                                from subTd in g.DefaultIfEmpty()
                                select new
                                {
                                    Month = sd.Month,
                                    StudentCount = sd.Count,
                                    TeacherCount = subTd?.Count ?? 0
                                }).ToList();

            int totalStudents = studentData.Sum(sd => sd.Count);
            int totalTeachers = teacherData.Sum(td => td.Count);

            return combinedData.Select(x => (x.Month, x.StudentCount, x.TeacherCount, totalStudents, totalTeachers));
        }
    }
}
