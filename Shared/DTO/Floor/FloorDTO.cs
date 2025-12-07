namespace Shared.DTO.Floor
{
    public class FloorDTO
    {

        public int Id { get; init; }
        public int Number_of_Blocks { get; init; }
        // المدير
        public int? ManagerId { get; init; }
        public string? ManagerName { get; init; }

        // ملخّص الأعداد
        public int EmployeesWorkCount { get; init; }
        public int ShelvesCount { get; init; }

        // قوائم مبسّطة لتفاصيل الموظفين والرفوف (اختياري)
        public IReadOnlyList<EmployeeBriefDto> EmployeesWork { get; init; } = Array.Empty<EmployeeBriefDto>();

    }

    public sealed class EmployeeBriefDto
    {
        public int Id { get; init; }
        public string Name { get; init; } = null!;
        public string PhoneNumber { get; init; } = null!;

    }

}
