namespace Shared.DTO.Categories
{
    public class CategorieDTO
    {

        public int Id { get; set; }
        public string CategoryName { get; set; } = null!;

        // معلومات إضافية اختيارية
        public int BooksCount { get; set; }
        public List<int> BookIds { get; set; } = new();

    }
}
