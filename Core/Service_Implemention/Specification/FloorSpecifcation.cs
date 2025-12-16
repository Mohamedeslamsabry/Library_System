using Domain_Layer.Models.Floors_Models;
using Domain_Layer.Models.Shelf_Models;
using Shared;

namespace Service_Implemention.Specification
{
    public class FloorSpecifcation : BaseSpecification<Floors>
    {
        public FloorSpecifcation(FloorQueryParamter floorQuery) :
         base(null!)
        {
            ApplyPagention(floorQuery.pageSize, floorQuery.PageIndex);

        }
    }

    public class ShelfSpecifcation : BaseSpecification<Shelf>
    {
        public ShelfSpecifcation(ShelfQueryParamter shelfQuery) :
         base(null!)
        {
            ApplyPagention(shelfQuery.pageSize, shelfQuery.PageIndex);

        }
    }
}
