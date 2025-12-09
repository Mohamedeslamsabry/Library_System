using Domain_Layer.Models.Book_Models;
using Shared;

namespace Service_Implemention.Specification
{
    public class BookCountSpecification : BaseSpecification<Book>
    {
        public BookCountSpecification(BookQueryPartmer bookQuery) :
         base(P => string.IsNullOrEmpty(bookQuery.search) || P.Name.ToLower().Contains(bookQuery.search.ToLower()))

        {

        }
    }
}
