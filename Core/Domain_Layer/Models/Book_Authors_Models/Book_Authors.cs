using Domain_Layer.Models.Authors_Models;
using Domain_Layer.Models.Book_Models;
using Domain_Layer.Models.Shared;

namespace Domain_Layer.Models.Book_Authors_Models
{
    public class Book_Authors : BaseEntity
    {
        #region RelationShips (Own) M-M
        public  virtual Book Book { get; set; } = null!;
        public int BookId { get; set; }

        public virtual Authors Author { get; set; } = null!;
        public int AuthorId { get; set; }
        #endregion
    }
}
