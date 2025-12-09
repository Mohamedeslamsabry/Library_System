namespace Shared.DTO.Book
{
    public class CreateOrUpdateBookDto
    {
        public string Title { get; set; } = null!;
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public int? ShelfId { get; set; }
        public int? CategoryId { get; set; }
        public int? PublisherId { get; set; }
        // Authors
        public List<int>? AuthorIds { get; set; } = new();
    }
}
