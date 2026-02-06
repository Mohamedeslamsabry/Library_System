using AutoMapper;
using Domain_Layer.Contract.UnitOfWork;
using Domain_Layer.Exceptions;
using Domain_Layer.Models.Authors_Models;
using Microsoft.EntityFrameworkCore;
using Service_Abstraction.Interfaces;
using Service_Implemention.Specification;
using Shared;
using Shared.DTO.authors;
using Shared.Error;

namespace Service_Implemention.Service
{
    public class AuthorService(IMapper _mapper, IUnitOfWork _UnitOfWork) : IAuthorsService
    {
        #region GetAllAsync
        public async Task<PaginatedResult<AuthorDTO>> GetAllAsync(AuthorsQueryParamter authorsQuery)
        {
            var Specification = new AuthorSpecification(authorsQuery);
            var Author = await _UnitOfWork.GetRepoartory<Authors>().GetAllAsync(Specification);
            var AuthorDto = _mapper.Map<IEnumerable<Authors>, IEnumerable<AuthorDTO>>(Author);

            #region Paggention
            var spec = new AuthorsCountSpecifaction(authorsQuery);
            var TotalCount = await _UnitOfWork.GetRepoartory<Authors>().CountAsync(spec);
            #endregion

            return new PaginatedResult<AuthorDTO>(TotalCount, Author.Count(), authorsQuery.PageIndex, AuthorDto);
        }

        #endregion

        #region GetByIdAsync
        public async Task<AuthorDTO?> GetByIdAsync(int Id)
        {
            var author = await _UnitOfWork.GetRepoartory<Authors>().GetByIdAsync(Id);
            return author == null ? throw new AuthorNotFoundException(Id) : _mapper.Map<AuthorDTO>(author);
        }
        #endregion

        #region Create Author
        public async Task<Result<int>> CreateAsync(CreateOrUpdateAuthorDTO createAuthor)
        {
            if (createAuthor is null)
                return Result<int>.Fail("Request body is missing.", ErrorCodes.ValidationNull);

            if (string.IsNullOrWhiteSpace(createAuthor.Name))
                return Result<int>.Fail("Author name is required.", ErrorCodes.NameRequired);

            try
            {
                var authorsRepo = _UnitOfWork.GetRepoartory<Authors>();

                var normalized = createAuthor.Name.Trim().ToLower();
                var exists = await authorsRepo.AnyAsync(a => a.Auth_Name.ToLower() == normalized);
                if (exists)
                    return Result<int>.Fail("The author's name already exists.", ErrorCodes.AuthorDuplicate);

                var author = _mapper.Map<CreateOrUpdateAuthorDTO, Authors>(createAuthor);
                await authorsRepo.AddAsync(author);

                var saved = await _UnitOfWork.SaveChangesAsync() > 0;
                if (!saved)
                    return Result<int>.Fail("Failed to save changes.", ErrorCodes.DbSaveFailed);

                return Result<int>.Ok(author.Id, "Author created successfully.");
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
                return Result<int>.Fail("An unexpected error occurred.", ErrorCodes.Unexpected);
            }
        }
        #endregion

        #region Update Author
        public async Task<Result<int>> UpdateAsync(int id, CreateOrUpdateAuthorDTO updateAuthor)
        {
            if (updateAuthor is null)
                return Result<int>.Fail("Request body is missing.", ErrorCodes.ValidationNull);

            if (string.IsNullOrWhiteSpace(updateAuthor.Name))
                return Result<int>.Fail("Author name is required.", ErrorCodes.NameRequired);

            try
            {
                var repo = _UnitOfWork.GetRepoartory<Authors>();
                var author = await repo.GetByIdAsync(id);

                if (author is null)
                    return Result<int>.Fail("Author not found.", ErrorCodes.AuthorNotFound);

                var normalized = updateAuthor.Name.Trim().ToLower();
                var exists = await _UnitOfWork.GetRepoartory<Authors>()
                    .AnyAsync(a => a.Id != id && a.Auth_Name.ToLower() == normalized);

                if (exists)
                    return Result<int>.Fail("The author's name already exists.", ErrorCodes.AuthorDuplicate);

                _mapper.Map(updateAuthor, author);
                repo.Update(author);

                var saved = await _UnitOfWork.SaveChangesAsync() > 0;
                if (!saved)
                    return Result<int>.Fail("Failed to save changes.", ErrorCodes.DbSaveFailed);

                return Result<int>.Ok(author.Id, "Author updated successfully.");
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
                return Result<int>.Fail("An unexpected error occurred.", ErrorCodes.Unexpected);
            }
        }
        #endregion

    }
}
