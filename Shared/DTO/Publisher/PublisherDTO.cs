namespace Shared.DTO.Publisher
{
    public class PublisherDTO
    {

        public int Id { get; set; }
        public string Publisher_Name { get; set; } = null!;
        public List<BookShortDto> Books { get; set; } = new();
    }


    public class BookShortDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
    }
}

