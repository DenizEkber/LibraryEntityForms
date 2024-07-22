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
    internal class LibraryHandler
    {
        private LibraryContext ctx;
        public void Handle(HttpListenerContext context)
        {
            ctx = new LibraryContext();
            if (context.Request.HttpMethod.Equals("GET", StringComparison.OrdinalIgnoreCase))
            {
                var library = GetLibrary();
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


        private IEnumerable<LibraryDto> GetLibrary()
        {
            using (var ctx = new LibraryContext())
            {
                var data = (from library in ctx.Libraries
                                 select new LibraryDto
                                 {
                                     Name = library.Name,
                                     PhotoData = library.PhotoData 
                                 }).ToList();

                return data;
            }
        }
    }
    public class LibraryDto
    {
        public string Name { get; set; }
        public byte[] PhotoData { get; set; } 
    }
}
