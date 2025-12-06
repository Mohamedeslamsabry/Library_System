namespace Shared
{
    public class PaginatedResult<TEntity>
    {
        public PaginatedResult(int totalCount, int pageSize, int pageIndex, IEnumerable<TEntity> data)
        {
            count = totalCount;
            PageSize = pageSize;
            PageIndex = pageIndex;
            Data = data;
        }

        public int count { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public IEnumerable<TEntity> Data { get; set; }
    }
}
