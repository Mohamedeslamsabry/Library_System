using Domain_Layer.Book_Models;
using Domain_Layer.Floors_Models;
using Domain_Layer.Shared;

namespace Domain_Layer.Shelf_Models
{
    public class Shelf : BaseEntity
    {
        #region Properies
        //public int Code { get; set; } //Id

        #endregion

        #region RelationShips

        #region Floors (R01) (Located)
        public Floors Floor { get; set; } = null!;
        public int FloorNumber { get; set; }
        #endregion

        #region Book (R02) Assigned
        public ICollection<Book> Book { get; set; } = new HashSet<Book>();  
      
        #endregion

        #endregion
    }
}
