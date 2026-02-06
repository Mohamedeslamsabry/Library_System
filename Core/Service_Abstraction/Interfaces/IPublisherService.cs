using Shared;
using Shared.DTO.Publisher;
using Shared.Error;

namespace Service_Abstraction.Interfaces
{
    public interface IPublisherService
    {
        #region GetAllAsync
        Task<PaginatedResult<PublisherDTO>> GetAllAsync(PublisherQueryParamter publisherQuery);
        #endregion

        #region GetByIdAsync
        Task<PublisherDTO?> GetByIdAsync(int Id);
        #endregion

        #region CreateAsync
        Task<Result<int>> CreateAsync(CreateOrUpdatePublisherDTO CreatePublisher);
        #endregion

        #region UpdateAsync
        Task<Result<int>> UpdateAsync(int Id, CreateOrUpdatePublisherDTO updatePublisher);
        #endregion

        #region DeleteAsync
        Task<Result<int>> DeleteAsync(int Id);
        #endregion
    }
}
