namespace Shared
{
    public class EmployeeQueryParamter
    {
        public EmployeeSortingSpecifications sort { get; set; }
        public string? search { get; set; }

        #region pagention
        private const int DeafultPageSize = 5;
        private const int MaxPageSize = 10;
        public int PageIndex { get; set; } = 1;

        private int PageSize = DeafultPageSize;

        public int pageSize
        {
            get { return PageSize; }
            set { PageSize = value > MaxPageSize ? DeafultPageSize : value; }
        }
        #endregion
    }
}
