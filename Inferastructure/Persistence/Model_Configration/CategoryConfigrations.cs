using Domain_Layer.Models.Categories_Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Model_Configration
{
    public class CategoryConfigrations : IEntityTypeConfiguration<Categories>
    {
        public void Configure(EntityTypeBuilder<Categories> builder)
        {
            builder.Property(M => M.Id)
               .UseIdentityColumn(1, 1);

            builder.Property(M => M.CreatedAt)
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(M => M.UpdatedAt)
                   .HasComputedColumnSql("GETDATE()");


            builder.Property(M => M.CategoryName)
                .HasColumnType("varchar")
                .HasMaxLength(100);

        }
    }
}
