using LibraryEntityForms.CodeFirst.Context;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
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
                var objectMapper = new JsonSerializer();
                var requestData = default(LoginRequest);
                using (var reader = new StreamReader(context.Request.InputStream))
                {
                    var body = await reader.ReadToEndAsync();
                    var data = JsonConvert.DeserializeObject<LoginRequest>(body);
                    var userData = GetUserData(data.Email, data.Password);

                    var responseJson = new
                    {
                        success = userData != null,
                        data = userData // Kullanıcı verilerini döndür
                    };

                    var response = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(responseJson));
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                    context.Response.OutputStream.Write(response, 0, response.Length);
                }
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
            }
        }

        private dynamic GetUserData(string userEmail, string password)
        {
            using (var ctx = new LibraryContext())
            {
                var user = (from u in ctx.Users
                            join ud in ctx.UserDetail on u.Id equals ud.Id_User
                            where ud.Email == userEmail
                            select new
                            {
                                u.Id,
                                u.Name,
                                u.Password,
                                u.PasswordSalt,
                                u.Role,
                                ud.Email,
                                ud.FirstName,
                                ud.LastName
                            }).FirstOrDefault();

                if (user != null && VerifyPassword(password, user.Password, user.PasswordSalt))
                {
                    return new
                    {
                        user.Id,
                        UserName = user.Name,
                        Role = user.Role,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName
                    };
                }

                return null;
            }
        }

        private bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt)
        {
            using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(enteredPassword, Convert.FromBase64String(storedSalt), 10000))
            {
                string hash = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(20));
                return hash == storedHash;
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
