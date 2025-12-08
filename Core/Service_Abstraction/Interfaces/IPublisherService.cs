using Shared.DTO.Publisher;

namespace Service_Abstraction.Interfaces
{
    public interface IPublisherService
    {
        #region GetAllAsync
        Task<IEnumerable<PublisherDTO>> GetAllAsync();
        #endregion

        #region GetByIdAsync
        Task<PublisherDTO?> GetByIdAsync(int Id);
        #endregion

        #region CreateAsync
        Task<bool> CreateAsync(CreateOrUpdatePublisherDTO CreatePublisher);
        #endregion

        #region UpdateAsync
        Task<bool> UpdateAsync(int Id, CreateOrUpdatePublisherDTO updatePublisher);
        #endregion

        #region DeleteAsync
        Task<bool> DeleteAsync(int Id);
        #endregion
    }
}
