using Domain_Layer.Book_Authors_Models;
using Domain_Layer.Categories_Models;
using Domain_Layer.Puplishers_Models;
using Domain_Layer.Shared;
using Domain_Layer.Shelf_Models;

namespace Domain_Layer.Book_Models
{
    public class Book : BaseEntity
    {
        #region Properties
        public string TiTle { get; set; } = null!;

        #endregion

        #region RelationShips

        #region Shelf(R01) Assigned
        public Shelf Shelf { get; set; } = null!;
        public int ShelfId { get; set; }
        #endregion

        #region Book_Authors (R02) (Own)
        public ICollection<Book_Authors> Book_Authors { get; set; } = new HashSet<Book_Authors>();
        #endregion

        #region Categories (R03) Classified
        public Categories Category { get; set; } = null!;
        public int CategoryId { get; set; }
        #endregion

        #region Puplishers (R04) Have
        public Puplishers puplisher { get; set; } = null!;
        public int puplisherId { get; set; }
        #endregion  

        #endregion
    }
}
