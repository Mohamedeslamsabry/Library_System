using Domain_Layer.Models.Book_Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Model_Configration
{
    public class BookConfigrations : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            #region Properties Configrations
            builder.Property(M => M.Id)
                  .UseIdentityColumn(100, 1);

            builder.Property(M => M.CreatedAt)
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(M => M.UpdatedAt)
                   .HasComputedColumnSql("GETDATE()");


            builder.Property(M => M.TiTle)
                .HasColumnType("varchar")
                .HasMaxLength(100);
            #endregion


            builder.HasOne(E => E.Shelf)
                .WithMany(U => U.Book)
                .HasForeignKey(U => U.ShelfId);


        }
    }
}
