using Domain_Layer.Models.Borrow_Models;
using Shared;

namespace Service_Implemention.Specification
{
    public class BorrowSpecification : BaseSpecification<Borrow>
    {
        public BorrowSpecification(BorrowQueryParamter borrowQuery) :
         base(null!)

        {
            ApplyPagention(borrowQuery.pageSize, borrowQuery.PageIndex);
        }
    }
}
