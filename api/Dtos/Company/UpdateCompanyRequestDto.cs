using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Company
{
    public class UpdateCompanyRequestDto
    {
        public string Name { get; set; } = string.Empty; 

        

        public string Street { get; set; } = string.Empty; 
        public string StreetNumber { get; set; } = string.Empty; 
        public int Postalcode { get; set; }
        public string City { get; set; } = string.Empty; 
        public int IndustryId { get; set; }
        public string ContactPerson { get; set; } = string.Empty; 
        public string ContactEmail { get; set; } = string.Empty; 
        public string ContactTel { get; set; } = string.Empty; 
        public string Latitude { get; set; } = string.Empty; 
        public string Longitude { get; set; } = string.Empty; 
        public string WebsiteLink { get; set; } = string.Empty;
        
        // wird das hier gebraucht? 
        // public List<SdgTarget> SdgTargets { get; set; } = new List<SdgTarget>(); 
        // public List<AppUser> AppUsers { get; set; } = new List<AppUser>(); 

    }
}