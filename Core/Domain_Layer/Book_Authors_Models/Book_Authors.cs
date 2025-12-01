using Domain_Layer.Authors_Models;
using Domain_Layer.Book_Models;
using Domain_Layer.Shared;

namespace Domain_Layer.Book_Authors_Models
{
    public class Book_Authors : BaseEntity
    {
        #region RelationShips (Own) M-M
        public Book Book { get; set; } = null!;
        public int BookId { get; set; }

        public Authors Author { get; set; } = null!;
        public int AuthorId { get; set; }
        #endregion
    }
}
