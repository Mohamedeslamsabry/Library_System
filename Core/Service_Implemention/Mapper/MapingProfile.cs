using AutoMapper;
using Domain_Layer.Models.Employee_Models;
using Domain_Layer.Models.Shared;
using Shared.DTO;

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

            #region Create Employee
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

        }

    }
}
