namespace Shared.DTO.Floor
{
    public class CreateOrUpdateFloorDTO
    {
        public int Number_of_Blocks { get; init; }

        // المدير اختياري (زي الكيان عندك    // المدير اختياري (زي الكيان عندك EmployeeMangeId nullable)
        public int? ManagerId { get; init; }


    }
}
