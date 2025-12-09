using AutoMapper;
using Domain_Layer.Contract.UnitOfWork;
using Domain_Layer.Models.Floors_Models;
using Domain_Layer.Models.Shelf_Models;
using Service_Abstraction.Interfaces;
using Shared.DTO.Shelf;

namespace Service_Implemention.Service
{
    public class ShelfService(IUnitOfWork _UnitOfWork, IMapper _mapper) : IShelfService
    {
        #region GetAllAsync
        public async Task<IEnumerable<ShelfDTO>> GetAllAsync()
        {
            var Shelf = await _UnitOfWork.GetRepoartory<Shelf>().GetAllAsync();
            if (Shelf is null)
            {
                return Enumerable.Empty<ShelfDTO>();
            }
            else
            {
                return _mapper.Map<IEnumerable<Shelf>, IEnumerable<ShelfDTO>>(Shelf);
            }
        }

        #endregion

        #region GetByIdAsync
        public async Task<ShelfDTO?> GetByIdAsync(int Id)
        {
            var Shelf = await _UnitOfWork.GetRepoartory<Shelf>().GetByIdAsync(Id);
            return Shelf == null ? null : _mapper.Map<ShelfDTO>(Shelf);
        }
        #endregion

        #region CreateAsync
        public async Task<bool> CreateAsync(CreateOrUpdateShelfDTO CreateShelf)
        {
            try
            {
                var Shelf = _mapper.Map<CreateOrUpdateShelfDTO, Shelf>(CreateShelf);
                // تأكد أن الدور موجود
                var floorExists = await _UnitOfWork.GetRepoartory<Floors>().AnyAsync(f => f.Id == CreateShelf.FloorNumber);
                if (!floorExists)
                    throw new ArgumentException($"Floor Number ({CreateShelf.FloorNumber}) Not Found.", nameof(CreateShelf.FloorNumber));


                await _UnitOfWork.GetRepoartory<Shelf>().AddAsync(Shelf);
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

        #region Update Shelf
        public async Task<bool> UpdateAsync(int Id, CreateOrUpdateShelfDTO UpdateShelf)
        {
            try
            {
                var Repo = _UnitOfWork.GetRepoartory<Shelf>();
                var Shelf = await Repo.GetByIdAsync(Id);
                if (Shelf is null) { return false; }

                // تأكد أن الدور موجود
                var floorExists = await _UnitOfWork.GetRepoartory<Floors>().AnyAsync(f => f.Id == UpdateShelf.FloorNumber);
                if (!floorExists)
                    throw new ArgumentException($"Floor Number ({UpdateShelf.FloorNumber}) Not Found.", nameof(UpdateShelf.FloorNumber));

                _mapper.Map(UpdateShelf, Shelf);
                Repo.Update(Shelf);
                return await _UnitOfWork.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {

                return false;
            }
        }

        #endregion

        #region Delete Shelf
        public async Task<bool> DeleteAsync(int Id)
        {
            try
            {
                var Shelf = await _UnitOfWork.GetRepoartory<Shelf>().GetByIdAsync(Id);
                if (Shelf is null) { return false; }

                if (Shelf.Book.Any())
                {
                    foreach (var Book in Shelf.Book)
                    {
                        Book.ShelfId = null;
                    }
                }

                _UnitOfWork.GetRepoartory<Shelf>().Remove(Shelf);
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
