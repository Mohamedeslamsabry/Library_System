namespace Domain_Layer.Shared
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } 
        public DateTime UodatedAt { get; set; }
    }
}
