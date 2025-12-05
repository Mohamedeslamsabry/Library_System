using Domain_Layer.Models.Book_Authors_Models;
using Domain_Layer.Models.Shared;

namespace Domain_Layer.Models.Authors_Models
{
    public class Authors : BaseEntity
    {
        #region Properties
        public string Auth_Name { get; set; } = null!;

        #endregion

        #region RelationShips

        #region Book_Authors (R01) Own
        public virtual ICollection<Book_Authors> Book_Authors { get; set; } = new HashSet<Book_Authors>();

        #endregion

        #endregion
    }
}
