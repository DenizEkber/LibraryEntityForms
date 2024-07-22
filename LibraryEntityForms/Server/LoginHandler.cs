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
    public class LoginHandler
    {
        public async Task Handle(HttpListenerContext context)
        {
            if (context.Request.HttpMethod.Equals("POST", StringComparison.OrdinalIgnoreCase))
            {
                using (var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
                {
                    var body = await reader.ReadToEndAsync();
                    var data = JsonConvert.DeserializeObject<LoginRequest>(body);
                    var userData = GetUserData(data.Email, data.Password);

                    var response = new
                    {
                        success = userData != null,
                        data = userData // Kullanıcı verilerini döndür
                    };

                    context.Response.StatusCode = userData != null ? (int)HttpStatusCode.OK : (int)HttpStatusCode.Unauthorized;
                    await SendResponseAsync(context, response);
                }
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                await SendResponseAsync(context, new { success = false, message = "Method Not Allowed" });
            }
        }

        private dynamic GetUserData(string email, string password)
        {
            using (var ctx = new LibraryContext())
            {
                var user = (from u in ctx.Users
                            join ud in ctx.UserDetail on u.Id equals ud.Id_User
                            where ud.Email == email && u.Password == password
                            select new
                            {
                                Id = u.Id,
                                UserName = u.Name,
                                Role = u.Role.ToString(),
                                Email = ud.Email,
                                FirstName = ud.FirstName,
                                LastName = ud.LastName
                            }).FirstOrDefault();

                return user;
            }
        }

        private async Task SendResponseAsync(HttpListenerContext context, object response)
        {
            var responseString = JsonConvert.SerializeObject(response);
            var responseBuffer = Encoding.UTF8.GetBytes(responseString);

            context.Response.ContentType = "application/json";
            context.Response.ContentLength64 = responseBuffer.Length;

            await context.Response.OutputStream.WriteAsync(responseBuffer, 0, responseBuffer.Length);
            context.Response.Close();
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }



}
