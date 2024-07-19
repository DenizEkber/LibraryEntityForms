using LibraryEntityForms.CodeFirst.Entity.LibraryData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEntityForms.CodeFirst.Configuration.LibraryData
{
    internal class BooksConfiguration : IEntityTypeConfiguration<Books>
    {
        public void Configure(EntityTypeBuilder<Books> builder)
        {
            builder.Property(p => p.Id)
               .IsRequired()
               .ValueGeneratedOnAdd();

            builder.HasOne(p => p.Themes)
                .WithMany(p => p.Books)
                .HasForeignKey(p => p.Id_Themes);

            builder.HasOne(p => p.Categories)
                .WithMany(p => p.Books)
                .HasForeignKey(p => p.Id_Category);

            builder.HasOne(p => p.Authors)
                .WithMany(p => p.Books)
                .HasForeignKey(p => p.Id_Author);

            builder.HasOne(p => p.Press)
                .WithMany(p => p.Books)
                .HasForeignKey(p => p.Id_Press);

        }
    }
}
