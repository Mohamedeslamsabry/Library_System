using Domain_Layer.Models.Book_Models;
using Domain_Layer.Models.Floors_Models;
using Domain_Layer.Models.Shared;

namespace Domain_Layer.Models.Shelf_Models
{
    public class Shelf : BaseEntity
    {
        #region Properies
        //public int Code { get; set; } //Id

        #endregion

        #region RelationShips

        #region Floors (R01) (Located)
        public virtual Floors Floor { get; set; } = null!;
        public int? FloorNumber { get; set; }
        #endregion

        #region Book (R02) Assigned
        public virtual ICollection<Book> Book { get; set; } = new HashSet<Book>();

        #endregion

        #endregion
    }
}
