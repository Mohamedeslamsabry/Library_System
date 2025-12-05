using Domain_Layer.Models.Book_Models;
using Domain_Layer.Models.Shared;

namespace Domain_Layer.Models.Puplishers_Models
{
    public class Puplishers : BaseEntity
    {
        #region Proprties
        public string Publisher_Name { get; set; } = null!;
        #endregion

        #region RelationShips

        #region Book (R01) Have
        public virtual ICollection<Book> Book { get; set; } = new HashSet<Book>();
        #endregion

        #endregion
    }
}
