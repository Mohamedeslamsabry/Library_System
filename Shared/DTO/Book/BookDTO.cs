namespace Shared.DTO.Book
{
    public class BookDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Title { get; set; } = null!;
        public decimal Price { get; set; }
        public int Amount { get; set; }
        public int? ShelfId { get; set; }
        //public string? ShelfName { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public int? PublisherId { get; set; }
        public string? PublisherName { get; set; }

        // Authors (IDs + أسماء اختيارياً)
        public List<int> AuthorIds { get; set; } = new();
        public List<string> AuthorNames { get; set; } = new();

        public int AuthorsCount { get; set; }

        // Borrows

        public int BorrowsCount { get; set; }
        public List<int> BorrowIds { get; set; } = new();
        public DateTime? LastBorrowDate { get; set; }
        public DateTime? LastDueDate { get; set; }
        public int OverdueCount { get; set; }

        // تقدير اختياري (لو عايزه): نشط غالبًا (آخر DueDate >= Today)
        public bool IsLikelyActive { get; set; }



        public bool HasActiveBorrow { get; set; }

    }
}
