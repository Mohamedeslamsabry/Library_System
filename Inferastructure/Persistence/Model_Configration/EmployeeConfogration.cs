using Domain_Layer.Models.Employee_Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Model_Configration
{
    public class EmployeeConfogration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            #region Configration Propeties

            builder.Property(M => M.Id)
               .UseIdentityColumn(1, 1);

            builder.Property(M => M.CreatedAt)
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(M => M.UpdatedAt)
                   .HasComputedColumnSql("GETDATE()");


            builder.Property(M => M.Email)
                .HasColumnType("varchar")
                .HasMaxLength(100);


            builder.Property(M => M.PhoneNumber)
                .HasColumnType("varchar")
                .HasMaxLength(11);

            builder.ToTable(P =>
            {
                P.HasCheckConstraint("GymUserValidEmailCheck", "Email Like '_%@_%._%'");
                P.HasCheckConstraint("GymUserValidPhoneNumberCheck", "PhoneNumber Like '01%' and PhoneNumber Not Like '%[^0-9]%'");
                P.HasCheckConstraint("ValidSalary", "Salary >= 4000");
                //P.HasCheckConstraint("ValidBouns", "Bouns Between 100 and 10000");
            });

            builder.Property(M => M.FirstName)
                   .HasColumnType("varchar")
                   .HasMaxLength(50);

            builder.Property(M => M.LastName)
                   .HasColumnType("varchar")
                   .HasMaxLength(50);

            builder.HasIndex(M => M.Email).IsUnique(); //Unique Non ClusterdIndex
            builder.HasIndex(M => M.PhoneNumber).IsUnique(); //Unique Non ClusterdIndex

            builder.OwnsOne(M => M.Address, Address =>
            {
                Address.WithOwner();

                Address.Property(a => a.City)
                       .HasColumnName("city")
                       .HasColumnType("varchar")
                       .HasMaxLength(30);

                Address.Property(a => a.Country)
                       .HasColumnName("Country")
                       .HasColumnType("varchar")
                       .HasMaxLength(30);

                Address.Property(a => a.Street)
                       .HasColumnName("Street")
                       .HasColumnType("varchar")
                       .HasMaxLength(30);

                Address.Property(a => a.Area)
                       .HasColumnName("Area")
                       .HasColumnType("varchar")
                       .HasMaxLength(30);

                Address.Property(a => a.BuildingNumber)
                       .HasColumnName("BuildingNumber");

            });

            #endregion

            #region Configration RelationShips

            builder.HasMany(E => E.Users)
                .WithOne(U => U.Employee)
                .HasForeignKey(U => U.EmployeeId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(E => E.Floors)
                   .WithMany(F => F.employeesWork)
                   .HasForeignKey(E => E.FloorsNumber)
                   .OnDelete(DeleteBehavior.SetNull);

            //?
            builder.HasOne(E => E.FloorsMange)
                  .WithOne(F => F.EmployeeMange)
                   .OnDelete(DeleteBehavior.SetNull);


            builder.HasOne(E => E.Supervisor)
                   .WithMany(F => F.Subordinates)
                   .HasForeignKey(x => x.SupervisorId)
                   .OnDelete(DeleteBehavior.Restrict);





            #endregion
        }
    }
}
