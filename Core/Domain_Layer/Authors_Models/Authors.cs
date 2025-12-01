using Domain_Layer.Book_Authors_Models;
using Domain_Layer.Shared;

namespace Domain_Layer.Authors_Models
{
    public class Authors : BaseEntity
    {
        #region Properties
        public string Auth_Name { get; set; } = null!;

        #endregion

        #region RelationShips

        #region Book_Authors (R01) Own
        public ICollection<Book_Authors> Book_Authors { get; set; } = new HashSet<Book_Authors>();

        #endregion

        #endregion
    }
}
