using LibraryEntityForms.CodeFirst.Entity.UserData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEntityForms.CodeFirst.Configuration.UserData
{
    internal class UserDetailConfiguration : IEntityTypeConfiguration<UserDetail>
    {
        public void Configure(EntityTypeBuilder<UserDetail> builder)
        {
            builder.Property(p => p.Id)
               .IsRequired()
               .ValueGeneratedOnAdd();

            builder.HasOne(p => p.Users)
                .WithOne(p => p.UserDetails)
                .HasForeignKey<UserDetail>(p => p.Id_User);
        }
    }
}
