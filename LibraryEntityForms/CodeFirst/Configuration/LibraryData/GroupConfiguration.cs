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
    internal class GroupConfiguration : IEntityTypeConfiguration<Groups>
    {
        public void Configure(EntityTypeBuilder<Groups> builder)
        {
            builder.Property(p => p.Id)
               .IsRequired()
               .ValueGeneratedOnAdd();

            builder.HasOne(p => p.Faculties)
                .WithMany(p => p.Groups)
                .HasForeignKey(p => p.Id_Faculty);
        }
    }
}
