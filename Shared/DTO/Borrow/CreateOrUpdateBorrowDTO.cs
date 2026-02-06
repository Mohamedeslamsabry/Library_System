namespace Shared.DTO.Borrow
{
    public class CreateOrUpdateBorrowDTO
    {
        public int UserId { get; set; }
        public int? EmployeeId { get; set; }
        public int BookId { get; set; }
        public int Amount { get; set; }
        public DateTime DateBorrow { get; set; }
        public DateTime DueDate { get; set; }


    }
}
