using Domain_Layer.Models.Floors_Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Model_Configration
{
    public class FloorConfigrations : IEntityTypeConfiguration<Floors>
    {
        public void Configure(EntityTypeBuilder<Floors> builder)
        {
            builder.Property(M => M.Id)
                .HasColumnName("FloorNumber")
               .UseIdentityColumn(100, 100);

            builder.Property(M => M.CreatedAt)
                .HasColumnName("HiringDate")
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(M => M.UpdatedAt)
                   .HasComputedColumnSql("GETDATE()");


            //builder.HasOne(E => E.EmployeeMange)
            //       .WithOne(U => U.Floors);

        }
    }
}
