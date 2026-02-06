namespace Shared.DTO.authors
{
    public class AuthorDTO
    {

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public int BooksCount { get; set; }
        public List<int> BookIds { get; set; } = new();
    }

}

