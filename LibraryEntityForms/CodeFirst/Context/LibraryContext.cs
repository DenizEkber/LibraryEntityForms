using LibraryEntityForms.CodeFirst.Entity.LibraryData;
using LibraryEntityForms.CodeFirst.Entity.UserData;
using LibraryEntityForms.CodeFirst.Configuration.LibraryData;
using LibraryEntityForms.CodeFirst.Configuration.UserData;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEntityForms.CodeFirst.Context
{
    public class LibraryContext : DbContext
    {

        private string connectionString = "Server=USER\\SQLEXPRESS;Database=LibraryDb;Trusted_Connection=True;TrustServerCertificate=True;";
        public DbSet<Authors> Authors { get; set; }
        public DbSet<Books> Books { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Departments> Departments { get; set; }
        public DbSet<Faculties> Faculties { get; set; }
        public DbSet<Groups> Groups { get; set; }
        public DbSet<Libs> Libraries { get; set; }
        public DbSet<Press> Presses { get; set; }
        public DbSet<S_Cards> S_Cards { get; set; }
        public DbSet<Students> Students { get; set; }
        public DbSet<T_Cards> T_Cards { get; set; }
        public DbSet<Teachers> Teachers { get; set; }
        public DbSet<Themes> Themes { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<UserDetail> UserDetail { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            /*modelBuilder.Entity<Books>()
                 .HasOne(b => b.Authors)
                 .WithMany(a => a.Books)
                 .HasForeignKey(b => b.Id_Author);

            modelBuilder.Entity<Books>()
                .HasOne(b => b.Categories)
                .WithMany(c => c.Books)
                .HasForeignKey(b => b.Id_Category);

            modelBuilder.Entity<Books>()
                .HasOne(b => b.Press)
                .WithMany(p => p.Books)
                .HasForeignKey(b => b.Id_Press);

            modelBuilder.Entity<Books>()
                .HasOne(b => b.Themes)
                .WithMany(t => t.Books)
                .HasForeignKey(b => b.Id_Themes);

            

            modelBuilder.Entity<Students>()
                .HasOne(s => s.Groups)
                .WithMany(g => g.Students)
                .HasForeignKey(s => s.Id_Group);

            modelBuilder.Entity<Teachers>()
                .HasOne(t => t.Departments)
                .WithMany(d => d.Teachers)
                .HasForeignKey(t => t.Id_Dep);

            modelBuilder.Entity<S_Cards>()
                .HasOne(sc => sc.Students)
                .WithMany(s => s.S_Cards)
                .HasForeignKey(sc => sc.Id_Student);

            modelBuilder.Entity<S_Cards>()
                .HasOne(sc => sc.Books)
                .WithMany(b => b.S_Cards)
                .HasForeignKey(sc => sc.Id_Book);

            modelBuilder.Entity<S_Cards>()
                .HasOne(sc => sc.Libs)
                .WithMany(l => l.S_Cards)
                .HasForeignKey(sc => sc.Id_Lib);

            modelBuilder.Entity<T_Cards>()
                .HasOne(tc => tc.Teachers)
                .WithMany(t => t.T_Cards)
                .HasForeignKey(tc => tc.Id_Teacher);

            modelBuilder.Entity<T_Cards>()
                .HasOne(tc => tc.Books)
                .WithMany(b => b.T_Cards)
                .HasForeignKey(tc => tc.Id_Book);

            modelBuilder.Entity<T_Cards>()
                .HasOne(tc => tc.Libs)
                .WithMany(l => l.T_Cards)
                .HasForeignKey(tc => tc.Id_Lib);

            modelBuilder.Entity<UserDetail>()
                .HasOne(ud => ud.Users)
                .WithMany(u => u.UserDetails)
                .HasForeignKey(ud => ud.Id_User);*/

            modelBuilder.ApplyConfiguration(new AuthorConfiguration());
            modelBuilder.ApplyConfiguration(new BooksConfiguration());
            modelBuilder.ApplyConfiguration(new CategoriesConfiguration());
            modelBuilder.ApplyConfiguration(new DepartmentsConfguration());
            modelBuilder.ApplyConfiguration(new FacuiltyConfiguration());
            modelBuilder.ApplyConfiguration(new GroupConfiguration());
            modelBuilder.ApplyConfiguration(new LibConfiguration());
            modelBuilder.ApplyConfiguration(new PresConfiguration());
            modelBuilder.ApplyConfiguration(new SCardConfiguration());
            modelBuilder.ApplyConfiguration(new StudentConfiguration());
            modelBuilder.ApplyConfiguration(new TCardConfiguration());
            modelBuilder.ApplyConfiguration(new TeacherConfiguration());
            modelBuilder.ApplyConfiguration(new ThemeConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserDetailConfiguration());
        }
    }
}
