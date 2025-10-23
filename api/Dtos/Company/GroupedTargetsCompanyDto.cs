using api.Models;
using api.Dtos.SdgTargetDtos;


namespace api.Dtos.Company
{
    public class groupedTargetsCompanyDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; 

        

        public string Street { get; set; } = string.Empty; 
        public string StreetNumber { get; set; } = string.Empty; 
        public int Postalcode { get; set; }
        public string City { get; set; } = string.Empty; 
        public string Latitude { get; set; } = string.Empty; 
        public string Longitude { get; set; } = string.Empty; 
        public string ContactPerson { get; set; } = string.Empty; 
        public string ContactEmail { get; set; } = string.Empty; 
        public string ContactTel { get; set; } = string.Empty; 
        public string IndustryName { get; set; } = string.Empty; 
        public string WebsiteLink { get; set; } = string.Empty; 
        // wenn hier fehler einfach alt 
        public Dictionary<string, List<SdgTargetDto>> GroupedSdgTargetsByColor { get; set; } = new(); 
        public List<AppUser> AppUsers { get; set; } = new List<AppUser>(); 
    }
}