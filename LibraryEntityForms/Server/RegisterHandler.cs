/*using LibraryEntityForms.CodeFirst.Context;
using LibraryEntityForms.CodeFirst.Entity.UserData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEntityForms.Server
{
    public class RegisterHandler
    {
        public async Task Handle(HttpListenerContext context)
        {
            if (context.Request.HttpMethod.Equals("POST", StringComparison.OrdinalIgnoreCase))
            {
                using (var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
                {
                    var body = await reader.ReadToEndAsync();
                    var data = JsonConvert.DeserializeObject<RegisterRequest>(body);
                    var success = RegisterUser(data.Email, data.Password, data.FirstName, data.LastName);

                    var response = new
                    {
                        success = success,
                        message = success ? "User registered successfully" : "User registration failed"
                    };

                    context.Response.StatusCode = success ? (int)HttpStatusCode.OK : (int)HttpStatusCode.BadRequest;
                    await SendResponseAsync(context, response);
                }
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                await SendResponseAsync(context, new { success = false, message = "Method Not Allowed" });
            }
        }

        private bool RegisterUser(string email, string password, string firstName, string lastName, Role role, string Password)
        {
            try
            {
                using (var ctx = new LibraryContext())
                {
                    var user = new Users
                    {
                        Name = email, // or some username
                        Password = password,
                        Role =  role,// or default role
                        Password = password
                    };

                    var userDetail = new UserDetail
                    {
                        Email = email,
                        FirstName = firstName,
                        LastName = lastName
                        
                    };

                    ctx.Users.Add(user);
                    ctx.UserDetail.Add(userDetail);
                    ctx.SaveChanges();

                    return true;
                }
            }
            catch
            {
                return false;
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

    public class RegisterRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
*/