using LibraryEntityForms.CodeFirst.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace LibraryEntityForms.Server
{
    internal class BooksHandler
    {
        private LibraryContext ctx;
        public void Handle(HttpListenerContext context)
        {
            ctx = new LibraryContext();
            if (context.Request.HttpMethod.Equals("GET", StringComparison.OrdinalIgnoreCase))
            {
                var books = GetBooks();
                if (books != null)
                {
                    var responseJson = new { success = true, data = books };
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


        private IEnumerable<BookDto> GetBooks()
        {
            using (var ctx = new LibraryContext())
            {
                var data = (from book in ctx.Books
                            join author in ctx.Authors on book.Id_Author equals author.Id
                            join theme in ctx.Themes on book.Id_Themes equals theme.Id
                            join category in ctx.Categories on book.Id_Category equals category.Id
                            join press in ctx.Presses on book.Id_Press equals press.Id
                            select new BookDto
                            {
                                BookName = book.Name,
                                BookPages = book.Pages,
                                BookYearPress = book.YearPress,
                                BookComment = book.Comment,
                                BookQuantity = book.Quantity,
                                AuthorFirstName = author.FirstName,
                                AuthorLastName = author.LastName,
                                ThemeName = theme.Name,
                                CategoryName = category.Name,
                                PressName = press.Name,
                                PhotoData = book.PhotoData // Base64 encoded photo data
                            }).ToList();

                return data;
            }
        }
    }
    public class BookDto
    {
        public string BookName { get; set; }
        public int BookPages { get; set; }
        public int BookYearPress { get; set; }
        public string BookComment { get; set; }
        public int BookQuantity { get; set; }
        public string AuthorFirstName { get; set; }
        public string AuthorLastName { get; set; }
        public string ThemeName { get; set; }
        public string CategoryName { get; set; }
        public string PressName { get; set; }
        public byte[] PhotoData { get; set; } // Base64 string
    }
}