using Domain_Layer.Book_Models;
using Domain_Layer.Shared;

namespace Domain_Layer.Puplishers_Models
{
    public class Puplishers : BaseEntity
    {
        #region Proprties
        public string Publisher_Name { get; set; } = null!;
        #endregion

        #region RelationShips

        #region Book (R01) Have
        public ICollection<Book> Book { get; set; } = new HashSet<Book>();
        #endregion

        #endregion
    }
}
