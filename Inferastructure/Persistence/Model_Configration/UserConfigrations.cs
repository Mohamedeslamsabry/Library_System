using Domain_Layer.Users_Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Model_Configration
{
    public class UserConfigrations : IEntityTypeConfiguration<Users>
    {
        public void Configure(EntityTypeBuilder<Users> builder)
        {
            builder.Property(M => M.Id)
               .UseIdentityColumn(10, 10);

            builder.Property(M => M.CreatedAt)
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(M => M.UpdatedAt)
                   .HasComputedColumnSql("GETDATE()");


            builder.Property(M => M.User_Email)
                .HasColumnType("varchar")
                .HasMaxLength(100);


            builder.Property(M => M.User_Phone)
                .HasColumnType("varchar")
                .HasMaxLength(11);

            builder.ToTable(P =>
            {
                P.HasCheckConstraint("GymUserValidEmailCheck", "User_Email Like '_%@_%._%'");
                P.HasCheckConstraint("GymUserValidPhoneNumberCheck", "User_Phone Like '01%' and User_Phone Not Like '%[^0-9]%'");
            });
        }
    }
}
