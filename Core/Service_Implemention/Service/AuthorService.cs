using AutoMapper;
using Domain_Layer.Contract.UnitOfWork;
using Domain_Layer.Exceptions;
using Domain_Layer.Models.Authors_Models;
using Domain_Layer.Models.Users_Models;
using Service_Abstraction.Interfaces;
using Service_Implemention.Specification;
using Shared;
using Shared.DTO.authors;

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
        public async Task<bool> CreateAsync(CreateOrUpdateAuthorDTO CreateAuthor)
        {
            try
            {               
                var Author = _mapper.Map<CreateOrUpdateAuthorDTO, Authors>(CreateAuthor);

                await _UnitOfWork.GetRepoartory<Authors>().AddAsync(Author);
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

        #region Update Author
        public async Task<bool> UpdateAsync(int Id, CreateOrUpdateAuthorDTO UpdateAuthor)
        {
            try
            {
                var Repo = _UnitOfWork.GetRepoartory<Authors>();
                var author = await Repo.GetByIdAsync(Id);
                if (author is null) { return false; }


                _mapper.Map(UpdateAuthor, author);
                Repo.Update(author);
                return await _UnitOfWork.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {

                return false;
            }
        }

        #endregion

    }
}
