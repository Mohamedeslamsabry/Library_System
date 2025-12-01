using Domain_Layer.Book_Models;
using Domain_Layer.Shared;

namespace Domain_Layer.Categories_Models
{
    public class Categories : BaseEntity
    {
        #region Proprties
        public string CategoryName { get; set; } = null!;

        #endregion

        #region RelationShips

        #region Book (R01) Classified
        public ICollection<Book> Book { get; set; } = new HashSet<Book>();
        #endregion

        #endregion
    }
}
