using LibraryEntityForms.CodeFirst.Context;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEntityForms.Server
{
    internal class StudentHandler
    {
        private LibraryContext ctx;
        public void Handle(HttpListenerContext context)
        {
            ctx = new LibraryContext();
            if (context.Request.HttpMethod.Equals("GET", StringComparison.OrdinalIgnoreCase))
            {
                var library = GetStudent();
                if (library != null)
                {
                    var responseJson = new { success = true, data = library };
                    var response = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(responseJson));
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                    context.Response.OutputStream.Write(response, 0, response.Length);
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.NoContent;
                }
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
            }
        }


        private IEnumerable<StudentDto> GetStudent()
        {
            using (var ctx = new LibraryContext())
            {
                var data = (from student in ctx.Students
                            join groups in ctx.Groups on student.Id_Group equals groups.Id
                            join faculty in ctx.Faculties on groups.Id_Faculty equals faculty.Id
                            select new StudentDto
                            {
                                FirstName = student.FirstName,
                                LastName = student.LastName,
                                Term = student.Term,
                                Group = groups.Name,
                                Faculty = faculty.Name,
                                PhotoData = student.PhotoData,
                            }).ToList();

                return data;
            }
        }
    }
    public class StudentDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Term { get; set; }
        public string Group { get; set; }
        public string Faculty { get; set; }
        public byte[] PhotoData { get; set; }
    }
}
