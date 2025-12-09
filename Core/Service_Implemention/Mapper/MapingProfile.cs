using AutoMapper;
using Domain_Layer.Models.Authors_Models;
using Domain_Layer.Models.Book_Authors_Models;
using Domain_Layer.Models.Book_Models;
using Domain_Layer.Models.Employee_Models;
using Domain_Layer.Models.Floors_Models;
using Domain_Layer.Models.Puplishers_Models;
using Domain_Layer.Models.Shared;
using Domain_Layer.Models.Shelf_Models;
using Domain_Layer.Models.Users_Models;
using Shared.DTO.authors;
using Shared.DTO.Book;
using Shared.DTO.Employee;
using Shared.DTO.Floor;
using Shared.DTO.Publisher;
using Shared.DTO.Shelf;
using Shared.DTO.User;

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
                .ForMember(d => d.Name, opt => opt.MapFrom(s => $"{s.FirstName}_{s.LastName}"))
                .ForMember(d => d.PhoneNumber, opt => opt.MapFrom(s => s.PhoneNumber));


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

            #region  Publishers

            CreateMap<Puplishers, PublisherDTO>()
                        .ForMember(d => d.Books, opt => opt.MapFrom(s => s.Book));


            CreateMap<CreateOrUpdatePublisherDTO, Puplishers>();


            CreateMap<Book, BookShortDto>();

            #endregion

            #region User

            CreateMap<Users, UserDTO>()
                .ForMember(d => d.Gender, opt => opt.MapFrom(s => s.Gender.ToString()))
                 .ForMember(d => d.Employee, opt => opt.MapFrom(s => s.Employee));


            CreateMap<CreateOrUpdateUserDTO, Users>();


            // اختياري: خريطة الموظف المختصر
            CreateMap<Employee, EmployeeShortDto>()
                           .ForMember(d => d.Name, opt => opt.MapFrom(s => $"{s.FirstName}_{s.LastName}"));


            #endregion

            #region Book

            CreateMap<Book, BookDTO>()
                      .ForMember(d => d.CategoryName, opt => opt.MapFrom(s => s.Category != null ? s.Category.CategoryName : null))
                      .ForMember(d => d.PublisherId, opt => opt.MapFrom(s => s.puplisherId))
                      .ForMember(d => d.PublisherName, opt => opt.MapFrom(s => s.puplisher != null ? s.puplisher.Publisher_Name : null))
                      .ForMember(d => d.AuthorIds, opt => opt.MapFrom(s => s.Book_Authors.Select(ba => ba.AuthorId)))
                      .ForMember(d => d.AuthorNames, opt => opt.MapFrom(s =>
                          s.Book_Authors.Where(ba => ba.Author != null).Select(ba => ba.Author.Auth_Name)))
                      .ForMember(d => d.IsBorrowed, opt => opt.MapFrom(s => s.Borrow != null))
                      .ForMember(d => d.BorrowId, opt => opt.MapFrom(s => s.Borrow != null ? (int?)s.Borrow.Id : null));



            CreateMap<CreateOrUpdateBookDto, Book>()
                       // مفاتيح العلاقات
                       .ForMember(d => d.puplisherId, opt => opt.MapFrom(s => s.PublisherId))
                       // تجاهل الملاحة: بنضبطها في EF عند التتبع/التحميل
                       .ForMember(d => d.Shelf, opt => opt.Ignore())
                       .ForMember(d => d.Category, opt => opt.Ignore())
                       .ForMember(d => d.puplisher, opt => opt.Ignore())
                       .ForMember(d => d.Borrow, opt => opt.Ignore())
                       .ForMember(d => d.Book_Authors, opt => opt.Ignore())
                       .ForMember(d => d.Book_Authors, opt => opt.Ignore()) // مهم جدًا
                       .AfterMap((src, dest) =>
                       {
                           // لو الكيان جديد، Book_Authors غالبًا تكون فاضية
                           // في الحالتين (Create/Update): نعمل مزامنة بسيطة للـ AuthorIds

                           var newAuthorIds = (src.AuthorIds ?? new List<int>()).Distinct().ToList();

                           // إزالة العلاقات غير المطلوبة
                           dest.Book_Authors = dest.Book_Authors
                               .Where(ba => newAuthorIds.Contains(ba.AuthorId))
                               .ToHashSet();

                           // إضافة العلاقات الجديدة
                           var existingAuthorIds = dest.Book_Authors.Select(ba => ba.AuthorId).ToHashSet();
                           var toAdd = newAuthorIds.Where(id => !existingAuthorIds.Contains(id));

                           foreach (var authorId in toAdd)

                           {
                               dest.Book_Authors.Add(new Book_Authors
                               {
                                   BookId = dest.Id,     // EF ممكن يضبطه عند الإضافة
                                   AuthorId = authorId
                               });
                           }
                       });
            #endregion

            #region authors

            CreateMap<Authors, AuthorDTO>()
                      .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Auth_Name))
                      .ForMember(d => d.BooksCount, opt => opt.MapFrom(s => s.Book_Authors != null ? s.Book_Authors.Count : 0))
                      .ForMember(d => d.BookIds, opt => opt.MapFrom(s =>
                          (s.Book_Authors ?? Enumerable.Empty<Book_Authors>()).Select(ba => ba.BookId)));

            // Create/Update DTO -> Entity
            CreateMap<CreateOrUpdateAuthorDTO, Authors>()
                .ForMember(d => d.Auth_Name, opt => opt.MapFrom(s => s.Name))
            // تجاهل علاقات الربط في الإنشاء/التحديث

            .ForMember(d => d.Book_Authors, opt => opt.Ignore());

            #endregion
        }
    }
}