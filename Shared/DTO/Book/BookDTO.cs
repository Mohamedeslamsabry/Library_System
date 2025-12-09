namespace Shared.DTO.Book
{
    public class BookDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Title { get; set; } = null!;
        public decimal Price { get; set; }
        public int? ShelfId { get; set; }
        //public string? ShelfName { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public int? PublisherId { get; set; }
        public string? PublisherName { get; set; }

        // Authors (IDs + أسماء اختيارياً)
        public List<int> AuthorIds { get; set; } = new();
        public List<string> AuthorNames { get; set; } = new();

        // حالة الاستعارة (اختياري)
        public bool IsBorrowed { get; set; }
        public int? BorrowId { get; set; }
    }
}
