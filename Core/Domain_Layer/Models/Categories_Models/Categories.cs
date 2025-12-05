using Domain_Layer.Models.Book_Models;
using Domain_Layer.Models.Shared;

namespace Domain_Layer.Models.Categories_Models
{
    public class Categories : BaseEntity
    {
        #region Proprties
        public string CategoryName { get; set; } = null!;

        #endregion

        #region RelationShips

        #region Book (R01) Classified
        public virtual ICollection<Book> Book { get; set; } = new HashSet<Book>();
        #endregion

        #endregion
    }
}
