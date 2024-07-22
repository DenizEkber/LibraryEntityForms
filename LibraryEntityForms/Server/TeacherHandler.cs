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
    internal class TeacherHandler
    {
        private LibraryContext ctx;
        public void Handle(HttpListenerContext context)
        {
            ctx = new LibraryContext();
            if (context.Request.HttpMethod.Equals("GET", StringComparison.OrdinalIgnoreCase))
            {
                var library = GetTeacher();
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


        private IEnumerable<TeacherDto> GetTeacher()
        {
            using (var ctx = new LibraryContext())
            {
                var data = (from teacher in ctx.Teachers
                                 join department in ctx.Departments on teacher.Id_Dep equals department.Id
                                 select new TeacherDto
                                 {
                                     FirstName = teacher.FirstName,
                                     LastName = teacher.LastName,
                                     Department = department.Name,
                                     PhotoData = teacher.PhotoData // Base64 fotoğraf verisi
                                 }).ToList();

                return data;
            }
        }
    }
    public class TeacherDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Department { get; set; }
        public byte[] PhotoData { get; set; }
    }
}
