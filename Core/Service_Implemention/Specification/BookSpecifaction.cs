using Domain_Layer.Models.Book_Models;
using Shared;

namespace Service_Implemention.Specification
{
    public class BookSpecifaction : BaseSpecification<Book>

    {
        public BookSpecifaction(BookQueryPartmer bookQuery) :
            base(P => string.IsNullOrEmpty(bookQuery.search) || P.Name.ToLower().Contains(bookQuery.search.ToLower()))


        {

            switch (bookQuery.sort)
            {
                case BookSorting.NameAsc:
                    SetOrdery(P => P.Name);
                    break;
                case BookSorting.NameDesc:
                    SetOrderyDesc(P => P.Name);
                    break;
                case BookSorting.PriceAsc:
                    SetOrdery(P => P.Price);
                    break;
                case BookSorting.PriceDesc:
                    SetOrderyDesc(P => P.Price);
                    break;
                default:
                    break;
            }

            ApplyPagention(bookQuery.pageSize, bookQuery.PageIndex);
        }
    }
}
