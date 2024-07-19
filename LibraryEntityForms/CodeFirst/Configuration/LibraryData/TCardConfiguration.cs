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
    internal class TCardConfiguration : IEntityTypeConfiguration<T_Cards>
    {
        public void Configure(EntityTypeBuilder<T_Cards> builder)
        {
            builder.Property(p => p.Id)
               .IsRequired()
               .ValueGeneratedOnAdd();

            builder.HasOne(p => p.Books)
                .WithMany(p => p.T_Cards)
                .HasForeignKey(p => p.Id_Book);

            builder.HasOne(p => p.Libs)
                .WithMany(P => P.T_Cards)
                .HasForeignKey(P => P.Id_Lib);

            builder.HasOne(p => p.Teachers)
                .WithMany(p => p.T_Cards)
                .HasForeignKey(p => p.Id_Teacher);


        }
    }
}
