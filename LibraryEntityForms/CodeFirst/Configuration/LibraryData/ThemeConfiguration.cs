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
    internal class ThemeConfiguration : IEntityTypeConfiguration<Themes>
    {
        public void Configure(EntityTypeBuilder<Themes> builder)
        {
            builder.Property(p => p.Id)
               .IsRequired()
               .ValueGeneratedOnAdd();

        }
    }
}
