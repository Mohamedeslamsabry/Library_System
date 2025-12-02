using Domain_Layer.Shelf_Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Model_Configration
{
    public class ShelfConfigrations : IEntityTypeConfiguration<Shelf>
    {
        public void Configure(EntityTypeBuilder<Shelf> builder)
        {
            builder.Property(M => M.Id)
                .HasColumnName("Code")
                .UseIdentityColumn(10, 1);

            builder.Property(M => M.CreatedAt)
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(M => M.UpdatedAt)
                   .HasComputedColumnSql("GETDATE()");

            builder.HasOne(E => E.Floor)
              .WithMany(U => U.Shelfs)
              .HasForeignKey(U => U.FloorNumber);


        }
    }
}
