using Domain_Layer.Models.Puplishers_Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Model_Configration
{
    public class PuplishersConfigrations : IEntityTypeConfiguration<Puplishers>
    {
        public void Configure(EntityTypeBuilder<Puplishers> builder)
        {

            builder.Property(M => M.Id)
                   .UseIdentityColumn(5, 10);

            builder.Property(M => M.CreatedAt)
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(M => M.UpdatedAt)
                   .HasComputedColumnSql("GETDATE()");


            builder.Property(M => M.Publisher_Name)
                .HasColumnType("varchar")
                .HasMaxLength(100);

        }
    }
}
