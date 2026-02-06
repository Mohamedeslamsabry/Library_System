using System.ComponentModel.DataAnnotations;

namespace Shared.DTO.Publisher
{
    public class CreateOrUpdatePublisherDTO
    {

        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string Publisher_Name { get; set; } = null!;

    }
}
