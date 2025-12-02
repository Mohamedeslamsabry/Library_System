using Domain_Layer.Book_Authors_Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Model_Configration
{
    public class Book_Authors_Configrations : IEntityTypeConfiguration<Book_Authors>
    {
        public void Configure(EntityTypeBuilder<Book_Authors> builder)
        {

            builder.Property(M => M.CreatedAt)
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(M => M.UpdatedAt)
                   .HasComputedColumnSql("GETDATE()");

            builder.HasKey(X => new
                {
                    X.BookId,
                    X.AuthorId
                });

            builder.Ignore(X => X.Id);

        }
    }
}
