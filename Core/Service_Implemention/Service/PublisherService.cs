using AutoMapper;
using Domain_Layer.Contract.UnitOfWork;
using Domain_Layer.Exceptions;
using Domain_Layer.Models.Puplishers_Models;
using Microsoft.EntityFrameworkCore;
using Service_Abstraction.Interfaces;
using Service_Implemention.Specification;
using Shared;
using Shared.DTO.Publisher;
using Shared.Error;

namespace Service_Implemention.Service
{
    public class PublisherService(IUnitOfWork _UnitOfWork, IMapper _mapper) : IPublisherService
    {
        #region GetAllAsync
        public async Task<PaginatedResult<PublisherDTO>> GetAllAsync(PublisherQueryParamter publisherQuery)
        {
            var Specification = new PublisherSpecifcation(publisherQuery);
            var publisher = await _UnitOfWork.GetRepoartory<Puplishers>().GetAllAsync(Specification);
            var publisherDto = _mapper.Map<IEnumerable<Puplishers>, IEnumerable<PublisherDTO>>(publisher);

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
            return Publisher == null ? throw new PublisherNotFoundException(Id) : _mapper.Map<PublisherDTO>(Publisher);
        }
        #endregion

        #region Create Publisher
        //public async Task<bool> CreateAsync(CreateOrUpdatePublisherDTO CreatePublisher)
        //{
        //    try
        //    {
        //        var Publisher = _mapper.Map<CreateOrUpdatePublisherDTO, Puplishers>(CreatePublisher);

        //        await _UnitOfWork.GetRepoartory<Puplishers>().AddAsync(Publisher);
        //        var isCreated = await _UnitOfWork.SaveChangesAsync() > 0;
        //        if (!isCreated)
        //        {
        //            return false;
        //        }
        //        else
        //        {
        //            return isCreated;
        //        }

        //    }
        //    catch (Exception)
        //    {

        //        return false;
        //    }
        //}

        public async Task<Result<int>> CreateAsync(CreateOrUpdatePublisherDTO createPublisher)
        {
            if (createPublisher is null)
                return Result<int>.Fail("Request body is missing.", ErrorCodes.ValidationNull);

            try
            {
                var publisher = _mapper.Map<CreateOrUpdatePublisherDTO, Puplishers>(createPublisher);

                await _UnitOfWork.GetRepoartory<Puplishers>().AddAsync(publisher);

                var saved = await _UnitOfWork.SaveChangesAsync() > 0;
                if (!saved)
                    return Result<int>.Fail("Failed to save changes.", ErrorCodes.DbSaveFailed);

                return Result<int>.Ok(publisher.Id, "Publisher created successfully.");
            }
            catch (OperationCanceledException)
            {
                return Result<int>.Fail("Operation was canceled.", ErrorCodes.Canceled);
            }
            catch (AutoMapperMappingException)
            {
                return Result<int>.Fail("Data mapping failed.", ErrorCodes.MappingError);
            }
            catch (DbUpdateException)
            {
                return Result<int>.Fail("Database update failed during create.", ErrorCodes.DbUpdateError);
            }
            catch (Exception)
            {
                return Result<int>.Fail("Unexpected error occurred.", ErrorCodes.Unexpected);
            }
        }

        #endregion

        #region Update Publisher
        //public async Task<bool> UpdateAsync(int Id, CreateOrUpdatePublisherDTO UpdatePublisher)
        //{
        //    try
        //    {
        //        var Repo = _UnitOfWork.GetRepoartory<Puplishers>();
        //        var Publosher = await Repo.GetByIdAsync(Id);
        //        if (Publosher is null) { return false; }


        //        _mapper.Map(UpdatePublisher, Publosher);
        //        Repo.Update(Publosher);
        //        return await _UnitOfWork.SaveChangesAsync() > 0;
        //    }
        //    catch (Exception)
        //    {

        //        return false;
        //    }
        //}


        public async Task<Result<int>> UpdateAsync(int id, CreateOrUpdatePublisherDTO updatePublisher)
        {
            if (updatePublisher is null)
                return Result<int>.Fail("Request body is missing.", ErrorCodes.ValidationNull);

            try
            {
                var repo = _UnitOfWork.GetRepoartory<Puplishers>();
                var publisher = await repo.GetByIdAsync(id);

                if (publisher is null)
                    return Result<int>.Fail("Publisher not found.", ErrorCodes.PublisherNotFound);

                _mapper.Map(updatePublisher, publisher);
                repo.Update(publisher);

                var saved = await _UnitOfWork.SaveChangesAsync() > 0;
                if (!saved)
                    return Result<int>.Fail("Failed to save changes.", ErrorCodes.DbSaveFailed);

                return Result<int>.Ok(publisher.Id, "Publisher updated successfully.");
            }
            catch (OperationCanceledException)
            {
                return Result<int>.Fail("Operation was canceled.", ErrorCodes.Canceled);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Result<int>.Fail("Concurrency conflict while updating.", ErrorCodes.ConcurrencyError);
            }
            catch (AutoMapperMappingException)
            {
                return Result<int>.Fail("Data mapping failed.", ErrorCodes.MappingError);
            }
            catch (DbUpdateException)
            {
                return Result<int>.Fail("Database update failed during update.", ErrorCodes.DbUpdateError);
            }
            catch (Exception)
            {
                return Result<int>.Fail("Unexpected error occurred.", ErrorCodes.Unexpected);
            }
        }
        #endregion

        #region Delete publisher
        //public async Task<bool> DeleteAsync(int Id)
        //{
        //    try
        //    {
        //        var Pubblisher = await _UnitOfWork.GetRepoartory<Puplishers>().GetByIdAsync(Id);
        //        if (Pubblisher is null) { return false; }



        //        if (Pubblisher.Book.Any())
        //        {
        //            foreach (var Book in Pubblisher.Book)
        //            {
        //                Book.puplisherId = null;
        //            }
        //        }

        //        _UnitOfWork.GetRepoartory<Puplishers>().Remove(Pubblisher);
        //        var IsRemoved = await _UnitOfWork.SaveChangesAsync() > 0;
        //        return IsRemoved;
        //    }
        //    catch (Exception)
        //    {

        //        return false;
        //    }
        //}


        public async Task<Result<int>> DeleteAsync(int id)
        {
            try
            {
                var publisherRepo = _UnitOfWork.GetRepoartory<Puplishers>();
                var publisher = await publisherRepo.GetByIdAsync(id);

                if (publisher is null)
                    return Result<int>.Fail("Publisher not found.", ErrorCodes.PublisherNotFound);

                // Detach related books by nulling        // Detach related books by nulling FK (if lazy loading is enabled or navs are loaded)
                if (publisher.Book?.Any() == true)
                {
                    foreach (var book in publisher.Book)
                        book.puplisherId = null;
                }

                publisherRepo.Remove(publisher);

                var saved = await _UnitOfWork.SaveChangesAsync() > 0;
                if (!saved)
                    return Result<int>.Fail("Failed to save changes.", ErrorCodes.DbSaveFailed, id);

                return Result<int>.Ok(id, "Publisher deleted successfully.");
            }
            catch (OperationCanceledException)
            {
                return Result<int>.Fail("Operation was canceled.", ErrorCodes.Canceled);
            }
            catch (DbUpdateException)
            {
                // Likely FK restriction or constraint
                return Result<int>.Fail("Database update failed during delete.", ErrorCodes.DbUpdateError);
            }
            catch (Exception)
            {
                return Result<int>.Fail("Unexpected error occurred.", ErrorCodes.Unexpected);
            }
        }
        #endregion
    }
}
