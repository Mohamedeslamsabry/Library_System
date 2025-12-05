using Domain_Layer.Models.Authors_Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Model_Configration
{
    public class AuthorConfigrations : IEntityTypeConfiguration<Authors>
    {
        public void Configure(EntityTypeBuilder<Authors> builder)
        {
            builder.Property(M => M.Id)
              .UseIdentityColumn(1000, 1);


            builder.Property(M => M.Auth_Name)
                .HasColumnType("varchar")
                .HasMaxLength(100);

            builder.Property(M => M.CreatedAt)
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(M => M.UpdatedAt)
                   .HasComputedColumnSql("GETDATE()");
        }
    }
}
