namespace Shared.DTO.Shelf
{
    public class ShelfDTO
    {
        public int Id { get; init; }
        public int FloorNumber { get; init; }
        public int BooksCount { get; init; }
        public IReadOnlyList<BookBriefDto> Books { get; init; } = Array.Empty<BookBriefDto>();
        public FloorBriefDto? Floor { get; init; }

    }

    public sealed class BookBriefDto
    {
        public int Id { get; init; }
        public string Title { get; init; } = default!;     
    }

    public sealed class FloorBriefDto
    {
        public int FloorNumber { get; init; }
        public int Number_of_Blocks { get; init; }
    }
}
