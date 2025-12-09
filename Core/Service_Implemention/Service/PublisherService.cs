using AutoMapper;
using Domain_Layer.Contract.UnitOfWork;
using Domain_Layer.Models.Puplishers_Models;
using Service_Abstraction.Interfaces;
using Service_Implemention.Specification;
using Shared;
using Shared.DTO.Publisher;

namespace Service_Implemention.Service
{
    public class PublisherService(IUnitOfWork _UnitOfWork, IMapper _mapper) : IPublisherService
    {
        #region GetAllAsync
        public async Task<PaginatedResult<PublisherDTO>> GetAllAsync(PublisherQueryParamter publisherQuery)
        {
            var Specification = new PublisherSpecifcation(publisherQuery);
            var publisher = await _UnitOfWork.GetRepoartory<Puplishers>().GetAllAsync(Specification);
            var publisherDto =  _mapper.Map<IEnumerable<Puplishers>, IEnumerable<PublisherDTO>>(publisher);

            #region Paggention
            var spec = new PublisherCountSpecifcation(publisherQuery);
            var TotalCount = await _UnitOfWork.GetRepoartory<Puplishers>().CountAsync(spec);
            #endregion

            return new PaginatedResult<PublisherDTO>(TotalCount, publisher.Count(), publisherQuery.PageIndex, publisherDto);
            //var Publisher = await _UnitOfWork.GetRepoartory<Puplishers>().GetAllAsync();
            //if (Publisher is null)
            //{
            //    return Enumerable.Empty<PublisherDTO>();
            //}
            //else
            //{
            //    return _mapper.Map<IEnumerable<Puplishers>, IEnumerable<PublisherDTO>>(Publisher);
            //}
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

        #region Delete publisher
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
                        Book.puplisherId = null;
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
