namespace Shared.DTO.Borrow
{
    public class BorrowDTO
    {

        //public int Id { get; set; }
        public DateTime DateBorrow { get; set; }
        public DateTime DueDate { get; set; }
        public int Amount { get; set; }

        public int UserId { get; set; }
        public string? UserName { get; set; }

        public int? EmployeeId { get; set; }
        public string? EmployeeName { get; set; }

        public int BookId { get; set; }
        public string? BookTitle { get; set; }


    }
}
