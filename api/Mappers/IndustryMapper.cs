
using api.Dtos.Industry;
using api.Models;

namespace api.Mappers
{
    public static class IndustryMapper
    {
        public static IndustryDto ToIndustryDto(this Industry industryModel){
            return new IndustryDto{
                Id = industryModel.Id,
                Name = industryModel.Name,
            };
        }
        public static Industry ToIndustryFromCreateDto(this CreateIndustryDto industryDto){
            return new Industry{
                Name = industryDto.Name,
            };
        }
    }
}