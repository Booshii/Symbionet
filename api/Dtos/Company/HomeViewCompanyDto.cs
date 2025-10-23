using api.Dtos.SdgTypeDtos;
using api.Models;

namespace api.Dtos.Company
{
    public class HomeViewCompanyDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty; 
        public string StreetNumber { get; set; } = string.Empty; 
        public int Postalcode { get; set; }
        public string City { get; set; } = string.Empty; 
        public string Latitude { get; set; } = string.Empty; 
        public string Longitude { get; set; } = string.Empty; 
        public string IndustryName { get; set; } = string.Empty; 
        public int IndustryId { get; set; }
        public List<SimpleSdgTypeDto> SdgTypes { get; set; } = new List<SimpleSdgTypeDto>();
    }
}