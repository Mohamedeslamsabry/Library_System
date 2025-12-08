using AutoMapper;
using Domain_Layer.Contract.UnitOfWork;
using Domain_Layer.Models.Employee_Models;
using Domain_Layer.Models.Floors_Models;
using Domain_Layer.Models.Puplishers_Models;
using Domain_Layer.Models.Shelf_Models;
using Service_Abstraction.Interfaces;
using Shared.DTO.Floor;
using Shared.DTO.Publisher;

namespace Service_Implemention.Service
{
    public class PublisherService(IUnitOfWork _UnitOfWork, IMapper _mapper) : IPublisherService
    {
        #region GetAllAsync
        public async Task<IEnumerable<PublisherDTO>> GetAllAsync()
        {
            var Publisher = await _UnitOfWork.GetRepoartory<Puplishers>().GetAllAsync();
            if (Publisher is null)
            {
                return Enumerable.Empty<PublisherDTO>();
            }
            else
            {
                return _mapper.Map<IEnumerable<Puplishers>, IEnumerable<PublisherDTO>>(Publisher);
            }
        }

        #endregion

        #region GetByIdAsync
        public async Task<PublisherDTO?> GetByIdAsync(int Id)
        {
            var Publisher = await _UnitOfWork.GetRepoartory<Puplishers>().GetByIdAsync(Id);
            return Publisher == null ? null : _mapper.Map<PublisherDTO>(Publisher);
        }
        #endregion

        #region Create Publisher
        public async Task<bool> CreateAsync(CreateOrUpdatePublisherDTO CreatePublisher)
        {
            try
            {
                var Publisher = _mapper.Map<CreateOrUpdatePublisherDTO, Puplishers>(CreatePublisher);

                await _UnitOfWork.GetRepoartory<Puplishers>().AddAsync(Publisher);
                var isCreated = await _UnitOfWork.SaveChangesAsync() > 0;
                if (!isCreated)
                {
                    return false;
                }
                else
                {
                    return isCreated;
                }

            }
            catch (Exception)
            {

                return false;
            }
        }

        #endregion

        #region Update Publisher
        public async Task<bool> UpdateAsync(int Id, CreateOrUpdatePublisherDTO UpdatePublisher)
        {
            try
            {
                var Repo = _UnitOfWork.GetRepoartory<Puplishers>();
                var Publosher = await Repo.GetByIdAsync(Id);
                if (Publosher is null) { return false; }


                _mapper.Map(UpdatePublisher, Publosher);
                Repo.Update(Publosher);
                return await _UnitOfWork.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {

                return false;
            }
        }

        #endregion

        #region Delete Floor
        public async Task<bool> DeleteAsync(int Id)
        {
            try
            {
                var Pubblisher = await _UnitOfWork.GetRepoartory<Puplishers>().GetByIdAsync(Id);
                if (Pubblisher is null) { return false; }



                if (Pubblisher.Book.Any())
                {
                    foreach (var Book in Pubblisher.Book)
                    {
                        Book.ShelfId = null;
                    }
                }

                _UnitOfWork.GetRepoartory<Puplishers>().Remove(Pubblisher);
                var IsRemoved = await _UnitOfWork.SaveChangesAsync() > 0;
                return IsRemoved;
            }
            catch (Exception)
            {

                return false;
            }
        }
        #endregion
    }
}
