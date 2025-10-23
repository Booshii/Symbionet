
using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Industry
{
    public class CreateIndustryDto
    {
        [Required]
        [MaxLength(100, ErrorMessage = "Name is too long")]
        public string Name { get; set; } = string.Empty;
    }
}