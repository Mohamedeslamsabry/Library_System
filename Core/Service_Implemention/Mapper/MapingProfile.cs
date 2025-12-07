using AutoMapper;
using Domain_Layer.Models.Book_Models;
using Domain_Layer.Models.Employee_Models;
using Domain_Layer.Models.Floors_Models;
using Domain_Layer.Models.Shared;
using Domain_Layer.Models.Shelf_Models;
using Shared.DTO.Employee;
using Shared.DTO.Floor;
using Shared.DTO.Shelf;
using System.Drawing;

namespace Service_Implemention.Mapper
{
    public class MapingProfile : Profile
    {
        public MapingProfile() : base()
        {
            #region Get Employee

            // Employee => EmployeeDto
            CreateMap<Employee, EmployeeDTO>()
                .ForMember(dest => dest.Gender, options => options.MapFrom(src => src.Gender.ToString()))

                // Id (لو BaseEntity فيه Id من نوع int)
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))

                // FullName
                .ForMember(d => d.FullName,
                    opt => opt.MapFrom(s => $"{s.FirstName} {s.LastName}".Trim()))

                // Address
                .ForMember(d => d.Address,
                    opt => opt.MapFrom(s => s.Address))

                // Users count (guard)
                .ForMember(d => d.UsersCount,
                    opt => opt.MapFrom(s => s.Users != null ? s.Users.Count : 0))

                // Floors Work
                .ForMember(d => d.FloorsNumberWork,
                    opt => opt.MapFrom(s => s.Floors != null ? (int?)s.Floors.Id : null))
                .ForMember(d => d.FloorsNumberWork,
                    opt => opt.MapFrom(s => s.FloorsNumber))

                // Floors Manage
                .ForMember(d => d.FloorMangeNumber,
                    opt => opt.MapFrom(s => s.FloorsMange != null ? (int?)s.FloorsMange.Id : null))

                // Supervisor
                .ForMember(d => d.SupervisorId,
                    opt => opt.MapFrom(s => s.SupervisorId))
                .ForMember(d => d.SupervisorFullName,
                    opt => opt.MapFrom(s =>
                        s.Supervisor != null
                            ? $"{s.Supervisor.FirstName} {s.Supervisor.LastName}".Trim()
                            : null))

                // Subordinates count
                .ForMember(d => d.SubordinatesCount,
                    opt => opt.MapFrom(s => s.Subordinates != null ? s.Subordinates.Count : 0));

            CreateMap<Address, AddressDTO>().ReverseMap();
            #endregion

            #region Create Or Update Employee
            // CreateEmployeeDto -> Employee
            CreateMap<CreateOrUpdateEmployeeDTO, Employee>();
            // كوّن Address من حقول الـDTO المسطّحة
            //.ForMember(d => d.Address, opt => opt.MapFrom((src, dest) => new Address
            //{
            //    Country = src.Country,
            //    City = src.City,
            //    BuildingNumber = src.BuildingNumber
            //})); 
            #endregion

            #region Get Floors

            // Entity -> DTO
            CreateMap<Floors, FloorDTO>()
                // نفس تسمية الخاصية في الـ DTO (Number_of_Blocks)
                .ForMember(d => d.Number_of_Blocks,
                    opt => opt.MapFrom(s => s.Number_of_Blocks))

                // المدير (Id + Name)
                .ForMember(d => d.ManagerId,
                    opt => opt.MapFrom(s => s.EmployeeMangeId))
                .ForMember(d => d.ManagerName,
                    opt => opt.MapFrom(s => s.EmployeeMange != null ? $"{s.EmployeeMange.FirstName}_{s.EmployeeMange.LastName}" : null))

                // العدّادات
                .ForMember(d => d.EmployeesWorkCount,
                    opt => opt.MapFrom(s => s.employeesWork.Count))
                .ForMember(d => d.ShelvesCount,
                    opt => opt.MapFrom(s => s.Shelfs.Count))

                // قائمة الموظفين المختصرة
                .ForMember(d => d.EmployeesWork,
                    opt => opt.MapFrom(s => s.employeesWork));

            // Sub DTO: Employee -> EmployeeBriefDto
            CreateMap<Employee, EmployeeBriefDto>()
                .ForMember(d => d.Name, opt => opt.MapFrom(s => $"{ s.FirstName}_{s.LastName}")) 
                .ForMember(d => d.PhoneNumber, opt => opt.MapFrom(s=>s.PhoneNumber));


            #endregion

            #region Create Or Update Floor

            CreateMap<CreateOrUpdateFloorDTO, Floors>()
                       .ForMember(s => s.Number_of_Blocks, opt => opt.MapFrom(d => d.Number_of_Blocks))
                       .ForMember(s => s.EmployeeMangeId, opt => opt.MapFrom(d => d.ManagerId));
            #endregion

            #region Get Shelf

            // Shelf -> ShelfDetailsDTO (تفاصيل)
            CreateMap<Shelf, ShelfDTO>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.FloorNumber, opt => opt.MapFrom(s => s.FloorNumber))
                .ForMember(d => d.BooksCount, opt => opt.MapFrom(s => s.Book.Count))
                .ForMember(d => d.Books, opt => opt.MapFrom(s => s.Book))
                .ForMember(d => d.Floor, opt => opt.MapFrom(s => s.Floor));

            // Sub mappings
            CreateMap<Book, BookBriefDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.Title, opt => opt.MapFrom(s => s.TiTle));

            CreateMap<Floors, FloorBriefDto>()
                       .ForMember(d => d.FloorNumber, opt => opt.MapFrom(s => s.Id))
                       .ForMember(d => d.Number_of_Blocks, opt => opt.MapFrom(s => s.Number_of_Blocks));



            #endregion

            #region Create Or Update Shelf
            CreateMap<CreateOrUpdateShelfDTO, Shelf>(); 
            #endregion

        }


    }

}

