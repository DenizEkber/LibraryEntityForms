using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using LibraryEntityForms.CodeFirst.Context;
using LibraryEntityForms.CodeFirst.Entity.LibraryData;
using LibraryEntityForms.CodeFirst.Entity.UserData; // Your entity models namespace


namespace LibraryEntityForms.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            /*using (LibraryContext context = new LibraryContext())
            {


                var categories = new List<Categories>
{
    new Categories { Name = "Fiction" },
    new Categories { Name = "Science Fiction" },
    new Categories { Name = "Fantasy" },
    new Categories { Name = "Mystery" },
    new Categories { Name = "Romance" },
    new Categories { Name = "Horror" },
    new Categories { Name = "Adventure" },
    new Categories { Name = "Thriller" },
    new Categories { Name = "Historical Fiction" },
    new Categories   { Name = "Biography" }
};

                context.Categories.AddRange(categories);
                context.SaveChanges();
                var faculties = new List<Faculties>
{
    new Faculties { Name = "Faculty of Arts" },
    new Faculties { Name = "Faculty of Science" },
    new Faculties { Name = "Faculty of Engineering" }
};

                context.Faculties.AddRange(faculties);
                context.SaveChanges();

                var departments = new List<Departments>
{
    new Departments { Name = "English Department" },
    new Departments { Name = "Mathematics Department" },
    new Departments { Name = "Computer Science Department" }
};

                context.Departments.AddRange(departments);
                context.SaveChanges();

                var libraries = new List<Libs>
{
    new Libs { FirstName = "Library 1", LastName = "Librarian 1" },
    new Libs { FirstName = "Library 2", LastName = "Librarian 2" },
    new Libs { FirstName = "Library 3", LastName = "Librarian 3" }
};

                context.Libraries.AddRange(libraries);
                context.SaveChanges();

                var presses = new List<Press>
{
    new Press { Name = "Press 1" },
    new Press { Name = "Press 2" },
    new Press { Name = "Press 3" }
};

                context.Presses.AddRange(presses);
                context.SaveChanges();

                var themes = new List<Themes>
{
    new Themes { Name = "Theme 1" },
    new Themes { Name = "Theme 2" },
    new Themes { Name = "Theme 3" }
};

                context.Themes.AddRange(themes);
                context.SaveChanges();

                var users = new List<Users>
{
    new Users { Name = "user1", Password = "password1", Role = Role.Admin, CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now },
    new Users { Name = "user2", Password = "password2", Role = Role.Teacher, CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now },
    new Users { Name = "user3", Password = "password3", Role = Role.Student, CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now }
};

                context.Users.AddRange(users);
                context.SaveChanges();


                var userDetails = new List<UserDetail>
{
    new UserDetail { FirstName = "John", LastName = "Doe", Email = "john@example.com", Id_User = 1, CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now },
    new UserDetail { FirstName = "Jane", LastName = "Smith", Email = "jane@example.com", Id_User = 2, CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now },
    new UserDetail { FirstName = "Michael", LastName = "Brown", Email = "michael@example.com", Id_User = 3, CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now }
};

                context.UserDetail.AddRange(userDetails);
                context.SaveChanges();


                var authors = new List<Authors>
{
    new Authors { FirstName = "Jane", LastName = "Austen" },
    new Authors { FirstName = "Charles", LastName = "Dickens" },
    new Authors { FirstName = "Leo", LastName = "Tolstoy" },
    new Authors { FirstName = "J.K.", LastName = "Rowling" },
    new Authors { FirstName = "Ernest", LastName = "Hemingway" },
    new Authors { FirstName = "Agatha", LastName = "Christie" },
    new Authors { FirstName = "Mark", LastName = "Twain" },
    new Authors { FirstName = "Gabriel", LastName = "García Márquez" },
    new Authors { FirstName = "Haruki", LastName = "Murakami" },
    new Authors { FirstName = "Terry", LastName = "Pratchett" }
};

                context.Authors.AddRange(authors);
                context.SaveChanges();

                var books = new List<Books>
{
    new Books
    {
        Name = "Pride and Prejudice",
        Pages = 279,
        YearPress = 1813,
        Id_Press =1,
        Id_Themes = 1,
        Id_Category = 1,
        Id_Author = 1,
        Comment = "A classic novel by Jane Austen.",
        Quantity = 4
    },
    new Books
    {
        Name = "War and Peace",
        Pages = 1225,
        YearPress = 1869,
        Id_Press =2,
        Id_Themes = 1,
        Id_Category = 1,
        Id_Author = 3,
        Comment = "Epic novel by Leo Tolstoy.",
        Quantity = 4
    },
    new Books
    {
        Name = "Harry Potter and the Philosopher's Stone",
        Pages = 320,
        YearPress = 1997,
        Id_Category = 1,
        Id_Press =3,
        Id_Themes = 1,
        Id_Author = 4,
        Comment = "First book in the Harry Potter series.",
        Quantity = 3
    },
    new Books
    {
        Name = "The Great Gatsby",
        Pages = 180,
        YearPress = 1925,
        Id_Press =1,
        Id_Themes = 1,
        Id_Category = 1,
        Id_Author = 5,
        Comment = "F. Scott Fitzgerald's masterpiece.",
        Quantity = 10
    },
    new Books
    {
        Name = "Murder on the Orient Express",
        Pages = 274,
        YearPress = 1934,
        Id_Press =2,
        Id_Themes = 1,
        Id_Category = 4,
        Id_Author = 6,
        Comment = "Classic detective fiction by Agatha Christie.",
        Quantity = 4
    },
    new Books
    {
        Name = "1984",
        Pages = 328,
        YearPress = 1949,
        Id_Press =3,
        Id_Themes = 1,
        Id_Category = 1,
        Id_Author = 7,
        Comment = "George Orwell's dystopian novel.",
        Quantity = 7
    },
    new Books
    {
        Name = "One Hundred Years of Solitude",
        Pages = 417,
        YearPress = 1967,
        Id_Press =1,
        Id_Themes = 1,
        Id_Category = 1,
        Id_Author = 8,
        Comment = "Magical realism novel by Gabriel García Márquez.",
        Quantity = 9
    },
    new Books
    {
        Name = "Norwegian Wood",
        Pages = 296,
        YearPress = 1987,
        Id_Press =2,
        Id_Themes = 1,
        Id_Category = 1,
        Id_Author = 9,
        Comment = "A novel by Haruki Murakami.",
        Quantity = 1
    },
    new Books
    {
        Name = "Good Omens",
        Pages = 369,
        YearPress = 1990,
        Id_Press =3,
        Id_Themes = 1,
        Id_Category = 2,
        Id_Author = 10,
        Comment = "Collaborative work by Terry Pratchett and Neil Gaiman.",
        Quantity = 9
    },
    new Books
    {
        Name = "The Adventures of Tom Sawyer",
        Pages = 224,
        YearPress = 1876,
        Id_Press =1,
        Id_Themes = 1,
        Id_Category = 1,
        Id_Author = 7,
        Comment = "Classic novel by Mark Twain.",
        Quantity = 10
    }
};

                context.Books.AddRange(books);
                context.SaveChanges();

                var groups = new List<Groups>
{
    new Groups { Name = "Group A", Id_Faculty = 1 },
    new Groups { Name = "Group B", Id_Faculty = 2 },
    new Groups { Name = "Group C", Id_Faculty = 3 },
    new Groups { Name = "Group D", Id_Faculty = 1 },
    new Groups { Name = "Group E", Id_Faculty = 2 },
    new Groups { Name = "Group F", Id_Faculty = 3 },
    new Groups { Name = "Group G", Id_Faculty = 1 },
    new Groups { Name = "Group H", Id_Faculty = 2 },
    new Groups { Name = "Group I", Id_Faculty = 3 },
    new Groups { Name = "Group J", Id_Faculty = 1 }
};

                context.Groups.AddRange(groups);
                context.SaveChanges();

                var students = new List<Students>
{
    new Students { FirstName = "John", LastName = "Smith", Id_Group = 1, Term = 2 },
    new Students { FirstName = "Emma", LastName = "Johnson", Id_Group = 2, Term = 3 },
    new Students { FirstName = "Michael", LastName = "Williams", Id_Group = 3, Term = 1 },
    new Students { FirstName = "Sophia", LastName = "Brown", Id_Group = 1, Term = 2 },
    new Students { FirstName = "James", LastName = "Jones", Id_Group = 2, Term = 3 },
    new Students { FirstName = "Olivia", LastName = "Davis", Id_Group = 3, Term = 1 },
    new Students { FirstName = "Robert", LastName = "Miller", Id_Group = 1, Term = 2 },
    new Students { FirstName = "Emily", LastName = "Wilson", Id_Group = 2, Term = 3 },
    new Students { FirstName = "William", LastName = "Moore", Id_Group = 3, Term = 1 },
    new Students { FirstName = "Ava", LastName = "Taylor", Id_Group = 1, Term = 2 }
};

                context.Students.AddRange(students);
                context.SaveChanges();

                var sCards = new List<S_Cards>
{
    new S_Cards { Id_Student = 1, Id_Book = 1, DataOut = DateTime.Now, DataIn = DateTime.Now.AddDays(14), Id_Lib = 1, TimeLimit = 14 },
    new S_Cards { Id_Student = 2, Id_Book = 3, DataOut = DateTime.Now, DataIn = DateTime.Now.AddDays(7), Id_Lib = 1, TimeLimit = 7 },
    new S_Cards { Id_Student = 3, Id_Book = 5, DataOut = DateTime.Now, DataIn = DateTime.Now.AddDays(21), Id_Lib = 1, TimeLimit = 21 },
    new S_Cards { Id_Student = 4, Id_Book = 7, DataOut = DateTime.Now, DataIn = DateTime.Now.AddDays(10), Id_Lib = 1, TimeLimit = 10 },
    new S_Cards { Id_Student = 5, Id_Book = 9, DataOut = DateTime.Now, DataIn = DateTime.Now.AddDays(14), Id_Lib = 1, TimeLimit = 14 },
    new S_Cards { Id_Student = 6, Id_Book = 2, DataOut = DateTime.Now, DataIn = DateTime.Now.AddDays(7), Id_Lib = 1, TimeLimit = 7 },
    new S_Cards { Id_Student = 7, Id_Book = 4, DataOut = DateTime.Now, DataIn = DateTime.Now.AddDays(21), Id_Lib = 1, TimeLimit = 21 },
    new S_Cards { Id_Student = 8, Id_Book = 6, DataOut = DateTime.Now, DataIn = DateTime.Now.AddDays(10), Id_Lib = 1, TimeLimit = 10 },
    new S_Cards { Id_Student = 9, Id_Book = 8, DataOut = DateTime.Now, DataIn = DateTime.Now.AddDays(14), Id_Lib = 1, TimeLimit = 14 },
    new S_Cards { Id_Student = 10, Id_Book = 10, DataOut = DateTime.Now, DataIn = DateTime.Now.AddDays(7), Id_Lib = 1, TimeLimit = 7 }
};

                context.S_Cards.AddRange(sCards);
                context.SaveChanges();

                var teachers = new List<Teachers>
{
    new Teachers { FirstName = "David", LastName = "Smith", Id_Dep = 1 },
    new Teachers { FirstName = "Sarah", LastName = "Johnson", Id_Dep = 2 },
    new Teachers { FirstName = "Paul", LastName = "Williams", Id_Dep = 3 },
    new Teachers { FirstName = "Jessica", LastName = "Brown", Id_Dep = 1 },
    new Teachers { FirstName = "Andrew", LastName = "Jones", Id_Dep = 2 },
    new Teachers { FirstName = "Jennifer", LastName = "Davis", Id_Dep = 3 },
    new Teachers { FirstName = "Michael", LastName = "Miller", Id_Dep = 1 },
    new Teachers { FirstName = "Emily", LastName = "Wilson", Id_Dep = 2 },
    new Teachers { FirstName = "Daniel", LastName = "Moore", Id_Dep = 3 },
    new Teachers { FirstName = "Linda", LastName = "Taylor", Id_Dep = 1 }
};

                context.Teachers.AddRange(teachers);
                context.SaveChanges();

                var tCards = new List<T_Cards>
{
    new T_Cards { Id_Teacher = 1, Id_Book = 1, DataOut = DateTime.Now, DataIn = DateTime.Now.AddDays(14), Id_Lib = 1, TimeLimit = 14 },
    new T_Cards { Id_Teacher = 2, Id_Book = 3, DataOut = DateTime.Now, DataIn = DateTime.Now.AddDays(7), Id_Lib = 1, TimeLimit = 7 },
    new T_Cards { Id_Teacher = 3, Id_Book = 5, DataOut = DateTime.Now, DataIn = DateTime.Now.AddDays(21), Id_Lib = 1, TimeLimit = 21 },
    new T_Cards { Id_Teacher = 4, Id_Book = 7, DataOut = DateTime.Now, DataIn = DateTime.Now.AddDays(10), Id_Lib = 1, TimeLimit = 10 },
    new T_Cards { Id_Teacher = 5, Id_Book = 9, DataOut = DateTime.Now, DataIn = DateTime.Now.AddDays(14), Id_Lib = 1, TimeLimit = 14 },
    new T_Cards { Id_Teacher = 6, Id_Book = 2, DataOut = DateTime.Now, DataIn = DateTime.Now.AddDays(7), Id_Lib = 1, TimeLimit = 7 },
    new T_Cards { Id_Teacher = 7, Id_Book = 4, DataOut = DateTime.Now, DataIn = DateTime.Now.AddDays(21), Id_Lib = 1, TimeLimit = 21 },
    new T_Cards { Id_Teacher = 8, Id_Book = 6, DataOut = DateTime.Now, DataIn = DateTime.Now.AddDays(10), Id_Lib = 1, TimeLimit = 10 },
    new T_Cards { Id_Teacher = 9, Id_Book = 8, DataOut = DateTime.Now, DataIn = DateTime.Now.AddDays(14), Id_Lib = 1, TimeLimit = 14 },
    new T_Cards { Id_Teacher = 10, Id_Book = 10, DataOut = DateTime.Now, DataIn = DateTime.Now.AddDays(7), Id_Lib = 1, TimeLimit = 7 }
};

                context.T_Cards.AddRange(tCards);
                context.SaveChanges();


            }*/
            /*using (LibraryContext ctx = new LibraryContext())
            {
                var authors = ctx.Authors.ToList();
                foreach (var auth in authors)
                {
                    Console.WriteLine($"{auth.FirstName}");
                }
            }*/
           /* using (LibraryContext ctx =new LibraryContext())
            {
                ctx.S_Cards.Add(new S_Cards { Id_Student = 10, Id_Book = 10, DataOut = DateTime.Now.AddDays(-20), DataIn = DateTime.Now.AddDays(7), Id_Lib = 1, TimeLimit = 7 });
            }*/
        }
    }
}