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
    internal class SCardConfiguration : IEntityTypeConfiguration<S_Cards>
    {
        public void Configure(EntityTypeBuilder<S_Cards> builder)
        {
            builder.Property(p => p.Id)
               .IsRequired()
               .ValueGeneratedOnAdd();

            builder.HasOne(p => p.Books)
                .WithMany(p => p.S_Cards)
                .HasForeignKey(p => p.Id_Book);

            builder.HasOne(p => p.Students)
                .WithMany(p => p.S_Cards)
                .HasForeignKey(p => p.Id_Student);

            builder.HasOne(p => p.Libs)
                .WithMany(p => p.S_Cards)
                .HasForeignKey(p => p.Id_Lib);

        }
    }
}
