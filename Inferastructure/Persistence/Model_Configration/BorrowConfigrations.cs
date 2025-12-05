using Domain_Layer.Models.Borrow_Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Model_Configration
{
    public class BorrowConfigrations : IEntityTypeConfiguration<Borrow>
    {
        public void Configure(EntityTypeBuilder<Borrow> builder)
        {
            builder.Property(M => M.CreatedAt)
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(M => M.DateBorrow)
                   .HasDefaultValueSql("GETDATE()");


            builder.Property(M => M.UpdatedAt)
                   .HasComputedColumnSql("GETDATE()");

            builder.HasKey(X => new
            {
                X.BookId,
                X.UserId,
                X.DateBorrow
            });

            builder.Ignore(X => X.Id);

            builder.ToTable(P =>
            {
                P.HasCheckConstraint("ValidAmount", "Amount <= 3");
                P.HasCheckConstraint("CheckDate", "DateBorrow > DueDate");
            });



            builder.HasOne(b => b.Employee)
                   .WithMany()
                   .HasForeignKey(b => b.EmployeeId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(b => b.User)
                   .WithMany()
                   .HasForeignKey(b => b.UserId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(b => b.Book)
                   .WithMany()
                   .HasForeignKey(b => b.BookId)
                   .OnDelete(DeleteBehavior.NoAction);


        }
    }
}
