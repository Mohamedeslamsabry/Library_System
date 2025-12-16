namespace Shared.DTO.Categories
{
    public class CategorieDTO
    {

        public int Id { get; set; }
        public string CategoryName { get; set; } = null!;

        public int BooksCount { get; set; }
        public List<int> BookIds { get; set; } = new();

    }
}
