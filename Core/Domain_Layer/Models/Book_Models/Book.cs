using Domain_Layer.Models.Book_Authors_Models;
using Domain_Layer.Models.Borrow_Models;
using Domain_Layer.Models.Categories_Models;
using Domain_Layer.Models.Puplishers_Models;
using Domain_Layer.Models.Shared;
using Domain_Layer.Models.Shelf_Models;

namespace Domain_Layer.Models.Book_Models
{
    public class Book : BaseEntity
    {
        #region Properties
        public string TiTle { get; set; } = null!;

        #endregion

        #region RelationShips

        #region Shelf(R01) Assigned
        public virtual Shelf Shelf { get; set; } = null!;
        public int? ShelfId { get; set; }
        #endregion

        #region Book_Authors (R02) (Own)
        public virtual ICollection<Book_Authors> Book_Authors { get; set; } = new HashSet<Book_Authors>();
        #endregion

        #region Categories (R03) Classified
        public virtual Categories Category { get; set; } = null!;
        public int CategoryId { get; set; }
        #endregion

        #region Puplishers (R04) Have
        public virtual Puplishers puplisher { get; set; } = null!;
        public int puplisherId { get; set; }
        #endregion

        #region Borrow
        public virtual Borrow? Borrow { get; set; }
        #endregion

        #endregion
    }
}
