namespace Shared.Error
{

    public class Result<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? ErrorCode { get; set; }
        public T? Id { get; set; }

        public static Result<T> Ok(T? data = default, string message = "")
            => new Result<T> { Success = true, Message = message, Id = data };

        public static Result<T> Fail(string message, string? errorCode = null, T? data = default)
            => new Result<T> { Success = false, Message = message, ErrorCode = errorCode, Id = data };
    }

    public static class ErrorCodes
    {


        public const string ValidationNull = "VALIDATION_NULL";
        public const string ManagerNotFound = "MANAGER_NOT_FOUND";
        public const string FloorNotFound = "FLOOR_NOT_FOUND";
        public const string EmployeeNotFound = "EMPLOYEE_NOT_FOUND";
        public const string AmountInvalid = "AMOUNT_INVALID";
        public const string DueDateInvalid = "DUE_DATE_INVALID";
        public const string UserNotFound = "USER_NOT_FOUND";

        public const string AuthorNotFound = "AUTHOR_NOT_FOUND";

        public const string AuthorDuplicate = "AUTHOR_DUPLICATE_NAME";


        public const string StockInsufficient = "STOCK_INSUFFICIENT";

        public const string OldBorrowNotFound = "BORROW_OLD_NOT_FOUND";

        public const string BorrowDuplicate = "BORROW_DUPLICATE";

        public const string BorrowNotFound = "BORROW_NOT_FOUND";

        // Validation
        public const string NameRequired = "VALIDATION_NAME_REQUIRED";
        public const string InvalidForeignKey = "INVALID_FOREIGN_KEY";

        // Not Found / Duplicates
        public const string BookNotFound = "BOOK_NOT_FOUND";
        public const string BookDuplicateName = "BOOK_DUPLICATE_NAME";
        public const string PublisherNotFound = "PUBLISHER_NOT_FOUND";
        public const string CategoryNotFound = "CATEGORY_NOT_FOUND";
        public const string ShelfNotFound = "SHELF_NOT_FOUND";

        // Business rules
        public const string BorrowExists = "BORROW_EXISTS";

        // Persistence
        public const string DbSaveFailed = "DB_SAVE_FAILED";
        public const string DbUpdateError = "DB_UPDATE_ERROR";
        public const string ConcurrencyError = "DB_CONCURRENCY";

        // Mapping / Unexpected
        public const string MappingError = "MAPPING_ERROR";
        public const string Canceled = "    public const string Canceled";
        public const string Unexpected = "UNEXPECTED_ERROR";

    }
}