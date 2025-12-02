
using Domain_Layer.Authors_Models;
using Domain_Layer.Book_Authors_Models;
using Domain_Layer.Book_Models;
using Domain_Layer.Borrow_Models;
using Domain_Layer.Categories_Models;
using Domain_Layer.Employee_Models;
using Domain_Layer.Floors_Models;
using Domain_Layer.Puplishers_Models;
using Domain_Layer.Shelf_Models;
using Domain_Layer.Users_Models;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using System.Reflection;

namespace Persistence.Data.DbContexts
{
    public class LibraryDbContext(DbContextOptions<LibraryDbContext> options) : DbContext(options)
    {
        #region OnModelCreating
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
           
        }
        #endregion

        #region Dbsets

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Authors> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Floors> Floors { get; set; }
        public DbSet<Shelf> Shelves { get; set; }
        public DbSet<Puplishers> Puplishers { get; set; }
        public DbSet<Borrow> Borrows { get; set; }
        public DbSet<Book_Authors> Book_Authors { get; set; }


        #endregion
    }
}
