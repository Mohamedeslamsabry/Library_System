using Microsoft.EntityFrameworkCore;

namespace Domain_Layer.Models.Shared
{
    [Owned]
    public class Address
    {
        #region Properties
        public string City { get; set; } = null!;
        public string Country { get; set; } = null!;
        public int BuildingNumber { get; set; } 
        #endregion
    }
}
